namespace Resource_Redactor.Resources.Redactors
{
    partial class EntityControl
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
            this.components = new System.ComponentModel.Container();
            this.GLFrameTimer = new System.Windows.Forms.Timer(this.components);
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.ControlsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ParamsTabControl = new System.Windows.Forms.TabControl();
            this.PropertiesTabPage = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.EntityParametersListBox = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.RagdollLinkTextBox = new Resource_Redactor.Resources.Redactors.SubresourceTextBox();
            this.EntityParameterNumeric = new System.Windows.Forms.NumericUpDown();
            this.AnimationsTabPage = new System.Windows.Forms.TabPage();
            this.AnimationParameterDomain = new System.Windows.Forms.DomainUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.AnimationNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AnimationsListBox = new System.Windows.Forms.ListBox();
            this.AnimationParameterNumeric = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AnimationActionTextBox = new System.Windows.Forms.TextBox();
            this.AnimationLinkTextBox = new Resource_Redactor.Resources.Redactors.SubresourceTextBox();
            this.AnimationParametersListBox = new System.Windows.Forms.CheckedListBox();
            this.HoldersTabPage = new System.Windows.Forms.TabPage();
            this.ToolDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.ToolCheckBox = new System.Windows.Forms.CheckBox();
            this.ToolLinkTextBox = new Resource_Redactor.Resources.Redactors.SubresourceTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.HolderNodeNumeric = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.HolderActionTextBox = new System.Windows.Forms.TextBox();
            this.HolderAnimationLinkTextBox = new Resource_Redactor.Resources.Redactors.SubresourceTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.HolderNameTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.HoldersListBox = new System.Windows.Forms.ListBox();
            this.GLSurface = new Resource_Redactor.Resources.Redactors.PreviewSurface();
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).BeginInit();
            this.ControlsSplitContainer.Panel1.SuspendLayout();
            this.ControlsSplitContainer.Panel2.SuspendLayout();
            this.ControlsSplitContainer.SuspendLayout();
            this.ParamsTabControl.SuspendLayout();
            this.PropertiesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EntityParameterNumeric)).BeginInit();
            this.AnimationsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationParameterNumeric)).BeginInit();
            this.HoldersTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ToolDelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HolderNodeNumeric)).BeginInit();
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
            this.ControlsSplitContainer.Panel1.Controls.Add(this.ParamsTabControl);
            this.ControlsSplitContainer.Panel1MinSize = 0;
            // 
            // ControlsSplitContainer.Panel2
            // 
            this.ControlsSplitContainer.Panel2.Controls.Add(this.GLSurface);
            this.ControlsSplitContainer.Size = new System.Drawing.Size(537, 423);
            this.ControlsSplitContainer.SplitterDistance = 292;
            this.ControlsSplitContainer.TabIndex = 1;
            // 
            // ParamsTabControl
            // 
            this.ParamsTabControl.Controls.Add(this.PropertiesTabPage);
            this.ParamsTabControl.Controls.Add(this.AnimationsTabPage);
            this.ParamsTabControl.Controls.Add(this.HoldersTabPage);
            this.ParamsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParamsTabControl.Location = new System.Drawing.Point(0, 0);
            this.ParamsTabControl.Name = "ParamsTabControl";
            this.ParamsTabControl.SelectedIndex = 0;
            this.ParamsTabControl.Size = new System.Drawing.Size(288, 419);
            this.ParamsTabControl.TabIndex = 2;
            this.ParamsTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.ParamsTabControl_Selected);
            // 
            // PropertiesTabPage
            // 
            this.PropertiesTabPage.Controls.Add(this.label11);
            this.PropertiesTabPage.Controls.Add(this.EntityParametersListBox);
            this.PropertiesTabPage.Controls.Add(this.label10);
            this.PropertiesTabPage.Controls.Add(this.RagdollLinkTextBox);
            this.PropertiesTabPage.Controls.Add(this.EntityParameterNumeric);
            this.PropertiesTabPage.Location = new System.Drawing.Point(4, 22);
            this.PropertiesTabPage.Name = "PropertiesTabPage";
            this.PropertiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PropertiesTabPage.Size = new System.Drawing.Size(280, 393);
            this.PropertiesTabPage.TabIndex = 0;
            this.PropertiesTabPage.Text = "Properties";
            this.PropertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Parameters";
            // 
            // EntityParametersListBox
            // 
            this.EntityParametersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EntityParametersListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EntityParametersListBox.FormattingEnabled = true;
            this.EntityParametersListBox.Location = new System.Drawing.Point(6, 58);
            this.EntityParametersListBox.Name = "EntityParametersListBox";
            this.EntityParametersListBox.Size = new System.Drawing.Size(268, 303);
            this.EntityParametersListBox.TabIndex = 3;
            this.EntityParametersListBox.SelectedIndexChanged += new System.EventHandler(this.EntityParametersListBox_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Ragdoll Link";
            // 
            // RagdollLinkTextBox
            // 
            this.RagdollLinkTextBox.AllowDrop = true;
            this.RagdollLinkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RagdollLinkTextBox.BackColor = System.Drawing.Color.Red;
            this.RagdollLinkTextBox.Location = new System.Drawing.Point(6, 19);
            this.RagdollLinkTextBox.Name = "RagdollLinkTextBox";
            this.RagdollLinkTextBox.Size = new System.Drawing.Size(268, 20);
            this.RagdollLinkTextBox.TabIndex = 2;
            this.RagdollLinkTextBox.TextChanged += new System.EventHandler(this.RagdollLinkTextBox_TextChanged);
            // 
            // EntityParameterNumeric
            // 
            this.EntityParameterNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EntityParameterNumeric.DecimalPlaces = 6;
            this.EntityParameterNumeric.Location = new System.Drawing.Point(6, 367);
            this.EntityParameterNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.EntityParameterNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.EntityParameterNumeric.Name = "EntityParameterNumeric";
            this.EntityParameterNumeric.Size = new System.Drawing.Size(268, 20);
            this.EntityParameterNumeric.TabIndex = 2;
            this.EntityParameterNumeric.ValueChanged += new System.EventHandler(this.EntityParameterNumeric_ValueChanged);
            // 
            // AnimationsTabPage
            // 
            this.AnimationsTabPage.Controls.Add(this.AnimationParameterDomain);
            this.AnimationsTabPage.Controls.Add(this.label4);
            this.AnimationsTabPage.Controls.Add(this.AnimationNameTextBox);
            this.AnimationsTabPage.Controls.Add(this.label3);
            this.AnimationsTabPage.Controls.Add(this.AnimationsListBox);
            this.AnimationsTabPage.Controls.Add(this.AnimationParameterNumeric);
            this.AnimationsTabPage.Controls.Add(this.label2);
            this.AnimationsTabPage.Controls.Add(this.label1);
            this.AnimationsTabPage.Controls.Add(this.AnimationActionTextBox);
            this.AnimationsTabPage.Controls.Add(this.AnimationLinkTextBox);
            this.AnimationsTabPage.Controls.Add(this.AnimationParametersListBox);
            this.AnimationsTabPage.Location = new System.Drawing.Point(4, 22);
            this.AnimationsTabPage.Name = "AnimationsTabPage";
            this.AnimationsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AnimationsTabPage.Size = new System.Drawing.Size(280, 393);
            this.AnimationsTabPage.TabIndex = 1;
            this.AnimationsTabPage.Text = "Animations";
            this.AnimationsTabPage.UseVisualStyleBackColor = true;
            // 
            // AnimationParameterDomain
            // 
            this.AnimationParameterDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimationParameterDomain.Enabled = false;
            this.AnimationParameterDomain.Location = new System.Drawing.Point(117, 328);
            this.AnimationParameterDomain.Name = "AnimationParameterDomain";
            this.AnimationParameterDomain.ReadOnly = true;
            this.AnimationParameterDomain.Size = new System.Drawing.Size(157, 20);
            this.AnimationParameterDomain.TabIndex = 2;
            this.AnimationParameterDomain.Text = "SosiPisos";
            this.AnimationParameterDomain.Visible = false;
            this.AnimationParameterDomain.SelectedItemChanged += new System.EventHandler(this.AnimationParameterNumeric_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 351);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Name";
            // 
            // AnimationNameTextBox
            // 
            this.AnimationNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AnimationNameTextBox.Enabled = false;
            this.AnimationNameTextBox.Location = new System.Drawing.Point(6, 367);
            this.AnimationNameTextBox.Name = "AnimationNameTextBox";
            this.AnimationNameTextBox.Size = new System.Drawing.Size(105, 20);
            this.AnimationNameTextBox.TabIndex = 2;
            this.AnimationNameTextBox.TextChanged += new System.EventHandler(this.AnimationNameTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Animations";
            // 
            // AnimationsListBox
            // 
            this.AnimationsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.AnimationsListBox.FormattingEnabled = true;
            this.AnimationsListBox.Location = new System.Drawing.Point(6, 19);
            this.AnimationsListBox.Name = "AnimationsListBox";
            this.AnimationsListBox.Size = new System.Drawing.Size(105, 329);
            this.AnimationsListBox.TabIndex = 2;
            this.AnimationsListBox.SelectedIndexChanged += new System.EventHandler(this.AnimationsListBox_SelectedIndexChanged);
            // 
            // AnimationParameterNumeric
            // 
            this.AnimationParameterNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimationParameterNumeric.DecimalPlaces = 6;
            this.AnimationParameterNumeric.Enabled = false;
            this.AnimationParameterNumeric.Location = new System.Drawing.Point(117, 328);
            this.AnimationParameterNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.AnimationParameterNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.AnimationParameterNumeric.Name = "AnimationParameterNumeric";
            this.AnimationParameterNumeric.Size = new System.Drawing.Size(157, 20);
            this.AnimationParameterNumeric.TabIndex = 5;
            this.AnimationParameterNumeric.Visible = false;
            this.AnimationParameterNumeric.ValueChanged += new System.EventHandler(this.AnimationParameterNumeric_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 351);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Animation Link";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Action";
            // 
            // AnimationActionTextBox
            // 
            this.AnimationActionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimationActionTextBox.Enabled = false;
            this.AnimationActionTextBox.Location = new System.Drawing.Point(117, 19);
            this.AnimationActionTextBox.Name = "AnimationActionTextBox";
            this.AnimationActionTextBox.Size = new System.Drawing.Size(157, 20);
            this.AnimationActionTextBox.TabIndex = 9;
            this.AnimationActionTextBox.TextChanged += new System.EventHandler(this.AnimationActionTextBox_TextChanged);
            // 
            // AnimationLinkTextBox
            // 
            this.AnimationLinkTextBox.AllowDrop = true;
            this.AnimationLinkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimationLinkTextBox.BackColor = System.Drawing.Color.Red;
            this.AnimationLinkTextBox.Enabled = false;
            this.AnimationLinkTextBox.Location = new System.Drawing.Point(117, 367);
            this.AnimationLinkTextBox.Name = "AnimationLinkTextBox";
            this.AnimationLinkTextBox.Size = new System.Drawing.Size(157, 20);
            this.AnimationLinkTextBox.TabIndex = 7;
            this.AnimationLinkTextBox.TextChanged += new System.EventHandler(this.AnimationLinkTextBox_TextChanged);
            // 
            // AnimationParametersListBox
            // 
            this.AnimationParametersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimationParametersListBox.Enabled = false;
            this.AnimationParametersListBox.FormattingEnabled = true;
            this.AnimationParametersListBox.Items.AddRange(new object[] {
            "Velocity X Low Bound",
            "Velocity X High Bound",
            "Velocity Y Low Bound",
            "Velocity Y High Bound",
            "Acceleration X Low Bound",
            "Acceleration X High Bound",
            "Acceleration Y Low Bound",
            "Acceleration Y High Bound",
            "On Ground",
            "On Roof",
            "On Wall",
            "Direction"});
            this.AnimationParametersListBox.Location = new System.Drawing.Point(117, 45);
            this.AnimationParametersListBox.Name = "AnimationParametersListBox";
            this.AnimationParametersListBox.Size = new System.Drawing.Size(157, 274);
            this.AnimationParametersListBox.TabIndex = 8;
            this.AnimationParametersListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.AnimationParametersListBox_ItemCheck);
            this.AnimationParametersListBox.SelectedIndexChanged += new System.EventHandler(this.AnimationParametersListBox_SelectedIndexChanged);
            // 
            // HoldersTabPage
            // 
            this.HoldersTabPage.Controls.Add(this.ToolDelayNumeric);
            this.HoldersTabPage.Controls.Add(this.ToolCheckBox);
            this.HoldersTabPage.Controls.Add(this.ToolLinkTextBox);
            this.HoldersTabPage.Controls.Add(this.label9);
            this.HoldersTabPage.Controls.Add(this.HolderNodeNumeric);
            this.HoldersTabPage.Controls.Add(this.label7);
            this.HoldersTabPage.Controls.Add(this.label8);
            this.HoldersTabPage.Controls.Add(this.HolderActionTextBox);
            this.HoldersTabPage.Controls.Add(this.HolderAnimationLinkTextBox);
            this.HoldersTabPage.Controls.Add(this.label5);
            this.HoldersTabPage.Controls.Add(this.HolderNameTextBox);
            this.HoldersTabPage.Controls.Add(this.label6);
            this.HoldersTabPage.Controls.Add(this.HoldersListBox);
            this.HoldersTabPage.Location = new System.Drawing.Point(4, 22);
            this.HoldersTabPage.Name = "HoldersTabPage";
            this.HoldersTabPage.Size = new System.Drawing.Size(280, 393);
            this.HoldersTabPage.TabIndex = 2;
            this.HoldersTabPage.Text = "Holders";
            this.HoldersTabPage.UseVisualStyleBackColor = true;
            // 
            // ToolDelayNumeric
            // 
            this.ToolDelayNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolDelayNumeric.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ToolDelayNumeric.Location = new System.Drawing.Point(117, 367);
            this.ToolDelayNumeric.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ToolDelayNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ToolDelayNumeric.Name = "ToolDelayNumeric";
            this.ToolDelayNumeric.Size = new System.Drawing.Size(160, 20);
            this.ToolDelayNumeric.TabIndex = 2;
            this.ToolDelayNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ToolDelayNumeric.ValueChanged += new System.EventHandler(this.ToolDelayNumeric_ValueChanged);
            // 
            // ToolCheckBox
            // 
            this.ToolCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ToolCheckBox.AutoSize = true;
            this.ToolCheckBox.Checked = true;
            this.ToolCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolCheckBox.Location = new System.Drawing.Point(117, 318);
            this.ToolCheckBox.Name = "ToolCheckBox";
            this.ToolCheckBox.Size = new System.Drawing.Size(88, 17);
            this.ToolCheckBox.TabIndex = 2;
            this.ToolCheckBox.Text = "Preview Tool";
            this.ToolCheckBox.UseVisualStyleBackColor = true;
            this.ToolCheckBox.CheckedChanged += new System.EventHandler(this.ToolCheckBox_CheckedChanged);
            // 
            // ToolLinkTextBox
            // 
            this.ToolLinkTextBox.AllowDrop = true;
            this.ToolLinkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolLinkTextBox.BackColor = System.Drawing.Color.Red;
            this.ToolLinkTextBox.Location = new System.Drawing.Point(117, 341);
            this.ToolLinkTextBox.Name = "ToolLinkTextBox";
            this.ToolLinkTextBox.Size = new System.Drawing.Size(160, 20);
            this.ToolLinkTextBox.TabIndex = 3;
            this.ToolLinkTextBox.TextChanged += new System.EventHandler(this.ToolLinkTextBox_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(114, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Holder node";
            // 
            // HolderNodeNumeric
            // 
            this.HolderNodeNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HolderNodeNumeric.Enabled = false;
            this.HolderNodeNumeric.Location = new System.Drawing.Point(117, 58);
            this.HolderNodeNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.HolderNodeNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.HolderNodeNumeric.Name = "HolderNodeNumeric";
            this.HolderNodeNumeric.Size = new System.Drawing.Size(160, 20);
            this.HolderNodeNumeric.TabIndex = 17;
            this.HolderNodeNumeric.ValueChanged += new System.EventHandler(this.HolderNodeNumeric_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(114, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Animation Link";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(114, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Action";
            // 
            // HolderActionTextBox
            // 
            this.HolderActionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HolderActionTextBox.Enabled = false;
            this.HolderActionTextBox.Location = new System.Drawing.Point(117, 19);
            this.HolderActionTextBox.Name = "HolderActionTextBox";
            this.HolderActionTextBox.Size = new System.Drawing.Size(160, 20);
            this.HolderActionTextBox.TabIndex = 20;
            this.HolderActionTextBox.TextChanged += new System.EventHandler(this.HolderActionTextBox_TextChanged);
            // 
            // HolderAnimationLinkTextBox
            // 
            this.HolderAnimationLinkTextBox.AllowDrop = true;
            this.HolderAnimationLinkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HolderAnimationLinkTextBox.BackColor = System.Drawing.Color.Red;
            this.HolderAnimationLinkTextBox.Enabled = false;
            this.HolderAnimationLinkTextBox.Location = new System.Drawing.Point(117, 97);
            this.HolderAnimationLinkTextBox.Name = "HolderAnimationLinkTextBox";
            this.HolderAnimationLinkTextBox.Size = new System.Drawing.Size(160, 20);
            this.HolderAnimationLinkTextBox.TabIndex = 19;
            this.HolderAnimationLinkTextBox.TextChanged += new System.EventHandler(this.HolderAnimationLinkTextBox_TextChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 351);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Name";
            // 
            // HolderNameTextBox
            // 
            this.HolderNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.HolderNameTextBox.Enabled = false;
            this.HolderNameTextBox.Location = new System.Drawing.Point(6, 367);
            this.HolderNameTextBox.Name = "HolderNameTextBox";
            this.HolderNameTextBox.Size = new System.Drawing.Size(105, 20);
            this.HolderNameTextBox.TabIndex = 13;
            this.HolderNameTextBox.TextChanged += new System.EventHandler(this.HolderNameTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Holders";
            // 
            // HoldersListBox
            // 
            this.HoldersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.HoldersListBox.FormattingEnabled = true;
            this.HoldersListBox.Location = new System.Drawing.Point(6, 19);
            this.HoldersListBox.Name = "HoldersListBox";
            this.HoldersListBox.Size = new System.Drawing.Size(105, 329);
            this.HoldersListBox.TabIndex = 14;
            this.HoldersListBox.SelectedIndexChanged += new System.EventHandler(this.HoldersListBox_SelectedIndexChanged);
            // 
            // GLSurface
            // 
            this.GLSurface.BackColor = System.Drawing.Color.Black;
            this.GLSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GLSurface.Location = new System.Drawing.Point(0, 0);
            this.GLSurface.Name = "GLSurface";
            this.GLSurface.NoClear = false;
            this.GLSurface.Size = new System.Drawing.Size(237, 419);
            this.GLSurface.TabIndex = 1;
            this.GLSurface.Text = "OpenGLSurface1";
            this.GLSurface.Zoom = 16F;
            this.GLSurface.GLPaint += new System.EventHandler(this.GLSurface_GLPaint);
            this.GLSurface.GLSizeChanged += new System.EventHandler(this.GLSurface_GLSizeChanged);
            this.GLSurface.GLMouseWheel += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseWheel);
            this.GLSurface.GLMouseDown += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseDown);
            this.GLSurface.GLMouseMove += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseMove);
            this.GLSurface.GLMouseUp += new ExtraForms.GLMouseEventHandler(this.GLSurface_GLMouseUp);
            this.GLSurface.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GLSurface_KeyDown);
            this.GLSurface.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GLSurface_KeyUp);
            this.GLSurface.Resize += new System.EventHandler(this.GLSurface_Resize);
            // 
            // EntityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ControlsSplitContainer);
            this.Name = "EntityControl";
            this.Size = new System.Drawing.Size(537, 423);
            this.ControlsSplitContainer.Panel1.ResumeLayout(false);
            this.ControlsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ControlsSplitContainer)).EndInit();
            this.ControlsSplitContainer.ResumeLayout(false);
            this.ParamsTabControl.ResumeLayout(false);
            this.PropertiesTabPage.ResumeLayout(false);
            this.PropertiesTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EntityParameterNumeric)).EndInit();
            this.AnimationsTabPage.ResumeLayout(false);
            this.AnimationsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationParameterNumeric)).EndInit();
            this.HoldersTabPage.ResumeLayout(false);
            this.HoldersTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ToolDelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HolderNodeNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer GLFrameTimer;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private Resource_Redactor.Resources.Redactors.PreviewSurface GLSurface;
        private System.Windows.Forms.SplitContainer ControlsSplitContainer;
        private System.Windows.Forms.TabControl ParamsTabControl;
        private System.Windows.Forms.TabPage PropertiesTabPage;
        private System.Windows.Forms.TabPage AnimationsTabPage;
        private System.Windows.Forms.TabPage HoldersTabPage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AnimationNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox AnimationsListBox;
        private System.Windows.Forms.NumericUpDown AnimationParameterNumeric;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AnimationActionTextBox;
        private Resource_Redactor.Resources.Redactors.SubresourceTextBox AnimationLinkTextBox;
        private System.Windows.Forms.CheckedListBox AnimationParametersListBox;
        private System.Windows.Forms.NumericUpDown HolderNodeNumeric;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox HolderActionTextBox;
        private Resource_Redactor.Resources.Redactors.SubresourceTextBox HolderAnimationLinkTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox HolderNameTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox HoldersListBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown EntityParameterNumeric;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox EntityParametersListBox;
        private System.Windows.Forms.Label label10;
        private Resource_Redactor.Resources.Redactors.SubresourceTextBox RagdollLinkTextBox;
        private System.Windows.Forms.DomainUpDown AnimationParameterDomain;
        private System.Windows.Forms.CheckBox ToolCheckBox;
        private Resource_Redactor.Resources.Redactors.SubresourceTextBox ToolLinkTextBox;
        private System.Windows.Forms.NumericUpDown ToolDelayNumeric;
    }
}
