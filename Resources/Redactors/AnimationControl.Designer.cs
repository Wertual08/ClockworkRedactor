namespace Resource_Redactor.Resources.Redactors
{
    partial class AnimationControl
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
            this.ControlsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.LocalFPURScaleNumeric = new ExtraForms.SafeNumericUpDown();
            this.FPURNumeric = new ExtraForms.SafeNumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.NodesListBox = new System.Windows.Forms.CheckedListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.LinkTextBox = new Resource_Redactor.Resources.Redactors.SubresourceTextBox();
            this.OLICheckBox = new ExtraForms.SafeCheckBox();
            this.ALICheckBox = new ExtraForms.SafeCheckBox();
            this.TypeUpDown = new System.Windows.Forms.DomainUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.AngleNumeric = new ExtraForms.SafeNumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OffsetYNumeric = new ExtraForms.SafeNumericUpDown();
            this.OffsetXNumeric = new ExtraForms.SafeNumericUpDown();
            this.FramesListBox = new System.Windows.Forms.ListBox();
            this.GLSurface = new ExtraForms.OpenGLSurface();
            this.GLFrameTimer = new System.Windows.Forms.Timer(this.components);
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).BeginInit();
            this.ControlsSplitContainer.Panel1.SuspendLayout();
            this.ControlsSplitContainer.Panel2.SuspendLayout();
            this.ControlsSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LocalFPURScaleNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPURNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetYNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetXNumeric)).BeginInit();
            this.SuspendLayout();
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
            this.ControlsSplitContainer.Panel1.Controls.Add(this.LocalFPURScaleNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.FPURNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label4);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.NodesListBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label6);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.LinkTextBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.OLICheckBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.ALICheckBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.TypeUpDown);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label1);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.AngleNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label3);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label2);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.OffsetYNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.OffsetXNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.FramesListBox);
            this.ControlsSplitContainer.Panel1MinSize = 235;
            // 
            // ControlsSplitContainer.Panel2
            // 
            this.ControlsSplitContainer.Panel2.Controls.Add(this.GLSurface);
            this.ControlsSplitContainer.Size = new System.Drawing.Size(705, 420);
            this.ControlsSplitContainer.SplitterDistance = 235;
            this.ControlsSplitContainer.TabIndex = 0;
            // 
            // LocalFPURScaleNumeric
            // 
            this.LocalFPURScaleNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LocalFPURScaleNumeric.DecimalPlaces = 6;
            this.LocalFPURScaleNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.LocalFPURScaleNumeric.Location = new System.Drawing.Point(3, 393);
            this.LocalFPURScaleNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.LocalFPURScaleNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.LocalFPURScaleNumeric.Name = "LocalFPURScaleNumeric";
            this.LocalFPURScaleNumeric.Size = new System.Drawing.Size(103, 20);
            this.LocalFPURScaleNumeric.TabIndex = 30;
            this.LocalFPURScaleNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // FPURNumeric
            // 
            this.FPURNumeric.DecimalPlaces = 6;
            this.FPURNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.FPURNumeric.Location = new System.Drawing.Point(3, 19);
            this.FPURNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.FPURNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.FPURNumeric.Name = "FPURNumeric";
            this.FPURNumeric.Size = new System.Drawing.Size(103, 20);
            this.FPURNumeric.TabIndex = 29;
            this.FPURNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.FPURNumeric.ValueChanged += new System.EventHandler(this.AnimationProperty_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Frames per unit ratio";
            // 
            // NodesListBox
            // 
            this.NodesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NodesListBox.FormattingEnabled = true;
            this.NodesListBox.Location = new System.Drawing.Point(112, 71);
            this.NodesListBox.Name = "NodesListBox";
            this.NodesListBox.Size = new System.Drawing.Size(116, 169);
            this.NodesListBox.TabIndex = 2;
            this.NodesListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.NodesListBox_ItemCheck);
            this.NodesListBox.SelectedIndexChanged += new System.EventHandler(this.NodesListBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(109, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Ragdoll link";
            // 
            // LinkTextBox
            // 
            this.LinkTextBox.AllowDrop = true;
            this.LinkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LinkTextBox.BackColor = System.Drawing.Color.Red;
            this.LinkTextBox.Location = new System.Drawing.Point(112, 19);
            this.LinkTextBox.Name = "LinkTextBox";
            this.LinkTextBox.Size = new System.Drawing.Size(116, 20);
            this.LinkTextBox.TabIndex = 2;
            this.LinkTextBox.TextChanged += new System.EventHandler(this.LinkTextBox_TextChanged);
            // 
            // OLICheckBox
            // 
            this.OLICheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OLICheckBox.AutoSize = true;
            this.OLICheckBox.Location = new System.Drawing.Point(112, 279);
            this.OLICheckBox.Name = "OLICheckBox";
            this.OLICheckBox.Size = new System.Drawing.Size(114, 17);
            this.OLICheckBox.TabIndex = 24;
            this.OLICheckBox.Text = "Offset linear interp.";
            this.OLICheckBox.UseVisualStyleBackColor = true;
            this.OLICheckBox.CheckedChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // ALICheckBox
            // 
            this.ALICheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ALICheckBox.AutoSize = true;
            this.ALICheckBox.Location = new System.Drawing.Point(112, 256);
            this.ALICheckBox.Name = "ALICheckBox";
            this.ALICheckBox.Size = new System.Drawing.Size(113, 17);
            this.ALICheckBox.TabIndex = 2;
            this.ALICheckBox.Text = "Angle linear interp.";
            this.ALICheckBox.UseVisualStyleBackColor = true;
            this.ALICheckBox.CheckedChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // TypeUpDown
            // 
            this.TypeUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypeUpDown.Location = new System.Drawing.Point(112, 45);
            this.TypeUpDown.Name = "TypeUpDown";
            this.TypeUpDown.ReadOnly = true;
            this.TypeUpDown.Size = new System.Drawing.Size(116, 20);
            this.TypeUpDown.TabIndex = 2;
            this.TypeUpDown.Text = "ThisIsShit";
            this.TypeUpDown.SelectedItemChanged += new System.EventHandler(this.AnimationProperty_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 299);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Angle";
            // 
            // AngleNumeric
            // 
            this.AngleNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AngleNumeric.DecimalPlaces = 6;
            this.AngleNumeric.Enabled = false;
            this.AngleNumeric.Increment = new decimal(new int[] {
            17453,
            0,
            0,
            393216});
            this.AngleNumeric.Location = new System.Drawing.Point(112, 315);
            this.AngleNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.AngleNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.AngleNumeric.Name = "AngleNumeric";
            this.AngleNumeric.Size = new System.Drawing.Size(116, 20);
            this.AngleNumeric.TabIndex = 22;
            this.AngleNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.AngleNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(109, 377);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Offset Y";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 338);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Offset X";
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
            this.OffsetYNumeric.Location = new System.Drawing.Point(112, 393);
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
            this.OffsetYNumeric.TabIndex = 19;
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
            this.OffsetXNumeric.Location = new System.Drawing.Point(112, 354);
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
            this.OffsetXNumeric.TabIndex = 18;
            this.OffsetXNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.OffsetXNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // FramesListBox
            // 
            this.FramesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.FramesListBox.FormattingEnabled = true;
            this.FramesListBox.Location = new System.Drawing.Point(3, 45);
            this.FramesListBox.Name = "FramesListBox";
            this.FramesListBox.Size = new System.Drawing.Size(103, 342);
            this.FramesListBox.TabIndex = 0;
            this.FramesListBox.SelectedIndexChanged += new System.EventHandler(this.FramesListBox_SelectedIndexChanged);
            this.FramesListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FramesListBox_KeyDown);
            // 
            // GLSurface
            // 
            this.GLSurface.BackColor = System.Drawing.Color.Black;
            this.GLSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GLSurface.Location = new System.Drawing.Point(0, 0);
            this.GLSurface.Name = "GLSurface";
            this.GLSurface.NoClear = false;
            this.GLSurface.Size = new System.Drawing.Size(462, 416);
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
            // GLFrameTimer
            // 
            this.GLFrameTimer.Interval = 1;
            this.GLFrameTimer.Tick += new System.EventHandler(this.GLFrameTimer_Tick);
            // 
            // AnimationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ControlsSplitContainer);
            this.Name = "AnimationControl";
            this.Size = new System.Drawing.Size(705, 420);
            this.ControlsSplitContainer.Panel1.ResumeLayout(false);
            this.ControlsSplitContainer.Panel1.PerformLayout();
            this.ControlsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).EndInit();
            this.ControlsSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LocalFPURScaleNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPURNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetYNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetXNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer ControlsSplitContainer;
        private ExtraForms.OpenGLSurface GLSurface;
        private System.Windows.Forms.Timer GLFrameTimer;
        private System.Windows.Forms.ListBox FramesListBox;
        private System.Windows.Forms.Label label6;
        private Resource_Redactor.Resources.Redactors.SubresourceTextBox LinkTextBox;
        private ExtraForms.SafeCheckBox OLICheckBox;
        private ExtraForms.SafeCheckBox ALICheckBox;
        private System.Windows.Forms.DomainUpDown TypeUpDown;
        private System.Windows.Forms.Label label1;
        private ExtraForms.SafeNumericUpDown AngleNumeric;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private ExtraForms.SafeNumericUpDown OffsetYNumeric;
        private ExtraForms.SafeNumericUpDown OffsetXNumeric;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private System.Windows.Forms.CheckedListBox NodesListBox;
        private ExtraForms.SafeNumericUpDown FPURNumeric;
        private System.Windows.Forms.Label label4;
        private ExtraForms.SafeNumericUpDown LocalFPURScaleNumeric;
    }
}
