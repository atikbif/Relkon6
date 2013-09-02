using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Kontel.Relkon
{
    public sealed class Relkon4Ehernet: Relkon37Ehernet
    {
        private int bufferSize = 64; // максимальное число байт, которое можно считать с контроллера за одну операцию чтения

        public Relkon4Ehernet(string IPAddress, int PortName, ProtocolType Protocol, System.Net.Sockets.ProtocolType interfaceProtocol)
            : base(IPAddress, PortName, Protocol, interfaceProtocol)
        {

        }

        public Relkon4Ehernet(Ethernet Port)
            : base(Port.IPAddress, Port.PortName, Port.Protocol,Port.SelectProtocol)
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
        /// Возвращает максимальный размер блока данных, который можно считатать / записать в контроллер
        /// </summary>
        public override int GetPacketSize(MemoryType MemoryType)
        {
            int MaxBufferSize = 1024;
            if (MemoryType == MemoryType.SDCard)
                MaxBufferSize = 100;
            else if (MemoryType == MemoryType.FRAM || MemoryType == MemoryType.EEPROM || MemoryType == MemoryType.RAM || MemoryType == MemoryType.XRAM)
                MaxBufferSize = 97;
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
                byte[] response = this.SendRequest(Relkon4Protocol.CreateReadMemoryRequest(MemoryType, this.ControllerAddress, Address, i), i + 3);
                if (System.Net.Sockets.ProtocolType.Tcp == curentProtocol)
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
                byte[] response = this.SendRequest(Relkon4Protocol.CreateWriteMemoryRequest(MemoryType,this.ControllerAddress, Address, Utils.GetSubArray<byte>(Data, c, i)), 4);
                c += i;
               
                if (response == null && !this.canceled)
                    throw new Exception("Сбой при обращении к контроллеру");
                Address += i;
            }
            while (c != Data.Length && !this.canceled);
        }
        /// <summary>
        /// Отсылает пароль на запись данных во Flash память
        /// контроллра на базе MB90F347
        /// </summary>
        public byte[] SendWriteFlashPassword(byte[] password)
        {
            List<byte> l = new List<byte>();
            l.AddRange(new byte[] { (byte)this.ControllerAddress, 0xF2, (byte)password.Length });
            l.AddRange(password);
            byte[] response = this.SendRequest(l.ToArray(), 4 + 16);
            return (response == null) ? response : Utils.GetSubArray<byte>(response, 2);
        }
        /// <summary>
        /// Авторизация пароля на запись Flash; возвращает key3
        /// </summary>
        public byte[] AuthenticateWriteFlashPassword(byte[] key2)
        {
            List<byte> l = new List<byte>();
            l.AddRange(new byte[] { (byte)this.ControllerAddress, 0xF3 });
            l.AddRange(key2);
            byte[] response = this.SendRequest(l.ToArray(), 4 + 16);
            return (response == null) ? null : Utils.GetSubArray<byte>(response, 2);
        }
        /// <summary>
        /// Стиает сектор Flash-памяти контроллера на базе процессора
        /// MB9F347, начинающийся по указанному адресу
        /// </summary>
        public void EraseFlashSector(int address)
        {
            List<byte> l = new List<byte>();
            l.Add((byte)this.ControllerAddress);
            l.Add((byte)0xE6);
            l.AddRange(AppliedMath.IntToBytes(address));
            this.minimalTimeout = 5000;
            byte[] r = this.SendRequest(l.ToArray(), -1);
            this.minimalTimeout = 100;
            if (r == null && !this.canceled)
                throw new Exception("Сбой при удалении сектора");
        }
        /// <summary>
        /// Читает данные с SD-карточки
        /// </summary>
        /// <param name="offset">Смещение от начала карточки</param>
        /// <param name="count">Число байт</param>
        public byte[] ReadSDCard(int offset, int count)
        {
            return this.ReadFromMemory(MemoryType.SDCard, offset, count);
        }
        /// <summary>
        /// Запмсывает данные на SD-карточку
        /// </summary>
        /// <param name="offset">Смещение от начала карточки</param>
        /// <param name="data">Записываемые данные</param>
        public void WriteSDCard(int offset, byte[] data)
        {
            this.WriteToMemory(MemoryType.SDCard, offset, data);
        }        
        /// <summary>
        /// Сбрасывает контроллер на базе процессора MB90F347
        /// </summary>
        public void ResetController()
        {
            this.SendRequest(new byte[] { (byte)this.ControllerAddress, 0xFE }, 2);
        }
    }
}
