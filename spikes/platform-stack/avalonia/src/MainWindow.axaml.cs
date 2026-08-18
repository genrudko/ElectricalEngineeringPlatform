using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Markup.Xaml;

namespace Eep.PlatformStack.P1.Avalonia;

public sealed partial class MainWindow : Window
{
    private static readonly IReadOnlyDictionary<string, string> ShortcutMap = new Dictionary<string, string>
    {
        ["open"] = "Ctrl+O",
        ["save"] = "Ctrl+S",
        ["undo"] = "Ctrl+Z",
        ["redo"] = "Ctrl+Y"
    };

    private readonly P1Fixture _fixture;
    private readonly Dictionary<string, TreeViewItem> _treeItems = [];
    private readonly HashSet<string> _equipmentIds = [];
    private readonly List<string> _renderedPropertyStates = [];
    private readonly List<string> _renderedPropertyModes = [];
    private string _selectedEquipmentId = string.Empty;

    public MainWindow()
    {
        Console.WriteLine("P1 MainWindow: InitializeComponent begin");
        InitializeComponent();
        Console.WriteLine("P1 MainWindow: InitializeComponent complete");
        _fixture = P1FixtureLoader.Load();
        Console.WriteLine("P1 MainWindow: fixture loaded");
        Title = _fixture.ApplicationTitle;
        Console.WriteLine("P1 MainWindow: title assigned");

        Console.WriteLine("P1 MainWindow: BuildEquipmentTree begin");
        BuildEquipmentTree();
        Console.WriteLine("P1 MainWindow: BuildEquipmentTree complete");
        BuildDocumentTabs();
        Console.WriteLine("P1 MainWindow: BuildDocumentTabs complete");
        SelectEquipmentForTest(_fixture.SelectedEquipmentId);
        Console.WriteLine("P1 MainWindow: initial equipment selected");

        StatusText.Text = _fixture.Status.Project;
        DiagnosticsText.Text = _fixture.Status.Diagnostics;
        ConnectionText.Text = _fixture.Status.Connection;
        Console.WriteLine("P1 MainWindow: constructor complete");
    }

    public string FixtureSchema => _fixture.Schema;
    public IReadOnlyList<string> DocumentTitles => _fixture.Documents.Select(document => document.Title).ToArray();
    public IReadOnlyCollection<string> EquipmentIds => _equipmentIds;
    public string SelectedEquipmentId => _selectedEquipmentId;
    public IReadOnlyList<string> RenderedPropertyStates => _renderedPropertyStates;
    public IReadOnlyList<string> RenderedPropertyModes => _renderedPropertyModes;
    public IReadOnlyDictionary<string, string> Shortcuts => ShortcutMap;
    public int SelectedDocumentIndex => DocumentTabs.SelectedIndex;
    public bool IsUiGallerySelected => DocumentTabs.SelectedIndex == _fixture.Documents.FindIndex(document => document.Kind == "ui-gallery");

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void BuildEquipmentTree()
    {
        var root = CreateTreeItem(_fixture.EquipmentTree);
        EquipmentTree.ItemsSource = new[] { root };
    }

    private TreeViewItem CreateTreeItem(EquipmentNodeFixture node)
    {
        var item = new TreeViewItem
        {
            Header = node.Label,
            Tag = node.Id,
            IsExpanded = node.Expanded
        };

        _treeItems[node.Id] = item;
        _equipmentIds.Add(node.Id);

        if (node.Children.Count > 0)
        {
            item.ItemsSource = node.Children.Select(CreateTreeItem).ToArray();
        }

        return item;
    }

    private void BuildDocumentTabs()
    {
        var tabs = new List<TabItem>();
        foreach (var document in _fixture.Documents)
        {
            tabs.Add(new TabItem
            {
                Header = document.Title,
                Content = document.Kind == "ui-gallery" ? BuildUiGallery() : BuildDocumentPlaceholder(document)
            });
        }

        DocumentTabs.ItemsSource = tabs;
        DocumentTabs.SelectedIndex = 0;
    }

    private Control BuildDocumentPlaceholder(DocumentFixture document)
    {
        return new Grid
        {
            RowDefinitions = RowDefinitions.Parse("Auto,*"),
            Margin = new global::Avalonia.Thickness(18),
            Children =
            {
                new TextBlock
                {
                    Text = document.Title,
                    FontSize = 18,
                    FontWeight = FontWeight.SemiBold
                },
                new Border
                {
                    [Grid.RowProperty] = 1,
                    Margin = new global::Avalonia.Thickness(0, 16, 0, 0),
                    BorderBrush = new SolidColorBrush(Color.Parse("#D8DEE6")),
                    BorderThickness = new global::Avalonia.Thickness(1),
                    CornerRadius = new global::Avalonia.CornerRadius(4),
                    Background = new SolidColorBrush(Color.Parse("#FAFBFC")),
                    Padding = new global::Avalonia.Thickness(24),
                    Child = new StackPanel
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Spacing = 8,
                        Children =
                        {
                            new TextBlock { Text = "P1 presentation workspace", FontSize = 16, FontWeight = FontWeight.SemiBold },
                            new TextBlock
                            {
                                Text = "Семантический холст и импорт намеренно не реализованы на P1.",
                                Foreground = new SolidColorBrush(Color.Parse("#5D6875")),
                                TextWrapping = TextWrapping.Wrap
                            }
                        }
                    }
                }
            }
        };
    }

    private Control BuildUiGallery()
    {
        var gallery = new StackPanel { Spacing = 14, Margin = new global::Avalonia.Thickness(18) };
        gallery.Children.Add(new TextBlock { Text = "UI Gallery", FontSize = 20, FontWeight = FontWeight.SemiBold });
        gallery.Children.Add(new TextBlock
        {
            Text = "Детерминированная P1 surface для сопоставления базовых desktop controls и русской типографики.",
            Foreground = new SolidColorBrush(Color.Parse("#5D6875")),
            TextWrapping = TextWrapping.Wrap
        });

        var buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        var primary = new Button { Content = "Основное действие" };
        primary.Classes.Add("accent");
        buttons.Children.Add(primary);
        buttons.Children.Add(new Button { Content = "Вторичное действие" });
        buttons.Children.Add(new Button { Content = "Недоступно", IsEnabled = false });
        gallery.Children.Add(Section("Кнопки", buttons));

        var inputs = new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse("180,*,180,*"),
            RowDefinitions = RowDefinitions.Parse("Auto,Auto,Auto"),
            ColumnSpacing = 8,
            RowSpacing = 8
        };
        AddLabeledControl(inputs, 0, 0, "Текст", new TextBox { Text = _fixture.Gallery.TextInput });
        AddLabeledControl(inputs, 0, 2, "Номинальный ток", new NumericUpDown { Value = _fixture.Gallery.NumericInput, Minimum = 0, Maximum = 10000, Increment = 100 });
        AddLabeledControl(inputs, 1, 0, "Тип оборудования", new ComboBox { ItemsSource = _fixture.Gallery.ComboOptions, SelectedItem = _fixture.Gallery.ComboSelected });
        AddLabeledControl(inputs, 1, 2, "Флаг", new CheckBox { Content = "Учитывать в проверке", IsChecked = _fixture.Gallery.Checkbox });
        var radios = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };
        foreach (var option in _fixture.Gallery.RadioOptions)
        {
            radios.Children.Add(new RadioButton { Content = option, GroupName = "gallery-state", IsChecked = option == _fixture.Gallery.RadioSelected });
        }
        AddLabeledControl(inputs, 2, 0, "Режим", radios, columnSpan: 3);
        gallery.Children.Add(Section("Ввод и выбор", inputs));

        var propertyRows = new StackPanel { Spacing = 6 };
        propertyRows.Children.Add(GalleryPropertyRow("Редактируемое свойство", new TextBox { Text = "QF-35-01" }, "normal"));
        propertyRows.Children.Add(GalleryPropertyRow("Только чтение", new TextBlock { Text = "Выключатель 35 кВ", VerticalAlignment = VerticalAlignment.Center }, "read-only"));
        propertyRows.Children.Add(GalleryPropertyRow("Предупреждение", new TextBox { Text = "2500 А" }, "warning"));
        propertyRows.Children.Add(GalleryPropertyRow("Ошибка", new TextBox { Text = "Неподтверждённое значение" }, "error"));
        propertyRows.Children.Add(GalleryPropertyRow("UNKNOWN", new TextBlock { Text = "Состояние неизвестно", VerticalAlignment = VerticalAlignment.Center }, "unknown"));
        gallery.Children.Add(Section("Состояния свойств", propertyRows));

        var treeDemo = new TreeViewItem { Header = "КРУ 35 кВ", IsExpanded = true };
        treeDemo.ItemsSource = new[]
        {
            new TreeViewItem { Header = "QF-35-01", IsSelected = true },
            new TreeViewItem { Header = "QS-35-01" }
        };
        gallery.Children.Add(Section("Tree states", new TreeView { ItemsSource = new[] { treeDemo }, Height = 110 }));

        var badgeRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        badgeRow.Children.Add(Badge("Норма", "#E9F7EF", "#176B3A"));
        badgeRow.Children.Add(Badge("Предупреждение", "#FFF5D9", "#7A5200"));
        badgeRow.Children.Add(Badge("Ошибка", "#FDECEC", "#A02727"));
        badgeRow.Children.Add(Badge("UNKNOWN", "#EEF1F4", "#4D5864"));
        gallery.Children.Add(Section("Статусы", badgeRow));

        var notifications = new StackPanel { Spacing = 6 };
        foreach (var notification in _fixture.Gallery.Notifications)
        {
            notifications.Children.Add(Notification(notification));
        }
        gallery.Children.Add(Section("Уведомления", notifications));

        gallery.Children.Add(Section("Русская типографика", new StackPanel
        {
            Spacing = 6,
            Children =
            {
                new TextBlock { Text = _fixture.Gallery.LongLabel, FontWeight = FontWeight.SemiBold, TextWrapping = TextWrapping.Wrap },
                new TextBlock { Text = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ · абвгдеёжзийклмнопрстуфхцчшщъыьэюя", TextWrapping = TextWrapping.Wrap },
                new TextBlock { Text = "Номинальный ток — 2500 А · Активная мощность: 52,4 МВт · Реактивная мощность: −4,8 Мвар · № 12 · ΔP = 1,5 %", TextWrapping = TextWrapping.Wrap },
                new Border
                {
                    Background = new SolidColorBrush(Color.Parse("#FDECEC")),
                    BorderBrush = new SolidColorBrush(Color.Parse("#E6AAAA")),
                    BorderThickness = new global::Avalonia.Thickness(1),
                    CornerRadius = new global::Avalonia.CornerRadius(4),
                    Padding = new global::Avalonia.Thickness(10),
                    Child = new TextBlock { Text = _fixture.Gallery.MultilineError, TextWrapping = TextWrapping.Wrap }
                }
            }
        }));

        return new ScrollViewer { Content = gallery };
    }

    private static Border Section(string title, Control content) => new()
    {
        BorderBrush = new SolidColorBrush(Color.Parse("#D8DEE6")),
        BorderThickness = new global::Avalonia.Thickness(1),
        CornerRadius = new global::Avalonia.CornerRadius(4),
        Padding = new global::Avalonia.Thickness(12),
        Child = new StackPanel
        {
            Spacing = 10,
            Children =
            {
                new TextBlock { Text = title, FontWeight = FontWeight.SemiBold },
                content
            }
        }
    };

    private static void AddLabeledControl(Grid grid, int row, int column, string label, Control control, int columnSpan = 1)
    {
        var labelBlock = new TextBlock { Text = label, VerticalAlignment = VerticalAlignment.Center };
        Grid.SetRow(labelBlock, row);
        Grid.SetColumn(labelBlock, column);
        grid.Children.Add(labelBlock);

        Grid.SetRow(control, row);
        Grid.SetColumn(control, column + 1);
        Grid.SetColumnSpan(control, columnSpan);
        grid.Children.Add(control);
    }

    private static Border GalleryPropertyRow(string label, Control editor, string state)
    {
        var border = new Border
        {
            BorderThickness = new global::Avalonia.Thickness(1),
            BorderBrush = StateBrush(state),
            Background = StateBackground(state),
            CornerRadius = new global::Avalonia.CornerRadius(3),
            Padding = new global::Avalonia.Thickness(8),
            Child = new Grid
            {
                ColumnDefinitions = ColumnDefinitions.Parse("190,*"),
                Children =
                {
                    new TextBlock { Text = label, VerticalAlignment = VerticalAlignment.Center },
                    editor
                }
            }
        };
        Grid.SetColumn(editor, 1);
        return border;
    }

    private static Border Badge(string text, string background, string foreground) => new()
    {
        Background = new SolidColorBrush(Color.Parse(background)),
        CornerRadius = new global::Avalonia.CornerRadius(10),
        Padding = new global::Avalonia.Thickness(9, 3),
        Child = new TextBlock { Text = text, Foreground = new SolidColorBrush(Color.Parse(foreground)), FontSize = 12, FontWeight = FontWeight.SemiBold }
    };

    private static Border Notification(NotificationFixture notification)
    {
        var (background, border) = notification.Severity switch
        {
            "warning" => ("#FFF5D9", "#E4C565"),
            "error" => ("#FDECEC", "#E6AAAA"),
            _ => ("#EAF3FC", "#A9C9E8")
        };
        return new Border
        {
            Background = new SolidColorBrush(Color.Parse(background)),
            BorderBrush = new SolidColorBrush(Color.Parse(border)),
            BorderThickness = new global::Avalonia.Thickness(1),
            CornerRadius = new global::Avalonia.CornerRadius(4),
            Padding = new global::Avalonia.Thickness(9),
            Child = new TextBlock { Text = notification.Text, TextWrapping = TextWrapping.Wrap }
        };
    }

    private void EquipmentTree_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (EquipmentTree.SelectedItem is TreeViewItem { Tag: string id })
        {
            SelectEquipmentForTest(id);
        }
    }

    public bool SelectEquipmentForTest(string id)
    {
        if (!_fixture.Equipment.TryGetValue(id, out var equipment))
        {
            return false;
        }

        _selectedEquipmentId = id;
        if (_treeItems.TryGetValue(id, out var item) && !ReferenceEquals(EquipmentTree.SelectedItem, item))
        {
            EquipmentTree.SelectedItem = item;
        }

        RenderProperties(equipment);
        return true;
    }

    private void RenderProperties(EquipmentFixture equipment)
    {
        PropertiesHost.Children.Clear();
        _renderedPropertyStates.Clear();
        _renderedPropertyModes.Clear();

        PropertiesHost.Children.Add(new TextBlock { Text = equipment.Designation, FontSize = 17, FontWeight = FontWeight.SemiBold });
        PropertiesHost.Children.Add(new TextBlock { Text = equipment.Name, Foreground = new SolidColorBrush(Color.Parse("#5D6875")), Margin = new global::Avalonia.Thickness(0, 0, 0, 6) });

        foreach (var property in equipment.Properties)
        {
            _renderedPropertyStates.Add(property.State);
            _renderedPropertyModes.Add(property.Editable ? "editable" : "read-only");

            Control valueControl = property.Editable
                ? new TextBox { Text = property.DisplayValue ?? property.Value }
                : new TextBlock { Text = property.DisplayValue ?? property.Value, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Color.Parse("#4D5864")) };

            var rowContent = new StackPanel { Spacing = 4 };
            rowContent.Children.Add(new TextBlock { Text = property.Label, FontSize = 12, Foreground = new SolidColorBrush(Color.Parse("#5D6875")) });
            rowContent.Children.Add(valueControl);
            if (!string.IsNullOrWhiteSpace(property.Message))
            {
                rowContent.Children.Add(new TextBlock { Text = property.Message, FontSize = 11, Foreground = StateBrush(property.State), TextWrapping = TextWrapping.Wrap });
            }

            PropertiesHost.Children.Add(new Border
            {
                BorderBrush = StateBrush(property.State),
                BorderThickness = property.State == "normal" ? new global::Avalonia.Thickness(0) : new global::Avalonia.Thickness(1),
                Background = StateBackground(property.State),
                CornerRadius = new global::Avalonia.CornerRadius(3),
                Padding = new global::Avalonia.Thickness(7),
                Child = rowContent
            });
        }
    }

    private static IBrush StateBrush(string state) => new SolidColorBrush(Color.Parse(state switch
    {
        "warning" => "#B78300",
        "error" => "#B33A3A",
        "unknown" => "#697582",
        "read-only" => "#AEB7C1",
        _ => "#D8DEE6"
    }));

    private static IBrush StateBackground(string state) => new SolidColorBrush(Color.Parse(state switch
    {
        "warning" => "#FFF9E8",
        "error" => "#FFF4F4",
        "unknown" => "#F2F4F6",
        "read-only" => "#F7F8F9",
        _ => "#FFFFFF"
    }));

    public void SelectDocumentForTest(int index)
    {
        if (index < 0 || index >= _fixture.Documents.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        DocumentTabs.SelectedIndex = index;
    }

    private void MainWindow_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            return;
        }

        switch (e.Key)
        {
            case Key.O:
                ExecuteDemoCommand("Открыть: demo fixture state");
                e.Handled = true;
                break;
            case Key.S:
                ExecuteDemoCommand("Сохранить: demo fixture state");
                e.Handled = true;
                break;
            case Key.Z:
                ExecuteDemoCommand("Отменить: demo command");
                e.Handled = true;
                break;
            case Key.Y:
                ExecuteDemoCommand("Повторить: demo command");
                e.Handled = true;
                break;
        }
    }

    private void ExecuteDemoCommand(string message) => StatusText.Text = message;

    private void Open_OnClick(object? sender, RoutedEventArgs e) => ExecuteDemoCommand("Открыть: demo fixture state");
    private void Save_OnClick(object? sender, RoutedEventArgs e) => ExecuteDemoCommand("Сохранить: demo fixture state");
    private void Undo_OnClick(object? sender, RoutedEventArgs e) => ExecuteDemoCommand("Отменить: demo command");
    private void Redo_OnClick(object? sender, RoutedEventArgs e) => ExecuteDemoCommand("Повторить: demo command");
    private void Validate_OnClick(object? sender, RoutedEventArgs e) => ExecuteDemoCommand("Проверка: 2 замечания · 1 ошибка");
    private void Exit_OnClick(object? sender, RoutedEventArgs e) => Close();
    private void About_OnClick(object? sender, RoutedEventArgs e) => ExecuteDemoCommand("Electrical Engineering Platform · PLATFORM-STACK-SPIKE-001 · P1");
    private void ToggleEquipment_OnClick(object? sender, RoutedEventArgs e) => EquipmentPane.IsVisible = !EquipmentPane.IsVisible;
    private void ToggleProperties_OnClick(object? sender, RoutedEventArgs e) => PropertiesPane.IsVisible = !PropertiesPane.IsVisible;
    private void ToggleDiagnostics_OnClick(object? sender, RoutedEventArgs e) => DiagnosticsPane.IsVisible = !DiagnosticsPane.IsVisible;
}
