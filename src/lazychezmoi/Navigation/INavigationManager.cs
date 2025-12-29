namespace LazyChezmoi.Navigation;

public interface INavigationManager
{
    IScreenView? CurrentScreen { get; }
    event EventHandler<ScreenChangedEventArgs>? ScreenChanged;
    void NavigateTo<TScreen>()
        where TScreen : IScreenView;
    void NavigateTo(Type screenType);
}

public class ScreenChangedEventArgs : EventArgs
{
    public IScreenView? PreviousScreen { get; }
    public IScreenView? NewScreen { get; }

    public ScreenChangedEventArgs(IScreenView? previous, IScreenView? newScreen)
    {
        PreviousScreen = previous;
        NewScreen = newScreen;
    }
}

public interface IScreenView<T> : IScreenView
{
    IEnumerable<T> GetMenuItems();
}

public interface IScreenView
{
    string Title { get; }
    object? ViewModel { get; }
    void HandleMenuAction(string action);
    void OnActivated();
    void OnDeactivated();
}
