using ExtraSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resource_Redactor.Descriptions
{
    class EntityResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Entity;
        public static readonly string CurrentVersion = "0.0.0.1";

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

            BackColor = r.ReadInt32();
            PointBoundsX = r.ReadSingle();
            PointBoundsY = r.ReadSingle();
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

            w.Write(BackColor);
            w.Write(PointBoundsX);
            w.Write(PointBoundsY);
            w.Write(GridEnabled);
            w.Write(Transparency);
        }

        public class Trigger : IDisposable
        {
            public string Name { get; set; } = "";

            public string Action { get; set; } = "";

            public double VelocityXLowBound { get; set; } = double.NegativeInfinity;
            public double VelocityXHighBound { get; set; } = double.PositiveInfinity;

            public double VelocityYLowBound { get; set; } = double.NegativeInfinity;
            public double VelocityYHighBound { get; set; } = double.PositiveInfinity;

            public double AccelerationXLowBound { get; set; } = double.NegativeInfinity;
            public double AccelerationXHighBound { get; set; } = double.PositiveInfinity;

            public double AccelerationYLowBound { get; set; } = double.NegativeInfinity;
            public double AccelerationYHighBound { get; set; } = double.PositiveInfinity;

            public int OnGround { get; set; } = DoNotCare;
            public int OnRoof { get; set; } = DoNotCare;
            public int OnWall { get; set; } = DoNotCare;
            public int Direction { get; set; } = DoNotCare;

            public Subresource<AnimationResource> Animation { get; set; } = new Subresource<AnimationResource>();

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
                Both = 0,
                Right = 1,
            }
            public readonly static int DoNotCare = int.MinValue;
            [JsonIgnore]
            public BoolConditional IOnGround
            {
                get { return (BoolConditional)OnGround; }
                set { OnGround = (int)value; }
            }
            [JsonIgnore]
            public BoolConditional IOnRoof
            {
                get { return (BoolConditional)OnRoof; }
                set { OnRoof = (int)value; }
            }
            [JsonIgnore]
            public DirectionConditional IOnWall
            {
                get { return (DirectionConditional)OnWall; }
                set { OnWall = (int)value; }
            }
            [JsonIgnore]
            public DirectionConditional IDirection
            {
                get { return (DirectionConditional)Direction; }
                set { Direction = (int)value; }
            }

            public bool Active = false;

            protected bool IsDisposed { get; private set; } = false;
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            public virtual void Dispose(bool disposing)
            {
                if (IsDisposed) return;
                if (disposing) Animation.Dispose();
                IsDisposed = true;
            }
            ~Trigger()
            {
                Dispose(false);
            }
        }
        public class Holder : IDisposable
        {
            public string Name { get; set; } = "";
            public string Action { get; set; } = "";
            public int HolderPoint { get; set; } = -1;
            public Subresource<AnimationResource> Animation { get; set; } = new Subresource<AnimationResource>();

            public Holder()
            {
            }
            public Holder(BinaryReader r)
            {
                Name = r.ReadString();
                Action = r.ReadString();
                HolderPoint = r.ReadInt32();
                Animation.Read(r);
            }
            public void Write(BinaryWriter w)
            {
                w.Write(Name);
                w.Write(Action);
                w.Write(HolderPoint);
                Animation.Write(w);
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
                if (disposing) Animation.Dispose();
                IsDisposed = true;
            }
            ~Holder()
            {
                Dispose(false);
            }
        }

        // Resource //
        public Subresource<RagdollResource> Ragdoll { get; set; } = new Subresource<RagdollResource>();
        public List<Holder> Holders { get; set; } = new List<Holder>();
        public List<Trigger> Triggers { get; set; } = new List<Trigger>();

        public ulong MaxHealth { get; set; } = 0;
        public ulong MaxEnergy { get; set; } = 0;
        public double Mass { get; set; } = 1d;

        public double GravityAcceleration { get; set; } = 0d;
        public double JumpVelocity { get; set; } = 0d;
        public double DragX { get; set; } = 0d;
        public double DragY { get; set; } = 0d;
        public double SqrDragX { get; set; } = 0d;
        public double SqrDragY { get; set; } = 0d;
        public double MoveForceX { get; set; } = 0d;
        public double MoveForceY { get; set; } = 0d;

        // Redactor //
        public int BackColor { get; set; } = Color.Black.ToArgb();
        public float PointBoundsX { get; set; } = 5f;
        public float PointBoundsY { get; set; } = 4f;
        public bool GridEnabled { get; set; } = true;
        public bool Transparency { get; set; } = false;

        public EntityResource() : base(CurrentType, CurrentVersion)
        {
        }
        public EntityResource(string path) : base(path)
        {

        }

        public void Render(float x, float y, float a, long time, int cycle, int[] sel = null, float sx = 1f, float sy = 1f)
        {
            
        }

        public override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Ragdoll.Dispose();
                Holders.ForEach((Holder h) => h.Dispose());
                Triggers.ForEach((Trigger t) => t.Dispose());
            }

            base.Dispose(disposing);
        }
    }
}
