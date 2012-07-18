namespace Kontel.Relkon.Components.Documents
{
    partial class ViewStructursTabbedDocument
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.borderPanel1 = new Kontel.Relkon.BorderPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgv_askStructs = new System.Windows.Forms.DataGridView();
            this.structNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayValueDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.memoryTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structsDataSet = new Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewStructursTabbedDocument.StructsDataSet();
            this.clb_structurs = new System.Windows.Forms.CheckedListBox();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.borderPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_askStructs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.structsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // borderPanel1
            // 
            this.borderPanel1.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.borderPanel1.Controls.Add(this.groupBox1);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel1.Location = new System.Drawing.Point(5, 5);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.borderPanel1.Size = new System.Drawing.Size(540, 390);
            this.borderPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgv_askStructs);
            this.groupBox1.Controls.Add(this.clb_structurs);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(534, 384);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Структуры проекта";
            // 
            // dgv_askStructs
            // 
            this.dgv_askStructs.AllowUserToAddRows = false;
            this.dgv_askStructs.AllowUserToDeleteRows = false;
            this.dgv_askStructs.AllowUserToResizeRows = false;
            this.dgv_askStructs.AutoGenerateColumns = false;
            this.dgv_askStructs.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv_askStructs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_askStructs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_askStructs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_askStructs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.structNameDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.displayValueDataGridViewTextBoxColumn1,
            this.displayAddressDataGridViewTextBoxColumn,
            this.memoryTypeDataGridViewTextBoxColumn,
            this.sizeDataGridViewTextBoxColumn});
            this.dgv_askStructs.DataMember = "dtDisplayVars";
            this.dgv_askStructs.DataSource = this.structsDataSet;
            this.dgv_askStructs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_askStructs.Location = new System.Drawing.Point(123, 16);
            this.dgv_askStructs.Name = "dgv_askStructs";
            this.dgv_askStructs.RowHeadersVisible = false;
            this.dgv_askStructs.Size = new System.Drawing.Size(408, 365);
            this.dgv_askStructs.TabIndex = 0;
            this.dgv_askStructs.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgv_askStructs_CellBeginEdit);
            this.dgv_askStructs.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_askStructs_CellEndEdit);
            // 
            // structNameDataGridViewTextBoxColumn
            // 
            this.structNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.structNameDataGridViewTextBoxColumn.DataPropertyName = "StructName";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.structNameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.structNameDataGridViewTextBoxColumn.HeaderText = "Название структуры";
            this.structNameDataGridViewTextBoxColumn.Name = "structNameDataGridViewTextBoxColumn";
            this.structNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.structNameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.nameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.nameDataGridViewTextBoxColumn.HeaderText = "Название переменной";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // displayValueDataGridViewTextBoxColumn1
            // 
            this.displayValueDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.displayValueDataGridViewTextBoxColumn1.DataPropertyName = "DisplayValue";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.displayValueDataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle4;
            this.displayValueDataGridViewTextBoxColumn1.HeaderText = "Значение";
            this.displayValueDataGridViewTextBoxColumn1.Name = "displayValueDataGridViewTextBoxColumn1";
            this.displayValueDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // displayAddressDataGridViewTextBoxColumn
            // 
            this.displayAddressDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.displayAddressDataGridViewTextBoxColumn.DataPropertyName = "DisplayAddress";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.displayAddressDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.displayAddressDataGridViewTextBoxColumn.HeaderText = "Адрес";
            this.displayAddressDataGridViewTextBoxColumn.Name = "displayAddressDataGridViewTextBoxColumn";
            this.displayAddressDataGridViewTextBoxColumn.ReadOnly = true;
            this.displayAddressDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.displayAddressDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // memoryTypeDataGridViewTextBoxColumn
            // 
            this.memoryTypeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.memoryTypeDataGridViewTextBoxColumn.DataPropertyName = "MemoryType";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.memoryTypeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.memoryTypeDataGridViewTextBoxColumn.HeaderText = "Тип памяти";
            this.memoryTypeDataGridViewTextBoxColumn.Name = "memoryTypeDataGridViewTextBoxColumn";
            this.memoryTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.memoryTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // sizeDataGridViewTextBoxColumn
            // 
            this.sizeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sizeDataGridViewTextBoxColumn.DataPropertyName = "Size";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sizeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.sizeDataGridViewTextBoxColumn.HeaderText = "Размер";
            this.sizeDataGridViewTextBoxColumn.Name = "sizeDataGridViewTextBoxColumn";
            this.sizeDataGridViewTextBoxColumn.ReadOnly = true;
            this.sizeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // structsDataSet
            // 
            this.structsDataSet.DataSetName = "StructsDataSet";
            this.structsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // clb_structurs
            // 
            this.clb_structurs.Dock = System.Windows.Forms.DockStyle.Left;
            this.clb_structurs.FormattingEnabled = true;
            this.clb_structurs.Location = new System.Drawing.Point(3, 16);
            this.clb_structurs.Name = "clb_structurs";
            this.clb_structurs.Size = new System.Drawing.Size(120, 364);
            this.clb_structurs.TabIndex = 1;
            this.clb_structurs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clb_structurs_ItemCheck);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "StructName";
            this.dataGridViewTextBoxColumn2.HeaderText = "StructName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // displayValueDataGridViewTextBoxColumn
            // 
            this.displayValueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.displayValueDataGridViewTextBoxColumn.DataPropertyName = "DisplayValue";
            this.displayValueDataGridViewTextBoxColumn.HeaderText = "DisplayValue";
            this.displayValueDataGridViewTextBoxColumn.Name = "displayValueDataGridViewTextBoxColumn";
            this.displayValueDataGridViewTextBoxColumn.ReadOnly = true;
            this.displayValueDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ViewStructursTabbedDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.borderPanel1);
            this.Name = "ViewStructursTabbedDocument";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "ViewStructursTabbedDocument";
            this.Load += new System.EventHandler(this.ViewStructursTabbedDocument_Load);
            this.LocationChanged += new System.EventHandler(this.ViewStructursTabbedDocument_Leave);
            this.VisibleChanged += new System.EventHandler(this.ViewStructursTabbedDocument_Leave);
            this.Closed += new System.EventHandler(this.ViewStructursTabbedDocument_Closed);
            this.Leave += new System.EventHandler(this.ViewStructursTabbedDocument_Leave);
            this.Enter += new System.EventHandler(this.ViewStructursTabbedDocument_Leave);
            this.SizeChanged += new System.EventHandler(this.ViewStructursTabbedDocument_Leave);
            this.borderPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_askStructs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.structsDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Kontel.Relkon.BorderPanel borderPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgv_askStructs;
        private System.Windows.Forms.CheckedListBox clb_structurs;
        private Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewStructursTabbedDocument.StructsDataSet structsDataSet;
        //private System.Windows.Forms.DataGridViewTextBoxColumn MemoryType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn structNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayValueDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn memoryTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
    }
}