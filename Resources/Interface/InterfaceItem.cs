using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResrouceRedactor.Resources.Interface
{
    class InterfaceItem : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.Item;
        //public Subresource<ItemResource> Item { get; set; } = new Subresource<ItemResource>();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Item.Dispose();
            }

            base.Dispose(disposing);
        }

        public override void Render(int ox, int oy, long time)
        {
            

            base.Render(ox, oy, time);
        }
    }
}
