using System.Drawing;
namespace Kontel.Relkon
{
    partial class MainForm
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
            TD.SandDock.DocumentContainer documentContainer1;
            TD.SandDock.DockContainer dockContainer2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            TD.SandDock.DockingRules dockingRules2 = new TD.SandDock.DockingRules();
            TD.SandDock.DockingRules dockingRules3 = new TD.SandDock.DockingRules();
            TD.SandDock.DockContainer dockContainer1;
            TD.SandDock.DockingRules dockingRules4 = new TD.SandDock.DockingRules();
            TD.SandDock.DockingRules dockingRules1 = new TD.SandDock.DockingRules();
            this.DocumentManager2 = new TD.SandDock.SandDockManager();
            this.ErrorsList = new Kontel.Relkon.Components.ErrorList();
            this.Console = new Kontel.Relkon.Components.OutputList();
            this.InformationMessages = new Kontel.Relkon.Components.OutputList();
            this.SolutionExplorer = new Kontel.Relkon.Components.SolutionExplorer();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.dockContainer3 = new TD.SandDock.DockContainer();
            this.DebuggerParametersList = new Kontel.Relkon.Components.DebuggerParametersPanel();
            this.TooolStripContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiPultToolStripVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDebuggerToolStripVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiMainToolStripVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewProject = new System.Windows.Forms.ToolStripMenuItem();
            this.проектRelkonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenSolution = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miCloseProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveProjectAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.miPrintSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.miPrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.miPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.miPrintToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.miRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.miFindAndReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.miFind = new System.Windows.Forms.ToolStripMenuItem();
            this.miReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.miView = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowSolutionExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowErrorList = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.miInforamtionList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
            this.панелиИнсToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miPultToolStripVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.miDebuggerToolStripVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainToolStripVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.miFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditorFont = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
            this.miDefaultFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.miProject = new System.Windows.Forms.ToolStripMenuItem();
            this.miAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddNewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddExistingFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miCompile = new System.Windows.Forms.ToolStripMenuItem();
            this.miUploadProgramAndParams = new System.Windows.Forms.ToolStripMenuItem();
            this.miLoadProjectProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.miLoadProjectParams = new System.Windows.Forms.ToolStripMenuItem();
            this.miGetEmbVarsFromController = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.miProjectProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.miDebugger = new System.Windows.Forms.ToolStripMenuItem();
            this.miIO = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewMemory = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewVars = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewStructurs = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewGraphics = new System.Windows.Forms.ToolStripMenuItem();
            this.miSituations = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator32 = new System.Windows.Forms.ToolStripSeparator();
            this.miStartDebugger = new System.Windows.Forms.ToolStripMenuItem();
            this.miStopDebugger = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator33 = new System.Windows.Forms.ToolStripSeparator();
            this.miDebuggerOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.miUtilits = new System.Windows.Forms.ToolStripMenuItem();
            this.miModulesSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.kontelReLoaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miRunHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.DebuggerToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbViewIO = new System.Windows.Forms.ToolStripButton();
            this.tsbViewVars = new System.Windows.Forms.ToolStripButton();
            this.tsbViewStructs = new System.Windows.Forms.ToolStripButton();
            this.tsbViewMemory = new System.Windows.Forms.ToolStripButton();
            this.tsbViewGraphics = new System.Windows.Forms.ToolStripButton();
            this.tsbSituations = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator30 = new System.Windows.Forms.ToolStripSeparator();
            this.tbHex = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator31 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDebuggerStart = new System.Windows.Forms.ToolStripButton();
            this.tsbDebuggerStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator34 = new System.Windows.Forms.ToolStripSeparator();
            this.tbDebuggerOptions = new System.Windows.Forms.ToolStripButton();
            this.PultToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbPultTypeLabel = new System.Windows.Forms.ToolStripLabel();
            this.tsbPultType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClearPult = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbImportPultModel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSaveToFromatRelkon42 = new System.Windows.Forms.ToolStripButton();
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbNewProject = new System.Windows.Forms.ToolStripSplitButton();
            this.tsbNewRelkonProject = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCut = new System.Windows.Forms.ToolStripButton();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.tsbPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUndo = new System.Windows.Forms.ToolStripButton();
            this.tsbRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbEmulationMode = new System.Windows.Forms.ToolStripButton();
            this.tsbEmulationMode2 = new System.Windows.Forms.ToolStripButton();
            this.tsbCompile = new System.Windows.Forms.ToolStripButton();
            this.tsbProgrammer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPrintSetup = new System.Windows.Forms.ToolStripButton();
            this.tsbPrintPreview = new System.Windows.Forms.ToolStripButton();
            this.tsbPrint = new System.Windows.Forms.ToolStripButton();
            this.tsbPrintToFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmModulesSetup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator25 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSolutionExplorer = new System.Windows.Forms.ToolStripButton();
            this.tsbErrorList = new System.Windows.Forms.ToolStripButton();
            this.tsbConsole = new System.Windows.Forms.ToolStripButton();
            this.tbInformationList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFind = new System.Windows.Forms.ToolStripButton();
            this.tsbRunHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
            this.проектНаОсновеСуществующихФайловToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.CompileBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.UsefullImages = new System.Windows.Forms.ImageList(this.components);
            documentContainer1 = new TD.SandDock.DocumentContainer();
            dockContainer2 = new TD.SandDock.DockContainer();
            dockContainer1 = new TD.SandDock.DockContainer();
            dockContainer2.SuspendLayout();
            dockContainer1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.dockContainer3.SuspendLayout();
            this.TooolStripContextMenu.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.DebuggerToolStrip.SuspendLayout();
            this.PultToolStrip.SuspendLayout();
            this.MainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // DocumentManager
            // 
            this.DocumentManager2.DockSystemContainer = this.toolStripContainer1.ContentPanel;
            this.DocumentManager2.OwnerForm = this;
            this.DocumentManager2.ActiveTabbedDocumentChanged += new System.EventHandler(this.DocumentManager_ActiveTabbedDocumentChanged);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.Filter = "Файлы проектов (*.rp6)|*.rp6|Файлы программ (*.kon, *.c, *.asm)|*.kon;*.c;*.asm|К" +
    "арты памяти (*.map)|*.map|Файлы пульта (*.fpr, *.plt)|*.fpr;*.plt|Файлы параметр" +
    "ов отладчика (*.dbg)|*.dbg";
            // 
            // documentContainer1
            // 
            documentContainer1.ContentSize = 124;
            documentContainer1.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(491F, 124F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[0]);
            documentContainer1.Location = new System.Drawing.Point(258, 0);
            documentContainer1.Manager = this.DocumentManager2;
            documentContainer1.Name = "documentContainer1";
            documentContainer1.Size = new System.Drawing.Size(453, 331);
            documentContainer1.TabIndex = 3;
            // 
            // dockContainer2
            // 
            dockContainer2.ContentSize = 182;
            dockContainer2.Controls.Add(this.ErrorsList);
            dockContainer2.Controls.Add(this.Console);
            dockContainer2.Controls.Add(this.InformationMessages);
            dockContainer2.Dock = System.Windows.Forms.DockStyle.Bottom;
            dockContainer2.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(493F, 182F), System.Windows.Forms.Orientation.Vertical, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(493F, 182F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.ErrorsList)),
                        ((TD.SandDock.DockControl)(this.Console)),
                        ((TD.SandDock.DockControl)(this.InformationMessages))}, this.ErrorsList)))});
            dockContainer2.Location = new System.Drawing.Point(0, 331);
            dockContainer2.Manager = this.DocumentManager2;
            dockContainer2.Name = "dockContainer2";
            dockContainer2.Size = new System.Drawing.Size(711, 186);
            dockContainer2.TabIndex = 1;
            // 
            // ErrorsList
            // 
            this.ErrorsList.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.ErrorsList.DescriptionColumnWidth = 100;
            this.ErrorsList.FileColumnWidth = 100;
            this.ErrorsList.Guid = new System.Guid("d5d5d59c-7cb7-4f31-98f9-77cec051c546");
            this.ErrorsList.LastModifySizeColumnIndex = -1;
            this.ErrorsList.LineColumnWidth = 100;
            this.ErrorsList.Location = new System.Drawing.Point(0, 22);
            this.ErrorsList.Name = "ErrorsList";
            this.ErrorsList.Size = new System.Drawing.Size(711, 140);
            this.ErrorsList.TabImage = ((System.Drawing.Image)(resources.GetObject("ErrorsList.TabImage")));
            this.ErrorsList.TabIndex = 1;
            this.ErrorsList.TabText = "Список ошибок";
            this.ErrorsList.Text = "Список ошибок";
            this.ErrorsList.RelkonCompilationErrorShow += new Kontel.Relkon.Components.RelkonCompilationErrorEventHandler(this.ErrorsList_RelkonCompilationErrorShow);
            this.ErrorsList.ErrorFileMustOpen += new Kontel.Relkon.Components.FileEventHandler(this.ErrorsList_ErrorFileMustOpen);
            // 
            // Console
            // 
            dockingRules2.AllowDockBottom = true;
            dockingRules2.AllowDockLeft = false;
            dockingRules2.AllowDockRight = false;
            dockingRules2.AllowDockTop = false;
            dockingRules2.AllowFloat = false;
            dockingRules2.AllowTab = false;
            this.Console.DockingRules = dockingRules2;
            this.Console.Guid = new System.Guid("f5cc9265-a225-4aa4-a8bd-12cef3f429f1");
            this.Console.Location = new System.Drawing.Point(0, 0);
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(493, 140);
            this.Console.TabImage = ((System.Drawing.Image)(resources.GetObject("Console.TabImage")));
            this.Console.TabIndex = 1;
            this.Console.Text = "Консоль";
            this.Console.Visible = false;
            // 
            // InformationMessages
            // 
            dockingRules3.AllowDockBottom = true;
            dockingRules3.AllowDockLeft = false;
            dockingRules3.AllowDockRight = false;
            dockingRules3.AllowDockTop = false;
            dockingRules3.AllowFloat = false;
            dockingRules3.AllowTab = false;
            this.InformationMessages.DockingRules = dockingRules3;
            this.InformationMessages.Guid = new System.Guid("301645a0-0f7e-4f3f-8014-18075c9a1dbd");
            this.InformationMessages.Location = new System.Drawing.Point(0, 0);
            this.InformationMessages.Name = "InformationMessages";
            this.InformationMessages.Size = new System.Drawing.Size(493, 140);
            this.InformationMessages.TabImage = ((System.Drawing.Image)(resources.GetObject("InformationMessages.TabImage")));
            this.InformationMessages.TabIndex = 1;
            this.InformationMessages.Text = "Информационные сообщения";
            this.InformationMessages.Visible = false;
            // 
            // dockContainer1
            // 
            dockContainer1.ContentSize = 247;
            dockContainer1.Controls.Add(this.SolutionExplorer);
            dockContainer1.Dock = System.Windows.Forms.DockStyle.Right;
            dockContainer1.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(247F, 312F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(247F, 312F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.SolutionExplorer))}, this.SolutionExplorer)))});
            dockContainer1.Location = new System.Drawing.Point(711, 0);
            dockContainer1.Manager = this.DocumentManager2;
            dockContainer1.Name = "dockContainer1";
            dockContainer1.Size = new System.Drawing.Size(251, 517);
            dockContainer1.TabIndex = 0;
            // 
            // SolutionExplorer
            // 
            dockingRules4.AllowDockBottom = false;
            dockingRules4.AllowDockLeft = false;
            dockingRules4.AllowDockRight = true;
            dockingRules4.AllowDockTop = false;
            dockingRules4.AllowFloat = true;
            dockingRules4.AllowTab = false;
            this.SolutionExplorer.DockingRules = dockingRules4;
            this.SolutionExplorer.Guid = new System.Guid("f1738490-445c-4132-86fa-e5cc4afd48f0");
            this.SolutionExplorer.Location = new System.Drawing.Point(4, 18);
            this.SolutionExplorer.Name = "SolutionExplorer";
            this.SolutionExplorer.ShowOptions = false;
            this.SolutionExplorer.Size = new System.Drawing.Size(247, 475);
            this.SolutionExplorer.TabImage = ((System.Drawing.Image)(resources.GetObject("SolutionExplorer.TabImage")));
            this.SolutionExplorer.TabIndex = 0;
            this.SolutionExplorer.TabText = "Обозреватель проекта";
            this.SolutionExplorer.Text = "Обозреватель проекта";
            this.SolutionExplorer.ProperitiesTreeNodeDoubleClick += new Kontel.Relkon.Components.ProperitiesTreeNodeDoubleClickEventHandler(this.SolutionExplorer_ProperitiesTreeNodeDoubleClick);
            this.SolutionExplorer.FileTreeNodeDoubleClick += new Kontel.Relkon.Components.FileTreeNodeDoubleClickEventHandler(this.SolutionExplorer_FileTreeNodeDoubleClick);
            this.SolutionExplorer.FileTreeNodeTextChanged += new Kontel.Relkon.Components.FileTreeNodeTextChangedEventHandler(this.SolutionExplorer_FileTreeNodeTextChanged);
            this.SolutionExplorer.ControllerProgramSolutionTreeNodeTextChanged += new Kontel.Relkon.Components.ControllerProgramSolutionTreeNodeTextChangedEventHandler(this.SolutionExplorer_RelkonSolutionTreeNodeTextChanged);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.toolStripContainer1.ContentPanel.Controls.Add(documentContainer1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.dockContainer3);
            this.toolStripContainer1.ContentPanel.Controls.Add(dockContainer2);
            this.toolStripContainer1.ContentPanel.Controls.Add(dockContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(962, 517);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(962, 616);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.ContextMenuStrip = this.TooolStripContextMenu;
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.MainMenu);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.DebuggerToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.MainToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.PultToolStrip);
            // 
            // dockContainer3
            // 
            this.dockContainer3.ContentSize = 254;
            this.dockContainer3.Controls.Add(this.DebuggerParametersList);
            this.dockContainer3.Dock = System.Windows.Forms.DockStyle.Left;
            this.dockContainer3.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(214F, 312F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.DebuggerParametersList))}, this.DebuggerParametersList)))});
            this.dockContainer3.Location = new System.Drawing.Point(0, 0);
            this.dockContainer3.Manager = this.DocumentManager2;
            this.dockContainer3.Name = "dockContainer3";
            this.dockContainer3.Size = new System.Drawing.Size(258, 331);
            this.dockContainer3.TabIndex = 4;
            // 
            // DebuggerParametersList
            // 
            this.DebuggerParametersList.AutoScroll = true;
            this.DebuggerParametersList.DebuggerEngine = null;
            dockingRules1.AllowDockBottom = false;
            dockingRules1.AllowDockLeft = true;
            dockingRules1.AllowDockRight = true;
            dockingRules1.AllowDockTop = false;
            dockingRules1.AllowFloat = true;
            dockingRules1.AllowTab = false;
            this.DebuggerParametersList.DockingRules = dockingRules1;
            this.DebuggerParametersList.FloatingSize = new System.Drawing.Size(251, 661);
            this.DebuggerParametersList.Guid = new System.Guid("4987d871-2500-4487-995b-90165dd8c7af");
            this.DebuggerParametersList.Location = new System.Drawing.Point(0, 18);
            this.DebuggerParametersList.MinimumSize = new System.Drawing.Size(251, 50);
            this.DebuggerParametersList.Name = "DebuggerParametersList";
            this.DebuggerParametersList.Padding = new System.Windows.Forms.Padding(2);
            this.DebuggerParametersList.ShowOptions = false;
            this.DebuggerParametersList.Size = new System.Drawing.Size(254, 289);
            this.DebuggerParametersList.TabIndex = 1;
            this.DebuggerParametersList.Text = "Параметры отладчика";
            this.DebuggerParametersList.Closing += new TD.SandDock.DockControlClosingEventHandler(this.DebuggerParametersList_Closing);
            // 
            // TooolStripContextMenu
            // 
            this.TooolStripContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiPultToolStripVisible,
            this.cmiDebuggerToolStripVisible,
            this.cmiMainToolStripVisible});
            this.TooolStripContextMenu.Name = "TooolStripContextMenu";
            this.TooolStripContextMenu.Size = new System.Drawing.Size(144, 70);
            this.TooolStripContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TooolStripContextMenu_Opening);
            // 
            // cmiPultToolStripVisible
            // 
            this.cmiPultToolStripVisible.CheckOnClick = true;
            this.cmiPultToolStripVisible.Name = "cmiPultToolStripVisible";
            this.cmiPultToolStripVisible.Size = new System.Drawing.Size(143, 22);
            this.cmiPultToolStripVisible.Text = "&Пульты";
            this.cmiPultToolStripVisible.Click += new System.EventHandler(this.cmiPultToolStripVisible_Click);
            // 
            // cmiDebuggerToolStripVisible
            // 
            this.cmiDebuggerToolStripVisible.CheckOnClick = true;
            this.cmiDebuggerToolStripVisible.Name = "cmiDebuggerToolStripVisible";
            this.cmiDebuggerToolStripVisible.Size = new System.Drawing.Size(143, 22);
            this.cmiDebuggerToolStripVisible.Text = "О&тладчик";
            this.cmiDebuggerToolStripVisible.Click += new System.EventHandler(this.cmiDebuggerToolStripVisible_Click);
            // 
            // cmiMainToolStripVisible
            // 
            this.cmiMainToolStripVisible.CheckOnClick = true;
            this.cmiMainToolStripVisible.Name = "cmiMainToolStripVisible";
            this.cmiMainToolStripVisible.Size = new System.Drawing.Size(143, 22);
            this.cmiMainToolStripVisible.Text = "&Стандартная";
            this.cmiMainToolStripVisible.Click += new System.EventHandler(this.cmiMainToolStripVisible_Click);
            // 
            // MainMenu
            // 
            this.MainMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
            this.miView,
            this.miFormat,
            this.miProject,
            this.miDebugger,
            this.miUtilits,
            this.miHelp});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(962, 24);
            this.MainMenu.TabIndex = 3;
            this.MainMenu.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNewProject,
            this.miOpen,
            this.toolStripSeparator1,
            this.miClose,
            this.miCloseProject,
            this.toolStripSeparator3,
            this.miSave,
            this.miSaveAs,
            this.miSaveAll,
            this.miSaveProjectAs,
            this.toolStripSeparator4,
            this.miPrintSetup,
            this.miPrintPreview,
            this.miPrint,
            this.miPrintToFile,
            this.toolStripSeparator5,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(48, 20);
            this.miFile.Text = "&Файл";
            this.miFile.DropDownOpening += new System.EventHandler(this.miFile_DropDownOpening);
            // 
            // miNewProject
            // 
            this.miNewProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.проектRelkonToolStripMenuItem});
            this.miNewProject.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miNewProject.Name = "miNewProject";
            this.miNewProject.Size = new System.Drawing.Size(266, 22);
            this.miNewProject.Text = "&Новый проект";
            // 
            // проектRelkonToolStripMenuItem
            // 
            this.проектRelkonToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("проектRelkonToolStripMenuItem.Image")));
            this.проектRelkonToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.проектRelkonToolStripMenuItem.Name = "проектRelkonToolStripMenuItem";
            this.проектRelkonToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.проектRelkonToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.проектRelkonToolStripMenuItem.Text = "Проект Relkon (STM32)";
            this.проектRelkonToolStripMenuItem.Click += new System.EventHandler(this.tsbNewRelkonProject_Click);
            // 
            // miOpen
            // 
            this.miOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenSolution,
            this.miOpenFile});
            this.miOpen.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.miOpen.Name = "miOpen";
            this.miOpen.Size = new System.Drawing.Size(266, 22);
            this.miOpen.Text = "О&ткрыть";
            // 
            // miOpenSolution
            // 
            this.miOpenSolution.Image = ((System.Drawing.Image)(resources.GetObject("miOpenSolution.Image")));
            this.miOpenSolution.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miOpenSolution.Name = "miOpenSolution";
            this.miOpenSolution.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.miOpenSolution.Size = new System.Drawing.Size(189, 22);
            this.miOpenSolution.Text = "Проект";
            this.miOpenSolution.Click += new System.EventHandler(this.miOpenSolution_Click);
            // 
            // miOpenFile
            // 
            this.miOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("miOpenFile.Image")));
            this.miOpenFile.Name = "miOpenFile";
            this.miOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miOpenFile.Size = new System.Drawing.Size(189, 22);
            this.miOpenFile.Text = "Файл";
            this.miOpenFile.Click += new System.EventHandler(this.miOpenFile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(263, 6);
            // 
            // miClose
            // 
            this.miClose.Enabled = false;
            this.miClose.Name = "miClose";
            this.miClose.Size = new System.Drawing.Size(266, 22);
            this.miClose.Text = "&Закрыть";
            this.miClose.Click += new System.EventHandler(this.miClose_Click);
            // 
            // miCloseProject
            // 
            this.miCloseProject.Enabled = false;
            this.miCloseProject.Image = ((System.Drawing.Image)(resources.GetObject("miCloseProject.Image")));
            this.miCloseProject.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCloseProject.Name = "miCloseProject";
            this.miCloseProject.Size = new System.Drawing.Size(266, 22);
            this.miCloseProject.Text = "&Закрыть проект";
            this.miCloseProject.Click += new System.EventHandler(this.miCloseProject_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(263, 6);
            // 
            // miSave
            // 
            this.miSave.Enabled = false;
            this.miSave.Image = ((System.Drawing.Image)(resources.GetObject("miSave.Image")));
            this.miSave.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miSave.Name = "miSave";
            this.miSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miSave.Size = new System.Drawing.Size(266, 22);
            this.miSave.Text = "&Сохранить";
            this.miSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // miSaveAs
            // 
            this.miSaveAs.Enabled = false;
            this.miSaveAs.Name = "miSaveAs";
            this.miSaveAs.Size = new System.Drawing.Size(266, 22);
            this.miSaveAs.Text = "Сохранить &как";
            this.miSaveAs.Click += new System.EventHandler(this.miSaveAs_Click);
            // 
            // miSaveAll
            // 
            this.miSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("miSaveAll.Image")));
            this.miSaveAll.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miSaveAll.Name = "miSaveAll";
            this.miSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.miSaveAll.Size = new System.Drawing.Size(266, 22);
            this.miSaveAll.Text = "Сохранить &все";
            this.miSaveAll.Click += new System.EventHandler(this.tsbSaveAll_Click);
            // 
            // miSaveProjectAs
            // 
            this.miSaveProjectAs.Enabled = false;
            this.miSaveProjectAs.Name = "miSaveProjectAs";
            this.miSaveProjectAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.miSaveProjectAs.Size = new System.Drawing.Size(266, 22);
            this.miSaveProjectAs.Text = "&Сохранить проект как...";
            this.miSaveProjectAs.Click += new System.EventHandler(this.miSaveProjectAs_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(263, 6);
            // 
            // miPrintSetup
            // 
            this.miPrintSetup.Enabled = false;
            this.miPrintSetup.Image = ((System.Drawing.Image)(resources.GetObject("miPrintSetup.Image")));
            this.miPrintSetup.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miPrintSetup.Name = "miPrintSetup";
            this.miPrintSetup.Size = new System.Drawing.Size(266, 22);
            this.miPrintSetup.Text = "&Параметры страницы";
            this.miPrintSetup.Click += new System.EventHandler(this.tsbPrintSetup_Click);
            // 
            // miPrintPreview
            // 
            this.miPrintPreview.Enabled = false;
            this.miPrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("miPrintPreview.Image")));
            this.miPrintPreview.Name = "miPrintPreview";
            this.miPrintPreview.Size = new System.Drawing.Size(266, 22);
            this.miPrintPreview.Text = "&Предварительный просмотр";
            this.miPrintPreview.Click += new System.EventHandler(this.tsbPrintPreview_Click);
            // 
            // miPrint
            // 
            this.miPrint.Enabled = false;
            this.miPrint.Image = ((System.Drawing.Image)(resources.GetObject("miPrint.Image")));
            this.miPrint.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miPrint.Name = "miPrint";
            this.miPrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.miPrint.Size = new System.Drawing.Size(266, 22);
            this.miPrint.Text = "&Печать...";
            this.miPrint.Click += new System.EventHandler(this.tsbPrint_Click);
            // 
            // miPrintToFile
            // 
            this.miPrintToFile.Enabled = false;
            this.miPrintToFile.Image = ((System.Drawing.Image)(resources.GetObject("miPrintToFile.Image")));
            this.miPrintToFile.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miPrintToFile.Name = "miPrintToFile";
            this.miPrintToFile.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.miPrintToFile.Size = new System.Drawing.Size(266, 22);
            this.miPrintToFile.Text = "&Печать в файл ";
            this.miPrintToFile.Click += new System.EventHandler(this.tsbPrintToFile_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(263, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.miExit.Size = new System.Drawing.Size(266, 22);
            this.miExit.Text = "&Выход";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miUndo,
            this.miRedo,
            this.toolStripSeparator6,
            this.miCut,
            this.miCopy,
            this.miPaste,
            this.miDelete,
            this.toolStripSeparator7,
            this.miSelectAll,
            this.toolStripSeparator8,
            this.miFindAndReplace});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(59, 20);
            this.miEdit.Text = "&Правка";
            // 
            // miUndo
            // 
            this.miUndo.Enabled = false;
            this.miUndo.Image = ((System.Drawing.Image)(resources.GetObject("miUndo.Image")));
            this.miUndo.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miUndo.Name = "miUndo";
            this.miUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.miUndo.Size = new System.Drawing.Size(190, 22);
            this.miUndo.Text = "О&тменить";
            this.miUndo.Click += new System.EventHandler(this.tsbUndo_Click);
            // 
            // miRedo
            // 
            this.miRedo.Enabled = false;
            this.miRedo.Image = ((System.Drawing.Image)(resources.GetObject("miRedo.Image")));
            this.miRedo.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miRedo.Name = "miRedo";
            this.miRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.miRedo.Size = new System.Drawing.Size(190, 22);
            this.miRedo.Text = "&Вернуть";
            this.miRedo.Click += new System.EventHandler(this.tsbRedo_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(187, 6);
            // 
            // miCut
            // 
            this.miCut.Enabled = false;
            this.miCut.Image = ((System.Drawing.Image)(resources.GetObject("miCut.Image")));
            this.miCut.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCut.Name = "miCut";
            this.miCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miCut.Size = new System.Drawing.Size(190, 22);
            this.miCut.Text = "&Вырезать";
            this.miCut.Click += new System.EventHandler(this.tsbCut_Click);
            // 
            // miCopy
            // 
            this.miCopy.Enabled = false;
            this.miCopy.Image = ((System.Drawing.Image)(resources.GetObject("miCopy.Image")));
            this.miCopy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCopy.Name = "miCopy";
            this.miCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miCopy.Size = new System.Drawing.Size(190, 22);
            this.miCopy.Text = "&Копировать";
            this.miCopy.Click += new System.EventHandler(this.tsbCopy_Click);
            // 
            // miPaste
            // 
            this.miPaste.Enabled = false;
            this.miPaste.Image = ((System.Drawing.Image)(resources.GetObject("miPaste.Image")));
            this.miPaste.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miPaste.Name = "miPaste";
            this.miPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miPaste.Size = new System.Drawing.Size(190, 22);
            this.miPaste.Text = "В&ставить";
            this.miPaste.Click += new System.EventHandler(this.tsbPaste_Click);
            // 
            // miDelete
            // 
            this.miDelete.Enabled = false;
            this.miDelete.Image = ((System.Drawing.Image)(resources.GetObject("miDelete.Image")));
            this.miDelete.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miDelete.Name = "miDelete";
            this.miDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.miDelete.Size = new System.Drawing.Size(190, 22);
            this.miDelete.Text = "У&далить";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(187, 6);
            // 
            // miSelectAll
            // 
            this.miSelectAll.Enabled = false;
            this.miSelectAll.Name = "miSelectAll";
            this.miSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.miSelectAll.Size = new System.Drawing.Size(190, 22);
            this.miSelectAll.Text = "Выделить в&се";
            this.miSelectAll.Click += new System.EventHandler(this.miSelectAll_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(187, 6);
            // 
            // miFindAndReplace
            // 
            this.miFindAndReplace.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFind,
            this.miReplace});
            this.miFindAndReplace.Enabled = false;
            this.miFindAndReplace.Name = "miFindAndReplace";
            this.miFindAndReplace.Size = new System.Drawing.Size(190, 22);
            this.miFindAndReplace.Text = "&Поиск и замена";
            // 
            // miFind
            // 
            this.miFind.Image = ((System.Drawing.Image)(resources.GetObject("miFind.Image")));
            this.miFind.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miFind.Name = "miFind";
            this.miFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.miFind.Size = new System.Drawing.Size(170, 22);
            this.miFind.Text = "&Найти";
            this.miFind.Click += new System.EventHandler(this.tsbFind_Click);
            // 
            // miReplace
            // 
            this.miReplace.Image = ((System.Drawing.Image)(resources.GetObject("miReplace.Image")));
            this.miReplace.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miReplace.Name = "miReplace";
            this.miReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.miReplace.Size = new System.Drawing.Size(170, 22);
            this.miReplace.Text = "&Заменить";
            this.miReplace.Click += new System.EventHandler(this.miReplace_Click);
            // 
            // miView
            // 
            this.miView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miShowSolutionExplorer,
            this.miShowErrorList,
            this.miShowOutput,
            this.miInforamtionList,
            this.toolStripSeparator24,
            this.панелиИнсToolStripMenuItem});
            this.miView.Name = "miView";
            this.miView.Size = new System.Drawing.Size(39, 20);
            this.miView.Text = "&Вид";
            // 
            // miShowSolutionExplorer
            // 
            this.miShowSolutionExplorer.Image = ((System.Drawing.Image)(resources.GetObject("miShowSolutionExplorer.Image")));
            this.miShowSolutionExplorer.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miShowSolutionExplorer.Name = "miShowSolutionExplorer";
            this.miShowSolutionExplorer.Size = new System.Drawing.Size(245, 22);
            this.miShowSolutionExplorer.Text = "&Файлы проекта";
            this.miShowSolutionExplorer.Click += new System.EventHandler(this.tsbSolutionExplorer_Click);
            // 
            // miShowErrorList
            // 
            this.miShowErrorList.Image = ((System.Drawing.Image)(resources.GetObject("miShowErrorList.Image")));
            this.miShowErrorList.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miShowErrorList.Name = "miShowErrorList";
            this.miShowErrorList.Size = new System.Drawing.Size(245, 22);
            this.miShowErrorList.Text = "&Сообщения об ошибках";
            this.miShowErrorList.Click += new System.EventHandler(this.tsbErrorList_Click);
            // 
            // miShowOutput
            // 
            this.miShowOutput.Image = ((System.Drawing.Image)(resources.GetObject("miShowOutput.Image")));
            this.miShowOutput.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miShowOutput.Name = "miShowOutput";
            this.miShowOutput.Size = new System.Drawing.Size(245, 22);
            this.miShowOutput.Text = "&Консоль";
            this.miShowOutput.Click += new System.EventHandler(this.tsbConsole_Click);
            // 
            // miInforamtionList
            // 
            this.miInforamtionList.Image = ((System.Drawing.Image)(resources.GetObject("miInforamtionList.Image")));
            this.miInforamtionList.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miInforamtionList.Name = "miInforamtionList";
            this.miInforamtionList.Size = new System.Drawing.Size(245, 22);
            this.miInforamtionList.Text = "И&нформационные сообщения";
            this.miInforamtionList.Click += new System.EventHandler(this.tbInformationList_Click);
            // 
            // toolStripSeparator24
            // 
            this.toolStripSeparator24.Name = "toolStripSeparator24";
            this.toolStripSeparator24.Size = new System.Drawing.Size(242, 6);
            // 
            // панелиИнсToolStripMenuItem
            // 
            this.панелиИнсToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPultToolStripVisible,
            this.miDebuggerToolStripVisible,
            this.miMainToolStripVisible});
            this.панелиИнсToolStripMenuItem.Name = "панелиИнсToolStripMenuItem";
            this.панелиИнсToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.панелиИнсToolStripMenuItem.Text = "&Панели инструментов";
            this.панелиИнсToolStripMenuItem.DropDownOpened += new System.EventHandler(this.панелиИнсToolStripMenuItem_DropDownOpened);
            // 
            // miPultToolStripVisible
            // 
            this.miPultToolStripVisible.CheckOnClick = true;
            this.miPultToolStripVisible.Name = "miPultToolStripVisible";
            this.miPultToolStripVisible.Size = new System.Drawing.Size(143, 22);
            this.miPultToolStripVisible.Text = "&Пульты";
            this.miPultToolStripVisible.Click += new System.EventHandler(this.cmiPultToolStripVisible_Click);
            // 
            // miDebuggerToolStripVisible
            // 
            this.miDebuggerToolStripVisible.CheckOnClick = true;
            this.miDebuggerToolStripVisible.Name = "miDebuggerToolStripVisible";
            this.miDebuggerToolStripVisible.Size = new System.Drawing.Size(143, 22);
            this.miDebuggerToolStripVisible.Text = "О&тладчик";
            this.miDebuggerToolStripVisible.Click += new System.EventHandler(this.cmiDebuggerToolStripVisible_Click);
            // 
            // miMainToolStripVisible
            // 
            this.miMainToolStripVisible.CheckOnClick = true;
            this.miMainToolStripVisible.Name = "miMainToolStripVisible";
            this.miMainToolStripVisible.Size = new System.Drawing.Size(143, 22);
            this.miMainToolStripVisible.Text = "&Стандартная";
            this.miMainToolStripVisible.Click += new System.EventHandler(this.cmiMainToolStripVisible_Click);
            // 
            // miFormat
            // 
            this.miFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditorFont,
            this.toolStripSeparator22,
            this.miDefaultFormat});
            this.miFormat.Name = "miFormat";
            this.miFormat.Size = new System.Drawing.Size(62, 20);
            this.miFormat.Text = "&Формат";
            // 
            // miEditorFont
            // 
            this.miEditorFont.Image = ((System.Drawing.Image)(resources.GetObject("miEditorFont.Image")));
            this.miEditorFont.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miEditorFont.Name = "miEditorFont";
            this.miEditorFont.Size = new System.Drawing.Size(159, 22);
            this.miEditorFont.Text = "&Шрифт";
            this.miEditorFont.Click += new System.EventHandler(this.miEditorFont_Click);
            // 
            // toolStripSeparator22
            // 
            this.toolStripSeparator22.Name = "toolStripSeparator22";
            this.toolStripSeparator22.Size = new System.Drawing.Size(156, 6);
            // 
            // miDefaultFormat
            // 
            this.miDefaultFormat.Name = "miDefaultFormat";
            this.miDefaultFormat.Size = new System.Drawing.Size(159, 22);
            this.miDefaultFormat.Text = "&По умолчанию";
            this.miDefaultFormat.Click += new System.EventHandler(this.miDefaultFormat_Click);
            // 
            // miProject
            // 
            this.miProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAdd,
            this.toolStripSeparator2,
            this.miCompile,
            this.miUploadProgramAndParams,
            this.miLoadProjectProgram,
            this.miLoadProjectParams,
            this.miGetEmbVarsFromController,
            this.toolStripSeparator9,
            this.miProjectProperties});
            this.miProject.Name = "miProject";
            this.miProject.Size = new System.Drawing.Size(59, 20);
            this.miProject.Text = "&Проект";
            // 
            // miAdd
            // 
            this.miAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddNewFile,
            this.miAddExistingFile});
            this.miAdd.Enabled = false;
            this.miAdd.Name = "miAdd";
            this.miAdd.Size = new System.Drawing.Size(213, 22);
            this.miAdd.Text = "&Добавить";
            this.miAdd.Visible = false;
            // 
            // miAddNewFile
            // 
            this.miAddNewFile.Image = ((System.Drawing.Image)(resources.GetObject("miAddNewFile.Image")));
            this.miAddNewFile.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddNewFile.Name = "miAddNewFile";
            this.miAddNewFile.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.miAddNewFile.Size = new System.Drawing.Size(274, 22);
            this.miAddNewFile.Text = "&Новый объект";
            // 
            // miAddExistingFile
            // 
            this.miAddExistingFile.Image = ((System.Drawing.Image)(resources.GetObject("miAddExistingFile.Image")));
            this.miAddExistingFile.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddExistingFile.Name = "miAddExistingFile";
            this.miAddExistingFile.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.miAddExistingFile.Size = new System.Drawing.Size(274, 22);
            this.miAddExistingFile.Text = "&Существующий объект";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(210, 6);
            this.toolStripSeparator2.Visible = false;
            // 
            // miCompile
            // 
            this.miCompile.Enabled = false;
            this.miCompile.Image = ((System.Drawing.Image)(resources.GetObject("miCompile.Image")));
            this.miCompile.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCompile.Name = "miCompile";
            this.miCompile.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.miCompile.Size = new System.Drawing.Size(213, 22);
            this.miCompile.Text = "&Компилировать";
            this.miCompile.Click += new System.EventHandler(this.tsbCompile_Click);
            // 
            // miUploadProgramAndParams
            // 
            this.miUploadProgramAndParams.Enabled = false;
            this.miUploadProgramAndParams.Image = ((System.Drawing.Image)(resources.GetObject("miUploadProgramAndParams.Image")));
            this.miUploadProgramAndParams.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miUploadProgramAndParams.Name = "miUploadProgramAndParams";
            this.miUploadProgramAndParams.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.miUploadProgramAndParams.Size = new System.Drawing.Size(213, 22);
            this.miUploadProgramAndParams.Text = "Загрузить &всё";
            this.miUploadProgramAndParams.Click += new System.EventHandler(this.tsbProgrammer_Click);
            // 
            // miLoadProjectProgram
            // 
            this.miLoadProjectProgram.Enabled = false;
            this.miLoadProjectProgram.Name = "miLoadProjectProgram";
            this.miLoadProjectProgram.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.miLoadProjectProgram.Size = new System.Drawing.Size(213, 22);
            this.miLoadProjectProgram.Text = "Загрузить &программу";
            this.miLoadProjectProgram.Click += new System.EventHandler(this.miLoadProjectProgram_Click);
            // 
            // miLoadProjectParams
            // 
            this.miLoadProjectParams.Enabled = false;
            this.miLoadProjectParams.Name = "miLoadProjectParams";
            this.miLoadProjectParams.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.miLoadProjectParams.Size = new System.Drawing.Size(213, 22);
            this.miLoadProjectParams.Text = "Загрузить &настройки";
            this.miLoadProjectParams.Click += new System.EventHandler(this.miLoadProjectParams_Click);
            // 
            // miGetEmbVarsFromController
            // 
            this.miGetEmbVarsFromController.Enabled = false;
            this.miGetEmbVarsFromController.Name = "miGetEmbVarsFromController";
            this.miGetEmbVarsFromController.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.miGetEmbVarsFromController.Size = new System.Drawing.Size(213, 22);
            this.miGetEmbVarsFromController.Text = "Прочитать &уставки";
            this.miGetEmbVarsFromController.Click += new System.EventHandler(this.miGetEmbVarsFromController_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(210, 6);
            // 
            // miProjectProperties
            // 
            this.miProjectProperties.Enabled = false;
            this.miProjectProperties.Image = ((System.Drawing.Image)(resources.GetObject("miProjectProperties.Image")));
            this.miProjectProperties.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miProjectProperties.Name = "miProjectProperties";
            this.miProjectProperties.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.miProjectProperties.Size = new System.Drawing.Size(213, 22);
            this.miProjectProperties.Text = "&Настройки";
            this.miProjectProperties.Click += new System.EventHandler(this.miProjectProperties_Click);
            // 
            // miDebugger
            // 
            this.miDebugger.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miIO,
            this.miViewMemory,
            this.miViewVars,
            this.miViewStructurs,
            this.miViewGraphics,
            this.miSituations,
            this.toolStripSeparator32,
            this.miStartDebugger,
            this.miStopDebugger,
            this.toolStripSeparator33,
            this.miDebuggerOptions});
            this.miDebugger.Name = "miDebugger";
            this.miDebugger.Size = new System.Drawing.Size(72, 20);
            this.miDebugger.Text = "О&тладчик";
            this.miDebugger.DropDownOpening += new System.EventHandler(this.miDebugger_DropDownOpening);
            // 
            // miIO
            // 
            this.miIO.Image = ((System.Drawing.Image)(resources.GetObject("miIO.Image")));
            this.miIO.Name = "miIO";
            this.miIO.Size = new System.Drawing.Size(221, 22);
            this.miIO.Text = "&Входы - выходы";
            this.miIO.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // miViewMemory
            // 
            this.miViewMemory.Image = ((System.Drawing.Image)(resources.GetObject("miViewMemory.Image")));
            this.miViewMemory.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miViewMemory.Name = "miViewMemory";
            this.miViewMemory.Size = new System.Drawing.Size(221, 22);
            this.miViewMemory.Text = "&Память контроллера";
            this.miViewMemory.Click += new System.EventHandler(this.miViewMemory_Click);
            // 
            // miViewVars
            // 
            this.miViewVars.Image = ((System.Drawing.Image)(resources.GetObject("miViewVars.Image")));
            this.miViewVars.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miViewVars.Name = "miViewVars";
            this.miViewVars.Size = new System.Drawing.Size(221, 22);
            this.miViewVars.Text = "&Переменные контроллера";
            this.miViewVars.Click += new System.EventHandler(this.miViewVars_Click);
            // 
            // miViewStructurs
            // 
            this.miViewStructurs.Image = ((System.Drawing.Image)(resources.GetObject("miViewStructurs.Image")));
            this.miViewStructurs.ImageTransparentColor = System.Drawing.SystemColors.ControlText;
            this.miViewStructurs.Name = "miViewStructurs";
            this.miViewStructurs.Size = new System.Drawing.Size(221, 22);
            this.miViewStructurs.Text = "&Структуры контроллера";
            this.miViewStructurs.Click += new System.EventHandler(this.miViewStructurs_Click);
            // 
            // miViewGraphics
            // 
            this.miViewGraphics.Image = ((System.Drawing.Image)(resources.GetObject("miViewGraphics.Image")));
            this.miViewGraphics.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miViewGraphics.Name = "miViewGraphics";
            this.miViewGraphics.Size = new System.Drawing.Size(221, 22);
            this.miViewGraphics.Text = "&Графики";
            this.miViewGraphics.Click += new System.EventHandler(this.miViewGraphics_Click);
            // 
            // miSituations
            // 
            this.miSituations.Image = ((System.Drawing.Image)(resources.GetObject("miSituations.Image")));
            this.miSituations.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miSituations.Name = "miSituations";
            this.miSituations.Size = new System.Drawing.Size(221, 22);
            this.miSituations.Text = "&Ситуации";
            this.miSituations.Click += new System.EventHandler(this.tsbSituations_Click);
            // 
            // toolStripSeparator32
            // 
            this.toolStripSeparator32.Name = "toolStripSeparator32";
            this.toolStripSeparator32.Size = new System.Drawing.Size(218, 6);
            // 
            // miStartDebugger
            // 
            this.miStartDebugger.Image = ((System.Drawing.Image)(resources.GetObject("miStartDebugger.Image")));
            this.miStartDebugger.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miStartDebugger.Name = "miStartDebugger";
            this.miStartDebugger.Size = new System.Drawing.Size(221, 22);
            this.miStartDebugger.Text = "&Запустить опрос";
            this.miStartDebugger.Click += new System.EventHandler(this.tsbDebuggerStart_Click);
            // 
            // miStopDebugger
            // 
            this.miStopDebugger.Enabled = false;
            this.miStopDebugger.Image = ((System.Drawing.Image)(resources.GetObject("miStopDebugger.Image")));
            this.miStopDebugger.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miStopDebugger.Name = "miStopDebugger";
            this.miStopDebugger.Size = new System.Drawing.Size(221, 22);
            this.miStopDebugger.Text = "Остановить опрос";
            this.miStopDebugger.Click += new System.EventHandler(this.tsbDebuggerStop_Click);
            // 
            // toolStripSeparator33
            // 
            this.toolStripSeparator33.Name = "toolStripSeparator33";
            this.toolStripSeparator33.Size = new System.Drawing.Size(218, 6);
            // 
            // miDebuggerOptions
            // 
            this.miDebuggerOptions.Image = ((System.Drawing.Image)(resources.GetObject("miDebuggerOptions.Image")));
            this.miDebuggerOptions.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miDebuggerOptions.Name = "miDebuggerOptions";
            this.miDebuggerOptions.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.miDebuggerOptions.Size = new System.Drawing.Size(221, 22);
            this.miDebuggerOptions.Text = "Панель управления";
            this.miDebuggerOptions.Click += new System.EventHandler(this.tbDebuggerOptions_Click);
            // 
            // miUtilits
            // 
            this.miUtilits.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miModulesSetup,
            this.kontelReLoaderToolStripMenuItem});
            this.miUtilits.Name = "miUtilits";
            this.miUtilits.Size = new System.Drawing.Size(66, 20);
            this.miUtilits.Text = "У&тилиты";
            // 
            // miModulesSetup
            // 
            this.miModulesSetup.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miModulesSetup.Name = "miModulesSetup";
            this.miModulesSetup.Size = new System.Drawing.Size(240, 22);
            this.miModulesSetup.Text = "&Настройка модулей Matchbox";
            this.miModulesSetup.Click += new System.EventHandler(this.miModulesSetup_Click);
            // 
            // kontelReLoaderToolStripMenuItem
            // 
            this.kontelReLoaderToolStripMenuItem.Name = "kontelReLoaderToolStripMenuItem";
            this.kontelReLoaderToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.kontelReLoaderToolStripMenuItem.Text = "Kontel ReLoader";
            this.kontelReLoaderToolStripMenuItem.Click += new System.EventHandler(this.kontelReLoaderToolStripMenuItem_Click_1);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRunHelp,
            this.miAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(65, 20);
            this.miHelp.Text = "&Справка";
            // 
            // miRunHelp
            // 
            this.miRunHelp.Enabled = false;
            this.miRunHelp.Image = ((System.Drawing.Image)(resources.GetObject("miRunHelp.Image")));
            this.miRunHelp.Name = "miRunHelp";
            this.miRunHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.miRunHelp.Size = new System.Drawing.Size(175, 22);
            this.miRunHelp.Text = "&Вызов справки";
            // 
            // miAbout
            // 
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(175, 22);
            this.miAbout.Text = "О &программе";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // DebuggerToolStrip
            // 
            this.DebuggerToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.DebuggerToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbViewIO,
            this.tsbViewVars,
            this.tsbViewStructs,
            this.tsbViewMemory,
            this.tsbViewGraphics,
            this.tsbSituations,
            this.toolStripSeparator30,
            this.tbHex,
            this.toolStripSeparator31,
            this.tsbDebuggerStart,
            this.tsbDebuggerStop,
            this.toolStripSeparator34,
            this.tbDebuggerOptions});
            this.DebuggerToolStrip.Location = new System.Drawing.Point(3, 24);
            this.DebuggerToolStrip.Name = "DebuggerToolStrip";
            this.DebuggerToolStrip.Size = new System.Drawing.Size(831, 25);
            this.DebuggerToolStrip.TabIndex = 8;
            // 
            // tsbViewIO
            // 
            this.tsbViewIO.Image = ((System.Drawing.Image)(resources.GetObject("tsbViewIO.Image")));
            this.tsbViewIO.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbViewIO.Name = "tsbViewIO";
            this.tsbViewIO.Size = new System.Drawing.Size(114, 22);
            this.tsbViewIO.Text = "Входы - выходы";
            this.tsbViewIO.ToolTipText = "Опрос датчиков ввода / вывода контроллера";
            this.tsbViewIO.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // tsbViewVars
            // 
            this.tsbViewVars.Image = ((System.Drawing.Image)(resources.GetObject("tsbViewVars.Image")));
            this.tsbViewVars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbViewVars.Name = "tsbViewVars";
            this.tsbViewVars.Size = new System.Drawing.Size(99, 22);
            this.tsbViewVars.Text = "Переменные";
            this.tsbViewVars.ToolTipText = "Просмотр значений переменных";
            this.tsbViewVars.Click += new System.EventHandler(this.miViewVars_Click);
            // 
            // tsbViewStructs
            // 
            this.tsbViewStructs.Image = ((System.Drawing.Image)(resources.GetObject("tsbViewStructs.Image")));
            this.tsbViewStructs.ImageTransparentColor = System.Drawing.SystemColors.ControlText;
            this.tsbViewStructs.Name = "tsbViewStructs";
            this.tsbViewStructs.Size = new System.Drawing.Size(86, 22);
            this.tsbViewStructs.Text = "Структуры";
            this.tsbViewStructs.ToolTipText = "Просмотр значений структур";
            this.tsbViewStructs.Click += new System.EventHandler(this.miViewStructurs_Click);
            // 
            // tsbViewMemory
            // 
            this.tsbViewMemory.Image = ((System.Drawing.Image)(resources.GetObject("tsbViewMemory.Image")));
            this.tsbViewMemory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbViewMemory.Name = "tsbViewMemory";
            this.tsbViewMemory.Size = new System.Drawing.Size(68, 22);
            this.tsbViewMemory.Text = "Память";
            this.tsbViewMemory.ToolTipText = "Просмотр памяти контроллера";
            this.tsbViewMemory.Click += new System.EventHandler(this.miViewMemory_Click);
            // 
            // tsbViewGraphics
            // 
            this.tsbViewGraphics.Image = ((System.Drawing.Image)(resources.GetObject("tsbViewGraphics.Image")));
            this.tsbViewGraphics.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbViewGraphics.Name = "tsbViewGraphics";
            this.tsbViewGraphics.Size = new System.Drawing.Size(75, 22);
            this.tsbViewGraphics.Text = "Графики";
            this.tsbViewGraphics.ToolTipText = "Графическое отображение данных";
            this.tsbViewGraphics.Click += new System.EventHandler(this.miViewGraphics_Click);
            // 
            // tsbSituations
            // 
            this.tsbSituations.Image = ((System.Drawing.Image)(resources.GetObject("tsbSituations.Image")));
            this.tsbSituations.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSituations.Name = "tsbSituations";
            this.tsbSituations.Size = new System.Drawing.Size(80, 22);
            this.tsbSituations.Text = "Ситуации";
            this.tsbSituations.ToolTipText = "Активные ситуации";
            this.tsbSituations.Click += new System.EventHandler(this.tsbSituations_Click);
            // 
            // toolStripSeparator30
            // 
            this.toolStripSeparator30.Name = "toolStripSeparator30";
            this.toolStripSeparator30.Size = new System.Drawing.Size(6, 25);
            // 
            // tbHex
            // 
            this.tbHex.CheckOnClick = true;
            this.tbHex.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbHex.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbHex.Name = "tbHex";
            this.tbHex.Size = new System.Drawing.Size(31, 22);
            this.tbHex.Text = "Hex";
            this.tbHex.ToolTipText = "Шестнадцатеричное / десятичное отображение";
            this.tbHex.CheckedChanged += new System.EventHandler(this.tbHex_CheckedChanged);
            // 
            // toolStripSeparator31
            // 
            this.toolStripSeparator31.Name = "toolStripSeparator31";
            this.toolStripSeparator31.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDebuggerStart
            // 
            this.tsbDebuggerStart.Image = ((System.Drawing.Image)(resources.GetObject("tsbDebuggerStart.Image")));
            this.tsbDebuggerStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDebuggerStart.Name = "tsbDebuggerStart";
            this.tsbDebuggerStart.Size = new System.Drawing.Size(58, 22);
            this.tsbDebuggerStart.Text = "Старт";
            this.tsbDebuggerStart.ToolTipText = "Запуск опроса контроллера";
            this.tsbDebuggerStart.Click += new System.EventHandler(this.tsbDebuggerStart_Click);
            // 
            // tsbDebuggerStop
            // 
            this.tsbDebuggerStop.Enabled = false;
            this.tsbDebuggerStop.Image = ((System.Drawing.Image)(resources.GetObject("tsbDebuggerStop.Image")));
            this.tsbDebuggerStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDebuggerStop.Name = "tsbDebuggerStop";
            this.tsbDebuggerStop.Size = new System.Drawing.Size(54, 22);
            this.tsbDebuggerStop.Text = "Стоп";
            this.tsbDebuggerStop.ToolTipText = "Остановка опроса контроллера";
            this.tsbDebuggerStop.Click += new System.EventHandler(this.tsbDebuggerStop_Click);
            // 
            // toolStripSeparator34
            // 
            this.toolStripSeparator34.Name = "toolStripSeparator34";
            this.toolStripSeparator34.Size = new System.Drawing.Size(6, 25);
            // 
            // tbDebuggerOptions
            // 
            this.tbDebuggerOptions.Image = ((System.Drawing.Image)(resources.GetObject("tbDebuggerOptions.Image")));
            this.tbDebuggerOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbDebuggerOptions.Name = "tbDebuggerOptions";
            this.tbDebuggerOptions.Size = new System.Drawing.Size(136, 22);
            this.tbDebuggerOptions.Text = "Панель управления";
            this.tbDebuggerOptions.ToolTipText = "Открытие панели управления отладчика";
            this.tbDebuggerOptions.Click += new System.EventHandler(this.tbDebuggerOptions_Click);
            // 
            // PultToolStrip
            // 
            this.PultToolStrip.ContextMenuStrip = this.TooolStripContextMenu;
            this.PultToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.PultToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPultTypeLabel,
            this.tsbPultType,
            this.toolStripSeparator17,
            this.tsbClearPult,
            this.toolStripSeparator18,
            this.tsbImportPultModel,
            this.toolStripSeparator23,
            this.tsbSaveToFromatRelkon42});
            this.PultToolStrip.Location = new System.Drawing.Point(3, 74);
            this.PultToolStrip.Name = "PultToolStrip";
            this.PultToolStrip.Size = new System.Drawing.Size(640, 25);
            this.PultToolStrip.TabIndex = 5;
            // 
            // tsbPultTypeLabel
            // 
            this.tsbPultTypeLabel.Enabled = false;
            this.tsbPultTypeLabel.Name = "tsbPultTypeLabel";
            this.tsbPultTypeLabel.Size = new System.Drawing.Size(71, 22);
            this.tsbPultTypeLabel.Text = "Тип пульта:";
            // 
            // tsbPultType
            // 
            this.tsbPultType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsbPultType.Enabled = false;
            this.tsbPultType.Name = "tsbPultType";
            this.tsbPultType.Size = new System.Drawing.Size(121, 25);
            this.tsbPultType.SelectedIndexChanged += new System.EventHandler(this.tsbPultType_SelectedIndexChanged);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbClearPult
            // 
            this.tsbClearPult.Enabled = false;
            this.tsbClearPult.Image = ((System.Drawing.Image)(resources.GetObject("tsbClearPult.Image")));
            this.tsbClearPult.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearPult.Name = "tsbClearPult";
            this.tsbClearPult.Size = new System.Drawing.Size(79, 22);
            this.tsbClearPult.Text = "Очистить";
            this.tsbClearPult.Click += new System.EventHandler(this.tsbClearPult_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbImportPultModel
            // 
            this.tsbImportPultModel.Enabled = false;
            this.tsbImportPultModel.Image = ((System.Drawing.Image)(resources.GetObject("tsbImportPultModel.Image")));
            this.tsbImportPultModel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportPultModel.Name = "tsbImportPultModel";
            this.tsbImportPultModel.Size = new System.Drawing.Size(134, 22);
            this.tsbImportPultModel.Text = "Загрузить из файла";
            this.tsbImportPultModel.Click += new System.EventHandler(this.tsbImportPultModel_Click);
            // 
            // toolStripSeparator23
            // 
            this.toolStripSeparator23.Name = "toolStripSeparator23";
            this.toolStripSeparator23.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSaveToFromatRelkon42
            // 
            this.tsbSaveToFromatRelkon42.Enabled = false;
            this.tsbSaveToFromatRelkon42.Image = ((System.Drawing.Image)(resources.GetObject("tsbSaveToFromatRelkon42.Image")));
            this.tsbSaveToFromatRelkon42.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveToFromatRelkon42.Name = "tsbSaveToFromatRelkon42";
            this.tsbSaveToFromatRelkon42.Size = new System.Drawing.Size(203, 22);
            this.tsbSaveToFromatRelkon42.Text = "Сохранить в формате Relkon 4.2";
            this.tsbSaveToFromatRelkon42.ToolTipText = "Сохранить файл пультов в формате Relkon 4.2";
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.ContextMenuStrip = this.TooolStripContextMenu;
            this.MainToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewProject,
            this.tsbOpen,
            this.tsbSave,
            this.tsbSaveAll,
            this.toolStripSeparator10,
            this.tsbCut,
            this.tsbCopy,
            this.tsbPaste,
            this.toolStripSeparator11,
            this.tsbUndo,
            this.tsbRedo,
            this.toolStripSeparator12,
            this.tsbEmulationMode,
            this.tsbEmulationMode2,
            this.tsbCompile,
            this.tsbProgrammer,
            this.toolStripSeparator13,
            this.tsbPrintSetup,
            this.tsbPrintPreview,
            this.tsbPrint,
            this.tsbPrintToFile,
            this.toolStripSeparator14,
            this.tsmModulesSetup,
            this.toolStripSeparator25,
            this.tsbSolutionExplorer,
            this.tsbErrorList,
            this.tsbConsole,
            this.tbInformationList,
            this.toolStripSeparator16,
            this.tsbFind,
            this.tsbRunHelp});
            this.MainToolStrip.Location = new System.Drawing.Point(3, 49);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.Size = new System.Drawing.Size(615, 25);
            this.MainToolStrip.TabIndex = 4;
            // 
            // tsbNewProject
            // 
            this.tsbNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewRelkonProject});
            this.tsbNewProject.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewProject.Image")));
            this.tsbNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewProject.Name = "tsbNewProject";
            this.tsbNewProject.Size = new System.Drawing.Size(32, 22);
            this.tsbNewProject.ToolTipText = "Новый проект";
            this.tsbNewProject.ButtonClick += new System.EventHandler(this.tsbNewRelkonProject_Click);
            // 
            // tsbNewRelkonProject
            // 
            this.tsbNewRelkonProject.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewRelkonProject.Image")));
            this.tsbNewRelkonProject.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.tsbNewRelkonProject.Name = "tsbNewRelkonProject";
            this.tsbNewRelkonProject.Size = new System.Drawing.Size(153, 22);
            this.tsbNewRelkonProject.Text = "Проект Relkon";
            this.tsbNewRelkonProject.Click += new System.EventHandler(this.tsbNewRelkonProject_Click);
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpen.Image")));
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbOpen.Text = "Открыть";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Enabled = false;
            this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "Сохранить";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbSaveAll
            // 
            this.tsbSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbSaveAll.Image")));
            this.tsbSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveAll.Name = "tsbSaveAll";
            this.tsbSaveAll.Size = new System.Drawing.Size(23, 22);
            this.tsbSaveAll.Text = "Сохранить все";
            this.tsbSaveAll.Click += new System.EventHandler(this.tsbSaveAll_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCut
            // 
            this.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCut.Enabled = false;
            this.tsbCut.Image = ((System.Drawing.Image)(resources.GetObject("tsbCut.Image")));
            this.tsbCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCut.Name = "tsbCut";
            this.tsbCut.Size = new System.Drawing.Size(23, 22);
            this.tsbCut.Text = "Вырезать";
            this.tsbCut.Click += new System.EventHandler(this.tsbCut_Click);
            // 
            // tsbCopy
            // 
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopy.Enabled = false;
            this.tsbCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsbCopy.Image")));
            this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Size = new System.Drawing.Size(23, 22);
            this.tsbCopy.Text = "Копировать";
            this.tsbCopy.Click += new System.EventHandler(this.tsbCopy_Click);
            // 
            // tsbPaste
            // 
            this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaste.Enabled = false;
            this.tsbPaste.Image = ((System.Drawing.Image)(resources.GetObject("tsbPaste.Image")));
            this.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPaste.Name = "tsbPaste";
            this.tsbPaste.Size = new System.Drawing.Size(23, 22);
            this.tsbPaste.Text = "Вставить";
            this.tsbPaste.Click += new System.EventHandler(this.tsbPaste_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbUndo
            // 
            this.tsbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUndo.Enabled = false;
            this.tsbUndo.Image = ((System.Drawing.Image)(resources.GetObject("tsbUndo.Image")));
            this.tsbUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUndo.Name = "tsbUndo";
            this.tsbUndo.Size = new System.Drawing.Size(23, 22);
            this.tsbUndo.Text = "Отменить";
            this.tsbUndo.Click += new System.EventHandler(this.tsbUndo_Click);
            // 
            // tsbRedo
            // 
            this.tsbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRedo.Enabled = false;
            this.tsbRedo.Image = ((System.Drawing.Image)(resources.GetObject("tsbRedo.Image")));
            this.tsbRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRedo.Name = "tsbRedo";
            this.tsbRedo.Size = new System.Drawing.Size(23, 22);
            this.tsbRedo.Text = "Вернуть";
            this.tsbRedo.Click += new System.EventHandler(this.tsbRedo_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbEmulationMode
            // 
            this.tsbEmulationMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEmulationMode.Enabled = false;
            this.tsbEmulationMode.Image = ((System.Drawing.Image)(resources.GetObject("tsbEmulationMode.Image")));
            this.tsbEmulationMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEmulationMode.Name = "tsbEmulationMode";
            this.tsbEmulationMode.Size = new System.Drawing.Size(23, 22);
            this.tsbEmulationMode.Text = "Включить режим эмуляции";
            this.tsbEmulationMode.Click += new System.EventHandler(this.tsbEmulationMode_Click);
            // 
            // tsbEmulationMode2
            // 
            this.tsbEmulationMode2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEmulationMode2.Enabled = false;
            this.tsbEmulationMode2.Image = ((System.Drawing.Image)(resources.GetObject("tsbEmulationMode2.Image")));
            this.tsbEmulationMode2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEmulationMode2.Name = "tsbEmulationMode2";
            this.tsbEmulationMode2.Size = new System.Drawing.Size(23, 22);
            this.tsbEmulationMode2.Text = "Включить режим эмуляции входов";
            this.tsbEmulationMode2.Click += new System.EventHandler(this.tsbEmulationMode2_Click);
            // 
            // tsbCompile
            // 
            this.tsbCompile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCompile.Enabled = false;
            this.tsbCompile.Image = ((System.Drawing.Image)(resources.GetObject("tsbCompile.Image")));
            this.tsbCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCompile.Name = "tsbCompile";
            this.tsbCompile.Size = new System.Drawing.Size(23, 22);
            this.tsbCompile.Text = "Компиляция";
            this.tsbCompile.Click += new System.EventHandler(this.tsbCompile_Click);
            // 
            // tsbProgrammer
            // 
            this.tsbProgrammer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbProgrammer.Enabled = false;
            this.tsbProgrammer.Image = ((System.Drawing.Image)(resources.GetObject("tsbProgrammer.Image")));
            this.tsbProgrammer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbProgrammer.Name = "tsbProgrammer";
            this.tsbProgrammer.Size = new System.Drawing.Size(23, 22);
            this.tsbProgrammer.Text = "Прошить контролер";
            this.tsbProgrammer.Click += new System.EventHandler(this.tsbProgrammer_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbPrintSetup
            // 
            this.tsbPrintSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrintSetup.Enabled = false;
            this.tsbPrintSetup.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrintSetup.Image")));
            this.tsbPrintSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrintSetup.Name = "tsbPrintSetup";
            this.tsbPrintSetup.Size = new System.Drawing.Size(23, 22);
            this.tsbPrintSetup.Text = "Параметры страницы";
            this.tsbPrintSetup.Click += new System.EventHandler(this.tsbPrintSetup_Click);
            // 
            // tsbPrintPreview
            // 
            this.tsbPrintPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrintPreview.Enabled = false;
            this.tsbPrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrintPreview.Image")));
            this.tsbPrintPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrintPreview.Name = "tsbPrintPreview";
            this.tsbPrintPreview.Size = new System.Drawing.Size(23, 22);
            this.tsbPrintPreview.Text = "Предварительный просмотр";
            this.tsbPrintPreview.Click += new System.EventHandler(this.tsbPrintPreview_Click);
            // 
            // tsbPrint
            // 
            this.tsbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrint.Enabled = false;
            this.tsbPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrint.Image")));
            this.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Size = new System.Drawing.Size(23, 22);
            this.tsbPrint.Text = "Печать";
            this.tsbPrint.Click += new System.EventHandler(this.tsbPrint_Click);
            // 
            // tsbPrintToFile
            // 
            this.tsbPrintToFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrintToFile.Enabled = false;
            this.tsbPrintToFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrintToFile.Image")));
            this.tsbPrintToFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrintToFile.Name = "tsbPrintToFile";
            this.tsbPrintToFile.Size = new System.Drawing.Size(23, 22);
            this.tsbPrintToFile.Text = "Печать в файл";
            this.tsbPrintToFile.Click += new System.EventHandler(this.tsbPrintToFile_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // tsmModulesSetup
            // 
            this.tsmModulesSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsmModulesSetup.Image = global::Kontel.Relkon.Properties.Resources.IOModule;
            this.tsmModulesSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmModulesSetup.Name = "tsmModulesSetup";
            this.tsmModulesSetup.Size = new System.Drawing.Size(23, 22);
            this.tsmModulesSetup.ToolTipText = "Настройка адресов модулей Matchbox";
            this.tsmModulesSetup.Click += new System.EventHandler(this.miModulesSetup_Click);
            // 
            // toolStripSeparator25
            // 
            this.toolStripSeparator25.Name = "toolStripSeparator25";
            this.toolStripSeparator25.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSolutionExplorer
            // 
            this.tsbSolutionExplorer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSolutionExplorer.Image = ((System.Drawing.Image)(resources.GetObject("tsbSolutionExplorer.Image")));
            this.tsbSolutionExplorer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSolutionExplorer.Name = "tsbSolutionExplorer";
            this.tsbSolutionExplorer.Size = new System.Drawing.Size(23, 22);
            this.tsbSolutionExplorer.Text = "Файлы проекта";
            this.tsbSolutionExplorer.Click += new System.EventHandler(this.tsbSolutionExplorer_Click);
            // 
            // tsbErrorList
            // 
            this.tsbErrorList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbErrorList.Image = ((System.Drawing.Image)(resources.GetObject("tsbErrorList.Image")));
            this.tsbErrorList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbErrorList.Name = "tsbErrorList";
            this.tsbErrorList.Size = new System.Drawing.Size(23, 22);
            this.tsbErrorList.Text = "toolStripButton1";
            this.tsbErrorList.ToolTipText = "Список ошибок";
            this.tsbErrorList.Click += new System.EventHandler(this.tsbErrorList_Click);
            // 
            // tsbConsole
            // 
            this.tsbConsole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbConsole.Image = ((System.Drawing.Image)(resources.GetObject("tsbConsole.Image")));
            this.tsbConsole.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConsole.Name = "tsbConsole";
            this.tsbConsole.Size = new System.Drawing.Size(23, 22);
            this.tsbConsole.Text = "toolStripButton2";
            this.tsbConsole.ToolTipText = "Консоль";
            this.tsbConsole.Click += new System.EventHandler(this.tsbConsole_Click);
            // 
            // tbInformationList
            // 
            this.tbInformationList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbInformationList.Image = ((System.Drawing.Image)(resources.GetObject("tbInformationList.Image")));
            this.tbInformationList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbInformationList.Name = "tbInformationList";
            this.tbInformationList.Size = new System.Drawing.Size(23, 22);
            this.tbInformationList.Text = "toolStripButton3";
            this.tbInformationList.ToolTipText = "Информационные сообщения";
            this.tbInformationList.Click += new System.EventHandler(this.tbInformationList_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbFind
            // 
            this.tsbFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFind.Enabled = false;
            this.tsbFind.Image = ((System.Drawing.Image)(resources.GetObject("tsbFind.Image")));
            this.tsbFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFind.Name = "tsbFind";
            this.tsbFind.Size = new System.Drawing.Size(23, 22);
            this.tsbFind.Text = "Поиск и замена";
            this.tsbFind.Click += new System.EventHandler(this.tsbFind_Click);
            // 
            // tsbRunHelp
            // 
            this.tsbRunHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsbRunHelp.Image")));
            this.tsbRunHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunHelp.Name = "tsbRunHelp";
            this.tsbRunHelp.Size = new System.Drawing.Size(23, 22);
            this.tsbRunHelp.Text = "Вызов справки";
            // 
            // toolStripSeparator21
            // 
            this.toolStripSeparator21.Name = "toolStripSeparator21";
            this.toolStripSeparator21.Size = new System.Drawing.Size(6, 6);
            // 
            // проектНаОсновеСуществующихФайловToolStripMenuItem
            // 
            this.проектНаОсновеСуществующихФайловToolStripMenuItem.Name = "проектНаОсновеСуществующихФайловToolStripMenuItem";
            this.проектНаОсновеСуществующихФайловToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            this.toolStripSeparator20.Size = new System.Drawing.Size(6, 6);
            // 
            // CompileBackgroundWorker
            // 
            this.CompileBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CompileBackgroundWorker_DoWork);
            this.CompileBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.CompileBackgroundWorker_RunWorkerCompleted);
            // 
            // UsefullImages
            // 
            this.UsefullImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("UsefullImages.ImageStream")));
            this.UsefullImages.TransparentColor = System.Drawing.Color.Fuchsia;
            this.UsefullImages.Images.SetKeyName(0, "InformationList.bmp");
            this.UsefullImages.Images.SetKeyName(1, "Debugger.bmp");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 638);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.DocumentsClosing += new System.EventHandler<Kontel.TabbedDocumentsForm.ClosingDocumentsEventArgs>(this.MainForm_DocumentsClosing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.LocationChanged += new System.EventHandler(this.MainForm_LocationChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Controls.SetChildIndex(this.toolStripContainer1, 0);
            dockContainer2.ResumeLayout(false);
            dockContainer1.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.dockContainer3.ResumeLayout(false);
            this.TooolStripContextMenu.ResumeLayout(false);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.DebuggerToolStrip.ResumeLayout(false);
            this.DebuggerToolStrip.PerformLayout();
            this.PultToolStrip.ResumeLayout(false);
            this.PultToolStrip.PerformLayout();
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

}

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private Kontel.Relkon.Components.OutputList Console;
        private Kontel.Relkon.Components.OutputList InformationMessages;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miNewProject;
        private System.Windows.Forms.ToolStripMenuItem проектRelkonToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator21;
        private System.Windows.Forms.ToolStripMenuItem проектНаОсновеСуществующихФайловToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miOpen;
        private System.Windows.Forms.ToolStripMenuItem miOpenSolution;
        private System.Windows.Forms.ToolStripMenuItem miOpenFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miClose;
        private System.Windows.Forms.ToolStripMenuItem miCloseProject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miSave;
        private System.Windows.Forms.ToolStripMenuItem miSaveAs;
        private System.Windows.Forms.ToolStripMenuItem miSaveAll;
        private System.Windows.Forms.ToolStripMenuItem miSaveProjectAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem miPrintSetup;
        private System.Windows.Forms.ToolStripMenuItem miPrintPreview;
        private System.Windows.Forms.ToolStripMenuItem miPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem miUndo;
        private System.Windows.Forms.ToolStripMenuItem miRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem miCut;
        private System.Windows.Forms.ToolStripMenuItem miCopy;
        private System.Windows.Forms.ToolStripMenuItem miPaste;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem miSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem miFindAndReplace;
        private System.Windows.Forms.ToolStripMenuItem miFind;
        private System.Windows.Forms.ToolStripMenuItem miReplace;
        private System.Windows.Forms.ToolStripMenuItem miView;
        private System.Windows.Forms.ToolStripMenuItem miShowSolutionExplorer;
        private System.Windows.Forms.ToolStripMenuItem miShowErrorList;
        private System.Windows.Forms.ToolStripMenuItem miShowOutput;
        private System.Windows.Forms.ToolStripMenuItem miInforamtionList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator24;
        private System.Windows.Forms.ToolStripMenuItem панелиИнсToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miMainToolStripVisible;
        private System.Windows.Forms.ToolStripMenuItem miPultToolStripVisible;
        private System.Windows.Forms.ToolStripMenuItem miFormat;
        private System.Windows.Forms.ToolStripMenuItem miEditorFont;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator22;
        private System.Windows.Forms.ToolStripMenuItem miDefaultFormat;
        private System.Windows.Forms.ToolStripMenuItem miProject;
        private System.Windows.Forms.ToolStripMenuItem miAdd;
        private System.Windows.Forms.ToolStripMenuItem miAddNewFile;
        private System.Windows.Forms.ToolStripMenuItem miAddExistingFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miCompile;
        private System.Windows.Forms.ToolStripMenuItem miUploadProgramAndParams;
        private System.Windows.Forms.ToolStripMenuItem miLoadProjectParams;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem miProjectProperties;
        private System.Windows.Forms.ToolStripMenuItem miUtilits;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miRunHelp;
        private System.Windows.Forms.ToolStripMenuItem miAbout;
        private System.Windows.Forms.ToolStrip MainToolStrip;
        private System.Windows.Forms.ToolStripSplitButton tsbNewProject;
        private System.Windows.Forms.ToolStripMenuItem tsbNewRelkonProject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;        
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton tsbCut;
        private System.Windows.Forms.ToolStripButton tsbCopy;
        private System.Windows.Forms.ToolStripButton tsbPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripButton tsbUndo;
        private System.Windows.Forms.ToolStripButton tsbRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripButton tsbEmulationMode;
        private System.Windows.Forms.ToolStripButton tsbCompile;
        private System.Windows.Forms.ToolStripButton tsbProgrammer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton tsbPrintSetup;
        private System.Windows.Forms.ToolStripButton tsbPrintPreview;
        private System.Windows.Forms.ToolStripButton tsbPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator25;
        private System.Windows.Forms.ToolStripButton tsbSolutionExplorer;
        private System.Windows.Forms.ToolStripButton tsbErrorList;
        private System.Windows.Forms.ToolStripButton tsbConsole;
        private System.Windows.Forms.ToolStripButton tbInformationList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripButton tsbFind;
        private System.Windows.Forms.ToolStripButton tsbRunHelp;
        private System.Windows.Forms.ToolStrip PultToolStrip;
        private System.Windows.Forms.ToolStripLabel tsbPultTypeLabel;
        internal System.Windows.Forms.ToolStripComboBox tsbPultType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripButton tsbClearPult;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripButton tsbImportPultModel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
        private System.Windows.Forms.ToolStripButton tsbSaveToFromatRelkon42;
        private System.Windows.Forms.ContextMenuStrip TooolStripContextMenu;
        private System.Windows.Forms.ToolStripMenuItem cmiMainToolStripVisible;
        private System.Windows.Forms.ToolStripMenuItem cmiPultToolStripVisible;
        private System.ComponentModel.BackgroundWorker CompileBackgroundWorker;
        private System.Windows.Forms.ImageList UsefullImages;
        private System.Windows.Forms.ToolStripButton tsbPrintToFile;
        private System.Windows.Forms.ToolStripMenuItem miPrintToFile;
        private System.Windows.Forms.ToolStripMenuItem miDebugger;
        private System.Windows.Forms.ToolStripMenuItem miIO;
        private System.Windows.Forms.ToolStripMenuItem miViewMemory;
        private System.Windows.Forms.ToolStripMenuItem miViewVars;
        private System.Windows.Forms.ToolStripMenuItem miViewGraphics;
        private System.Windows.Forms.ToolStripMenuItem miModulesSetup;
        private System.Windows.Forms.ToolStripButton tsmModulesSetup;
        private System.Windows.Forms.ToolStrip DebuggerToolStrip;
        private System.Windows.Forms.ToolStripButton tsbViewIO;
        private System.Windows.Forms.ToolStripButton tsbViewVars;
        private System.Windows.Forms.ToolStripButton tsbViewMemory;
        private System.Windows.Forms.ToolStripButton tsbViewGraphics;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator30;
        private System.Windows.Forms.ToolStripButton tsbDebuggerStart;
        private System.Windows.Forms.ToolStripButton tsbDebuggerStop;
        private System.Windows.Forms.ToolStripButton tbHex;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator31;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator32;
        private System.Windows.Forms.ToolStripMenuItem miStartDebugger;
        private System.Windows.Forms.ToolStripMenuItem miStopDebugger;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator33;
        private System.Windows.Forms.ToolStripMenuItem miDebuggerOptions;
        private System.Windows.Forms.ToolStripMenuItem cmiDebuggerToolStripVisible;
        private System.Windows.Forms.ToolStripMenuItem miDebuggerToolStripVisible;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator34;
        private System.Windows.Forms.ToolStripButton tbDebuggerOptions;
        private TD.SandDock.DockContainer dockContainer3;
        private System.Windows.Forms.ToolStripButton tsbSituations;
        private System.Windows.Forms.ToolStripMenuItem miSituations;
        private System.Windows.Forms.ToolStripMenuItem miViewStructurs;
        private System.Windows.Forms.ToolStripButton tsbViewStructs;
        private System.Windows.Forms.ToolStripButton tsbEmulationMode2;
        private System.Windows.Forms.ToolStripMenuItem miLoadProjectProgram;
        private System.Windows.Forms.ToolStripMenuItem miGetEmbVarsFromController;        
        private System.Windows.Forms.ToolStripMenuItem kontelReLoaderToolStripMenuItem;
        private Components.SolutionExplorer SolutionExplorer;
        private Components.DebuggerParametersPanel DebuggerParametersList;
        private Components.ErrorList ErrorsList;
        private TD.SandDock.SandDockManager DocumentManager2;


    }
}