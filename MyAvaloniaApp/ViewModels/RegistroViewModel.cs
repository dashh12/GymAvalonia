using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Models;
using MyAvaloniaApp.Services;
using System;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace MyAvaloniaApp.ViewModels;

public partial class RegistroViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly ApiService _api = new();

    [ObservableProperty] private string _usuario = "";
    [ObservableProperty] private string _contrasena = "";
    [ObservableProperty] private string _repetirContrasena = "";
    [ObservableProperty] private string _mensaje = "";

    public RegistroViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    private async Task Registrar()
    {
        if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contrasena))
        {
            Mensaje = "Completá usuario y contraseña.";
            return;
        }

        if (Usuario.Trim().Length < 3)
        {
            Mensaje = "El usuario debe tener al menos 3 caracteres.";
            return;
        }

        if (Contrasena.Length < 4)
        {
            Mensaje = "La contraseña debe tener al menos 4 caracteres.";
            return;
        }

        if (Contrasena != RepetirContrasena)
        {
            Mensaje = "Las contraseñas no coinciden.";
            return;
        }

        try
        {
            await _api.Crear("api/usuarios/registro", new Credenciales { NombreUsuario = Usuario, Contrasena = Contrasena });
            _mainViewModel.CurrentViewModel = new LoginViewModel(_mainViewModel) { Mensaje = "Cuenta creada. Ya podés iniciar sesión." };
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    [RelayCommand]
    private void VolverLogin()
    {
        _mainViewModel.CurrentViewModel = new LoginViewModel(_mainViewModel);
    }
}