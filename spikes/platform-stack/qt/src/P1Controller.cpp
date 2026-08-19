#include "P1Controller.h"

#include <QJsonDocument>

namespace {
QVariantMap jsonObjectToMap(const QJsonObject &object)
{
    return object.toVariantMap();
}
}

P1Controller::P1Controller(QObject *parent)
    : QObject(parent), m_fixture(P1Fixture::load()), m_selectedEquipmentId(m_fixture.selectedEquipmentId())
{
}

QVariantMap P1Controller::fixture() const { return jsonObjectToMap(m_fixture.root()); }
QVariantMap P1Controller::selectedEquipment() const { return jsonObjectToMap(m_fixture.equipment(m_selectedEquipmentId)); }

bool P1Controller::selectEquipment(const QString &id)
{
    if (m_fixture.equipment(id).isEmpty())
        return false;
    if (m_selectedEquipmentId == id)
        return true;
    m_selectedEquipmentId = id;
    emit selectedEquipmentChanged();
    return true;
}

QVariantMap P1Controller::shortcuts() const
{
    return {
        {QStringLiteral("open"), QStringLiteral("Ctrl+O")},
        {QStringLiteral("save"), QStringLiteral("Ctrl+S")},
        {QStringLiteral("undo"), QStringLiteral("Ctrl+Z")},
        {QStringLiteral("redo"), QStringLiteral("Ctrl+Y")}
    };
}
