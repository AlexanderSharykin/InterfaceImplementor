using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using TypeCode = InterfaceImplementor.TypeCode;

namespace InterfaceImplementor
{
    public partial class MainForm : Form
    {        
        private IList<Type> _inf;
        public MainForm()
        {
            InitializeComponent();
            LoadAssembly(Application.ExecutablePath);
        }

        private void OpenFileClick(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Assembly|*.dll|Project|*.exe";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;
                LoadAssembly(ofd.FileName);
            }
        } 

        private void LoadAssembly(string assembly)
        {
            Assembly a;
            try
            {
                a = Assembly.LoadFile(assembly);
                _inf = a.GetTypes().Where(t => t.IsInterface).ToList();
            }
            catch (ReflectionTypeLoadException ex)
            {
                MessageBox.Show(ex.LoaderExceptions[0].Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtFile.Text = assembly;
            lstInterfaces.Items.Clear();

            foreach (var i in _inf)
                lstInterfaces.Items.Add(new TypeCode(i).FullName);

            if (_inf.Count > 0)
                lstInterfaces.SelectedIndex = 0;
        }

        private void SelectedInterfaceChanged(object sender, EventArgs e)
        {
            int idx = lstInterfaces.SelectedIndex;
            if (idx < 0)
                return;
            var t = _inf[idx];
            txtCode.Text = new CodeBuilder().ImplementInterface(t);
        }

        private void CloseClick(object sender, EventArgs e)
        {
            Close();
        }    
    }
}
