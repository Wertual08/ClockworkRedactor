namespace Resource_Redactor
{
    partial class RedactorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RedactorForm));
            this.RedactorMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SpriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RagdollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OutfitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ParticleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToSpriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToRagdollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToOutfitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DescriptionOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResourceOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadLinksResolverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ApplyLinksResolverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CancelLinksResolverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CompilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UndoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RedoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseOthersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SwitchViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExplorerSplitContainer = new System.Windows.Forms.SplitContainer();
            this.RedactorsTabControl = new System.Windows.Forms.TabControl();
            this.OpenDescriptionDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDescriptionDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImportFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ExportFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.TabsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CloseTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SwapPanelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResourceExplorer = new Resource_Redactor.Resources.Redactors.ExplorerControl();
            this.RedactorMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExplorerSplitContainer)).BeginInit();
            this.ExplorerSplitContainer.Panel1.SuspendLayout();
            this.ExplorerSplitContainer.Panel2.SuspendLayout();
            this.ExplorerSplitContainer.SuspendLayout();
            this.TabsContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // RedactorMenuStrip
            // 
            this.RedactorMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.ResourceToolStripMenuItem});
            this.RedactorMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.RedactorMenuStrip.Name = "RedactorMenuStrip";
            this.RedactorMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.RedactorMenuStrip.TabIndex = 2;
            this.RedactorMenuStrip.Text = "RedactorMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateToolStripMenuItem,
            this.convertToolStripMenuItem,
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.SaveAsToolStripMenuItem,
            this.SaveAllToolStripMenuItem,
            this.DeleteToolStripMenuItem,
            this.RenameToolStripMenuItem,
            this.CopyToolStripMenuItem,
            this.CutToolStripMenuItem,
            this.PasteToolStripMenuItem,
            this.ToolsToolStripMenuItem,
            this.CompilerToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // CreateToolStripMenuItem
            // 
            this.CreateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DescriptionToolStripMenuItem,
            this.FolderToolStripMenuItem,
            this.TextureToolStripMenuItem,
            this.SoundToolStripMenuItem,
            this.TileToolStripMenuItem,
            this.EventToolStripMenuItem,
            this.SpriteToolStripMenuItem,
            this.EntityToolStripMenuItem,
            this.RagdollToolStripMenuItem,
            this.AnimationToolStripMenuItem,
            this.OutfitToolStripMenuItem,
            this.ToolToolStripMenuItem,
            this.ItemToolStripMenuItem,
            this.ParticleToolStripMenuItem});
            this.CreateToolStripMenuItem.Name = "CreateToolStripMenuItem";
            this.CreateToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.CreateToolStripMenuItem.Text = "Create";
            // 
            // DescriptionToolStripMenuItem
            // 
            this.DescriptionToolStripMenuItem.Name = "DescriptionToolStripMenuItem";
            this.DescriptionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.DescriptionToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.DescriptionToolStripMenuItem.Text = "Description";
            this.DescriptionToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // FolderToolStripMenuItem
            // 
            this.FolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("FolderToolStripMenuItem.Image")));
            this.FolderToolStripMenuItem.Name = "FolderToolStripMenuItem";
            this.FolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.FolderToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.FolderToolStripMenuItem.Text = "Folder";
            this.FolderToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // TextureToolStripMenuItem
            // 
            this.TextureToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("TextureToolStripMenuItem.Image")));
            this.TextureToolStripMenuItem.Name = "TextureToolStripMenuItem";
            this.TextureToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.TextureToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.TextureToolStripMenuItem.Text = "Texture";
            this.TextureToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // SoundToolStripMenuItem
            // 
            this.SoundToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("SoundToolStripMenuItem.Image")));
            this.SoundToolStripMenuItem.Name = "SoundToolStripMenuItem";
            this.SoundToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            this.SoundToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.SoundToolStripMenuItem.Text = "Sound";
            this.SoundToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // TileToolStripMenuItem
            // 
            this.TileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("TileToolStripMenuItem.Image")));
            this.TileToolStripMenuItem.Name = "TileToolStripMenuItem";
            this.TileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.TileToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.TileToolStripMenuItem.Text = "Tile";
            this.TileToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // EventToolStripMenuItem
            // 
            this.EventToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("EventToolStripMenuItem.Image")));
            this.EventToolStripMenuItem.Name = "EventToolStripMenuItem";
            this.EventToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.V)));
            this.EventToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.EventToolStripMenuItem.Text = "Event";
            this.EventToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // SpriteToolStripMenuItem
            // 
            this.SpriteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("SpriteToolStripMenuItem.Image")));
            this.SpriteToolStripMenuItem.Name = "SpriteToolStripMenuItem";
            this.SpriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.SpriteToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.SpriteToolStripMenuItem.Text = "Sprite";
            this.SpriteToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // EntityToolStripMenuItem
            // 
            this.EntityToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("EntityToolStripMenuItem.Image")));
            this.EntityToolStripMenuItem.Name = "EntityToolStripMenuItem";
            this.EntityToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
            this.EntityToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.EntityToolStripMenuItem.Text = "Entity";
            this.EntityToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // RagdollToolStripMenuItem
            // 
            this.RagdollToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("RagdollToolStripMenuItem.Image")));
            this.RagdollToolStripMenuItem.Name = "RagdollToolStripMenuItem";
            this.RagdollToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.RagdollToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.RagdollToolStripMenuItem.Text = "Ragdoll";
            this.RagdollToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // AnimationToolStripMenuItem
            // 
            this.AnimationToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("AnimationToolStripMenuItem.Image")));
            this.AnimationToolStripMenuItem.Name = "AnimationToolStripMenuItem";
            this.AnimationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.AnimationToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.AnimationToolStripMenuItem.Text = "Animation";
            this.AnimationToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // OutfitToolStripMenuItem
            // 
            this.OutfitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("OutfitToolStripMenuItem.Image")));
            this.OutfitToolStripMenuItem.Name = "OutfitToolStripMenuItem";
            this.OutfitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
            this.OutfitToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.OutfitToolStripMenuItem.Text = "Outfit";
            this.OutfitToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // ToolToolStripMenuItem
            // 
            this.ToolToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ToolToolStripMenuItem.Image")));
            this.ToolToolStripMenuItem.Name = "ToolToolStripMenuItem";
            this.ToolToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Q)));
            this.ToolToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.ToolToolStripMenuItem.Text = "Tool";
            this.ToolToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // ItemToolStripMenuItem
            // 
            this.ItemToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ItemToolStripMenuItem.Image")));
            this.ItemToolStripMenuItem.Name = "ItemToolStripMenuItem";
            this.ItemToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.I)));
            this.ItemToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.ItemToolStripMenuItem.Text = "Item";
            this.ItemToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // ParticleToolStripMenuItem
            // 
            this.ParticleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ParticleToolStripMenuItem.Image")));
            this.ParticleToolStripMenuItem.Name = "ParticleToolStripMenuItem";
            this.ParticleToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.ParticleToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.ParticleToolStripMenuItem.Text = "Particle";
            this.ParticleToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // convertToolStripMenuItem
            // 
            this.convertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToSpriteToolStripMenuItem,
            this.ToRagdollToolStripMenuItem,
            this.ToOutfitToolStripMenuItem});
            this.convertToolStripMenuItem.Name = "convertToolStripMenuItem";
            this.convertToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.convertToolStripMenuItem.Text = "Convert";
            // 
            // ToSpriteToolStripMenuItem
            // 
            this.ToSpriteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ToSpriteToolStripMenuItem.Image")));
            this.ToSpriteToolStripMenuItem.Name = "ToSpriteToolStripMenuItem";
            this.ToSpriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.ToSpriteToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.ToSpriteToolStripMenuItem.Text = "To sprite";
            this.ToSpriteToolStripMenuItem.Click += new System.EventHandler(this.ToSpriteToolStripMenuItem_Click);
            // 
            // ToRagdollToolStripMenuItem
            // 
            this.ToRagdollToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ToRagdollToolStripMenuItem.Image")));
            this.ToRagdollToolStripMenuItem.Name = "ToRagdollToolStripMenuItem";
            this.ToRagdollToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.ToRagdollToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.ToRagdollToolStripMenuItem.Text = "To ragdoll";
            this.ToRagdollToolStripMenuItem.Click += new System.EventHandler(this.ToRagdollToolStripMenuItem_Click);
            // 
            // ToOutfitToolStripMenuItem
            // 
            this.ToOutfitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ToOutfitToolStripMenuItem.Image")));
            this.ToOutfitToolStripMenuItem.Name = "ToOutfitToolStripMenuItem";
            this.ToOutfitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.ToOutfitToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.ToOutfitToolStripMenuItem.Text = "To outfit";
            this.ToOutfitToolStripMenuItem.Click += new System.EventHandler(this.ToOutfitToolStripMenuItem_Click);
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DescriptionOpenToolStripMenuItem,
            this.ResourceOpenToolStripMenuItem});
            this.OpenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("OpenToolStripMenuItem.Image")));
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.OpenToolStripMenuItem.Text = "Open";
            // 
            // DescriptionOpenToolStripMenuItem
            // 
            this.DescriptionOpenToolStripMenuItem.Name = "DescriptionOpenToolStripMenuItem";
            this.DescriptionOpenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.DescriptionOpenToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.DescriptionOpenToolStripMenuItem.Text = "Description";
            this.DescriptionOpenToolStripMenuItem.Click += new System.EventHandler(this.DescriptionOpenToolStripMenuItem_Click);
            // 
            // ResourceOpenToolStripMenuItem
            // 
            this.ResourceOpenToolStripMenuItem.Name = "ResourceOpenToolStripMenuItem";
            this.ResourceOpenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ResourceOpenToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.ResourceOpenToolStripMenuItem.Text = "Resource";
            this.ResourceOpenToolStripMenuItem.Click += new System.EventHandler(this.ResourceOpenToolStripMenuItem_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("SaveToolStripMenuItem.Image")));
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.SaveToolStripMenuItem.Text = "Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("SaveAsToolStripMenuItem.Image")));
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.SaveAsToolStripMenuItem.Text = "Save As";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // SaveAllToolStripMenuItem
            // 
            this.SaveAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("SaveAllToolStripMenuItem.Image")));
            this.SaveAllToolStripMenuItem.Name = "SaveAllToolStripMenuItem";
            this.SaveAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.SaveAllToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.SaveAllToolStripMenuItem.Text = "Save All";
            this.SaveAllToolStripMenuItem.Click += new System.EventHandler(this.SaveAllToolStripMenuItem_Click);
            // 
            // DeleteToolStripMenuItem
            // 
            this.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            this.DeleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.DeleteToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.DeleteToolStripMenuItem.Text = "Delete";
            this.DeleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // RenameToolStripMenuItem
            // 
            this.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem";
            this.RenameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.RenameToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.RenameToolStripMenuItem.Text = "Rename";
            this.RenameToolStripMenuItem.Click += new System.EventHandler(this.RenameToolStripMenuItem_Click);
            // 
            // CopyToolStripMenuItem
            // 
            this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
            this.CopyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.CopyToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.CopyToolStripMenuItem.Text = "Copy";
            this.CopyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // CutToolStripMenuItem
            // 
            this.CutToolStripMenuItem.Name = "CutToolStripMenuItem";
            this.CutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.CutToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.CutToolStripMenuItem.Text = "Cut";
            this.CutToolStripMenuItem.Click += new System.EventHandler(this.CutToolStripMenuItem_Click);
            // 
            // PasteToolStripMenuItem
            // 
            this.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem";
            this.PasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.PasteToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.PasteToolStripMenuItem.Text = "Paste";
            this.PasteToolStripMenuItem.Click += new System.EventHandler(this.PasteToolStripMenuItem_Click);
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadLinksResolverToolStripMenuItem,
            this.ApplyLinksResolverToolStripMenuItem,
            this.CancelLinksResolverToolStripMenuItem});
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.ToolsToolStripMenuItem.Text = "Tools";
            // 
            // LoadLinksResolverToolStripMenuItem
            // 
            this.LoadLinksResolverToolStripMenuItem.Name = "LoadLinksResolverToolStripMenuItem";
            this.LoadLinksResolverToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.LoadLinksResolverToolStripMenuItem.Text = "Load Links Resolver";
            // 
            // ApplyLinksResolverToolStripMenuItem
            // 
            this.ApplyLinksResolverToolStripMenuItem.Enabled = false;
            this.ApplyLinksResolverToolStripMenuItem.Name = "ApplyLinksResolverToolStripMenuItem";
            this.ApplyLinksResolverToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.ApplyLinksResolverToolStripMenuItem.Text = "Apply Links Resolver";
            // 
            // CancelLinksResolverToolStripMenuItem
            // 
            this.CancelLinksResolverToolStripMenuItem.Enabled = false;
            this.CancelLinksResolverToolStripMenuItem.Name = "CancelLinksResolverToolStripMenuItem";
            this.CancelLinksResolverToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.CancelLinksResolverToolStripMenuItem.Text = "Cancel Links Resolver";
            // 
            // CompilerToolStripMenuItem
            // 
            this.CompilerToolStripMenuItem.Name = "CompilerToolStripMenuItem";
            this.CompilerToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.CompilerToolStripMenuItem.Text = "Compiler";
            this.CompilerToolStripMenuItem.Click += new System.EventHandler(this.CompilerToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoToolStripMenuItem,
            this.RedoToolStripMenuItem,
            this.CloseToolStripMenuItem,
            this.CloseAllToolStripMenuItem,
            this.CloseOthersToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // UndoToolStripMenuItem
            // 
            this.UndoToolStripMenuItem.Enabled = false;
            this.UndoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("UndoToolStripMenuItem.Image")));
            this.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem";
            this.UndoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.UndoToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.UndoToolStripMenuItem.Text = "Undo";
            this.UndoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItem_Click);
            // 
            // RedoToolStripMenuItem
            // 
            this.RedoToolStripMenuItem.Enabled = false;
            this.RedoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("RedoToolStripMenuItem.Image")));
            this.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem";
            this.RedoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.RedoToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.RedoToolStripMenuItem.Text = "Redo";
            this.RedoToolStripMenuItem.Click += new System.EventHandler(this.RedoToolStripMenuItem_Click);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("CloseToolStripMenuItem.Image")));
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.CloseToolStripMenuItem.Text = "Close";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // CloseAllToolStripMenuItem
            // 
            this.CloseAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("CloseAllToolStripMenuItem.Image")));
            this.CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem";
            this.CloseAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
            this.CloseAllToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.CloseAllToolStripMenuItem.Text = "Close All";
            this.CloseAllToolStripMenuItem.Click += new System.EventHandler(this.CloseAllToolStripMenuItem_Click);
            // 
            // CloseOthersToolStripMenuItem
            // 
            this.CloseOthersToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("CloseOthersToolStripMenuItem.Image")));
            this.CloseOthersToolStripMenuItem.Name = "CloseOthersToolStripMenuItem";
            this.CloseOthersToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Q)));
            this.CloseOthersToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.CloseOthersToolStripMenuItem.Text = "Close Others";
            this.CloseOthersToolStripMenuItem.Click += new System.EventHandler(this.CloseOthersToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToggleExplorerToolStripMenuItem,
            this.SwitchViewToolStripMenuItem,
            this.SwapPanelsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // ToggleExplorerToolStripMenuItem
            // 
            this.ToggleExplorerToolStripMenuItem.Name = "ToggleExplorerToolStripMenuItem";
            this.ToggleExplorerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.ToggleExplorerToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.ToggleExplorerToolStripMenuItem.Text = "Toggle Explorer";
            this.ToggleExplorerToolStripMenuItem.Click += new System.EventHandler(this.ToggleExplorerToolStripMenuItem_Click);
            // 
            // SwitchViewToolStripMenuItem
            // 
            this.SwitchViewToolStripMenuItem.Name = "SwitchViewToolStripMenuItem";
            this.SwitchViewToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.E)));
            this.SwitchViewToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.SwitchViewToolStripMenuItem.Text = "Switch View";
            this.SwitchViewToolStripMenuItem.Click += new System.EventHandler(this.SwitchViewToolStripMenuItem_Click);
            // 
            // ResourceToolStripMenuItem
            // 
            this.ResourceToolStripMenuItem.Name = "ResourceToolStripMenuItem";
            this.ResourceToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.ResourceToolStripMenuItem.Text = "Resource";
            // 
            // ExplorerSplitContainer
            // 
            this.ExplorerSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ExplorerSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExplorerSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.ExplorerSplitContainer.Location = new System.Drawing.Point(0, 24);
            this.ExplorerSplitContainer.Name = "ExplorerSplitContainer";
            // 
            // ExplorerSplitContainer.Panel1
            // 
            this.ExplorerSplitContainer.Panel1.Controls.Add(this.ResourceExplorer);
            // 
            // ExplorerSplitContainer.Panel2
            // 
            this.ExplorerSplitContainer.Panel2.Controls.Add(this.RedactorsTabControl);
            this.ExplorerSplitContainer.Size = new System.Drawing.Size(800, 426);
            this.ExplorerSplitContainer.SplitterDistance = 251;
            this.ExplorerSplitContainer.TabIndex = 3;
            // 
            // RedactorsTabControl
            // 
            this.RedactorsTabControl.AllowDrop = true;
            this.RedactorsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RedactorsTabControl.HotTrack = true;
            this.RedactorsTabControl.Location = new System.Drawing.Point(0, 0);
            this.RedactorsTabControl.Name = "RedactorsTabControl";
            this.RedactorsTabControl.Padding = new System.Drawing.Point(3, 3);
            this.RedactorsTabControl.SelectedIndex = 0;
            this.RedactorsTabControl.Size = new System.Drawing.Size(541, 422);
            this.RedactorsTabControl.TabIndex = 0;
            this.RedactorsTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.RedactorsTabControl_DrawItem);
            this.RedactorsTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.RedactorsTabControl_Selected);
            this.RedactorsTabControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.RedactorsTabControl_DragEnter);
            this.RedactorsTabControl.DragOver += new System.Windows.Forms.DragEventHandler(this.RedactorsTabControl_DragOver);
            this.RedactorsTabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RedactorsTabControl_MouseClick);
            this.RedactorsTabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RedactorsTabControl_MouseDown);
            this.RedactorsTabControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RedactorsTabControl_MouseMove);
            this.RedactorsTabControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RedactorsTabControl_MouseUp);
            // 
            // TabsContextMenuStrip
            // 
            this.TabsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseTabToolStripMenuItem});
            this.TabsContextMenuStrip.Name = "TabsContextMenuStrip";
            this.TabsContextMenuStrip.Size = new System.Drawing.Size(104, 26);
            // 
            // CloseTabToolStripMenuItem
            // 
            this.CloseTabToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("CloseTabToolStripMenuItem.Image")));
            this.CloseTabToolStripMenuItem.Name = "CloseTabToolStripMenuItem";
            this.CloseTabToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.CloseTabToolStripMenuItem.Text = "Close";
            this.CloseTabToolStripMenuItem.Click += new System.EventHandler(this.CloseTabToolStripMenuItem_Click);
            // 
            // SwapPanelsToolStripMenuItem
            // 
            this.SwapPanelsToolStripMenuItem.Name = "SwapPanelsToolStripMenuItem";
            this.SwapPanelsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.SwapPanelsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.SwapPanelsToolStripMenuItem.Text = "Swap Panels";
            this.SwapPanelsToolStripMenuItem.Click += new System.EventHandler(this.SwapPanelsToolStripMenuItem_Click);
            // 
            // ResourceExplorer
            // 
            this.ResourceExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResourceExplorer.Location = new System.Drawing.Point(0, 0);
            this.ResourceExplorer.MultiSelect = true;
            this.ResourceExplorer.Name = "ResourceExplorer";
            this.ResourceExplorer.Size = new System.Drawing.Size(247, 422);
            this.ResourceExplorer.TabIndex = 0;
            this.ResourceExplorer.ViewMode = Resource_Redactor.Resources.Redactors.ListViewMode.MediumIcon;
            this.ResourceExplorer.ItemLoaded += new Resource_Redactor.Resources.Redactors.ExplorerControl.ItemLoadedEventHandler(this.ResourceExplorer_ItemLoaded);
            this.ResourceExplorer.StateChanged += new Resource_Redactor.Resources.Redactors.ExplorerControl.StateChangedEventHandler(this.ResourceExplorer_StateChanged);
            // 
            // RedactorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ExplorerSplitContainer);
            this.Controls.Add(this.RedactorMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RedactorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExplorerForm_FormClosing);
            this.RedactorMenuStrip.ResumeLayout(false);
            this.RedactorMenuStrip.PerformLayout();
            this.ExplorerSplitContainer.Panel1.ResumeLayout(false);
            this.ExplorerSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ExplorerSplitContainer)).EndInit();
            this.ExplorerSplitContainer.ResumeLayout(false);
            this.TabsContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip RedactorMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.SplitContainer ExplorerSplitContainer;
        private System.Windows.Forms.OpenFileDialog OpenDescriptionDialog;
        private System.Windows.Forms.SaveFileDialog SaveDescriptionDialog;
        private System.Windows.Forms.TabControl RedactorsTabControl;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UndoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RedoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DescriptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TextureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SpriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EntityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RagdollToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OutfitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ParticleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RenameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private Resources.Redactors.ExplorerControl ResourceExplorer;
        private System.Windows.Forms.ToolStripMenuItem ToggleExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseOthersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToSpriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToRagdollToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SwitchViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DescriptionOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResourceOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadLinksResolverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ApplyLinksResolverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CancelLinksResolverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CompilerToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ImportFileDialog;
        private System.Windows.Forms.FolderBrowserDialog ExportFolderBrowserDialog;
        private System.Windows.Forms.ContextMenuStrip TabsContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem CloseTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToOutfitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SwapPanelsToolStripMenuItem;
    }
}

