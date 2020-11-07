using ExtraForms.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources.Interface
{
    public enum InterfaceTextureStretchType
    {
        Repeat,
        Scale,
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class InterfaceImage : IDisposable
    {
        private Subresource<TextureResource> TextureStorage = new Subresource<TextureResource>();

        public Color Color { get; set; } = Color.White;
        [Editor(typeof(UISubresourceEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(SubresourceConverter<Subresource<TextureResource>>))]
        public Subresource<TextureResource> Texture
        {
            get => TextureStorage;
            set { TextureStorage?.Dispose(); TextureStorage = value; }
        }
        public int FrameCount { get; set; } = 1;
        public int FrameDelay { get; set; } = 1000;
        public bool VerticalFrames { get; set; } = false;
        public InterfaceTextureStretchType Stretch { get; set; } = InterfaceTextureStretchType.Repeat;

        protected bool IsDisposed { get; private set; } = false;
        ~InterfaceImage()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) TextureStorage.Dispose();

            IsDisposed = true;
        }

        public override string ToString()
        {
            return TextureStorage.Link;
        }

        public void Render(int px, int py, int ex, int ey, long time)
        {
            var texture = Texture.Resource;
            if (texture != null)
            {
                float tx = 0;
                float ty = 0;
                float tw = 1;
                float th = 1;
                int iw = texture.Bitmap.Width;
                int ih = texture.Bitmap.Height;
                int frame = (int)(time / FrameDelay % FrameCount);

                if (VerticalFrames)
                {
                    ty = frame;
                    th = 1f / FrameCount;
                    ih /= FrameCount;
                }
                else
                {
                    tx = frame;
                    tw = 1f / FrameCount;
                    iw /= FrameCount;
                }

                gl.Enable(GL.TEXTURE_2D);
                texture.Bind();

                gl.Color4ub(Color.R, Color.G, Color.B, Color.A);
                gl.Begin(GL.QUADS);

                switch (Stretch)
                {
                    case InterfaceTextureStretchType.Repeat:
                        for (int x = 0; x < (ex + iw - 1) / iw; x++)
                        {
                            for (int y = 0; y < (ey + ih - 1) / ih; y++)
                            {
                                int qx = px + x * iw;
                                int qy = py + y * ih;
                                int qw = Math.Min(ex - x * iw, iw);
                                int qh = Math.Min(ey - y * ih, ih);

                                gl.TexCoord2f(tx * tw, (ty + 1) * th);
                                gl.Vertex2i(qx, qy);
                                gl.TexCoord2f((tx + (float)qw / iw) * tw, (ty + 1) * th);
                                gl.Vertex2i(qx + qw, qy);
                                gl.TexCoord2f((tx + (float)qw / iw) * tw, (ty + 1 - (float)qh / ih) * th);
                                gl.Vertex2i(qx + qw, qy + qh);
                                gl.TexCoord2f(tx * tw, (ty + 1 - (float)qh / ih) * th);
                                gl.Vertex2i(qx, qy + qh);
                            }
                        }
                        break;

                    case InterfaceTextureStretchType.Scale:
                        gl.TexCoord2f(tx * tw, (ty + 1) * th);
                        gl.Vertex2i(px, py);
                        gl.TexCoord2f((tx + 1) * tw, (ty + 1) * th);
                        gl.Vertex2i(px + ex, py);
                        gl.TexCoord2f((tx + 1) * tw, ty * th);
                        gl.Vertex2i(px + ex, py + ey);
                        gl.TexCoord2f(tx * tw, ty * th);
                        gl.Vertex2i(px, py + ey);
                        break;
                }

                gl.End();
            }
            else
            {
                gl.Disable(GL.TEXTURE_2D);
                gl.Color4ub(Color.R, Color.G, Color.B, Color.A);
                gl.Begin(GL.QUADS);
                gl.Vertex2i(px, py);
                gl.Vertex2i(px + ex, py);
                gl.Vertex2i(px + ex, py + ey);
                gl.Vertex2i(px, py + ey);
                gl.End();
            }
        }
    }
}
