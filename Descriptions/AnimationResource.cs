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
    public class AnimationResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Animation;
        public static readonly string CurrentVersion = "0.0.0.0";

        protected override void ReadData(BinaryReader r)
        {
            if (Type != CurrentType) throw new Exception(
                "Resource have wrong type [" +
                TypeToString(Type) + "]. [" +
                CurrentType + "] required.");
            if (Version != CurrentVersion) throw new Exception(
                "Resource have wrong version [" + Version +
                "]. [" + CurrentVersion + "] required.");

            NodesCount = r.ReadInt32();
            Dependency = (AnimationType)r.ReadInt32();
            FramesPerUnitRatio = r.ReadSingle();
            Frames = new List<Frame>(r.ReadInt32());
            for (int i = 0; i < Frames.Capacity; i++) Frames.Add(new Frame(r));

            BackColor = r.ReadInt32();
            PointBoundsX = r.ReadSingle();
            PointBoundsY = r.ReadSingle();
            PixelPerfect = r.ReadBoolean();
            GridEnabled = r.ReadBoolean();
            Transparency = r.ReadBoolean();
            Ragdoll.Read(r);
        }
        protected override void WriteData(BinaryWriter w)
        {
            w.Write(NodesCount);
            w.Write((int)Dependency);
            w.Write(FramesPerUnitRatio);
            w.Write(Frames.Count);
            foreach (var f in Frames) f.Write(w);

            w.Write(BackColor);
            w.Write(PointBoundsX);
            w.Write(PointBoundsY);
            w.Write(PixelPerfect);
            w.Write(GridEnabled);
            w.Write(Transparency);
            Ragdoll.Write(w);
        }

        public int NodesCount { get; set; } = 0;
        public AnimationType Dependency { get; set; } = AnimationType.TimeLoop;
        public float FramesPerUnitRatio { get; set; } = 1f;
        public List<Frame> Frames { get; set; } = new List<Frame>();

        // Redactor //
        public int BackColor { get; set; } = Color.Black.ToArgb();
        public float PointBoundsX { get; set; } = 5f;
        public float PointBoundsY { get; set; } = 4f;
        public bool PixelPerfect { get; set; } = false;
        public bool GridEnabled { get; set; } = true;
        public bool Transparency { get; set; } = true;
        public WeakSubresource<RagdollResource> Ragdoll { get; set; } = new WeakSubresource<RagdollResource>();

        public AnimationResource() : base(CurrentType, CurrentVersion)
        {

        }
        public AnimationResource(string path) : base(path)
        {

        }

        [JsonIgnore]
        public int Count { get { return Frames.Count; } }
        public Frame this[int i]
        {
            get { return Frames[i]; }
            set { Frames[i] = value; }
        }

        private float FrameBuffer = 0f;
        [JsonIgnore]
        public int Index { get { return (int)FrameBuffer; } set { FrameBuffer = value; } }
        [JsonIgnore]
        public bool Playing { get; private set; } = false;
        public void Update(float delta, float fpur_scale)
        {
            if (!Playing || Count < 1) return;
            switch (Dependency)
            {
                case AnimationType.TimeLoop:
                case AnimationType.TimeOnce:
                    {
                        FrameBuffer += delta * FramesPerUnitRatio;
                    }
                    break;

                case AnimationType.MovementX:
                    {
                        FrameBuffer += delta * FramesPerUnitRatio * fpur_scale;
                    }
                    break;

                case AnimationType.MovementY:
                    {
                        FrameBuffer += delta * FramesPerUnitRatio * fpur_scale;
                    }
                    break;

                case AnimationType.VelocityX:
                    {
                        FrameBuffer = (float)(Math.Atan(-fpur_scale * FramesPerUnitRatio) / Math.PI + 0.5d) * (Count - 1);
                    }
                    break;

                case AnimationType.VelocityY:
                    {
                        FrameBuffer = (float)(Math.Atan(-fpur_scale * FramesPerUnitRatio) / Math.PI + 0.5d) * (Count - 1);
                    }
                    break;

                case AnimationType.AimCursor:
                    {
                        FrameBuffer += delta * FramesPerUnitRatio;
                    }
                    break;

                case AnimationType.FollowCursor:
                    {
                        FrameBuffer += delta * FramesPerUnitRatio;
                    }
                    break;
            }

            if ((Dependency == AnimationType.TimeOnce ||
                Dependency == AnimationType.AimCursor ||
                Dependency == AnimationType.FollowCursor) &&
                FrameBuffer > Count - 1)
            {
                FrameBuffer = 0f;
                Playing = false;
            }

            FrameBuffer %= Count;
            if (FrameBuffer < 0) FrameBuffer += Count;
        }
        [JsonIgnore]
        public Frame CurrentFrame
        {
            get
            {
                if (Count < 1) return null;
                return Frame.Unite(Frames[(int)FrameBuffer % Count], Frames[((int)FrameBuffer + 1) % Count], FrameBuffer - (int)FrameBuffer);
            }
        }
        public void Reset()
        {
            FrameBuffer = 0f;
        }
        public void Play(int begin = 0)
        {
            FrameBuffer = begin;
            Playing = true;
        }
        public void Stop()
        {
            Playing = false;
        }

        public override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Ragdoll.Dispose();
            }

            base.Dispose(disposing);
        }
    }
    public enum AnimationType : int
    {
        TimeLoop,
        TimeOnce,
        MovementX,
        MovementY,
        VelocityX,
        VelocityY, 
        AimCursor,
        FollowCursor
    }
}
