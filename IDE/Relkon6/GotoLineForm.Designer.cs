namespace Kontel.Relkon
{
    partial class GotoLineForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tbLineNumber = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Номер строки";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(77, 51);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 2;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            this.bOk.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbLineNumber_KeyDown);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(158, 51);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            this.bCancel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbLineNumber_KeyDown);
            // 
            // tbLineNumber
            // 
            this.tbLineNumber.Location = new System.Drawing.Point(12, 25);
            this.tbLineNumber.Name = "tbLineNumber";
            this.tbLineNumber.Size = new System.Drawing.Size(221, 20);
            this.tbLineNumber.TabIndex = 5;
            this.tbLineNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLineNumber_KeyPress);
            this.tbLineNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbLineNumber_KeyDown);
            // 
            // GotoLineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 79);
            this.Controls.Add(this.tbLineNumber);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(251, 111);
            this.MinimizeBox = false;
            this.Name = "GotoLineForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Перейти к строке";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbLineNumber_KeyDown);
            this.Load += new System.EventHandler(this.GotoLineForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TextBox tbLineNumber;
    }
}