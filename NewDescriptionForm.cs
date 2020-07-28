using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor
{
    public partial class NewDescriptionForm : Form
    {
        public new string Name { get; private set; } = null;
        public string Root { get; private set; } = null;

        public NewDescriptionForm()
        {
            InitializeComponent();
            PathTextBox.Text = Directory.GetCurrentDirectory();
        }
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            RootFolderDialog.SelectedPath = PathTextBox.Text;
            if (RootFolderDialog.ShowDialog(this) != DialogResult.OK) return;
            else PathTextBox.Text = RootFolderDialog.SelectedPath;
        }
        private void CreateButton_Click(object sender, EventArgs e)
        {
            Name = NameTextBox.Text;
            Root = PathTextBox.Text;
            if (Path.Combine(Root, Name).IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                MessageBox.Show(this, "Invalid description name.", "Error.", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.OK;
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
