using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources.Interface
{
    public class InterfacePanel : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.Panel;
        public InterfaceImage Image { get; set; } = new InterfaceImage();

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
}
