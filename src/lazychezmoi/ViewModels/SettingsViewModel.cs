using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyChezmoi.Navigation;
using LazyChezmoi.Services;
using LazyChezmoi.Views;
using Microsoft.Extensions.Logging;

namespace LazyChezmoi.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navService;
    private readonly ILogger<SettingsViewModel> _logger;

    [ObservableProperty]
    private string _username = "Guest";

    [ObservableProperty]
    private bool _enableLogging = true;

    [ObservableProperty]
    private string _theme = "Base";

    public SettingsViewModel(
        IDialogService dialogService,
        INavigationService navService,
        ILogger<SettingsViewModel> logger
    )
    {
        _dialogService = dialogService;
        _navService = navService;
        _logger = logger;
    }

    [RelayCommand]
    private void Save()
    {
        if (_dialogService.Confirm("Save?", "Save settings?"))
        {
            _logger.LogInformation("Settings saved");
            _navService.NavigateTo<HomeViewModel>();
        }
        else
        {
            _logger.LogInformation("Not saved");
        }
    }

    [RelayCommand]
    private void Cancel() => _navService.NavigateTo<HomeViewModel>();
}
