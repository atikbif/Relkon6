namespace Kontel.Relkon.Components.Documents
{
    partial class DigitalIO
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
            this.tbEditLabel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbEditLabel
            // 
            this.tbEditLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbEditLabel.Location = new System.Drawing.Point(21, 25);
            this.tbEditLabel.Name = "tbEditLabel";
            this.tbEditLabel.Size = new System.Drawing.Size(100, 20);
            this.tbEditLabel.TabIndex = 1;
            this.tbEditLabel.Visible = false;
            this.tbEditLabel.VisibleChanged += new System.EventHandler(this.tbEditLabel_VisibleChanged);
            this.tbEditLabel.TextChanged += new System.EventHandler(this.tbEditLabel_TextChanged);
            this.tbEditLabel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbEditLabel_KeyDown);
            this.tbEditLabel.Leave += new System.EventHandler(this.tbEditLabel_Leave);
            this.tbEditLabel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbEditLabel_KeyUp);
            this.tbEditLabel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbEditLabel_KeyPress);
            // 
            // DigitalIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbEditLabel);
            this.Name = "DigitalIO";
            this.Leave += new System.EventHandler(this.tbEditLabel_Leave);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DigitalIO_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DigitalIO_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbEditLabel;


    }
}
