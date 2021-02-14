using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResrouceRedactor.Resources.Interface
{
    public class InterfaceSlider : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.Slider;
        public InterfaceImage BackgroundImage { get; set; } = new InterfaceImage();
        public InterfaceImage SliderImage { get; set; } = new InterfaceImage();
        public int SliderStartX { get; set; } = 0;
        public int SliderStartY { get; set; } = 0;
        public int SliderLengthX { get; set; } = 0;
        public int SliderLengthY { get; set; } = 0;
        public int SliderExtentX { get; set; } = 16;
        public int SliderExtentY { get; set; } = 16;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BackgroundImage.Dispose();
                SliderImage.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
