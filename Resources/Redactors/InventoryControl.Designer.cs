namespace Resource_Redactor.Resources.Redactors
{
    partial class InventoryControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ControlsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.PropertiesTabPage = new System.Windows.Forms.TabPage();
            this.ElementPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.ElementsTabPage = new System.Windows.Forms.TabPage();
            this.ElementsListBox = new System.Windows.Forms.ListBox();
            this.GLSurface = new ExtraForms.OpenGLSurface();
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).BeginInit();
            this.ControlsSplitContainer.Panel1.SuspendLayout();
            this.ControlsSplitContainer.Panel2.SuspendLayout();
            this.ControlsSplitContainer.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.PropertiesTabPage.SuspendLayout();
            this.ElementsTabPage.SuspendLayout();
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
            this.ControlsSplitContainer.Panel1.Controls.Add(this.tabControl1);
            // 
            // ControlsSplitContainer.Panel2
            // 
            this.ControlsSplitContainer.Panel2.Controls.Add(this.GLSurface);
            this.ControlsSplitContainer.Size = new System.Drawing.Size(768, 469);
            this.ControlsSplitContainer.SplitterDistance = 281;
            this.ControlsSplitContainer.TabIndex = 17;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.PropertiesTabPage);
            this.tabControl1.Controls.Add(this.ElementsTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(277, 465);
            this.tabControl1.TabIndex = 1;
            // 
            // PropertiesTabPage
            // 
            this.PropertiesTabPage.Controls.Add(this.ElementPropertyGrid);
            this.PropertiesTabPage.Location = new System.Drawing.Point(4, 22);
            this.PropertiesTabPage.Name = "PropertiesTabPage";
            this.PropertiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PropertiesTabPage.Size = new System.Drawing.Size(269, 439);
            this.PropertiesTabPage.TabIndex = 0;
            this.PropertiesTabPage.Text = "Properties";
            this.PropertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // ElementPropertyGrid
            // 
            this.ElementPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ElementPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.ElementPropertyGrid.Name = "ElementPropertyGrid";
            this.ElementPropertyGrid.Size = new System.Drawing.Size(263, 433);
            this.ElementPropertyGrid.TabIndex = 1;
            // 
            // ElementsTabPage
            // 
            this.ElementsTabPage.Controls.Add(this.ElementsListBox);
            this.ElementsTabPage.Location = new System.Drawing.Point(4, 22);
            this.ElementsTabPage.Name = "ElementsTabPage";
            this.ElementsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ElementsTabPage.Size = new System.Drawing.Size(269, 439);
            this.ElementsTabPage.TabIndex = 1;
            this.ElementsTabPage.Text = "Elements";
            this.ElementsTabPage.UseVisualStyleBackColor = true;
            // 
            // ElementsListBox
            // 
            this.ElementsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ElementsListBox.FormattingEnabled = true;
            this.ElementsListBox.Location = new System.Drawing.Point(3, 3);
            this.ElementsListBox.Name = "ElementsListBox";
            this.ElementsListBox.Size = new System.Drawing.Size(263, 433);
            this.ElementsListBox.TabIndex = 1;
            this.ElementsListBox.SelectedIndexChanged += new System.EventHandler(this.ElementsListBox_SelectedIndexChanged);
            // 
            // GLSurface
            // 
            this.GLSurface.BackColor = System.Drawing.Color.Black;
            this.GLSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GLSurface.Location = new System.Drawing.Point(0, 0);
            this.GLSurface.Name = "GLSurface";
            this.GLSurface.NoClear = false;
            this.GLSurface.Size = new System.Drawing.Size(479, 465);
            this.GLSurface.TabIndex = 0;
            this.GLSurface.Text = "GLSurface";
            this.GLSurface.Zoom = 1F;
            this.GLSurface.GLPaint += new System.EventHandler(this.GLSurface_GLPaint);
            this.GLSurface.GLSizeChanged += new System.EventHandler(this.GLSurface_GLSizeChanged);
            this.GLSurface.GLMouseWheel += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseWheel);
            this.GLSurface.GLMouseDown += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseDown);
            this.GLSurface.GLMouseMove += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseMove);
            this.GLSurface.GLMouseUp += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseUp);
            // 
            // InventoryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ControlsSplitContainer);
            this.Name = "InventoryControl";
            this.Size = new System.Drawing.Size(768, 469);
            this.ControlsSplitContainer.Panel1.ResumeLayout(false);
            this.ControlsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).EndInit();
            this.ControlsSplitContainer.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.PropertiesTabPage.ResumeLayout(false);
            this.ElementsTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer ControlsSplitContainer;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private ExtraForms.OpenGLSurface GLSurface;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage PropertiesTabPage;
        private System.Windows.Forms.PropertyGrid ElementPropertyGrid;
        private System.Windows.Forms.TabPage ElementsTabPage;
        private System.Windows.Forms.ListBox ElementsListBox;
    }
}
