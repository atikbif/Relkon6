using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using System.Drawing;
using Kontel.Relkon;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace Kontel.Relkon
{
    /// <summary>
    /// Отдельный символ индикатора
    /// </summary>
    public sealed class LCDIndicatorSymbol : Control
    {
        private char symbol; // символ, отображаемый компонентом
        private static Hashtable nonStandartSymbolCodes; // коды всех символов, которые может отображать компонент
        private static Hashtable masks; // содержит маски для отрисовки символов
        private Color defaultBackColor = Color.FromKnownColor(KnownColor.Control); // цвет фона по умолчанию
        private Color activePixelColor = Color.Black; // цвет активного пикселя
        private Color inactivePixelColor = Color.White; // цвет неактивного пикселя
        private Color selectedSymbolColor = Color.FromArgb(178, 180, 191); // цает выделеннго символа
        private int horizontalResolution = 5; // разрешение символа по вериткали
        private int verticalResolution = 8; // разрешение символа по горизонтали
        private int pixelWidth = 3; // ширина 1 пикселя символа в пикселях экрана
        private int sizeBetweenPixels = 1; // расстояние между пикселями
        private Point positionInIndicator; // позиция символа(Y - строка, X - индекс в строке) в индикаторе, к которому он относится
        private bool selected = false;

        public LCDIndicatorSymbol()
        {
            this.Size = this.MaximumSize;
            if (LCDIndicatorSymbol.nonStandartSymbolCodes == null)
                LCDIndicatorSymbol.CreateCodesHashtable();
            if (LCDIndicatorSymbol.masks == null)
                LCDIndicatorSymbol.CreateSymbolMasksHashtable();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Symbol = ' ';
        }
        /// <summary>
        /// Возвращает или устанавливает позиция символа(Y - строка, X - индекс в строке)
        /// в индикаторе, к которому он относится
        /// </summary>
        public Point PositionInIndicator
        {
            get
            {
                return this.positionInIndicator;
            }
            set
            {
                this.positionInIndicator = value;
            }
        }
        /// <summary>
        /// Создает хеш-таблицу символов и их кодов
        /// </summary>
        private static void CreateCodesHashtable()
        {
            LCDIndicatorSymbol.nonStandartSymbolCodes = new Hashtable();
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('А',0x41);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Б',0xA0);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('В',0x42);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Г',0xA1);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Д',0xE0);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Е',0x45);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ё',0xA2);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ж',0xA3);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('З',0xA4);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('И',0xA5);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Й',0xA6);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('К',0x4B);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Л',0xA7);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('М',0x4D);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Н',0x48);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('О',0x4F);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('П',0xA8);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Р',0x50);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('С',0x43);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Т',0x54);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('У',0xA9);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ф',0xAA);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Х',0x58);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ц',0xE1);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ч',0xAB);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ш',0xAC);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Щ',0xE2);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ъ',0xAD);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ы',0xAE);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ь',0x62);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Э',0xAF);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Ю',0xB0);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('Я',0xB1);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('а',0x61);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('б',0xB2);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('в',0xB3);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('г',0xB4);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('д',0xE3);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('е',0x65);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ё',0xB5);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ж',0xB6);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('з',0xB7);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('и',0xB8);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('й',0xB9);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('к',0xBA);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('л',0xBB);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('м',0xBC);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('н',0xBD);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('о',0x6F);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('п',0xBE);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('р',0x70);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('с',0x63);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('т',0xBF);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('у',0x79);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ф',0xE4);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('х',0x78);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ц',0xE5);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ч',0xC0);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ш',0xC1);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('щ',0xE6);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ъ',0xC2);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ы',0xC3);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ь',0xC4);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('э',0xC5);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('ю',0xC6);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add('я',0xC7);
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0x7B), 0x7B); // 10
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0x7C), 0x7C); // 12
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0x7D), 0x7D); // 15
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0x7E), 0x7E); // Enter
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xC8), 0xC8); // <<
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xC9), 0xC9); // >>
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xCE), 0xCE); // f
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xD7), 0xD7); // I
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xD8), 0xD8); // II
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xD9), 0xD9); // Up
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xDA), 0xDA); // Down
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xED), 0xED); // Ring
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xF0), 0xF0); // 1/4
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xF1), 0xF1); // 1/3
            LCDIndicatorSymbol.nonStandartSymbolCodes.Add((char)(0xFF00 + 0xF2), 0xF2); // 1/2
        }
        /// <summary>
        /// Создает хеш-таблицу символов и их масок; каждый байт массива маски
        /// определяет соответствующий столбец пикселей символа
        /// </summary>
        private static void CreateSymbolMasksHashtable()
        {
            LCDIndicatorSymbol.masks = new Hashtable();

            LCDIndicatorSymbol.masks.Add('!', new byte[5] { 0, 0, 242, 0, 0 });
            LCDIndicatorSymbol.masks.Add('"', new byte[5] { 0, 224, 0, 224, 0 });
            LCDIndicatorSymbol.masks.Add('#', new byte[5] { 40, 254, 40, 254, 40 });
            LCDIndicatorSymbol.masks.Add('$', new byte[5] { 36, 84, 254, 84, 72 });
            LCDIndicatorSymbol.masks.Add('%', new byte[5] { 196, 200, 16, 38, 70 });
            LCDIndicatorSymbol.masks.Add('&', new byte[5] { 108, 146, 170, 68, 10 });
            LCDIndicatorSymbol.masks.Add('\'', new byte[5] { 0, 160, 192, 0, 0 });
            LCDIndicatorSymbol.masks.Add('(', new byte[5] { 0, 56, 68, 130, 0 });
            LCDIndicatorSymbol.masks.Add(')', new byte[5] { 0, 130, 68, 56, 0 });
            LCDIndicatorSymbol.masks.Add('*', new byte[5] { 40, 16, 124, 16, 40 });
            LCDIndicatorSymbol.masks.Add('+', new byte[5] { 16, 16, 124, 16, 16 });
            LCDIndicatorSymbol.masks.Add(',', new byte[5] { 0, 10, 12, 0, 0 });
            LCDIndicatorSymbol.masks.Add('-', new byte[5] { 16, 16, 16, 16, 16 });
            LCDIndicatorSymbol.masks.Add('.', new byte[5] { 0, 6, 6, 0, 0 });
            LCDIndicatorSymbol.masks.Add('/', new byte[5] { 4, 8, 16, 32, 64 });
            LCDIndicatorSymbol.masks.Add('0', new byte[5] { 124, 138, 146, 162, 124 });
            LCDIndicatorSymbol.masks.Add('1', new byte[5] { 0, 66, 254, 2, 0 });
            LCDIndicatorSymbol.masks.Add('2', new byte[5] { 66, 134, 138, 146, 98 });
            LCDIndicatorSymbol.masks.Add('3', new byte[5] { 132, 130, 162, 210, 140 });
            LCDIndicatorSymbol.masks.Add('4', new byte[5] { 24, 40, 72, 254, 8 });
            LCDIndicatorSymbol.masks.Add('5', new byte[5] { 228, 162, 162, 162, 156 });
            LCDIndicatorSymbol.masks.Add('6', new byte[5] { 60, 82, 146, 146, 28 });
            LCDIndicatorSymbol.masks.Add('7', new byte[5] { 128, 142, 144, 160, 192 });
            LCDIndicatorSymbol.masks.Add('8', new byte[5] { 108, 146, 146, 146, 108 });
            LCDIndicatorSymbol.masks.Add('9', new byte[5] { 96, 146, 146, 148, 120 });
            LCDIndicatorSymbol.masks.Add(':', new byte[5] { 0, 108, 108, 0, 0 });
            LCDIndicatorSymbol.masks.Add(';', new byte[5] { 0, 106, 108, 0, 0 });
            LCDIndicatorSymbol.masks.Add('<', new byte[5] { 16, 40, 68, 130, 0 });
            LCDIndicatorSymbol.masks.Add('=', new byte[5] { 40, 40, 40, 40, 40 });
            LCDIndicatorSymbol.masks.Add('>', new byte[5] { 0, 130, 68, 40, 16 });
            LCDIndicatorSymbol.masks.Add('?', new byte[5] { 64, 128, 138, 144, 96 });
            LCDIndicatorSymbol.masks.Add('@', new byte[5] { 76, 146, 158, 130, 124 });
            LCDIndicatorSymbol.masks.Add('A', new byte[5] { 126, 136, 136, 136, 126 });
            LCDIndicatorSymbol.masks.Add('B', new byte[5] { 254, 146, 146, 146, 108 });
            LCDIndicatorSymbol.masks.Add('C', new byte[5] { 124, 130, 130, 130, 68 });
            LCDIndicatorSymbol.masks.Add('D', new byte[5] { 254, 130, 130, 68, 56 });
            LCDIndicatorSymbol.masks.Add('E', new byte[5] { 254, 146, 146, 146, 130 });
            LCDIndicatorSymbol.masks.Add('F', new byte[5] { 254, 144, 144, 128, 128 });
            LCDIndicatorSymbol.masks.Add('G', new byte[5] { 124, 130, 146, 146, 94 });
            LCDIndicatorSymbol.masks.Add('H', new byte[5] { 254, 16, 16, 16, 254 });
            LCDIndicatorSymbol.masks.Add('I', new byte[5] { 0, 130, 254, 130, 0 });
            LCDIndicatorSymbol.masks.Add('J', new byte[5] { 4, 2, 130, 252, 128 });
            LCDIndicatorSymbol.masks.Add('K', new byte[5] { 254, 16, 40, 68, 130 });
            LCDIndicatorSymbol.masks.Add('L', new byte[5] { 254, 2, 2, 2, 2 });
            LCDIndicatorSymbol.masks.Add('M', new byte[5] { 254, 64, 48, 64, 254 });
            LCDIndicatorSymbol.masks.Add('N', new byte[5] { 254, 32, 16, 8, 254 });
            LCDIndicatorSymbol.masks.Add('O', new byte[5] { 124, 130, 130, 130, 124 });
            LCDIndicatorSymbol.masks.Add('P', new byte[5] { 254, 144, 144, 144, 96 });
            LCDIndicatorSymbol.masks.Add('Q', new byte[5] { 124, 130, 138, 132, 122 });
            LCDIndicatorSymbol.masks.Add('R', new byte[5] { 254, 144, 152, 148, 98 });
            LCDIndicatorSymbol.masks.Add('S', new byte[5] { 98, 146, 146, 146, 140 });
            LCDIndicatorSymbol.masks.Add('T', new byte[5] { 128, 128, 254, 128, 128 });
            LCDIndicatorSymbol.masks.Add('U', new byte[5] { 252, 2, 2, 2, 252 });
            LCDIndicatorSymbol.masks.Add('V', new byte[5] { 248, 4, 2, 4, 248 });
            LCDIndicatorSymbol.masks.Add('W', new byte[5] { 252, 2, 28, 2, 252 });
            LCDIndicatorSymbol.masks.Add('X', new byte[5] { 198, 40, 16, 40, 198 });
            LCDIndicatorSymbol.masks.Add('Y', new byte[5] { 224, 16, 14, 16, 224 });
            LCDIndicatorSymbol.masks.Add('Z', new byte[5] { 134, 138, 146, 162, 194 });
            LCDIndicatorSymbol.masks.Add('[', new byte[5] { 0, 254, 130, 130, 0 });
            LCDIndicatorSymbol.masks.Add('~', new byte[5] { 64, 128, 64, 64, 128 });
            LCDIndicatorSymbol.masks.Add(']', new byte[5] { 0, 130, 130, 254, 0 });
            LCDIndicatorSymbol.masks.Add('^', new byte[5] { 32, 64, 128, 64, 32 });
            LCDIndicatorSymbol.masks.Add('_', new byte[5] { 2, 2, 2, 2, 2 });
            LCDIndicatorSymbol.masks.Add('\\', new byte[5] { 0, 0, 0, 0, 0 });
            LCDIndicatorSymbol.masks.Add('a', new byte[5] { 4, 42, 42, 42, 30 });
            LCDIndicatorSymbol.masks.Add('b', new byte[5] { 254, 18, 34, 34, 28 });
            LCDIndicatorSymbol.masks.Add('c', new byte[5] { 28, 34, 34, 34, 4 });
            LCDIndicatorSymbol.masks.Add('d', new byte[5] { 28, 34, 34, 18, 254 });
            LCDIndicatorSymbol.masks.Add('e', new byte[5] { 28, 42, 42, 42, 24 });
            LCDIndicatorSymbol.masks.Add('f', new byte[5] { 16, 126, 144, 128, 64 });
            LCDIndicatorSymbol.masks.Add('g', new byte[5] { 16, 42, 42, 42, 60 });
            LCDIndicatorSymbol.masks.Add('h', new byte[5] { 254, 16, 32, 32, 30 });
            LCDIndicatorSymbol.masks.Add('i', new byte[5] { 0, 34, 190, 2, 0 });
            LCDIndicatorSymbol.masks.Add('j', new byte[5] { 0, 4, 2, 34, 188 });
            LCDIndicatorSymbol.masks.Add('k', new byte[5] { 254, 8, 20, 34, 0 });
            LCDIndicatorSymbol.masks.Add('l', new byte[5] { 0, 130, 254, 2, 0 });
            LCDIndicatorSymbol.masks.Add('m', new byte[5] { 62, 32, 24, 32, 30 });
            LCDIndicatorSymbol.masks.Add('n', new byte[5] { 62, 16, 32, 32, 30 });
            LCDIndicatorSymbol.masks.Add('o', new byte[5] { 28, 34, 34, 34, 28 });
            LCDIndicatorSymbol.masks.Add('p', new byte[5] { 62, 40, 40, 40, 16 });
            LCDIndicatorSymbol.masks.Add('q', new byte[5] { 16, 40, 40, 40, 62 });
            LCDIndicatorSymbol.masks.Add('r', new byte[5] { 62, 16, 32, 32, 16 });
            LCDIndicatorSymbol.masks.Add('s', new byte[5] { 18, 42, 42, 42, 4 });
            LCDIndicatorSymbol.masks.Add('t', new byte[5] { 32, 252, 34, 2, 4 });
            LCDIndicatorSymbol.masks.Add('u', new byte[5] { 60, 2, 2, 4, 62 });
            LCDIndicatorSymbol.masks.Add('v', new byte[5] { 56, 4, 2, 4, 56 });
            LCDIndicatorSymbol.masks.Add('w', new byte[5] { 60, 2, 12, 2, 60 });
            LCDIndicatorSymbol.masks.Add('x', new byte[5] { 34, 20, 8, 20, 34 });
            LCDIndicatorSymbol.masks.Add('y', new byte[5] { 48, 10, 10, 10, 60 });
            LCDIndicatorSymbol.masks.Add('z', new byte[5] { 34, 38, 42, 50, 34 });
            LCDIndicatorSymbol.masks.Add(' ', new byte[5] { 0, 0, 0, 0, 0 });
            LCDIndicatorSymbol.masks.Add('А', new byte[5] { 126, 136, 136, 136, 126 });
            LCDIndicatorSymbol.masks.Add('Б', new byte[5] { 254, 146, 146, 146, 204 });
            LCDIndicatorSymbol.masks.Add('В', new byte[5] { 254, 146, 146, 146, 108 });
            LCDIndicatorSymbol.masks.Add('Г', new byte[5] { 254, 128, 128, 128, 192 });
            LCDIndicatorSymbol.masks.Add('Д', new byte[5] { 7, 138, 242, 130, 255 });
            LCDIndicatorSymbol.masks.Add('Е', new byte[5] { 254, 146, 146, 146, 130 });
            LCDIndicatorSymbol.masks.Add('Ё', new byte[5] { 62, 170, 42, 162, 34 });
            LCDIndicatorSymbol.masks.Add('Ж', new byte[5] { 238, 16, 254, 16, 238 });
            LCDIndicatorSymbol.masks.Add('З', new byte[5] { 146, 146, 146, 146, 108 });
            LCDIndicatorSymbol.masks.Add('И', new byte[5] { 254, 8, 16, 32, 254 });
            LCDIndicatorSymbol.masks.Add('Й', new byte[5] { 62, 132, 72, 144, 62 });
            LCDIndicatorSymbol.masks.Add('К', new byte[5] { 254, 16, 40, 68, 130 });
            LCDIndicatorSymbol.masks.Add('Л', new byte[5] { 4, 130, 252, 128, 254 });
            LCDIndicatorSymbol.masks.Add('М', new byte[5] { 254, 64, 48, 64, 254 });
            LCDIndicatorSymbol.masks.Add('Н', new byte[5] { 254, 16, 16, 16, 254 });
            LCDIndicatorSymbol.masks.Add('О', new byte[5] { 124, 130, 130, 130, 124 });
            LCDIndicatorSymbol.masks.Add('П', new byte[5] { 254, 128, 128, 128, 254 });
            LCDIndicatorSymbol.masks.Add('Р', new byte[5] { 254, 144, 144, 144, 96 });
            LCDIndicatorSymbol.masks.Add('С', new byte[5] { 124, 130, 130, 130, 68 });
            LCDIndicatorSymbol.masks.Add('Т', new byte[5] { 128, 128, 254, 128, 128 });
            LCDIndicatorSymbol.masks.Add('У', new byte[5] { 226, 20, 8, 16, 224 });
            LCDIndicatorSymbol.masks.Add('Ф', new byte[5] { 56, 68, 254, 68, 56 });
            LCDIndicatorSymbol.masks.Add('Х', new byte[5] { 198, 40, 16, 40, 198 });
            LCDIndicatorSymbol.masks.Add('Ц', new byte[5] { 254, 2, 2, 2, 255 });
            LCDIndicatorSymbol.masks.Add('Ч', new byte[5] { 224, 16, 16, 16, 254 });
            LCDIndicatorSymbol.masks.Add('Ш', new byte[5] { 254, 2, 254, 2, 254 });
            LCDIndicatorSymbol.masks.Add('Щ', new byte[5] { 254, 2, 254, 2, 255 });
            LCDIndicatorSymbol.masks.Add('Ъ', new byte[5] { 128, 254, 18, 18, 12 });
            LCDIndicatorSymbol.masks.Add('Ы', new byte[5] { 254, 18, 12, 0, 254 });
            LCDIndicatorSymbol.masks.Add('Ь', new byte[5] { 254, 18, 34, 34, 28 });
            LCDIndicatorSymbol.masks.Add('Э', new byte[5] { 68, 130, 146, 146, 124 });
            LCDIndicatorSymbol.masks.Add('Ю', new byte[5] { 254, 16, 124, 130, 124 });
            LCDIndicatorSymbol.masks.Add('Я', new byte[5] { 98, 148, 152, 144, 254 });
            LCDIndicatorSymbol.masks.Add('а', new byte[5] { 4, 42, 42, 42, 30 });
            LCDIndicatorSymbol.masks.Add('б', new byte[5] { 60, 82, 82, 146, 140 });
            LCDIndicatorSymbol.masks.Add('в', new byte[5] { 62, 42, 42, 20, 0 });
            LCDIndicatorSymbol.masks.Add('г', new byte[5] { 62, 32, 32, 32, 48 });
            LCDIndicatorSymbol.masks.Add('д', new byte[5] { 7, 42, 50, 34, 63 });
            LCDIndicatorSymbol.masks.Add('е', new byte[5] { 28, 42, 42, 42, 24 });
            LCDIndicatorSymbol.masks.Add('ё', new byte[5] { 28, 170, 42, 170, 24 });
            LCDIndicatorSymbol.masks.Add('ж', new byte[5] { 54, 8, 62, 8, 54 });
            LCDIndicatorSymbol.masks.Add('з', new byte[5] { 42, 42, 42, 42, 20 });
            LCDIndicatorSymbol.masks.Add('и', new byte[5] { 62, 4, 8, 16, 62 });
            LCDIndicatorSymbol.masks.Add('й', new byte[5] { 30, 66, 36, 72, 30 });
            LCDIndicatorSymbol.masks.Add('к', new byte[5] { 62, 8, 20, 34, 0 });
            LCDIndicatorSymbol.masks.Add('л', new byte[5] { 4, 34, 60, 32, 62 });
            LCDIndicatorSymbol.masks.Add('м', new byte[5] { 62, 16, 8, 16, 62 });
            LCDIndicatorSymbol.masks.Add('н', new byte[5] { 62, 8, 8, 8, 62 });
            LCDIndicatorSymbol.masks.Add('о', new byte[5] { 28, 34, 34, 34, 28 });
            LCDIndicatorSymbol.masks.Add('п', new byte[5] { 62, 32, 32, 32, 62 });
            LCDIndicatorSymbol.masks.Add('р', new byte[5] { 62, 40, 40, 40, 16 });
            LCDIndicatorSymbol.masks.Add('с', new byte[5] { 28, 34, 34, 34, 4 });
            LCDIndicatorSymbol.masks.Add('т', new byte[5] { 32, 32, 62, 32, 32 });
            LCDIndicatorSymbol.masks.Add('у', new byte[5] { 48, 10, 10, 10, 60 });
            LCDIndicatorSymbol.masks.Add('ф', new byte[5] { 12, 18, 127, 18, 12 });
            LCDIndicatorSymbol.masks.Add('х', new byte[5] { 34, 20, 8, 20, 34 });
            LCDIndicatorSymbol.masks.Add('ц', new byte[5] { 62, 2, 2, 2, 63 });
            LCDIndicatorSymbol.masks.Add('ч', new byte[5] { 48, 8, 8, 8, 62 });
            LCDIndicatorSymbol.masks.Add('ш', new byte[5] { 62, 2, 62, 2, 62 });
            LCDIndicatorSymbol.masks.Add('щ', new byte[5] { 62, 2, 62, 2, 63 });
            LCDIndicatorSymbol.masks.Add('ъ', new byte[5] { 32, 62, 10, 10, 4 });
            LCDIndicatorSymbol.masks.Add('ы', new byte[5] { 62, 10, 4, 0, 62 });
            LCDIndicatorSymbol.masks.Add('ь', new byte[5] { 62, 10, 10, 4, 0 });
            LCDIndicatorSymbol.masks.Add('э', new byte[5] { 20, 34, 42, 42, 28 });
            LCDIndicatorSymbol.masks.Add('ю', new byte[5] { 62, 8, 28, 34, 28 });
            LCDIndicatorSymbol.masks.Add('я', new byte[5] { 16, 42, 44, 40, 62 });
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0x7B), new byte[5] { 248, 0, 248, 136, 248 });   // 10
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0x7C), new byte[5] { 248, 0, 184, 168, 232 });   // 12
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0x7D), new byte[5] { 248, 0, 232, 168, 184 });   // 15
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0x7E), new byte[5] { 16, 56, 84, 16, 240 });      // Enter 
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xC8), new byte[5] { 8, 20, 34, 8, 20 });        // <<
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xC9), new byte[5] { 20, 8, 34, 20, 8 });        // >>
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xCE), new byte[5] { 2, 18, 124, 144, 128 });    // f
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xD7), new byte[5] { 0, 132, 252, 132, 0 });     // I
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xD8), new byte[5] { 132, 252, 132, 252, 132 }); // II
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xD9), new byte[5] { 32, 64, 254, 64, 32 });     // Up
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xDA), new byte[5] { 8, 4, 254, 4, 8 });         // Down
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xED), new byte[5] { 4, 124, 254, 124, 4 });     // Ring
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xF0), new byte[5] { 232, 16, 40, 88, 188 });    // 1/4
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xF1), new byte[5] { 232, 16, 34, 106, 190 });   // 1/3
            LCDIndicatorSymbol.masks.Add((char)(0xFF00 + 0xF2), new byte[5] { 232, 16, 38, 106, 186 });   // 1/2
        }
        /// <summary>
        /// Возвращает или устанавливает символ, отображаемый индикатором;
        /// если вводится символ, чей код >255, то это специальный символ
        /// и для получения его кода надо из кода вводимого символа вычесть 0xFF00.
        /// </summary>
        public char Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Возвращает код символа в кодировке пульта
        /// </summary>
        public int Code
        {
            get
            {
                object value = null;
                if((int)(value)>255 || Regex.IsMatch(value.ToString(),"[а-я]",RegexOptions.Multiline))
                    value = LCDIndicatorSymbol.nonStandartSymbolCodes[this.symbol];
                else
                    value = (byte)this.symbol;
                if (value == null)
                    value = 20;
                return (int)value;
            }
        }
        /// Возвращает маску символа ввиде двумерного массива
        /// </remarks>
        /// <param name="RowMask">
        /// Маска символа по столбцам (каждый элемент массива определяет
        /// закрашивание столбца
        /// </param>
        /// <returns>
        /// Маска символа: 1-закрашенный пиксель, 
        /// 0-пустой
        /// </returns>
        public byte[,] ComputePixelMask(byte[] RowMasks)
        {
            byte[,] res = new byte[this.verticalResolution, this.horizontalResolution];
            for(int i = 0; i<this.horizontalResolution; i++)
            {
                byte[] bits = AppliedMath.DecToBin(RowMasks[i],8);
                for(int j = 0; j<8; j++)
                {
                    res[j,i] = bits[j];
                }
            }
            return res;
        }
        /// <summary>
        /// Возвращает маску символа (каждый элемент массива соответствует пикселю символа)
        /// </summary>
        private byte[,] Mask
        {
            get
            {
                object value = LCDIndicatorSymbol.masks[this.symbol];
                if (value == null)
                    value = new byte[5];
                return this.ComputePixelMask((byte[])value);
            }
        }
        /// <summary>
        /// Возвращает или устанавливает цвет активного пикселя символа
        /// </summary>
        public Color ActivePixelColor
        {
            get
            {
                return this.activePixelColor;
            }
            set
            {
                this.activePixelColor = value;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает цвет неактивного пикселя символа
        /// </summary>
        public Color InactivePixelColor
        {
            get
            {
                return this.inactivePixelColor;
            }
            set
            {
                this.inactivePixelColor = value;
            }
        }

        internal bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                if (this.selected != value)
                {
                    this.selected = value;
                    this.Invalidate();
                }
                
            }
        }

        #region Overrides

        public override System.Drawing.Color BackColor
        {
            get
            {
                return (this.Parent != null) ? this.Parent.BackColor : this.defaultBackColor;
            }
        }

        [Browsable(false)]
        public override Size MaximumSize
        {
            get
            {
                return new Size(this.horizontalResolution * (this.pixelWidth + this.sizeBetweenPixels) - this.sizeBetweenPixels, this.verticalResolution * (this.pixelWidth + this.sizeBetweenPixels) - this.sizeBetweenPixels);
            }
        }

        [Browsable(false)]
        public override Size MinimumSize
        {
            get
            {
                return this.MaximumSize;
            }
            set
            {
                base.MinimumSize = value;
            }
        }

        /*private Point ScreenPositionToPixelPosition(Point p)
        {
            int x = (p.X % (this.pixelWidth + this.sizeBetweenPixels) == this.pixelWidth) ? -1 : p.X / (this.pixelWidth + this.sizeBetweenPixels);
            int y = (p.Y % (this.pixelWidth + this.sizeBetweenPixels) == this.pixelWidth) ? -1 : p.Y / (this.pixelWidth + this.sizeBetweenPixels);
            return new Point(x, y);
        }

        private void CreateBitmap()
        {
            Bitmap b = new Bitmap(this.Width, this.Height);
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    Point p = this.ScreenPositionToPixelPosition(new Point(j, i));
                    if (p.X == -1 || p.Y == -1)
                        b.SetPixel(j, i, this.BackColor);
                    else
                        b.SetPixel(j, i, this.Mask[p.Y, p.X] == 1 ? this.activePixelColor : this.inactivePixelColor);
                }
            }
            
            this.OutputBitmap = b;
        }*/

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            SolidBrush brush = new SolidBrush(this.BackColor);
            graphics.FillRectangle(brush, 0, 0, this.Width, this.Height);
            for (int i = 0; i < this.verticalResolution; i++)
            {
                for (int j = 0; j < this.horizontalResolution; j++)
                {
                    brush.Color = this.selected ? this.selectedSymbolColor : this.inactivePixelColor;
                    if (this.Mask[i, j] == 1)
                        brush.Color = this.selected ? Color.Black : this.activePixelColor;
                    graphics.FillRectangle(brush, new RectangleF(j * (this.pixelWidth + this.sizeBetweenPixels),
                        i * (this.pixelWidth + this.sizeBetweenPixels),
                        this.pixelWidth, this.pixelWidth));
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            int MK_LBUTTON = 0x0001;
            switch(m.Msg) 
            {
                case 0x0200: //WM_MOUSEMOVE
                    return;
                case 0x0201: // WM_LBUTTONDOWN
                    int lparam = (this.Location.Y<<16) | this.Location.X;
                    Utils.SendMessage(this.Parent.Handle, (uint)m.Msg, new IntPtr(MK_LBUTTON), new IntPtr(lparam));
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

    }
}
