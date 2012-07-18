namespace Kontel.Relkon.Components.Documents
{
    partial class ViewMemoryTabbedDocument
    {
        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.borderPanel1 = new Kontel.Relkon.BorderPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.borderPanel2 = new Kontel.Relkon.BorderPanel();
            this.heMemory = new Kontel.Relkon.HexEditior();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gMainSettings = new System.Windows.Forms.GroupBox();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.ddlMemoryType = new System.Windows.Forms.ComboBox();
            this.bWrite = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbStartAddress = new System.Windows.Forms.TextBox();
            this.bTimerRead = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.bRead = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbReadingSize = new System.Windows.Forms.TextBox();
            this.borderPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.borderPanel2.SuspendLayout();
            this.gMainSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // borderPanel1
            // 
            this.borderPanel1.AutoScroll = true;
            this.borderPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.borderPanel1.Controls.Add(this.groupBox1);
            this.borderPanel1.Controls.Add(this.panel1);
            this.borderPanel1.Controls.Add(this.gMainSettings);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel1.Location = new System.Drawing.Point(5, 5);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.borderPanel1.Size = new System.Drawing.Size(842, 751);
            this.borderPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.borderPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(231, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(606, 741);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Содержимое памяти";
            // 
            // borderPanel2
            // 
            this.borderPanel2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.borderPanel2.Controls.Add(this.heMemory);
            this.borderPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel2.Location = new System.Drawing.Point(5, 18);
            this.borderPanel2.Name = "borderPanel2";
            this.borderPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.borderPanel2.Size = new System.Drawing.Size(596, 718);
            this.borderPanel2.TabIndex = 1;
            // 
            // heMemory
            // 
            this.heMemory.AddressColor = System.Drawing.SystemColors.WindowText;
            this.heMemory.BackColor = System.Drawing.SystemColors.Window;
            this.heMemory.CodeColor = System.Drawing.SystemColors.WindowText;
            this.heMemory.CodingType = 16;
            this.heMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.heMemory.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.heMemory.Location = new System.Drawing.Point(1, 1);
            this.heMemory.Name = "heMemory";
            this.heMemory.PresentationColor = System.Drawing.SystemColors.WindowText;
            this.heMemory.SegmentSize = 16;
            this.heMemory.Size = new System.Drawing.Size(594, 716);
            this.heMemory.TabIndex = 0;
            this.heMemory.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Memory_Scroll);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(226, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 741);
            this.panel1.TabIndex = 28;
            // 
            // gMainSettings
            // 
            this.gMainSettings.BackColor = System.Drawing.SystemColors.Control;
            this.gMainSettings.Controls.Add(this.pbStatus);
            this.gMainSettings.Controls.Add(this.ddlMemoryType);
            this.gMainSettings.Controls.Add(this.bWrite);
            this.gMainSettings.Controls.Add(this.label1);
            this.gMainSettings.Controls.Add(this.tbStartAddress);
            this.gMainSettings.Controls.Add(this.bTimerRead);
            this.gMainSettings.Controls.Add(this.label3);
            this.gMainSettings.Controls.Add(this.bRead);
            this.gMainSettings.Controls.Add(this.label2);
            this.gMainSettings.Controls.Add(this.tbReadingSize);
            this.gMainSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.gMainSettings.Location = new System.Drawing.Point(5, 5);
            this.gMainSettings.Name = "gMainSettings";
            this.gMainSettings.Size = new System.Drawing.Size(221, 741);
            this.gMainSettings.TabIndex = 26;
            this.gMainSettings.TabStop = false;
            this.gMainSettings.Text = "Параметры опроса";
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(6, 175);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(207, 23);
            this.pbStatus.TabIndex = 27;
            this.pbStatus.Visible = false;
            // 
            // ddlMemoryType
            // 
            this.ddlMemoryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlMemoryType.FormattingEnabled = true;
            this.ddlMemoryType.Items.AddRange(new object[] {
            "Clock",
            "RAM",
            "FRAM",
            "XRAM"});
            this.ddlMemoryType.Location = new System.Drawing.Point(6, 38);
            this.ddlMemoryType.Name = "ddlMemoryType";
            this.ddlMemoryType.Size = new System.Drawing.Size(207, 21);
            this.ddlMemoryType.TabIndex = 6;
            this.ddlMemoryType.SelectedValueChanged += new System.EventHandler(this.MemoryType_SelectedValueChanged);
            this.ddlMemoryType.TextChanged += new System.EventHandler(this.Data_TextChanged);
            // 
            // bWrite
            // 
            this.bWrite.Enabled = false;
            this.bWrite.Location = new System.Drawing.Point(6, 204);
            this.bWrite.Name = "bWrite";
            this.bWrite.Size = new System.Drawing.Size(207, 23);
            this.bWrite.TabIndex = 23;
            this.bWrite.Text = "Запись";
            this.bWrite.UseVisualStyleBackColor = true;
            this.bWrite.Click += new System.EventHandler(this.bWrite_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Начальный адрес";
            // 
            // tbStartAddress
            // 
            this.tbStartAddress.BackColor = System.Drawing.SystemColors.Window;
            this.tbStartAddress.Location = new System.Drawing.Point(6, 79);
            this.tbStartAddress.Name = "tbStartAddress";
            this.tbStartAddress.Size = new System.Drawing.Size(207, 20);
            this.tbStartAddress.TabIndex = 2;
            this.tbStartAddress.Text = "0";
            this.tbStartAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbStartAddress_KeyDown);
            this.tbStartAddress.Leave += new System.EventHandler(this.StartAddress_Leave);
            // 
            // bTimerRead
            // 
            this.bTimerRead.Location = new System.Drawing.Point(89, 146);
            this.bTimerRead.Name = "bTimerRead";
            this.bTimerRead.Size = new System.Drawing.Size(124, 23);
            this.bTimerRead.TabIndex = 15;
            this.bTimerRead.Text = "Циклическое чтение";
            this.bTimerRead.UseVisualStyleBackColor = true;
            this.bTimerRead.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Тип памяти";
            // 
            // bRead
            // 
            this.bRead.Enabled = false;
            this.bRead.Location = new System.Drawing.Point(6, 146);
            this.bRead.Name = "bRead";
            this.bRead.Size = new System.Drawing.Size(77, 23);
            this.bRead.TabIndex = 1;
            this.bRead.Text = "Чтение";
            this.bRead.UseVisualStyleBackColor = true;
            this.bRead.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Число байт";
            // 
            // tbReadingSize
            // 
            this.tbReadingSize.BackColor = System.Drawing.SystemColors.Window;
            this.tbReadingSize.Location = new System.Drawing.Point(6, 120);
            this.tbReadingSize.Name = "tbReadingSize";
            this.tbReadingSize.Size = new System.Drawing.Size(207, 20);
            this.tbReadingSize.TabIndex = 4;
            this.tbReadingSize.Text = "8";
            this.tbReadingSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbReadingSize_KeyDown);
            this.tbReadingSize.Leave += new System.EventHandler(this.ReadingSize_Leave);
            // 
            // ViewMemoryTabbedDocument
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.borderPanel1);
            this.Name = "ViewMemoryTabbedDocument";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(852, 761);
            this.Load += new System.EventHandler(this.ViewMemoryTabbedDocument_Load);
            this.LocationChanged += new System.EventHandler(this.ViewMemoryTabbedDocument_VisibleChanged);
            this.VisibleChanged += new System.EventHandler(this.ViewMemoryTabbedDocument_VisibleChanged);
            this.Closed += new System.EventHandler(this.ViewMemoryTabbedDocument_Closed);
            this.Leave += new System.EventHandler(this.ViewMemoryTabbedDocument_VisibleChanged);
            this.Enter += new System.EventHandler(this.ViewMemoryTabbedDocument_VisibleChanged);
            this.borderPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.borderPanel2.ResumeLayout(false);
            this.gMainSettings.ResumeLayout(false);
            this.gMainSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Kontel.Relkon.BorderPanel borderPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Kontel.Relkon.HexEditior heMemory;
        private System.Windows.Forms.GroupBox gMainSettings;
        private System.Windows.Forms.ProgressBar pbStatus;
        private System.Windows.Forms.ComboBox ddlMemoryType;
        private System.Windows.Forms.Button bWrite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbStartAddress;
        private System.Windows.Forms.Button bTimerRead;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bRead;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbReadingSize;
        private System.Windows.Forms.Panel panel1;
        private Kontel.Relkon.BorderPanel borderPanel2;
    }
}
