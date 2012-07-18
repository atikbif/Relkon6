using Kontel.Relkon;
namespace Kontel.Relkon.Components.Documents
{
    partial class StandartPultTabbedDocument
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandartPultTabbedDocument));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.gbVar = new System.Windows.Forms.GroupBox();
            this.bAddVar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbHasSign = new System.Windows.Forms.CheckBox();
            this.ddlVarNames = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbVarMask = new System.Windows.Forms.TextBox();
            this.cbReadOnly = new System.Windows.Forms.CheckBox();
            this.pIndicator = new System.Windows.Forms.Panel();
            this.gbIndicator = new System.Windows.Forms.GroupBox();
            this.lcdIndicator = new Kontel.Relkon.LCDIndicator();
            this.panel8 = new System.Windows.Forms.Panel();
            this.gbRows = new System.Windows.Forms.GroupBox();
            this.tvRows = new Kontel.Relkon.TreeViewEx();
            this.ViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAddView = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddViewFromTop = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddViewFromBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeleteView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miDisableView = new System.Windows.Forms.ToolStripMenuItem();
            this.miEnableView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miCutView = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopyView = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopyAsText = new System.Windows.Forms.ToolStripMenuItem();
            this.miPasteView = new System.Windows.Forms.ToolStripMenuItem();
            this.miReplaceView = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemsList = new System.Windows.Forms.ImageList(this.components);
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.gbVar.SuspendLayout();
            this.pIndicator.SuspendLayout();
            this.gbIndicator.SuspendLayout();
            this.gbRows.SuspendLayout();
            this.ViewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(948, 1);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(5, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1, 509);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(6, 514);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(947, 1);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(952, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1, 508);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Control;
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.pIndicator);
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Controls.Add(this.gbRows);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(6, 6);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.panel5.Size = new System.Drawing.Size(946, 508);
            this.panel5.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.gbVar);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(274, 185);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(667, 174);
            this.panel6.TabIndex = 11;
            // 
            // gbVar
            // 
            this.gbVar.Controls.Add(this.bAddVar);
            this.gbVar.Controls.Add(this.label1);
            this.gbVar.Controls.Add(this.cbHasSign);
            this.gbVar.Controls.Add(this.ddlVarNames);
            this.gbVar.Controls.Add(this.label2);
            this.gbVar.Controls.Add(this.tbVarMask);
            this.gbVar.Controls.Add(this.cbReadOnly);
            this.gbVar.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbVar.Location = new System.Drawing.Point(0, 0);
            this.gbVar.Name = "gbVar";
            this.gbVar.Padding = new System.Windows.Forms.Padding(10, 4, 10, 10);
            this.gbVar.Size = new System.Drawing.Size(499, 174);
            this.gbVar.TabIndex = 6;
            this.gbVar.TabStop = false;
            this.gbVar.Text = "Настройка переменных";
            // 
            // bAddVar
            // 
            this.bAddVar.Enabled = false;
            this.bAddVar.Location = new System.Drawing.Point(411, 25);
            this.bAddVar.Name = "bAddVar";
            this.bAddVar.Size = new System.Drawing.Size(75, 21);
            this.bAddVar.TabIndex = 12;
            this.bAddVar.Text = "Добавить";
            this.bAddVar.UseVisualStyleBackColor = true;
            this.bAddVar.Click += new System.EventHandler(this.bAddVar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя переменной:";
            // 
            // cbHasSign
            // 
            this.cbHasSign.AutoSize = true;
            this.cbHasSign.Location = new System.Drawing.Point(137, 75);
            this.cbHasSign.Name = "cbHasSign";
            this.cbHasSign.Size = new System.Drawing.Size(75, 17);
            this.cbHasSign.TabIndex = 5;
            this.cbHasSign.Text = "Знаковое";
            this.cbHasSign.UseVisualStyleBackColor = true;
            this.cbHasSign.CheckedChanged += new System.EventHandler(this.cbHasSign_CheckedChanged);
            // 
            // ddlVarNames
            // 
            this.ddlVarNames.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.ddlVarNames.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ddlVarNames.FormattingEnabled = true;
            this.ddlVarNames.Location = new System.Drawing.Point(137, 25);
            this.ddlVarNames.Name = "ddlVarNames";
            this.ddlVarNames.Size = new System.Drawing.Size(268, 21);
            this.ddlVarNames.Sorted = true;
            this.ddlVarNames.TabIndex = 1;
            this.ddlVarNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ddlVarNames_KeyDown);
            this.ddlVarNames.TextChanged += new System.EventHandler(this.ddlVarNames_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Маска вывода:";
            // 
            // tbVarMask
            // 
            this.tbVarMask.Location = new System.Drawing.Point(137, 49);
            this.tbVarMask.Name = "tbVarMask";
            this.tbVarMask.Size = new System.Drawing.Size(268, 20);
            this.tbVarMask.TabIndex = 3;
            this.tbVarMask.TextChanged += new System.EventHandler(this.ddlVarNames_TextChanged);
            this.tbVarMask.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ddlVarNames_KeyDown);
            this.tbVarMask.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbVarMask_KeyPress);
            // 
            // cbReadOnly
            // 
            this.cbReadOnly.AutoSize = true;
            this.cbReadOnly.Location = new System.Drawing.Point(9, 76);
            this.cbReadOnly.Name = "cbReadOnly";
            this.cbReadOnly.Size = new System.Drawing.Size(100, 17);
            this.cbReadOnly.TabIndex = 4;
            this.cbReadOnly.Text = "Только чтение";
            this.cbReadOnly.UseVisualStyleBackColor = true;
            this.cbReadOnly.CheckedChanged += new System.EventHandler(this.cbReadOnly_CheckedChanged);
            // 
            // pIndicator
            // 
            this.pIndicator.Controls.Add(this.gbIndicator);
            this.pIndicator.Dock = System.Windows.Forms.DockStyle.Top;
            this.pIndicator.Location = new System.Drawing.Point(274, 0);
            this.pIndicator.Name = "pIndicator";
            this.pIndicator.Size = new System.Drawing.Size(667, 185);
            this.pIndicator.TabIndex = 10;
            // 
            // gbIndicator
            // 
            this.gbIndicator.Controls.Add(this.lcdIndicator);
            this.gbIndicator.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbIndicator.Location = new System.Drawing.Point(0, 0);
            this.gbIndicator.Name = "gbIndicator";
            this.gbIndicator.Padding = new System.Windows.Forms.Padding(10, 4, 10, 10);
            this.gbIndicator.Size = new System.Drawing.Size(499, 185);
            this.gbIndicator.TabIndex = 6;
            this.gbIndicator.TabStop = false;
            this.gbIndicator.Text = "Редактор строк";
            // 
            // lcdIndicator
            // 
            this.lcdIndicator.BackColor = System.Drawing.Color.Gainsboro;
            this.lcdIndicator.Location = new System.Drawing.Point(10, 17);
            this.lcdIndicator.MaximumSize = new System.Drawing.Size(476, 156);
            this.lcdIndicator.MinimumSize = new System.Drawing.Size(476, 156);
            this.lcdIndicator.Name = "lcdIndicator";
            this.lcdIndicator.ShowGrid = true;
            this.lcdIndicator.Size = new System.Drawing.Size(476, 156);
            this.lcdIndicator.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(269, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(5, 503);
            this.panel8.TabIndex = 9;
            // 
            // gbRows
            // 
            this.gbRows.Controls.Add(this.tvRows);
            this.gbRows.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbRows.Location = new System.Drawing.Point(5, 0);
            this.gbRows.Name = "gbRows";
            this.gbRows.Padding = new System.Windows.Forms.Padding(7, 3, 7, 5);
            this.gbRows.Size = new System.Drawing.Size(264, 503);
            this.gbRows.TabIndex = 8;
            this.gbRows.TabStop = false;
            this.gbRows.Text = "Строки пульта";
            // 
            // tvRows
            // 
            this.tvRows.AllowDrop = true;
            this.tvRows.ContextMenuStrip = this.ViewContextMenu;
            this.tvRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRows.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvRows.FullRowSelect = true;
            this.tvRows.HideSelection = false;
            this.tvRows.ImageIndex = 0;
            this.tvRows.ImageList = this.ItemsList;
            this.tvRows.Location = new System.Drawing.Point(7, 16);
            this.tvRows.Name = "tvRows";
            this.tvRows.SelectedImageIndex = 0;
            this.tvRows.ShowSubNodesNumbers = true;
            this.tvRows.Size = new System.Drawing.Size(250, 482);
            this.tvRows.TabIndex = 0;
            this.tvRows.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvRows_AfterSelect);
            this.tvRows.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvRows_KeyPress);
            this.tvRows.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvRows_KeyDown);
            // 
            // ViewContextMenu
            // 
            this.ViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddView,
            this.miAddViewFromTop,
            this.miAddViewFromBottom,
            this.miDeleteView,
            this.toolStripSeparator1,
            this.miDisableView,
            this.miEnableView,
            this.toolStripSeparator3,
            this.miCutView,
            this.miCopyView,
            this.miCopyAsText,
            this.miPasteView,
            this.miReplaceView});
            this.ViewContextMenu.Name = "ViewContextMenu";
            this.ViewContextMenu.Size = new System.Drawing.Size(269, 258);
            this.ViewContextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.ViewContextMenu_Closed);
            this.ViewContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ViewContextMenu_Opening);
            // 
            // miAddView
            // 
            this.miAddView.Image = ((System.Drawing.Image)(resources.GetObject("miAddView.Image")));
            this.miAddView.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddView.Name = "miAddView";
            this.miAddView.Size = new System.Drawing.Size(268, 22);
            this.miAddView.Text = "Добавить вид";
            this.miAddView.Click += new System.EventHandler(this.miAddView_Click);
            // 
            // miAddViewFromTop
            // 
            this.miAddViewFromTop.Image = ((System.Drawing.Image)(resources.GetObject("miAddViewFromTop.Image")));
            this.miAddViewFromTop.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddViewFromTop.Name = "miAddViewFromTop";
            this.miAddViewFromTop.Size = new System.Drawing.Size(268, 22);
            this.miAddViewFromTop.Text = "Добавить вид выше";
            this.miAddViewFromTop.Click += new System.EventHandler(this.miAddViewFromTop_Click);
            // 
            // miAddViewFromBottom
            // 
            this.miAddViewFromBottom.Image = ((System.Drawing.Image)(resources.GetObject("miAddViewFromBottom.Image")));
            this.miAddViewFromBottom.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddViewFromBottom.Name = "miAddViewFromBottom";
            this.miAddViewFromBottom.Size = new System.Drawing.Size(268, 22);
            this.miAddViewFromBottom.Text = "Добавить вид ниже";
            this.miAddViewFromBottom.Click += new System.EventHandler(this.miAddViewFromBottom_Click);
            // 
            // miDeleteView
            // 
            this.miDeleteView.Image = ((System.Drawing.Image)(resources.GetObject("miDeleteView.Image")));
            this.miDeleteView.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miDeleteView.Name = "miDeleteView";
            this.miDeleteView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.miDeleteView.Size = new System.Drawing.Size(268, 22);
            this.miDeleteView.Text = "Удалить вид";
            this.miDeleteView.Click += new System.EventHandler(this.miDeleteView_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(265, 6);
            // 
            // miDisableView
            // 
            this.miDisableView.Name = "miDisableView";
            this.miDisableView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.miDisableView.Size = new System.Drawing.Size(268, 22);
            this.miDisableView.Text = "Отключить вид";
            this.miDisableView.Click += new System.EventHandler(this.miDisableView_Click);
            // 
            // miEnableView
            // 
            this.miEnableView.Name = "miEnableView";
            this.miEnableView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.miEnableView.Size = new System.Drawing.Size(268, 22);
            this.miEnableView.Text = "Подключить вид";
            this.miEnableView.Click += new System.EventHandler(this.miEnableView_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(265, 6);
            // 
            // miCutView
            // 
            this.miCutView.Image = ((System.Drawing.Image)(resources.GetObject("miCutView.Image")));
            this.miCutView.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCutView.Name = "miCutView";
            this.miCutView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miCutView.Size = new System.Drawing.Size(268, 22);
            this.miCutView.Text = "Вырезать";
            this.miCutView.Click += new System.EventHandler(this.miCutView_Click);
            // 
            // miCopyView
            // 
            this.miCopyView.Image = ((System.Drawing.Image)(resources.GetObject("miCopyView.Image")));
            this.miCopyView.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCopyView.Name = "miCopyView";
            this.miCopyView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miCopyView.Size = new System.Drawing.Size(268, 22);
            this.miCopyView.Text = "Копировать";
            this.miCopyView.Click += new System.EventHandler(this.miCopyView_Click);
            // 
            // miCopyAsText
            // 
            this.miCopyAsText.Name = "miCopyAsText";
            this.miCopyAsText.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.C)));
            this.miCopyAsText.Size = new System.Drawing.Size(268, 22);
            this.miCopyAsText.Text = "Копировать как текст";
            this.miCopyAsText.Click += new System.EventHandler(this.miCopyAsText_Click);
            // 
            // miPasteView
            // 
            this.miPasteView.Image = ((System.Drawing.Image)(resources.GetObject("miPasteView.Image")));
            this.miPasteView.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miPasteView.Name = "miPasteView";
            this.miPasteView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miPasteView.Size = new System.Drawing.Size(268, 22);
            this.miPasteView.Text = "Вставить";
            this.miPasteView.Click += new System.EventHandler(this.miPasteView_Click);
            // 
            // miReplaceView
            // 
            this.miReplaceView.Image = ((System.Drawing.Image)(resources.GetObject("miReplaceView.Image")));
            this.miReplaceView.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miReplaceView.Name = "miReplaceView";
            this.miReplaceView.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.V)));
            this.miReplaceView.Size = new System.Drawing.Size(268, 22);
            this.miReplaceView.Text = "Заменить";
            this.miReplaceView.Click += new System.EventHandler(this.miReplaceView_Click);
            // 
            // ItemsList
            // 
            this.ItemsList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ItemsList.ImageStream")));
            this.ItemsList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.ItemsList.Images.SetKeyName(0, "VSObject_Structure.bmp");
            this.ItemsList.Images.SetKeyName(1, "VSObject_Field.bmp");
            this.ItemsList.Images.SetKeyName(2, "VSObject_Method.bmp");
            // 
            // StandartPultTabbedDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "StandartPultTabbedDocument";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(958, 520);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.gbVar.ResumeLayout(false);
            this.gbVar.PerformLayout();
            this.pIndicator.ResumeLayout(false);
            this.gbIndicator.ResumeLayout(false);
            this.gbRows.ResumeLayout(false);
            this.ViewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.GroupBox gbVar;
        private System.Windows.Forms.CheckBox cbHasSign;
        private System.Windows.Forms.CheckBox cbReadOnly;
        private System.Windows.Forms.TextBox tbVarMask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlVarNames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pIndicator;
        private System.Windows.Forms.GroupBox gbIndicator;
        private Kontel.Relkon.LCDIndicator lcdIndicator;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.GroupBox gbRows;
        private TreeViewEx tvRows;
        private System.Windows.Forms.Button bAddVar;
        private System.Windows.Forms.ImageList ItemsList;
        private System.Windows.Forms.ContextMenuStrip ViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem miAddViewFromTop;
        private System.Windows.Forms.ToolStripMenuItem miAddViewFromBottom;
        private System.Windows.Forms.ToolStripMenuItem miDeleteView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miDisableView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miCopyView;
        private System.Windows.Forms.ToolStripMenuItem miPasteView;
        private System.Windows.Forms.ToolStripMenuItem miReplaceView;
        private System.Windows.Forms.ToolStripMenuItem miCutView;
        private System.Windows.Forms.ToolStripMenuItem miAddView;
        private System.Windows.Forms.ToolStripMenuItem miEnableView;
        private System.Windows.Forms.ToolStripMenuItem miCopyAsText;











    }
}
