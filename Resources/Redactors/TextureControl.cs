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
using ExtraForms;

namespace Resource_Redactor.Resources.Redactors
{
    [DefaultEvent("StateChanged")]
    public partial class TextureControl : ResourceControl<TextureResource, StoryItem<TextureControl.State>>, IResourceControl
    {
        public struct State
        {
            public Bitmap Texture;
            public Color BackColor;
            public State(TextureResource r)
            {
                Texture = r.Bitmap;
                BackColor = r.BackColor;
            }
            public void ToResource(TextureResource r)
            {
                r.Bitmap = Texture;
                r.BackColor = BackColor;
            }
        }

        public TextureControl(string path)
        {
            InitializeComponent();
            OpenGLSurface.ResetCurrent();

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Import texture", null, ImportTextureMenuItem_Click, Keys.Control | Keys.I),
                new ToolStripMenuItem("Export texture", null, ExportTextureMenuItem_Click, Keys.Control | Keys.U),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };

            Open(path);
            Story = new StoryItem<State>(new State(LoadedResource));
            Story.ValueChanged += Story_ValueChanged;

            TextureBitmapBox.Bitmap = LoadedResource.Bitmap;
            TextureBitmapBox.BackColor = LoadedResource.BackColor;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                LoadedResource.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Story_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                OpenGLSurface.ResetCurrent();

                Story.Item.ToResource(LoadedResource);

                TextureBitmapBox.Bitmap = LoadedResource.Bitmap;
                TextureBitmapBox.BackColor = LoadedResource.BackColor;

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
                OpenGLSurface.ResetCurrent();

                LoadedResource.Bitmap = new Bitmap(ImportFileDialog.FileName);

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

                LoadedResource.Bitmap.Save(ExportFileDialog.FileName, ImageFormat.Png);
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
                BackgroundColorDialog.Color = LoadedResource.BackColor;
                if (BackgroundColorDialog.ShowDialog(this) != DialogResult.OK) return;
                if (LoadedResource.BackColor == BackgroundColorDialog.Color) return;

                LoadedResource.BackColor = BackgroundColorDialog.Color;

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
                OpenGLSurface.ResetCurrent();
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

                    LoadedResource.Bitmap = new Bitmap(paths[0]);

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
