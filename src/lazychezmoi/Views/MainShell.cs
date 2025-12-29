using System.ComponentModel;
using LazyChezmoi.Navigation;
using LazyChezmoi.ViewModels;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace LazyChezmoi.Views;

public class MainShell : Window
{
    private readonly MainViewModel _viewModel;
    private readonly INavigationService _navService;
    private readonly IServiceProvider _serviceProvider;
    private readonly View _contentContainer;
    private readonly Label _statusLabel;

    public MainShell(MainViewModel viewModel, INavigationService navService, IServiceProvider sp)
    {
        _viewModel = viewModel;
        _navService = navService;
        _serviceProvider = sp;

        // 1. Setup Layout
        Title = _viewModel.AppTitle;

        // Status Bar at the bottom
        _statusLabel = new Label
        {
            Y = Pos.AnchorEnd(1),
            Width = Dim.Fill(),
            Text = _viewModel.StatusText,
        };

        // Main Content Area
        _contentContainer = new View
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 1, // Leave room for status bar
        };

        Add(_contentContainer, _statusLabel);

        // 2. Wiring (The Glue)
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        _navService.PropertyChanged += OnNavServicePropertyChanged;

        // Initial Navigation
        _viewModel.NavigateHomeCommand.Execute(null);
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.StatusText))
            _statusLabel.Text = _viewModel.StatusText;
    }

    private void OnNavServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(INavigationService.CurrentViewModel))
        {
            UpdateContent(_navService.CurrentViewModel);
        }
    }

    private void UpdateContent(object? viewModel)
    {
        if (viewModel == null)
            return;

        _contentContainer.RemoveAll();

        // Use the ViewLocator logic to get the View for this VM
        var view = ViewLocator.GetView(_serviceProvider, viewModel);
        _contentContainer.Add(view);
    }
}
