using ExtraSharp;
using Resource_Redactor.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor.Compiler
{
    public partial class CompilerForm : Form
    {
        private static readonly string Version = "0.0.0.0";
        private MessageQueue LogQueue = new MessageQueue();

        private string TablePath;
        private IDTable TilesIDTable = new IDTable();
        private IDTable EventsIDTable = new IDTable();
        private IDTable SpritesIDTable = new IDTable();
        private IDTable RagdollsIDTable = new IDTable();
        private IDTable AnimationsIDTable = new IDTable();
        private IDTable EntitiesIDTable = new IDTable();
        private IDTable OutfitsIDTable = new IDTable();
        private IDTable ItemsIDTable = new IDTable();

        private void CompileTiles()
        {
            LogQueue.Put("Compiling tiles...");
            var TileTexturePixels = new List<uint>();
            var TileTextureIndexes = new Dictionary<string, int>();

            int id = 0;
            Compiled.Tile[] CompiledTiles = new Compiled.Tile[TilesIDTable.LastID + 1];
            foreach (var t in TilesIDTable.Items)
            {
                int dist = id;
                while (id < t.ID) CompiledTiles[id++] = new Compiled.Tile();
                dist = id - dist;
                if (dist > 0) LogQueue.Put("IDs skipped: " + dist);

                LogQueue.Put("Compiling [" + t.Path + "]...");
                TileResource res = null;
                try { res = new TileResource(t.Path); }
                catch
                {
                    LogQueue.Put("Tile [" + t.Path + "] not found. ID skipped.");
                    CompiledTiles[id] = new Compiled.Tile();
                    id++;
                    continue;
                }

                CompiledTiles[id].SetupEventID = EventsIDTable[res.SetupEvent.Link];
                CompiledTiles[id].ReformEventID = EventsIDTable[res.ReformEvent.Link];
                CompiledTiles[id].TouchEventID = EventsIDTable[res.TouchEvent.Link];
                CompiledTiles[id].ActivateEventID = EventsIDTable[res.ActivateEvent.Link];
                CompiledTiles[id].RecieveEventID = EventsIDTable[res.RecieveEvent.Link];
                CompiledTiles[id].RemoveEventID = EventsIDTable[res.RemoveEvent.Link];

                if (res.SetupEvent.Link.Length > 0 && CompiledTiles[id].SetupEventID < 0)
                    LogQueue.Put("Warning: Setup event [" + res.SetupEvent.Link + "] not found.");
                if (res.ReformEvent.Link.Length > 0 && CompiledTiles[id].ReformEventID < 0)
                    LogQueue.Put("Warning: Reform event [" + res.ReformEvent.Link + "] not found.");
                if (res.TouchEvent.Link.Length > 0 && CompiledTiles[id].TouchEventID < 0)
                    LogQueue.Put("Warning: Touch event [" + res.TouchEvent.Link + "] not found.");
                if (res.ActivateEvent.Link.Length > 0 && CompiledTiles[id].ActivateEventID < 0)
                    LogQueue.Put("Warning: Activate event [" + res.ActivateEvent.Link + "] not found.");
                if (res.RecieveEvent.Link.Length > 0 && CompiledTiles[id].RecieveEventID < 0)
                    LogQueue.Put("Warning: Recieve event [" + res.RecieveEvent.Link + "] not found.");
                if (res.RemoveEvent.Link.Length > 0 && CompiledTiles[id].RemoveEventID < 0)
                    LogQueue.Put("Warning: Remove event [" + res.RemoveEvent.Link + "] not found.");

                CompiledTiles[id].OffsetX = res.OffsetX;
                CompiledTiles[id].OffsetY = res.OffsetY;

                CompiledTiles[id].Properties = (uint)res.Properties;
                CompiledTiles[id].Form = (uint)res.Form;
                CompiledTiles[id].Anchors = (uint)res.Anchors;
                CompiledTiles[id].Reactions = (uint)res.Reactions;
                CompiledTiles[id].Light = res.Light;
                CompiledTiles[id].Solidity = res.Solidity;

                if (!TileTextureIndexes.ContainsKey(res.Texture.Link))
                {
                    try
                    {
                        int ind = TileTexturePixels.Count;
                        var tex = res.Texture.Resource.Texture;
                        int tw = tex.Width;
                        int th = tex.Height;

                        int ucw = tw / res.PartSize;
                        int uch = th / res.FramesCount / res.PartSize;

                        if (tex.Width % res.PartSize != 0 || tex.Height % res.FramesCount != 0 || uch != ucw ||
                            tex.Height / res.FramesCount % res.PartSize != 0 || ucw % 2 != 0 || uch % 2 != 0)
                            LogQueue.Put("Warning: Bad tile texture proporions.");
                        int uc = Math.Min(ucw, uch);

                        if (uc == 2)
                        {
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 0, 0));
                        }
                        else if (uc == 4)
                        {
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 1, 1));
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 1, 0));
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 0, 1));
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 0, 0));
                        }
                        else if (uc == 6)
                        {
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 2, 2));
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 2, 1));
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 1, 2));
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 1, 1));
                            TileTexturePixels.AddRange(Compiled.GetTilePartsCompound(tex, res.PartSize, uc, res.FramesCount, 0, 0));
                        }
                        else throw new Exception("Tile texture compilation error: Unsupported texture [" + 
                            res.Texture.Link + "] format.");

                        TileTextureIndexes.Add(res.Texture.Link, ind);
                        CompiledTiles[id].TextureIndex = TileTextureIndexes[res.Texture.Link];
                        LogQueue.Put("Loaded texture [" + res.Texture.Link + "] index [" + CompiledTiles[id].TextureIndex + "].");
                    }
                    catch
                    {
                        CompiledTiles[id].TextureIndex = -1;
                        LogQueue.Put("Texture [" + res.Texture.Link + "] nod found index [" + CompiledTiles[id].TextureIndex + "].");
                    }
                }
                else
                {
                    CompiledTiles[id].TextureIndex = TileTextureIndexes[res.Texture.Link];
                    LogQueue.Put("Found texture [" + res.Texture.Link + "] index [" + CompiledTiles[id].TextureIndex + "].");
                }
                CompiledTiles[id].PartSize = res.PartSize;
                CompiledTiles[id].FramesCount = res.FramesCount;
                CompiledTiles[id].FrameDelay = res.FrameDelay;
                CompiledTiles[id].Layer = res.Layer;

                LogQueue.Put("Tile [" + t.Path + "] compiled with id [" + id + "].");
                id++;
            }
            LogQueue.Put("Tiles compiled.");

            //var bmp = new Bitmap(8, TileTexturePixels.Count / 8);
            //for (int y = 0; y < TileTexturePixels.Count / 8; y++)
            //{
            //    for (int x = 0; x < 8; x++)
            //    {
            //        uint p = TileTexturePixels[y * 8 + x];
            //        bmp.SetPixel(x, y, Color.FromArgb(((int)p >> 8) | ((int)p << 24)));
            //    }
            //}
            //bmp.Save("out.png", System.Drawing.Imaging.ImageFormat.Png);
            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Tiles")))
            {
                w.Write(TileTexturePixels.Count);
                foreach (var p in TileTexturePixels) w.Write(p);
                w.Write(CompiledTiles.Length);
                foreach (var t in CompiledTiles) w.Write(t);
            }
        }
        private void CompileEvents()
        {
            LogQueue.Put("Compiling events...");
            var TileTexturePixels = new List<uint>();
            var TileTextureIndexes = new Dictionary<string, int>();

            int id = 0;
            var CompiledEvents = new Compiled.Event[EventsIDTable.LastID + 1];
            var CompiledActions = new List<Compiled.Event.Action>();
            foreach (var e in EventsIDTable.Items)
            {
                int dist = id;
                while (id < e.ID) CompiledEvents[id++] = new Compiled.Event();
                dist = id - dist;
                if (dist > 0) LogQueue.Put("IDs skipped: " + dist);

                LogQueue.Put("Compiling [" + e.Path + "]...");
                EventResource res = null;
                try { res = new EventResource(e.Path); }
                catch
                {
                    LogQueue.Put("Event [" + e.Path + "] not found. ID skipped.");
                    CompiledEvents[id] = new Compiled.Event();
                    id++;
                    continue;
                }

                CompiledEvents[id].MinDelay = res.MinDelay;
                CompiledEvents[id].MaxDelay = res.MaxDelay;
                CompiledEvents[id].FirstAction = CompiledActions.Count;
                CompiledEvents[id].ActionsCount = res.Actions.Count;

                foreach (var a in res.Actions)
                {
                    var ca = new Compiled.Event.Action();

                    ca.LinkID = TilesIDTable[a.Link];
                    ca.Type = (int)a.Type;
                    ca.OffsetX = a.OffsetX;
                    ca.OffsetY = a.OffsetY;

                    if (ca.LinkID < 0) LogQueue.Put("Warning: Link [" + a.Link + "] not found.");

                    CompiledActions.Add(ca);
                }

                LogQueue.Put("Event [" + e.Path + "] compiled with id [" + id + "].");
                id++;
            }
            LogQueue.Put("Events compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Events")))
            {
                w.Write(CompiledEvents.Length);
                foreach (var e in CompiledEvents) w.Write(e);
                w.Write(CompiledActions.Count);
                foreach (var a in CompiledActions) w.Write(a);
            }
        }
        private void CompileSprites()
        {
            LogQueue.Put("Compiling sprites...");
            var SpriteTexturePixels = new List<uint>();
            var SpriteTextureIndexes = new Dictionary<string, int>();

            int id = 0;
            var CompiledSprites = new Compiled.Sprite[SpritesIDTable.LastID + 1];
            foreach (var s in SpritesIDTable.Items)
            {
                int dist = id;
                while (id < s.ID) CompiledSprites[id++] = new Compiled.Sprite();
                dist = id - dist;
                if (dist > 0) LogQueue.Put("IDs skipped: " + dist);

                LogQueue.Put("Compiling [" + s.Path + "]...");
                SpriteResource res = null;
                try { res = new SpriteResource(s.Path); }
                catch
                {
                    LogQueue.Put("Sprite [" + s.Path + "] not found. ID skipped.");
                    CompiledSprites[id] = new Compiled.Sprite();
                    id++;
                    continue;
                }

                CompiledSprites[id].TextureIndex = -1;
                CompiledSprites[id].FramesCount = res.FramesCount;
                CompiledSprites[id].FrameDelay = res.FrameDelay;
                CompiledSprites[id].ImgboxW = res.ImgboxW;
                CompiledSprites[id].ImgboxH = res.ImgboxH;
                CompiledSprites[id].AxisX = res.AxisX;
                CompiledSprites[id].AxisY = res.AxisY;
                CompiledSprites[id].Angle = res.Angle;

                if (!SpriteTextureIndexes.ContainsKey("~:/" + res.FramesCount + res.VerticalFrames + "\\:~" + res.Texture.Link))
                {
                    try
                    {
                        int ind = SpriteTexturePixels.Count;
                        var tex = res.Texture.Resource.Texture;
                        int tw = tex.Width;
                        int th = tex.Height;

                        var pixels = new uint[tw * th];
                        if (res.VerticalFrames)
                        {
                            if (th % res.FramesCount != 0) LogQueue.Put("Warning: Bad texture proporions.");
                            th /= res.FramesCount;

                            for (int f = 0; f < res.FramesCount; f++)
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
                            if (tw % res.FramesCount != 0) LogQueue.Put("Warning: Bad texture proporions.");
                            tw /= res.FramesCount;

                            for (int f = 0; f < res.FramesCount; f++)
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
                        SpriteTexturePixels.Add((uint)tw);
                        SpriteTexturePixels.Add((uint)th);
                        SpriteTexturePixels.AddRange(pixels);

                        SpriteTextureIndexes.Add("~:/" + res.FramesCount + res.VerticalFrames + "\\:~" + res.Texture.Link, ind);
                        CompiledSprites[id].TextureIndex = SpriteTextureIndexes["~:/" + res.FramesCount + res.VerticalFrames + "\\:~" + res.Texture.Link];
                        LogQueue.Put("Loaded texture [" + res.Texture.Link + "] index [" + CompiledSprites[id].TextureIndex + "].");
                    }
                    catch
                    {
                        CompiledSprites[id].TextureIndex = -1;
                        LogQueue.Put("Texture [" + res.Texture.Link + "] nod found index [" + CompiledSprites[id].TextureIndex + "].");
                    }
                }
                else
                {
                    CompiledSprites[id].TextureIndex = SpriteTextureIndexes["~:/" + res.FramesCount + res.VerticalFrames + "\\:~" + res.Texture.Link];
                    LogQueue.Put("Found texture [" + res.Texture.Link + "] index [" + CompiledSprites[id].TextureIndex + "].");
                }

                LogQueue.Put("Sprite [" + s.Path + "] compiled with id [" + id + "].");
                id++;
            }
            LogQueue.Put("Sprites compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Sprites")))
            {
                w.Write(SpriteTexturePixels.Count);
                foreach (var p in SpriteTexturePixels) w.Write(p);
                w.Write(CompiledSprites.Length);
                foreach (var t in CompiledSprites) w.Write(t);
            }
        }
        private void CompileRagdolls()
        {
            LogQueue.Put("Compiling ragdolls...");

            int id = 0;
            var CompiledRagdolls = new Compiled.Ragdoll[RagdollsIDTable.LastID + 1];
            var RagdollNodes = new List<Compiled.Ragdoll.Node>();
            foreach (var r in RagdollsIDTable.Items)
            {
                int dist = id;
                while (id < r.ID) CompiledRagdolls[id++] = new Compiled.Ragdoll();
                dist = id - dist;
                if (dist > 0) LogQueue.Put("IDs skipped: " + dist);

                LogQueue.Put("Compiling [" + r.Path + "]...");
                RagdollResource res = null;
                try { res = new RagdollResource(r.Path); }
                catch
                {
                    LogQueue.Put("Ragdoll [" + r.Path + "] not found. ID skipped.");
                    CompiledRagdolls[id++] = new Compiled.Ragdoll();
                    continue;
                }

                CompiledRagdolls[id].FirstNode = RagdollNodes.Count;
                CompiledRagdolls[id].NodesCount = res.Count;
                CompiledRagdolls[id].HitboxW = res.HitboxW;
                CompiledRagdolls[id].HitboxH = res.HitboxH;

                foreach (var node in res.Nodes)
                {
                    var cnode = new Compiled.Ragdoll.Node();
                    cnode.MainNode = node.MainNode;
                    cnode.OffsetX = node.OffsetX;
                    cnode.OffsetY = node.OffsetY;
                    RagdollNodes.Add(cnode);
                }

                LogQueue.Put("Ragdoll [" + r.Path + "] compiled with id [" + id + "].");
                id++;
            }
            LogQueue.Put("Ragdolls compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Ragdolls")))
            {
                w.Write(RagdollNodes.Count);
                foreach (var n in RagdollNodes) w.Write(n);
                w.Write(CompiledRagdolls.Length);
                foreach (var r in CompiledRagdolls) w.Write(r);
            }
        }
        private void CompileAnimations()
        {
            LogQueue.Put("Compiling animations...");

            int id = 0;
            var CompiledAnimations = new Compiled.Animation[AnimationsIDTable.LastID + 1];
            var AnimationNodes = new List<Compiled.Animation.Node>();
            foreach (var r in AnimationsIDTable.Items)
            {
                int dist = id;
                while (id < r.ID) CompiledAnimations[id++] = new Compiled.Animation();
                dist = id - dist;
                if (dist > 0) LogQueue.Put("IDs skipped: " + dist);

                LogQueue.Put("Compiling [" + r.Path + "]...");
                AnimationResource res = null;
                try { res = new AnimationResource(r.Path); }
                catch
                {
                    LogQueue.Put("Animation [" + r.Path + "] not found. ID skipped.");
                    CompiledAnimations[id++] = new Compiled.Animation();
                    continue;
                }

                CompiledAnimations[id].FirstNode = AnimationNodes.Count;
                CompiledAnimations[id].FramesCount = res.Count;
                CompiledAnimations[id].NodesPerFrame = res.NodesCount;
                CompiledAnimations[id].Dependency = (int)res.Dependency;
                CompiledAnimations[id].FramesPerUnitRatio = res.FramesPerUnitRatio;

                foreach (var frame in res.Frames)
                {
                    for (int n = 0; n < frame.Count; n++)
                    {
                        var node = frame[n];
                        var cnode = new Compiled.Animation.Node();
                        cnode.Properties = (int)node.Properties;
                        cnode.OffsetX = node.OffsetX;
                        cnode.OffsetY = node.OffsetY;
                        cnode.Angle = node.Angle;
                        AnimationNodes.Add(cnode);
                    }
                }

                LogQueue.Put("Animations [" + r.Path + "] compiled with id [" + id + "].");
                id++;
            }
            LogQueue.Put("Animations compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Animations")))
            {
                w.Write(AnimationNodes.Count);
                foreach (var n in AnimationNodes) w.Write(n);
                w.Write(CompiledAnimations.Length);
                foreach (var a in CompiledAnimations) w.Write(a);
            }
        }
        private void CompileEntities()
        {
            LogQueue.Put("Compiling entities...");

            int id = 0;
            var CompiledEntities = new Compiled.Entity[EntitiesIDTable.LastID + 1];
            var EntityTriggers = new List<Compiled.Entity.Trigger>();
            var EntityHolders = new List<Compiled.Entity.Holder>();
            var TriggerActionsSet = new Dictionary<string, int>();
            var HolderActionsSet = new Dictionary<string, int>();
            foreach (var e in EntitiesIDTable.Items)
            {
                int dist = id;
                while (id < e.ID) CompiledEntities[id++] = new Compiled.Entity();
                dist = id - dist;
                if (dist > 0) LogQueue.Put("IDs skipped: " + dist);

                LogQueue.Put("Compiling [" + e.Path + "]...");
                EntityResource res = null;
                try { res = new EntityResource(e.Path); }
                catch
                {
                    LogQueue.Put("Entity [" + e.Path + "] not found. ID skipped.");
                    CompiledEntities[id++] = new Compiled.Entity();
                    continue;
                }

                CompiledEntities[id].RagdollID = RagdollsIDTable[res.Ragdoll.Link];
                CompiledEntities[id].FirstTrigger = EntityTriggers.Count;
                CompiledEntities[id].TriggersCount = res.Triggers.Count;
                CompiledEntities[id].FirstHolder = EntityHolders.Count;
                CompiledEntities[id].HoldersCount = res.Holders.Count;

                CompiledEntities[id].MaxHealth = res.MaxHealth;
                CompiledEntities[id].MaxEnergy = res.MaxEnergy;
                CompiledEntities[id].Mass = res.Mass;
                CompiledEntities[id].GravityAcceleration = res.GravityAcceleration;
                CompiledEntities[id].JumpVelocity = res.JumpVelocity;
                CompiledEntities[id].DragX = res.DragX;
                CompiledEntities[id].DragY = res.DragY;
                CompiledEntities[id].SqrDragX = res.SqrDragX;
                CompiledEntities[id].SqrDragY = res.SqrDragY;
                CompiledEntities[id].MoveForceX = res.MoveForceX;
                CompiledEntities[id].MoveForceY = res.MoveForceY;

                foreach (var trigger in res.Triggers)
                {
                    var ctrigger = new Compiled.Entity.Trigger();

                    if (TriggerActionsSet.ContainsKey(trigger.Action)) ctrigger.ActionID = TriggerActionsSet[trigger.Action];
                    else TriggerActionsSet.Add(trigger.Action, ctrigger.ActionID = TriggerActionsSet.Count);

                    ctrigger.VelocityXLowBound = trigger.VelocityXLowBound;
                    ctrigger.VelocityYLowBound = trigger.VelocityYLowBound;

                    ctrigger.VelocityXHighBound = trigger.VelocityXHighBound;
                    ctrigger.VelocityYHighBound = trigger.VelocityYHighBound;

                    ctrigger.AccelerationXLowBound = trigger.AccelerationXLowBound;
                    ctrigger.AccelerationYLowBound = trigger.AccelerationYLowBound;

                    ctrigger.AccelerationXHighBound = trigger.AccelerationXHighBound;
                    ctrigger.AccelerationYHighBound = trigger.AccelerationYHighBound;

                    ctrigger.OnGround = trigger.OnGround;
                    ctrigger.OnRoof = trigger.OnRoof;
                    ctrigger.OnWall = trigger.OnWall;
                    ctrigger.Direction = trigger.Direction;

                    ctrigger.AnimationID = AnimationsIDTable[trigger.Animation.Link];

                    EntityTriggers.Add(ctrigger);
                }
                foreach (var holder in res.Holders)
                {
                    var cholder = new Compiled.Entity.Holder();

                    if (HolderActionsSet.ContainsKey(holder.Action)) cholder.ActionID = HolderActionsSet[holder.Action];
                    else HolderActionsSet.Add(holder.Action, cholder.ActionID = HolderActionsSet.Count);

                    cholder.HolderPoint = holder.HolderPoint;

                    cholder.AnimationID = AnimationsIDTable[holder.Animation.Link];

                    EntityHolders.Add(cholder);
                }

                LogQueue.Put("Entity [" + e.Path + "] compiled with id [" + id + "].");
                id++;
            }
            LogQueue.Put("Entities compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Entities")))
            {
                w.Write(EntityTriggers.Count);
                foreach (var t in EntityTriggers) w.Write(t);
                w.Write(EntityHolders.Count);
                foreach (var h in EntityHolders) w.Write(h);
                w.Write(CompiledEntities.Length);
                foreach (var e in CompiledEntities) w.Write(e);
            }
        }
        private void CompileOutfits()
        {
            LogQueue.Put("Compiling outfits...");

            int id = 0;
            var CompiledOutfits = new Compiled.Outfit[EntitiesIDTable.LastID + 1];
            var OutfitNodes = new List<Compiled.Outfit.Node>();
            foreach (var e in OutfitsIDTable.Items)
            {
                int dist = id;
                while (id < e.ID) CompiledOutfits[id++] = new Compiled.Outfit();
                dist = id - dist;
                if (dist > 0) LogQueue.Put("IDs skipped: " + dist);

                LogQueue.Put("Compiling [" + e.Path + "]...");
                OutfitResource res = null;
                try { res = new OutfitResource(e.Path); }
                catch
                {
                    LogQueue.Put("Outfit [" + e.Path + "] not found. ID skipped.");
                    CompiledOutfits[id++] = new Compiled.Outfit();
                    continue;
                }

                CompiledOutfits[id].FirstNode = OutfitNodes.Count;
                CompiledOutfits[id].NodesCount = res.Nodes.Count;

                foreach (var node in res.Nodes)
                {
                    var cnode = new Compiled.Outfit.Node();

                    cnode.SpriteID = SpritesIDTable[node.Sprite.Link];
                    cnode.RagdollNodeIndex = node.RagdollNode;
                    cnode.ClotheType = (int)node.ClotheType;

                    OutfitNodes.Add(cnode);
                }

                LogQueue.Put("Outfit [" + e.Path + "] compiled with id [" + id + "].");
                id++;
            }
            LogQueue.Put("Outfits compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Outfits")))
            {
                w.Write(OutfitNodes.Count);
                foreach (var n in OutfitNodes) w.Write(n);
                w.Write(CompiledOutfits.Length);
                foreach (var o in CompiledOutfits) w.Write(o);
            }
        }

        public CompilerForm(string table)
        {
            InitializeComponent();
            Text = "Clockwork engine resource compiler V" + Version;
            TablePath = table;

            MainSplitContainer.Panel1.Enabled = false;
            LoaderWorker.RunWorkerAsync();
        }

        private static void InitResourcesListBox(ListBox list, IDTable table)
        {
            int id = 0;
            list.BeginUpdate();
            list.Items.Clear();
            foreach (var i in table.Items)
            {
                while (id < i.ID - 1) list.Items.Add("[" + id++ + "]<:EMPTY:>");
                list.Items.Add("[" + (id = i.ID) + "][" + (i.Valid ? "V" : "I") + "][" + (i.Old ? "old" : "new") + "]" + i.Path);
            }
            list.EndUpdate();
        }
        private void LoaderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoaderWorker.ReportProgress(0);

            if (File.Exists(TablePath))
            {
                LogQueue.Put("Reading ID table...");
                using (var r = File.OpenText(TablePath))
                {
                    while (!r.EndOfStream)
                    {
                        var line = r.ReadLine();
                        switch (line)
                        {
                            case "[Tiles]": TilesIDTable.Read(r, (string path) => { return Resource.GetType(path) == ResourceType.Tile; }); break;
                            case "[Events]": EventsIDTable.Read(r, (string path) => { return Resource.GetType(path) == ResourceType.Event; }); break;
                            case "[Entities]": EntitiesIDTable.Read(r, (string path) => { return Resource.GetType(path) == ResourceType.Entity; }); break;
                            case "[Outfits]": OutfitsIDTable.Read(r, (string path) => { return Resource.GetType(path) == ResourceType.Outfit; }); break;
                            case "[Items]": ItemsIDTable.Read(r, (string path) => { return Resource.GetType(path) == ResourceType.Item; }); break;
                        }
                    }
                }
                LogQueue.Put("Tiles loaded: " + TilesIDTable.Count);
                LogQueue.Put("Tiles valid: " + TilesIDTable.Items.Count((IDTable.Item i) => { return i.Valid; }));
                LogQueue.Put("Events loaded: " + EventsIDTable.Count);
                LogQueue.Put("Events valid: " + EventsIDTable.Items.Count((IDTable.Item i) => { return i.Valid; }));
                LogQueue.Put("Entities loaded: " + EntitiesIDTable.Count);
                LogQueue.Put("Entities valid: " + EntitiesIDTable.Items.Count((IDTable.Item i) => { return i.Valid; }));
                LogQueue.Put("Outfits loaded: " + OutfitsIDTable.Count);
                LogQueue.Put("Outfits valid: " + OutfitsIDTable.Items.Count((IDTable.Item i) => { return i.Valid; }));
                LogQueue.Put("items loaded: " + ItemsIDTable.Count);
                LogQueue.Put("Items valid: " + ItemsIDTable.Items.Count((IDTable.Item i) => { return i.Valid; }));
                LogQueue.Put("Reading ID table done.");
            }
            else LogQueue.Put("ID table not found.");
            LogQueue.Put("");

            LoaderWorker.ReportProgress(5);

            LogQueue.Put("Indexing files...");
            string[] paths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; i++) paths[i] = ExtraPath.MakeDirectoryRelated(Directory.GetCurrentDirectory(), paths[i]);
            LogQueue.Put("Files found: " + paths.Length);
            LogQueue.Put("Indexing files done.");
            LogQueue.Put("");

            LoaderWorker.ReportProgress(10);

            LogQueue.Put("Searching resources...");
            int progress = 0;
            for (int i = 0; i < paths.Length; i++)
            {
                int current_progress = i * 90 / (paths.Length - 1);
                if (progress != current_progress)
                {
                    progress = current_progress;
                    LoaderWorker.ReportProgress(10 + progress);
                }    

                var path = paths[i];
                switch (Resource.GetType(path))
                {
                    case ResourceType.Tile: TilesIDTable.Add(path); break;
                    case ResourceType.Event: EventsIDTable.Add(path); break;
                    case ResourceType.Sprite: SpritesIDTable.Add(path); break;
                    case ResourceType.Ragdoll: RagdollsIDTable.Add(path); break;
                    case ResourceType.Animation: AnimationsIDTable.Add(path); break;
                    case ResourceType.Entity: EntitiesIDTable.Add(path); break;
                    case ResourceType.Outfit: OutfitsIDTable.Add(path); break;
                    case ResourceType.Item: ItemsIDTable.Add(path); break;
                }
            }
            LogQueue.Put("Tiles found: " + TilesIDTable.Count);
            LogQueue.Put("Events found: " + EventsIDTable.Count);
            LogQueue.Put("Sprites found: " + SpritesIDTable.Count);
            LogQueue.Put("Ragdolls found: " + RagdollsIDTable.Count);
            LogQueue.Put("Animations found: " + AnimationsIDTable.Count);
            LogQueue.Put("Entities found: " + EntitiesIDTable.Count);
            LogQueue.Put("Outfits found: " + OutfitsIDTable.Count);
            LogQueue.Put("Items found: " + ItemsIDTable.Count);
            LogQueue.Put("Total resources found: " + (
                TilesIDTable.Count + 
                EventsIDTable.Count + 
                SpritesIDTable.Count +
                RagdollsIDTable.Count + 
                AnimationsIDTable.Count + 
                EntitiesIDTable.Count + 
                OutfitsIDTable.Count + 
                ItemsIDTable.Count));
            LogQueue.Put("Searching resources done.");
            LogQueue.Put("");
        }
        private void LoaderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CompilerProgressBar.Value = e.ProgressPercentage;
        }
        private void LoaderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            InitResourcesListBox(TilesListBox, TilesIDTable);
            InitResourcesListBox(EventsListBox, EventsIDTable);
            InitResourcesListBox(SpritesListBox, SpritesIDTable);
            InitResourcesListBox(EntitiesListBox, EntitiesIDTable);
            InitResourcesListBox(OutfitsListBox, OutfitsIDTable);
            InitResourcesListBox(ItemsListBox, ItemsIDTable);

            CompilerProgressBar.Value = 0;
            MainSplitContainer.Panel1.Enabled = true;
        }

        private void CompilerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CompileTiles();
            LogQueue.Put("");
            CompileEvents();
            LogQueue.Put("");
            CompileSprites();
            LogQueue.Put("");
            CompileRagdolls();
            LogQueue.Put("");
            CompileAnimations();
            LogQueue.Put("");
            CompileEntities();
            LogQueue.Put("");
            CompileOutfits();
            LogQueue.Put("");

            LogQueue.Put("Updating ID table [" + TablePath + "]...");
            using (var file = File.CreateText(TablePath))
            {
                file.WriteLine("[Tiles]"); 
                TilesIDTable.Write(file);
                file.WriteLine("[Events]");
                EventsIDTable.Write(file);
                file.WriteLine("[Entities]"); 
                EntitiesIDTable.Write(file);
                file.WriteLine("[Outfits]");
                OutfitsIDTable.Write(file);
                file.WriteLine("[Items]"); 
                ItemsIDTable.Write(file);
            }
            LogQueue.Put("ID table updated.");
        }
        private void CompilerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled) LogQueue.Put("Compilation successful.");
            else LogQueue.Put("Compilation terminated with error:" + Environment.NewLine + e.Error?.ToString());
            LogQueue.Put(Environment.NewLine);
            CompileButton.Enabled = true;
        }

        private void CompileButton_Click(object sender, EventArgs e)
        {
            CompileButton.Enabled = false;
            LogQueue.Put("");
            LogQueue.Put("Starting compilation...");
            LogQueue.Put("");
            CompilerWorker.RunWorkerAsync();
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            string message;
            while ((message = LogQueue.Get()) != null) LogTextBox.AppendText(message + Environment.NewLine);
        }
    }
}
