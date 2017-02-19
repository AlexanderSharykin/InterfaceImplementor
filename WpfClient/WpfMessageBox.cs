using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ViewModels;

namespace WpfClient
{
    /// <summary>
    /// IVisualDialog wrapper for WPF MessageBox
    /// </summary>
    public class WpfMessageBox: IVisualDialog
    {
        public bool? ShowDialog(object dataContext)
        {
            var vm = dataContext as MessageVm;
            if (vm == null)
                return null;
            var res = MessageBox.Show(vm.Text, vm.Caption, MessageBoxButton.OK, MessageBoxImage.Error);
            return res == ExpectedResult;
        }

        public MessageBoxButton Buttons { get; set; }

        public MessageBoxImage Image { get; set; }

        public MessageBoxResult ExpectedResult { get; set; }
    }
}
