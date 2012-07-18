using System.ComponentModel;

namespace Kontel.TabbedDocumentsForm
{
    partial class TabbedDocumentsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabbedDocumentsForm));
            this.DocumentManager = new TD.SandDock.SandDockManager();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.PrintDocument = new System.Drawing.Printing.PrintDocument();
            this.PrintPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.PageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.PrintDialog = new System.Windows.Forms.PrintDialog();
            this.WatchForChangesTimer = new System.Windows.Forms.Timer(this.components);
            this.AnimationTimer = new System.Windows.Forms.Timer(this.components);
            this.SaveBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.Images = new System.Windows.Forms.ImageList(this.components);
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.LeftBorderStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.InformationStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.AnimationStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.PositionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.FontDialog = new System.Windows.Forms.FontDialog();
            this.TabbedDocumentContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.miNewHorizontalTabGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewVerticalTabGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip.SuspendLayout();
            this.TabbedDocumentContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // DocumentManager
            // 
            this.DocumentManager.DockSystemContainer = this;
            this.DocumentManager.OwnerForm = this;
            this.DocumentManager.ShowControlContextMenu += new TD.SandDock.ShowControlContextMenuEventHandler(this.DocumentManager_ShowControlContextMenu);
            this.DocumentManager.DockControlClosing += new TD.SandDock.DockControlClosingEventHandler(this.DocumentManager_DockControlClosing);
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.RestoreDirectory = true;
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.RestoreDirectory = true;
            // 
            // PrintPreviewDialog
            // 
            this.PrintPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.PrintPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.PrintPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.PrintPreviewDialog.Document = this.PrintDocument;
            this.PrintPreviewDialog.Enabled = true;
            this.PrintPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("PrintPreviewDialog.Icon")));
            this.PrintPreviewDialog.Name = "ppd1";
            this.PrintPreviewDialog.Visible = false;
            // 
            // PageSetupDialog
            // 
            this.PageSetupDialog.Document = this.PrintDocument;
            // 
            // PrintDialog
            // 
            this.PrintDialog.AllowSelection = true;
            this.PrintDialog.Document = this.PrintDocument;
            this.PrintDialog.ShowHelp = true;
            this.PrintDialog.UseEXDialog = true;
            // 
            // WatchForChangesTimer
            // 
            this.WatchForChangesTimer.Interval = 500;
            this.WatchForChangesTimer.Tick += new System.EventHandler(this.WatchForChangesTimer_Tick);
            // 
            // AnimationTimer
            // 
            this.AnimationTimer.Interval = 200;
            this.AnimationTimer.Tick += new System.EventHandler(this.AnimationTimer_Tick);
            // 
            // SaveBackgroundWorker
            // 
            this.SaveBackgroundWorker.WorkerReportsProgress = true;
            this.SaveBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SaveBackgroundWorker_DoWork);
            this.SaveBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.SaveBackgroundWorker_RunWorkerCompleted);
            // 
            // Images
            // 
            this.Images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Images.ImageStream")));
            this.Images.TransparentColor = System.Drawing.Color.Fuchsia;
            this.Images.Images.SetKeyName(0, "SaveStrip.bmp");
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LeftBorderStatusLabel,
            this.InformationStatusLabel,
            this.AnimationStatusLabel,
            this.PositionStatusLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 407);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(780, 22);
            this.StatusStrip.TabIndex = 1;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // LeftBorderStatusLabel
            // 
            this.LeftBorderStatusLabel.Name = "LeftBorderStatusLabel";
            this.LeftBorderStatusLabel.Size = new System.Drawing.Size(10, 17);
            this.LeftBorderStatusLabel.Text = " ";
            // 
            // InformationStatusLabel
            // 
            this.InformationStatusLabel.AutoSize = false;
            this.InformationStatusLabel.Name = "InformationStatusLabel";
            this.InformationStatusLabel.Size = new System.Drawing.Size(185, 17);
            this.InformationStatusLabel.Spring = true;
            this.InformationStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AnimationStatusLabel
            // 
            this.AnimationStatusLabel.AutoSize = false;
            this.AnimationStatusLabel.Name = "AnimationStatusLabel";
            this.AnimationStatusLabel.Size = new System.Drawing.Size(210, 17);
            this.AnimationStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PositionStatusLabel
            // 
            this.PositionStatusLabel.AutoSize = false;
            this.PositionStatusLabel.Name = "PositionStatusLabel";
            this.PositionStatusLabel.Size = new System.Drawing.Size(360, 17);
            // 
            // TabbedDocumentContextMenu
            // 
            this.TabbedDocumentContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSave,
            this.miClose,
            this.miCloseAllButThis,
            this.toolStripSeparator15,
            this.miNewHorizontalTabGroup,
            this.miNewVerticalTabGroup});
            this.TabbedDocumentContextMenu.Name = "TabbedWindowContextMenu";
            this.TabbedDocumentContextMenu.Size = new System.Drawing.Size(241, 120);
            this.TabbedDocumentContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TabbedDocumentContextMenu_Opening);
            // 
            // miSave
            // 
            this.miSave.Image = ((System.Drawing.Image)(resources.GetObject("miSave.Image")));
            this.miSave.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miSave.Name = "miSave";
            this.miSave.Size = new System.Drawing.Size(240, 22);
            this.miSave.Text = "&Сохранить";
            this.miSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // miClose
            // 
            this.miClose.Name = "miClose";
            this.miClose.Size = new System.Drawing.Size(240, 22);
            this.miClose.Text = "&Закрыть";
            this.miClose.Click += new System.EventHandler(this.miClose_Click);
            // 
            // miCloseAllButThis
            // 
            this.miCloseAllButThis.Name = "miCloseAllButThis";
            this.miCloseAllButThis.Size = new System.Drawing.Size(240, 22);
            this.miCloseAllButThis.Text = "Закрыть &все остальные";
            this.miCloseAllButThis.Click += new System.EventHandler(this.miCloseAllButThis_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(237, 6);
            // 
            // miNewHorizontalTabGroup
            // 
            this.miNewHorizontalTabGroup.Image = ((System.Drawing.Image)(resources.GetObject("miNewHorizontalTabGroup.Image")));
            this.miNewHorizontalTabGroup.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miNewHorizontalTabGroup.Name = "miNewHorizontalTabGroup";
            this.miNewHorizontalTabGroup.Size = new System.Drawing.Size(240, 22);
            this.miNewHorizontalTabGroup.Text = "Новая &горизонтальная группа";
            this.miNewHorizontalTabGroup.Click += new System.EventHandler(this.miNewHorizontalTabGroup_Click);
            // 
            // miNewVerticalTabGroup
            // 
            this.miNewVerticalTabGroup.Image = ((System.Drawing.Image)(resources.GetObject("miNewVerticalTabGroup.Image")));
            this.miNewVerticalTabGroup.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miNewVerticalTabGroup.Name = "miNewVerticalTabGroup";
            this.miNewVerticalTabGroup.Size = new System.Drawing.Size(240, 22);
            this.miNewVerticalTabGroup.Text = "Новая &вертикальная группа";
            this.miNewVerticalTabGroup.Click += new System.EventHandler(this.miNewVerticalTabGroup_Click);
            // 
            // TabbedDocumentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 429);
            this.Controls.Add(this.StatusStrip);
            this.Name = "TabbedDocumentsForm";
            this.Text = "TabbedDocumentsForm";
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.TabbedDocumentContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected TD.SandDock.SandDockManager DocumentManager;
        protected System.Drawing.Printing.PrintDocument PrintDocument;
        protected System.Windows.Forms.PrintPreviewDialog PrintPreviewDialog;
        protected System.Windows.Forms.PageSetupDialog PageSetupDialog;
        protected System.Windows.Forms.PrintDialog PrintDialog;
        protected System.Windows.Forms.SaveFileDialog SaveFileDialog;
        protected System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.Timer WatchForChangesTimer;
        private System.Windows.Forms.Timer AnimationTimer;
        private System.ComponentModel.BackgroundWorker SaveBackgroundWorker;
        private System.Windows.Forms.ImageList Images;
        public System.Windows.Forms.StatusStrip StatusStrip;
        public System.Windows.Forms.ToolStripStatusLabel LeftBorderStatusLabel;
        public System.Windows.Forms.ToolStripStatusLabel InformationStatusLabel;
        public System.Windows.Forms.ToolStripStatusLabel AnimationStatusLabel;
        public System.Windows.Forms.ToolStripStatusLabel PositionStatusLabel;
        protected System.Windows.Forms.FontDialog FontDialog;
        protected System.Windows.Forms.ContextMenuStrip TabbedDocumentContextMenu;
        private System.Windows.Forms.ToolStripMenuItem miSave;
        private System.Windows.Forms.ToolStripMenuItem miClose;
        private System.Windows.Forms.ToolStripMenuItem miCloseAllButThis;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem miNewHorizontalTabGroup;
        private System.Windows.Forms.ToolStripMenuItem miNewVerticalTabGroup;
    }
}