namespace Resource_Redactor.Descriptions.Redactors
{
    partial class OutfitControl
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
            this.GLFrameTimer = new System.Windows.Forms.Timer(this.components);
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.ControlsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.ClotheTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RagdollNodeNumeric = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.NodesListBox = new System.Windows.Forms.ListBox();
            this.GLSurface = new ExtraForms.OpenGLSurface();
            this.SpriteLinkTextBox = new Resource_Redactor.Descriptions.Redactors.SubresourceTextBox();
            this.RagdollLinkTextBox = new Resource_Redactor.Descriptions.Redactors.SubresourceTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).BeginInit();
            this.ControlsSplitContainer.Panel1.SuspendLayout();
            this.ControlsSplitContainer.Panel2.SuspendLayout();
            this.ControlsSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RagdollNodeNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // GLFrameTimer
            // 
            this.GLFrameTimer.Interval = 1;
            this.GLFrameTimer.Tick += new System.EventHandler(this.GLFrameTimer_Tick);
            // 
            // ControlsSplitContainer
            // 
            this.ControlsSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ControlsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlsSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.ControlsSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.ControlsSplitContainer.Name = "ControlsSplitContainer";
            // 
            // ControlsSplitContainer.Panel1
            // 
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label3);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.ClotheTypeComboBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label2);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.RagdollNodeNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label1);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.SpriteLinkTextBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label6);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.RagdollLinkTextBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.NodesListBox);
            this.ControlsSplitContainer.Panel1MinSize = 200;
            // 
            // ControlsSplitContainer.Panel2
            // 
            this.ControlsSplitContainer.Panel2.Controls.Add(this.GLSurface);
            this.ControlsSplitContainer.Size = new System.Drawing.Size(728, 487);
            this.ControlsSplitContainer.SplitterDistance = 200;
            this.ControlsSplitContainer.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 443);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Clothe type";
            // 
            // ClotheTypeComboBox
            // 
            this.ClotheTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClotheTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ClotheTypeComboBox.Enabled = false;
            this.ClotheTypeComboBox.FormattingEnabled = true;
            this.ClotheTypeComboBox.Location = new System.Drawing.Point(3, 459);
            this.ClotheTypeComboBox.Name = "ClotheTypeComboBox";
            this.ClotheTypeComboBox.Size = new System.Drawing.Size(190, 21);
            this.ClotheTypeComboBox.TabIndex = 2;
            this.ClotheTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ClotheTypeComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 403);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Ragdoll node";
            // 
            // RagdollNodeNumeric
            // 
            this.RagdollNodeNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RagdollNodeNumeric.Enabled = false;
            this.RagdollNodeNumeric.Location = new System.Drawing.Point(3, 419);
            this.RagdollNodeNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.RagdollNodeNumeric.Name = "RagdollNodeNumeric";
            this.RagdollNodeNumeric.Size = new System.Drawing.Size(190, 20);
            this.RagdollNodeNumeric.TabIndex = 2;
            this.RagdollNodeNumeric.ValueChanged += new System.EventHandler(this.RagdollNodeNumeric_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 364);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Sprite link";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Ragdoll link";
            // 
            // NodesListBox
            // 
            this.NodesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NodesListBox.FormattingEnabled = true;
            this.NodesListBox.Location = new System.Drawing.Point(3, 45);
            this.NodesListBox.Name = "NodesListBox";
            this.NodesListBox.Size = new System.Drawing.Size(190, 316);
            this.NodesListBox.TabIndex = 0;
            this.NodesListBox.SelectedIndexChanged += new System.EventHandler(this.NodesListBox_SelectedIndexChanged);
            // 
            // GLSurface
            // 
            this.GLSurface.BackColor = System.Drawing.Color.Black;
            this.GLSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GLSurface.Location = new System.Drawing.Point(0, 0);
            this.GLSurface.Name = "GLSurface";
            this.GLSurface.NoClear = false;
            this.GLSurface.Size = new System.Drawing.Size(520, 483);
            this.GLSurface.TabIndex = 1;
            this.GLSurface.Text = "OpenGLSurface1";
            this.GLSurface.Zoom = 16F;
            this.GLSurface.GLStart += new System.EventHandler(this.GLSurface_GLStart);
            this.GLSurface.GLPaint += new System.EventHandler(this.GLSurface_GLPaint);
            this.GLSurface.GLStop += new System.EventHandler(this.GLSurface_GLStop);
            this.GLSurface.GLSizeChanged += new System.EventHandler(this.GLSurface_GLSizeChanged);
            this.GLSurface.GLMouseWheel += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseWheel);
            this.GLSurface.GLMouseDown += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseDown);
            this.GLSurface.GLMouseMove += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseMove);
            this.GLSurface.GLMouseUp += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseUp);
            // 
            // SpriteLinkTextBox
            // 
            this.SpriteLinkTextBox.AllowDrop = true;
            this.SpriteLinkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpriteLinkTextBox.BackColor = System.Drawing.Color.Red;
            this.SpriteLinkTextBox.Enabled = false;
            this.SpriteLinkTextBox.Location = new System.Drawing.Point(3, 380);
            this.SpriteLinkTextBox.Name = "SpriteLinkTextBox";
            this.SpriteLinkTextBox.Size = new System.Drawing.Size(190, 20);
            this.SpriteLinkTextBox.Subresource = null;
            this.SpriteLinkTextBox.TabIndex = 28;
            this.SpriteLinkTextBox.TextChanged += new System.EventHandler(this.SpriteLinkTextBox_TextChanged);
            // 
            // RagdollLinkTextBox
            // 
            this.RagdollLinkTextBox.AllowDrop = true;
            this.RagdollLinkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RagdollLinkTextBox.BackColor = System.Drawing.Color.Red;
            this.RagdollLinkTextBox.Location = new System.Drawing.Point(3, 19);
            this.RagdollLinkTextBox.Name = "RagdollLinkTextBox";
            this.RagdollLinkTextBox.Size = new System.Drawing.Size(190, 20);
            this.RagdollLinkTextBox.Subresource = null;
            this.RagdollLinkTextBox.TabIndex = 2;
            this.RagdollLinkTextBox.TextChanged += new System.EventHandler(this.RagdollLinkTextBox_TextChanged);
            // 
            // OutfitControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ControlsSplitContainer);
            this.Name = "OutfitControl";
            this.Size = new System.Drawing.Size(728, 487);
            this.ControlsSplitContainer.Panel1.ResumeLayout(false);
            this.ControlsSplitContainer.Panel1.PerformLayout();
            this.ControlsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).EndInit();
            this.ControlsSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RagdollNodeNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer GLFrameTimer;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private System.Windows.Forms.SplitContainer ControlsSplitContainer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ClotheTypeComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown RagdollNodeNumeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox NodesListBox;
        private ExtraForms.OpenGLSurface GLSurface;
        private Resource_Redactor.Descriptions.Redactors.SubresourceTextBox SpriteLinkTextBox;
        private Resource_Redactor.Descriptions.Redactors.SubresourceTextBox RagdollLinkTextBox;
    }
}
