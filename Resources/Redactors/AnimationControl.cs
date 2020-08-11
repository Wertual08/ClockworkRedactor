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
using ExtraForms.OpenGL;
using System.Diagnostics;

namespace Resource_Redactor.Resources.Redactors
{
    public partial class AnimationControl : ResourceControl<AnimationResource, StoryList<Frame>>, IResourceControl
    {
        public int FPS 
        { 
            get { return 1000 / GLFrameTimer.Interval; } 
            set { GLFrameTimer.Interval = Math.Max(1, 1000 / value); } 
        }

        public AnimationControl(string path)
        {
            InitializeComponent();

            LocalFPURScaleNumeric.FixMouseWheel();
            TypeUpDown.FixMouseWheel();
            FPURNumeric.FixMouseWheel();
            AngleNumeric.FixMouseWheel();
            OffsetXNumeric.FixMouseWheel();
            OffsetYNumeric.FixMouseWheel();

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Toggle animation", null, ToggleAnimationMenuItem_Click, Keys.Control | Keys.T),
                new ToolStripMenuItem("Link ragdoll", null, LinkRagdollMenuItem_Click, Keys.Control | Keys.I),
                new ToolStripMenuItem("Create frame", null, CreateFrameMenuItem_Click, Keys.Control | Keys.A),
                new ToolStripMenuItem("Remove frame", null, RemoveFrameMenuItem_Click, Keys.Control | Keys.D),
                new ToolStripMenuItem("Move frame up", null, MoveFrameUpMenuItem_Click, Keys.Control | Keys.Up),
                new ToolStripMenuItem("Move frame down", null, MoveFrameDownMenuItem_Click, Keys.Control | Keys.Down),
                new ToolStripMenuItem("Create nodes", null, CreateNodesMenuItem_Click, Keys.Control | Keys.Shift | Keys.A),
                new ToolStripMenuItem("Remove nodes", null, RemoveNodesMenuItem_Click, Keys.Control | Keys.Shift  | Keys.D),
                new ToolStripMenuItem("Move nodes up", null, MoveNodesUpMenuItem_Click, Keys.Control | Keys.Shift | Keys.Up),
                new ToolStripMenuItem("Move nodes down", null, MoveNodesDownMenuItem_Click, Keys.Control | Keys.Shift | Keys.Down),
                new ToolStripMenuItem("Toggle grid", null, ToggleGridMenuItem_Click, Keys.Control | Keys.G),
                new ToolStripMenuItem("Toggle transparency", null, ToggleTransparencyMenuItem_Click, Keys.Control | Keys.H),
                new ToolStripMenuItem("Pixel perfect", null, PixelPerfectMenuItem_Click, Keys.Control | Keys.P),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
                new ToolStripMenuItem("Lock nodes", null, LockNodesMenuItem_Click, Keys.Control | Keys.F),
                new ToolStripMenuItem("Invert view", null, InvertViewMenuItem_Click, Keys.Control | Keys.Shift | Keys.V),
                new ToolStripMenuItem("Toggle visible", null, ToggleVisibleMenuItem_Click, Keys.Control | Keys.Alt | Keys.V),
            };
            for (int i = 3; i <= 9; i++) MenuTabs[i].Enabled = false;
            MenuTabs[6].Enabled = true;

            GLSurface.MakeCurrent();
            Open(path);
            LinkTextBox.Text = LoadedResource.Ragdoll.Link;
            Story = new StoryList<Frame>(LoadedResource.Frames);
            Story.ListChanged += Story_ListChanged;

            LinkTextBox.Subresource = LoadedResource.Ragdoll;
            LoadedResource.Ragdoll.Reload();

            GetTab("Toggle grid").Checked = LoadedResource.GridEnabled;
            GetTab("Toggle transparency").Checked = LoadedResource.Transparency;
            GetTab("Pixel perfect").Checked = LoadedResource.PixelPerfect;

            FramesListBox.BeginUpdate();
            while (FramesListBox.Items.Count < Story.Count) 
                FramesListBox.Items.Add("Frame: " + FramesListBox.Items.Count);
            while (FramesListBox.Items.Count > Story.Count)
                FramesListBox.Items.RemoveAt(FramesListBox.Items.Count - 1);
            FramesListBox.EndUpdate();

            GLSurface.BackColor = Color.FromArgb(LoadedResource.BackColor);

            FPURNumeric.Value = (decimal)LoadedResource.FramesPerUnitRatio;
            TypeUpDown.Items.AddRange(Enum.GetNames(typeof(AnimationType)));
            TypeUpDown.SelectedIndex = (int)LoadedResource.Dependency;

            if (FramesListBox.Items.Count > 0) FramesListBox.SelectedIndex = 0;

            UpdateState(); 
            
            if (CurrentRagdoll != null)
            {
                NodeVisible = new bool[CurrentRagdoll.Count];
                for (int i = 0; i < NodeVisible.Length; i++) NodeVisible[i] = true;
            }
            else NodeVisible = null;

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

        private DragManager MouseManager = new DragManager(0.015625f);
        private Stopwatch AnimationClock = new Stopwatch();
        private int CapturedNode = -1, SelectedNode = -1;
        private bool NodesLocked = false, ViewInverted = false;
        private bool[] NodeVisible;
        private float OffsetX = 0f, OffsetY = 0f, GridX = 0f, GridY = 0f;
        private Frame LockedFrame = null;
        private Frame CurrentFrame
        {
            get
            {
                var index = FramesListBox.SelectedIndex;
                if (index < 0 || index >= Story.Count) return null;
                else return LockedFrame == null ? new Frame(Story[index]) : Story[index];
            }
            set
            {
                var index = FramesListBox.SelectedIndex;
                if (index >= 0 && index < Story.Count)
                {
                    if (LockedFrame == null) Story[index] = value;
                    else Story[index].Copy(value);
                }
            }
        }
        private Frame.Node CurrentNode
        {
            get { return CurrentFrame?[NodesListBox.SelectedIndex]; }
            set
            {
                var frame = CurrentFrame;
                frame[NodesListBox.SelectedIndex]?.Copy(value);
                CurrentFrame = frame;
            }
        }
        private RagdollResource CurrentRagdoll
        {
            get
            {
                return LoadedResource.Ragdoll.Resource;
            }
        }
        private RagdollResource.Node CurrentRagdollNode
        {
            get { return CurrentRagdoll?[NodesListBox.SelectedIndex]; }
            set { if (CurrentRagdoll != null) CurrentRagdoll[NodesListBox.SelectedIndex] = value; }
        }

        private void LockStory()
        {
            if (LockedFrame != null) return;
            LockedFrame = CurrentFrame;
        }
        private void UnlockStory()
        {
            if (LockedFrame == null) return;
            var frame = new Frame(CurrentFrame);
            CurrentFrame.Copy(LockedFrame);
            var lf = LockedFrame;
            LockedFrame = null;
            CurrentFrame = frame;
            if (NodesLocked)
            {
                var delta_frame = new Frame(CurrentFrame);
                for (int i = 0; i < lf.Count; i++)
                {
                    delta_frame[i].Angle -= lf[i].Angle;
                    delta_frame[i].OffsetX -= lf[i].OffsetX;
                    delta_frame[i].OffsetY -= lf[i].OffsetY;
                }

                for (int i = 0; i < Story.Count; i++)
                {
                    if (i != FramesListBox.SelectedIndex)
                    {
                        Story.AppendAction();
                        var ext_frame = new Frame(Story[i]);
                        for (int j = 0; j < delta_frame.Count; j++)
                        {
                            ext_frame[j].Angle += delta_frame[j].Angle;
                            ext_frame[j].OffsetX += delta_frame[j].OffsetX;
                            ext_frame[j].OffsetY += delta_frame[j].OffsetY;
                        }
                        Story[i] = ext_frame;
                    }
                }
            }
        }
        private void UpdateState()
        {
            GetTab("Create frame").Enabled = !LoadedResource.Playing;
            GetTab("Create nodes").Enabled = !LoadedResource.Playing;
            GetTab("Remove frame").Enabled = CurrentFrame != null && !LoadedResource.Playing;
            GetTab("Move frame up").Enabled = CurrentFrame != null && !LoadedResource.Playing;
            GetTab("Move frame down").Enabled = CurrentFrame != null && !LoadedResource.Playing;
            GetTab("Remove nodes").Enabled = NodesListBox.SelectedIndex >= 0 && 
                NodesListBox.SelectedIndex < LoadedResource.NodesCount && !LoadedResource.Playing;
            GetTab("Move nodes up").Enabled = NodesListBox.SelectedIndex >= 0 &&
                NodesListBox.SelectedIndex < LoadedResource.NodesCount && !LoadedResource.Playing;
            GetTab("Move nodes down").Enabled = NodesListBox.SelectedIndex >= 0 &&
                NodesListBox.SelectedIndex < LoadedResource.NodesCount && !LoadedResource.Playing;

            var frame = CurrentFrame;
            if (frame != null)
            {
                for (int i = 0; i < frame.Count; i++)
                {
                    if (i < NodesListBox.Items.Count) NodesListBox.
                            SetItemChecked(i, frame[i][NodeProperties.Enabled]);
                    else NodesListBox.Items.Add("Node: " + NodesListBox.Items.Count,
                        frame[i][NodeProperties.Enabled]);
                }
                while (NodesListBox.Items.Count > frame.Count)
                    NodesListBox.Items.RemoveAt(NodesListBox.Items.Count - 1);
            }
            else
            {
                NodesListBox.Items.Clear();
            }

            var node = CurrentNode;
            if (node != null)
            {
                ALICheckBox.Enabled = OLICheckBox.Enabled = AngleNumeric.Enabled =
                    OffsetXNumeric.Enabled = OffsetYNumeric.Enabled = !LoadedResource.Playing;

                ALICheckBox.Checked = node[NodeProperties.ALI];
                OLICheckBox.Checked = node[NodeProperties.OLI];
                AngleNumeric.Value = (decimal)node.Angle;
                OffsetXNumeric.Value = (decimal)node.OffsetX;
                OffsetYNumeric.Value = (decimal)node.OffsetY;
            }
            else
            {
                ALICheckBox.Checked = OLICheckBox.Checked = false;
                AngleNumeric.Value = OffsetXNumeric.Value = OffsetYNumeric.Value = 0m;
                ALICheckBox.Enabled = OLICheckBox.Enabled = AngleNumeric.Enabled = 
                    OffsetXNumeric.Enabled = OffsetYNumeric.Enabled = false;
            }
        }
        private void RepairOffset()
        {

        }
        private void UpdateAnimation()
        {
            if (LoadedResource.Playing)
            {
                switch (LoadedResource.Dependency)
                {
                    case AnimationType.TimeLoop:
                    case AnimationType.TimeOnce:
                        {
                        }
                        break;

                    case AnimationType.MovementX:
                        {
                            GridX -= (float)AnimationClock.Elapsed.TotalSeconds *
                                (float)LocalFPURScaleNumeric.Value;
                            GridX -= (int)GridX;
                        }
                        break;

                    case AnimationType.MovementY:
                        {
                            GridY -= (float)AnimationClock.Elapsed.TotalSeconds *
                                (float)LocalFPURScaleNumeric.Value;
                            GridY -= (int)GridY;
                        }
                        break;

                    case AnimationType.VelocityX:
                        {
                            GridX -= (float)AnimationClock.Elapsed.TotalSeconds *
                                (float)LocalFPURScaleNumeric.Value;
                            GridX -= (int)GridX;
                        }
                        break;

                    case AnimationType.VelocityY:
                        {
                            GridY -= (float)AnimationClock.Elapsed.TotalSeconds *
                                (float)LocalFPURScaleNumeric.Value;
                            GridY -= (int)GridY;
                        }
                        break;

                    case AnimationType.AimCursor:
                        {
                        }
                        break;

                    case AnimationType.FollowCursor:
                        {
                        }
                        break;
                }

                LoadedResource.Update((float)AnimationClock.Elapsed.TotalSeconds, (float)LocalFPURScaleNumeric.Value);

                FramesListBox.SelectedIndex = LoadedResource.Index;
            }
            else LoadedResource.Index = LoadedResource.Index;
            AnimationClock.Restart();
            GetTab("Toggle animation").Checked = LoadedResource.Playing;
        }

        private void Story_ListChanged(object sender, StoryEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case StoryAction.Changed:
                        {
                            LoadedResource.NodesCount = Story[e.Index].Count;
                        }
                        break;
                    case StoryAction.Created:
                        {
                            FramesListBox.Items.Add("Frame: " + FramesListBox.Items.Count);
                        }
                        break;
                    case StoryAction.Removed:
                        {
                            FramesListBox.Items.RemoveAt(FramesListBox.Items.Count - 1);
                        }
                        break;
                }

                MakeUnsaved();
                UpdateState();
                UpdateRedactor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not update resource data.", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FramesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!LoadedResource.Playing)
                {
                    var prev = LoadedResource.Index;
                    LoadedResource.Index = FramesListBox.SelectedIndex;
                    if (LoadedResource.Index >= 0 && LoadedResource.Index < LoadedResource.Count) LoadedResource.Index = LoadedResource.Index;
                    int dist = LoadedResource.Index - prev;
                    if (Math.Abs(dist) > Story.Count / 2)
                    {
                        if (dist < 0) dist += Story.Count;
                        else if (dist > 0) dist -= Story.Count;
                    }

                    switch (LoadedResource.Dependency)
                    {
                        case AnimationType.MovementX:
                            GridX -= dist / LoadedResource.FramesPerUnitRatio;
                            GridX -= (int)GridX;
                            break;

                        case AnimationType.MovementY:
                            GridY -= dist / LoadedResource.FramesPerUnitRatio;
                            GridY -= (int)GridY;
                            break;
                    }
                }
                UpdateState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select frame.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void NodesListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (LoadedResource.Playing)
                {
                    e.NewValue = e.CurrentValue;
                    return;
                }
                var frame = CurrentFrame;
                if (frame != null)
                {
                    var node = frame[e.Index];
                    if ((e.NewValue == CheckState.Checked) != node[NodeProperties.Enabled])
                    {
                        node[NodeProperties.Enabled] = e.NewValue == CheckState.Checked;
                        CurrentFrame = frame;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not enable node.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void NodesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedNode = NodesListBox.SelectedIndex;
                if (NodeVisible != null)
                {
                    int index = NodesListBox.SelectedIndex;
                    if (index >= 0 && index < NodeVisible.Length)
                    {
                        GetTab("Toggle visible").Checked = NodeVisible[index];
                    }
                }

                UpdateState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select node.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LinkTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();

                LoadedResource.Ragdoll.Reload();
                if (CurrentRagdoll != null)
                {
                    if (LoadedResource.Count == 0) LoadedResource.NodesCount = CurrentRagdoll.Count;
                    else if (LoadedResource.NodesCount != CurrentRagdoll.Count)
                        MessageBox.Show(this, "If you want use that ragdoll, you must adjust nodes count.",
                        "Warning: Animation and ragdoll node counts does not match.",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                MakeUnsaved();

                UpdateState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AnimationProperty_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (LoadedResource.FramesPerUnitRatio != (float)FPURNumeric.Value)
                {
                    LoadedResource.FramesPerUnitRatio = (float)FPURNumeric.Value;
                    MakeUnsaved();
                }
                if (LoadedResource.Dependency != (AnimationType)TypeUpDown.SelectedIndex)
                {
                    LoadedResource.Dependency = (AnimationType)TypeUpDown.SelectedIndex;
                    MakeUnsaved();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change animation property.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Numeric_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                var node = CurrentNode;
                if (node != null)
                {
                    float last_angle = node.Angle;
                    float last_offsetx = node.OffsetX;
                    float last_offsety = node.OffsetY;
                    AngleNumeric.Value %= (decimal)Math.PI * 2m;
                    if (AngleNumeric.Value < 0m) AngleNumeric.Value += (decimal)Math.PI * 2m;

                    node[NodeProperties.ALI] = ALICheckBox.Checked;
                    node[NodeProperties.OLI] = OLICheckBox.Checked;
                    node.Angle = (float)AngleNumeric.Value;
                    node.OffsetX = (float)OffsetXNumeric.Value;
                    node.OffsetY = (float)OffsetYNumeric.Value;
                    CurrentNode = node;

                    if (NodesLocked)
                    {
                        float delta_a = node.Angle - last_angle;
                        float delta_x = node.OffsetX - last_offsetx;
                        float delta_y = node.OffsetY - last_offsety;
                        for (int i = 0; i < Story.Count; i++)
                        {
                            if (i != LoadedResource.Index)
                            {
                                Story.AppendAction();
                                var ext_frame = new Frame(Story[i]);
                                ext_frame[SelectedNode].Angle += delta_a;
                                ext_frame[SelectedNode].OffsetX += delta_x;
                                ext_frame[SelectedNode].OffsetY += delta_y;
                                Story[i] = ext_frame;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not change node property.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GLSurface_GLPaint(object sender, EventArgs e)
        {
            try
            {
                long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                UpdateAnimation();

                if (CurrentRagdoll != null)
                {
                    gl.Disable(GL.TEXTURE_2D);
                    gl.Begin(GL.LINES);
                    gl.Color4ub(0, 0, 255, 255);
                    gl.Vertex2f(OffsetX + (float)CurrentRagdoll.HitboxW / 2f, OffsetY + (float)CurrentRagdoll.HitboxH / 2f);
                    gl.Vertex2f(OffsetX - (float)CurrentRagdoll.HitboxW / 2f, OffsetY + (float)CurrentRagdoll.HitboxH / 2f);
                    gl.Vertex2f(OffsetX - (float)CurrentRagdoll.HitboxW / 2f, OffsetY + (float)CurrentRagdoll.HitboxH / 2f);
                    gl.Vertex2f(OffsetX - (float)CurrentRagdoll.HitboxW / 2f, OffsetY - (float)CurrentRagdoll.HitboxH / 2f);
                    gl.Vertex2f(OffsetX - (float)CurrentRagdoll.HitboxW / 2f, OffsetY - (float)CurrentRagdoll.HitboxH / 2f);
                    gl.Vertex2f(OffsetX + (float)CurrentRagdoll.HitboxW / 2f, OffsetY - (float)CurrentRagdoll.HitboxH / 2f);
                    gl.Vertex2f(OffsetX + (float)CurrentRagdoll.HitboxW / 2f, OffsetY - (float)CurrentRagdoll.HitboxH / 2f);
                    gl.Vertex2f(OffsetX + (float)CurrentRagdoll.HitboxW / 2f, OffsetY + (float)CurrentRagdoll.HitboxH / 2f);
                    gl.End();

                    var frame = CurrentRagdoll.MakeFrame(LoadedResource.CurrentFrame);
                    if (frame == null) return;
                    int count = CurrentRagdoll.Nodes.Count;

                    if (LoadedResource.GridEnabled)
                    {
                        gl.Begin(GL.LINES);
                        var color = GLSurface.BackColor;
                        gl.Color4ub((byte)((color.R + 128) % 256), (byte)((color.G + 128) % 256), (byte)((color.B + 128) % 256), 255);
                        for (float x = -(float)Math.Ceiling(GLSurface.SurfaceSize.Width / 2f) + GridX - (int)GridX + OffsetX - (int)OffsetX - 1f;
                            x < Math.Ceiling(GLSurface.SurfaceSize.Width / 2f) + GridX - (int)GridX + OffsetX - (int)OffsetX + 1f; x++)
                        {
                            gl.Vertex2f(x, -GLSurface.SurfaceSize.Height / 2f);
                            gl.Vertex2f(x, GLSurface.SurfaceSize.Height / 2f);
                        }
                        for (float y = -(float)Math.Ceiling(GLSurface.SurfaceSize.Height / 2f) + GridY - (int)GridY + OffsetY - (int)OffsetY - 1f;
                            y < Math.Ceiling(GLSurface.SurfaceSize.Height / 2f) + GridY - (int)GridY + OffsetY - (int)OffsetY + 1f; y++)
                        {
                            gl.Vertex2f(-GLSurface.SurfaceSize.Width / 2f, y);
                            gl.Vertex2f(GLSurface.SurfaceSize.Width / 2f, y);
                        }
                        gl.End();
                    }

                    gl.Color4ub(255, 255, 255, 255);
                    if (ViewInverted)
                    {
                        if (!LoadedResource.Transparency) CurrentRagdoll.RenderInverted(frame, OffsetX, OffsetY, time, 1, 1, 1, null, -1, 0, NodeVisible);
                        else CurrentRagdoll.RenderInverted(frame, OffsetX, OffsetY, time, NodesListBox.SelectedIndex, 1, 1, 1, null, -1, 0, NodeVisible);
                    }
                    else
                    {
                        if (!LoadedResource.Transparency) CurrentRagdoll.Render(frame, OffsetX, OffsetY, time, 1, 1, 1, null, -1, 0, NodeVisible);
                        else CurrentRagdoll.Render(frame, OffsetX, OffsetY, time, NodesListBox.SelectedIndex, 1, 1, 1, null, -1, 0, NodeVisible);
                    }

                    float b = LoadedResource.PointBoundsX / GLSurface.Zoom;
                    float s = LoadedResource.PointBoundsY / GLSurface.Zoom;
                    gl.Disable(GL.TEXTURE_2D);
                    gl.Begin(GL.QUADS);
                    for (int i = 0; i < count; i++)
                    {
                        var f = frame[i];
                        var mn = CurrentRagdoll[i].MainNode;


                        if (mn >= 0 && mn < count)
                        {
                            var mf = frame[mn];

                            float w = s;
                            float dx = f.OffsetX - mf.OffsetX;
                            float dy = f.OffsetY - mf.OffsetY;
                            float d = (float)Math.Sqrt(dx * dx + dy * dy);
                            float cs = dx / d;
                            float sn = dy / d;

                            if (SelectedNode == i) gl.Color4ub(0, 0, 255, 255);
                            else gl.Color4ub(255, 255, 255, 128);
                            gl.Vertex2f(OffsetX + f.OffsetX - w * sn, OffsetY + f.OffsetY + w * cs);
                            if (SelectedNode == i) gl.Color4ub(255, 255, 255, 128);
                            gl.Vertex2f(OffsetX + mf.OffsetX - w * sn, OffsetY + mf.OffsetY + w * cs);
                            gl.Vertex2f(OffsetX + mf.OffsetX + w * sn, OffsetY + mf.OffsetY - w * cs);
                            if (SelectedNode == i) gl.Color4ub(0, 0, 255, 255);
                            gl.Vertex2f(OffsetX + f.OffsetX + w * sn, OffsetY + f.OffsetY - w * cs);
                        }
                    }
                    for (int i = 0; i < count; i++)
                    {
                        var f = frame[i];

                        if (SelectedNode == i) gl.Color4ub(0, 0, 255, 255);
                        else gl.Color4ub(0, 0, 0, 255);
                        gl.Vertex2f(OffsetX + f.OffsetX - b, OffsetY + f.OffsetY - b);
                        gl.Vertex2f(OffsetX + f.OffsetX + b, OffsetY + f.OffsetY - b);
                        gl.Vertex2f(OffsetX + f.OffsetX + b, OffsetY + f.OffsetY + b);
                        gl.Vertex2f(OffsetX + f.OffsetX - b, OffsetY + f.OffsetY + b);
                        if (CapturedNode == i) gl.Color4ub(255, 0, 255, 255);
                        else gl.Color4ub(255, 0, 255, 128);
                        gl.Vertex2f(OffsetX + f.OffsetX - s, OffsetY + f.OffsetY - s);
                        gl.Vertex2f(OffsetX + f.OffsetX + s, OffsetY + f.OffsetY - s);
                        gl.Vertex2f(OffsetX + f.OffsetX + s, OffsetY + f.OffsetY + s);
                        gl.Vertex2f(OffsetX + f.OffsetX - s, OffsetY + f.OffsetY + s);
                    }
                    gl.End();
                    if (CapturedNode >= 0)
                    {
                        gl.Begin(GL.LINES);
                        gl.Color4ub(255, 0, 0, 128);
                        gl.Vertex2f(OffsetX + frame[CapturedNode].OffsetX, -GLSurface.SurfaceSize.Height / 2f);
                        gl.Vertex2f(OffsetX + frame[CapturedNode].OffsetX, GLSurface.SurfaceSize.Height / 2f);
                        gl.Vertex2f(-GLSurface.SurfaceSize.Width / 2f, OffsetY + frame[CapturedNode].OffsetY);
                        gl.Vertex2f(GLSurface.SurfaceSize.Width / 2f, OffsetY + frame[CapturedNode].OffsetY);
                        gl.End();
                    }
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
                if (CapturedNode >= 0)
                {
                    var node = CurrentNode;
                    if (node != null)
                    {
                        node.Angle += (float)AngleNumeric.Increment * e.Delta;
                        AngleNumeric.Value = (decimal)node.Angle;
                    }
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

                if (CurrentRagdoll == null) return;

                float b = LoadedResource.PointBoundsX / GLSurface.Zoom;
                var frame = CurrentRagdoll.MakeFrame(CurrentFrame);
                if (frame == null) return;

                CapturedNode = -1;
                for (int i = frame.Count - 1; i >= 0; i--)
                {
                    var f = frame[i];

                    if (MouseManager.CurrentLocation.X >= OffsetX + f.OffsetX - b &&
                        MouseManager.CurrentLocation.X <= OffsetX + f.OffsetX + b &&
                        MouseManager.CurrentLocation.Y >= OffsetY + f.OffsetY - b &&
                        MouseManager.CurrentLocation.Y <= OffsetY + f.OffsetY + b)
                    {
                        if (!LoadedResource.Playing && CurrentFrame != null)
                        {
                            NodesListBox.SelectedIndex = i;

                            if (!e.Button.HasFlag(MouseButtons.Middle))
                            {
                                CapturedNode = i;
                                LockStory();
                            }
                            else
                            {
                                if (ModifierKeys == Keys.Shift)
                                {
                                    var node = CurrentNode;
                                    var rnode = CurrentRagdollNode;
                                    var mnode = CurrentFrame[rnode?.MainNode ?? -1];
                                    if (node != null && mnode != null) node.Angle = mnode.Angle;
                                    CurrentNode = node;
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

                if (CurrentRagdoll == null) return;

                var frame = CurrentRagdoll.MakeFrame(CurrentFrame);

                if (CapturedNode >= 0)
                {
                    var node = CurrentNode;
                    var rnode = CurrentRagdollNode;
                    if (node != null && rnode != null)
                    {
                        if (e.Button.HasFlag(MouseButtons.Left) && frame != null)
                        {
                            var mnode = CurrentFrame[rnode.MainNode];
                            var fnode = frame[rnode.MainNode];

                            if (mnode != null && fnode != null)
                            {
                                float x = e.X - OffsetX - fnode.OffsetX;
                                float y = e.Y - OffsetY - fnode.OffsetY;

                                float base_angle = (float)Math.Atan2(rnode.OffsetY + node.OffsetY, rnode.OffsetX + node.OffsetX);
                                float mouse_angle = (float)Math.Atan2(y, x);

                                mnode.Angle = mouse_angle - base_angle;
                                if (ModifierKeys == Keys.Shift)
                                    node.Angle = mnode.Angle;
                            }
                        }
                        if (e.Button.HasFlag(MouseButtons.Right))
                        {
                            if (LoadedResource.PixelPerfect)
                            {
                                var p = Frame.Rotate(MouseManager.CurrentStepDelta, -CurrentFrame[rnode.MainNode]?.Angle ?? 0f);
                                node.OffsetX += p.X;
                                node.OffsetY += p.Y;
                            }
                            else
                            {
                                var p = Frame.Rotate(MouseManager.CurrentDelta, -CurrentFrame[rnode.MainNode]?.Angle ?? 0f);
                                node.OffsetX += p.X;
                                node.OffsetY += p.Y;
                            }
                            AngleNumeric.Value = (decimal)node.Angle;
                            OffsetXNumeric.Value = (decimal)node.OffsetX;
                            OffsetYNumeric.Value = (decimal)node.OffsetY;
                        }
                    }
                }
                else
                {
                    if (e.Button.HasFlag(MouseButtons.Left))
                    {
                        OffsetX += MouseManager.CurrentDelta.X;
                        OffsetY += MouseManager.CurrentDelta.Y;
                    }
                    if (e.Button.HasFlag(MouseButtons.Right))
                    {
                        GridX += MouseManager.CurrentDelta.X;
                        GridY += MouseManager.CurrentDelta.Y;
                        GridX -= (int)GridX;
                        GridY -= (int)GridY;
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

                if (CurrentRagdoll == null) return;
                UnlockStory();

                //if (CapturedNode >= 0) Story[CapturedNode] = new Node(CurrentNode);

                CapturedNode = -1;
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

        private void ToggleAnimationMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = sender as ToolStripMenuItem;
                if (item != null)
                {
                    if (item.Checked = !item.Checked) LoadedResource.Play(FramesListBox.SelectedIndex);
                    else LoadedResource.Stop();

                    UpdateState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle animation.",
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
                    LinkTextBox.Text = subform.SelectedResources[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateFrameMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frame = CurrentFrame;
                if (frame != null)
                {
                    var index = FramesListBox.SelectedIndex + 1;
                    Story.Insert(index, new Frame(frame));
                    FramesListBox.SelectedIndex = index;
                }
                else
                {
                    Story.Add(new Frame(LoadedResource.NodesCount));
                    FramesListBox.SelectedIndex = Story.Count - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not create frame.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveFrameMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = FramesListBox.SelectedIndex;
                if (index >= 0 && index < Story.Count)
                {
                    Story.RemoveAt(index);
                    if (index == Story.Count) FramesListBox.SelectedIndex = index - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not remove frame.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveFrameUpMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = FramesListBox.SelectedIndex;
                var nindex = index - 1;
                if (index >= 0 && index < Story.Count)
                {
                    if (nindex < 0)
                    {
                        for (int i = 0; i < Story.Count - 1; i++)
                        {
                            Story.Swap(i, i + 1);
                            if (i < Story.Count - 2) Story.AppendAction();
                        }
                        nindex = Story.Count - 1;
                    }
                    else Story.Swap(index, nindex);
                    FramesListBox.SelectedIndex = nindex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move frame up.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveFrameDownMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = FramesListBox.SelectedIndex;
                var nindex = index + 1;
                if (index >= 0 && index < Story.Count)
                {
                    if (nindex >= Story.Count)
                    {
                        for (int i = Story.Count - 1; i > 0; i--)
                        {
                            Story.Swap(i, i - 1);
                            if (i > 1) Story.AppendAction();
                        }
                        nindex = 0;
                    }
                    else Story.Swap(index, nindex);
                    FramesListBox.SelectedIndex = nindex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move frame down.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateNodesMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentRagdoll != null && CurrentRagdoll.Count == LoadedResource.NodesCount)
                    if (MessageBox.Show(this, "Are you sure you want to create node?",
                        "Warning: Current node count suits for ragdoll.", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) != DialogResult.Yes) return;

                var index = NodesListBox.SelectedIndex + 1;
                if (index < 0 || index > LoadedResource.NodesCount) return;
                for (int i = 0; i < Story.Count; i++)
                {
                    Story[i] = Story[i].Insert(index);
                    if (i < Story.Count - 1) Story.AppendAction();
                }
                LoadedResource.NodesCount++;
                NodesListBox.SelectedIndex = index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not create nodes.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveNodesMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentRagdoll != null && CurrentRagdoll.Count == LoadedResource.NodesCount)
                    if (MessageBox.Show(this, "Are you sure you want to remove node?",
                        "Warning: Current node count suits for ragdoll.", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) != DialogResult.Yes) return;

                var index = NodesListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.NodesCount) return;
                LoadedResource.NodesCount--;
                for (int i = 0; i < Story.Count; i++)
                {
                    Story[i] = Story[i].Remove(index);
                    if (i < Story.Count - 1) Story.AppendAction();
                }
                NodesListBox.SelectedIndex = index < NodesListBox.Items.Count ? index : index - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not remove nodes.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveNodesUpMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = NodesListBox.SelectedIndex;
                var nindex = index - 1;
                if (index >= 0 && index < LoadedResource.NodesCount)
                {
                    for (int s = 0; s < Story.Count; s++)
                    {
                        var f = new Frame(Story[s]);
                        if (nindex < 0)
                        {
                            for (int i = 0; i < f.Count - 1; i++) f.Swap(i, i + 1);
                            nindex = f.Count - 1;
                        }
                        else f.Swap(index, nindex);
                        Story[s] = f;
                        if (s < Story.Count - 1) Story.AppendAction();
                    }
                    NodesListBox.SelectedIndex = nindex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move nodes up.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveNodesDownMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var index = NodesListBox.SelectedIndex;
                var nindex = index + 1;
                if (index >= 0 && index < LoadedResource.NodesCount)
                {
                    for (int s = 0; s < Story.Count; s++)
                    {
                        var f = new Frame(Story[s]);

                        if (nindex >= f.Count)
                        {
                            for (int i = f.Count - 1; i > 0; i--) f.Swap(i, i - 1);
                            nindex = 0;
                        }
                        else f.Swap(index, nindex);

                        Story[s] = f;
                        if (s < Story.Count - 1) Story.AppendAction();
                    }
                    NodesListBox.SelectedIndex = nindex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not nodes down.",
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
                GLSurface.BackColor = BackgroundColorDialog.Color;
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
        private void LockNodesMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = sender as ToolStripMenuItem;
                if (item != null) item.Checked = (NodesLocked = !NodesLocked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle transparency.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InvertViewMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = sender as ToolStripMenuItem;
                if (item != null) item.Checked = (ViewInverted = !ViewInverted);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle transparency.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ToggleVisibleMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = sender as ToolStripMenuItem;
                if (item != null)
                {
                    if (NodeVisible != null)
                    {
                        int index = NodesListBox.SelectedIndex;
                        if (index >= 0 && index < NodeVisible.Length)
                        {
                            item.Checked = (NodeVisible[index] = !NodeVisible[index]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle transparency.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FramesListBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if (FramesListBox.SelectedIndex == 0)
                        {
                            FramesListBox.SelectedIndex = FramesListBox.Items.Count - 1;
                            e.Handled = true;
                        }
                        break;
                    case Keys.Down:
                        if (FramesListBox.SelectedIndex == FramesListBox.Items.Count - 1)
                        {
                            FramesListBox.SelectedIndex = 0;
                            e.Handled = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle KeyDown event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
