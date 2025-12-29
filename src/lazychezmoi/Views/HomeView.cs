using LazyChezmoi.ViewModels;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace LazyChezmoi.Views;

public class HomeView : View
{
    public HomeView(HomeViewModel vm)
    {
        Width = Dim.Fill();
        Height = Dim.Fill();

        var lbl = new Label
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            Text = vm.WelcomeMessage,
        };
        Add(lbl);
    }
}
