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
using ExtraForms;
using System.IO;
using System.Globalization;
using ExtraForms.OpenGL;

namespace Resource_Redactor.Resources.Redactors
{
    public partial class TileControl : ResourceControl<TileResource, StoryItem<TileControl.State>>, IResourceControl
    {
        private static readonly string TileName = "|\\{:Super***77_#[#]Zalupa:}//|";
        private DragManager MouseManager = new DragManager(0.015625f);
        private float OffsetX = 0.0f, OffsetY = 0.0f;
        private enum MouseStatus
        {
            None,
            Removing,
            Placing,
        }
        private MouseStatus MouseState;

        public struct State
        {
            public string TextureLink;

            public TileResource.Property Properties;
            public TileResource.Shape Form;
            public TileResource.Anchor Anchors;
            public TileResource.Reaction Reactions;
            public int Solidity;
            public uint Light;

            public int Layer;
            public int PartSize;
            public int FramesCount;
            public int FrameDelay;

            public int OffsetX;
            public int OffsetY;

            public string SetupEvent;
            public string ReformEvent;
            public string TouchEvent;
            public string ActivateEvent;
            public string RecieveEvent;
            public string RemoveEvent;

            public int BackColor;
            public bool GridEnabled;

            public State(TileResource r)
            {
                TextureLink = r.Texture.Link;

                Properties = r.Properties;
                Form = r.Form;
                Anchors = r.Anchors;
                Reactions = r.Reactions;
                Solidity = r.Solidity;
                Light = r.Light;

                Layer = r.Layer;
                PartSize = r.PartSize;
                FramesCount = r.FramesCount;
                FrameDelay = r.FrameDelay;

                OffsetX = r.OffsetX;
                OffsetY = r.OffsetY;

                SetupEvent = r.SetupEvent.Link;
                ReformEvent = r.ReformEvent.Link;
                TouchEvent = r.TouchEvent.Link;
                ActivateEvent = r.ActivateEvent.Link;
                RecieveEvent = r.RecieveEvent.Link;
                RemoveEvent = r.RemoveEvent.Link;

                BackColor = r.BackColor;
                GridEnabled = r.GridEnabled;
            }
            public void ToResource(TileResource r)
            {
                r.Texture.Link = TextureLink;

                r.Properties = Properties;
                r.Form = Form;
                r.Anchors = Anchors;
                r.Reactions = Reactions;
                r.Solidity = Solidity;
                r.Light = Light;

                r.Layer = Layer;
                r.PartSize = PartSize;
                r.FramesCount = FramesCount;
                r.FrameDelay = FrameDelay;

                r.OffsetX = OffsetX;
                r.OffsetY = OffsetY;

                r.SetupEvent.Link = SetupEvent;
                r.ReformEvent.Link = ReformEvent;
                r.TouchEvent.Link = TouchEvent;
                r.ActivateEvent.Link = ActivateEvent;
                r.RecieveEvent.Link = RecieveEvent;
                r.RemoveEvent.Link = RemoveEvent;

                r.BackColor = BackColor;
                r.GridEnabled = GridEnabled;
            }
            public override bool Equals(object obj)
            {
                if (!(obj is State)) return false;
                var s = (State)obj;

                if (s.TextureLink != TextureLink) return false;
                if (s.Properties != Properties) return false;
                if (s.Form != Form) return false;
                if (s.Anchors != Anchors) return false;
                if (s.Reactions != Reactions) return false;
                if (s.Solidity != Solidity) return false;
                if (s.Light != Light) return false;
                if (s.Layer != Layer) return false;
                if (s.PartSize != PartSize) return false;
                if (s.FramesCount != FramesCount) return false;
                if (s.FrameDelay != FrameDelay) return false;
                if (s.OffsetX != OffsetX) return false;
                if (s.OffsetY != OffsetY) return false;
                if (s.SetupEvent != SetupEvent) return false;
                if (s.ReformEvent != ReformEvent) return false;
                if (s.TouchEvent != TouchEvent) return false;
                if (s.ActivateEvent != ActivateEvent) return false;
                if (s.RecieveEvent != RecieveEvent) return false;
                if (s.RemoveEvent != RemoveEvent) return false;
                if (s.BackColor != BackColor) return false;
                if (s.GridEnabled != GridEnabled) return false;

                return true;
            }
        };

        public int FPS
        {
            get { return 1000 / GLFrameTimer.Interval; }
            set { GLFrameTimer.Interval = Math.Max(1, 1000 / value); }
        }

        public TileControl(string path)
        {
            InitializeComponent();
            SolidityNumeric.FixMouseWheel();
            LayerNumeric.FixMouseWheel();
            SizeNumeric.FixMouseWheel();
            FramesCountNumeric.FixMouseWheel();
            FrameDelayNumeric.FixMouseWheel();
            OffsetXNumeric.FixMouseWheel();
            OffsetYNumeric.FixMouseWheel();

            TileComboBox.SelectedIndex = 0;

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Link texture", null, LinkTextureMenuItem_Click, Keys.Control | Keys.I),
                new ToolStripMenuItem("Link event", null, LinkEventMenuItem_Click, Keys.Control | Keys.Shift | Keys.I),
                new ToolStripMenuItem("Toggle grid", null, ToggleGridMenuItem_Click, Keys.Control | Keys.G),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };
            
            GLSurface.MakeCurrent();
            Open(path);
            Story = new StoryItem<State>(new State(LoadedResource));
            Story.ValueChanged += Story_ValueChanged;
            TextureLinkTextBox.Subresource = LoadedResource.Texture;

            SolidityNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.Solidity, v => LoadedResource.Solidity = v);
            LayerNumeric.ValueChanged += (object sender, EventArgs e) =>
            {
                SyncNumericValue(sender, LoadedResource.Layer, v => LoadedResource.Layer = v);
                GLSurface.ResortTiles();
            };
            SizeNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.PartSize, v => LoadedResource.PartSize = v);
            FramesCountNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.FramesCount, v => LoadedResource.FramesCount = v);
            FrameDelayNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.FrameDelay, v => LoadedResource.FrameDelay = v);
            OffsetXNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.OffsetX, v => LoadedResource.OffsetX = v);
            OffsetYNumeric.ValueChanged += (object sender, EventArgs e) => SyncNumericValue(sender, LoadedResource.OffsetY, v => LoadedResource.OffsetY = v);

            TReactionBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxReaction(sender, LoadedResource.Reactions, v => LoadedResource.Reactions = v, TileResource.Reaction.T);
            TLReactionBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxReaction(sender, LoadedResource.Reactions, v => LoadedResource.Reactions = v, TileResource.Reaction.TL);
            LReactionBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxReaction(sender, LoadedResource.Reactions, v => LoadedResource.Reactions = v, TileResource.Reaction.L);
            LDReactionBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxReaction(sender, LoadedResource.Reactions, v => LoadedResource.Reactions = v, TileResource.Reaction.LD);
            DReactionBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxReaction(sender, LoadedResource.Reactions, v => LoadedResource.Reactions = v, TileResource.Reaction.D);
            DRReactionBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxReaction(sender, LoadedResource.Reactions, v => LoadedResource.Reactions = v, TileResource.Reaction.DR);
            RReactionBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxReaction(sender, LoadedResource.Reactions, v => LoadedResource.Reactions = v, TileResource.Reaction.R);
            RUReactionBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxReaction(sender, LoadedResource.Reactions, v => LoadedResource.Reactions = v, TileResource.Reaction.RT);
            MReactionBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxReaction(sender, LoadedResource.Reactions, v => LoadedResource.Reactions = v, TileResource.Reaction.M);

            TAnchorBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxAnchor(sender, LoadedResource.Anchors, v =>LoadedResource.Anchors = v, TileResource.Anchor.T);
            LAnchorBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxAnchor(sender, LoadedResource.Anchors, v =>LoadedResource.Anchors = v, TileResource.Anchor.L);
            DAnchorBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxAnchor(sender, LoadedResource.Anchors, v =>LoadedResource.Anchors = v, TileResource.Anchor.D);
            RAnchorBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxAnchor(sender, LoadedResource.Anchors, v =>LoadedResource.Anchors = v, TileResource.Anchor.R);
            FAnchorBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxAnchor(sender, LoadedResource.Anchors, v =>LoadedResource.Anchors = v, TileResource.Anchor.F);
            BAnchorBox.CheckedChanged += (object sender, EventArgs e) => SyncCheckBoxAnchor(sender, LoadedResource.Anchors, v =>LoadedResource.Anchors = v, TileResource.Anchor.B);

            PropertiesListBox.ItemCheck += (object sender, ItemCheckEventArgs e) => SyncFlags(sender, e, LoadedResource.Properties, v => LoadedResource.Properties = v);

            ShapeComboBox.Items.AddRange(Enum.GetNames(typeof(TileResource.Shape)));
            EventsComboBox.SelectedIndex = 0;

            ShapeComboBox.SelectedIndex = (int)LoadedResource.Form;

            for (int i = 0; i < PropertiesListBox.Items.Count; i++)
                PropertiesListBox.SetItemChecked(i, LoadedResource[i]);
            PropertiesListBox.SelectedIndexChanged += (object sender, EventArgs e) => PropertiesListBox.SelectedIndex = -1;

            SolidityNumeric.Value = (decimal)LoadedResource.Solidity;
            LayerNumeric.Value = (decimal)LoadedResource.Layer;
            SizeNumeric.Value = (decimal)LoadedResource.PartSize;
            FramesCountNumeric.Value = (decimal)LoadedResource.FramesCount;
            FrameDelayNumeric.Value = (decimal)LoadedResource.FrameDelay;
            OffsetXNumeric.Value = (decimal)LoadedResource.OffsetX;
            OffsetYNumeric.Value = (decimal)LoadedResource.OffsetY;

            TReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.T);
            TLReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.TL);
            LReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.L);
            LDReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.LD);
            DReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.D);
            DRReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.DR);
            RReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.R);
            RUReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.RT);
            MReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.M);

            TAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.T);
            LAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.L);
            DAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.D);
            RAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.R);
            FAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.F);
            BAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.B);
            GLFrameTimer.Start();

            int ox = (int)(GLSurface.FieldW / 2);
            int oy = (int)(GLSurface.FieldH / 2);
            GLSurface.LoadTile(LoadedResource, TileName);
            GLSurface.PlaceTile(TileName, ox + 0, oy + 0);
            GLSurface.PlaceTile(TileName, ox - 1, oy + 0);
            GLSurface.PlaceTile(TileName, ox + 1, oy + 0);
            GLSurface.PlaceTile(TileName, ox + 0, oy - 1);
            GLSurface.PlaceTile(TileName, ox + 0, oy + 1);
            GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);
            GLSurface.GridEnabled = LoadedResource.GridEnabled;
            GetTab("Toggle grid").Checked = LoadedResource.GridEnabled;
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
        private void SyncFlags(object sender, ItemCheckEventArgs e, TileResource.Property value, Action<TileResource.Property> set_value)
        {
            var box = sender as CheckedListBox;
            if (box != null)
            {
                bool ch = e.NewValue == CheckState.Checked;
                if (ch != (((int)value & (1 << e.Index)) != 0))
                {
                    if (ch) set_value(value | (TileResource.Property)(1 << e.Index));
                    else set_value(value & (TileResource.Property)~(1 << e.Index));
                    BackupChanges();
                    MakeUnsaved();
                }
            }
        }
        private void SyncCheckBoxReaction(object sender, TileResource.Reaction value, Action<TileResource.Reaction> set_value, TileResource.Reaction i)
        {
            var box = sender as CheckBox;
            if (box != null && value.HasFlag(i) != box.Checked)
            {
                if (box.Checked) set_value(value | i);
                else set_value(value & ~i);
                BackupChanges();
                MakeUnsaved();
            }
        }
        private void SyncCheckBoxAnchor(object sender, TileResource.Anchor value, Action<TileResource.Anchor> set_value, TileResource.Anchor i)
        {
            var box = sender as CheckBox;
            if (box != null && value.HasFlag(i) != box.Checked)
            {
                if (box.Checked) set_value(value | i);
                else set_value(value & ~i);
                BackupChanges();
                MakeUnsaved();
            }
        }
        private void RepairOffset()
        {
            float sw = GLSurface.SurfaceSize.Width / 2.0f;
            float sh = GLSurface.SurfaceSize.Height / 2.0f;
            
            float lx = -(float)GLSurface.FieldW / 2f;
            float rx = (float)GLSurface.FieldW / 2f;
            float dy = -(float)GLSurface.FieldH / 2f;
            float uy = (float)GLSurface.FieldH / 2f;
            
            if (OffsetX + rx < -sw) OffsetX = -sw - rx;
            if (OffsetX + lx > sw) OffsetX = sw - lx;
            if (OffsetY + uy < -sh) OffsetY = -sh - uy;
            if (OffsetY + dy > sh) OffsetY = sh - dy;
        }
        private void BackupChanges()
        {
            Story.Item = new State(LoadedResource);
        }

        private void Story_ValueChanged(object sender, EventArgs e)
        {
            Story.Item.ToResource(LoadedResource);

            ShapeComboBox.SelectedIndex = (int)LoadedResource.Form;

            for (int i = 0; i < PropertiesListBox.Items.Count; i++)
                PropertiesListBox.SetItemChecked(i, LoadedResource[i]);
            
            var ss = LightTextBox.SelectionStart;
            var hex = LoadedResource.Light.ToString("X");
            var str = new string('0', Math.Max(0, 8 - hex.Length)) + hex;
            bool c = LightTextBox.Text != str;
            LightTextBox.Text = str;
            LightTextBox.SelectionStart = ss + (c ? 1 : 0);

            SolidityNumeric.Value = (decimal)LoadedResource.Solidity;
            LayerNumeric.Value = (decimal)LoadedResource.Layer;
            SizeNumeric.Value = (decimal)LoadedResource.PartSize;
            FramesCountNumeric.Value = (decimal)LoadedResource.FramesCount;
            FrameDelayNumeric.Value = (decimal)LoadedResource.FrameDelay;
            OffsetXNumeric.Value = (decimal)LoadedResource.OffsetX;
            OffsetYNumeric.Value = (decimal)LoadedResource.OffsetY;

            TReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.T);
            TLReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.TL);
            LReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.L);
            LDReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.LD);
            DReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.D);
            DRReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.DR);
            RReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.R);
            RUReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.RT);
            MReactionBox.Checked = LoadedResource.Reactions.HasFlag(TileResource.Reaction.M);

            TAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.T);
            LAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.L);
            DAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.D);
            RAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.R);
            FAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.F);
            BAnchorBox.Checked = LoadedResource.Anchors.HasFlag(TileResource.Anchor.B);

            GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);
            GLSurface.GridEnabled = LoadedResource.GridEnabled;

            MakeUnsaved();
            UpdateRedactor();
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
                long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                float ox = GLSurface.ClientSize.Width % 2 == 0 ? 0 : 0.5f / GLSurface.Zoom;
                float oy = GLSurface.ClientSize.Height % 2 == 0 ? 0 : 0.5f / GLSurface.Zoom;

                GLSurface.Render(-ox - OffsetX, -oy - OffsetY);

                float tx = (int)Math.Round(-OffsetX + MouseManager.CurrentLocation.X - GLSurface.FieldW / 2 + 0.5) + OffsetX + (float)GLSurface.FieldW / 2 - 0.5f;
                float ty = (int)Math.Round(-OffsetY + MouseManager.CurrentLocation.Y - GLSurface.FieldH / 2 + 0.5) + OffsetY + (float)GLSurface.FieldH / 2 - 0.5f;
                gl.Color4ub(255, 0, 0, 255);
                gl.Begin(GL.LINE_LOOP);
                gl.Vertex2f(tx - 0.5f, ty - 0.5f);
                gl.Vertex2f(tx + 0.5f, ty - 0.5f);
                gl.Vertex2f(tx + 0.5f, ty + 0.5f);
                gl.Vertex2f(tx - 0.5f, ty + 0.5f);
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
                var z = 1f + e.Delta / 10f;
                GLSurface.Zoom *= z;
                OffsetX -= e.X * (z - 1f);
                OffsetY -= e.Y * (z - 1f);
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
                MouseManager.BeginDrag(e.Location);

                if (e.Button.HasFlag(MouseButtons.Left))
                {
                    int tx = (int)Math.Round(-OffsetX + e.X + GLSurface.FieldW / 2 - 0.5);
                    int ty = (int)Math.Round(-OffsetY + e.Y + GLSurface.FieldH / 2 - 0.5);

                    if (GLSurface.GetTile(tx, ty) == null)
                    {
                        GLSurface.PlaceTile(TileComboBox.SelectedIndex <= 0 ? TileName : (string)TileComboBox.SelectedItem, tx, ty);
                        MouseState = MouseStatus.Placing;
                    }
                    else
                    {
                        GLSurface.PlaceTile(null, tx, ty);
                        MouseState = MouseStatus.Removing;
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
                    int tx = (int)Math.Round(-OffsetX + e.X + GLSurface.FieldW / 2 - 0.5);
                    int ty = (int)Math.Round(-OffsetY + e.Y + GLSurface.FieldH / 2 - 0.5);
                    if (MouseState == MouseStatus.Placing) GLSurface.PlaceTile(TileComboBox.SelectedIndex <= 0 ? 
                        TileName : (string)TileComboBox.SelectedItem, tx, ty);
                    else GLSurface.PlaceTile(null, tx, ty); 
                }
                if (e.Button.HasFlag(MouseButtons.Right))
                {
                    OffsetX += MouseManager.CurrentDelta.X;
                    OffsetY += MouseManager.CurrentDelta.Y;
                    RepairOffset();
                }
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
            try
            {
                GLSurface.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle Tick event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GLSurface_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths.Length != 1 || Resource.GetType(paths[0]) != ResourceType.Tile) return;
            if (e.AllowedEffect.HasFlag(DragDropEffects.Link))
                e.Effect = DragDropEffects.Link;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }
        private void GLSurface_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                var pos = GLSurface.AdjustMouse(GLSurface.PointToClient(new Point(e.X, e.Y)));
                MouseManager.UpdateLocation(pos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not drag tile.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
                var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (paths.Length != 1 || Resource.GetType(paths[0]) != ResourceType.Tile) return;
                var path = ExtraPath.MakeDirectoryRelated(Directory.GetCurrentDirectory(), paths[0]);

                bool insert = !GLSurface.TileLoaded(path); 

                if (GLSurface.LoadTile(path, path))
                {
                    if (insert) TileComboBox.Items.Add(path);
                    var pos = GLSurface.AdjustMouse(GLSurface.PointToClient(new Point(e.X, e.Y)));
                    MouseManager.UpdateLocation(pos);

                    int tx = (int)Math.Round(-OffsetX + pos.X + GLSurface.FieldW / 2 - 0.5);
                    int ty = (int)Math.Round(-OffsetY + pos.Y + GLSurface.FieldH / 2 - 0.5);

                    if (GLSurface.GetTile(tx, ty) == null) GLSurface.PlaceTile(path, tx, ty);
                    else GLSurface.PlaceTile(null, tx, ty);

                    TileComboBox.SelectedItem = path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not place tile.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LightTextBox_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (LightTextBox.SelectionStart > 7) LightTextBox.SelectionStart = 0;
                LightTextBox.SelectionLength = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not fix light selecton.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LightTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (LightTextBox.Text != LightTextBox.Text.ToUpper())
                {
                    int ss = LightTextBox.SelectionStart;
                    LightTextBox.Text = LightTextBox.Text.ToUpper();
                    LightTextBox.SelectionStart = ss;
                }

                uint k, old = LoadedResource.Light;
                if (uint.TryParse(LightTextBox.Text.Substring(0, Math.Min(8, LightTextBox.Text.Length)),
                    NumberStyles.HexNumber, CultureInfo.CurrentCulture, out k)) LoadedResource.Light = k;
                else
                {
                    int ss = LightTextBox.SelectionStart;
                    var hex = LoadedResource.Light.ToString("X");
                    LightTextBox.Text = new string('0', Math.Max(0, 8 - hex.Length)) + hex;
                    LightTextBox.SelectionStart = ss;
                }
                if (old != LoadedResource.Light) Story.Item = new State(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change light value.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShapeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (LoadedResource.Form != (TileResource.Shape)ShapeComboBox.SelectedIndex)
                {
                    LoadedResource.Form = (TileResource.Shape)ShapeComboBox.SelectedIndex;
                    BackupChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change shape.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void EventsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (EventsComboBox.SelectedIndex)
            {
                case 0: EventLinkTextBox.Subresource = LoadedResource.SetupEvent; break;
                case 1: EventLinkTextBox.Subresource = LoadedResource.ReformEvent; break;
                case 2: EventLinkTextBox.Subresource = LoadedResource.TouchEvent; break;
                case 3: EventLinkTextBox.Subresource = LoadedResource.ActivateEvent; break;
                case 4: EventLinkTextBox.Subresource = LoadedResource.RecieveEvent; break;
                case 5: EventLinkTextBox.Subresource = LoadedResource.RemoveEvent; break;
            }
        }
        private void EventLinkTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Story.Item = new State(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change event link.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TextureLinkTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Story.Item = new State(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change texture link.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetPositionMenuItem_Click(object sender, EventArgs e)
        {
            OffsetX = 0f;
            OffsetY = 0f;
            GLSurface.Zoom = 16f;
        }
        private void BackColorMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BackgroundColorDialog.Color = Color.FromArgb(LoadedResource.BackColor);
                if (BackgroundColorDialog.ShowDialog(this) != DialogResult.OK) return;
                if (LoadedResource.BackColor == BackgroundColorDialog.Color.ToArgb()) return;

                LoadedResource.BackColor = BackgroundColorDialog.Color.ToArgb();
                BackupChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change background color.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ToggleGridMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = sender as ToolStripMenuItem;
                if (item != null)
                {
                    if (GLSurface.GridEnabled != (item.Checked = !item.Checked))
                    {
                        LoadedResource.GridEnabled = GLSurface.GridEnabled = item.Checked;
                        BackupChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle grid.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LinkEventMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void LinkTextureMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
