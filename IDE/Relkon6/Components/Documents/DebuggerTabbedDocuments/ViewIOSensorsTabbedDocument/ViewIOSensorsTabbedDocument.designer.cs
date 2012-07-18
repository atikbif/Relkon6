namespace Kontel.Relkon.Components.Documents
{
    partial class ViewIOSensorsTabbedDocument
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
            this.digitalIO = new Kontel.Relkon.Components.Documents.DigitalIO();
            this.tabControl1 = new TD.SandDock.TabControl();
            this.tpDigital = new TD.SandDock.TabPage();
            this.borderPanel1 = new Kontel.Relkon.BorderPanel();
            this.gbOutputs = new System.Windows.Forms.GroupBox();
            this.pOutput = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbInputs = new System.Windows.Forms.GroupBox();
            this.pInput = new System.Windows.Forms.Panel();
            this.borderPanel3 = new Kontel.Relkon.BorderPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pSettings = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.cbDefault = new System.Windows.Forms.CheckBox();
            this.borderPanel2 = new Kontel.Relkon.BorderPanel();
            this.tabControl1.SuspendLayout();
            this.tpDigital.SuspendLayout();
            this.borderPanel1.SuspendLayout();
            this.gbOutputs.SuspendLayout();
            this.gbInputs.SuspendLayout();
            this.borderPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pSettings.SuspendLayout();
            this.borderPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // digitalIO
            // 
            this.digitalIO.BackColor = System.Drawing.SystemColors.Control;
            this.digitalIO.EnabledMouseClick = true;
            this.digitalIO.Location = new System.Drawing.Point(1, 0);
            this.digitalIO.Margin = new System.Windows.Forms.Padding(10);
            this.digitalIO.Name = "digitalIO";
            this.digitalIO.Padding = new System.Windows.Forms.Padding(15);
            this.digitalIO.Size = new System.Drawing.Size(226, 142);
            this.digitalIO.TabIndex = 0;
            this.digitalIO.StateChange += new System.EventHandler<Kontel.Relkon.Components.Documents.DigitalIO.StateChangeEventArgs>(this.digitalIO_StateChange);
            this.digitalIO.LabelChange += new System.EventHandler<Kontel.Relkon.Components.Documents.DigitalIO.LabelChangeEventArgs>(this.digitalIO_LabelChange);
            // 
            // tabControl1
            // 
            this.tabControl1.BorderStyle = TD.SandDock.Rendering.BorderStyle.None;
            this.tabControl1.Controls.Add(this.tpDigital);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(5, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Renderer = new TD.SandDock.Rendering.MilborneRenderer();
            this.tabControl1.Size = new System.Drawing.Size(949, 648);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedPageChanged += new System.EventHandler(this.tabControl1_SelectedPageChanged);
            // 
            // tpDigital
            // 
            this.tpDigital.AutoSize = true;
            this.tpDigital.Controls.Add(this.borderPanel1);
            this.tpDigital.Controls.Add(this.borderPanel3);
            this.tpDigital.Location = new System.Drawing.Point(4, 25);
            this.tpDigital.Name = "tpDigital";
            this.tpDigital.Size = new System.Drawing.Size(941, 619);
            this.tpDigital.TabIndex = 0;
            this.tpDigital.Text = "Входы-выходы";
            // 
            // borderPanel1
            // 
            this.borderPanel1.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.borderPanel1.Controls.Add(this.gbOutputs);
            this.borderPanel1.Controls.Add(this.panel1);
            this.borderPanel1.Controls.Add(this.gbInputs);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel1.Location = new System.Drawing.Point(0, 244);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.borderPanel1.Size = new System.Drawing.Size(941, 375);
            this.borderPanel1.TabIndex = 5;
            // 
            // gbOutputs
            // 
            this.gbOutputs.Controls.Add(this.pOutput);
            this.gbOutputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOutputs.Location = new System.Drawing.Point(290, 1);
            this.gbOutputs.Name = "gbOutputs";
            this.gbOutputs.Size = new System.Drawing.Size(650, 373);
            this.gbOutputs.TabIndex = 3;
            this.gbOutputs.TabStop = false;
            this.gbOutputs.Text = "Выходные датчики";
            // 
            // pOutput
            // 
            this.pOutput.AutoScroll = true;
            this.pOutput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pOutput.Location = new System.Drawing.Point(3, 16);
            this.pOutput.Name = "pOutput";
            this.pOutput.Size = new System.Drawing.Size(644, 354);
            this.pOutput.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(285, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 373);
            this.panel1.TabIndex = 4;
            // 
            // gbInputs
            // 
            this.gbInputs.Controls.Add(this.pInput);
            this.gbInputs.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbInputs.Location = new System.Drawing.Point(1, 1);
            this.gbInputs.Name = "gbInputs";
            this.gbInputs.Size = new System.Drawing.Size(284, 373);
            this.gbInputs.TabIndex = 1;
            this.gbInputs.TabStop = false;
            this.gbInputs.Text = "Входные датчики";
            // 
            // pInput
            // 
            this.pInput.AutoScroll = true;
            this.pInput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pInput.Location = new System.Drawing.Point(3, 16);
            this.pInput.Name = "pInput";
            this.pInput.Size = new System.Drawing.Size(278, 354);
            this.pInput.TabIndex = 0;
            // 
            // borderPanel3
            // 
            this.borderPanel3.AutoScroll = true;
            this.borderPanel3.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.borderPanel3.Controls.Add(this.panel2);
            this.borderPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.borderPanel3.Location = new System.Drawing.Point(0, 0);
            this.borderPanel3.Name = "borderPanel3";
            this.borderPanel3.Padding = new System.Windows.Forms.Padding(5);
            this.borderPanel3.Size = new System.Drawing.Size(941, 244);
            this.borderPanel3.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.digitalIO);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(931, 234);
            this.panel2.TabIndex = 1;
            // 
            // pSettings
            // 
            this.pSettings.Controls.Add(this.button1);
            this.pSettings.Controls.Add(this.cbDefault);
            this.pSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pSettings.Location = new System.Drawing.Point(5, 653);
            this.pSettings.Name = "pSettings";
            this.pSettings.Size = new System.Drawing.Size(949, 23);
            this.pSettings.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(714, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(235, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Созадать новую вкладку";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbDefault
            // 
            this.cbDefault.AutoSize = true;
            this.cbDefault.Checked = true;
            this.cbDefault.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDefault.Location = new System.Drawing.Point(3, 4);
            this.cbDefault.Name = "cbDefault";
            this.cbDefault.Size = new System.Drawing.Size(175, 17);
            this.cbDefault.TabIndex = 3;
            this.cbDefault.Text = "Отображать базовый модуль";
            this.cbDefault.UseVisualStyleBackColor = true;
            this.cbDefault.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // borderPanel2
            // 
            this.borderPanel2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.borderPanel2.Controls.Add(this.tabControl1);
            this.borderPanel2.Controls.Add(this.pSettings);
            this.borderPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel2.Location = new System.Drawing.Point(5, 5);
            this.borderPanel2.Name = "borderPanel2";
            this.borderPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.borderPanel2.Size = new System.Drawing.Size(959, 681);
            this.borderPanel2.TabIndex = 5;
            // 
            // ViewIOSensorsTabbedDocument
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.borderPanel2);
            this.Name = "ViewIOSensorsTabbedDocument";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(969, 691);
            this.Closing += new TD.SandDock.DockControlClosingEventHandler(this.ViewIOSensorsTabbedDocument_Closing);
            this.Load += new System.EventHandler(this.DigitalIOTabbedDocument_Load);
            this.LocationChanged += new System.EventHandler(this.DigitalIOTabbedDocument_VisibleChanged);
            this.VisibleChanged += new System.EventHandler(this.DigitalIOTabbedDocument_VisibleChanged);
            this.Leave += new System.EventHandler(this.DigitalIOTabbedDocument_VisibleChanged);
            this.Enter += new System.EventHandler(this.DigitalIOTabbedDocument_VisibleChanged);
            this.SizeChanged += new System.EventHandler(this.DigitalIOTabbedDocument_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabControl1.PerformLayout();
            this.tpDigital.ResumeLayout(false);
            this.borderPanel1.ResumeLayout(false);
            this.gbOutputs.ResumeLayout(false);
            this.gbInputs.ResumeLayout(false);
            this.borderPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pSettings.ResumeLayout(false);
            this.pSettings.PerformLayout();
            this.borderPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private DigitalIO digitalIO;
        private TD.SandDock.TabControl tabControl1;
        private TD.SandDock.TabPage tpDigital;
        private System.Windows.Forms.GroupBox gbInputs;
        private Kontel.Relkon.BorderPanel borderPanel2;
        private System.Windows.Forms.GroupBox gbOutputs;
        private System.Windows.Forms.CheckBox cbDefault;
        private Kontel.Relkon.BorderPanel borderPanel3;
        private System.Windows.Forms.Panel pInput;
        private System.Windows.Forms.Panel pOutput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Kontel.Relkon.BorderPanel borderPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel pSettings;

    }
}
