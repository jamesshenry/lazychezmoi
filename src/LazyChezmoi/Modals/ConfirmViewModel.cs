using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyChezmoi.Navigation;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace LazyChezmoi.Modals;

public partial class ConfirmViewModel : ObservableObject, IModalViewModel<bool>
{
    public bool Result { get; private set; }
    public event EventHandler? RequestClose;

    [ObservableProperty]
    private string _message = "";

    [RelayCommand]
    private void Confirm()
    {
        Result = true;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Cancel()
    {
        Result = false;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}

public class ConfirmView : Dialog
{
    public ConfirmView(ConfirmViewModel vm, IApplication app)
    {
        Title = "Confirmation";
        Width = 40;
        Height = 10;

        var lbl = new Label
        {
            Text = vm.Message,
            X = Pos.Center(),
            Y = 1,
        };

        var btnYes = new Button
        {
            Text = "Yes",
            X = 5,
            Y = Pos.AnchorEnd(1),
        };
        var btnNo = new Button
        {
            Text = "No",
            X = Pos.AnchorEnd(10),
            Y = Pos.AnchorEnd(1),
        };

        // Glue
        btnYes.Accepted += (s, e) => vm.ConfirmCommand.Execute(null);
        btnNo.Accepted += (s, e) => vm.CancelCommand.Execute(null);

        // THE KEY: The View stops the app loop when the VM is "done"
        vm.RequestClose += (s, e) => app.RequestStop();

        Add(lbl, btnYes, btnNo);
    }
}
