namespace Resource_Redactor.Compiler
{
    partial class CompilerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompilerForm));
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.CompilerProgressBar = new System.Windows.Forms.ProgressBar();
            this.ExitButton = new System.Windows.Forms.Button();
            this.CompileButton = new System.Windows.Forms.Button();
            this.ResourcesTabControl = new System.Windows.Forms.TabControl();
            this.TilesTabPage = new System.Windows.Forms.TabPage();
            this.TilesListBox = new System.Windows.Forms.ListBox();
            this.SpritesTabPage = new System.Windows.Forms.TabPage();
            this.SpritesListBox = new System.Windows.Forms.ListBox();
            this.EventsTabPage = new System.Windows.Forms.TabPage();
            this.EventsListBox = new System.Windows.Forms.ListBox();
            this.EntitiesTabPage = new System.Windows.Forms.TabPage();
            this.EntitiesListBox = new System.Windows.Forms.ListBox();
            this.OutfitsTabPage = new System.Windows.Forms.TabPage();
            this.OutfitsListBox = new System.Windows.Forms.ListBox();
            this.ItemsTabPage = new System.Windows.Forms.TabPage();
            this.ItemsListBox = new System.Windows.Forms.ListBox();
            this.InterfacesTabPage = new System.Windows.Forms.TabPage();
            this.InterfacesListBox = new System.Windows.Forms.ListBox();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.LogTimer = new System.Windows.Forms.Timer(this.components);
            this.CompilerWorker = new System.ComponentModel.BackgroundWorker();
            this.LoaderWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.ResourcesTabControl.SuspendLayout();
            this.TilesTabPage.SuspendLayout();
            this.SpritesTabPage.SuspendLayout();
            this.EventsTabPage.SuspendLayout();
            this.EntitiesTabPage.SuspendLayout();
            this.OutfitsTabPage.SuspendLayout();
            this.ItemsTabPage.SuspendLayout();
            this.InterfacesTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.CompilerProgressBar);
            this.MainSplitContainer.Panel1.Controls.Add(this.ExitButton);
            this.MainSplitContainer.Panel1.Controls.Add(this.CompileButton);
            this.MainSplitContainer.Panel1.Controls.Add(this.ResourcesTabControl);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.LogTextBox);
            this.MainSplitContainer.Size = new System.Drawing.Size(800, 450);
            this.MainSplitContainer.SplitterDistance = 315;
            this.MainSplitContainer.TabIndex = 0;
            // 
            // CompilerProgressBar
            // 
            this.CompilerProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CompilerProgressBar.ForeColor = System.Drawing.Color.Purple;
            this.CompilerProgressBar.Location = new System.Drawing.Point(3, 391);
            this.CompilerProgressBar.Name = "CompilerProgressBar";
            this.CompilerProgressBar.Size = new System.Drawing.Size(305, 23);
            this.CompilerProgressBar.Step = 1;
            this.CompilerProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.CompilerProgressBar.TabIndex = 1;
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.Location = new System.Drawing.Point(233, 420);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // CompileButton
            // 
            this.CompileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CompileButton.Location = new System.Drawing.Point(3, 420);
            this.CompileButton.Name = "CompileButton";
            this.CompileButton.Size = new System.Drawing.Size(75, 23);
            this.CompileButton.TabIndex = 1;
            this.CompileButton.Text = "Compile";
            this.CompileButton.UseVisualStyleBackColor = true;
            this.CompileButton.Click += new System.EventHandler(this.CompileButton_Click);
            // 
            // ResourcesTabControl
            // 
            this.ResourcesTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourcesTabControl.Controls.Add(this.TilesTabPage);
            this.ResourcesTabControl.Controls.Add(this.SpritesTabPage);
            this.ResourcesTabControl.Controls.Add(this.EventsTabPage);
            this.ResourcesTabControl.Controls.Add(this.EntitiesTabPage);
            this.ResourcesTabControl.Controls.Add(this.OutfitsTabPage);
            this.ResourcesTabControl.Controls.Add(this.ItemsTabPage);
            this.ResourcesTabControl.Controls.Add(this.InterfacesTabPage);
            this.ResourcesTabControl.Location = new System.Drawing.Point(-2, 0);
            this.ResourcesTabControl.Name = "ResourcesTabControl";
            this.ResourcesTabControl.SelectedIndex = 0;
            this.ResourcesTabControl.Size = new System.Drawing.Size(315, 385);
            this.ResourcesTabControl.TabIndex = 0;
            // 
            // TilesTabPage
            // 
            this.TilesTabPage.Controls.Add(this.TilesListBox);
            this.TilesTabPage.Location = new System.Drawing.Point(4, 22);
            this.TilesTabPage.Name = "TilesTabPage";
            this.TilesTabPage.Size = new System.Drawing.Size(307, 359);
            this.TilesTabPage.TabIndex = 5;
            this.TilesTabPage.Text = "Tiles";
            this.TilesTabPage.UseVisualStyleBackColor = true;
            // 
            // TilesListBox
            // 
            this.TilesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TilesListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TilesListBox.FormattingEnabled = true;
            this.TilesListBox.Location = new System.Drawing.Point(0, 0);
            this.TilesListBox.Name = "TilesListBox";
            this.TilesListBox.Size = new System.Drawing.Size(307, 359);
            this.TilesListBox.TabIndex = 3;
            // 
            // SpritesTabPage
            // 
            this.SpritesTabPage.Controls.Add(this.SpritesListBox);
            this.SpritesTabPage.Location = new System.Drawing.Point(4, 22);
            this.SpritesTabPage.Name = "SpritesTabPage";
            this.SpritesTabPage.Size = new System.Drawing.Size(307, 359);
            this.SpritesTabPage.TabIndex = 8;
            this.SpritesTabPage.Text = "Sprites";
            this.SpritesTabPage.UseVisualStyleBackColor = true;
            // 
            // SpritesListBox
            // 
            this.SpritesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpritesListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SpritesListBox.FormattingEnabled = true;
            this.SpritesListBox.Location = new System.Drawing.Point(0, 0);
            this.SpritesListBox.Name = "SpritesListBox";
            this.SpritesListBox.Size = new System.Drawing.Size(307, 359);
            this.SpritesListBox.TabIndex = 4;
            // 
            // EventsTabPage
            // 
            this.EventsTabPage.Controls.Add(this.EventsListBox);
            this.EventsTabPage.Location = new System.Drawing.Point(4, 22);
            this.EventsTabPage.Name = "EventsTabPage";
            this.EventsTabPage.Size = new System.Drawing.Size(307, 359);
            this.EventsTabPage.TabIndex = 6;
            this.EventsTabPage.Text = "Events";
            this.EventsTabPage.UseVisualStyleBackColor = true;
            // 
            // EventsListBox
            // 
            this.EventsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventsListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EventsListBox.FormattingEnabled = true;
            this.EventsListBox.Location = new System.Drawing.Point(0, 0);
            this.EventsListBox.Name = "EventsListBox";
            this.EventsListBox.Size = new System.Drawing.Size(307, 359);
            this.EventsListBox.TabIndex = 8;
            // 
            // EntitiesTabPage
            // 
            this.EntitiesTabPage.Controls.Add(this.EntitiesListBox);
            this.EntitiesTabPage.Location = new System.Drawing.Point(4, 22);
            this.EntitiesTabPage.Name = "EntitiesTabPage";
            this.EntitiesTabPage.Size = new System.Drawing.Size(307, 359);
            this.EntitiesTabPage.TabIndex = 2;
            this.EntitiesTabPage.Text = "Entities";
            this.EntitiesTabPage.UseVisualStyleBackColor = true;
            // 
            // EntitiesListBox
            // 
            this.EntitiesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntitiesListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EntitiesListBox.FormattingEnabled = true;
            this.EntitiesListBox.Location = new System.Drawing.Point(0, 0);
            this.EntitiesListBox.Name = "EntitiesListBox";
            this.EntitiesListBox.Size = new System.Drawing.Size(307, 359);
            this.EntitiesListBox.TabIndex = 3;
            // 
            // OutfitsTabPage
            // 
            this.OutfitsTabPage.Controls.Add(this.OutfitsListBox);
            this.OutfitsTabPage.Location = new System.Drawing.Point(4, 22);
            this.OutfitsTabPage.Name = "OutfitsTabPage";
            this.OutfitsTabPage.Size = new System.Drawing.Size(307, 359);
            this.OutfitsTabPage.TabIndex = 3;
            this.OutfitsTabPage.Text = "Outfits";
            this.OutfitsTabPage.UseVisualStyleBackColor = true;
            // 
            // OutfitsListBox
            // 
            this.OutfitsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutfitsListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OutfitsListBox.FormattingEnabled = true;
            this.OutfitsListBox.Location = new System.Drawing.Point(0, 0);
            this.OutfitsListBox.Name = "OutfitsListBox";
            this.OutfitsListBox.Size = new System.Drawing.Size(307, 359);
            this.OutfitsListBox.TabIndex = 2;
            // 
            // ItemsTabPage
            // 
            this.ItemsTabPage.Controls.Add(this.ItemsListBox);
            this.ItemsTabPage.Location = new System.Drawing.Point(4, 22);
            this.ItemsTabPage.Name = "ItemsTabPage";
            this.ItemsTabPage.Size = new System.Drawing.Size(307, 359);
            this.ItemsTabPage.TabIndex = 4;
            this.ItemsTabPage.Text = "Items";
            this.ItemsTabPage.UseVisualStyleBackColor = true;
            // 
            // ItemsListBox
            // 
            this.ItemsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemsListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ItemsListBox.FormattingEnabled = true;
            this.ItemsListBox.Location = new System.Drawing.Point(0, 0);
            this.ItemsListBox.Name = "ItemsListBox";
            this.ItemsListBox.Size = new System.Drawing.Size(307, 359);
            this.ItemsListBox.TabIndex = 2;
            // 
            // InterfacesTabPage
            // 
            this.InterfacesTabPage.Controls.Add(this.InterfacesListBox);
            this.InterfacesTabPage.Location = new System.Drawing.Point(4, 22);
            this.InterfacesTabPage.Name = "InterfacesTabPage";
            this.InterfacesTabPage.Size = new System.Drawing.Size(307, 359);
            this.InterfacesTabPage.TabIndex = 9;
            this.InterfacesTabPage.Text = "Interfaces";
            this.InterfacesTabPage.UseVisualStyleBackColor = true;
            // 
            // InterfacesListBox
            // 
            this.InterfacesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InterfacesListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InterfacesListBox.FormattingEnabled = true;
            this.InterfacesListBox.Location = new System.Drawing.Point(0, 0);
            this.InterfacesListBox.Name = "InterfacesListBox";
            this.InterfacesListBox.Size = new System.Drawing.Size(307, 359);
            this.InterfacesListBox.TabIndex = 3;
            // 
            // LogTextBox
            // 
            this.LogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LogTextBox.Location = new System.Drawing.Point(0, 0);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTextBox.Size = new System.Drawing.Size(477, 446);
            this.LogTextBox.TabIndex = 0;
            // 
            // LogTimer
            // 
            this.LogTimer.Enabled = true;
            this.LogTimer.Tick += new System.EventHandler(this.LogTimer_Tick);
            // 
            // CompilerWorker
            // 
            this.CompilerWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CompilerWorker_DoWork);
            this.CompilerWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.CompilerWorker_RunWorkerCompleted);
            // 
            // LoaderWorker
            // 
            this.LoaderWorker.WorkerReportsProgress = true;
            this.LoaderWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoaderWorker_DoWork);
            this.LoaderWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.LoaderWorker_ProgressChanged);
            this.LoaderWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.LoaderWorker_RunWorkerCompleted);
            // 
            // CompilerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainSplitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CompilerForm";
            this.Text = "CompilerForm";
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            this.MainSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.ResourcesTabControl.ResumeLayout(false);
            this.TilesTabPage.ResumeLayout(false);
            this.SpritesTabPage.ResumeLayout(false);
            this.EventsTabPage.ResumeLayout(false);
            this.EntitiesTabPage.ResumeLayout(false);
            this.OutfitsTabPage.ResumeLayout(false);
            this.ItemsTabPage.ResumeLayout(false);
            this.InterfacesTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.Timer LogTimer;
        private System.Windows.Forms.TabControl ResourcesTabControl;
        private System.Windows.Forms.Button CompileButton;
        private System.Windows.Forms.TabPage EntitiesTabPage;
        private System.Windows.Forms.TabPage OutfitsTabPage;
        private System.Windows.Forms.TabPage ItemsTabPage;
        private System.Windows.Forms.TabPage TilesTabPage;
        private System.Windows.Forms.ListBox TilesListBox;
        private System.Windows.Forms.TabPage EventsTabPage;
        private System.Windows.Forms.ListBox EventsListBox;
        private System.Windows.Forms.ListBox EntitiesListBox;
        private System.Windows.Forms.ListBox OutfitsListBox;
        private System.Windows.Forms.ListBox ItemsListBox;
        private System.ComponentModel.BackgroundWorker CompilerWorker;
        private System.Windows.Forms.TabPage SpritesTabPage;
        private System.Windows.Forms.ListBox SpritesListBox;
        private System.ComponentModel.BackgroundWorker LoaderWorker;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.ProgressBar CompilerProgressBar;
        private System.Windows.Forms.TabPage InterfacesTabPage;
        private System.Windows.Forms.ListBox InterfacesListBox;
    }
}