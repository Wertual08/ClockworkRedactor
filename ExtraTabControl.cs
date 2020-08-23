using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor
{
    class ExtraTabControl : TabControl
    {
        public ContextMenuStrip TabsContextMenuStrip { get; set; }
        public event TabControlCancelEventHandler TabClosing;

        public void TryCloseTab(int index)
        {
            var tab = TabPages[index];
            var args = new TabControlCancelEventArgs(tab, index, false, TabControlAction.Selected);
            TabClosing?.Invoke(this, args);
            if (!args.Cancel) TabPages.RemoveAt(index);
            tab.Dispose();
        }
        public void TryCloseTab(TabPage tab)
        {
            TryCloseTab(TabPages.IndexOf(tab));
        }

        private TabPage DraggingPage = null;
        private void SwapTabPages(TabPage src, TabPage dst)
        {
            int index_src = TabPages.IndexOf(src);
            int index_dst = TabPages.IndexOf(dst);
            TabPages[index_dst] = src;
            TabPages[index_src] = dst;
            Refresh();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            var tab = TabPages[e.Index];

            if (SelectedIndex != e.Index) e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds.Left, e.Bounds.Top + 2, e.Bounds.Width, e.Bounds.Height);
            else e.Graphics.FillRectangle(Brushes.White, e.Bounds.Left, e.Bounds.Top + 2, e.Bounds.Width, e.Bounds.Height);
            e.Graphics.DrawImage(ImageList.Images[tab.ImageIndex], e.Bounds.Left + 3, e.Bounds.Top + 3, 16, 16);
            e.Graphics.DrawString(tab.Text, e.Font, Brushes.Black, e.Bounds.Left + 20, e.Bounds.Top + 4);
            //e.Graphics.DrawImage(CloseImage, e.Bounds.Right - 19, e.Bounds.Top + 3, 16, 16);

            base.OnDrawItem(e);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < TabCount; ++i)
                {
                    if (GetTabRect(i).Contains(e.Location))
                    {
                        SelectedIndex = i;
                        TabsContextMenuStrip?.Show(this, e.Location);
                        break;
                    }
                }
            }
            if (e.Button == MouseButtons.Middle)
            {
                for (int i = 0; i < TabCount; ++i)
                {
                    if (GetTabRect(i).Contains(e.Location))
                    {
                        TryCloseTab(i);
                        break;
                    }
                }
            }

            base.OnMouseClick(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            for (int i = 0; i < TabPages.Count; i++)
            {
                Rectangle r = GetTabRect(i);
                //Rectangle closeButton = new Rectangle(r.Right - 16, r.Top + 4, 14, 14);
                //if (closeButton.Contains(e.Location) && false)
                //{
                //    if (TabPages[i].Controls.Count != 1) continue;
                //    var redactor = TabPages[i].Controls[0] as IResourceControl;
                //    if (redactor == null) continue;
                //    if (redactor.Saved) TabPages.RemoveAt(i);
                //    else
                //    {
                //        var result = MessageBox.Show(this, "Save changes before closing?",
                //            "Warning: You have unsaved changes in [" + redactor.ResourceName +
                //            "]!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                //        if (result == DialogResult.Yes) redactor.Save(redactor.ResourcePath);
                //        if (result != DialogResult.Cancel) RedactorsTabControl.TabPages.RemoveAt(i);
                //    }
                //}
                if (r.Contains(e.Location)) DraggingPage = TabPages[i];
            }

            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (DraggingPage != null))
                DoDragDrop(DraggingPage, DragDropEffects.All);

            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            DraggingPage = null;

            base.OnMouseUp(e);
        }
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            //if (e.Data.GetDataPresent(typeof(TabPage))) return;
            base.OnDragEnter(drgevent);
        }
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            if (drgevent.Data.GetData(typeof(TabPage)) != null)
            {
                TabPage dragTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));
                int dragTab_index = TabPages.IndexOf(dragTab);

                int hoverTab_index = -1;
                for (int i = 0; i < TabPages.Count; i++)
                {
                    Rectangle r = GetTabRect(i);
                    Point p = new Point(drgevent.X, drgevent.Y);
                    if (r.Contains(PointToClient(p))) hoverTab_index = i;
                }
                if (hoverTab_index < 0) drgevent.Effect = DragDropEffects.None;
                else
                {
                    var hoverTab = TabPages[hoverTab_index];
                    drgevent.Effect = DragDropEffects.Move;

                    if (dragTab != hoverTab)
                    {
                        Rectangle dragTabRect = GetTabRect(dragTab_index);
                        Rectangle hoverTabRect = GetTabRect(hoverTab_index);

                        if (dragTabRect.Width < hoverTabRect.Width)
                        {
                            Point tcLocation = PointToScreen(Location);

                            if (dragTab_index < hoverTab_index)
                            {
                                if ((drgevent.X - tcLocation.X) > ((hoverTabRect.X + hoverTabRect.Width) - dragTabRect.Width))
                                    SwapTabPages(dragTab, hoverTab);
                            }
                            else if (dragTab_index > hoverTab_index)
                            {
                                if ((drgevent.X - tcLocation.X) < (hoverTabRect.X + dragTabRect.Width))
                                    SwapTabPages(dragTab, hoverTab);
                            }
                        }
                        else SwapTabPages(dragTab, hoverTab);

                        SelectedIndex = TabPages.IndexOf(dragTab);
                    }
                }
            }

            base.OnDragOver(drgevent);
        }
    }
}
