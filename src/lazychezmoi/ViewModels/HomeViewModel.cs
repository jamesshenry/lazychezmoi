using CommunityToolkit.Mvvm.ComponentModel;

namespace LazyChezmoi.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [ObservableProperty]
    private string _welcomeMessage = "Welcome to the Dashboard!";
}
