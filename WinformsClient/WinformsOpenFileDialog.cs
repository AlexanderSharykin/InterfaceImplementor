using System.Windows.Forms;
using ViewModels;

namespace WinformsClient
{
    /// <summary>
    /// IVisualDialog wrapper for WinForms OpenFileDialog
    /// </summary>
    public class WinformsOpenFileDialog: IVisualDialog
    {
        public bool? ShowDialog(object dataContext)
        {
            var vm = dataContext as OpenFileVm;
            if (vm == null)
                return null;
            var ofd = new OpenFileDialog
            {
                Filter = vm.Filter,
                Title = vm.Title
            };
            var res = ofd.ShowDialog();
            if (res == DialogResult.OK)
            {
                vm.FileName = ofd.FileName;
                return true;
            }            
            return false;
        }
    }
}
