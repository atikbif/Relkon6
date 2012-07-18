namespace Kontel.Relkon.Components
{
    partial class ErrorList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorList));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsbShowErrors = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbShowWarnings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbOpenErrorFile = new System.Windows.Forms.ToolStripButton();
            this.dgErrors = new System.Windows.Forms.DataGridView();
            this.Category = new System.Windows.Forms.DataGridViewImageColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgErrors)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tsbShowErrors,
            this.toolStripSeparator1,
            this.tsbShowWarnings,
            this.toolStripSeparator2,
            this.tsbOpenErrorFile});
            this.toolStrip1.Location = new System.Drawing.Point(1, 1);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(248, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(10, 22);
            this.toolStripLabel1.Text = " ";
            // 
            // tsbShowErrors
            // 
            this.tsbShowErrors.Checked = true;
            this.tsbShowErrors.CheckOnClick = true;
            this.tsbShowErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbShowErrors.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowErrors.Image")));
            this.tsbShowErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowErrors.Name = "tsbShowErrors";
            this.tsbShowErrors.Size = new System.Drawing.Size(76, 22);
            this.tsbShowErrors.Text = "0 Ошибок";
            this.tsbShowErrors.CheckedChanged += new System.EventHandler(this.tsbShowErrors_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbShowWarnings
            // 
            this.tsbShowWarnings.Checked = true;
            this.tsbShowWarnings.CheckOnClick = true;
            this.tsbShowWarnings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbShowWarnings.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowWarnings.Image")));
            this.tsbShowWarnings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowWarnings.Name = "tsbShowWarnings";
            this.tsbShowWarnings.Size = new System.Drawing.Size(125, 22);
            this.tsbShowWarnings.Text = "0 Предупреждений";
            this.tsbShowWarnings.CheckedChanged += new System.EventHandler(this.tsbShowWarnings_CheckedChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbOpenErrorFile
            // 
            this.tsbOpenErrorFile.Enabled = false;
            this.tsbOpenErrorFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpenErrorFile.Image")));
            this.tsbOpenErrorFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenErrorFile.Name = "tsbOpenErrorFile";
            this.tsbOpenErrorFile.Size = new System.Drawing.Size(143, 20);
            this.tsbOpenErrorFile.Text = "Открыть файл ошибок";
            this.tsbOpenErrorFile.Click += new System.EventHandler(this.tsbOpenErrorFile_Click);
            // 
            // dgErrors
            // 
            this.dgErrors.AllowUserToAddRows = false;
            this.dgErrors.AllowUserToResizeRows = false;
            this.dgErrors.BackgroundColor = System.Drawing.Color.White;
            this.dgErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgErrors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Category,
            this.Description,
            this.File,
            this.Line});
            this.dgErrors.ContextMenuStrip = this.contextMenuStrip1;
            this.dgErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgErrors.GridColor = System.Drawing.Color.White;
            this.dgErrors.Location = new System.Drawing.Point(1, 26);
            this.dgErrors.MultiSelect = false;
            this.dgErrors.Name = "dgErrors";
            this.dgErrors.RowHeadersVisible = false;
            this.dgErrors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgErrors.Size = new System.Drawing.Size(248, 373);
            this.dgErrors.TabIndex = 1;
            this.dgErrors.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgErrors_CellDoubleClick);
            this.dgErrors.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgErrors_ColumnWidthChanged);
            // 
            // Category
            // 
            this.Category.FillWeight = 20F;
            this.Category.HeaderText = "";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            this.Category.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Category.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Category.ToolTipText = "Категория";
            this.Category.Width = 20;
            // 
            // Description
            // 
            this.Description.HeaderText = "Описание";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // File
            // 
            this.File.HeaderText = "Файл";
            this.File.Name = "File";
            this.File.ReadOnly = true;
            // 
            // Line
            // 
            this.Line.HeaderText = "Строка";
            this.Line.Name = "Line";
            this.Line.ReadOnly = true;
            this.Line.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "ErrorList.bmp");
            this.imageList1.Images.SetKeyName(1, "Error.bmp");
            this.imageList1.Images.SetKeyName(2, "Warning.bmp");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCopy});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // miCopy
            // 
            this.miCopy.Image = ((System.Drawing.Image)(resources.GetObject("miCopy.Image")));
            this.miCopy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCopy.Name = "miCopy";
            this.miCopy.Size = new System.Drawing.Size(146, 22);
            this.miCopy.Text = "Копировать";
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // ErrorList
            // 
            this.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.Controls.Add(this.dgErrors);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ErrorList";
            this.Text = "Список ошибок";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgErrors)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataGridView dgErrors;
        private System.Windows.Forms.ToolStripButton tsbShowErrors;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbShowWarnings;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbOpenErrorFile;
        private System.Windows.Forms.DataGridViewImageColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miCopy;

    }
}
