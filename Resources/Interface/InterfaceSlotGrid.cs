using ExtraForms.OpenGL;
using Resource_Redactor.Resources.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources.Interface
{
    public class InterfaceSlotGrid : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.SlotGrid;

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
                gl.Vertex2i(ox + PositionX, oy + PositionY + y * ExtentY / GridY);
                gl.Vertex2i(ox + PositionX + ExtentX, oy + PositionY + y * ExtentY / GridY);
            }
            gl.End();

            base.Render(ox, oy, time);
        }
    }
}
