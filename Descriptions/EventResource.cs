using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Descriptions
{
    class EventResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Event;
        public static readonly string CurrentVersion = "0.0.0.0";

        protected override void ReadData(BinaryReader r)
        {
            if (Type != CurrentType) throw new Exception(
                "Resource have wrong type [" + TypeToString(Type) + "]. [" +
                TypeToString(CurrentType) + "] required.");
            if (Version != CurrentVersion) throw new Exception(
                "Resource have wrong version \"" + Version +
                "]. [" + CurrentVersion + "] required.");



            BackColor = r.ReadInt32();
            GridEnabled = r.ReadBoolean();
        }
        protected override void WriteData(BinaryWriter w)
        {



            w.Write(BackColor);
            w.Write(GridEnabled);
        }

        public enum ActionType : int
        {
            PlaceTile,
            RemoveTile,
        }
        public class Action
        {
            public ActionType Type { get; set; }
            public string Link { get; set; }
            public int OffsetX { get; set; }
            public int OffsetY { get; set; }
        }

        public ulong MinDelay { get; set; } = 0;
        public ulong MaxDelay { get; set; } = 0;

        public List<Action> Actions { get; set; } = new List<Action>();

        public int BackColor { get; set; } = Color.Black.ToArgb();
        public bool GridEnabled { get; set; } = false;

        public EventResource() : base(CurrentType, CurrentVersion)
        {
            // Current fps to previous
        }
        public EventResource(string path) : base(path)
        {
        }
        public override void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            base.Dispose(disposing);
        }
    }
}
