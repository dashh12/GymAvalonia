using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MyAvaloniaApp.Views;

public partial class MiembrosView : UserControl
{
    public MiembrosView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}