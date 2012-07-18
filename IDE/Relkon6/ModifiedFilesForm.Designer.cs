namespace Kontel.Relkon
{
    partial class ModifiedFilesForm
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
            this.bYes = new System.Windows.Forms.Button();
            this.bNo = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbFileNames = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // bYes
            // 
            this.bYes.Location = new System.Drawing.Point(130, 215);
            this.bYes.Name = "bYes";
            this.bYes.Size = new System.Drawing.Size(75, 23);
            this.bYes.TabIndex = 1;
            this.bYes.Text = "Да";
            this.bYes.UseVisualStyleBackColor = true;
            this.bYes.Click += new System.EventHandler(this.bYes_Click);
            // 
            // bNo
            // 
            this.bNo.Location = new System.Drawing.Point(211, 215);
            this.bNo.Name = "bNo";
            this.bNo.Size = new System.Drawing.Size(75, 23);
            this.bNo.TabIndex = 2;
            this.bNo.Text = "Нет";
            this.bNo.UseVisualStyleBackColor = true;
            this.bNo.Click += new System.EventHandler(this.bNo_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(292, 215);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Следующие файлы был изменены. Сохранить ?";
            // 
            // lbFileNames
            // 
            this.lbFileNames.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFileNames.FormattingEnabled = true;
            this.lbFileNames.HorizontalScrollbar = true;
            this.lbFileNames.Location = new System.Drawing.Point(12, 35);
            this.lbFileNames.Name = "lbFileNames";
            this.lbFileNames.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbFileNames.Size = new System.Drawing.Size(355, 160);
            this.lbFileNames.TabIndex = 5;            
            // 
            // ModifiedFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 250);
            this.Controls.Add(this.lbFileNames);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bNo);
            this.Controls.Add(this.bYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(385, 282);
            this.MinimizeBox = false;
            this.Name = "ModifiedFilesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relkon";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bYes;
        private System.Windows.Forms.Button bNo;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbFileNames;
    }
}