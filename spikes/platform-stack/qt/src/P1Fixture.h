#pragma once

#include <QJsonObject>
#include <QString>

class P1Fixture final
{
public:
    static P1Fixture load();

    const QJsonObject &root() const noexcept { return m_root; }
    QString schema() const;
    QString applicationTitle() const;
    QString selectedEquipmentId() const;
    QJsonObject equipment(const QString &id) const;

    static QString fixtureResourcePath();
    static QString regularFontResourcePath();
    static QString semiBoldFontResourcePath();

private:
    explicit P1Fixture(QJsonObject root) : m_root(std::move(root)) {}
    QJsonObject m_root;
};
