using ExtraForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Descriptions
{
    public enum ResourceType : int
    { 
        NotResource = -1,
        MissingFile,
        Unknown,
        Outdated,
        Folder,
        Texture,
        Sound,
        Sprite,
        Ragdoll,
        Animation,
        Tile,
        Event,
        Entity,
        Outfit,
        Tool,
        Item,
        Particle,
    }
    public enum ResourceIcon : int
    {
        Invalid,
        Warning,
        Outdated,
        PFolder,
        Folder,
        Texture,
        Sound,
        Tile,
        Event,
        Sprite,
        Entity,
        Ragdoll,
        Animation,
        Outfit,
        Tool,
        Item,
        Particle,
    }
    public static class ResourceTypeExt
    {
        public static bool Valid(this ResourceType type)
        {
            return type >= ResourceType.Folder;
        }
        public static bool ValidBinary(this ResourceType type)
        {
            return type > ResourceType.Folder;
        }
    }

    public class ResourceEventArgs : EventArgs
    {
    }

    static class ResourceSignature
    {
        private static readonly string Stamp = "CLOCKWORK_ENGINE_REDACTOR_RESOURCE";
        public static string FileTimeStamp { get { return DateTime.Now.ToString("[yyyy-MM-dd HH-mm-ss]"); } }
        public static string TimeStamp { get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); } }
        public static void Write(BinaryWriter w)
        {
            w.Write(0x00f00fff);
            w.Write(Stamp.ToCharArray());
            w.Write(0xff0ff000);
        }
        public static bool Read(BinaryReader r)
        {
            if (r == null) return false;
            var astamp = Stamp.ToCharArray();
            long length = astamp.Length + 8L + 4L;
            if (r.BaseStream.Length - r.BaseStream.Position < length) return false;
            if (r.ReadUInt32() != 0x00f00fff) return false;
            foreach (var c in astamp) if (c != r.ReadChar()) return false;
            if (r.ReadUInt32() != 0xff0ff000) return false;
            return true;
        }
    }
    public class ResourceID : IComparable
    {
        private static Random Generator = new Random();
        private long TimePart;
        private long RandPart;

        public override bool Equals(object obj)
        {
            var res = obj as ResourceID;
            return res != null && TimePart == res.TimePart && RandPart == res.RandPart;
        }
        public int CompareTo(object obj)
        {
            if (!(obj is ResourceID)) throw new Exception("ResourceID error: ResourceID can be compared only with itself.");
            var res = obj as ResourceID;

            if (TimePart < res.TimePart) return 1;
            if (TimePart > res.TimePart) return -1;
            if (RandPart < res.RandPart) return 1;
            if (RandPart > res.RandPart) return -1;
            return 0;
        }

        private ResourceID(long time_part, long rand_part)
        {
            TimePart = time_part;
            RandPart = rand_part;
        }
        public ResourceID()
        {
            TimePart = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            RandPart = ((long)Generator.Next(int.MinValue, int.MaxValue) << 32) | (long)Generator.Next(int.MinValue, int.MaxValue);
        }
        public ResourceID(BinaryReader reader)
        {
            TimePart = reader.ReadInt64();
            RandPart = reader.ReadInt64();
        }
        public void Write(BinaryWriter writer)
        {
            writer.Write(TimePart);
            writer.Write(RandPart);
        }
    }
    public class ResourceLink
    {
        private string FilePath;
        private ResourceType Type;
        private ResourceID ID;

        public ResourceLink()
        {
            FilePath = null;
            Type = ResourceType.MissingFile;
            ID = null;
        }
        public ResourceLink(string path)
        {
            FilePath = path;
            Type = Resource.GetType(path);
            ID = Resource.GetID(path);
        }
        public ResourceLink(BinaryReader reader)
        {
            FilePath = reader.ReadString();
            Type = Resource.StringToType(reader.ReadString());
        }
    }
    public abstract class Resource : IDisposable
    {
        private static string CurrentVersion(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Texture: return TextureResource.CurrentVersion;
                case ResourceType.Sprite: return SpriteResource.CurrentVersion;
                case ResourceType.Ragdoll: return RagdollResource.CurrentVersion;
                case ResourceType.Animation: return AnimationResource.CurrentVersion;
                case ResourceType.Tool: return ToolResource.CurrentVersion;
                case ResourceType.Entity: return EntityResource.CurrentVersion;
                case ResourceType.Tile: return TileResource.CurrentVersion;
                case ResourceType.Event: return EventResource.CurrentVersion;
                case ResourceType.Outfit: return OutfitResource.CurrentVersion;
                default: return "_._._._";
            }
        }
        public static string TypeToString(ResourceType type)
        {
            return Enum.GetName(typeof(ResourceType), type);
        }
        public static ResourceType StringToType(string type)
        {
            ResourceType t;
            if (Enum.TryParse(type, out t)) return t;
            else return ResourceType.Unknown;
        }
        public static int TypeToIcon(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Outdated: return (int)ResourceIcon.Outdated;
                case ResourceType.Folder: return (int)ResourceIcon.Folder;
                case ResourceType.Texture: return (int)ResourceIcon.Texture;
                case ResourceType.Sound: return (int)ResourceIcon.Sound;
                case ResourceType.Tile: return (int)ResourceIcon.Tile;
                case ResourceType.Event: return (int)ResourceIcon.Event;
                case ResourceType.Sprite: return (int)ResourceIcon.Sprite;
                case ResourceType.Entity: return (int)ResourceIcon.Entity;
                case ResourceType.Ragdoll: return (int)ResourceIcon.Ragdoll;
                case ResourceType.Animation: return (int)ResourceIcon.Animation;
                case ResourceType.Outfit: return (int)ResourceIcon.Outfit;
                case ResourceType.Tool: return (int)ResourceIcon.Tool;
                case ResourceType.Item: return (int)ResourceIcon.Item;
                case ResourceType.Particle: return (int)ResourceIcon.Particle;
                default: return (int)ResourceIcon.Invalid;
            }
        }

        public static bool PrimalFile(string file)
        {
            try
            {
                var ext = Path.GetExtension(file).ToLower();
                switch (ext)
                {
                    case ".bmp": return true;
                    case ".gif": return true;
                    case ".exif": return true;
                    case ".jpg": return true;
                    case ".jpeg": return true;
                    case ".png": return true;
                    case ".tif": return true;
                    default: return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string Factory(string path, ResourceType type)
        {
            string name = "New " + TypeToString(type);
            int i = 0;
            if (File.Exists(Path.Combine(path, name)) ||
                Directory.Exists(Path.Combine(path, name)))
                while (File.Exists(Path.Combine(path, name + " " + ++i)) ||
                    Directory.Exists(Path.Combine(path, name + " " + i))) ;
            if (i != 0) name += " " + i;
            path = Path.Combine(path, name);

            switch (type)
            {
                case ResourceType.Folder:
                    Directory.CreateDirectory(path);
                    break;
                case ResourceType.Texture:
                    using (var resource = new TextureResource()) resource.Save(path);
                    break;
                case ResourceType.Sprite:
                    using (var resource = new SpriteResource()) resource.Save(path);
                    break;
                case ResourceType.Ragdoll:
                    using (var resource = new RagdollResource()) resource.Save(path);
                    break;
                case ResourceType.Animation:
                    using (var resource = new AnimationResource()) resource.Save(path);
                    break;
                case ResourceType.Tool:
                    using (var resource = new ToolResource()) resource.Save(path);
                    break;
                case ResourceType.Entity:
                    using (var resource = new EntityResource()) resource.Save(path);
                    break;
                case ResourceType.Tile:
                    using (var resource = new TileResource()) resource.Save(path);
                    break;
                case ResourceType.Event:
                    using (var resource = new EventResource()) resource.Save(path);
                    break;
                case ResourceType.Outfit:
                    using (var resource = new OutfitResource()) resource.Save(path);
                    break;
                default: throw new NotImplementedException("Resource [" + TypeToString(type) + "] not implemented.");
            }

            return name;
        }
        public static string Factory(string path, ResourceType type, string[] src)
        {
            switch (type)
            {
                case ResourceType.Sprite:

                    using (var resource = new SpriteResource())
                    {
                        if (src.Length != 1) throw new Exception(
                            "Sources count [" + src.Length + "] is invalid for sprite.");
                        var src_path = src[0];

                        string name = TypeToString(type) + " " + Path.GetFileName(src_path);
                        int i = 0;
                        if (File.Exists(Path.Combine(path, name)) ||
                            Directory.Exists(Path.Combine(path, name)))
                            while (File.Exists(Path.Combine(path, name + " " + ++i)) ||
                                Directory.Exists(Path.Combine(path, name + " " + i))) ;
                        if (i != 0) name += " " + i;
                        path = Path.Combine(path, name);
                        var src_type = GetType(src_path);

                        if (src_type != ResourceType.Texture) throw new Exception(
                            "Source [" + TypeToString(src_type) + "] is ivalid source for sprite.");
                        resource.Texture.Link = src_path;
                        resource.AdjustImgbox();
                        resource.Save(path);
                        return path;
                    }
                case ResourceType.Ragdoll:
                    using (var resource = new RagdollResource())
                    {
                        Array.Sort(src);
                        string name = "New " + TypeToString(type);
                        int i = 0;
                        if (File.Exists(Path.Combine(path, name)) ||
                            Directory.Exists(Path.Combine(path, name)))
                            while (File.Exists(Path.Combine(path, name + " " + ++i)) ||
                                Directory.Exists(Path.Combine(path, name + " " + i))) ;
                        if (i != 0) name += " " + i;
                        path = Path.Combine(path, name);
                        foreach (var src_path in src)
                        {
                            var src_type = GetType(src_path);
                            if (src_type != ResourceType.Sprite) throw new Exception(
                                "Source [" + TypeToString(src_type) + "] is ivalid source for ragdoll.");
                            var node = new RagdollResource.Node();
                            node.Sprites.Add(new Subresource<SpriteResource>(src_path, true));
                            resource.Nodes.Add(node);
                        }
                        resource.Save(path);
                        return path;
                    }

                default: throw new NotImplementedException("Resource [" + TypeToString(type) + "] factory implemented.");
            }
        }
        public static string Factory(string path, string src)
        {
            var type = ResourceType.Unknown;
            var ext = Path.GetExtension(src).ToLower();
            switch (ext)
            {
                case ".bmp": type = ResourceType.Texture; break;
                case ".gif": type = ResourceType.Texture; break;
                case ".exif": type = ResourceType.Texture; break;
                case ".jpg": type = ResourceType.Texture; break;
                case ".jpeg": type = ResourceType.Texture; break;
                case ".png": type = ResourceType.Texture; break;
                case ".tif": type = ResourceType.Texture; break;
            }

            string name = Path.GetFileNameWithoutExtension(src);
            int i = 0;
            if (File.Exists(Path.Combine(path, name)) ||
                Directory.Exists(Path.Combine(path, name)))
                while (File.Exists(Path.Combine(path, name + " " + ++i)) ||
                    Directory.Exists(Path.Combine(path, name + " " + i))) ;
            if (i != 0) name += " " + i;
            path = Path.Combine(path, name);

            if (type == ResourceType.Folder)
            {
                Directory.CreateDirectory(path);
                return name;
            }

            switch (type)
            {
                case ResourceType.Texture:
                    using (var resource = new TextureResource())
                    {
                        resource.Texture = new Bitmap(src);
                        resource.Save(path);
                    }
                    break;

                default: throw new Exception("Can not generate resource [" + 
                    TypeToString(type) + "] from source [" + src + "].");
            }

            return name;
        }
        public static ResourceType GetType(string path)
        {
            try
            {
                if (Directory.Exists(path)) return ResourceType.Folder;
                if (!File.Exists(path)) return ResourceType.MissingFile;

                using (var r = new BinaryReader(File.Open(path, FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite)))
                {
                    if (!ResourceSignature.Read(r)) return ResourceType.NotResource;
                    var type = StringToType(r.ReadString());
                    var version = r.ReadString();
                    if (CurrentVersion(type) != version) return ResourceType.Outdated;
                    return type;
                }
            }
            catch
            {
                return ResourceType.NotResource;
            }
        }
        public static ResourceType GetType(Type type)
        {
            if (type == typeof(TextureResource)) return TextureResource.CurrentType;
            if (type == typeof(SpriteResource)) return SpriteResource.CurrentType;
            if (type == typeof(RagdollResource)) return RagdollResource.CurrentType;
            if (type == typeof(AnimationResource)) return AnimationResource.CurrentType;
            if (type == typeof(ToolResource)) return ToolResource.CurrentType;
            if (type == typeof(EntityResource)) return EntityResource.CurrentType;
            if (type == typeof(TileResource)) return TileResource.CurrentType;
            if (type == typeof(EventResource)) return EventResource.CurrentType;
            if (type == typeof(OutfitResource)) return OutfitResource.CurrentType;
            return ResourceType.NotResource;
        }
        public static ResourceID GetID(string path)
        {
            if (!File.Exists(path)) return null;

            using (var r = new BinaryReader(File.OpenRead(path)))
            {
                if (!ResourceSignature.Read(r)) return null;
                var type = r.ReadString();
                var version = r.ReadString();
                var timestamp = r.ReadString(); 
                return null;
            }
        }


        protected abstract void ReadData(BinaryReader r);
        protected abstract void WriteData(BinaryWriter w);

        public ResourceType Type { get; private set; }
        public string Version { get; private set; }
        public string TimeStamp { get; private set; }
        public ResourceID ID { get; private set; } 

        public Resource(ResourceType type, string version)
        {
            Type = type;
            Version = version;
            TimeStamp = ResourceSignature.TimeStamp;
            ID = new ResourceID();
        }
        public Resource(string path)
        {
            Open(path);
        }
        public void Open(string path)
        {
            using (var r = new BinaryReader(File.OpenRead(path)))
            {
                if (!ResourceSignature.Read(r)) throw new Exception(
                    "Invalid resource [" + path + "] signature.");

                Type = StringToType(r.ReadString());
                Version = r.ReadString();
                TimeStamp = r.ReadString();
                ID = new ResourceID(r);
                ReadData(r);
            }
        }
        public void Save(string path)
        {
            using (var w = new BinaryWriter(File.OpenWrite(path)))
            {
                ResourceSignature.Write(w);
                w.Write(TypeToString(Type));
                w.Write(Version);
                w.Write(ResourceSignature.TimeStamp);
                ID.Write(w);
                WriteData(w);
            }
        }

        protected bool IsDisposed { get; private set; } = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            IsDisposed = true;
        }
        ~Resource()
        {
            Dispose(false);
        }
    }
}
