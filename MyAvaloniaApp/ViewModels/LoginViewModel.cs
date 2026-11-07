using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Models;
using MyAvaloniaApp.Services;


namespace MyAvaloniaApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly ApiService _api = new();

    [ObservableProperty] private string _usuario = "";
    [ObservableProperty] private string _contrasena = "";
    [ObservableProperty] private string _mensaje = "";

    public LoginViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    private async Task IniciarSesion()
    {
        if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contrasena))
        {
            Mensaje = "Ingresá usuario y contraseña.";
            return;
        }

        try
        {
            await _api.Crear("api/usuarios/login", new Credenciales { NombreUsuario = Usuario, Contrasena = Contrasena });
            _mainViewModel.CurrentViewModel = new InicioViewModel(_mainViewModel);
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    [RelayCommand]
    private void IrARegistro()
    {
        _mainViewModel.CurrentViewModel = new RegistroViewModel(_mainViewModel);
    }
}