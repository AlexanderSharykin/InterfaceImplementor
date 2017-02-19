using System;
using System.ComponentModel;
using System.Windows.Forms;
using ViewModels;

namespace WinformsClient
{
    public partial class MainForm : Form
    {        
        private AssemblyVm _dataContext;

        public MainForm()
        {
            InitializeComponent();
        }

        public AssemblyVm DataContext
        {
            get { return _dataContext; }
            set
            {
                if (_dataContext != null)
                    _dataContext.PropertyChanged -= HandlePropertyChanged;

                _dataContext = value;

                if (_dataContext != null)
                    _dataContext.PropertyChanged += HandlePropertyChanged;
            }
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Code": txtCode.Text = DataContext.Code; break;
                case "Location": txtCode.Text = DataContext.Location; break;
                case "SelectedInterface": lstInterfaces.SelectedItem = DataContext.SelectedInterface; break;
                case "Interfaces": lstInterfaces.DataSource = DataContext.Interfaces; break;
            }
        }

        private void OpenFileClick(object sender, EventArgs e)
        {
            DataContext.ShowSelectAssemblyDialog();
        }

        private void SelectedInterfaceChanged(object sender, EventArgs e)
        {
            int idx = lstInterfaces.SelectedIndex;
            if (idx < 0)
                return;
            DataContext.SelectedInterface = (Type) lstInterfaces.SelectedItem;
        }

        private void CloseClick(object sender, EventArgs e)
        {
            Close();
        }    
    }
}
