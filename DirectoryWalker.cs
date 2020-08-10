using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources
{
    public class WalkerEventArgs : EventArgs
    {
        public string OldPath { get; private set; }
        public string NewPath { get; private set; }

        public WalkerEventArgs(string old_path, string new_path)
        {
            OldPath = old_path;
            NewPath = new_path;
        }
    }
    public class DirectoryWalker
    {
        private FileSystemWatcher DirectoryWatcher;
        private void DirectoryWatcher_Changed(object sender, EventArgs e)
        {
            var path = Path.Combine(RootDirectory, CurrentDirectory);
            DirectoryChanged?.Invoke(this, new WalkerEventArgs(path, path));
        }

        public string RootDirectory { get; private set; } = null;
        public string CurrentDirectory { get; private set; } = "";
        public string CurrentPath
        { 
            get
            {
                if (RootDirectory == null) return null;
                return Path.Combine(RootDirectory, CurrentDirectory);
            }
        }
        public List<string> CurrentContent
        {
            get
            {
                var path = CurrentPath;

                var result = new List<string>(Directory.EnumerateDirectories(path));
                result.AddRange(Directory.EnumerateFiles(path));
                if (path != RootDirectory) result.Insert(0, "..");
                return result;
            }
        }

        public delegate void DirectoryEventHandler(object sender, WalkerEventArgs e);
        public event DirectoryEventHandler DirectoryChanged;

        public DirectoryWalker(FileSystemWatcher watcher)
        {
            DirectoryWatcher = watcher;
            DirectoryWatcher.Filter = "*";
            DirectoryWatcher.Renamed += DirectoryWatcher_Changed;
            DirectoryWatcher.Changed += DirectoryWatcher_Changed;
            DirectoryWatcher.Deleted += DirectoryWatcher_Changed;
            DirectoryWatcher.Created += DirectoryWatcher_Changed;
        }

        public DirectoryWalker()
        {
            DirectoryWatcher = new FileSystemWatcher();
            DirectoryWatcher.Filter = "*";
            DirectoryWatcher.Renamed += DirectoryWatcher_Changed;
            DirectoryWatcher.Changed += DirectoryWatcher_Changed;
            DirectoryWatcher.Deleted += DirectoryWatcher_Changed;
            DirectoryWatcher.Created += DirectoryWatcher_Changed;
        }
        public bool Load(string root)
        {
            root = Path.GetFullPath(root);
            if (!Directory.Exists(root)) return false;

            string old_path = RootDirectory;
            RootDirectory = root;
            CurrentDirectory = "";

            DirectoryWatcher.Path = RootDirectory;
            DirectoryWatcher.EnableRaisingEvents = true;
            DirectoryChanged?.Invoke(this, new WalkerEventArgs(old_path, RootDirectory));
            return true;
        }

        public string Move(string name)
        {
            var old_path = CurrentPath;
            var sep = Path.DirectorySeparatorChar;
            var path = name.Replace(':', sep).Replace('/', sep);
            path = Path.GetFullPath(Path.Combine(CurrentPath, path));

            name = "";
            if (File.Exists(path))
            {
                name = Path.GetFileName(path);
                path = Path.GetDirectoryName(path);
            }
            if (!Directory.Exists(path)) return null;
            if (!path.StartsWith(RootDirectory)) return null;
            if (old_path != path)
            {
                CurrentDirectory = path.Replace(RootDirectory, "");
                if (CurrentDirectory.Length > 0 && CurrentDirectory[0] == sep)
                    CurrentDirectory = CurrentDirectory.Remove(0, 1);
                DirectoryWatcher.Path = CurrentPath;
                DirectoryWatcher.EnableRaisingEvents = true;
                DirectoryChanged?.Invoke(this, new WalkerEventArgs(old_path, path));
            }
            return name;
        }
        public string Open(string name)
        {
            if (name == null || RootDirectory == null) return null;
            var old_path = CurrentPath;
            var sep = Path.DirectorySeparatorChar;
            var path = name.Replace(':', sep).Replace('/', sep);
            path = Path.GetFullPath(Path.Combine(RootDirectory, path));

            name = "";
            if (File.Exists(path))
            {
                name = Path.GetFileName(path);
                path = Path.GetDirectoryName(path);
            }
            if (!Directory.Exists(path)) return null;
            if (!path.StartsWith(RootDirectory)) return null;
            CurrentDirectory = path.Replace(RootDirectory, "");
            if (CurrentDirectory.Length > 0 && CurrentDirectory[0] == sep)
                CurrentDirectory = CurrentDirectory.Remove(0, 1);
            DirectoryWatcher.Path = CurrentPath;
            DirectoryWatcher.EnableRaisingEvents = true;
            DirectoryChanged?.Invoke(this, new WalkerEventArgs(old_path, path));
            return name;
        }
        public string GetPath(string name)
        {
            var c = Path.DirectorySeparatorChar;
            return Path.GetFullPath(Path.Combine(RootDirectory, 
                CurrentDirectory, name.Replace(':', c).Replace('/', c)));
        }
        public string GetLocalPath(string name)
        {
            var c = Path.DirectorySeparatorChar;
            return Path.Combine(CurrentDirectory, name.Replace(':', c).Replace('/', c));
        }
        public void Rename(string name, string new_name)
        {
            if (name == new_name) return;
            name = Path.Combine(CurrentPath, name);
            new_name = Path.Combine(CurrentPath, new_name);
            if (File.Exists(name)) File.Move(name, new_name);
            if (Directory.Exists(name)) Directory.Move(name, new_name);
        }
        public void Delete(string name)
        {
            name = Path.Combine(CurrentPath, name);
            if (File.Exists(name)) File.Delete(name);
            if (Directory.Exists(name)) Directory.Delete(name, true);
        }
    }
}
