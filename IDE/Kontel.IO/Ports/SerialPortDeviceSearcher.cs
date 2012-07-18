using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.IO.Ports;
using System.Collections;

namespace Kontel.Relkon
{
    /// <summary>
    /// Делегат события DeviceSearchCompleeted класса SerialPortDeviceSearcher
    /// </summary>
    public delegate void DeviceSearchCompletedEventHandler(object sender, DeviceSearchCompletedEventArgs e);
    
    /// <summary>
    /// Осуществляет поиск некоторогого устройства, подключенного к какому-либо
    /// последовательному порту компьютера
    /// </su,sdfk iyehjr&mmary>
    public sealed class SerialPortDeviceSearcher
    {
        private delegate void WorkerEventHandler(string PortName, AsyncOperation asyncOp);
        private SendOrPostCallback onProgressReportDelegate;
        private SendOrPostCallback onCompletedDelegate;
        private SortedList<string, AsyncOperation> requestingPorts = new SortedList<string, AsyncOperation>(); // список опрашиваемых в текущий момент портов
        private double progress = 0; // доля завершения поиска
        private bool canceled = false; // флаг, показывающий, что поиск был отменен пользователем
        private SerialPort485 devicePort = null; // порт, к которому подключено найденое устройтво
        private byte[] deviceResponse = null; // ответ устройства
        private int portsCount = 0;

        /// <summary>
        /// Генерируется во время поиска, позволяет узнать процент завершения
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;
        /// <summary>
        /// Генерируетсмя по завершении поиска 
        /// </summary>
        public event DeviceSearchCompletedEventHandler DeviceSearchCompleted;
        /// <summary>
        /// Возвращает или устанавливает массив протоколов, по которым может работать устройство
        /// </summary>
        public ProtocolType[] Protocols { get; set; }
        /// <summary>
        /// Возвращает или устанавливает массив скоростей, на которых может работать устройство
        /// </summary>
        public int[] BaudRates { get; set; }
        /// <summary>
        /// Возвращает или устанавливает запрос, на который должно ответить искомое устройство
        /// </summary>
        public byte[] Request { get; set; }
        /// <summary>
        /// Возвращает или устанавливает строку, позволяющую идентифицировать устройство
        /// </summary>
        public string Pattern { get; set; }
        public string BootPattern { get; set; }
        /// <summary>
        /// Время (мс) ожидания ответа устройства
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// Возвращает или устанавливает имя COM-порта, по которому будет осуществляться поиск устройства;
        /// если значение свойства равно null или пустой  строке, то поиск будет осуществляться по всем
        /// портам, обнаруженным в системе
        /// </summary>
        public string SearchedPortName { get; set; }
        /// <summary>
        /// Показывает, осуществляется ли в данный момент поиск
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.requestingPorts.Count != 0;
            }
        }

        public SerialPort485 DevicePort
        {
            set { devicePort = value; }
            get { return devicePort; }
        }

        /// <summary>
        /// Показывет, что требуется остановить процесс поиска
        /// </summary>
        private bool SearchingStopped
        {
            get
            {
                return this.canceled || this.devicePort != null;
            }
        }

        /// <summary>
        /// Создает новый экземпляр класса SerialPortDeviceSearcher; поиск осуществляется по протоколам 
        /// RC51BIN, RC51ASCII и скоростям 19200, 115200, 57600, 38400, 9600, 4800, 2400, 1200 бод
        /// </summary>
        /// <param name="Request">Запрос, на который должно ответить искомое устройство</param>
        /// <param name="Pattern">Строка, позволяющая идентифицировать устройство</param>
        public SerialPortDeviceSearcher(byte[] Request, string Pattern)
        {
            this.BaudRates = new int[] { 115200, 19200, 57600, 38400, 9600, 4800 };
            this.Protocols = new ProtocolType[] { ProtocolType.RC51BIN, ProtocolType.RC51ASCII };
            this.Timeout = 100;
            this.Request = Request;
            this.Pattern = Pattern;
            this.Initialize();
        }      

        public SerialPortDeviceSearcher(byte[] Request, string Pattern, string PortName, int[] BaudRates, ProtocolType[] Protocols, int Timeout)
        {
            this.BaudRates = BaudRates;
            this.Protocols = Protocols;
            this.Timeout = Timeout;
            this.Request = Request;
            this.Pattern = Pattern;
            this.SearchedPortName = PortName;
            this.Initialize();
        }

        private void Initialize()
        {
            this.onProgressReportDelegate = new SendOrPostCallback(this.ReportProgress);
            this.onCompletedDelegate = new SendOrPostCallback(this.SearchCompleted);
        }

        private void SearchCompleted(object operationState)
        {
            DeviceSearchCompletedEventArgs e = operationState as DeviceSearchCompletedEventArgs;
            if (this.DeviceSearchCompleted != null)
                this.DeviceSearchCompleted(this, e);
        }

        private void ReportProgress(object state)
        {
            ProgressChangedEventArgs e = state as ProgressChangedEventArgs;
            if (this.ProgressChanged != null)
                this.ProgressChanged(this, e);
        }

        private void CompletionMethod(SerialPort485 port, byte[] response, Exception exception, bool canceled,  AsyncOperation asyncOp)
        {
            DeviceSearchCompletedEventArgs e = new DeviceSearchCompletedEventArgs(port, response, exception, canceled);
            asyncOp.PostOperationCompleted(onCompletedDelegate, e);
        }

        private void ChangeProgressMethod(AsyncOperation asyncOp)
        {
            lock (this.requestingPorts)
            {
                this.progress += 1.0 / (this.portsCount * this.Protocols.Length * this.BaudRates.Length);
            }
            asyncOp.Post(this.onProgressReportDelegate, new ProgressChangedEventArgs((int)Math.Round(this.progress * 100), null));
        }

        private void SearchDevice(string PortName, AsyncOperation asyncOp)
        {           
            SerialPort485 port = new SerialPort485(PortName, 19200, ProtocolType.None);
            port.MinimalTimeout = this.Timeout;

            for (int i = 0; i < this.Protocols.Length && !this.SearchingStopped; i++)
            {
                port.Protocol = this.Protocols[i];            
                for (int j = 0; j < this.BaudRates.Length && !this.SearchingStopped; j++)
                {
                    port.BaudRate = this.BaudRates[j];
                    try
                    {
                        
                        port.Open();
                        byte[] res = port.SendRequest(this.Request, this.Pattern.Length, 2);
                        port.DiscardInBuffer();
                       
                        if (res != null)
                        {
                            string s = Encoding.ASCII.GetString(res);

                            if (s.ToLower().Contains(this.Pattern.ToLower()) || s.ToLower().Contains(this.BootPattern.ToLower()))
                            {
                                // Устройство обнаружено
                                this.devicePort = port;
                                this.deviceResponse = res;
                            }
                            
                        }
                    }
                    catch { }
                    finally
                    {
                        port.Close();
                    }
                    this.ChangeProgressMethod(asyncOp);
                }
            }

            lock (this.requestingPorts)
            {
                this.requestingPorts.Remove(PortName);
                if (this.requestingPorts.Count == 0)
                {
                    if (this.devicePort != null)
                        this.CompletionMethod(this.devicePort, this.deviceResponse, null, false, asyncOp);
                    else
                        this.CompletionMethod(null, null, (this.canceled ? null : new Exception("Устройство не найдено")), this.canceled, asyncOp);
                }
            }
        }
        /// <summary>
        /// Запускает поиск устройства
        /// </summary>
        public void StartSearch()
        {
            lock (this.requestingPorts)
            {
                if (this.requestingPorts.Count != 0)
                    throw new Exception("Поиск устройства уже запущен, необходимо дождаться завершения");
            }

            this.deviceResponse = null;

            if (devicePort != null)
            {
                byte[] res = { };
                try
                {
                    devicePort.Open();
                    res = devicePort.SendRequest(this.Request, this.Pattern.Length, 2);
                    if (res != null && (Encoding.ASCII.GetString(res).ToLower().Contains(this.Pattern.ToLower())
                                    || Encoding.ASCII.GetString(res).ToLower().Contains(this.BootPattern.ToLower())))
                    {
                        this.deviceResponse = res;
                    }                                       
                }
                catch { }
                finally
                {
                    devicePort.Close();                                                  
                }
            }

            if (deviceResponse != null)
            {
                DeviceSearchCompletedEventArgs e = new DeviceSearchCompletedEventArgs(this.devicePort, this.deviceResponse, null, canceled);
                if (this.DeviceSearchCompleted != null)
                    this.DeviceSearchCompleted(this, e);
            }
            else
            {
                this.devicePort = null;
                this.canceled = false;
                this.progress = 0;
                this.portsCount = 0;
                //if (this.SearchedPortName == null || this.SearchedPortName == "")
                //{
                    string[] portNames = SerialPort.GetPortNames();
                    this.portsCount = portNames.Length;
                    foreach (string portName in portNames)
                    {
                        if (!this.requestingPorts.Keys.Contains(portName))
                        {
                            WorkerEventHandler workerDelegate = new WorkerEventHandler(this.SearchDevice);
                            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(portName);
                            this.requestingPorts.Add(portName, asyncOp);
                            workerDelegate.BeginInvoke(portName, asyncOp, null, null);
                        }
                    }
            }
            //}
            //else
            //{
            //    this.portsCount = 1;
            //    WorkerEventHandler workerDelegate = new WorkerEventHandler(this.SearchDevice);
            //    AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(this.SearchedPortName);
            //    this.requestingPorts.Add(this.SearchedPortName, asyncOp);
            //    workerDelegate.BeginInvoke(this.SearchedPortName, asyncOp, null, null);
            //}            
        }
        /// <summary>
        /// Останавливает поиск устройства
        /// </summary>
        public void StopSearch()
        {
            this.canceled = true;
        }
    }

    /// <summary>
    /// Аргумент события DeviceSearchCompletedEventHandler
    /// </summary>
    public class DeviceSearchCompletedEventArgs : AsyncCompletedEventArgs
    {
        public byte[] DeviceResponse { get; private set; } // ответ устройства на запрос (при успешном завершении поиска)
        public SerialPort485 Port { get; private set; } // описывает порт, к которому подключено устройство

        /// <summary>
        /// Создает новый экземпляр класса DeviceSearchCompleetedEventArgs (рекомендуется
        /// использовать при успешном завершении поиска)
        /// </summary>
        /// <param name="Port">Порт, к которому подключено устройство</param>
        /// <param name="DeviceResponse">Ответ устройства</param>
        public DeviceSearchCompletedEventArgs(SerialPort485 Port, byte[] DeviceResponse)
            : base(null, false, null)
        {
            this.Port = Port;
            this.DeviceResponse = DeviceResponse;
        }
        /// <summary>
        /// Создает новый экземпляр класса DeviceSearchCompleetedEventArgs (рекомендуется
        /// использовать при неудачном завершении поиска)
        /// </summary>
        /// <param name="e">Исключение, возникшее в процессе поиска</param>
        /// <param name="canceled">Флаг, показывающий, что поиск был остановлен пользователем</param>
        /// <param name="state">User state (см. AsyncCompletedEventArgs)</param>
        public DeviceSearchCompletedEventArgs(Exception e, bool canceled)
            : base(e, canceled, null)
        {
            
        }

        public DeviceSearchCompletedEventArgs(SerialPort485 Port, byte[] DeviceResponse, Exception e, bool canceled)
            : base(e, canceled, null)
        {
            this.Port = Port;
            this.DeviceResponse = DeviceResponse;
        }
    }

}
