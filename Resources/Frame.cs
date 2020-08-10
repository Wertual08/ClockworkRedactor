using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources
{
    public enum NodeProperties : int
    {
        Empty = 0,
        Enabled = 1,
        ALI = Enabled * 2,
        OLI = ALI * 2,
        Default = Enabled | ALI | OLI,
    }
    public class Frame
    {
        private static float InterpolateAngle(float f, float s, float c)
        {
            float pi = (float)Math.PI;
            if (f - s > pi) return (1f - c) * f + c * (s + pi * 2f);
            else if (f - s < -pi) return (1f - c) * (f + pi * 2f) + c * s;
            else return (1f - c) * f + c * s;
        }
        public static PointF Rotate(PointF p, float a)
        {
            float sn = (float)Math.Sin(a);
            float cs = (float)Math.Cos(a);
            return new PointF(p.X * cs - p.Y * sn, p.X * sn + p.Y * cs);
        }
        public class Node
        {
            public NodeProperties Properties { get; set; } = NodeProperties.Default;
            public float OffsetX { get; set; } = 0f;
            public float OffsetY { get; set; } = 0f;
            public float Angle { get; set; } = 0f;

            public Node()
            {

            }
            public Node(Node n)
            {
                Properties = n.Properties;
                OffsetX = n.OffsetX;
                OffsetY = n.OffsetY;
                Angle = n.Angle;
            }
            public Node(int p, float ox, float oy, float a)
            {
                Properties = (NodeProperties)p;
                OffsetX = ox;
                OffsetY = oy;
                Angle = a;
            }
            public bool this[NodeProperties p]
            {
                get
                {
                    return Properties.HasFlag(p);
                }
                set
                {
                    if (value) Properties |= p;
                    else Properties &= ~p;
                }
            }
            public void Copy(Node n)
            {
                Properties = n.Properties;
                OffsetX = n.OffsetX;
                OffsetY = n.OffsetY;
                Angle = n.Angle;
            }

            public override bool Equals(object obj)
            {
                var n = obj as Node;
                if (n == null) return false;
                if (Properties != n.Properties) return false;
                if (OffsetX != n.OffsetX) return false;
                if (OffsetY != n.OffsetY) return false;
                if (Angle != n.Angle) return false;
                return true;
            }
        }

        public Node[] Nodes { get; set; }  = new Node[0];

        [JsonIgnore]
        public int Count { get { return Nodes.Length; } }
        public Node this[int i]
        {
            get { return i >= 0 && i < Count ? Nodes[i] : null; }
            set { if (i >= 0 && i < Count) Nodes[i] = value; }
        }

        public Frame() { }
        public Frame(Frame f)
        {
            Nodes = new Node[f.Count];
            for (int i = 0; i < f.Count; i++) Nodes[i] = new Node(f[i]);
        }
        public Frame(int size)
        {
            Nodes = new Node[size];
            for (int i = 0; i < size; i++) Nodes[i] = new Node();
        }
        public void Copy(Frame f)
        {
            if (Nodes.Length != f.Count) Nodes = new Node[f.Count];
            for (int i = 0; i < f.Count; i++) Nodes[i] = new Node(f[i]);
        }

        public override bool Equals(object obj)
        {
            var f = obj as Frame;
            if (f == null) return false;
            if (f.Count != Count) return false;
            for (int i = 0; i < Count; i++) if (!f.Nodes[i].Equals(Nodes[i])) return false;
            return true;
        }

        public static Frame Unite(Frame f, Frame s, float c)
        {
            if (f == null && s == null) return null;
            if (f == null) f = s;
            if (s == null) s = f;
            if (f.Count != s.Count) return null;
            var frame = new Frame(f.Count);
            for (int i = 0; i < frame.Count; i++)
            {
                frame[i].Properties = f[i].Properties;
                frame[i].Angle = f[i][NodeProperties.ALI] ? InterpolateAngle(f[i].Angle, s[i].Angle, c) : f[i].Angle;
                frame[i].OffsetX = f[i][NodeProperties.OLI] ? f[i].OffsetX * (1f - c) + s[i].OffsetX * c : f[i].OffsetX;
                frame[i].OffsetY = f[i][NodeProperties.OLI] ? f[i].OffsetY * (1f - c) + s[i].OffsetY * c : f[i].OffsetY;
            }
            return frame;
        }
        public Frame Insert(int index, Node node)
        {
            if (index < 0 || index > Count) return null;
            var result = new Frame(Count + 1);
            for (int i = 0; i < Count + 1; i++)
            {
                if (i < index) result[i] = Nodes[i];
                else if (i > index) result[i] = Nodes[i - 1];
                else result[i] = node;
            }
            return result;
        }
        public Frame Insert(int index)
        {
            if (index < 0 || index > Count) return null;
            var result = new Frame(Count + 1);
            for (int i = 0; i < Count + 1; i++)
            {
                if (i < index) result[i] = Nodes[i];
                else if (i > index) result[i] = Nodes[i - 1];
                else result[i] = new Node();
            }
            return result;
        }
        public Frame Remove(int index)
        {
            if (index < 0 || index >= Count) return null;
            var result = new Frame(Count - 1);
            for (int i = 0; i < Count - 1; i++)
            {
                if (i < index) result[i] = Nodes[i];
                else if (i >= index) result[i] = Nodes[i + 1];
            }
            return result;
        }
        public void Swap(int first, int second)
        {
            var t = Nodes[first];
            Nodes[first] = Nodes[second];
            Nodes[second] = t;
        }
        public Frame Rotate(float a)
        {
            var result = new Frame(this);
            for (int i = 0; i < Count; i++) result[i].Angle += a;
            return result;
        }
        public static Frame Overlap(Frame l, Frame r)
        {
            if (l == null || r == null) return null;
            if (l.Count != r.Count) return null;
            var result = new Frame(l);
            for (int i = 0; i < l.Count; i++)
                if (r[i][NodeProperties.Enabled])
                    result[i].Copy(r[i]);
            return result;
        }
        public void Overlap(Frame frame)
        {
            if (frame == null) return;
            if (frame.Count != Count) return;
            for (int i = 0; i < Count; i++) 
                if (frame[i][NodeProperties.Enabled]) 
                    Nodes[i].Copy(frame[i]);
        }
    }
}
