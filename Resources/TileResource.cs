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

namespace Resource_Redactor.Resources
{
    public class TileResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Tile;
        public static readonly string CurrentVersion = "0.0.0.0";

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

        public Subresource<TextureResource> Texture { get; set; } = new Subresource<TextureResource>();

        public Property Properties { get; set; } = Property.None;
        public Shape Form { get; set; } = Shape.Quad;
        public Anchor Anchors { get; set; } = Anchor.None;
        public Reaction Reactions { get; set; } = Reaction.None;
        public int Solidity { get; set; } = 0;
        public uint Light { get; set; } = 0;

        public int Layer { get; set; } = 0;
        public int PartSize { get; set; } = 8;
        public int FrameCount { get; set; } = 1;
        public int FrameDelay { get; set; } = 0;

        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;

        public Subresource<EventResource> SetupEvent { get; set; } = new Subresource<EventResource>(); 
        public Subresource<EventResource> ReformEvent { get; set; } = new Subresource<EventResource>();
        public Subresource<EventResource> TouchEvent { get; set; } = new Subresource<EventResource>(); 
        public Subresource<EventResource> ActivateEvent { get; set; } = new Subresource<EventResource>(); // TODO: Sprit to 4 events: ActivateFromTop, FromLeft...
        public Subresource<EventResource> RecieveEvent { get; set; } = new Subresource<EventResource>(); 
        public Subresource<EventResource> RemoveEvent { get; set; } = new Subresource<EventResource>();  

        public Color BackColor { get; set; } = Color.Black;
        public bool GridEnabled { get; set; } = false;

        public void Render(float x, float y, long t, TileResource[] tiles = null)
        {
            if (FrameCount <= 0) return;
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
                if (FrameDelay > 0) f = ((t / FrameDelay) % FrameCount);
                else if (FrameDelay < 0) f = ((t / FrameDelay) % FrameCount) + FrameCount - 1;

                float pw = 1.0f * PartSize / tr.Texture.Width;
                float ph = 1.0f * PartSize / tr.Texture.Height;
                float ox = 0.5f;
                float oy = 0.5f / FrameCount + f / FrameCount;

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

        public TileResource() : base(CurrentType, CurrentVersion)
        {
            // Current fps to previous
        }
        public TileResource(string path) : base(path)
        {
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Texture?.Dispose();
                SetupEvent?.Dispose();
                ReformEvent?.Dispose();
                TouchEvent?.Dispose();
                ActivateEvent?.Dispose();
                RecieveEvent?.Dispose();
                RemoveEvent?.Dispose();
            }

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
            if (FrameCount != t.FrameCount) return false;
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
