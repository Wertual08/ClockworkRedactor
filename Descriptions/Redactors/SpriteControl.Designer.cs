namespace Resource_Redactor.Descriptions.Redactors
{
    partial class SpriteControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LinkTextBox = new System.Windows.Forms.TextBox();
            this.ControlsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.VFramesCheckBox = new System.Windows.Forms.CheckBox();
            this.AngleNumeric = new ExtraForms.SafeNumericUpDown();
            this.AxisYNumeric = new ExtraForms.SafeNumericUpDown();
            this.AxisXNumeric = new ExtraForms.SafeNumericUpDown();
            this.ImgboxHNumeric = new ExtraForms.SafeNumericUpDown();
            this.ImgboxWNumeric = new ExtraForms.SafeNumericUpDown();
            this.FramesNumeric = new ExtraForms.SafeNumericUpDown();
            this.DelayNumeric = new ExtraForms.SafeNumericUpDown();
            this.GLSurface = new ExtraForms.OpenGLSurface();
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.GLFrameTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).BeginInit();
            this.ControlsSplitContainer.Panel1.SuspendLayout();
            this.ControlsSplitContainer.Panel2.SuspendLayout();
            this.ControlsSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AngleNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AxisYNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AxisXNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgboxHNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgboxWNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FramesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Frames count";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Frame delay";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Image box width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Image box height";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Axis X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Axis Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 234);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Base angle";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 296);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Texture";
            // 
            // LinkTextBox
            // 
            this.LinkTextBox.AllowDrop = true;
            this.LinkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LinkTextBox.BackColor = System.Drawing.Color.Red;
            this.LinkTextBox.Location = new System.Drawing.Point(3, 312);
            this.LinkTextBox.Name = "LinkTextBox";
            this.LinkTextBox.Size = new System.Drawing.Size(116, 20);
            this.LinkTextBox.TabIndex = 16;
            this.LinkTextBox.TextChanged += new System.EventHandler(this.LinkTextBox_TextChanged);
            this.LinkTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.LinkTextBox_DragDrop);
            this.LinkTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.LinkTextBox_DragEnter);
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
            this.ControlsSplitContainer.Panel1.Controls.Add(this.VFramesCheckBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label1);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.LinkTextBox);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label2);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label8);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label3);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.AngleNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label4);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.AxisYNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label7);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.AxisXNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label6);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.ImgboxHNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.label5);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.ImgboxWNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.FramesNumeric);
            this.ControlsSplitContainer.Panel1.Controls.Add(this.DelayNumeric);
            // 
            // ControlsSplitContainer.Panel2
            // 
            this.ControlsSplitContainer.Panel2.Controls.Add(this.GLSurface);
            this.ControlsSplitContainer.Size = new System.Drawing.Size(512, 339);
            this.ControlsSplitContainer.SplitterDistance = 126;
            this.ControlsSplitContainer.TabIndex = 17;
            // 
            // VFramesCheckBox
            // 
            this.VFramesCheckBox.AutoSize = true;
            this.VFramesCheckBox.Location = new System.Drawing.Point(3, 276);
            this.VFramesCheckBox.Name = "VFramesCheckBox";
            this.VFramesCheckBox.Size = new System.Drawing.Size(95, 17);
            this.VFramesCheckBox.TabIndex = 1;
            this.VFramesCheckBox.Text = "Vertical frames";
            this.VFramesCheckBox.UseVisualStyleBackColor = true;
            this.VFramesCheckBox.CheckedChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // AngleNumeric
            // 
            this.AngleNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AngleNumeric.DecimalPlaces = 6;
            this.AngleNumeric.Increment = new decimal(new int[] {
            17453,
            0,
            0,
            393216});
            this.AngleNumeric.Location = new System.Drawing.Point(3, 250);
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
            this.AngleNumeric.TabIndex = 14;
            this.AngleNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.AngleNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // AxisYNumeric
            // 
            this.AxisYNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AxisYNumeric.DecimalPlaces = 6;
            this.AxisYNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.AxisYNumeric.Location = new System.Drawing.Point(3, 211);
            this.AxisYNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.AxisYNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.AxisYNumeric.Name = "AxisYNumeric";
            this.AxisYNumeric.Size = new System.Drawing.Size(116, 20);
            this.AxisYNumeric.TabIndex = 13;
            this.AxisYNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.AxisYNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // AxisXNumeric
            // 
            this.AxisXNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AxisXNumeric.DecimalPlaces = 6;
            this.AxisXNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.AxisXNumeric.Location = new System.Drawing.Point(3, 172);
            this.AxisXNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.AxisXNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.AxisXNumeric.Name = "AxisXNumeric";
            this.AxisXNumeric.Size = new System.Drawing.Size(116, 20);
            this.AxisXNumeric.TabIndex = 12;
            this.AxisXNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.AxisXNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // ImgboxHNumeric
            // 
            this.ImgboxHNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImgboxHNumeric.DecimalPlaces = 6;
            this.ImgboxHNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.ImgboxHNumeric.Location = new System.Drawing.Point(3, 133);
            this.ImgboxHNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ImgboxHNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ImgboxHNumeric.Name = "ImgboxHNumeric";
            this.ImgboxHNumeric.Size = new System.Drawing.Size(116, 20);
            this.ImgboxHNumeric.TabIndex = 11;
            this.ImgboxHNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ImgboxHNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // ImgboxWNumeric
            // 
            this.ImgboxWNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImgboxWNumeric.DecimalPlaces = 6;
            this.ImgboxWNumeric.Increment = new decimal(new int[] {
            15625,
            0,
            0,
            393216});
            this.ImgboxWNumeric.Location = new System.Drawing.Point(3, 94);
            this.ImgboxWNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ImgboxWNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ImgboxWNumeric.Name = "ImgboxWNumeric";
            this.ImgboxWNumeric.Size = new System.Drawing.Size(116, 20);
            this.ImgboxWNumeric.TabIndex = 10;
            this.ImgboxWNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ImgboxWNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // FramesNumeric
            // 
            this.FramesNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FramesNumeric.Location = new System.Drawing.Point(3, 16);
            this.FramesNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.FramesNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FramesNumeric.Name = "FramesNumeric";
            this.FramesNumeric.Size = new System.Drawing.Size(116, 20);
            this.FramesNumeric.TabIndex = 8;
            this.FramesNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FramesNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // DelayNumeric
            // 
            this.DelayNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DelayNumeric.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.DelayNumeric.Location = new System.Drawing.Point(2, 55);
            this.DelayNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.DelayNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.DelayNumeric.Name = "DelayNumeric";
            this.DelayNumeric.Size = new System.Drawing.Size(116, 20);
            this.DelayNumeric.TabIndex = 9;
            this.DelayNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.DelayNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            // 
            // GLSurface
            // 
            this.GLSurface.BackColor = System.Drawing.Color.Black;
            this.GLSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GLSurface.Location = new System.Drawing.Point(0, 0);
            this.GLSurface.Name = "GLSurface";
            this.GLSurface.NoClear = false;
            this.GLSurface.Size = new System.Drawing.Size(378, 335);
            this.GLSurface.TabIndex = 0;
            this.GLSurface.Text = "GLSurface";
            this.GLSurface.Zoom = 64F;
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
            // SpriteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ControlsSplitContainer);
            this.Name = "SpriteControl";
            this.Size = new System.Drawing.Size(512, 339);
            this.ControlsSplitContainer.Panel1.ResumeLayout(false);
            this.ControlsSplitContainer.Panel1.PerformLayout();
            this.ControlsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).EndInit();
            this.ControlsSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AngleNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AxisYNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AxisXNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgboxHNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgboxWNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FramesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private ExtraForms.SafeNumericUpDown FramesNumeric;
        private ExtraForms.SafeNumericUpDown DelayNumeric;
        private ExtraForms.SafeNumericUpDown ImgboxWNumeric;
        private ExtraForms.SafeNumericUpDown ImgboxHNumeric;
        private ExtraForms.SafeNumericUpDown AxisXNumeric;
        private ExtraForms.SafeNumericUpDown AxisYNumeric;
        private ExtraForms.SafeNumericUpDown AngleNumeric;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox LinkTextBox;
        private System.Windows.Forms.SplitContainer ControlsSplitContainer;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private ExtraForms.OpenGLSurface GLSurface;
        private System.Windows.Forms.CheckBox VFramesCheckBox;
        private System.Windows.Forms.Timer GLFrameTimer;
    }
}
