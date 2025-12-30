using LazyChezmoi.Modals;
using LazyChezmoi.ViewModels;
using LazyChezmoi.Views;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.ViewBase;

namespace LazyChezmoi.Navigation;

public static class ViewLocator
{
    public static View GetView(IServiceProvider sp, object viewModel) =>
        viewModel switch
        {
            HomeViewModel => sp.GetRequiredService<HomeView>(),
            SettingsViewModel => sp.GetRequiredService<SettingsView>(),
            ConfirmDialogViewModel => sp.GetRequiredService<ConfirmDialog>(), // Inherits Dialog
            _ => throw new ArgumentException($"No view registered for {viewModel.GetType()}"),
        };
}
