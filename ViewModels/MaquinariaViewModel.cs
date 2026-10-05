using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace MyAvaloniaApp.ViewModels;

public class Maquina
{
    public string Nombre { get; set; } = "";
    public string Musculo { get; set; } = "";
}

public partial class MaquinariaViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainViewModel;

    public ObservableCollection<Maquina> Maquinas { get; } = new()
    {
        new Maquina { Nombre = "Press de banca", Musculo = "Pecho" },
        new Maquina { Nombre = "Peck deck", Musculo = "Pecho" },
        new Maquina { Nombre = "Polea alta", Musculo = "Espalda" },
        new Maquina { Nombre = "Remo en máquina", Musculo = "Espalda" },
        new Maquina { Nombre = "Press de hombros", Musculo = "Hombros" },
        new Maquina { Nombre = "Curl de bíceps", Musculo = "Bíceps" },
        new Maquina { Nombre = "Extensión de tríceps", Musculo = "Tríceps" },
        new Maquina { Nombre = "Prensa de piernas", Musculo = "Piernas" },
        new Maquina { Nombre = "Camilla de cuádriceps", Musculo = "Cuádriceps" },
        new Maquina { Nombre = "Camilla femoral", Musculo = "Isquiotibiales" },
        new Maquina { Nombre = "Abductores", Musculo = "Glúteos" },
        new Maquina { Nombre = "Abdominales en máquina", Musculo = "Abdomen" },
    };

    // Constructor que usa la navegación
    public MaquinariaViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    // Constructor para el previsualizador de Avalonia
    public MaquinariaViewModel()
    {
    }

    [RelayCommand]
    private void VolverAlInicio()
    {
        if (_mainViewModel is null)
            return;

        _mainViewModel.CurrentViewModel = new InicioViewModel(_mainViewModel);
    }
}