using Microsoft.Extensions.DependencyInjection;

namespace LazyChezmoi.Navigation;

public class NavigationManager : INavigationManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, IScreenView> _screenCache = [];
    private IScreenView? _currentScreen;

    public NavigationManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IScreenView? CurrentScreen => _currentScreen;

    public event EventHandler<ScreenChangedEventArgs>? ScreenChanged;

    public void NavigateTo<TScreen>()
        where TScreen : IScreenView
    {
        NavigateTo(typeof(TScreen));
    }

    public void NavigateTo(Type screenType)
    {
        if (!typeof(IScreenView).IsAssignableFrom(screenType))
            throw new ArgumentException("Screen type must implement IScreenView");

        var previousScreen = _currentScreen;

        // Get or create screen instance
        if (!_screenCache.TryGetValue(screenType, out var screen))
        {
            screen = (IScreenView)_serviceProvider.GetRequiredService(screenType);
            _screenCache[screenType] = screen;
        }

        _currentScreen = screen;

        // Notify listeners
        previousScreen?.OnDeactivated();
        screen.OnActivated();

        ScreenChanged?.Invoke(this, new ScreenChangedEventArgs(previousScreen, screen));
    }
}
