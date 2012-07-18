namespace Kontel.Relkon.Components.Documents
{
    partial class ViewSituationsTabbedDocument
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.borderPanel1 = new Kontel.Relkon.BorderPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgProcess = new System.Windows.Forms.DataGridView();
            this.cProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSituationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.borderPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // borderPanel1
            // 
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.borderPanel1.Controls.Add(this.groupBox1);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel1.Location = new System.Drawing.Point(5, 5);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.borderPanel1.Size = new System.Drawing.Size(540, 390);
            this.borderPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgProcess);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(530, 380);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Активные ситуации";
            // 
            // dgProcess
            // 
            this.dgProcess.AllowUserToAddRows = false;
            this.dgProcess.AllowUserToDeleteRows = false;
            this.dgProcess.AllowUserToResizeRows = false;
            this.dgProcess.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgProcess.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgProcess.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cProcessName,
            this.cSituationName});
            this.dgProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgProcess.Location = new System.Drawing.Point(5, 18);
            this.dgProcess.Name = "dgProcess";
            this.dgProcess.ReadOnly = true;
            this.dgProcess.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dgProcess.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgProcess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgProcess.Size = new System.Drawing.Size(520, 357);
            this.dgProcess.TabIndex = 0;
            // 
            // cProcessName
            // 
            this.cProcessName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cProcessName.DefaultCellStyle = dataGridViewCellStyle2;
            this.cProcessName.HeaderText = "Процесс";
            this.cProcessName.Name = "cProcessName";
            this.cProcessName.ReadOnly = true;
            // 
            // cSituationName
            // 
            this.cSituationName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSituationName.DefaultCellStyle = dataGridViewCellStyle3;
            this.cSituationName.HeaderText = "Активная ситуация";
            this.cSituationName.Name = "cSituationName";
            this.cSituationName.ReadOnly = true;
            this.cSituationName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ViewSituationsTabbedDocument
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.borderPanel1);
            this.Name = "ViewSituationsTabbedDocument";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Load += new System.EventHandler(this.ViewSituationsTabbedDocument_Load);
            this.LocationChanged += new System.EventHandler(this.ViewSituationsTabbedDocument_VisibleChanged);
            this.VisibleChanged += new System.EventHandler(this.ViewSituationsTabbedDocument_VisibleChanged);
            this.Closed += new System.EventHandler(this.ViewSituationsTabbedDocument_Closed);
            this.Leave += new System.EventHandler(this.ViewSituationsTabbedDocument_VisibleChanged);
            this.Enter += new System.EventHandler(this.ViewSituationsTabbedDocument_VisibleChanged);
            this.borderPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgProcess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Kontel.Relkon.BorderPanel borderPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgProcess;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProcessName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSituationName;

    }
}
