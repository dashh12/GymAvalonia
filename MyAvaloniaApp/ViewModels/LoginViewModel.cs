using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tmds.DBus.Protocol;

namespace MyAvaloniaApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;

    [ObservableProperty] private string _usuario = "";
    [ObservableProperty] private string _contraseña = "";
    [ObservableProperty] private string _mensaje = "";

    public LoginViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    private void IniciarSesion()
    {
        if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contraseña))
        {
            Mensaje = "Ingresa usuario y contraseña.";
            return;
        }

        if (Usuario != "gym" || Contraseña != "123")
        {
            Mensaje = "Usuario o contraseña incorrectos.";
            return;
        }

        _mainViewModel.CurrentViewModel = new InicioViewModel(_mainViewModel);
    }
}