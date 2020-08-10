namespace Resource_Redactor
{
    partial class ExplorerForm
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
            this.ControlsPanel = new System.Windows.Forms.Panel();
            this.RejectButton = new System.Windows.Forms.Button();
            this.SelectButton = new System.Windows.Forms.Button();
            this.ResourceExplorer = new Resource_Redactor.Resources.Redactors.ExplorerControl();
            this.ControlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlsPanel
            // 
            this.ControlsPanel.Controls.Add(this.RejectButton);
            this.ControlsPanel.Controls.Add(this.SelectButton);
            this.ControlsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ControlsPanel.Location = new System.Drawing.Point(0, 421);
            this.ControlsPanel.Name = "ControlsPanel";
            this.ControlsPanel.Size = new System.Drawing.Size(800, 29);
            this.ControlsPanel.TabIndex = 0;
            // 
            // RejectButton
            // 
            this.RejectButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RejectButton.Location = new System.Drawing.Point(641, 3);
            this.RejectButton.Name = "RejectButton";
            this.RejectButton.Size = new System.Drawing.Size(75, 23);
            this.RejectButton.TabIndex = 1;
            this.RejectButton.Text = "Cancel";
            this.RejectButton.UseVisualStyleBackColor = true;
            this.RejectButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SelectButton
            // 
            this.SelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectButton.Location = new System.Drawing.Point(722, 3);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(75, 23);
            this.SelectButton.TabIndex = 0;
            this.SelectButton.Text = "Select";
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // ResourceExplorer
            // 
            this.ResourceExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResourceExplorer.Location = new System.Drawing.Point(0, 0);
            this.ResourceExplorer.MultiSelect = true;
            this.ResourceExplorer.Name = "ResourceExplorer";
            this.ResourceExplorer.Size = new System.Drawing.Size(800, 421);
            this.ResourceExplorer.TabIndex = 1;
            this.ResourceExplorer.ItemLoaded += new Resource_Redactor.Resources.Redactors.ExplorerControl.ItemLoadedEventHandler(this.ResourceExplorer_ItemLoaded);
            // 
            // ExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ResourceExplorer);
            this.Controls.Add(this.ControlsPanel);
            this.Name = "ExplorerForm";
            this.Text = "ExplorerForm";
            this.ControlsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ControlsPanel;
        private Resources.Redactors.ExplorerControl ResourceExplorer;
        private System.Windows.Forms.Button RejectButton;
        private System.Windows.Forms.Button SelectButton;
    }
}