using ExtraForms.OpenGL;
using Resource_Redactor.Resources.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources.Interface
{
    public class InterfaceList : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.List;

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
}
