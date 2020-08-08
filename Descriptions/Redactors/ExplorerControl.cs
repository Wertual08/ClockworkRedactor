using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Resource_Redactor.Descriptions.Redactors
{
    [DefaultEvent("ItemLoaded")]
    public partial class ExplorerControl : UserControl
    {
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        private string PendRenaming = null;
        private DirectoryWalker Walker;

        private void LoadDirectory(bool new_location = false)
        {
            if (Walker == null) return;
            
            ResourcesListView.BeginUpdate();
            ResourcesListView.Items.Clear();
            var files = Walker.CurrentContent;
            foreach (var file in files)
            {
                var type = Resource.GetType(file);
                
                if (type == ResourceType.Folder || TypeFilter == null || 
                    TypeFilter.Length < 1 || TypeFilter.Contains(type))
                {
                    var name = Path.GetFileName(file);
                    int img = Resource.TypeToIcon(type);
                    if (name == "..") img = (int)ResourceIcon.PFolder;
                    var item = ResourcesListView.Items.Add(name, img);

                    if (name == PendRenaming) item.BeginEdit();
                }
            }
            ResourcesListView.EndUpdate();
            NameTextBox.Text = Walker.CurrentDirectory.Replace(
                Path.DirectorySeparatorChar, '/');
        }
        private void InvokeItemLoaded(string path)
        {
            ItemLoaded?.Invoke(this, new ExplorerEventArgs(Path.GetFileName(path), 
                    path, Path.GetFullPath(path), Resource.GetType(path)));
        }
        private bool LoadItem(string name)
        {
            var move = Walker.Move(name);
            NameTextBox.Text = Walker.CurrentDirectory.Replace(
                Path.DirectorySeparatorChar, '/');
            if (move == null) return false;
            if (move == "") return true; 
            InvokeItemLoaded(Walker.GetLocalPath(move));
            return false;
        }

        private void NameTextBox_Leave(object sender, EventArgs e)
        {
            var open = Walker.Open(NameTextBox.Text);
            NameTextBox.Text = Walker.CurrentDirectory.Replace(
                Path.DirectorySeparatorChar, '/');
            NameTextBox.SelectionStart = NameTextBox.Text.Length;
            if (open == null) return;
            if (open == "") return;
            InvokeItemLoaded(Walker.GetPath(open));
        }
        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                
                var open = Walker.Open(NameTextBox.Text);
                NameTextBox.Text = Walker.CurrentDirectory.Replace(
                    Path.DirectorySeparatorChar, '/');
                NameTextBox.SelectionStart = NameTextBox.Text.Length;
                if (open == null) return;
                if (open == "") return;
                InvokeItemLoaded(Walker.GetPath(open));
            }
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                NameTextBox.Text = Walker.CurrentDirectory.Replace(
                    Path.DirectorySeparatorChar, '/');
                NameTextBox.SelectionStart = NameTextBox.Text.Length;
            }
        }

        private void ResourcesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResourcesListView.SelectedItems.Count < 1) return;
            var item = ResourcesListView.SelectedItems[0];
            var index = ResourcesListView.SelectedIndices[0];
            var name = item.Text;
            var path = Walker.GetLocalPath(name);
            var type = Resource.GetType(path);
            StateChanged?.Invoke(this, new ExplorerEventArgs(name, path, Path.GetFullPath(path), type));
        }
        private void ResourcesListView_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ResourcesListView.SelectedItems)
                if (LoadItem(item.Text)) break;
        }
        private void ResourcesListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                foreach (ListViewItem item in ResourcesListView.SelectedItems)
                    if (LoadItem(item.Text)) break;
            }
            if (e.KeyCode == Keys.Escape)
            {
                ResourcesListView.SelectedItems.Clear();
            }
        }
        private void ResourcesListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.CancelEdit) return;
            if (e.Label == null) return;
            PendRenaming = null;

            try
            {
                Walker.Rename(ResourcesListView.Items[e.Item].Text, e.Label);
            }
            catch (Exception ex)
            {
                e.CancelEdit = true;
                MessageBox.Show(this, ex.ToString(), "Can not rename resource.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Walker_DirectoryChanged(object sender, WalkerEventArgs e)
        {
            LoadDirectory(e.OldPath != e.NewPath);
        }

        private void ResourcesListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var files = new string[ResourcesListView.SelectedItems.Count];
            for (int i = 0; i < ResourcesListView.SelectedItems.Count; i++)
            {
                ListViewItem item = ResourcesListView.SelectedItems[i];
                if (item.Text == "." || item.Text == ".." || item.Text == "") return;
                files[i] = Walker.GetPath(item.Text);
            }
            var data = new DataObject(DataFormats.FileDrop, files);
            DoDragDrop(data, DragDropEffects.All | DragDropEffects.Link);
        }
        private void ResourcesListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            if ((e.KeyState & (8 + 32)) == (8 + 32) &&
                e.AllowedEffect.HasFlag(DragDropEffects.Link))
                e.Effect = DragDropEffects.Link;
            else if ((e.KeyState & 32) == 32 &&
              e.AllowedEffect.HasFlag(DragDropEffects.Link))
                e.Effect = DragDropEffects.Link;
            else if ((e.KeyState & 4) == 4 &&
              e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else if ((e.KeyState & 8) == 8 &&
              e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }
        private void ResourcesListView_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];

            var point = ResourcesListView.PointToClient(new Point(e.X, e.Y));
            var item = ResourcesListView.GetItemAt(point.X, point.Y);
            if (item != null)
            {
                var ipath = Walker.GetPath(item.Text);
                if (Resource.GetType(ipath) != ResourceType.Folder) return;
                
                foreach (var path in paths) if (path == ipath) return;
            }

            if ((e.KeyState & (8 + 32)) == (8 + 32) &&
                e.AllowedEffect.HasFlag(DragDropEffects.Link))
                return;//e.Effect = DragDropEffects.Link;
            else if ((e.KeyState & 32) == 32 &&
              e.AllowedEffect.HasFlag(DragDropEffects.Link))
                return;//e.Effect = DragDropEffects.Link;
            else if ((e.KeyState & 4) == 4 &&
              e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else if ((e.KeyState & 8) == 8 &&
              e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }
        private void ResourcesListView_DragDrop(object sender, DragEventArgs e)
        {
            var point = ResourcesListView.PointToClient(new Point(e.X, e.Y));
            var item = ResourcesListView.GetItemAt(point.X, point.Y);
            string dest;
            if (item == null) dest = Walker.CurrentPath;
            else dest = Walker.GetPath(item.Text);

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var paths = (string[])e.Data.GetData(DataFormats.FileDrop);

                bool convert = false;
                foreach (var path in paths)
                {
                    if (Resource.PrimalFile(path))
                    {
                        convert = true;
                        break;
                    }
                }
                if (convert)
                {
                    var result = MessageBox.Show(this,
                        "Would you like to create resources from them automatically?",
                        "One or more of importing files are primal.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result != DialogResult.Yes) convert = false;
                }
                foreach (var path in paths)
                {
                    try
                    {
                        var fpath = Path.GetFullPath(path);
                        if (Resource.PrimalFile(fpath) && convert)
                        {
                            Resource.Factory(dest, fpath);
                        }
                        else
                        {
                            var new_path = Path.Combine(dest, Path.GetFileName(fpath));
                            if (new_path == fpath) continue;
                            if (e.Effect == DragDropEffects.Move)
                            {
                                if (File.Exists(fpath)) File.Move(fpath, new_path);
                                if (Directory.Exists(fpath)) Directory.Move(fpath, new_path);
                            }
                            else if (e.Effect == DragDropEffects.Copy)
                            {
                                if (File.Exists(fpath)) File.Copy(fpath, new_path);
                                if (Directory.Exists(fpath)) DirectoryCopy(fpath, new_path, true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString(),
                            "Error: Can not import item [" + path + "].",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        public ListViewMode ViewMode
        {
            get
            {
                switch (ResourcesListView.View)
                {
                    case View.LargeIcon:
                        if (ResourcesListView.LargeImageList == LargeIconList) 
                            return ListViewMode.LargeIcon;
                        else return ListViewMode.MediumIcon;
                    case View.SmallIcon: return ListViewMode.SmallIcon;
                    case View.List: return ListViewMode.List;
                };
                return (ListViewMode)(-1);
            }
            set
            {
                switch (value)
                {
                    case ListViewMode.LargeIcon: 
                        ResourcesListView.LargeImageList = LargeIconList; 
                        ResourcesListView.View = View.LargeIcon; 
                        break;
                    case ListViewMode.MediumIcon:
                        ResourcesListView.LargeImageList = MediumIconList;
                        ResourcesListView.View = View.LargeIcon; 
                        break;
                    case ListViewMode.SmallIcon: 
                        ResourcesListView.View = View.SmallIcon; 
                        break;
                    case ListViewMode.List: 
                        ResourcesListView.View = View.List; 
                        break;
                }
            }
        }
        public ResourceType[] TypeFilter;
        public string CurrentDirectory { get { return Walker.CurrentDirectory; } }
        public bool MultiSelect { get { return ResourcesListView.MultiSelect; } set { ResourcesListView.MultiSelect = value; } }
        public int SelectedCount { get { return ResourcesListView.SelectedItems.Count; } }
        public bool Loaded { get { return Walker.RootDirectory != null; } }
        public List<string> SelectedItems
        {
            get
            {
                var list = new List<string>();

                foreach (ListViewItem i in ResourcesListView.SelectedItems)
                {
                    var path = Walker.GetPath(i.Text);
                    var type = Resource.GetType(path);
                    if (type != ResourceType.Folder || (TypeFilter != null && TypeFilter.Contains(type)))
                        list.Add(Walker.GetLocalPath(i.Text));
                }

                return list;
            }
        }
        public List<string> SelectedNames
        {
            get
            {
                var list = new List<string>();

                foreach (ListViewItem i in ResourcesListView.SelectedItems)
                {
                    var path = Walker.GetPath(i.Text);
                    var type = Resource.GetType(path);
                    if (TypeFilter == null || TypeFilter.Contains(type)) list.Add(Walker.GetLocalPath(i.Text));
                }

                return list;
            }
        }
        public string[] SelectedPaths
        {
            get
            {
                var list = new string[ResourcesListView.SelectedItems.Count];
                for (int i = 0; i < ResourcesListView.SelectedItems.Count; i++) 
                    list[i] = Walker.GetPath(ResourcesListView.SelectedItems[i].Text);
                return list;
            }
        }

        public delegate void ItemLoadedEventHandler(object sender, ExplorerEventArgs e);
        public event ItemLoadedEventHandler ItemLoaded;
        public delegate void StateChangedEventHandler(object sender, ExplorerEventArgs e);
        public event StateChangedEventHandler StateChanged;

        public ExplorerControl()
        {
            InitializeComponent();
            Walker = new DirectoryWalker(ResourceWatcher);
            Walker.DirectoryChanged += Walker_DirectoryChanged;
        }

        public void LoadLocation(string path)
        {
            Walker.Load(path);
            StateChanged?.Invoke(this, new ExplorerEventArgs(null, null, null, ResourceType.MissingFile));
        }
        public void CreateResource(ResourceType type)
        {
            try
            {
                PendRenaming = Resource.Factory(Walker.CurrentPath, type);
                LoadDirectory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Can not create resource [" +
                    type + "].", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }
        public void DeleteSelected()
        {
            foreach (ListViewItem i in ResourcesListView.SelectedItems)
            {
                string name = "";
                try
                {
                    name = i.Text;
                    Walker.Delete(name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "Can not delete resource [" +
                        name + "].", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void RenameSelected()
        {
            if (ResourcesListView.SelectedItems.Count <= 0) return;
            ResourcesListView.SelectedItems[0].BeginEdit();
        }
    }
    public class ExplorerEventArgs : EventArgs
    {
        public string Name { get; private set; }
        public string LocalPath { get; private set; }
        public string Path { get; private set; }
        public ResourceType Type { get; private set; }
        public ExplorerEventArgs(string name, string localpath, string path, ResourceType type) : base()
        {
            Name = name;
            LocalPath = localpath;
            Path = path;
            Type = type;
        }
    }
    public enum ListViewMode : int
    {
        LargeIcon,
        MediumIcon,
        SmallIcon,
        List,
    }
}
