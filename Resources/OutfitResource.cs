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
    public class OutfitResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Outfit;
        public static readonly string CurrentVersion = "0.0.0.0";

        public class Node : IDisposable
        {
            public enum Clothe : int
            {
                Over,
                Under,
                Replace,
            }
            public Subresource<SpriteResource> Sprite { get; set; } = new Subresource<SpriteResource>();
            public int RagdollNode { get; set; } = -1;
            public Clothe ClotheType { get; set; } = Clothe.Over;
            public Node()
            {
            }
            public Node(string link, int node, Clothe clothe = Clothe.Over)
            {
                Sprite.Link = link;
                RagdollNode = node;
                ClotheType = clothe;
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
                    Sprite.Dispose();
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

        // Redactor //
        public Color BackColor { get; set; } = Color.Black;
        public float PointBoundsX { get; set; } = 5f;
        public float PointBoundsY { get; set; } = 4f;
        public bool PixelPerfect { get; set; } = false;
        public bool GridEnabled { get; set; } = true;
        public bool Transparency { get; set; } = true;
        public WeakSubresource<RagdollResource> Ragdoll { get; set; } = new WeakSubresource<RagdollResource>();

        public OutfitResource() : base(CurrentType, CurrentVersion)
        {

        }
        public OutfitResource(string path) : base(path)
        {

        }

        [JsonIgnore]
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

        protected override void Dispose(bool disposing)
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
