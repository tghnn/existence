namespace shard0w
{
    partial class shard0w
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(shard0w));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.mM_File = new System.Windows.Forms.ToolStripMenuItem();
            this.file_New = new System.Windows.Forms.ToolStripMenuItem();
            this.file_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.file_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.file_Calculate = new System.Windows.Forms.ToolStripMenuItem();
            this.file_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.mM_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.edit_Undo = new System.Windows.Forms.ToolStripMenuItem();
            this.edit_Redo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.edit_Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.edit_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.edit_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.edit_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.Tools = new System.Windows.Forms.ToolStrip();
            this.tb_New = new System.Windows.Forms.ToolStripButton();
            this.tb_Open = new System.Windows.Forms.ToolStripButton();
            this.tb_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tb_Cut = new System.Windows.Forms.ToolStripButton();
            this.tb_Copy = new System.Windows.Forms.ToolStripButton();
            this.tb_Paste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tool_Calculate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tb_ZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tb_ZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tb_FontSize = new System.Windows.Forms.ToolStripComboBox();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.lineCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_ZoomFactor = new System.Windows.Forms.ToolStripStatusLabel();
            this.Document = new System.Windows.Forms.RichTextBox();
            this.rcMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.rc_Undo = new System.Windows.Forms.ToolStripMenuItem();
            this.rc_Redo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.rc_Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.rc_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.rc_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.openWork = new System.Windows.Forms.OpenFileDialog();
            this.saveWork = new System.Windows.Forms.SaveFileDialog();
            this.Result = new System.Windows.Forms.RichTextBox();
            this.mainMenu.SuspendLayout();
            this.Tools.SuspendLayout();
            this.Status.SuspendLayout();
            this.rcMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mM_File,
            this.mM_Edit});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.mainMenu.Size = new System.Drawing.Size(995, 28);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // mM_File
            // 
            this.mM_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file_New,
            this.file_Open,
            this.toolStripSeparator11,
            this.file_Save,
            this.toolStripSeparator13,
            this.file_Calculate,
            this.file_Exit});
            this.mM_File.Name = "mM_File";
            this.mM_File.Size = new System.Drawing.Size(44, 24);
            this.mM_File.Text = "&File";
            // 
            // file_New
            // 
            this.file_New.Image = ((System.Drawing.Image)(resources.GetObject("file_New.Image")));
            this.file_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.file_New.Name = "file_New";
            this.file_New.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.file_New.Size = new System.Drawing.Size(188, 24);
            this.file_New.Text = "&New";
            this.file_New.Click += new System.EventHandler(this.file_New_Click);
            // 
            // file_Open
            // 
            this.file_Open.Image = ((System.Drawing.Image)(resources.GetObject("file_Open.Image")));
            this.file_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.file_Open.Name = "file_Open";
            this.file_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.file_Open.Size = new System.Drawing.Size(188, 24);
            this.file_Open.Text = "&Open";
            this.file_Open.Click += new System.EventHandler(this.file_Open_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(185, 6);
            // 
            // file_Save
            // 
            this.file_Save.Image = ((System.Drawing.Image)(resources.GetObject("file_Save.Image")));
            this.file_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.file_Save.Name = "file_Save";
            this.file_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.file_Save.Size = new System.Drawing.Size(188, 24);
            this.file_Save.Text = "&Save";
            this.file_Save.Click += new System.EventHandler(this.file_Save_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(185, 6);
            // 
            // file_Calculate
            // 
            this.file_Calculate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.file_Calculate.Name = "file_Calculate";
            this.file_Calculate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.file_Calculate.Size = new System.Drawing.Size(188, 24);
            this.file_Calculate.Text = "Calculate";
            this.file_Calculate.Click += new System.EventHandler(this.file_Calculate_Click);
            // 
            // file_Exit
            // 
            this.file_Exit.Name = "file_Exit";
            this.file_Exit.Size = new System.Drawing.Size(188, 24);
            this.file_Exit.Text = "E&xit";
            this.file_Exit.Click += new System.EventHandler(this.file_Exit_Click);
            // 
            // mM_Edit
            // 
            this.mM_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.edit_Undo,
            this.edit_Redo,
            this.toolStripSeparator14,
            this.edit_Cut,
            this.edit_Copy,
            this.edit_Paste,
            this.toolStripSeparator15,
            this.edit_SelectAll});
            this.mM_Edit.Name = "mM_Edit";
            this.mM_Edit.Size = new System.Drawing.Size(47, 24);
            this.mM_Edit.Text = "&Edit";
            // 
            // edit_Undo
            // 
            this.edit_Undo.Name = "edit_Undo";
            this.edit_Undo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.edit_Undo.Size = new System.Drawing.Size(165, 24);
            this.edit_Undo.Text = "&Undo";
            this.edit_Undo.Click += new System.EventHandler(this.edit_Undo_Click);
            // 
            // edit_Redo
            // 
            this.edit_Redo.Name = "edit_Redo";
            this.edit_Redo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.edit_Redo.Size = new System.Drawing.Size(165, 24);
            this.edit_Redo.Text = "&Redo";
            this.edit_Redo.Click += new System.EventHandler(this.edit_Redo_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(162, 6);
            // 
            // edit_Cut
            // 
            this.edit_Cut.Image = ((System.Drawing.Image)(resources.GetObject("edit_Cut.Image")));
            this.edit_Cut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_Cut.Name = "edit_Cut";
            this.edit_Cut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.edit_Cut.Size = new System.Drawing.Size(165, 24);
            this.edit_Cut.Text = "Cu&t";
            this.edit_Cut.Click += new System.EventHandler(this.edit_Cut_Click);
            // 
            // edit_Copy
            // 
            this.edit_Copy.Image = ((System.Drawing.Image)(resources.GetObject("edit_Copy.Image")));
            this.edit_Copy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_Copy.Name = "edit_Copy";
            this.edit_Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.edit_Copy.Size = new System.Drawing.Size(165, 24);
            this.edit_Copy.Text = "&Copy";
            this.edit_Copy.Click += new System.EventHandler(this.edit_Copy_Click);
            // 
            // edit_Paste
            // 
            this.edit_Paste.Image = ((System.Drawing.Image)(resources.GetObject("edit_Paste.Image")));
            this.edit_Paste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_Paste.Name = "edit_Paste";
            this.edit_Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.edit_Paste.Size = new System.Drawing.Size(165, 24);
            this.edit_Paste.Text = "&Paste";
            this.edit_Paste.Click += new System.EventHandler(this.edit_Paste_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(162, 6);
            // 
            // edit_SelectAll
            // 
            this.edit_SelectAll.Name = "edit_SelectAll";
            this.edit_SelectAll.Size = new System.Drawing.Size(165, 24);
            this.edit_SelectAll.Text = "Select &All";
            this.edit_SelectAll.Click += new System.EventHandler(this.edit_SelectAll_Click);
            // 
            // Tools
            // 
            this.Tools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Tools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tb_New,
            this.tb_Open,
            this.tb_Save,
            this.toolStripSeparator,
            this.tb_Cut,
            this.tb_Copy,
            this.tb_Paste,
            this.toolStripSeparator1,
            this.tool_Calculate,
            this.toolStripSeparator8,
            this.tb_ZoomIn,
            this.tb_ZoomOut,
            this.toolStripSeparator2,
            this.tb_FontSize});
            this.Tools.Location = new System.Drawing.Point(0, 28);
            this.Tools.Name = "Tools";
            this.Tools.Size = new System.Drawing.Size(995, 28);
            this.Tools.TabIndex = 1;
            this.Tools.Text = "toolStrip1";
            // 
            // tb_New
            // 
            this.tb_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tb_New.Image = ((System.Drawing.Image)(resources.GetObject("tb_New.Image")));
            this.tb_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tb_New.Name = "tb_New";
            this.tb_New.Size = new System.Drawing.Size(23, 25);
            this.tb_New.Text = "&New";
            this.tb_New.Click += new System.EventHandler(this.tb_New_Click);
            // 
            // tb_Open
            // 
            this.tb_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tb_Open.Image = ((System.Drawing.Image)(resources.GetObject("tb_Open.Image")));
            this.tb_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tb_Open.Name = "tb_Open";
            this.tb_Open.Size = new System.Drawing.Size(23, 25);
            this.tb_Open.Text = "&Open";
            this.tb_Open.Click += new System.EventHandler(this.tb_Open_Click);
            // 
            // tb_Save
            // 
            this.tb_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tb_Save.Image = ((System.Drawing.Image)(resources.GetObject("tb_Save.Image")));
            this.tb_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tb_Save.Name = "tb_Save";
            this.tb_Save.Size = new System.Drawing.Size(23, 25);
            this.tb_Save.Text = "&Save";
            this.tb_Save.Click += new System.EventHandler(this.tb_Save_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 28);
            // 
            // tb_Cut
            // 
            this.tb_Cut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tb_Cut.Image = ((System.Drawing.Image)(resources.GetObject("tb_Cut.Image")));
            this.tb_Cut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tb_Cut.Name = "tb_Cut";
            this.tb_Cut.Size = new System.Drawing.Size(23, 25);
            this.tb_Cut.Text = "C&ut";
            this.tb_Cut.Click += new System.EventHandler(this.tb_Cut_Click);
            // 
            // tb_Copy
            // 
            this.tb_Copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tb_Copy.Image = ((System.Drawing.Image)(resources.GetObject("tb_Copy.Image")));
            this.tb_Copy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tb_Copy.Name = "tb_Copy";
            this.tb_Copy.Size = new System.Drawing.Size(23, 25);
            this.tb_Copy.Text = "&Copy";
            this.tb_Copy.Click += new System.EventHandler(this.tb_Copy_Click);
            // 
            // tb_Paste
            // 
            this.tb_Paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tb_Paste.Image = ((System.Drawing.Image)(resources.GetObject("tb_Paste.Image")));
            this.tb_Paste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tb_Paste.Name = "tb_Paste";
            this.tb_Paste.Size = new System.Drawing.Size(23, 25);
            this.tb_Paste.Text = "&Paste";
            this.tb_Paste.Click += new System.EventHandler(this.tb_Paste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // tool_Calculate
            // 
            this.tool_Calculate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tool_Calculate.Image = ((System.Drawing.Image)(resources.GetObject("tool_Calculate.Image")));
            this.tool_Calculate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tool_Calculate.Name = "tool_Calculate";
            this.tool_Calculate.Size = new System.Drawing.Size(23, 25);
            this.tool_Calculate.Text = "Calculate";
            this.tool_Calculate.Click += new System.EventHandler(this.file_Calculate_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 28);
            // 
            // tb_ZoomIn
            // 
            this.tb_ZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tb_ZoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_ZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("tb_ZoomIn.Image")));
            this.tb_ZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tb_ZoomIn.Name = "tb_ZoomIn";
            this.tb_ZoomIn.Size = new System.Drawing.Size(23, 25);
            this.tb_ZoomIn.Text = "+";
            this.tb_ZoomIn.ToolTipText = "Zoom In";
            this.tb_ZoomIn.Click += new System.EventHandler(this.tb_ZoomIn_Click);
            // 
            // tb_ZoomOut
            // 
            this.tb_ZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tb_ZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_ZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("tb_ZoomOut.Image")));
            this.tb_ZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tb_ZoomOut.Name = "tb_ZoomOut";
            this.tb_ZoomOut.Size = new System.Drawing.Size(23, 25);
            this.tb_ZoomOut.Text = "-";
            this.tb_ZoomOut.ToolTipText = "Zoom Out";
            this.tb_ZoomOut.Click += new System.EventHandler(this.tb_ZoomOut_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // tb_FontSize
            // 
            this.tb_FontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tb_FontSize.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.tb_FontSize.Name = "tb_FontSize";
            this.tb_FontSize.Size = new System.Drawing.Size(99, 28);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineCount,
            this.toolStripStatusLabel2,
            this.status_ZoomFactor});
            this.Status.Location = new System.Drawing.Point(0, 510);
            this.Status.Name = "Status";
            this.Status.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.Status.Size = new System.Drawing.Size(995, 25);
            this.Status.TabIndex = 2;
            this.Status.Text = "statusStrip1";
            // 
            // lineCount
            // 
            this.lineCount.Name = "lineCount";
            this.lineCount.Size = new System.Drawing.Size(36, 20);
            this.lineCount.Text = "Line";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(859, 20);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // status_ZoomFactor
            // 
            this.status_ZoomFactor.Name = "status_ZoomFactor";
            this.status_ZoomFactor.Size = new System.Drawing.Size(49, 20);
            this.status_ZoomFactor.Text = "Zoom";
            // 
            // Document
            // 
            this.Document.ContextMenuStrip = this.rcMenu;
            this.Document.Dock = System.Windows.Forms.DockStyle.Top;
            this.Document.Location = new System.Drawing.Point(0, 56);
            this.Document.Margin = new System.Windows.Forms.Padding(4);
            this.Document.Name = "Document";
            this.Document.Size = new System.Drawing.Size(995, 239);
            this.Document.TabIndex = 3;
            this.Document.Text = "";
            this.Document.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.Document_LinkClicked);
            // 
            // rcMenu
            // 
            this.rcMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rc_Undo,
            this.rc_Redo,
            this.toolStripSeparator10,
            this.rc_Cut,
            this.rc_Copy,
            this.rc_Paste});
            this.rcMenu.Name = "rcMenu";
            this.rcMenu.Size = new System.Drawing.Size(115, 130);
            // 
            // rc_Undo
            // 
            this.rc_Undo.Name = "rc_Undo";
            this.rc_Undo.Size = new System.Drawing.Size(114, 24);
            this.rc_Undo.Text = "Undo";
            this.rc_Undo.Click += new System.EventHandler(this.rc_Undo_Click);
            // 
            // rc_Redo
            // 
            this.rc_Redo.Name = "rc_Redo";
            this.rc_Redo.Size = new System.Drawing.Size(114, 24);
            this.rc_Redo.Text = "Redo";
            this.rc_Redo.Click += new System.EventHandler(this.rc_Redo_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(111, 6);
            // 
            // rc_Cut
            // 
            this.rc_Cut.Name = "rc_Cut";
            this.rc_Cut.Size = new System.Drawing.Size(114, 24);
            this.rc_Cut.Text = "Cut";
            this.rc_Cut.Click += new System.EventHandler(this.rc_Cut_Click);
            // 
            // rc_Copy
            // 
            this.rc_Copy.Name = "rc_Copy";
            this.rc_Copy.Size = new System.Drawing.Size(114, 24);
            this.rc_Copy.Text = "Copy";
            this.rc_Copy.Click += new System.EventHandler(this.rc_Copy_Click);
            // 
            // rc_Paste
            // 
            this.rc_Paste.Name = "rc_Paste";
            this.rc_Paste.Size = new System.Drawing.Size(114, 24);
            this.rc_Paste.Text = "Paste";
            this.rc_Paste.Click += new System.EventHandler(this.rc_Paste_Click);
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Interval = 1;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick_1);
            // 
            // openWork
            // 
            this.openWork.Filter = "Text Files|*.txt|RTF Files|*.rtf";
            this.openWork.Title = "Open Work";
            // 
            // saveWork
            // 
            this.saveWork.Filter = "Text Files|*.txt|RTF Files|*.rtf";
            this.saveWork.Title = "Save Work";
            // 
            // Result
            // 
            this.Result.ContextMenuStrip = this.rcMenu;
            this.Result.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Result.Location = new System.Drawing.Point(0, 303);
            this.Result.Margin = new System.Windows.Forms.Padding(4);
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.Size = new System.Drawing.Size(995, 207);
            this.Result.TabIndex = 4;
            this.Result.Text = "";
            // 
            // shard0w
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 535);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.Document);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Tools);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(991, 572);
            this.Name = "shard0w";
            this.ShowIcon = false;
            this.Text = "shard0w";
            this.Load += new System.EventHandler(this.shard0w_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.Tools.ResumeLayout(false);
            this.Tools.PerformLayout();
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.rcMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStrip Tools;
        private System.Windows.Forms.ToolStripButton tb_New;
        private System.Windows.Forms.ToolStripButton tb_Open;
        private System.Windows.Forms.ToolStripButton tb_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton tb_Cut;
        private System.Windows.Forms.ToolStripButton tb_Copy;
        private System.Windows.Forms.ToolStripButton tb_Paste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox tb_FontSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripStatusLabel lineCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel status_ZoomFactor;
        private System.Windows.Forms.RichTextBox Document;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton tb_ZoomIn;
        private System.Windows.Forms.ToolStripButton tb_ZoomOut;
        private System.Windows.Forms.ContextMenuStrip rcMenu;
        private System.Windows.Forms.ToolStripMenuItem rc_Undo;
        private System.Windows.Forms.ToolStripMenuItem rc_Redo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem rc_Cut;
        private System.Windows.Forms.ToolStripMenuItem rc_Copy;
        private System.Windows.Forms.ToolStripMenuItem rc_Paste;
        private System.Windows.Forms.ToolStripMenuItem mM_Edit;
        private System.Windows.Forms.ToolStripMenuItem edit_Undo;
        private System.Windows.Forms.ToolStripMenuItem edit_Redo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem edit_Cut;
        private System.Windows.Forms.ToolStripMenuItem edit_Copy;
        private System.Windows.Forms.ToolStripMenuItem edit_Paste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem edit_SelectAll;
        private System.Windows.Forms.ToolStripMenuItem mM_File;
        private System.Windows.Forms.ToolStripMenuItem file_New;
        private System.Windows.Forms.ToolStripMenuItem file_Open;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem file_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem file_Exit;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.OpenFileDialog openWork;
        private System.Windows.Forms.SaveFileDialog saveWork;
        private System.Windows.Forms.ToolStripMenuItem file_Calculate;
        private System.Windows.Forms.ToolStripButton tool_Calculate;
        private System.Windows.Forms.RichTextBox Result;
    }
}

