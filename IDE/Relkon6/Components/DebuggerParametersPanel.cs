using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using TD.SandDock;
using Kontel.Relkon.Solutions;
using Kontel.Relkon.Components.Documents;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using Kontel.Relkon.Debugger;
using Kontel.Relkon.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Kontel.Relkon.Components
{
    /// <summary>
    /// Движок опроса контроллера
    /// </summary>
    public sealed partial class DebuggerParametersPanel : UserDockableWindow
    {
        private SerialPortDeviceSearcher controllerSearcher = null; // осуществляет автоматический поиск контролера
        private ProgressForm progressForm = null; // Отображает процесс поиска контроллера
        private DebuggerEngine engine = null; // движок отладчика, работой которого управляет компонент
        private bool lockDateTimePeaker = false; // показывает, что значение dateTimePicker для времени контроллера обновлять не требуется
        private bool clockWrite = false; // показывает, что в данный момент идет запись времени в контроллер
        private System.Timers.Timer timer = null;

        //значения входов, выходов в сохраненнмо файле
        private List<Kontel.Relkon.DebuggerParameters.AnalogSensorDescription> adc = new List<DebuggerParameters.AnalogSensorDescription>();
        private List<Kontel.Relkon.DebuggerParameters.AnalogSensorDescription> dac = new List<DebuggerParameters.AnalogSensorDescription>();
        private List<Kontel.Relkon.DebuggerParameters.DigitalSensorDescription> din = new List<DebuggerParameters.DigitalSensorDescription>();
        private List<Kontel.Relkon.DebuggerParameters.DigitalSensorDescription> dout = new List<DebuggerParameters.DigitalSensorDescription>();

        /// <summary>
        /// Возникает при смене типа процессора опрашиваемого контроллера
        /// </summary>
        public event EventHandler<EventArgs<ProcessorType>> ProcessorChanged;
        /// <summary>
        /// Возникает после загрузки параметров отладчика из файла (после щелчка по кнопке "Загрузить конфигурацию")
        /// </summary>
        public event EventHandler DebuggertParametersUpdated;

        public DebuggerParametersPanel()
        {
            InitializeComponent();

            this.timer = new System.Timers.Timer(100);
            this.timer.AutoReset = false;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);         

            this.ddlPortName.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            
            //foreach (ProcessorType pt in Enum.GetValues(typeof(ProcessorType)))
            //    ddlProcessorType.Items.Add(pt);

            //ddlProcessorType.SelectedItem = ProcessorType.STM32F107;


           
            ddlProtocol.Items.Add(ProtocolType.RC51ASCII);
            ddlProtocol.Items.Add(ProtocolType.RC51BIN);
        }
        /// <summary>
        /// Движок отладчика, работой которого управляет компонент
        /// </summary>
        public DebuggerEngine DebuggerEngine 
        {
            get
            {
                return this.engine;
            }
            set
            {
                if (this.engine != value)
                {
                    this.engine = value;
                    this.engine.EngineStatusChanged += new EventHandler<DebuggerEngineStatusChangedEventArgs>(engine_EngineStatusChanged);
                    this.engine.RequestTimeChanged += new EventHandler<EventArgs<int>>(engine_RequestTimeChanged);
                    this.engine.AddReadItem(0, MemoryType.Clock, 8, "clock_reading", null, new ProceedingCompleetedDelegate(this.ClockReaded));
                }
            }
        }
        /// <summary>
        /// Обновляет значения компонентов, задающих параметры связи с контроллером
        /// </summary>
        public void UpdateControlerParameters()
        {
            ///////////
            LoadValues(this.engine.Parameters);
            ///////////

            ProcessorType p = ProcessorType.STM32F107;
            this.DebuggerEngine.Parameters.ProcessorType = p;
            ControllerProgramSolution sln = ControllerProgramSolution.Create(p);
            //this.rbInverse.Checked = sln.ProcessorParams.InverseByteOrder;
            Program.Settings.DeBugger_SettingsProcessesorType = p.ToString();
            this.RaiseProcessorChangedEvent(p);


            //this.ddlProcessorType.SelectedItem = this.engine.Parameters.ProcessorType;
            this.ddlPortName.SelectedItem = this.engine.Parameters.PortName;
            this.nudControllerAddress.Value = this.engine.Parameters.ControllerNumber;
            this.ddlBaudRate.SelectedItem = this.engine.Parameters.BaudRate.ToString();
            this.ddlProtocol.SelectedItem = this.engine.Parameters.ProtocolType;
            //this.tbReadPassword.Text = this.engine.Parameters.ReadPassword;
            //this.tbWritePassword.Text = this.engine.Parameters.WritePassword;
            //this.nudPort.Value = this.engine.Parameters.PortNumber;
            //this.tbIP.Text = this.engine.Parameters.PortIP;
            //this.tabControl1.SelectedTab = this.engine.Parameters.ComConection ? this.tabCom : this.tabEthernet;
        }
        /// <summary>
        /// Функция, вызывающаяся после считывания времени с контролера
        /// </summary>
        public void ClockReaded(object Sender, byte[] Data, bool Error)
        {
            if (Error)
                return;
            DateTime value = (DateTime)Relkon4Protocol.ConvertDate(Data);
            if (!this.lockDateTimePeaker && value != null)
                this.dateTimePicker1.Value = value;
        }
        /// <summary>
        /// Функция, вызывающаяся после записи времени в контроллер
        /// </summary>
        private void ClockWrited(object Sender, byte[] Data, bool Error)
        {
            this.lockDateTimePeaker = this.clockWrite = false;
            dateTimePicker1_Enter(null, null);
        }
        /// <summary>
        /// Генерирует событие ProcessorChanged 
        /// </summary>
        private void RaiseProcessorChangedEvent(ProcessorType ProcessorType)
        {
            if (this.ProcessorChanged != null)
                this.ProcessorChanged(this, new EventArgs<ProcessorType>(ProcessorType));
        }
        /// <summary>
        /// Генерирует событие DebuggerParametersUpdated
        /// </summary>
        private void RaiseDebuggerParametersUpdated()
        {
            if (this.DebuggertParametersUpdated != null)
                this.DebuggertParametersUpdated(this, EventArgs.Empty);
        }

        private void EnableParametersControls()
        {

        }

        

        private void ddPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DebuggerEngine.Parameters.PortName = Program.Settings.DeBugger_Settings_ComPort = (string)this.ddlPortName.SelectedItem;
        }

        private void bRefreshPortNames_Click(object sender, EventArgs e)
        {
            this.ddlPortName.Items.Clear();
            this.ddlPortName.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            if (this.ddlPortName.Items.Count != 0)
                this.ddlPortName.SelectedIndex = 0;
        }

        private void nudControllerAddress_ValueChanged(object sender, EventArgs e)
        {
            this.DebuggerEngine.Parameters.ControllerNumber = Program.Settings.DeBugger_Settings_NumberLink = (int)this.nudControllerAddress.Value;
        }

        private void ddlBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DebuggerEngine.Parameters.BaudRate = Program.Settings.DeBugger_Settings_BaudRate = int.Parse((string)this.ddlBaudRate.SelectedItem);
        }

        private void ddlProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DebuggerEngine.Parameters.ProtocolType = (ProtocolType)this.ddlProtocol.SelectedItem;
            Program.Settings.DeBugger_Settings_Protocol = ((ProtocolType)this.ddlProtocol.SelectedItem).ToString();
        }

        //private void rbInverse_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.DebuggerEngine.Parameters.InverseByteOrder = this.rbInverse.Checked;
        //}

        private void bAutoScan_Click(object sender, EventArgs e)
        {
            this.progressForm = new ProgressForm(MainForm.MainFormInstance);
            this.progressForm.FormClosing += new FormClosingEventHandler(this.progressForm_FormClosing);
            this.progressForm.Message = "Поиск контроллера...";


            this.controllerSearcher = new SerialPortDeviceSearcher(null, null);
            this.controllerSearcher.Pattern = "Relkon 6";
            this.controllerSearcher.BootPattern = "boot";
            this.controllerSearcher.Request = new byte[] { 0x00, 0xA0 };
           
            this.controllerSearcher.DeviceSearchCompleted += new DeviceSearchCompletedEventHandler(controllerSearcher_DeviceSearchCompleted);
            this.controllerSearcher.ProgressChanged += new ProgressChangedEventHandler(controllerSearcher_ProgressChanged);
          
            this.controllerSearcher.StartSearch();
            this.progressForm.ShowDialog();
        }

        private void progressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.controllerSearcher.StopSearch();
            while (this.controllerSearcher.IsBusy)
                Application.DoEvents();
        }

        private void controllerSearcher_DeviceSearchCompleted(object sender, DeviceSearchCompletedEventArgs e)
        {
            if (e.Error != null)
                Utils.ErrorMessage(e.Error.Message);
            else if (!e.Cancelled)
            {
                //Получение строки из массива байт
                string response = Encoding.Default.GetString(e.DeviceResponse).ToLower();

               
                if (response.Contains("relkon 6"))
                {
                    this.DebuggerEngine.Parameters.ProcessorType = ProcessorType.STM32F107;
                    this.nudControllerAddress.Value = e.DeviceResponse[0];

                    this.ddlPortName.SelectedItem = e.Port.PortName;
                    this.ddlBaudRate.SelectedItem = e.Port.BaudRate.ToString();
                    this.ddlProtocol.SelectedItem = e.Port.Protocol;
                }
                else
                {
                    Utils.WarningMessage("Контроллер находится в режиме загрузчика.\n Работка отладчика не возможна.");
                }
              
            }
            this.progressForm.Close();
        }

        private void controllerSearcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressForm.ProgressPercentage = e.ProgressPercentage;
            this.progressForm.Message = "Поиск контроллера...";
        }

        private void bStartEngine_Click(object sender, EventArgs e)
        {
            if (DebuggerEngine.EngineStatus == DebuggerEngineStatus.Started)
            {
                this.DebuggerEngine.Stop();
            }
            else if (this.DebuggerEngine.EngineStatus == DebuggerEngineStatus.Stopped)
            {
                //установка недоступности полей на время соединения
                this.gbSettingsConnect.Enabled = false;
                //попытка запуска отладчика
                this.DebuggerEngine.Start();
            }
        }
        
        private void RefreshFormTimer_Tick(object sender, EventArgs e)
        {
            this.lReadPacket.Text = this.DebuggerEngine.ReadedRequestsCount.ToString();
            this.lErrorReadPacket.Text = this.DebuggerEngine.ErrorReadedRequestsCount.ToString();
            this.lWritePacket.Text = this.DebuggerEngine.WritedRequestsCount.ToString();
            this.lErrorWritePacket.Text = this.DebuggerEngine.ErrorWritedRequestsCount.ToString();
        }

        private void engine_RequestTimeChanged(object sender, EventArgs<int> e)
        {
            this.lRequestTime.Text = e.Value + " мс";
        }

        private void engine_EngineStatusChanged(object sender, DebuggerEngineStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case DebuggerEngineStatus.Started:
                    this.lEngineStatus.Text = "Работает";
                    this.lEngineStatus.ForeColor = Color.FromArgb(0, 153, 0);
                    ///////////////////////////////
                    this.RefreshFormTimer.Start();
                    this.gbSettingsConnect.Enabled = false;
                    this.bSyncTimeWithPC.Enabled = true;
                    this.bStart.Text = "Стоп";
                    break;
                case DebuggerEngineStatus.Stopping:
                    this.lEngineStatus.Text = "Останавливается";
                    this.lEngineStatus.ForeColor = Color.FromArgb(255, 204, 0);
                    break;
                case DebuggerEngineStatus.Stopped:
                    this.lEngineStatus.Text = "Остановлен";
                    this.lEngineStatus.ForeColor = Color.FromArgb(255, 51, 0);
                    if (e.Error != null)
                        Utils.ErrorMessage(e.Error.Message);
                    /////////////////////////////
                    this.gbSettingsConnect.Enabled = true;
                    this.bStart.Text = "Старт";
                    this.bSyncTimeWithPC.Enabled = false;
                    this.RefreshFormTimer.Stop();
                    break;
            }
        }
       

        /// <summary>
        /// Запись в массивы значений из указанного класса
        /// </summary>
        /// <param name="curentEngine"></param>
        private void LoadValues(DebuggerParameters curentEngine)
        {
            //сохранение значений датчиков
            adc = new List<DebuggerParameters.AnalogSensorDescription>();
            dac = new List<DebuggerParameters.AnalogSensorDescription>();
            din = new List<DebuggerParameters.DigitalSensorDescription>();
            dout = new List<DebuggerParameters.DigitalSensorDescription>();
            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription asc in curentEngine.ADCSensors)
            {
                Kontel.Relkon.DebuggerParameters.AnalogSensorDescription asd = new Kontel.Relkon.DebuggerParameters.AnalogSensorDescription();
                asd.Address = asc.Address;
                asd.Caption = asc.Caption;
                asd.DisplayOneByte = asc.DisplayOneByte;
                asd.HasSign = asc.HasSign;
                asd.MemoryType = asc.MemoryType;
                asd.Name = asc.Name;
                asd.Size = asc.Size;
                asd.Type = asc.Type;
                //asd.Value = asc.Value;
                if (asc.Value != null)
                {
                    asd.Value = new byte[asc.Value.Length];
                    Array.Copy(asc.Value, asd.Value, asc.Value.Length);
                }
                else { asd.Value = new byte[asc.Size]; }
                adc.Add(asd);
            }
            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription asc in curentEngine.DACSensors)
            {
                Kontel.Relkon.DebuggerParameters.AnalogSensorDescription asd = new Kontel.Relkon.DebuggerParameters.AnalogSensorDescription();
                asd.Address = asc.Address;
                asd.Caption = asc.Caption;
                asd.DisplayOneByte = asc.DisplayOneByte;
                asd.HasSign = asc.HasSign;
                asd.MemoryType = asc.MemoryType;
                asd.Name = asc.Name;
                asd.Size = asc.Size;
                asd.Type = asc.Type;
                //asd.Value = asc.Value;
                if (asc.Value != null)
                {
                    asd.Value = new byte[asc.Value.Length];
                    Array.Copy(asc.Value, asd.Value, asc.Value.Length);
                }
                else { asd.Value = new byte[asc.Size]; }
                dac.Add(asd);
            }
            foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription asc in curentEngine.DINSensors)
            {
                Kontel.Relkon.DebuggerParameters.DigitalSensorDescription asd = new Kontel.Relkon.DebuggerParameters.DigitalSensorDescription();
                asd.Address = asc.Address;
                asd.HasSign = asc.HasSign;
                asd.MemoryType = asc.MemoryType;
                asd.Name = asc.Name;
                asd.Size = asc.Size;
                asd.Type = asc.Type;
                asd.Labels = asc.Labels;
                //asd.Value = asc.Value;
                if (asc.Value != null)
                {
                    asd.Value = new byte[asc.Value.Length];
                    Array.Copy(asc.Value, asd.Value, asc.Value.Length);
                }
                else { asd.Value = new byte[asc.Size]; }
                asd.BitNumber = asc.BitNumber;
                din.Add(asd);
            }
            foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription asc in curentEngine.DOUTSensors)
            {
                Kontel.Relkon.DebuggerParameters.DigitalSensorDescription asd = new Kontel.Relkon.DebuggerParameters.DigitalSensorDescription();
                asd.Address = asc.Address;
                asd.HasSign = asc.HasSign;
                asd.MemoryType = asc.MemoryType;
                asd.Name = asc.Name;
                asd.Size = asc.Size;
                asd.Type = asc.Type;
                asd.Labels = asc.Labels;
                //asd.Value = asc.Value;
                if (asc.Value != null)
                {
                    asd.Value = new byte[asc.Value.Length];
                    Array.Copy(asc.Value, asd.Value, asc.Value.Length);
                }
                else { asd.Value = new byte[asc.Size]; }
                asd.BitNumber = asc.BitNumber;
                dout.Add(asd);
            }
        }

        private void bLoadConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.InitialDirectory =
            dialog.Filter = "Файл параметров отладчика|*.pdb";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DebuggerParameters dp = DebuggerParameters.FromFile(dialog.FileName);
                    if (engine != null && engine.EngineStatus == DebuggerEngineStatus.Started)
                    {
                        dp.ComConection = this.engine.Parameters.ComConection;
                        if (!this.engine.Parameters.ComConection)
                        {
                            dp.PortIP = this.engine.Parameters.PortIP;
                            dp.PortNumber = this.engine.Parameters.PortNumber;
                            dp.InterfaceProtocol = this.engine.Parameters.InterfaceProtocol;
                        }
                    }
                    this.engine.Parameters = dp;
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                    return;
                }
                this.UpdateControlerParameters();
                this.RaiseDebuggerParametersUpdated();
            }
        }

        private void bSaveConfig_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Файл параметров отладчика|*.pdb";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadValues(this.engine.Parameters);
                    this.engine.Parameters.Save(dialog.FileName);
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                }
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    this.lockDateTimePeaker = true;
        //    this.clockWrite = true;
        //    this.engine.AddWriteItem(0, MemoryType.Clock, Relkon37SerialPort.ConvertDate(this.dateTimePicker1.Value), "clock_writing", null, new ProceedingCompleetedDelegate(this.ClockWrited));
        //}

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.lockDateTimePeaker = true;
                this.clockWrite = true;
                this.engine.AddWriteItem(0, MemoryType.Clock, Relkon4Protocol.ConvertDate(this.dateTimePicker1.Value), "clock_writing", null, new ProceedingCompleetedDelegate(this.ClockWrited));
            }
        }
        private void dateTimePicker1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                dateTimePicker1_Leave(null, null);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            timer.Start();            
        }
        
        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            this.lockDateTimePeaker = true;
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!this.clockWrite)
                this.lockDateTimePeaker = false;
        }

        //private void tbReadPassword_TextChanged(object sender, EventArgs e)
        //{
        //    this.engine.Parameters.ReadPassword = this.tbReadPassword.Text;
        //}

        //private void tbWritePassword_TextChanged(object sender, EventArgs e)
        //{
        //    this.engine.Parameters.WritePassword = this.tbWritePassword.Text;
        //}

        //private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //        this.DebuggerEngine.Parameters.ComConection = this.tabControl1.SelectedTab == tabCom;
        //}

        //private void nudPort_ValueChanged(object sender, EventArgs e)
        //{
        //        this.DebuggerEngine.Parameters.PortNumber = (int)this.nudPort.Value;
        //}

        //private void tbIP_TextChanged(object sender, EventArgs e)
        //{
        //        this.DebuggerEngine.Parameters.PortIP = this.tbIP.Text;
        //}

        private void dateTimePicker1_MouseDown(object sender, MouseEventArgs e)
        {
            dateTimePicker1_Enter(null, null);
        }

        //private void rbTcp_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.rbTcp.Checked)
        //        this.engine.Parameters.InterfaceProtocol ="Tcp" /*System.Net.Sockets.ProtocolType.Tcp*/;
        //}

        //private void rbUdp_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.rbUdp.Checked)
        //        this.engine.Parameters.InterfaceProtocol ="Udp" /*System.Net.Sockets.ProtocolType.Udp*/;
        //}

        /// <summary>
        /// запись в контроллер начальных значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (din == null || din.Count == 0) return;
            //Kontel.Relkon.DebuggerParameters.DigitalSensorDescription dsd = din[0];
            foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription dsd in din)
            {
                this.engine.AddWriteItem(dsd.Address, dsd.MemoryType, dsd.Value, dsd.Name + "writing", null, null);
            }
            foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription dsd in dout)
            {
                this.engine.AddWriteItem(dsd.Address, dsd.MemoryType, dsd.Value, dsd.Name + "writing", null, null);
            }
            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription dsd in adc)
            {
                this.engine.AddWriteItem(dsd.Address, dsd.MemoryType, dsd.Value, dsd.Name + "writing", null, null);
            }
            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription dsd in dac)
            {
                this.engine.AddWriteItem(dsd.Address, dsd.MemoryType, dsd.Value, dsd.Name + "writing", null, null);
            }
        }

        private void bSyncTimeWithPC_Click(object sender, EventArgs e)
        {
            this.lockDateTimePeaker = true;
            this.clockWrite = true;
            this.engine.AddWriteItem(0, MemoryType.Clock, Relkon4Protocol.ConvertDate(DateTime.Now), "clock_writing", null, new ProceedingCompleetedDelegate(this.ClockWrited));
            dateTimePicker1_Leave(null, null);
        }     
   }
}
