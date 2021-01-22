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
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.LogTimer = new System.Windows.Forms.Timer(this.components);
            this.CompilerWorker = new System.ComponentModel.BackgroundWorker();
            this.LoaderWorker = new System.ComponentModel.BackgroundWorker();
            this.ResourcesListBox = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
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
            this.MainSplitContainer.Panel1.Controls.Add(this.ResourcesListBox);
            this.MainSplitContainer.Panel1.Controls.Add(this.CompilerProgressBar);
            this.MainSplitContainer.Panel1.Controls.Add(this.ExitButton);
            this.MainSplitContainer.Panel1.Controls.Add(this.CompileButton);
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
            // ResourcesListBox
            // 
            this.ResourcesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourcesListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ResourcesListBox.FormattingEnabled = true;
            this.ResourcesListBox.Location = new System.Drawing.Point(3, 3);
            this.ResourcesListBox.Name = "ResourcesListBox";
            this.ResourcesListBox.Size = new System.Drawing.Size(305, 381);
            this.ResourcesListBox.TabIndex = 6;
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.Timer LogTimer;
        private System.Windows.Forms.Button CompileButton;
        private System.ComponentModel.BackgroundWorker CompilerWorker;
        private System.ComponentModel.BackgroundWorker LoaderWorker;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.ProgressBar CompilerProgressBar;
        private System.Windows.Forms.ListBox ResourcesListBox;
    }
}