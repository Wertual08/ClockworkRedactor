using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtraSharp;
using ExtraForms.OpenGL;
using ExtraForms;
using System.IO;

namespace Resource_Redactor.Descriptions.Redactors
{
    [DefaultEvent("StateChanged")]
    public partial class SpriteControl : UserControl, IResourceControl
    {
        private DragManager MouseManager = new DragManager(0.015625f);
        private bool PointCaptured = false;
        private float OffsetX = 0.0f, OffsetY = 0.0f, Angle = 0.0f;

        struct State
        {
            public int FramesCount;
            public int FrameDelay;
            public float ImgboxW;
            public float ImgboxH;
            public float AxisX;
            public float AxisY;
            public float Angle;
            public bool VerticalFrames;
            public string TextureLink;

            public int BackColor;
            public float PointBoundsX;
            public float PointBoundsY;

            public State(SpriteResource r)
            {
                FramesCount = r.FramesCount;
                FrameDelay = r.FrameDelay;
                ImgboxW = r.ImgboxW;
                ImgboxH = r.ImgboxH;
                AxisX = r.AxisX;
                AxisY = r.AxisY;
                Angle = r.Angle;
                VerticalFrames = r.VerticalFrames;
                TextureLink = r.Texture.Link;

                BackColor = r.BackColor;
                PointBoundsX = r.PointBoundsX;
                PointBoundsY = r.PointBoundsY;
            }
            public void ToResource(SpriteResource r)
            {
                r.FramesCount = FramesCount;
                r.FrameDelay = FrameDelay;
                r.ImgboxW = ImgboxW;
                r.ImgboxH = ImgboxH;
                r.AxisX = AxisX;
                r.AxisY = AxisY;
                r.Angle = Angle;
                r.VerticalFrames = VerticalFrames;
                if (r.Texture.Link != TextureLink) r.Texture.Link = TextureLink;
                r.BackColor = BackColor;
                r.PointBoundsX = PointBoundsX;
                r.PointBoundsY = PointBoundsY;
            }
        };

        private StoryItem<State> Story;
        private SpriteResource LoadedResource = null;

        public ToolStripMenuItem[] MenuTabs { get; private set; }
        public bool Saved { get; private set; } = true;
        public bool UndoEnabled { get { return Story.PrevState; } }
        public bool RedoEnabled { get { return Story.NextState; } }

        public event StateChangedEventHandler StateChanged;

        public string ResourcePath { get; private set; }
        public string ResourceName { get; private set; }

        public int FPS
        {
            get { return 1000 / GLFrameTimer.Interval; }
            set { GLFrameTimer.Interval = Math.Max(1, 1000 / value); }
        }
        public void Activate()
        {
            GLSurface.MakeCurrent();
        }

        public void Save(string path)
        {
            ResourcePath = path;
            ResourceName = Path.GetFileName(path);

            LoadedResource.Save(path);

            Saved = true;
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        public void Undo()
        {
            Story.Undo();
        }
        public void Redo()
        {
            Story.Redo();
        }

        public SpriteControl(string path)
        {
            InitializeComponent();

            FramesNumeric.FixMouseWheel();
            DelayNumeric.FixMouseWheel();
            ImgboxWNumeric.FixMouseWheel();
            ImgboxHNumeric.FixMouseWheel();
            AxisXNumeric.FixMouseWheel();
            AxisYNumeric.FixMouseWheel();
            AngleNumeric.FixMouseWheel();

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Link texture", null, LinkTextureMenuItem_Click, Keys.Control | Keys.I),
                new ToolStripMenuItem("Adjust size", null, AdjustSizeMenuItem_Click, Keys.Control | Keys.A),
                new ToolStripMenuItem("Pixel perfect", null, PixelPerfectMenuItem_Click, Keys.Control | Keys.P),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };

            GLSurface.MakeCurrent();
            LoadedResource = new SpriteResource(path);
            Story = new StoryItem<State>(new State(LoadedResource));
            Story.ValueChanged += Story_ValueChanged;

            MenuTabs[4].Checked = LoadedResource.PixelPerfect;

            ResourcePath = path;
            ResourceName = Path.GetFileName(path);

            LoadedResource.Texture.SynchronizingObject = this;
            LoadedResource.Texture.Reloaded += Texture_Reloaded;

            FramesNumeric.Value = LoadedResource.FramesCount;
            DelayNumeric.Value = (decimal)LoadedResource.FrameDelay;
            ImgboxWNumeric.Value = (decimal)LoadedResource.ImgboxW;
            ImgboxHNumeric.Value = (decimal)LoadedResource.ImgboxH;
            AxisXNumeric.Value = (decimal)LoadedResource.AxisX;
            AxisYNumeric.Value = (decimal)LoadedResource.AxisY;
            AngleNumeric.Value = (decimal)LoadedResource.Angle;
            VFramesCheckBox.Checked = LoadedResource.VerticalFrames;
            LinkTextBox.Text = LoadedResource.Texture.Link;

            GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);

            GLFrameTimer.Start();
        }

        private void RepairOffset()
        {
            var bounds = LoadedResource.GetBounds();

            float sw = GLSurface.SurfaceSize.Width / 2.0f;
            float sh = GLSurface.SurfaceSize.Height / 2.0f;
            
            float lx = bounds.Left;
            float rx = bounds.Right;
            float dy = bounds.Top;
            float uy = bounds.Bottom;

            if (OffsetX + rx < -sw) OffsetX = -sw - rx;
            if (OffsetX + lx > sw) OffsetX = sw - lx;
            if (OffsetY + uy < -sh) OffsetY = -sh - uy;
            if (OffsetY + dy > sh) OffsetY = sh - dy;
        }
        private void UpdateRedactor()
        {
            StateChanged.Invoke(this, EventArgs.Empty);
        }
        private void MakeUnsaved()
        {
            Saved = false;
            UpdateRedactor();
        }

        private void Texture_Reloaded(object sender, EventArgs e)
        {
            LinkTextBox.Text = LoadedResource.Texture.Link;
            if (!LoadedResource.Texture.Loaded) LinkTextBox.BackColor = Color.Red;
            else LinkTextBox.BackColor = SystemColors.Control;
        }
        private void Story_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Story.Item.ToResource(LoadedResource);

                RepairOffset();

                GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);
                FramesNumeric.Value = LoadedResource.FramesCount;
                DelayNumeric.Value = (decimal)LoadedResource.FrameDelay;
                ImgboxWNumeric.Value = (decimal)LoadedResource.ImgboxW;
                ImgboxHNumeric.Value = (decimal)LoadedResource.ImgboxH;
                AxisXNumeric.Value = (decimal)LoadedResource.AxisX;
                AxisYNumeric.Value = (decimal)LoadedResource.AxisY;
                AngleNumeric.Value = (decimal)LoadedResource.Angle;
                VFramesCheckBox.Checked = LoadedResource.VerticalFrames;
                LinkTextBox.Text = LoadedResource.Texture.Link;

                MakeUnsaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not update resource data.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Numeric_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                AngleNumeric.Value %= (decimal)Math.PI * 2m;
                if (AngleNumeric.Value < 0) AngleNumeric.Value += (decimal)Math.PI * 2m;
                LoadedResource.FramesCount = (int)FramesNumeric.Value;
                LoadedResource.FrameDelay = (int)DelayNumeric.Value;
                LoadedResource.ImgboxW = (float)ImgboxWNumeric.Value;
                LoadedResource.ImgboxH = (float)ImgboxHNumeric.Value;
                LoadedResource.AxisX = (float)AxisXNumeric.Value;
                LoadedResource.AxisY = (float)AxisYNumeric.Value;
                LoadedResource.Angle = (float)AngleNumeric.Value;
                LoadedResource.VerticalFrames = VFramesCheckBox.Checked;
                Story.Item = new State(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change resource properties.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LinkTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadedResource.Texture.Link = LinkTextBox.Text;
                if (!LoadedResource.Texture.Loaded) LinkTextBox.BackColor = Color.Red;
                else LinkTextBox.BackColor = SystemColors.Control;

                Story.Item = new State(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link texture.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LinkTextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths.Length != 1 || Resource.GetType(paths[0]) != ResourceType.Texture) return;
            if (e.AllowedEffect.HasFlag(DragDropEffects.Link))
                e.Effect = DragDropEffects.Link;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }
        private void LinkTextBox_DragDrop(object sender, DragEventArgs e)
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

                    var fpath = paths[0];
                    var dpath = Directory.GetCurrentDirectory();
                    if (!fpath.StartsWith(dpath)) MessageBox.Show(this,
                        "Resource is not in description directory.",
                        "Error: Invalid resource.", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    else LinkTextBox.Text = ExtraPath.MakeDirectoryRelated(dpath, fpath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link texture.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GLSurface_GLStart(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not start OpenGL.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_GLStop(object sender, EventArgs e)
        {
            try
            {
                GLFrameTimer.Stop();
                LoadedResource.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not stop OpenGL.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_GLPaint(object sender, EventArgs e)
        {
            try
            {
                gl.Color4ub(255, 255, 255, 255);
                LoadedResource.Render(OffsetX, OffsetY, Angle,
                    DateTimeOffset.Now.ToUnixTimeMilliseconds());

                float b = LoadedResource.PointBoundsX / GLSurface.Zoom;
                float s = LoadedResource.PointBoundsY / GLSurface.Zoom;

                gl.Disable(GL.TEXTURE_2D);

                if (PointCaptured)
                {
                    gl.Begin(GL.LINES);
                    gl.Color4ub(255, 0, 0, 128);
                    gl.Vertex2f(OffsetX, -GLSurface.SurfaceSize.Height / 2f);
                    gl.Vertex2f(OffsetX, GLSurface.SurfaceSize.Height / 2f);
                    gl.Vertex2f(-GLSurface.SurfaceSize.Width / 2f, OffsetY);
                    gl.Vertex2f(GLSurface.SurfaceSize.Width / 2f, OffsetY);
                    gl.End();
                }

                gl.Begin(GL.QUADS);
                gl.Color4ub(0, 0, 0, 255);
                gl.Vertex2f(OffsetX - b, OffsetY - b);
                gl.Vertex2f(OffsetX + b, OffsetY - b);
                gl.Vertex2f(OffsetX + b, OffsetY + b);
                gl.Vertex2f(OffsetX - b, OffsetY + b);
                if (PointCaptured) gl.Color4ub(255, 0, 255, 255);
                else gl.Color4ub(255, 0, 255, 128);
                gl.Vertex2f(OffsetX - s, OffsetY - s);
                gl.Vertex2f(OffsetX + s, OffsetY - s);
                gl.Vertex2f(OffsetX + s, OffsetY + s);
                gl.Vertex2f(OffsetX - s, OffsetY + s);
                gl.End();
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
                if (PointCaptured)
                {
                    LoadedResource.Angle += e.Delta * (float)AngleNumeric.Increment;
                    AngleNumeric.Value = (decimal)LoadedResource.Angle;

                    Story.Item = new State(LoadedResource);
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
                if (MouseManager.CurrentLocation.X >= OffsetX - b &&
                    MouseManager.CurrentLocation.X <= OffsetX + b &&
                    MouseManager.CurrentLocation.Y >= OffsetY - b &&
                    MouseManager.CurrentLocation.Y <= OffsetY + b) PointCaptured = true;
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
                    if (PointCaptured)
                    {
                        if (LoadedResource.PixelPerfect)
                        {
                            LoadedResource.AxisX += MouseManager.CurrentStepDelta.X;
                            LoadedResource.AxisY += MouseManager.CurrentStepDelta.Y;
                            OffsetX += MouseManager.CurrentStepDelta.X;
                            OffsetY += MouseManager.CurrentStepDelta.Y;
                            AxisXNumeric.Value = (decimal)LoadedResource.AxisX;
                            AxisYNumeric.Value = (decimal)LoadedResource.AxisY;
                        }
                        else
                        {
                            LoadedResource.AxisX += MouseManager.CurrentDelta.X;
                            LoadedResource.AxisY += MouseManager.CurrentDelta.Y;
                            OffsetX += MouseManager.CurrentDelta.X;
                            OffsetY += MouseManager.CurrentDelta.Y;
                            AxisXNumeric.Value = (decimal)LoadedResource.AxisX;
                            AxisYNumeric.Value = (decimal)LoadedResource.AxisY;
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
                PointCaptured = false;
                Story.Item = new State(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle MouseUp event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_GLSizeChanged(object sender, EventArgs e)
        {
            try
            {
                RepairOffset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle SizeChanged event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLFrameTimer_Tick(object sender, EventArgs e)
        {
            GLSurface.Refresh();
        }

        private void LinkTextureMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                var subform = new ExplorerForm(Directory.GetCurrentDirectory(), false, ResourceType.Texture);
                if (subform.ShowDialog(this) == DialogResult.OK && subform.SelectedResources.Count == 1)
                    LinkTextBox.Text = subform.SelectedResources[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link texture.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AdjustSizeMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LoadedResource.AdjustImgbox();
                Story.Item = new State(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not adjust size.",
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
