using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace LazyChezmoi.Modals;

public class ConfirmDialog : Dialog
{
    public ConfirmDialog(ConfirmDialogViewModel vm, IApplication app)
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
        btnYes.Accepting += (s, e) => vm.ConfirmCommand.Execute(null);
        btnNo.Accepting += (s, e) => vm.CancelCommand.Execute(null);

        // THE KEY: The View stops the app loop when the VM is "done"
        vm.RequestClose += (s, e) => app.RequestStop();

        Add(lbl, btnYes, btnNo);
    }
}
