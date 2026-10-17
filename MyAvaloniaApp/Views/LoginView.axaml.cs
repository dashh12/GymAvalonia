using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MyAvaloniaApp.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}