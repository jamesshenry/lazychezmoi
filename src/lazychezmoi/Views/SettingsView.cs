using LazyChezmoi.ViewModels;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace LazyChezmoi.Views;

public class SettingsView : View
{
    private readonly SettingsViewModel _vm;
    private Label _lblName = default!;
    private TextField _txtName = default!;
    private CheckBox _chkLog = default!;
    private Button _btnSave = default!;
    private Button _btnCancel = default!;

    public SettingsView(SettingsViewModel viewModel)
    {
        _vm = viewModel;
        InitializeComponent();
        BindViewModel();
    }

    private void BindViewModel()
    {
        _txtName.TextChanged += (_, args) => _vm.Username = _txtName.Text;
        _chkLog.Accepted += (_, args) =>
            _vm.EnableLogging = _chkLog.CheckedState == CheckState.Checked;
        _btnSave.Accepting += (s, e) => _vm.SaveCommand.Execute(null);
        _btnCancel.Accepting += (s, e) => _vm.CancelCommand.Execute(null);

        _vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(_vm.Username))
                _txtName.Text = _vm.Username;
            if (e.PropertyName == nameof(_vm.EnableLogging))
                _chkLog.CheckedState = _vm.EnableLogging
                    ? CheckState.Checked
                    : CheckState.UnChecked;
        };
    }

    private void InitializeComponent()
    {
        CanFocus = true;
        Width = Dim.Fill();
        Height = Dim.Fill();

        _lblName = new Label { Text = "Username:" };
        _txtName = new TextField { X = Pos.Right(_lblName) + 2, Width = Dim.Fill(5) };

        _chkLog = new CheckBox
        {
            Y = Pos.Bottom(_lblName) + 1,
            Text = "Enable Background Logging",
            CheckedState = _vm.EnableLogging ? CheckState.Checked : CheckState.UnChecked,
        };

        _btnSave = new Button
        {
            X = 0,
            Y = Pos.Bottom(_chkLog) + 2,
            Text = "Save Settings",
        };

        _btnCancel = new Button
        {
            X = Pos.Right(_btnSave) + 2,
            Y = Pos.Y(_btnSave),
            Text = "Cancel",
        };

        Add(_lblName, _txtName, _chkLog, _btnSave, _btnCancel);
    }
}
