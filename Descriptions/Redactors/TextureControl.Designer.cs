namespace Resource_Redactor.Descriptions.Redactors
{
    partial class TextureControl
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
            this.ImportFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.TextureBitmapBox = new ExtraForms.BitmapBox();
            this.ExportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // TextureBitmapBox
            // 
            this.TextureBitmapBox.AllowDrop = true;
            this.TextureBitmapBox.Bitmap = null;
            this.TextureBitmapBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TextureBitmapBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextureBitmapBox.Location = new System.Drawing.Point(0, 0);
            this.TextureBitmapBox.Name = "TextureBitmapBox";
            this.TextureBitmapBox.ShowProperties = true;
            this.TextureBitmapBox.Size = new System.Drawing.Size(548, 410);
            this.TextureBitmapBox.TabIndex = 0;
            this.TextureBitmapBox.ZoomLimit = 100F;
            this.TextureBitmapBox.ZoomStep = 1.1F;
            this.TextureBitmapBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextureBitmapBox_DragDrop);
            this.TextureBitmapBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.TextureBitmapBox_DragEnter);
            // 
            // TextureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TextureBitmapBox);
            this.Name = "TextureControl";
            this.Size = new System.Drawing.Size(548, 410);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtraForms.BitmapBox TextureBitmapBox;
        private System.Windows.Forms.OpenFileDialog ImportFileDialog;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private System.Windows.Forms.SaveFileDialog ExportFileDialog;
    }
}
