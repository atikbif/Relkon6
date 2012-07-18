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
    /// Перечиление доступных типов памяти контроллеров
    /// </summary>
    public enum MemoryType
    {
        RAM,
        XRAM,
        FRAM,
        EEPROM,
        Clock,
        Flash,
        SDCard
    }
    /// <summary>
    /// Перечилсение типов протоколов, по которым может работать класс SerialPort485
    /// </summary>
    public enum ProtocolType
    {
        RC51ASCII = 1,
        RC51BIN = 2,
        ATCommand = 3,
        None = 4
    }
    /// <summary>
    /// Перечисление типов UART'ов микроконтроллеров Fujitsu
    /// </summary>
    public enum PortType
    {
        RS232,
        RS485
    }
    /// <summary>
    /// Перечисление ошибок связи, поддерживаемых классом Ehernet
    /// </summary>
    public enum ErrorType
    {
        DeviceNotAnswer = 1,
        DeviceReturnError = 2,
        InvalidCRC = 3,
    }
    public static class RelkonProtocol
    {
        /// <summary>
        /// Вычисляет контрольную сумму указанного буфера
        /// </summary>
        public static int GetCRC(byte[] buffer)
        {
            int Res = 0xFFFF;
            for (int j = 0; j < buffer.Length; j++)
            {
                Res ^= buffer[j];

                for (int i = 8; i > 0; i--)
                {
                    if ((Res & 1) != 0)
                        Res = (Res >> 1) ^ 0xA001;
                    else
                        Res >>= 1;
                }
            }
            return Res;
        }
        /// <summary>
        /// Добавляет в конец указанного буфера его CRC и 
        /// возвращает новый массив
        /// </summary>
        public static byte[] AddCRC(byte[] buffer)
        {
            byte[] res = new byte[buffer.Length + 2];
            buffer.CopyTo(res, 0);
            int crc = RelkonProtocol.GetCRC(buffer);
            res[res.Length - 1] = AppliedMath.Hi(crc);
            res[res.Length - 2] = AppliedMath.Low(crc);
            return res;
        }
        /// <summary>
        /// Преобразует указанный массив к формату протокола
        /// RC51ASCII (CRC не добавляется)
        /// </summary>
        public static byte[] ConvertToRC51ASCII(byte[] buffer)
        {
            byte[] res = new byte[buffer.Length * 2 + 2];
            res[0] = 0x24;
            for (int i = 0; i < buffer.Length; i++)
            {
                string h = Convert.ToString(buffer[i], 16).ToUpper();
                if (h.Length == 1)
                    h = "0" + h;
                byte[] code = Encoding.ASCII.GetBytes(h);
                res[(i + 1) * 2 - 1] = code[0];
                res[(i + 1) * 2] = code[1];
            }
            res[res.Length - 1] = 0x0D;
            return res;
        }
        /// <summary>
        /// Преобразует указанный массив из формата RC51ASCII в бинарный формат
        /// (CRC не удаляется)
        /// </summary>
        public static byte[] ConvertFromRC51ASCII(byte[] buffer)
        {
            byte[] tmp = Utils.GetSubArray(buffer, 1, buffer.Length - 2);
            byte[] res = new byte[tmp.Length / 2];
            for (int i = 0; i < tmp.Length; i += 2)
            {
                byte[] code = { tmp[i], tmp[i + 1] };
                string h = Encoding.ASCII.GetString(code);
                res[i / 2] = (byte)AppliedMath.HexToDec(h);
            }
            return res;
        }
        /// <summary>
        /// Преобразует указанный массив к формату текущего протокола
        /// </summary>
        public static byte[] ConvertToCurrentProtocolType(byte[] buffer, ProtocolType protocol)
        {
            byte[] res = buffer;
            switch (protocol)
            {
                case ProtocolType.RC51ASCII:
                    res = RelkonProtocol.ConvertToRC51ASCII(RelkonProtocol.AddCRC(buffer));
                    break;
                case ProtocolType.RC51BIN:
                    res = RelkonProtocol.AddCRC(buffer);
                    break;
                case ProtocolType.ATCommand:
                    res = new byte[buffer.Length + 1];
                    Array.Copy(buffer, res, buffer.Length);
                    res[res.Length - 1] = 0x0D;
                    break;
            }
            return res;
        }
        /// <summary>
        /// Преобразует указанный массив из формату текущего протокола
        /// </summary>
        public static byte[] ConvertFromCurrentProtocolType(byte[] buffer, ProtocolType protocol, ErrorType errorType)
        {
            byte[] res = buffer;
            switch (protocol)
            {
                case ProtocolType.RC51ASCII:
                case ProtocolType.RC51BIN:
                    // Проверяем буфер на наличие ошибок
                    if ((protocol == ProtocolType.RC51BIN && buffer.Length == 4 && buffer[1] == 0xFF) || buffer.Length < 3 ||
                        (protocol == ProtocolType.RC51ASCII && (buffer[buffer.Length - 1] != 0x0d || buffer[0] != '!')))
                    {
                        errorType = ErrorType.DeviceReturnError;
                        res = null;
                    }
                    if (res != null)
                    {
                        if (protocol == ProtocolType.RC51ASCII)
                            res = RelkonProtocol.ConvertFromRC51ASCII(buffer);
                        // Проверяем crc (если crc верна, то crc от буффера с crc = 0) 
                        if (RelkonProtocol.GetCRC(res) != 0)
                        {
                            errorType = ErrorType.InvalidCRC;
                            res = null;
                        }
                        else
                            res = Utils.GetSubArray<byte>(res, 0, res.Length - 2);
                    }
                    break;
                case ProtocolType.ATCommand:
                    res = Utils.GetSubArray<byte>(res, 0, res.Length - 2);
                    break;
            }
            return res;
        }
    }

    public static class Relkon37Protocol
    {
        /// <summary>
        /// Возвращает запрос (без CRC) на чтение указанного числа байт из заданной области памяти
        /// </summary>
        public static byte[] CreateReadMemoryRequest(MemoryType MemoryType, int controllerAddress, int Address, Type type)
        {
            List<byte> l = new List<byte>();
            l.Add((byte)controllerAddress);
            byte command = 0;
            int AddressLength = 0;
            switch (MemoryType)
            {
                case MemoryType.RAM:
                    command = (byte)0x50;
                    AddressLength = 1;
                    break;
                case MemoryType.Clock:
                    command = (byte)0x51;
                    AddressLength = 1;
                    break;
                case MemoryType.EEPROM:
                    command = (byte)0x52;
                    AddressLength = 2;
                    break;
                case MemoryType.FRAM:
                    command = (byte)0x53;
                    AddressLength = 2;
                    break;
                case MemoryType.XRAM:
                    command = (byte)0x54;
                    AddressLength = 2;
                    break;
                case MemoryType.Flash:
                    command = (byte)0x55;
                    AddressLength = 2;
                    break;
                default:
                    throw new Exception("Указанный тип памяти " + MemoryType + " не поддерживается в " + type);
            }
            l.Add(command);
            byte[] a = AppliedMath.IntToBytes(Address);
            l.AddRange(Utils.GetSubArray<byte>(a, a.Length - AddressLength));
            return l.ToArray();
        }
        /// <summary>
        /// Возвращает запрос на запись данных в память определенного типа (без CRC)
        /// </summary>
        public static byte[] CreateWriteMemoryRequest(MemoryType MemoryType, int controllerAddress, int Address, byte[] Data, Type type)
        {
            List<byte> l = new List<byte>();
            l.Add((byte)controllerAddress);
            byte command = 0;
            int AddressLength = 0;
            switch (MemoryType)
            {
                case MemoryType.RAM:
                    command = (byte)0x60;
                    AddressLength = 1;
                    break;
                case MemoryType.Clock:
                    command = (byte)0x61;
                    AddressLength = 1;
                    break;
                case MemoryType.EEPROM:
                    command = (byte)0x62;
                    AddressLength = 2;
                    break;
                case MemoryType.FRAM:
                    command = (byte)0x63;
                    AddressLength = 2;
                    break;
                case MemoryType.XRAM:
                    command = (byte)0x64;
                    AddressLength = 2;
                    break;
                case MemoryType.Flash:
                    command = (byte)0x65;
                    AddressLength = 2;
                    break;
                default:
                    throw new Exception("Указанный тип памяти " + MemoryType + " не поддерживается в " + type);
            }
            l.Add(command);
            byte[] a = AppliedMath.IntToBytes(Address);
            l.AddRange(Utils.GetSubArray<byte>(a, a.Length - AddressLength));
            l.AddRange(Data);
            return l.ToArray();
        }      
     
    }
    public static class Relkon4Protocol
    {
        /// <summary>
        /// Возвращает запрос на чтение данных с области памяти определенного типа (без CRC)
        /// </summary>
        public static byte[] CreateReadMemoryRequest(MemoryType MemoryType, int controllerAddress, int Address, int Count)
        {
            List<byte> request = new List<byte>();
            request.Add((byte)controllerAddress);
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
        public static byte[] CreateWriteMemoryRequest(MemoryType MemoryType, int controllerAddress, int Address, byte[] Data)
        {
            List<byte> request = new List<byte>();
            request.Add((byte)controllerAddress);
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
        /// Преобразует дату-время из массива байт в формате контроллера в тип DateTime;
        /// в случае ошибки преобразования возвращает null
        /// </summary>
        public static DateTime? ConvertDate(byte[] Data)
        {
            try
            {
                byte Second = (byte)(((((int)Data[0] >> 4) * 10) + ((int)Data[0] & 0x0F)) & 0x7F);
                byte Minute = (byte)((((int)Data[1] >> 4) * 10) + ((int)Data[1] & 0x0F));
                byte Hour = (byte)((((int)Data[2] >> 4) * 10) + ((int)Data[2] & 0x0F));
                byte Day = (byte)(((((int)Data[4] >> 4) * 10) + ((int)Data[4] & 0x0F)) & 0x7F);
                byte Month = (byte)((((int)Data[5] >> 4) * 10) + ((int)Data[5] & 0x0F));
                byte Year = (byte)((((int)Data[6] >> 4) * 10) + ((int)Data[6] & 0x0F));

                Second &= 0x7F;
                Day &= 0x7F;
                DateTime res = DateTime.MinValue;
                res = res.AddSeconds(Second);
                res = res.AddMinutes(Minute);
                res = res.AddHours(Hour);
                res = res.AddDays(Day - 1);
                res = res.AddMonths(Month - 1);
                res = res.AddYears(2000 + Year - 1);
                return res;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Преобразует дату-время из типа DateTime в массив байт в формате контроллера
        /// </summary>
        public static Byte[] ConvertDate(DateTime dateTime)
        {
            List<Byte> m_Return = new List<Byte>();

            m_Return.Add((Byte)(((dateTime.Second / 10) << 4) + dateTime.Second % 10));
            m_Return.Add((Byte)(((dateTime.Minute / 10) << 4) + dateTime.Minute % 10));
            m_Return.Add((Byte)(((dateTime.Hour / 10) << 4) + dateTime.Hour % 10));
            m_Return.Add(0x00);
            m_Return.Add((Byte)(((dateTime.Day / 10) << 4) + dateTime.Day % 10));
            m_Return.Add((Byte)(((dateTime.Month / 10) << 4) + dateTime.Month % 10));
            m_Return.Add((Byte)((((dateTime.Year - 2000) / 10) << 4) + (dateTime.Year - 2000) % 10));

            return m_Return.ToArray();
        }    
    }

}
