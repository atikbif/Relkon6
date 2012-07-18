namespace Kontel.Relkon.Forms
{
    partial class ProgressForm
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
            this.lMessage = new System.Windows.Forms.Label();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pCloseButton = new Kontel.Relkon.BorderPanel();
            this.SuspendLayout();
            // 
            // lMessage
            // 
            this.lMessage.AutoSize = true;
            this.lMessage.Location = new System.Drawing.Point(16, 9);
            this.lMessage.Name = "lMessage";
            this.lMessage.Size = new System.Drawing.Size(0, 13);
            this.lMessage.TabIndex = 3;
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(16, 25);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(248, 23);
            this.pbProgress.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(1, 55);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(278, 1);
            this.panel4.TabIndex = 9;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(1, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(278, 1);
            this.panel3.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1, 56);
            this.panel2.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(279, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 56);
            this.panel1.TabIndex = 6;
            // 
            // pCloseButton
            // 
            this.pCloseButton.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.pCloseButton.Location = new System.Drawing.Point(264, 2);
            this.pCloseButton.Name = "pCloseButton";
            this.pCloseButton.Padding = new System.Windows.Forms.Padding(1);
            this.pCloseButton.Size = new System.Drawing.Size(14, 13);
            this.pCloseButton.TabIndex = 10;
            this.pCloseButton.MouseLeave += new System.EventHandler(this.pCloseButton_MouseLeave);
            this.pCloseButton.Paint += new System.Windows.Forms.PaintEventHandler(this.pCloseButton_Paint);
            this.pCloseButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pCloseButton_MouseClick);
            this.pCloseButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pCloseButton_MouseDown);
            this.pCloseButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pCloseButton_MouseUp);
            this.pCloseButton.MouseEnter += new System.EventHandler(this.pCloseButton_MouseEnter);
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 56);
            this.Controls.Add(this.pCloseButton);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lMessage);
            this.Controls.Add(this.pbProgress);
            this.Name = "ProgressForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProgressForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lMessage;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private BorderPanel pCloseButton;
    }
}