using ExtraForms;
using ExtraSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtraForms.OpenGL;

namespace Resource_Redactor.Descriptions
{
    class TileResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Tile;
        public static readonly string CurrentVersion = "0.0.0.0";

        protected override void ReadData(BinaryReader r)
        {
            if (Type != CurrentType) throw new Exception(
                "Resource have wrong type [" + TypeToString(Type) + "]. [" +
                TypeToString(CurrentType) + "] required.");
            if (Version != CurrentVersion) throw new Exception(
                "Resource have wrong version \"" + Version +
                "]. [" + CurrentVersion + "] required.");

            Texture.Read(r);

            Properties = (Property)r.ReadUInt32();
            Form = (Shape)r.ReadUInt32();
            Anchors = (Anchor)r.ReadUInt32();
            Reactions = (Reaction)r.ReadUInt32();
            Solidity = r.ReadInt32();
            Light = r.ReadUInt32();

            Layer = r.ReadInt32();
            PartSize = r.ReadInt32();
            FramesCount = r.ReadInt32();
            FrameDelay = r.ReadInt32();

            OffsetX = r.ReadInt32();
            OffsetY = r.ReadInt32();

            SetupEvent.Read(r);
            ReformEvent.Read(r);
            TouchEvent.Read(r);
            ActivateEvent.Read(r);
            RecieveEvent.Read(r);
            RemoveEvent.Read(r);

            BackColor = Color.FromArgb(r.ReadInt32());
            GridEnabled = r.ReadBoolean();
        }
        protected override void WriteData(BinaryWriter w)
        {
            Texture.Write(w);

            w.Write((uint)Properties);
            w.Write((uint)Form);
            w.Write((uint)Anchors);
            w.Write((uint)Reactions);
            w.Write(Solidity);
            w.Write(Light);

            w.Write(Layer);
            w.Write(PartSize);
            w.Write(FramesCount);
            w.Write(FrameDelay);

            w.Write(OffsetX);
            w.Write(OffsetY);

            SetupEvent.Write(w);
            ReformEvent.Write(w);
            TouchEvent.Write(w);
            ActivateEvent.Write(w);
            RecieveEvent.Write(w);
            RemoveEvent.Write(w);

            w.Write(BackColor.ToArgb());
            w.Write(GridEnabled);
        }

        public enum Property : uint
        {
            None = 0,
            Standalone = 1,
            EveryAnchor = Standalone * 2,
            Occupant = EveryAnchor * 2,
            Falling = Occupant * 2,
            Effect = Falling * 2,
            Effected = Effect * 2,
            Distorted = Effected * 2,
        }
        public enum Shape : uint
        {
            Empty,
            Quad,
            LDSlope,
            RDSlope,
            RUSlope,
            LUSlope,
            UPanel,
            LPanel,
            DPanel,
            RPanel,
            LDPanel,
            RDPanel,
            RUPanel,
            LUPanel,
        }
        public enum Anchor : uint
        {
            None = 0,
            Top = 1,
            Left = Top * 2,
            Down = Left * 2,
            Right = Down * 2,
            Front = Right * 2,
            Back = Front * 2,
            Every = Back * 2,
            Standalone = Every * 2,
            T = Top,
            L = Left,
            D = Down,
            R = Right,
            F = Front,
            B = Back,
        }
        public enum Reaction : uint
        {
            None = 0,
            Every = 0xffffffffu,
            T = 1,
            TL = T * 2,
            L = TL * 2,
            LD = L * 2,
            D = LD * 2,
            DR = D * 2,
            R = DR * 2,
            RT = R * 2,
            M = RT * 2
        }

        public Subresource<TextureResource> Texture = new Subresource<TextureResource>();

        public Property Properties = Property.None;
        public Shape Form = Shape.Quad;
        public Anchor Anchors = Anchor.None;
        public Reaction Reactions = Reaction.None;
        public int Solidity = 0;
        public uint Light = 0;

        public int Layer = 0;
        public int PartSize = 8;
        public int FramesCount = 1;
        public int FrameDelay = 0;

        public int OffsetX = 0;
        public int OffsetY = 0;

        public Subresource<EventResource> SetupEvent = new Subresource<EventResource>(); 
        public Subresource<EventResource> ReformEvent = new Subresource<EventResource>();
        public Subresource<EventResource> TouchEvent = new Subresource<EventResource>(); 
        public Subresource<EventResource> ActivateEvent = new Subresource<EventResource>(); // TODO: Sprit to 4 events: ActivateFromTop, FromLeft...
        public Subresource<EventResource> RecieveEvent = new Subresource<EventResource>(); 
        public Subresource<EventResource> RemoveEvent = new Subresource<EventResource>();  

        public void Render(float x, float y, long t, TileResource[] tiles = null)
        {
            if (FramesCount <= 0) return;
            float ps = PartSize / 16.0f;

            if (Texture.Resource == null)
            {
                gl.Disable(GL.TEXTURE_2D);
                gl.Begin(GL.QUADS);
                gl.Vertex2f(x - ps, y - ps);
                gl.Vertex2f(x + ps, y - ps);
                gl.Vertex2f(x + ps, y + ps);
                gl.Vertex2f(x - ps, y + ps);
                gl.End();
            }
            else
            {
                Reaction react = Reaction.Every;
                if (tiles != null && tiles.Length >= 8)
                {
                    if (tiles[0] != null && (tiles[0].Form == Shape.Quad || tiles[0].Form == Shape.LDSlope || tiles[0].Form == Shape.RDSlope) && (Equals(tiles[0]) || 
                        (Properties.HasFlag(Property.Effected) && tiles[0].Properties.HasFlag(Property.Effect)))) react &= ~Reaction.T;
                    if (tiles[1] != null && (tiles[1].Form == Shape.Quad || tiles[1].Form == Shape.RDSlope) && (Equals(tiles[1]) ||
                       (Properties.HasFlag(Property.Effected) && tiles[1].Properties.HasFlag(Property.Effect)))) react &= ~Reaction.TL;
                    if (tiles[2] != null && (tiles[2].Form == Shape.Quad || tiles[2].Form == Shape.RDSlope || tiles[2].Form == Shape.RUSlope) && (Equals(tiles[2]) ||
                        (Properties.HasFlag(Property.Effected) && tiles[2].Properties.HasFlag(Property.Effect)))) react &= ~Reaction.L;
                    if (tiles[3] != null && (tiles[3].Form == Shape.Quad || tiles[3].Form == Shape.RUSlope) && (Equals(tiles[3]) ||
                       (Properties.HasFlag(Property.Effected) && tiles[3].Properties.HasFlag(Property.Effect)))) react &= ~Reaction.LD;
                    if (tiles[4] != null && (tiles[4].Form == Shape.Quad || tiles[4].Form == Shape.LUSlope || tiles[4].Form == Shape.RUSlope) && (Equals(tiles[4]) ||
                        (Properties.HasFlag(Property.Effected) && tiles[4].Properties.HasFlag(Property.Effect)))) react &= ~Reaction.D;
                    if (tiles[5] != null && (tiles[5].Form == Shape.Quad || tiles[5].Form == Shape.LUSlope) && (Equals(tiles[5]) ||
                       (Properties.HasFlag(Property.Effected) && tiles[5].Properties.HasFlag(Property.Effect)))) react &= ~Reaction.DR;
                    if (tiles[6] != null && (tiles[6].Form == Shape.Quad || tiles[6].Form == Shape.LUSlope || tiles[6].Form == Shape.LDSlope) && (Equals(tiles[6]) ||
                        (Properties.HasFlag(Property.Effected) && tiles[6].Properties.HasFlag(Property.Effect)))) react &= ~Reaction.R;
                    if (tiles[7] != null && (tiles[7].Form == Shape.Quad || tiles[7].Form == Shape.LDSlope) && (Equals(tiles[7]) ||
                       (Properties.HasFlag(Property.Effected) && tiles[7].Properties.HasFlag(Property.Effect)))) react &= ~Reaction.RT;
                }

                react &= Reactions;

                var tr = Texture.Resource;
                float f = 0;
                if (FrameDelay > 0) f = ((t / FrameDelay) % FramesCount);
                else if (FrameDelay < 0) f = ((t / FrameDelay) % FramesCount) + FramesCount - 1;

                float pw = 1.0f * PartSize / tr.Texture.Width;
                float ph = 1.0f * PartSize / tr.Texture.Height;
                float ox = 0.5f;
                float oy = 0.5f / FramesCount + f / FramesCount;

                int rux = react.HasFlag(Reaction.RT) && !react.HasFlag(Reaction.R) && !react.HasFlag(Reaction.T) ? 2 : react.HasFlag(Reaction.R) ? 1 : 0;
                int ruy = react.HasFlag(Reaction.RT) && !react.HasFlag(Reaction.R) && !react.HasFlag(Reaction.T) ? 2 : react.HasFlag(Reaction.T) ? 1 : 0;

                int lux = react.HasFlag(Reaction.TL) && !react.HasFlag(Reaction.T) && !react.HasFlag(Reaction.L) ? 2 : react.HasFlag(Reaction.L) ? 1 : 0;
                int luy = react.HasFlag(Reaction.TL) && !react.HasFlag(Reaction.T) && !react.HasFlag(Reaction.L) ? 2 : react.HasFlag(Reaction.T) ? 1 : 0;

                int ldx = react.HasFlag(Reaction.LD) && !react.HasFlag(Reaction.L) && !react.HasFlag(Reaction.D) ? 2 : react.HasFlag(Reaction.L) ? 1 : 0;
                int ldy = react.HasFlag(Reaction.LD) && !react.HasFlag(Reaction.L) && !react.HasFlag(Reaction.D) ? 2 : react.HasFlag(Reaction.D) ? 1 : 0;

                int rdx = react.HasFlag(Reaction.DR) && !react.HasFlag(Reaction.D) && !react.HasFlag(Reaction.R) ? 2 : react.HasFlag(Reaction.R) ? 1 : 0;
                int rdy = react.HasFlag(Reaction.DR) && !react.HasFlag(Reaction.D) && !react.HasFlag(Reaction.R) ? 2 : react.HasFlag(Reaction.D) ? 1 : 0;

                gl.Enable(GL.TEXTURE_2D);
                tr.Bind();
                gl.Begin(GL.QUADS);

                gl.TexCoord2f(ox + rux * pw, oy - ruy * ph);
                gl.Vertex2d(x, y);
                gl.TexCoord2f(ox + rux * pw, oy - (ruy + 1) * ph);
                gl.Vertex2d(x, y + ps);
                gl.TexCoord2f(ox + (rux + 1) * pw, oy - (ruy + 1) * ph);
                gl.Vertex2d(x + ps, y + ps);
                gl.TexCoord2f(ox + (rux + 1) * pw, oy - ruy * ph);
                gl.Vertex2d(x + ps, y);

                gl.TexCoord2f(ox - lux * pw, oy - luy * ph);
                gl.Vertex2d(x, y);
                gl.TexCoord2f(ox - lux * pw, oy - (luy + 1) * ph);
                gl.Vertex2d(x, y + ps);
                gl.TexCoord2f(ox - (lux + 1) * pw, oy - (luy + 1) * ph);
                gl.Vertex2d(x - ps, y + ps);
                gl.TexCoord2f(ox - (lux + 1) * pw, oy - luy * ph);
                gl.Vertex2d(x - ps, y);

                gl.TexCoord2f(ox - ldx * pw, oy + ldy * ph);
                gl.Vertex2d(x, y);
                gl.TexCoord2f(ox - ldx * pw, oy + (ldy + 1) * ph);
                gl.Vertex2d(x, y - ps);
                gl.TexCoord2f(ox - (ldx + 1) * pw, oy + (ldy + 1) * ph);
                gl.Vertex2d(x - ps, y - ps);
                gl.TexCoord2f(ox - (ldx + 1) * pw, oy + ldy * ph);
                gl.Vertex2d(x - ps, y);

                gl.TexCoord2f(ox + rdx * pw, oy + rdy * ph);
                gl.Vertex2d(x, y);
                gl.TexCoord2f(ox + rdx * pw, oy + (rdy + 1) * ph);
                gl.Vertex2d(x, y - ps);
                gl.TexCoord2f(ox + (rdx + 1) * pw, oy + (rdy + 1) * ph);
                gl.Vertex2d(x + ps, y - ps);
                gl.TexCoord2f(ox + (rdx + 1) * pw, oy + rdy * ph);
                gl.Vertex2d(x + ps, y);

                gl.End();
            }

        }

        public Color BackColor = Color.Black;
        public bool GridEnabled = false;

        public TileResource() : base(CurrentType, CurrentVersion)
        {
            // Current fps to previous
        }
        public TileResource(string path) : base(path)
        {
        }
        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public bool this[Property p]
        { 
            get => Properties.HasFlag(p);
            set
            {
                if (value) Properties |= p;
                else Properties &= ~p;
            }
        }
        public bool this[Anchor a]
        {
            get => Anchors.HasFlag(a);
            set
            {
                if (value) Anchors |= a;
                else Anchors &= ~a;
            }
        }
        public bool this[Reaction r]
        {
            get => Reactions.HasFlag(r);
            set
            {
                if (value) Reactions |= r;
                else Reactions &= ~r;
            }
        }
        public bool this[int i]
        {
            get => Properties.HasFlag((Property)(1 << i));
            set
            {
                if (value) Properties |= (Property)(1 << i);
                else Properties &= (Property)(~(1 << i));
            }
        }

        public override bool Equals(object obj)
        {
            var t = obj as TileResource;
            if (t == null) return false;
            if (t == this) return true;
            if (Texture.Link != t.Texture.Link) return false;

            if (Properties != t.Properties) return false;
            if (Form != t.Form) return false;
            if (Anchors != t.Anchors) return false;
            if (Reactions != t.Reactions) return false;
            if (Solidity != t.Solidity) return false;
            if (Light != t.Light) return false;

            if (Layer != t.Layer) return false;
            if (PartSize != t.PartSize) return false;
            if (FramesCount != t.FramesCount) return false;
            if (FrameDelay != t.FrameDelay) return false;

            if (OffsetX != t.OffsetX) return false;
            if (OffsetY != t.OffsetY) return false;

            if (SetupEvent.Link != t.SetupEvent.Link) return false; 
            if (ReformEvent.Link != t.ReformEvent.Link) return false;
            if (TouchEvent.Link != t.TouchEvent.Link) return false; 
            if (ActivateEvent.Link != t.ActivateEvent.Link) return false;
            if (RecieveEvent.Link != t.RecieveEvent.Link) return false; 
            if (RemoveEvent.Link != t.RemoveEvent.Link) return false; 

            return true;
        }
    }
}
