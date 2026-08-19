using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace Eep.PlatformStack.P1.Avalonia;

public sealed partial class MainWindow
{
    private SemanticSchemeCanvas? _p2Canvas;

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        InstallP2Canvas();
    }

    private void InstallP2Canvas()
    {
        if (_p2Canvas is not null)
        {
            return;
        }

        if (DocumentTabs.ItemsSource is not IEnumerable<TabItem> source)
        {
            throw new InvalidOperationException("P2 canvas cannot mount because document tabs are unavailable.");
        }

        var tabs = source.ToList();
        if (tabs.Count == 0)
        {
            throw new InvalidOperationException("P2 canvas cannot mount because no document tabs exist.");
        }

        var canvas = new SemanticSchemeCanvas
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            MinHeight = 420
        };
        canvas.StatusChanged += message => StatusText.Text = message;
        canvas.EntitySelected += entityId => ApplyEquipmentSelection(entityId);

        var header = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#F7F9FB")),
            BorderBrush = new SolidColorBrush(Color.Parse("#D8DEE6")),
            BorderThickness = new global::Avalonia.Thickness(0, 0, 0, 1),
            Padding = new global::Avalonia.Thickness(10, 7),
            Child = new TextBlock
            {
                Text = "P2 · Semantic Scheme Canvas · electrical topology отдельно от geometry · connection + Ctrl+click terminal = reconnect",
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.Parse("#4D5864"))
            }
        };

        var host = new Grid
        {
            RowDefinitions = RowDefinitions.Parse("Auto,*"),
            Background = new SolidColorBrush(Color.Parse("#FFFFFF"))
        };
        host.Children.Add(header);
        Grid.SetRow(canvas, 1);
        host.Children.Add(canvas);

        tabs[0].Content = host;
        DocumentTabs.ItemsSource = tabs;
        _p2Canvas = canvas;
        StatusText.Text = "P2 Semantic Scheme Canvas baseline loaded";
    }
}
