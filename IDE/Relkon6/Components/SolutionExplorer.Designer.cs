using Kontel.Relkon;
namespace Kontel.Relkon.Components
{
    partial class SolutionExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionExplorer));
            this.tvSolutionExplorer = new Kontel.Relkon.TreeViewEx();
            this.SolutionExplorerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiAddNewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiAddNewPult = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiConvertLCDPanelProject = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperities = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCompile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiExcludeFromSolution = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiPultProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSetAsActivePult = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiAddExistingItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiAddExistingPult = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiAddLCDPanelProject = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiLoadToController = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemsIcons = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SolutionExplorerContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvSolutionExplorer
            // 
            this.tvSolutionExplorer.AllowDrop = true;
            this.tvSolutionExplorer.ContextMenuStrip = this.SolutionExplorerContextMenu;
            this.tvSolutionExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSolutionExplorer.ImageIndex = 0;
            this.tvSolutionExplorer.ImageList = this.ItemsIcons;
            this.tvSolutionExplorer.ItemHeight = 18;
            this.tvSolutionExplorer.LabelEdit = true;
            this.tvSolutionExplorer.Location = new System.Drawing.Point(0, 0);
            this.tvSolutionExplorer.Name = "tvSolutionExplorer";
            this.tvSolutionExplorer.SelectedImageIndex = 0;
            this.tvSolutionExplorer.ShowNodeToolTips = true;
            this.tvSolutionExplorer.ShowSubNodesNumbers = false;
            this.tvSolutionExplorer.Size = new System.Drawing.Size(250, 400);
            this.tvSolutionExplorer.TabIndex = 0;
            this.tvSolutionExplorer.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSolutionExplorer_NodeMouseDoubleClick);
            this.tvSolutionExplorer.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvSolutionExplorer_AfterLabelEdit);
            this.tvSolutionExplorer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSolutionExplorer_AfterSelect);
            this.tvSolutionExplorer.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSolutionExplorer_NodeMouseClick);
            this.tvSolutionExplorer.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvSolutionExplorer_BeforeLabelEdit);
            // 
            // SolutionExplorerContextMenu
            // 
            this.SolutionExplorerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiAddNewItem,
            this.cmiOpen,
            this.cmiRename,
            this.cmiConvertLCDPanelProject,
            this.cmiProperities,
            this.cmiCompile,
            this.cmiExcludeFromSolution,
            this.cmiPultProperties,
            this.cmiSetAsActivePult,
            this.cmiAddExistingItem,
            this.cmiLoadToController});
            this.SolutionExplorerContextMenu.Name = "SolutionExplorerContextMenu";
            this.SolutionExplorerContextMenu.Size = new System.Drawing.Size(250, 246);
            this.SolutionExplorerContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.SolutionExplorerContextMenu_Opening);
            // 
            // cmiAddNewItem
            // 
            this.cmiAddNewItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiAddNewPult});
            this.cmiAddNewItem.Name = "cmiAddNewItem";
            this.cmiAddNewItem.Size = new System.Drawing.Size(249, 22);
            this.cmiAddNewItem.Text = "&Добавить новый...";
            // 
            // cmiAddNewPult
            // 
            this.cmiAddNewPult.Image = ((System.Drawing.Image)(resources.GetObject("cmiAddNewPult.Image")));
            this.cmiAddNewPult.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cmiAddNewPult.Name = "cmiAddNewPult";
            this.cmiAddNewPult.Size = new System.Drawing.Size(143, 22);
            this.cmiAddNewPult.Text = "&Файл пульта";
            this.cmiAddNewPult.Click += new System.EventHandler(this.cmiAddNewPult_Click);
            // 
            // cmiOpen
            // 
            this.cmiOpen.Image = ((System.Drawing.Image)(resources.GetObject("cmiOpen.Image")));
            this.cmiOpen.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cmiOpen.Name = "cmiOpen";
            this.cmiOpen.Size = new System.Drawing.Size(249, 22);
            this.cmiOpen.Text = "О&ткрыть";
            this.cmiOpen.Click += new System.EventHandler(this.cmiOpen_Click);
            // 
            // cmiRename
            // 
            this.cmiRename.Name = "cmiRename";
            this.cmiRename.Size = new System.Drawing.Size(249, 22);
            this.cmiRename.Text = "&Переименовать";
            this.cmiRename.Click += new System.EventHandler(this.cmiRename_Click);
            // 
            // cmiConvertLCDPanelProject
            // 
            this.cmiConvertLCDPanelProject.Name = "cmiConvertLCDPanelProject";
            this.cmiConvertLCDPanelProject.Size = new System.Drawing.Size(249, 22);
            this.cmiConvertLCDPanelProject.Text = "По&дставить адреса переменных";
            this.cmiConvertLCDPanelProject.Click += new System.EventHandler(this.cmiConvertLCDPanelProject_Click);
            // 
            // cmiProperities
            // 
            this.cmiProperities.Image = ((System.Drawing.Image)(resources.GetObject("cmiProperities.Image")));
            this.cmiProperities.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cmiProperities.Name = "cmiProperities";
            this.cmiProperities.Size = new System.Drawing.Size(249, 22);
            this.cmiProperities.Text = "&Настройки";
            this.cmiProperities.Click += new System.EventHandler(this.cmiProperities_Click);
            // 
            // cmiCompile
            // 
            this.cmiCompile.Image = ((System.Drawing.Image)(resources.GetObject("cmiCompile.Image")));
            this.cmiCompile.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cmiCompile.Name = "cmiCompile";
            this.cmiCompile.Size = new System.Drawing.Size(249, 22);
            this.cmiCompile.Text = "&Компилировать";
            this.cmiCompile.Click += new System.EventHandler(this.cmiCompile_Click);
            // 
            // cmiExcludeFromSolution
            // 
            this.cmiExcludeFromSolution.Name = "cmiExcludeFromSolution";
            this.cmiExcludeFromSolution.Size = new System.Drawing.Size(249, 22);
            this.cmiExcludeFromSolution.Text = "И&сключить из проекта";
            this.cmiExcludeFromSolution.Click += new System.EventHandler(this.cmiExcludeFromSolution_Click);
            // 
            // cmiPultProperties
            // 
            this.cmiPultProperties.Image = ((System.Drawing.Image)(resources.GetObject("cmiPultProperties.Image")));
            this.cmiPultProperties.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cmiPultProperties.Name = "cmiPultProperties";
            this.cmiPultProperties.Size = new System.Drawing.Size(249, 22);
            this.cmiPultProperties.Text = "&Настройки";
            this.cmiPultProperties.Click += new System.EventHandler(this.cmiPultProperties_Click);
            // 
            // cmiSetAsActivePult
            // 
            this.cmiSetAsActivePult.Name = "cmiSetAsActivePult";
            this.cmiSetAsActivePult.Size = new System.Drawing.Size(249, 22);
            this.cmiSetAsActivePult.Text = "&Сделать активным";
            this.cmiSetAsActivePult.Click += new System.EventHandler(this.cmiSetAsActivePult_Click);
            // 
            // cmiAddExistingItem
            // 
            this.cmiAddExistingItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiAddExistingPult,
            this.cmiAddLCDPanelProject});
            this.cmiAddExistingItem.Name = "cmiAddExistingItem";
            this.cmiAddExistingItem.Size = new System.Drawing.Size(249, 22);
            this.cmiAddExistingItem.Text = "Добавить &существующий...";
            this.cmiAddExistingItem.DropDownOpening += new System.EventHandler(this.cmiAddExistingItem_DropDownOpening);
            // 
            // cmiAddExistingPult
            // 
            this.cmiAddExistingPult.Image = ((System.Drawing.Image)(resources.GetObject("cmiAddExistingPult.Image")));
            this.cmiAddExistingPult.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cmiAddExistingPult.Name = "cmiAddExistingPult";
            this.cmiAddExistingPult.Size = new System.Drawing.Size(184, 22);
            this.cmiAddExistingPult.Text = "Файл пульта";
            this.cmiAddExistingPult.Click += new System.EventHandler(this.cmiAddExistingPult_Click);
            // 
            // cmiAddLCDPanelProject
            // 
            this.cmiAddLCDPanelProject.Image = ((System.Drawing.Image)(resources.GetObject("cmiAddLCDPanelProject.Image")));
            this.cmiAddLCDPanelProject.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cmiAddLCDPanelProject.Name = "cmiAddLCDPanelProject";
            this.cmiAddLCDPanelProject.Size = new System.Drawing.Size(184, 22);
            this.cmiAddLCDPanelProject.Text = "Проект LCD-панели";
            this.cmiAddLCDPanelProject.Click += new System.EventHandler(this.cmiAddLCDPanelProject_Click);
            // 
            // cmiLoadToController
            // 
            this.cmiLoadToController.Image = ((System.Drawing.Image)(resources.GetObject("cmiLoadToController.Image")));
            this.cmiLoadToController.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cmiLoadToController.Name = "cmiLoadToController";
            this.cmiLoadToController.Size = new System.Drawing.Size(249, 22);
            this.cmiLoadToController.Text = "Прошить контроллер";
            this.cmiLoadToController.Click += new System.EventHandler(this.cmiLoadToController_Click);
            // 
            // ItemsIcons
            // 
            this.ItemsIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ItemsIcons.ImageStream")));
            this.ItemsIcons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.ItemsIcons.Images.SetKeyName(0, "AsmFile.bmp");
            this.ItemsIcons.Images.SetKeyName(1, "CFile.bmp");
            this.ItemsIcons.Images.SetKeyName(2, "MapFile.bmp");
            this.ItemsIcons.Images.SetKeyName(3, "ProjectProperities.bmp");
            this.ItemsIcons.Images.SetKeyName(4, "RelkonProgram.bmp");
            this.ItemsIcons.Images.SetKeyName(5, "RelkonProject.bmp");
            this.ItemsIcons.Images.SetKeyName(6, "UnavailableRelkonProgram.bmp");
            this.ItemsIcons.Images.SetKeyName(7, "LCDPanelProject.bmp");
            this.ItemsIcons.Images.SetKeyName(8, "UnavailableLCDPanelProject.bmp");
            this.ItemsIcons.Images.SetKeyName(9, "UnavailablePultFile.bmp");
            this.ItemsIcons.Images.SetKeyName(10, "PultFile.bmp");
            this.ItemsIcons.Images.SetKeyName(11, "SolutionExplorer.bmp");
            this.ItemsIcons.Images.SetKeyName(12, "ControllersFile.bmp");
            this.ItemsIcons.Images.SetKeyName(13, "TC65Solution.png");
            this.ItemsIcons.Images.SetKeyName(14, "Archive");
            this.ItemsIcons.Images.SetKeyName(15, "UnavailableControllersFile.bmp");
            this.ItemsIcons.Images.SetKeyName(16, "HFile.bmp");
            // 
            // SolutionExplorer
            // 
            this.Controls.Add(this.tvSolutionExplorer);
            this.Name = "SolutionExplorer";
            this.Text = "";
            this.SolutionExplorerContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewEx tvSolutionExplorer;
        private System.Windows.Forms.ContextMenuStrip SolutionExplorerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem cmiAddNewItem;
        private System.Windows.Forms.ToolStripMenuItem cmiAddNewPult;
        private System.Windows.Forms.ToolStripMenuItem cmiOpen;
        private System.Windows.Forms.ToolStripMenuItem cmiRename;
        private System.Windows.Forms.ToolStripMenuItem cmiConvertLCDPanelProject;
        private System.Windows.Forms.ToolStripMenuItem cmiProperities;
        private System.Windows.Forms.ToolStripMenuItem cmiCompile;
        private System.Windows.Forms.ToolStripMenuItem cmiExcludeFromSolution;
        private System.Windows.Forms.ToolStripMenuItem cmiPultProperties;
        private System.Windows.Forms.ToolStripMenuItem cmiSetAsActivePult;
        private System.Windows.Forms.ToolStripMenuItem cmiAddExistingItem;
        private System.Windows.Forms.ToolStripMenuItem cmiAddExistingPult;
        private System.Windows.Forms.ToolStripMenuItem cmiAddLCDPanelProject;
        private System.Windows.Forms.ImageList ItemsIcons;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem cmiLoadToController;
    }
}
