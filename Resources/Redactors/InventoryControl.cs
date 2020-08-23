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

namespace Resource_Redactor.Resources.Redactors
{
    [DefaultEvent("StateChanged")]
    public partial class InventoryControl : ResourceControl<InventoryResource, StoryItem<InventoryControl.State>>, IResourceControl
    {
        private DragManager MouseManager = new DragManager(1.0f);
        private enum SizingCornerType
        {
            None = 0b0,
            Up = 0b1,
            Left = 0b10,
            Down = 0b100,
            Right = 0b1000,
        }
        private InventoryResource.Element CapturedElement = null;
        private SizingCornerType SizingCorner = SizingCornerType.None;
        private static readonly int BorderSize = 2;
        private float OffsetX = 0, OffsetY = 0;
        private InventoryResource.Element SelectedElement
        {
            get
            {
                int index = ElementsListBox.SelectedIndex;
                if (index >= 0 && index < LoadedResource.Count) return LoadedResource[index];
                else return null;
            }
            set => ElementsListBox.SelectedIndex = LoadedResource.Elements.IndexOf(value);
        }

        public struct State
        {
        };

        public int FPS
        {
            get { return 1000 / GLFrameTimer.Interval; }
            set { GLFrameTimer.Interval = Math.Max(1, 1000 / value); }
        }

        public InventoryControl(string path)
        {
            InitializeComponent();

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Create", null, new ToolStripItem[] {
                    new ToolStripMenuItem("Panel", null, CreatePanelMenuItem_Click),
                }),
                new ToolStripMenuItem("Remove", null, BackColorMenuItem_Click, Keys.Control | Keys.D),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };

            GLSurface.MakeCurrent();
            Open(path);
            Story = new StoryItem<State>(new State());
            Story.ValueChanged += Story_ValueChanged;

            ElementsListBox.BeginUpdate();
            foreach (var element in LoadedResource.Elements)
                ElementsListBox.Items.Add(element.Type.ToString());
            ElementsListBox.EndUpdate();

            GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);

            OffsetX = -LoadedResource.Width / 2.0f;
            OffsetY = -LoadedResource.Height / 2.0f;

            GLFrameTimer.Start();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GLSurface.MakeCurrent();
                GLFrameTimer.Stop();
                LoadedResource.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
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

        private void Story_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //Story.Item.ToResource(LoadedResource);
                //
                //RepairOffset();
                //
                //GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);
                //FramesNumeric.Value = LoadedResource.FramesCount;
                //DelayNumeric.Value = (decimal)LoadedResource.FrameDelay;
                //ImgboxWNumeric.Value = (decimal)LoadedResource.ImgboxW;
                //ImgboxHNumeric.Value = (decimal)LoadedResource.ImgboxH;
                //AxisXNumeric.Value = (decimal)LoadedResource.AxisX;
                //AxisYNumeric.Value = (decimal)LoadedResource.AxisY;
                //AngleNumeric.Value = (decimal)LoadedResource.Angle;
                //VFramesCheckBox.Checked = LoadedResource.VerticalFrames;
                //LinkTextBox.Text = LoadedResource.Texture.Link;
                //
                //MakeUnsaved();

                int index = ElementsListBox.SelectedIndex;
                ElementsListBox.BeginUpdate();
                ElementsListBox.Items.Clear();
                foreach (var element in LoadedResource.Elements) 
                    ElementsListBox.Items.Add(element.Type.ToString());
                ElementsListBox.SelectedIndex = Math.Min(ElementsListBox.Items.Count - 1, index);
                ElementsListBox.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not update resource data.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreatePanelMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LoadedResource.Elements.Add(new InventoryResource.Panel());
                ElementsListBox.Items.Add("Panel");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not create panel.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ElementsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ElementPropertyGrid.SelectedObject = SelectedElement;
        }

        private void GLSurface_GLPaint(object sender, EventArgs e)
        {
            try
            {
                LoadedResource.Render((int)OffsetX, (int)OffsetY, DateTimeOffset.Now.ToUnixTimeMilliseconds());

                if (SelectedElement != null)
                {
                    int ox = (int)OffsetX;
                    int oy = (int)OffsetY;

                    gl.Disable(GL.TEXTURE_2D);
                    gl.Color4ub(0, 255, 0, 255);
                    gl.Begin(GL.LINE_LOOP);
                    gl.Vertex2i(ox + SelectedElement.PositionX - BorderSize, oy + SelectedElement.PositionY - BorderSize);
                    gl.Vertex2i(ox + SelectedElement.PositionX + SelectedElement.Width + BorderSize, oy + SelectedElement.PositionY - BorderSize);
                    gl.Vertex2i(ox + SelectedElement.PositionX + SelectedElement.Width + BorderSize, oy + SelectedElement.PositionY + SelectedElement.Height + BorderSize);
                    gl.Vertex2i(ox + SelectedElement.PositionX - BorderSize, oy + SelectedElement.PositionY + SelectedElement.Height + BorderSize);
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
                var z = e.Delta / 10f;
                GLSurface.Zoom *= 1f + z;
                OffsetX -= e.X * z;
                OffsetY -= e.Y * z;
                MouseManager.UpdateLocation(e.Location);

                RepairOffset();
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
                if (e.Button.HasFlag(MouseButtons.Left))
                {
                    MouseManager.BeginDrag(e.Location);
                    for (int i = LoadedResource.Count - 1; i >= 0; i--)
                    {
                        var element = LoadedResource[i];

                        float lx = OffsetX + element.PositionX;
                        float rx = lx + element.Width;
                        float dy = OffsetY + element.PositionY;
                        float uy = dy + element.Height;

                        if (e.X >= lx && e.X <= rx && e.Y >= dy && e.Y <= uy)
                        {
                            SelectedElement = CapturedElement = element;
                            break;
                        }

                        if (element == SelectedElement)
                        {
                            lx -= BorderSize;
                            if (e.X >= lx) SizingCorner |= SizingCornerType.Left;
                            rx += BorderSize;
                            if (e.X <= rx) SizingCorner |= SizingCornerType.Right;
                            dy -= BorderSize;
                            if (e.Y >= dy) SizingCorner |= SizingCornerType.Down;
                            uy += BorderSize;
                            if (e.Y <= uy) SizingCorner |= SizingCornerType.Up;
                        }
                    }
                }

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
                    if (CapturedElement == null)
                    {
                        OffsetX += MouseManager.CurrentDelta.X;
                        OffsetY += MouseManager.CurrentDelta.Y;
                    }
                    else
                    {
                        CapturedElement.PositionX += (int)MouseManager.CurrentStepDelta.X;
                        CapturedElement.PositionY += (int)MouseManager.CurrentStepDelta.Y;
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
                if (CapturedElement != null)
                {
                    ElementPropertyGrid.SelectedObject = CapturedElement;
                    CapturedElement = null;
                }
                SizingCorner = SizingCornerType.None;
                //Story.Item = new State(LoadedResource);
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

        private void BackColorMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BackgroundColorDialog.Color = Color.FromArgb(LoadedResource.BackColor);
                if (BackgroundColorDialog.ShowDialog(this) != DialogResult.OK) return;
                if (LoadedResource.BackColor == BackgroundColorDialog.Color.ToArgb()) return;

                LoadedResource.BackColor = BackgroundColorDialog.Color.ToArgb();
                //Story.Item = new State(LoadedResource);
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
                OffsetX = -LoadedResource.Width / 2.0f;
                OffsetY = -LoadedResource.Height / 2.0f;
                GLSurface.Zoom = 1f;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not reset position.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
