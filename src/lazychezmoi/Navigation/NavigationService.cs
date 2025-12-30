using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.App;
using Terminal.Gui.Views;

namespace LazyChezmoi.Navigation;

public interface INavigationService : INotifyPropertyChanged
{
    ObservableObject CurrentViewModel { get; }
    void NavigateTo<TViewModel>()
        where TViewModel : ObservableObject;
    TResult? ShowModal<TViewModel, TResult>(Action<TViewModel>? configure = null) where TViewModel : class, IModalViewModel<TResult>;

    event EventHandler<EventArgs>? ScreenChanged;
}

public class NavigationService : ObservableObject, INavigationService
{
    private readonly IServiceProvider _services;
    private readonly IApplication _app;

    public NavigationService(IServiceProvider services, IApplication app)
    {
        _services = services;
        _app = app;
    }

    public ObservableObject CurrentViewModel
    {
        get => field;
        private set => SetProperty(ref field, value);
    }

    public event EventHandler<EventArgs>? ScreenChanged;

    public void NavigateTo<TViewModel>()
        where TViewModel : ObservableObject
    {
        (CurrentViewModel as INavigationAware)?.OnNavigatedFrom();
        CurrentViewModel = _services.GetRequiredService<TViewModel>();
        ScreenChanged?.Invoke(this, EventArgs.Empty);
        (CurrentViewModel as INavigationAware)?.OnNavigatedTo();
    }

    // In NavigationService
    public TResult? ShowModal<TViewModel, TResult>(Action<TViewModel>? configure = null)
        where TViewModel : class, IModalViewModel<TResult>
    {
        var vm = _services.GetRequiredService<TViewModel>();
        configure?.Invoke(vm);
        var view = ViewLocator.GetView(_services, vm);

        if (view is IRunnable runnable)
        {
            EventHandler? handler = null;
            handler = (s, e) =>
            {
                _app.RequestStop();
                vm.RequestClose -= handler; // Cleanup
            };
            vm.RequestClose += handler;
            _app.Run(runnable);
        }
        else
        {
            var host = new Dialog { Title = "Modal Host" };
            host.Add(view);
            _app.Run(host);
        }

        return vm.Result; // VM stores the result before RequestStop() was called
    }
}

internal interface INavigationAware
{
    void OnNavigatedTo();
    void OnNavigatedFrom();
}

public interface IModalViewModel<TResult>
{
    // The result the modal will return (e.g., a bool, a string, or a complex object)
    TResult? Result { get; }

    // An event to tell the View: "I am done, please stop the loop"
    event EventHandler? RequestClose;
}
