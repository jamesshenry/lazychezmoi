using System.ComponentModel;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
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

        Title = _viewModel.AppTitle;

        _statusLabel = new Label
        {
            Y = Pos.AnchorEnd(1),
            Width = Dim.Fill(),
            Text = _viewModel.StatusText,
        };

        _contentContainer = new View
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 3,
            CanFocus = true,
        };

        Add(_contentContainer, _statusLabel);

        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        _navService.PropertyChanged += OnNavServicePropertyChanged;

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

    private void UpdateContent(ObservableObject? viewModel)
    {
        if (viewModel == null)
            return;

        _contentContainer.RemoveAll();

        var view = ViewLocator.GetView(_serviceProvider, viewModel);

        view.Width = Dim.Fill();
        view.Height = Dim.Fill();

        _contentContainer.Add(view);

        view.SetFocus();
    }
}
