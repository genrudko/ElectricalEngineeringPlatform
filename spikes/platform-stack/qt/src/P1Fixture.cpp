#include "P1Fixture.h"

#include <QFile>
#include <QJsonDocument>
#include <QJsonParseError>
#include <stdexcept>

QString P1Fixture::fixtureResourcePath() { return QStringLiteral(":/eep/p1/p1-shell-fixture.json"); }
QString P1Fixture::regularFontResourcePath() { return QStringLiteral(":/eep/p1/NotoSans-Regular.ttf"); }
QString P1Fixture::semiBoldFontResourcePath() { return QStringLiteral(":/eep/p1/NotoSans-SemiBold.ttf"); }

P1Fixture P1Fixture::load()
{
    QFile file(fixtureResourcePath());
    if (!file.open(QIODevice::ReadOnly))
        throw std::runtime_error("Unable to open embedded P1 fixture");

    QJsonParseError error{};
    const auto document = QJsonDocument::fromJson(file.readAll(), &error);
    if (error.error != QJsonParseError::NoError || !document.isObject())
        throw std::runtime_error(("Invalid P1 fixture JSON: " + error.errorString()).toStdString());

    return P1Fixture(document.object());
}

QString P1Fixture::schema() const { return m_root.value(QStringLiteral("schema")).toString(); }
QString P1Fixture::applicationTitle() const { return m_root.value(QStringLiteral("applicationTitle")).toString(); }
QString P1Fixture::selectedEquipmentId() const { return m_root.value(QStringLiteral("selectedEquipmentId")).toString(); }
QJsonObject P1Fixture::equipment(const QString &id) const
{
    return m_root.value(QStringLiteral("equipment")).toObject().value(id).toObject();
}
