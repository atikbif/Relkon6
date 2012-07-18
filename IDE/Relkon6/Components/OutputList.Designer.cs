namespace Kontel.Relkon.Components
{
    partial class OutputList
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputList));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.list = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "Output.bmp");
            // 
            // list
            // 
            this.list.BackColor = System.Drawing.Color.White;
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.list.Location = new System.Drawing.Point(0, 0);
            this.list.MaxLength = 100000;
            this.list.Multiline = true;
            this.list.Name = "list";
            this.list.ReadOnly = true;
            this.list.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.list.Size = new System.Drawing.Size(250, 310);
            this.list.TabIndex = 0;
            this.list.WordWrap = false;
            // 
            // OutputList
            // 
            this.DockingRules.AllowDockLeft = false;
            this.DockingRules.AllowDockRight = false;
            this.DockingRules.AllowDockTop = false;
            this.DockingRules.AllowFloat = false;
            this.Controls.Add(this.list);
            this.Name = "OutputList";
            this.Size = new System.Drawing.Size(250, 310);
            this.Text = "Output List";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox list;
    }
}
