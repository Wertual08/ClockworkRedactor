using ExtraForms.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ResrouceRedactor.Resources.Interface
{
    public enum InterfaceElementType : int
    {
        Element,
        Panel,
        Label,
        Dragger,
        Item,
        Button,
        Indicator,
        List,
        Slider,
        Slot,
        SlotGrid,
    }
    [Flags]
    public enum InterfaceAnchor : int
    {
        None,
        Lesser,
        Larger,
        Middle,
    };

    public class InterfaceElementConverter : JsonConverter<InterfaceElementBase>
    {
        public override InterfaceElementBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject || !reader.Read())
                throw new Exception("InterfaceElementConverter error: Invalid object format.");

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new Exception("InterfaceElementConverter error: Invalid object format.");
            if (reader.GetString() != "ElementType" || !reader.Read())
                throw new Exception("InterfaceElementConverter error: Invalid object format.");

            if (reader.TokenType != JsonTokenType.String)
                throw new Exception("InterfaceElementConverter error: Invalid object format.");

            string str_type = reader.GetString();
            InterfaceElementType type;
            if (!Enum.TryParse(str_type, out type))
                throw new Exception("Invalid element type [" + str_type + "].");

            if (!reader.Read()) throw new Exception("InterfaceElementConverter error: Invalid object format.");
            

            if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
                throw new Exception("InterfaceElementConverter error: Invalid object format.");

            var result = JsonSerializer.Deserialize(ref reader, InterfaceElementBase.GetType(type), options);

            if (reader.TokenType != JsonTokenType.EndObject || !reader.Read())
                throw new Exception("InterfaceElementConverter error: Invalid object format.");

            return result as InterfaceElementBase;
        }
        public override void Write(Utf8JsonWriter writer, InterfaceElementBase value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("ElementType", value.Type.ToString());
            writer.WritePropertyName("Element");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    [JsonConverter(typeof(InterfaceElementConverter))]
    abstract public class InterfaceElementBase : IDisposable
    {
        public static InterfaceElement Factory(InterfaceElementType type)
        {
            switch (type)
            {
                case InterfaceElementType.Element: return new InterfaceElement();
                case InterfaceElementType.Panel: return new InterfacePanel();
                case InterfaceElementType.Label: return new InterfaceLabel();
                case InterfaceElementType.Button: return new InterfaceButton();
                case InterfaceElementType.List: return new InterfaceList();
                case InterfaceElementType.Slider: return new InterfaceSlider();
                case InterfaceElementType.Slot: return new InterfaceSlot();
                case InterfaceElementType.SlotGrid: return new InterfaceSlotGrid();
                default: throw new Exception("InterfaceElement error: Invalid element type for factory.");
            }
        }
        public static Type GetType(InterfaceElementType type)
        {
            switch (type)
            {
                case InterfaceElementType.Element: return typeof(InterfaceElement);
                case InterfaceElementType.Panel: return typeof(InterfacePanel);
                case InterfaceElementType.Label: return typeof(InterfaceLabel);
                case InterfaceElementType.Button: return typeof(InterfaceButton);
                case InterfaceElementType.List: return typeof(InterfaceList);
                case InterfaceElementType.Slider: return typeof(InterfaceSlider);
                case InterfaceElementType.Slot: return typeof(InterfaceSlot);
                case InterfaceElementType.SlotGrid: return typeof(InterfaceSlotGrid);
                default: throw new Exception("InterfaceElement error: Invalid element type for factory.");
            }
        }

        protected InterfaceElementsList ElementsStorage;
        protected int PPositionX = 0;
        protected int PPositionY = 0;
        protected int PExtentY = 16;
        protected int PExtentX = 16;
        protected InterfaceAnchor PAnchorU { get; set; } = InterfaceAnchor.None;
        protected InterfaceAnchor PAnchorL { get; set; } = InterfaceAnchor.Lesser;
        protected InterfaceAnchor PAnchorD { get; set; } = InterfaceAnchor.Lesser;
        protected InterfaceAnchor PAnchorR { get; set; } = InterfaceAnchor.None;
        protected void AdjustConstraints()
        {
            if (Owner != null && AutoConstraints)
            {
                if (PAnchorU == InterfaceAnchor.Larger) OffsetU = PPositionY + PExtentY - Owner.ExtentY;
                if (PAnchorU == InterfaceAnchor.Middle) OffsetU = PPositionY + PExtentY - Owner.ExtentY / 2;
                if (PAnchorU == InterfaceAnchor.Lesser) OffsetU = PPositionY + PExtentY;

                if (PAnchorL == InterfaceAnchor.Larger) OffsetL = PPositionX - Owner.ExtentY;
                if (PAnchorL == InterfaceAnchor.Middle) OffsetL = PPositionX - Owner.ExtentY / 2;
                if (PAnchorL == InterfaceAnchor.Lesser) OffsetL = PPositionX;

                if (PAnchorD == InterfaceAnchor.Larger) OffsetD = PPositionY - Owner.ExtentY;
                if (PAnchorD == InterfaceAnchor.Middle) OffsetD = PPositionY - Owner.ExtentY / 2;
                if (PAnchorD == InterfaceAnchor.Lesser) OffsetD = PPositionY;

                if (PAnchorR == InterfaceAnchor.Larger) OffsetR = PPositionX + PExtentX - Owner.ExtentX;
                if (PAnchorR == InterfaceAnchor.Middle) OffsetR = PPositionX + PExtentX - Owner.ExtentX / 2;
                if (PAnchorR == InterfaceAnchor.Lesser) OffsetR = PPositionX + PExtentX;
            }
        }

        public virtual InterfaceElementType Type => InterfaceElementType.Element;
        public string Name { get; set; } = "";
        public int PositionX { get => PPositionX; set { PPositionX = value; AdjustConstraints(); } }
        public int PositionY { get => PPositionY; set { PPositionY = value; AdjustConstraints(); } }
        public int ExtentX { get => PExtentX; set { PExtentX = value; AdjustConstraints(); } }
        public int ExtentY { get => PExtentY; set { PExtentY = value; AdjustConstraints(); } }
        public InterfaceAnchor AnchorU { get => PAnchorU; set { PAnchorU = value; AdjustConstraints(); } }
        public InterfaceAnchor AnchorL { get => PAnchorL; set { PAnchorL = value; AdjustConstraints(); } }
        public InterfaceAnchor AnchorD { get => PAnchorD; set { PAnchorD = value; AdjustConstraints(); } }
        public InterfaceAnchor AnchorR { get => PAnchorR; set { PAnchorR = value; AdjustConstraints(); } }
        public int OffsetU { get; set; } = 0;
        public int OffsetL { get; set; } = 0;
        public int OffsetD { get; set; } = 0;
        public int OffsetR { get; set; } = 0;
        public bool Resizable { get; set; } = false;
        public IList<InterfaceElementBase> Elements
        {
            get => ElementsStorage;
            set
            {
                var elements = value as InterfaceElementsList;
                if (elements == null) elements = new InterfaceElementsList(value);
                elements.Owner = this;
                ElementsStorage = elements;
            }
        }
        [JsonIgnore]
        public InterfaceElementBase Owner { get; set; } = null;
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
        // Redactor
        public bool AutoConstraints { get; set; } = true;

        protected bool IsDisposed { get; private set; } = false;
        public InterfaceElementBase()
        {
            ElementsStorage = new InterfaceElementsList(this);
        }
        ~InterfaceElementBase()
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
            if (IsDisposed) return;

            if (disposing)
            {
                foreach (var element in Elements)
                    element.Dispose();
            }

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

            foreach (var element in Elements) element.Render(ox + PositionX, oy + PositionY, time);
        }
    }

    public class InterfaceElement : InterfaceElementBase
    {
    }
}
