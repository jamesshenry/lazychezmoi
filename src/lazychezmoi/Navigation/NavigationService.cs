using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.ViewBase;

namespace LazyChezmoi.Navigation;

public interface INavigationService
{
    ObservableObject CurrentViewModel { get; }
    void NavigateTo<TViewModel>()
        where TViewModel : ObservableObject;
}

public class NavigationService : ObservableObject, INavigationService
{
    private readonly IServiceProvider _services;

    public NavigationService(IServiceProvider services) => _services = services;

    public ObservableObject CurrentViewModel
    {
        get => field;
        private set => SetProperty(ref field, value);
    }

    public void NavigateTo<TViewModel>()
        where TViewModel : ObservableObject
    {
        var vm = _services.GetRequiredService<TViewModel>();

        (CurrentViewModel as INavigationAware)?.OnNavigatedFrom();
        CurrentViewModel = vm;
        (CurrentViewModel as INavigationAware)?.OnNavigatedTo();
    }
}

internal interface INavigationAware
{
    void OnNavigatedTo();
    void OnNavigatedFrom();
}

public static class ViewLocator
{
    public static View GetView(IServiceProvider sp, object viewModel)
    {
        return viewModel switch
        {
            // DashboardViewModel => sp.GetRequiredService<DashboardView>(),
            // SettingsViewModel => sp.GetRequiredService<SettingsView>(),
            _ => throw new UnreachableException(),
        };
    }
}
