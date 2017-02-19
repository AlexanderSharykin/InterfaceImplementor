using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using InterfaceImplementor;
using MvvmFoundation.Wpf;

namespace ViewModels
{
    public class AssemblyVm: ObservableObject
    {
        private ICommand _selectAssemblyCmd;
        private IList<Type> _interfaces;
        private Type _selectedInterface;
        private string _location;
        private string _code;

        /// <summary>
        /// Path to assembly file (.dll)
        /// </summary>
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                RaisePropertyChanged("Location");
            }
        }

        /// <summary>
        /// Get or sets interfaces, declared in selected assembly
        /// </summary>
        public IList<Type> Interfaces
        {
            get { return _interfaces; }
            set
            {
                _interfaces = value;
                RaisePropertyChanged("Interfaces");
                if (_interfaces.Count > 0)
                    SelectedInterface = _interfaces[0];
                else
                    SelectedInterface = null;
            }
        }

        /// <summary>
        /// Gets or sets selected interface
        /// </summary>
        public Type SelectedInterface
        {
            get { return _selectedInterface; }
            set
            {
                _selectedInterface = value;
                RaisePropertyChanged("SelectedInterface");
                if (_selectedInterface != null)
                    Code = new CodeBuilder().ImplementInterface(_selectedInterface);
                else 
                    Code = String.Empty;
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                RaisePropertyChanged("Code");
            }
        }

        /// <summary>
        /// Contains VisualDialogs used by view models 
        /// </summary>
        public VisualDialogContainer Dialogs { get; set; }

        /// <summary>
        /// Command to select an assembly and load a list of interfaces
        /// </summary>
        public ICommand SelectAssemblyCmd
        {
            get
            {
                if (_selectAssemblyCmd == null)                
                    _selectAssemblyCmd = new RelayCommand(ShowSelectAssemblyDialog);                
                                    
                return _selectAssemblyCmd;
            }            
        }

        /// <summary>
        /// Select an assembly and load a list of interfaces
        /// </summary>
        public void ShowSelectAssemblyDialog()
        {
            // show a dialog to select an assembly
            IVisualDialog d = Dialogs.Get("Assembly");
            var vm = new OpenFileVm
            {
                Title = "Select .Net assembly",
                Filter = "Component Files (*.dll,*.exe)|*.dll;*.exe"
            };
            bool result = d.ShowDialog(vm) ?? false;

            if (false == result)
                return;

            Location = vm.FileName;

            // get all interfaces from selected assembly
            Assembly a;
            try
            {
                a = Assembly.LoadFile(vm.FileName);
                Interfaces = a.GetTypes().Where(t => t.IsInterface).ToList();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var mb = Dialogs.Get("ErrorMessage");
                var msg = new MessageVm
                {
                    Text = ex.LoaderExceptions[0].Message,
                    Caption = ex.GetType().Name
                };
                mb.ShowDialog(msg);
            }
            catch (Exception ex)
            {
                var mb = Dialogs.Get("ErrorMessage");
                var msg = new MessageVm
                {
                    Text = ex.Message,
                    Caption = ex.GetType().Name
                };
                mb.ShowDialog(msg);
            }
        }
    }
}
