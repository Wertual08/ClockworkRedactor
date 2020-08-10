using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using ExtraSharp;

namespace Resource_Redactor.Resources.Redactors
{
    [DefaultEvent("StateChanged")]
    public partial class TextureControl : UserControl, IResourceControl
    {
        private struct State
        {
            public Bitmap Texture;
            public int BackColor;
            public State(TextureResource r)
            {
                Texture = r.Texture;
                BackColor = r.BackColor;
            }
            public void ToResource(TextureResource r)
            {
                r.Texture = Texture;
                r.BackColor = BackColor;
            }
        }

        private StoryItem<State> Story = null;
        private TextureResource LoadedResource = null;
        
        public ToolStripMenuItem[] MenuTabs { get; private set; }
        public bool Saved { get; private set; } = true;
        public bool UndoEnabled { get { return Story.PrevState; } }
        public bool RedoEnabled { get { return Story.NextState; } }

        public event StateChangedEventHandler StateChanged;

        public string ResourcePath { get; private set; }
        public string ResourceName { get; private set; }

        public int FPS { get; set; }
        public void Activate()
        {
        }

        public void Save(string path)
        {
            ResourcePath = path;
            ResourceName = Path.GetFileName(path);

            LoadedResource.Save(path);

            Saved = true;
            UpdateRedactor();
        }
        public void Undo()
        {
            Story.Undo();
        }
        public void Redo()
        {
            Story.Redo();
        }

        public TextureControl(string path)
        {
            InitializeComponent();

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Import texture", null, ImportTextureMenuItem_Click, Keys.Control | Keys.I),
                new ToolStripMenuItem("Export texture", null, ExportTextureMenuItem_Click, Keys.Control | Keys.U),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };

            LoadedResource = new TextureResource(path);
            Story = new StoryItem<State>(new State(LoadedResource));
            Story.ValueChanged += Story_ValueChanged;

            ResourcePath = path;
            ResourceName = Path.GetFileName(path);

            TextureBitmapBox.Bitmap = LoadedResource.Texture;
            TextureBitmapBox.BackColor = Color.FromArgb(LoadedResource.BackColor);

            Saved = true;
        }

        private void UpdateRedactor()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        private void MakeUnsaved()
        {
            Saved = false;
            UpdateRedactor();
        }

        private void Story_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Story.Item.ToResource(LoadedResource);

                TextureBitmapBox.Bitmap = LoadedResource.Texture;
                TextureBitmapBox.BackColor = Color.FromArgb(LoadedResource.BackColor);

                MakeUnsaved();
                UpdateRedactor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not update resource data.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportTextureMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ImportFileDialog.ShowDialog() != DialogResult.OK) return;

                LoadedResource.Texture = new Bitmap(ImportFileDialog.FileName);

                Story.Item = new State(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not import texture.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExportTextureMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExportFileDialog.ShowDialog() != DialogResult.OK) return;

                LoadedResource.Texture.Save(ExportFileDialog.FileName, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not export texture.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BackColorMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BackgroundColorDialog.Color = Color.FromArgb(LoadedResource.BackColor);
                if (BackgroundColorDialog.ShowDialog(this) != DialogResult.OK) return;
                if (LoadedResource.BackColor == BackgroundColorDialog.Color.ToArgb()) return;

                LoadedResource.BackColor = BackgroundColorDialog.Color.ToArgb();

                Story.Item = new State(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change background color.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResetPositionMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TextureBitmapBox.ResetLocation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not reset position.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextureBitmapBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect.HasFlag(DragDropEffects.Copy) &&
                e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }
        private void TextureBitmapBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (paths.Length != 1)
                    {
                        MessageBox.Show(this, "You must choose only one image file.",
                            "Error: Can not import [" + paths.Length + "] files.",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    LoadedResource.Texture = new Bitmap(paths[0]);

                    Story.Item = new State(LoadedResource);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not import texture.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
