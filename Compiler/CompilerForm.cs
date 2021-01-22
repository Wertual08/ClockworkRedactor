using ExtraSharp;
using Resource_Redactor.Resources;
using Resource_Redactor.Resources.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor.Compiler
{
    public partial class CompilerForm : Form
    {
        private static readonly string Version = "0.0.0.0";
        private MessageQueue LogQueue = new MessageQueue();

        private string TablePath;
        private IDTable Table = new IDTable()
        {
            Delimiter = (string path) => Resource.GetType(path).ToString(),
            Validator = (string path) => Resource.GetType(path).Valid(),
        };

        private void CompileTiles()
        {
            var cat = ResourceType.Tile.ToString();
            if (!Table.Categories.ContainsKey(cat)) return;

            LogQueue.Put("Compiling tiles...");
            var compiler = new TileCompiler(Table, LogQueue);
            foreach (var t in Table.Categories[cat])
                compiler.Compile(t.Path, t.ID);
            LogQueue.Put("Tiles compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Tiles")))
                compiler.Write(w);
        }
        private void CompileEvents()
        {
            var cat = ResourceType.Event.ToString();
            if (!Table.Categories.ContainsKey(cat)) return;

            LogQueue.Put("Compiling events...");
            var compiler = new EventCompiler(Table, LogQueue);
            foreach (var e in Table.Categories[cat])
                compiler.Compile(e.Path, e.ID);
            LogQueue.Put("Events compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Events")))
                compiler.Write(w);
        }
        private void CompileSprites()
        {
            var cat = ResourceType.Sprite.ToString();
            if (!Table.Categories.ContainsKey(cat)) return;

            LogQueue.Put("Compiling sprites...");
            var compiler = new SpriteCompiler(Table, LogQueue);
            foreach (var s in Table.Categories[cat])
                compiler.Compile(s.Path, s.ID);
            LogQueue.Put("Sprites compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Sprites")))
                compiler.Write(w);
        }
        private void CompileRagdolls()
        {
            var cat = ResourceType.Ragdoll.ToString();
            if (!Table.Categories.ContainsKey(cat)) return;

            LogQueue.Put("Compiling ragdolls...");
            var compiler = new RagdollCompiler(Table, LogQueue);
            foreach (var r in Table.Categories[cat])
                compiler.Compile(r.Path, r.ID);
            LogQueue.Put("Ragdolls compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Ragdolls")))
                compiler.Write(w);
        }
        private void CompileAnimations()
        {
            var cat = ResourceType.Animation.ToString();
            if (!Table.Categories.ContainsKey(cat)) return;

            LogQueue.Put("Compiling animations...");
            var compiler = new AnimationCompiler(Table, LogQueue);
            foreach (var r in Table.Categories[cat])
                compiler.Compile(r.Path, r.ID);
            LogQueue.Put("Animations compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Animations")))
                compiler.Write(w);
        }
        private void CompileEntities()
        {
            var cat = ResourceType.Entity.ToString();
            if (!Table.Categories.ContainsKey(cat)) return;

            LogQueue.Put("Compiling entities...");
            var compiler = new EntityCompiler(Table, LogQueue);
            foreach (var e in Table.Categories[cat])
                compiler.Compile(e.Path, e.ID);
            LogQueue.Put("Entities compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Entities")))
                compiler.Write(w);
        }
        private void CompileOutfits()
        {
            var cat = ResourceType.Outfit.ToString();
            if (!Table.Categories.ContainsKey(cat)) return;

            LogQueue.Put("Compiling outfits...");
            var compiler = new OutfitCompiler(Table, LogQueue);
            foreach (var e in Table.Categories[cat])
                compiler.Compile(e.Path, e.ID);
            LogQueue.Put("Outfits compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Outfits")))
                compiler.Write(w);
        }
        private void CompileInterfaces()
        {
            var cat = ResourceType.Interface.ToString();
            if (!Table.Categories.ContainsKey(cat)) return;

            LogQueue.Put("Compiling interfaces...");
            var compiler = new InterfaceCompiler(Table, LogQueue);
            foreach (var i in Table.Categories[cat])
                compiler.Compile(i.Path, i.ID);
            LogQueue.Put("Interfaces compiled.");

            Directory.CreateDirectory("../Compilation");
            using (var w = new BinaryWriter(File.Create("../Compilation/Interface"), new UTF8Encoding()))
                compiler.Write(w);
            using (var w = new BinaryWriter(File.Create("../Compilation/InterfaceTexture")))
                compiler.WriteTexture(w);
        }

        public CompilerForm(string table)
        {
            InitializeComponent();
            Text = "Clockwork engine resource compiler V" + Version;
            TablePath = table;

            MainSplitContainer.Panel1.Enabled = false;
            LoaderWorker.RunWorkerAsync();
        }

        private void LoaderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoaderWorker.ReportProgress(0);

            if (File.Exists(TablePath))
            {
                LogQueue.Put("Reading ID table...");
                using (var file = File.OpenText(TablePath))
                    Table.Read(file);

                foreach (var items in Table.Categories)
                {
                    string name = items.Key;
                    int loaded = items.Value.Count;
                    int valid = items.Value.Count((IDTable.Item i) => { return i.Valid; });
                    LogQueue.Put($"{name} loaded: {loaded}");
                    LogQueue.Put($"{name} valid: {valid}");
                }
                LogQueue.Put("Reading ID table done.");
            }
            else LogQueue.Put("ID table not found.");
            LogQueue.Put("");

            LoaderWorker.ReportProgress(5);

            LogQueue.Put("Indexing files...");
            string[] paths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; i++) paths[i] = ExtraPath.MakeDirectoryRelated(Directory.GetCurrentDirectory(), paths[i]);
            LogQueue.Put("Files found: " + paths.Length);
            LogQueue.Put("Indexing files done.");
            LogQueue.Put("");

            LoaderWorker.ReportProgress(10);

            LogQueue.Put("Searching resources...");
            int progress = 0;
            for (int i = 0; i < paths.Length; i++)
            {
                int current_progress = i * 90 / (paths.Length - 1);
                if (progress != current_progress)
                {
                    progress = current_progress;
                    LoaderWorker.ReportProgress(10 + progress);
                }    

                var path = paths[i];
                Table.Add(path);
            }
            foreach (var items in Table.Categories)
            {
                string name = items.Key;
                int found = items.Value.Count;
                LogQueue.Put($"{name} found: {found}");
            }
            LogQueue.Put($"Total resources found: {Table.Count}");
            LogQueue.Put("Searching resources done.");
            LogQueue.Put("");
        }
        private void LoaderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CompilerProgressBar.Value = e.ProgressPercentage;
        }
        private void LoaderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            int id = 0;
            ResourcesListBox.BeginUpdate();
            ResourcesListBox.Items.Clear();
            foreach (var cat in Table.Categories)
            {
                foreach (var i in cat.Value)
                {
                    while (id < i.ID - 1) ResourcesListBox.Items.Add($"[{cat.Key}][{(id++)}]<:EMPTY:>");
                    id = i.ID;
                    ResourcesListBox.Items.Add($"[{cat.Key}][{id}][{(i.Valid ? "V" : "I")}][{(i.Old ? "old" : "new")}]{i.Path}");
                }
            }
            ResourcesListBox.EndUpdate();

            CompilerProgressBar.Value = 0;
            MainSplitContainer.Panel1.Enabled = true;
        }

        private void CompilerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CompileTiles();
            LogQueue.Put("");
            CompileEvents();
            LogQueue.Put("");
            CompileSprites();
            LogQueue.Put("");
            CompileRagdolls();
            LogQueue.Put("");
            CompileAnimations();
            LogQueue.Put("");
            CompileEntities();
            LogQueue.Put("");
            CompileOutfits();
            LogQueue.Put("");
            CompileInterfaces();
            LogQueue.Put("");

            LogQueue.Put("Updating ID table [" + TablePath + "]...");
            using (var file = File.CreateText(TablePath))
            {
                var cats = new string[] { "Tile", "Event", "Entity", "Outfit", "Item" };
                Table.Write(file, cats);
            }
            LogQueue.Put("ID table updated.");
        }
        private void CompilerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled) LogQueue.Put("Compilation successful.");
            else LogQueue.Put("Compilation terminated with error:" + Environment.NewLine + e.Error?.ToString());
            LogQueue.Put(Environment.NewLine);
            CompileButton.Enabled = true;
        }

        private void CompileButton_Click(object sender, EventArgs e)
        {
            CompileButton.Enabled = false;
            LogQueue.Put("");
            LogQueue.Put("Starting compilation...");
            LogQueue.Put("");
            CompilerWorker.RunWorkerAsync();
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            string message;
            while ((message = LogQueue.Get()) != null) 
                LogTextBox.AppendText(message + Environment.NewLine);
        }
    }
}
