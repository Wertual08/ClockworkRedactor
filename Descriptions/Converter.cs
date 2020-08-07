using ExtraSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Resource_Redactor.Descriptions
{
    public class Converter
    {
        private static class Subresources
        {
            public interface IV0000
            {
                event EventHandler Reloaded;
                event EventHandler Updated;
                event EventHandler Refreshed;
                ISynchronizeInvoke SynchronizingObject { get; set; }
                string Link { get; set; }
                bool Loaded { get; }
                bool Active { get; set; }
                void Reload();
                void Read(BinaryReader r);
                void Write(BinaryWriter w);
                ResourceType Type { get; }
            }

            public class V0000<T> : IV0000, IDisposable where T : Resource, new()
            {
                private FileSystemWatcher Watcher = new FileSystemWatcher();
                private void Watcher_Renamed(object sender, RenamedEventArgs e)
                {
                    _Link = ExtraPath.MakeDirectoryRelated(Directory.GetCurrentDirectory(), e.FullPath);
                    Updated?.Invoke(this, EventArgs.Empty);
                    Reload();
                }
                private void Watcher_Changed(object sender, FileSystemEventArgs e)
                {
                    Updated?.Invoke(this, EventArgs.Empty);
                    Reload();
                }

                protected string _Link = "";
                protected T _Resource;
                protected void Refresh()
                {
                    Refreshed?.Invoke(this, EventArgs.Empty);
                }

                [JsonIgnore]
                public ResourceType Type { get { return Descriptions.Resource.GetType(typeof(T)); } }
                [JsonIgnore]
                public ISynchronizeInvoke SynchronizingObject { get { return Watcher.SynchronizingObject; } set { Watcher.SynchronizingObject = value; } }
                public event EventHandler Reloaded;
                public event EventHandler Updated;
                public event EventHandler Refreshed;
                [JsonIgnore]
                public T Resource { get { return Loaded && Active ? _Resource : null; } }
                [JsonIgnore]
                public bool Loaded { get; protected set; } = false;
                public virtual string Link
                {
                    get
                    {
                        return _Link;
                    }
                    set
                    {
                        if (_Link != value)
                        {
                            _Link = value;
                            Reload();
                        }
                    }
                }
                public bool Active { get; set; } = true;

                public V0000()
                {
                    Watcher.Changed += Watcher_Changed;
                    Watcher.Created += Watcher_Changed;
                    Watcher.Deleted += Watcher_Changed;
                    Watcher.Renamed += Watcher_Renamed;
                }
                public V0000(string path, bool active)
                {
                    Watcher.Changed += Watcher_Changed;
                    Watcher.Created += Watcher_Changed;
                    Watcher.Deleted += Watcher_Changed;
                    Watcher.Renamed += Watcher_Renamed;
                    Link = path;
                    Active = active;
                }
                public V0000(BinaryReader r)
                {
                    Watcher.Changed += Watcher_Changed;
                    Watcher.Created += Watcher_Changed;
                    Watcher.Deleted += Watcher_Changed;
                    Watcher.Renamed += Watcher_Renamed;
                    Read(r);
                }
                public void Reload()
                {
                    try
                    {
                        _Resource?.Dispose();
                        _Resource = null;

                        if (!File.Exists(_Link))
                        {
                            Watcher.EnableRaisingEvents = false;
                            Loaded = false;
                            Reloaded?.Invoke(this, EventArgs.Empty);
                            Refreshed?.Invoke(this, EventArgs.Empty);
                            return;
                        }
                        try
                        {
                            Watcher.Filter = Path.GetFileName(_Link);
                            Watcher.Path = Path.GetDirectoryName(Path.GetFullPath(_Link));
                            Watcher.EnableRaisingEvents = true;
                        }
                        catch
                        {
                            Watcher.EnableRaisingEvents = false;
                        }
                        _Resource = new T();
                        _Resource.Open(_Link);
                        Loaded = true;
                    }
                    catch
                    {
                        Loaded = false;
                    }
                    Reloaded?.Invoke(this, EventArgs.Empty);
                    Refreshed?.Invoke(this, EventArgs.Empty);
                }

                public void Read(BinaryReader r)
                {
                    Link = r.ReadString();
                    Active = r.ReadBoolean();
                }
                public void Write(BinaryWriter w)
                {
                    w.Write(Link);
                    w.Write(Active);
                }

                protected bool IsDisposed { get; private set; } = false;
                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
                public virtual void Dispose(bool disposing)
                {
                    if (IsDisposed) return;

                    if (disposing)
                    {
                        Watcher.Dispose();
                        _Resource?.Dispose();
                    }

                    IsDisposed = true;
                }
                ~V0000()
                {
                    Dispose(false);
                }
            }

            public class WeakV0000<T> : V0000<T> where T : Resource, new()
            {
                public override string Link
                {
                    get => base.Link;
                    set
                    {
                        if (_Link != value)
                        {
                            _Link = value;
                            Loaded = false;
                            Refresh();
                        }
                    }
                }

                public WeakV0000()
                {
                }
                public WeakV0000(string path, bool active) : base(path, active)
                {
                }
                public WeakV0000(BinaryReader r) : base(r)
                {
                }
            }
        }
        private static class Sprites
        {

        }
        private static class Ragdolls
        {

        }
        private static class Animations
        {

        }
        private static class Tiles
        {

        }
        private static class Entities
        {
            public class EntityResource0000to0001 : Resource
            {
                public static readonly ResourceType CurrentType = ResourceType.Entity;
                public static readonly string CurrentVersion = "0.0.0.0";

                protected override void ReadData(BinaryReader r)
                {
                    if (Type != CurrentType) throw new Exception(
                        "Resource have wrong type [" + TypeToString(Type) + "]. [" +
                        TypeToString(CurrentType) + "] required.");
                    if (Version != CurrentVersion) throw new Exception(
                        "Resource have wrong version [" + Version +
                        "]. [" + CurrentVersion + "] required.");

                    Ragdoll.Read(r);
                    Holders = new List<Holder>(r.ReadInt32());
                    for (int i = 0; i < Holders.Capacity; i++)
                        Holders.Add(new Holder(r));
                    Triggers = new List<Trigger>(r.ReadInt32());
                    for (int i = 0; i < Triggers.Capacity; i++)
                        Triggers.Add(new Trigger(r));

                    MaxHealth = r.ReadUInt64();
                    MaxEnergy = r.ReadUInt64();
                    Mass = r.ReadDouble();
                    GravityAcceleration = r.ReadDouble();
                    JumpVelocity = r.ReadDouble();
                    DragX = r.ReadDouble();
                    DragY = r.ReadDouble();
                    SqrDragX = r.ReadDouble();
                    SqrDragY = r.ReadDouble();
                    MoveForceX = r.ReadDouble();
                    MoveForceY = r.ReadDouble();

                    BackColor = Color.FromArgb(r.ReadInt32());
                    PointBounds = r.ReadStruct<PointF>();
                    GridEnabled = r.ReadBoolean();
                    Transparency = r.ReadBoolean();
                }
                protected override void WriteData(BinaryWriter w)
                {
                    Ragdoll.Write(w);
                    w.Write(Holders.Count);
                    for (int i = 0; i < Holders.Count; i++)
                        Holders[i].Write(w);
                    w.Write(Triggers.Count);
                    for (int i = 0; i < Triggers.Count; i++)
                        Triggers[i].Write(w);

                    w.Write(MaxHealth);
                    w.Write(MaxEnergy);
                    w.Write(Mass);
                    w.Write(GravityAcceleration);
                    w.Write(JumpVelocity);
                    w.Write(DragX);
                    w.Write(DragY);
                    w.Write(SqrDragX);
                    w.Write(SqrDragY);
                    w.Write(MoveForceX);
                    w.Write(MoveForceY);

                    w.Write(BackColor.ToArgb());
                    w.Write(PointBounds);
                    w.Write(GridEnabled);
                    w.Write(Transparency);
                }

                public class Trigger
                {
                    public string Name = "";

                    public string Action = "";

                    public double VelocityXLowBound = double.NegativeInfinity;
                    public double VelocityXHighBound = double.PositiveInfinity;

                    public double VelocityYLowBound = double.NegativeInfinity;
                    public double VelocityYHighBound = double.PositiveInfinity;

                    public double AccelerationXLowBound = double.NegativeInfinity;
                    public double AccelerationXHighBound = double.PositiveInfinity;

                    public double AccelerationYLowBound = double.NegativeInfinity;
                    public double AccelerationYHighBound = double.PositiveInfinity;

                    public int OnGround = DoNotCare;
                    public int OnRoof = DoNotCare;
                    public int OnWall = DoNotCare;
                    public int Direction = DoNotCare;

                    public Subresource<AnimationResource> Animation = new Subresource<AnimationResource>();

                    public Trigger()
                    {
                    }
                    public Trigger(BinaryReader r)
                    {
                        Name = r.ReadString();
                        Action = r.ReadString();
                        VelocityXLowBound = r.ReadDouble();
                        VelocityXHighBound = r.ReadDouble();
                        VelocityYLowBound = r.ReadDouble();
                        VelocityYHighBound = r.ReadDouble();
                        AccelerationXLowBound = r.ReadDouble();
                        AccelerationXHighBound = r.ReadDouble();
                        AccelerationYLowBound = r.ReadDouble();
                        AccelerationYHighBound = r.ReadDouble();
                        OnGround = r.ReadInt32();
                        OnWall = r.ReadInt32();
                        OnRoof = r.ReadInt32();
                        Direction = r.ReadInt32();
                        Animation.Read(r);
                    }
                    public void Write(BinaryWriter w)
                    {
                        w.Write(Name);
                        w.Write(Action);
                        w.Write(VelocityXLowBound);
                        w.Write(VelocityXHighBound);
                        w.Write(VelocityYLowBound);
                        w.Write(VelocityYHighBound);
                        w.Write(AccelerationXLowBound);
                        w.Write(AccelerationXHighBound);
                        w.Write(AccelerationYLowBound);
                        w.Write(AccelerationYHighBound);
                        w.Write(OnGround);
                        w.Write(OnWall);
                        w.Write(OnRoof);
                        w.Write(Direction);
                        Animation.Write(w);
                    }

                    public enum BoolConditional : int
                    {
                        False = 0,
                        True = 1,
                    }
                    public enum DirectionConditional : int
                    {
                        Left = -1,
                        Right = 1,
                    }
                    public readonly static int DoNotCare = int.MinValue;
                    public BoolConditional IOnGround
                    {
                        get { return (BoolConditional)OnGround; }
                        set { OnGround = (int)value; }
                    }
                    public BoolConditional IOnRoof
                    {
                        get { return (BoolConditional)OnRoof; }
                        set { OnRoof = (int)value; }
                    }
                    public DirectionConditional IOnWall
                    {
                        get { return (DirectionConditional)OnWall; }
                        set { OnWall = (int)value; }
                    }
                    public DirectionConditional IDirection
                    {
                        get { return (DirectionConditional)Direction; }
                        set { Direction = (int)value; }
                    }

                    public bool Active = false;
                }
                public class Holder
                {
                    public string Name = "";
                    public string Slot = "";
                    public string Action = "";
                    public int HolderPoint = -1;
                    public Subresource<AnimationResource> Animation = new Subresource<AnimationResource>();

                    public Holder()
                    {
                    }
                    public Holder(BinaryReader r)
                    {
                        Name = r.ReadString();
                        Slot = r.ReadString();
                        Action = r.ReadString();
                        HolderPoint = r.ReadInt32();
                        Animation.Read(r);
                    }
                    public void Write(BinaryWriter w)
                    {
                        w.Write(Name);
                        //w.Write(Slot);
                        w.Write(Action);
                        w.Write(HolderPoint);
                        Animation.Write(w);
                    }
                }

                // Resource //
                public Subresources.V0000<RagdollResource> Ragdoll = new Subresources.V0000<RagdollResource>();
                public List<Holder> Holders { get; private set; } = new List<Holder>();
                public List<Trigger> Triggers { get; private set; } = new List<Trigger>();

                public ulong MaxHealth = 0;
                public ulong MaxEnergy = 0;
                public double Mass = 1d;

                public double GravityAcceleration = 0d;
                public double JumpVelocity = 0d;
                public double DragX = 0d;
                public double DragY = 0d;
                public double SqrDragX = 0d;
                public double SqrDragY = 0d;
                public double MoveForceX = 0d;
                public double MoveForceY = 0d;

                // Redactor //
                public Color BackColor = Color.Black;
                public PointF PointBounds = new PointF(5f, 4f);
                public bool GridEnabled = true;
                public bool Transparency = false;

                public EntityResource0000to0001() : base(CurrentType, CurrentVersion)
                {
                }
                public EntityResource0000to0001(string path) : base(path)
                {

                }

                public void Render(float x, float y, float a, long time, int cycle, int[] sel = null, float sx = 1f, float sy = 1f)
                {

                }
            }
        }
        private static class Outfits
        {

        }
        private static class Tools
        {

        }


        private static void Description____to0000(string path)
        {
            string name = null;
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                if (!Description.ReadSignature(reader)) throw new Exception("Invalid description base file format.");
                name = reader.ReadString();
            }
            using (var writer = new BinaryWriter(File.OpenWrite(path)))
            {
                Description.WriteSignature(writer);
                writer.Write("0.0.0.0");
                writer.Write(name);
            }
        }
        private static void Description0000to0001(string path)
        {
            string name = null;
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                if (!Description.ReadSignature(reader)) throw new Exception("Invalid description base file format.");
                reader.ReadString(); // do not care about current version
                name = reader.ReadString();
            }

            var resources_path = Path.GetFullPath(path);
            resources_path = Path.Combine(Path.GetDirectoryName(resources_path), "Resources");
            var files = Directory.GetFiles(resources_path, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (Resource.GetType(file).ValidBinary())
                {
                    var bytes = File.ReadAllBytes(file);
                    using (var reader = new BinaryReader(new MemoryStream(bytes)))
                    using (var writer = new BinaryWriter(File.OpenWrite(file)))
                    {
                        ResourceSignature.Read(reader);
                        ResourceSignature.Write(writer);
                        writer.Write(reader.ReadString());
                        writer.Write(reader.ReadString());
                        writer.Write(reader.ReadString());
                        new ResourceID().Write(writer);
                        writer.Write(reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position)));
                    }
                }
            }

            using (var writer = new BinaryWriter(File.OpenWrite(path)))
            {
                Description.WriteSignature(writer);
                writer.Write("0.0.0.1");
                writer.Write(name);
            }
        }
        public static void ConvertDescription(string path)
        {
            string version;
            while ((version = Description.CheckVersion(path)) != Description.CurrentVersion)
            {
                switch (version)
                {
                    case "_._._._": Description____to0000(path); break;
                    case "0.0.0.0": Description0000to0001(path); break;

                    default: throw new Exception("Converter error: Failed to convert description. Final version: " + version);
                }
            }
        }


        private static void ConvertTexture(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Texture] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertSound(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Sound] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertSprite(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Sprite] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertRagdoll(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Ragdoll] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertAnimation(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Animation] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertTile(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Tile] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertEvent(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Event] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertEntity(string path)
        {
            var version = Resource.GetVersion(path);
            if (version == "0.0.0.0")
            {
                using (var resource = new Entities.EntityResource0000to0001(path)) resource.Save(path);
                version = "0.0.0.1";
            }

            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Entity] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertOutfit(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Outfit] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertTool(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Tool] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertItem(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Item] Version [" + version + "] not implemented.");
            }
        }
        private static void ConvertParticle(string path)
        {
            var version = Resource.GetVersion(path);
            switch (version)
            {
                default: throw new Exception("Converter error: Converter for [Particle] Version [" + version + "] not implemented.");
            }
        }

        public static void ConvertResource(string path)
        {
            var type = Resource.GetLegacyType(path);
            switch (type)
            {
                case ResourceType.Texture: ConvertTexture(path); break;
                case ResourceType.Sound: ConvertSound(path);  break;
                case ResourceType.Sprite: ConvertSprite(path); break;
                case ResourceType.Ragdoll: ConvertRagdoll(path); break;
                case ResourceType.Animation: ConvertAnimation(path); break;
                case ResourceType.Tile: ConvertTile(path); break;
                case ResourceType.Event: ConvertEvent(path); break;
                case ResourceType.Entity: ConvertEntity(path); break;
                case ResourceType.Outfit: ConvertOutfit(path); break;
                case ResourceType.Tool: ConvertTool(path); break;
                case ResourceType.Item: ConvertItem(path); break;
                case ResourceType.Particle: ConvertParticle(path); break;
                default: throw new Exception("Converter error: Converter for resource [" + type + "] not implemented.");
            }
        }
    }
}
