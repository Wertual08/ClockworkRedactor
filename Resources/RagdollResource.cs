using ExtraForms;
using ExtraForms.OpenGL;
using ExtraSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ResrouceRedactor.Resources
{
    public class RagdollResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Ragdoll;
        public static readonly string CurrentVersion = "0.0.0.0";

        public class Node : IDisposable
        {
            [JsonIgnore]
            public List<Subresource<SpriteResource>> SpritesBackup { get; set; } = new List<Subresource<SpriteResource>>();
            public List<Subresource<SpriteResource>> Sprites { get; set; } = new List<Subresource<SpriteResource>>();

            public float OffsetX { get; set; } = 0f;
            public float OffsetY { get; set; } = 0f;
            public int MainNode { get; set; } = -1;

            public Node()
            {
            }
            public Node(Node n)
            {
                OffsetX = n.OffsetX;
                OffsetY = n.OffsetY;
                MainNode = n.MainNode;
            }
            public Node(float x, float y, int m)
            {
                OffsetX = x;
                OffsetY = y;
                MainNode = m;
            }

            public PointF Rotate(float a)
            {
                float sn = (float)Math.Sin(a);
                float cs = (float)Math.Cos(a);
                return new PointF(OffsetX * cs - OffsetY * sn, OffsetX * sn + OffsetY * cs);
            }
            public PointF Rotate(float a, float ox, float oy)
            {
                float sn = (float)Math.Sin(a);
                float cs = (float)Math.Cos(a);
                return new PointF((OffsetX + ox) * cs - (OffsetY + oy) * sn, (OffsetX + ox) * sn + (OffsetY + oy) * cs);
            }

            [JsonIgnore]
            public int Count { get { return Sprites.Count; } }
            public Subresource<SpriteResource> this[int i]
            {
                get { return i >= 0 && i < Count ? Sprites[i] : null; }
                set { if (i >= 0 && i < Count) Sprites[i] = value; }
            }

            public void Render(float x, float y, float a, long t, float sx = 1f, float sy = 1f, float sa = 1f)
            {
                for (int i = 0; i < Sprites.Count; i++)
                    Sprites[i].Resource?.Render(x, y, a, t, sx, sy, sa);
            }

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
                    SpritesBackup.ForEach((Subresource<SpriteResource> s) => s.Dispose());
                    Sprites.ForEach((Subresource<SpriteResource> s) => s.Dispose());
                }

                IsDisposed = true;
            }
            ~Node()
            {
                Dispose(false);
            }
        }

        // Resource //
        public List<Node> Nodes { get; set; } = new List<Node>();
        [JsonIgnore]
        public int Count { get { return Nodes.Count; } }
        public double HitboxW { get; set; }
        public double HitboxH { get; set; }

        // Redactor //
        public Color BackColor { get; set; } = Color.Black;
        public float PointBoundsX { get; set; } = 5f;
        public float PointBoundsY { get; set; } = 4f;
        public bool PixelPerfect { get; set; } = true;
        public bool Transparency { get; set; } = true;


        public RagdollResource() : base(CurrentType, CurrentVersion)
        {
            BuildUpdateQueue();
        }
        public RagdollResource(string path) : base(path)
        {
        }
        public override void Open(string path)
        {
            base.Open(path);
            BuildUpdateQueue();
        }
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            if (disposing)
            {
                Nodes.ForEach((Node n) => n.Dispose());
            }
            base.Dispose(disposing);
        }

        public Node this[int i]
        {
            get { return i >= 0 && i < Count ? Nodes[i] : null; }
            set { if (i >= 0 && i < Count) Nodes[i] = value; }
        }

        public PointF GetBaseOffset(int node)
        {
            if (node < 0 || node >= Count) throw new IndexOutOfRangeException();
            var i = node;
            var n = Nodes[i];
            var point = new PointF(n.OffsetX, n.OffsetY);

            while (n.MainNode != node && n.MainNode != i && 
                n.MainNode >= 0 && n.MainNode < Count)
            {
                i = n.MainNode;
                n = Nodes[i];

                point.X += n.OffsetX;
                point.Y += n.OffsetY;
            }

            return point;
        }
        public bool ValidMainNode(int node, int main)
        {
            if (node < 0 || node >= Count || main < -1 || main >= Count)
                throw new IndexOutOfRangeException();
            if (main == -1) return true;
            if (main == node) return false;
            var c = main;
            while (c >= 0)
            {
                if (Nodes[c].MainNode == node) return false;
                c = Nodes[c].MainNode;
            }
            return true;
        }
        public PointF AdjustNodeOffset(int node, int main)
        {
            if (node < 0 || node >= Count)
                throw new IndexOutOfRangeException();

            if (main == -1 || main >= Count) return GetBaseOffset(node);

            var bp = GetBaseOffset(node);
            var mp = GetBaseOffset(main);

            return new PointF(bp.X - mp.X, bp.Y - mp.Y);
        }

        private int[] UpdateQueue = null;
        public void BuildUpdateQueue()
        {
            if (UpdateQueue == null || UpdateQueue.Length != Nodes.Count)
                UpdateQueue = new int[Nodes.Count];
            var weights = new int[Nodes.Count];
            for (int i = 0; i < UpdateQueue.Length; i++)
            {
                UpdateQueue[i] = i;
                if (Nodes[i].MainNode < 0) weights[i] = 0;
                else weights[i] = -1;
            }
            for (int i = 0; i < UpdateQueue.Length; i++)
                for (int j = 0; j < UpdateQueue.Length; j++)
                    if (Nodes[j].MainNode >= 0 && Nodes[j].MainNode < Nodes.Count && 
                        weights[Nodes[j].MainNode] == i) weights[j] = i + 1;
            for (int i = 1; i < UpdateQueue.Length; i++)
            {
                for (int j = 0; j < UpdateQueue.Length - i; j++)
                {
                    if (weights[j] > weights[j + 1])
                    {
                        var tw = weights[j];
                        var tu = UpdateQueue[j];
                        weights[j] = weights[j + 1];
                        UpdateQueue[j] = UpdateQueue[j + 1];
                        weights[j + 1] = tw;
                        UpdateQueue[j + 1] = tu; 
                    }
                }
            }
        }

        public void Clothe(OutfitResource outfit)
        {
            if (outfit == null) return;
            foreach (var node in outfit.Nodes)
            {
                if (node.RagdollNode < 0 || node.RagdollNode >= Count) continue;
                switch (node.ClotheType)
                {
                    case OutfitResource.Node.Clothe.Over: {
                            Nodes[node.RagdollNode].Sprites.Add(node.Sprite);
                        } break;
                    case OutfitResource.Node.Clothe.Replace: {
                            Nodes[node.RagdollNode].SpritesBackup.AddRange(Nodes[node.RagdollNode].Sprites);
                            Nodes[node.RagdollNode].Sprites.Clear();
                            Nodes[node.RagdollNode].Sprites.Add(node.Sprite);
                        } break;
                    case OutfitResource.Node.Clothe.Under:
                        {
                            Nodes[node.RagdollNode].Sprites.Insert(0, node.Sprite);
                        } break;
                }
            }
        }
        public void Unclothe(OutfitResource outfit)
        {
            if (outfit == null) return;
            foreach (var node in outfit.Nodes)
            {
                if (node.RagdollNode < 0 || node.RagdollNode >= Count) continue;
                switch (node.ClotheType)
                {
                    case OutfitResource.Node.Clothe.Over: {
                            Nodes[node.RagdollNode].Sprites.Remove(node.Sprite);
                        }
                        break;
                    case OutfitResource.Node.Clothe.Replace: {
                            Nodes[node.RagdollNode].Sprites.Clear();
                            Nodes[node.RagdollNode].Sprites.AddRange(Nodes[node.RagdollNode].SpritesBackup);
                            Nodes[node.RagdollNode].SpritesBackup.Clear();
                        }
                        break;
                    case OutfitResource.Node.Clothe.Under: {
                            Nodes[node.RagdollNode].Sprites.Remove(node.Sprite);
                        }
                        break;
                }
            }
        }

        public Frame MakeFrame(Frame frame = null, float sx = 1f, float sy = 1f, float sa = 1f)
        {
            if (UpdateQueue == null || UpdateQueue.Length != Nodes.Count) return null;
            if (frame == null) frame = new Frame(Nodes.Count);
            else
            {
                if (frame.Count != Nodes.Count) return null;
                frame = new Frame(frame);
            }
            for (int i = 0; i < Nodes.Count; i++)
            {
                int ui = UpdateQueue[i];
                var n = new Node(Nodes[ui]);
                var f = frame[ui];

                n.OffsetX *= sx;
                n.OffsetY *= sy;

                f.Angle *= sa;
                f.OffsetX *= sx;
                f.OffsetY *= sy;

                if (n.MainNode >= 0)
                {
                    var mf = frame[n.MainNode];
                    var o = n.Rotate(mf.Angle, f.OffsetX, f.OffsetY);
                    f.OffsetX = frame[n.MainNode].OffsetX + o.X;
                    f.OffsetY = frame[n.MainNode].OffsetY + o.Y;
                }
                else
                {
                    f.OffsetX += n.OffsetX;
                    f.OffsetY += n.OffsetY;
                }
            }
            return frame;
        }
        public void Render(Frame frame, float x, float y, long t, float sx = 1f, float sy = 1f, float sa = 1f, ToolResource tool = null, int p = -1, int c = 0, bool[] visible = null)
        {
            if (frame == null) throw new ArgumentNullException("frame");
            if (frame.Count != Nodes.Count) return;

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (visible != null && i < visible.Length && !visible[i]) continue;
                var f = frame[i];
                Nodes[i].Render(x + f.OffsetX, y + f.OffsetY, f.Angle, t, sx, sy, sa);
                if (i == p) tool?.Render(x + f.OffsetX, y + f.OffsetY, f.Angle, t, c, null, sx, sy, sa);
            }
        }
        public void Render(Frame frame, float x, float y, long t, int h, float sx = 1f, float sy = 1f, float sa = 1f, ToolResource tool = null, int p = -1, int c = 0, bool[] visible = null)
        {
            if (frame == null) throw new ArgumentNullException("frame");
            if (frame.Count != Nodes.Count) return;

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (visible != null && i < visible.Length && !visible[i]) continue;
                var f = frame[i];
                if (i == h) gl.Color4ub(255, 255, 255, 255);
                else gl.Color4ub(255, 255, 255, 150);
                Nodes[i].Render(x + f.OffsetX, y + f.OffsetY, f.Angle, t, sx, sy, sa);
                if (i == p) tool?.Render(x + f.OffsetX, y + f.OffsetY, f.Angle, t, c, null, sx, sy, sa);
            }
        }
        public void RenderInverted(Frame frame, float x, float y, long t, float sx = 1f, float sy = 1f, float sa = 1f, ToolResource tool = null, int p = -1, int c = 0, bool[] visible = null)
        {
            if (frame == null) throw new ArgumentNullException("frame");
            if (frame.Count != Nodes.Count) return;

            for (int i = Nodes.Count - 1; i >= 0; i--)
            {
                if (visible != null && i < visible.Length && !visible[i]) continue;
                var f = frame[i];
                Nodes[i].Render(x + f.OffsetX, y + f.OffsetY, f.Angle, t, sx, sy, sa);
                if (i == p) tool?.Render(x + f.OffsetX, y + f.OffsetY, f.Angle, t, c, null, sx, sy, sa);
            }
        }
        public void RenderInverted(Frame frame, float x, float y, long t, int h, float sx = 1f, float sy = 1f, float sa = 1f, ToolResource tool = null, int p = -1, int c = 0, bool[] visible = null)
        {
            if (frame == null) throw new ArgumentNullException("frame");
            if (frame.Count != Nodes.Count) return;

            for (int i = Nodes.Count - 1; i >= 0; i--)
            {
                if (visible != null && i < visible.Length && !visible[i]) continue;
                var f = frame[i];
                if (i == h) gl.Color4ub(255, 255, 255, 255);
                else gl.Color4ub(255, 255, 255, 150);
                Nodes[i].Render(x + f.OffsetX, y + f.OffsetY, f.Angle, t, sx, sy, sa);
                if (i == p) tool?.Render(x + f.OffsetX, y + f.OffsetY, f.Angle, t, c, null, sx, sy, sa);
            }
        }
    }
}
