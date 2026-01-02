using LazyChezmoi.ViewModels;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace LazyChezmoi.Views;

public class HomeView : View
{
    private readonly HomeViewModel _vm;

    public HomeView(HomeViewModel vm)
    {
        _vm = vm;
        Width = Dim.Fill();
        Height = Dim.Fill();

        var lbl = new Label
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            Text = vm.WelcomeMessage,
        };

        var btn = new Button
        {
            X = Pos.Center(),
            Y = Pos.Bottom(lbl),
            Text = "Go Settings",
        };

        btn.Accepting += (s, e) => _vm.NavigateSettingsCommand.Execute(null);
        Add(lbl);
        Add(btn);
    }
}
