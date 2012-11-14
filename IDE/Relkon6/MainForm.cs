using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon.Forms;
using Kontel.Relkon.Solutions;
using System.Xml.Serialization;
using Kontel.Relkon.Components.Documents;
using System.IO;
using Kontel.Relkon;
using System.Diagnostics;
using TD.SandDock;
using Kontel.Relkon.Components;
using Kontel.Relkon.Classes;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Collections;
using Kontel.TabbedDocumentsForm;
using Kontel.Relkon.Debugger;
using System.Reflection;
using System.Threading;

namespace Kontel.Relkon
{
    public sealed partial class MainForm : Kontel.TabbedDocumentsForm.TabbedDocumentsForm
    {
        private string defaultCaption = "IDE Relkon"; // заголовок формы
        private ControllerProgramSolution solution = null; //хранит информацию о текущем проекте
        private FindReplaceForm findReplaceForm = null; // форма поиска и замены     
        private bool initialize = false; // становится true после полной инициализации формы
        private bool convertRequired = false; // показывает, требуется ли преобразование печати
        private ProgressForm progressForm = null; // отображает процесс загрузки данных        
        private DebuggerEngine debuggerEngine = null; // движок отладчика
        internal static MainForm MainFormInstance;

        private BackgroundWorker _bgEmbVarsReader = new BackgroundWorker();
            

        public MainForm(string FileName)
        {
            InitializeComponent();
            this.SetStartupParams();
            this.initialize = true;
            XmlSerializer xs = new XmlSerializer(typeof(ControllerProgramSolution));
            EditorTabbedDocument doc = new EditorTabbedDocument(null, "", null);
            doc.Dispose();
            MainForm.MainFormInstance = this;
            if (FileName != null && File.Exists(FileName) && FileCanLoadedWOSolution(FileName))
                this.LoadFile(FileName);
            this.DebuggerParametersList.DebuggertParametersUpdated += new EventHandler(DebuggerParametersList_DebuggertParametersUpdated);
            this.DebuggerParametersList.ProcessorChanged += new EventHandler<EventArgs<ProcessorType>>(DebuggerParametersListProcessesorChanged);          
        }
   

        #region Saving and loading settings
        /// <summary>
        /// Добавляет ToolStrip на указанную панель
        /// </summary>
        private void SetToolStripPanel(ToolStrip Strip, string PanelType)
        {
            if (PanelType == "Bottom")
                this.toolStripContainer1.BottomToolStripPanel.Controls.Add(Strip);
            if (PanelType == "Left")
                this.toolStripContainer1.LeftToolStripPanel.Controls.Add(Strip);
            if (PanelType == "Right")
                this.toolStripContainer1.RightToolStripPanel.Controls.Add(Strip);
        }
        /// <summary>
        /// Возвращает положение панели, на которой расположен ToolStrip - Left, Right, Top, Bottom
        /// </summary>
        private string GetToolStripPanel(ToolStrip Strip)
        {
            string res = "Top";
            if (Strip.Parent == this.toolStripContainer1.BottomToolStripPanel)
                res = "Bottom";
            if (Strip.Parent == this.toolStripContainer1.LeftToolStripPanel)
                res = "Left";
            if (Strip.Parent == this.toolStripContainer1.RightToolStripPanel)
                res = "Right";
            return res;
        }
        /// <summary>
        /// Сохраняет состояния панелей инструментов и главного меню
        /// </summary>
        private void SaveStripsState()
        {
            // MainToolStrip
            Program.Settings.MainToolStripPanel = this.GetToolStripPanel(this.MainToolStrip);
            Program.Settings.MainToolStriplLocation = this.MainToolStrip.Location;
            Program.Settings.MainToolStripVisible = this.MainToolStrip.Visible;
            // PultToolStrip
            Program.Settings.PultToolStripPanel = this.GetToolStripPanel(this.PultToolStrip);
            Program.Settings.PultToolStripLocation = this.PultToolStrip.Location;
            Program.Settings.PultToolStripVisible = this.PultToolStrip.Visible;
            // DebuggerToolStrip
            Program.Settings.DebuggerToolStripPanel = this.GetToolStripPanel(this.DebuggerToolStrip);
            Program.Settings.DebuggerToolStripLocation = this.DebuggerToolStrip.Location;
            Program.Settings.DebuggerToolStripVisible = this.DebuggerToolStrip.Visible;
            Program.Settings.DebuggerHex = this.tbHex.Checked;                       
            // FbdToolStrip
            //Program.Settings.FbdToolStripLocation = this.FbdToolStrip.Location;
        }
        /// <summary>
        /// Загружает состояния панелей инструментов и главного меню
        /// </summary>
        private void LoadStripsState()
        {
            // MainToolStrip
            this.SetToolStripPanel(this.MainToolStrip, Program.Settings.MainToolStripPanel);
            this.MainToolStrip.Location = Program.Settings.MainToolStriplLocation;
            this.MainToolStrip.Visible = Program.Settings.MainToolStripVisible;
            // PultToolStrip
            this.SetToolStripPanel(this.PultToolStrip, Program.Settings.PultToolStripPanel);
            this.PultToolStrip.Location = Program.Settings.PultToolStripLocation;
            this.PultToolStrip.Visible = Program.Settings.PultToolStripVisible;
            // DebuggerToolStrip
            this.SetToolStripPanel(this.DebuggerToolStrip, Program.Settings.DebuggerToolStripPanel);
            this.DebuggerToolStrip.Location = Program.Settings.DebuggerToolStripLocation;
            this.DebuggerToolStrip.Visible = Program.Settings.DebuggerToolStripVisible;
            this.tbHex.Checked = Program.Settings.DebuggerHex;          

            //this.FbdToolStrip.Location = Program.Settings.FbdToolStripLocation;
        }
        /// <summary>
        /// Сохраняет состояние Solution Explorer
        /// </summary>
        private void SaveSolutionExplorerState()
        {
            //Program.Settings.SolutionExplorerIsOpen = this.SolutionExplorer.IsOpen || this.SolutionExplorer.Collapsed;
            //Program.Settings.SolutionExplorerCollapsed = this.SolutionExplorer.Collapsed;
            //Program.Settings.SolutionExplorerFloatingSize = this.SolutionExplorer.FloatingSize;
            //Program.Settings.SolutionExplorerPopupSize = this.SolutionExplorer.PopupSize;
            //if (this.SolutionExplorer.Parent != null && this.SolutionExplorer.Parent.GetType() == typeof(TD.SandDock.DockContainer))
            //    Program.Settings.SolutionExplorerStaticSize = ((TD.SandDock.DockContainer)this.SolutionExplorer.Parent).Size;

        }
        /// <summary>
        /// Загружает состояние Solution Explorer
        /// </summary>
        private void LoadSolutionExplorerState()
        {
            this.SolutionExplorer.Close();
            if (Program.Settings.SolutionExplorerIsOpen)
                this.SolutionExplorer.Open();
            else
                this.SolutionExplorer.Close();
            this.SolutionExplorer.Collapsed = Program.Settings.SolutionExplorerCollapsed;
            this.SolutionExplorer.FloatingSize = Program.Settings.SolutionExplorerFloatingSize;
            this.SolutionExplorer.PopupSize = Program.Settings.SolutionExplorerPopupSize;
            if (this.SolutionExplorer.Parent != null && this.SolutionExplorer.Parent.GetType() == typeof(TD.SandDock.DockContainer))
                ((TD.SandDock.DockContainer)this.SolutionExplorer.Parent).Size = Program.Settings.SolutionExplorerStaticSize;
        }
        /// <summary>
        /// Загружает состояние вкладки настроек отладчика
        /// </summary>
        private void LoadDebuggerParametersListState()
        {
            this.DebuggerParametersList.Close();
            this.DebuggerParametersList.Collapsed = Program.Settings.DebuggerParametersListCollapsed;
            this.DebuggerParametersList.Size = this.DebuggerParametersList.FloatingSize = Program.Settings.DebuggerParametersListSize;
            //this.DebuggerParametersList.PopupSize = Program.Settings.DebuggerParametersListPopupSize;
            if (this.DebuggerParametersList.Parent != null && this.DebuggerParametersList.Parent.GetType() == typeof(TD.SandDock.DockContainer))
                ((TD.SandDock.DockContainer)this.DebuggerParametersList.Parent).Size = Program.Settings.DebuggerParametersListStaticSize;
            this.DebuggerParametersList.TabImage = this.UsefullImages.Images[1];
        }
        /// <summary>
        /// Сохраняет состояние вкладки настроек отладчика
        /// </summary>
        private void SaveDebuggerParametersListState()
        {
            Program.Settings.DebuggerParametersListCollapsed = this.DebuggerParametersList.Collapsed;
            Program.Settings.DebuggerParametersListCollapsed = this.DebuggerParametersList.Collapsed;
            Program.Settings.DebuggerParametersListSize = this.DebuggerParametersList.Size;
            Program.Settings.DebuggerParametersListPopupSize = this.DebuggerParametersList.PopupSize;
            if (this.DebuggerParametersList.Parent != null && this.DebuggerParametersList.Parent.GetType() == typeof(TD.SandDock.DockContainer))
                Program.Settings.DebuggerParametersListStaticSize = ((TD.SandDock.DockContainer)this.DebuggerParametersList.Parent).Size;
        }
        /// <summary>
        /// Сохраняет состояние Error list'а
        /// </summary>
        private void SaveErrorsListState()
        {
            Program.Settings.ErrorsListDescriptionColumnWidth = this.ErrorsList.DescriptionColumnWidth;
            Program.Settings.ErrorsListFileColumnWidth = this.ErrorsList.FileColumnWidth;
            Program.Settings.ErrorsListLineColumnWidth = this.ErrorsList.LineColumnWidth;
            Program.Settings.ErrorsListLastModifySizeColumnIndex = this.ErrorsList.LastModifySizeColumnIndex;
        }
        /// <summary>
        /// Загружает состояние Error list'а
        /// </summary>
        private void LoadErrorsListState()
        {
            this.ErrorsList.Close();
            this.ErrorsList.DescriptionColumnWidth = Program.Settings.ErrorsListDescriptionColumnWidth;
            this.ErrorsList.FileColumnWidth = Program.Settings.ErrorsListFileColumnWidth;
            this.ErrorsList.LineColumnWidth = Program.Settings.ErrorsListLineColumnWidth;
            //this.ErrorsList.ComputeLineNumberWidth();
            this.ErrorsList.LastModifySizeColumnIndex = Program.Settings.ErrorsListLastModifySizeColumnIndex;
        }
        /// <summary>
        /// Сохраняет состояние формы - размер, положение и т.д.
        /// </summary>
        private void SaveFormState()
        {
            Program.Settings.FormWindowState = (this.WindowState != FormWindowState.Minimized) ? this.WindowState : FormWindowState.Normal;
            this.SaveSolutionExplorerState();
            this.SaveErrorsListState();
            this.SaveStripsState();
            this.SaveDebuggerParametersListState();
            Program.Settings.Save();
        }
        /// <summary>
        /// Загружает ранее сохраненное состояние формы
        /// </summary>
        private void LoadFormState()
        {
            this.MainMenu.Location = new Point(0, 0);
            this.Location = Program.Settings.FormLocation;
            this.Size = Program.Settings.FormSize;
            this.WindowState = Program.Settings.FormWindowState;
            this.LoadSolutionExplorerState();
            this.LoadErrorsListState();
            this.LoadDebuggerParametersListState();
            this.LoadStripsState();
            this.Console.Close();
            this.InformationMessages.Close();
            this.DebuggerParametersList.Close();
            this.OpenFileDialog.InitialDirectory = Program.Settings.LastOpenedSolutionDirectory;
            this.SaveFileDialog.InitialDirectory = Program.Settings.LastOpenedSolutionDirectory;
        }
        #endregion

        /// <summary>
        /// Возвращает флаг отображения данных отладчика в шестнадцатеричной системе
        /// </summary>
        internal bool Hex
        {
            get
            {
                return this.tbHex.Checked;
            }
        }
        /// <summary>
        /// Устанавливает стартовые параметры формы
        /// </summary>
        private void SetStartupParams()
        {
            this.Text = this.defaultCaption;
            this.InformationMessages.TabImage = this.UsefullImages.Images[0];
            //this.SetProcessorListItems();
            //this.tsbProcessorType.SelectedIndex = 0;
            this.LoadFormState();
            this.ScanTimerToolStripItems = new ScanTimerToolStripItemsDelegate(this.SetTimerItemsEnabledState);
            this.StatusStrip.MouseDoubleClick += new MouseEventHandler(StatusStrip_MouseDoubleClick);
            //AT89C51ED2Solution.SDKDirectory = Program.RelkonDirectory + "\\SDK\\AT89C51ED2";
            //MB90F347Solution.SDKDirectory = Program.RelkonDirectory + "\\SDK\\MB90F347";
            STM32F107Solution.SDKDirectory = Program.RelkonDirectory + "\\Firmware\\STM32F107";
            STM32F107Solution.CompilerDirectory = Program.RelkonDirectory + "\\arm-gcc\\bin";
            this.SolutionExplorer.ControllerProgramSolutionNodeCompileClick += new EventHandler<EventArgs<ControllerProgramSolution>>(SolutionExplorer_ControllerProgramSolutionNodeCompileClick);           
        }
        /// <summary>
        /// Возвращает процессор, выранный в списке процессоров
        /// </summary>
        //private ProcessorType SelectedProcessor
        //{
        //    get
        //    {
        //        return (ProcessorType)this.tsbProcessorType.SelectedItem;
        //    }
        //    set
        //    {
        //        this.tsbProcessorType.SelectedItem = value;
        //    }
        //}
        /// <summary>
        /// Возвращает тип пульта, выбранный в списке панели инструментов
        /// </summary>
        internal PultType SelectedPultType
        {
            get
            {
                return (PultType)this.tsbPultType.SelectedItem;
            }
            set
            {
                this.tsbPultType.SelectedItem = value;
            }
        }
        /// <summary>
        /// Открывает форму поиска и замены; если форма уже открыта, то на нее переносится фокус ввода
        /// </summary>
        /// <param name="ReplaceMode">Если true, то форма будет открыта в режиме замены</param>
        internal void RunFindReplaceForm(bool ReplaceMode)
        {
            if (this.findReplaceForm != null)
            {
                this.findReplaceForm.Select();
                if (ReplaceMode)
                    this.findReplaceForm.SwitchToReplaceMode();
                else
                    this.findReplaceForm.SwitchToFindMode();
            }
            else
            {
                this.findReplaceForm = new FindReplaceForm(ReplaceMode, (EditorTabbedDocument)this.ActiveDocument);
                this.findReplaceForm.FormClosed += new FormClosedEventHandler(findReplaceForm_FormClosed);
                this.findReplaceForm.Owner = this;
                this.findReplaceForm.Show();
            }
        }
        /// <summary>
        /// Выводит в некотором месте текущую позицию курсора редактора
        /// </summary>
        private void SetPositionString(string PositionString)
        {
            this.PositionStatusLabel.Text = PositionString;
        }
        /// <summary>
        /// Устанавливает значение свойства Enabled для пунктов меню и кнопок ToolStrip, чье состояние 
        /// может измениться в произвольный момент времени и поэтому отслеживается по таймеру
        /// </summary>
        private void SetTimerItemsEnabledState()
        {
            IEditableTabbedDocument doc = this.ActiveDocument as IEditableTabbedDocument;
            this.tsbUndo.Enabled = this.miUndo.Enabled = (doc == null) ? false : doc.CanUndo;
            this.tsbRedo.Enabled = this.miRedo.Enabled = (doc == null) ? false : doc.CanRedo;
            this.tsbPaste.Enabled = this.miPaste.Enabled = (doc == null) ? false : doc.CanPaste;
            this.tsbPrintSetup.Enabled = this.miPrintSetup.Enabled = this.tsbPrintPreview.Enabled = 
                this.miPrintPreview.Enabled = this.tsbPrint.Enabled = this.miPrint.Enabled = (this.ActiveDocument == null) ? false : this.ActiveDocument is IPrintableTabbedDocument;
        }
        /// <summary>
        /// Возвращает массив специфичных для проекта Relkon элементов панели инструментов и
        /// главного меню
        /// </summary>
        private ToolStripItem[] ControllerSolutionToolStripItems
        {
            get
            {
                return new ToolStripItem[] { this.tsbEmulationMode, this.tsbEmulationMode2, this.tsbCompile, this.miCompile, 
                    /*this.tsbProcessorType, this.tsbProcessorTypeLabel, */this.miLoadProjectParams};
            }
        }
        /// <summary>
        /// Возвращает массив специфичных для проекта Relkon элементов панели инструментов и
        /// главного меню
        /// </summary>
        private ToolStripItem[] CommonSolutionToolStripItems
        {
            get
            {
                return new ToolStripItem[] { this.tsbProgrammer, this.miUploadProgramAndParams, this.miLoadProjectProgram, this.miSaveProjectAs, this.miCloseProject, this.miProjectProperties, this.miGetEmbVarsFromController };
            }
        }
        /// <summary>
        /// Возвращает массив специфичных для документов пультов элементов панели
        /// </summary>
        private ToolStripItem[] PultTabbedDocumentToolStripItems
        {
            get
            {
                return new ToolStripItem[] {this.tsbPultType, this.tsbPultTypeLabel, this.tsbClearPult, 
                    this.tsbImportPultModel, this.tsbSaveToFromatRelkon42, this.tsbPrintToFile, this.miPrintToFile};
            }
        }
        /// <summary>
        /// Возвращает массив специфичных для документов пультов элементов панели
        /// </summary>
        //private ToolStripItem[] FbdTabbedDocumentToolStripItems
        //{
        //    get
        //    {
        //        return new ToolStripItem[] { this.tsbSaveFbdAsBmp, this.tsbFbdFullView, this.tSBGenerateCodeFromFbd };
        //    }
        //}
        /// <summary>
        /// Возвращает массив специфичных для всех документов элементы панели
        /// инструментов и главного меню
        /// </summary>
        private ToolStripItem[] TabbedDocumentToolStripItems
        {
            get
            {
                return new ToolStripItem[] { this.miSave, this.miSaveAs, this.tsbSave, this.miClose, 
                                             this.miDelete, this.miSelectAll, this.miCut, this.tsbCut };
            }
        }
        /// <summary>
        /// Возвращает массив специфичных для документов редактора элементы панели
        /// инструментов и главного меню
        /// </summary>
        private ToolStripItem[] EditorTabbedDocumentToolStripItems
        {
            get
            {
                return new ToolStripItem[] { this.tsbFind, this.miFindAndReplace, this.miFind, this.miReplace, this.miDelete};
            }
        }
       
        /// <summary>
        /// Подключает / отключает специфичных для документов редактора элементы панели
        /// инструментов и главного меню
        /// </summary>
        private void SetEditorTabbedDocumentToolStripItemsEnabledState(bool state)
        {
            foreach (ToolStripItem item in this.EditorTabbedDocumentToolStripItems)
            {
                item.Enabled = state;
            }
        }
        /// <summary>
        /// Подключает / отключает специфичных для всех документов элементы панели
        /// инструментов и главного меню
        /// </summary>
        private void SetTabbedDocumentToolStripItemsEnabledState(bool state)
        {
            foreach (ToolStripItem item in this.TabbedDocumentToolStripItems)
            {
                item.Enabled = state;
            }
        }
        /// <summary>
        /// Отображает / скрывает специфичные для документов пультов
        /// элементы панели инструментов и главного меню
        /// </summary>
        private void SetPultTabbedDocumentToolStripItemsEnabledState(bool state)
        {
            foreach (ToolStripItem item in this.PultTabbedDocumentToolStripItems)
            {
                item.Enabled = state;
            }
        }
        //private void SetFbdTabbedDocumentToolStripItemsEnabledState(bool state)
        //{
        //    foreach (ToolStripItem item in this.FbdTabbedDocumentToolStripItems)
        //    {
        //        item.Enabled = state;
        //    }
        //}       
       
        /// <summary>
        /// Акивирует / дезактивирует специфичные для указанного проекта
        /// элементы панели инструментов и главного меню
        /// </summary>
        private void SetSolutionItemsEnabledState(ControllerProgramSolution solution, bool state)
        {
            ToolStripItem[] items = null;
            if (solution is ControllerProgramSolution)
                items = this.ControllerSolutionToolStripItems;           
            else
                throw new Exception("Тип проекта " + solution.GetType().ToString() + " не поддреживается");
            foreach (ToolStripItem item in items)
            {
                item.Enabled = state;
            }
            foreach (ToolStripItem item in this.CommonSolutionToolStripItems)
            {
                item.Enabled = state;
            }
        }
        /// <summary>
        /// Акивирует / дезактивирует панель инструментов для просмотра архива
        /// </summary>
        //private void SetArchiveItemsVisibleState(bool state)
        //{
        //    this.ArchiveToolStrip.Visible = this.ArchiveToolStrip.Enabled = state;
        //}        
        /// <summary>
        /// Заролняет список лоступных процессоров панели инструментов
        /// </summary>
        //private void SetProcessorListItems()
        //{
        //    for (int i = 0; i < this.AvailableProcessors.Length; i++)
        //        this.tsbProcessorType.Items.Add(this.AvailableProcessors[i]);
        //}
        /// <summary>
        /// Возвращает массив доступных процессов
        /// </summary>
        //private ProcessorType[] AvailableProcessors
        //{
        //    get
        //    {
        //        if (this.availableProcessors == null)
        //        {
        //            this.availableProcessors = new ProcessorType[2];
        //            this.availableProcessors[0] = ProcessorType.AT89C51ED2;
        //            this.availableProcessors[1] = ProcessorType.MB90F347;
        //        }
        //        return this.availableProcessors;
        //    }
        //}
        /// <summary>
        /// Заполняет список типов пультов панели инструментов для выбранного процессора
        /// </summary>
        private void SetPultListItems()
        {
            this.tsbPultType.Items.Clear();
            if (this.ControllerProgramSolution != null)
            {
                foreach (PultType pult in this.ControllerProgramSolution.PultParams.AvailablePultTypes)
                {
                    this.tsbPultType.Items.Add(pult);
                }
            }
            else
                this.tsbPultType.Items.Add(((StandartPultTabbedDocument)this.ActiveDocument).Pult.Type);
            this.SelectedPultType = ((StandartPultTabbedDocument)this.ActiveDocument).Pult.Type;
        }
        /// <summary>
        /// Отображает на экране окно файлов проекта
        /// </summary>
        private void ShowSolutionExplorer()
        {
            this.SolutionExplorer.Open();
        }
        /// <summary>
        /// Отображает на экране список ошибок
        /// </summary>
        private void ShowErrorsList()
        {
            this.ErrorsList.Open();
        }
        /// <summary>
        /// Отображает на экране окно выходного потока данных
        /// </summary>
        private void ShowConsole()
        {
            this.Console.Open();
        }
        /// <summary>
        /// Отображает на экране окно информационных сообщений
        /// </summary>
        private void ShowInforamtionMessagesList()
        {
            this.InformationMessages.Open();
        }
        /// <summary>
        /// Возвращает параметры отладчика из указанного проекта: если доступен файл параметров отладчика,
        /// то данные беруться из него; в противном случае - из параметров проекта
        /// </summary>
        private DebuggerParameters GetDebuggerParametersFromSolution(ControllerProgramSolution solution)
        {
            DebuggerParameters res = null;
            if (this.ControllerProgramSolution.DebuggerFileName != "")
            {
                // Загрузка параметров отладчика из файла
                try
                {
                    res = DebuggerParameters.FromFile(this.ControllerProgramSolution.DebuggerFileName);
                }
                catch { }
            }
            if(res == null)
            {
                // Загрузка параметров отладчика из настроек проекта
                res = new DebuggerParameters();
                res.ProcessorType = solution.ProcessorParams.Type;
                res.ControllerNumber = solution.ControllerAddress;
                //if (solution is AT89C51ED2Solution)
                //{
                //    res.BaudRate = ((AT89C51ED2Solution)solution).BaudRate;
                //    res.ProtocolType = ((AT89C51ED2Solution)solution).Protocol;
                //    res.ReadPassword = ((AT89C51ED2Solution)solution).ReadPassword;
                //    res.WritePassword = ((AT89C51ED2Solution)solution).WritePassword;
                //}
                //else if (solution is MB90F347Solution)
                //{
                //    res.BaudRate = ((MB90F347Solution)solution).Uarts[0].BaudRate;
                //    res.ProtocolType = ((MB90F347Solution)solution).Uarts[0].Protocol;
                //    res.ReadPassword = ((MB90F347Solution)solution).Uarts[0].ReadPassword;
                //    res.WritePassword = ((MB90F347Solution)solution).Uarts[0].WritePassword;
                //}
                if (solution is STM32F107Solution)
                {
                    res.BaudRate = ((STM32F107Solution)solution).Uarts[0].BaudRate;
                    res.ProtocolType = ((STM32F107Solution)solution).Uarts[0].Protocol;                   
                }
                else
                    throw new Exception("Проекты типа " + solution.GetType() + " не поддерживаются");
            }
            return res;
        }
        /// <summary>
        /// Инициализирует движок отладчика
        /// </summary>
        private void CreateDebuggerEngine()
        {
            DebuggerParameters prm = null;
            if (this.solution is ControllerProgramSolution)
            {
                prm = this.GetDebuggerParametersFromSolution(this.ControllerProgramSolution);
            }
            else
                prm = this.GetDebuggerParametersFromSettings();
            this.debuggerEngine = new DebuggerEngine();
            this.debuggerEngine.Parameters = prm;
            this.debuggerEngine.EngineStatusChanged += new EventHandler<DebuggerEngineStatusChangedEventArgs>(debuggerEngine_EngineStatusChanged);
            this.DebuggerParametersList.DebuggerEngine = this.debuggerEngine;
            this.DebuggerParametersList.UpdateControlerParameters();
        }
        /// <summary>
        /// Отображает вкладку параметров отладчика
        /// </summary>
        private void ShowDebuggerParametersList()
        {
            if(this.debuggerEngine == null)
            {
                this.CreateDebuggerEngine();
            }
            this.DebuggerParametersList.Open();
        }
        /// <summary>
        /// Возвращает параметры отладчика на основаниии данных, сохраненных в Settings
        /// </summary>
        private DebuggerParameters GetDebuggerParametersFromSettings()
        {
            DebuggerParameters res = new DebuggerParameters();
            res.BaudRate = Program.Settings.DeBugger_Settings_BaudRate;
            res.ControllerNumber = Program.Settings.DeBugger_Settings_NumberLink;
            res.PortName = Program.Settings.DeBugger_Settings_ComPort;
            res.ProtocolType = (ProtocolType)Enum.Parse(typeof(ProtocolType),Program.Settings.DeBugger_Settings_Protocol);
            res.ProcessorType = (ProcessorType)Enum.Parse(typeof(ProcessorType), Program.Settings.DeBugger_SettingsProcessesorType);
            return res;
        }
        /// <summary>
        /// Возвращает екущий проект, преобразованный к проекту Relkon
        /// </summary>
        internal ControllerProgramSolution ControllerProgramSolution
        {
            get
            {
                return this.solution as ControllerProgramSolution;
            }
        }
        /// <summary>
        /// Загружает проект из указанного файла
        /// </summary>
        private void LoadSolution(string SolutionFileName)
        {
            ControllerProgramSolution sln = null;
            switch(Path.GetExtension(SolutionFileName))
            {
                case ".rp6":
                    sln = ControllerProgramSolution.FromFile(SolutionFileName);
                    break;              
                default:
                    throw new Exception("Данный ти проекта не поддерживается!");
            }
            this.LoadSolution(sln);
        }
        /// <summary>
        /// Заггружает указанный проект
        /// </summary>
        private void LoadSolution(ControllerProgramSolution solution)
        {
            if (this.solution != null && !this.CloseSolution())
                return;
            this.solution = solution;
            
            if (this.solution is ControllerProgramSolution)
            {
                //this.SelectedProcessor = this.ControllerProgramSolution.ProcessorParams.Type;
                this.LoadTextFile(this.ControllerProgramSolution.ProgramFileName);
                this.LoadPultFile(this.ControllerProgramSolution.PultFileName);
                this.solution.ActiveFileName = this.ControllerProgramSolution.ProgramFileName;
                if (this.debuggerEngine == null)
                    this.CreateDebuggerEngine();
                else
                {
                    this.debuggerEngine.Parameters = this.GetDebuggerParametersFromSolution(this.ControllerProgramSolution);
                    this.DebuggerParametersList.UpdateControlerParameters();
                }
            }
            this.SolutionExplorer.AddSolutionNode(this.solution);
            foreach (string FileName in this.solution.OpenedFiles)
            {
                this.LoadFile(FileName);
            }
            if (this.solution.ActiveFileName != "")
                this.LoadFile(this.solution.ActiveFileName);
            Program.Settings.LastOpenedSolutionDirectory = this.solution.DirectoryName;
            this.OpenFileDialog.InitialDirectory = this.solution.DirectoryName;
            this.SaveFileDialog.InitialDirectory = this.solution.DirectoryName;
            //this.SetSolutionItemsVisibleState(this.solution, true);
            this.SetSolutionItemsEnabledState(this.solution, true);
            this.DebuggerParametersList_DebuggertParametersUpdated(null, EventArgs.Empty);
            this.Text = this.solution.Name + " - " + this.defaultCaption;

            ((ControllerProgramSolution)this.solution).CompilationParams.EmulationMode = tsbEmulationMode.Checked;
            ((ControllerProgramSolution)this.solution).CompilationParams.EmulationMode2 = tsbEmulationMode2.Checked;
        }
        /// <summary>
        /// Сохраняет документ; позволяет пользователю задать имя файла
        /// </summary>
        private void SaveDocumentAs(FileTabbedDocument Document)
        {
            string filter = "";
            switch (Path.GetExtension(Document.FileName))
            {
                case ".kon":
                    filter = "Программа Relkon (*.kon)|*.kon|";
                    break;
                case ".c":
                    filter = "Файлы C (*.c)|*.c|";
                    break;
                case ".asm":
                    filter = "Файлы ассемблера (*.asm)|*.asm|";
                    break;
                case ".plt":
                    filter = "Файлы пультов (*.plt)|*.plt|";
                    break;
                case ".map":
                    filter = "Карты памяти (*.map)|*.map|";
                    break;             
                case ".fbr":
                    filter = "Файлы функц-ных блок-диаграмм (*.fbr)|*.fbr|";
                    break;                    
            }
            this.SaveFileDialog.Filter = filter + "Все файлы|*.*";
            this.SaveFileDialog.FileName = Document.FileName;            
            if (this.SaveFileDialog.ShowDialog() == DialogResult.OK)
            {                                                        
                Document.SaveAs(this.SaveFileDialog.FileName);                
            }
        }
        /// <summary>
        /// Вывводит на консоль данные, получаемые от текущего процессора
        /// </summary>
        private void ControllerSolution_OutputDataReceived(object sender, RelkonDataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                this.Invoke(new ParameterizedDelegate<string>(this.Console.WriteLine), new[] { e.Data });
            }
        }
        /// <summary>
        /// Сохраняет проект в указанном пользователем месте
        /// </summary>
        /// <returns>Возвращает true, если проект был сохранен и false, если сохранение было отменено</returns>
        private bool SaveSolutionAs()
        {
            return this.SaveSolutionAs(true);
        }
        /// <summary>
        /// Сохраняет проект в указанном пользователем месте
        /// </summary>
        /// <returns>Возвращает true, если проект был сохранен и false, если сохранение было отменено</returns>
        /// <param name="reload">Требуется ли загрузить сохраненный проект</param>
        private bool SaveSolutionAs(bool reload)
        {
            this.SaveFileDialog.Filter = this.solution.FileDialogFilter;
            this.SaveFileDialog.FileName = this.solution.IsNewSolution ? (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\project" + this.solution.Extension) : this.solution.SolutionFileName;
            if (this.SaveFileDialog.ShowDialog() != DialogResult.OK)
                return false;
            ControllerProgramSolution sln = this.solution;
            try
            {
                foreach (TabbedDocument doc in this.Documents)
                {
                    if (!(doc is FileTabbedDocument))
                        continue;
                    if (this.solution.Files.Contains(((FileTabbedDocument)doc).FileName))
                        ((FileTabbedDocument)doc).Save();
                }
                this.solution.IsNewSolution = false;
                this.CloseSolution();
                sln.SaveAs(this.SaveFileDialog.FileName);
                if(reload)
                    this.LoadSolution(this.SaveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("SaveSolutionAs: " + ex.Message);
            }
            return true;
        }
        /// <summary>
        /// Заполняет список открытых документов проекта
        /// </summary>
        private void FillOpenedFilesList()
        {
            this.solution.OpenedFiles.Clear();
            DockControl[] docs = this.DocumentManager.GetDockControls(DockSituation.Document);
            //Array.Sort<DockControl>(docs, new Comparison<DockControl>((a, b) => b.TabBounds.X - a.TabBounds.Y));
            Array.Sort<DockControl>(docs, new Comparison<DockControl>((a, b) => b.TabBounds.X - a.TabBounds.X));
            foreach (TabbedDocument doc in docs)
            {
                if (doc is FileTabbedDocument)
                    this.solution.OpenedFiles.Add(((FileTabbedDocument)doc).FileName);
            }
        }
        /// <summary>
        /// Закрывает проект
        /// </summary>
        /// <returns>Возвращает true, если проект был закрыт</returns>
        private bool CloseSolution()
        {
            if (this.debuggerEngine != null && this.debuggerEngine.EngineStatus == DebuggerEngineStatus.Started)
            {
                this.debuggerEngine.Stop();
            }
            this.FillOpenedFilesList();
            if (this.ActiveDocument != null)
                this.solution.ActiveFileName = (this.ActiveDocument is FileTabbedDocument) ? ((FileTabbedDocument)this.ActiveDocument).FileName : "";
            else
                this.solution.ActiveFileName = "";
            if (this.solution.IsNewSolution)
            {
                switch (Utils.QuestionMessage("Сохранить проект ?", "Relkon"))
                {
                    case DialogResult.No:
                        foreach (RelkonTabbedDocument doc in this.DocumentManager.GetDockControls(DockSituation.Document))
                        {
                            if(doc is IEditableTabbedDocument)
                                ((IEditableTabbedDocument)doc).SaveRequired = false;
                        }
                        break;
                    case DialogResult.Cancel:
                        return false;
                    case DialogResult.Yes:
                        return this.SaveSolutionAs(false);
                }
            }
            try
            { //Relkon.MainForm.MainFormInstance.DebuggerParametersList.DebuggerParameters.Save();
                this.debuggerEngine.Parameters.Save(this.solution.SolutionFileName.Remove(this.solution.SolutionFileName.LastIndexOf('.'))+".dbg");
                ((ControllerProgramSolution)this.solution).DebuggerFileName = 
                    this.solution.SolutionFileName.Remove(this.solution.SolutionFileName.LastIndexOf('.')) + ".dbg";
            }
            catch { }
            if (!this.CloseDocuments(this.Documents))
                return false;
            this.SolutionExplorer.Clear();
            try
            {
                this.solution.Save();
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("Сохранение проекта: " + ex.Message);
            }
            this.SetSolutionItemsEnabledState(this.solution, false);
            //this.SetSolutionItemsVisibleState(this.solution, false);
            this.solution = null;
            this.Text = this.defaultCaption;
            if (this.DebuggerParametersList.IsOpen)
                this.DebuggerParametersList.Close();
            this.debuggerEngine = null;
            return true;
        }
        /// <summary>
        /// Закрывает активный документ
        /// </summary>
        private void CloseActiveDocument()
        {
            this.DocumentManager.ActiveTabbedDocument.Close();
        }
        /// <summary>
        /// Создает новый проект Relkon
        /// </summary>
        private void CreateNewRelkonSolution()
        {
            try
            {                
                ControllerProgramSolution sln = ControllerProgramSolution.CreateNewSolution(ProcessorType.STM32F107, "project", Program.NewProjectDirectory);
                this.LoadSolution(sln);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("CreateNewRelkonSolution: " + ex.Message);
            }
        }
       
        /// <summary>
        /// Проверяет, открыт ли указанный файл
        /// </summary>
        /// <returns>Документ, содержащий файл или null</returns>
        private FileTabbedDocument FileAlreadyLoaded(string FileName)
        {
            foreach (RelkonTabbedDocument doc in this.DocumentManager.GetDockControls(DockSituation.Document))
            {
                if (doc is FileTabbedDocument && ((FileTabbedDocument)doc).FileName == FileName)
                    return (FileTabbedDocument)doc;
            }
            return null;
        }
        /// <summary>
        /// Проверяет, является ли файл текстовым (для отображения его в редакторе)
        /// </summary>
        private static bool IsTextFile(string FileName)
        {
            string ext = Path.GetExtension(FileName);
            return (ext == ".kon" || ext == ".c" || ext == ".h" || ext == ".err" || ext == ".cpp" || ext == ".asm" || ext == ".map" || ext == ".txt");
        }
        /// <summary>
        /// Проверяет, является ли файл файлом пультов
        /// </summary>
        private static bool IsPultFile(string FileName)
        {
            string ext = Path.GetExtension(FileName);
            return (ext == ".plt");
        }
        /// <summary>
        /// Проверяет, является ли файл файлом проекта
        /// </summary>
        private static bool IsSolutionFile(string FileName)
        {
            string ext = Path.GetExtension(FileName);
            return (ext == ".rp6");
        }
        /// <summary>
        /// Проверяет, является ли файл файлом ФБД
        /// </summary>
        private static bool IsFbdFile(string FileName)
        {            
            return Path.GetExtension(FileName) == ".fbr";
        }       
        /// <summary>
        /// Показывает, является ли заданный файл файлом параметров пульта
        /// </summary>
        private static bool IsDebuggerParametersFile(string FileName)
        {
            return Path.GetExtension(FileName) == ".dbg";
        }       
        /// <summary>
        /// Показывает, можно ли изменять указанный файл
        /// </summary>
        private bool IsReadOnlyFile(string FileName)
        {
            bool res = false;
            string Extension = Path.GetExtension(FileName);
            if (this.solution != null && (Extension == ".map" || Extension == ".err") && (this.solution.Files.Contains(FileName) ||
                (this.ControllerProgramSolution != null && FileName == this.ControllerProgramSolution.CompilationParams.CompilationErrorsFileName)))
                res = true;
            return res;
        }
        /// <summary>
        /// Показывает, может ли файл быть открыт средой Relkon, когда не
        /// загружено ни одного проекта
        /// </summary>
        private static bool FileCanLoadedWOSolution(string FileName)
        {
            return (IsSolutionFile(FileName) || IsTextFile(FileName));
        }
        /// <summary>
        /// Загружает указанный текстовый файл (который должен отображаться в редакторе)
        /// </summary>
        private void LoadTextFile(string FileName)
        {
            EditorTabbedDocument doc = null;
            if ((doc = (EditorTabbedDocument)this.FileAlreadyLoaded(FileName)) != null)
            {
                doc.Activate();
                //this.DocumentManager.seActiveTabbedDocument = doc; sd35
            }
            else
            {
                doc = new EditorTabbedDocument((this.solution != null) ? this.solution.GetSolutionThatContainsFile(FileName) : null, FileName, this.SetPositionString);
                if (this.IsReadOnlyFile(FileName))
                    doc.Editor.Readonly = true;
                this.OpenDocument(doc);
            }
        }        
        /// <summary>
        /// Загружает указанный файл пультов
        /// </summary>
        private void LoadPultFile(string FileName)
        {
            StandartPultTabbedDocument doc;
            if ((doc = (StandartPultTabbedDocument)this.FileAlreadyLoaded(FileName)) != null)
                //this.DocumentManager.ActiveTabbedDocument = doc; sd35
                doc.Activate();
            else
            {
                doc = new StandartPultTabbedDocument((this.solution != null) ? this.solution.GetSolutionThatContainsFile(FileName) as ControllerProgramSolution : null, FileName);
                if (doc.Pult.Type != PultType.Pult4x20)
                    doc.ConvertPultModel(PultType.Pult4x20);
                this.OpenDocument(doc);
            }
        }       
        /// <summary>
        /// Загружает файл параметров отладчика
        /// </summary>
        private void LoadDebuggerParametersFile(string FileName)
        {
            if (this.debuggerEngine == null)
                this.CreateDebuggerEngine();
            try
            {
                this.debuggerEngine.Parameters = DebuggerParameters.FromFile(FileName);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("Ошибка загрузки файла: " + ex.Message);
                return;
            }
            this.DebuggerParametersList.UpdateControlerParameters();
            this.DebuggerParametersList_DebuggertParametersUpdated(null, EventArgs.Empty);
        }
        /// <summary>
        /// Загружает файл ФБД
        /// </summary>
        //private void LoadFbdFile(string FileName)
        //{            
        //    FbdTabbedDocument doc;
        //    if ((doc = (FbdTabbedDocument)this.FileAlreadyLoaded(FileName)) != null)
        //        //this.DocumentManager.ActiveTabbedDocument = doc; sd35
        //        doc.Activate();
        //    else
        //    {
        //        doc = new FbdTabbedDocument((this.solution != null) ? this.solution.GetSolutionThatContainsFile(FileName) as ControllerProgramSolution : null, FileName);
        //        this.OpenDocument(doc);
        //    }           
        //}
              
        /// <summary>
        /// Открывает файл неизвестного типа
        /// </summary>
        private void LoadUnknownFile(string FileName)
        {
            Process p = new Process();
            p.StartInfo.FileName = FileName;
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }
        /// <summary>
        /// Загружает указанный файл
        /// </summary>
        internal void LoadFile(string FileName)
        {          
            try
            {
                if (!File.Exists(FileName))
                    return;
                if (IsSolutionFile(FileName))
                    this.LoadSolution(FileName);
                else if (IsTextFile(FileName))
                    this.LoadTextFile(FileName);
                else if (IsPultFile(FileName))
                    this.LoadPultFile(FileName);               
                else if (IsDebuggerParametersFile(FileName))
                    this.LoadDebuggerParametersFile(FileName);
                //else if (IsFbdFile(FileName))               
                //    this.LoadFbdFile(FileName);                                   
                else
                    this.LoadUnknownFile(FileName);
                this.DocumentManager_ActiveTabbedDocumentChanged(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("LoadFile: " + Utils.FirstLetterToLower(ex.Message));
            }
        }
        /// <summary>
        /// Загружает новый файл; позоляет пользователю выбрать имя файла для загрузки
        /// </summary>
        private void LoadFile()
        {                       
            this.OpenFileDialog.FileName = "";
            if (this.OpenFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.LoadFile(this.OpenFileDialog.FileName);
        }
        /// <summary>
        /// Открывает окно свойств проекта
        /// </summary>
        private void OpenSolutionPropertiesDocument(ControllerProgramSolution Solution)
        {
            PropertiesTabbedDocument doc = this.PropertiesDocumentOpen(Solution);
            if (doc == null)
            {                         
                if (Solution is STM32F107Solution)
                    doc = new STM32F107PropertiesTabbedDocument((STM32F107Solution)Solution);
                else
                    throw new Exception("Проекты типа " + Solution.GetType() + " не поддерживаются");
                this.OpenDocument(doc);
            }
            else
                doc.Activate();

        }
        /// <summary>
        /// Если окно свойств проекта уже открыто, то возвращает ссылку на него, иначе - null
        /// </summary>
        public PropertiesTabbedDocument PropertiesDocumentOpen(ControllerProgramSolution Solution)
        {
            foreach (RelkonTabbedDocument doc in this.Documents)
            {
                if (doc is PropertiesTabbedDocument && ((PropertiesTabbedDocument)doc).Solution == Solution)
                    return (PropertiesTabbedDocument)doc;
            }
            return null;
        }
        
        /// <summary>
        /// Перезагружает страницу свойств проекта, если она открыта
        /// </summary>
        internal void ReloadSolutionPropertiesDocument(ControllerProgramSolution Solution)
        {
            RelkonTabbedDocument doc = this.PropertiesDocumentOpen(Solution);
            if (doc != null)
            {
                ((PropertiesTabbedDocument)doc).Reload();
            }
        }
        /// <summary>
        /// Сохраняет проект и все его открытые файлы
        /// </summary>
        private void SaveSolution(ControllerProgramSolution solution)
        {
            try
            {
                foreach (RelkonTabbedDocument doc in this.DocumentManager.GetDockControls(DockSituation.Document))
                {
                    if (doc.Solution!= null && doc.Solution.ID == solution.ID && doc is IEditableTabbedDocument)
                        ((IEditableTabbedDocument)doc).Save();
                }
                solution.Save();
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("SaveSolution: " + ex.Message);
            }
        }
        /// <summary>
        /// Выполняют подготовительные действия перед компиляцией
        /// </summary>
        private void PrepareToCompile(ControllerProgramSolution Solution)
        {
            this.SaveSolution(Solution);
            this.ErrorsList.Clear();
            this.InformationMessages.Clear();
            this.Console.Clear();
            this.ShowConsole();
            this.Console.Collapsed = false;
            this.InformationMessages.Close();
            Solution.OutputDataReceived += new RelkonDataReceivedEventHandler(this.ControllerSolution_OutputDataReceived);
            this.Console.WriteLine("------ Начало компиляции: Проект: " + Solution.Name + " Процессор: " + Solution.ProcessorParams.Type + " ------");
        }
        /// <summary>
        /// Компилирует проект Relkon
        /// </summary>
        private void Compile(ControllerProgramSolution Solution)
        {            
            if (this.CompileBackgroundWorker.IsBusy)
                return;
            this.PrepareToCompile(Solution);
            this.StartShowingAnimation(Relkon.Properties.Resources.CompileStrip);
            this.CompileBackgroundWorker.RunWorkerAsync(Solution);            
        }
        /// <summary>
        /// Выполняет действия после завершения компиляции
        /// </summary>
        /// <param name="Solution">Проект, который был скомпилирован</param>
        private void PerformPostCompileActions(ControllerProgramSolution Solution)
        {
            // Добавление в проект файлов, созданных при компиляции
            foreach (string FileName in Solution.CompilationParams.CompilationCreatedFilesNames)
            {
                if (!Solution.Files.Contains(FileName))
                    this.SolutionExplorer.AddFileToRelkonSolution(Solution, FileName);
                else
                {
                    EditorTabbedDocument doc = (EditorTabbedDocument)this.FileAlreadyLoaded(FileName);
                    if (doc != null)
                    {
                        try
                        {
                            doc.Reload();
                        }
                        catch (Exception ex)
                        {
                            Utils.ErrorMessage("Reload Document: " + ex.Message);
                        }
                    }
                }
            }
            // Обновление отображаемого файла ошибок
            EditorTabbedDocument d = (EditorTabbedDocument)this.FileAlreadyLoaded(Solution.CompilationParams.CompilationErrorsFileName);
            if (d != null)
            {
                try
                {
                    d.Reload();
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage("Reload Document: " + ex.Message);
                }
            }
            Solution.OutputDataReceived -= new RelkonDataReceivedEventHandler(this.ControllerSolution_OutputDataReceived);

            this.Console.WriteLine("========== Компиляция завершена " + (Solution.CompilationParams.HasErrors ? "с ошибками" : "успешно") + " ==========");
            this.Console.Close();
            // Отображение списка информационных сообщений после компиляции
            if (Solution.CompilationParams.PostcompileMessages.Count > 0)
            {
                this.InformationMessages.Clear();
                this.InformationMessages.WriteLines(Solution.CompilationParams.PostcompileMessages.ToArray());
                this.ShowInforamtionMessagesList();
                this.InformationMessages.Collapsed = true;
            }
            // Сохранение проекта
            if (!Solution.CompilationParams.HasErrors)
            {
                try
                {
                    Solution.Save();
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                }
            }
            // Отображение списка ошибок и предупреждений
            if (Solution.CompilationParams.HasErrorsOrWarnings)
            {
                this.ErrorsList.PrintErrors(Solution.CompilationParams.Errors, Solution.CompilationParams.CompilationErrorsFileName);
                this.ErrorsList.Open();
                this.ErrorsList.Select();
            }
            else
            {
                this.ErrorsList.SetToDefaultState();
                this.ErrorsList.Close();
            }           
        }
        /// <summary>
        /// Конвертирует старые проекты Relkon (3.3, 3.5) к новому формату
        /// </summary>
        private void ConvertOldRelkonProject()
        {
            if (this.solution != null)
            {
                if (!this.CloseSolution())
                    return;
            }
            else if (!this.CloseDocuments(this.Documents))
                return;
            ConvertFromOldProjectForm cf = new ConvertFromOldProjectForm();
            if (cf.ShowDialog() == DialogResult.OK)
                this.LoadSolution(cf.FileName);
        }
        /// <summary>
        /// Изменяет шрифт текстового редактора для всех документов
        /// </summary>
        private void ChangeEditorFont(Font Font)
        {
            Program.Settings.EditorFont = Font;
            foreach (RelkonTabbedDocument doc in this.Documents)
            {
                if (doc is EditorTabbedDocument)
                    ((EditorTabbedDocument)doc).ReloadFont();
            }
        }
        /// <summary>
        /// Создает древовидный список сохраненных файлов
        /// </summary>
        private List<string> CreateModifiedFilesList(Hashtable SolutionFiles)
        {
            List<string> res = new List<string>();
            if (this.solution != null && SolutionFiles[this.solution.SolutionFileName] != null)
            {
                res.Add(" " + Path.GetFileName(this.solution.SolutionFileName));
                foreach (string FileCaption in (List<string>)SolutionFiles[this.solution.SolutionFileName])
                {
                    res.Add("     " + FileCaption);
                }
            }
            foreach (object key in SolutionFiles.Keys)
            {
                // Если key = 5, то значит документ не относится ни к какому проекту
                if ((key is string && ((string)key) == this.solution.SolutionFileName) || (key is int && (int)key == 5))
                    continue;
                res.Add(" " + Path.GetFileName((string)key));
                foreach (string FileCaption in (List<string>)SolutionFiles[key])
                {
                    res.Add("     " + FileCaption);
                }
            }
            if (SolutionFiles[5] != null)
            {
                foreach (string FileCaption in (List<string>)SolutionFiles[5])
                {
                    res.Add(" " + FileCaption);
                }
            }
            return res;
        }        
        /// <summary>
        /// Запускает утилиту программирования процессоров ATMEL Flip
        /// </summary>
        /// <param name="solution">Если не null, то для Flip'а будут установлены пути к hex-файлам указанного проекта</param>
        //internal void RunFlip(AT89C51ED2Solution solution)
        //{
        //    try
        //    {
        //        // Установка текущего каталога программатора и типа процессора
        //        using (StreamWriter writer = new StreamWriter(Program.ProgrammatorsDirectory + "\\flip\\bin\\prefs.tcl", false))
        //        {
        //            writer.WriteLine("set flipStates(selectedDevice) AT89C51ED2");
        //            if (solution != null)
        //            {
        //                writer.WriteLine("set flipStates(lastVisitedHexDir) \"" + solution.DirectoryName.Replace("\\", "/") + "\"");
        //            }
        //        }
        //        // Запуск программатора
        //        Utils.RunProgram(Program.ProgrammatorsDirectory + "\\flip\\bin\\flip.exe");
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.ErrorMessage(ex.Message);
        //    }
        //}
        /// <summary>
        /// Загружает данные указанного проекта
        /// </summary>
        public void UploadToDevice(ControllerProgramSolution solution, bool onlyProgram, bool onlyParams, bool readEmbVars)
        {
            foreach (RelkonTabbedDocument doc in this.DocumentManager.GetDockControls(DockSituation.Document))
            {
                if (doc is PropertiesTabbedDocument)
                {
                    ((PropertiesTabbedDocument)doc).Save();
                    if (!((PropertiesTabbedDocument)doc).SuccessSave)
                        return;
                }
            }

            if (debuggerEngine != null && debuggerEngine.EngineStatus == DebuggerEngineStatus.Started)
            {
                debuggerEngine.Stop();
                if (debuggerEngine.EngineStatus == DebuggerEngineStatus.Stopping)
                    Thread.Sleep(200);
            }
                    
            this.progressForm = new ProgressForm(this);
            this.progressForm.FormClosing += new FormClosingEventHandler(progressForm_FormClosing);

            this.solution.UploadingToDeviceCompleted += new AsyncCompletedEventHandler(solution_UploadingToDeviceCompleted);
            this.solution.UploadingToDeviceProgressChanged += new UploadMgrProgressChangedEventHandler(solution_UploadingToDeviceProgressChanged);

            ((STM32F107Solution)solution).UploadToDevice(onlyProgram, onlyParams, readEmbVars);

            if (this.progressForm.IsDisposed)
            {
               
            }

            this.progressForm.ShowDialog();
         
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveFormState();
            if (this.solution != null)
                e.Cancel = !this.CloseSolution();
            else
                e.Cancel = !this.CloseDocuments(this.Documents);
            if (this.DebuggerParametersList.IsOpen)
                this.DebuggerParametersList.Close();
        }

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (this.initialize && this.WindowState == FormWindowState.Normal)
                Program.Settings.FormLocation = this.Location;
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            this.LoadFile();
        }

        private void tsbSolutionExplorer_Click(object sender, EventArgs e)
        {
            this.LoadSolutionExplorerState();
            this.ShowSolutionExplorer();
        }

        private void miFile_DropDownOpening(object sender, EventArgs e)
        {
            this.miSave.Text = "&Сохранить";
            this.miSaveAs.Text = "Сохранить &как...";
            this.miSave.Enabled = this.miSaveAs.Enabled = false;
            if (this.ActiveDocument != null)
            {
                if (this.ActiveDocument is IEditableTabbedDocument)
                {
                    this.miSave.Text = "&Сохранить ";
                    this.miSave.Enabled = true;
                    if (this.ActiveDocument is FileTabbedDocument)
                    {
                        this.miSave.Text += Path.GetFileName(((FileTabbedDocument)this.ActiveDocument).FileName);
                        this.miSaveAs.Text = "Сохранить " + Path.GetFileName(((FileTabbedDocument)this.ActiveDocument).FileName) + " &как...";
                        this.miSaveAs.Enabled = true;
                    }
                }
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.solution != null && this.solution.IsNewSolution)
                this.SaveSolutionAs();
            else 
                this.SaveDocument(this.ActiveDocument);
        }

        private void tsbSaveAll_Click(object sender, EventArgs e)
        {
            if (this.solution != null && this.solution.IsNewSolution)
                this.SaveSolutionAs();
            this.SaveDocuments(this.Documents);
        }

        private void miSaveAs_Click(object sender, EventArgs e)
        {
            if(this.ActiveDocument is FileTabbedDocument)
                this.SaveDocumentAs((FileTabbedDocument)this.ActiveDocument);
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void miClose_Click(object sender, EventArgs e)
        {
            this.CloseActiveDocument();
        }

        private void miCloseProject_Click(object sender, EventArgs e)
        {
            this.CloseSolution();
        }

        private void DocumentManager_ActiveTabbedDocumentChanged(object sender, EventArgs e)
        {
            this.SetTabbedDocumentToolStripItemsEnabledState(this.ActiveDocument != null);
            this.SetEditorTabbedDocumentToolStripItemsEnabledState(this.ActiveDocument != null && this.ActiveDocument is EditorTabbedDocument);
            this.SetPultTabbedDocumentToolStripItemsEnabledState(this.ActiveDocument != null && this.ActiveDocument is StandartPultTabbedDocument);
            //this.SetFbdTabbedDocumentToolStripItemsEnabledState(this.ActiveDocument != null && this.ActiveDocument is FbdTabbedDocument);                        

            if (this.ActiveDocument == null)
                return;
            RelkonTabbedDocument ActiveDocument = (RelkonTabbedDocument)this.ActiveDocument;
            if (ActiveDocument is EditorTabbedDocument)
            {             
                EditorTabbedDocument doc = ActiveDocument as EditorTabbedDocument;
                this.SetPositionString("Cтрока " + (doc.Editor.Position.Y + 1) + new string(' ', 6) + "Cимвол " + (doc.Editor.Position.X + 1));
                if (this.findReplaceForm != null)
                    this.findReplaceForm.SearchedDocument = (EditorTabbedDocument)this.ActiveDocument;
            }
            else if (ActiveDocument is PropertiesTabbedDocument)
            {               
                ((PropertiesTabbedDocument)ActiveDocument).UpdateEmbeddedVarsValues();
            }
            else if (ActiveDocument is StandartPultTabbedDocument)
            {
                StandartPultTabbedDocument pdoc = (StandartPultTabbedDocument)ActiveDocument;
                //FbdTabbedDocument fbdDoc = null;
                if (pdoc.ControllerProgramSolution != null)
                {
                    if (this.tsbPultType.Items.Count != pdoc.ControllerProgramSolution.PultParams.AvailablePultTypes.Length)
                        this.SetPultListItems();
                    this.tsbPultType.SelectedItem = pdoc.Pult.Type;                    
                    //((StandartPultTabbedDocument)ActiveDocument).LoadPultModel(((StandartPultTabbedDocument)ActiveDocument).Pult);
                    EditorTabbedDocument doc = (EditorTabbedDocument)this.FileAlreadyLoaded(pdoc.ControllerProgramSolution.ProgramFileName);
                    if (doc == null)
                        pdoc.ControllerProgramSolution.UpdateCodeModel();
                    else
                        pdoc.ControllerProgramSolution.UpdateCodeModel(doc.Text);
                    pdoc.FillVarNamesList(pdoc.ControllerProgramSolution.Vars);
                   
                    //for (int i = 0; i < this.Documents.Length; i++)                    
                    //    if (this.Documents[i] is FbdTabbedDocument)
                    //    {
                    //        fbdDoc = (FbdTabbedDocument)Documents[i];
                    //        break;
                    //    }
                    //if (fbdDoc != null)
                    //{
                    //    //pdoc.FillFBDVarNamesList(fbdDoc.Editor);
                    //    pdoc.ActiveFbdEditor = fbdDoc.Editor;
                    //}
                    //else
                    //{
                    //    FbdEditor fbd = new FbdEditor();
                    //    fbd.Load(ControllerProgramSolution.FbdFileName);
                    //    pdoc.ActiveFbdEditor = fbd;
                    //    //pdoc.FillFBDVarNamesList(fbd);

                    //}
                }
                else
                {
                    this.tsbPultType.Items.Clear();
                    this.tsbPultType.Items.Add(pdoc.Pult.Type);
                    this.tsbPultType.SelectedIndex = 0;
                }
                pdoc.SetInputModeString();
            }
            //else if (ActiveDocument is FbdTabbedDocument)
            //{
            //    ((FbdTabbedDocument) ActiveDocument).UpdateUserToolBox();
            //} 
            else
                this.PositionStatusLabel.Text = "";
        }

        private void tsbCut_Click(object sender, EventArgs e)
        {
            ((IEditableTabbedDocument)this.ActiveDocument).Cut();
        }

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            ((IEditableTabbedDocument)this.ActiveDocument).Copy();
        }

        private void tsbPaste_Click(object sender, EventArgs e)
        {
            ((IEditableTabbedDocument)this.ActiveDocument).Paste();
        }

        private void tsbUndo_Click(object sender, EventArgs e)
        {
            ((IEditableTabbedDocument)this.ActiveDocument).Undo();
        }

        private void tsbRedo_Click(object sender, EventArgs e)
        {
            ((IEditableTabbedDocument)this.ActiveDocument).Redo();
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            ((IEditableTabbedDocument)this.ActiveDocument).Delete();
        }

        private void miSelectAll_Click(object sender, EventArgs e)
        {
            ((IEditableTabbedDocument)this.ActiveDocument).SelectAll();
        }

        private void tsbFind_Click(object sender, EventArgs e)
        {
            this.RunFindReplaceForm(false);
        }

        private void findReplaceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.findReplaceForm = null;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    if (this.findReplaceForm != null)
                        this.findReplaceForm.FindNext();
                    break;
                case Keys.F:
                    if (e.Control)
                        this.RunFindReplaceForm(false);
                    break;
                case Keys.H:
                    if (e.Control)
                        this.RunFindReplaceForm(true);
                    break;
            }
        }

        private void miReplace_Click(object sender, EventArgs e)
        {
            this.RunFindReplaceForm(true);
        }

        private void miSaveProjectAs_Click(object sender, EventArgs e)
        {
            this.SaveSolutionAs();

        }        

        private void SolutionExplorer_FileTreeNodeDoubleClick(object sender, Kontel.Relkon.Components.FileTreeNodeEventArgs e)
        {
            // Пытаемя загрузить файл, по которому дважду щекнули в Solution Eplorer'е
            if (!File.Exists(e.Node.FileName))
                Utils.ErrorMessage("Файл не найден. Возможно он был перемещен, переименован или удален.");
            else
                this.LoadFile(e.Node.FileName);
        }

        private void SolutionExplorer_FileTreeNodeTextChanged(object sender, Kontel.Relkon.Components.FileTreeNodeTextChangedEventArgs e)
        {
            if (e.Text == null)
                return;
            string ofn = e.Node.FileName; // старое имя файла
            try
            {
                string nfn = Path.GetDirectoryName(e.Node.FileName) + "\\" + e.Text;
                if (File.Exists(nfn))
                    throw new Exception("Файл \"" + nfn + "\" уже существует.");
                e.Node.FileName = e.Text;
                FileSystem.Rename(ofn, e.Node.FileName, true);
                e.Node.Solution.ChangeCustomFileName(ofn, e.Node.FileName);
                FileTabbedDocument doc = this.FileAlreadyLoaded(ofn);
                if (doc != null)
                    doc.FileName = e.Node.FileName;
                e.Node.Solution.Save();
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
                e.Node.FileName = Path.GetFileName(ofn);
                e.Node.Solution.ChangeCustomFileName(ofn, e.Node.FileName);
                e.Cancel = true;
            }
        }

        private void SolutionExplorer_RelkonSolutionTreeNodeTextChanged(object sender, Kontel.Relkon.Components.SolutionTreeNodeTextChangedEventArgs e)
        {
            if (e.Text == null || e.Node == null)
                return;
            ControllerProgramSolution sln = e.Node.Solution as ControllerProgramSolution;
            string sn = sln.Name;
            string prfn = sln.ProgramFileName;
            string plfn = sln.PultFileName;
            try
            {
                e.Node.Solution.Rename(e.Text);
                e.Node.Refresh();
                FileTabbedDocument doc = this.FileAlreadyLoaded(prfn);
                if (doc != null)
                    doc.FileName = sln.ProgramFileName;
                doc = this.FileAlreadyLoaded(plfn);
                if (doc != null)
                    doc.FileName = sln.PultFileName;
                if (this.solution is ControllerProgramSolution)
                    this.Text = this.solution.Name + " - " + this.defaultCaption;
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("SolutionExplorer_RelkonSolutionTreeNodeTextChanged: " + ex.Message);
                e.Cancel = true;
            }
        }

        private void SolutionExplorer_ProperitiesTreeNodeDoubleClick(object sender, Kontel.Relkon.Components.ProperitiesTreeNodeEventArgs e)
        {
             this.OpenSolutionPropertiesDocument(e.Node.Solution);
        }

        private void SolutionExplorer_ControllerProgramSolutionNodeCompileClick(object sender, EventArgs<ControllerProgramSolution> e)
        {
            this.Compile(e.Value);
        }

        private void tsbCompile_Click(object sender, EventArgs e)
        {
            this.Compile(this.ControllerProgramSolution);
        }

        private void CompileBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ControllerProgramSolution Solution = e.Argument as ControllerProgramSolution;
            Solution.Compile();
            e.Result = Solution;
        }

        private void CompileBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.PerformPostCompileActions((ControllerProgramSolution)e.Result);
            this.StopShowingAnimation();
        }

        private void tsbEmulationMode_Click(object sender, EventArgs e)
        {
            if (tsbEmulationMode2.Checked)
                tsbEmulationMode2.Checked = false;
            this.ControllerProgramSolution.CompilationParams.EmulationMode = tsbEmulationMode.Checked = !tsbEmulationMode.Checked;
            this.ControllerProgramSolution.CompilationParams.EmulationMode2 = tsbEmulationMode2.Checked;

            if (tsbEmulationMode.Checked)
                tsbEmulationMode.ToolTipText = "Выключить режим полной эмуляции";
            else
                tsbEmulationMode.ToolTipText = "Включить режим полной эмуляции";

            if (tsbEmulationMode2.Checked)
                tsbEmulationMode2.ToolTipText = "Выключить режим эмуляции входов";
            else
                tsbEmulationMode2.ToolTipText = "Включить режим эмуляции входов";
        }
       

        private void tsbErrorList_Click(object sender, EventArgs e)
        {
            this.ShowErrorsList();
        }

        private void ErrorsList_RelkonCompilationErrorShow(object sender, CompilationErrorEventArgs e)
        {
            if (e.Error is CompilationError && e.Error.LineNumber != -1)
            {
                this.LoadFile(e.Error.FileName);
                if (this.ActiveDocument is EditorTabbedDocument)
                    ((EditorTabbedDocument)this.ActiveDocument).SelectLine(e.Error.LineNumber);
                //else
                //    ((FBlockCreatorTabbedDocument) this.ActiveDocument).SelectLine(e.Error.LineNumber);
                    
                
            }
            else if (e.Error is PultTranslationError)
            {
                this.LoadPultFile(e.Error.FileName);
                PultTranslationError error = (PultTranslationError)e.Error;
                ((StandartPultTabbedDocument)this.ActiveDocument).SelectSymbol(error.Row, error.View, error.Symbol);
            }            
        }

        private void ErrorsList_ErrorFileMustOpen(object sender, FileEventArgs e)
        {
            this.LoadFile(e.FileName);
        }

        private void tbInformationList_Click(object sender, EventArgs e)
        {
            this.ShowInforamtionMessagesList();
        }

        private void tsbConsole_Click(object sender, EventArgs e)
        {
            this.ShowConsole();
        }

        private void miProjectProperties_Click(object sender, EventArgs e)
        {
            this.OpenSolutionPropertiesDocument(this.solution);
        }

        private void tsbPultType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ActiveDocument == null)
                return;
            StandartPultTabbedDocument doc = (StandartPultTabbedDocument)this.ActiveDocument;
            try
            {
                doc.ConvertPultModel((PultType)this.tsbPultType.SelectedItem);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
            }
            this.tsbPultType.SelectedItem = doc.Pult.Type;
        }

        private void tsbImportPultModel_Click(object sender, EventArgs e)
        {
            string filter = this.OpenFileDialog.Filter;
            this.OpenFileDialog.FilterIndex = 4;
            this.OpenFileDialog.FileName = "";
            if (this.OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                StandartPultTabbedDocument doc = (StandartPultTabbedDocument)this.ActiveDocument;
                try
                {
                    RelkonPultModel pm = RelkonPultModel.FromFile(this.OpenFileDialog.FileName);
                    if (doc.ControllerProgramSolution != null && !doc.ControllerProgramSolution.PultParams.PultTypeAllowed(pm.Type))
                        pm.ChangePultType(doc.ControllerProgramSolution.PultParams.DefaultPultType);
                    doc.LoadPultModel(pm);
                    //doc.MarkDocumentAsModified();
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                }
            }
            this.OpenFileDialog.FilterIndex = 1;
        }

        private void miOpenSolution_Click(object sender, EventArgs e)
        {
            this.OpenFileDialog.FilterIndex = 1;
            this.OpenFileDialog.FileName = "";
            if (this.OpenFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.LoadFile(this.OpenFileDialog.FileName);
        }

        private void miOpenFile_Click(object sender, EventArgs e)
        {
            this.OpenFileDialog.FilterIndex = 5;
            this.OpenFileDialog.FileName = "";
            if (this.OpenFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.LoadFile(this.OpenFileDialog.FileName);
        }

        private void tsbClearPult_Click(object sender, EventArgs e)
        {
            StandartPultTabbedDocument doc = (StandartPultTabbedDocument)this.ActiveDocument;
            doc.Clear();
        }        

       

        private void miFlashMCU_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ControllerProgramSolution != null && File.Exists(this.ControllerProgramSolution.DirectoryName + "\\" + this.ControllerProgramSolution.Name + "_original.mhx"))
                {
                    // Установка текущего каталога программатора и типа процессора
                    string s = File.ReadAllText(Program.ProgrammatorsDirectory + "\\FUJITSU FLASH MCU Programmer\\FMC16LX\\DATAFILE.TXT", Encoding.Default);
                    string pattern = "^Write File=.*$";
                    s = Regex.Replace(s, pattern, "Write File=" + this.ControllerProgramSolution.DirectoryName + "\\" + this.ControllerProgramSolution.Name + "_original.mhx", RegexOptions.Multiline);
                    File.WriteAllText(Program.ProgrammatorsDirectory + "\\FUJITSU FLASH MCU Programmer\\FMC16LX\\DATAFILE.TXT", s, Encoding.Default);
                }
                Utils.RunProgram(Program.ProgrammatorsDirectory + "\\FUJITSU FLASH MCU Programmer\\FMC16LX\\flash.exe");
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
            }
        }

        private void miDebugger_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.RunProgram(Program.RelkonDirectory + "\\Debugger\\Debugger.exe");
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
            }
        }

        private void проектНаОсновеСуществующихФайловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.ConvertOldRelkonProject();
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("Project Convertation: " + ex.Message);
            }
        }

        private void tsbPrintSetup_Click(object sender, EventArgs e)
        {
            if (this.convertRequired)
                this.PageSetupDialog.PageSettings.Margins = PrinterUnitConvert.Convert(this.PageSetupDialog.PageSettings.Margins, PrinterUnit.Display, PrinterUnit.TenthsOfAMillimeter);
            if (this.PageSetupDialog.ShowDialog() == DialogResult.OK)
                this.convertRequired = true;
            else
                this.convertRequired = false;
        }

        private void tsbPrintPreview_Click(object sender, EventArgs e)
        {
            this.PrintPreviewDialog.Owner = this;
            ((IPrintableTabbedDocument)this.ActiveDocument).PrintPreview(this.PrintPreviewDialog);
        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {
            ((IPrintableTabbedDocument)this.ActiveDocument).Print(this.PrintDialog);
        }

        private void miEditorFont_Click(object sender, EventArgs e)
        {
            this.FontDialog.Font = Program.Settings.EditorFont;
            if (this.FontDialog.ShowDialog() == DialogResult.OK)
                this.ChangeEditorFont(this.FontDialog.Font);
        }

        private void miDefaultFormat_Click(object sender, EventArgs e)
        {
            this.ChangeEditorFont(Program.Settings.DefaultEditorFont);
        }

        private void StatusStrip_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((!this.PositionStatusLabel.Bounds.Contains(e.Location)) || (!(this.ActiveDocument is EditorTabbedDocument)))
                return;
            EditorTabbedDocument doc = this.ActiveDocument as EditorTabbedDocument;
            GotoLineForm f = new GotoLineForm(doc.LinesCount, doc.CurrentLineNumer);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                doc.MoveToLine(f.LineNumber);
        }       

        //private void tsbSaveToFromatRelkon42_Click(object sender, EventArgs e)
        //{
        //    FileTabbedDocument doc = this.ActiveDocument as FileTabbedDocument;
        //    if (doc.Solution == null)
        //    {
        //        Utils.ErrorMessage("С файлом не связано проекта, сохранение в формате Relkon42 невозможно");
        //        return;
        //    }
        //    string s = this.SaveFileDialog.Filter;
        //    this.SaveFileDialog.Filter = "Файл пультов (*.plt)|*.plt";
        //    this.SaveFileDialog.InitialDirectory = Path.GetDirectoryName(doc.FileName);
        //    this.SaveFileDialog.FileName = this.SaveFileDialog.InitialDirectory + "\\" + Path.GetFileNameWithoutExtension(doc.FileName) + "_42" + Path.GetExtension(doc.FileName);
        //    if (this.SaveFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        try
        //        {
        //            ((StandartPultTabbedDocument)doc).Pult.SaveToRelkon42Format(this.SaveFileDialog.FileName, ((StandartPultTabbedDocument)doc).ControllerProgramSolution);
        //        }
        //        catch (Exception ex)
        //        {
        //            Utils.ErrorMessage(ex.Message);
        //        }
        //    }
        //    this.SaveFileDialog.Filter = s;
        //}

        private void cmiMainToolStripVisible_Click(object sender, EventArgs e)
        {
            this.MainToolStrip.Visible = !this.MainToolStrip.Visible;
        }

        private void cmiPultToolStripVisible_Click(object sender, EventArgs e)
        {
            this.PultToolStrip.Visible = !this.PultToolStrip.Visible;            
        }
       

        private void cmiDebuggerToolStripVisible_Click(object sender, EventArgs e)
        {
            this.DebuggerToolStrip.Visible = !this.DebuggerToolStrip.Visible;
        }

        //private void cmiFbdToolStripVisible_Click(object sender, EventArgs e)
        //{
        //    this.FbdToolStrip.Visible = !this.FbdToolStrip.Visible;
        //}  
        
        private void панелиИнсToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            this.miMainToolStripVisible.Checked = this.MainToolStrip.Visible;
            this.miPultToolStripVisible.Checked = this.PultToolStrip.Visible;
            this.miDebuggerToolStripVisible.Checked = this.DebuggerToolStrip.Visible;               
        }

        private void TooolStripContextMenu_Opening(object sender, CancelEventArgs e)
        {
            this.cmiMainToolStripVisible.Checked = this.MainToolStrip.Visible;
            this.cmiPultToolStripVisible.Checked = this.PultToolStrip.Visible;
            this.cmiDebuggerToolStripVisible.Checked = this.DebuggerToolStrip.Visible;
            //this.cmiFbdToolStripVisible.Checked = this.FbdToolStrip.Visible;
        }
        
        private void MainForm_DocumentsClosing(object sender, ClosingDocumentsEventArgs e)
        {
            List<TabbedDocument> MustSaveDocuments = new List<TabbedDocument>();
            List<string> FileNames = new List<string>();
            Hashtable SolutionFiles = new Hashtable();
            object key = null;
            foreach (RelkonTabbedDocument doc in e.Documents)
            {
                if (!(doc is FileTabbedDocument))
                    continue;
                if (((FileTabbedDocument)doc).SaveRequired)
                {
                    MustSaveDocuments.Add(doc);
                    if (doc.Solution != null)
                        key = doc.Solution.SolutionFileName;
                    else
                        key = 5;
                    if (SolutionFiles[key] == null)
                        SolutionFiles.Add(key, new List<string>());
                    ((List<string>)SolutionFiles[key]).Add(((FileTabbedDocument)doc).FileName);
                }
            }
            if (MustSaveDocuments.Count == 0)
                return;
            ModifiedFilesForm mf = new ModifiedFilesForm(this.CreateModifiedFilesList(SolutionFiles));
            DialogResult mfDialogResult = mf.ShowDialog();
            switch (mfDialogResult)
            {
                case DialogResult.Yes:
                    this.SaveDocuments(MustSaveDocuments.ToArray());
                    this.WaitWhileSaving();
                    break;
                case DialogResult.No:
                    foreach(FileTabbedDocument doc in MustSaveDocuments)
                    {
                        doc.SaveRequired = false;
                    }                  
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        private void tsbNewRelkonProject_Click(object sender, EventArgs e)
        {
            if (this.solution != null)
            {
                if (!this.CloseSolution())
                    return;
            }
            else if (!this.CloseDocuments(this.Documents))
                return;
            this.CreateNewRelkonSolution();
        }        
       

        private void tsbPrintToFile_Click(object sender, EventArgs e)
        {
            ((StandartPultTabbedDocument)this.ActiveDocument).PrintViewsToFile(); ;
        }

        private void miViewMemory_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Documents.Length; i++)
                if (this.Documents[i] is ViewMemoryTabbedDocument) { this.Documents[i].Activate(); return; }
            this.ShowDebuggerParametersList();
            this.OpenDocument(new ViewMemoryTabbedDocument(this.solution as ControllerProgramSolution, this.debuggerEngine));
        }

        private void miViewVars_Click(object sender, EventArgs e)
        {
           
            for (int i = 0; i < this.Documents.Length; i++)
                if (this.Documents[i] is ViewVarsTabbedDocument) { this.Documents[i].Activate(); return; }
            this.ShowDebuggerParametersList();
            ControllerProgramSolution cps = this.solution as ControllerProgramSolution;          
            ViewVarsTabbedDocument vvtd = new ViewVarsTabbedDocument(cps, this.debuggerEngine);          
            this.OpenDocument(vvtd);
          
        }

        private void miViewStructurs_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Documents.Length; i++)
                if (this.Documents[i] is ViewStructursTabbedDocument) { this.Documents[i].Activate(); return; }
            this.ShowDebuggerParametersList();
            this.OpenDocument(new ViewStructursTabbedDocument(this.solution as ControllerProgramSolution, this.debuggerEngine));
        }
        
        private void miViewGraphics_Click(object sender, EventArgs e)
        {
            this.ShowDebuggerParametersList();
            for (int i = 0; i < this.Documents.Length; i++)
                if (this.Documents[i] is ViewGraphicsTabbedDocument) { this.Documents[i].Activate(); return; }
            this.OpenDocument(new ViewGraphicsTabbedDocument(this.solution as ControllerProgramSolution, this.debuggerEngine));
        }

        private void miModulesSetup_Click(object sender, EventArgs e)
        {
            new SetModulesAddressForm().Show();
        }

        private void solution_UploadingToDeviceProgressChanged(object sender, UploadMgrProgressChangedEventArgs e)
        {
            if (this.progressForm != null)
            {
                if (e.StepName != null)
                    this.progressForm.Message = e.StepName;
                if (e.ProgressPercentage != -1)
                    this.progressForm.ProgressPercentage = e.ProgressPercentage;
            }
        }

        private void solution_UploadingToDeviceCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                Utils.ErrorMessage(e.Error.Message);
            if (this.progressForm != null)
                this.progressForm.Close();
            ((ControllerProgramSolution)sender).UploadingToDeviceCompleted -= new AsyncCompletedEventHandler(solution_UploadingToDeviceCompleted);
            ((ControllerProgramSolution)sender).UploadingToDeviceProgressChanged -= new UploadMgrProgressChangedEventHandler(solution_UploadingToDeviceProgressChanged);

            foreach (RelkonTabbedDocument doc in this.DocumentManager.GetDockControls(DockSituation.Document))
            {
                if (doc is PropertiesTabbedDocument)
                {
                    ((PropertiesTabbedDocument)doc).Reload();                   
                }
            }

        }

        private void progressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.progressForm.FormClosing -= new FormClosingEventHandler(progressForm_FormClosing);
            if (this.solution != null)
            {
                this.solution.StopUploading();
                while (this.solution.IsBusy)
                    Application.DoEvents();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Documents.Length; i++)
                if (this.Documents[i] is ViewIOSensorsTabbedDocument) { this.Documents[i].Activate(); return; }
            this.ShowDebuggerParametersList();
            this.OpenDocument(new ViewIOSensorsTabbedDocument(this.solution as ControllerProgramSolution, this.debuggerEngine));
        }

        private void tbHex_CheckedChanged(object sender, EventArgs e)
        {
            foreach (RelkonTabbedDocument doc in this.DocumentManager.GetDockControls(DockSituation.Document))
            {
                if (doc is DebuggerTabbedDocument)
                    ((DebuggerTabbedDocument)doc).UpdateDataPresentation(this.tbHex.Checked);
            }
        }

        private void tbDebuggerOptions_Click(object sender, EventArgs e)
        {
            this.ShowDebuggerParametersList();
            if (this.DebuggerParametersList.Parent != null && this.DebuggerParametersList.Parent.GetType() == typeof(TD.SandDock.DockContainer))
                ((TD.SandDock.DockContainer)this.DebuggerParametersList.Parent).Size = Program.Settings.DebuggerParametersListStaticSize;         
        }

        private void tsbDebuggerStart_Click(object sender, EventArgs e)
        {
            this.tsbDebuggerStart.Enabled = false;
            if (this.debuggerEngine == null)
                this.CreateDebuggerEngine();
            this.debuggerEngine.Start();
        }

        private void tsbDebuggerStop_Click(object sender, EventArgs e)
        {
            this.tsbDebuggerStop.Enabled = false;
            this.debuggerEngine.Stop();
        }

        private void debuggerEngine_EngineStatusChanged(object sender, DebuggerEngineStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case DebuggerEngineStatus.Started:
                    this.tsbDebuggerStart.Enabled = false;
                    this.tsbDebuggerStop.Enabled = true;
                    break;
                case DebuggerEngineStatus.Stopping:
                    this.tsbDebuggerStart.Enabled = false;
                    this.tsbDebuggerStop.Enabled = true;
                    break;
                case DebuggerEngineStatus.Stopped:
                    this.tsbDebuggerStart.Enabled = true;
                    this.tsbDebuggerStop.Enabled = false;
                    break;
            }
        }

        private void miDebugger_DropDownOpening(object sender, EventArgs e)
        {
            this.miStartDebugger.Enabled = (this.debuggerEngine == null || 
                this.debuggerEngine.EngineStatus == DebuggerEngineStatus.Stopped);
            
            this.miStopDebugger.Enabled = (this.debuggerEngine != null &&
                this.debuggerEngine.EngineStatus == DebuggerEngineStatus.Started);
        }

        private void DebuggerParametersListProcessesorChanged(object sender, EventArgs<ProcessorType> e)
        {
            foreach (RelkonTabbedDocument doc in this.Documents)
            {
                if (doc is DebuggerTabbedDocument)
                {
                    ((DebuggerTabbedDocument)doc).Update(this.solution as ControllerProgramSolution, this.debuggerEngine);
                }
            }
        }

        private void DebuggerParametersList_DebuggertParametersUpdated(object sender, EventArgs e)
        {
            foreach (RelkonTabbedDocument doc in this.Documents)
            {
                if (doc is DebuggerTabbedDocument)
                {
                    ((DebuggerTabbedDocument)doc).Update(this.solution as ControllerProgramSolution, this.debuggerEngine);
                }
            }
        }

        private void DebuggerParametersList_Closing(object sender, DockControlClosingEventArgs e)
        {
            if (this.initialize && this.DebuggerParametersList.Parent != null && this.DebuggerParametersList.Parent.GetType() == typeof(TD.SandDock.DockContainer))
                Program.Settings.DebuggerParametersListStaticSize = ((TD.SandDock.DockContainer)this.DebuggerParametersList.Parent).Size;
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void tsbSituations_Click(object sender, EventArgs e)
        {
            this.ShowDebuggerParametersList();
            for (int i = 0; i < this.Documents.Length; i++)
                if (this.Documents[i] is ViewSituationsTabbedDocument) { this.Documents[i].Activate(); return; }
            this.OpenDocument(new ViewSituationsTabbedDocument(this.solution as ControllerProgramSolution, this.debuggerEngine));
        }
    

        private void tSBGenerateCodeFromFbd_Click(object sender, EventArgs e)
        {
            //GenerateRelkonCodeFromFbd();
        }

        private void cortexToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void tsbEmulationMode2_Click(object sender, EventArgs e)
        {
            if (tsbEmulationMode.Checked)
                tsbEmulationMode.Checked = false;
            this.ControllerProgramSolution.CompilationParams.EmulationMode2 = tsbEmulationMode2.Checked = !tsbEmulationMode2.Checked;
            this.ControllerProgramSolution.CompilationParams.EmulationMode = tsbEmulationMode.Checked;

            if (tsbEmulationMode.Checked)
                tsbEmulationMode.ToolTipText = "Выключить режим полной эмуляции";
            else
                tsbEmulationMode.ToolTipText = "Включить режим полной эмуляции";

            if (tsbEmulationMode2.Checked)
                tsbEmulationMode2.ToolTipText = "Выключить режим эмуляции входов";
            else
                tsbEmulationMode2.ToolTipText = "Включить режим эмуляции входов";
        }

        private void tsbProgrammer_Click(object sender, EventArgs e)
        {
            this.UploadToDevice(this.solution, true, true, false);
        }

        private void miLoadProjectParams_Click(object sender, EventArgs e)
        {           
            this.UploadToDevice(this.solution, false, true, false);
        }

        private void miLoadProjectProgram_Click(object sender, EventArgs e)
        {
            this.UploadToDevice(this.solution, true, false, false);
        }       

        private void miGetEmbVarsFromController_Click(object sender, EventArgs e)
        {
            this.UploadToDevice(this.solution, false, false, true);
        }        
    }
}