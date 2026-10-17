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

public partial class MaquinariaViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly ApiService _api = new();
    private List<Maquinaria> _todas = new();

    // Opciones de la lista desplegable
    public string[] Estados { get; } = { "Operativa", "En mantenimiento", "Fuera de servicio" };

    // Lista que se ve en pantalla
    public ObservableCollection<Maquinaria> Maquinas { get; } = new();

    [ObservableProperty] private string _busqueda = "";
    [ObservableProperty] private string _nombre = "";
    [ObservableProperty] private string _musculo = "";
    [ObservableProperty] private string _estado = "Operativa";
    [ObservableProperty] private string _mensaje = "";
    [ObservableProperty] private Maquinaria? _seleccionada;

    public MaquinariaViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _ = Cargar();
    }

    // Cada vez que se escribe en el buscador, se filtra la lista
    partial void OnBusquedaChanged(string value) => Filtrar();

    // Al elegir una de la lista, sus datos pasan al formulario
    partial void OnSeleccionadaChanged(Maquinaria? value)
    {
        if (value == null)
            return;

        Nombre = value.Nombre;
        Musculo = value.Musculo;
        Estado = value.Estado;
    }

    //CONSULTAS LINQ
    private void Filtrar()
    {
        var texto = Busqueda.ToLower();

        var filtradas = _todas
            .Where(m => m.Nombre.ToLower().Contains(texto)
                     || m.Musculo.ToLower().Contains(texto)
                     || m.Estado.ToLower().Contains(texto))
            .OrderBy(m => m.Nombre);

        Maquinas.Clear();
        foreach (var maquina in filtradas)
            Maquinas.Add(maquina);
    }

    // Trae las máquinas desde la API
    [RelayCommand]
    private async Task Cargar()
    {
        try
        {
            _todas = await _api.Listar<Maquinaria>("api/maquinaria");
            Filtrar();
        }
        catch (Exception ex)
        {
            Mensaje = ex.Message;
        }
    }

    // Si hay una elegida la modifica, si no crea una nueva
    [RelayCommand]
    private async Task Guardar()
    {
        if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Musculo))
        {
            Mensaje = "Completá el nombre y el músculo.";
            return;
        }

        var maquina = new Maquinaria
        {
            Id = Seleccionada?.Id ?? 0,
            Nombre = Nombre,
            Musculo = Musculo,
            Estado = Estado,
            FechaUltimoMantenimiento = Seleccionada?.FechaUltimoMantenimiento ?? DateTime.Now
        };

        try
        {
            if (Seleccionada == null)
                await _api.Crear("api/maquinaria", maquina);
            else
                await _api.Modificar($"api/maquinaria/{maquina.Id}", maquina);

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
        if (Seleccionada == null)
        {
            Mensaje = "Elegí una máquina de la lista.";
            return;
        }

        try
        {
            await _api.Eliminar($"api/maquinaria/{Seleccionada.Id}");
            await Cargar();
            Limpiar();
            Mensaje = "Máquina eliminada.";
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
        Seleccionada = null;
        Nombre = "";
        Musculo = "";
        Estado = "Operativa";
    }
}
