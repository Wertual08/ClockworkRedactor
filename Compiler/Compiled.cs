using ExtraForms;
using ExtraSharp;
using Resource_Redactor.Resources;
using Resource_Redactor.Resources.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            public int OutfitID;
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

        public static class Interface
        {
            public class Texture
            {
                private Dictionary<string, int> Indices = new Dictionary<string, int>();
                private List<uint> Pixels = new List<uint>();

                public int this[InterfaceImage image]
                {
                    get
                    {
                        var name = "~:/" + image.FrameCount + image.FrameDelay + image.VerticalFrames + "\\:~" + image.Texture.Link;
                        if (Indices.ContainsKey(name)) return Indices[name];

                        if (image.Texture.Resource == null) return -1;

                        var tex = image.Texture.Resource.Bitmap;
                        int tw = image.Texture.Resource.Bitmap.Width;
                        int th = image.Texture.Resource.Bitmap.Height;

                        var pixels = new uint[tw * th];
                        if (image.VerticalFrames)
                        {
                            th /= image.FrameCount;

                            for (int f = 0; f < image.FrameCount; f++)
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
                            tw /= image.FrameCount;

                            for (int f = 0; f < image.FrameCount; f++)
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


                        int index = Pixels.Count;

                        Pixels.Add((uint)tw);
                        Pixels.Add((uint)th);
                        Pixels.Add(image.FrameCount >= 0 ? (uint)image.FrameCount : (uint)(uint.MaxValue + image.FrameCount));
                        Pixels.Add(image.FrameDelay >= 0 ? (uint)image.FrameDelay : (uint)(uint.MaxValue + image.FrameDelay));
                        Pixels.AddRange(pixels);

                        return index;
                    }
                }
                public void Write(BinaryWriter w)
                {
                    w.Write(Pixels.Count);
                    foreach (var p in Pixels) w.Write(p);
                }
            }


            public enum Type : int
            {
                Element,
		        Panel,
                Label,
            }
            public enum Anchor : int
            {
                None,
                Lesser,
                Larger,
                Middle,
            }
            public struct Constraint
            {
                public Anchor AnchorU;
                public Anchor AnchorL;
                public Anchor AnchorD;
                public Anchor AnchorR;
                public int OffsetU;
                public int OffsetL;
                public int OffsetD;
                public int OffsetR;
            }
            public struct Image
            {
                public enum StretchType : int
                {
                    Solid,
                    Repeat,
                    Scale,
                    Glyph,
                };
                public uint Color;
                public int Texture;
                public StretchType Stretch;

                public Image(InterfaceImage img, Texture texture)
                {
                    Color = ((uint)img.Color.R << 24) | ((uint)img.Color.G << 16) | ((uint)img.Color.B << 8) |((uint)img.Color.A << 0);
                    Stretch = StretchType.Solid; 
                    if (img.Texture.Resource != null)
                    {
                        switch (img.Stretch)
                        {
                            case InterfaceTextureStretchType.Repeat: Stretch = StretchType.Repeat; break;
                            case InterfaceTextureStretchType.Scale: Stretch = StretchType.Scale; break;
                        }
                    }
                    Texture = texture[img];
                }
            }
            public struct Element 
            {
                public int PositionX;
                public int PositionY;
                public int ExtentX;
                public int ExtentY;
                public Constraint Constraints;
                public bool Resizable;
                public int Count;

                public Element(InterfaceElement elem)
                {
                    PositionX = elem.PositionX;
                    PositionY = elem.PositionY;
                    ExtentX = elem.ExtentX;
                    ExtentY = elem.ExtentY;

                    Constraints.AnchorU = (Anchor)elem.AnchorU;
                    Constraints.AnchorL = (Anchor)elem.AnchorL;
                    Constraints.AnchorD = (Anchor)elem.AnchorD;
                    Constraints.AnchorR = (Anchor)elem.AnchorR;

                    Constraints.OffsetU = elem.OffsetU;
                    Constraints.OffsetL = elem.OffsetL;
                    Constraints.OffsetD = elem.OffsetD;
                    Constraints.OffsetR = elem.OffsetR;

                    Resizable = elem.Resizable;

                    Count = elem.Elements.Count;
                }
                
                public void Write(BinaryWriter w)
                {
                    w.Write(PositionX);
                    w.Write(PositionY);
                    w.Write(ExtentX);
                    w.Write(ExtentY);
                    w.Write(Constraints);
                    w.Write(Resizable);
                    w.Write(Count);
                }
            }
            public struct Panel
            {
                public Element Base;
                public Image Image;

                public Panel(InterfacePanel elem, Texture texture)
                {
                    Base = new Element(elem);
                    Image = new Image(elem.Image, texture);
                }

                public void Write(BinaryWriter w)
                {
                    Base.Write(w);
                    w.Write(Image);
                }
            }
            public struct Label
            {
                public Element Base;
                public uint Color;
                public string Text;

                public Label(InterfaceLabel elem)
                {
                    Base = new Element(elem);
                    Color = ((uint)elem.Color.R << 24) | ((uint)elem.Color.G << 16) | ((uint)elem.Color.B << 8) | ((uint)elem.Color.A << 0);
                    Text = elem.Text;
                }

                public void Write(BinaryWriter w)
                {
                    Base.Write(w);
                    w.Write(Color);
                    byte[] utf8 = Encoding.UTF8.GetBytes(Text);
                    w.Write(utf8.Length);
                    w.Write(utf8);
                }
            }
            public struct Button
            {

            }

            public static List<object> Compile(InterfaceElement elem, Texture texture)
            {
                var elements = new List<object>(1);
                switch (elem.Type)
                {
                    case InterfaceElementType.Element:
                        elements.Add(new Element(elem)); 
                        break;

                    case InterfaceElementType.Panel:
                        elements.Add(new Panel((elem as InterfacePanel), texture));
                        break;

                    case InterfaceElementType.Label:
                        elements.Add(new Label(elem as InterfaceLabel));
                        break;

                    default: throw new Exception("Inventory.Compile error: Unsopported element type [" + elem.Type + "].");
                }
                foreach (var sub_elem in elem.Elements)
                    elements.AddRange(Compile(sub_elem as InterfaceElement, texture));

                return elements;
            }
        }
    }
}
