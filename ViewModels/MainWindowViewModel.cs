using CommunityToolkit.Mvvm.ComponentModel;

namespace MyAvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentViewModel;

    public MainWindowViewModel()
    {
        _currentViewModel = new LoginViewModel(this);
    }
}