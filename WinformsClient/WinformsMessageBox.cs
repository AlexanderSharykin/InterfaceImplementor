using System.Windows.Forms;
using ViewModels;

namespace WinformsClient
{
    /// <summary>
    /// IVisualDialog wrapper for WinForms MessageBox
    /// </summary>
    public class WinformsMessageBox: IVisualDialog
    {
        public bool? ShowDialog(object dataContext)
        {
            var vm = dataContext as MessageVm;
            if (vm == null)
                return null;
            var res = MessageBox.Show(vm.Text, vm.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return res == ExpectedResult;
        }

        public MessageBoxButtons Buttons { get; set; }

        public MessageBoxIcon Image { get; set; }

        public DialogResult ExpectedResult { get; set; }
    }
}
