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
    class TileCompiler
    {
        public static uint[] GetTilePartPixels(Bitmap tex, int size, int px, int py)
        {
            var result = new uint[size * size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var p = tex.GetPixel(px * size + x, py * size + size - 1 - y);
                    result[y * size + x] = ((uint)p.R << 24) | ((uint)p.G << 16) | ((uint)p.B << 8) | ((uint)p.A << 0);
                }
            }

            return result;
        }
        public static uint[] GetTilePartFrames(Bitmap tex, int size, int uc, int frames, int px, int py)
        {
            var result = new uint[size * size * frames];
            for (int f = 0; f < frames; f++)
                GetTilePartPixels(tex, size, px, py + f * uc).CopyTo(result, f * size * size);
            return result;
        }
        public static uint[] GetTilePartsCompound(Bitmap tex, int size, int uc, int frames, int px, int py)
        {
            var result = new uint[size * size * frames * 4];
            GetTilePartFrames(tex, size, uc, frames, px, py).CopyTo(result, size * size * frames * 0);
            GetTilePartFrames(tex, size, uc, frames, px, uc - 1 - py).CopyTo(result, size * size * frames * 1);
            GetTilePartFrames(tex, size, uc, frames, uc - 1 - px, uc - 1 - py).CopyTo(result, size * size * frames * 2);
            GetTilePartFrames(tex, size, uc, frames, uc - 1 - px, py).CopyTo(result, size * size * frames * 3);
            return result;
        }
        public static uint[] GetTilePixels(Bitmap tex, int size, int uc, int frames)
        {
            int unit_size = size * size * frames * 4;
            if (uc == 2)
            {
                return GetTilePartsCompound(tex, size, uc, frames, 0, 0);
            }                                                        
            else if (uc == 4)
            {
                var result = new uint[unit_size * 4];
                GetTilePartsCompound(tex, size, uc, frames, 1, 1).CopyTo(result, unit_size * 0);
                GetTilePartsCompound(tex, size, uc, frames, 1, 0).CopyTo(result, unit_size * 1);
                GetTilePartsCompound(tex, size, uc, frames, 0, 1).CopyTo(result, unit_size * 2);
                GetTilePartsCompound(tex, size, uc, frames, 0, 0).CopyTo(result, unit_size * 3); 
                return result;
            }                                                        
            else if (uc == 6)
            {
                var result = new uint[unit_size * 5];
                GetTilePartsCompound(tex, size, uc, frames, 2, 2).CopyTo(result, unit_size * 0);
                GetTilePartsCompound(tex, size, uc, frames, 2, 1).CopyTo(result, unit_size * 1);
                GetTilePartsCompound(tex, size, uc, frames, 1, 2).CopyTo(result, unit_size * 2);
                GetTilePartsCompound(tex, size, uc, frames, 1, 1).CopyTo(result, unit_size * 3);
                GetTilePartsCompound(tex, size, uc, frames, 0, 0).CopyTo(result, unit_size * 4);
                return result;
            }
            else throw new Exception("Unsupported texture format.");
        }
        public struct Tile
        {
            public int SetupEventID;
            public int ReformEventID;
            public int TouchEventID;
            public int ActivateEventID;
            public int RecieveEventID;
            public int RemoveEventID;

            public int OffsetX;
            public int OffsetY;

            public uint Properties;
            public uint Form;
            public uint Anchors;
            public uint Reactions;
            public uint Light;
            public int Solidity;

            public int TextureIndex;
            public int PartSize;
            public int FrameCount;
            public int FrameDelay;
            public int Layer;
        }

        private IDTable Table;
        private MessageQueue LogQueue;

        private TextureCompiler Texture = new TextureCompiler();
        private List<Tile> Tiles = new List<Tile>();

        public TileCompiler(IDTable table, MessageQueue log_queue)
        {
            Table = table;
            LogQueue = log_queue;
        }

        public void Compile(string path, int id)
        {
            var tile = new Tile();

            LogQueue.Put("Compiling [" + path + "]...");
            TileResource res = null;
            try { res = new TileResource(path); }
            catch (Exception ex)
            {
                LogQueue.Put("Error compiling tile [" + path + "]. ID skipped." +
                    Environment.NewLine + "Error: " + ex.ToString());
                return;
            }

            tile.SetupEventID = Table[res.SetupEvent.Link];
            tile.ReformEventID = Table[res.ReformEvent.Link];
            tile.TouchEventID = Table[res.TouchEvent.Link];
            tile.ActivateEventID = Table[res.ActivateEvent.Link];
            tile.RecieveEventID = Table[res.RecieveEvent.Link];
            tile.RemoveEventID = Table[res.RemoveEvent.Link];

            if (res.SetupEvent.Link.Length > 0 && tile.SetupEventID < 0)
                LogQueue.Put("Warning: Setup event [" + res.SetupEvent.Link + "] was not found.");
            if (res.ReformEvent.Link.Length > 0 && tile.ReformEventID < 0)
                LogQueue.Put("Warning: Reform event [" + res.ReformEvent.Link + "] was not found.");
            if (res.TouchEvent.Link.Length > 0 && tile.TouchEventID < 0)
                LogQueue.Put("Warning: Touch event [" + res.TouchEvent.Link + "] was not found.");
            if (res.ActivateEvent.Link.Length > 0 && tile.ActivateEventID < 0)
                LogQueue.Put("Warning: Activate event [" + res.ActivateEvent.Link + "] was not found.");
            if (res.RecieveEvent.Link.Length > 0 && tile.RecieveEventID < 0)
                LogQueue.Put("Warning: Recieve event [" + res.RecieveEvent.Link + "] was not found.");
            if (res.RemoveEvent.Link.Length > 0 && tile.RemoveEventID < 0)
                LogQueue.Put("Warning: Remove event [" + res.RemoveEvent.Link + "] was not found.");

            tile.OffsetX = res.OffsetX;
            tile.OffsetY = res.OffsetY;

            tile.Properties = (uint)res.Properties;
            tile.Form = (uint)res.Form;
            tile.Anchors = (uint)res.Anchors;
            tile.Reactions = (uint)res.Reactions;
            tile.Light = res.Light;
            tile.Solidity = res.Solidity;

            if (!res.Texture.Loaded)
            {
                tile.TextureIndex = -1;
                LogQueue.Put("Warning: No texture for tile [" + path + "].");
            }
            else
            {
                tile.TextureIndex = Texture.FindLoad(res.Texture.Link, res.Texture.Resource,
                    (Bitmap tex) =>
                    {
                        int tw = tex.Width;
                        int th = tex.Height;

                        int ucw = tw / res.PartSize;
                        int uch = th / res.FrameCount / res.PartSize;

                        if (tex.Width % res.PartSize != 0 || tex.Height % res.FrameCount != 0 || uch != ucw ||
                            tex.Height / res.FrameCount % res.PartSize != 0 || ucw % 2 != 0 || uch % 2 != 0)
                            LogQueue.Put("Warning: Bad tile texture proporions.");
                        int uc = Math.Min(ucw, uch);

                        return GetTilePixels(tex, res.PartSize, uc, res.FrameCount);
                    });

                LogQueue.Put("Linked texture [" + res.Texture.Link + "] index [" + tile.TextureIndex + "].");
            }
            tile.PartSize = res.PartSize;
            tile.FrameCount = res.FrameCount;
            tile.FrameDelay = res.FrameDelay;
            tile.Layer = res.Layer;

            LogQueue.Put($"Tile [{path}] compiled with id [{id}].");
            res.Dispose();

            while (Tiles.Count <= id) Tiles.Add(new Tile());
            Tiles[id] = tile;
        }
        public void Write(BinaryWriter w)
        {
            Texture.Write(w);
            w.Write(Tiles.Count);
            foreach (var t in Tiles) w.Write(t);
        }
    }
}
