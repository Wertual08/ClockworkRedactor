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
using System.IO;
using ExtraForms;
using ExtraForms.OpenGL;

namespace Resource_Redactor.Descriptions.Redactors
{
    public partial class OutfitControl : UserControl, IResourceControl
    {
        public ToolStripMenuItem[] MenuTabs { get; private set; }

        public bool Saved { get; private set; } = true;
        public bool UndoEnabled { get { return Story.PrevState; } }
        public bool RedoEnabled { get { return Story.NextState; } }

        public string ResourcePath { get; private set; }
        public string ResourceName { get; private set; }

        public event StateChangedEventHandler StateChanged;

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

        public OutfitControl(string path)
        {
            InitializeComponent();

            RagdollNodeNumeric.FixMouseWheel();

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Link ragdoll", null, LinkRagdollMenuItem_Click, Keys.Control | Keys.I),
                new ToolStripMenuItem("Link sprite", null, LinkSpriteMenuItem_Click, Keys.Control | Keys.Shift | Keys.I),
                new ToolStripMenuItem("Create node", null, CreateNodeMenuItem_Click, Keys.Control | Keys.A),
                new ToolStripMenuItem("Remove node", null, RemoveNodeMenuItem_Click, Keys.Control | Keys.D),
                new ToolStripMenuItem("Move node up", null, MoveNodeUpMenuItem_Click, Keys.Control | Keys.Up),
                new ToolStripMenuItem("Move node down", null, MoveNodeDownMenuItem_Click, Keys.Control | Keys.Down),
                new ToolStripMenuItem("Toggle grid", null, ToggleGridMenuItem_Click, Keys.Control | Keys.G),
                new ToolStripMenuItem("Toggle transparency", null, ToggleTransparencyMenuItem_Click, Keys.Control | Keys.H),
                new ToolStripMenuItem("Pixel perfect", null, PixelPerfectMenuItem_Click, Keys.Control | Keys.P),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };
            for (int i = 3; i <= 5; i++) MenuTabs[i].Enabled = false;

            GLSurface.MakeCurrent();
            LoadedResource = new OutfitResource(path);
            RagdollLinkTextBox.Text = LoadedResource.Ragdoll.Link;
            Story = new StoryItem<StoryState>(new StoryState(LoadedResource));
            Story.ValueChanged += Story_ValueChanged;
            LoadedResource.Ragdoll.Reload();
            RagdollLinkTextBox.Subresource = LoadedResource.Ragdoll;
            LoadedResource.Ragdoll.Resource?.Clothe(LoadedResource);

            ResourcePath = path;
            ResourceName = Path.GetFileName(path);

            GetTab("Toggle grid").Checked = LoadedResource.GridEnabled;
            GetTab("Toggle transparency").Checked = LoadedResource.Transparency;
            GetTab("Pixel perfect").Checked = LoadedResource.PixelPerfect;

            NodesListBox.BeginUpdate();
            while (NodesListBox.Items.Count < LoadedResource.Count)
                NodesListBox.Items.Add("Node: " + NodesListBox.Items.Count);
            while (NodesListBox.Items.Count > LoadedResource.Count)
                NodesListBox.Items.RemoveAt(NodesListBox.Items.Count - 1);
            NodesListBox.EndUpdate();

            GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);

            ClotheTypeComboBox.Items.AddRange(Enum.GetNames(typeof(OutfitResource.Node.Clothe)));

            UpdateRedactor();

            GLFrameTimer.Start();
        }

        private void Story_ValueChanged(object sender, EventArgs e)
        {
            GLSurface.MakeCurrent();
            LoadedResource.Ragdoll.Resource?.Unclothe(LoadedResource);
            Story.Item.ToResource(LoadedResource);
            LoadedResource.Ragdoll.Resource?.Clothe(LoadedResource);

            while (NodesListBox.Items.Count < LoadedResource.Count)
                NodesListBox.Items.Add("Node: " + NodesListBox.Items.Count);
            while (NodesListBox.Items.Count > LoadedResource.Count)
                NodesListBox.Items.RemoveAt(NodesListBox.Items.Count - 1);

            UpdateInterface();
            MakeUnsaved();
        }
        private void UpdateInterface()
        {
            try
            {
                GetTab("Toggle grid").Checked = LoadedResource.GridEnabled;
                GetTab("Toggle transparency").Checked = LoadedResource.Transparency;
                GetTab("Pixel perfect").Checked = LoadedResource.PixelPerfect;

                int index = NodesListBox.SelectedIndex;
                if (index >= 0 && index < LoadedResource.Count)
                {
                    var node = LoadedResource[index];
                    RagdollNodeNumeric.Maximum = LoadedResource.Ragdoll.Resource?.Count - 1 ?? -1;

                    SpriteLinkTextBox.Subresource = node.Sprite;
                    SpriteLinkTextBox.Enabled = true;
                    RagdollNodeNumeric.Value = Math.Min(node.RagdollNode, RagdollNodeNumeric.Maximum);
                    RagdollNodeNumeric.Enabled = true;
                    ClotheTypeComboBox.SelectedIndex = ClotheTypeComboBox.Items.IndexOf(
                        Enum.GetName(typeof(OutfitResource.Node.Clothe), node.ClotheType));
                    ClotheTypeComboBox.Enabled = true;
                    for (int i = 3; i <= 5; i++) MenuTabs[i].Enabled = true;
                }
                else
                {
                    SpriteLinkTextBox.Subresource = null;
                    SpriteLinkTextBox.Enabled = false;
                    RagdollNodeNumeric.Value = -1;
                    RagdollNodeNumeric.Enabled = false;
                    ClotheTypeComboBox.SelectedIndex = -1;
                    ClotheTypeComboBox.Enabled = false;
                    for (int i = 3; i <= 5; i++) MenuTabs[i].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not update interface.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        private void RepairOffset()
        {

        }
        private ToolStripMenuItem GetTab(string title)
        {
            return MenuTabs.First((ToolStripMenuItem item) => { return item.Text == title; });
        }

        private DragManager MouseManager = new DragManager(0.015625f);
        private float OffsetX, OffsetY;
        private OutfitResource LoadedResource;
        private struct StoryState
        {
            private string[] Links;
            private int[] Nodes;
            private int[] Types;
            private int BackColor;
            private float PointBoundsX;
            private float PointBoundsY;
            private bool PixelPerfect;
            private bool GridEnabled;
            private bool Transparency;
            private string Ragdoll;

            public StoryState(OutfitResource r)
            {
                Links = new string[r.Count];
                Nodes = new int[r.Count];
                Types = new int[r.Count];
                for (int i = 0; i < r.Count; i++)
                {
                    Links[i] = r[i].Sprite.Link;
                    Nodes[i] = r[i].RagdollNode;
                    Types[i] = (int)r[i].ClotheType;
                }
                BackColor = r.BackColor;
                PointBoundsX = r.PointBoundsX;
                PointBoundsY = r.PointBoundsY;
                PixelPerfect = r.PixelPerfect;
                GridEnabled = r.GridEnabled;
                Transparency = r.Transparency;
                Ragdoll = r.Ragdoll.Link;
            }
            public void ToResource(OutfitResource r)
            {
                r.Count = Links.Length;
                for (int i = 0; i < r.Count; i++)
                {
                    r[i].Sprite.Link = Links[i];
                    r[i].RagdollNode = Nodes[i];
                    r[i].ClotheType = (OutfitResource.Node.Clothe)Types[i];
                }
                r.BackColor = BackColor;
                r.PointBoundsX = PointBoundsX;
                r.PointBoundsY = PointBoundsY;
                r.PixelPerfect = PixelPerfect;
                r.GridEnabled = GridEnabled;
                r.Transparency = Transparency;
                r.Ragdoll.Link = Ragdoll;
            }
            public override bool Equals(object obj)
            {
                if (!(obj is StoryState)) return false;
                var s = (StoryState)obj;
                if (s.Links.Length != Links.Length) return false;
                for (int i = 0; i < Links.Length; i++)
                {
                    if (s.Links[i] != Links[i]) return false;
                    if (s.Nodes[i] != Nodes[i]) return false;
                    if (s.Types[i] != Types[i]) return false;
                }
                if (s.BackColor != BackColor) return false;
                if (s.PointBoundsX != PointBoundsX) return false;
                if (s.PointBoundsY != PointBoundsY) return false;
                if (s.PixelPerfect != PixelPerfect) return false;
                if (s.GridEnabled != GridEnabled) return false;
                if (s.Transparency != Transparency) return false;
                if (s.Ragdoll != Ragdoll) return false;

                return true;
            }
        }
        private StoryItem<StoryState> Story;

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

                var frame = LoadedResource.Ragdoll.Resource?.MakeFrame() ?? null;
                LoadedResource.Ragdoll.Resource?.Render(frame, OffsetX, OffsetY, time);
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

        private void LinkRagdollMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                var subform = new ExplorerForm(Directory.GetCurrentDirectory(), false, ResourceType.Ragdoll);
                if (subform.ShowDialog(this) == DialogResult.OK && subform.SelectedResources.Count == 1)
                    RagdollLinkTextBox.Text = subform.SelectedResources[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LinkSpriteMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                var subform = new ExplorerForm(Directory.GetCurrentDirectory(), false, ResourceType.Ragdoll);
                if (subform.ShowDialog(this) == DialogResult.OK && subform.SelectedResources.Count == 1)
                    SpriteLinkTextBox.Text = subform.SelectedResources[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateNodeMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                var index = NodesListBox.SelectedIndex + 1;
                LoadedResource.Ragdoll.Resource?.Unclothe(LoadedResource);
                LoadedResource.Nodes.Insert(index, new OutfitResource.Node());
                LoadedResource.Ragdoll.Resource?.Clothe(LoadedResource);
                Story.Item = new StoryState(LoadedResource);
                NodesListBox.SelectedIndex = index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not create frame.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveNodeMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                var index = NodesListBox.SelectedIndex;
                if (index >= 0 && index < LoadedResource.Count)
                {
                    LoadedResource.Ragdoll.Resource?.Unclothe(LoadedResource);
                    LoadedResource.Nodes[index].Dispose(); 
                    LoadedResource.Nodes.RemoveAt(index);
                    LoadedResource.Ragdoll.Resource?.Clothe(LoadedResource);
                    Story.Item = new StoryState(LoadedResource);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not remove frame.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveNodeUpMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = NodesListBox.SelectedIndex;
                var nindex = index - 1;
                if (index >= 0 && index < LoadedResource.Count)
                {
                    LoadedResource.Ragdoll.Resource?.Unclothe(LoadedResource);
                    if (nindex < 0)
                    {
                        for (int i = 0; i < LoadedResource.Count - 1; i++) LoadedResource.Swap(i, i + 1);
                        nindex = LoadedResource.Count - 1;
                    }
                    else LoadedResource.Swap(index, nindex);
                    LoadedResource.Ragdoll.Resource?.Clothe(LoadedResource);
                    NodesListBox.SelectedIndex = nindex;
                    Story.Item = new StoryState(LoadedResource);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move frame up.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveNodeDownMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = NodesListBox.SelectedIndex;
                var nindex = index + 1;
                if (index >= 0 && index < LoadedResource.Count)
                {
                    LoadedResource.Ragdoll.Resource?.Unclothe(LoadedResource);
                    if (nindex >= LoadedResource.Count)
                    {
                        for (int i = LoadedResource.Count - 1; i > 0; i--) LoadedResource.Swap(i, i - 1);
                        nindex = 0;
                    }
                    else LoadedResource.Swap(index, nindex);
                    LoadedResource.Ragdoll.Resource?.Clothe(LoadedResource);
                    NodesListBox.SelectedIndex = nindex;
                    Story.Item = new StoryState(LoadedResource);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move frame down.",
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
                    if (LoadedResource.GridEnabled != (item.Checked = !item.Checked))
                    {
                        LoadedResource.GridEnabled = item.Checked;
                        MakeUnsaved();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle grid.",
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
                GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);
                MakeUnsaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change background color.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NodesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface();
        }
        private void RagdollLinkTextBox_TextChanged(object sender, EventArgs e)
        {
            Story.Item = new StoryState(LoadedResource);
        }
        private void SpriteLinkTextBox_TextChanged(object sender, EventArgs e)
        {
            Story.Item = new StoryState(LoadedResource);
        }
        private void RagdollNodeNumeric_ValueChanged(object sender, EventArgs e)
        {
            int index = NodesListBox.SelectedIndex;
            if (index >= 0 && index < LoadedResource.Count)
            {
                LoadedResource.Ragdoll.Resource?.Unclothe(LoadedResource);
                var node = LoadedResource[index];
                node.RagdollNode = (int)RagdollNodeNumeric.Value;
                LoadedResource.Ragdoll.Resource?.Clothe(LoadedResource);
                Story.Item = new StoryState(LoadedResource);
            }
        }
        private void ClotheTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = NodesListBox.SelectedIndex;
            if (index >= 0 && index < LoadedResource.Count)
            {
                LoadedResource.Ragdoll.Resource?.Unclothe(LoadedResource);
                var node = LoadedResource[index];
                node.ClotheType = (OutfitResource.Node.Clothe)Enum.Parse(
                    typeof(OutfitResource.Node.Clothe), ClotheTypeComboBox.SelectedItem as string);
                LoadedResource.Ragdoll.Resource?.Clothe(LoadedResource);
                Story.Item = new StoryState(LoadedResource);
            }
        }

        private void ResetPositionMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OffsetX = 0f;
                OffsetY = 0f;
                GLSurface.Zoom = 16f;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not reset position.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
