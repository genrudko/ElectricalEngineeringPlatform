#!/usr/bin/env bash
set -euo pipefail

if [[ ${EUID} -ne 0 ]]; then
  echo "ERROR: run as root (sudo bash scripts/install_development_bridge.sh)" >&2
  exit 2
fi

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SOURCE_DIR="$REPO_ROOT/tools/development_bridge"
DEPLOY_DIR="/opt/eep-dev-bridge"
STATE_DIR="/var/lib/eep-dev-bridge"
READONLY_REPO="/home/eep-workspace/workspace/ElectricalEngineeringPlatform"
BRIDGE_SERVICE="eep-dev-bridge.service"
SYNC_SERVICE="eep-workspace-sync.service"
SYNC_TIMER="eep-workspace-sync.timer"
BACKUP_DIR="/var/backups/eep-dev-bridge/$(date -u +%Y%m%dT%H%M%SZ)"

for user in eepbridge eep-workspace; do
  if ! id "$user" >/dev/null 2>&1; then
    echo "ERROR: required user '$user' does not exist" >&2
    exit 3
  fi
done

for file in "$SOURCE_DIR/app.py" "$SOURCE_DIR/bridge_core.py"; do
  if [[ ! -f "$file" ]]; then
    echo "ERROR: source file missing: $file" >&2
    exit 4
  fi
done

if [[ ! -d "$READONLY_REPO/.git" ]]; then
  echo "ERROR: read-only repository workspace missing: $READONLY_REPO" >&2
  exit 5
fi

mkdir -p "$BACKUP_DIR"
if [[ -f "$DEPLOY_DIR/app.py" ]]; then
  cp -a "$DEPLOY_DIR/app.py" "$BACKUP_DIR/app.py"
fi
if [[ -f "$DEPLOY_DIR/bridge_core.py" ]]; then
  cp -a "$DEPLOY_DIR/bridge_core.py" "$BACKUP_DIR/bridge_core.py"
fi

rollback() {
  rc=$?
  echo "ERROR: deployment failed; restoring previous Bridge source" >&2
  if [[ -f "$BACKUP_DIR/app.py" ]]; then
    install -o eepbridge -g eepbridge -m 0640 "$BACKUP_DIR/app.py" "$DEPLOY_DIR/app.py"
  fi
  if [[ -f "$BACKUP_DIR/bridge_core.py" ]]; then
    install -o eepbridge -g eepbridge -m 0640 "$BACKUP_DIR/bridge_core.py" "$DEPLOY_DIR/bridge_core.py"
  else
    rm -f "$DEPLOY_DIR/bridge_core.py"
  fi
  systemctl restart "$BRIDGE_SERVICE" >/dev/null 2>&1 || true
  exit "$rc"
}
trap rollback ERR

printf '%s\n' "=== 1. Prepare identities and read-only repository access ==="
usermod -aG eep-workspace eepbridge
chmod g+rx /home/eep-workspace /home/eep-workspace/workspace
chmod -R g+rX "$READONLY_REPO"
chmod -R g-w "$READONLY_REPO"

printf '%s\n' "=== 2. Install Bridge v0.2 source ==="
install -d -o eepbridge -g eepbridge -m 0750 "$DEPLOY_DIR"
install -d -o eepbridge -g eepbridge -m 0750 "$STATE_DIR" "$STATE_DIR/tasks"
install -o eepbridge -g eepbridge -m 0640 "$SOURCE_DIR/app.py" "$DEPLOY_DIR/app.py"
install -o eepbridge -g eepbridge -m 0640 "$SOURCE_DIR/bridge_core.py" "$DEPLOY_DIR/bridge_core.py"

printf '%s\n' "=== 3. Install read-only repository sync service ==="
cat > "/etc/systemd/system/$SYNC_SERVICE" <<'UNIT'
[Unit]
Description=EEP read-only GitHub workspace fetch
After=network-online.target
Wants=network-online.target

[Service]
Type=oneshot
User=eep-workspace
Group=eep-workspace
Environment=HOME=/home/eep-workspace
WorkingDirectory=/home/eep-workspace/workspace/ElectricalEngineeringPlatform
ExecStart=/usr/bin/git fetch --prune --no-tags origin
NoNewPrivileges=true
PrivateTmp=true
ProtectSystem=strict
ProtectHome=read-only
ReadWritePaths=/home/eep-workspace/workspace/ElectricalEngineeringPlatform/.git

[Install]
WantedBy=multi-user.target
UNIT

cat > "/etc/systemd/system/$SYNC_TIMER" <<'UNIT'
[Unit]
Description=Periodic EEP read-only GitHub workspace fetch

[Timer]
OnBootSec=30s
OnUnitActiveSec=60s
RandomizedDelaySec=10s
Persistent=true
Unit=eep-workspace-sync.service

[Install]
WantedBy=timers.target
UNIT

systemctl daemon-reload
systemctl enable --now "$SYNC_TIMER"
systemctl start "$SYNC_SERVICE"

printf '%s\n' "=== 4. Verify eepbridge read-only repository access and credential isolation ==="
sudo -u eepbridge -H \
  git -c "safe.directory=$READONLY_REPO" -C "$READONLY_REPO" rev-parse HEAD >/dev/null
if sudo -u eepbridge -H test -w "$READONLY_REPO"; then
  echo "ERROR: eepbridge can write repository root" >&2
  false
fi
if sudo -u eepbridge -H test -w "$READONLY_REPO/.git"; then
  echo "ERROR: eepbridge can write repository metadata" >&2
  false
fi
if sudo -u eepbridge -H test -r /home/eep-workspace/.ssh/eep_github_ed25519; then
  echo "ERROR: eepbridge can read eep-workspace deploy key" >&2
  false
fi

printf '%s\n' "=== 5. Restart Bridge ==="
systemctl restart "$BRIDGE_SERVICE"
sleep 2
systemctl is-active --quiet "$BRIDGE_SERVICE"

printf '%s\n' "=== 6. Verify hardened unauthenticated surface ==="
health_code="$(curl --silent --output /dev/null --write-out '%{http_code}' http://127.0.0.1:8788/health)"
openapi_code="$(curl --silent --output /dev/null --write-out '%{http_code}' http://127.0.0.1:8788/openapi.json)"
[[ "$health_code" == "401" ]]
[[ "$openapi_code" == "404" ]]

printf '%s\n' "=== 7. Verify authenticated health without printing credential ==="
TOKEN="$(python3 - <<'PY'
from pathlib import Path

path = Path('/etc/eep-dev-bridge.env')
pairs = {}
for raw in path.read_text(encoding='utf-8').splitlines():
    line = raw.strip()
    if not line or line.startswith('#'):
        continue
    if line.startswith('export '):
        line = line[7:].strip()
    if '=' not in line:
        continue
    key, value = line.split('=', 1)
    pairs[key.strip()] = value.strip().strip('"').strip("'")

preferred = ('EEP_BRIDGE_TOKEN', 'EEP_DEV_BRIDGE_TOKEN', 'BRIDGE_TOKEN', 'EEP_BRIDGE_API_KEY')
for key in preferred:
    if pairs.get(key):
        print(pairs[key], end='')
        raise SystemExit(0)

candidates = [
    value for key, value in pairs.items()
    if value and ('BRIDGE' in key.upper() or key.upper().startswith('EEP_'))
    and ('TOKEN' in key.upper() or 'KEY' in key.upper())
]
if len(candidates) != 1:
    raise SystemExit('unable to identify bridge credential in env file')
print(candidates[0], end='')
PY
)"

auth_body="$(curl --fail --silent --show-error -H "Authorization: Bearer $TOKEN" http://127.0.0.1:8788/health)"
unset TOKEN
python3 - "$auth_body" <<'PY'
import json
import sys
body = json.loads(sys.argv[1])
assert body['ok'] is True
assert body['service'] == 'eep-dev-bridge'
assert body['version'] == '0.2.0'
PY

printf '%s\n' "=== 8. Service and timer state ==="
systemctl --no-pager --full is-active "$BRIDGE_SERVICE"
systemctl --no-pager --full is-active "$SYNC_TIMER"
systemctl --no-pager --full is-active "$SYNC_SERVICE" || [[ "$(systemctl is-active "$SYNC_SERVICE")" == "inactive" ]]

trap - ERR
printf '%s\n' "======================================================"
printf '%s\n' "PASS: EEP Development Bridge v0.2 deployed"
printf '%s\n' "Backup: $BACKUP_DIR"
printf '%s\n' "Bridge: active, authenticated, OpenAPI/docs hidden"
printf '%s\n' "Repo sync: timer enabled, read-only deploy key remains isolated"
printf '%s\n' "======================================================"
