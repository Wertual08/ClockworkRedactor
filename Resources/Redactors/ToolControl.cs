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
using ExtraForms;
using ExtraSharp;
using ExtraForms.OpenGL;

namespace Resource_Redactor.Resources.Redactors
{
    public partial class ToolControl : ResourceControl<ToolResource, StoryItem<ToolControl.State>>, IResourceControl
    {
        private DragManager MouseManager = new DragManager(0.015625f);
        private int CapturedPoint = -1;
        private float OffsetX = 0.0f, OffsetY = 0.0f, Angle = 0.0f;

        public struct State
        {
            public string ActionName { get; private set; }
            public float FirePointX { get; private set; }
            public float FirePointY { get; private set; }
            public float FireVectorX { get; private set; }
            public float FireVectorY { get; private set; }
            public bool AngleAttached { get; private set; }

            private bool[] Locked;
            private string[][] Links;

            public int BackColor { get; private set; }
            public float PointBoundsX { get; private set; }
            public float PointBoundsY { get; private set; }
            public bool PixelPerfect { get; private set; }
            public bool Transparency { get; private set; }

            public State(ToolResource r)
            {
                ActionName = r.ActionName;
                FirePointX = r.FirePointX;
                FirePointY = r.FirePointY;
                FireVectorX = r.FireVectorX;
                FireVectorY = r.FireVectorY;
                AngleAttached = r.AngleAttached;

                Locked = new bool[r.Count];
                Links = new string[r.Count][];
                for (int i = 0; i < r.Count; i++)
                {
                    Locked[i] = r.SpriteLockedOnCycle[i];
                    Links[i] = new string[r[i].Count];
                    for (int j = 0; j < r[i].Count; j++)
                        Links[i][j] = r[i][j].Link;
                }

                BackColor = r.BackColor;
                PointBoundsX = r.PointBoundsX;
                PointBoundsY = r.PointBoundsY;
                PixelPerfect = r.PixelPerfect;
                Transparency = r.Transparency;
            }
            public int Count { get { return Links.Length; } }
            public int LockedCount { get { return Locked.Length; } }
            public bool IsLocked(int i) { return Locked[i]; }
            public string[] this[int i] { get { return Links[i]; } }
            public override bool Equals(object obj)
            {
                if (!(obj is State)) return false;
                var s = (State)obj;

                if (ActionName != s.ActionName) return false;
                if (FirePointX != s.FirePointX) return false;
                if (FirePointY != s.FirePointY) return false;
                if (FireVectorX != s.FireVectorX) return false;
                if (FireVectorY != s.FireVectorY) return false;

                if (Links.Length != s.Links.Length) return false;
                for (int i = 0; i < Links.Length; i++)
                {
                    if (Locked[i] != s.Locked[i]) return false;
                    if (Links[i].Length != s.Links[i].Length) return false;
                    for (int j = 0; j < s.Links[i].Length; j++)
                        if (Links[i][j] != s.Links[i][j]) return false;
                }

                if (BackColor != s.BackColor) return false;
                if (PointBoundsX != s.PointBoundsX) return false;
                if (PointBoundsY != s.PointBoundsY) return false;
                if (PixelPerfect != s.PixelPerfect) return false;
                if (Transparency != s.Transparency) return false;

                return true;
            }
        };

        private List<int> SelectedSprites = new List<int>();

        public int FPS
        {
            get { return 1000 / GLFrameTimer.Interval; }
            set { GLFrameTimer.Interval = Math.Max(1, 1000 / value); }
        }
       
        public ToolControl(string path)
        {
            InitializeComponent();

            FirePointXNumeric.FixMouseWheel();
            FirePointYNumeric.FixMouseWheel();
            FireVectorXNumeric.FixMouseWheel();
            FireVectorYNumeric.FixMouseWheel();

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Link sprite", null, LinkSpriteMenuItem_Click, Keys.Control | Keys.I),
                new ToolStripMenuItem("Unlink sprite", null, UnlinkSpriteMenuItem_Click, Keys.Control | Keys.U),
                new ToolStripMenuItem("Create part", null, CreatePartMenuItem_Click, Keys.Control | Keys.A),
                new ToolStripMenuItem("Remove part", null, RemovePartMenuItem_Click, Keys.Control | Keys.D),
                new ToolStripMenuItem("Adjust fire vector", null, AdjustFireVectorMenuItem_Click, Keys.Control | Keys.F),
                new ToolStripMenuItem("Toggle transparency", null, ToggleTransparencyMenuItem_Click, Keys.Control | Keys.H),
                new ToolStripMenuItem("Pixel perfect", null, PixelPerfectMenuItem_Click, Keys.Control | Keys.P),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };

            GLSurface.MakeCurrent();

            Open(path);
            while (SelectedSprites.Count < LoadedResource.Count) SelectedSprites.Add(0);
            Story = new StoryItem<State>(new State(LoadedResource));
            Story.ValueChanged += Story_ValueChanged;

            ActionTextBox.TextChanged += (object sender, EventArgs e) => SyncTextBoxValue(sender, LoadedResource.ActionName, v => LoadedResource.ActionName = v);
            FirePointXNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.FirePointX, v => LoadedResource.FirePointX = v);
            FirePointYNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.FirePointY, v => LoadedResource.FirePointY = v);
            FireVectorXNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.FireVectorX, v => LoadedResource.FireVectorX = v);
            FireVectorYNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.FireVectorY, v => LoadedResource.FireVectorY = v);
            AttachedCheckBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxValue(sender, LoadedResource.AngleAttached, v => LoadedResource.AngleAttached = v);

            PartsListBox.SelectedIndexChanged += (object sender, EventArgs e) => RestoreChanges(false);
            PartsListBox.ItemCheck += (object sender, ItemCheckEventArgs e) => SyncCheckedValue(sender, e, LoadedResource.SpriteLockedOnCycle);

            GetTab("Pixel perfect").Checked = LoadedResource.PixelPerfect;
            GetTab("Toggle transparency").Checked = LoadedResource.Transparency;
            RestoreChanges();

            GLFrameTimer.Start();
        }

        private void SyncNumericValue<T>(object sender, T value, Action<T> set_value) where T : struct
        {
            var numeric = sender as NumericUpDown;
            var v = (T)Convert.ChangeType(numeric.Value, typeof(T));
            if (numeric != null && !value.Equals(v))
            {
                set_value(v);
                BackupChanges();
                MakeUnsaved();
            }
        }
        private void SyncDomainValue(object sender, ref int value) 
        {
            var domain = sender as DomainUpDown;
            if (domain != null && value != domain.SelectedIndex)
            {
                value = domain.SelectedIndex;
                BackupChanges();
                MakeUnsaved();
            }
        }
        private void SyncTextBoxValue(object sender, string value, Action<string> set_value)
        {
            var textbox = sender as TextBox;
            if (textbox != null && value != textbox.Text)
            {
                set_value(textbox.Text);
                BackupChanges();
                MakeUnsaved();
            }
        }
        private void SyncCheckBoxValue(object sender, bool value, Action<bool> set_value)
        {
            var control = sender as CheckBox;
            if (control != null && value != control.Checked)
            {
                set_value(control.Checked);
                BackupChanges();
                MakeUnsaved();
            }
        }
        private void SyncCheckedValue(object sender, ItemCheckEventArgs e, List<bool> values)
        {
            var listbox = sender as CheckedListBox;
            if (listbox != null && values != null && e.Index >= 0 && e.Index < values.Count && 
                values[e.Index] != (e.NewValue == CheckState.Checked))
            {
                values[e.Index] = e.NewValue == CheckState.Checked;
                BackupChanges();
                MakeUnsaved();
            }
        }

        private void RepairOffset()
        {
            //var bounds = LoadedResource.GetBounds();
            //
            //float sw = GLSurface.SurfaceSize.Width / 2.0f;
            //float sh = GLSurface.SurfaceSize.Height / 2.0f;
            //
            //float lx = bounds.Left;
            //float rx = bounds.Right;
            //float dy = bounds.Top;
            //float uy = bounds.Bottom;
            //
            //if (OffsetX + rx < -sw) OffsetX = -sw - rx;
            //if (OffsetX + lx > sw) OffsetX = sw - lx;
            //if (OffsetY + uy < -sh) OffsetY = -sh - uy;
            //if (OffsetY + dy > sh) OffsetY = sh - dy;
        }
        private void BackupChanges()
        {
            Story.Item = new State(LoadedResource);
        }
        private void RestoreChanges(bool data_changed = true)
        {
            GLSurface.MakeCurrent();

            var item = Story.Item;

            var pindex = PartsListBox.SelectedIndex;
            var vindex = VariantsListBox.SelectedIndex;

            if (data_changed)
            {
                LoadedResource.ActionName = item.ActionName;
                LoadedResource.FirePointX = item.FirePointX;
                LoadedResource.FirePointY = item.FirePointY;
                LoadedResource.FireVectorX = item.FireVectorX;
                LoadedResource.FireVectorY = item.FireVectorY;

                ActionTextBox.Text = item.ActionName;
                FirePointXNumeric.Value = (decimal)item.FirePointX;
                FirePointYNumeric.Value = (decimal)item.FirePointY;
                FireVectorXNumeric.Value = (decimal)item.FireVectorX;
                FireVectorYNumeric.Value = (decimal)item.FireVectorY;
                AttachedCheckBox.Checked = item.AngleAttached;

                if (LoadedResource.Count != item.Count)
                {
                    MakeUnsaved();

                    while (LoadedResource.Count < item.Count)
                        LoadedResource.Sprites.Add(new List<Subresource<SpriteResource>>());
                    while (LoadedResource.Count > item.Count)
                    {
                        foreach (var res in LoadedResource[LoadedResource.Count - 1]) res.Resource?.Dispose();
                        LoadedResource.Sprites.RemoveAt(LoadedResource.Count - 1);
                    }
                }
                for (int i = 0; i < LoadedResource.Count; i++)
                {
                    var icol = item[i];
                    var col = LoadedResource[i];
                    if (col.Count != icol.Length)
                    {
                        MakeUnsaved();
                        while (col.Count < item.Count) col.Add(new Subresource<SpriteResource>());
                        while (col.Count > item.Count)
                        {
                            col[col.Count - 1].Resource?.Dispose();
                            col.RemoveAt(col.Count - 1);
                        }
                    }

                    for (int j = 0; j < col.Count; j++)
                    {
                        if (col[j].Link != icol[j])
                        {
                            MakeUnsaved();
                            col[j].Link = icol[j];
                        }
                    }
                }
                if (LoadedResource.SpriteLockedOnCycle.Count != item.LockedCount)
                {
                    while (LoadedResource.SpriteLockedOnCycle.Count < item.LockedCount)
                        LoadedResource.SpriteLockedOnCycle.Add(false);
                    while (LoadedResource.SpriteLockedOnCycle.Count > item.LockedCount)
                        LoadedResource.SpriteLockedOnCycle.RemoveAt(LoadedResource.SpriteLockedOnCycle.Count - 1);
                }
                for (int i = 0; i < LoadedResource.SpriteLockedOnCycle.Count; i++)
                    LoadedResource.SpriteLockedOnCycle[i] = item.IsLocked(i);

                if (item.Count != PartsListBox.Items.Count)
                {
                    PartsListBox.BeginUpdate();
                    while (PartsListBox.Items.Count < item.Count)
                        PartsListBox.Items.Add("Part: " + PartsListBox.Items.Count);
                    while (PartsListBox.Items.Count > item.Count)
                        PartsListBox.Items.RemoveAt(PartsListBox.Items.Count - 1);
                    PartsListBox.EndUpdate();

                    PartsListBox.SelectedIndex = Math.Min(pindex, PartsListBox.Items.Count - 1);
                }
                for (int i = 0; i < LoadedResource.SpriteLockedOnCycle.Count; i++)
                    PartsListBox.SetItemChecked(i, LoadedResource.SpriteLockedOnCycle[i]);

                GLSurface.BackColor = Color.FromArgb(item.BackColor);
                LoadedResource.PointBoundsX = item.PointBoundsX;
                LoadedResource.PointBoundsY = item.PointBoundsY;
                GetTab("Pixel perfect").Checked = item.PixelPerfect;
                GetTab("Toggle transparency").Checked = item.Transparency;
            }

            if (pindex >= 0 && pindex < item.Count)
            {
                VariantsListBox.Enabled = true;
                var col = item[pindex];
                VariantsListBox.BeginUpdate();
                for (int i = 0; i < col.Length; i++)
                {
                    if (i < VariantsListBox.Items.Count) VariantsListBox.Items[i] = col[i];
                    else VariantsListBox.Items.Add(col[i]);
                }
                while (VariantsListBox.Items.Count > col.Length)
                    VariantsListBox.Items.RemoveAt(VariantsListBox.Items.Count - 1);
                VariantsListBox.EndUpdate();
            }
            else
            {
                VariantsListBox.Items.Clear();
                VariantsListBox.Enabled = false;
            }

        }

        private void Story_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                RestoreChanges();
                MakeUnsaved();
                UpdateRedactor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not update resource data.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void VariantsListBox_DragEnter(object sender, DragEventArgs e)
        {
            int sind = PartsListBox.SelectedIndex;
            if (sind < 0 || sind >= LoadedResource.Sprites.Count) return;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            foreach (var path in paths) if (Resource.GetType(path) != ResourceType.Sprite) return;
            if (e.AllowedEffect.HasFlag(DragDropEffects.Link))
                e.Effect = DragDropEffects.Link;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }
        private void VariantsListBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                int sind = PartsListBox.SelectedIndex;
                if (sind < 0 || sind >= LoadedResource.Sprites.Count) return;
                var sprites = LoadedResource.Sprites[sind];
                //int index = VariantsListBox.SelectedIndex;
                //if (index < 0 || index >= LoadedResource.Sprites[sind].Count) return;

                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                    var dpath = Directory.GetCurrentDirectory();
                    foreach (var path in paths)
                    {
                        if (!path.StartsWith(dpath)) MessageBox.Show(this,
                            "Resource is not in description directory.",
                            "Error: Invalid resource.", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        else
                        {
                            var rpath = ExtraPath.MakeDirectoryRelated(dpath, path);
                            sprites.Add(new Subresource<SpriteResource>(rpath));
                            VariantsListBox.Items.Add(rpath);
                        }
                    }
                    BackupChanges();
                    MakeUnsaved();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link sprites.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void VariantsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int sind = PartsListBox.SelectedIndex;
                if (sind < 0 || sind >= SelectedSprites.Count) return;
                if (SelectedSprites[sind] != VariantsListBox.SelectedIndex)
                {
                    SelectedSprites[sind] = VariantsListBox.SelectedIndex;
                    BackupChanges();
                    MakeUnsaved();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select variant.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GLSurface_GLStart(object sender, EventArgs e)
        {
        }
        private void GLSurface_GLStop(object sender, EventArgs e)
        {
            GLFrameTimer.Stop();
            LoadedResource.Dispose();
        }
        private void GLSurface_GLPaint(object sender, EventArgs e)
        {
            try
            {
                long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (LoadedResource.Transparency) gl.Color4ub(255, 255, 255, 150);
                else gl.Color4ub(255, 255, 255, 255);
                LoadedResource.Render(OffsetX, OffsetY, Angle, time, 1000, SelectedSprites); // FIX THIS SHITTTTTT

                float b = LoadedResource.PointBoundsX / GLSurface.Zoom;
                float w = LoadedResource.PointBoundsY / GLSurface.Zoom;
                float lx = LoadedResource.FireVectorX;
                float ly = LoadedResource.FireVectorY;
                float x1 = LoadedResource.FirePointX;
                float y1 = LoadedResource.FirePointY;
                float x2 = x1 + LoadedResource.FireVectorX;
                float y2 = y1 + LoadedResource.FireVectorY;
                float d = (float)Math.Sqrt(lx * lx + ly * ly);
                float cs = LoadedResource.FireVectorX / d;
                float sn = LoadedResource.FireVectorY / d;

                gl.Disable(GL.TEXTURE_2D);

                gl.Begin(GL.TRIANGLES);
                gl.Color4ub(255, 255, 255, 128);
                gl.Vertex2f(OffsetX + x2 + (-b * cs - 0 * sn), OffsetY + y2 + (-b * sn + 0 * cs));
                gl.Vertex2f(OffsetX + x2 + ((-b - w * 2f) * cs - w * 2f * sn), OffsetY + y2 + ((-b - w * 2f) * sn + w * 2f * cs));
                gl.Vertex2f(OffsetX + x2 + ((-b - w * 2f) * cs - -w * 2f * sn), OffsetY + y2 + ((-b - w * 2f) * sn + -w * 2f * cs));
                gl.End();

                float lx2 = x2 + ((-b - w * 2f) * cs - 0 * sn);
                float ly2 = y2 + ((-b - w * 2f) * sn + 0 * cs);

                gl.Begin(GL.QUADS);
                gl.Color4ub(255, 255, 255, 128);
                gl.Vertex2f(OffsetX + x1 - w * sn, OffsetY + y1 + w * cs);
                gl.Vertex2f(OffsetX + lx2 - w * sn, OffsetY + ly2 + w * cs);
                gl.Vertex2f(OffsetX + lx2 + w * sn, OffsetY + ly2 - w * cs);
                gl.Vertex2f(OffsetX + x1 + w * sn, OffsetY + y1 - w * cs);

                gl.Color4ub(0, 0, 0, 255);
                gl.Vertex2f(OffsetX + x2 - b, OffsetY + y2 - b);
                gl.Vertex2f(OffsetX + x2 + b, OffsetY + y2 - b);
                gl.Vertex2f(OffsetX + x2 + b, OffsetY + y2 + b);
                gl.Vertex2f(OffsetX + x2 - b, OffsetY + y2 + b);
                if (CapturedPoint == 2) gl.Color4ub(255, 0, 255, 255);
                else gl.Color4ub(255, 0, 255, 128);
                gl.Vertex2f(OffsetX + x2 - w, OffsetY + y2 - w);
                gl.Vertex2f(OffsetX + x2 + w, OffsetY + y2 - w);
                gl.Vertex2f(OffsetX + x2 + w, OffsetY + y2 + w);
                gl.Vertex2f(OffsetX + x2 - w, OffsetY + y2 + w);

                gl.Color4ub(0, 0, 0, 255);
                gl.Vertex2f(OffsetX + x1 - b, OffsetY + y1 - b);
                gl.Vertex2f(OffsetX + x1 + b, OffsetY + y1 - b);
                gl.Vertex2f(OffsetX + x1 + b, OffsetY + y1 + b);
                gl.Vertex2f(OffsetX + x1 - b, OffsetY + y1 + b);
                if (CapturedPoint == 1) gl.Color4ub(0, 255, 255, 255);
                else gl.Color4ub(0, 255, 255, 128);
                gl.Vertex2f(OffsetX + x1 - w, OffsetY + y1 - w);
                gl.Vertex2f(OffsetX + x1 + w, OffsetY + y1 - w);
                gl.Vertex2f(OffsetX + x1 + w, OffsetY + y1 + w);
                gl.Vertex2f(OffsetX + x1 - w, OffsetY + y1 + w);
                gl.End();

                if (CapturedPoint == 1)
                {
                    gl.Begin(GL.LINES);
                    gl.Color4ub(255, 0, 0, 128);
                    gl.Vertex2f(OffsetX + x1, -GLSurface.SurfaceSize.Height / 2f);
                    gl.Vertex2f(OffsetX + x1, GLSurface.SurfaceSize.Height / 2f);
                    gl.Vertex2f(-GLSurface.SurfaceSize.Width / 2f, OffsetY + y1);
                    gl.Vertex2f(GLSurface.SurfaceSize.Width / 2f, OffsetY + y1);
                    gl.End();
                }
                if (CapturedPoint == 2)
                {
                    gl.Begin(GL.LINES);
                    gl.Color4ub(255, 0, 0, 128);
                    gl.Vertex2f(OffsetX + x2, -GLSurface.SurfaceSize.Height / 2f);
                    gl.Vertex2f(OffsetX + x2, GLSurface.SurfaceSize.Height / 2f);
                    gl.Vertex2f(-GLSurface.SurfaceSize.Width / 2f, OffsetY + y2);
                    gl.Vertex2f(GLSurface.SurfaceSize.Width / 2f, OffsetY + y2);
                    gl.End();
                }
            }
            catch (Exception ex)
            {
                GLFrameTimer.Stop();
                MessageBox.Show(this, ex.ToString(), "Error: Can not render frame.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_GLMouseWheel(object sender, GLMouseEventArgs e)
        {
            try
            {
                if (false)
                {
                    //LoadedResource.Angle += e.Delta * (float)UCTNumeric.Increment;
                    //UCTNumeric.Value = (decimal)LoadedResource.Angle;

                    //Story.Item = new State(LoadedResource);
                }
                else
                {
                    if (ModifierKeys == Keys.Control)
                    {
                        if (LoadedResource.PointBoundsX + e.Delta > 1.0f)
                        {
                            LoadedResource.PointBoundsX += e.Delta;
                            LoadedResource.PointBoundsY += e.Delta;
                            Story.Item = new State(LoadedResource);
                        }
                    }
                    else
                    {
                        var z = 1f + e.Delta / 10f;
                        GLSurface.Zoom *= z;
                        OffsetX -= e.X * (z - 1f);
                        OffsetY -= e.Y * (z - 1f);
                        MouseManager.UpdateLocation(e.Location);

                        RepairOffset();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle MouseWheel event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_GLMouseDown(object sender, GLMouseEventArgs e)
        {
            try
            {
                MouseManager.BeginDrag(e.Location);

                float b = LoadedResource.PointBoundsX / GLSurface.Zoom;
                float x1 = LoadedResource.FirePointX;
                float y1 = LoadedResource.FirePointY;
                float x2 = x1 + LoadedResource.FireVectorX;
                float y2 = y1 + LoadedResource.FireVectorY;

                if (MouseManager.CurrentLocation.X >= OffsetX + x2 - b &&
                    MouseManager.CurrentLocation.X <= OffsetX + x2 + b &&
                    MouseManager.CurrentLocation.Y >= OffsetY + y2 - b &&
                    MouseManager.CurrentLocation.Y <= OffsetY + y2 + b) CapturedPoint = 2;
                else 
                if (MouseManager.CurrentLocation.X >= OffsetX + x1 - b &&
                    MouseManager.CurrentLocation.X <= OffsetX + x1 + b &&
                    MouseManager.CurrentLocation.Y >= OffsetY + y1 - b &&
                    MouseManager.CurrentLocation.Y <= OffsetY + y1 + b) CapturedPoint = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle MouseDown event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_GLMouseMove(object sender, GLMouseEventArgs e)
        {
            try
            {
                MouseManager.UpdateLocation(e.Location);

                if (e.Button.HasFlag(MouseButtons.Left))
                {
                    if (CapturedPoint == 1)
                    {
                        if (LoadedResource.PixelPerfect)
                        {
                            LoadedResource.FirePointX += MouseManager.CurrentStepDelta.X;
                            LoadedResource.FirePointY += MouseManager.CurrentStepDelta.Y;
                            FirePointXNumeric.Value = (decimal)LoadedResource.FirePointX;
                            FirePointYNumeric.Value = (decimal)LoadedResource.FirePointY;
                        }
                        else
                        {
                            LoadedResource.FirePointX += MouseManager.CurrentDelta.X;
                            LoadedResource.FirePointY += MouseManager.CurrentDelta.Y;
                            FirePointXNumeric.Value = (decimal)LoadedResource.FirePointX;
                            FirePointYNumeric.Value = (decimal)LoadedResource.FirePointY;
                        }
                    }
                    else if (CapturedPoint == 2)
                    {
                        if (LoadedResource.PixelPerfect)
                        {
                            LoadedResource.FireVectorX += MouseManager.CurrentStepDelta.X;
                            LoadedResource.FireVectorY += MouseManager.CurrentStepDelta.Y;
                            FireVectorXNumeric.Value = (decimal)LoadedResource.FireVectorX;
                            FireVectorYNumeric.Value = (decimal)LoadedResource.FireVectorY;
                        }
                        else
                        {
                            LoadedResource.FireVectorX += MouseManager.CurrentDelta.X;
                            LoadedResource.FireVectorY += MouseManager.CurrentDelta.Y;
                            FireVectorXNumeric.Value = (decimal)LoadedResource.FireVectorX;
                            FireVectorYNumeric.Value = (decimal)LoadedResource.FireVectorY;
                        }
                    }
                    else
                    {
                        OffsetX += MouseManager.CurrentDelta.X;
                        OffsetY += MouseManager.CurrentDelta.Y;
                    }
                }

                RepairOffset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle MouseMove event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_GLMouseUp(object sender, GLMouseEventArgs e)
        {
            try
            {
                MouseManager.EndDrag();
                if (CapturedPoint > 0) BackupChanges();
                CapturedPoint = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle MouseUp event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_GLSizeChanged(object sender, EventArgs e)
        {

        }
        private void GLFrameTimer_Tick(object sender, EventArgs e)
        {
            GLSurface.Refresh();
        }

        private void LinkSpriteMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int sind = PartsListBox.SelectedIndex;
                if (sind < 0 || sind >= LoadedResource.Sprites.Count) return;
                var sprites = LoadedResource.Sprites[sind];

                GLSurface.MakeCurrent();
                var subform = new ExplorerForm(Directory.GetCurrentDirectory(), true, ResourceType.Sprite);
                if (subform.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (var link in subform.SelectedResources)
                    {
                        sprites.Add(new Subresource<SpriteResource>(link));
                        VariantsListBox.Items.Add(link);
                    }
                    BackupChanges();
                    MakeUnsaved();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not .",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UnlinkSpriteMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                int sind = PartsListBox.SelectedIndex;
                if (sind < 0 || sind >= LoadedResource.Sprites.Count) return;
                var sprites = LoadedResource.Sprites[sind];
                int index = VariantsListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Sprites[sind].Count) return;

                sprites[index].Dispose();
                sprites.RemoveAt(index);
                VariantsListBox.Items.RemoveAt(index);
                BackupChanges();
                MakeUnsaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not unlink sprite.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreatePartMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                int index = PartsListBox.SelectedIndex + 1;
                if (index < 0 || index > LoadedResource.Count) return;
                LoadedResource.Sprites.Insert(index, new List<Subresource<SpriteResource>>());
                SelectedSprites.Insert(index, -1);
                LoadedResource.SpriteLockedOnCycle.Insert(index, false);
                PartsListBox.Items.Add("Part: " + PartsListBox.Items.Count);
                BackupChanges();
                MakeUnsaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not create part.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemovePartMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int index = PartsListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Count) return;
                LoadedResource.Sprites[index].ForEach((Subresource<SpriteResource> s) => s.Dispose());
                LoadedResource.Sprites.RemoveAt(index);
                SelectedSprites.RemoveAt(index);
                LoadedResource.SpriteLockedOnCycle.RemoveAt(index);
                PartsListBox.Items.RemoveAt(PartsListBox.Items.Count - 1);
                BackupChanges();
                MakeUnsaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not remove part.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AdjustFireVectorMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LoadedResource.FireVectorX = 1.0f;
                LoadedResource.FireVectorY = 0.0f;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not .",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ToggleTransparencyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = sender as ToolStripMenuItem;
                if (item != null)
                {
                    if (LoadedResource.Transparency != (item.Checked = !item.Checked))
                    {
                        LoadedResource.Transparency = item.Checked;
                        MakeUnsaved();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle transparency.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PixelPerfectMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = sender as ToolStripMenuItem;
                if (item != null)
                {
                    if (LoadedResource.PixelPerfect != (item.Checked = !item.Checked))
                    {
                        LoadedResource.PixelPerfect = item.Checked;
                        MakeUnsaved();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle pixel perfect.",
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
                OffsetX = 0f;
                OffsetY = 0f;
                Angle = 0f;
                GLSurface.Zoom = 64f;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not reset position.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
