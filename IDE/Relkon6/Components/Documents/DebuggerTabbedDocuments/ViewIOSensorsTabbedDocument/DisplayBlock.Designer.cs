namespace Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument
{
    partial class DisplayBlock
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
            this.gbUnReadVars = new System.Windows.Forms.GroupBox();
            this.clb_moduls = new System.Windows.Forms.CheckedListBox();
            this.tbCaption = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bDelete = new System.Windows.Forms.Button();
            this.borderPanel1 = new Kontel.Relkon.BorderPanel();
            this.gbOutputs = new System.Windows.Forms.GroupBox();
            this.pOutput = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.gbInputs = new System.Windows.Forms.GroupBox();
            this.pInput = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.borderPanel3 = new Kontel.Relkon.BorderPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.digitalIO = new Kontel.Relkon.Components.Documents.DigitalIO();
            this.gbUnReadVars.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.borderPanel1.SuspendLayout();
            this.gbOutputs.SuspendLayout();
            this.gbInputs.SuspendLayout();
            this.borderPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbUnReadVars
            // 
            this.gbUnReadVars.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.gbUnReadVars.Controls.Add(this.clb_moduls);
            this.gbUnReadVars.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbUnReadVars.Location = new System.Drawing.Point(1, 1);
            this.gbUnReadVars.Name = "gbUnReadVars";
            this.gbUnReadVars.Padding = new System.Windows.Forms.Padding(5);
            this.gbUnReadVars.Size = new System.Drawing.Size(87, 635);
            this.gbUnReadVars.TabIndex = 7;
            this.gbUnReadVars.TabStop = false;
            this.gbUnReadVars.Text = "Модули";
            // 
            // clb_moduls
            // 
            this.clb_moduls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clb_moduls.FormattingEnabled = true;
            this.clb_moduls.Location = new System.Drawing.Point(5, 18);
            this.clb_moduls.Name = "clb_moduls";
            this.clb_moduls.Size = new System.Drawing.Size(77, 604);
            this.clb_moduls.TabIndex = 2;
            this.clb_moduls.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clb_moduls_ItemCheck);
            // 
            // tbCaption
            // 
            this.tbCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCaption.Location = new System.Drawing.Point(3, 16);
            this.tbCaption.Name = "tbCaption";
            this.tbCaption.Size = new System.Drawing.Size(718, 20);
            this.tbCaption.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbCaption);
            this.groupBox1.Controls.Add(this.bDelete);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(88, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(836, 42);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Назавание вкладки";
            // 
            // bDelete
            // 
            this.bDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.bDelete.Location = new System.Drawing.Point(721, 16);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(112, 23);
            this.bDelete.TabIndex = 4;
            this.bDelete.Text = "Удалить вкладку";
            this.bDelete.UseVisualStyleBackColor = true;
            // 
            // borderPanel1
            // 
            this.borderPanel1.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.borderPanel1.Controls.Add(this.gbOutputs);
            this.borderPanel1.Controls.Add(this.splitter2);
            this.borderPanel1.Controls.Add(this.gbInputs);
            this.borderPanel1.Controls.Add(this.splitter1);
            this.borderPanel1.Controls.Add(this.borderPanel3);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel1.Location = new System.Drawing.Point(88, 43);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.borderPanel1.Size = new System.Drawing.Size(836, 593);
            this.borderPanel1.TabIndex = 6;
            // 
            // gbOutputs
            // 
            this.gbOutputs.Controls.Add(this.pOutput);
            this.gbOutputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOutputs.Location = new System.Drawing.Point(1, 377);
            this.gbOutputs.Name = "gbOutputs";
            this.gbOutputs.Padding = new System.Windows.Forms.Padding(2);
            this.gbOutputs.Size = new System.Drawing.Size(834, 215);
            this.gbOutputs.TabIndex = 3;
            this.gbOutputs.TabStop = false;
            this.gbOutputs.Text = "Выходные датчики";
            // 
            // pOutput
            // 
            this.pOutput.AutoScroll = true;
            this.pOutput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pOutput.Location = new System.Drawing.Point(2, 15);
            this.pOutput.Name = "pOutput";
            this.pOutput.Size = new System.Drawing.Size(830, 198);
            this.pOutput.TabIndex = 0;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(1, 374);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(834, 3);
            this.splitter2.TabIndex = 6;
            this.splitter2.TabStop = false;
            this.splitter2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter2_SplitterMoved);
            // 
            // gbInputs
            // 
            this.gbInputs.Controls.Add(this.pInput);
            this.gbInputs.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbInputs.Location = new System.Drawing.Point(1, 232);
            this.gbInputs.Name = "gbInputs";
            this.gbInputs.Padding = new System.Windows.Forms.Padding(2);
            this.gbInputs.Size = new System.Drawing.Size(834, 142);
            this.gbInputs.TabIndex = 1;
            this.gbInputs.TabStop = false;
            this.gbInputs.Text = "Входные датчики";
            // 
            // pInput
            // 
            this.pInput.AutoScroll = true;
            this.pInput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pInput.Location = new System.Drawing.Point(2, 15);
            this.pInput.Name = "pInput";
            this.pInput.Size = new System.Drawing.Size(830, 125);
            this.pInput.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(1, 229);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(834, 3);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            this.splitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoved);
            // 
            // borderPanel3
            // 
            this.borderPanel3.AutoScroll = true;
            this.borderPanel3.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.borderPanel3.Controls.Add(this.panel2);
            this.borderPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.borderPanel3.Location = new System.Drawing.Point(1, 1);
            this.borderPanel3.Name = "borderPanel3";
            this.borderPanel3.Padding = new System.Windows.Forms.Padding(3);
            this.borderPanel3.Size = new System.Drawing.Size(834, 228);
            this.borderPanel3.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.digitalIO);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(828, 222);
            this.panel2.TabIndex = 1;
            // 
            // digitalIO
            // 
            this.digitalIO.BackColor = System.Drawing.SystemColors.Control;
            this.digitalIO.EnabledMouseClick = true;
            this.digitalIO.Interval = 17;
            this.digitalIO.Location = new System.Drawing.Point(1, 0);
            this.digitalIO.Margin = new System.Windows.Forms.Padding(10);
            this.digitalIO.Name = "digitalIO";
            this.digitalIO.Padding = new System.Windows.Forms.Padding(15);
            this.digitalIO.Size = new System.Drawing.Size(226, 142);
            this.digitalIO.TabIndex = 0;
            this.digitalIO.StateChange += new System.EventHandler<Kontel.Relkon.Components.Documents.DigitalIO.StateChangeEventArgs>(this.digitalIO_StateChange);
            this.digitalIO.LabelChange += new System.EventHandler<Kontel.Relkon.Components.Documents.DigitalIO.LabelChangeEventArgs>(this.digitalIO_LabelChange);
            // 
            // DisplayBlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.borderPanel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbUnReadVars);
            this.Name = "DisplayBlock";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(925, 637);
            this.gbUnReadVars.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.borderPanel1.ResumeLayout(false);
            this.gbOutputs.ResumeLayout(false);
            this.gbInputs.ResumeLayout(false);
            this.borderPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Kontel.Relkon.BorderPanel borderPanel3;
        private System.Windows.Forms.Panel panel2;
        private DigitalIO digitalIO;
        private Kontel.Relkon.BorderPanel borderPanel1;
        private System.Windows.Forms.GroupBox gbOutputs;
        private System.Windows.Forms.Panel pOutput;
        private System.Windows.Forms.GroupBox gbInputs;
        private System.Windows.Forms.Panel pInput;
        private System.Windows.Forms.GroupBox gbUnReadVars;
        private System.Windows.Forms.CheckedListBox clb_moduls;
        public System.Windows.Forms.TextBox tbCaption;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter1;
    }
}
