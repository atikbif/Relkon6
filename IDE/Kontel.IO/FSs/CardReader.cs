using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Kontel.Relkon
{
    /// <summary>
    /// Предоставляет средства для чтения данных с
    /// кард-ридера
    /// </summary>
    public sealed class CardReader
    {
        #region API fuctions defenitions

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode, IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile);
        //Constants for errors:
        private const UInt32 ERROR_FILE_NOT_FOUND = 2;
        private const UInt32 ERROR_INVALID_NAME = 123;
        private const UInt32 ERROR_ACCESS_DENIED = 5;
        private const UInt32 ERROR_IO_PENDING = 997;
        ////////////////////////////////////////////////
        private const Int32 INVALID_HANDLE_VALUE = -1;
        ////////////////////////////////////////////////
        private const Int32 FILE_READ_DATA = 0x01;
        private const Int32 FILE_WITE_DATA = 0x02;
        private const UInt32 GENERIC_READ = 0x80000000;
        private const UInt32 GENERIC_WRITE = 0x40000000;
        private const Int32 FILE_SHARE_READ = 0x01;
        private const Int32 FILE_SHARE_WRITE = 0x02;
        private const UInt32 FILE_ATTRIBUTE_NORMAL = 0x80;
        private const UInt32 OPEN_EXISTING = 0x03;
              
        [DllImport("kernel32.dll")]
        private static extern Boolean CloseHandle(IntPtr hObject);
        
        /// Reading and writing.
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Boolean WriteFile(IntPtr fFile, Byte[] lpBuffer, UInt32 nNumberOfBytesToWrite, out UInt32 lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Boolean ReadFile(IntPtr hFile, [Out] Byte[] lpBuffer, UInt32 nNumberOfBytesToRead, out UInt32 nNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        private static extern UInt32 GetDriveType(string lpRootPathName);
        private const UInt32 DRIVE_REMOVABLE = 0x02;

        [DllImport("kernel32.dll")]
        private static extern UInt32 GetLastError();

        [DllImport("kernel32.dll")]
        private static extern UInt32 SetFilePointer(IntPtr hFile, Int32 lDistanceToMove, IntPtr lpDistanceToMoveHigh, UInt32 dwMoveMethod);
        private const UInt32 FILE_BEGIN = 0x00;
        private const UInt32 INVALID_SET_FILE_POINTER = unchecked((UInt32)(-1));
        #endregion

        private IntPtr handle = IntPtr.Zero;

        public CardReader(string DriveName)
        {
            this.handle = CreateFile("\\\\.\\" + DriveName + ":", GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
            if (handle.ToInt32() == INVALID_HANDLE_VALUE)
                throw new Exception("Ошибка подключения к устройству. Код ошибки: " + GetLastError());
        }

        /// <summary>
        /// Ищет подключенный к компьютеру CardReader, а в нем - карточку с архивом
        /// </summary>
        /// <param name="Offset">Смещение, с которого будет искаться строка идентификации</param>
        /// <param name="Pattern">Строка, по которой будет идентифицировано устройство</param>
        /// <returns>В случае неудачи возвращает null</returns>
        public static CardReader FindCardReader(int Offset, string Pattern)
        {
            for (char c = 'B'; (byte)c <= 'Z'; c = (char)(((byte)c) + 1))
            {
                if (GetDriveType(c.ToString() + ":\\") == DRIVE_REMOVABLE)
                {
                    CardReader res = null;
                    try
                    {
                        res = new CardReader(c.ToString());
                        if (Encoding.ASCII.GetString(res.Read(Offset, 512)).Contains(Pattern))
                            return res;
                    }
                    catch
                    {
                        continue;
                    }
                    
                    res.Close();
                }
            }
            return null;
        }
        /// <summary>
        /// Читает с устройства массив байт
        /// </summary>
        /// <param name="Offset">Смещение, с которого начинается чтение</param>
        /// <param name="Size">Число байт которое надо прочитать</param>
        /// <returns>Массив байт, считанный с устройства. Размер массива может быть меньше</returns>
        public byte[] Read(int Offset, int Size)
        {
            int RealSize = 512 * (int)Math.Ceiling(Size / 512.0); // т.к. читать можно блоками кратными 512 байт
            byte[] res = new byte[RealSize];
            uint ReadSize = 0;
            if (SetFilePointer(this.handle, Offset, IntPtr.Zero, FILE_BEGIN) == INVALID_SET_FILE_POINTER)
                throw new Exception("Ошибка установки указателя чтения. Код ошибке: " + GetLastError());
            if (!ReadFile(this.handle, res, (uint)RealSize, out ReadSize, IntPtr.Zero))
                throw new Exception("Ошибка чтения с устройства. Код ошибки: " + GetLastError());
            if (ReadSize < RealSize)
                res = Utils.GetSubArray(res, 0, (int)ReadSize);
            else
                res = Utils.GetSubArray(res, 0, (int)Size);
            return res;
        }
        /// <summary>
        /// Записывает массив байт
        /// </summary>
        public void Write(byte[] Buffer)
        {
            uint ui = 0;
            if (!WriteFile(this.handle, Buffer, (uint)Buffer.Length, out ui, IntPtr.Zero))
                throw new Exception("Ошибка записи: " + GetLastError());
        }

        public void Close()
        {
            CloseHandle(this.handle);
        }
    }
}
