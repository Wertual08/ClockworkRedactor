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
    public static class JsonElementExt
    {
        public static long TryFind(this JsonElement element, string name, long def)
        {
            JsonElement property;
            long result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt64(out result)) return result;
            else return def;
        }
        public static int TryFind(this JsonElement element, string name, int def)
        {
            JsonElement property;
            int result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt32(out result)) return result;
            else return def;
        }
        public static short TryFind(this JsonElement element, string name, short def)
        {
            JsonElement property;
            short result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt16(out result)) return result;
            else return def;
        }
        public static sbyte TryFind(this JsonElement element, string name, sbyte def)
        {
            JsonElement property;
            sbyte result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetSByte(out result)) return result;
            else return def;
        }
        public static ulong TryFind(this JsonElement element, string name, ulong def)
        {
            JsonElement property;
            ulong result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt64(out result)) return result;
            else return def;
        }
        public static uint TryFind(this JsonElement element, string name, uint def)
        {
            JsonElement property;
            uint result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt32(out result)) return result;
            else return def;
        }
        public static ushort TryFind(this JsonElement element, string name, ushort def)
        {
            JsonElement property;
            ushort result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt16(out result)) return result;
            else return def;
        }
        public static byte TryFind(this JsonElement element, string name, byte def)
        {
            JsonElement property;
            byte result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetByte(out result)) return result;
            else return def;
        }
        public static float TryFind(this JsonElement element, string name, float def)
        {
            JsonElement property;
            float result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetSingle(out result)) return result;
            else return def;
        }
        public static double TryFind(this JsonElement element, string name, double def)
        {
            JsonElement property;
            double result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDouble(out result)) return result;
            else return def;
        }
        public static DateTime TryFind(this JsonElement element, string name, DateTime def)
        {
            JsonElement property;
            DateTime result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDateTime(out result)) return result;
            else return def;
        }
        public static DateTimeOffset TryFind(this JsonElement element, string name, DateTimeOffset def)
        {
            JsonElement property;
            DateTimeOffset result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDateTimeOffset(out result)) return result;
            else return def;
        }
        public static decimal TryFind(this JsonElement element, string name, decimal def)
        {
            JsonElement property;
            decimal result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDecimal(out result)) return result;
            else return def;
        }
        public static Guid TryFind(this JsonElement element, string name, Guid def)
        {
            JsonElement property;
            Guid result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetGuid(out result)) return result;
            else return def;
        }
        public static string TryFind(this JsonElement element, string name, string def)
        {
            JsonElement property;

            if (element.TryGetProperty(name, out property)) return property.GetString();
            else return def;
        }
        public static bool TryFind(this JsonElement element, string name, bool def)
        {
            JsonElement property;

            if (element.TryGetProperty(name, out property)) return property.GetBoolean();
            else return def;
        }

        public static bool TryFind(this JsonElement element, string name, ref long val)
        {
            JsonElement property;
            long result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt64(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref int val)
        {
            JsonElement property;
            int result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt32(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref short val)
        {
            JsonElement property;
            short result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt16(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref sbyte val)
        {
            JsonElement property;
            sbyte result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetSByte(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref ulong val)
        {
            JsonElement property;
            ulong result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt64(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref uint val)
        {
            JsonElement property;
            uint result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt32(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref ushort val)
        {
            JsonElement property;
            ushort result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt16(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref byte val)
        {
            JsonElement property;
            byte result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetByte(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref float val)
        {
            JsonElement property;
            float result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetSingle(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref double val)
        {
            JsonElement property;
            double result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDouble(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref DateTime val)
        {
            JsonElement property;
            DateTime result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDateTime(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref DateTimeOffset val)
        {
            JsonElement property;
            DateTimeOffset result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDateTimeOffset(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref decimal val)
        {
            JsonElement property;
            decimal result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDecimal(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref Guid val)
        {
            JsonElement property;
            Guid result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetGuid(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref string val)
        {
            JsonElement property;

            if (element.TryGetProperty(name, out property))
            {
                val = property.GetString();
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref bool val)
        {
            JsonElement property;

            if (element.TryGetProperty(name, out property))
            {
                val = property.GetBoolean();
                return true;
            }
            else return false;
        }
    }
    public class JsonHandleSpecialDoublesAsStrings : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return double.Parse(reader.GetString());
            }
            return reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            if (double.IsNaN(value) || double.IsInfinity(value)) 
                writer.WriteStringValue(value.ToString());
            else writer.WriteNumberValue(value);
        }
    }
    public class JsonHandleSpecialFloatsAsStrings : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return float.Parse(reader.GetString());
            }
            return reader.GetSingle();
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                writer.WriteStringValue(value.ToString());
            else writer.WriteNumberValue(value);
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

            if (type == ResourceType.Folder) Directory.CreateDirectory(path);
            else
            {
                using (var resource = Factory(type))
                {
                    if (resource != null) resource.Save(path);
                    else throw new NotImplementedException("Resource [" + 
                        TypeToString(type) + "] not implemented.");
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
            ResourceType type = GetType(path);
            switch (type)
            {
                case ResourceType.Texture: return new TextureResource(path);
                case ResourceType.Sprite: return new SpriteResource(path);
                case ResourceType.Ragdoll: return new RagdollResource(path);
                case ResourceType.Animation: return new AnimationResource(path);
                case ResourceType.Tool: return new ToolResource(path);
                case ResourceType.Entity: return new EntityResource(path);
                case ResourceType.Tile: return new TileResource(path);
                case ResourceType.Event: return new EventResource(path);
                case ResourceType.Outfit: return new OutfitResource(path);
                default: return null;
            }
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
        public static ResourceType GetLegacyType(string path)
        {
            try
            {
                if (Directory.Exists(path)) return ResourceType.Folder;
                if (!File.Exists(path)) return ResourceType.MissingFile;

                using (var r = new BinaryReader(File.Open(path, FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite)))
                {
                    if (!ResourceSignature.Read(r)) return ResourceType.NotResource;
                    return StringToType(r.ReadString());
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
                if (!ResourceSignature.Read(r)) return null;
                r.ReadString(); // Type
                r.ReadString(); // Version
                r.ReadString(); // TimeStamp
                return new ResourceID(r);
            }
        }
        public static string GetVersion(string path)
        {
            if (Directory.Exists(path)) return "_._._._";
            if (!File.Exists(path)) return "_._._._";

            using (var r = new BinaryReader(File.Open(path, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite)))
            {
                if (!ResourceSignature.Read(r)) return "_._._._";
                r.ReadString(); // Type
                return r.ReadString();
            }
        }

        public static void Import(string json_path, string resource_path)
        {
            string json = File.ReadAllText(json_path);
            Type type;

            using (var json_doc = JsonDocument.Parse(json))
            {
                type = GetType(StringToType(json_doc.RootElement.GetProperty("TypeName").GetString()));
            }

            var options = new JsonSerializerOptions
            {
                IgnoreReadOnlyProperties = true,
                WriteIndented = true,
            };
            options.Converters.Add(new JsonHandleSpecialDoublesAsStrings());
            options.Converters.Add(new JsonHandleSpecialFloatsAsStrings());
            options.Converters.Add(new JsonStringEnumConverter());

            using (Resource resource = JsonSerializer.Deserialize(json, type, options) as Resource) resource.Save(resource_path);
        }
        public static Resource Import(string json_path)
        {
            string json = File.ReadAllText(json_path);
            Type type;

            using (var json_doc = JsonDocument.Parse(json))
            {
                type = GetType(StringToType(json_doc.RootElement.GetProperty("TypeName").GetString()));
            }

            return JsonSerializer.Deserialize(json, type) as Resource;
        }
        public static void Export(string resource_path, string json_path)
        {
            using (Resource resource = Factory(resource_path))
            {
                if (resource == null) throw new NotImplementedException("Resource exporter does not implemented.");

                var options = new JsonSerializerOptions
                {
                    IgnoreReadOnlyProperties = true,
                    WriteIndented = true,
                };
                options.Converters.Add(new JsonStringEnumConverter());
                options.Converters.Add(new JsonHandleSpecialFloatsAsStrings());
                options.Converters.Add(new JsonHandleSpecialDoublesAsStrings());
                File.WriteAllText(json_path, JsonSerializer.Serialize(resource, resource.GetType(), options));
            }
        }
        public static void Export(Resource resource, string json_path)
        {
            var p = new JsonSerializerOptions
            {
                IgnoreReadOnlyProperties = true,
                WriteIndented = true,
            };
            if (resource == null) throw new NotImplementedException("Resource exporter does not implemented.");
            File.WriteAllText(json_path, JsonSerializer.Serialize(resource, resource.GetType(), p));
        }


        protected abstract void ReadData(BinaryReader r);
        protected abstract void WriteData(BinaryWriter w);

        public ResourceType Type { get; set; }
        public string Version { get; set; }
        public string TimeStamp { get; set; }
        public ResourceID ID { get; set; } 

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
            using (var w = new BinaryWriter(File.Create(path)))
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
