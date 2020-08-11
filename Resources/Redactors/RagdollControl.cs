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
    public partial class RagdollControl : ResourceControl<RagdollResource, StoryList<RagdollControl.Node>>, IResourceControl
    {
        private DragManager MouseManager = new DragManager(0.015625f);
        private int CapturedPoint = -1, SelectedPoint = -1;
        private float OffsetX = 0.0f, OffsetY = 0.0f;

        public struct Node
        {
            public float OffsetX;
            public float OffsetY;
            public int MainNode;

            public string[] Links;

            public Node(Node n)
            {
                OffsetX = n.OffsetX;
                OffsetY = n.OffsetY;
                MainNode = n.MainNode;
                Links = new string[n.Links.Length];

                for (int i = 0; i < n.Links.Length; i++)
                    Links[i] = n.Links[i];
            }
            public Node(RagdollResource.Node n)
            {
                OffsetX = n.OffsetX;
                OffsetY = n.OffsetY;
                MainNode = n.MainNode;
                Links = new string[n.Sprites.Count];

                for (int i = 0; i < n.Sprites.Count; i++)
                    Links[i] = n.Sprites[i].Link;
            }
            public void ToNode(RagdollResource.Node n)
            {
                n.OffsetX = OffsetX;
                n.OffsetY = OffsetY;
                n.MainNode = MainNode;
                while (n.Sprites.Count > Links.Count())
                    n.Sprites.RemoveAt(n.Sprites.Count - 1);
                while (n.Sprites.Count < Links.Count()) n.Sprites.Add(new Subresource<SpriteResource>());
                for (int i = 0; i < n.Sprites.Count; i++)
                    if (n.Sprites[i].Link != Links[i])
                        n.Sprites[i].Link = Links[i];
            }
            public RagdollResource.Node ToNode()
            {
                var n = new RagdollResource.Node();
                n.OffsetX = OffsetX;
                n.OffsetY = OffsetY;
                n.MainNode = MainNode;
                while (n.Sprites.Count < Links.Count()) n.Sprites.Add(new Subresource<SpriteResource>());
                for (int i = 0; i < n.Sprites.Count; i++)
                    if (n.Sprites[i].Link != Links[i])
                        n.Sprites[i].Link = Links[i];
                return n;
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Node)) return false;
                Node n = (Node)obj;

                if (OffsetX != n.OffsetX) return false;
                if (OffsetY != n.OffsetY) return false;
                if (MainNode != n.MainNode) return false;
                if (Links.Length != n.Links.Length) return false;
                for (int i = 0; i < Links.Length; i++)
                    if (Links[i] != n.Links[i]) return false;

                return true;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        };

        private RagdollResource.Node CurrentNode
        {
            get
            {
                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Nodes.Count) return null;
                else return LoadedResource.Nodes[index];
            }
        }
        private Node CurrentSNode
        {
            get
            {
                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Nodes.Count) return new Node();
                else return new Node(Story[index]);
            }
            set
            {
                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Nodes.Count) return;
                else Story[index] = value;
            }
        }
        private Subresource<SpriteResource> CurrentSprite
        {
            get
            {
                var node_index = NodesListBox.SelectedIndex;
                var link_index = LinksListBox.SelectedIndex;
                if (node_index < 0 || node_index >= LoadedResource.Nodes.Count) return null;
                if (LoadedResource.Nodes[node_index] == null) return null;
                if (link_index < 0 || link_index >= LoadedResource.
                    Nodes[node_index].Sprites.Count) return null;
                return LoadedResource.Nodes[node_index].Sprites[link_index];
            }
        }

        public int FPS
        {
            get { return 1000 / GLFrameTimer.Interval; }
            set { GLFrameTimer.Interval = Math.Max(1, 1000 / value); }
        }

        public RagdollControl(string path)
        {
            InitializeComponent();
            GLSurface.MakeCurrent();

            HitboxWNumeric.FixMouseWheel();
            HitboxHNumeric.FixMouseWheel();
            OffsetXNumeric.FixMouseWheel();
            OffsetYNumeric.FixMouseWheel();
            MainNodeNumeric.FixMouseWheel();

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Create node", null, CreateNodeMenuItem_Click, Keys.Control | Keys.A),
                new ToolStripMenuItem("Remove node", null, RemoveNodeMenuItem_Click, Keys.Control | Keys.D),
                new ToolStripMenuItem("Move node up", null, MoveNodeUpMenuItem_Click, Keys.Control | Keys.Up),
                new ToolStripMenuItem("Move node down", null, MoveNodeDownMenuItem_Click, Keys.Control | Keys.Down),
                new ToolStripMenuItem("Link sprite", null, LinkSpriteMenuItem_Click, Keys.Control | Keys.I),
                new ToolStripMenuItem("Unlink sprite", null, UnlinkSpriteMenuItem_Click, Keys.Control | Keys.U),
                new ToolStripMenuItem("Move link up", null, MoveLinkUpMenuItem_Click, Keys.Control | Keys.Shift | Keys.Up),
                new ToolStripMenuItem("Move link down", null, MoveLinkDownMenuItem_Click, Keys.Control | Keys.Shift | Keys.Down),
                new ToolStripMenuItem("Toggle transparency", null, ToggleTransparancyMenuItem_Click, Keys.Control | Keys.H),
                new ToolStripMenuItem("Pixel perfect", null, PixelPerfectMenuItem_Click, Keys.Control | Keys.P),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };
            for (int i = 1; i <= 8; i++) MenuTabs[i].Enabled = false;


            Open(path);
            var ln = new List<Node>(LoadedResource.Nodes.Count);
            for (int i = 0; i < LoadedResource.Nodes.Count; i++)
                ln.Add(new Node(LoadedResource.Nodes[i]));
            Story = new StoryList<Node>(ln);
            Story.ListChanged += Story_ListChanged;

            MenuTabs[9].Checked = LoadedResource.Transparency;
            MenuTabs[10].Checked = LoadedResource.PixelPerfect;

            HitboxWNumeric.Value = (decimal)LoadedResource.HitboxW;
            HitboxHNumeric.Value = (decimal)LoadedResource.HitboxH;

            NodesListBox.BeginUpdate();
            foreach (var node in LoadedResource.Nodes)
                NodesListBox.Items.Add("Node: " + NodesListBox.Items.Count);
            NodesListBox.EndUpdate();

            GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);

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

            //float sw = GLSurface.ClientSize.Width / GLSurface.Zoom / 2.0f;
            //float sh = GLSurface.ClientSize.Height / GLSurface.Zoom / 2.0f;
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

        private void Sprite_Reloaded(object sender, EventArgs e)
        {
            var node = CurrentNode;
            if (node != null)
            {
                LinksListBox.BeginUpdate();
                while (LinksListBox.Items.Count < node.Sprites.Count) LinksListBox.Items.Add("");
                while (LinksListBox.Items.Count > node.Sprites.Count) LinksListBox.Items.RemoveAt(LinksListBox.Items.Count - 1);
                for (int i = 0; i < node.Sprites.Count; i++)
                {
                    LinksListBox.Items[i] = node.Sprites[i].Link;
                }
                LinksListBox.EndUpdate();
            }
        }
        private void Story_ListChanged(object sender, StoryEventArgs e)
        {
            try
            {
                var index = e.Index;
                var action = e.Action;

                GLSurface.MakeCurrent();
                switch (action)
                {
                    case StoryAction.Changed:
                        {
                            var main = LoadedResource[index].MainNode;
                            Story[index].ToNode(LoadedResource[index]);
                            if (main != LoadedResource[index].MainNode)
                                LoadedResource.BuildUpdateQueue();
                        }
                        break;

                    case StoryAction.Created:
                        {
                            LoadedResource.Nodes.Insert(index, Story[index].ToNode());
                            LoadedResource.BuildUpdateQueue();

                            NodesListBox.BeginUpdate();
                            NodesListBox.Items.Add("Node: " + NodesListBox.Items.Count);
                            NodesListBox.SelectedIndex = index;
                            NodesListBox.EndUpdate();
                        }
                        break;

                    case StoryAction.Removed:
                        {
                            LoadedResource.Nodes.RemoveAt(index);
                            LoadedResource.BuildUpdateQueue();

                            NodesListBox.BeginUpdate();
                            NodesListBox.Items.RemoveAt(NodesListBox.Items.Count - 1);
                            if (index < -1) index = -1;
                            if (index >= NodesListBox.Items.Count)
                                index = NodesListBox.Items.Count - 1;
                            NodesListBox.SelectedIndex = index;
                            NodesListBox.EndUpdate();
                        }
                        break;
                }

                RepairOffset();

                var node = CurrentNode;
                if (node != null)
                {
                    OffsetXNumeric.Value = (decimal)node.OffsetX;
                    OffsetYNumeric.Value = (decimal)node.OffsetY;
                    MainNodeNumeric.Value = node.MainNode;

                    LinksListBox.BeginUpdate();
                    while (LinksListBox.Items.Count < node.Sprites.Count) LinksListBox.Items.Add("");
                    while (LinksListBox.Items.Count > node.Sprites.Count) LinksListBox.Items.RemoveAt(LinksListBox.Items.Count - 1);
                    for (int i = 0; i < node.Sprites.Count; i++)
                        LinksListBox.Items[i] = node.Sprites[i].Link;
                    LinksListBox.EndUpdate();
                }

                MakeUnsaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not update resource data.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void NodesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedPoint = NodesListBox.SelectedIndex;
                var node = CurrentNode;
                if (node != null)
                {
                    OffsetXNumeric.Value = (decimal)node.OffsetX;
                    OffsetYNumeric.Value = (decimal)node.OffsetY;
                    MainNodeNumeric.Value = node.MainNode;


                    LinksListBox.BeginUpdate();
                    LinksListBox.Items.Clear();
                    for (int i = 0; i < node.Sprites.Count; i++)
                    {
                        node.Sprites[i].Reloaded += Sprite_Reloaded;
                        LinksListBox.Items.Add(node.Sprites[i].Link);
                    }
                    LinksListBox.EndUpdate();

                    OffsetXNumeric.Enabled =
                    OffsetYNumeric.Enabled =
                    MainNodeNumeric.Enabled =
                    LinksListBox.Enabled = true;

                    for (int i = 1; i <= 4; i++) MenuTabs[i].Enabled = true;
                    if (LinksListBox.SelectedIndex < 0 ||
                        LinksListBox.SelectedIndex >= node.Count)
                    {
                        MenuTabs[5].Enabled = MenuTabs[6].Enabled =
                            MenuTabs[7].Enabled = MenuTabs[8].Enabled = false;
                    }
                    else
                    {
                        MenuTabs[5].Enabled = MenuTabs[6].Enabled =
                            MenuTabs[7].Enabled = MenuTabs[8].Enabled = true;
                    }
                }
                else
                {
                    OffsetXNumeric.Value = 0m;
                    OffsetYNumeric.Value = 0m;
                    MainNodeNumeric.Value = 0m;
                    LinksListBox.Items.Clear();

                    OffsetXNumeric.Enabled =
                    OffsetYNumeric.Enabled =
                    MainNodeNumeric.Enabled =
                    LinksListBox.Enabled = false;

                    for (int i = 1; i <= 8; i++) MenuTabs[i].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select node.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HitboxNumeric_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadedResource.HitboxW = (float)HitboxWNumeric.Value;
                LoadedResource.HitboxH = (float)HitboxHNumeric.Value;
                MakeUnsaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change resource properties.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Numeric_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int index = NodesListBox.SelectedIndex;
                if (index >= 0)
                {
                    var node = Story[index];

                    var main = node.MainNode;

                    node.OffsetX = (float)OffsetXNumeric.Value;
                    node.OffsetY = (float)OffsetYNumeric.Value;
                    node.MainNode = (int)MainNodeNumeric.Value; // <---

                    var dir = 0;
                    if (node.MainNode - main > 0) dir = 1;
                    if (node.MainNode - main < 0) dir = -1;

                    node.MainNode %= LoadedResource.Count + 1;
                    if (node.MainNode == LoadedResource.Count) node.MainNode = -1;
                    if (node.MainNode < -1) node.MainNode += LoadedResource.Count + 1;

                    if (main != node.MainNode)
                    {
                        while (!LoadedResource.ValidMainNode(index, node.MainNode) && dir != 0)
                        {
                            node.MainNode += dir;
                            node.MainNode %= LoadedResource.Count + 1;
                            if (node.MainNode == LoadedResource.Count) node.MainNode = -1;
                            if (node.MainNode < -1) node.MainNode += LoadedResource.Count + 1;
                        }
                        var o = LoadedResource.AdjustNodeOffset(index, node.MainNode);
                        node.OffsetX = o.X;
                        node.OffsetY = o.Y;

                        MainNodeNumeric.Value = node.MainNode;
                    }

                    Story[index] = node;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change node properties.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LinksListBox_DragEnter(object sender, DragEventArgs e)
        {
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
        private void LinksListBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var paths = e.Data.GetData(DataFormats.FileDrop) as string[];

                    GLSurface.MakeCurrent();
                    foreach (var fpath in paths)
                    {
                        var dpath = Directory.GetCurrentDirectory();
                        if (!fpath.StartsWith(dpath)) MessageBox.Show(this,
                            "Resource is not in description directory.",
                            "Error: Invalid resource.", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        else
                        {
                            var link = ExtraPath.MakeDirectoryRelated(dpath, fpath);
                            var node = CurrentNode;
                            if (node != null)
                            {
                                var sprite = new Subresource<SpriteResource>(link);
                                if (sprite.Resource != null)
                                {
                                    node.Sprites.Add(sprite);
                                    LinksListBox.Items.Add(link);
                                }
                            }
                        }
                    }

                    if (CurrentNode != null) CurrentSNode = new Node(CurrentNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link sprite.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LinksListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var node = CurrentNode;
                if (node == null || LinksListBox.SelectedIndex < 0 ||
                    LinksListBox.SelectedIndex >= node.Count)
                {
                    MenuTabs[5].Enabled = MenuTabs[6].Enabled =
                        MenuTabs[7].Enabled = MenuTabs[8].Enabled = false;
                }
                else
                {
                    MenuTabs[5].Enabled = MenuTabs[6].Enabled =
                        MenuTabs[7].Enabled = MenuTabs[8].Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not seect sprite.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NodesListBox_DragEnter(object sender, DragEventArgs e)
        {
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
        private void NodesListBox_DragOver(object sender, DragEventArgs e)
        {
            var index = NodesListBox.IndexFromPoint(NodesListBox.PointToClient(new Point(e.X, e.Y)));
            if (index >= 0) NodesListBox.SelectedIndex = index;
        }
        private void NodesListBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var paths = e.Data.GetData(DataFormats.FileDrop) as string[];

                    GLSurface.MakeCurrent();
                    foreach (var fpath in paths)
                    {
                        var dpath = Directory.GetCurrentDirectory();
                        if (!fpath.StartsWith(dpath)) MessageBox.Show(this,
                            "Resource is not in description directory.",
                            "Error: Invalid resource.", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        else
                        {
                            var link = ExtraPath.MakeDirectoryRelated(dpath, fpath);
                            var node = CurrentNode;
                            if (node != null)
                            {
                                var sprite = new Subresource<SpriteResource>(link);
                                if (sprite.Resource != null)
                                {
                                    node.Sprites.Add(sprite);
                                    LinksListBox.Items.Add(link);
                                }
                            }
                        }
                    }

                    if (CurrentNode != null) CurrentSNode = new Node(CurrentNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link sprite.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GLSurface_GLPaint(object sender, EventArgs e)
        {
            try
            {
                long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                gl.Disable(GL.TEXTURE_2D);
                gl.Begin(GL.LINES);
                gl.Color4ub(0, 0, 255, 255);
                gl.Vertex2f(OffsetX + (float)LoadedResource.HitboxW / 2f, OffsetY + (float)LoadedResource.HitboxH / 2f);
                gl.Vertex2f(OffsetX - (float)LoadedResource.HitboxW / 2f, OffsetY + (float)LoadedResource.HitboxH / 2f);
                gl.Vertex2f(OffsetX - (float)LoadedResource.HitboxW / 2f, OffsetY + (float)LoadedResource.HitboxH / 2f);
                gl.Vertex2f(OffsetX - (float)LoadedResource.HitboxW / 2f, OffsetY - (float)LoadedResource.HitboxH / 2f);
                gl.Vertex2f(OffsetX - (float)LoadedResource.HitboxW / 2f, OffsetY - (float)LoadedResource.HitboxH / 2f);
                gl.Vertex2f(OffsetX + (float)LoadedResource.HitboxW / 2f, OffsetY - (float)LoadedResource.HitboxH / 2f);
                gl.Vertex2f(OffsetX + (float)LoadedResource.HitboxW / 2f, OffsetY - (float)LoadedResource.HitboxH / 2f);
                gl.Vertex2f(OffsetX + (float)LoadedResource.HitboxW / 2f, OffsetY + (float)LoadedResource.HitboxH / 2f);
                gl.End();

                var frame = LoadedResource.MakeFrame();
                if (frame == null) return;
                int count = LoadedResource.Nodes.Count;

                gl.Color4ub(255, 255, 255, 255);
                if (!LoadedResource.Transparency) LoadedResource.Render(frame, OffsetX, OffsetY, time);
                else LoadedResource.Render(frame, OffsetX, OffsetY, time, NodesListBox.SelectedIndex);

                float b = LoadedResource.PointBoundsX / GLSurface.Zoom;
                float s = LoadedResource.PointBoundsY / GLSurface.Zoom;
                gl.Disable(GL.TEXTURE_2D);
                gl.Begin(GL.QUADS);
                for (int i = 0; i < count; i++)
                {
                    var f = frame[i];
                    var mn = LoadedResource[i].MainNode;


                    if (mn >= 0 && mn < count)
                    {
                        var mf = frame[mn];

                        float w = s;
                        float dx = f.OffsetX - mf.OffsetX;
                        float dy = f.OffsetY - mf.OffsetY;
                        float d = (float)Math.Sqrt(dx * dx + dy * dy);
                        float cs = dx / d;
                        float sn = dy / d;

                        if (SelectedPoint == i) gl.Color4ub(0, 0, 255, 255);
                        else gl.Color4ub(255, 255, 255, 128);
                        gl.Vertex2f(OffsetX + f.OffsetX - w * sn, OffsetY + f.OffsetY + w * cs);
                        if (SelectedPoint == i) gl.Color4ub(255, 255, 255, 128);
                        gl.Vertex2f(OffsetX + mf.OffsetX - w * sn, OffsetY + mf.OffsetY + w * cs);
                        gl.Vertex2f(OffsetX + mf.OffsetX + w * sn, OffsetY + mf.OffsetY - w * cs);
                        if (SelectedPoint == i) gl.Color4ub(0, 0, 255, 255);
                        gl.Vertex2f(OffsetX + f.OffsetX + w * sn, OffsetY + f.OffsetY - w * cs);
                    }
                }
                for (int i = 0; i < count; i++)
                {
                    var f = frame[i];

                    if (SelectedPoint == i) gl.Color4ub(0, 0, 255, 255);
                    else gl.Color4ub(0, 0, 0, 255);
                    gl.Vertex2f(OffsetX + f.OffsetX - b, OffsetY + f.OffsetY - b);
                    gl.Vertex2f(OffsetX + f.OffsetX + b, OffsetY + f.OffsetY - b);
                    gl.Vertex2f(OffsetX + f.OffsetX + b, OffsetY + f.OffsetY + b);
                    gl.Vertex2f(OffsetX + f.OffsetX - b, OffsetY + f.OffsetY + b);
                    if (CapturedPoint == i) gl.Color4ub(255, 0, 255, 255);
                    else gl.Color4ub(255, 0, 255, 128);
                    gl.Vertex2f(OffsetX + f.OffsetX - s, OffsetY + f.OffsetY - s);
                    gl.Vertex2f(OffsetX + f.OffsetX + s, OffsetY + f.OffsetY - s);
                    gl.Vertex2f(OffsetX + f.OffsetX + s, OffsetY + f.OffsetY + s);
                    gl.Vertex2f(OffsetX + f.OffsetX - s, OffsetY + f.OffsetY + s);
                }
                gl.End();

                if (CapturedPoint >= 0)
                {
                    gl.Begin(GL.LINES);
                    gl.Color4ub(255, 0, 0, 128);
                    gl.Vertex2f(OffsetX + frame[CapturedPoint].OffsetX, -GLSurface.SurfaceSize.Height / 2f);
                    gl.Vertex2f(OffsetX + frame[CapturedPoint].OffsetX, GLSurface.SurfaceSize.Height / 2f);
                    gl.Vertex2f(-GLSurface.SurfaceSize.Width / 2f, OffsetY + frame[CapturedPoint].OffsetY);
                    gl.Vertex2f(GLSurface.SurfaceSize.Width / 2f, OffsetY + frame[CapturedPoint].OffsetY);
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
                float b = LoadedResource.PointBoundsX / GLSurface.Zoom;

                if (CapturedPoint >= 0)
                {
                    //LoadedResource.Angle += e.Delta / 120.0f * (float)AngleNumeric.Increment;
                    //AngleNumeric.Value = (decimal)LoadedResource.Angle;

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
                            MakeUnsaved();
                        }
                    }
                    else
                    {
                        var z = 1f + e.Delta / 10f;
                        GLSurface.Zoom *= z;
                        OffsetX -= e.X * (z - 1f);
                        OffsetY -= e.Y * (z - 1f);

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
                var frame = LoadedResource.MakeFrame();
                if (frame == null) return;

                CapturedPoint = -1;
                for (int i = frame.Count - 1; i >= 0; i--)
                {
                    var f = frame[i];

                    if (MouseManager.CurrentLocation.X >= OffsetX + f.OffsetX - b &&
                        MouseManager.CurrentLocation.X <= OffsetX + f.OffsetX + b &&
                        MouseManager.CurrentLocation.Y >= OffsetY + f.OffsetY - b &&
                        MouseManager.CurrentLocation.Y <= OffsetY + f.OffsetY + b)
                    {
                        if (e.Button.HasFlag(MouseButtons.Left))
                        {
                            CapturedPoint = i;
                            NodesListBox.SelectedIndex = i;
                        }
                        if (e.Button.HasFlag(MouseButtons.Right))
                        {
                            if (SelectedPoint >= 0 && SelectedPoint < frame.Count)
                            {
                                if (SelectedPoint >= 0)
                                {
                                    if (LoadedResource.ValidMainNode(SelectedPoint, i))
                                    {
                                        var node = Story[SelectedPoint];
                                        var o = LoadedResource.AdjustNodeOffset(SelectedPoint, i);
                                        node.OffsetX = o.X;
                                        node.OffsetY = o.Y;
                                        node.MainNode = i;
                                        Story[SelectedPoint] = node;
                                    }
                                }
                            }
                        }

                        break;
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
                    if (CapturedPoint >= 0)
                    {
                        if (LoadedResource.PixelPerfect)
                        {
                            if (CurrentNode != null)
                            {
                                CurrentNode.OffsetX += MouseManager.CurrentStepDelta.X;
                                CurrentNode.OffsetY += MouseManager.CurrentStepDelta.Y;
                                OffsetXNumeric.Value = (decimal)CurrentNode.OffsetX;
                                OffsetYNumeric.Value = (decimal)CurrentNode.OffsetY;
                            }
                        }
                        else
                        {
                            if (CurrentNode != null)
                            {
                                CurrentNode.OffsetX += MouseManager.CurrentDelta.X;
                                CurrentNode.OffsetY += MouseManager.CurrentDelta.Y;
                                OffsetXNumeric.Value = (decimal)CurrentNode.OffsetX;
                                OffsetYNumeric.Value = (decimal)CurrentNode.OffsetY;
                            }
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

                if (CapturedPoint >= 0) Story[CapturedPoint] = new Node(CurrentNode);

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

        private void CreateNodeMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                var index = NodesListBox.SelectedIndex + 1;
                if (index < 0) index = 0;
                if (index > LoadedResource.Count)
                    index = LoadedResource.Count;

                for (int i = 0; i < LoadedResource.Count; i++)
                {
                    var n = Story[i];
                    if (n.MainNode >= index)
                    {
                        n.MainNode++;
                        Story[i] = n;
                        Story.AppendAction();
                    }
                }

                Story.Insert(index, new Node(new RagdollResource.Node(0f, 0f, -1)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not create node.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveNodeMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();

                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Count) return;

                for (int i = 0; i < LoadedResource.Count; i++)
                {
                    var n = Story[i];

                    if (n.MainNode > index)
                    {
                        n.MainNode--;
                        Story[i] = n;
                        Story.AppendAction();
                    }
                    else if (n.MainNode == index)
                    {
                        n.MainNode = -1;
                        var o = LoadedResource.AdjustNodeOffset(i, n.MainNode);
                        n.OffsetX = o.X;
                        n.OffsetY = o.Y;

                        Story[i] = n;
                        Story.AppendAction();
                    }
                }

                LoadedResource[index].Dispose();
                Story.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not remove node.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveNodeUpMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Count) return;
                var nindex = index - 1;

                if (nindex < 0)
                {
                    for (int i = 0; i < LoadedResource.Count; i++)
                    {
                        var n = Story[i];
                        if (n.MainNode == index)
                        {
                            n.MainNode = LoadedResource.Count - 1;
                            Story[i] = n;
                            Story.AppendAction();
                        }
                        else if (n.MainNode >= 0)
                        {
                            n.MainNode--;
                            Story[i] = n;
                            Story.AppendAction();
                        }
                    }
                    var node = Story[0];
                    for (int i = 0; i < LoadedResource.Count - 1; i++)
                    {
                        Story[i] = Story[i + 1];
                        Story.AppendAction();
                    }
                    Story[LoadedResource.Count - 1] = node;
                    NodesListBox.SelectedIndex = LoadedResource.Count - 1;
                }
                else
                {
                    for (int i = 0; i < LoadedResource.Count; i++)
                    {
                        var n = Story[i];
                        if (n.MainNode == index)
                        {
                            n.MainNode = nindex;
                            Story[i] = n;
                            Story.AppendAction();
                        }
                        else if (n.MainNode == nindex)
                        {
                            n.MainNode = index;
                            Story[i] = n;
                            Story.AppendAction();
                        }
                    }

                    Story.Swap(index, nindex);

                    NodesListBox.SelectedIndex = nindex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move node up.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveNodeDownMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Count) return;
                var nindex = index + 1;

                if (nindex >= LoadedResource.Count)
                {
                    for (int i = 0; i < LoadedResource.Count; i++)
                    {
                        var n = Story[i];
                        if (n.MainNode == index)
                        {
                            n.MainNode = 0;
                            Story[i] = n;
                            Story.AppendAction();
                        }
                        else if (n.MainNode >= 0)
                        {
                            n.MainNode++;
                            Story[i] = n;
                            Story.AppendAction();
                        }
                    }
                    var node = Story[index];
                    for (int i = index; i > 0; i--)
                    {
                        Story[i] = Story[i - 1];
                        Story.AppendAction();
                    }
                    Story[0] = node;
                    NodesListBox.SelectedIndex = 0;
                }
                else
                {
                    for (int i = 0; i < LoadedResource.Count; i++)
                    {
                        var n = Story[i];
                        if (n.MainNode == index)
                        {
                            n.MainNode = nindex;
                            Story[i] = n;
                            Story.AppendAction();
                        }
                        else if (n.MainNode == nindex)
                        {
                            n.MainNode = index;
                            Story[i] = n;
                            Story.AppendAction();
                        }
                    }

                    Story.Swap(index, nindex);

                    NodesListBox.SelectedIndex = nindex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move node down.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LinkSpriteMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var subform = new ExplorerForm(Directory.GetCurrentDirectory(), true, ResourceType.Sprite);
                if (subform.ShowDialog(this) == DialogResult.OK)
                {
                    var node = CurrentNode;
                    if (node != null)
                    {
                        GLSurface.MakeCurrent();
                        foreach (var link in subform.SelectedResources)
                        {
                            if (node != null)
                            {
                                var sprite = new Subresource<SpriteResource>(link);
                                if (sprite.Resource != null)
                                {
                                    node.Sprites.Add(sprite);
                                    LinksListBox.Items.Add(link);
                                }
                            }
                        }
                        CurrentSNode = new Node(CurrentNode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link sprite.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UnlinkSpriteMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Count) return;
                var lindex = LinksListBox.SelectedIndex;
                if (lindex < 0 || lindex >= LoadedResource[index].Count) return;
                var node = Story[index];
                var tl = node.Links;
                node.Links = new string[node.Links.Length - 1];

                var c = 0;
                for (int i = 0; i < tl.Length; i++)
                    if (i != lindex) node.Links[c] = tl[i];

                Story[index] = node;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not unlink sprite.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveLinkUpMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Count) return;
                var lindex = LinksListBox.SelectedIndex;
                if (lindex < 0 || lindex >= LoadedResource[index].Count) return;
                var nlindex = lindex - 1;

                var snode = CurrentSNode;
                LinksListBox.BeginUpdate();
                if (nlindex < 0)
                {
                    var ti = LinksListBox.Items[lindex];
                    var tl = snode.Links[lindex];
                    for (int i = 0; i < LinksListBox.Items.Count - 1; i++)
                    {
                        LinksListBox.Items[i] = LinksListBox.Items[i + 1];
                        snode.Links[i] = snode.Links[i + 1];
                    }
                    LinksListBox.Items[LinksListBox.Items.Count - 1] = ti;
                    snode.Links[LinksListBox.Items.Count - 1] = tl;
                    LinksListBox.SelectedIndex = LinksListBox.Items.Count - 1;
                }
                else
                {
                    var ti = LinksListBox.Items[lindex];
                    LinksListBox.Items[lindex] = LinksListBox.Items[nlindex];
                    LinksListBox.Items[nlindex] = ti;

                    var tl = snode.Links[lindex];
                    snode.Links[lindex] = snode.Links[nlindex];
                    snode.Links[nlindex] = tl;

                    LinksListBox.SelectedIndex = nlindex;
                }
                LinksListBox.EndUpdate();
                CurrentSNode = snode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move link up.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveLinkDownMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Count) return;
                var lindex = LinksListBox.SelectedIndex;
                if (lindex < 0 || lindex >= LoadedResource[index].Count) return;
                var nlindex = lindex + 1;

                var snode = CurrentSNode;
                LinksListBox.BeginUpdate();
                if (nlindex >= LoadedResource[index].Count)
                {
                    var ti = LinksListBox.Items[lindex];
                    var tl = snode.Links[lindex];
                    for (int i = lindex; i > 0; i--)
                    {
                        LinksListBox.Items[i] = LinksListBox.Items[i - 1];
                        snode.Links[i] = snode.Links[i - 1];
                    }
                    LinksListBox.Items[0] = ti;
                    snode.Links[0] = tl;
                    LinksListBox.SelectedIndex = 0;
                }
                else
                {
                    var ti = LinksListBox.Items[lindex];
                    LinksListBox.Items[lindex] = LinksListBox.Items[nlindex];
                    LinksListBox.Items[nlindex] = ti;

                    var tl = snode.Links[lindex];
                    snode.Links[lindex] = snode.Links[nlindex];
                    snode.Links[nlindex] = tl;

                    LinksListBox.SelectedIndex = nlindex;
                }
                LinksListBox.EndUpdate();
                CurrentSNode = snode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move link down.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ToggleTransparancyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = sender as ToolStripMenuItem;
                if (item != null)
                {
                    LoadedResource.Transparency = item.Checked = !item.Checked;
                    MakeUnsaved();
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
                    LoadedResource.PixelPerfect = item.Checked = !item.Checked;
                    MakeUnsaved();
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
