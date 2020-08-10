using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources
{
    public class InventoryResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Outfit;
        public static readonly string CurrentVersion = "0.0.0.0";

        public class Slot
        {
            public string Appliance { get; set; } = "";
            public bool NotifyOwner { get; set; } = false;
            public int Capacity { get; set; } = 999;
        }
        public enum ElementType : int
        {
            Panel,
            Border,
            Button,
            Slot,
            SlotGrid,
            Indicator,
            List,
        }
        [Flags]
        public enum ElementAnchors : int
        {
            Up = 0b1,
            Left = 0b10,
            Down = 0b100,
            Right = 0b1000,
            Every = 0b1111,
            None = 0b0,
        }
        public class Element
        {
            public int PositionX { get; set; } = 0;
            public int PositionY { get; set; } = 0;
            public int Width { get; set; } = 16;
            public int Height { get; set; } = 16;
            public ElementAnchors Anchors { get; set; } = ElementAnchors.Every;
            public bool Resizable { get; set; } = false;
        }
        public class Panel : Element
        {
        }

        // Resource

        // Redactor
        public int BackColor { get; set; } = Color.Black.ToArgb();

        public InventoryResource() : base(CurrentType, CurrentVersion)
        {

        }
        public InventoryResource(string path) : base(path)
        {

        }
    }
}
