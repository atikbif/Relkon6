namespace Kontel.Relkon
{
    partial class SetModulesAddressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetModulesAddressForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ddlComPortName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ibWriteModulesAddresses = new Kontel.Relkon.ImageButton();
            this.ibReadModulesAddresses = new Kontel.Relkon.ImageButton();
            this.bwRW = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbWriteProgrss = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgModules = new System.Windows.Forms.DataGridView();
            this.ModulesColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lbErrors = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgModules)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ddlComPortName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ibWriteModulesAddresses);
            this.panel1.Controls.Add(this.ibReadModulesAddresses);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(153, 318);
            this.panel1.TabIndex = 0;
            // 
            // ddlComPortName
            // 
            this.ddlComPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlComPortName.FormattingEnabled = true;
            this.ddlComPortName.Location = new System.Drawing.Point(3, 289);
            this.ddlComPortName.Name = "ddlComPortName";
            this.ddlComPortName.Size = new System.Drawing.Size(147, 21);
            this.ddlComPortName.Sorted = true;
            this.ddlComPortName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 273);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Имя COM-порта:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Записать данные в модули";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Считать данные с модулей";
            // 
            // ibWriteModulesAddresses
            // 
            this.ibWriteModulesAddresses.Image = ((System.Drawing.Image)(resources.GetObject("ibWriteModulesAddresses.Image")));
            this.ibWriteModulesAddresses.InactiveImage = ((System.Drawing.Image)(resources.GetObject("ibWriteModulesAddresses.InactiveImage")));
            this.ibWriteModulesAddresses.Location = new System.Drawing.Point(3, 132);
            this.ibWriteModulesAddresses.Name = "ibWriteModulesAddresses";
            this.ibWriteModulesAddresses.Size = new System.Drawing.Size(147, 110);
            this.ibWriteModulesAddresses.TabIndex = 3;
            this.ibWriteModulesAddresses.Click += new System.EventHandler(this.ibWriteModulesAddresses_Click);
            // 
            // ibReadModulesAddresses
            // 
            this.ibReadModulesAddresses.Image = ((System.Drawing.Image)(resources.GetObject("ibReadModulesAddresses.Image")));
            this.ibReadModulesAddresses.InactiveImage = ((System.Drawing.Image)(resources.GetObject("ibReadModulesAddresses.InactiveImage")));
            this.ibReadModulesAddresses.Location = new System.Drawing.Point(3, 3);
            this.ibReadModulesAddresses.Name = "ibReadModulesAddresses";
            this.ibReadModulesAddresses.Size = new System.Drawing.Size(148, 110);
            this.ibReadModulesAddresses.TabIndex = 2;
            this.ibReadModulesAddresses.Click += new System.EventHandler(this.ibReadModulesAddresses_Click);
            // 
            // bwRW
            // 
            this.bwRW.WorkerReportsProgress = true;
            this.bwRW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwRW_DoWork);
            this.bwRW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwRW_RunWorkerCompleted);
            this.bwRW.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwRW_ProgressChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 318);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(584, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabel1.Text = "Запись данных:";
            this.toolStripStatusLabel1.Visible = false;
            // 
            // pbWriteProgrss
            // 
            this.pbWriteProgrss.Name = "pbWriteProgrss";
            this.pbWriteProgrss.Size = new System.Drawing.Size(100, 16);
            this.pbWriteProgrss.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(153, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgModules);
            this.splitContainer1.Panel1MinSize = 110;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lbErrors);
            this.splitContainer1.Size = new System.Drawing.Size(431, 318);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 3;
            // 
            // dgModules
            // 
            this.dgModules.AllowUserToAddRows = false;
            this.dgModules.AllowUserToDeleteRows = false;
            this.dgModules.AllowUserToResizeColumns = false;
            this.dgModules.AllowUserToResizeRows = false;
            this.dgModules.BackgroundColor = System.Drawing.Color.White;
            this.dgModules.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgModules.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgModules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgModules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ModulesColumn});
            this.dgModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgModules.Location = new System.Drawing.Point(0, 0);
            this.dgModules.MultiSelect = false;
            this.dgModules.Name = "dgModules";
            this.dgModules.RowHeadersWidth = 40;
            this.dgModules.Size = new System.Drawing.Size(431, 240);
            this.dgModules.TabIndex = 2;
            this.dgModules.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgModules_CellMouseDown);
            // 
            // ModulesColumn
            // 
            this.ModulesColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ModulesColumn.HeaderText = "Список модулей";
            this.ModulesColumn.Name = "ModulesColumn";
            this.ModulesColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // lbErrors
            // 
            this.lbErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbErrors.FormattingEnabled = true;
            this.lbErrors.Location = new System.Drawing.Point(0, 0);
            this.lbErrors.Name = "lbErrors";
            this.lbErrors.Size = new System.Drawing.Size(431, 69);
            this.lbErrors.TabIndex = 0;
            this.lbErrors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbErrors_MouseDoubleClick);
            // 
            // SetModulesAddressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 340);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 10000);
            this.MinimumSize = new System.Drawing.Size(600, 376);
            this.Name = "SetModulesAddressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Установка адресов модулей Matchbox";
            this.Load += new System.EventHandler(this.SetModulesAddressForm_Load);
            this.SizeChanged += new System.EventHandler(this.SetModulesAddressForm_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetModulesAddressForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgModules)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Kontel.Relkon.ImageButton ibWriteModulesAddresses;
        private Kontel.Relkon.ImageButton ibReadModulesAddresses;
        private System.Windows.Forms.ComboBox ddlComPortName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker bwRW;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar pbWriteProgrss;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgModules;
        private System.Windows.Forms.DataGridViewComboBoxColumn ModulesColumn;
        private System.Windows.Forms.ListBox lbErrors;
    }
}