using ExtraSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor.Resources.Redactors
{
    public class ResourceControl<TResource, TStory> : UserControl where TResource : Resource, new() where TStory : class, IStory
    {
        public ToolStripMenuItem[] MenuTabs { get; protected set; }

        public event StateChangedEventHandler StateChanged;

        protected void UpdateRedactor()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        protected void MakeUnsaved()
        {
            Saved = false;
            UpdateRedactor();
        }
        protected ToolStripMenuItem GetTab(string title)
        {
            return MenuTabs.First((ToolStripMenuItem item) => { return item.Text == title; });
        }
        public virtual void Activate() { }

        protected TResource LoadedResource { get; private set; } = new TResource();
        public bool Saved { get; private set; } = true;
        public string ResourcePath { get; private set; }
        public string ResourceName { get; private set; }
        public virtual void Save(string path)
        {
            ResourcePath = path;
            ResourceName = Path.GetFileName(path);

            LoadedResource.Save(path);

            Saved = true;
            UpdateRedactor();
        }
        public virtual void Open(string path)
        {
            LoadedResource.Open(path);
            ResourcePath = path;
            ResourceName = Path.GetFileName(path);
            Saved = true;
        }

        protected TStory Story = null;
        public bool UndoEnabled { get { return Story.PrevState; } }
        public bool RedoEnabled { get { return Story.NextState; } }
        public void Undo()
        {
            Story.Undo();
        }
        public void Redo()
        {
            Story.Redo();
        }

        public virtual void Render() { }
    }
}
