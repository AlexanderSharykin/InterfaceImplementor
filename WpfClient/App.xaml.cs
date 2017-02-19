using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using ViewModels;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var dialogs = new VisualDialogContainer();

            dialogs.Set<WpfOpenFileDialog>("Assembly");
            dialogs.Set<WpfMessageBox>("ErrorMessage");

            var mainVm = new AssemblyVm
            {
                Dialogs = dialogs
            };

            var w = new MainWindow();
            w.DataContext = mainVm;
            Current.MainWindow = w;
            w.ShowDialog();
        }
    }
}
