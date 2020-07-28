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

namespace Resource_Redactor.Descriptions.Redactors
{
    public partial class EventControl : UserControl, IResourceControl
    {
        private struct State
        {
            public struct Action
            {
                public EventResource.ActionType Type;
                public string Link;
                public int OffsetX;
                public int OffsetY;
            }

            public ulong MinDelay;
            public ulong MaxDelay;

            public Action[] Actions;

            public Color BackColor;
            public bool GridEnabled;

            public State(EventResource r)
            {
                Actions = new Action[r.Actions.Count];

                for (int i = 0; i < r.Actions.Count; i++)
                {
                    Actions[i].Type = r.Actions[i].Type;
                    Actions[i].Link = r.Actions[i].Link;
                    Actions[i].OffsetX = r.Actions[i].OffsetX;
                    Actions[i].OffsetY = r.Actions[i].OffsetY;
                }

                MinDelay = r.MinDelay;
                MaxDelay = r.MaxDelay;

                BackColor = r.BackColor;
                GridEnabled = r.GridEnabled;
            }
            public void ToResource(EventResource r)
            {
                r.Actions.Clear();
                for (int i = 0; i < Actions.Length; i++)
                {
                    r.Actions.Add(new EventResource.Action());
                    r.Actions[i].Type = Actions[i].Type;
                    r.Actions[i].Link = Actions[i].Link;
                    r.Actions[i].OffsetX = Actions[i].OffsetX;
                    r.Actions[i].OffsetY = Actions[i].OffsetY;
                }

                r.MinDelay = MinDelay;
                r.MaxDelay = MaxDelay;

                r.BackColor = BackColor;
                r.GridEnabled = GridEnabled;
            }
            public override bool Equals(object obj)
            {
                if (!(obj is State)) return false;
                var s = (State)obj;

                if (s.Actions.Length != Actions.Length) return false;
                for (int i = 0; i < Actions.Length; i++)
                {
                    if (s.Actions[i].Type != Actions[i].Type) return false;
                    if (s.Actions[i].Link != Actions[i].Link) return false;
                    if (s.Actions[i].OffsetX != Actions[i].OffsetX) return false;
                    if (s.Actions[i].OffsetY != Actions[i].OffsetY) return false;
                }
                if (s.MinDelay != MinDelay) return false;
                if (s.MaxDelay != MaxDelay) return false;
                if (s.BackColor != BackColor) return false;
                if (s.GridEnabled != GridEnabled) return false;

                return true;
            }
        };

        private StoryItem<State> Story;
        private EventResource LoadedResource;

        public ToolStripMenuItem[] MenuTabs { get; private set; }
        public bool Saved { get; private set; } = true;
        public bool UndoEnabled { get { return Story.PrevState; } }
        public bool RedoEnabled { get { return Story.NextState; } }

        public event StateChangedEventHandler StateChanged;

        public string ResourcePath { get; private set; }
        public string ResourceName { get; private set; }

        public int FPS
        {
            get { return 1000/* / GLFrameTimer.Interval*/; }
            set { /*GLFrameTimer.Interval = Math.Max(1, 1000 / value);*/ }
        }
        public void Activate()
        {
            //GLSurface.MakeCurrent();
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

        public EventControl(string path)
        {
            InitializeComponent();

            MenuTabs = new ToolStripMenuItem[] {
            };

            LoadedResource = new EventResource(path);
            Story = new StoryItem<State>(new State(LoadedResource));
            Story.ValueChanged += Story_ValueChanged;

            ResourcePath = path;
            ResourceName = Path.GetFileName(path);
        }

        private ToolStripMenuItem GetTab(string title)
        {
            return MenuTabs.First((ToolStripMenuItem item) => { return item.Text == title; });
        }
        private void SyncNumericValue<T>(object sender, ref T value) where T : struct
        {
            var numeric = sender as NumericUpDown;
            var v = (T)Convert.ChangeType(numeric.Value, typeof(T));
            if (numeric != null && !value.Equals(v))
            {
                value = v;
                BackupChanges();
                MakeUnsaved();
            }
        }
        private void SyncFlags(object sender, ItemCheckEventArgs e, ref TileResource.Property value)
        {
            var box = sender as CheckedListBox;
            if (box != null)
            {
                bool ch = e.NewValue == CheckState.Checked;
                if (ch != (((int)value & (1 << e.Index)) != 0))
                {
                    if (ch) value |= (TileResource.Property)(1 << e.Index);
                    else value &= (TileResource.Property)(~(1 << e.Index));
                    BackupChanges();
                    MakeUnsaved();
                }
            }
        }
        private void SyncCheckBoxReaction(object sender, ref TileResource.Reaction value, TileResource.Reaction i)
        {
            var box = sender as CheckBox;
            if (box != null && value.HasFlag(i) != box.Checked)
            {
                if (box.Checked) value |= i;
                else value &= ~i;
                BackupChanges();
                MakeUnsaved();
            }
        }
        private void SyncCheckBoxAnchor(object sender, ref TileResource.Anchor value, TileResource.Anchor i)
        {
            var box = sender as CheckBox;
            if (box != null && value.HasFlag(i) != box.Checked)
            {
                if (box.Checked) value |= i;
                else value &= ~i;
                BackupChanges();
                MakeUnsaved();
            }
        }
        private void RepairOffset()
        {
            //float sw = GLSurface.SurfaceSize.Width / 2.0f;
            //float sh = GLSurface.SurfaceSize.Height / 2.0f;
            //
            //float lx = -(float)GLSurface.FieldW / 2f;
            //float rx = (float)GLSurface.FieldW / 2f;
            //float dy = -(float)GLSurface.FieldH / 2f;
            //float uy = (float)GLSurface.FieldH / 2f;
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
        private void UpdateRedactor()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        private void MakeUnsaved()
        {
            Saved = false;
            UpdateRedactor();
        }

        private void Story_ValueChanged(object sender, EventArgs e)
        {
            Story.Item.ToResource(LoadedResource);

            // TODO: Insert controls state update

            MakeUnsaved();
            UpdateRedactor();
        }
    }
}
