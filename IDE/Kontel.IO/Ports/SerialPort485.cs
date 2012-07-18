using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Kontel.Relkon
{
    /// <summary>
    /// Предоставляет средства для работы с COM-портом по
    /// протоколу RS-485
    /// </summary>
    public class SerialPort485
    {
        ///// <summary>
        ///// Перечисление ошибок связи, поддерживаемых классом SerialPort485
        ///// </summary>
        //public enum ErrorType
        //{
        //    DeviceNotAnswer = 1,
        //    DeviceReturnError = 2,
        //    InvalidCRC = 3,
        //}
        protected SerialPort port = null; // обеспечивает рабоу с COM-портом
        protected ProtocolType protocol = ProtocolType.None; // протокол, по которому работает порт
        protected ErrorType errorType; // хранит тип последней зафиксированной ошибки
        protected int minimalTimeout = 100; // минимальный интервал (мс.) ожидания ответа от устройства
        protected bool canceled = false; // флаг, показывающий, что нужно остановить прцесс чтения / записи

        /// <summary>
        /// Периодически генерируется после чтения/записи определенного количества байт,
        /// доля прочитанных/записанных байт от всего их числа передается в EventArgs
        /// </summary>
        public event EventHandler<EventArgs<double>> ProgressChanged;

        public SerialPort485(string PortName, int BaudRate, ProtocolType Protocol)
        {
            this.port = new SerialPort();
            this.port.PortName = PortName;
            this.port.BaudRate = BaudRate;
            this.port.DtrEnable = true;
            this.port.RtsEnable = true;
            this.protocol = Protocol;
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
        /// Возвращает или устанавливает имя рабочего порта
        /// </summary>
        public string PortName
        {
            get
            {
                return this.port.PortName;
            }
            set
            {
                this.port.PortName = value;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает скорость обмена данными с портом
        /// </summary>
        public int BaudRate
        {
            get
            {
                return this.port.BaudRate;
            }
            set
            {
                this.port.BaudRate = value;
            }
        }
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

        public SerialPort DirectPort
        {
            get
            {
                return this.port;
            }
        }
        /// <summary>
        /// Генерирует событие изменения процесса
        /// </summary>
        protected void RaiseProgressChangedEvent(double value)
        {
            if (this.ProgressChanged != null)
                this.ProgressChanged(this, new EventArgs<double>(value));
        }
        ///// <summary>
        ///// Вычисляет контрольную сумму указанного буфера
        ///// </summary>
        //public static int GetCRC(byte[] buffer)
        //{
        //    int Res = 0xFFFF;
        //    for (int j = 0; j < buffer.Length; j++)
        //    {
        //        Res ^= buffer[j];

        //        for (int i = 8; i > 0; i--)
        //        {
        //            if ((Res & 1) != 0)
        //                Res = (Res >> 1) ^ 0xA001;
        //            else
        //                Res >>= 1;
        //        }
        //    }
        //    return Res;
        //}
        ///// <summary>
        ///// Добавляет в конец указанного буфера его CRC и 
        ///// возвращает новый массив
        ///// </summary>
        //public static byte[] AddCRC(byte[] buffer)
        //{
        //    byte[] res = new byte[buffer.Length + 2];
        //    buffer.CopyTo(res, 0);
        //    int crc = RelkonProtocol.GetCRC(buffer);
        //    res[res.Length - 1] = AppliedMath.Hi(crc);
        //    res[res.Length - 2] = AppliedMath.Low(crc);
        //    return res;
        //}
        ///// <summary>
        ///// Преобразует указанный массив к формату протокола
        ///// RC51ASCII (CRC не добавляется)
        ///// </summary>
        //public byte[] ConvertToRC51ASCII(byte[] buffer)
        //{
        //    byte[] res = new byte[buffer.Length * 2 + 2];
        //    res[0] = 0x24;
        //    for (int i = 0; i < buffer.Length; i++)
        //    {
        //        string h = Convert.ToString(buffer[i], 16).ToUpper();
        //        if (h.Length == 1)
        //            h = "0" + h;
        //        byte[] code = Encoding.ASCII.GetBytes(h);
        //        res[(i + 1) * 2 - 1] = code[0];
        //        res[(i + 1) * 2] = code[1];
        //    }
        //    res[res.Length - 1] = 0x0D;
        //    return res;
        //}
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
        //    byte[] tmp = Utils.GetSubArray(buffer, 1, buffer.Length - 2);
        //    byte[] res = new byte[tmp.Length / 2];
        //    for (int i = 0; i < tmp.Length; i += 2)
        //    {
        //        byte[] code = { tmp[i], tmp[i + 1] };
        //        string h = Encoding.ASCII.GetString(code);
        //        res[i / 2] = (byte)AppliedMath.HexToDec(h);
        //    }
            return RelkonProtocol.ConvertFromRC51ASCII(buffer);
        }
        /// <summary>
        /// Преобразует указанный массив к формату текущего протокола
        /// </summary>
        protected byte[] ConvertToCurrentProtocolType(byte[] buffer)
        {
            //byte[] res = buffer;
            //switch (this.protocol)
            //{
            //    case ProtocolType.RC51ASCII:
            //        res = RelkonProtocol.ConvertToRC51ASCII(RelkonProtocol.AddCRC(buffer));
            //        break;
            //    case ProtocolType.RC51BIN:
            //        res = RelkonProtocol.AddCRC(buffer);
            //        break;
            //    case ProtocolType.ATCommand:
            //        res = new byte[buffer.Length + 1];
            //        Array.Copy(buffer, res, buffer.Length);
            //        res[res.Length - 1] = 0x0D;
            //        break;
            //}
            return RelkonProtocol.ConvertToCurrentProtocolType(buffer, this.protocol);
        }
        ///// <summary>
        ///// Преобразует указанный массив к формату текущего протокола
        ///// </summary>
        //protected byte[] ConvertFromCurrentProtocolType(byte[] buffer)
        //{
        //    byte[] res = buffer;
        //    switch (this.protocol)
        //    {
        //        case ProtocolType.RC51ASCII:
        //        case ProtocolType.RC51BIN:
        //            // Проверяем буфер на наличие ошибок
        //            if ((this.protocol == ProtocolType.RC51BIN && buffer.Length == 4 && buffer[1] == 0xFF) || buffer.Length < 3 || 
        //                (this.protocol == ProtocolType.RC51ASCII && (buffer[buffer.Length - 1] != 0x0d|| buffer[0] != '!')))
        //            {
        //                res = null;
        //                this.errorType = ErrorType.DeviceReturnError;
        //            }
        //            if (res != null)
        //            {
        //                if (this.protocol == ProtocolType.RC51ASCII)
        //                    res = this.ConvertFromRC51ASCII(buffer);
        //                // Проверяем crc (если crc верна, то crc от буффера с crc = 0) 
        //                if (RelkonProtocol.GetCRC(res) != 0)
        //                {
        //                    res = null;
        //                    this.errorType = ErrorType.InvalidCRC;
        //                }
        //                else
        //                    res = Utils.GetSubArray<byte>(res, 0, res.Length - 2);
        //            }
        //            break;
        //        case ProtocolType.ATCommand:
        //            res = Utils.GetSubArray<byte>(res, 0, res.Length - 2);
        //            break;
        //    }
        //    return res;
        //}
        /// <summary>
        /// Отсылает буфер на порт и читает с него ответ
        /// </summary>
        /// <param name="ExpectedResponseSize">Ожидаемый размер ответа</param>
        /// <param name="DelayFactor">Коэффицент задержки для достижения запроса контроллера</param>
        private byte[] Send(byte[] buffer, int ExpectedResponseSize, int DelayFactor)
        {
            this.canceled = false;
            this.port.Write(buffer, 0, buffer.Length);
            int t = (int)Math.Round(1.5 * buffer.Length * 8 * 1000 / this.port.BaudRate); // время необходимое для передачи массива при текущей скорости порта
            int timeout = this.minimalTimeout;
            if (this.BaudRate < 9600)
                timeout = 500;
            if (ExpectedResponseSize != -1)
                timeout = Math.Max(timeout, (int)Math.Round(1.5 * ExpectedResponseSize * 8 * 1000 / this.port.BaudRate + 1));           
            Thread.Sleep(t * DelayFactor); // задержка на время, достаточное для достижения запросом контроллера умноженная на десять, что бы наверняка контроллер успел послать ответ
            bool finish = false;
            List<byte> res = new List<byte>();
            int DelayInterval = 1; // Интервал задержки между попытками чтения
            int idx = 0; // счетчик попыток чтения
            while (!finish && !this.canceled)
            {
                int c = this.port.BytesToRead;
                if (c == 0)
                {
                    if ((++idx) * DelayInterval < timeout)
                        Thread.Sleep(DelayInterval);
                    else
                        finish = true;
                }
                else
                {
                    byte[] tmp = new byte[c];
                    this.port.Read(tmp, 0, c);
                    res.AddRange(tmp);
                    if (this.ResponseRecieved(res.ToArray(), ExpectedResponseSize))
                    {
                        if (this.protocol == ProtocolType.RC51BIN && (res[res.Count - 2] == 0 || res[res.Count - 1] == 0))
                        {
                            Thread.Sleep(10);
                            if (this.port.BytesToRead != 0)
                            {
                                tmp = new byte[this.port.BytesToRead];
                                this.port.Read(tmp, 0, tmp.Length);
                                res.AddRange(tmp);
                                if (this.ResponseRecieved(res.ToArray(), ExpectedResponseSize))
                                    finish = true;
                            }
                            else
                                finish = true;
                        }
                        else
                            finish = true;
                    }
                    idx = 0;
                }
            }
            this.port.DiscardInBuffer();
            return (res.Count == 0) ? null : res.ToArray();
        }
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
        /// <param name="DelayFactor">Коэффицент задержки для достижения запроса контроллера</param>
        public byte[] SendRequest(byte[] request, int ExpectedResponseSize, int DelayFactor)
        {
            if (this.protocol == ProtocolType.RC51ASCII && ExpectedResponseSize != -1)
                ExpectedResponseSize = ExpectedResponseSize * 2 + 2;
            byte[] response = null;
            for (int i = 0; i < 3; i++)
            {
                response = this.Send(this.ConvertToCurrentProtocolType(request), ExpectedResponseSize, DelayFactor);
                if (response != null)
                    break;
            }
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
            if (!this.port.IsOpen)
                this.port.Open();
        }
        /// <summary>
        /// Закрывает порт
        /// </summary>
        public void Close()
        {
            if (this.port.IsOpen)
                this.port.Close();
        }
        /// <summary>
        /// Выполняет дозвон до указанного номера через присоедененный к порту модем
        /// </summary>
        /// <param name="PhoneNumber">Телефонный номер удаленного модема</param>
        public void DialUp(string PhoneNumber)
        {
            int i = this.minimalTimeout;
            this.SendRequest(Encoding.ASCII.GetBytes("ate0"), 11, 2);
            this.minimalTimeout = 90000;
            byte[] response = this.SendRequest(Encoding.ASCII.GetBytes("atd" + PhoneNumber), 5, 2);
            if (response == null || !Encoding.ASCII.GetString(response).ToLower().Contains("connect"))
                throw new Exception("Не удалось установить соединение с модемом: " + ((response != null) ? Encoding.ASCII.GetString(response) : ""));
            this.minimalTimeout = i;
        }
        /// <summary>
        /// Разрывает связь с модемом
        /// </summary>
        public void BreakModemConnection()
        {
            ProtocolType p = this.protocol;
            this.protocol = ProtocolType.None;
            this.minimalTimeout = 2000;
            Thread.Sleep(1500);
            byte[] response = this.SendRequest(Encoding.ASCII.GetBytes("+++"), 2, 2);
            if (response == null || Encoding.ASCII.GetString(response).ToLower().Contains("error"))
            {
                this.protocol = p;
                return;
            }
            Thread.Sleep(1500);
            this.SendRequest(Encoding.ASCII.GetBytes("ath\r"), 2, 2);
            this.protocol = p;
        }
        /// <summary>
        /// Очищает входной буфер порта
        /// </summary>
        public void DiscardInBuffer()
        {
            this.port.DiscardInBuffer();
        }       
    }
}
