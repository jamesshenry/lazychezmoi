using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyChezmoi.Navigation;
using LazyChezmoi.Services;

namespace LazyChezmoi.ViewModels;

public partial class MainViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private string _appTitle = "lazychezmoi";

    [ObservableProperty]
    private string _statusText = "Ready";

    public MainViewModel(INavigationService navigationService, IDialogService dialogService)
    {
        _navigationService = navigationService;
        _dialogService = dialogService;
    }

    public void OnNavigatedFrom()
    {
        throw new NotImplementedException();
    }

    public void OnNavigatedTo()
    {
        throw new NotImplementedException();
    }

    [RelayCommand]
    private void NavigateHome() => _navigationService.NavigateTo<HomeViewModel>();

    [RelayCommand]
    private void NavigateSettings() => _navigationService.NavigateTo<SettingsViewModel>();

    [RelayCommand]
    private void ShowAbout()
    {
        _dialogService.ShowError("About", "LazyChezmoi: A Terminal.Gui v2 MVVM Demo");
    }
}
