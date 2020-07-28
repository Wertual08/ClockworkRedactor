namespace Resource_Redactor.Descriptions.Redactors
{
    partial class ExplorerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerControl));
            this.ResourcesListView = new System.Windows.Forms.ListView();
            this.MediumIconList = new System.Windows.Forms.ImageList(this.components);
            this.SmallIconList = new System.Windows.Forms.ImageList(this.components);
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.LargeIconList = new System.Windows.Forms.ImageList(this.components);
            this.ResourceWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.ResourceWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // ResourcesListView
            // 
            this.ResourcesListView.AllowDrop = true;
            this.ResourcesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResourcesListView.HideSelection = false;
            this.ResourcesListView.LabelEdit = true;
            this.ResourcesListView.LargeImageList = this.MediumIconList;
            this.ResourcesListView.Location = new System.Drawing.Point(0, 20);
            this.ResourcesListView.Name = "ResourcesListView";
            this.ResourcesListView.Size = new System.Drawing.Size(128, 108);
            this.ResourcesListView.SmallImageList = this.SmallIconList;
            this.ResourcesListView.TabIndex = 3;
            this.ResourcesListView.UseCompatibleStateImageBehavior = false;
            this.ResourcesListView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.ResourcesListView_AfterLabelEdit);
            this.ResourcesListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ResourcesListView_ItemDrag);
            this.ResourcesListView.SelectedIndexChanged += new System.EventHandler(this.ResourcesListView_SelectedIndexChanged);
            this.ResourcesListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.ResourcesListView_DragDrop);
            this.ResourcesListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.ResourcesListView_DragEnter);
            this.ResourcesListView.DragOver += new System.Windows.Forms.DragEventHandler(this.ResourcesListView_DragOver);
            this.ResourcesListView.DoubleClick += new System.EventHandler(this.ResourcesListView_DoubleClick);
            this.ResourcesListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ResourcesListView_KeyDown);
            // 
            // MediumIconList
            // 
            this.MediumIconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MediumIconList.ImageStream")));
            this.MediumIconList.TransparentColor = System.Drawing.Color.Transparent;
            this.MediumIconList.Images.SetKeyName(0, "micon invalid.png");
            this.MediumIconList.Images.SetKeyName(1, "micon warning.png");
            this.MediumIconList.Images.SetKeyName(2, "micon outdated.png");
            this.MediumIconList.Images.SetKeyName(3, "micon pfolder.png");
            this.MediumIconList.Images.SetKeyName(4, "micon folder.png");
            this.MediumIconList.Images.SetKeyName(5, "micon texture.png");
            this.MediumIconList.Images.SetKeyName(6, "micon sound.png");
            this.MediumIconList.Images.SetKeyName(7, "micon tile.png");
            this.MediumIconList.Images.SetKeyName(8, "micon event.png");
            this.MediumIconList.Images.SetKeyName(9, "micon sprite.png");
            this.MediumIconList.Images.SetKeyName(10, "micon entity.png");
            this.MediumIconList.Images.SetKeyName(11, "micon ragdoll.png");
            this.MediumIconList.Images.SetKeyName(12, "micon animation.png");
            this.MediumIconList.Images.SetKeyName(13, "micon outfit.png");
            this.MediumIconList.Images.SetKeyName(14, "micon tool.png");
            this.MediumIconList.Images.SetKeyName(15, "micon item.png");
            this.MediumIconList.Images.SetKeyName(16, "micon particle.png");
            // 
            // SmallIconList
            // 
            this.SmallIconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SmallIconList.ImageStream")));
            this.SmallIconList.TransparentColor = System.Drawing.Color.Transparent;
            this.SmallIconList.Images.SetKeyName(0, "sicon invalid.png");
            this.SmallIconList.Images.SetKeyName(1, "sicon warning.png");
            this.SmallIconList.Images.SetKeyName(2, "sicon outdated.png");
            this.SmallIconList.Images.SetKeyName(3, "sicon pfolder.png");
            this.SmallIconList.Images.SetKeyName(4, "sicon folder.png");
            this.SmallIconList.Images.SetKeyName(5, "sicon texture.png");
            this.SmallIconList.Images.SetKeyName(6, "sicon sound.png");
            this.SmallIconList.Images.SetKeyName(7, "sicon tile.png");
            this.SmallIconList.Images.SetKeyName(8, "sicon event.png");
            this.SmallIconList.Images.SetKeyName(9, "sicon sprite.png");
            this.SmallIconList.Images.SetKeyName(10, "sicon entity.png");
            this.SmallIconList.Images.SetKeyName(11, "sicon ragdoll.png");
            this.SmallIconList.Images.SetKeyName(12, "sicon animation.png");
            this.SmallIconList.Images.SetKeyName(13, "sicon outfit.png");
            this.SmallIconList.Images.SetKeyName(14, "sicon tool.png");
            this.SmallIconList.Images.SetKeyName(15, "sicon item.png");
            this.SmallIconList.Images.SetKeyName(16, "sicon particle.png");
            // 
            // NameTextBox
            // 
            this.NameTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.NameTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NameTextBox.Location = new System.Drawing.Point(0, 0);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(128, 20);
            this.NameTextBox.TabIndex = 4;
            this.NameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NameTextBox_KeyDown);
            this.NameTextBox.Leave += new System.EventHandler(this.NameTextBox_Leave);
            // 
            // LargeIconList
            // 
            this.LargeIconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LargeIconList.ImageStream")));
            this.LargeIconList.TransparentColor = System.Drawing.Color.Transparent;
            this.LargeIconList.Images.SetKeyName(0, "icon invalid.png");
            this.LargeIconList.Images.SetKeyName(1, "Warning.png");
            this.LargeIconList.Images.SetKeyName(2, "icon outdated.png");
            this.LargeIconList.Images.SetKeyName(3, "icon pfolder.png");
            this.LargeIconList.Images.SetKeyName(4, "icon folder.png");
            this.LargeIconList.Images.SetKeyName(5, "icon texture.png");
            this.LargeIconList.Images.SetKeyName(6, "icon sound.png");
            this.LargeIconList.Images.SetKeyName(7, "icon tile.png");
            this.LargeIconList.Images.SetKeyName(8, "icon event.png");
            this.LargeIconList.Images.SetKeyName(9, "icon sprite.png");
            this.LargeIconList.Images.SetKeyName(10, "icon entity.png");
            this.LargeIconList.Images.SetKeyName(11, "icon ragdoll.png");
            this.LargeIconList.Images.SetKeyName(12, "icon animation.png");
            this.LargeIconList.Images.SetKeyName(13, "icon outfit.png");
            this.LargeIconList.Images.SetKeyName(14, "icon tool.png");
            this.LargeIconList.Images.SetKeyName(15, "icon item.png");
            this.LargeIconList.Images.SetKeyName(16, "icon particle.png");
            // 
            // ResourceWatcher
            // 
            this.ResourceWatcher.EnableRaisingEvents = true;
            this.ResourceWatcher.Filter = "*";
            this.ResourceWatcher.SynchronizingObject = this;
            // 
            // ExplorerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ResourcesListView);
            this.Controls.Add(this.NameTextBox);
            this.Name = "ExplorerControl";
            this.Size = new System.Drawing.Size(128, 128);
            ((System.ComponentModel.ISupportInitialize)(this.ResourceWatcher)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ResourcesListView;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.IO.FileSystemWatcher ResourceWatcher;
        public System.Windows.Forms.ImageList MediumIconList;
        public System.Windows.Forms.ImageList SmallIconList;
        public System.Windows.Forms.ImageList LargeIconList;
    }
}
