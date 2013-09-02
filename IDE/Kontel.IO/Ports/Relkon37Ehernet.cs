using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon
{
    /// <summary>
    /// Предоставляет средства для связи через COM-порт с контроллерами, 
    /// работающими под управлением Relkon 3.7
    /// </summary>
    public class Relkon37Ehernet: Ethernet
    {
        private int controllerAddress = 1; // сетевой адрес опрашиваемого контроллера

        public Relkon37Ehernet(string IPAddress, int PortName, ProtocolType Protocol, System.Net.Sockets.ProtocolType interfaceProtocol)
            : base(IPAddress,PortName,Protocol, interfaceProtocol)
        {

        }

        public Relkon37Ehernet(Ethernet Port)
            : base(Port.IPAddress, Port.PortName, Port.Protocol, Port.SelectProtocol)
        {

        }

        /// <summary>
        /// Возвращает или устанавливает сетевой адрес
        /// опрашиваемого контроллера
        /// </summary>
        public int ControllerAddress
        {
            get
            {
                return this.controllerAddress;
            }
            set
            {
                this.controllerAddress = value;
            }
        }
        /// <summary>
        /// Читает версию Relkon контроллера
        /// </summary>
        public string ReadRelkonVersion()
        {
            byte[] res = this.SendRequest(new byte[] { (byte)this.controllerAddress, 0x20 }, 10);
            return (res == null) ? null : Encoding.ASCII.GetString(Utils.GetSubArray<byte>(res, 2));
        }
        /// <summary>
        /// Читает тип контроллера
        /// </summary>
        public string ReadControllerType()
        {
            byte[] res = this.SendRequest(new byte[] { (byte)this.controllerAddress, 0x21 }, 4);
            return (res == null) ? null : Encoding.ASCII.GetString(Utils.GetSubArray<byte>(res, 2));
        }
       
        /// <summary>
        /// Возвращает максимальный размер блока данных, который можно считатать / записать в контроллер
        /// </summary>
        public virtual int GetPacketSize(MemoryType MemotyType)
        {
            return 8;
        }
        /// <summary>
        /// Читает указанное число байт с некоторой области памяти
        /// </summary>
        public virtual byte[] ReadFromMemory(MemoryType MemoryType, int Address, int Count)
        {
            int c = 0;
            this.canceled = false;
            List<byte> res = new List<byte>(Count);
            do
            {
                int i = 8;
                byte[] response = this.SendRequest(Relkon37Protocol.CreateReadMemoryRequest(MemoryType, this.controllerAddress, Address, this.GetType()), i + 3);
                c += i;
             
                if (response == null)
                    return response;
                res.AddRange(Utils.GetSubArray<byte>(response, 1, i));
                Address += i;
            }
            while (c < Count && !this.canceled);
            res.RemoveRange(Count, res.Count - Count);
            return res.ToArray();
        }
        /// <summary>
        /// Записывает указанный массив байт в некоторую область памяти
        /// </summary>
        public virtual void WriteToMemory(MemoryType MemoryType, int Address, byte[] Data)
        {
            int c = 0;
            do
            {
                int i = 8;
                byte[] response = this.SendRequest(Relkon37Protocol.CreateWriteMemoryRequest(MemoryType, this.controllerAddress, Address, Utils.GetSubArray<byte>(Data, c, i), this.GetType()), 4);
                c += i;
               
                if (response == null && !this.canceled)
                    throw new Exception("Сбой при обращении к контроллеру");
                Address += i;
            }
            while (c < Data.Length && !this.canceled);
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
