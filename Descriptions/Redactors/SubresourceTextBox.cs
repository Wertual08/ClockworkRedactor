using ExtraSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor.Descriptions.Redactors
{
    class SubresourceTextBox : TextBox
    {
        private ISubresource _Subresource = null;
        private ISynchronizeInvoke OldSyncer = null;

        public ISubresource Subresource
        {
            get => _Subresource;
            set
            {
                if (_Subresource != null)
                {
                    _Subresource.Refreshed -= Subresource_Refreshed;
                    _Subresource.SynchronizingObject = OldSyncer;
                }
                _Subresource = value;
                if (_Subresource != null)
                {
                    _Subresource.Refreshed += Subresource_Refreshed;
                    OldSyncer = _Subresource.SynchronizingObject;
                    _Subresource.SynchronizingObject = this;
                    Text = _Subresource.Link;
                    BackColor = _Subresource.Loaded ? DefaultBackColor : Color.Red;
                }
                else
                {
                    OldSyncer = null;
                    Text = "";
                }
            }
        }

        public SubresourceTextBox() : base()
        {
            BackColor = Color.Red;
            AllowDrop = true;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            try
            {
                if (_Subresource == null) return;
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
                var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (paths.Length != 1 || Resource.GetType(paths[0]) != _Subresource.Type) return;
                if (e.AllowedEffect.HasFlag(DragDropEffects.Link))
                    e.Effect = DragDropEffects.Link;
                else if (e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                    e.Effect = DragDropEffects.Copy;
                else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                    e.Effect = DragDropEffects.Move;
                else e.Effect = DragDropEffects.None;
            }
            catch
            {

            }
            base.OnDragEnter(e);
        }
        protected override void OnDragDrop(DragEventArgs e)
        {
            try
            {
                if (_Subresource == null) return;
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

                    var fpath = paths[0];
                    var dpath = Directory.GetCurrentDirectory();
                    if (!fpath.StartsWith(dpath)) MessageBox.Show(this,
                        "Resource is not in description directory.",
                        "Error: Invalid resource.", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    else Text = ExtraPath.MakeDirectoryRelated(dpath, fpath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link resoruce.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            base.OnDragDrop(e);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            try
            {
                if (_Subresource == null) return;
                _Subresource.Link = Text;
                if (!_Subresource.Loaded) _Subresource.Reload();
            }
            catch
            {

            }
            base.OnTextChanged(e);
        }
        protected override void Dispose(bool disposing)
        {
            try
            {
                Subresource = null;
            }
            catch
            {

            }
            base.Dispose(disposing);
        }

        private void Subresource_Refreshed(object sender, EventArgs e)
        {
            try
            {
                if (_Subresource == null) return;
                if (Text != _Subresource.Link) Text = _Subresource.Link;
                BackColor = _Subresource.Loaded ? DefaultBackColor : Color.Red;
            }
            catch
            {

            }
        }
    }
}
