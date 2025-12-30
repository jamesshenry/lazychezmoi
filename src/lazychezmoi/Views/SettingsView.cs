using LazyChezmoi.ViewModels;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace LazyChezmoi.Views;

public class SettingsView : View
{
    private readonly SettingsViewModel _viewModel;

    public SettingsView(SettingsViewModel viewModel)
    {
        _viewModel = viewModel;
        Width = Dim.Fill();
        Height = Dim.Fill();

        // --- UI Elements ---

        var lblName = new Label { Text = "Username:" };
        var txtName = new TextField
        {
            X = Pos.Right(lblName) + 2,
            Width = Dim.Fill(5),
            Text = _viewModel.Username,
        };

        var chkLog = new CheckBox
        {
            Y = Pos.Bottom(lblName) + 1,
            Text = "Enable Background Logging",
            CheckedState = _viewModel.EnableLogging ? CheckState.Checked : CheckState.UnChecked,
        };

        var btnSave = new Button
        {
            X = 0,
            Y = Pos.Bottom(chkLog) + 2,
            Text = "Save Settings",
        };

        var btnCancel = new Button
        {
            X = Pos.Right(btnSave) + 2,
            Y = Pos.Y(btnSave),
            Text = "Cancel",
        };

        // --- THE GLUE (Manual Binding) ---

        // View -> ViewModel
        txtName.TextChanged += (_, args) => _viewModel.Username = txtName.Text;
        chkLog.Accepted += (_, args) =>
            _viewModel.EnableLogging = chkLog.CheckedState == CheckState.Checked;

        // ViewModel -> View (Optional: only if VM properties change from logic)
        _viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(_viewModel.Username))
                txtName.Text = _viewModel.Username;
            if (e.PropertyName == nameof(_viewModel.EnableLogging))
                chkLog.CheckedState = _viewModel.EnableLogging
                    ? CheckState.Checked
                    : CheckState.UnChecked;
        };

        // UI Commands
        btnSave.Accepting += (s, e) => _viewModel.SaveCommand.Execute(null);
        btnCancel.Accepting += (s, e) => _viewModel.CancelCommand.Execute(null);

        Add(lblName, txtName, chkLog, btnSave, btnCancel);
    }
}
