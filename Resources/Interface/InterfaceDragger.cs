using Resource_Redactor.Resources.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources.Interface
{
    public enum InterfaceDragActionType : int
    {
        None,
        Drag,
        DragParrent,
        ResizeU,
        ResizeL,
        ResizeD,
        ResizeR,
        ResizeUL,
        ResizeLD,
        ResizeDR,
        ResizeRU,
    }

    public class InterfaceDragger : InterfaceElement
    {
        public override InterfaceElementType Type => InterfaceElementType.Dragger;
        public InterfaceDragActionType DragAction { get; set; } = InterfaceDragActionType.None;
    }
}
