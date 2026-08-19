cmake_minimum_required(VERSION 4.4.2)
project(EEP_P1_Qt VERSION 0.1.0 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)
set(CMAKE_AUTOMOC ON)

find_package(Qt6 6.11.1 EXACT REQUIRED COMPONENTS Core Gui Qml Quick QuickControls2 Test)
qt_standard_project_setup(REQUIRES 6.11)

enable_testing()

add_library(eep_p1_qt_fixture STATIC
    src/P1Fixture.cpp
    src/P1Fixture.h
)
target_link_libraries(eep_p1_qt_fixture PUBLIC Qt6::Core Qt6::Gui)
target_compile_definitions(eep_p1_qt_fixture PRIVATE QT_NO_KEYWORDS)

qt_add_resources(eep_p1_qt_fixture "eep_p1_shared_fixture"
    PREFIX "/eep/p1"
    FILES
        ../shared/fixtures/p1-shell-fixture.json
        fonts/NotoSans-Regular.ttf
        fonts/NotoSans-SemiBold.ttf
)

qt_add_executable(eep-p1-qt
    src/main.cpp
    src/P1Controller.cpp
    src/P1Controller.h
)
target_link_libraries(eep-p1-qt PRIVATE
    eep_p1_qt_fixture
    Qt6::Core Qt6::Gui Qt6::Qml Qt6::Quick Qt6::QuickControls2
)

qt_add_qml_module(eep-p1-qt
    URI Eep.P1
    VERSION 1.0
    QML_FILES qml/Main.qml
)

add_executable(eep-p1-qt-smoke
    tests/tst_p1.cpp
    src/P1Controller.cpp
    src/P1Controller.h
)
target_link_libraries(eep-p1-qt-smoke PRIVATE
    eep_p1_qt_fixture
    Qt6::Core Qt6::Gui Qt6::Test
)
add_test(NAME eep-p1-qt-smoke COMMAND eep-p1-qt-smoke)

install(TARGETS eep-p1-qt
    RUNTIME DESTINATION .
    BUNDLE DESTINATION .
)
