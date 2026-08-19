#pragma once

#include "P1Fixture.h"

#include <QObject>
#include <QVariantMap>

class P1Controller final : public QObject
{
    Q_OBJECT
    Q_PROPERTY(QVariantMap fixture READ fixture CONSTANT)
    Q_PROPERTY(QVariantMap selectedEquipment READ selectedEquipment NOTIFY selectedEquipmentChanged)
    Q_PROPERTY(QString selectedEquipmentId READ selectedEquipmentId NOTIFY selectedEquipmentChanged)

public:
    explicit P1Controller(QObject *parent = nullptr);

    QVariantMap fixture() const;
    QVariantMap selectedEquipment() const;
    QString selectedEquipmentId() const { return m_selectedEquipmentId; }

    Q_INVOKABLE bool selectEquipment(const QString &id);
    Q_INVOKABLE QVariantMap shortcuts() const;

signals:
    void selectedEquipmentChanged();

private:
    P1Fixture m_fixture;
    QString m_selectedEquipmentId;
};
