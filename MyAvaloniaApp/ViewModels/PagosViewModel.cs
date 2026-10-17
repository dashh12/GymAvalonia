using CommunityToolkit.Mvvm.Input;

namespace MyAvaloniaApp.ViewModels;

public partial class PagosViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;

    public PagosViewModel(MainWindowViewModel mainViewModel)
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