using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using Kontel.Relkon;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Globalization;

namespace Kontel.Relkon.Solutions
{
    public sealed class UploadMgr
    {
        private delegate void WorkerEventHandler(Relkon4SerialPort port);

        private ControllerProgramSolution solution = null;        
        private bool uploadOnlyParams = false; // если true, то будут загружены только параметры проекта (без программы)
        private bool uploadOnlyProgram = false;
        private bool _readEmbVars = false;
        private bool bootLoaderMode = true;
        private byte[] inputdata = new byte[0xFFFF];


        private List<AsyncOperation> userStateToLifeTime = new List<AsyncOperation>();
        private SerialPortDeviceSearcher deviceSearcher = null;
        private SendOrPostCallback onProgressReportDelegate;
        private SendOrPostCallback onCompletedDelegate;
        private bool canceled = false;
        private SerialPort485 devicePort = null;

        /// <summary>
        /// Периодически возникает в процессе загузки данных проекта в контроллер
        /// </summary>
        public event UploadMgrProgressChangedEventHandler ProgressChanged;
        /// <summary>
        /// Генерируется по завершении загрузки данных проекта в контроллер
        /// </summary>
        public event AsyncCompletedEventHandler UploadingCompleted;

        public UploadMgr(ControllerProgramSolution solution)
            : base()
        {          
            this.solution = solution;

            this.deviceSearcher = new SerialPortDeviceSearcher(null, null);
            this.deviceSearcher.ProgressChanged += new ProgressChangedEventHandler(deviceSearcher_ProgressChanged);
            this.deviceSearcher.DeviceSearchCompleted += new DeviceSearchCompletedEventHandler(deviceSearcher_DeviceSearchCompleted);

            this.onCompletedDelegate = new SendOrPostCallback(this.RaiseUploadingCompletedEvent);
            this.onProgressReportDelegate = new SendOrPostCallback(this.RaiseProgressChangeEvent);
        }

        /// <summary>
        /// Показывает, выполняется ли процесс загрузки данных
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.userStateToLifeTime.Count != 0 || this.deviceSearcher.IsBusy;
            }
        }
        /// <summary>
        /// Запускает процесс загрузки данных
        /// </summary>
        public void StartUploading()
        {
            lock (this.userStateToLifeTime)
            {
                if (this.userStateToLifeTime.Count != 0)
                    throw new Exception("Процесс загрузки уже запущен");
            }
            this.devicePort = null;
            this.canceled = false;
            //this.UpdateSeacherParams();
            this.deviceSearcher_ProgressChanged(this, new ProgressChangedEventArgs(0, null)); // чтобы при создании Message ProgressForm было непустым
            this.deviceSearcher.StartSearch();
        }
        /// <summary>
        /// Останавливает процесс загрузки данных
        /// </summary>
        public void StopUploading()
        {
            if (!this.deviceSearcher.IsBusy)
                this.canceled = true;
            if (this.devicePort != null)
                lock (this.devicePort)
                    this.devicePort.Stop();
            else
                this.deviceSearcher.StopSearch();
        }

        private void CompletionMethod(Exception exception, bool canceled)
        {
            AsyncCompletedEventArgs e = new AsyncCompletedEventArgs(exception, canceled, null);
            this.userStateToLifeTime[0].PostOperationCompleted(this.onCompletedDelegate, e);
        }

        private void RaiseUploadingCompletedEvent(object state)
        {
            if (this.UploadingCompleted != null)
                this.UploadingCompleted(this, (AsyncCompletedEventArgs)state);
        }

        private void ChangeProgressMethod(string StepName, int ProgressPercentage)
        {
            UploadMgrProgressChangedEventArgs e = new UploadMgrProgressChangedEventArgs(StepName, ProgressPercentage);
            this.userStateToLifeTime[0].Post(this.onProgressReportDelegate, e);
        }

        private void RaiseProgressChangeEvent(object state)
        {
            if (this.ProgressChanged != null)
                this.ProgressChanged(this, (UploadMgrProgressChangedEventArgs)state);
        }

        private void deviceSearcher_DeviceSearchCompleted(object sender, DeviceSearchCompletedEventArgs e)
        {            
            if (e.Cancelled || e.Error != null)
            {
                this.RaiseUploadingCompletedEvent(new AsyncCompletedEventArgs(e.Error, e.Cancelled, null));
            }
            else
            {
                if (Encoding.ASCII.GetString(e.DeviceResponse).ToLower().Contains(this.deviceSearcher.Pattern.ToLower()))
                    bootLoaderMode = false;

                Relkon4SerialPort port = new Relkon4SerialPort(e.Port);
                port.ControllerAddress = e.DeviceResponse[0];    
           
                this.devicePort = this.solution.LastWorkedPort = port;               
            
                AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(Guid.NewGuid());
                this.userStateToLifeTime.Add(asyncOp);
                WorkerEventHandler workerDelegate = new WorkerEventHandler(this.Upload);
                workerDelegate.BeginInvoke(port, null, null);
            }                                               
        }

        private void deviceSearcher_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.RaiseProgressChangeEvent(new UploadMgrProgressChangedEventArgs("Поиск контроллера...", e.ProgressPercentage));
        }

        public void StartUploading(bool onlyProgram, bool onlyParams, bool readEmbVars)
        {
            this.uploadOnlyParams = onlyParams;
            this.uploadOnlyProgram = onlyProgram;
            this._readEmbVars = readEmbVars;
            this.deviceSearcher.DevicePort = this.solution.LastWorkedPort;
            this.deviceSearcher.Pattern = "Relkon 6";
            this.deviceSearcher.BootPattern = "boot";
            this.deviceSearcher.Request = new byte[] { (byte)solution.SearchedControllerAddress, 0xA0 };
            this.StartUploading();
        }

        private void Upload(Relkon4SerialPort port)
        {
            Exception error = null;                                 
            try
            {
                int oldBaudRate = port.BaudRate;
                ProtocolType oldProtocol = port.Protocol;
                long programSize = 0;

                port.Open();

                byte[] req;

                req = port.SendRequest(new byte[] { 0x00, 0xA2 }, 16, 2);
                if (req != null)
                {
                    string s = Encoding.ASCII.GetString(req);
                    if (s.Substring(9, 4) != "PROG")
                        throw new Exception("Данный порт не предназначен для программирования!");
                }


                req = port.SendRequest(this.deviceSearcher.Request, this.deviceSearcher.Pattern.Length, 2);
                if (req != null)
                {
                    string s = Encoding.ASCII.GetString(req);
                    bootLoaderMode = s.ToLower().Contains(this.deviceSearcher.Pattern.ToLower()) ? false : true;
                }

                if (uploadOnlyProgram)
                {
                    if (!bootLoaderMode)
                    {
                        if (this.canceled)
                            throw new StopUploadinException();
                        port.Open();
                        this.ChangeProgressMethod("Сброс контроллера...", 0);
                        //Перетераем первый символ слова для загрузчика                    
                        port.WriteByteToMemory(MemoryType.FRAM, 0x7FF5, 0x00);
                        port.ResetController();
                        port.Close();
                        this.ChangeProgressMethod("Сброс контроллера...", 100);
                        if (this.canceled)
                            throw new StopUploadinException();
                        Thread.Sleep(1000);    
                    }                    

                    port.BaudRate = 115200;
                    port.Protocol = ProtocolType.RC51BIN;
                    port.Open();

                    port.DirectPort.Write("1");                  
                    waitfor(port.DirectPort, 'C');
                
                    Stream fs = new FileStream(solution.DirectoryName + "\\" + solution.Name + ".bin", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    MemoryStream ms = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(ms);
                    byte[] bytes = new byte[br.BaseStream.Length];
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        bw.Write(br.ReadBytes(1024));
                        bw.Flush();
                    }
                    fs.Close();
                    br.Close();
                    bytes = ms.ToArray();
                    programSize = bytes.Length;
                    ms.Close();
                    bw.Flush();
                    bw.Close();

                    //waitforack(port.DirectPort);
                    //waitfor(port.DirectPort, 'C');
                    ushort packetnum = 0;

                    YModemPacket initpacket = new YModemPacket();
                    initpacket.isinit = true;
                    initpacket.packetnum = packetnum;
                    initpacket.filename = solution.Name + ".bin";
                    initpacket.filelength = bytes.Length;

                    if ((solution.Name + ".bin").Length > 125)
                        initpacket.longpacket = true;
                    else
                        initpacket.longpacket = false;

                    initpacket.createPacket();

                    port.DiscardInBuffer();
                    SendPacket(port.DirectPort, initpacket);
                    waitfor(port.DirectPort, 'C');
                    //port.DiscardInBuffer();

                    MemoryStream ms2 = new MemoryStream(bytes);
                    byte[] temparr = new byte[1024];
                    YModemPacket sendPacket;
                    long numpack = Math.Abs(((long)ms2.Length) / ((long)1024));

                    BinaryReader br2 = new BinaryReader(ms2);

                    this.ChangeProgressMethod("Загрузка программы в контроллер...", 0);
                    while (ms2.Position != ms2.Length)
                    {
                        if (this.canceled)
                            throw new StopUploadinException();

                        int pers = (int)((float)100 / (float)ms2.Length * ms2.Position);
                        this.ChangeProgressMethod("Загрузка программы в контроллер...", pers);
                        Application.DoEvents();
                        //_waitforack = false;
                        packetnum++;
                        temparr = br2.ReadBytes(1024);
                        sendPacket = new YModemPacket();
                        sendPacket.packetnum = packetnum;
                        sendPacket.longpacket = true;
                        sendPacket.isinit = false;
                        sendPacket.data = temparr;
                        sendPacket.createPacket();

                        SendPacket(port.DirectPort, sendPacket);   
                    }


                    port.DirectPort.Write(new byte[] { (byte)0x04 }, 0, 1);
                    YModemPacket endpacket = new YModemPacket();
                    endpacket.isend = true;
                    endpacket.longpacket = false;
                    endpacket.packetnum = 0;
                    endpacket.data = new byte[128];
                    endpacket.createPacket();

                    SendPacket(port.DirectPort, endpacket);   

                    Thread.Sleep(2000);
                    port.DiscardInBuffer();                    
                }


               
                if (!bootLoaderMode)
                {
                    port.BaudRate = oldBaudRate;
                    port.Protocol = oldProtocol;
                }
                else
                {
                    this.ChangeProgressMethod("Поиск параметров контроллера...", 0);

                    string pattern = "relkon 6.0";
                    byte[] request = new byte[] { 0x00, 0xA0 };
                    bool searchingStopped = false;
                    int[] baudRates = new int[] { 115200, 19200, 57600, 38400, 9600, 4800 };
                    ProtocolType[] protocols = new ProtocolType[] { ProtocolType.RC51BIN, ProtocolType.RC51ASCII };
                    int totalProgress = baudRates.Length * protocols.Length;
                    for (int i = 0; i < protocols.Length && !searchingStopped; i++)
                    {
                        port.Protocol = protocols[i];
                        for (int j = 0; j < baudRates.Length && !searchingStopped; j++)
                        {
                            port.BaudRate = baudRates[j];
                            try
                            {
                                if (this.canceled)
                                    throw new StopUploadinException();

                                port.Open();
                                byte[] res = port.SendRequest(request, pattern.Length, 2);
                                port.DiscardInBuffer();
                                if (res != null && Encoding.ASCII.GetString(res).ToLower().Contains(pattern))
                                {
                                    searchingStopped = true;
                                    break;
                                }
                            }
                            catch { }
                            finally
                            {
                                port.Close();
                            }

                            this.ChangeProgressMethod("Поиск параметров контроллера...", (int)((100 / totalProgress) * (i * j + j)));
                        }
                    }
                }

                if (uploadOnlyParams)
                {

                    port.Open();

                    this.ChangeProgressMethod("Загрузка настроек...", 1);

                    if (uploadOnlyParams)
                    {
                        byte[] b = port.ReadFRAM(0x7F06, 4);
                        programSize = (long)BitConverter.ToUInt32(b, 0);
                    }

                    byte[] ParamsBuffer = this.CreateParamsBuffer((uint)programSize);

                    this.ChangeProgressMethod("Загрузка настроек...", 50);
                    port.WriteToMemory(MemoryType.FRAM, 0x7F00, ParamsBuffer);
                    this.ChangeProgressMethod("Загрузка настроек...", 100);
                    this.ChangeProgressMethod("Загрузка заводских уставок...", 0);

                    int adr = 0x7B00;
                    for (int i = 0; i < 8; i++)
                    {
                        byte[] buf = new byte[128];
                        for (int j = 0; j < 128; j++)
                        {
                            if (this.canceled)
                                throw new StopUploadinException();

                            buf[j] = (byte)this.solution.Vars.GetEmbeddedVar("EE" + (i*128 + j).ToString()).Value;
                        }
                        port.WriteToMemory(MemoryType.FRAM, adr, buf);
                        adr += 128;
                        this.ChangeProgressMethod("Загрузка заводских уставок...", (int)(i * ((double)100 / (double)8)));
                    }

                    //this.ChangeProgressMethod("Загрузка настроек...", 75);
                    //port.WriteToMemory(MemoryType.FRAM, 0x7B00, EmbeddedVarsBuffer);

                    this.ChangeProgressMethod("Загрузка настроек...", 100);
                    this.ChangeProgressMethod("Сброс контроллера...", -1);
                }

                if (_readEmbVars && !bootLoaderMode)
                {
                    this.ChangeProgressMethod("Чтение заводских уставок...", 0);
                    int adr = 0x7B00;

                    for (int i = 0; i < 8; i++)
                    {
                        byte[] buf = port.ReadFromMemory(MemoryType.FRAM, adr, 128);

                        for (int j = 0; j < 128; j++)
                            this.solution.Vars.GetEmbeddedVar("EE" + (i * 128 + j).ToString()).Value = buf[j];

                        adr += 128;

                        this.ChangeProgressMethod("Чтение заводских уставок...", (int)(i * ((double)100 / (double)8)));
                    }
                                       
                    
                }

                try
                {
                    port.ResetController();
                }
                catch
                {
                    Utils.WarningMessage("Системе не удалось перезагрузить контроллер. Чтобы настройки вступили в силу, пересбросьте питание вручную.");
                }

            }
            catch (Exception ex)
            {
                if (!(ex is StopUploadinException))
                    error = ex;
                //throw ex;
            }
            finally
            {
                port.Close();
            }

            lock (this.userStateToLifeTime)
            {
                this.CompletionMethod(error, this.canceled);
                this.userStateToLifeTime.Clear();
            }                       
        }
       

        private void SendPacket(SerialPort sp, YModemPacket pack)
        {              
            for (int i = 0; i < 3; i++)
            {
                sp.Write(pack.packet, 0, pack.packet.Length);
                for (int j = 0; j < 10; j++)
                {
                    int b = sp.ReadByte();                   
                    if (b == 0x06)
                        return;
                    Thread.Sleep(50);
                }                
            }

            throw new Exception("Не удаётся отправить пакет с данными.\nСвязь с контроллером прервана!");
        }        

        private void waitfor(SerialPort sp, char p)
        {
            int c;
            do
            {
                for (int i = 0; i < 800; i++)
                {
                    if (sp.BytesToRead > 0)
                        break;
                    Thread.Sleep(5);
                }
                if (sp.BytesToRead == 0)
                    throw new Exception("Обмен данными с контроллером не возможен!");

                c = sp.ReadChar();
            }
            while (c != (int)p);           
        }       
        
         /// <summary>
        /// Создает буфер настроек для записи в контроллер
        /// </summary>
        /// <returns></returns>
        private byte[] CreateParamsBuffer(uint programSize)
        {
            List<byte> res = new List<byte>();
            res.Add((byte)this.solution.ControllerAddress);
            int speed = 5;
            switch (this.solution.Uarts[0].BaudRate)
            {
                case 4800:
                    speed = 0;
                    break;
                case 9600:
                    speed = 1;
                    break;
                case 19200:
                    speed = 2;
                    break;
                case 38400:
                    speed = 3;
                    break;
                case 57600:
                    speed = 4;
                    break;
                case 115200:
                   speed = 5;
                    break;                   
            }
            
            res.Add((byte)speed);
            res.Add((byte)this.solution.Uarts[0].Protocol);

            switch (this.solution.Uarts[1].BaudRate)
            {
                case 4800:
                    speed = 0;
                    break;
                case 9600:
                    speed = 1;
                    break;
                case 19200:
                    speed = 2;
                    break;
                case 38400:
                    speed = 3;
                    break;
                case 57600:
                    speed = 4;
                    break;
                case 115200:
                   speed = 5;
                    break;                   
            }
            
            res.Add((byte)speed);
            res.Add((byte)this.solution.Uarts[1].Protocol);

            int emu = 0;
            if (this.solution.CompilationParams.EmulationMode2)
                emu = 1;
            if (this.solution.CompilationParams.EmulationMode)
                emu = 2;

            res.Add((byte)emu);

            res.AddRange(RelkonProtocol.AddCRC(BitConverter.GetBytes(programSize)));         //размер проекта
            if (this.solution.Label.Length > 64)
                this.solution.Label = this.solution.Label.Substring(0, 64);
            byte[] buf = new byte[64];
            byte[] label = Encoding.Unicode.GetBytes(this.solution.Label);
            Array.Copy(label, buf, label.Length); 
            res.AddRange(buf);   //Метка
            res.AddRange(this.solution.ControllerIPAdress);
            //byte[] mac = string.
            
            long mac = Int64.Parse(this.solution.ControllerMACAdress, NumberStyles.AllowHexSpecifier);
            byte[] macBytes = BitConverter.GetBytes(mac);
            Array.Resize<byte>(ref macBytes, 6);
            Array.Reverse(macBytes);
            res.AddRange(macBytes);


            res.Add((byte)(DateTime.Now.Year - 2000)); //0x7F56 – год (от 0 до 99)

            //0x7F57 – разрешение коммуникационного канала вместо пульта
            //(0x31 – канал, любое другое значение - пульт)
            res.Add(this.solution.PultEnable ? (byte)0 : (byte)0x31);

            res.Add((byte)0); //Зарезервированн под радио канал
            //(0x31 – вкл, любое другое значение - выкл)
            res.Add(this.solution.SDEnable ? (byte)0x31 : (byte)0);

            return res.ToArray();
        }

        /// <summary>
        /// Создает буфер настроек переменных заводских установок для записи в контроллер
        /// </summary>
        private byte[] CreateEmbeddedVarsBuffer()
        {
            List<byte> res = new List<byte>();
            for (int i = 0; i < 1024; i++)
                res.Add((byte)this.solution.Vars.GetEmbeddedVar("EE" + i).Value);         
            return res.ToArray();
        }      
    }

    /// <summary>
    /// Описывает событие UploadMgr.LoadToDeviceProgressChanged
    /// </summary>
    public delegate void UploadMgrProgressChangedEventHandler(object sender, UploadMgrProgressChangedEventArgs e);

    /// <summary>
    /// Аргумент события UploadMgr.ProgressChanged
    /// </summary>
    public class UploadMgrProgressChangedEventArgs : ProgressChangedEventArgs
    {
        private string stepName = ""; // имя текущего шага загрузки

        public UploadMgrProgressChangedEventArgs(string StepName, int ProgressPercentage)
            : base(ProgressPercentage, null)
        {
            this.stepName = StepName;
        }
        /// <summary>
        /// Возвращает имя текущего шага загрузки
        /// </summary>
        public string StepName
        {
            get
            {
                return this.stepName;
            }
        }
    }
    /// <summary>
    /// Исключение, возникающее, которое будет генерироваться тогда, когда надо будет остановить процесс загрузки
    /// </summary>
    public class StopUploadinException : Exception
    {

    }
}
