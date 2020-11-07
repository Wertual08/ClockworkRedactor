using ExtraForms.OpenGL;
using Resource_Redactor.Resources.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources.Interface
{
    public class InterfaceText : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.Text;

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
