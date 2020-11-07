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

namespace Resource_Redactor.Resources
{
    public enum TextureStretch
    {
        Clamp,
        Repeat,
    }
    public class TextureResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Texture;
        public static readonly string CurrentVersion = "0.0.0.0";

        private TextureStretch StretchStorage = TextureStretch.Clamp;
        private Bitmap BitmapStorage;
        private uint GLTexture = 0;

        // Resource //
        [JsonIgnore]
        public TextureStretch StretchMode
        {
            get => StretchStorage; 
            set
            {
                if (StretchStorage != value)
                {
                    StretchStorage = value;
                    Refresh();
                }
            }
        }
        [JsonIgnore]
        public Bitmap Bitmap
        {
            get => BitmapStorage; 
            set { BitmapStorage = value; Refresh(); }
        }
        public byte[] TextureData
        {
            get => Bitmap.ToBytes();
            set => Bitmap = BitmapStreamer.FromBytes(value);
        }

        // Redactor //
        public Color BackColor { get; set; } = Color.Black;

        public TextureResource() : base(CurrentType, CurrentVersion)
        {
        }
        public TextureResource(string path) : base(path)
        {
        }
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                BitmapStorage?.Dispose();
                BitmapStorage = null;
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

            int wrap;
            switch (StretchStorage)
            {
                case TextureStretch.Clamp: wrap = GL.CLAMP; break;
                case TextureStretch.Repeat: wrap = GL.REPEAT; break;
                default: throw new Exception("TextureResource error: Invalid TextureWrap value.");
            }

            GLTexture = fgl.BitmapToGLTexture(BitmapStorage, wrap);
        }
        public void Bind()
        {
            gl.BindTexture(GL.TEXTURE_2D, GLTexture);
        }
    }
}
