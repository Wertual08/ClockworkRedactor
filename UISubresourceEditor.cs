using Resource_Redactor.Resources;
using Resource_Redactor.Resources.Redactors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace Resource_Redactor
{
    class UISubresourceEditor : UITypeEditor
    {
        private SubresourceTextBox DropDownControl = new SubresourceTextBox();

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var subresource = value as ISubresource;
            if (subresource == null) return value;

            var edSvc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (edSvc != null)
            {
                DropDownControl.Subresource = subresource;
                edSvc.DropDownControl(DropDownControl);
            }
            return value;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            var subresource = e.Value as ISubresource;
            var color = Color.Gray;
            if (subresource != null) color = subresource.Loaded ? subresource.ValidID ? Color.Green : Color.Yellow : Color.Red;
            e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
        }
    }
    class SubresourceConverter<T> : TypeConverter where T : ISubresource, new()
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                var subresource = new T();
                subresource.Link = value as string;
                return subresource;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
