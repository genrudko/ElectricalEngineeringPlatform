using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Eep.PlatformStack.P2.Semantics;

namespace Eep.PlatformStack.P1.Avalonia;

public sealed partial class MainWindow
{
    private SemanticSchemeCanvas? _p2Canvas;
    private Grid? _p2Host;
    private P2SceneTierManifest? _p2TierManifest;
    private P2ViewportMode _p2ViewportMode = P2ViewportMode.Normal;
    private string _p2Tier = "DEMO";

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        InstallP2Canvas();
    }

    private void InstallP2Canvas()
    {
        if (_p2Host is not null)
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

        _p2TierManifest = P2SceneTierManifest.Load();
        var host = new Grid
        {
            RowDefinitions = RowDefinitions.Parse("Auto,*"),
            Background = new SolidColorBrush(Color.Parse("#FFFFFF"))
        };
        host.Children.Add(BuildP2Toolbar());
        _p2Host = host;

        tabs[0].Content = host;
        DocumentTabs.ItemsSource = tabs;
        LoadP2Tier("DEMO");
    }

    private Control BuildP2Toolbar()
    {
        var root = new StackPanel { Spacing = 6 };
        root.Children.Add(new TextBlock
        {
            Text = "P2 · Semantic Scheme Canvas · topology отдельно от SchemeView geometry",
            FontSize = 12,
            FontWeight = FontWeight.SemiBold,
            Foreground = new SolidColorBrush(Color.Parse("#34404C"))
        });

        var controls = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
        controls.Children.Add(new TextBlock { Text = "Scene:", VerticalAlignment = VerticalAlignment.Center, Margin = new global::Avalonia.Thickness(0, 0, 3, 0) });
        foreach (var tier in new[] { "DEMO", "S", "M", "L", "XL" })
        {
            controls.Children.Add(P2Button(tier, () => LoadP2Tier(tier)));
        }
        controls.Children.Add(new Border { Width = 1, Height = 24, Background = new SolidColorBrush(Color.Parse("#D8DEE6")), Margin = new global::Avalonia.Thickness(6, 0) });
        controls.Children.Add(new TextBlock { Text = "Viewport:", VerticalAlignment = VerticalAlignment.Center, Margin = new global::Avalonia.Thickness(0, 0, 3, 0) });
        controls.Children.Add(P2Button("NORMAL ~500", () => SetP2ViewportMode(P2ViewportMode.Normal)));
        controls.Children.Add(P2Button("DENSE ~2000", () => SetP2ViewportMode(P2ViewportMode.Dense)));
        controls.Children.Add(P2Button("FIT", () => SetP2ViewportMode(P2ViewportMode.ZoomToFit)));
        root.Children.Add(controls);

        return new Border
        {
            Background = new SolidColorBrush(Color.Parse("#F7F9FB")),
            BorderBrush = new SolidColorBrush(Color.Parse("#D8DEE6")),
            BorderThickness = new global::Avalonia.Thickness(0, 0, 0, 1),
            Padding = new global::Avalonia.Thickness(10, 7),
            Child = root
        };
    }

    private static Button P2Button(string text, Action action)
    {
        var button = new Button { Content = text, Padding = new global::Avalonia.Thickness(9, 4) };
        button.Click += (_, _) => action();
        return button;
    }

    private void LoadP2Tier(string tier)
    {
        if (_p2Host is null || _p2TierManifest is null)
        {
            return;
        }

        P2SemanticScene scene;
        GeneratedP2Scene? generated = null;
        if (tier == "DEMO")
        {
            scene = P2SemanticFixtureLoader.Load();
        }
        else
        {
            generated = P2SceneScaleGenerator.Generate(tier, _p2TierManifest);
            scene = generated.Scene;
        }

        if (_p2Canvas is not null)
        {
            _p2Host.Children.Remove(_p2Canvas);
        }

        var canvas = new SemanticSchemeCanvas(scene, tier)
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            MinHeight = 420
        };
        canvas.StatusChanged += message => StatusText.Text = message;
        canvas.EntitySelected += HandleP2EntitySelected;
        Grid.SetRow(canvas, 1);
        _p2Host.Children.Add(canvas);
        _p2Canvas = canvas;
        _p2Tier = tier;
        canvas.ApplyViewportMode(_p2ViewportMode);

        if (generated is null)
        {
            StatusText.Text = $"P2 DEMO loaded · semantic={scene.Entities.Count + scene.Terminals.Count + scene.Connections.Count}";
        }
        else
        {
            StatusText.Text = $"P2 {tier} loaded · semantic={generated.Counts.SemanticElementCount:N0} · entities={generated.Counts.EntityCount:N0} · sha={generated.FingerprintSha256[..12]}";
        }
    }

    private void SetP2ViewportMode(P2ViewportMode mode)
    {
        _p2ViewportMode = mode;
        _p2Canvas?.ApplyViewportMode(mode);
        StatusText.Text = $"P2 {_p2Tier} viewport → {mode}";
    }

    private void HandleP2EntitySelected(string entityId)
    {
        if (ApplyEquipmentSelection(entityId))
        {
            return;
        }

        var canvas = _p2Canvas;
        if (canvas is null || !canvas.Controller.Scene.Entities.TryGetValue(entityId, out var entity))
        {
            return;
        }

        var placement = canvas.Controller.Scene.Placements[entityId].Position;
        PropertiesHost.Children.Clear();
        PropertiesHost.Children.Add(new TextBlock { Text = entity.Designation, FontSize = 17, FontWeight = FontWeight.SemiBold });
        PropertiesHost.Children.Add(new TextBlock
        {
            Text = "P2 generated semantic scene",
            Foreground = new SolidColorBrush(Color.Parse("#5D6875")),
            Margin = new global::Avalonia.Thickness(0, 0, 0, 6)
        });
        AddP2InspectorRow("Semantic kind", entity.Kind.ToString());
        AddP2InspectorRow("State", entity.State.ToString());
        AddP2InspectorRow("Terminals", entity.Terminals.Count.ToString());
        AddP2InspectorRow("View X", placement.X.ToString("F1"));
        AddP2InspectorRow("View Y", placement.Y.ToString("F1"));
        AddP2InspectorRow("Topology identity", entity.Id);

        foreach (var marker in canvas.Controller.Scene.ValidationMarkers.Where(marker => marker.EntityId == entityId))
        {
            PropertiesHost.Children.Add(new Border
            {
                Background = new SolidColorBrush(Color.Parse("#FFF9E8")),
                BorderBrush = new SolidColorBrush(Color.Parse("#E4C565")),
                BorderThickness = new global::Avalonia.Thickness(1),
                CornerRadius = new global::Avalonia.CornerRadius(3),
                Padding = new global::Avalonia.Thickness(7),
                Child = new TextBlock { Text = $"{marker.Layer}: {marker.Message}", TextWrapping = TextWrapping.Wrap }
            });
        }
    }

    private void AddP2InspectorRow(string label, string value)
    {
        var row = new Grid { ColumnDefinitions = ColumnDefinitions.Parse("120,*"), Margin = new global::Avalonia.Thickness(0, 2) };
        row.Children.Add(new TextBlock { Text = label, Foreground = new SolidColorBrush(Color.Parse("#5D6875")) });
        var valueText = new TextBlock { Text = value, TextWrapping = TextWrapping.Wrap };
        Grid.SetColumn(valueText, 1);
        row.Children.Add(valueText);
        PropertiesHost.Children.Add(row);
    }
}
