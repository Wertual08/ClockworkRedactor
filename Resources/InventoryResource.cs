using ExtraForms.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public class Slot
        {
            public string Appliance { get; set; } = "";
            public bool NotifyOwner { get; set; } = false;
            public int Capacity { get; set; } = 999;
        }
        public enum ElementType : int
        {
            None,
            Panel,
            Button,
            Slot,
            SlotGrid,
            Indicator,
            List,
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
            DragInventory,
            HorizontalResize,
            VerticalResize,
            Resize,
        }

        [JsonConverter(typeof(InventoryElementConverter))]
        public class Element : IDisposable
        {
            public virtual ElementType Type => ElementType.None;
            public int PositionX { get; set; } = 0;
            public int PositionY { get; set; } = 0;
            public int Width { get; set; } = 16;
            public int Height { get; set; } = 16;
            public ElementAnchors Anchors { get; set; } = ElementAnchors.Every;
            public bool Resizable { get; set; } = false;

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

            public virtual void Render(int ox, int oy)
            {
                gl.Disable(GL.TEXTURE_2D);
                gl.Color4ub(0, 255, 255, 255);
                gl.Begin(GL.LINE_LOOP);
                gl.Vertex2i(ox + PositionX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + Width, oy + PositionY);
                gl.Vertex2i(ox + PositionX + Width, oy + PositionY + Height);
                gl.Vertex2i(ox + PositionX, oy + PositionY + Height);
                gl.End();
            }
        }
        public class Panel : Element
        {
            public override ElementType Type => ElementType.Panel;
            public Subresource<TextureResource> Texture { get; set; } = new Subresource<TextureResource>();
            public TextureStretch Stretch { get; set; } = TextureStretch.Repeat;
            public DragActionType DragAction { get; set; } = DragActionType.None;

            protected override void Dispose(bool disposing)
            {
                if (disposing) Texture.Dispose();

                base.Dispose(disposing);
            }

            public override void Render(int ox, int oy)
            {
                var texture = Texture.Resource;
                if (texture != null)
                {
                    gl.Enable(GL.TEXTURE_2D);
                    texture.StretchMode = Stretch;
                    texture.Bind();
                }
                else gl.Disable(GL.TEXTURE_2D);

                gl.Color4ub(255, 255, 255, 255);
                gl.Begin(GL.QUADS);
                gl.Vertex2i(ox + PositionX, oy + PositionY);
                gl.Vertex2i(ox + PositionX + Width, oy + PositionY);
                gl.Vertex2i(ox + PositionX + Width, oy + PositionY + Height);
                gl.Vertex2i(ox + PositionX, oy + PositionY + Height);
                gl.End();

                base.Render(ox, oy);
            }
        }

        // Resource
        public int Width { get; set; } = 512;
        public int Height { get; set; } = 384;
        public bool Resizable { get; set; } = false;
        public List<Element> Elements { get; set; } = new List<Element>();

        // Redactor
        public int BackColor { get; set; } = Color.Black.ToArgb();

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

            foreach (var element in Elements) element.Render(ox, oy);
        }
    }
}
