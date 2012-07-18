namespace Kontel.Relkon.Components.Documents
{
    partial class EditorTabbedDocument
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorTabbedDocument));
            this.LanguageParser = new QWhale.Syntax.Schemes.LanguageParser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.editor = new QWhale.Editor.SyntaxEdit(this.components);
            this.EditiorContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.miRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miFind = new System.Windows.Forms.ToolStripMenuItem();
            this.miReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.EditiorContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // LanguageParser
            // 
            this.LanguageParser.DefaultState = 0;
            this.LanguageParser.Language = QWhale.Syntax.Schemes.Languages.Custom;
            this.LanguageParser.XmlScheme = resources.GetString("LanguageParser.XmlScheme");
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(585, 5);
            this.panel2.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(585, 1);
            this.panel5.TabIndex = 4;
            // 
            // editor
            // 
            this.editor.BackColor = System.Drawing.Color.White;
            this.editor.BorderStyle = QWhale.Common.EditBorderStyle.None;
            this.editor.ContextMenuStrip = this.EditiorContextMenu;
            this.editor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editor.Font = new System.Drawing.Font("Courier New", 10F);
            this.editor.Lexer = this.LanguageParser;
            this.editor.Location = new System.Drawing.Point(0, 6);
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(585, 394);
            this.editor.TabIndex = 5;
            this.editor.Text = "";
            this.editor.SourceStateChanged += new QWhale.Editor.NotifyEvent(this.Editor_SourceStateChanged);
            this.editor.TextChanged += new System.EventHandler(this.Editor_TextChanged);
            this.editor.DragDrop += new System.Windows.Forms.DragEventHandler(this.editor_DragDrop);
            // 
            // EditiorContextMenu
            // 
            this.EditiorContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCut,
            this.miCopy,
            this.miPaste,
            this.miDelete,
            this.toolStripSeparator1,
            this.miUndo,
            this.miRedo,
            this.toolStripSeparator2,
            this.miSelectAll,
            this.toolStripSeparator3,
            this.miFind,
            this.miReplace});
            this.EditiorContextMenu.Name = "EditiorContextMenu";
            this.EditiorContextMenu.Size = new System.Drawing.Size(196, 220);
            this.EditiorContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.EditiorContextMenu_Opening);
            // 
            // miCut
            // 
            this.miCut.Image = ((System.Drawing.Image)(resources.GetObject("miCut.Image")));
            this.miCut.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCut.Name = "miCut";
            this.miCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miCut.Size = new System.Drawing.Size(195, 22);
            this.miCut.Text = "&Вырезать";
            this.miCut.Click += new System.EventHandler(this.miCut_Click);
            // 
            // miCopy
            // 
            this.miCopy.Image = ((System.Drawing.Image)(resources.GetObject("miCopy.Image")));
            this.miCopy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCopy.Name = "miCopy";
            this.miCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miCopy.Size = new System.Drawing.Size(195, 22);
            this.miCopy.Text = "&Копировать";
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // miPaste
            // 
            this.miPaste.Image = ((System.Drawing.Image)(resources.GetObject("miPaste.Image")));
            this.miPaste.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miPaste.Name = "miPaste";
            this.miPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miPaste.Size = new System.Drawing.Size(195, 22);
            this.miPaste.Text = "&Вставить";
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // miDelete
            // 
            this.miDelete.Image = ((System.Drawing.Image)(resources.GetObject("miDelete.Image")));
            this.miDelete.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miDelete.Name = "miDelete";
            this.miDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.miDelete.Size = new System.Drawing.Size(195, 22);
            this.miDelete.Text = "У&далить";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // miUndo
            // 
            this.miUndo.Image = ((System.Drawing.Image)(resources.GetObject("miUndo.Image")));
            this.miUndo.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miUndo.Name = "miUndo";
            this.miUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.miUndo.Size = new System.Drawing.Size(195, 22);
            this.miUndo.Text = "О&тменить";
            this.miUndo.Click += new System.EventHandler(this.miUndo_Click);
            // 
            // miRedo
            // 
            this.miRedo.Image = ((System.Drawing.Image)(resources.GetObject("miRedo.Image")));
            this.miRedo.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miRedo.Name = "miRedo";
            this.miRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.miRedo.Size = new System.Drawing.Size(195, 22);
            this.miRedo.Text = "&Вернуть";
            this.miRedo.Click += new System.EventHandler(this.miRedo_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
            // 
            // miSelectAll
            // 
            this.miSelectAll.Name = "miSelectAll";
            this.miSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.miSelectAll.Size = new System.Drawing.Size(195, 22);
            this.miSelectAll.Text = "&Выделить все";
            this.miSelectAll.Click += new System.EventHandler(this.miSelectAll_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(192, 6);
            // 
            // miFind
            // 
            this.miFind.Image = ((System.Drawing.Image)(resources.GetObject("miFind.Image")));
            this.miFind.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miFind.Name = "miFind";
            this.miFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.miFind.Size = new System.Drawing.Size(195, 22);
            this.miFind.Text = "&Найти";
            this.miFind.Click += new System.EventHandler(this.miFind_Click);
            // 
            // miReplace
            // 
            this.miReplace.Image = ((System.Drawing.Image)(resources.GetObject("miReplace.Image")));
            this.miReplace.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miReplace.Name = "miReplace";
            this.miReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.miReplace.Size = new System.Drawing.Size(195, 22);
            this.miReplace.Text = "&Заменить";
            this.miReplace.Click += new System.EventHandler(this.miReplace_Click);
            // 
            // EditorTabbedDocument
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.editor);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "EditorTabbedDocument";
            this.Size = new System.Drawing.Size(585, 400);
            this.EditiorContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private QWhale.Syntax.Schemes.LanguageParser LanguageParser;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private QWhale.Editor.SyntaxEdit editor;
        private System.Windows.Forms.ContextMenuStrip EditiorContextMenu;
        private System.Windows.Forms.ToolStripMenuItem miCut;
        private System.Windows.Forms.ToolStripMenuItem miCopy;
        private System.Windows.Forms.ToolStripMenuItem miPaste;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miUndo;
        private System.Windows.Forms.ToolStripMenuItem miRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miFind;
        private System.Windows.Forms.ToolStripMenuItem miReplace;

    }
}
