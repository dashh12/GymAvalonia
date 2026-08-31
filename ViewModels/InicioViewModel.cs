using CommunityToolkit.Mvvm.Input;

namespace MyAvaloniaApp.ViewModels;

public partial class InicioViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;

    public InicioViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    private void IrAMiembros()
    {
        _mainViewModel.CurrentViewModel =
            new MiembrosViewModel(_mainViewModel);
    }

    [RelayCommand]
    private void IrAEntrenadores()
    {
        _mainViewModel.CurrentViewModel =
            new EntrenadoresViewModel(_mainViewModel);
    }
    [RelayCommand]
    private void IrAPagos()
    {
       _mainViewModel.CurrentViewModel =
            new PagosViewModel(_mainViewModel);
    }
}