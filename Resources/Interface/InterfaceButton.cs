using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResrouceRedactor.Resources.Interface
{
    public class InterfaceButton : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.Button;
        public InterfaceImage SelectedImage { get; set; } = new InterfaceImage();
        public InterfaceImage PressedImage { get; set; } = new InterfaceImage();
        public InterfaceImage ReleasedImage { get; set; } = new InterfaceImage();

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
}
