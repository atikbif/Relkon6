namespace Kontel.Relkon.Components.Documents
{
    partial class ViewVarsTabbedDocument
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewVarsTabbedDocument));
            this.borderPanel1 = new Kontel.Relkon.BorderPanel();
            this.gbReadVars = new System.Windows.Forms.GroupBox();
            this.dgVars = new System.Windows.Forms.DataGridView();
            this.varsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.viewVarsDataSet = new Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewVarsTabbedDocument.ViewVarsDataSet();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbUnReadVars = new System.Windows.Forms.GroupBox();
            this.tvVars = new System.Windows.Forms.TreeView();
            this.VarsList = new System.Windows.Forms.ImageList(this.components);
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.memoryTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.borderPanel1.SuspendLayout();
            this.gbReadVars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgVars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.varsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewVarsDataSet)).BeginInit();
            this.gbUnReadVars.SuspendLayout();
            this.SuspendLayout();
            // 
            // borderPanel1
            // 
            this.borderPanel1.AutoScroll = true;
            this.borderPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.borderPanel1.Controls.Add(this.gbReadVars);
            this.borderPanel1.Controls.Add(this.panel1);
            this.borderPanel1.Controls.Add(this.gbUnReadVars);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel1.Location = new System.Drawing.Point(5, 5);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.borderPanel1.Size = new System.Drawing.Size(566, 390);
            this.borderPanel1.TabIndex = 0;
            // 
            // gbReadVars
            // 
            this.gbReadVars.BackColor = System.Drawing.SystemColors.Control;
            this.gbReadVars.Controls.Add(this.dgVars);
            this.gbReadVars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbReadVars.Location = new System.Drawing.Point(257, 5);
            this.gbReadVars.Name = "gbReadVars";
            this.gbReadVars.Padding = new System.Windows.Forms.Padding(5);
            this.gbReadVars.Size = new System.Drawing.Size(304, 380);
            this.gbReadVars.TabIndex = 2;
            this.gbReadVars.TabStop = false;
            this.gbReadVars.Text = "Опрашиваемые переменные";
            // 
            // dgVars
            // 
            this.dgVars.AllowUserToAddRows = false;
            this.dgVars.AllowUserToDeleteRows = false;
            this.dgVars.AllowUserToResizeRows = false;
            this.dgVars.AutoGenerateColumns = false;
            this.dgVars.BackgroundColor = System.Drawing.Color.White;
            this.dgVars.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgVars.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgVars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgVars.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.displayValueDataGridViewTextBoxColumn,
            this.memoryTypeDataGridViewTextBoxColumn,
            this.displayAddressDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn2});
            this.dgVars.DataSource = this.varsBindingSource;
            this.dgVars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgVars.Location = new System.Drawing.Point(5, 18);
            this.dgVars.Name = "dgVars";
            this.dgVars.RowHeadersVisible = false;
            this.dgVars.RowHeadersWidth = 25;
            this.dgVars.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgVars.Size = new System.Drawing.Size(294, 357);
            this.dgVars.TabIndex = 1;
            this.dgVars.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgVars_CellMouseClick);
            this.dgVars.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgVars_CellBeginEdit);
            this.dgVars.Sorted += new System.EventHandler(this.dgVars_Sorted);
            this.dgVars.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgVars_ColumnHeaderMouseClick);
            this.dgVars.DoubleClick += new System.EventHandler(this.dgVars_DoubleClick);
            this.dgVars.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgVars_CellEndEdit);
            this.dgVars.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgVars_CellEnter);
            // 
            // varsBindingSource
            // 
            this.varsBindingSource.DataMember = "Vars";
            this.varsBindingSource.DataSource = this.viewVarsDataSet;
            // 
            // viewVarsDataSet
            // 
            this.viewVarsDataSet.DataSetName = "ViewVarsDataSet";
            this.viewVarsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(252, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 380);
            this.panel1.TabIndex = 3;
            // 
            // gbUnReadVars
            // 
            this.gbUnReadVars.BackColor = System.Drawing.SystemColors.Control;
            this.gbUnReadVars.Controls.Add(this.tvVars);
            this.gbUnReadVars.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbUnReadVars.Location = new System.Drawing.Point(5, 5);
            this.gbUnReadVars.Name = "gbUnReadVars";
            this.gbUnReadVars.Padding = new System.Windows.Forms.Padding(5);
            this.gbUnReadVars.Size = new System.Drawing.Size(247, 380);
            this.gbUnReadVars.TabIndex = 1;
            this.gbUnReadVars.TabStop = false;
            this.gbUnReadVars.Text = "Переменные проекта";
            // 
            // tvVars
            // 
            this.tvVars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvVars.ImageIndex = 1;
            this.tvVars.ImageList = this.VarsList;
            this.tvVars.Location = new System.Drawing.Point(5, 18);
            this.tvVars.Name = "tvVars";
            this.tvVars.SelectedImageIndex = 1;
            this.tvVars.Size = new System.Drawing.Size(237, 357);
            this.tvVars.TabIndex = 0;
            this.tvVars.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvVars_NodeMouseDoubleClick);
            // 
            // VarsList
            // 
            this.VarsList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("VarsList.ImageStream")));
            this.VarsList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.VarsList.Images.SetKeyName(0, "VSObject_Structure.bmp");
            this.VarsList.Images.SetKeyName(1, "VSObject_Field.bmp");
            // 
            // Index
            // 
            this.Index.Name = "Index";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "Имя переменной";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // displayValueDataGridViewTextBoxColumn
            // 
            this.displayValueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.displayValueDataGridViewTextBoxColumn.DataPropertyName = "DisplayValue";
            this.displayValueDataGridViewTextBoxColumn.HeaderText = "Значение";
            this.displayValueDataGridViewTextBoxColumn.Name = "displayValueDataGridViewTextBoxColumn";
            // 
            // memoryTypeDataGridViewTextBoxColumn
            // 
            this.memoryTypeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.memoryTypeDataGridViewTextBoxColumn.DataPropertyName = "MemoryType";
            this.memoryTypeDataGridViewTextBoxColumn.HeaderText = "Тип памяти";
            this.memoryTypeDataGridViewTextBoxColumn.Name = "memoryTypeDataGridViewTextBoxColumn";
            this.memoryTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // displayAddressDataGridViewTextBoxColumn
            // 
            this.displayAddressDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.displayAddressDataGridViewTextBoxColumn.DataPropertyName = "DisplayAddress";
            this.displayAddressDataGridViewTextBoxColumn.HeaderText = "Адрес";
            this.displayAddressDataGridViewTextBoxColumn.Name = "displayAddressDataGridViewTextBoxColumn";
            this.displayAddressDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "VarSize";
            this.dataGridViewTextBoxColumn2.HeaderText = "Размер";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // ViewVarsTabbedDocument
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.borderPanel1);
            this.Name = "ViewVarsTabbedDocument";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(576, 400);
            this.Load += new System.EventHandler(this.ViewVarsTabbedDocument_Load);
            this.LocationChanged += new System.EventHandler(this.ViewVarsTabbedDocument_Leave);
            this.VisibleChanged += new System.EventHandler(this.ViewVarsTabbedDocument_Leave);
            this.Closed += new System.EventHandler(this.ViewVarsTabbedDocument_Closed);
            this.Leave += new System.EventHandler(this.ViewVarsTabbedDocument_Leave);
            this.Enter += new System.EventHandler(this.ViewVarsTabbedDocument_Leave);
            this.SizeChanged += new System.EventHandler(this.ViewVarsTabbedDocument_Leave);
            this.borderPanel1.ResumeLayout(false);
            this.gbReadVars.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgVars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.varsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewVarsDataSet)).EndInit();
            this.gbUnReadVars.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Kontel.Relkon.BorderPanel borderPanel1;
        private System.Windows.Forms.TreeView tvVars;
        private System.Windows.Forms.DataGridView dgVars;
        //private Kontel.Relkon.Components.Documents.ViewVars viewVars;
        //private Kontel.Relkon.Components.Documents.ViewVarsDataSets viewVars;
        private System.Windows.Forms.BindingSource varsBindingSource;
        private System.Windows.Forms.GroupBox gbReadVars;
        private System.Windows.Forms.GroupBox gbUnReadVars;
        private System.Windows.Forms.ImageList VarsList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.Panel panel1;
        //private Kontel.Relkon.Components.Documents.VeiwVarsDataSet veiwVarsDataSet;
        //private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn varSizeDataGridViewTextBoxColumn;
        private Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewVarsTabbedDocument.ViewVarsDataSet viewVarsDataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn memoryTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        //private Kontel.Relkon.Components.Documents.VeiwVarsDataSets veiwVarsDataSets;

    }
}
