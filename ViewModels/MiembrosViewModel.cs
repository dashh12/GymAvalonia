using CommunityToolkit.Mvvm.Input;

namespace MyAvaloniaApp.ViewModels;

public partial class MiembrosViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;

    public MiembrosViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }
    [RelayCommand]
    private void VolverInicio()
    {
        _mainViewModel.CurrentViewModel =
        new InicioViewModel(_mainViewModel);
    }
}