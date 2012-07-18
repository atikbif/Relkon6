using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Kontel.Relkon
{
    /// <summary>
    /// ��������� ������ ��������� �� ����
    /// ����������� Epson
    /// </summary>
    public sealed class EpsonPultEncoding
    {
        private static Hashtable nonStandartSymbolCodes = null;

        /// <summary>
        /// ��������� hash-table ������� ���������� ������
        /// </summary>
        private static void CreateRussianEncoding()
        {
            if (EpsonPultEncoding.nonStandartSymbolCodes != null)
                return;
            EpsonPultEncoding.nonStandartSymbolCodes = new Hashtable();
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x41);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA0);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x42);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA1);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xE0);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x45);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA2);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA3);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA4);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA5);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA6);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x4B);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA7);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x4D);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x48);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x4F);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA8);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x50);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x43);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x54);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xA9);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xAA);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x58);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xE1);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xAB);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xAC);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xE2);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xAD);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xAE);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x62);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xAF);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB0);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB1);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x61);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB2);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB3);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB4);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xE3);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x65);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB5);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB6);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB7);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB8);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xB9);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xBA);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xBB);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xBC);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xBD);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x6F);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xBE);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x70);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x63);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xBF);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x79);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xE4);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0x78);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xE5);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xC0);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xC1);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xE6);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xC2);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xC3);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xC4);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xC5);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xC6);
            EpsonPultEncoding.nonStandartSymbolCodes.Add('�', 0xC7);
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
        /// ���������� ��� ������� � ��������� ������
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
        /// ����������� ������ � ������ ���� � ��������� ������
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
        /// ���������� ������ �� ��� ���� � ��������� ������
        /// </summary>
        /// <returns>������, ��c������ �� �������� �������</returns>
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
        /// ���������� ������ �� �� ������������� � ��������� ������
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
