using Avalonia.Controls;
using MyAvaloniaApp.ViewModels;

namespace MyAvaloniaApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}