using ExtraSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor.Descriptions
{
    public interface ISubresource
    {
        EventHandler Reloaded { get; set; }
        EventHandler Updated { get; set; }
        event EventHandler Refreshed;
        ISynchronizeInvoke SynchronizingObject { get; set; }
        string Link { get; set; }
        bool Loaded { get; }
        bool Active { get; set; }
        void Reload();
        void Read(BinaryReader r);
        void Write(BinaryWriter w);
        ResourceType Type { get; }
    }

    public class Subresource<T> : ISubresource, IDisposable where T : Resource, new()
    {
        private FileSystemWatcher Watcher = new FileSystemWatcher();
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

        public ResourceType Type { get { return Descriptions.Resource.GetType(typeof(T)); } }
        public ISynchronizeInvoke SynchronizingObject { get { return Watcher.SynchronizingObject; } set { Watcher.SynchronizingObject = value; } }
        public EventHandler Reloaded { get; set; }
        public EventHandler Updated { get; set; }
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
        public T Resource { get { return Loaded && Active ? _Resource : null; } }
        public bool Loaded { get; protected set; } = false;
        public bool Active { get; set; } = true;

        public Subresource()
        {
            Watcher.Changed += Watcher_Changed;
            Watcher.Created += Watcher_Changed;
            Watcher.Deleted += Watcher_Changed;
            Watcher.Renamed += Watcher_Renamed;
        }
        public Subresource(string path, bool active)
        {
            Watcher.Changed += Watcher_Changed;
            Watcher.Created += Watcher_Changed;
            Watcher.Deleted += Watcher_Changed;
            Watcher.Renamed += Watcher_Renamed;
            Link = path;
            Active = active;
        }
        public Subresource(BinaryReader r)
        {
            Watcher.Changed += Watcher_Changed;
            Watcher.Created += Watcher_Changed;
            Watcher.Deleted += Watcher_Changed;
            Watcher.Renamed += Watcher_Renamed;
            Read(r);
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
                    Loaded = false;
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
                Loaded = true;
            }
            catch 
            {
                Loaded = false;
            }
            Reloaded?.Invoke(this, EventArgs.Empty);
            Refreshed?.Invoke(this, EventArgs.Empty);
        }

        public void Read(BinaryReader r)
        {
            Link = r.ReadString();
            Active = r.ReadBoolean();
        }
        public void Write(BinaryWriter w)
        {
            w.Write(Link);
            w.Write(Active);
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
                    Loaded = false; 
                    Refresh();
                }
            } 
        }

        public WeakSubresource()
        {
        }
        public WeakSubresource(string path, bool active) : base(path, active)
        {
        }
        public WeakSubresource(BinaryReader r) : base(r)
        {
        }
    }
}
