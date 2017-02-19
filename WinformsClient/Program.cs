using System;
using System.Windows.Forms;
using ViewModels;

namespace WinformsClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var dialogs = new VisualDialogContainer();

            dialogs.Set<WinformsOpenFileDialog>("Assembly");
            dialogs.Set<WinformsMessageBox>("ErrorMessage");

            var f = new MainForm { DataContext = new AssemblyVm {  Dialogs = dialogs } };
            Application.Run(f);
        }
    }
}
