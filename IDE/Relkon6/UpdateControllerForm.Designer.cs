namespace Kontel.Relkon
{
    partial class UpdateControllerForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.bBrowse = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgSolutions = new System.Windows.Forms.DataGridView();
            this.SolutionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSolutions)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bBrowse);
            this.panel1.Controls.Add(this.bOk);
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(5, 294);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(463, 32);
            this.panel1.TabIndex = 0;
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(3, 6);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(75, 23);
            this.bBrowse.TabIndex = 2;
            this.bBrowse.Text = "Обзор...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(298, 6);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 1;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(379, 6);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 0;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgSolutions);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
            this.groupBox1.Size = new System.Drawing.Size(463, 294);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Доступные проекты";
            // 
            // dgSolutions
            // 
            this.dgSolutions.AllowUserToAddRows = false;
            this.dgSolutions.AllowUserToDeleteRows = false;
            this.dgSolutions.AllowUserToResizeRows = false;
            this.dgSolutions.BackgroundColor = System.Drawing.Color.White;
            this.dgSolutions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgSolutions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgSolutions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSolutions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SolutionName,
            this.Path});
            this.dgSolutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSolutions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgSolutions.GridColor = System.Drawing.Color.White;
            this.dgSolutions.Location = new System.Drawing.Point(5, 16);
            this.dgSolutions.MultiSelect = false;
            this.dgSolutions.Name = "dgSolutions";
            this.dgSolutions.RowHeadersVisible = false;
            this.dgSolutions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSolutions.Size = new System.Drawing.Size(453, 273);
            this.dgSolutions.TabIndex = 0;
            this.dgSolutions.SelectionChanged += new System.EventHandler(this.dgSolutions_SelectionChanged);
            this.dgSolutions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgSolutions_MouseDoubleClick);
            // 
            // SolutionName
            // 
            this.SolutionName.HeaderText = "Имя проекта";
            this.SolutionName.Name = "SolutionName";
            this.SolutionName.Width = 150;
            // 
            // Path
            // 
            this.Path.HeaderText = "Путь к файлу";
            this.Path.Name = "Path";
            this.Path.Width = 300;
            // 
            // UpdateControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 326);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateControllerForm";
            this.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обновленне данных контроллера";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSolutions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgSolutions;
        private System.Windows.Forms.DataGridViewTextBoxColumn SolutionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;


    }
}