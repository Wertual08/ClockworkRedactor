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
    class InterfaceCompiler
    {
        private enum Type : int
        {
            Element,
            Panel,
            Label,
        }
        private enum Anchor : int
        {
            None,
            Lesser,
            Larger,
            Middle,
        }
        private struct Constraint
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
        private struct Image
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

            public Image(InterfaceImage img, TextureCompiler texture)
            {
                Color = ((uint)img.Color.R << 24) | ((uint)img.Color.G << 16) | ((uint)img.Color.B << 8) | ((uint)img.Color.A << 0);
                Stretch = StretchType.Solid;
                if (img.Texture.Resource != null)
                {
                    switch (img.Stretch)
                    {
                        case InterfaceTextureStretchType.Repeat: Stretch = StretchType.Repeat; break;
                        case InterfaceTextureStretchType.Scale: Stretch = StretchType.Scale; break;
                    }
                }
                int w = img.Texture.Resource.Bitmap.Width;
                int h = img.Texture.Resource.Bitmap.Height;
                if (img.VerticalFrames) h /= img.FrameCount;
                else w /= img.FrameCount;
                Texture = texture.FindLoad(img.Texture.Link, (Bitmap tex) =>
                {
                    int tw = tex.Width;
                    int th = tex.Height;

                    var pixels = new uint[tw * th];
                    if (img.VerticalFrames)
                    {
                        th /= img.FrameCount;

                        for (int f = 0; f < img.FrameCount; f++)
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
                        tw /= img.FrameCount;

                        for (int f = 0; f < img.FrameCount; f++)
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

                    return pixels;
                },
                new uint[] {
                    (uint)w,
                    (uint)h,
                    img.FrameCount >= 0 ? (uint)img.FrameCount : (uint)(uint.MaxValue + img.FrameCount),
                    img.FrameDelay >= 0 ? (uint)img.FrameDelay : (uint)(uint.MaxValue + img.FrameDelay)
                });
            }
        }
        private struct Element
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
        private struct Panel
        {
            public Element Base;
            public Image Image;

            public Panel(InterfacePanel elem, TextureCompiler texture)
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
        private struct Label
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
        private struct Button
        {

        }

        private List<object> Compile(InterfaceElement elem)
        {
            var elements = new List<object>(16);
            switch (elem.Type)
            {
                case InterfaceElementType.Element:
                    elements.Add(new Element(elem));
                    break;

                case InterfaceElementType.Panel:
                    elements.Add(new Panel((elem as InterfacePanel), Texture));
                    break;

                case InterfaceElementType.Label:
                    elements.Add(new Label(elem as InterfaceLabel));
                    break;

                default: throw new Exception($"Inventory.Compile error: Unsopported element type [{elem.Type}]."); 
            }
            foreach (var sub_elem in elem.Elements)
                elements.AddRange(Compile(sub_elem as InterfaceElement));

            return elements;
        }

        private IDTable Table;
        private MessageQueue LogQueue;

        private TextureCompiler Texture = new TextureCompiler();
        private List<int> Indexes = new List<int>();
        private List<object> Elements = new List<object>();

        public InterfaceCompiler(IDTable table, MessageQueue log_queue)
        {
            Table = table;
            LogQueue = log_queue;
        }

        public void Compile(string path, int id)
        {
            LogQueue.Put($"Compiling [{path}]...");
            InterfaceResource res = null;
            try { res = new InterfaceResource(path); }
            catch
            {
                LogQueue.Put($"Interface [{path}] was not found.");
                return;
            }

            int index = Elements.Count;
            Elements.AddRange(Compile(res.BaseElement as InterfaceElement));
            res.Dispose();

            while (Indexes.Count <= id) Indexes.Add(-1);
            Indexes[id] = index;
        }
        public void Write(BinaryWriter w)
        {
            w.Write(Indexes.Count);
            foreach (var i in Indexes)
                w.Write(i);
            w.Write(Elements.Count);
            foreach (var e in Elements)
            {
                if (e.GetType() == typeof(Element))
                {
                    w.Write((int)Type.Element);
                    ((Element)e).Write(w);
                }
                else if (e.GetType() == typeof(Panel))
                {
                    w.Write((int)Type.Panel);
                    ((Panel)e).Write(w);
                }
                else if (e.GetType() == typeof(Label))
                {
                    w.Write((int)Type.Label);
                    ((Label)e).Write(w);
                }
            }
        }
        public void WriteTexture(BinaryWriter w)
        {
            Texture.Write(w);
        }
    }
}
