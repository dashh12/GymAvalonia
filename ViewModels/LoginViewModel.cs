using CommunityToolkit.Mvvm.Input;

namespace MyAvaloniaApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;

    public LoginViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    private void IniciarSesion()
    {
        _mainViewModel.CurrentViewModel = new InicioViewModel(_mainViewModel);
    }
}