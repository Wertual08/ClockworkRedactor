using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtraSharp;
using ExtraForms;
using Resource_Redactor.Resources;
using Resource_Redactor.Resources.Redactors;
using System.Collections.Specialized;
using Resource_Redactor.Compiler;

namespace Resource_Redactor
{
    public partial class RedactorForm : Form
    {
        private IResourceControl SelectedRedactor
        {
            get
            {
                if (RedactorsTabControl.TabCount < 1) return null;
                var tab = RedactorsTabControl.SelectedTab;
                if (tab == null) return null;
                if (tab.Controls.Count != 1) return null;
                return tab.Controls[0] as IResourceControl;
            }
        }

        private bool SplitContainerPanelsSwapped = false;
        private void SetSplitContainerPanelsSwapped(bool swapped)
        {
            if (SplitContainerPanelsSwapped == swapped) return;
            else SplitContainerPanelsSwapped = swapped;

            var panel1Controls = new List<Control>(ExplorerSplitContainer.Panel1.Controls.Count);
            foreach (Control control in ExplorerSplitContainer.Panel1.Controls)
                panel1Controls.Add(control);

            var panel1MinSize = ExplorerSplitContainer.Panel1MinSize;
            var panel1Collapsed = ExplorerSplitContainer.Panel1Collapsed;

            ExplorerSplitContainer.Panel1.Controls.Clear();
            foreach (Control control in ExplorerSplitContainer.Panel2.Controls)
                ExplorerSplitContainer.Panel1.Controls.Add(control);

            ExplorerSplitContainer.Panel1MinSize = ExplorerSplitContainer.Panel2MinSize;
            ExplorerSplitContainer.Panel1Collapsed = ExplorerSplitContainer.Panel2Collapsed;

            ExplorerSplitContainer.Panel2.Controls.Clear();
            foreach (var control in panel1Controls)
                ExplorerSplitContainer.Panel2.Controls.Add(control);

            ExplorerSplitContainer.Panel2MinSize = panel1MinSize;
            ExplorerSplitContainer.Panel2Collapsed = panel1Collapsed;

            if (ExplorerSplitContainer.FixedPanel == FixedPanel.Panel1) 
                ExplorerSplitContainer.FixedPanel = FixedPanel.Panel2;
            else ExplorerSplitContainer.FixedPanel = FixedPanel.Panel1;
            ExplorerSplitContainer.SplitterDistance = ExplorerSplitContainer.Width - ExplorerSplitContainer.SplitterDistance;
        }

        private void LoadRedactor(string path, string name)
        {
            try
            {
                foreach (TabPage tab in RedactorsTabControl.TabPages)
                {
                    if (tab.Controls.Count == 1)
                    {
                        var redactor = tab.Controls[0] as IResourceControl;
                        if (redactor != null)
                        {
                            if (redactor.ResourcePath == path)
                            {
                                RedactorsTabControl.SelectedTab = tab;
                                return;
                            }
                        }
                    }
                }

                ResourceType type = Resource.GetType(path);
                if (type == ResourceType.Outdated)
                {
                    type = Resource.GetLegacyType(path);
                    string current_version = Resource.GetVersion(path);
                    string actual_version = Resource.GetVersion(type);

                    string dated = current_version.CompareTo(actual_version) < 0 ? "outdated" : "overdated";
                    var result = MessageBox.Show(this, "Resource [" + type + ":" + name +
                       "] version is [" + current_version + "]. Actual version is [" +
                       actual_version + "]. Would you like to convert it? Some data may be lost.", "Resource is " + dated + ".",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result != DialogResult.Yes) return;
                }

                Control control = null;
                switch (type)
                {
                    case ResourceType.Texture: control = new TextureControl(path); break;
                    case ResourceType.Sprite: control = new SpriteControl(path); break;
                    case ResourceType.Ragdoll: control = new RagdollControl(path); break;
                    case ResourceType.Animation: control = new AnimationControl(path); break;
                    case ResourceType.Tool: control = new ToolControl(path); break;
                    case ResourceType.Entity: control = new EntityControl(path); break;
                    case ResourceType.Tile: control = new TileControl(path); break;
                    case ResourceType.Event: control = new EventControl(path); break;
                    case ResourceType.Outfit: control = new OutfitControl(path); break;

                    default:
                        MessageBox.Show(this, "Resource [" + type +
                           "] redactor does not implemented.", "Warning!",
                           MessageBoxButtons.OK, MessageBoxIcon.Warning); break;
                }

                if (control == null) return;
                control.Dock = DockStyle.Fill;

                var page = new TabPage(name);
                page.ImageIndex = Resource.TypeToIcon(type);
                bool refresh = RedactorsTabControl.TabPages.Count == 0;
                RedactorsTabControl.TabPages.Add(page);

                page.Controls.Add(control);

                var iresource = control as IResourceControl;
                iresource.StateChanged += Redactor_StateChanged;

                RedactorsTabControl.SelectedTab = page;
                if (RedactorsTabControl.TabPages.Count == 1)
                {
                    RedactorsTabControl.SelectedTab.Text = iresource.ResourceName + (iresource.Saved ? "" : "*");
                    UndoToolStripMenuItem.Enabled = iresource.UndoEnabled;
                    RedoToolStripMenuItem.Enabled = iresource.RedoEnabled;
                    CloseToolStripMenuItem.Enabled = true;
                    SaveToolStripMenuItem.Enabled = !iresource.Saved;
                    SaveAsToolStripMenuItem.Enabled = true;
                    CloseOthersToolStripMenuItem.Enabled = true;
                    ResourceToolStripMenuItem.DropDownItems.Clear();
                    ResourceToolStripMenuItem.DropDownItems.AddRange(iresource.MenuTabs);
                    ResourceToolStripMenuItem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not load resource redactor [" + name + "].",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CloseRedactor(TabPage tab)
        {
            var redactor = tab.Controls.Count == 1 ? tab.Controls[0] as IResourceControl : null;
            if (redactor == null) return;
            if (redactor.Saved)
            {
                RedactorsTabControl.TabPages.Remove(tab);
                tab.Dispose();
            }
            else
            {
                var result = MessageBox.Show(this, "Save changes before closing?",
                    "Warning: You have unsaved changes in [" + redactor.ResourceName +
                    "]!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) redactor.Save(redactor.ResourcePath);
                if (result != DialogResult.Cancel)
                {
                    RedactorsTabControl.TabPages.Remove(tab);
                    tab.Dispose();
                }
            }
        }
        private void RedactorsTabControl_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                var redactor = SelectedRedactor;
                if (redactor == null)
                {
                    UndoToolStripMenuItem.Enabled = false;
                    RedoToolStripMenuItem.Enabled = false;
                    CloseToolStripMenuItem.Enabled = false;
                    SaveToolStripMenuItem.Enabled = false;
                    SaveAsToolStripMenuItem.Enabled = false;
                    CloseOthersToolStripMenuItem.Enabled = false;
                    ResourceToolStripMenuItem.DropDownItems.Clear();
                    ResourceToolStripMenuItem.Enabled = false;
                }
                else
                {
                    RedactorsTabControl.SelectedTab.Text = redactor.ResourceName + (redactor.Saved ? "" : "*");
                    UndoToolStripMenuItem.Enabled = redactor.UndoEnabled;
                    RedoToolStripMenuItem.Enabled = redactor.RedoEnabled;
                    CloseToolStripMenuItem.Enabled = true;
                    SaveToolStripMenuItem.Enabled = !redactor.Saved;
                    SaveAsToolStripMenuItem.Enabled = true;
                    CloseOthersToolStripMenuItem.Enabled = true;
                    ResourceToolStripMenuItem.DropDownItems.Clear();
                    ResourceToolStripMenuItem.DropDownItems.AddRange(redactor.MenuTabs);
                    ResourceToolStripMenuItem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not select redactor.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Redactor_StateChanged(object sender, EventArgs e)
        {
            try
            {
                var control = sender as Control;
                var redactor = sender as IResourceControl;
                if (control == null || redactor == null) return;
                var tab = control.Parent as TabPage;
                if (tab == null) return;
                
                var name = redactor.ResourceName + (redactor.Saved ? "" : "*");
                if (tab.Text != name) tab.Text = name;
                if (tab == RedactorsTabControl.SelectedTab)
                {
                    UndoToolStripMenuItem.Enabled = redactor.UndoEnabled;
                    RedoToolStripMenuItem.Enabled = redactor.RedoEnabled;
                    SaveToolStripMenuItem.Enabled = !redactor.Saved;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle redactor state change.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResourceExplorer_ItemLoaded(object sender, ExplorerEventArgs e)
        {
            try
            {
                LoadRedactor(e.Path, e.Name);
            }
            catch (Exception ex)
            {
                var name = "";
                if (e != null) name = e.Name;
                MessageBox.Show(this, ex.ToString(), "Error: Can not load item [" + name + "].",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RedactorsTabControl_MouseClick(object sender, MouseEventArgs e)
        {
            var control = sender as TabControl;
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < control.TabCount; ++i)
                {
                    if (control.GetTabRect(i).Contains(e.Location))
                    {
                        control.SelectedIndex = i;
                        TabsContextMenuStrip.Show(control, e.Location);
                        break;
                    }
                }
            }
            if (e.Button == MouseButtons.Middle)
            {
                for (int i = 0; i < control.TabCount; ++i)
                {
                    if (control.GetTabRect(i).Contains(e.Location))
                    {
                        CloseRedactor(control.TabPages[i]);
                        break;
                    }
                }
            }
        }
        private void CloseTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseToolStripMenuItem.PerformClick();
        }

        private void OpenDescription(string path)
        {
            try
            {
                var version = Description.CheckVersion(path);
                if (version != Description.CurrentVersion)
                {
                    var result = MessageBox.Show(this, "Export description from previous version of redactor.", 
                        "Description is outdated.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                path = Path.GetFullPath(path);
                var desc = new Description(path);
                path = Path.Combine(Path.GetDirectoryName(path), "Resources");
                Directory.SetCurrentDirectory(path);
                ResourceExplorer.LoadLocation(path);

                var properties = new RedactorProperties("../Properties");
                ExplorerSplitContainer.SplitterDistance = properties.SplitterDistance;
                ExplorerSplitContainer.Panel1Collapsed = !properties.ExplorerVisible;
                SetSplitContainerPanelsSwapped(properties.ExplorerRight);
                ResourceExplorer.ViewMode = properties.ExplorerMode;
                foreach (var resource in properties.OpenedTabs) 
                    LoadRedactor(resource, Path.GetFileName(resource));
                ResourceExplorer.MoveLocation(properties.ExplorerPath);
                if (properties.SelectedTab >= 0 && properties.SelectedTab < RedactorsTabControl.TabCount)
                    RedactorsTabControl.SelectedIndex = properties.SelectedTab;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not open description.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveDescription()
        {
            try
            {
                var properties = new RedactorProperties();
                if (SplitContainerPanelsSwapped)
                {
                    properties.SplitterDistance = ExplorerSplitContainer.Width - ExplorerSplitContainer.SplitterDistance;
                    properties.ExplorerVisible = !ExplorerSplitContainer.Panel2Collapsed;
                }
                else
                {
                    properties.SplitterDistance = ExplorerSplitContainer.SplitterDistance;
                    properties.ExplorerVisible = !ExplorerSplitContainer.Panel1Collapsed;
                }
                properties.ExplorerMode = ResourceExplorer.ViewMode;
                properties.ExplorerRight = SplitContainerPanelsSwapped;
                properties.ExplorerPath = ResourceExplorer.CurrentDirectory;
                properties.SelectedTab = RedactorsTabControl.SelectedIndex;

                foreach (TabPage tab in RedactorsTabControl.TabPages)
                {
                    if (tab.Controls.Count == 1)
                    {
                        var control = tab.Controls[0] as IResourceControl;
                        if (control != null) properties.OpenedTabs.Add(ExtraPath.MakeDirectoryRelated(
                            Directory.GetCurrentDirectory(), control.ResourcePath));
                    }
                }

                properties.Save("../Properties");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not save description.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public RedactorForm(string initial_file = null)
        {
            InitializeComponent();
            Subresource.SynchronizingObject = this;

            RedactorsTabControl.ImageList = ResourceExplorer.SmallIconList;
            Text = "Clockwork engine resource redactor V" + Description.RedactorVersion;
            
            if (initial_file != null) OpenDescription(initial_file);
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                if (item == null) return; 
                switch (item.Text)
                {
                    case "Description":
                        try
                        {
                            var Subform = new NewDescriptionForm();
                            if (Subform.ShowDialog(this) != DialogResult.OK) return;
                            Description.Create(Subform.Root, Subform.Name);
                            OpenDescription(Path.Combine(Subform.Root, Subform.Name, Subform.Name + "." + Description.Extension));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, ex.ToString(), "Can not create description.",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    default: ResourceExplorer.CreateResource((ResourceType)Enum.Parse(typeof(ResourceType), item.Text)); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not create resource.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DescriptionOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenDescriptionDialog.DefaultExt = Description.Extension;
                if (OpenDescriptionDialog.ShowDialog(this) != DialogResult.OK) return;
                OpenDescription(OpenDescriptionDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not open description.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResourceOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var path in ResourceExplorer.SelectedItems)
            {
                try
                {
                    LoadRedactor(path, Path.GetFileName(path));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "Error: Can not open resource.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedRedactor?.Save(SelectedRedactor.ResourcePath);
            }
            catch (Exception ex)
            {
                var name = "";
                if (RedactorsTabControl.SelectedTab != null)
                    name = RedactorsTabControl.SelectedTab.Text;
                if (SelectedRedactor != null) name = SelectedRedactor.ResourceName;
                MessageBox.Show(this, ex.ToString(), "Error: Can not save resource [" + name + "].",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Add resource explorer
            }
            catch (Exception ex)
            {
                var name = "";
                if (RedactorsTabControl.SelectedTab != null)
                    name = RedactorsTabControl.SelectedTab.Text;
                if (SelectedRedactor != null) name = SelectedRedactor.ResourceName;
                MessageBox.Show(this, ex.ToString(), "Error: Can not save resource [" + name + "].",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TabPage tab in RedactorsTabControl.TabPages)
            {
                if (tab.Controls.Count != 1) continue;
                var rcontrol = tab.Controls[0] as IResourceControl;
                if (rcontrol == null) continue;

                try
                {
                    rcontrol.Save(rcontrol.ResourcePath);
                    tab.Text = rcontrol.ResourceName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "Error: Can not save resource [" + 
                        rcontrol.ResourceName + "].", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToSpriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var res in ResourceExplorer.SelectedItems)
            {
                try
                {
                    Resource.Factory(ResourceExplorer.CurrentDirectory,
                        ResourceType.Sprite, new string[] { res });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "Error: Can convert resource to sprite.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ToRagdollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Resource.Factory(ResourceExplorer.CurrentDirectory,
                    ResourceType.Ragdoll, ResourceExplorer.SelectedItems.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can convert resources to ragdoll.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ToOutfitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var res in ResourceExplorer.SelectedItems)
            {
                try
                {
                    Resource.Factory(ResourceExplorer.CurrentDirectory,
                        ResourceType.Outfit, new string[] { res });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "Error: Can convert resource to outfit.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Are you sure you want to delete " + ResourceExplorer.SelectedCount + 
                    " selected items?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, 
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                var recycle = Path.Combine(Path.GetDirectoryName(Directory.GetCurrentDirectory()), "Recycle");
                foreach (var path in ResourceExplorer.SelectedNames)
                {
                    try
                    {
                        var dest = Path.Combine(Path.GetDirectoryName(Path.Combine(recycle, path)),
                            "[" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "]" + Path.GetFileName(path));
                        if (!Directory.Exists(dest)) Directory.CreateDirectory(Path.GetDirectoryName(dest));
                        Directory.Move(path, dest);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString(), "Error: Can not delete resource.",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not delete resource.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ResourceExplorer.RenameSelected();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not rename resource.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StringCollection paths = new StringCollection();
                if (ResourceExplorer.SelectedPaths.Length < 1) return;
                paths.AddRange(ResourceExplorer.SelectedPaths);
                Clipboard.SetFileDropList(paths);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not copy resource.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StringCollection paths = new StringCollection();
                if (ResourceExplorer.SelectedPaths.Length < 1) return;
                paths.AddRange(ResourceExplorer.SelectedPaths);

                DataObject data = new DataObject();
                data.SetFileDropList(paths);
                data.SetData("Preferred DropEffect", new MemoryStream(new byte[] { (byte)DragDropEffects.Move, 0, 0, 0 }));

                Clipboard.SetDataObject(data, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not cut resource.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsFileDropList())
                {
                    DragDropEffects effect = DragDropEffects.Copy;
                    if (Clipboard.ContainsData("Preferred DropEffect")) 
                        effect = (DragDropEffects)((MemoryStream)Clipboard.
                            GetData("Preferred DropEffect")).ReadByte();

                    var paths = Clipboard.GetFileDropList();
                    foreach (var path in paths)
                    {
                        var dest = Path.Combine(ResourceExplorer.CurrentDirectory, Path.GetFileName(path));
                        int p = 0;
                        string s = "";
                        while (File.Exists(dest + s) || Directory.Exists(dest + s)) s = " - copy " + (p++);
                        if (Directory.Exists(path))
                        {
                            if (effect.HasFlag(DragDropEffects.Move)) Directory.Move(path, dest + s);
                            else Utilities.DirectoryCopy(path, dest + s, true);
                        }
                        else if (File.Exists(path))
                        {
                            if (effect.HasFlag(DragDropEffects.Move)) File.Move(path, dest + s);
                            else File.Copy(path, dest + s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not paste resource.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            { 
                SelectedRedactor?.Undo();
            }
            catch (Exception ex)
            {
                var name = "";
                if (RedactorsTabControl.SelectedTab != null)
                    name = RedactorsTabControl.SelectedTab.Text;
                if (SelectedRedactor != null) name = SelectedRedactor.ResourceName;
                MessageBox.Show(this, ex.ToString(), "Error: Can not undo resource [" + name + "].",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedRedactor?.Redo();
            }
            catch (Exception ex)
            {
                var name = "";
                if (RedactorsTabControl.SelectedTab != null)
                    name = RedactorsTabControl.SelectedTab.Text;
                if (SelectedRedactor != null) name = SelectedRedactor.ResourceName;
                MessageBox.Show(this, ex.ToString(), "Error: Can not redo resource [" + name + "].",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CloseRedactor(RedactorsTabControl.SelectedTab);
            }
            catch (Exception ex)
            {
                var name = RedactorsTabControl.SelectedTab?.Text ?? "{INVALID_NAME}";
                if (SelectedRedactor != null) name = SelectedRedactor.ResourceName;
                MessageBox.Show(this, ex.ToString(), "Error: Can not close resource [" + name + "].",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TabPage tab in RedactorsTabControl.TabPages)
            {
                try
                {
                    CloseRedactor(tab);
                }
                catch (Exception ex)
                {
                    var name = "";
                    if (RedactorsTabControl.SelectedTab != null)
                        name = RedactorsTabControl.SelectedTab.Text;
                    if (SelectedRedactor != null) name = SelectedRedactor.ResourceName;
                    MessageBox.Show(this, ex.ToString(), "Error: Can not close resource [" + name + "].",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void CloseOthersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TabPage tab in RedactorsTabControl.TabPages)
            {
                if (tab == RedactorsTabControl.SelectedTab) continue;
                try
                {
                    CloseRedactor(tab);
                }
                catch (Exception ex)
                {
                    var name = "";
                    if (RedactorsTabControl.SelectedTab != null)
                        name = RedactorsTabControl.SelectedTab.Text;
                    if (SelectedRedactor != null) name = SelectedRedactor.ResourceName;
                    MessageBox.Show(this, ex.ToString(), "Error: Can not close resource [" + name + "].",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ToggleExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SplitContainerPanelsSwapped)
                ExplorerSplitContainer.Panel2Collapsed =
                    !ExplorerSplitContainer.Panel2Collapsed;
            else ExplorerSplitContainer.Panel1Collapsed = 
                    !ExplorerSplitContainer.Panel1Collapsed;
        }
        private void SwitchViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mode = ResourceExplorer.ViewMode;
            switch (mode)
            {
                case ListViewMode.LargeIcon: ResourceExplorer.ViewMode = ListViewMode.MediumIcon; break;
                case ListViewMode.MediumIcon: ResourceExplorer.ViewMode = ListViewMode.SmallIcon; break;
                case ListViewMode.SmallIcon: ResourceExplorer.ViewMode = ListViewMode.List; break;
                case ListViewMode.List: ResourceExplorer.ViewMode = ListViewMode.LargeIcon; break;
            }
        }
        private void SwapPanelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSplitContainerPanelsSwapped(!SplitContainerPanelsSwapped);
        }

        private void ExplorerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDescription();
            foreach (TabPage tab in RedactorsTabControl.TabPages)
            {
                try
                {
                    CloseRedactor(tab);
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    MessageBox.Show(this, ex.ToString(), "Error: Can not close redactor.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        static readonly Image CloseImage = (Image)(new ComponentResourceManager(typeof(RedactorForm)).GetObject("CloseToolStripMenuItem.Image"));
        private void RedactorsTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tc = sender as TabControl;
            var tab = tc.TabPages[e.Index];
            
            if (tc.SelectedIndex != e.Index) e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds.Left, e.Bounds.Top + 2, e.Bounds.Width, e.Bounds.Height);
            else e.Graphics.FillRectangle(Brushes.White, e.Bounds.Left, e.Bounds.Top + 2, e.Bounds.Width, e.Bounds.Height);
            e.Graphics.DrawImage(tc.ImageList.Images[tab.ImageIndex], e.Bounds.Left + 3, e.Bounds.Top + 3, 16, 16);
            e.Graphics.DrawString(tab.Text, e.Font, Brushes.Black, e.Bounds.Left + 20, e.Bounds.Top + 4);
            e.Graphics.DrawImage(CloseImage, e.Bounds.Right - 19, e.Bounds.Top + 3, 16, 16);
        }
        private void RedactorsTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                var tc = sender as TabControl;
                if (tc == null) return;
                for (int i = 0; i < tc.TabPages.Count; i++)
                {
                    Rectangle r = tc.GetTabRect(i);
                    Rectangle closeButton = new Rectangle(r.Right - 16, r.Top + 4, 14, 14);
                    if (closeButton.Contains(e.Location) && false)
                    {
                        if (tc.TabPages[i].Controls.Count != 1) continue;
                        var redactor = tc.TabPages[i].Controls[0] as IResourceControl;
                        if (redactor == null) continue;
                        if (redactor.Saved) RedactorsTabControl.TabPages.RemoveAt(i);
                        else
                        {
                            var result = MessageBox.Show(this, "Save changes before closing?",
                                "Warning: You have unsaved changes in [" + redactor.ResourceName +
                                "]!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes) redactor.Save(redactor.ResourcePath);
                            if (result != DialogResult.Cancel) RedactorsTabControl.TabPages.RemoveAt(i);
                        }
                    }
                    if (r.Contains(e.Location)) tc.Tag = tc.TabPages[i];
                }
            }
            catch (Exception ex)
            {
                var name = "";
                if (RedactorsTabControl.SelectedTab != null)
                    name = RedactorsTabControl.SelectedTab.Text;
                if (SelectedRedactor != null) name = SelectedRedactor.ResourceName;
                MessageBox.Show(this, ex.ToString(), "Error: Can not close resource [" + name + "].",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RedactorsTabControl_MouseMove(object sender, MouseEventArgs e)
        {
            TabControl tc = sender as TabControl;
            if ((e.Button != MouseButtons.Left) || (tc.Tag == null)) return;
            tc.DoDragDrop(tc.Tag as TabPage, DragDropEffects.All);
        }
        private void RedactorsTabControl_MouseUp(object sender, MouseEventArgs e)
        {
            var tc = sender as TabControl;
            if (tc != null) tc.Tag = null;
        }
        private void RedactorsTabControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage))) return;
        }
        private void RedactorsTabControl_DragOver(object sender, DragEventArgs e)
        {
            var tc = sender as TabControl;

            if (e.Data.GetData(typeof(TabPage)) == null) return;
            TabPage dragTab = (TabPage)e.Data.GetData(typeof(TabPage));
            int dragTab_index = tc.TabPages.IndexOf(dragTab);

            int hoverTab_index = -1;
            for (int i = 0; i < tc.TabPages.Count; i++)
            {
                Rectangle r = tc.GetTabRect(i);
                if (r.Contains(tc.PointToClient(new Point(e.X, e.Y)))) hoverTab_index = i;
            }
            if (hoverTab_index < 0) { e.Effect = DragDropEffects.None; return; }
            var hoverTab = tc.TabPages[hoverTab_index];
            e.Effect = DragDropEffects.Move;

            if (dragTab == hoverTab) return;

            Rectangle dragTabRect = tc.GetTabRect(dragTab_index);
            Rectangle hoverTabRect = tc.GetTabRect(hoverTab_index);

            if (dragTabRect.Width < hoverTabRect.Width)
            {
                Point tcLocation = tc.PointToScreen(tc.Location);

                if (dragTab_index < hoverTab_index)
                {
                    if ((e.X - tcLocation.X) > ((hoverTabRect.X + hoverTabRect.Width) - dragTabRect.Width))
                        SwapTabPages(tc, dragTab, hoverTab);
                }
                else if (dragTab_index > hoverTab_index)
                {
                    if ((e.X - tcLocation.X) < (hoverTabRect.X + dragTabRect.Width))
                        SwapTabPages(tc, dragTab, hoverTab);
                }
            }
            else SwapTabPages(tc, dragTab, hoverTab);

            tc.SelectedIndex = tc.TabPages.IndexOf(dragTab);
        }
        private void SwapTabPages(TabControl tc, TabPage src, TabPage dst)
        {
            int index_src = tc.TabPages.IndexOf(src);
            int index_dst = tc.TabPages.IndexOf(dst);
            tc.TabPages[index_dst] = src;
            tc.TabPages[index_src] = dst;
            tc.Refresh();
        }

        private void CompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var Compiler = new CompilerForm("../IDTable.txt"))
                    this.LoadSubform(Compiler);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error: Can not handle compiler.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Visible = true;
            }
        }
    }
}
