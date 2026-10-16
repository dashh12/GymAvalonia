using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Models;
using MyAvaloniaApp.Services;

namespace MyAvaloniaApp.ViewModels;

public partial class MiembrosViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly ApiService _api = new();
    private List<Miembro> _todos = new();

    // Lista que se ve en pantalla y opciones de la lista desplegable
    public ObservableCollection<Miembro> Miembros { get; } = new();
    public ObservableCollection<Membresia> Membresias { get; } = new();

    [ObservableProperty] private string _busqueda = "";
    [ObservableProperty] private string _nombre = "";
    [ObservableProperty] private string _apellido = "";
    [ObservableProperty] private string _email = "";
    [ObservableProperty] private Membresia? _membresiaElegida;
    [ObservableProperty] private string _mensaje = "";
    [ObservableProperty] private Miembro? _seleccionado;

    public MiembrosViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _ = Cargar();
    }

    // Cada vez que se escribe en el buscador, se filtra la lista
    partial void OnBusquedaChanged(string value) => Filtrar();

    // Al elegir uno de la lista, sus datos pasan al formulario
    partial void OnSeleccionadoChanged(Miembro? value)
    {
        if (value == null)
            return;

        Nombre = value.Nombre;
        Apellido = value.Apellido;
        Email = value.Email;
        MembresiaElegida = Membresias.FirstOrDefault(m => m.Id == value.MembresiaId);
    }

    //CONSULTAS LINQ
    private void Filtrar()
    {
        var texto = Busqueda.ToLower();

        var filtrados = _todos
            .Where(m => m.Nombre.ToLower().Contains(texto)
                     || m.Apellido.ToLower().Contains(texto)
                     || m.Email.ToLower().Contains(texto)
                     || (m.Membresia != null && m.Membresia.Tipo.ToLower().Contains(texto)))
            .OrderBy(m => m.Apellido);

        Miembros.Clear();
        foreach (var miembro in filtrados)
            Miembros.Add(miembro);
    }

    private async Task CargarMiembros()
    {
        _todos = await _api.Listar<Miembro>("api/miembros");
        Filtrar();
    }

    // Trae las membresías (para la lista desplegable) y los miembros desde la API
    [RelayCommand]
    private async Task Cargar()
    {
        try
        {
            var membresias = await _api.Listar<Membresia>("api/membresias");
            Membresias.Clear();
            foreach (var membresia in membresias)
                Membresias.Add(membresia);

            await CargarMiembros();
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    // Si hay uno elegido lo modifica, si no crea uno nuevo
    [RelayCommand]
    private async Task Guardar()
    {
        if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Apellido) || string.IsNullOrWhiteSpace(Email))
        {
            Mensaje = "Completá nombre, apellido y email.";
            return;
        }

        if (!Email.Contains('@'))
        {
            Mensaje = "El email no es válido.";
            return;
        }

        if (MembresiaElegida == null)
        {
            Mensaje = "Elegí una membresía.";
            return;
        }

        var miembro = new Miembro
        {
            Id = Seleccionado?.Id ?? 0,
            Nombre = Nombre,
            Apellido = Apellido,
            Email = Email,
            MembresiaId = MembresiaElegida.Id,
            RutinaId = Seleccionado?.RutinaId
        };

        try
        {
            if (Seleccionado == null)
                await _api.Crear("api/miembros", miembro);
            else
                await _api.Modificar($"api/miembros/{miembro.Id}", miembro);

            await CargarMiembros();
            Limpiar();
            Mensaje = "Guardado correctamente.";
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    [RelayCommand]
    private async Task Eliminar()
    {
        if (Seleccionado == null)
        {
            Mensaje = "Elegí un miembro de la lista.";
            return;
        }

        try
        {
            await _api.Eliminar($"api/miembros/{Seleccionado.Id}");
            await CargarMiembros();
            Limpiar();
            Mensaje = "Miembro eliminado.";
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    [RelayCommand]
    private void Nuevo()
    {
        Limpiar();
        Mensaje = "";
    }

    [RelayCommand]
    private void VolverInicio()
    {
        _mainViewModel.CurrentViewModel = new InicioViewModel(_mainViewModel);
    }

    private void Limpiar()
    {
        Seleccionado = null;
        Nombre = "";
        Apellido = "";
        Email = "";
        MembresiaElegida = null;
    }
}
