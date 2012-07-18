namespace Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument
{
    partial class AnalogSensorControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbarValue = new System.Windows.Forms.TrackBar();
            this.nudValue = new System.Windows.Forms.NumericUpDown();
            this.rb8b = new System.Windows.Forms.RadioButton();
            this.rb12b = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.tbarValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label";
            // 
            // tbDescription
            // 
            this.tbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDescription.Location = new System.Drawing.Point(107, 5);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.ReadOnly = true;
            this.tbDescription.Size = new System.Drawing.Size(89, 13);
            this.tbDescription.TabIndex = 1;
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            this.tbDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDescription_KeyDown);
            this.tbDescription.Leave += new System.EventHandler(this.tbDescription_Leave_1);
            this.tbDescription.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbDescription_MouseDown);
            // 
            // tbarValue
            // 
            this.tbarValue.Location = new System.Drawing.Point(285, 3);
            this.tbarValue.Maximum = 65535;
            this.tbarValue.Name = "tbarValue";
            this.tbarValue.Size = new System.Drawing.Size(83, 42);
            this.tbarValue.TabIndex = 2;
            this.tbarValue.TickFrequency = 6553;
            this.tbarValue.ValueChanged += new System.EventHandler(this.nudValue_ValueChanged);
            this.tbarValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbarValue_KeyDown);
            this.tbarValue.Leave += new System.EventHandler(this.tbDescription_Leave);
            // 
            // nudValue
            // 
            this.nudValue.Location = new System.Drawing.Point(204, 6);
            this.nudValue.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudValue.Name = "nudValue";
            this.nudValue.Size = new System.Drawing.Size(75, 20);
            this.nudValue.TabIndex = 3;
            this.nudValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudValue.ValueChanged += new System.EventHandler(this.nudValue_ValueChanged);
            this.nudValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbarValue_KeyDown);
            this.nudValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nudValue_KeyPress);
            this.nudValue.Leave += new System.EventHandler(this.tbDescription_Leave);
            // 
            // rb8b
            // 
            this.rb8b.AutoSize = true;
            this.rb8b.Checked = true;
            this.rb8b.Location = new System.Drawing.Point(4, -2);
            this.rb8b.Name = "rb8b";
            this.rb8b.Size = new System.Drawing.Size(57, 17);
            this.rb8b.TabIndex = 4;
            this.rb8b.TabStop = true;
            this.rb8b.Text = "1 байт";
            this.rb8b.UseVisualStyleBackColor = true;
            this.rb8b.CheckedChanged += new System.EventHandler(this.rb8b_CheckedChanged);
            // 
            // rb12b
            // 
            this.rb12b.AutoSize = true;
            this.rb12b.Location = new System.Drawing.Point(4, 12);
            this.rb12b.Name = "rb12b";
            this.rb12b.Size = new System.Drawing.Size(63, 17);
            this.rb12b.TabIndex = 5;
            this.rb12b.Text = "2 байта";
            this.rb12b.UseVisualStyleBackColor = true;
            this.rb12b.CheckedChanged += new System.EventHandler(this.rb12b_CheckedChanged);
            // 
            // AnalogSensorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.rb12b);
            this.Controls.Add(this.rb8b);
            this.Controls.Add(this.nudValue);
            this.Controls.Add(this.tbarValue);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "AnalogSensorControl";
            this.Size = new System.Drawing.Size(536, 29);
            this.Leave += new System.EventHandler(this.AnalogSensorControl_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.tbarValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tbarValue;
        private System.Windows.Forms.NumericUpDown nudValue;
        private System.Windows.Forms.RadioButton rb8b;
        private System.Windows.Forms.RadioButton rb12b;
        private System.Windows.Forms.TextBox tbDescription;
    }
}
