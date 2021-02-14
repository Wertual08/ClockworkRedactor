using ExtraForms.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResrouceRedactor.Resources.Interface
{
    public class InterfaceLabel : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.Label;

        public string Text { get; set; } = "";
        public Color Color { get; set; } = Color.Black;

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
}
