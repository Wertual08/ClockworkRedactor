using ResrouceRedactor.Resources;
using ResrouceRedactor.Resources.Redactors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResrouceRedactor
{
    public partial class ExplorerForm : Form
    {
        public List<string> SelectedResources { get; private set; } = null;

        public ExplorerForm(string path, bool multiselect = false, params ResourceType[] type)
        {
            InitializeComponent();
            ResourceExplorer.TypeFilter = type;
            ResourceExplorer.MultiSelect = multiselect;
            ResourceExplorer.LoadLocation(path);
        }

        private void ResourceExplorer_ItemLoaded(object sender, ExplorerEventArgs e)
        {
            DialogResult = DialogResult.OK;
            SelectedResources = new List<string>(1);
            SelectedResources.Add(e.LocalPath);
            Close();
        }
        private void SelectButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            SelectedResources = ResourceExplorer.SelectedItems;
            Close();
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
