using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace _3D_Tree_Generator
{
    partial class Window
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
            this.glControl1 = new OpenTK.GLControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forrestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.environmentToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldOfViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frameRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leafToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.environmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Branching = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.TreeHeight = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.Quality = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.Trunk_Thickness = new System.Windows.Forms.NumericUpDown();
            this.Seed = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.TrunkFunction = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.BranchFunction = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.Segments = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Branching)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TreeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Quality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trunk_Thickness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Seed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Segments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.glControl1.Location = new System.Drawing.Point(12, 27);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(823, 642);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseWheel);
            this.glControl1.Move += new System.EventHandler(this.glControl1_Move);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.backgroundToolStripMenuItem,
            this.autoRefreshToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 92);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.backgroundToolStripMenuItem.Text = "Background";
            // 
            // autoRefreshToolStripMenuItem
            // 
            this.autoRefreshToolStripMenuItem.CheckOnClick = true;
            this.autoRefreshToolStripMenuItem.Name = "autoRefreshToolStripMenuItem";
            this.autoRefreshToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.autoRefreshToolStripMenuItem.Text = "Auto-Refresh";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.leafToolStripMenuItem,
            this.environmentToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.Open);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAs);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentTreeToolStripMenuItem,
            this.forrestToolStripMenuItem,
            this.environmentToolStripMenuItem1});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // currentTreeToolStripMenuItem
            // 
            this.currentTreeToolStripMenuItem.Name = "currentTreeToolStripMenuItem";
            this.currentTreeToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.currentTreeToolStripMenuItem.Text = "Current Tree";
            // 
            // forrestToolStripMenuItem
            // 
            this.forrestToolStripMenuItem.Name = "forrestToolStripMenuItem";
            this.forrestToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.forrestToolStripMenuItem.Text = "Forrest";
            // 
            // environmentToolStripMenuItem1
            // 
            this.environmentToolStripMenuItem1.Name = "environmentToolStripMenuItem1";
            this.environmentToolStripMenuItem1.Size = new System.Drawing.Size(142, 22);
            this.environmentToolStripMenuItem1.Text = "Environment";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fieldOfViewToolStripMenuItem,
            this.frameRateToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // fieldOfViewToolStripMenuItem
            // 
            this.fieldOfViewToolStripMenuItem.Name = "fieldOfViewToolStripMenuItem";
            this.fieldOfViewToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.fieldOfViewToolStripMenuItem.Text = "Field Of View";
            // 
            // frameRateToolStripMenuItem
            // 
            this.frameRateToolStripMenuItem.Name = "frameRateToolStripMenuItem";
            this.frameRateToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.frameRateToolStripMenuItem.Text = "Frame Rate";
            // 
            // leafToolStripMenuItem
            // 
            this.leafToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.screenshotToolStripMenuItem});
            this.leafToolStripMenuItem.Name = "leafToolStripMenuItem";
            this.leafToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.leafToolStripMenuItem.Text = "Leaf";
            // 
            // createNewToolStripMenuItem
            // 
            this.createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
            this.createNewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.createNewToolStripMenuItem.Text = "Create New";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load From File";
            // 
            // screenshotToolStripMenuItem
            // 
            this.screenshotToolStripMenuItem.Name = "screenshotToolStripMenuItem";
            this.screenshotToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.screenshotToolStripMenuItem.Text = "Screenshot";
            // 
            // environmentToolStripMenuItem
            // 
            this.environmentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewToolStripMenuItem1,
            this.loadToolStripMenuItem1});
            this.environmentToolStripMenuItem.Name = "environmentToolStripMenuItem";
            this.environmentToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.environmentToolStripMenuItem.Text = "Environment";
            // 
            // createNewToolStripMenuItem1
            // 
            this.createNewToolStripMenuItem1.Name = "createNewToolStripMenuItem1";
            this.createNewToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.createNewToolStripMenuItem1.Text = "Create New";
            // 
            // loadToolStripMenuItem1
            // 
            this.loadToolStripMenuItem1.Name = "loadToolStripMenuItem1";
            this.loadToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.loadToolStripMenuItem1.Text = "Load";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.guideToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // guideToolStripMenuItem
            // 
            this.guideToolStripMenuItem.Name = "guideToolStripMenuItem";
            this.guideToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.guideToolStripMenuItem.Text = "Guide";
            // 
            // Branching
            // 
            this.Branching.AccessibleName = "MaxDist";
            this.Branching.DecimalPlaces = 2;
            this.Branching.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Branching.Location = new System.Drawing.Point(172, 16);
            this.Branching.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Branching.Name = "Branching";
            this.Branching.Size = new System.Drawing.Size(43, 20);
            this.Branching.TabIndex = 4;
            this.Branching.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Branching.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1153, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Height";
            // 
            // TreeHeight
            // 
            this.TreeHeight.AccessibleName = "Height";
            this.TreeHeight.DecimalPlaces = 1;
            this.TreeHeight.Location = new System.Drawing.Point(1198, 143);
            this.TreeHeight.Name = "TreeHeight";
            this.TreeHeight.Size = new System.Drawing.Size(43, 20);
            this.TreeHeight.TabIndex = 6;
            this.TreeHeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TreeHeight.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1087, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Horizontal Segments";
            // 
            // Quality
            // 
            this.Quality.AccessibleName = "Quality";
            this.Quality.Location = new System.Drawing.Point(1198, 169);
            this.Quality.Name = "Quality";
            this.Quality.Size = new System.Drawing.Size(43, 20);
            this.Quality.TabIndex = 8;
            this.Quality.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.Quality.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1104, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Trunk Thickness";
            // 
            // Trunk_Thickness
            // 
            this.Trunk_Thickness.AccessibleName = "TrunkRadius";
            this.Trunk_Thickness.DecimalPlaces = 2;
            this.Trunk_Thickness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Trunk_Thickness.Location = new System.Drawing.Point(1198, 221);
            this.Trunk_Thickness.Name = "Trunk_Thickness";
            this.Trunk_Thickness.Size = new System.Drawing.Size(43, 20);
            this.Trunk_Thickness.TabIndex = 10;
            this.Trunk_Thickness.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Trunk_Thickness.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // Seed
            // 
            this.Seed.AccessibleName = "Seed";
            this.Seed.Location = new System.Drawing.Point(1156, 69);
            this.Seed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Seed.Name = "Seed";
            this.Seed.Size = new System.Drawing.Size(81, 20);
            this.Seed.TabIndex = 12;
            this.Seed.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1116, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Seed";
            // 
            // RefreshButton
            // 
            this.RefreshButton.AccessibleName = "Refresh";
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshButton.Location = new System.Drawing.Point(970, 584);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(168, 51);
            this.RefreshButton.TabIndex = 16;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // TrunkFunction
            // 
            this.TrunkFunction.AccessibleName = "TrunkFunction";
            this.TrunkFunction.Location = new System.Drawing.Point(978, 506);
            this.TrunkFunction.Name = "TrunkFunction";
            this.TrunkFunction.Size = new System.Drawing.Size(194, 20);
            this.TrunkFunction.TabIndex = 17;
            this.TrunkFunction.Text = "x";
            // 
            // label8
            // 
            this.label8.AccessibleName = "";
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(893, 509);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Trunk Function";
            // 
            // button1
            // 
            this.button1.AccessibleName = "TrunkFunction";
            this.button1.Location = new System.Drawing.Point(1178, 506);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 20);
            this.button1.TabIndex = 19;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Function_Update);
            // 
            // label9
            // 
            this.label9.AccessibleName = "";
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(887, 535);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Branch Function";
            // 
            // BranchFunction
            // 
            this.BranchFunction.AccessibleName = "BranchFunction";
            this.BranchFunction.Location = new System.Drawing.Point(978, 532);
            this.BranchFunction.Name = "BranchFunction";
            this.BranchFunction.Size = new System.Drawing.Size(194, 20);
            this.BranchFunction.TabIndex = 20;
            this.BranchFunction.Text = "x";
            // 
            // button2
            // 
            this.button2.AccessibleName = "BranchFunction";
            this.button2.Location = new System.Drawing.Point(1178, 532);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 20);
            this.button2.TabIndex = 22;
            this.button2.Text = "Update";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Function_Update);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1099, 197);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Vertical Segments";
            // 
            // Segments
            // 
            this.Segments.AccessibleName = "Segments";
            this.Segments.Location = new System.Drawing.Point(1198, 195);
            this.Segments.Name = "Segments";
            this.Segments.Size = new System.Drawing.Size(43, 20);
            this.Segments.TabIndex = 23;
            this.Segments.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.Segments.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.AccessibleName = "MinDist";
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(66, 16);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown1.TabIndex = 25;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Minimum";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(114, 18);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Maximum";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.Branching);
            this.groupBox1.Location = new System.Drawing.Point(1014, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 47);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Branch Distance";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.AccessibleName = "Depth";
            this.numericUpDown2.Location = new System.Drawing.Point(1198, 247);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown2.TabIndex = 29;
            this.numericUpDown2.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1116, 249);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Branch Depth";
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Segments);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.BranchFunction);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.TrunkFunction);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Seed);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Trunk_Thickness);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Quality);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TreeHeight);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Window";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "3D Tree Gerenerator by Jeremy Zolnai-Lucas";
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Branching)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TreeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Quality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trunk_Thickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Seed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Segments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GLControl glControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forrestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leafToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem environmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown Branching;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown TreeHeight;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem backgroundToolStripMenuItem;
        private ToolStripMenuItem fieldOfViewToolStripMenuItem;
        private ToolStripMenuItem frameRateToolStripMenuItem;
        private ToolStripMenuItem createNewToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem screenshotToolStripMenuItem;
        private ToolStripMenuItem createNewToolStripMenuItem1;
        private ToolStripMenuItem loadToolStripMenuItem1;
        private ToolStripMenuItem guideToolStripMenuItem;
        private ToolStripMenuItem environmentToolStripMenuItem1;
        private Label label4;
        private NumericUpDown Quality;
        private Label label5;
        private NumericUpDown Trunk_Thickness;
        private NumericUpDown Seed;
        private Label label6;
        private ToolStripMenuItem autoRefreshToolStripMenuItem;
        private Button RefreshButton;
        private TextBox TrunkFunction;
        private Label label8;
        private Button button1;
        private Label label9;
        private TextBox BranchFunction;
        private Button button2;
        private Label label10;
        private NumericUpDown Segments;
        private NumericUpDown numericUpDown1;
        private Label label1;
        private Label label11;
        private GroupBox groupBox1;
        private NumericUpDown numericUpDown2;
        private Label label2;
    }
}

