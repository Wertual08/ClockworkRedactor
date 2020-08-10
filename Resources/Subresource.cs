using ExtraSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Resource_Redactor.Resources
{
    public interface ISubresource
    {
        event EventHandler Reloaded;
        event EventHandler Updated;
        event EventHandler Refreshed;
        string Link { get; set; }
        bool Loaded { get; }
        bool ValidID { get; }
        bool ResolveByID();
        void ActualizeID();
        void Reload();
        ResourceType Type { get; }
    }

    public static class Subresource
    {
        public static ISynchronizeInvoke SynchronizingObject;
    }
    public class Subresource<T> : ISubresource, IDisposable where T : Resource, new()
    {
        private FileSystemWatcher Watcher = new FileSystemWatcher() { SynchronizingObject = Subresource.SynchronizingObject };
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            _Link = ExtraPath.MakeDirectoryRelated(Directory.GetCurrentDirectory(), e.FullPath);
            Updated?.Invoke(this, EventArgs.Empty);
            Reload();
        }
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Updated?.Invoke(this, EventArgs.Empty);
            Reload();
        }

        protected string _Link = "";
        protected T _Resource;
        protected void Refresh()
        {
            Refreshed?.Invoke(this, EventArgs.Empty);
        }

        [JsonIgnore]
        public ResourceType Type { get { return Resources.Resource.GetType(typeof(T)); } }
        public event EventHandler Reloaded;
        public event EventHandler Updated;
        public event EventHandler Refreshed;

        public virtual string Link
        {
            get
            {
                return _Link;
            }
            set
            {
                if (_Link != value)
                {
                    _Link = value;
                    Reload();
                }
            }
        }
        public ResourceID ID { get; set; } = null;

        [JsonIgnore]
        public T Resource { get => _Resource; }
        [JsonIgnore]
        public bool Loaded { get => _Resource != null; }
        [JsonIgnore]
        public bool ValidID { get => ID != null && _Resource != null && ID.Equals(_Resource.ID); }
        public bool ResolveByID()
        {
            if (ID == null) return false;
            foreach (var file in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories))
            {
                if (ID.Equals(Resources.Resource.GetID(file)))
                {
                    Link = ExtraPath.MakeDirectoryRelated(Directory.GetCurrentDirectory(), file);
                    return true;
                }
            }
            return false;
        }
        public void ActualizeID()
        {
            var id = ID;
            ID = _Resource?.ID?.Copy() ?? null;
            if (id != ID) Refresh();
        }

        public Subresource()
        {
            Watcher.Changed += Watcher_Changed;
            Watcher.Created += Watcher_Changed;
            Watcher.Deleted += Watcher_Changed;
            Watcher.Renamed += Watcher_Renamed;
        }
        public Subresource(string path)
        {
            Watcher.Changed += Watcher_Changed;
            Watcher.Created += Watcher_Changed;
            Watcher.Deleted += Watcher_Changed;
            Watcher.Renamed += Watcher_Renamed;
            Link = path;
        }
        public void Reload()
        {
            try
            {
                _Resource?.Dispose();
                _Resource = null;
                
                if (!File.Exists(_Link))
                {
                    Watcher.EnableRaisingEvents = false;
                    Reloaded?.Invoke(this, EventArgs.Empty);
                    Refreshed?.Invoke(this, EventArgs.Empty);
                    return;
                }
                try
                {
                    Watcher.Filter = Path.GetFileName(_Link);
                    Watcher.Path = Path.GetDirectoryName(Path.GetFullPath(_Link));
                    Watcher.EnableRaisingEvents = true;
                }
                catch 
                {
                    Watcher.EnableRaisingEvents = false;
                }
                _Resource = new T();
                _Resource.Open(_Link);
                if (ID == null) ActualizeID();
            }
            catch 
            {
                _Resource = null;
            }
            Reloaded?.Invoke(this, EventArgs.Empty);
            Refreshed?.Invoke(this, EventArgs.Empty);
        }

        protected bool IsDisposed { get; private set; } = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Watcher.Dispose();
                _Resource?.Dispose();
            }

            IsDisposed = true;
        }
        ~Subresource()
        {
            Dispose(false);
        }
    }

    public class WeakSubresource<T> : Subresource<T> where T : Resource, new()
    {
        public override string Link { 
            get => base.Link; 
            set 
            {
                if (_Link != value)
                {
                    _Link = value;
                    Refresh();
                }
            } 
        }

        public WeakSubresource()
        {
        }
        public WeakSubresource(string path) : base(path)
        {
        }
    }
}
