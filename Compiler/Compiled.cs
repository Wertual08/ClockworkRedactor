using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Compiler
{
    public static class Compiled
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

        public struct Event
        {
            public struct Action
            {
                public int Type;
                public int LinkID;
                public int OffsetX;
                public int OffsetY;
            }
            public ulong MinDelay;
            public ulong MaxDelay;
            public int FirstAction;
            public int ActionsCount;
        }

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

        public struct Ragdoll
        {
            public struct Node
            {
                public float OffsetX;
                public float OffsetY;
                public int MainNode;
            }

            public int FirstNode;
            public int NodesCount;
            public double HitboxW;
            public double HitboxH;
        }

        public struct Animation
        {
            public struct Node
            {
                public int Properties;
                public float OffsetX;
                public float OffsetY;
                public float Angle;
            }
            public int FirstNode;
            public int FrameCount;
            public int NodesPerFrame;
            public int Dependency;
            public float FramesPerUnitRatio;
        }

        public struct Entity
        {
            public struct Trigger
            {
                public int ActionID;

                public double VelocityXLowBound;
                public double VelocityYLowBound;

                public double VelocityXHighBound;
                public double VelocityYHighBound;

                public double AccelerationXLowBound;
                public double AccelerationYLowBound;

                public double AccelerationXHighBound;
                public double AccelerationYHighBound;

                public int OnGround;
                public int OnRoof;
                public int OnWall;
                public int Direction;

                public int AnimationID;
            }
            public struct Holder
            {
                public int ActionID;
                public int HolderPoint;
                public int AnimationID;
            }

            public int RagdollID;
            public int FirstTrigger;
            public int TriggersCount;
            public int FirstHolder;
            public int HoldersCount;

            public ulong MaxHealth;
            public ulong MaxEnergy;
            public double Mass;

            public double GravityAcceleration;
            public double JumpVelocity;
            public double DragX;
            public double DragY;
            public double SqrDragX;
            public double SqrDragY;
            public double MoveForceX;
            public double MoveForceY;
        }

        public struct Outfit
        {
            public struct Node
            {
                public int SpriteID;
                public int RagdollNodeIndex;
                public int ClotheType;
            }

            public int FirstNode;
            public int NodesCount;
        }
    }
}
