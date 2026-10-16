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

public partial class EntrenadoresViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly ApiService _api = new();
    private List<Entrenador> _todos = new();

    // Lista que se ve en pantalla
    public ObservableCollection<Entrenador> Entrenadores { get; } = new();

    [ObservableProperty] private string _busqueda = "";
    [ObservableProperty] private string _nombre = "";
    [ObservableProperty] private string _apellido = "";
    [ObservableProperty] private string _especialidad = "";
    [ObservableProperty] private string _mensaje = "";
    [ObservableProperty] private Entrenador? _seleccionado;

    public EntrenadoresViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _ = Cargar();
    }

    // Cada vez que se escribe en el buscador, se filtra la lista
    partial void OnBusquedaChanged(string value) => Filtrar();

    // Al elegir uno de la lista, sus datos pasan al formulario
    partial void OnSeleccionadoChanged(Entrenador? value)
    {
        if (value == null)
            return;

        Nombre = value.Nombre;
        Apellido = value.Apellido;
        Especialidad = value.Especialidad;
    }

    // Consulta LINQ: busca por texto y ordena por apellido
    private void Filtrar()
    {
        var texto = Busqueda.ToLower();

        var filtrados = _todos
            .Where(e => e.Nombre.ToLower().Contains(texto)
                     || e.Apellido.ToLower().Contains(texto)
                     || e.Especialidad.ToLower().Contains(texto))
            .OrderBy(e => e.Apellido);

        Entrenadores.Clear();
        foreach (var entrenador in filtrados)
            Entrenadores.Add(entrenador);
    }

    // Trae los entrenadores desde la API
    [RelayCommand]
    private async Task Cargar()
    {
        try
        {
            _todos = await _api.Listar<Entrenador>("api/entrenadores");
            Filtrar();
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
        if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Apellido) || string.IsNullOrWhiteSpace(Especialidad))
        {
            Mensaje = "Completá todos los campos.";
            return;
        }

        var entrenador = new Entrenador
        {
            Id = Seleccionado?.Id ?? 0,
            Nombre = Nombre,
            Apellido = Apellido,
            Especialidad = Especialidad
        };

        try
        {
            if (Seleccionado == null)
                await _api.Crear("api/entrenadores", entrenador);
            else
                await _api.Modificar($"api/entrenadores/{entrenador.Id}", entrenador);

            await Cargar();
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
            Mensaje = "Elegí un entrenador de la lista.";
            return;
        }

        try
        {
            await _api.Eliminar($"api/entrenadores/{Seleccionado.Id}");
            await Cargar();
            Limpiar();
            Mensaje = "Entrenador eliminado.";
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
        Especialidad = "";
    }
}
