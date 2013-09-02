using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace Kontel.Relkon
{
    /// <summary>
    /// Предоставляет средства для работы с COM-портом по
    /// протоколу RS-485
    /// </summary>
    public class Ethernet
    {
        protected Socket controllerSocket;//обеспечивает работу по Ethernet
        protected IPEndPoint ipeController;//ip контроллера
        protected ProtocolType protocol = ProtocolType.None; // протокол, по которому работает порт
        protected ErrorType errorType; // хранит тип последней зафиксированной ошибки
        protected int minimalTimeout = 10000; // минимальный интервал (мс.) ожидания ответа от устройства
        protected bool canceled = false; // флаг, показывающий, что нужно остановить прцесс чтения / записи
        protected System.Net.Sockets.ProtocolType curentProtocol = System.Net.Sockets.ProtocolType.Tcp;//протокол, по которому происходит соединение
        //Для протокола UDP
        //protected int udpTimeout = 10; // интервал ожидания ответа от устройства 
        //protected Timer udpTimer = new Timer(timer1_Tick);
        protected UdpClient listener;
        protected IPEndPoint groupEP;

        
        public Ethernet(string IPAddress, int PortName, ProtocolType Protocol, System.Net.Sockets.ProtocolType interfaceProtocol)
        {
            IPAddress IP = new IPAddress(GetLongIP(IPAddress));
            ipeController = new IPEndPoint(IP, PortName);
            this.protocol = Protocol;
            SelectProtocol = interfaceProtocol;
        }
        /// <summary>
        /// Возвращает значение IP в виде long, по его строковому представлениею
        /// </summary>
        /// <param name="stringIP"></param>
        /// <returns></returns>
        static private long GetLongIP(string stringIP)
        {
            long lIP = 0;
            string[] sIP = stringIP.Split('.');
            int k = 1;
            for (int i = 0; i < 4; i++)
            {
                lIP = lIP + long.Parse(sIP[i]) * k;
                k = k * 256;
            }
            return lIP;
        }
        /// <summary>
        /// Возвращает значение IP в виде строки, по его строковому представлениею
        /// </summary>
        /// <param name="stringIP"></param>
        /// <returns></returns>
        static private string GetStringIP(byte[] byteIP)
        {
            string sIP = "";
            for (int i = 0; i < 4; i++)
            {
                sIP = sIP + byteIP[i].ToString();
            }
            return sIP;
        }
        /// <summary>
        /// Возвращает или устанавливает протокол, по которому работает порт
        /// </summary>
        public ProtocolType Protocol
        {
            get
            {
                return this.protocol;
            }
            set
            {
                this.protocol = value;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает номер рабочего порта
        /// </summary>
        public int PortName
        {
            get
            {
                return this.ipeController.Port;
            }
            set
            {
                //новые параметры вступают в силу только после переоткрытия порта
                this.ipeController.Port = value;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает ip адресс
        /// </summary>
        public string IPAddress
        {
            get
            {
                return GetStringIP(this.ipeController.Address.GetAddressBytes());
            }
            set
            {
                //новые параметры вступают в силу только после переоткрытия порта
                this.ipeController.Address = new IPAddress(GetLongIP(value));
            }
        }
        /// <summary>
        /// При открытии порта использется это свойство как протокол соединения
        /// </summary>
        public System.Net.Sockets.ProtocolType SelectProtocol { get; set; }
        /// <summary>
        /// Возвращает или устанавливает минимальный интервал (мс.) ожидания 
        /// ответ устройства
        /// </summary>
        public int MinimalTimeout
        {
            get
            {
                return this.minimalTimeout;
            }
            set
            {
                this.minimalTimeout = value;
            }
        }
        /// <summary>
        /// Возвращает состояние последней ошибки
        /// </summary>
        public ErrorType LastErrorType
        {
            get
            {
                return this.errorType;
            }
        }
       
      
        /// <summary>
        /// Преобразует указанный массив из формата RC51ASCII в бинарный формат
        /// (CRC не удаляется)
        /// </summary>
        public byte[] ConvertFromRC51ASCII(byte[] buffer)
        {
            if (buffer[0] != '!')
            {
                this.errorType = ErrorType.DeviceReturnError;
                return null;
            }
           
            return RelkonProtocol.ConvertFromRC51ASCII(buffer);
        }
        /// <summary>
        /// Преобразует указанный массив к формату текущего протокола
        /// </summary>
        protected byte[] ConvertToCurrentProtocolType(byte[] buffer)
        {       
            return RelkonProtocol.ConvertToCurrentProtocolType(buffer, this.protocol);
        }
     
        /// <summary>
        /// Отсылает буфер на порт и читает с него ответ
        /// </summary>
        /// <param name="ExpectedResponseSize">Ожидаемый размер ответа</param>
        private byte[] Send(byte[] buffer, int ExpectedResponseSize)
        {
            this.canceled = false;
            //Установка соединения для протокола Tcp
            if (System.Net.Sockets.ProtocolType.Tcp == curentProtocol)
            {
                if (this.controllerSocket == null || !this.controllerSocket.Connected)
                    this.Open();//Установка соединения, если оно не установленно

                if (this.controllerSocket == null || !this.controllerSocket.Connected)
                {
                    this.errorType = ErrorType.DeviceNotAnswer;//ошибка, если соединение не установленно
                    return null;
                }
            }
            int bytes = 0;//число байт в ответе
            byte[] bytesReceived = new Byte[255];//полученный массив
            byte[] returnBuffer = new Byte[255];//возвращаемый массив
            try
            {
                if (!this.canceled)
                {
                    switch (curentProtocol)
                    {
                        case System.Net.Sockets.ProtocolType.Tcp:
                            this.controllerSocket.Send(buffer, buffer.Length, 0);
                            bytes = this.controllerSocket.Receive(bytesReceived, bytesReceived.Length, 0);
                            break;
                        case System.Net.Sockets.ProtocolType.Udp:
                            this.controllerSocket.SendTo(buffer, ipeController);
                            Console.WriteLine("---" + DateTime.Now.Millisecond.ToString());
                            int c = 5;
                            while (c > 0)
                            {
                                Thread.Sleep(5);//ожидание в течении допустимого времени
                                if (listener.Available > 0)
                                {
                                    bytesReceived = listener.Receive(ref groupEP);
                                    bytes = bytesReceived.Length;
                                    break;
                                }
                                c--;
                            }
                            Console.WriteLine("---" + DateTime.Now.Millisecond.ToString());
                            break;
                        default:
                            break;
                    }
                    returnBuffer = new byte[bytes];
                    Array.Copy(bytesReceived, returnBuffer, bytes);
                    return returnBuffer;
                }
                else
                    return null;
            }
            catch
            {
                Console.WriteLine("---" + DateTime.Now.Millisecond.ToString());
                return null;
            }
        }
        /// <summary>
        /// опрос ответа от контроллера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //                    Console.WriteLine("---" + DateTime.Now.Millisecond.ToString());
        //    if
        //                    bytesReceived = listener.Receive(ref groupEP);
        //                    listener.Available//возвращает наличие бданных в буфере
        //                    Console.WriteLine("---" + DateTime.Now.Millisecond.ToString());
        //}
        /// <summary>
        /// Проверяет, является ли указанный буфер сообщением текущего протокола
        /// </summary>
        /// <param name="ExpectedResponseSize">Ожидаемый размер сообщения протокола</param>
        private bool ResponseRecieved(byte[] buffer, int ExpectedResponseSize)
        {
            bool res = false;
            switch (this.protocol)
            {
                case ProtocolType.RC51BIN:
                    res = (RelkonProtocol.GetCRC(buffer) == 0 || (ExpectedResponseSize != -1 && buffer.Length >= ExpectedResponseSize * 100));
                    break;
                case ProtocolType.RC51ASCII:
                    res = (buffer[buffer.Length - 1] == 0x0D || (ExpectedResponseSize != -1 && buffer.Length >= ExpectedResponseSize * 100));
                    break;
                case ProtocolType.ATCommand:
                    string s = Encoding.ASCII.GetString(buffer);
                    res = ((buffer[buffer.Length - 1] == 0x0A && Regex.Matches(s, "\r\n").Count >= 2) || (ExpectedResponseSize != -1 && buffer.Length >= ExpectedResponseSize * 100));
                    break;
                case ProtocolType.None:
                    res = (buffer.Length > ExpectedResponseSize);
                    break;
            }
            return res;
        }
        /// <summary>
        /// Останавливает процесс чтения / записи, происходящий в данный момент
        /// </summary>
        public void Stop()
        {
            this.canceled = true;
        }
        /// <summary>
        /// Преобразует буфер к текущему типу протокола (для RC51ASCII и RC51BIN добавляет CRC) и 
        /// отправляет на COM-порт; возвращает ответ устройства, преобразованный из формата
        /// текущего протокола в массив байт (для RC51ASCII и RC51BIN удаляет CRC)
        /// </summary>
        /// <param name="ExpectedResponseSize">Ожидаемый размер ответа (без CRC)</param>
        public byte[] SendRequest(byte[] request, int ExpectedResponseSize)
        {
            if (this.protocol == ProtocolType.RC51ASCII && ExpectedResponseSize != -1)
                ExpectedResponseSize = ExpectedResponseSize * 2 + 2;
            byte[] response = this.Send(this.ConvertToCurrentProtocolType(request), ExpectedResponseSize);
            if (response == null)
                this.errorType = ErrorType.DeviceNotAnswer;
            else
                response = RelkonProtocol.ConvertFromCurrentProtocolType(response,this.protocol,this.errorType);
            return response;
        }
        /// <summary>
        /// Открывает порт
        /// </summary>
        public void Open()
        {
            this.curentProtocol = this.SelectProtocol;
            if (this.curentProtocol == System.Net.Sockets.ProtocolType.Tcp && (this.controllerSocket == null || !this.controllerSocket.Connected))
            {
                try { this.controllerSocket.Disconnect(false); }
                catch { }
                controllerSocket = new Socket(ipeController.AddressFamily, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                this.controllerSocket.SendTimeout = this.minimalTimeout;
                this.controllerSocket.ReceiveTimeout = this.minimalTimeout;
                this.controllerSocket.Connect(this.ipeController);
            }
            else
            {
                controllerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
                this.controllerSocket.SendTimeout = this.minimalTimeout;
                this.controllerSocket.ReceiveTimeout = this.minimalTimeout;
                //this.controllerSocket.SendTimeout = 5;
                //this.controllerSocket.ReceiveTimeout = 5;
                //this.controllerSocket.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.ChecksumCoverage,void)=true;
                listener = new UdpClient(this.PortName);
                groupEP = new IPEndPoint(System.Net.IPAddress.Any, this.PortName);
                //listener.MulticastLoopback = false;
                //listener.Client.ReceiveTimeout = this.minimalTimeout;
                //listener.Client.ReceiveTimeout = this.minimalTimeout;
                listener.Client.ReceiveTimeout = 5;
                listener.Client.SendTimeout = 5;
                //listener.ExclusiveAddressUse = true;
                //listener.Client.UseOnlyOverlappedIO=true;
                //listener.Client.Ttl=5;
                //listener.Ttl = 5;
            }
        }
        /// <summary>
        /// Закрывает порт
        /// </summary>
        public void Close()
        {
            if (this.curentProtocol == System.Net.Sockets.ProtocolType.Tcp && this.controllerSocket.Connected)
                this.controllerSocket.Disconnect(false);
            if (this.curentProtocol == System.Net.Sockets.ProtocolType.Udp)
                try { listener.Close(); }
                catch { }
        }        
    }
}
