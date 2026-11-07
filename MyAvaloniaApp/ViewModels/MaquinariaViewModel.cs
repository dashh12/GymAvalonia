using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
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

    public string[] Musculos => MusculoImagenes.Musculos;
    public string[] Estados { get; } = { "Operativa", "En mantenimiento", "Fuera de servicio" };
    public ObservableCollection<Maquinaria> Maquinas { get; } = new();

    public Bitmap? ImagenMusculo => MusculoImagenes.Cargar(Musculo);

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

    partial void OnBusquedaChanged(string value) => Filtrar();

    partial void OnMusculoChanged(string value) => OnPropertyChanged(nameof(ImagenMusculo));

    partial void OnSeleccionadaChanged(Maquinaria? value)
    {
        if (value == null)
            return;

        Nombre = value.Nombre;
        Musculo = Musculos.FirstOrDefault(m => m.Equals(value.Musculo, StringComparison.OrdinalIgnoreCase)) ?? value.Musculo;
        Estado = value.Estado;
    }

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