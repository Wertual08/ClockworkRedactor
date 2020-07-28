using ExtraForms;
using ExtraSharp;
using ExtraForms.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Descriptions
{
    public class SpriteResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Sprite;
        public static readonly string CurrentVersion = "0.0.0.0";

        protected override void ReadData(BinaryReader r)
        {
            if (Type != CurrentType) throw new Exception(
                "Resource have wrong type [" + TypeToString(Type) + "]. [" +
                TypeToString(CurrentType) + "] required.");
            if (Version != CurrentVersion) throw new Exception(
                "Resource have wrong version [" + Version +
                "]. [" + CurrentVersion + "] required.");
            
            FramesCount = r.ReadInt32();
            FrameDelay = r.ReadInt32();
            ImgboxW = r.ReadSingle();
            ImgboxH = r.ReadSingle();
            AxisX = r.ReadSingle();
            AxisY = r.ReadSingle();
            Angle = r.ReadSingle();
            VerticalFrames = r.ReadBoolean();

            Texture.Link = r.ReadString();

            BackColor = Color.FromArgb(r.ReadInt32());
            PointBounds = r.ReadStruct<PointF>();
            PixelPerfect = r.ReadBoolean();
        }
        protected override void WriteData(BinaryWriter w)
        {
            w.Write(FramesCount);
            w.Write(FrameDelay);
            w.Write(ImgboxW);
            w.Write(ImgboxH);
            w.Write(AxisX);
            w.Write(AxisY);
            w.Write(Angle);
            w.Write(VerticalFrames);

            w.Write(Texture.Link);

            w.Write(BackColor.ToArgb());
            w.Write(PointBounds);
            w.Write(PixelPerfect);
        }

        // Resource //
        public int FramesCount = 1;
        public int FrameDelay = 0;
        public float ImgboxW = 1f;
        public float ImgboxH = 1f;
        public float AxisX = 0f;
        public float AxisY = 0f;
        public float Angle = 0f;
        public bool VerticalFrames = false;
        public Subresource<TextureResource> Texture = new Subresource<TextureResource>();

        // Redactor //
        public Color BackColor = Color.Black;
        public PointF PointBounds = new PointF(5f, 4f);
        public bool PixelPerfect = true;

        public SpriteResource() : base(CurrentType, CurrentVersion)
        {
        }
        public SpriteResource(string path) : base(path)
        {
        }
        public override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Texture.Dispose();
            }
            
            base.Dispose(disposing);
        }

        public void AdjustImgbox()
        {
            var bmp = Texture.Resource?.Texture;
            if (bmp == null) return;

            if (VerticalFrames)
            {
                ImgboxW = bmp.Width / 16f;
                ImgboxH = bmp.Height / 16f / FramesCount;

                for (int x = 1; x < bmp.Width - 1; x++)
                {
                    for (int y = 1; y < bmp.Height / FramesCount - 1; y++)
                    {
                        if (bmp.GetPixel(x, y).A < 10 &&
                            bmp.GetPixel(x - 1, y - 1).A > 20 &&
                            bmp.GetPixel(x, y - 1).A > 20 &&
                            bmp.GetPixel(x + 1, y - 1).A > 20 &&
                            bmp.GetPixel(x + 1, y).A > 20 &&
                            bmp.GetPixel(x + 1, y + 1).A > 20 &&
                            bmp.GetPixel(x, y + 1).A > 20 &&
                            bmp.GetPixel(x - 1, y + 1).A > 20 &&
                            bmp.GetPixel(x - 1, y).A > 20)
                        {
                            AxisX = (x - bmp.Width / 2f + 0.5f) / 16f;
                            AxisY = -(y - bmp.Height / FramesCount / 2f + 0.5f) / 16f;
                            return;
                        }
                    }
                }
            }
            else
            {
                ImgboxW = bmp.Width / 16f / FramesCount;
                ImgboxH = bmp.Height / 16f;

                for (int x = 1; x < bmp.Width / FramesCount - 1; x++)
                {
                    for (int y = 1; y < bmp.Height - 1; y++)
                    {
                        if (bmp.GetPixel(x, y).A < 10 &&
                            bmp.GetPixel(x - 1, y - 1).A > 20 &&
                            bmp.GetPixel(x, y - 1).A > 20 &&
                            bmp.GetPixel(x + 1, y - 1).A > 20 &&
                            bmp.GetPixel(x + 1, y).A > 20 &&
                            bmp.GetPixel(x + 1, y + 1).A > 20 &&
                            bmp.GetPixel(x, y + 1).A > 20 &&
                            bmp.GetPixel(x - 1, y + 1).A > 20 &&
                            bmp.GetPixel(x - 1, y).A > 20)
                        {
                            AxisX = (x - bmp.Width / FramesCount / 2f + 0.5f) / 16f;
                            AxisY = -(y - bmp.Height / 2f + 0.5f) / 16f;
                            return;
                        }
                    }
                }
            }
        }
        public RectangleF GetBounds(float a = 0.0f)
        {
            float hx = AxisX;
            float hy = AxisY;
            float an = Angle + a;

            float cs = (float)Math.Cos(an);
            float sn = (float)Math.Sin(an);
            float hw = ImgboxW / 2.0f;
            float hh = ImgboxH / 2.0f;
            float xoffcosp = (hw + hx) * cs;
            float xoffcosn = (hw - hx) * cs;
            float yoffcosp = (hh + hy) * sn;
            float yoffcosn = (hh - hy) * sn;
            float xoffsinp = (hw + hx) * sn;
            float xoffsinn = (hw - hx) * sn;
            float yoffsinp = (hh + hy) * cs;
            float yoffsinn = (hh - hy) * cs;

            float x1 = Math.Min(-xoffcosp - yoffcosn, -xoffcosp + yoffcosp);
            float x2 = Math.Min(xoffcosn + yoffcosp, xoffcosn - yoffcosn);
            float x3 = Math.Max(-xoffcosp - yoffcosn, -xoffcosp + yoffcosp);
            float x4 = Math.Max(xoffcosn + yoffcosp, xoffcosn - yoffcosn);
            float lx = Math.Min(x1, x2);
            float rx = Math.Max(x3, x4);

            float y1 = Math.Min(-xoffsinp + yoffsinn, -xoffsinp - yoffsinp);
            float y2 = Math.Min(xoffsinn - yoffsinp, xoffsinn + yoffsinn);
            float y3 = Math.Max(-xoffsinp + yoffsinn, -xoffsinp - yoffsinp);
            float y4 = Math.Max(xoffsinn - yoffsinp, xoffsinn + yoffsinn);
            float dy = Math.Min(y1, y2);
            float uy = Math.Max(y3, y4);
            return new RectangleF(lx, dy, rx - lx, uy - dy);
        }

        public void Render(float x, float y, float a, long t, float sx = 1f, float sy = 1f, float sa = 1f)
        {
            if (FramesCount <= 0) return;

            float ax = AxisX * sx;
            float ay = AxisY * sy;
            float an = (Angle + a) * sa;
            float cs = (float)Math.Cos(an);
            float sn = (float)Math.Sin(an);
            float hw = ImgboxW / 2.0f * sx;
            float hh = ImgboxH / 2.0f * sy;
            float xoffcosp = (hw + ax) * cs;
            float xoffcosn = (hw - ax) * cs;
            float yoffcosp = (hh + ay) * sn;
            float yoffcosn = (hh - ay) * sn;
            float xoffsinp = (hw + ax) * sn;
            float xoffsinn = (hw - ax) * sn;
            float yoffsinp = (hh + ay) * cs;
            float yoffsinn = (hh - ay) * cs;

            float f = 0;
            if (FrameDelay > 0) f = ((t / FrameDelay) % FramesCount);
            else if (FrameDelay < 0) f = ((t / FrameDelay) % FramesCount) + FramesCount - 1;

            float tlx = f / FramesCount;
            float tdy = 0f;
            float trx = (f + 1f) / FramesCount;
            float tuy = 1f;

            if (VerticalFrames)
            {
                tlx = 0f;
                tdy = f / FramesCount;
                trx = 1f;
                tuy = (f + 1f) / FramesCount;
            }

            if (Texture.Resource == null) gl.Disable(GL.TEXTURE_2D);
            else
            {
                gl.Enable(GL.TEXTURE_2D);
                Texture.Resource.Bind();
            }
            gl.Begin(GL.QUADS);
            gl.TexCoord2f(tlx, tdy);
            gl.Vertex2d(x - xoffcosp - yoffcosn, y - xoffsinp + yoffsinn);
            gl.TexCoord2f(tlx, tuy);
            gl.Vertex2d(x - xoffcosp + yoffcosp, y - xoffsinp - yoffsinp);
            gl.TexCoord2f(trx, tuy);
            gl.Vertex2d(x + xoffcosn + yoffcosp, y + xoffsinn - yoffsinp);
            gl.TexCoord2f(trx, tdy);
            gl.Vertex2d(x + xoffcosn - yoffcosn, y + xoffsinn + yoffsinn);
            gl.End();
        }
        public void Render(float x, float y, float a, int f, float sx = 1f, float sy = 1f, float sa = 1f)
        {
            if (FramesCount <= 0) return;

            float ax = AxisX * sx;
            float ay = AxisY * sy;
            float an = (Angle + a) * sa;
            float cs = (float)Math.Cos(an);
            float sn = (float)Math.Sin(an);
            float hw = ImgboxW / 2.0f * sx;
            float hh = ImgboxH / 2.0f * sy;
            float xoffcosp = (hw + ax) * cs;
            float xoffcosn = (hw - ax) * cs;
            float yoffcosp = (hh + ay) * sn;
            float yoffcosn = (hh - ay) * sn;
            float xoffsinp = (hw + ax) * sn;
            float xoffsinn = (hw - ax) * sn;
            float yoffsinp = (hh + ay) * cs;
            float yoffsinn = (hh - ay) * cs;


            float tlx = (float)f / FramesCount;
            float tdy = 0f;
            float trx = (f + 1f) / FramesCount;
            float tuy = 1f;

            if (VerticalFrames)
            {
                tlx = 0f;
                tdy = (float)f / FramesCount;
                trx = 1f;
                tuy = (f + 1f) / FramesCount;
            }

            if (Texture.Resource == null) gl.Disable(GL.TEXTURE_2D);
            else
            {
                gl.Enable(GL.TEXTURE_2D);
                Texture.Resource.Bind();
            }
            gl.Begin(GL.QUADS);
            gl.TexCoord2f(tlx, tdy);
            gl.Vertex2f(x - xoffcosp - yoffcosn, y - xoffsinp + yoffsinn);
            gl.TexCoord2f(tlx, tuy);
            gl.Vertex2f(x - xoffcosp + yoffcosp, y - xoffsinp - yoffsinp);
            gl.TexCoord2f(trx, tuy);
            gl.Vertex2f(x + xoffcosn + yoffcosp, y + xoffsinn - yoffsinp);
            gl.TexCoord2f(trx, tdy);
            gl.Vertex2f(x + xoffcosn - yoffcosn, y + xoffsinn + yoffsinn);
            gl.End();
        }
    }
}
