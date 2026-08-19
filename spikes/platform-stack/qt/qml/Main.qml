import QtQuick
import QtQuick.Controls
import QtQuick.Layouts

ApplicationWindow {
    id: root
    visible: true
    width: 1360
    height: 860
    minimumWidth: 1050
    minimumHeight: 680
    title: p1.fixture.applicationTitle
    color: "#eef1f4"

    property bool equipmentVisible: true
    property bool propertiesVisible: true
    property bool diagnosticsVisible: true
    property int currentDocument: 0

    Shortcut { sequence: "Ctrl+O"; onActivated: statusText.text = "Открытие demo fixture…" }
    Shortcut { sequence: "Ctrl+S"; onActivated: statusText.text = "Demo state сохранён" }
    Shortcut { sequence: "Ctrl+Z"; onActivated: statusText.text = "Отмена demo action" }
    Shortcut { sequence: "Ctrl+Y"; onActivated: statusText.text = "Повтор demo action" }

    menuBar: MenuBar {
        Menu { title: "Файл"; MenuItem { text: "Открыть…\tCtrl+O"; onTriggered: statusText.text = "Открытие demo fixture…" }; MenuItem { text: "Сохранить\tCtrl+S"; onTriggered: statusText.text = "Demo state сохранён" }; MenuSeparator {}; MenuItem { text: "Выход"; onTriggered: Qt.quit() } }
        Menu { title: "Правка"; MenuItem { text: "Отменить\tCtrl+Z" }; MenuItem { text: "Повторить\tCtrl+Y" } }
        Menu { title: "Вид"; MenuItem { text: "Панель оборудования"; checkable: true; checked: equipmentVisible; onTriggered: equipmentVisible = checked }; MenuItem { text: "Свойства"; checkable: true; checked: propertiesVisible; onTriggered: propertiesVisible = checked }; MenuItem { text: "Диагностика"; checkable: true; checked: diagnosticsVisible; onTriggered: diagnosticsVisible = checked } }
        Menu { title: "Справка"; MenuItem { text: "О программе"; onTriggered: statusText.text = "Electrical Engineering Platform — P1 Qt baseline" } }
    }

    header: ToolBar {
        RowLayout {
            anchors.fill: parent
            ToolButton { text: "Открыть"; onClicked: statusText.text = "Открытие demo fixture…" }
            ToolButton { text: "Сохранить"; onClicked: statusText.text = "Demo state сохранён" }
            ToolSeparator {}
            ToolButton { text: "Отменить" }
            ToolButton { text: "Повторить" }
            Item { Layout.fillWidth: true }
            Label { text: "P1 · Professional Shell / UI Gallery"; color: "#5d6875" }
        }
    }

    footer: ToolBar {
        visible: diagnosticsVisible
        RowLayout {
            anchors.fill: parent
            Label { id: statusText; text: p1.fixture.status.project; Layout.fillWidth: true }
            Label { text: p1.fixture.status.diagnostics }
            ToolSeparator {}
            Label { text: p1.fixture.status.connection }
        }
    }

    SplitView {
        anchors.fill: parent
        orientation: Qt.Horizontal

        Pane {
            visible: equipmentVisible
            SplitView.preferredWidth: 250
            SplitView.minimumWidth: 190
            ColumnLayout {
                anchors.fill: parent
                Label { text: "Оборудование"; font.bold: true; font.pixelSize: 15 }
                Rectangle { Layout.fillWidth: true; height: 1; color: "#d8dee6" }
                ListView {
                    id: equipmentList
                    Layout.fillWidth: true
                    Layout.fillHeight: true
                    clip: true
                    model: [
                        { label: "▾ ПС 110/35/10 кВ", id: "substation", indent: 0 },
                        { label: "▾ ОРУ 110 кВ", id: "oru110", indent: 14 },
                        { label: "▾ КРУ 35 кВ", id: "kru35", indent: 14 },
                        { label: "▾ Секция 1", id: "section35-1", indent: 28 },
                        { label: "QF-35-01", id: "QF-35-01", indent: 42 },
                        { label: "QS-35-01", id: "QS-35-01", indent: 42 },
                        { label: "QSG-35-01", id: "QSG-35-01", indent: 42 },
                        { label: "▸ Секция 2", id: "section35-2", indent: 28 },
                        { label: "▸ КРУ 10 кВ", id: "kru10", indent: 14 }
                    ]
                    delegate: ItemDelegate {
                        required property var modelData
                        width: ListView.view.width
                        leftPadding: 8 + modelData.indent
                        text: modelData.label
                        highlighted: p1.selectedEquipmentId === modelData.id
                        onClicked: p1.selectEquipment(modelData.id)
                    }
                }
            }
        }

        Pane {
            SplitView.fillWidth: true
            ColumnLayout {
                anchors.fill: parent
                TabBar {
                    id: tabs
                    Layout.fillWidth: true
                    currentIndex: root.currentDocument
                    onCurrentIndexChanged: root.currentDocument = currentIndex
                    Repeater {
                        model: p1.fixture.documents
                        TabButton { required property var modelData; text: modelData.title; width: Math.max(180, implicitWidth) }
                    }
                }
                StackLayout {
                    Layout.fillWidth: true
                    Layout.fillHeight: true
                    currentIndex: root.currentDocument
                    Repeater {
                        model: p1.fixture.documents
                        Loader {
                            required property var modelData
                            sourceComponent: modelData.kind === "ui-gallery" ? galleryComponent : documentComponent
                            property var documentData: modelData
                        }
                    }
                }
            }
        }

        Pane {
            visible: propertiesVisible
            SplitView.preferredWidth: 320
            SplitView.minimumWidth: 250
            ColumnLayout {
                anchors.fill: parent
                Label { text: "Свойства"; font.bold: true; font.pixelSize: 15 }
                Label { text: p1.selectedEquipment.designation || "—"; font.bold: true; font.pixelSize: 16 }
                Label { text: p1.selectedEquipment.name || "Выберите оборудование"; color: "#5d6875" }
                Rectangle { Layout.fillWidth: true; height: 1; color: "#d8dee6" }
                ScrollView {
                    Layout.fillWidth: true; Layout.fillHeight: true
                    Column {
                        width: parent.width
                        spacing: 8
                        Repeater {
                            model: p1.selectedEquipment.properties || []
                            Frame {
                                required property var modelData
                                width: parent.width
                                ColumnLayout {
                                    anchors.fill: parent
                                    Label { text: modelData.label; color: "#5d6875"; font.pixelSize: 12 }
                                    TextField { visible: modelData.editable; text: modelData.displayValue || modelData.value || ""; Layout.fillWidth: true }
                                    Label { visible: !modelData.editable; text: modelData.displayValue || modelData.value || ""; Layout.fillWidth: true }
                                    Label { visible: (modelData.message || "").length > 0; text: modelData.message || ""; wrapMode: Text.Wrap; color: modelData.state === "error" ? "#a02727" : "#7a5200"; Layout.fillWidth: true }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    Component {
        id: documentComponent
        Item {
            required property var documentData
            ColumnLayout {
                anchors.centerIn: parent
                width: Math.min(parent.width - 60, 620)
                spacing: 10
                Label { text: documentData.title; font.bold: true; font.pixelSize: 20; Layout.alignment: Qt.AlignHCenter }
                Label { text: "P1 presentation workspace"; color: "#5d6875"; Layout.alignment: Qt.AlignHCenter }
                Label { text: "Семантический холст и импорт намеренно не реализованы на P1."; wrapMode: Text.Wrap; horizontalAlignment: Text.AlignHCenter; Layout.fillWidth: true }
            }
        }
    }

    Component {
        id: galleryComponent
        ScrollView {
            required property var documentData
            contentWidth: availableWidth
            ColumnLayout {
                width: parent.width
                spacing: 12
                Label { text: "UI Gallery"; font.bold: true; font.pixelSize: 20 }
                Label { text: "Детерминированная P1 surface для desktop controls и русской типографики."; color: "#5d6875"; wrapMode: Text.Wrap; Layout.fillWidth: true }
                RowLayout { Button { text: "Основное действие"; highlighted: true }; Button { text: "Вторичное действие" }; Button { text: "Недоступно"; enabled: false } }
                GridLayout {
                    columns: 2; Layout.fillWidth: true
                    Label { text: "Текст" }; TextField { text: p1.fixture.gallery.textInput; Layout.fillWidth: true }
                    Label { text: "Номинальный ток" }; SpinBox { from: 0; to: 10000; value: p1.fixture.gallery.numericInput; stepSize: 100 }
                    Label { text: "Тип оборудования" }; ComboBox { model: p1.fixture.gallery.comboOptions; currentIndex: 0; Layout.fillWidth: true }
                    Label { text: "Флаг" }; CheckBox { text: "Учитывать в проверке"; checked: p1.fixture.gallery.checkbox }
                    Label { text: "Режим" }; RowLayout { RadioButton { text: "Работа"; checked: true }; RadioButton { text: "Ремонт" } }
                }
                Frame { Layout.fillWidth: true; RowLayout { anchors.fill: parent; Label { text: "Редактируемое свойство"; Layout.preferredWidth: 190 }; TextField { text: "QF-35-01"; Layout.fillWidth: true } } }
                Frame { Layout.fillWidth: true; RowLayout { anchors.fill: parent; Label { text: "Только чтение"; Layout.preferredWidth: 190 }; Label { text: "Выключатель 35 кВ"; Layout.fillWidth: true } } }
                Label { text: "⚠ Предупреждение · 2500 А"; color: "#7a5200" }
                Label { text: "⛔ Ошибка · Неподтверждённое значение"; color: "#a02727" }
                Label { text: "UNKNOWN · Состояние неизвестно"; color: "#4d5864" }
                RowLayout { Label { text: "Норма"; color: "#176b3a" }; Label { text: "Предупреждение"; color: "#7a5200" }; Label { text: "Ошибка"; color: "#a02727" }; Label { text: "UNKNOWN"; color: "#4d5864" } }
                Frame { Layout.fillWidth: true; Label { anchors.fill: parent; text: "ℹ Fixture загружен · ⚠ Требуется проверка · ⛔ Ошибка валидации"; wrapMode: Text.Wrap } }
                Label { text: p1.fixture.gallery.longLabel; font.bold: true; wrapMode: Text.Wrap; Layout.fillWidth: true }
                Label { text: "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ · абвгдеёжзийклмнопрстуфхцчшщъыьэюя"; wrapMode: Text.Wrap; Layout.fillWidth: true }
                Label { text: "Номинальный ток — 2500 А · Активная мощность: 52,4 МВт · Реактивная мощность: −4,8 Мвар · № 12 · ΔP = 1,5 %"; wrapMode: Text.Wrap; Layout.fillWidth: true }
                Label { text: p1.fixture.gallery.multilineError; color: "#a02727"; wrapMode: Text.Wrap; Layout.fillWidth: true }
                Item { Layout.fillHeight: true }
            }
        }
    }
}
