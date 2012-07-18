using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Kontel.Relkon
{
    /// <summary>
    /// Кодировка пульов индикации на базе
    /// контроллера Epson
    /// </summary>
    public sealed class EpsonPultEncoding
    {
        private static Hashtable nonStandartSymbolCodes = null;

        /// <summary>
        /// Заполняет hash-table русской кодировкой пульта
        /// </summary>
        private static void CreateRussianEncoding()
        {
            if (EpsonPultEncoding.nonStandartSymbolCodes != null)
                return;
            EpsonPultEncoding.nonStandartSymbolCodes = new Hashtable();
            EpsonPultEncoding.nonStandartSymbolCodes.Add('А', 0x41);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Б', 0xA0);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('В', 0x42);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Г', 0xA1);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Д', 0xE0);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Е', 0x45);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ё', 0xA2);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ж', 0xA3);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('З', 0xA4);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('И', 0xA5);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Й', 0xA6);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('К', 0x4B);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Л', 0xA7);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('М', 0x4D);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Н', 0x48);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('О', 0x4F);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('П', 0xA8);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Р', 0x50);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('С', 0x43);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Т', 0x54);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('У', 0xA9);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ф', 0xAA);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Х', 0x58);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ц', 0xE1);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ч', 0xAB);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ш', 0xAC);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Щ', 0xE2);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ъ', 0xAD);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ы', 0xAE);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ь', 0x62);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Э', 0xAF);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Ю', 0xB0);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('Я', 0xB1);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('а', 0x61);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('б', 0xB2);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('в', 0xB3);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('г', 0xB4);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('д', 0xE3);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('е', 0x65);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ё', 0xB5);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ж', 0xB6);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('з', 0xB7);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('и', 0xB8);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('й', 0xB9);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('к', 0xBA);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('л', 0xBB);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('м', 0xBC);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('н', 0xBD);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('о', 0x6F);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('п', 0xBE);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('р', 0x70);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('с', 0x63);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('т', 0xBF);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('у', 0x79);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ф', 0xE4);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('х', 0x78);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ц', 0xE5);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ч', 0xC0);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ш', 0xC1);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('щ', 0xE6);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ъ', 0xC2);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ы', 0xC3);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ь', 0xC4);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('э', 0xC5);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('ю', 0xC6);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('я', 0xC7);
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0x7B), 0x7B); // 10
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0x7C), 0x7C); // 12
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0x7D), 0x7D); // 15
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0x7E), 0x7E); // Enter
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xC8), 0xC8); // <<
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xC9), 0xC9); // >>
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xCE), 0xCE); // f
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xD7), 0xD7); // I
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xD8), 0xD8); // II
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xD9), 0xD9); // Up
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xDA), 0xDA); // Down
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xED), 0xED); // Ring
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xF0), 0xF0); // 1/4
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xF1), 0xF1); // 1/3
            EpsonPultEncoding.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xF2), 0xF2); // 1/2
        }
        /// <summary>
        /// Возвращает код символа в кодировке пульта
        /// </summary>
        public static byte GetByte(char Symbol)
        {
            EpsonPultEncoding.CreateRussianEncoding();
            if (EpsonPultEncoding.nonStandartSymbolCodes.ContainsKey(Symbol))
            {
                return (byte)((int)EpsonPultEncoding.nonStandartSymbolCodes[Symbol]);
            }
            else
            {
                return Encoding.ASCII.GetBytes(Symbol.ToString())[0];
            }
        }
        /// <summary>
        /// Преобразует строку в массив байт в кодировке пульта
        /// </summary>
        public static byte[] GetBytes(string s)
        {
            EpsonPultEncoding.CreateRussianEncoding();
            List<byte> res = new List<byte>();
            for (int i = 0; i < s.Length; i++)
            {
                res.Add(EpsonPultEncoding.GetByte(s[i]));
            }
            return res.ToArray();
        }
        /// <summary>
        /// Возвращает символ по его коду в кодировке пульта
        /// </summary>
        /// <returns>Строка, соcтоящая из искомого символа</returns>
        public static string GetChar(byte Code)
        {
            EpsonPultEncoding.CreateRussianEncoding();
            string res = "";
            if (EpsonPultEncoding.nonStandartSymbolCodes.ContainsValue((int)Code))
            {
                foreach (char key in EpsonPultEncoding.nonStandartSymbolCodes.Keys)
                {
                    if ((int)EpsonPultEncoding.nonStandartSymbolCodes[key] == Code)
                    {
                        res = key.ToString();
                        break;
                    }
                }
            }
            else
            {
                res = Encoding.ASCII.GetString(new byte[] { Code });
            }
            return res;
        }
        /// <summary>
        /// Возвращает строку по ее представлению в кодировке пульта
        /// </summary>
        public static string GetString(byte[] Bytes)
        {
            EpsonPultEncoding.CreateRussianEncoding();
            string res = "";
            for (int i = 0; i < Bytes.Length; i++)
                res += EpsonPultEncoding.GetChar(Bytes[i]);
            return res;
        }
    }
}
