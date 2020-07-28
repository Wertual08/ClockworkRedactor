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
    public class OutfitResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Outfit;
        public static readonly string CurrentVersion = "0.0.0.0";

        public class Node
        {
            public enum Clothe : int
            {
                Over,
                Under,
                Replace,
            }
            public Subresource<SpriteResource> Sprite;
            public int RagdollNode;
            public Clothe ClotheType;
            public Node()
            {
                Sprite = new Subresource<SpriteResource>();
                RagdollNode = -1;
                ClotheType = Clothe.Over;
            }
            public Node(BinaryReader r)
            {
                Sprite = new Subresource<SpriteResource>(r);
                RagdollNode = r.ReadInt32();
                ClotheType = (Clothe)r.ReadInt32();
            }
            public void Write(BinaryWriter w)
            {
                Sprite.Write(w);
                w.Write(RagdollNode);
                w.Write((int)ClotheType);
            }
        }

        protected override void ReadData(BinaryReader r)
        {
            if (Type != CurrentType) throw new Exception(
                "Resource have wrong type [" + TypeToString(Type) + "]. [" +
                TypeToString(CurrentType) + "] required.");
            if (Version != CurrentVersion) throw new Exception(
                "Resource have wrong version [" + Version +
                "]. [" + CurrentVersion + "] required.");

            Nodes = new List<Node>(r.ReadInt32());
            for (int i = 0; i < Nodes.Capacity; i++) Nodes.Add(new Node(r));


            BackColor = Color.FromArgb(r.ReadInt32());
            PointBounds = r.ReadStruct<PointF>();
            PixelPerfect = r.ReadBoolean();
            GridEnabled = r.ReadBoolean();
            Transparency = r.ReadBoolean();
            Ragdoll.Read(r);
        }
        protected override void WriteData(BinaryWriter w)
        {
            w.Write(Nodes.Count);
            foreach (var n in Nodes) n.Write(w);

            w.Write(BackColor.ToArgb());
            w.Write(PointBounds);
            w.Write(PixelPerfect);
            w.Write(GridEnabled);
            w.Write(Transparency);
            Ragdoll.Write(w);
        }

        // Resource //
        public List<Node> Nodes = new List<Node>();

        // Redactor //
        public Color BackColor = Color.Black;
        public PointF PointBounds = new PointF(5f, 4f);
        public bool PixelPerfect = false;
        public bool GridEnabled = true;
        public bool Transparency = true;
        public WeakSubresource<RagdollResource> Ragdoll { get; private set; } = new WeakSubresource<RagdollResource>();

        public OutfitResource() : base(CurrentType, CurrentVersion)
        {

        }
        public OutfitResource(string path) : base(path)
        {

        }

        public int Count 
        { 
            get { return Nodes.Count; } 
            set
            {
                while (Nodes.Count < value) Nodes.Add(new Node());
                while (Nodes.Count > value) Nodes.RemoveAt(Nodes.Count - 1);
            }
        }
        public Node this[int i]
        {
            get { return Nodes[i]; }
            set { Nodes[i] = value; }
        }

        public void Swap(int first, int second)
        {
            var t = Nodes[first];
            Nodes[first] = Nodes[second];
            Nodes[second] = t;
        }

        public override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                foreach (var node in Nodes) node.Sprite.Dispose();
                Ragdoll.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
