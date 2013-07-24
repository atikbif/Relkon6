using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using Kontel.Relkon.Solutions;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Kontel.Relkon;
using System.Collections;
using Kontel.Relkon.Debugger;
using Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument;
using Kontel.Relkon.Classes;

namespace Kontel.Relkon.Components.Documents
{
    partial class ViewIOSensorsTabbedDocument : DebuggerTabbedDocument
    {
        private ControllerProgramSolution _solution;//Текущий проект     
        //Список цифровых входов
        private SortedList<int, Kontel.Relkon.Classes.ControllerVar> _inVarsDigital = new SortedList<int, Kontel.Relkon.Classes.ControllerVar>();
        //Список цифровых выходов
        private SortedList<int, Kontel.Relkon.Classes.ControllerVar> _outVarsDigital = new SortedList<int, Kontel.Relkon.Classes.ControllerVar>();
        //Список аналоговых входов
        private SortedList<int, Kontel.Relkon.Classes.ControllerVar> _inVarsAnalog = new SortedList<int, Kontel.Relkon.Classes.ControllerVar>();
        //Список аналоговых выходов
        private SortedList<int, Kontel.Relkon.Classes.ControllerVar> _outVarsAnalog = new SortedList<int, Kontel.Relkon.Classes.ControllerVar>();

        private bool _IsReading = false;//Запущены ли опросы
        private bool _IsOpen = true;//Вкладка существует
        private int _deletedPage = -1;

        //Вкладки для опроса модулей
        private SortedList<int, TD.SandDock.TabPage> tpBloks = new SortedList<int, TD.SandDock.TabPage>();
        //Вкладки для отображения встроенных датчиков
        private TD.SandDock.TabPage _tpDefault=new TD.SandDock.TabPage();


        //Компоненты для аналоговых входов
        Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl[] ascInputs;
        //Компоненты для аналоговых выходов
        Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl[] ascOutputs;


        protected override string ProtectedTabText
        {
            get
            {
                return "Входы - выходы";
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="solution"></param>
        public ViewIOSensorsTabbedDocument(ControllerProgramSolution solution, DebuggerEngine engine)
            : base(solution, engine)
        {
            debuggerEngine = engine;
            _solution = solution;
            InitializeComponent();
        }

        /// <summary>
        /// Формрированиее формы в зависимости от параметров при создании 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DigitalIOTabbedDocument_Load(object sender, EventArgs e)
        {
            debuggerEngine.EngineStatusChanged += new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
            //Загрузка параметров
            this._tpDefault = this.tabControl1.TabPages[0];
            this.Update(_solution, debuggerEngine);
        }


        /// <summary>
        /// Загрузка новых параметров, загрузка меток из параметров отладчика
        /// </summary>
        public override void Update(ControllerProgramSolution solution, DebuggerEngine engine)
        {
            debuggerEngine = engine;
            if (solution != null)
                _solution = solution;
            else
                _solution = Kontel.Relkon.Solutions.ControllerProgramSolution.Create(debuggerEngine.Parameters.ProcessorType);
            CreateComponents();
            this.pSettings.Visible = true;
            if (_solution != null)
            {
                //Загрузка меток дискретных датчиков
                this.digitalIO.ClearLabels();
                Match m;
                Relkon.DebuggerParameters.DigitalSensorDescription[] m_sensors = debuggerEngine.Parameters.DINSensors.ToArray();
                for (int i = 0; i < m_sensors.Length; i++)
                {
                    Relkon.DebuggerParameters.SensorLabels[] m_labelse = m_sensors[i].Labels.ToArray();
                    m = Regex.Match(m_sensors[i].Name, "DIN(\\d+)");
                    for (int j = 0; j < m_labelse.Length; j++)
                    {
                        try
                        {
                            this.digitalIO.ChangeLabel(true, Convert.ToInt32(m.Groups[1].Value), m_labelse[j].Number, m_labelse[j].Caption);
                        }
                        catch{}
                    }
                }
                m_sensors = debuggerEngine.Parameters.DOUTSensors.ToArray();
                for (int i = 0; i < m_sensors.Length; i++)
                {
                    Relkon.DebuggerParameters.SensorLabels[] m_labelse = m_sensors[i].Labels.ToArray();
                    m = Regex.Match(m_sensors[i].Name, "DOUT(\\d+)");
                    for (int j = 0; j < m_labelse.Length; j++)
                    {
                        try
                        {
                            this.digitalIO.ChangeLabel(false, Convert.ToInt32(m.Groups[1].Value), m_labelse[j].Number, m_labelse[j].Caption);
                        }
                        catch{}
                    }
                }
                //Загрузка меток аналоговых датчиков
                IList<int> m_key = this._inVarsAnalog.Keys;
                for (int i = 0; i < this._inVarsAnalog.Count; i++)
                {
                    for (int j = 0; j < debuggerEngine.Parameters.ADCSensors.Count; j++)
                        if (debuggerEngine.Parameters.ADCSensors[j].Name == this.ascInputs[i].SensorName)
                        {
                            this.ascInputs[i].SensorLabel = debuggerEngine.Parameters.ADCSensors[j].Caption;
                            this.ascInputs[i].SigleByte = debuggerEngine.Parameters.ADCSensors[j].DisplayOneByte;
                            break;
                        }
                }
                m_key = this._outVarsAnalog.Keys;
                for (int i = 0; i < this._outVarsAnalog.Count; i++)
                {
                    for (int j = 0; j < debuggerEngine.Parameters.DACSensors.Count; j++)
                        if (debuggerEngine.Parameters.DACSensors[j].Name == this.ascOutputs[i].SensorName)
                        {
                            this.ascOutputs[i].SensorLabel = debuggerEngine.Parameters.DACSensors[j].Caption;
                            this.ascOutputs[i].SigleByte = debuggerEngine.Parameters.DACSensors[j].DisplayOneByte;
                            break;
                        }
                }
            }
            //Установка доступности полей в зависимости от состояния отладчика
            DebuggerParametersList_ChangeStatusEngine(null, new Debugger.DebuggerEngineStatusChangedEventArgs(debuggerEngine.EngineStatus, null));
            //Установка невидемыми встроенных датчиков

            if (this.cbDefault.Checked != debuggerEngine.Parameters.DisplayDefault || debuggerEngine.Parameters.ProcessorType == ProcessorType.AT89C51ED2)
            {
                this.cbDefault.Checked = debuggerEngine.Parameters.ProcessorType == ProcessorType.STM32F107 ? debuggerEngine.Parameters.DisplayDefault : true;
                checkBox1_CheckedChanged(this.cbDefault, null);
            }

            if (this.tabControl1.TabPages.Count > 1) this.tabControl1.SelectedIndex=1;
            #region модули
            //Закрытие всех вкладок
            foreach (TD.SandDock.TabPage tp in this.tpBloks.Values)
            {
                tp.Dispose();
            }
            this.tpBloks = new SortedList<int, TD.SandDock.TabPage>();
            if (debuggerEngine.Parameters.ProcessorType == ProcessorType.STM32F107)
            {
                //Создание всех вкалдок из параметров отладчика
                foreach (Kontel.Relkon.DebuggerParameters.Block b in debuggerEngine.Parameters.ModulBlocks)
                {
                    this.CreateTabPage(b.Number, b.Caption,false);
                }
                //Обновление всех вкладок
                foreach (TD.SandDock.TabPage tp in this.tpBloks.Values)
                {
                    ((DisplayBlock)tp.Controls.Find("displayBlock", true)[0]).Update_presentation(solution, engine);
                }
            }
            #endregion
            if (this.tabControl1.TabPages.Count > 1) this.tabControl1.SelectedIndex = 0;
        }

        /// <summary>
        /// Смена режима работы отладчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DebuggerParametersList_ChangeStatusEngine(object sender, DebuggerEngineStatusChangedEventArgs e)
        {
            if (e.Status == DebuggerEngineStatus.Stopped)
            {
                //Установка полей недоступными
                this.digitalIO.EnabledMouseClick = false;
                for (int i = 0; i < this.ascInputs.Length; i++)
                    this.ascInputs[i].EnabledMouseClick = false;
                for (int i = 0; i < this.ascOutputs.Length; i++)
                    this.ascOutputs[i].EnabledMouseClick = false;
            }
            else
            {
                this.digitalIO.EnabledMouseClick = true;
                for (int i = 0; i < this.ascInputs.Length; i++)
                    this.ascInputs[i].EnabledMouseClick = true;
                for (int i = 0; i < this.ascOutputs.Length; i++)
                    this.ascOutputs[i].EnabledMouseClick = true;
            }
        }

        /// <summary>
        /// Обновление входов и выходов цифровых
        /// </summary>
        /// <param name="Sender"></param>
        private void RefreshInterfaseDigital(object Marker, byte[] Buffer, bool Error)
        {
            if (Buffer != null && !Error && _IsOpen)
            {
                Match m;
                m = Regex.Match((string)Marker, "DIN_(\\d+)");
                if (((m != null) && (m.Length != 0)) && (_inVarsDigital.ContainsKey(Convert.ToInt32(m.Groups[1].Value))))
                {
                    if (digitalIO.InVars[Convert.ToInt32(m.Groups[1].Value)].PrimaryValue != Buffer[0])
                    {
                        digitalIO.ChangeStatePictures(true, Convert.ToInt32(m.Groups[1].Value), Buffer[0]);
                        foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription dsd in debuggerEngine.Parameters.DINSensors)
                            if (dsd.Name == "DIN" + Convert.ToInt32(m.Groups[1].Value))
                            {
                                dsd.Value = Buffer;
                                return;
                            }
                        Kontel.Relkon.DebuggerParameters.DigitalSensorDescription dsd1 = new DebuggerParameters.DigitalSensorDescription();
                        ControllerIOVar CurentValue1 = _solution.Vars.GetIOVar("IN" + Convert.ToInt32(m.Groups[1].Value));
                        if (CurentValue1==null)return;
                        dsd1.Name = "D"+CurentValue1.Name;
                        dsd1.MemoryType = CurentValue1.Memory;
                        dsd1.Address = CurentValue1.Address;
                        dsd1.Value = Buffer;
                        debuggerEngine.Parameters.DINSensors.Add(dsd1);
                        return;
                    }
                }
                m = Regex.Match((string)Marker, "DOUT_(\\d+)");
                if (((m != null) && (m.Length != 0)) && (_outVarsDigital.ContainsKey(Convert.ToInt32(m.Groups[1].Value))))
                {
                    if (digitalIO.OutVars[Convert.ToInt32(m.Groups[1].Value)].PrimaryValue != Buffer[0])
                    {
                        digitalIO.ChangeStatePictures(false, Convert.ToInt32(m.Groups[1].Value), Buffer[0]);
                        foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription dsd in debuggerEngine.Parameters.DOUTSensors)
                            if (dsd.Name == "DOUT" + Convert.ToInt32(m.Groups[1].Value))
                            {
                                dsd.Value = Buffer;
                                return;
                            }
                        Kontel.Relkon.DebuggerParameters.DigitalSensorDescription dsd1 = new DebuggerParameters.DigitalSensorDescription();
                        ControllerIOVar CurentValue1 = _solution.Vars.GetIOVar("OUT" + Convert.ToInt32(m.Groups[1].Value));
                        if (CurentValue1 == null) return;
                        dsd1.Name = "D" + CurentValue1.Name;
                        dsd1.MemoryType = CurentValue1.Memory;
                        dsd1.Address = CurentValue1.Address;
                        dsd1.Value = Buffer;
                        debuggerEngine.Parameters.DOUTSensors.Add(dsd1);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Удаление признака чтения дактчика
        /// </summary>
        /// <param name="Marker"></param>
        /// <param name="Buffer"></param>
        /// <param name="Error"></param>
        private void WriteFinish(object Marker, byte[] Buffer, bool Error)
        {
            if (!Error)
            {
                Match m = Regex.Match((string)Marker, "IN(\\d+)_(\\d+)");
                if (m.Success )
                {
                    this.digitalIO.InVars[int.Parse(m.Groups[1].Value)].WriteSensors.Remove(int.Parse(m.Groups[2].Value));
                    return;
                }
                m = Regex.Match((string)Marker, "OUT(\\d+)_(\\d+)");
                if (m.Success)
                {
                    this.digitalIO.OutVars[int.Parse(m.Groups[1].Value)].WriteSensors.Remove(int.Parse(m.Groups[2].Value));
                    return;
                }
            }
        }

        /// <summary>
        /// Обновление входов и выходов аналоговых
        /// </summary>
        /// <param name="Sender"></param>
        private void RefreshInterfaseAnalog(object Marker, byte[] Buffer, bool Error)
        {
            if (Buffer != null && !Error)
            {
                try
                {
                    Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl m_sender = (Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl)Marker;
                    if (!m_sender.Edited)
                    {
                        m_sender.InverseByteOrder = debuggerEngine.Parameters.InverseByteOrder;
                        m_sender.SetData(Buffer);
                        if (m_sender.SensorName.Contains("ADC"))
                        {
                            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription dsd in debuggerEngine.Parameters.ADCSensors)
                            {
                                 if (dsd.Name == m_sender.SensorName)
                                {
                                    ControllerIOVar CurentValue = _solution.Vars.GetIOVar(dsd.Name);
                                    dsd.MemoryType = CurentValue.Memory;
                                    dsd.Address = CurentValue.Address;
                                    dsd.Value = Buffer;
                                    return;
                                }
                            }
                            Kontel.Relkon.DebuggerParameters.AnalogSensorDescription dsd1 = new Kontel.Relkon.DebuggerParameters.AnalogSensorDescription();
                            ControllerIOVar CurentValue1 = _solution.Vars.GetIOVar(m_sender.SensorName);
                            if (CurentValue1 == null) return;
                            dsd1.Name = CurentValue1.Name;
                            dsd1.MemoryType = CurentValue1.Memory;
                            dsd1.Address = CurentValue1.Address;
                            dsd1.Value = Buffer;
                            debuggerEngine.Parameters.ADCSensors.Add(dsd1);
                            return;
                        }
                        else if (m_sender.SensorName.Contains("DAC"))
                        {
                            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription dsd in debuggerEngine.Parameters.DACSensors)
                                if (dsd.Name == m_sender.SensorName)
                                {
                                    ControllerIOVar CurentValue = _solution.Vars.GetIOVar(m_sender.SensorName);
                                    dsd.MemoryType = CurentValue.Memory;
                                    dsd.Address = CurentValue.Address;
                                    dsd.Value = Buffer;
                                    return;
                                }
                            Kontel.Relkon.DebuggerParameters.AnalogSensorDescription dsd1 = new Kontel.Relkon.DebuggerParameters.AnalogSensorDescription();
                            ControllerIOVar CurentValue1 = _solution.Vars.GetIOVar(m_sender.SensorName);
                            if (CurentValue1 == null) return;
                            dsd1.Name = CurentValue1.Name;
                            dsd1.MemoryType = CurentValue1.Memory;
                            dsd1.Address = CurentValue1.Address;
                            dsd1.Value = Buffer;
                            debuggerEngine.Parameters.DACSensors.Add(dsd1);
                            return;
                        }
                    }
                }
                catch { }
            }
        }


        /// <summary>
        /// Создание компонентов для отображения цифровых входов и выходов, блоков для чтения, полос прокрутки
        /// </summary>
        private void CreateComponents()
        {
            _inVarsDigital.Clear();
            _outVarsDigital.Clear();
            _inVarsAnalog.Clear();
            _outVarsAnalog.Clear();


            if (_solution != null)
            {
                //Цифровые переменные
                Match m;
                for (int i = 0; i < _solution.Vars.IOVars.Count; i++)
                {
                    m = Regex.Match(_solution.Vars.IOVars[i].Name, "IN(\\d+)");
                    if (((m != null) && (m.Length != 0)) && (!_inVarsDigital.ContainsKey(Convert.ToInt32(m.Groups[1].Value))) && (_solution.Vars.IOVars[i].Address != 0))
                    {
                        if (!_solution.Vars.IOVars[i].ExternalModule)
                                _inVarsDigital.Add(Convert.ToInt32(m.Groups[1].Value), _solution.Vars.IOVars[i]);
                    }
                    m = Regex.Match(_solution.Vars.IOVars[i].Name, "OUT(\\d+)");
                    if (((m != null) && (m.Length != 0)) && (!_outVarsDigital.ContainsKey(Convert.ToInt32(m.Groups[1].Value))) && (_solution.Vars.IOVars[i].Address != 0))
                    {
                        if (!_solution.Vars.IOVars[i].ExternalModule)
                            _outVarsDigital.Add(Convert.ToInt32(m.Groups[1].Value), _solution.Vars.IOVars[i]);
                    }
                }
                if (_inVarsDigital.Count > 0 && _outVarsDigital.Count > 0)
                {
                    this.digitalIO.NewComponents(_inVarsDigital.Keys, _outVarsDigital.Keys,  SystemColors.ControlLightLight, SystemColors.Control);
                }
                else
                {
                    this.digitalIO.NewComponents("Нет проекта для определения адресов датчиков!!!");
                }

                //Аналоговые переменные
                for (int i = 0; i < _solution.Vars.IOVars.Count; i++)
                {
                    m = Regex.Match(_solution.Vars.IOVars[i].Name, "ADC(\\d+)");
                    if (((m != null) && (m.Length != 0)) && (!_inVarsAnalog.ContainsKey(Convert.ToInt32(m.Groups[1].Value))) && (_solution.Vars.IOVars[i].Address != 0))
                    {
                        if (!_solution.Vars.IOVars[i].ExternalModule)
                            _inVarsAnalog.Add(Convert.ToInt32(m.Groups[1].Value), _solution.Vars.IOVars[i]);
                    }
                    m = Regex.Match(_solution.Vars.IOVars[i].Name, "DAC(\\d+)");
                    if (((m != null) && (m.Length != 0)) && (!_outVarsAnalog.ContainsKey(Convert.ToInt32(m.Groups[1].Value))) && (_solution.Vars.IOVars[i].Address != 0))
                    {
                        if (!_solution.Vars.IOVars[i].ExternalModule)
                            _outVarsAnalog.Add(Convert.ToInt32(m.Groups[1].Value), _solution.Vars.IOVars[i]);
                    }
                }

                //Создание компонентов для отображения аналоговых датчиков
                this.pInput.Controls.Clear();
                this.ascInputs = new Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl[_inVarsAnalog.Count];
                IList<int> m_key=_inVarsAnalog.Keys;
                for (int i = 0; i < _inVarsAnalog.Count; i++)
                {
                    this.ascInputs[i]=new Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl(2);
                    this.pInput.Controls.Add(ascInputs[i]);
                    this.ascInputs[i].BackColor = System.Drawing.SystemColors.Control;
                    this.ascInputs[i].InverseByteOrder = debuggerEngine.Parameters.InverseByteOrder;
                    this.ascInputs[i].Location = new System.Drawing.Point(0, 0 + i * (this.ascInputs[i].Height + 0));
                    this.ascInputs[i].ValueFieldColor = Color.FromArgb(102, 254, 51);
                    this.ascInputs[i].SensorLabel = "";
                    this.ascInputs[i].SensorName = _inVarsAnalog[m_key[i]].Name;
                    this.ascInputs[i].ValueChanged += new System.EventHandler(this.ViewIOSensorsTabbedDocument_ValueChanged);
                    this.ascInputs[i].LabelChanged += new System.EventHandler(this.ViewIOSensorsTabbedDocument_LabelChanged);
                    this.ascInputs[i].OneByteChanged += new System.EventHandler(this.ViewIOSensorsTabbedDocument_OneByteChanged);
                }
                if (this.ascInputs.Length > 0)
                    this.gbInputs.Size = new System.Drawing.Size(this.ascInputs[0].Size.Width + 10, this.gbInputs.Size.Height);
                this.pOutput.Controls.Clear();
                this.ascOutputs = new Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl[_outVarsAnalog.Count];
                m_key = _outVarsAnalog.Keys;
                for (int i = 0; i < _outVarsAnalog.Count; i++)
                {
                    //если стандартные аналоговые выходы 1байт, то первая строка, если 2, то вторая
                    //this.ascOutputs[i] = new Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl(1);
                    this.ascOutputs[i] = new Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl(2);
                    this.pOutput.Controls.Add(ascOutputs[i]);
                    this.ascOutputs[i].BackColor = System.Drawing.SystemColors.Control;
                    this.ascOutputs[i].InverseByteOrder = debuggerEngine.Parameters.InverseByteOrder;
                    this.ascOutputs[i].Location = new System.Drawing.Point(0, 0 + i * (this.ascInputs[i].Height + 0));
                    this.ascOutputs[i].SensorLabel = "";
                    this.ascOutputs[i].ValueFieldColor = Color.FromArgb(255, 121, 75);
                    this.ascOutputs[i].SensorName = _outVarsAnalog[m_key[i]].Name;
                    this.ascOutputs[i].ValueChanged += new System.EventHandler(this.ViewIOSensorsTabbedDocument_ValueChanged);
                    this.ascOutputs[i].LabelChanged += new System.EventHandler(this.ViewIOSensorsTabbedDocument_LabelChanged);
                    this.ascOutputs[i].OneByteChanged += new System.EventHandler(this.ViewIOSensorsTabbedDocument_OneByteChanged);
                }
            }
            else
            {
                this.digitalIO.NewComponents("Нет проекта для определения адресов датчиков!!!");
            }
        }

        /// <summary>
        /// Составление опросов датчиков
        /// </summary>
        private void AddedReadItems()
        {
            IList<int> m_key;
            if (this.tabControl1.SelectedPage!=null && this.tabControl1.SelectedPage.Name == "tpDigital")
            {
                //Запуск стандартных датчиков
                //Запуск на четние цифровых датчиков
                m_key = _inVarsDigital.Keys;
                for (int i = 0; i < _inVarsDigital.Count; i++)
                {
                    if (this.cbDefault.Checked || debuggerEngine.Parameters.ProcessorType == ProcessorType.AT89C51ED2 || i > 3)
                    {
                        try { debuggerEngine.AddReadItem(_inVarsDigital[m_key[i]].Address, _inVarsDigital[m_key[i]].Memory, _inVarsDigital[m_key[i]].Size, "DIN_" + m_key[i], null, RefreshInterfaseDigital); }
                        catch { }
                    }
                }
                m_key = _outVarsDigital.Keys;
                for (int i = 0; i < _outVarsDigital.Count; i++)
                {
                    if (this.cbDefault.Checked || debuggerEngine.Parameters.ProcessorType == ProcessorType.AT89C51ED2 || i > 3)
                    {
                        try { debuggerEngine.AddReadItem(_outVarsDigital[m_key[i]].Address, _outVarsDigital[m_key[i]].Memory, _outVarsDigital[m_key[i]].Size, "DOUT_" + m_key[i], null, RefreshInterfaseDigital); }
                        catch { }
                    }
                }
                //Запуск на чтение аналоговых датчиков
                Kontel.Relkon.Classes.ControllerVar m_var;
                for (int i = 0; i < this.ascInputs.Length; i++)
                {
                    if (this.cbDefault.Checked || debuggerEngine.Parameters.ProcessorType == ProcessorType.AT89C51ED2 || i > 7)
                    {
                        m_var = _solution.Vars.GetIOVar(this.ascInputs[i].SensorName);
                        try { debuggerEngine.AddReadItem(m_var.Address, m_var.Memory, m_var.Size, this.ascInputs[i], null, RefreshInterfaseAnalog); }
                        catch { }
                    }
                }
                for (int i = 0; i < this.ascOutputs.Length; i++)
                {
                    if (this.cbDefault.Checked || debuggerEngine.Parameters.ProcessorType == ProcessorType.AT89C51ED2 || i > 1)
                    {
                        m_var = _solution.Vars.GetIOVar(this.ascOutputs[i].SensorName);
                        try { debuggerEngine.AddReadItem(m_var.Address, m_var.Memory, m_var.Size, this.ascOutputs[i], null, RefreshInterfaseAnalog); }
                        catch { }
                    }
                }
            }
            else
            {
                //Запуск датчиков с открытой вкладки
                for (int i = 0; i < tpBloks.Count; i++)
                {
                    TD.SandDock.TabPage tp = tpBloks[tpBloks.Keys[i]];
                    if (this.tabControl1.SelectedPage != null && tp.Name == this.tabControl1.SelectedPage.Name && tp.Name!="")
                    {
                        ((DisplayBlock)tp.Controls[0]).AddedReadItems();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Остановка опросов датчиков
        /// </summary>
        private void RemoveReadItems()
        {
            //Остановка чения стандартных датчиков
            //Остановка чтения цифровых датчиков
            IList<int> m_key = _inVarsDigital.Keys;
            for (int i = 0; i < _inVarsDigital.Count; i++)
            {
                try { debuggerEngine.RemoveReadItem(_inVarsDigital[m_key[i]].Address, _inVarsDigital[m_key[i]].Memory, "DIN_" + m_key[i]); }
                catch { }
            }
            m_key = _outVarsDigital.Keys;
            for (int i = 0; i < _outVarsDigital.Count; i++)
            {
                try { debuggerEngine.RemoveReadItem(_outVarsDigital[m_key[i]].Address, _outVarsDigital[m_key[i]].Memory, "DOUT_" + m_key[i]); }
                catch {}
            }
            //Остановка чения аналоговых датчиков
            for (int i = 0; i < this.ascInputs.Length; i++)
            {
                Kontel.Relkon.Classes.ControllerVar m_var = _solution.Vars.GetVarByName(this.ascInputs[i].SensorName);
                try { debuggerEngine.RemoveReadItem(m_var.Address, m_var.Memory, this.ascInputs[i]); }
                catch {}
            }
            for (int i = 0; i < this.ascOutputs.Length; i++)
            {
                Kontel.Relkon.Classes.ControllerVar m_var = _solution.Vars.GetVarByName(this.ascOutputs[i].SensorName);
                try { debuggerEngine.RemoveReadItem(m_var.Address, m_var.Memory, this.ascOutputs[i]); }
                catch {}
            }
            //Остановка чтения остальных вкладок
            for (int i = 0; i < tpBloks.Count; i++)
            {
                TD.SandDock.TabPage tp = tpBloks[tpBloks.Keys[i]];
                if ((_deletedPage == -1 || _deletedPage != (int)(((DisplayBlock)tp.Controls[0]).bDelete.Tag)) && tp.Controls.Count > 0/*tp.Disposing*/)
                    ((DisplayBlock)tp.Controls[0]).RemoveReadItems();
            }
        }

        /// <summary>
        /// Изменение значения аналогого датчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewIOSensorsTabbedDocument_ValueChanged(object sender, EventArgs e)
        {
            Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl Sender = (Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl)sender;
            if (debuggerEngine.EngineStatus == DebuggerEngineStatus.Started)
            {
                Kontel.Relkon.Classes.ControllerVar m_var = _solution.Vars.GetVarByName(Sender.SensorName);
                debuggerEngine.AddWriteItem(m_var.Address, m_var.Memory, Sender.GetData(), "Analog_W_" + m_var.Name, null, null);
            }

        }
        /// <summary>
        /// Изменение параметров отображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewIOSensorsTabbedDocument_OneByteChanged(object sender, EventArgs e)
        {
            Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl Sender = (Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl)sender;
            if (Sender.SensorName.Contains("ADC"))
            {
                for (int i = 0; i < debuggerEngine.Parameters.ADCSensors.Count; i++)
                    if (debuggerEngine.Parameters.ADCSensors[i].Name == Sender.SensorName)
                    {
                        debuggerEngine.Parameters.ADCSensors[i].DisplayOneByte = Sender.SigleByte;
                        return;
                    }
                //добавления метки в параметры отладчика
                Kontel.Relkon.DebuggerParameters.AnalogSensorDescription m_s = new DebuggerParameters.AnalogSensorDescription();
                m_s.Name = Sender.SensorName;
                m_s.Caption = Sender.SensorLabel;
                m_s.DisplayOneByte = Sender.SigleByte;
                ControllerIOVar CurentValue = _solution.Vars.GetIOVar(Sender.SensorName);
                m_s.MemoryType = CurentValue.Memory;
                m_s.Address = CurentValue.Address;
                debuggerEngine.Parameters.ADCSensors.Add(m_s);
            }
            else
            {
                for (int i = 0; i < debuggerEngine.Parameters.DACSensors.Count; i++)
                    if (debuggerEngine.Parameters.DACSensors[i].Name == Sender.SensorName)
                    {
                        debuggerEngine.Parameters.DACSensors[i].DisplayOneByte = Sender.SigleByte;
                        return;
                    }
                //добавления метки в параметры отладчика
                Kontel.Relkon.DebuggerParameters.AnalogSensorDescription m_s = new DebuggerParameters.AnalogSensorDescription();
                m_s.Name = Sender.SensorName;
                m_s.Caption = Sender.SensorLabel;
                m_s.DisplayOneByte = Sender.SigleByte;
                ControllerIOVar CurentValue = _solution.Vars.GetIOVar(Sender.SensorName);
                m_s.MemoryType = CurentValue.Memory;
                m_s.Address = CurentValue.Address;
                debuggerEngine.Parameters.DACSensors.Add(m_s);
            }
        }

        /// <summary>
        /// Изменение метки аналогого датчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewIOSensorsTabbedDocument_LabelChanged(object sender, EventArgs e)
        {
            Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl Sender = (Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl)sender;
            if (Sender.SensorName.Contains("ADC"))
            {
                for (int i = 0; i < debuggerEngine.Parameters.ADCSensors.Count; i++)
                    if (debuggerEngine.Parameters.ADCSensors[i].Name == Sender.SensorName)
                    {
                        debuggerEngine.Parameters.ADCSensors[i].Caption = Sender.SensorLabel;
                        return;
                    }
             //добавления метки в параметры отладчика
                Kontel.Relkon.DebuggerParameters.AnalogSensorDescription m_s = new DebuggerParameters.AnalogSensorDescription();
                m_s.Name = Sender.SensorName;
                m_s.Caption = Sender.SensorLabel;
                m_s.DisplayOneByte = Sender.SigleByte;
                ControllerIOVar CurentValue = _solution.Vars.GetIOVar(Sender.SensorName);
                m_s.MemoryType = CurentValue.Memory;
                m_s.Address = CurentValue.Address;
                debuggerEngine.Parameters.ADCSensors.Add(m_s);
            }
            else
            {
                for (int i = 0; i < debuggerEngine.Parameters.DACSensors.Count; i++)
                    if (debuggerEngine.Parameters.DACSensors[i].Name == Sender.SensorName)
                    {
                        debuggerEngine.Parameters.DACSensors[i].Caption = Sender.SensorLabel;
                        return;
                    }
                //добавления метки в параметры отладчика
                Kontel.Relkon.DebuggerParameters.AnalogSensorDescription m_s = new DebuggerParameters.AnalogSensorDescription();
                m_s.Name = Sender.SensorName;
                m_s.Caption = Sender.SensorLabel;
                m_s.DisplayOneByte = Sender.SigleByte;
                ControllerIOVar CurentValue = _solution.Vars.GetIOVar(Sender.SensorName);
                m_s.MemoryType = CurentValue.Memory;
                m_s.Address = CurentValue.Address;
                debuggerEngine.Parameters.DACSensors.Add(m_s);
            }
        }

        /// <summary>
        /// Запись цифровых входов/выходов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void digitalIO_StateChange(object sender, DigitalIO.StateChangeEventArgs e)
        {
            if (debuggerEngine.EngineStatus == DebuggerEngineStatus.Started)
                try
                {
                    if (!e.IsInput)
                    {
                        debuggerEngine.AddWriteItem(_outVarsDigital[e.Key].Address, _outVarsDigital[e.Key].Memory, new Byte[] { e.New_value }, "DOUT_W_" + _outVarsDigital[e.Key].Name+"_"+e.Index, null, WriteFinish);
                    }
                    else
                    {
                        debuggerEngine.AddWriteItem(_inVarsDigital[e.Key].Address, _inVarsDigital[e.Key].Memory, new Byte[] { e.New_value }, "DIN_W_" + _inVarsDigital[e.Key].Name+"_"+e.Index, null,WriteFinish);
                    }
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage("digitalIO_StateChange:" + ex.Message);
                    return;
                }
        }

        /// <summary>
        /// Остановка запросов, если окно перекрыто
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DigitalIOTabbedDocument_VisibleChanged(object sender, EventArgs e)
        {
            if ((this.IsOpen) && (!this._IsReading))
            {
                this._IsReading = true;
                //Запуск опросов
                this.AddedReadItems();
            }
            else
            {
                if ((this._IsReading) && (!this.IsOpen))
                {
                    this._IsReading = false;
                    //Остановка опросов
                    this.RemoveReadItems(); 
                }
            }
        }

        /// <summary>
        /// Смена описания цифрового датчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void digitalIO_LabelChange(object sender, DigitalIO.LabelChangeEventArgs e)
        {
            if (e.IsInput)
            {
                for (int i = 0; i < debuggerEngine.Parameters.DINSensors.Count+1; i++)
                {
                    if (i == debuggerEngine.Parameters.DINSensors.Count)
                    {
                        Kontel.Relkon.DebuggerParameters.SensorLabels m_caption = new DebuggerParameters.SensorLabels();
                        m_caption.Number = e.Index;
                        m_caption.Caption = e.Text;
                        Kontel.Relkon.DebuggerParameters.DigitalSensorDescription m_sensor = new DebuggerParameters.DigitalSensorDescription();
                        m_sensor.Labels.Add(m_caption);
                        m_sensor.Name = "DIN" + e.Key;
                        debuggerEngine.Parameters.DINSensors.Add(m_sensor);
                        break;
                    }
                    if (debuggerEngine.Parameters.DINSensors[i].Name == ("DIN" + e.Key))
                    {
                        if ((e.Text != null) && (e.Text != ""))
                        {
                            bool m_exit = false;
                            for (int j = 0; j < debuggerEngine.Parameters.DINSensors[i].Labels.Count; j++)
                                if (debuggerEngine.Parameters.DINSensors[i].Labels[j].Number == e.Index)
                                {
                                    debuggerEngine.Parameters.DINSensors[i].Labels[j].Caption = e.Text; m_exit = true;
                                    break;
                                }
                            if (!m_exit)
                            {
                                Kontel.Relkon.DebuggerParameters.SensorLabels m_caption = new DebuggerParameters.SensorLabels();
                                m_caption.Number = e.Index;
                                m_caption.Caption = e.Text;
                                debuggerEngine.Parameters.DINSensors[i].Labels.Add(m_caption);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < debuggerEngine.Parameters.DINSensors[i].Labels.Count; j++)
                            {
                                if (debuggerEngine.Parameters.DINSensors[i].Labels[j].Number == e.Index)
                                { debuggerEngine.Parameters.DINSensors[i].Labels.Remove(debuggerEngine.Parameters.DINSensors[i].Labels[j]); break; }
                            }
                        }
                        if (debuggerEngine.Parameters.DINSensors[i].Labels.Count == 0)
                            debuggerEngine.Parameters.DINSensors.Remove(debuggerEngine.Parameters.DINSensors[i]);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < debuggerEngine.Parameters.DOUTSensors.Count + 1; i++)
                {
                    if (i == debuggerEngine.Parameters.DOUTSensors.Count)
                    {
                        Kontel.Relkon.DebuggerParameters.SensorLabels m_caption = new DebuggerParameters.SensorLabels();
                        m_caption.Number = e.Index;
                        m_caption.Caption = e.Text;
                        Kontel.Relkon.DebuggerParameters.DigitalSensorDescription m_sensor = new DebuggerParameters.DigitalSensorDescription();
                        m_sensor.Labels.Add(m_caption);
                        m_sensor.Name = "DOUT" + e.Key;
                        debuggerEngine.Parameters.DOUTSensors.Add(m_sensor);
                        break;
                    }
                    if (debuggerEngine.Parameters.DOUTSensors[i].Name == ("DOUT" + e.Key))
                    {
                        if ((e.Text != null) && (e.Text != ""))
                        {
                            bool m_exit = false;
                            for (int j = 0; j < debuggerEngine.Parameters.DOUTSensors[i].Labels.Count; j++)
                                if (debuggerEngine.Parameters.DOUTSensors[i].Labels[j].Number == e.Index)
                                {
                                    debuggerEngine.Parameters.DOUTSensors[i].Labels[j].Caption = e.Text; m_exit = true;
                                    break;
                                }
                            if (!m_exit)
                            {
                                Kontel.Relkon.DebuggerParameters.SensorLabels m_caption = new DebuggerParameters.SensorLabels();
                                m_caption.Number = e.Index;
                                m_caption.Caption = e.Text;
                                debuggerEngine.Parameters.DOUTSensors[i].Labels.Add(m_caption);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < debuggerEngine.Parameters.DOUTSensors[i].Labels.Count; j++)
                            {
                                if (debuggerEngine.Parameters.DOUTSensors[i].Labels[j].Number == e.Index)
                                { debuggerEngine.Parameters.DOUTSensors[i].Labels.Remove(debuggerEngine.Parameters.DOUTSensors[i].Labels[j]); break; }
                            }
                        }
                        if (debuggerEngine.Parameters.DOUTSensors[i].Labels.Count == 0)
                            debuggerEngine.Parameters.DOUTSensors.Remove(debuggerEngine.Parameters.DOUTSensors[i]);
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// При изменении активной вкладки производится остановка запросов с одной вкладки и запуск с другой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedPageChanged(object sender, EventArgs e)
        {
            if ((this.IsOpen) && (this._IsReading))
            {
                //Остановка запросов
                this.RemoveReadItems();
                this.AddedReadItems();
            }
            try{((DisplayBlock)((TD.SandDock.TabControl)(sender)).SelectedPage.Controls[0]).ChangeLabels(); }
            catch{}
        }


        private void ViewIOSensorsTabbedDocument_Closing(object sender, TD.SandDock.DockControlClosingEventArgs e)
        {
            _IsOpen = false;
            debuggerEngine.EngineStatusChanged -= new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
            foreach (TD.SandDock.TabPage tp in this.tpBloks.Values)
            {
                debuggerEngine.EngineStatusChanged -= new EventHandler<DebuggerEngineStatusChangedEventArgs>(((DisplayBlock)tp.Controls.Find("displayBlock", true)[0]).DebuggerParametersList_ChangeStatusEngine);
            }
            this.RemoveReadItems();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Установка невидимыми стандартных аналоговых датчиков
            if (this.cbDefault.Checked)
            {
                this.tabControl1.Controls.Add(_tpDefault);
                this.tabControl1.Controls.SetChildIndex(_tpDefault, 0);
            }
            else
                this.tabControl1.Controls.Remove(_tpDefault);
            this.RemoveReadItems();
            this.AddedReadItems();
            debuggerEngine.Parameters.DisplayDefault = debuggerEngine.Parameters.ProcessorType == ProcessorType.STM32F107 ? this.cbDefault.Checked : debuggerEngine.Parameters.DisplayDefault;
            if (!debuggerEngine.Parameters.DisplayDefault) debuggerEngine.EngineStatusChanged -= new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
        }

        //`////////////////////////////////////////////////////////////////
        #region Возможность просмотра модулей
        /// <summary>
        /// Создание вкладкидля опроса нового модуля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Определение максимального номера вкладки
            int max = 0;
            for (int i = 0; i < tpBloks.Keys.Count; i++)
                if (tpBloks.Keys[i] > max) max = tpBloks.Keys[i];
            max++;
            CreateTabPage(max, "Вкладка" + max.ToString(),true);
            ((DisplayBlock)this.tpBloks[max].Controls.Find("displayBlock", true)[0]).Update_presentation(_solution, debuggerEngine);
        }

        private void CreateTabPage(int Number, string Name,bool IsNew)
        {
            //Создаение новой вкладки с заданным номером
            tpBloks.Add(Number, new TD.SandDock.TabPage());
            this.tabControl1.Controls.Add(tpBloks[Number]);
            DisplayBlock displayBlock = new DisplayBlock(_solution, debuggerEngine,Number);
            tpBloks[Number].Controls.Add(displayBlock);
            tpBloks[Number].Name="tpBloks_"+Number;//не уверена что надо
            displayBlock.Dock = DockStyle.Fill;
            displayBlock.tbCaption.Tag = Number;
            displayBlock.bDelete.Tag = Number;
            displayBlock.tbCaption.TextChanged += new System.EventHandler(this.tbCaption_TextChanged);
            displayBlock.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            //Запись новой вкадки в параметры отладчика
            if (IsNew)
            {
                Kontel.Relkon.DebuggerParameters.Block b = new DebuggerParameters.Block();
                b.Caption = "";
                b.Number = Number;
                b.Vars = new List<string>();
                debuggerEngine.Parameters.ModulBlocks.Add(b);
            }
            //Установка значения вкладки по умолчанию
            displayBlock.tbCaption.Text = Name;
        }

        private void tbCaption_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tpBloks[(int)tb.Tag].Text = tb.Text;
            foreach (Kontel.Relkon.DebuggerParameters.Block b in debuggerEngine.Parameters.ModulBlocks)
            {
                if (b.Number == (int)tb.Tag)
                {
                    b.Caption = tb.Text;
                    break;
                }
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            _deletedPage = (int)b.Tag;
            //Поиск и удаление вкладки из параметров отладчик
            foreach (Kontel.Relkon.DebuggerParameters.Block bl in debuggerEngine.Parameters.ModulBlocks)
            {
                if (bl.Number == (int)b.Tag)
                {
                    debuggerEngine.Parameters.ModulBlocks.Remove(bl);
                    break;
                }
            }
            ((DisplayBlock)this.tpBloks[(int)b.Tag].Controls.Find("displayBlock", true)[0]).RemoveReadItems();
            this.tpBloks[(int)b.Tag].Dispose();
            tpBloks.Remove((int)b.Tag);
            _deletedPage = -1;
        }
        #endregion
    }
}

