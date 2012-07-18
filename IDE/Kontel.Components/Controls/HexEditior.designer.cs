namespace Kontel.Relkon
{
    partial class HexEditior
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
            this.AddressBox = new System.Windows.Forms.RichTextBox();
            this.CodeBox = new System.Windows.Forms.RichTextBox();
            this.PresentationBox = new System.Windows.Forms.RichTextBox();
            this.MainScrollBar = new System.Windows.Forms.VScrollBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.HorizontalScrollBar = new System.Windows.Forms.HScrollBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddressBox
            // 
            this.AddressBox.BackColor = System.Drawing.SystemColors.Window;
            this.AddressBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AddressBox.Location = new System.Drawing.Point(3, 3);
            this.AddressBox.Name = "AddressBox";
            this.AddressBox.ReadOnly = true;
            this.AddressBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.AddressBox.Size = new System.Drawing.Size(100, 96);
            this.AddressBox.TabIndex = 0;
            this.AddressBox.Text = "";
            this.AddressBox.GotFocus += new System.EventHandler(this.AddressBoxGotFocus);
            // 
            // CodeBox
            // 
            this.CodeBox.BackColor = System.Drawing.SystemColors.Window;
            this.CodeBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CodeBox.Location = new System.Drawing.Point(109, 3);
            this.CodeBox.Name = "CodeBox";
            this.CodeBox.ReadOnly = true;
            this.CodeBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.CodeBox.Size = new System.Drawing.Size(100, 96);
            this.CodeBox.TabIndex = 0;
            this.CodeBox.Text = "";
            this.CodeBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CodeBoxKeyDown);
            this.CodeBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.CodeMouseRotate);
            this.CodeBox.DoubleClick += new System.EventHandler(this.CodeBoxDoubleClick);
            this.CodeBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CodeBoxMouseDown);
            this.CodeBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CodeBoxKeyPress);
            this.CodeBox.GotFocus += new System.EventHandler(this.TextGotFocus);
            // 
            // PresentationBox
            // 
            this.PresentationBox.BackColor = System.Drawing.SystemColors.Window;
            this.PresentationBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PresentationBox.Location = new System.Drawing.Point(215, 3);
            this.PresentationBox.Name = "PresentationBox";
            this.PresentationBox.ReadOnly = true;
            this.PresentationBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.PresentationBox.Size = new System.Drawing.Size(100, 96);
            this.PresentationBox.TabIndex = 0;
            this.PresentationBox.Text = "";
            this.PresentationBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PresentationBoxKeyDown);
            this.PresentationBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.CodeMouseRotate);
            this.PresentationBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PresentationBoxMouseDown);
            this.PresentationBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PresentationBoxKeyPress);
            this.PresentationBox.GotFocus += new System.EventHandler(this.TextGotFocus);
            // 
            // MainScrollBar
            // 
            this.MainScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainScrollBar.Location = new System.Drawing.Point(0, 0);
            this.MainScrollBar.Name = "MainScrollBar";
            this.MainScrollBar.Size = new System.Drawing.Size(18, 121);
            this.MainScrollBar.TabIndex = 1;
            this.MainScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.MainScrollBar_Scroll);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MainScrollBar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(466, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(18, 121);
            this.panel1.TabIndex = 2;
            // 
            // HorizontalScrollBar
            // 
            this.HorizontalScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.HorizontalScrollBar.Location = new System.Drawing.Point(0, 105);
            this.HorizontalScrollBar.Name = "HorizontalScrollBar";
            this.HorizontalScrollBar.Size = new System.Drawing.Size(466, 16);
            this.HorizontalScrollBar.TabIndex = 3;
            this.HorizontalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HorizontalScrollBar_Scroll);
            // 
            // HexEditior
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.HorizontalScrollBar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddressBox);
            this.Controls.Add(this.CodeBox);
            this.Controls.Add(this.PresentationBox);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "HexEditior";
            this.Size = new System.Drawing.Size(484, 121);
            this.Enter += new System.EventHandler(this.HexEditior_Enter);
            this.ClientSizeChanged += new System.EventHandler(this.HexEditior_ClientSizeChanged);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox AddressBox;
        private System.Windows.Forms.RichTextBox PresentationBox;
        private System.Windows.Forms.RichTextBox CodeBox;
        private System.Windows.Forms.VScrollBar MainScrollBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar HorizontalScrollBar;
    }
}
