using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LazyChezmoi.Navigation;

using Microsoft.Extensions.Logging;

namespace LazyChezmoi.Modals;

public partial class ConfirmDialogViewModel : ObservableObject, IModalViewModel<bool>
{
    public ConfirmDialogViewModel(ILogger<ConfirmDialogViewModel> logger)
    {
        _logger = logger;
    }
    public bool Result { get; private set; }
    public event EventHandler? RequestClose;

    [ObservableProperty]
    private string _message = "";
    private readonly ILogger<ConfirmDialogViewModel> _logger;

    [RelayCommand]
    private void Confirm()
    {
        _logger.LogInformation("Confirm clicked");
        Result = true;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Cancel()
    {
        _logger.LogInformation("Cancel clicked");
        Result = false;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}
