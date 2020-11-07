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
using Resource_Redactor.Resources.Interface;

namespace Resource_Redactor.Resources.Redactors
{
    [DefaultEvent("StateChanged")]
    public partial class InterfaceControl : ResourceControl<InterfaceResource, StoryItem<InterfaceControl.State>>, IResourceControl
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
        private InterfaceElement CapturedElement = null;
        private SizingCornerType SizingCorner = SizingCornerType.None;
        private static readonly int BorderSize = 2;
        private float OffsetX = 0, OffsetY = 0;
        private InterfaceElement SelectedContainer = null;
        private IList<InterfaceElementBase> SelectedElements { get => SelectedContainer?.Elements ?? LoadedResource.BaseElement.Elements; }
        private InterfaceElement SelectedElement
        {
            get
            {
                int index = ElementsListBox.SelectedIndex;
                if (index >= 0 && index < SelectedElements.Count) return SelectedElements[index] as InterfaceElement;
                else return LoadedResource.BaseElement as InterfaceElement;
            }
            set => ElementsListBox.SelectedIndex = SelectedElements.IndexOf(value);
        }

        public struct State
        {
        };

        public InterfaceControl(string path)
        {
            InitializeComponent();

            var element_names = Enum.GetNames(typeof(InterfaceElementType));
            var factory_tool_strip_items = new ToolStripItem[element_names.Length - 1];
            for (int i = 1; i < element_names.Length; i++)
            {
                factory_tool_strip_items[i - 1] = new ToolStripMenuItem(element_names[i], null, CreateMenuItem_Click);
            }

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Create", null, factory_tool_strip_items),
                new ToolStripMenuItem("Remove", null, BackColorMenuItem_Click, Keys.Control | Keys.D),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };

            GLSurface.MakeCurrent();
            Open(path);
            Story = new StoryItem<State>(new State());
            Story.ValueChanged += Story_ValueChanged;

            ElementsListBox.BeginUpdate();
            foreach (var element in SelectedElements)
                ElementsListBox.Items.Add(element.Type.ToString());
            ElementsListBox.EndUpdate();

            GLSurface.BackColor = LoadedResource.BackColor;

            OffsetX = -LoadedResource.BaseElement.ExtentX / 2.0f;
            OffsetY = -LoadedResource.BaseElement.ExtentY / 2.0f;

            ElementPropertyGrid.SelectedObject = SelectedElement;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GLSurface.MakeCurrent();
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
        private void UpdateCursor(PointF l)
        {
            float px = l.X;
            float py = l.Y;

            var cursor = Cursors.Arrow;

            for (int i = SelectedElements.Count - 1; i >= 0; i--)
            {
                var element = SelectedElements[i];

                float lx = OffsetX + element.DesignerPositionX;
                float rx = lx + element.ExtentX;
                float dy = OffsetY + element.DesignerPositionY;
                float uy = dy + element.ExtentY;

                if (px >= lx && px <= rx && py >= dy && py <= uy)
                {
                    cursor = Cursors.SizeAll;
                    break;
                }

                var sizing_corner = SizingCornerType.None;

                if (px >= lx - BorderSize && px <= lx && py >= dy - BorderSize && py <= uy + BorderSize)
                    sizing_corner |= SizingCornerType.Left;

                if (px >= rx && px <= rx + BorderSize && py >= dy - BorderSize && py <= uy + BorderSize)
                    sizing_corner |= SizingCornerType.Right;

                if (px >= lx - BorderSize && px <= rx + BorderSize && py >= dy - BorderSize && py <= dy)
                    sizing_corner |= SizingCornerType.Down;

                if (px >= lx - BorderSize && px <= rx + BorderSize && py >= uy && py <= uy + BorderSize)
                    sizing_corner |= SizingCornerType.Up;

                if (sizing_corner != SizingCornerType.None)
                {
                    if (sizing_corner.HasFlag(SizingCornerType.Left | SizingCornerType.Up))
                        cursor = Cursors.SizeNWSE;
                    else if (sizing_corner.HasFlag(SizingCornerType.Right | SizingCornerType.Down))
                        cursor = Cursors.SizeNWSE;
                    else if (sizing_corner.HasFlag(SizingCornerType.Left | SizingCornerType.Down))
                        cursor = Cursors.SizeNESW;
                    else if (sizing_corner.HasFlag(SizingCornerType.Right | SizingCornerType.Up))
                        cursor = Cursors.SizeNESW;
                    else if(sizing_corner.HasFlag(SizingCornerType.Up) || sizing_corner.HasFlag(SizingCornerType.Down))
                        cursor = Cursors.SizeNS;
                    else if (sizing_corner.HasFlag(SizingCornerType.Left) || sizing_corner.HasFlag(SizingCornerType.Right))
                        cursor = Cursors.SizeWE;
                    break;
                }
            }

            Cursor = cursor;
        }

        private void Story_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //Story.Item.ToResource(LoadedResource);
                //
                //RepairOffset();
                //
                //GLSurface.BackColor = LoadedResource.BackColor;
                //FramesNumeric.Value = LoadedResource.FrameCount;
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
                foreach (var element in SelectedElements) 
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

        private void CreateMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = sender as ToolStripMenuItem;

                SelectedElements.Add(InterfaceElement.Factory(
                    (InterfaceElementType)Enum.Parse(typeof(InterfaceElementType), item.Text)));
                ElementsListBox.Items.Add(item.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not create element.",
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
                    int x = (int)OffsetX + SelectedElement.DesignerPositionX;
                    int y = (int)OffsetY + SelectedElement.DesignerPositionY;

                    gl.Disable(GL.TEXTURE_2D);
                    gl.Color4ub(0, 255, 0, 255);
                    gl.Begin(GL.LINE_LOOP);
                    gl.Vertex2i(x - BorderSize, y - BorderSize);
                    gl.Vertex2i(x + SelectedElement.ExtentX + BorderSize, y - BorderSize);
                    gl.Vertex2i(x + SelectedElement.ExtentX + BorderSize, y + SelectedElement.ExtentY + BorderSize);
                    gl.Vertex2i(x - BorderSize, y + SelectedElement.ExtentY + BorderSize);
                    gl.End();
                }
                if (SelectedContainer != null)
                {
                    int x = (int)OffsetX + SelectedContainer.DesignerPositionX;
                    int y = (int)OffsetY + SelectedContainer.DesignerPositionY;

                    gl.Disable(GL.TEXTURE_2D);
                    gl.Color4ub(128, 0, 255, 255);
                    gl.Begin(GL.LINE_LOOP);
                    gl.Vertex2i(x - BorderSize, y - BorderSize);
                    gl.Vertex2i(x + SelectedContainer.ExtentX + BorderSize, y - BorderSize);
                    gl.Vertex2i(x + SelectedContainer.ExtentX + BorderSize, y + SelectedContainer.ExtentY + BorderSize);
                    gl.Vertex2i(x - BorderSize, y + SelectedContainer.ExtentY + BorderSize);
                    gl.End();
                }
            }
            catch (Exception ex)
            {
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
            GLSurface.Focus();
            try
            {
                float px = MouseManager.CurrentLocation.X;
                float py = MouseManager.CurrentLocation.Y;

                if (e.Button.HasFlag(MouseButtons.Left))
                {
                    MouseManager.BeginDrag(e.Location);

                    SelectedElement = null;
                    for (int i = SelectedElements.Count - 1; i >= 0; i--)
                    {
                        var element = SelectedElements[i] as InterfaceElement;

                        float lx = OffsetX + element.DesignerPositionX;
                        float rx = lx + element.ExtentX;
                        float dy = OffsetY + element.DesignerPositionY;
                        float uy = dy + element.ExtentY;

                        if (px >= lx && px <= rx && py >= dy && py <= uy)
                        {
                            SelectedElement = CapturedElement = element;
                            break;
                        }

                        if (px >= lx - BorderSize && px <= lx && py >= dy - BorderSize && py <= uy + BorderSize) 
                            SizingCorner |= SizingCornerType.Left;

                        if (px >= rx && px <= rx + BorderSize && py >= dy - BorderSize && py <= uy + BorderSize) 
                            SizingCorner |= SizingCornerType.Right;

                        if (px >= lx - BorderSize && px <= rx + BorderSize && py >= dy - BorderSize && py <= dy) 
                            SizingCorner |= SizingCornerType.Down;

                        if (px >= lx - BorderSize && px <= rx + BorderSize && py >= uy && py <= uy + BorderSize) 
                            SizingCorner |= SizingCornerType.Up;

                        if (SizingCorner != SizingCornerType.None)
                        {
                            SelectedElement = CapturedElement = element;
                            break;
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
                        if (SizingCorner == SizingCornerType.None)
                        {
                            CapturedElement.DesignerPositionX += (int)MouseManager.CurrentStepDelta.X;
                            CapturedElement.DesignerPositionY += (int)MouseManager.CurrentStepDelta.Y;
                        }
                        else
                        {
                            int dx = (int)MouseManager.CurrentStepDelta.X;
                            int dy = (int)MouseManager.CurrentStepDelta.Y;

                            if (SizingCorner.HasFlag(SizingCornerType.Up))
                            {
                                SelectedElement.ExtentY += dy;
                            }
                            if (SizingCorner.HasFlag(SizingCornerType.Right))
                            {
                                SelectedElement.ExtentX += dx;
                            }
                            if (SizingCorner.HasFlag(SizingCornerType.Down))
                            {
                                SelectedElement.ExtentY -= dy;
                                SelectedElement.DesignerPositionY += dy;
                            }
                            if (SizingCorner.HasFlag(SizingCornerType.Left))
                            {
                                SelectedElement.ExtentX -= dx;
                                SelectedElement.DesignerPositionX += dx;
                            }
                        }
                    }
                }

                RepairOffset();

                UpdateCursor(MouseManager.CurrentLocation);
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

        private void BackColorMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BackgroundColorDialog.Color = LoadedResource.BackColor;
                if (BackgroundColorDialog.ShowDialog(this) != DialogResult.OK) return;
                if (LoadedResource.BackColor == BackgroundColorDialog.Color) return;

                LoadedResource.BackColor = BackgroundColorDialog.Color;
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
                OffsetX = -LoadedResource.BaseElement.ExtentX / 2.0f;
                OffsetY = -LoadedResource.BaseElement.ExtentY / 2.0f;
                GLSurface.Zoom = 1f;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not reset position.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (SelectedElement != null)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up: if (e.Shift) SelectedElement.ExtentY++; 
                        else SelectedElement.DesignerPositionY++; break;
                    case Keys.Left: if (e.Shift) SelectedElement.ExtentX--; 
                        else SelectedElement.DesignerPositionX--; break;
                    case Keys.Down: if (e.Shift) SelectedElement.ExtentY--; 
                        else SelectedElement.DesignerPositionY--; break;
                    case Keys.Right: if (e.Shift) SelectedElement.ExtentX++; 
                        else SelectedElement.DesignerPositionX++; break;
                }
                ElementPropertyGrid.SelectedObject = SelectedElement;
            }
        }
        private void GLSurface_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectedContainer = SelectedElement;
        }

        public override void Render()
        {
            GLSurface.Refresh();
        }
    }
}
