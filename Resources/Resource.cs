using ExtraForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.Text.Json;
using System.Globalization;



namespace Resource_Redactor.Resources
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
        Inventory,
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
    
    public class ResourceID : IComparable
    {
        private static Random Generator = new Random();
        private long TimePart;
        private long RandPart;

        public string Value
        {
            get { return TimePart.ToString("X16") + RandPart.ToString("X16"); }
            set 
            { 
                TimePart = long.Parse(value.Substring(0, 16), NumberStyles.HexNumber);
                RandPart = long.Parse(value.Substring(16), NumberStyles.HexNumber);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
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

        public ResourceID()
        {
            TimePart = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            RandPart = ((long)Generator.Next(int.MinValue, int.MaxValue) << 32) | (long)Generator.Next(int.MinValue, int.MaxValue);
        }
        public ResourceID(string value)
        {
            if (value == "")
            {
                TimePart = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                RandPart = ((long)Generator.Next(int.MinValue, int.MaxValue) << 32) | (long)Generator.Next(int.MinValue, int.MaxValue);
            }
            var values = value.Split(':');
            TimePart = Convert.ToInt64(values[0]);
            RandPart = Convert.ToInt64(values[1]);
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
        public override string ToString()
        {
            return TimePart + ":" + RandPart;
        }
        public ResourceID Copy()
        {
            var id = new ResourceID();
            id.TimePart = TimePart;
            id.RandPart = RandPart;
            return id;
        }
    }
    public class Resource : IDisposable
    {
        private static JsonSerializerOptions SerializerOptions_ = null;
        private static JsonSerializerOptions SerializerOptions 
        {
            get
            {
                if (SerializerOptions_ != null) return SerializerOptions_;
                var options = new JsonSerializerOptions();
                options.IgnoreReadOnlyProperties = false;
                options.WriteIndented = false;
                options.Converters.Add(new JsonHandleSpecialDoublesAsStrings());
                options.Converters.Add(new JsonHandleSpecialFloatsAsStrings());
                options.Converters.Add(new JsonStringEnumConverter());
                return SerializerOptions_ = options;
            }
        }

        public static int TypeToIcon(ResourceType type)
        {
            ResourceIcon i;
            if (Enum.TryParse(type.ToString(), out i)) return (int)i;
            else return (int)ResourceIcon.Invalid;
        }

        public static ResourceType GetType(string path)
        {
            try
            {
                if (Directory.Exists(path)) return ResourceType.Folder;
                if (!File.Exists(path)) return ResourceType.MissingFile;

                using (var resource = new Resource(path))
                    return GetVersion(resource.Type) == resource.Version ? resource.Type : ResourceType.Outdated;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return ResourceType.NotResource;
            }
        }
        public static ResourceType GetLegacyType(string path)
        {
            try
            {
                if (Directory.Exists(path)) return ResourceType.Folder;
                if (!File.Exists(path)) return ResourceType.MissingFile;

                using (var resource = new Resource(path)) return resource.Type;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
        public static Type GetType(ResourceType type)
        {
            if (type == TextureResource.CurrentType) return typeof(TextureResource);
            if (type == SpriteResource.CurrentType) return typeof(SpriteResource);
            if (type == RagdollResource.CurrentType) return typeof(RagdollResource);
            if (type == AnimationResource.CurrentType) return typeof(AnimationResource);
            if (type == ToolResource.CurrentType) return typeof(ToolResource);
            if (type == EntityResource.CurrentType) return typeof(EntityResource);
            if (type == TileResource.CurrentType) return typeof(TileResource);
            if (type == EventResource.CurrentType) return typeof(EventResource);
            if (type == OutfitResource.CurrentType) return typeof(OutfitResource);
            return null;
        }
        public static ResourceID GetID(string path)
        {
            if (!File.Exists(path)) return null;

            using (var r = new BinaryReader(File.OpenRead(path)))
            {
                try
                {
                    using (var resource = new Resource(path)) return resource.ID;
                }
                catch
                {
                    return null;
                }
            }
        }
        public static string GetVersion(ResourceType type)
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
        public static string GetVersion(string path)
        {
            if (Directory.Exists(path)) return "_._._._";
            if (!File.Exists(path)) return "_._._._";

            using (var r = new BinaryReader(File.Open(path, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite)))
            {
                using (var resource = new Resource(path)) return resource.Version;
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
            string name = "New " + type;
            int i = 0;
            if (File.Exists(Path.Combine(path, name)) ||
                Directory.Exists(Path.Combine(path, name)))
                while (File.Exists(Path.Combine(path, name + " " + ++i)) ||
                    Directory.Exists(Path.Combine(path, name + " " + i))) ;
            if (i != 0) name += " " + i;
            path = Path.Combine(path, name);

            if (type == ResourceType.Folder) Directory.CreateDirectory(path);
            else
            {
                using (var resource = Factory(type))
                {
                    if (resource != null) resource.Save(path);
                    else throw new NotImplementedException("Resource [" +
                        type + "] not implemented.");
                }
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

                        string name = type + " " + Path.GetFileName(src_path);
                        int i = 0;
                        if (File.Exists(Path.Combine(path, name)) ||
                            Directory.Exists(Path.Combine(path, name)))
                            while (File.Exists(Path.Combine(path, name + " " + ++i)) ||
                                Directory.Exists(Path.Combine(path, name + " " + i))) ;
                        if (i != 0) name += " " + i;
                        path = Path.Combine(path, name);
                        var src_type = GetType(src_path);

                        if (src_type != ResourceType.Texture) throw new Exception(
                            "Source [" + src_type + "] is ivalid source for sprite.");
                        resource.Texture.Link = src_path;
                        resource.AdjustImgbox();
                        resource.Save(path);
                        return path;
                    }

                case ResourceType.Ragdoll:
                    using (var resource = new RagdollResource())
                    {
                        Array.Sort(src);
                        string name = "New " + type;
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
                                "Source [" + src_type + "] is ivalid source for ragdoll.");
                            var node = new RagdollResource.Node();
                            node.Sprites.Add(new Subresource<SpriteResource>(src_path));
                            resource.Nodes.Add(node);
                        }
                        resource.Save(path);
                        return path;
                    }

                case ResourceType.Outfit:
                    using (var resource = new OutfitResource())
                    {
                        if (src.Length != 1) throw new Exception(
                            "Sources count [" + src.Length + "] is invalid for outfit.");
                        var src_path = src[0];

                        string name = type + " " + Path.GetFileName(src_path);
                        int i = 0;
                        if (File.Exists(Path.Combine(path, name)) ||
                            Directory.Exists(Path.Combine(path, name)))
                            while (File.Exists(Path.Combine(path, name + " " + ++i)) ||
                                Directory.Exists(Path.Combine(path, name + " " + i))) ;
                        if (i != 0) name += " " + i;
                        path = Path.Combine(path, name);
                        var src_type = GetType(src_path);

                        if (src_type != ResourceType.Ragdoll) throw new Exception(
                            "Source [" + src_type + "] is ivalid source for outfit.");

                        using (var src_resource = new RagdollResource(src_path))
                        {
                            resource.Ragdoll.Link = src_path;
                            for (int j = 0; j < src_resource.Nodes.Count; j++)
                            {
                                var node = src_resource.Nodes[j];
                                foreach (var sprite in node.Sprites)
                                    resource.Nodes.Add(new OutfitResource.Node(sprite.Link, j));
                            }
                        }

                        resource.Save(path);

                        return path;
                    }

                default: throw new NotImplementedException("Resource [" + type + "] factory implemented.");
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

                default: throw new Exception("Can not generate resource [" + type + "] from source [" + src + "].");
            }

            return name;
        }
        public static Resource Factory(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Texture: return new TextureResource();
                case ResourceType.Sprite: return new SpriteResource();
                case ResourceType.Ragdoll: return new RagdollResource();
                case ResourceType.Animation: return new AnimationResource();
                case ResourceType.Tool: return new ToolResource();
                case ResourceType.Entity: return new EntityResource();
                case ResourceType.Tile: return new TileResource();
                case ResourceType.Event: return new EventResource();
                case ResourceType.Outfit: return new OutfitResource();
                default: return null;
            }
        }
        public static Resource Factory(string path)
        {
            Resource resource = Factory(GetType(path));
            resource?.Open(path);
            return resource;
        }


        public string Stamp { get => "CLOCKWORK_ENGINE_REDACTOR_RESOURCE"; }
        public ResourceType Type { get; set; } = ResourceType.Unknown;
        public string Version { get; set; } = "_._._._";
        public string TimeStamp { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public ResourceID ID { get; set; } = new ResourceID();

        public Resource(ResourceType type, string version)
        {
            Type = type;
            Version = version;
        }
        public Resource(string path)
        {
            Open(path);
        }
        public virtual void Open(string path)
        {
            var json_text = File.ReadAllText(path);
            using (var json = JsonDocument.Parse(json_text))
                json.RootElement.PopulateObject(this, GetType(), SerializerOptions);
        }
        public virtual void Save(string path)
        {
            Version = GetVersion(Type);
            TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            File.WriteAllText(path, JsonSerializer.Serialize(this, GetType(), SerializerOptions));
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
