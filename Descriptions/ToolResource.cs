using ExtraSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Descriptions
{
    public class ToolResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Tool;
        public static readonly string CurrentVersion = "0.0.0.0";

        protected override void ReadData(BinaryReader r)
        {
            if (Type != CurrentType) throw new Exception(
                "Resource have wrong type [" + TypeToString(Type) + "]. [" +
                TypeToString(CurrentType) + "] required.");
            if (Version != CurrentVersion) throw new Exception(
                "Resource have wrong version [" + Version +
                "]. [" + CurrentVersion + "] required.");

            ActionName = r.ReadString();
            FirePointX = r.ReadSingle();
            FirePointY = r.ReadSingle();
            FireVectorX = r.ReadSingle();
            FireVectorY = r.ReadSingle();
            AngleAttached = r.ReadBoolean();
            SpriteLockedOnCycle = new List<bool>(r.ReadInt32());
            for (int i = 0; i < SpriteLockedOnCycle.Capacity; i++)
                SpriteLockedOnCycle.Add(r.ReadBoolean());
            Sprites = new List<List<Subresource<SpriteResource>>>(r.ReadInt32());
            for (int i = 0; i < Sprites.Capacity; i++)
            {
                var list = new List<Subresource<SpriteResource>>(r.ReadInt32());
                Sprites.Add(list);
                for (int j = 0; j < list.Capacity; j++)
                    list.Add(new Subresource<SpriteResource>(r));
            }

            BackColor = Color.FromArgb(r.ReadInt32());
            PointBounds = r.ReadStruct<PointF>();
            PixelPerfect = r.ReadBoolean();
            Transparency = r.ReadBoolean();
            SelectedSprites = new List<int>(r.ReadInt32());
            for (int i = 0; i < SelectedSprites.Capacity; i++)
                SelectedSprites.Add(r.ReadInt32());
        }
        protected override void WriteData(BinaryWriter w)
        {
            w.Write(ActionName);
            w.Write(FirePointX);
            w.Write(FirePointY);
            w.Write(FireVectorX);
            w.Write(FireVectorY);
            w.Write(AngleAttached);
            w.Write(SpriteLockedOnCycle.Count);
            for (int i = 0; i < SpriteLockedOnCycle.Count; i++)
                w.Write(SpriteLockedOnCycle[i]);
            w.Write(Sprites.Count);
            for (int i = 0; i < Sprites.Count; i++)
            {
                w.Write(Sprites[i].Count);
                for (int j = 0; j < Sprites[i].Count; j++)
                    Sprites[i][j].Write(w);
            }

            w.Write(BackColor.ToArgb());
            w.Write(PointBounds);
            w.Write(PixelPerfect);
            w.Write(Transparency);
            w.Write(SelectedSprites.Count);
            for (int i = 0; i < SelectedSprites.Count; i++)
                w.Write(SelectedSprites[i]);
        }

        // Resource //
        public string ActionName = "";
        public float FirePointX = 0f;
        public float FirePointY = 0f;
        public float FireVectorX = 0f;
        public float FireVectorY = 0f;
        public bool AngleAttached = true;
        public List<bool> SpriteLockedOnCycle = new List<bool>();
        public List<List<Subresource<SpriteResource>>> Sprites { get; private set; } = new List<List<Subresource<SpriteResource>>>();

        // Redactor //
        public Color BackColor = Color.Black;
        public PointF PointBounds = new PointF(5f, 4f);
        public bool PixelPerfect = true;
        public bool Transparency = false;
        public List<int> SelectedSprites { get; private set; } = new List<int>();

        public ToolResource() : base(CurrentType, CurrentVersion)
        {

        }
        public ToolResource(string path) : base(path)
        {

        }
        public override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Sprites.ForEach((List<Subresource<SpriteResource>> l) => l.ForEach((Subresource<SpriteResource> s) => s.Dispose()));
            }

            base.Dispose(disposing);
        }

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
        public double Progress
        {
            get
            {
                return 0;
            }
        }

        public void Render(float x, float y, float a, long time, int cycle, int[] sel = null, float sx = 1f, float sy = 1f, float sa = 1f)
        {
            if (sel == null || sel.Length != Count)
            {
                sel = new int[Count];
                for (int i = 0; i < Count; i++) sel[i] = 0;
            }
            long local_time = CycleTimerPosition >= 0 ? DateTimeOffset.Now.ToUnixTimeMilliseconds() - CycleTimerPosition : 0;

            for (int i = 0; i < Count; i++)
            {
                int cur = SelectedSprites[i];
                var variants = Sprites[i];
                var sprite = variants != null && cur >= 0 &&
                    cur < variants.Count ? variants[cur] : null;
                
                if (i >= 0 && i < SpriteLockedOnCycle.Count ? SpriteLockedOnCycle[i] : false) 
                    sprite?.Resource?.Render(x, y, a, (int)Math.Min(local_time * sprite.Resource.FramesCount / cycle, sprite.Resource.FramesCount - 1), sx, sy, sa);
                else sprite?.Resource?.Render(x, y, a, time, sx, sy, sa);
            }
        }
    }
}
