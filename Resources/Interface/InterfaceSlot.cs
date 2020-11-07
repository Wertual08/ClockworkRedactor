using ExtraForms.OpenGL;
using Resource_Redactor.Resources.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources.Interface
{
    public class InterfaceSlot : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.Slot;

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
}
