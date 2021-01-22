using ExtraSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources
{
    public class ToolResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Tool;
        public static readonly string CurrentVersion = "0.0.0.0";

        // Resource //
        public string ActionName { get; set; } = "";
        public float FirePointX { get; set; } = 0f;
        public float FirePointY { get; set; } = 0f;
        public float FireVectorX { get; set; } = 0f;
        public float FireVectorY { get; set; } = 0f;
        public bool AngleAttached { get; set; } = true;
        public List<bool> SpriteLockedOnCycle { get; set; } = new List<bool>();
        public List<List<Subresource<SpriteResource>>> Sprites { get; set; } = new List<List<Subresource<SpriteResource>>>();

        // Redactor //
        public Color BackColor { get; set; } = Color.Black;
        public float PointBoundsX { get; set; } = 5f;
        public float PointBoundsY { get; set; } = 4f;
        public bool PixelPerfect { get; set; } = true;
        public bool Transparency { get; set; } = false;

        public ToolResource() : base(CurrentType, CurrentVersion)
        {

        }
        public ToolResource(string path) : base(path)
        {

        }
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Sprites.ForEach((List<Subresource<SpriteResource>> l) => 
                    l.ForEach((Subresource<SpriteResource> s) => s.Dispose()));
            }

            base.Dispose(disposing);
        }

        [JsonIgnore]
        public int Count { get { return Sprites.Count; } }
        public List<Subresource<SpriteResource>> this[int i]
        {
            get { return i >= 0 && i < Sprites.Count ? Sprites[i] : null; }
            set { if (i >= 0 && i < Sprites.Count) Sprites[i] = value; }
        }

        private long CycleTimerPosition = -1;
        public void BeginCycle()
        {
            CycleTimerPosition = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        public void EndCycle()
        {
            CycleTimerPosition = -1;
        }
        public bool CheckKD(int cycle)
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds() - CycleTimerPosition > cycle;
        }
        [JsonIgnore]
        public double Progress { get => 0; }

        public void Render(float x, float y, float a, long time, int cycle, List<int> sel = null, float sx = 1f, float sy = 1f, float sa = 1f)
        {
            if (sel == null || sel.Count != Count)
            {
                sel = new List<int>(Count);
                while (sel.Count < Count) sel.Add(0);
                for (int i = 0; i < Count; i++) sel[i] = 0;
            }
            long local_time = CycleTimerPosition >= 0 ? DateTimeOffset.Now.ToUnixTimeMilliseconds() - CycleTimerPosition : 0;

            for (int i = 0; i < Count; i++)
            {
                int cur = sel[i];
                var variants = Sprites[i];
                var sprite = variants != null && cur >= 0 &&
                    cur < variants.Count ? variants[cur] : null;
                
                if (i >= 0 && i < SpriteLockedOnCycle.Count ? SpriteLockedOnCycle[i] : false) 
                    sprite?.Resource?.Render(x, y, a, (int)Math.Min(local_time * sprite.Resource.FrameCount / cycle, sprite.Resource.FrameCount - 1), sx, sy, sa);
                else sprite?.Resource?.Render(x, y, a, time, sx, sy, sa);
            }
        }
    }
}
