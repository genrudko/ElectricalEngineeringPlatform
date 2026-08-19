#include "P1Controller.h"
#include "P1Fixture.h"

#include <QGuiApplication>
#include <QTest>

class P1Smoke final : public QObject
{
    Q_OBJECT
private slots:
    void fixtureAndCommands()
    {
        P1Controller controller;
        QCOMPARE(controller.fixture().value(QStringLiteral("schema")).toString(), QStringLiteral("eep.p1-shell-fixture/v1"));
        QCOMPARE(controller.fixture().value(QStringLiteral("applicationTitle")).toString(), QStringLiteral("Electrical Engineering Platform"));
        QCOMPARE(controller.selectedEquipmentId(), QStringLiteral("QF-35-01"));
        QVERIFY(controller.selectEquipment(QStringLiteral("QS-35-01")));
        QCOMPARE(controller.selectedEquipmentId(), QStringLiteral("QS-35-01"));
        QVERIFY(controller.selectEquipment(QStringLiteral("QF-35-01")));
        const auto shortcuts = controller.shortcuts();
        QCOMPARE(shortcuts.value(QStringLiteral("open")).toString(), QStringLiteral("Ctrl+O"));
        QCOMPARE(shortcuts.value(QStringLiteral("save")).toString(), QStringLiteral("Ctrl+S"));
        QCOMPARE(shortcuts.value(QStringLiteral("undo")).toString(), QStringLiteral("Ctrl+Z"));
        QCOMPARE(shortcuts.value(QStringLiteral("redo")).toString(), QStringLiteral("Ctrl+Y"));
    }

    void selectedEquipmentCarriesP1ValidationStates()
    {
        P1Controller controller;
        const auto properties = controller.selectedEquipment().value(QStringLiteral("properties")).toList();
        QStringList states;
        for (const auto &property : properties)
            states << property.toMap().value(QStringLiteral("state")).toString();
        QVERIFY(states.contains(QStringLiteral("warning")));
        QVERIFY(states.contains(QStringLiteral("error")));
        QVERIFY(states.contains(QStringLiteral("unknown")));
    }
};

int main(int argc, char **argv)
{
    QGuiApplication app(argc, argv);
    P1Smoke test;
    return QTest::qExec(&test, argc, argv);
}

#include "tst_p1.moc"
