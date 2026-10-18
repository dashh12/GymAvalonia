using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAvaloniaApp.Models;
using MyAvaloniaApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvaloniaApp.ViewModels;

public partial class MiembrosViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly ApiService _api = new();
    private List<Miembro> _todos = new();

    public ObservableCollection<Miembro> Miembros { get; } = new();
    public ObservableCollection<Membresia> Membresias { get; } = new();

    public ObservableCollection<Pago> MetodosPago { get; } = new()
    {
        new Pago { Id = 1, Metodo = "Efectivo" },
        new Pago { Id = 2, Metodo = "Transferencia" },
    };

    public ObservableCollection<Rutina> Rutinas { get; } = new()
    {
        new Rutina { Id = 1, Descripcion = "Full body 3 días", NivelDificultad = "Principiante", EntrenadorId = 1 },
        new Rutina { Id = 2, Descripcion = "Pecho y espalda", NivelDificultad = "Intermedio", EntrenadorId = 1 },
        new Rutina { Id = 3, Descripcion = "Circuito funcional", NivelDificultad = "Intermedio", EntrenadorId = 2 },
    };

    [ObservableProperty] private string _busqueda = "";
    [ObservableProperty] private string _nombre = "";
    [ObservableProperty] private string _apellido = "";
    [ObservableProperty] private string _email = "";
    [ObservableProperty] private Membresia? _membresiaElegida;
    [ObservableProperty] private Rutina? _rutinaElegida;
    [ObservableProperty] private Pago? _pagoElegido;
    [ObservableProperty] private string _mensaje = "";
    [ObservableProperty] private Miembro? _seleccionado;

    public MiembrosViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _ = Cargar();
    }

    partial void OnBusquedaChanged(string value) => Filtrar();

    partial void OnSeleccionadoChanged(Miembro? value)
    {
        if (value == null)
            return;

        Nombre = value.Nombre;
        Apellido = value.Apellido;
        Email = value.Email;
        MembresiaElegida = Membresias.FirstOrDefault(m => m.Id == value.MembresiaId);
        RutinaElegida = Rutinas.FirstOrDefault(r => r.Id == value.RutinaId);
        PagoElegido = MetodosPago.FirstOrDefault(p => p.Id == value.PagoId);
    }

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
            RutinaId = RutinaElegida?.Id,
            PagoId = PagoElegido?.Id
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
        RutinaElegida = null;
        PagoElegido = null;
    }
}