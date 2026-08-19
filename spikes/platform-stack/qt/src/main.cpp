#include "P1Controller.h"
#include "P1Fixture.h"

#include <QFontDatabase>
#include <QGuiApplication>
#include <QQmlApplicationEngine>
#include <QQmlContext>
#include <QQuickStyle>

int main(int argc, char *argv[])
{
    QGuiApplication app(argc, argv);
    QGuiApplication::setApplicationName(QStringLiteral("Electrical Engineering Platform"));
    QQuickStyle::setStyle(QStringLiteral("Fusion"));

    const int regularFont = QFontDatabase::addApplicationFont(P1Fixture::regularFontResourcePath());
    const int semiBoldFont = QFontDatabase::addApplicationFont(P1Fixture::semiBoldFontResourcePath());
    if (regularFont < 0 || semiBoldFont < 0)
        return 20;

    QFont applicationFont(QStringLiteral("Noto Sans"));
    applicationFont.setPointSize(10);
    QGuiApplication::setFont(applicationFont);

    P1Controller controller;
    QQmlApplicationEngine engine;
    engine.rootContext()->setContextProperty(QStringLiteral("p1"), &controller);
    QObject::connect(&engine, &QQmlApplicationEngine::objectCreationFailed, &app, [] { QCoreApplication::exit(21); }, Qt::QueuedConnection);
    engine.loadFromModule(QStringLiteral("Eep.P1"), QStringLiteral("Main"));
    return app.exec();
}
