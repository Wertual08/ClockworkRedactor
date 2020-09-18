using ExtraForms.OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources
{
    public class InventoryElementConverter : JsonConverter<InventoryResource.Element>
    {
        public override InventoryResource.Element Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject || !reader.Read())
                throw new Exception("InventoryElementConverter error: Invalid object format.");

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new Exception("InventoryElementConverter error: Invalid object format.");
            if (reader.GetString() != "ElementType" || !reader.Read())
                throw new Exception("InventoryElementConverter error: Invalid object format.");

            if (reader.TokenType != JsonTokenType.String)
                throw new Exception("InventoryElementConverter error: Invalid object format.");
            var type = Type.GetType(reader.GetString());
            if (!reader.Read()) throw new Exception("InventoryElementConverter error: Invalid object format.");

            if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
                throw new Exception("InventoryElementConverter error: Invalid object format.");

            var result = JsonSerializer.Deserialize(ref reader, type, options);

            if (reader.TokenType != JsonTokenType.EndObject || !reader.Read())
                throw new Exception("InventoryElementConverter error: Invalid object format.");

            return result as InventoryResource.Element;
        }
        public override void Write(Utf8JsonWriter writer, InventoryResource.Element value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("ElementType", value.GetType().AssemblyQualifiedName);
            writer.WritePropertyName("Element");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }
    public class InventoryResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Inventory;
        public static readonly string CurrentVersion = "0.0.0.0";

        //public class Slot
        //{
        //    public string Appliance { get; set; } = "";
        //    public bool NotifyOwner { get; set; } = false;
        //    public int Capacity { get; set; } = 999;
        //}
        public enum ElementType : int
        {
            None,
            Panel,
            Dragger,
            Button,
            Container,
            Indicator,
            List,
            Slider,
            Text,
            Slot,
            SlotGrid,
        }
        [Flags]
        public enum ElementAnchors : int
        {
            Up = 0b1,
            Left = 0b10,
            Down = 0b100,
            Right = 0b1000,
            Every = 0b1111,
            None = 0b0,
        }
        public enum DragActionType : int
        {
            None,
            Drag,
            DragParrent,
            ResizeU,
            ResizeL,
            ResizeD,
            ResizeR,
            ResizeUL,
            ResizeLD,
            ResizeDR,
            ResizeRU,
        }
        public enum TextureStretchType
        {
            Repeat,
            Scale,
        }

        public class ElementsList : IList<Element>
        {
            private Element OwnerStorage = null;
            private List<Element> Elements = new List<Element>();

            public ElementsList() { }
            public ElementsList(IList<Element> list)
            {
                foreach (var element in list) Add(element);
            }

            public Element Owner { 
                get => OwnerStorage;
                set 
                {
                    OwnerStorage = value;
                    foreach (var element in Elements) element.Owner = OwnerStorage;
                } 
            }

            public Element this[int index] 
            { 
                get => Elements[index]; 
                set
                {
                    value.Owner = OwnerStorage;
                    Elements[index] = value;
                }

            }

            public int Count => Elements.Count;

            public bool IsReadOnly => false;

            public void Add(Element item)
            {
                item.Owner = Owner;
                Elements.Add(item);
            }
        
            public void Clear()
            {
                Elements.Clear();
            }
        
            public bool Contains(Element item)
            {
                return Elements.Contains(item);
            }
        
            public void CopyTo(Element[] array, int arrayIndex)
            {
                Elements.CopyTo(array, arrayIndex);
            }
        
            public IEnumerator<Element> GetEnumerator()
            {
                return Elements.GetEnumerator();
            }
        
            public int IndexOf(Element item)
            {
                return Elements.IndexOf(item);
            }
        
            public void Insert(int index, Element item)
            {
                item.Owner = Owner;
                Elements.Insert(index, item);
            }
        
            public bool Remove(Element item)
            {
                if (item.Owner == Owner) item.Owner = null;
                return Elements.Remove(item);
            }
        
            public void RemoveAt(int index)
            {
                var item = Elements[index];
                if (item.Owner == Owner) item.Owner = null;
                Elements.RemoveAt(index);
            }
        
            IEnumerator IEnumerable.GetEnumerator()
            {
                return Elements.GetEnumerator();
            }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class Image : IDisposable
        {
            private Subresource<TextureResource> TextureStorage = new Subresource<TextureResource>();

            public Color Color { get; set; } = Color.White;
            [EditorAttribute(typeof(UISubresourceEditor), typeof(UITypeEditor))]
            [TypeConverter(typeof(SubresourceConverter<Subresource<TextureResource>>))]
            public Subresource<TextureResource> Texture
            {
                get => TextureStorage;
                set { TextureStorage?.Dispose(); TextureStorage = value; }
            }
            public int FrameCount { get; set; } = 1;
            public int FrameDelay { get; set; } = 1000;
            public bool VerticalFrames { get; set; } = false;
            public TextureStretchType Stretch { get; set; } = TextureStretchType.Repeat;

            protected bool IsDisposed { get; private set; } = false;
            ~Image()
            {
                Dispose(false);
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected virtual void Dispose(bool disposing)
            {
                if (disposing) TextureStorage.Dispose();

                IsDisposed = true;
            }

            public override string ToString()
            {
                return TextureStorage.Link;
            }

            public void Render(int px, int py, int ex, int ey, long time)
            {
                var texture = Texture.Resource;
                if (texture != null)
                {
                    float tx = 0;
                    float ty = 0;
                    float tw = 1;
                    float th = 1;
                    int iw = texture.Texture.Width;
                    int ih = texture.Texture.Height;
                    int frame = (int)(time / FrameDelay % FrameCount);

                    if (VerticalFrames)
                    {
                        ty = frame;
                        th = 1f / FrameCount;
                        ih /= FrameCount;
                    }
                    else
                    {
                        tx = frame;
                        tw = 1f / FrameCount;
                        iw /= FrameCount;
                    }

                    gl.Enable(GL.TEXTURE_2D);
                    texture.Bind();

                    gl.Color4ub(Color.R, Color.G, Color.B, Color.A);
                    gl.Begin(GL.QUADS);

                    switch (Stretch)
                    {
                        case TextureStretchType.Repeat:
                            for (int x = 0; x < (ex + iw - 1) / iw; x++)
                            {
                                for (int y = 0; y < (ey + ih - 1) / ih; y++)
                                {
                                    int qx = px + x * iw;
                                    int qy = py + y * ih;
                                    int qw = Math.Min(ex - x * iw, iw);
                                    int qh = Math.Min(ey - y * ih, ih);

                                    gl.TexCoord2f(tx * tw, ty * th);
                                    gl.Vertex2i(qx, qy);
                                    gl.TexCoord2f((tx + (float)qw / iw) * tw, ty * th);
                                    gl.Vertex2i(qx + qw, qy);
                                    gl.TexCoord2f((tx + (float)qw / iw) * tw, (ty + (float)qh / ih) * th);
                                    gl.Vertex2i(qx + qw, qy + qh);
                                    gl.TexCoord2f(tx * tw, (ty + (float)qh / ih) * th);
                                    gl.Vertex2i(qx, qy + qh);
                                }
                            }
                            break;

                        case TextureStretchType.Scale:
                            gl.TexCoord2f(tx * tw, ty * th);
                            gl.Vertex2i(px, py);
                            gl.TexCoord2f((tx + 1) * tw, ty * th);
                            gl.Vertex2i(px + ex, py);
                            gl.TexCoord2f((tx + 1) * tw, (ty + 1) * th);
                            gl.Vertex2i(px + ex, py + ey);
                            gl.TexCoord2f(tx * tw, (ty + 1) * th);
                            gl.Vertex2i(px, py + ey);
                            break;
                    }

                    gl.End();
                }
                else
                {
                    gl.Disable(GL.TEXTURE_2D);
                    gl.Color4ub(Color.R, Color.G, Color.B, Color.A);
                    gl.Begin(GL.QUADS);
                    gl.Vertex2i(px, py);
                    gl.Vertex2i(px + ex, py);
                    gl.Vertex2i(px + ex, py + ey);
                    gl.Vertex2i(px, py + ey);
                    gl.End();
                }
            }
        }
        [JsonConverter(typeof(InventoryElementConverter))]
        public class Element : IDisposable
        {
            public virtual ElementType Type => ElementType.None;
            public string Name { get; set; } = "";
            public int PositionX { get; set; } = 0;
            public int PositionY { get; set; } = 0;
            public int ExtentX { get; set; } = 16;
            public int ExtentY { get; set; } = 16;
            public ElementAnchors Anchors { get; set; } = ElementAnchors.Every;
            public bool Resizable { get; set; } = false;
            [JsonIgnore]
            public Element Owner { get; set; } = null;
            [JsonIgnore]
            public int DesignerPositionX
            {
                get => PositionX + (Owner?.DesignerPositionX ?? 0);
                set => PositionX = value - (Owner?.DesignerPositionX ?? 0);
            }
            [JsonIgnore]
            public int DesignerPositionY
            {
                get => PositionY + (Owner?.DesignerPositionY ?? 0);
                set => PositionY = value - (Owner?.DesignerPositionY ?? 0);
            }

            protected bool IsDisposed { get; private set; } = false;
            ~Element()
            {
                Dispose(false);
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected virtual void Dispose(bool disposing)
            {
                IsDisposed = true;
            }

            public virtual void Render(int ox, int oy, long time)
            {
                gl.Disable(GL.TEXTURE_2D);
                gl.Color4ub(0, 255, 255, 255);
                gl.Begin(GL.LINE_LOOP);
                gl.Vertex2i(ox + PositionX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY + ExtentY);
                gl.Vertex2i(ox + PositionX, oy + PositionY + ExtentY);
                gl.End();
            }
        }
        public class Panel : Element
        {
            public override ElementType Type => ElementType.Panel;
            public Image Image { get; set; } = new Image();

            protected override void Dispose(bool disposing)
            {
                if (disposing) Image.Dispose();

                base.Dispose(disposing);
            }

            public override void Render(int ox, int oy, long time)
            {
                Image.Render(ox + PositionX, oy + PositionY, ExtentX, ExtentY, time);

                base.Render(ox, oy, time);
            }
        }
        public class Dragger : Element
        {
            public override ElementType Type => ElementType.Dragger;
            public DragActionType DragAction { get; set; } = DragActionType.None;
        }
        public class Button : Element
        {
            public override ElementType Type => ElementType.Button;
            public Image SelectedImage { get; set; } = new Image();
            public Image PressedImage { get; set; } = new Image();
            public Image ReleasedImage { get; set; } = new Image();

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    SelectedImage.Dispose();
                    PressedImage.Dispose();
                    ReleasedImage.Dispose();
                }

                base.Dispose(disposing);
            }

            public override void Render(int ox, int oy, long time)
            {
                ReleasedImage.Render(ox + PositionX, oy + PositionY, ExtentX, ExtentY, time);

                base.Render(ox, oy, time);
            }
        }
        public class Container : Element
        {
            private ElementsList ElementsStorage = new ElementsList();

            public override ElementType Type => ElementType.Container;

            public IList<Element> Elements { 
                get => ElementsStorage; 
                set
                {
                    var elements = value as ElementsList;
                    if (elements == null) elements = new ElementsList(value);
                    elements.Owner = this;
                    ElementsStorage = elements;
                }
            }
            
            public Container()
            {
                ElementsStorage.Owner = this;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing) foreach (var element in Elements) element.Dispose();

                base.Dispose(disposing);
            }

            public override void Render(int ox, int oy, long time)
            {
                foreach (var element in Elements) element.Render(ox + PositionX, oy + PositionY, time);

                base.Render(ox, oy, time);
            }
        }
        public class List : Container
        {
            public override ElementType Type => ElementType.List;

            public bool Horizontal { get; set; } = false;
            public int ItemSize { get; set; } = 16;

            protected override void Dispose(bool disposing)
            {
                if (disposing) foreach (var element in Elements) element.Dispose();

                base.Dispose(disposing);
            }

            public override void Render(int ox, int oy, long time)
            {
                gl.Disable(GL.TEXTURE_2D);
                gl.Color4ub(128, 0, 255, 128);
                gl.Begin(GL.QUADS);
                if (Horizontal)
                {
                    gl.Vertex2i(ox + PositionX, oy + PositionY + ExtentY);
                    gl.Vertex2i(ox + PositionX, oy + PositionY);
                    gl.Vertex2i(ox + PositionX + ItemSize, oy + PositionY);
                    gl.Vertex2i(ox + PositionX + ItemSize, oy + PositionY + ExtentY);
                }
                else
                {
                    gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY);
                    gl.Vertex2i(ox + PositionX, oy + PositionY);
                    gl.Vertex2i(ox + PositionX, oy + PositionY + ItemSize);
                    gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY + ItemSize);
                }
                gl.End();

                gl.Color4ub(0, 255, 255, 255);
                gl.Begin(GL.LINES);
                if (Horizontal)
                {
                    gl.Vertex2i(ox + PositionX + ItemSize, oy + PositionY);
                    gl.Vertex2i(ox + PositionX + ItemSize, oy + PositionY + ExtentY);
                }
                else
                {
                    gl.Vertex2i(ox + PositionX, oy + PositionY + ItemSize);
                    gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY + ItemSize);
                }
                gl.End();

                base.Render(ox, oy, time);
            }
        }
        public class Slider : Element
        {
            public override ElementType Type => ElementType.Slider;
            public Image BackgroundImage { get; set; } = new Image();
            public Image SliderImage { get; set; } = new Image();
            public int SliderStartX { get; set; } = 0;
            public int SliderStartY { get; set; } = 0;
            public int SliderLengthX { get; set; } = 0;
            public int SliderLengthY { get; set; } = 0;
            public int SliderExtentX { get; set; } = 16;
            public int SliderExtentY { get; set; } = 16;

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    BackgroundImage.Dispose();
                    SliderImage.Dispose();
                }

                base.Dispose(disposing);
            }
        }
        public class Text : Element
        {
            public override ElementType Type => ElementType.Text;

            public override void Render(int ox, int oy, long time)
            {
                gl.Disable(GL.TEXTURE_2D);
                gl.Color4ub(255, 0, 0, 128);
                gl.Begin(GL.QUADS);
                gl.Vertex2i(ox + PositionX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY + ExtentY);
                gl.Vertex2i(ox + PositionX, oy + PositionY + ExtentY);
                gl.End();

                base.Render(ox, oy, time);
            }
        }
        public class Slot : Element
        {
            public override ElementType Type => ElementType.Slot;

            public override void Render(int ox, int oy, long time)
            {
                gl.Disable(GL.TEXTURE_2D);
                gl.Color4ub(0, 255, 255, 128);
                gl.Begin(GL.QUADS);
                gl.Vertex2i(ox + PositionX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY + ExtentY);
                gl.Vertex2i(ox + PositionX, oy + PositionY + ExtentY);
                gl.End();

                base.Render(ox, oy, time);
            }
        }
        public class SlotGrid : Element
        {
            public override ElementType Type => ElementType.SlotGrid;

            public int GridX { get; set; } = 1;
            public int GridY { get; set; } = 1;

            public override void Render(int ox, int oy, long time)
            {
                gl.Disable(GL.TEXTURE_2D);
                gl.Color4ub(0, 255, 255, 128);
                gl.Begin(GL.QUADS);
                gl.Vertex2i(ox + PositionX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY + ExtentY);
                gl.Vertex2i(ox + PositionX, oy + PositionY + ExtentY);
                gl.End();

                gl.Color4ub(0, 255, 255, 255);
                gl.Begin(GL.LINES);
                for (int x = 1; x < GridX; x++)
                {
                    gl.Vertex2i(ox + PositionX + x * ExtentX / GridX, oy + PositionY);
                    gl.Vertex2i(ox + PositionX + x * ExtentX / GridX, oy + PositionY + ExtentY);
                }
                for (int y = 1; y < GridY; y++)
                {
                    gl.Vertex2i(ox + PositionX , oy + PositionY + y * ExtentY / GridY);
                    gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY + y * ExtentY / GridY);
                }
                gl.End();

                base.Render(ox, oy, time);
            }
        }

        public static Element Factory(ElementType type)
        {
            switch (type)
            {
                case ElementType.Panel: return new Panel();
                case ElementType.Button: return new Button();
                case ElementType.Container: return new Container();
                //case ElementType.Indicator: return new Indicator();
                case ElementType.List: return new List();
                case ElementType.Slider: return new Slider();
                case ElementType.Text: return new Text();
                case ElementType.Slot: return new Slot();
                case ElementType.SlotGrid: return new SlotGrid();
                default: throw new Exception("InventoryResource error: Invalid element type for factory.");
            }
        }

        // Resource
        public int Width { get; set; } = 512;
        public int Height { get; set; } = 384;
        public bool Resizable { get; set; } = false;
        public IList<Element> Elements { get; set; } = new ElementsList();

        // Redactor
        public Color BackColor { get; set; } = Color.Black;

        [JsonIgnore]
        public int Count { get => Elements.Count; }
        public Element this[int i] { get => Elements[i]; set => Elements[i] = value; }

        public InventoryResource() : base(CurrentType, CurrentVersion)
        {

        }
        public InventoryResource(string path) : base(path)
        {

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var element in Elements) element.Dispose();
            }

            base.Dispose(disposing);
        }

        public void Render(int ox, int oy, long time)
        {
            gl.Disable(GL.TEXTURE_2D);
            gl.Color4ub(255, 0, 0, 255);
            gl.Begin(GL.LINE_LOOP);
            gl.Vertex2i(ox, oy);
            gl.Vertex2i(ox + Width, oy);
            gl.Vertex2i(ox + Width, oy + Height);
            gl.Vertex2i(ox, oy + Height);
            gl.End();

            foreach (var element in Elements) element.Render(ox, oy, time);
        }
    }
}
