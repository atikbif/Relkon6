namespace Kontel.Relkon
{
    partial class LCDIndicator
    {
        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LCDIndicator));
            this.cm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miSpecialSymbol = new System.Windows.Forms.ToolStripMenuItem();
            this.cm.SuspendLayout();
            this.SuspendLayout();
            // 
            // cm
            // 
            this.cm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCut,
            this.miCopy,
            this.miPaste,
            this.toolStripSeparator1,
            this.miSpecialSymbol});
            this.cm.Name = "cm";
            this.cm.Size = new System.Drawing.Size(201, 98);
            this.cm.Opening += new System.ComponentModel.CancelEventHandler(this.cm_Opening);
            // 
            // miCut
            // 
            this.miCut.Image = ((System.Drawing.Image)(resources.GetObject("miCut.Image")));
            this.miCut.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCut.Name = "miCut";
            this.miCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miCut.Size = new System.Drawing.Size(200, 22);
            this.miCut.Text = "Вырезать";
            this.miCut.Click += new System.EventHandler(this.miCut_Click);
            // 
            // miCopy
            // 
            this.miCopy.Image = ((System.Drawing.Image)(resources.GetObject("miCopy.Image")));
            this.miCopy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCopy.Name = "miCopy";
            this.miCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miCopy.Size = new System.Drawing.Size(200, 22);
            this.miCopy.Text = "Копировать";
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // miPaste
            // 
            this.miPaste.ForeColor = System.Drawing.SystemColors.ControlText;
            this.miPaste.Image = ((System.Drawing.Image)(resources.GetObject("miPaste.Image")));
            this.miPaste.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miPaste.Name = "miPaste";
            this.miPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miPaste.Size = new System.Drawing.Size(200, 22);
            this.miPaste.Text = "Вставить";
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(197, 6);
            // 
            // miSpecialSymbol
            // 
            this.miSpecialSymbol.Image = ((System.Drawing.Image)(resources.GetObject("miSpecialSymbol.Image")));
            this.miSpecialSymbol.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miSpecialSymbol.Name = "miSpecialSymbol";
            this.miSpecialSymbol.Size = new System.Drawing.Size(200, 22);
            this.miSpecialSymbol.Text = "Специальные символы";
            this.miSpecialSymbol.Click += new System.EventHandler(this.miSpecialSymbol_Click);
            // 
            // LCDIndicator
            // 
            this.ContextMenuStrip = this.cm;
            this.cm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cm;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ToolStripMenuItem miCut;
        private System.Windows.Forms.ToolStripMenuItem miCopy;
        private System.Windows.Forms.ToolStripMenuItem miPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miSpecialSymbol;

    }
}
