using ExtraSharp;
using Resource_Redactor.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Compiler
{
    class SpriteCompiler
    {
        public struct Sprite
        {
            public int TextureIndex;
            public int FrameCount;
            public int FrameDelay;
            public float ImgboxW;
            public float ImgboxH;
            public float AxisX;
            public float AxisY;
            public float Angle;
        }

        private IDTable Table;
        private MessageQueue LogQueue;

        private TextureCompiler Texture = new TextureCompiler();
        private List<Sprite> Sprites = new List<Sprite>();

        public SpriteCompiler(IDTable table, MessageQueue log_queue)
        {
            Table = table;
            LogQueue = log_queue;
        }

        public void Compile(string path, int id)
        {
            var sprite = new Sprite();

            LogQueue.Put($"Compiling [{path}]...");
            SpriteResource res = null;
            try { res = new SpriteResource(path); }
            catch
            {
                LogQueue.Put($"Sprite [{path}] was not found. ID skipped.");
                return;
            }

            int w = res.Texture.Resource?.Bitmap.Width ?? 0;
            int h = res.Texture.Resource?.Bitmap.Height ?? 0;
            if (res.VerticalFrames) h /= res.FrameCount;
            else w /= res.FrameCount;

            sprite.TextureIndex = Texture.FindLoad(res.Texture.Link, (Bitmap tex) =>
            {
                int tw = tex.Width;
                int th = tex.Height;

                var pixels = new uint[tw * th];
                if (res.VerticalFrames)
                {
                    if (th % res.FrameCount != 0) LogQueue.Put("Warning: Bad texture proporions.");
                    th /= res.FrameCount;

                    for (int f = 0; f < res.FrameCount; f++)
                    {
                        for (int y = 0; y < th; y++)
                        {
                            for (int x = 0; x < tw; x++)
                            {
                                var p = tex.GetPixel(x, f * th + th - 1 - y);
                                pixels[f * tw * th + y * tw + x] = ((uint)p.R << 24) | ((uint)p.G << 16) | ((uint)p.B << 8) | ((uint)p.A << 0);
                            }
                        }
                    }
                }
                else
                {
                    if (tw % res.FrameCount != 0) LogQueue.Put("Warning: Bad texture proporions.");
                    tw /= res.FrameCount;

                    for (int f = 0; f < res.FrameCount; f++)
                    {
                        for (int y = 0; y < th; y++)
                        {
                            for (int x = 0; x < tw; x++)
                            {
                                var p = tex.GetPixel(f * tw + x, th - 1 - y);
                                pixels[f * tw * th + y * tw + x] = ((uint)p.R << 24) | ((uint)p.G << 16) | ((uint)p.B << 8) | ((uint)p.A << 0);
                            }
                        }
                    }
                }
                return pixels;
            },
            new uint[] { (uint)w, (uint)h });
            LogQueue.Put($"Linked texture [{res.Texture.Link}] index [{sprite.TextureIndex}].");
            sprite.FrameCount = res.FrameCount;
            sprite.FrameDelay = res.FrameDelay;
            sprite.ImgboxW = res.ImgboxW;
            sprite.ImgboxH = res.ImgboxH;
            sprite.AxisX = res.AxisX;
            sprite.AxisY = res.AxisY;
            sprite.Angle = res.Angle;

            LogQueue.Put($"Sprite [{path}] compiled with id [{id}].");
            res.Dispose();

            while (Sprites.Count <= id) Sprites.Add(new Sprite());
            Sprites[id] = sprite;
        }
        public void Write(BinaryWriter w)
        {
            Texture.Write(w);
            w.Write(Sprites.Count);
            foreach (var t in Sprites) w.Write(t);
        }
    }
}
