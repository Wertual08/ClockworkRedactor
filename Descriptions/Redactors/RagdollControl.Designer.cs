namespace Resource_Redactor.Descriptions.Redactors
{
    partial class RagdollControl
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.NodesListBox = new System.Windows.Forms.ListBox();
            this.ControlsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.HitboxHNumeric = new ExtraForms.SafeNumericUpDown();
            this.HitboxWNumeric = new ExtraForms.SafeNumericUpDown();
            this.LinksListBox = new System.Windows.Forms.CheckedListBox();
            this.MainNodeNumeric = new ExtraForms.SafeNumericUpDown();
            this.OffsetYNumeric = new ExtraForms.SafeNumericUpDown();
            this.OffsetXNumeric = new ExtraForms.SafeNumericUpDown();
            this.GLSurface = new ExtraForms.OpenGLSurface();
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.GLFrameTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).BeginInit();
            this.ControlsSplitContainer.Panel1.SuspendLayout();
            this.ControlsSplitContainer.Panel2.SuspendLayout();
            this.ControlsSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HitboxHNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HitboxWNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainNodeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetYNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetXNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 282);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Sprite Links";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Main Node";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Offset Y";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Offset X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nodes";
            // 
            // NodesListBox
            // 
            this.NodesListBox.AllowDrop = true;
            this.NodesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NodesListBox.FormattingEnabled = true;
            this.NodesListBox.Location = new System.Drawing.Point(3, 94);
            this.NodesListBox.Name = "NodesListBox";
            this.NodesListBox.Size = new System.Drawing.Size(116, 69);
            this.NodesListBox.TabIndex = 0;
            this.NodesListBox.SelectedIndexChanged += new System.EventHandler(this.NodesListBox_SelectedIndexChanged);
            this.NodesListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.NodesListBox_DragDrop);
            this.NodesListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.NodesListBox_DragEnter);
            this.NodesListBox.DragOver += new System.Windows.Forms.DragEventHandler(this.NodesListBox_DragOver);
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
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label6);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label7);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.HitboxHNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.HitboxWNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.LinksListBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label5);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label4);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.MainNodeNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label3);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label2);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.OffsetYNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.OffsetXNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label1);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.NodesListBox);
            // 
            // ControlsSplitContainer.Panel2
            // 
            this.ControlsSplitContainer.Panel2.Controls.Add(this.GLSurface);
            this.ControlsSplitContainer.Size = new System.Drawing.Size(495, 399);
            this.ControlsSplitContainer.SplitterDistance = 126;
            this.ControlsSplitContainer.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Hitbox heigth";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Hitbox width";
            // 
            // HitboxHNumeric
            // 
            this.HitboxHNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HitboxHNumeric.DecimalPlaces = 6;
            this.HitboxHNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.HitboxHNumeric.Location = new System.Drawing.Point(3, 55);
            this.HitboxHNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.HitboxHNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.HitboxHNumeric.Name = "HitboxHNumeric";
            this.HitboxHNumeric.Size = new System.Drawing.Size(116, 20);
            this.HitboxHNumeric.TabIndex = 23;
            this.HitboxHNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.HitboxHNumeric.ValueChanged += new System.EventHandler(this.HitboxNumeric_ValueChanged);
            // 
            // HitboxWNumeric
            // 
            this.HitboxWNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HitboxWNumeric.DecimalPlaces = 6;
            this.HitboxWNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.HitboxWNumeric.Location = new System.Drawing.Point(3, 16);
            this.HitboxWNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.HitboxWNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.HitboxWNumeric.Name = "HitboxWNumeric";
            this.HitboxWNumeric.Size = new System.Drawing.Size(116, 20);
            this.HitboxWNumeric.TabIndex = 22;
            this.HitboxWNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.HitboxWNumeric.ValueChanged += new System.EventHandler(this.HitboxNumeric_ValueChanged);
            // 
            // LinksListBox
            // 
            this.LinksListBox.AllowDrop = true;
            this.LinksListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LinksListBox.Enabled = false;
            this.LinksListBox.FormattingEnabled = true;
            this.LinksListBox.Location = new System.Drawing.Point(3, 298);
            this.LinksListBox.Name = "LinksListBox";
            this.LinksListBox.Size = new System.Drawing.Size(116, 94);
            this.LinksListBox.TabIndex = 1;
            this.LinksListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.LinksListBox_ItemCheck);
            this.LinksListBox.SelectedIndexChanged += new System.EventHandler(this.LinksListBox_SelectedIndexChanged);
            this.LinksListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.LinksListBox_DragDrop);
            this.LinksListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.LinksListBox_DragEnter);
            // 
            // MainNodeNumeric
            // 
            this.MainNodeNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainNodeNumeric.Enabled = false;
            this.MainNodeNumeric.Location = new System.Drawing.Point(3, 259);
            this.MainNodeNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.MainNodeNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.MainNodeNumeric.Name = "MainNodeNumeric";
            this.MainNodeNumeric.Size = new System.Drawing.Size(116, 20);
            this.MainNodeNumeric.TabIndex = 18;
            this.MainNodeNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.MainNodeNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // OffsetYNumeric
            // 
            this.OffsetYNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OffsetYNumeric.DecimalPlaces = 6;
            this.OffsetYNumeric.Enabled = false;
            this.OffsetYNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.OffsetYNumeric.Location = new System.Drawing.Point(3, 220);
            this.OffsetYNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.OffsetYNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.OffsetYNumeric.Name = "OffsetYNumeric";
            this.OffsetYNumeric.Size = new System.Drawing.Size(116, 20);
            this.OffsetYNumeric.TabIndex = 15;
            this.OffsetYNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.OffsetYNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // OffsetXNumeric
            // 
            this.OffsetXNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OffsetXNumeric.DecimalPlaces = 6;
            this.OffsetXNumeric.Enabled = false;
            this.OffsetXNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.OffsetXNumeric.Location = new System.Drawing.Point(3, 181);
            this.OffsetXNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.OffsetXNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.OffsetXNumeric.Name = "OffsetXNumeric";
            this.OffsetXNumeric.Size = new System.Drawing.Size(116, 20);
            this.OffsetXNumeric.TabIndex = 14;
            this.OffsetXNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.OffsetXNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // GLSurface
            // 
            this.GLSurface.BackColor = System.Drawing.Color.Black;
            this.GLSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GLSurface.Location = new System.Drawing.Point(0, 0);
            this.GLSurface.Name = "GLSurface";
            this.GLSurface.NoClear = false;
            this.GLSurface.Size = new System.Drawing.Size(361, 395);
            this.GLSurface.TabIndex = 0;
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
            // GLFrameTimer
            // 
            this.GLFrameTimer.Interval = 1;
            this.GLFrameTimer.Tick += new System.EventHandler(this.GLFrameTimer_Tick);
            // 
            // RagdollControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ControlsSplitContainer);
            this.Name = "RagdollControl";
            this.Size = new System.Drawing.Size(495, 399);
            this.ControlsSplitContainer.Panel1.ResumeLayout(false);
            this.ControlsSplitContainer.Panel1.PerformLayout();
            this.ControlsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).EndInit();
            this.ControlsSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HitboxHNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HitboxWNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainNodeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetYNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetXNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox NodesListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private ExtraForms.SafeNumericUpDown OffsetYNumeric;
        private ExtraForms.SafeNumericUpDown OffsetXNumeric;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private ExtraForms.SafeNumericUpDown MainNodeNumeric;
        private ExtraForms.OpenGLSurface GLSurface;
        private System.Windows.Forms.SplitContainer ControlsSplitContainer;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private System.Windows.Forms.CheckedListBox LinksListBox;
        private System.Windows.Forms.Timer GLFrameTimer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private ExtraForms.SafeNumericUpDown HitboxHNumeric;
        private ExtraForms.SafeNumericUpDown HitboxWNumeric;
    }
}
