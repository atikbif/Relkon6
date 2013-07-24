namespace Kontel.Relkon.Components
{
    partial class DebuggerParametersPanel
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label14;
            System.Windows.Forms.Label label6;
            this.RefreshFormTimer = new System.Windows.Forms.Timer(this.components);
            this.borderPanel1 = new Kontel.Relkon.BorderPanel();         
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bSyncTimeWithPC = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.bLoadConfig = new System.Windows.Forms.Button();
            this.bSaveConfig = new System.Windows.Forms.Button();
            this.gbSettingsConnect = new System.Windows.Forms.GroupBox();
            this.ddlProtocol = new System.Windows.Forms.ComboBox();
            this.ddlBaudRate = new System.Windows.Forms.ComboBox();
            this.bRefreshPortNames = new System.Windows.Forms.Button();
            this.nudControllerAddress = new System.Windows.Forms.NumericUpDown();
            this.ddlPortName = new System.Windows.Forms.ComboBox();
            this.bAutoScan = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.bpState = new Kontel.Relkon.BorderPanel();
            this.lEngineStatus = new System.Windows.Forms.Label();
            this.bpErrorReadPacket = new Kontel.Relkon.BorderPanel();
            this.lErrorReadPacket = new System.Windows.Forms.Label();
            this.bpErrorWritePacket = new Kontel.Relkon.BorderPanel();
            this.lErrorWritePacket = new System.Windows.Forms.Label();
            this.bpReadPacket = new Kontel.Relkon.BorderPanel();
            this.lReadPacket = new System.Windows.Forms.Label();
            this.bpWritePacket = new Kontel.Relkon.BorderPanel();
            this.lWritePacket = new System.Windows.Forms.Label();
            this.lRequestTime = new System.Windows.Forms.Label();
            this.bStart = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            this.borderPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbSettingsConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudControllerAddress)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.bpState.SuspendLayout();
            this.bpErrorReadPacket.SuspendLayout();
            this.bpErrorWritePacket.SuspendLayout();
            this.bpReadPacket.SuspendLayout();
            this.bpWritePacket.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            label1.Location = new System.Drawing.Point(14, 76);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(62, 13);
            label1.TabIndex = 1;
            label1.Text = "COM-Порт:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            label4.Location = new System.Drawing.Point(9, 43);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(59, 13);
            label4.TabIndex = 10;
            label4.Text = "Протокол:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            label5.Location = new System.Drawing.Point(9, 21);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(85, 13);
            label5.TabIndex = 12;
            label5.Text = "Сетевой адрес:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            label2.Location = new System.Drawing.Point(14, 103);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(58, 13);
            label2.TabIndex = 2;
            label2.Text = "Скорость:";
            // 
            // label10
            // 
            label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            label10.Location = new System.Drawing.Point(18, 76);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(198, 19);
            label10.TabIndex = 10;
            label10.Text = "Записано пакетов";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            label8.Location = new System.Drawing.Point(50, 118);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(136, 13);
            label8.TabIndex = 8;
            label8.Text = "Статус работы отладчика";
            // 
            // label14
            // 
            label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            label14.Location = new System.Drawing.Point(21, 35);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(196, 19);
            label14.TabIndex = 11;
            label14.Text = "Прочитано пакетов";
            label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            label6.Location = new System.Drawing.Point(9, 16);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(87, 13);
            label6.TabIndex = 5;
            label6.Text = "Время отклика:";
            // 
            // RefreshFormTimer
            // 
            this.RefreshFormTimer.Tick += new System.EventHandler(this.RefreshFormTimer_Tick);
            // 
            // borderPanel1
            // 
            this.borderPanel1.AutoScroll = true;
            this.borderPanel1.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));           
            this.borderPanel1.Controls.Add(this.groupBox2);
            this.borderPanel1.Controls.Add(this.bLoadConfig);
            this.borderPanel1.Controls.Add(this.bSaveConfig);
            this.borderPanel1.Controls.Add(this.gbSettingsConnect);
            this.borderPanel1.Controls.Add(this.groupBox3);
            this.borderPanel1.Controls.Add(this.bStart);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel1.Location = new System.Drawing.Point(2, 2);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.borderPanel1.Size = new System.Drawing.Size(266, 719);
            this.borderPanel1.TabIndex = 28;        
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bSyncTimeWithPC);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Location = new System.Drawing.Point(9, 203);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 89);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Дата-время";
            // 
            // bSyncTimeWithPC
            // 
            this.bSyncTimeWithPC.Enabled = false;
            this.bSyncTimeWithPC.Location = new System.Drawing.Point(18, 56);
            this.bSyncTimeWithPC.Name = "bSyncTimeWithPC";
            this.bSyncTimeWithPC.Size = new System.Drawing.Size(202, 23);
            this.bSyncTimeWithPC.TabIndex = 30;
            this.bSyncTimeWithPC.Text = "Синхронизировать";
            this.bSyncTimeWithPC.UseVisualStyleBackColor = true;
            this.bSyncTimeWithPC.Click += new System.EventHandler(this.bSyncTimeWithPC_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Checked = false;
            this.dateTimePicker1.CustomFormat = "dd MMMM HH:mm:ss   yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(18, 30);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(202, 20);
            this.dateTimePicker1.TabIndex = 29;
            this.dateTimePicker1.Enter += new System.EventHandler(this.dateTimePicker1_Enter);
            this.dateTimePicker1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker1_KeyDown);
            this.dateTimePicker1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker1_KeyUp);
            this.dateTimePicker1.Leave += new System.EventHandler(this.dateTimePicker1_Leave);
            this.dateTimePicker1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dateTimePicker1_MouseDown);
            // 
            // bLoadConfig
            // 
            this.bLoadConfig.Location = new System.Drawing.Point(8, 461);
            this.bLoadConfig.Name = "bLoadConfig";
            this.bLoadConfig.Size = new System.Drawing.Size(117, 34);
            this.bLoadConfig.TabIndex = 22;
            this.bLoadConfig.Text = "Загрузить конфигурацию";
            this.bLoadConfig.UseVisualStyleBackColor = true;
            this.bLoadConfig.Click += new System.EventHandler(this.bLoadConfig_Click);
            // 
            // bSaveConfig
            // 
            this.bSaveConfig.Location = new System.Drawing.Point(123, 461);
            this.bSaveConfig.Name = "bSaveConfig";
            this.bSaveConfig.Size = new System.Drawing.Size(116, 34);
            this.bSaveConfig.TabIndex = 13;
            this.bSaveConfig.Text = "Сохранить конфигурацию";
            this.bSaveConfig.UseVisualStyleBackColor = true;
            this.bSaveConfig.Click += new System.EventHandler(this.bSaveConfig_Click);
            // 
            // gbSettingsConnect
            // 
            this.gbSettingsConnect.Controls.Add(label1);
            this.gbSettingsConnect.Controls.Add(this.ddlProtocol);
            this.gbSettingsConnect.Controls.Add(this.ddlBaudRate);
            this.gbSettingsConnect.Controls.Add(label4);
            this.gbSettingsConnect.Controls.Add(label5);
            this.gbSettingsConnect.Controls.Add(this.bRefreshPortNames);
            this.gbSettingsConnect.Controls.Add(this.nudControllerAddress);
            this.gbSettingsConnect.Controls.Add(this.ddlPortName);
            this.gbSettingsConnect.Controls.Add(this.bAutoScan);
            this.gbSettingsConnect.Controls.Add(label2);
            this.gbSettingsConnect.Location = new System.Drawing.Point(8, 4);
            this.gbSettingsConnect.Name = "gbSettingsConnect";
            this.gbSettingsConnect.Size = new System.Drawing.Size(230, 161);
            this.gbSettingsConnect.TabIndex = 26;
            this.gbSettingsConnect.TabStop = false;
            this.gbSettingsConnect.Text = "Настройки соединения";
            // 
            // ddlProtocol
            // 
            this.ddlProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlProtocol.FormattingEnabled = true;
            this.ddlProtocol.Location = new System.Drawing.Point(107, 40);
            this.ddlProtocol.Name = "ddlProtocol";
            this.ddlProtocol.Size = new System.Drawing.Size(112, 21);
            this.ddlProtocol.TabIndex = 11;
            this.ddlProtocol.SelectedIndexChanged += new System.EventHandler(this.ddlProtocol_SelectedIndexChanged);
            // 
            // ddlBaudRate
            // 
            this.ddlBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlBaudRate.FormattingEnabled = true;
            this.ddlBaudRate.Items.AddRange(new object[] {
            "115200",
            "57600",
            "38400",
            "28800",
            "19200",
            "9600",
            "4800",
            "2400",
            "1200"});
            this.ddlBaudRate.Location = new System.Drawing.Point(90, 100);
            this.ddlBaudRate.Name = "ddlBaudRate";
            this.ddlBaudRate.Size = new System.Drawing.Size(128, 21);
            this.ddlBaudRate.TabIndex = 8;
            this.ddlBaudRate.SelectedIndexChanged += new System.EventHandler(this.ddlBaudRate_SelectedIndexChanged);
            // 
            // bRefreshPortNames
            // 
            this.bRefreshPortNames.Location = new System.Drawing.Point(155, 72);
            this.bRefreshPortNames.Name = "bRefreshPortNames";
            this.bRefreshPortNames.Size = new System.Drawing.Size(64, 23);
            this.bRefreshPortNames.TabIndex = 21;
            this.bRefreshPortNames.Text = "Обновить";
            this.bRefreshPortNames.UseVisualStyleBackColor = true;
            this.bRefreshPortNames.Click += new System.EventHandler(this.bRefreshPortNames_Click);
            // 
            // nudControllerAddress
            // 
            this.nudControllerAddress.Location = new System.Drawing.Point(107, 14);
            this.nudControllerAddress.Name = "nudControllerAddress";
            this.nudControllerAddress.Size = new System.Drawing.Size(111, 20);
            this.nudControllerAddress.TabIndex = 13;
            this.nudControllerAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudControllerAddress.ValueChanged += new System.EventHandler(this.nudControllerAddress_ValueChanged);
            // 
            // ddlPortName
            // 
            this.ddlPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPortName.FormattingEnabled = true;
            this.ddlPortName.Location = new System.Drawing.Point(90, 73);
            this.ddlPortName.Name = "ddlPortName";
            this.ddlPortName.Size = new System.Drawing.Size(61, 21);
            this.ddlPortName.TabIndex = 6;
            this.ddlPortName.SelectedIndexChanged += new System.EventHandler(this.ddPortName_SelectedIndexChanged);
            // 
            // bAutoScan
            // 
            this.bAutoScan.Location = new System.Drawing.Point(90, 125);
            this.bAutoScan.Name = "bAutoScan";
            this.bAutoScan.Size = new System.Drawing.Size(129, 23);
            this.bAutoScan.TabIndex = 7;
            this.bAutoScan.Text = "Автоопределение";
            this.bAutoScan.UseVisualStyleBackColor = true;
            this.bAutoScan.Click += new System.EventHandler(this.bAutoScan_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bpState);
            this.groupBox3.Controls.Add(this.bpErrorReadPacket);
            this.groupBox3.Controls.Add(this.bpErrorWritePacket);
            this.groupBox3.Controls.Add(this.bpReadPacket);
            this.groupBox3.Controls.Add(this.bpWritePacket);
            this.groupBox3.Controls.Add(label10);
            this.groupBox3.Controls.Add(label8);
            this.groupBox3.Controls.Add(label14);
            this.groupBox3.Controls.Add(this.lRequestTime);
            this.groupBox3.Controls.Add(label6);
            this.groupBox3.Location = new System.Drawing.Point(8, 298);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(231, 157);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Состояние отладчика";
            // 
            // bpState
            // 
            this.bpState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.bpState.Controls.Add(this.lEngineStatus);
            this.bpState.Location = new System.Drawing.Point(27, 131);
            this.bpState.Name = "bpState";
            this.bpState.Padding = new System.Windows.Forms.Padding(1);
            this.bpState.Size = new System.Drawing.Size(178, 19);
            this.bpState.TabIndex = 20;
            // 
            // lEngineStatus
            // 
            this.lEngineStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lEngineStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lEngineStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(0)))));
            this.lEngineStatus.Location = new System.Drawing.Point(1, 1);
            this.lEngineStatus.Name = "lEngineStatus";
            this.lEngineStatus.Size = new System.Drawing.Size(176, 17);
            this.lEngineStatus.TabIndex = 9;
            this.lEngineStatus.Text = "Остановлен";
            this.lEngineStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bpErrorReadPacket
            // 
            this.bpErrorReadPacket.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.bpErrorReadPacket.Controls.Add(this.lErrorReadPacket);
            this.bpErrorReadPacket.Location = new System.Drawing.Point(116, 57);
            this.bpErrorReadPacket.Name = "bpErrorReadPacket";
            this.bpErrorReadPacket.Padding = new System.Windows.Forms.Padding(1);
            this.bpErrorReadPacket.Size = new System.Drawing.Size(97, 19);
            this.bpErrorReadPacket.TabIndex = 19;
            // 
            // lErrorReadPacket
            // 
            this.lErrorReadPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lErrorReadPacket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lErrorReadPacket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(0)))));
            this.lErrorReadPacket.Location = new System.Drawing.Point(1, 1);
            this.lErrorReadPacket.Name = "lErrorReadPacket";
            this.lErrorReadPacket.Size = new System.Drawing.Size(95, 17);
            this.lErrorReadPacket.TabIndex = 14;
            this.lErrorReadPacket.Text = "00";
            this.lErrorReadPacket.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bpErrorWritePacket
            // 
            this.bpErrorWritePacket.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.bpErrorWritePacket.Controls.Add(this.lErrorWritePacket);
            this.bpErrorWritePacket.Location = new System.Drawing.Point(116, 95);
            this.bpErrorWritePacket.Name = "bpErrorWritePacket";
            this.bpErrorWritePacket.Padding = new System.Windows.Forms.Padding(1);
            this.bpErrorWritePacket.Size = new System.Drawing.Size(97, 19);
            this.bpErrorWritePacket.TabIndex = 17;
            // 
            // lErrorWritePacket
            // 
            this.lErrorWritePacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lErrorWritePacket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lErrorWritePacket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(0)))));
            this.lErrorWritePacket.Location = new System.Drawing.Point(1, 1);
            this.lErrorWritePacket.Name = "lErrorWritePacket";
            this.lErrorWritePacket.Size = new System.Drawing.Size(95, 17);
            this.lErrorWritePacket.TabIndex = 15;
            this.lErrorWritePacket.Text = "00";
            this.lErrorWritePacket.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bpReadPacket
            // 
            this.bpReadPacket.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.bpReadPacket.Controls.Add(this.lReadPacket);
            this.bpReadPacket.Location = new System.Drawing.Point(15, 57);
            this.bpReadPacket.Name = "bpReadPacket";
            this.bpReadPacket.Padding = new System.Windows.Forms.Padding(1);
            this.bpReadPacket.Size = new System.Drawing.Size(100, 19);
            this.bpReadPacket.TabIndex = 18;
            // 
            // lReadPacket
            // 
            this.lReadPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lReadPacket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lReadPacket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.lReadPacket.Location = new System.Drawing.Point(1, 1);
            this.lReadPacket.Name = "lReadPacket";
            this.lReadPacket.Size = new System.Drawing.Size(98, 17);
            this.lReadPacket.TabIndex = 13;
            this.lReadPacket.Text = "00";
            this.lReadPacket.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bpWritePacket
            // 
            this.bpWritePacket.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.bpWritePacket.Controls.Add(this.lWritePacket);
            this.bpWritePacket.Location = new System.Drawing.Point(15, 95);
            this.bpWritePacket.Name = "bpWritePacket";
            this.bpWritePacket.Padding = new System.Windows.Forms.Padding(1);
            this.bpWritePacket.Size = new System.Drawing.Size(100, 19);
            this.bpWritePacket.TabIndex = 16;
            // 
            // lWritePacket
            // 
            this.lWritePacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWritePacket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lWritePacket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.lWritePacket.Location = new System.Drawing.Point(1, 1);
            this.lWritePacket.Name = "lWritePacket";
            this.lWritePacket.Size = new System.Drawing.Size(98, 17);
            this.lWritePacket.TabIndex = 12;
            this.lWritePacket.Text = "00";
            this.lWritePacket.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lRequestTime
            // 
            this.lRequestTime.AutoSize = true;
            this.lRequestTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lRequestTime.Location = new System.Drawing.Point(97, 16);
            this.lRequestTime.Name = "lRequestTime";
            this.lRequestTime.Size = new System.Drawing.Size(36, 13);
            this.lRequestTime.TabIndex = 6;
            this.lRequestTime.Text = "00 мс";
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(8, 174);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(230, 23);
            this.bStart.TabIndex = 2;
            this.bStart.Text = "Старт";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStartEngine_Click);
            // 
            // DebuggerParametersPanel
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.borderPanel1);
            this.DoubleBuffered = true;
            this.FloatingSize = new System.Drawing.Size(661, 50);
            this.MinimumSize = new System.Drawing.Size(255, 50);
            this.Name = "DebuggerParametersPanel";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowOptions = false;
            this.Size = new System.Drawing.Size(270, 723);
            this.Text = "Параметры отладчика";
            this.borderPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.gbSettingsConnect.ResumeLayout(false);
            this.gbSettingsConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudControllerAddress)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.bpState.ResumeLayout(false);
            this.bpErrorReadPacket.ResumeLayout(false);
            this.bpErrorWritePacket.ResumeLayout(false);
            this.bpReadPacket.ResumeLayout(false);
            this.bpWritePacket.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lEngineStatus;
        private System.Windows.Forms.Label lRequestTime;
        private System.Windows.Forms.GroupBox gbSettingsConnect;
        private System.Windows.Forms.Button bSaveConfig;
        private System.Windows.Forms.NumericUpDown nudControllerAddress;
        private System.Windows.Forms.ComboBox ddlProtocol;
        private System.Windows.Forms.Button bAutoScan;
        private System.Windows.Forms.ComboBox ddlPortName;
        private System.Windows.Forms.ComboBox ddlBaudRate;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Label lErrorReadPacket;
        private System.Windows.Forms.Label lReadPacket;
        private System.Windows.Forms.Label lWritePacket;
        private System.Windows.Forms.Label lErrorWritePacket;
        private System.Windows.Forms.Timer RefreshFormTimer;
        private System.Windows.Forms.Button bRefreshPortNames;
        private System.Windows.Forms.Button bLoadConfig;
        private Kontel.Relkon.BorderPanel borderPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private Kontel.Relkon.BorderPanel bpErrorReadPacket;
        private Kontel.Relkon.BorderPanel bpReadPacket;
        private Kontel.Relkon.BorderPanel bpErrorWritePacket;
        private Kontel.Relkon.BorderPanel bpWritePacket;
        private Kontel.Relkon.BorderPanel bpState;  
        private System.Windows.Forms.Button bSyncTimeWithPC;

    }
}
