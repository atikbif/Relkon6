using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Kontel.Relkon
{
    /// <summary>
    /// Содержит набор различных математических функций,
    /// не входящих в класс Math
    /// </summary>
    public sealed class AppliedMath
    {
        /// <summary>
        /// Возвращает НОД(a,b)
        /// </summary>
        public static int NOD(int a, int b)
        {
            int c = Math.Min(a, b);
            int d = Math.Max(a, b);
            while (d != 0)
            {
                int r = c % d;
                c = d;
                d = r;
            }
            return c;
        }
        /// <summary>
        /// Возвращает общий НОД для всех чисел из массива
        /// </summary>
        public static int NOD(int[] a)
        {
            if (a.Length == 1)
                return a[0];
            List<int> nods = new List<int>();
            for (int i = 0; i < a.Length - 1; i++)
            {
                for (int j = i + 1; j < a.Length; j++)
                {
                    int n = AppliedMath.NOD(a[i], a[j]);
                    if (n == 1)
                        return 1;
                    else
                        if (!nods.Contains(n))
                            nods.Add(n);
                }
            }
            nods.Sort(); // т.к. надо вернуть минимальное значение
            return nods[0];
        }
        /// <summary>
        /// Возвращает бинарный код числа
        /// </summary>
        public static byte[] DecToBin(int Value)
        {
            List<byte> tmp = new List<byte>();
            byte rem = 0;
            do
            {
                rem = (byte)(Value % 2);
                Value = Value / 2;
                tmp.Add(rem);
            }
            while (Value != 0);
            byte[] res = new byte[tmp.Count];
            for (int i = res.Length - 1; i >= 0; i--)
            {
                res[res.Length - 1 - i] = tmp[i];
            }
            return res;
        }
        /// <summary>
        /// Возвращает бинарный код числа
        /// </summary>
        /// <param name="DC">
        /// минимальное число разрядов, которые должен содержать код
        /// </param>
        public static byte[] DecToBin(int Value, int DC)
        {
            byte[] res = new byte[DC];
            byte[] code = AppliedMath.DecToBin(Value);
            if (code.Length >= DC)
                return code;
            int dx = DC - code.Length;
            for (int i = 0; i < DC; i++)
                if (i < dx)
                    res[i] = 0;
                else
                    res[i] = code[i - dx];
            return res;
        }
        /// <summary>
        /// Преобразует бинареый код числа в его десятичное представление
        /// </summary>
        public static int BinToDec(byte[] BinaryCode)
        {
            int res = 0;
            for (int i = 0; i < BinaryCode.Length; i++)
            {
                res += BinaryCode[i] * (int)Math.Pow(2, BinaryCode.Length - i - 1);
            }
            return res;
        }
        /// <summary>
        /// Преобразует бинарный код числа в его 
        /// шестнадцатеричное представление
        /// </summary>
        public static string BinToHex(int[] BinaryCode)
        {
            string res = "";
            for (int j = 0; j < 2; j++)
            {
                int s = 0;
                for (int i = 0; i < 4; i++)
                    s += BinaryCode[i + j * 4] << 3 - i;
                switch (s)
                {
                    case 10:
                        res += "A";
                        break;
                    case 11:
                        res += "B";
                        break;
                    case 12:
                        res += "C";
                        break;
                    case 13:
                        res += "D";
                        break;
                    case 14:
                        res += "E";
                        break;
                    case 15:
                        res += "F";
                        break;
                    default:
                        res += s.ToString();
                        break;
                }
            }
            return res;
        }
        /// <summary>
        /// Проверяет, является ли строка шестнадцатеричным числом
        /// </summary>
        public static bool IsValidHexNumber(string Value)
        {
            return (Regex.IsMatch(Value, "^0x[0-9A-F]+$", RegexOptions.IgnoreCase) || Regex.IsMatch(Value, "^[0-9A-F]+h$", RegexOptions.IgnoreCase) || Regex.IsMatch(Value, "^[0-9A-F]+$", RegexOptions.IgnoreCase));
        }
        /// <summary>
        /// Проверяет, являеся ли строка десятичным числом
        /// </summary>
        public static bool IsValidDecNumber(string Value)
        {
            try
            {
                int i = int.Parse(Value);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Проверяет,является ли строка числом типа decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidDecimalTypeNumber(string value)
        {
            try
            {
                decimal.Parse(value);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Проверяет, является ли строка десятичным или шестнадцатеричным числом
        /// </summary>
        public static bool IsValidNumber(string Value)
        {
            return (AppliedMath.IsValidDecNumber(Value) || AppliedMath.IsValidHexNumber(Value));
        }
        /// <summary>
        /// Возвращает старший байт двухбайтовой переменной
        /// </summary>
        public static byte Hi(int Value)
        {
            return (byte)((Value & 0x0000FF00) >> 8);
        }
        /// <summary>
        /// Возвращает младший байт
        /// </summary>
        public static byte Low(int Value)
        {
            return (byte)(Value & 0x000000FF);
        }
        /// <summary>
        /// Возвращает массив байт, образующих число, начиная со старшего
        /// </summary>
        public static  byte[] IntToBytes(int Value)
        {
            byte[] res = new byte[4];
            uint Mask = 0xFF000000;
            for (int i = 0; i < 4; i++)
            {
                res[i] = (byte)(((uint)Value & Mask) >> (3 - i) * 8);
                Mask >>= 8;
            }
            return res;
        }
        /// <summary>
        /// Восстанавливает число по байтам, его составляющим; байты начинаются со старшего
        /// </summary>
        public static int BytesToInt(byte[] Bytes)
        {
            return (int)AppliedMath.BytesToLong(Bytes);
        }
        public static long BytesToLong(byte[] bytes)
        {
            long res = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                long t = bytes[i];
                res |= t << (8 * (bytes.Length - 1 - i));
            }
            return res;
        }
        /// <summary>
        ///  Восстанавливает число по байтам, его составляющим
        /// </summary>
        /// <param name="Bytes"></param>
        /// <param name="InverseByteOrder">Порядок следования байт в памяти процессора (прямой - сначала идет 
        /// старший байт, а за ним младший, или обратный - сначала младший, а за ним - старший)</param>
        /// <returns></returns>
        public static int BytesToInt(byte[] Bytes, Int32 StartIndex, Boolean InverseByteOrder)
        {
            if (InverseByteOrder) return (Bytes[StartIndex]) + (Bytes[StartIndex+1] << 8);
            else return (Bytes[StartIndex] << 8) + (Bytes[StartIndex+1]);
        }
        /// <summary>
        /// Преобразует шестнадцатеричное представление числа в десятичное
        /// </summary>
        public static int HexToDec(string value)
        {
            string s = value.ToLower().Replace("0x", "").Replace("h", "");
            int j = 1;
            int res = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                int k = 0;
                switch (s[i])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        k = int.Parse(s[i].ToString());
                        break;
                    case 'a':
                        k = 10;
                        break;
                    case 'b':
                        k = 11;
                        break;
                    case 'c':
                        k = 12;
                        break;
                    case 'd':
                        k = 13;
                        break;
                    case 'e':
                        k = 14;
                        break;
                    case 'f':
                        k = 15;
                        break;
                }
                res += j * k;
                j *= 16;
            }
            return res;
        }
        /// <summary>
        /// Преобразует десятичное представление числа в шестнадцатеричное
        /// </summary>
        public static string DecToHex(int value)
        {
            string t = "";
            int rem = 0;
            do
            {
                rem = value % 16;
                value = value / 16;
                switch (rem)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        t += rem.ToString();
                        break;
                    case 10:
                        t += "A";
                        break;
                    case 11:
                        t += "B";
                        break;
                    case 12:
                        t += "C";
                        break;
                    case 13:
                        t += "D";
                        break;
                    case 14:
                        t += "E";
                        break;
                    case 15:
                        t += "F";
                        break;
                }
            }
            while (value != 0);
            string res = "";
            for (int i = t.Length - 1; i >= 0; i--)
            {
                res += t[i];
            }
            return res;
        }
    }
}
