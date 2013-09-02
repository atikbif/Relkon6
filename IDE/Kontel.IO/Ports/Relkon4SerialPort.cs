using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Kontel.Relkon
{
    public sealed class Relkon4SerialPort: Relkon37SerialPort
    {
        private int bufferSize = 64; // максимальное число байт, которое можно считать с контроллера за одну операцию чтения

        public Relkon4SerialPort(string PortName, int BaudeRate, ProtocolType Protocol)
            : base(PortName, BaudeRate, Protocol)
        {

        }

        public Relkon4SerialPort(SerialPort485 Port)
            : base(Port.PortName, Port.BaudRate, Port.Protocol)
        {

        }
        /// <summary>
        /// Возвращает или устанавливает максимальное число байт, которое можно 
        /// считать с контроллера за одну операцию чтения
        /// </summary>
        public int BufferSize
        {
            get
            {
                return this.bufferSize;
            }
            set
            {
                this.bufferSize = value;
            }
        }
        /// <summary>
        /// Возвращает запрос на чтение данных с области памяти определенного типа (без CRC)
        /// </summary>
        private byte[] CreateReadMemoryRequest(MemoryType MemoryType, int Address, int Count)
        {
            List<byte> request = new List<byte>();
            request.Add((byte)this.ControllerAddress);
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
            request.Add((byte)this.ControllerAddress);
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
        /// <summary>
        /// Возвращает максимальный размер блока данных, который можно считатать / записать в контроллер
        /// </summary>
        public override int GetPacketSize(MemoryType MemoryType)
        {
            int MaxBufferSize = 1024;
            if (MemoryType == MemoryType.SDCard)
                MaxBufferSize = 100;
            else
                MaxBufferSize = 128;
            int PacketSize = Math.Min(MaxBufferSize, this.bufferSize);
            if (this.protocol == ProtocolType.RC51ASCII)
                PacketSize = PacketSize / 2 - 2;
            PacketSize -= 8;
            if (MemoryType == MemoryType.Flash)
                PacketSize = (this.protocol == ProtocolType.RC51ASCII) ? 1000 : 2000;
            return Math.Max(PacketSize, 1);
        }
        /// <summary>
        /// Читает указанное число байт с некоторой области памяти
        /// </summary>
        public override byte[] ReadFromMemory(MemoryType MemoryType, int Address, int Count)
        {
            int PacketSize = this.GetPacketSize(MemoryType);
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
                byte[] response = this.SendRequest(this.CreateReadMemoryRequest(MemoryType, Address, i), i + 3, 2);
                Thread.Sleep(10);
                c += i;
              
                if (response == null)
                    return response;
                res.AddRange(Utils.GetSubArray<byte>(response, 1, i));
                Address += i;
            }
            while (c != Count && !this.canceled);
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
        public override void WriteToMemory(MemoryType MemoryType, int Address, byte[] Data)
        {
            int PacketSize = this.GetPacketSize(MemoryType);
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
                byte[] response = this.SendRequest(this.CreateWriteMemoryRequest(MemoryType, Address, Utils.GetSubArray<byte>(Data, c, i)), 4, 2);                
                c += i;
               
                if (response == null && !this.canceled)
                    throw new Exception("Сбой при обращении к контроллеру");
                Address += i;
            }
            while (c != Data.Length && !this.canceled);
        }       
      
        /// <summary>
        /// Сбрасывает контроллер на базе процессора MB90F347
        /// </summary>
        public void ResetController()
        {
            this.SendRequest(new byte[] { (byte)this.ControllerAddress, 0xFE }, 2, 2);
        }
    }
}
