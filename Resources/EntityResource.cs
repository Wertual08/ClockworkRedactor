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

namespace Resource_Redactor.Resources
{
    public class EntityResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Entity;
        public static readonly string CurrentVersion = "0.0.0.1";

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
            protected virtual void Dispose(bool disposing)
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

            protected bool IsDisposed { get; private set; } = false;
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected virtual void Dispose(bool disposing)
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
        public Subresource<OutfitResource> Outfit { get; set; } = new Subresource<OutfitResource>();
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
        public Color BackColor { get; set; } = Color.Black;
        public float PointBoundsX { get; set; } = 5f;
        public float PointBoundsY { get; set; } = 4f;
        public bool GridEnabled { get; set; } = true;
        public bool Transparency { get; set; } = false;

        private OutfitResource LastOutfit;
        public void UpdateOutfit(object sender, EventArgs e)
        {
            Ragdoll.Resource?.Unclothe(LastOutfit);
            LastOutfit = Outfit.Resource;
            Ragdoll.Resource?.Clothe(LastOutfit);
        }
        public EntityResource() : base(CurrentType, CurrentVersion)
        {
            Ragdoll.Refreshed += UpdateOutfit;
            Outfit.Refreshed += UpdateOutfit;
        }
        public EntityResource(string path) : base(path)
        {
            Ragdoll.Refreshed += UpdateOutfit;
            Outfit.Refreshed += UpdateOutfit;
        }

        public void Render(float x, float y, float a, long time, int cycle, int[] sel = null, float sx = 1f, float sy = 1f)
        {
            
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Ragdoll.Dispose();
                Outfit.Dispose();
                Holders.ForEach((Holder h) => h.Dispose());
                Triggers.ForEach((Trigger t) => t.Dispose());
            }

            base.Dispose(disposing);
        }
    }
}
