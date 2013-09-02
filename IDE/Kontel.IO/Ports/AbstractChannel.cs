using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace Kontel.Relkon
{
    public abstract class AbstractChannel
    {
        protected ProtocolType _relkonProtocolType = ProtocolType.None; // протокол, по которому работает канал
        private ErrorType _errorType; // хранит тип последней зафиксированной ошибки
        protected int _minimalTimeout = 100; // минимальный интервал (мс.) ожидания ответа от устройства
        protected bool _canceled = false; // флаг, показывающий, что нужно остановить прцесс чтения / записи
        private int _bufferSize = 128; // максимальное число байт, которое можно считать с контроллера за одну операцию чтения
        private int _controllerAddress = 1; // сетевой адрес опрашиваемого контроллера

        /// <summary>
        /// Возвращает или устанавливает протокол, по которому работает порт
        /// </summary>
        public ProtocolType RelkonProtocolType
        {
            get
            {
                return _relkonProtocolType;
            }
            set
            {
                _relkonProtocolType = value;
            }
        }

        /// <summary>
        /// Возвращает или устанавливает минимальный интервал (мс.) ожидания 
        /// ответ устройства
        /// </summary>
        internal int MinimalTimeout
        {
            get
            {
                return _minimalTimeout;
            }
            set
            {
                _minimalTimeout = value;
            }
        }

        ///// <summary>
        ///// Возвращает состояние последней ошибки
        ///// </summary>
        //public ErrorType LastErrorType
        //{
        //    get
        //    {
        //        return _errorType;
        //    }
        //}

        /// <summary>
        /// Возвращает или устанавливает сетевой адрес
        /// опрашиваемого контроллера
        /// </summary>
        public int ControllerAddress
        {
            get
            {
                return _controllerAddress;
            }
            set
            {
                _controllerAddress = value;
            }
        }

        /// <summary>
        /// Возвращает или устанавливает максимальное число байт, которое можно 
        /// считать с контроллера за одну операцию чтения
        /// </summary>
        public int BufferSize
        {
            get
            {
                return _bufferSize;
            }
            set
            {
                _bufferSize = value;
            }
        }


        protected abstract byte[] Send(byte[] buffer, int ExpectedResponseSize);
        public abstract void Open();
        public abstract void Close();
        public abstract bool IsOpen();
      
        /// <summary>
        /// Проверяет, является ли указанный буфер сообщением текущего протокола
        /// </summary>
        /// <param name="ExpectedResponseSize">Ожидаемый размер сообщения протокола</param>
        protected bool ResponseRecieved(byte[] buffer, int ExpectedResponseSize)
        {
            bool res = false;
            switch (_relkonProtocolType)
            {
                case ProtocolType.RC51BIN:
                    res = (RelkonProtocol.GetCRC(buffer) == 0 || (ExpectedResponseSize != -1 && buffer.Length >= ExpectedResponseSize * 100));
                    break;
                case ProtocolType.RC51ASCII:
                    res = (buffer[buffer.Length - 1] == 0x0D || (ExpectedResponseSize != -1 && buffer.Length >= ExpectedResponseSize * 100));
                    break;
            }
            return res;
        }

        /// <summary>
        /// Останавливает процесс чтения / записи, происходящий в данный момент
        /// </summary>
        public void Stop()
        {
            _canceled = true;
        }
       

        public int GetPacketSize(MemoryType MemoryType)
        {
            int MaxBufferSize = 1024;
            if (MemoryType == MemoryType.SDCard)
                MaxBufferSize = 100;
            else if (MemoryType == MemoryType.FRAM || MemoryType == MemoryType.EEPROM || MemoryType == MemoryType.RAM || MemoryType == MemoryType.XRAM)
                MaxBufferSize = 97;
            else
                MaxBufferSize = 128;
            int PacketSize = Math.Min(MaxBufferSize, _bufferSize);
            if (_relkonProtocolType == ProtocolType.RC51ASCII)
                PacketSize = PacketSize / 2 - 2;
            PacketSize -= 8;
            if (MemoryType == MemoryType.Flash)
                PacketSize = (this._relkonProtocolType == ProtocolType.RC51ASCII) ? 1000 : 2000;
            return Math.Max(PacketSize, 1); 
        }

        /// <summary>
        /// Преобразует указанный массив к формату текущего протокола
        /// </summary>
        private byte[] ConvertToCurrentProtocolType(byte[] buffer)
        {
            return RelkonProtocol.ConvertToCurrentProtocolType(buffer, _relkonProtocolType);
        }
       

        /// <summary>
        /// Преобразует указанный массив из формата RC51ASCII в бинарный формат
        /// (CRC не удаляется)
        /// </summary>
        private byte[] ConvertFromRC51ASCII(byte[] buffer)
        {
            if (buffer[0] != '!')
            {
                this._errorType = ErrorType.DeviceReturnError;
                return null;
            }
            return RelkonProtocol.ConvertFromRC51ASCII(buffer);
        }
        

        /// <summary>
        /// Возвращает запрос на чтение данных с области памяти определенного типа (без CRC)
        /// </summary>
        private byte[] CreateReadMemoryRequest(MemoryType MemoryType, int Address, int Count)
        {
            List<byte> request = new List<byte>();
            request.Add((byte)_controllerAddress);
            byte command = 0;
            int AddressLength = 0;
            switch (MemoryType)
            {
                case MemoryType.RAM:
                    command = (byte)0xD0;
                    AddressLength = 1;
                    break;
                case MemoryType.Clock:
                    command = (byte)0xD1;
                    AddressLength = 1;
                    break;
                case MemoryType.EEPROM:
                    command = (byte)0xD2;
                    AddressLength = 2;
                    break;
                case MemoryType.FRAM:
                    command = (byte)0xD3;
                    AddressLength = 2;
                    break;
                case MemoryType.XRAM:
                    command = (byte)0xD4;
                    AddressLength = 2;
                    break;
                case MemoryType.Flash:
                    command = (byte)0xD5;
                    AddressLength = 4;
                    if (Count % 2 != 0)
                        Count++; // в Relkon4 из Flash можно читать только четное число байт
                    break;
                case MemoryType.SDCard:
                    command = (byte)0xD7;
                    AddressLength = 4;
                    break;
            }
            // Добавление команды
            request.Add(command);
            // Добавление адреса
            byte[] a = AppliedMath.IntToBytes(Address);
            request.AddRange(Utils.GetSubArray<byte>(a, a.Length - AddressLength));
            // Добавление числа байт
            a = AppliedMath.IntToBytes(Count);
            if (MemoryType == MemoryType.Flash || MemoryType == MemoryType.SDCard)
                AddressLength = 2;
            request.AddRange(Utils.GetSubArray<byte>(a, a.Length - AddressLength));
            return request.ToArray();
        }
        /// <summary>
        /// Возвращает запрос на запись данных в память определенного типа (без CRC)
        /// </summary>
        private byte[] CreateWriteMemoryRequest(MemoryType MemoryType, int Address, byte[] Data)
        {
            List<byte> request = new List<byte>();
            request.Add((byte) _controllerAddress);
            byte command = 0;
            int AddressLength = 0;
            switch (MemoryType)
            {
                case MemoryType.RAM:
                    command = (byte)0xE0;
                    AddressLength = 1;
                    break;
                case MemoryType.Clock:
                    command = (byte)0xE1;
                    AddressLength = 1;
                    break;
                case MemoryType.EEPROM:
                    command = (byte)0xE2;
                    AddressLength = 2;
                    break;
                case MemoryType.FRAM:
                    command = (byte)0xE3;
                    AddressLength = 2;
                    break;
                case MemoryType.XRAM:
                    command = (byte)0xE4;
                    AddressLength = 2;
                    break;
                case MemoryType.Flash:
                    command = (byte)0xE5;
                    AddressLength = 4;
                    if (Data.Length % 2 != 0)
                    {
                        // В Relkon4 во Flash можно записывать только четное число байт
                        Array.Resize<byte>(ref Data, Data.Length + 1);
                        Data[Data.Length - 1] = 0xFF;
                    }
                    break;
                case MemoryType.SDCard:
                    command = (byte)0xE7;
                    AddressLength = 4;
                    break;
            }
            // Добавление команды
            request.Add(command);
            // Добавление адреса
            byte[] a = AppliedMath.IntToBytes(Address);
            request.AddRange(Utils.GetSubArray<byte>(a, a.Length - AddressLength));
            // Добавление размера буфера
            a = AppliedMath.IntToBytes(Data.Length);
            if (MemoryType == MemoryType.Flash || MemoryType == MemoryType.SDCard)
                AddressLength = 2;
            request.AddRange(Utils.GetSubArray<byte>(a, a.Length - AddressLength));
            // Добавление данных
            request.AddRange(Data);
            return request.ToArray();
        }
     
        /// <param name="ExpectedResponseSize">Ожидаемый размер ответа (без CRC)</param>
        /// <param name="DelayFactor">Коэффицент задержки для достижения запроса контроллера</param>
        public byte[] SendRequest(byte[] request, int ExpectedResponseSize)
        {
            if (_relkonProtocolType == ProtocolType.RC51ASCII && ExpectedResponseSize != -1)
                ExpectedResponseSize = ExpectedResponseSize * 2 + 2;
            byte[] response = null;
            for (int i = 0; i < 3; i++)
            {
                response = this.Send(this.ConvertToCurrentProtocolType(request), ExpectedResponseSize);
                if (response != null)
                    break;
            }
            if (response == null)
                _errorType = ErrorType.DeviceNotAnswer;
            else
                response = RelkonProtocol.ConvertFromCurrentProtocolType(response, _relkonProtocolType, _errorType);
            return response;
        }


        /// <summary>
        /// Читает указанное число байт с некоторой области памяти
        /// </summary>
        public byte[] ReadFromMemory(MemoryType MemoryType, int Address, int Count)
        {
            int PacketSize = _bufferSize - 16;
            int c = 0;
            List<byte> res = new List<byte>(Count);
            do
            {
                int i = Math.Min(Count - c, PacketSize);
                if (MemoryType == MemoryType.SDCard)
                {
                    // c SD Card можно читать в пределах 512-байтных секторов
                    int segmentEnd = Address / 512 * 512 + 512;
                    if (Address + i >= segmentEnd)
                        i = segmentEnd - Address;
                }
                byte[] response = this.SendRequest(this.CreateReadMemoryRequest(MemoryType, Address, i), i + 3);
                Thread.Sleep(10);
                c += i;

                if (response == null)
                    return response;
                res.AddRange(Utils.GetSubArray<byte>(response, 1, i));
                Address += i;
            }
            while (c != Count && !_canceled);
            return res.ToArray();
        }

        /// <summary>
        /// Записывает указанный байт в некоторую область памяти
        /// </summary>
        public void WriteByteToMemory(MemoryType MemoryType, int Address, byte Data)
        {
            WriteToMemory(MemoryType, Address, new byte[] { Data });
        }

        /// <summary>
        /// Записывает указанный массив байт в некоторую область памяти
        /// </summary>
        public void WriteToMemory(MemoryType MemoryType, int Address, byte[] Data)
        {
            int PacketSize = _bufferSize - 16;
            int c = 0;
            do
            {
                int i = Math.Min(Data.Length - c, PacketSize);
                if (MemoryType == MemoryType.SDCard)
                {
                    // на SD Card можно писать в пределах 512-байтных секторов
                    int segmentEnd = Address / 512 * 512 + 512;
                    if (Address + i >= segmentEnd)
                        i = segmentEnd - Address;
                }
                byte[] response = this.SendRequest(this.CreateWriteMemoryRequest(MemoryType, Address, Utils.GetSubArray<byte>(Data, c, i)), 4);
                c += i;

                if (response == null && !_canceled)
                    throw new Exception("Сбой при обращении к контроллеру");
                Address += i;
            }
            while (c != Data.Length && !_canceled);
        }

        /// <summary>
        /// Читает версию Relkon контроллера
        /// </summary>
        public string ReadRelkonVersion()
        {
            byte[] res = this.SendRequest(new byte[] { (byte)_controllerAddress, 0x20 }, 10);
            return (res == null) ? null : Encoding.ASCII.GetString(Utils.GetSubArray<byte>(res, 2));
        }
        /// <summary>
        /// Читает тип контроллера
        /// </summary>
        public string ReadControllerType()
        {
            byte[] res = this.SendRequest(new byte[] { (byte)_controllerAddress, 0xA1 }, 4);
            return (res == null) ? null : Encoding.ASCII.GetString(Utils.GetSubArray<byte>(res, 2));
        }    

        /// <summary>
        /// Сбрасывает контроллер на базе процессора MB90F347
        /// </summary>
        public void ResetController()
        {
            this.SendRequest(new byte[] { (byte)_controllerAddress, 0xFE }, 2);
        }

        /// <summary>
        /// Читает указанное число байт из RAM по указанному адресу
        /// </summary>
        public byte[] ReadRAM(int address, int count)
        {
            return this.ReadFromMemory(MemoryType.RAM, address, count);
        }
        /// <summary>
        /// Читает указанное число байт из памяти часов по указанному адресу
        /// </summary>
        public byte[] ReadClock(int address, int count)
        {
            return this.ReadFromMemory(MemoryType.Clock, address, count);
        }
        /// <summary>
        /// Читает указанное число байт из EEPROM по указанному адресу
        /// </summary>
        public byte[] ReadEEPROM(int address, int count)
        {
            return this.ReadFromMemory(MemoryType.EEPROM, address, count);
        }
        /// <summary>
        /// Читает указанное число байт из FRAM по указанному адресу
        /// </summary>
        public byte[] ReadFRAM(int address, int count)
        {
            return this.ReadFromMemory(MemoryType.FRAM, address, count);
        }
        /// <summary>
        /// Читает указанное число байт из XRAM по указанному адресу
        /// </summary>
        public byte[] ReadXRAM(int address, int count)
        {
            return this.ReadFromMemory(MemoryType.XRAM, address, count);
        }
        /// <summary>
        /// Читает указанное число байт из Flash по указанному адресу
        /// </summary>
        public byte[] ReadFlash(int address, int count)
        {
            return this.ReadFromMemory(MemoryType.Flash, address, count);
        }
        /// <summary>
        /// Записывает указанный массив байт в RAM по указанному адресу
        /// </summary>
        public void WriteRAM(int address, byte[] data)
        {
            this.WriteToMemory(MemoryType.RAM, address, data);
        }
        /// <summary>
        /// Записывает указанный массив байт в память часов по указанному адресу
        /// </summary>
        public void WriteClock(int address, byte[] data)
        {
            this.WriteToMemory(MemoryType.Clock, address, data);
        }
        /// <summary>
        /// Записывает указанный массив байт в EEPROM по указанному адресу
        /// </summary>
        public void WriteEEPROM(int address, byte[] data)
        {
            this.WriteToMemory(MemoryType.EEPROM, address, data);
        }
        /// <summary>
        /// Записывает указанный массив байт в FRAM по указанному адресу
        /// </summary>
        public void WriteFRAM(int address, byte[] data)
        {
            this.WriteToMemory(MemoryType.FRAM, address, data);
        }
        /// <summary>
        /// Записывает указанный массив байт в XRAM по указанному адресу
        /// </summary>
        public void WriteXRAM(int address, byte[] data)
        {
            this.WriteToMemory(MemoryType.XRAM, address, data);
        }
        /// <summary>
        /// Записывает указанный массив байт в Flash по указанному адресу
        /// </summary>
        public void WriteFlash(int address, byte[] data)
        {
            this.WriteToMemory(MemoryType.Flash, address, data);
        }    
     
    }
}
