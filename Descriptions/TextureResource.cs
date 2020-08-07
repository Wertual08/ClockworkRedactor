using ExtraForms;
using ExtraSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExtraForms.OpenGL;
using System.Text.Json.Serialization;

namespace Resource_Redactor.Descriptions
{
    public class TextureResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Texture;
        public static readonly string CurrentVersion = "0.0.0.0";

        private Bitmap TextureStorage;
        private uint GLTexture = 0;

        // Resource //
        [JsonIgnore]
        public Bitmap Texture
        {
            get { return TextureStorage; }
            set { TextureStorage = value; Refresh(); }
        }
        public byte[] TextureData
        {
            get => Texture.ToBytes();
            set => Texture = BitmapStreamer.FromBytes(value);
        }

        // Redactor //
        public int BackColor { get; set; } = Color.Black.ToArgb();

        public TextureResource() : base(CurrentType, CurrentVersion)
        {
        }
        public TextureResource(string path) : base(path)
        {
        }
        public override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                TextureStorage?.Dispose();
                TextureStorage = null;
            }
#if DEBUG
            else if (GLTexture != 0) throw new Exception("TextureResource memory leak: TextureResource [" + GLTexture + "] must be explicitly disposed.");
#endif

            if (GLTexture != 0)
            {
                gl.DeleteTexture(GLTexture);
                uint error = gl.GetError();
                if (error != 0) throw new Exception("TextureResource error: Can not delete OpenGL texture [" + GLTexture + "].");
                GLTexture = 0;
            }

            base.Dispose(disposing);
        }

        public void Refresh()
        {
            if (GLTexture != 0) gl.DeleteTexture(GLTexture);
            GLTexture = fgl.BitmapToGLTexture(TextureStorage);
        }
        public void Bind()
        {
            gl.BindTexture(GL.TEXTURE_2D, GLTexture);
        }
    }
}
