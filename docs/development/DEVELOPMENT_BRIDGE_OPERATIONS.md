# EEP Development Bridge — operations

Status: operational procedure for `INFRASTRUCTURE-SPIKE-001`.

This document covers the bounded `EEP Development Bridge` runtime on the development VPS. It intentionally does not contain any bearer token, private key, PAT, registration token, or industrial/private corpus data.

## 1. Runtime boundary

Canonical runtime:

```text
service: eep-dev-bridge.service
user: eepbridge
bind: 127.0.0.1:8788
application: /opt/eep-dev-bridge
state: /var/lib/eep-dev-bridge
credential env file: /etc/eep-dev-bridge.env
public TLS endpoint: https://eep-dev-5-181-177-72.nip.io
repository view: /home/eep-workspace/workspace/ElectricalEngineeringPlatform
```

Caddy remains the public TLS reverse proxy. The Bridge itself binds only localhost.

The bearer credential is stored outside Git and is never written to GitHub Actions artifacts or logs.

The Bridge service keeps home isolation enabled and receives only the fixed repository path as an explicit read-only systemd bind. The deploy-key directory is not exposed to `eepbridge`.

## 2. Normal status checks

```bash
sudo systemctl --no-pager --full status eep-dev-bridge.service
sudo systemctl is-active eep-dev-bridge.service
```

Unauthenticated probes should show the hardened surface:

```bash
curl -sS -o /dev/null -w '%{http_code}\n' http://127.0.0.1:8788/health
# expected: 401

curl -sS -o /dev/null -w '%{http_code}\n' http://127.0.0.1:8788/openapi.json
# expected: 404
```

Runtime OpenAPI/docs are deliberately disabled. The canonical ChatGPT Action schema is versioned in Git at:

`tools/development_bridge/openapi-action.yaml`.

## 3. Credential rotation contract

Rotation is performed only when required by security/operations, not as a ceremonial acceptance step.

Required invariants:

- generate a new high-entropy bearer token locally on the VPS;
- do not print the old or new token into chat, GitHub logs, shell transcript, or artifacts;
- keep `/etc/eep-dev-bridge.env` owned by root and mode `0600`;
- update the ChatGPT Action bearer secret before declaring rotation complete;
- verify authenticated `/health` with the new credential;
- verify the old credential no longer authenticates;
- keep rollback capability until both server and Action sides are proven.

Recommended controlled sequence:

1. Record current service state and create a root-only backup of `/etc/eep-dev-bridge.env`.
2. Generate a new token into a shell variable without echoing it.
3. Replace only the existing Bridge bearer credential value in `/etc/eep-dev-bridge.env`; do not change unrelated environment entries.
4. Verify file owner/mode remain `root:root` / `0600`.
5. Restart `eep-dev-bridge.service`.
6. Verify service `active` and authenticated local `/health` using the new token.
7. Update the ChatGPT Action secret through the ChatGPT configuration UI; never paste the token into project chat.
8. From Project chat call `getBridgeHealth` and one bounded read operation.
9. Verify the old token returns `401`.
10. Remove the temporary root-only env backup after acceptance.

A failure at steps 3–8 requires restoring the previous root-only env backup and restarting the service before further diagnosis.

## 4. Recovery after Bridge start/runtime failure

First determine whether the failure is application/configuration or dependency/runtime related:

```bash
sudo systemctl --no-pager --full status eep-dev-bridge.service
sudo journalctl -u eep-dev-bridge.service -n 120 --no-pager -o cat
```

Do not print `/etc/eep-dev-bridge.env` during diagnosis.

Deployment source has its own rollback in `scripts/install_development_bridge.sh`. The installer backs up previous Bridge source/systemd override before replacing the running runtime and restores them if the candidate fails deployment checks.

Known proven failure classes from `INFRASTRUCTURE-SPIKE-001`:

- Git cross-owner `dubious ownership` — handled with an explicit fixed `safe.directory`, never wildcard `safe.directory=*`;
- write failure under `ProtectSystem=strict` — handled with dedicated `StateDirectory=eep-dev-bridge` and `ReadWritePaths=/var/lib/eep-dev-bridge` rather than disabling systemd hardening;
- repository `Permission denied` only inside the service namespace — caused by home isolation; handled by keeping `ProtectHome=tmpfs` and exposing only the fixed repository path with an explicit read-only bind, not by disabling `ProtectHome` and not by exposing the SSH/deploy-key directory.

A host-side `sudo -u eepbridge git ...` success does not by itself prove service-namespace access. After any systemd sandbox change, verify through the authenticated Bridge endpoint itself.

## 5. Repository sync recovery

The fixed read-only repository workspace is refreshed by:

```text
eep-workspace-sync.timer
→ eep-workspace-sync.service
→ git fetch --prune --no-tags origin
```

Status:

```bash
sudo systemctl --no-pager --full status eep-workspace-sync.timer
sudo systemctl --no-pager --full status eep-workspace-sync.service
```

Manual one-shot fetch through the service:

```bash
sudo systemctl start eep-workspace-sync.service
```

`eepbridge` must remain unable to read `/home/eep-workspace/.ssh/eep_github_ed25519` and unable to write the repository root or `.git`.

## 6. ChatGPT Action recovery and approval behavior

If the public TLS endpoint works but Action calls fail:

1. Do not weaken Bridge authentication.
2. Verify unauthenticated `/health` still returns `401`.
3. Verify server-side authenticated `/health` locally without printing the token.
4. Verify the Action schema server URL is `https://eep-dev-5-181-177-72.nip.io`.
5. Verify bearer authentication remains configured in the Action UI.
6. Re-save the canonical schema from `tools/development_bridge/openapi-action.yaml` if operation IDs changed.
7. Test `getBridgeHealth` before any task operation.
8. Test `bridge_selftest` against an exact commit that actually contains the Bridge test directory; do not use an older `main` snapshot for a test introduced only in the active work-item branch.

Do not enable runtime `/openapi.json` merely to make the Action easier to configure.

The bounded `runBridgeTask` and `cancelTask` operations are explicitly marked non-consequential in the canonical Action schema. This does not broaden server permissions: the API still accepts only allowlisted profiles and validated refs. When the ChatGPT UI offers a persistent approval choice for this private Action, the owner may select it to avoid repeated approval clicks during routine bounded development work.

## 7. Decommission

Bridge decommission requires an explicit owner decision. At minimum:

- disable/stop `eep-dev-bridge.service`;
- remove/disable the ChatGPT Action configuration;
- remove the bearer credential file only after no rollback is required;
- preserve any required non-secret audit evidence;
- do not delete the shared `eep-workspace` or runner identities if they are still used by the GitHub/VPS infrastructure contour.
