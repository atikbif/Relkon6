namespace Reloader
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ssLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.ssStopBtn = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.logoPB = new System.Windows.Forms.PictureBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.gBBuffer = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cBUseConfig = new System.Windows.Forms.CheckBox();
            this.cBUseProgram = new System.Windows.Forms.CheckBox();
            this.lConfig = new System.Windows.Forms.Label();
            this.lProgram = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.mmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiOpenProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiOpenConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiSaveProgramAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiSaveConfigurationAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.операцииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mmiClearBuffer = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.cBPortNumber = new System.Windows.Forms.ComboBox();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPB)).BeginInit();
            this.gBBuffer.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssLabel
            // 
            this.ssLabel.AutoSize = false;
            this.ssLabel.Name = "ssLabel";
            this.ssLabel.Size = new System.Drawing.Size(270, 17);
            this.ssLabel.Text = "Готово";
            this.ssLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ssLabel.ToolTipText = "Статус приложения";
            // 
            // ssProgressBar
            // 
            this.ssProgressBar.AutoSize = false;
            this.ssProgressBar.Name = "ssProgressBar";
            this.ssProgressBar.Size = new System.Drawing.Size(150, 16);
            this.ssProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ssProgressBar.Visible = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssLabel,
            this.ssProgressBar,
            this.ssStopBtn});
            this.statusStrip.Location = new System.Drawing.Point(0, 124);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(484, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // ssStopBtn
            // 
            this.ssStopBtn.BackColor = System.Drawing.SystemColors.ControlText;
            this.ssStopBtn.ForeColor = System.Drawing.Color.White;
            this.ssStopBtn.Name = "ssStopBtn";
            this.ssStopBtn.Size = new System.Drawing.Size(62, 17);
            this.ssStopBtn.Text = "Стоп (Esc)";
            this.ssStopBtn.Visible = false;
            this.ssStopBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ssStopBtn_MouseDown);
            // 
            // btnUpload
            // 
            this.btnUpload.Enabled = false;
            this.btnUpload.Location = new System.Drawing.Point(235, 32);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(145, 26);
            this.btnUpload.TabIndex = 4;
            this.btnUpload.Text = "Прошить в контроллер";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // logoPB
            // 
            this.logoPB.Dock = System.Windows.Forms.DockStyle.Right;
            this.logoPB.Image = ((System.Drawing.Image)(resources.GetObject("logoPB.Image")));
            this.logoPB.Location = new System.Drawing.Point(384, 24);
            this.logoPB.Name = "logoPB";
            this.logoPB.Size = new System.Drawing.Size(100, 100);
            this.logoPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPB.TabIndex = 5;
            this.logoPB.TabStop = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Enabled = false;
            this.btnDownload.Location = new System.Drawing.Point(235, 64);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(145, 26);
            this.btnDownload.TabIndex = 6;
            this.btnDownload.Text = "Прочитать с контроллера";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // gBBuffer
            // 
            this.gBBuffer.Controls.Add(this.label3);
            this.gBBuffer.Controls.Add(this.cBUseConfig);
            this.gBBuffer.Controls.Add(this.cBUseProgram);
            this.gBBuffer.Controls.Add(this.lConfig);
            this.gBBuffer.Controls.Add(this.lProgram);
            this.gBBuffer.Controls.Add(this.label2);
            this.gBBuffer.Controls.Add(this.label1);
            this.gBBuffer.Location = new System.Drawing.Point(6, 27);
            this.gBBuffer.Name = "gBBuffer";
            this.gBBuffer.Size = new System.Drawing.Size(224, 90);
            this.gBBuffer.TabIndex = 7;
            this.gBBuffer.TabStop = false;
            this.gBBuffer.Text = "Буфер";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(113, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Использовать:";
            // 
            // cBUseConfig
            // 
            this.cBUseConfig.AutoSize = true;
            this.cBUseConfig.Location = new System.Drawing.Point(188, 64);
            this.cBUseConfig.Name = "cBUseConfig";
            this.cBUseConfig.Size = new System.Drawing.Size(15, 14);
            this.cBUseConfig.TabIndex = 5;
            this.cBUseConfig.UseVisualStyleBackColor = true;
            this.cBUseConfig.CheckedChanged += new System.EventHandler(this.cBUseConfig_CheckedChanged);
            // 
            // cBUseProgram
            // 
            this.cBUseProgram.AutoSize = true;
            this.cBUseProgram.Location = new System.Drawing.Point(188, 33);
            this.cBUseProgram.Name = "cBUseProgram";
            this.cBUseProgram.Size = new System.Drawing.Size(15, 14);
            this.cBUseProgram.TabIndex = 4;
            this.cBUseProgram.UseVisualStyleBackColor = true;
            this.cBUseProgram.CheckedChanged += new System.EventHandler(this.cBUseProgram_CheckedChanged);
            // 
            // lConfig
            // 
            this.lConfig.AutoSize = true;
            this.lConfig.ForeColor = System.Drawing.Color.Red;
            this.lConfig.Location = new System.Drawing.Point(7, 64);
            this.lConfig.Name = "lConfig";
            this.lConfig.Size = new System.Drawing.Size(37, 13);
            this.lConfig.TabIndex = 3;
            this.lConfig.Text = "Пусто";
            // 
            // lProgram
            // 
            this.lProgram.AutoSize = true;
            this.lProgram.ForeColor = System.Drawing.Color.Red;
            this.lProgram.Location = new System.Drawing.Point(7, 33);
            this.lProgram.Name = "lProgram";
            this.lProgram.Size = new System.Drawing.Size(37, 13);
            this.lProgram.TabIndex = 2;
            this.lProgram.Text = "Пусто";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Конфигурация:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Программа:";
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiFile,
            this.операцииToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(484, 24);
            this.mainMenuStrip.TabIndex = 8;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // mmiFile
            // 
            this.mmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiOpenProgram,
            this.mmiOpenConfiguration,
            this.mmiSaveProgramAs,
            this.mmiSaveConfigurationAs,
            this.toolStripSeparator1,
            this.mmiExit});
            this.mmiFile.Name = "mmiFile";
            this.mmiFile.Size = new System.Drawing.Size(48, 20);
            this.mmiFile.Text = "&Файл";
            // 
            // mmiOpenProgram
            // 
            this.mmiOpenProgram.Name = "mmiOpenProgram";
            this.mmiOpenProgram.Size = new System.Drawing.Size(249, 22);
            this.mmiOpenProgram.Text = "О&ткрыть программу";
            this.mmiOpenProgram.Click += new System.EventHandler(this.mmiOpenProgram_Click);
            // 
            // mmiOpenConfiguration
            // 
            this.mmiOpenConfiguration.Name = "mmiOpenConfiguration";
            this.mmiOpenConfiguration.Size = new System.Drawing.Size(249, 22);
            this.mmiOpenConfiguration.Text = "Открыть &конфигурацию";
            this.mmiOpenConfiguration.Click += new System.EventHandler(this.mmiOpenConfiguration_Click);
            // 
            // mmiSaveProgramAs
            // 
            this.mmiSaveProgramAs.Enabled = false;
            this.mmiSaveProgramAs.Name = "mmiSaveProgramAs";
            this.mmiSaveProgramAs.Size = new System.Drawing.Size(249, 22);
            this.mmiSaveProgramAs.Text = "С&охранить программу как...";
            this.mmiSaveProgramAs.Click += new System.EventHandler(this.mmiSaveProgramAs_Click);
            // 
            // mmiSaveConfigurationAs
            // 
            this.mmiSaveConfigurationAs.Enabled = false;
            this.mmiSaveConfigurationAs.Name = "mmiSaveConfigurationAs";
            this.mmiSaveConfigurationAs.Size = new System.Drawing.Size(249, 22);
            this.mmiSaveConfigurationAs.Text = "Сохран&ить конфигурацию как...";
            this.mmiSaveConfigurationAs.Click += new System.EventHandler(this.mmiSaveConfigurationAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(246, 6);
            // 
            // mmiExit
            // 
            this.mmiExit.Name = "mmiExit";
            this.mmiExit.Size = new System.Drawing.Size(249, 22);
            this.mmiExit.Text = "В&ыход";
            this.mmiExit.Click += new System.EventHandler(this.mmiExit_Click);
            // 
            // операцииToolStripMenuItem
            // 
            this.операцииToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiUpload,
            this.mmiDownload,
            this.toolStripSeparator2,
            this.mmiClearBuffer});
            this.операцииToolStripMenuItem.Name = "операцииToolStripMenuItem";
            this.операцииToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.операцииToolStripMenuItem.Text = "&Операции";
            // 
            // mmiUpload
            // 
            this.mmiUpload.Enabled = false;
            this.mmiUpload.Name = "mmiUpload";
            this.mmiUpload.Size = new System.Drawing.Size(164, 22);
            this.mmiUpload.Text = "&Записать";
            this.mmiUpload.ToolTipText = "Записать в контроллер";
            // 
            // mmiDownload
            // 
            this.mmiDownload.Enabled = false;
            this.mmiDownload.Name = "mmiDownload";
            this.mmiDownload.Size = new System.Drawing.Size(164, 22);
            this.mmiDownload.Text = "&Считать";
            this.mmiDownload.ToolTipText = "Считать с контроллера";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(161, 6);
            // 
            // mmiClearBuffer
            // 
            this.mmiClearBuffer.Enabled = false;
            this.mmiClearBuffer.Name = "mmiClearBuffer";
            this.mmiClearBuffer.Size = new System.Drawing.Size(164, 22);
            this.mmiClearBuffer.Text = "&Очистить буфер";
            this.mmiClearBuffer.ToolTipText = "Очистить буфер программы";
            this.mmiClearBuffer.Click += new System.EventHandler(this.mmiClearBuffer_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "&Справка";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(254, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Порт:";
            // 
            // cBPortNumber
            // 
            this.cBPortNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBPortNumber.Location = new System.Drawing.Point(314, 93);
            this.cBPortNumber.Name = "cBPortNumber";
            this.cBPortNumber.Size = new System.Drawing.Size(64, 21);
            this.cBPortNumber.TabIndex = 10;
            this.cBPortNumber.DropDownClosed += new System.EventHandler(this.cBPortNumber_DropDownClosed);
            this.cBPortNumber.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cBPortNumber_MouseDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 146);
            this.Controls.Add(this.cBPortNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.gBBuffer);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.logoPB);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kontel Reloader";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPB)).EndInit();
            this.gBBuffer.ResumeLayout(false);
            this.gBBuffer.PerformLayout();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel ssLabel;
        private System.Windows.Forms.ToolStripProgressBar ssProgressBar;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ssStopBtn;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.PictureBox logoPB;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.GroupBox gBBuffer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lConfig;
        private System.Windows.Forms.Label lProgram;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mmiFile;
        private System.Windows.Forms.ToolStripMenuItem mmiExit;
        private System.Windows.Forms.ToolStripMenuItem операцииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mmiUpload;
        private System.Windows.Forms.ToolStripMenuItem mmiDownload;
        private System.Windows.Forms.ToolStripMenuItem mmiClearBuffer;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mmiOpenProgram;
        private System.Windows.Forms.ToolStripMenuItem mmiOpenConfiguration;
        private System.Windows.Forms.ToolStripMenuItem mmiSaveProgramAs;
        private System.Windows.Forms.ToolStripMenuItem mmiSaveConfigurationAs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cBUseConfig;
        private System.Windows.Forms.CheckBox cBUseProgram;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cBPortNumber;
    }
}

