using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources
{
    public class EventResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Event;
        public static readonly string CurrentVersion = "0.0.0.0";

        public enum ActionType : int
        {
            None,
            PlaceTile,
            RemoveTile,
        }
        public class Action : IDisposable
        {
            public ActionType Type { get; set; } = ActionType.None;
            public WeakSubresource<TileResource> Tile { get; set; } = new WeakSubresource<TileResource>();
            public int OffsetX { get; set; } = 0;
            public int OffsetY { get; set; } = 0;

            protected bool IsDisposed { get; private set; } = false;
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected virtual void Dispose(bool disposing)
            {
                if (IsDisposed) return;

                if (disposing)
                {
                    Tile.Dispose();
                }

                IsDisposed = true;
            }
            ~Action()
            {
                Dispose(false);
            }
        }

        public ulong MinDelay { get; set; } = 0;
        public ulong MaxDelay { get; set; } = 0;

        public List<Action> Actions { get; set; } = new List<Action>();

        public int BackColor { get; set; } = Color.Black.ToArgb();
        public bool GridEnabled { get; set; } = false;

        public EventResource() : base(CurrentType, CurrentVersion)
        {
        }
        public EventResource(string path) : base(path)
        {
        }
        public override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                foreach (var action in Actions) action.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
