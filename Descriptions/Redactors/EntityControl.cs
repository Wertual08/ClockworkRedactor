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
using System.Diagnostics;

namespace Resource_Redactor.Descriptions.Redactors
{
    public partial class EntityControl : UserControl, IResourceControl
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

        private DragManager MouseManager = new DragManager(0.015625f);
        private static readonly List<string> BoolNames = new List<string>(Enum.GetNames(typeof(EntityResource.Trigger.BoolConditional)));
        private static readonly List<string> DirectionNames = new List<string>(Enum.GetNames(typeof(EntityResource.Trigger.DirectionConditional)));
        private enum Parameter
        {
            MaxHealth,
            MaxEnergy,
            Mass,
            GravityAcceleration,
            JumpVelocity,
            DragX,
            DragY,
            SqrDragX,
            SqrDragY,
            MoveForceX,
            MoveForceY,
        }
        private static string[] GetNames(Type t, int align)
        {
            var result = Enum.GetNames(t);
            for (int i = 0; i < result.Length; i++)
                result[i] += ":" + new string(' ', Math.Max(1, align - result[i].Length));
            return result;
        }
        private readonly string[] ParameterNames = GetNames(typeof(Parameter), 20);
        private struct Action
        {
            public struct Trigger
            {
                public string Name;
                public string Action;

                public double VelocityXLowBound;
                public double VelocityXHighBound;

                public double VelocityYLowBound;
                public double VelocityYHighBound;

                public double AccelerationXLowBound;
                public double AccelerationXHighBound;

                public double AccelerationYLowBound;
                public double AccelerationYHighBound;

                public int OnGround;
                public int OnWall;
                public int OnRoof;
                public int Direction;

                public string Animation;

                public Trigger(EntityResource.Trigger t)
                {
                    Name = t.Name;
                    Action = t.Action;

                    VelocityXLowBound = t.VelocityXLowBound;
                    VelocityXHighBound = t.VelocityXHighBound;

                    VelocityYLowBound = t.VelocityYLowBound;
                    VelocityYHighBound = t.VelocityYHighBound;

                    AccelerationXLowBound = t.AccelerationXLowBound;
                    AccelerationXHighBound = t.AccelerationXHighBound;

                    AccelerationYLowBound = t.AccelerationYLowBound;
                    AccelerationYHighBound = t.AccelerationYHighBound;

                    OnGround = t.OnGround;
                    OnWall = t.OnWall;
                    OnRoof = t.OnRoof;
                    Direction = t.Direction;

                    Animation = t.Animation.Link;
                }
                public void ToResource(EntityResource.Trigger t)
                {
                    t.Name = Name;
                    t.Action = Action;
                    t.VelocityXLowBound = VelocityXLowBound;
                    t.VelocityXHighBound = VelocityXHighBound;
                    t.VelocityYLowBound = VelocityYLowBound;
                    t.VelocityYHighBound = VelocityYHighBound;
                    t.AccelerationXLowBound = AccelerationXLowBound;
                    t.AccelerationXHighBound = AccelerationXHighBound;
                    t.AccelerationYLowBound = AccelerationYLowBound;
                    t.AccelerationYHighBound = AccelerationYHighBound;
                    t.OnGround = OnGround;
                    t.OnWall = OnWall;
                    t.OnRoof = OnRoof;
                    t.Direction = Direction;
                    if (t.Animation.Link != Animation) 
                        t.Animation.Link = Animation;
                }
                public override bool Equals(object obj)
                {
                    if (!(obj is Trigger)) return false;
                    var t = (Trigger)obj;

                    if (Name != t.Name) return false;
                    if (Action != t.Action) return false;

                    if (VelocityXLowBound != t.VelocityXLowBound) return false;
                    if (VelocityXHighBound != t.VelocityXHighBound) return false;

                    if (VelocityYLowBound != t.VelocityYLowBound) return false;
                    if (VelocityYHighBound != t.VelocityYHighBound) return false;

                    if (AccelerationXLowBound != t.AccelerationXLowBound) return false;
                    if (AccelerationXHighBound != t.AccelerationXHighBound) return false;

                    if (AccelerationYLowBound != t.AccelerationYLowBound) return false;
                    if (AccelerationYHighBound != t.AccelerationYHighBound) return false;

                    if (OnGround != t.OnGround) return false;
                    if (OnWall != t.OnWall) return false;
                    if (OnRoof != t.OnRoof) return false;
                    if (Direction != t.Direction) return false;

                    if (Animation != t.Animation) return false;

                    return true;
                }
            }
            public struct Holder
            {
                public string Name;
                public string Action;
                public int HolderPoint;
                public string Animation;

                public Holder(EntityResource.Holder h)
                {
                    Name = h.Name;
                    Action = h.Action;
                    HolderPoint = h.HolderPoint;
                    Animation = h.Animation.Link;
                }
                public void ToResource(EntityResource.Holder h)
                {
                    h.Name = Name;
                    h.Action = Action;
                    h.HolderPoint = HolderPoint;
                    if (h.Animation.Link != Animation) 
                        h.Animation.Link = Animation;
                }
                public override bool Equals(object obj)
                {
                    if (!(obj is Holder)) return false;
                    var h = (Holder)obj;

                    if (Name != h.Name) return false;
                    if (Action != h.Action) return false;
                    if (HolderPoint != h.HolderPoint) return false;
                    if (Animation != h.Animation) return false;

                    return true;
                }
            }
            public string Ragdoll;
            public Trigger[] Triggers;
            public Holder[] Holders;

            public ulong MaxHealth;
            public ulong MaxEnergy;
            public double Mass;

            public double GravityAcceleration;
            public double JumpVelocity;
            public double DragX;
            public double DragY;
            public double SqrDragX;
            public double SqrDragY;
            public double MoveForceX;
            public double MoveForceY;

            public Color BackColor;
            public PointF PointBounds;
            public bool GridEnabled;
            public bool Transparency;

            public Action(EntityResource e)
            {
                Ragdoll = e.Ragdoll.Link;
                Triggers = new Trigger[e.Triggers.Count];
                Holders = new Holder[e.Holders.Count];

                for (int i = 0; i < Triggers.Length; i++)
                    Triggers[i] = new Trigger(e.Triggers[i]);
                for (int i = 0; i < Holders.Length; i++)
                    Holders[i] = new Holder(e.Holders[i]);

                MaxHealth = e.MaxHealth;
                MaxEnergy = e.MaxEnergy;
                Mass = e.Mass;
                GravityAcceleration = e.GravityAcceleration;
                JumpVelocity = e.JumpVelocity;
                DragX = e.DragX;
                DragY = e.DragY;
                SqrDragX = e.SqrDragX;
                SqrDragY = e.SqrDragY;
                MoveForceX = e.MoveForceX;
                MoveForceY = e.MoveForceY;

                BackColor = e.BackColor;
                PointBounds = e.PointBounds;
                GridEnabled = e.GridEnabled;
                Transparency = e.Transparency;
            }
            public void ToResource(EntityResource e)
            {
                e.Ragdoll.Link = Ragdoll;
                
                for (int i = 0; i < Triggers.Length; i++)
                {
                    if (i >= e.Triggers.Count) e.Triggers.Add(new EntityResource.Trigger());
                    Triggers[i].ToResource(e.Triggers[i]);
                }
                while (e.Triggers.Count > Triggers.Length) e.Triggers.RemoveAt(e.Triggers.Count - 1);
                for (int i = 0; i < Holders.Length; i++)
                {
                    if (i >= e.Holders.Count) e.Holders.Add(new EntityResource.Holder());
                    Holders[i].ToResource(e.Holders[i]);
                }
                while (e.Holders.Count > Holders.Length) e.Holders.RemoveAt(e.Holders.Count - 1);

                e.MaxHealth = MaxHealth;
                e.MaxEnergy = MaxEnergy;
                e.Mass = Mass;
                e.GravityAcceleration = GravityAcceleration;
                e.JumpVelocity = JumpVelocity;
                e.DragX = DragX;
                e.DragY = DragY;
                e.SqrDragX = SqrDragX;
                e.SqrDragY = SqrDragY;
                e.MoveForceX = MoveForceX;
                e.MoveForceY = MoveForceY;

                e.BackColor = BackColor;
                e.PointBounds = PointBounds;
                e.GridEnabled = GridEnabled;
                e.Transparency = Transparency;
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Action)) return false;
                var a = (Action)obj;

                if (Triggers.Length != a.Triggers.Length) return false;
                if (Holders.Length != a.Holders.Length) return false;
                for (int i = 0; i < Triggers.Length; i++)
                    if (!Triggers[i].Equals(a.Triggers[i])) return false;
                for (int i = 0; i < Holders.Length; i++)
                    if (!Holders[i].Equals(a.Holders[i])) return false;

                if (MaxHealth != a.MaxHealth) return false;
                if (MaxEnergy != a.MaxEnergy) return false;
                if (Mass != a.Mass) return false;
                if (GravityAcceleration != a.GravityAcceleration) return false;
                if (JumpVelocity != a.JumpVelocity) return false;
                if (DragX != a.DragX) return false;
                if (DragY != a.DragY) return false;
                if (SqrDragX != a.SqrDragX) return false;
                if (SqrDragY != a.SqrDragY) return false;
                if (MoveForceX != a.MoveForceX) return false;
                if (MoveForceY != a.MoveForceY) return false;
                if (Ragdoll != a.Ragdoll) return false;

                if (BackColor != a.BackColor) return false;
                if (PointBounds != a.PointBounds) return false;
                if (GridEnabled != a.GridEnabled) return false;
                if (Transparency != a.Transparency) return false;

                return true;
            }
        }
        private StoryItem<Action> Story;

        private float OffsetX = 0f, OffsetY = 0f;
        private Stopwatch FrameTimer = new Stopwatch();
        private EntityResource LoadedResource;
        private PreviewSurface.Entity EntityController;

        private EntityResource.Trigger SelectedAnimation
        {
            get
            {
                int index = AnimationsListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Triggers.Count) return null;
                else return LoadedResource.Triggers[index];
            }
            set
            {
                int index = AnimationsListBox.SelectedIndex;
                if (index >= 0 || index < LoadedResource.Triggers.Count)
                    LoadedResource.Triggers[index] = value;
            }
        }
        private EntityResource.Holder SelectedHolder
        {
            get
            {
                int index = HoldersListBox.SelectedIndex;
                if (index < 0 || index >= LoadedResource.Holders.Count) return null;
                else return LoadedResource.Holders[index];
            }
            set
            {
                int index = HoldersListBox.SelectedIndex;
                if (index >= 0 || index < LoadedResource.Holders.Count)
                    LoadedResource.Holders[index] = value;
            }
        }

        public EntityControl(string path)
        {
            InitializeComponent();

            AnimationParameterNumeric.FixMouseWheel();
            AnimationParameterDomain.FixMouseWheel();
            EntityParameterNumeric.FixMouseWheel();
            HolderNodeNumeric.FixMouseWheel();
            ToolDelayNumeric.FixMouseWheel();

            MenuTabs = new ToolStripMenuItem[] {
                new ToolStripMenuItem("Link resource", null, LinkResourceMenuItem_Click, Keys.Control | Keys.I),
                new ToolStripMenuItem("Create", null, CreateMenuItem_Click, Keys.Control | Keys.A),
                new ToolStripMenuItem("Remove", null, RemoveMenuItem_Click, Keys.Control | Keys.D),
                new ToolStripMenuItem("Move up", null, MoveUpMenuItem_Click, Keys.Control | Keys.Up),
                new ToolStripMenuItem("Move down", null, MoveDownMenuItem_Click, Keys.Control | Keys.Down),
                new ToolStripMenuItem("Toggle grid", null, ToggleGridMenuItem_Click, Keys.Control | Keys.G),
                new ToolStripMenuItem("Toggle transparency", null, ToggleTransparencyMenuItem_Click, Keys.Control | Keys.H),
                new ToolStripMenuItem("Background color", null, BackColorMenuItem_Click, Keys.Control | Keys.L),
                new ToolStripMenuItem("Reset position", null, ResetPositionMenuItem_Click, Keys.Control | Keys.R),
            };

            GLSurface.MakeCurrent();
            LoadedResource = new EntityResource(path);
            RagdollLinkTextBox.Text = LoadedResource.Ragdoll.Link;
            Story = new StoryItem<Action>(new Action(LoadedResource));
            Story.ValueChanged += Story_ValueChanged;

            ResourcePath = path;
            ResourceName = Path.GetFileName(path);

            LoadedResource.Ragdoll.SynchronizingObject = this;
            LoadedResource.Ragdoll.Updated += Ragdoll_Reloaded;

            EntityParametersListBox.BeginUpdate();
            EntityParametersListBox.Items.AddRange(ParameterNames);
            for (int i = 0; i < EntityParametersListBox.Items.Count; i++)
                EntityParametersListBox.Items[i] = ParameterNames[i] + GetEntityParameter((Parameter)i);
            EntityParametersListBox.EndUpdate();

            AnimationsListBox.BeginUpdate();
            for (int i = 0; i < LoadedResource.Triggers.Count; i++)
                AnimationsListBox.Items.Add(LoadedResource.Triggers[i].Name);
            AnimationsListBox.EndUpdate();

            HoldersListBox.BeginUpdate();
            for (int i = 0; i < LoadedResource.Holders.Count; i++)
                HoldersListBox.Items.Add(LoadedResource.Holders[i].Name);
            HoldersListBox.EndUpdate();

            GLSurface.BackColor = LoadedResource.BackColor;

            GetTab("Create").Enabled = false;
            GetTab("Toggle grid").Checked = LoadedResource.GridEnabled;
            GetTab("Toggle transparency").Checked = LoadedResource.Transparency;

            GLSurface.SetFieldSize(20d, 20d);
            GLSurface.LoadEntity(LoadedResource, "LoadedResource");
            EntityController = GLSurface.GetEntity("LoadedResource");
            EntityController.ToolCycle = 1000;
            EntityController.Tool.SynchronizingObject = this;
            GLSurface.GridEnabled = LoadedResource.GridEnabled;
            GLFrameTimer.Start();
        }
        private void ParamsTabControl_Selected(object sender, TabControlEventArgs e)
        {
            try
            { 
                GetTab("Create").Enabled = e.TabPageIndex != 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select params tab.",
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
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        private ToolStripMenuItem GetTab(string title)
        {
            return MenuTabs.First((ToolStripMenuItem item) => { return item.Text == title; });
        }
        string GetEntityParameter(Parameter p)
        {
            switch (p)
            {
                case Parameter.MaxHealth: return LoadedResource.MaxHealth.ToString("0.000000"); 
                case Parameter.MaxEnergy: return LoadedResource.MaxEnergy.ToString("0.000000"); 
                case Parameter.Mass: return LoadedResource.Mass.ToString("0.000000"); 
                case Parameter.GravityAcceleration: return LoadedResource.GravityAcceleration.ToString("0.000000");
                case Parameter.JumpVelocity: return LoadedResource.JumpVelocity.ToString("0.000000"); 
                case Parameter.DragX: return LoadedResource.DragX.ToString("0.000000");
                case Parameter.DragY: return LoadedResource.DragY.ToString("0.000000");
                case Parameter.SqrDragX: return LoadedResource.SqrDragX.ToString("0.000000"); 
                case Parameter.SqrDragY: return LoadedResource.SqrDragY.ToString("0.000000");
                case Parameter.MoveForceX: return LoadedResource.MoveForceX.ToString("0.000000");
                case Parameter.MoveForceY: return LoadedResource.MoveForceY.ToString("0.000000");
                default: return "{UNKNOWN_PARAMETER}";
            }
        }
        private void EntityParametersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                EntityParameterNumeric.Enabled = true;
                EntityParameterNumeric.DecimalPlaces = 6;
                EntityParameterNumeric.Maximum = 2147483647;
                EntityParameterNumeric.Minimum = -2147483648;
                switch ((Parameter)EntityParametersListBox.SelectedIndex)
                {
                    case Parameter.MaxHealth:
                        EntityParameterNumeric.DecimalPlaces = 6;
                        EntityParameterNumeric.Maximum = ulong.MaxValue;
                        EntityParameterNumeric.Minimum = ulong.MinValue; 
                        EntityParameterNumeric.Value = (decimal)LoadedResource.MaxHealth; 
                        break;
                    case Parameter.MaxEnergy:
                        EntityParameterNumeric.DecimalPlaces = 6;
                        EntityParameterNumeric.Maximum = ulong.MaxValue;
                        EntityParameterNumeric.Minimum = ulong.MinValue; 
                        EntityParameterNumeric.Value = (decimal)LoadedResource.MaxEnergy; 
                        break;
                    case Parameter.Mass: EntityParameterNumeric.Value = (decimal)LoadedResource.Mass; break;
                    case Parameter.GravityAcceleration: EntityParameterNumeric.Value = (decimal)LoadedResource.GravityAcceleration; break;
                    case Parameter.JumpVelocity: EntityParameterNumeric.Value = (decimal)LoadedResource.JumpVelocity; break;
                    case Parameter.DragX: EntityParameterNumeric.Value = (decimal)LoadedResource.DragX; break;
                    case Parameter.DragY: EntityParameterNumeric.Value = (decimal)LoadedResource.DragY; break;
                    case Parameter.SqrDragX: EntityParameterNumeric.Value = (decimal)LoadedResource.SqrDragX; break;
                    case Parameter.SqrDragY: EntityParameterNumeric.Value = (decimal)LoadedResource.SqrDragY; break;
                    case Parameter.MoveForceX: EntityParameterNumeric.Value = (decimal)LoadedResource.MoveForceX; break;
                    case Parameter.MoveForceY: EntityParameterNumeric.Value = (decimal)LoadedResource.MoveForceY; break;
                    default: EntityParameterNumeric.Enabled = false; break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select parameter.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void EntityParameterNumeric_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int index = EntityParametersListBox.SelectedIndex;
                switch ((Parameter)index)
                {
                    case Parameter.MaxHealth: LoadedResource.MaxHealth = (ulong)EntityParameterNumeric.Value; break;
                    case Parameter.MaxEnergy: LoadedResource.MaxEnergy = (ulong)EntityParameterNumeric.Value; break;
                    case Parameter.Mass: LoadedResource.Mass = (double)EntityParameterNumeric.Value; break;
                    case Parameter.GravityAcceleration: LoadedResource.GravityAcceleration = (double)EntityParameterNumeric.Value; break;
                    case Parameter.JumpVelocity: LoadedResource.JumpVelocity = (double)EntityParameterNumeric.Value; break;
                    case Parameter.DragX: LoadedResource.DragX = (double)EntityParameterNumeric.Value; break;
                    case Parameter.DragY: LoadedResource.DragY = (double)EntityParameterNumeric.Value; break;
                    case Parameter.SqrDragX: LoadedResource.SqrDragX = (double)EntityParameterNumeric.Value; break;
                    case Parameter.SqrDragY: LoadedResource.SqrDragY = (double)EntityParameterNumeric.Value; break;
                    case Parameter.MoveForceX: LoadedResource.MoveForceX = (double)EntityParameterNumeric.Value; break;
                    case Parameter.MoveForceY: LoadedResource.MoveForceY = (double)EntityParameterNumeric.Value; break;
                }
                if (index >= 0 && index < EntityParametersListBox.Items.Count)
                    EntityParametersListBox.Items[index] = ParameterNames[index] + GetEntityParameter((Parameter)index);

                Story.Item = new Action(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not edit parameter.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Story_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Story.Item.ToResource(LoadedResource);

                for (int i = 0; i < EntityParametersListBox.Items.Count; i++)
                    EntityParametersListBox.Items[i] = ParameterNames[i] + GetEntityParameter((Parameter)i);
                EntityParameterNumeric.Enabled = true;
                switch ((Parameter)EntityParametersListBox.SelectedIndex)
                {
                    case Parameter.MaxHealth: EntityParameterNumeric.Value = (decimal)LoadedResource.MaxHealth; break;
                    case Parameter.MaxEnergy: EntityParameterNumeric.Value = (decimal)LoadedResource.MaxEnergy; break;
                    case Parameter.Mass: EntityParameterNumeric.Value = (decimal)LoadedResource.Mass; break;
                    case Parameter.GravityAcceleration: EntityParameterNumeric.Value = (decimal)LoadedResource.GravityAcceleration; break;
                    case Parameter.JumpVelocity: EntityParameterNumeric.Value = (decimal)LoadedResource.JumpVelocity; break;
                    case Parameter.DragX: EntityParameterNumeric.Value = (decimal)LoadedResource.DragX; break;
                    case Parameter.DragY: EntityParameterNumeric.Value = (decimal)LoadedResource.DragY; break;
                    case Parameter.SqrDragX: EntityParameterNumeric.Value = (decimal)LoadedResource.SqrDragX; break;
                    case Parameter.SqrDragY: EntityParameterNumeric.Value = (decimal)LoadedResource.SqrDragY; break;
                    case Parameter.MoveForceX: EntityParameterNumeric.Value = (decimal)LoadedResource.MoveForceX; break;
                    case Parameter.MoveForceY: EntityParameterNumeric.Value = (decimal)LoadedResource.MoveForceY; break;
                    default: EntityParameterNumeric.Enabled = false; break;
                }

                int aind = AnimationsListBox.SelectedIndex;
                AnimationsListBox.BeginUpdate();
                AnimationsListBox.Items.Clear();
                for (int i = 0; i < LoadedResource.Triggers.Count; i++)
                {
                    AnimationsListBox.Items.Add(LoadedResource.Triggers[i].Name);
                    LoadedResource.Triggers[i].Animation.SynchronizingObject = this;
                    LoadedResource.Triggers[i].Animation.Updated = Animation_Reloaded;
                }
                AnimationsListBox.EndUpdate();
                AnimationsListBox.SelectedIndex = Math.Min(AnimationsListBox.Items.Count - 1, aind);

                int hind = HoldersListBox.SelectedIndex;
                HoldersListBox.BeginUpdate();
                HoldersListBox.Items.Clear();
                for (int i = 0; i < LoadedResource.Holders.Count; i++)
                {
                    HoldersListBox.Items.Add(LoadedResource.Holders[i].Name);
                    LoadedResource.Holders[i].Animation.SynchronizingObject = this;
                    LoadedResource.Holders[i].Animation.Updated = Holder_Reloaded;
                }
                HoldersListBox.EndUpdate();
                HoldersListBox.SelectedIndex = Math.Min(HoldersListBox.Items.Count - 1, hind);

                GetTab("Toggle grid").Checked = LoadedResource.GridEnabled;
                GetTab("Toggle transparency").Checked = LoadedResource.Transparency;

                GLSurface.GridEnabled = LoadedResource.GridEnabled;
                GLSurface.BackColor = LoadedResource.BackColor;

                UpdateRedactor();
                MakeUnsaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not update resource data.",
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
                double dt = FrameTimer.Elapsed.TotalSeconds;
                FrameTimer.Restart();
                GLSurface.Update(dt);
                GLSurface.Render(OffsetX, OffsetY);
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
                if (ModifierKeys == Keys.Control)
                {
                    if (LoadedResource.PointBounds.X + e.Delta > 1.0f)
                    {
                        LoadedResource.PointBounds.X += e.Delta;
                        LoadedResource.PointBounds.Y += e.Delta;
                        MakeUnsaved();
                    }
                }
                else
                {
                    var z = 1f + e.Delta / 10f;
                    GLSurface.Zoom *= z;
                    OffsetX += e.X * (z - 1f);
                    OffsetY += e.Y * (z - 1f);

                    //RepairOffset();
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
                if (e.Button.HasFlag(MouseButtons.Left))
                {
                    EntityController.Fire = true;
                    EntityController.CursX = e.X + OffsetX;
                    EntityController.CursY = e.Y + OffsetY;
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

                if (e.Button.HasFlag(MouseButtons.Right))
                {
                    OffsetX -= MouseManager.CurrentDelta.X;
                    OffsetY -= MouseManager.CurrentDelta.Y;
                }

                if (e.Button.HasFlag(MouseButtons.Left))
                {
                }

                EntityController.CursX = e.X + OffsetX;
                EntityController.CursY = e.Y + OffsetY;
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

                if (e.Button.HasFlag(MouseButtons.Left))
                {
                    EntityController.Fire = false;
                }
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
        private void GLSurface_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.W: EntityController.MoveU = true; EntityController.Jump = true; e.Handled = true; break;
                    case Keys.A: EntityController.MoveL = true; e.Handled = true; break;
                    case Keys.S: EntityController.MoveD = true; e.Handled = true; break;
                    case Keys.D: EntityController.MoveR = true; e.Handled = true; break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle KeyDown event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.W: EntityController.MoveU = false; EntityController.Jump = false; e.Handled = true; break;
                    case Keys.A: EntityController.MoveL = false; e.Handled = true; break;
                    case Keys.S: EntityController.MoveD = false; e.Handled = true; break;
                    case Keys.D: EntityController.MoveR = false; e.Handled = true; break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle KeyUp event.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GLSurface_Resize(object sender, EventArgs e)
        {
            GLSurface.SetFieldSize(GLSurface.ClientSize.Width / 16 - 2f, GLSurface.ClientSize.Height / 16 - 2f);
        }

        private void LinkResourceMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                if (ParamsTabControl.SelectedIndex == 0)
                {
                    var subform = new ExplorerForm(Directory.GetCurrentDirectory(), false, ResourceType.Ragdoll);
                    if (subform.ShowDialog(this) == DialogResult.OK && subform.SelectedResources.Count == 1)
                        LoadedResource.Ragdoll.Link = subform.SelectedResources[0];
                    Story.Item = new Action(LoadedResource);
                }
                else if (ParamsTabControl.SelectedIndex == 1)
                {
                    if (SelectedAnimation == null) return;
                    var subform = new ExplorerForm(Directory.GetCurrentDirectory(), false, ResourceType.Animation);
                    if (subform.ShowDialog(this) == DialogResult.OK && subform.SelectedResources.Count == 1)
                        SelectedAnimation.Animation.Link = subform.SelectedResources[0];
                    Story.Item = new Action(LoadedResource);
                }
                else if (ParamsTabControl.SelectedIndex == 2)
                {
                    if (SelectedHolder == null) return;
                    var subform = new ExplorerForm(Directory.GetCurrentDirectory(), false, ResourceType.Animation);
                    if (subform.ShowDialog(this) == DialogResult.OK && subform.SelectedResources.Count == 1)
                        SelectedHolder.Animation.Link = subform.SelectedResources[0];
                    Story.Item = new Action(LoadedResource);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link resource.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ParamsTabControl.SelectedIndex == 1)
                {
                    int index = AnimationsListBox.SelectedIndex + 1;
                    LoadedResource.Triggers.Insert(index, new EntityResource.Trigger());

                    Story.Item = new Action(LoadedResource);

                    AnimationsListBox.SelectedIndex = index;
                }
                else if (ParamsTabControl.SelectedIndex == 2)
                {
                    int index = HoldersListBox.SelectedIndex + 1;
                    LoadedResource.Holders.Insert(index, new EntityResource.Holder());

                    Story.Item = new Action(LoadedResource);

                    HoldersListBox.SelectedIndex = index;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not create.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ParamsTabControl.SelectedIndex == 1)
                {
                    int index = AnimationsListBox.SelectedIndex;
                    if (index < 0 || index >= LoadedResource.Triggers.Count) return;

                    LoadedResource.Triggers.RemoveAt(index);

                    Story.Item = new Action(LoadedResource);

                    AnimationsListBox.SelectedIndex = Math.Min(index, AnimationsListBox.Items.Count - 1);
                }
                else if (ParamsTabControl.SelectedIndex == 2)
                {
                    int index = HoldersListBox.SelectedIndex;
                    if (index < 0 || index >= LoadedResource.Holders.Count) return;

                    LoadedResource.Holders.RemoveAt(index);

                    Story.Item = new Action(LoadedResource);

                    HoldersListBox.SelectedIndex = Math.Min(index, HoldersListBox.Items.Count - 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not remove.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveUpMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ParamsTabControl.SelectedIndex == 1)
                {
                    int index = AnimationsListBox.SelectedIndex;
                    if (index < 0 || index >= LoadedResource.Triggers.Count) return;
                    if (index - 1 >= 0)
                    {
                        var t = LoadedResource.Triggers[index];
                        LoadedResource.Triggers[index] = LoadedResource.Triggers[index - 1];
                        LoadedResource.Triggers[index - 1] = t;

                        Story.Item = new Action(LoadedResource);

                        AnimationsListBox.SelectedIndex = index - 1;
                    }
                    else
                    {
                        for (int i = 1; i < LoadedResource.Triggers.Count; i++)
                        {
                            var t = LoadedResource.Triggers[i];
                            LoadedResource.Triggers[i] = LoadedResource.Triggers[i - 1];
                            LoadedResource.Triggers[i - 1] = t;
                        }

                        Story.Item = new Action(LoadedResource);

                        AnimationsListBox.SelectedIndex = LoadedResource.Triggers.Count - 1;
                    }
                }
                else if (ParamsTabControl.SelectedIndex == 2)
                {
                    int index = HoldersListBox.SelectedIndex;
                    if (index < 0 || index >= LoadedResource.Holders.Count) return;
                    if (index - 1 >= 0)
                    {
                        var t = LoadedResource.Holders[index];
                        LoadedResource.Holders[index] = LoadedResource.Holders[index - 1];
                        LoadedResource.Holders[index - 1] = t;

                        Story.Item = new Action(LoadedResource);

                        HoldersListBox.SelectedIndex = index - 1;
                    }
                    else
                    {
                        for (int i = 1; i < LoadedResource.Holders.Count; i++)
                        {
                            var t = LoadedResource.Holders[i];
                            LoadedResource.Holders[i] = LoadedResource.Holders[i - 1];
                            LoadedResource.Holders[i - 1] = t;
                        }

                        Story.Item = new Action(LoadedResource);

                        HoldersListBox.SelectedIndex = LoadedResource.Holders.Count - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move up.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MoveDownMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ParamsTabControl.SelectedIndex == 1)
                {
                    int index = AnimationsListBox.SelectedIndex;
                    if (index < 0 || index >= LoadedResource.Triggers.Count) return;
                    if (index + 1 < LoadedResource.Triggers.Count)
                    {
                        var t = LoadedResource.Triggers[index];
                        LoadedResource.Triggers[index] = LoadedResource.Triggers[index + 1];
                        LoadedResource.Triggers[index + 1] = t;

                        Story.Item = new Action(LoadedResource);

                        AnimationsListBox.SelectedIndex = index + 1;
                    }
                    else
                    {
                        for (int i = LoadedResource.Triggers.Count - 1; i > 0; i--)
                        {
                            var t = LoadedResource.Triggers[i];
                            LoadedResource.Triggers[i] = LoadedResource.Triggers[i - 1];
                            LoadedResource.Triggers[i - 1] = t;
                        }

                        Story.Item = new Action(LoadedResource);

                        AnimationsListBox.SelectedIndex = 0;
                    }
                }
                else if (ParamsTabControl.SelectedIndex == 2)
                {
                    int index = HoldersListBox.SelectedIndex;
                    if (index < 0 || index >= LoadedResource.Holders.Count) return;
                    if (index + 1 < LoadedResource.Holders.Count)
                    {
                        var t = LoadedResource.Holders[index];
                        LoadedResource.Holders[index] = LoadedResource.Holders[index + 1];
                        LoadedResource.Holders[index + 1] = t;

                        Story.Item = new Action(LoadedResource);

                        HoldersListBox.SelectedIndex = index + 1;
                    }
                    else
                    {
                        for (int i = LoadedResource.Holders.Count - 1; i > 0; i--)
                        {
                            var t = LoadedResource.Holders[i];
                            LoadedResource.Holders[i] = LoadedResource.Holders[i - 1];
                            LoadedResource.Holders[i - 1] = t;
                        }

                        Story.Item = new Action(LoadedResource);

                        HoldersListBox.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not move down.",
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
                        Story.Item = new Action(LoadedResource);
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
                        Story.Item = new Action(LoadedResource);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle transparency.",
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
                Story.Item = new Action(LoadedResource);
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

        private void RagdollLinkTextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths.Length != 1 || Resource.GetType(paths[0]) != ResourceType.Ragdoll) return;
            if (e.AllowedEffect.HasFlag(DragDropEffects.Link))
                e.Effect = DragDropEffects.Link;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }
        private void RagdollLinkTextBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (paths.Length != 1)
                    {
                        MessageBox.Show(this, "You must choose only one resource file.",
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
                    else RagdollLinkTextBox.Text = ExtraPath.MakeDirectoryRelated(dpath, fpath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RagdollLinkTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                var link = LoadedResource.Ragdoll.Link;
                LoadedResource.Ragdoll.Link = RagdollLinkTextBox.Text;
                LoadedResource.Ragdoll.Reload();
                if (link != LoadedResource.Ragdoll.Link) Story.Item = new Action(LoadedResource);
                RagdollLinkTextBox.BackColor = LoadedResource.Ragdoll.Loaded ? SystemColors.Window : Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Ragdoll_Reloaded(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                RagdollLinkTextBox.Text = LoadedResource.Ragdoll.Link;
                RagdollLinkTextBox.BackColor = LoadedResource.Ragdoll.Loaded ? SystemColors.Window : Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not reload ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AnimationsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var animation = SelectedAnimation;
                if (animation != null)
                {
                    AnimationNameTextBox.Text = animation.Name;
                    AnimationNameTextBox.Enabled = true;
                    AnimationActionTextBox.Text = animation.Action;
                    AnimationActionTextBox.Enabled = true;
                    AnimationLinkTextBox.Text = animation.Animation.Link;
                    AnimationLinkTextBox.BackColor = animation.Animation.Loaded ? SystemColors.Window : Color.Red;
                    AnimationLinkTextBox.Enabled = true;
                    AnimationParametersListBox.Enabled = true;

                    AnimationParameterNumeric.Enabled = true;

                    int p = 0;
                    AnimationParametersListBox.SetItemChecked(p++, !double.IsNegativeInfinity(animation.VelocityXLowBound));
                    AnimationParametersListBox.SetItemChecked(p++, !double.IsPositiveInfinity(animation.VelocityXHighBound));
                    AnimationParametersListBox.SetItemChecked(p++, !double.IsNegativeInfinity(animation.VelocityYLowBound));
                    AnimationParametersListBox.SetItemChecked(p++, !double.IsPositiveInfinity(animation.VelocityYHighBound));
                    AnimationParametersListBox.SetItemChecked(p++, !double.IsNegativeInfinity(animation.AccelerationXLowBound));
                    AnimationParametersListBox.SetItemChecked(p++, !double.IsPositiveInfinity(animation.AccelerationXHighBound));
                    AnimationParametersListBox.SetItemChecked(p++, !double.IsNegativeInfinity(animation.AccelerationYLowBound));
                    AnimationParametersListBox.SetItemChecked(p++, !double.IsPositiveInfinity(animation.AccelerationYHighBound));
                    AnimationParametersListBox.SetItemChecked(p++, animation.OnGround != EntityResource.Trigger.DoNotCare);
                    AnimationParametersListBox.SetItemChecked(p++, animation.OnRoof != EntityResource.Trigger.DoNotCare);
                    AnimationParametersListBox.SetItemChecked(p++, animation.OnWall != EntityResource.Trigger.DoNotCare);
                    AnimationParametersListBox.SetItemChecked(p++, animation.Direction != EntityResource.Trigger.DoNotCare);

                    AnimationParametersListBox_SelectedIndexChanged(this, EventArgs.Empty);

                    GetTab("Remove").Enabled = true;
                }
                else
                {
                    AnimationNameTextBox.Text = "";
                    AnimationNameTextBox.Enabled = false;
                    AnimationActionTextBox.Text = "";
                    AnimationLinkTextBox.Text = "";
                    AnimationLinkTextBox.Enabled = false;
                    AnimationParametersListBox.Enabled = false;
                    AnimationParametersListBox.SelectedIndex = -1;

                    GetTab("Remove").Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select animation.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AnimationParametersListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                int index = e.Index;
                bool enabled = e.NewValue == CheckState.Checked;
                var animation = SelectedAnimation;
                if (animation == null) return;

                int p = 0;
                if (p++ == index && enabled == double.IsNegativeInfinity(animation.VelocityXLowBound)) animation.VelocityXLowBound = enabled ? 0 : double.NegativeInfinity;
                else if (p++ == index && enabled == double.IsPositiveInfinity(animation.VelocityXHighBound)) animation.VelocityXHighBound = enabled ? 0 : double.PositiveInfinity;
                else if (p++ == index && enabled == double.IsNegativeInfinity(animation.VelocityYLowBound)) animation.VelocityYLowBound = enabled ? 0 : double.NegativeInfinity;
                else if (p++ == index && enabled == double.IsPositiveInfinity(animation.VelocityYHighBound)) animation.VelocityYHighBound = enabled ? 0 : double.PositiveInfinity;
                else if (p++ == index && enabled == double.IsNegativeInfinity(animation.AccelerationXLowBound)) animation.AccelerationXLowBound = enabled ? 0 : double.NegativeInfinity;
                else if (p++ == index && enabled == double.IsPositiveInfinity(animation.AccelerationXHighBound)) animation.AccelerationXHighBound = enabled ? 0 : double.PositiveInfinity;
                else if (p++ == index && enabled == double.IsNegativeInfinity(animation.AccelerationYLowBound)) animation.AccelerationYLowBound = enabled ? 0 : double.NegativeInfinity;
                else if (p++ == index && enabled == double.IsPositiveInfinity(animation.AccelerationYHighBound)) animation.AccelerationYHighBound = enabled ? 0 : double.PositiveInfinity;
                else if (p++ == index && enabled == (animation.OnGround == EntityResource.Trigger.DoNotCare)) animation.OnGround = enabled ? 0 : EntityResource.Trigger.DoNotCare;
                else if (p++ == index && enabled == (animation.OnRoof == EntityResource.Trigger.DoNotCare)) animation.OnRoof = enabled ? 0 : EntityResource.Trigger.DoNotCare;
                else if (p++ == index && enabled == (animation.OnWall == EntityResource.Trigger.DoNotCare)) animation.OnWall = enabled ? 1 : EntityResource.Trigger.DoNotCare;
                else if (p++ == index && enabled == (animation.Direction == EntityResource.Trigger.DoNotCare)) animation.Direction = enabled ? 1 : EntityResource.Trigger.DoNotCare;
                else return;

                Story.Item = new Action(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not toggle animation parameter.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AnimationParametersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = AnimationParametersListBox.SelectedIndex;
                var animation = SelectedAnimation;
                if (animation == null || index < 0 ||
                    index >= AnimationParametersListBox.Items.Count ||
                    !AnimationParametersListBox.GetItemChecked(index))
                {
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible =
                        AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                    return;
                }

                int p = 0;
                if (p++ == index)
                {
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = true;
                    AnimationParameterNumeric.Value = (decimal)animation.VelocityXLowBound;
                }
                else if (p++ == index)
                {
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = true;
                    AnimationParameterNumeric.Value = (decimal)animation.VelocityXHighBound;
                }
                else if (p++ == index)
                {
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = true;
                    AnimationParameterNumeric.Value = (decimal)animation.VelocityYLowBound;
                }
                else if (p++ == index)
                {
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = true;
                    AnimationParameterNumeric.Value = (decimal)animation.VelocityYHighBound;
                }
                else if (p++ == index)
                {
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = true;
                    AnimationParameterNumeric.Value = (decimal)animation.AccelerationXLowBound;
                }
                else if (p++ == index)
                {
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = true;
                    AnimationParameterNumeric.Value = (decimal)animation.AccelerationXHighBound;
                }
                else if (p++ == index)
                {
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = true;
                    AnimationParameterNumeric.Value = (decimal)animation.AccelerationYLowBound;
                }
                else if (p++ == index)
                {
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = true;
                    AnimationParameterNumeric.Value = (decimal)animation.AccelerationYHighBound;
                }
                else if (p++ == index)
                {
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = false;
                    AnimationParameterDomain.Text = "";
                    AnimationParameterDomain.Items.Clear();
                    AnimationParameterDomain.Items.AddRange(BoolNames);
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = true;
                    AnimationParameterDomain.SelectedIndex = BoolNames.IndexOf(Enum.GetName(
                        typeof(EntityResource.Trigger.BoolConditional), animation.IOnGround));
                }
                else if (p++ == index)
                {
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = false;
                    AnimationParameterDomain.Text = "";
                    AnimationParameterDomain.Items.Clear();
                    AnimationParameterDomain.Items.AddRange(BoolNames);
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = true;
                    AnimationParameterDomain.SelectedIndex = BoolNames.IndexOf(Enum.GetName(
                        typeof(EntityResource.Trigger.BoolConditional), animation.IOnRoof));
                }
                else if (p++ == index)
                {
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = false;
                    AnimationParameterDomain.Text = "";
                    AnimationParameterDomain.Items.Clear();
                    AnimationParameterDomain.Items.AddRange(DirectionNames);
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = true;
                    AnimationParameterDomain.SelectedIndex = DirectionNames.IndexOf(Enum.GetName(
                        typeof(EntityResource.Trigger.DirectionConditional), animation.IOnWall));
                }
                else if (p++ == index)
                {
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible = false;
                    AnimationParameterDomain.Text = "";
                    AnimationParameterDomain.Items.Clear();
                    AnimationParameterDomain.Items.AddRange(DirectionNames);
                    AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = true;
                    AnimationParameterDomain.SelectedIndex = DirectionNames.IndexOf(Enum.GetName(
                        typeof(EntityResource.Trigger.DirectionConditional), animation.IDirection));
                }
                else
                {
                    AnimationParameterNumeric.Enabled =
                        AnimationParameterNumeric.Visible =
                        AnimationParameterDomain.Enabled =
                        AnimationParameterDomain.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select animation parameter.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AnimationParameterNumeric_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int index = AnimationParametersListBox.SelectedIndex;
                var animation = SelectedAnimation;
                if (animation == null || index < 0 ||
                    index >= AnimationParametersListBox.Items.Count ||
                    !AnimationParametersListBox.GetItemChecked(index))
                {
                    return;
                }

                int p = 0;
                if (p++ == index)
                {
                    animation.VelocityXLowBound = (double)AnimationParameterNumeric.Value;
                }
                else if (p++ == index)
                {
                    animation.VelocityXHighBound = (double)AnimationParameterNumeric.Value;
                }
                else if (p++ == index)
                {
                    animation.VelocityYLowBound = (double)AnimationParameterNumeric.Value;
                }
                else if (p++ == index)
                {
                    animation.VelocityYHighBound = (double)AnimationParameterNumeric.Value;
                }
                else if (p++ == index)
                {
                    animation.AccelerationXLowBound = (double)AnimationParameterNumeric.Value;
                }
                else if (p++ == index)
                {
                    animation.AccelerationXHighBound = (double)AnimationParameterNumeric.Value;
                }
                else if (p++ == index)
                {
                    animation.AccelerationYLowBound = (double)AnimationParameterNumeric.Value;
                }
                else if (p++ == index)
                {
                    animation.AccelerationYHighBound = (double)AnimationParameterNumeric.Value;
                }
                else if (p++ == index)
                {
                    EntityResource.Trigger.BoolConditional c;
                    if (Enum.TryParse(AnimationParameterDomain.Text, out c)) animation.IOnGround = c;
                }
                else if (p++ == index)
                {
                    EntityResource.Trigger.BoolConditional c;
                    if (Enum.TryParse(AnimationParameterDomain.Text, out c)) animation.IOnRoof = c;
                }
                else if (p++ == index)
                {
                    EntityResource.Trigger.DirectionConditional c;
                    if (Enum.TryParse(AnimationParameterDomain.Text, out c)) animation.IOnWall = c;
                }
                else if (p++ == index)
                {
                    EntityResource.Trigger.DirectionConditional c;
                    if (Enum.TryParse(AnimationParameterDomain.Text, out c)) animation.IDirection = c;
                }

                Story.Item = new Action(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not edit amination parameter.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AnimationNameTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var animation = SelectedAnimation;
                if (animation == null) return;
                if (animation.Name != AnimationNameTextBox.Text)
                {
                    animation.Name = AnimationNameTextBox.Text;
                    Story.Item = new Action(LoadedResource);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not edit animation name.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AnimationActionTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var animation = SelectedAnimation;
                if (animation != null)
                {
                    if (animation.Action != AnimationActionTextBox.Text)
                    {
                        animation.Action = AnimationActionTextBox.Text;
                        Story.Item = new Action(LoadedResource);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not edit animation action.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AnimationLinkTextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths.Length != 1 || Resource.GetType(paths[0]) != ResourceType.Animation) return;
            if (e.AllowedEffect.HasFlag(DragDropEffects.Link))
                e.Effect = DragDropEffects.Link;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }
        private void AnimationLinkTextBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (paths.Length != 1)
                    {
                        MessageBox.Show(this, "You must choose only one resource file.",
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
                    else AnimationLinkTextBox.Text = ExtraPath.MakeDirectoryRelated(dpath, fpath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AnimationLinkTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var animation = SelectedAnimation;
                if (animation == null) return;

                GLSurface.MakeCurrent();
                var link = animation.Animation.Link;
                animation.Animation.Link = AnimationLinkTextBox.Text;
                if (link != animation.Animation.Link) Story.Item = new Action(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link animation.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Animation_Reloaded(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                Story.Item = new Action(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not reload animation.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HoldersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var holder = SelectedHolder;
                if (holder == null)
                {
                    HolderNameTextBox.Text = "";
                    HolderActionTextBox.Text = "";
                    HolderNodeNumeric.Value = -1;
                    HolderAnimationLinkTextBox.Text = "";

                    HolderNameTextBox.Enabled = false;
                    HolderActionTextBox.Enabled = false;
                    HolderNodeNumeric.Enabled = false;
                    HolderAnimationLinkTextBox.Enabled = false;
                    HolderAnimationLinkTextBox.BackColor = Color.Red;
                }
                else
                {
                    HolderNameTextBox.Text = holder.Name;
                    HolderActionTextBox.Text = holder.Action;
                    HolderNodeNumeric.Value = holder.HolderPoint;
                    HolderAnimationLinkTextBox.Text = holder.Animation.Link;

                    HolderNameTextBox.Enabled = true;
                    HolderActionTextBox.Enabled = true;
                    HolderNodeNumeric.Enabled = true;
                    HolderAnimationLinkTextBox.Enabled = true;
                    HolderAnimationLinkTextBox.BackColor = holder.Animation.Loaded ? SystemColors.Window : Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select holder.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HolderNameTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var holder = SelectedHolder;
                if (holder == null) return;
                if (holder.Name != HolderNameTextBox.Text)
                {
                    holder.Name = HolderNameTextBox.Text;
                    Story.Item = new Action(LoadedResource);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not edit holder name.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HolderActionTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var holder = SelectedHolder;
                if (holder == null) return;

                if (holder.Action != HolderActionTextBox.Text)
                {
                    holder.Action = HolderActionTextBox.Text;
                    Story.Item = new Action(LoadedResource);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not edit holder action.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HolderNodeNumeric_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                var holder = SelectedHolder;
                if (holder == null) return;
                int val = (int)HolderNodeNumeric.Value;

                if (holder.HolderPoint != val)
                {
                    holder.HolderPoint = val;
                    Story.Item = new Action(LoadedResource);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not edit holder node.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HolderAnimationLinkTextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths.Length != 1 || Resource.GetType(paths[0]) != ResourceType.Animation) return;
            if (e.AllowedEffect.HasFlag(DragDropEffects.Link))
                e.Effect = DragDropEffects.Link;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }
        private void HolderAnimationLinkTextBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (paths.Length != 1)
                    {
                        MessageBox.Show(this, "You must choose only one resource file.",
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
                    else HolderAnimationLinkTextBox.Text = ExtraPath.MakeDirectoryRelated(dpath, fpath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HolderAnimationLinkTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var animation = SelectedHolder;
                if (animation == null) return;

                GLSurface.MakeCurrent();
                var link = animation.Animation.Link;
                animation.Animation.Link = HolderAnimationLinkTextBox.Text;
                if (link != animation.Animation.Link) Story.Item = new Action(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link animation.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Holder_Reloaded(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                Story.Item = new Action(LoadedResource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not reload holder.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolLinkTextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths.Length != 1 || Resource.GetType(paths[0]) != ResourceType.Tool) return;
            if (e.AllowedEffect.HasFlag(DragDropEffects.Link))
                e.Effect = DragDropEffects.Link;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }
        private void ToolLinkTextBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (paths.Length != 1)
                    {
                        MessageBox.Show(this, "You must choose only one resource file.",
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
                    else ToolLinkTextBox.Text = ExtraPath.MakeDirectoryRelated(dpath, fpath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ToolCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            EntityController.Tool.Active = ToolCheckBox.Checked;
        }
        private void ToolLinkTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GLSurface.MakeCurrent();
                EntityController.Tool.Link = ToolLinkTextBox.Text;
                ToolLinkTextBox.BackColor = EntityController.Tool.Loaded ? SystemColors.Window : Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not link animation.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ToolDelayNumeric_ValueChanged(object sender, EventArgs e)
        {
            EntityController.ToolCycle = (int)ToolDelayNumeric.Value;
        }
    }
}
