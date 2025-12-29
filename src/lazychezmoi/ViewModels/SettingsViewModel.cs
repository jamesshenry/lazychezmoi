using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyChezmoi.Navigation;
using LazyChezmoi.Services;

namespace LazyChezmoi.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navService;

    [ObservableProperty]
    private string _username = "Guest";

    [ObservableProperty]
    private bool _enableLogging = true;

    [ObservableProperty]
    private string _theme = "Base";

    public SettingsViewModel(IDialogService dialogService, INavigationService navService)
    {
        _dialogService = dialogService;
        _navService = navService;
    }

    [RelayCommand]
    private void Save()
    {
        // In a real app, you'd save this to config.json here
        _dialogService.ShowError("Success", $"Settings saved for {Username}!");
        _navService.NavigateTo<HomeViewModel>();
    }

    [RelayCommand]
    private void Cancel() => _navService.NavigateTo<HomeViewModel>();
}
