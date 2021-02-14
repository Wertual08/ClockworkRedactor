using ExtraForms.OpenGL;
using ResrouceRedactor.Resources.Interface;
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

namespace ResrouceRedactor.Resources
{
    public class InterfaceResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Interface;
        public static readonly string CurrentVersion = "0.0.0.0";

        // Resource
        public InterfaceElementBase BaseElement { get; set; } = new InterfaceElement();

        // Redactor
        public Color BackColor { get; set; } = Color.Black;

        [JsonIgnore]
        public int Count { get => BaseElement.Elements.Count; }
        public InterfaceElementBase this[int i] { get => BaseElement.Elements[i]; set => BaseElement.Elements[i] = value; }

        public InterfaceResource() : base(CurrentType, CurrentVersion)
        {

        }
        public InterfaceResource(string path) : base(path)
        {

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BaseElement.Dispose();
            }

            base.Dispose(disposing);
        }

        public void Render(int ox, int oy, long time)
        {
            gl.Disable(GL.TEXTURE_2D);
            gl.Color4ub(255, 0, 0, 255);
            gl.Begin(GL.LINE_LOOP);
            gl.Vertex2i(ox, oy);
            gl.Vertex2i(ox + BaseElement.ExtentX, oy);
            gl.Vertex2i(ox + BaseElement.ExtentX, oy + BaseElement.ExtentY);
            gl.Vertex2i(ox, oy + BaseElement.ExtentY);
            gl.End();

            BaseElement.Render(ox, oy, time);
        }
    }
}
