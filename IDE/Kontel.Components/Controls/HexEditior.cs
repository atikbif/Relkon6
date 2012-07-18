using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Kontel.Relkon
{

    public sealed partial class HexEditior : UserControl
    {
        private int _SymbolHeight = 14; //Высота символа, используемого шрифта

        public int SymbolHeight
        {
            get { return _SymbolHeight; }
        }
        private int _SymbolWidth = 7;   //Ширина символа, используемого шрифта
        private int _ScrollWidth = 16;  //Ширина вертикального ползунка

        private int _ArraySize = 35;    //Количество элементов массива
        private int _SegmentSize = 9;   //Текущий размер сегмента
        private int _SegmentAmount = 5; //Количестов сегментов, при заданной величене массива и размере сегмента
        private int _StartAddress = 4;  //Адрес первого элемента массива
        private int _CodingType = 16;   //Текущая кодировка(10,16);

        private System.Drawing.Color _CodeColor;            //Цвет основного поля
        private System.Drawing.Color _PresentationColor;    //Цвет поля предсавления

        private int _LastByte = -2;            //Байт, редактируемый при предыдущем вводе цифры (в поля байт)
        private int _CurrentByte = -1;         //Байт, редактриуемый при последнем вводе цифры (в поля байт)
        private int _CurrentByteEnter = 0;     //
        private int _CurrentLocationByte = 0;  //Бит на котроцй установлен курсор
        private bool _CursorInCode = true;     //Курсор в поле кода
        private int _Position = 0;             //Текущее положение ползунка
        private int _PositionX = 0;            //Текущее положение горизонтального ползунка
        private byte[] _ValuesArray;           //Массив переменных
        private SortedList<int, byte> _NewValuesList = new SortedList<int, byte>();//Массив измененных во время работы переменных

        public delegate void ByteChangeEventHandler(object sender, Kontel.Relkon.HexEditior.ByteChangeEventArgs e);
        public class ByteChangeEventArgs : EventArgs
        {
            /// <summary>
            /// Индекс байта, в котором было сгенерировано событие
            /// </summary>
            private int index;
            /// <summary>
            /// Новое значение байта, в котором было сгенерировано событие
            /// </summary>
            private int value;
            /// <summary>
            /// Поулчение индекса изменного байта
            /// </summary>
            public int Index
            {
                get
                {
                    return this.index;
                }
            }
            /// <summary>
            /// Поулчение нового значения байта
            /// </summary>
            public int Value
            {
                get
                {
                    return this.value;
                }
            }

            public ByteChangeEventArgs(int Index, byte Value)
            {
                this.index = Index;
                this.value = Value;
            }
        }
        /// <summary>
        /// Событие смены одного из байт
        /// </summary>
        public event ByteChangeEventHandler ChangedByte;


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="values">Входные переменные (массив)</param>
        /// <param name="Address">Стартовый адрес массива</param>
        public HexEditior()
        {
            InitializeComponent();
            this.TabIndex = 0;
            this.TabStop = true;
        }

        /// <summary>
        /// Запись новых значений в массив
        /// </summary>
        /// <param name="values">Входные переменные (массив)</param>
        /// <param name="Address">Стартовый адрес массива</param>
        public void ChangeValues(byte[] values, int Address, bool NewValue)
        {
            if (values.Length != 0)
            {
                int m_cp = 0;
                int m_cpx = 0;
                int m_index = 0;
                if ((values != null) && (!NewValue))
                {
                    m_cp = this._Position;
                    m_cpx = this._PositionX;
                    m_index = GetCodeIndex(this.CodeBox.SelectionStart);
                }
                if (NewValue)
                {
                    this._NewValuesList.Clear();
                }
                this._StartAddress = Address;
                this._ArraySize = values.Length;
                this._SegmentAmount = (this._ArraySize - (this._ArraySize % this._SegmentSize)) / this._SegmentSize + 1;
                this._ValuesArray = new byte[this._ArraySize];
                for (int i = 0; i < this._ArraySize; i++)
                {
                    if (!this._NewValuesList.ContainsKey(i + this._StartAddress))
                    {
                        this._ValuesArray[i] = values[i];
                    }
                }
                this._Position = m_cp;
                this._PositionX = m_cpx;
                ShowAll();
                ChangeNewValuesColor();
                this.MainScrollBar.Value = this._Position;
                this.HorizontalScrollBar.Value = this._PositionX;
                this.PresentationBox.SelectionStart = m_index + (m_index / this._SegmentSize); ;
                this.AddressBox.SelectionStart = (m_index / this._SegmentSize + 1) * 9 - 1;
                this.CodeBox.SelectionStart = GetCodeStart(m_index);
            }
        }

        /// <summary>
        /// Выделение измененных байт другим цветом
        /// </summary>
        private void ChangeNewValuesColor()
        {
            bool m_fok = this.CodeBox.Focused;
            if (m_fok)
            {
                this.PresentationBox.Focus();
            }
            for (int i = 0; i < _ArraySize; i++)
            {
                if (this._NewValuesList.ContainsKey(i+this._StartAddress))
                {
                    this.CodeBox.SelectionStart=GetCodeStart(i);
                    this.CodeBox.SelectionLength = _CodingType % 7;
                    this.CodeBox.SelectionColor = System.Drawing.Color.Brown;
                }
            }
            this.CodeBox.SelectionLength = 0;
            if (m_fok) 
            {
                this.CodeBox.Focus();
            }
        }


        /// <summary>
        /// Изменение размера и пересоздание строки для всех RichTextBox
        /// </summary>
        private void ShowAll()
        {
            //Изменение размеров основных элементов компонента
            int m_width;
            int m_height;
            //string m_s = "l\nl";
            //for (int i = 0; i < 100; i++)
            //    m_s = m_s + "W";
            using (Graphics g = this.AddressBox.CreateGraphics())
            {
                m_height = (int)g.MeasureString("0\n0", this.AddressBox.Font).Height;
                m_width = (int)g.MeasureString("00", this.AddressBox.Font).Width - (int)g.MeasureString("0", this.AddressBox.Font).Width;
            }
            this._SymbolWidth = m_width;
            this._SymbolHeight = m_height/2+1;
            this.AddressBox.Size = new System.Drawing.Size(this._SymbolWidth * 8 + 3, this._SymbolHeight * this._SegmentAmount + 2);
            this.CodeBox.Size = new System.Drawing.Size(((this._CodingType % 7 + 1) * this._SegmentSize + 1 + (this._SegmentSize - 1) / 8) * this._SymbolWidth + 3, this._SymbolHeight * this._SegmentAmount + 2);
            this.PresentationBox.Size = new System.Drawing.Size((this._SegmentSize + 1) * this._SymbolWidth + 3, this._SymbolHeight * this._SegmentAmount + 2);
            this.MainScrollBar.Size = new System.Drawing.Size(this._ScrollWidth, this.Size.Height);
            this.HorizontalScrollBar.Size = new System.Drawing.Size(this.Size.Width,this._ScrollWidth);
            this.MainScrollBar.Maximum = this._SymbolHeight * (this._SegmentAmount + 1) + 2 - this.Size.Height+this._ScrollWidth;
            this.MainScrollBar.Minimum = 0;
            this.HorizontalScrollBar.Maximum = this.AddressBox.Size.Width + this.CodeBox.Size.Width + this.PresentationBox.Size.Width+this._ScrollWidth+10 - this.Size.Width;
            this.HorizontalScrollBar.Minimum = 0;
            if (this.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.MainScrollBar.Size = new System.Drawing.Size(this._ScrollWidth, this.Size.Height - 4);
                this.HorizontalScrollBar.Size = new System.Drawing.Size(this.Size.Width - 4, this._ScrollWidth);
            }
            if (this.Size.Height <= this._SymbolHeight * (this._SegmentAmount + 1) + 2)
                this.MainScrollBar.Visible = true;
            else
                this.MainScrollBar.Visible = false;
            if (this.Size.Width <= this.AddressBox.Size.Width + this.CodeBox.Size.Width + this.PresentationBox.Size.Width)
                this.HorizontalScrollBar.Visible = true;
            else
                this.HorizontalScrollBar.Visible = false;

            this.AddressBox.Location = new System.Drawing.Point(0 - _PositionX, -_Position);
            this.CodeBox.Location = new System.Drawing.Point(this.AddressBox.Size.Width - _PositionX, -_Position);
            this.PresentationBox.Location = new System.Drawing.Point(this.AddressBox.Size.Width + this.CodeBox.Size.Width - _PositionX, -_Position);

            //Формирование сторок для RichTextBox
            int m_adr = this._StartAddress;
            int m_ind = 0;
            byte[] m_ByteString;
            m_ByteString = new byte[this._SegmentSize];
            StringBuilder m_AddressStroka = new StringBuilder("", 9 * this._SegmentAmount); ;
            StringBuilder m_CodeStroka = new StringBuilder("", (this._SegmentSize * (this._CodingType % 7 + 1) + 1) * this._SegmentAmount); ;
            StringBuilder m_PresentationStroka = new StringBuilder("", (this._SegmentSize + 1) * this._SegmentAmount); ;
            for (int i = 0; (i < this._SegmentAmount) && (i * this._SegmentSize < this._ArraySize); i++)
            {
                //Формирование стороки для адреса
                m_AddressStroka.Insert(m_AddressStroka.Length, TransformationDecToHex(m_adr, 8));
                m_AddressStroka.Insert(m_AddressStroka.Length, "\n");
                //Формирование стороки для основного поля и поя представлления
                int m_j = 0;
                for (; ((m_j < this._SegmentSize) && (i * this._SegmentSize + m_j < this._ArraySize)); m_j++)
                {
                    string s = "";
                    if (this._CodingType == 10)
                    {
                        if (this._NewValuesList.ContainsKey(m_ind + this._StartAddress))
                            s = "" + this._NewValuesList[m_ind + this._StartAddress];
                        else
                            s = "" + this._ValuesArray[m_ind];
                        while (s.Length < 3)
                            s = "0" + s;
                    }
                    else
                    {
                        if (this._NewValuesList.ContainsKey(m_ind + this._StartAddress))
                            s = TransformationDecToHex(this._NewValuesList[m_ind + this._StartAddress], 2);
                        else
                            s = TransformationDecToHex(this._ValuesArray[m_ind], 2);
                    }
                    m_CodeStroka.Insert(m_CodeStroka.Length, " ");
                    m_CodeStroka.Insert(m_CodeStroka.Length, s);
                    if (this._NewValuesList.ContainsKey(m_ind + this._StartAddress))
                        m_ByteString[m_j] = this._NewValuesList[m_ind + this._StartAddress];
                    else
                        m_ByteString[m_j] = this._ValuesArray[m_ind];
                    if (m_ByteString[m_j] < 32)
                        m_ByteString[m_j] = 32;
                    m_ind += 1;
                    if ((m_j % 8 == 7) && (m_j < this._SegmentSize - 1))
                    {
                        m_CodeStroka.Insert(m_CodeStroka.Length, " ");
                    }
                }
                for (; (m_j < this._SegmentSize); m_j++)
                    m_ByteString[m_j] = 32;
                m_CodeStroka.Insert(m_CodeStroka.Length, "\n");
                m_PresentationStroka.Insert(m_PresentationStroka.Length, Encoding.GetEncoding(866).GetString(m_ByteString));
                m_PresentationStroka.Insert(m_PresentationStroka.Length, "\n");
                m_adr = m_adr + this._SegmentSize;
            }
            //Запись сформированнях строк в RichTextBox
            this.AddressBox.Text = m_AddressStroka.ToString();
            this.CodeBox.Text = m_CodeStroka.ToString();
            this.PresentationBox.Text = m_PresentationStroka.ToString();
        }

        /// <summary>
        /// Сохранение изменных значений в основной массив
        /// </summary>
        public void SaveChanges()
        {
            for (int i = 0; i < this._ArraySize; i++)
                if (this._NewValuesList.ContainsKey(i + this._StartAddress))
                    this._ValuesArray[i] = this._NewValuesList[i + this._StartAddress];
            this._NewValuesList.Clear();
            this.CodeBox.ForeColor = this._CodeColor;
            //Смена цвета текста
            bool m_f = this.CodeBox.Focused;
            if (m_f) this.PresentationBox.Focus();
            int m_p = this.CodeBox.SelectionStart;
            this.CodeBox.SelectAll();
            this.CodeBox.SelectionColor = _CodeColor;
            this.CodeBox.SelectionStart = m_p;
            if (m_f) this.CodeBox.Focus();
        }

        /// <summary>
        /// Возвращает измененные занчения
        /// </summary>
        public SortedList<int, byte> GetValues()
        {
            SortedList<int, byte> m_newValues = this._NewValuesList;
            return m_newValues;
        }

        /// <summary>
        /// Возвращает количество элементов в массиве
        /// </summary>
        public int GetArraySize()
        {
            return this._ArraySize;
        }

        /// <summary>
        /// Возвращает начальный адресс
        /// </summary>
        public int GetStartAddress()
        {
            return this._StartAddress;
        }

        /// <summary>
        /// Получение индекса элемента по положениею курсора в основном поле
        /// </summary>
        /// <param name="Start">положение курсора в основном поле</param>
        /// <returns>индекс элемента в массиве</returns>
        private int GetCodeIndex(int Start)
        {
            int m_SpaceInSegment = (this._SegmentSize - 1) / 8;
            int m_EnterCount = Start / (this._SegmentSize * (this._CodingType % 7 + 1) + 1 + m_SpaceInSegment);
            int m_SymdolsInCurentLine = (Start - m_EnterCount * (m_SpaceInSegment + 1 + this._SegmentSize * (this._CodingType % 7 + 1)));
            int m_SpaceInCurentSegment = m_SymdolsInCurentLine / ((this._CodingType % 7 + 1) * 8 + 1);
            int m_index = (Start - 1 - m_EnterCount * (m_SpaceInSegment + 1) - m_SpaceInCurentSegment) / (this._CodingType % 7 + 1);
            return m_index;
        }

        /// <summary>
        /// Получение полежения курсовра в основном поле по индексу элемента в массиве
        /// </summary>
        /// <param name="index">индекс элемента в массиве</param>
        /// <returns>положение курсора в основном поле</returns>
        private int GetCodeStart(int index)
        {
            int m_FullSegmentCount = index / this._SegmentSize;
            int m_SpaceInSegment = (this._SegmentSize - 1) / 8;
            int m_SpaceInCurentSegment = (index - m_FullSegmentCount * this._SegmentSize) / 8;
            int m_Start = index * (this._CodingType % 7 + 1) + ((m_FullSegmentCount) * (m_SpaceInSegment + 1) + m_SpaceInCurentSegment) + 1;
            return m_Start;
        }


        /// <summary>
        /// Предсавление десятичного числа в 16-чном формате(результат сторковый)
        /// </summary>
        /// <param name="Number">пребразуемое число(10-ичное)</param>
        /// <param name="NumberBit">число разрядов</param>
        /// <returns>полученное число (16-чное)</returns>
        private unsafe string TransformationDecToHex(int Number, int NumberBit)
        {
            char* res = stackalloc char[NumberBit + 1];
            int n = NumberBit * 4;
            for (int i = 0; i < NumberBit; i++)
            {
                uint t = (uint)(Number << 32 - (n - 4 * i)) >> 28;
                switch (t)
                {
                    case 0:
                        res[i] = '0';
                        break;
                    case 1:
                        res[i] = '1';
                        break;
                    case 2:
                        res[i] = '2';
                        break;
                    case 3:
                        res[i] = '3';
                        break;
                    case 4:
                        res[i] = '4';
                        break;
                    case 5:
                        res[i] = '5';
                        break;
                    case 6:
                        res[i] = '6';
                        break;
                    case 7:
                        res[i] = '7';
                        break;
                    case 8:
                        res[i] = '8';
                        break;
                    case 9:
                        res[i] = '9';
                        break;
                    case 10:
                        res[i] = 'a';
                        break;
                    case 11:
                        res[i] = 'b';
                        break;
                    case 12:
                        res[i] = 'c';
                        break;
                    case 13:
                        res[i] = 'd';
                        break;
                    case 14:
                        res[i] = 'e';
                        break;
                    case 15:
                        res[i] = 'f';
                        break;
                }
            }
            res[NumberBit + 1] = '\0';
            return new string(res);
        }

        /// <summary>
        /// Предсавление шестнадцотиричного числа в десятичном формате(результат целочисленный)
        /// </summary>
        /// <param name="Number">пробразуемое число(16-ичное)</param>
        /// <param name="NumberBit">число разрядов</param>
        /// <returns>полученное число (10-чное)</returns>
        private int TransformationHexToDec(string HexNum)
        {
            int m_n = 0;
            int m_l = HexNum.Length;
            int m_factor = 1;
            int m_s = 0;
            for (int i = m_l - 1; i >= 0; i--)
            {
                switch (HexNum[i])
                {
                    case '0':
                        m_s = 0;
                        break;
                    case '1':
                        m_s = 1;
                        break;
                    case '2':
                        m_s = 2;
                        break;
                    case '3':
                        m_s = 3;
                        break;
                    case '4':
                        m_s = 4;
                        break;
                    case '5':
                        m_s = 5;
                        break;
                    case '6':
                        m_s = 6;
                        break;
                    case '7':
                        m_s = 7;
                        break;
                    case '8':
                        m_s = 8;
                        break;
                    case '9':
                        m_s = 9;
                        break;
                    case 'a':
                    case 'A':
                        m_s = 10;
                        break;
                    case 'b':
                    case 'B':
                        m_s = 11;
                        break;
                    case 'c':
                    case 'C':
                        m_s = 12;
                        break;
                    case 'd':
                    case 'D':
                        m_s = 13;
                        break;
                    case 'e':
                    case 'E':
                        m_s = 14;
                        break;
                    case 'f':
                    case 'F':
                        m_s = 15;
                        break;
                    default:
                        m_s = 0;
                        break;
                }
                m_n = m_n + m_factor * m_s;
                m_factor = m_factor * 16;
            };
            return m_n;
        }

        /// <summary>
        /// Установка или получение текущей кодировки
        /// </summary>
        public int CodingType
        {
            get
            {
                return this._CodingType;
            }
            set
            {
                int m_index = GetCodeIndex(this.CodeBox.SelectionStart);
                if (value == 10)
                    this._CodingType = 10;
                else
                    this._CodingType = 16;
                if (this._ValuesArray != null)
                {
                    this.ShowAll();
                    this.ChangeNewValuesColor();
                }
                this._CurrentByte = -1;
                this._LastByte = -2;
                this._CurrentByteEnter = 0;
                ChangeLocation(m_index);
            }
        }


        /// <summary>
        /// Установка или получение цвета поля адреса
        /// </summary>
        public Color AddressColor
        {
            get
            {
                return this.AddressBox.ForeColor;
            }
            set
            {
                this.AddressBox.ForeColor = value;
            }
        }

        /// <summary>
        /// Установка или получение цвета основного поля
        /// </summary>
        public Color CodeColor
        {
            get
            {
                return this._CodeColor;
            }
            set
            {
                this._CodeColor = value;
                this.CodeBox.ForeColor = this._CodeColor;
            }
        }

        /// <summary>
        /// Установка или получение цвета поля предсатвления
        /// </summary>
        public Color PresentationColor
        {
            get
            {
                return this._PresentationColor;
            }
            set
            {
                this._PresentationColor = value;
                this.PresentationBox.ForeColor = this._PresentationColor;
            }
        }


        /// <summary>
        /// Установка или получение размера сегмента
        /// </summary>
        public int SegmentSize
        {
            get
            {
                return this._SegmentSize;
            }
            set
            {
                int m_index = GetCodeIndex(this.CodeBox.SelectionStart);
                this._Position = 0;
                this._PositionX = 0;
                this._SegmentSize = value;
                this._SegmentAmount = (this._ArraySize / this._SegmentSize) + 1;
                if (this._ValuesArray != null)
                {
                    ShowAll();
                }
                ChangeLocation(m_index);
            }
        }


        /// <summary>
        /// Замена бита в памяти и поле представления 
        /// </summary>
        /// <param name="b">новое значение бита</param>
        /// <param name="index">индекс изменяемого бита</param>
        private void ChangeByte(byte b, int index)
        {
            if (this.PresentationBox.Focused)
            {
                this.CodeBox.Focus();
            }
            this._CurrentLocationByte = index;
            if (this._NewValuesList.ContainsKey(index + this._StartAddress))
                this._NewValuesList[index + this._StartAddress] = b;
            else
                this._NewValuesList.Add(index + this._StartAddress, b);
            //генерация события
            if (this.ChangedByte != null)
                this.ChangedByte(this, new ByteChangeEventArgs(index + this._StartAddress, b));
            this.PresentationBox.SelectionStart = (index) + (index / (this._SegmentSize));
            this.PresentationBox.SelectionLength = 1;
            byte[] m_ByteString = new byte[2];
            if (b < 20) b = 32;
            m_ByteString[0] = b;
            m_ByteString[1] = b;
            int Start = this.PresentationBox.SelectionStart;
            this.PresentationBox.SelectedText = "" + Encoding.GetEncoding(866).GetString(m_ByteString)[0];
            ChangePosition();
        }

        /// <summary>
        /// Замена бита при шеснадцатиричной системе
        /// </summary>
        /// <param name="b">новое значение бита</param>
        /// <param name="index">индекс изменяемого бита</param>
        private void ChangeByte16(byte b, int index)
        {
            ChangeByte(b, index);
            if (this.CodeBox.Focused)
            {
                this.PresentationBox.Focus();
            }
            this.CodeBox.SelectionStart = GetCodeStart(index);
            this.CodeBox.SelectionLength = 2;
            this.CodeBox.SelectionColor = System.Drawing.Color.Brown;
            this.CodeBox.SelectedText = TransformationDecToHex(b, 2);
        }

        /// <summary>
        /// Замена бита  при десятичном системе
        /// </summary>
        /// <param name="b">новое значение бита</param>
        /// <param name="index">индекс изменяемого бита</param>
        private void ChangeByte10(byte b, int index)
        {
            ChangeByte(b, index);
            if (this.CodeBox.Focused)
            {
                this.PresentationBox.Focus();
            }
            string m_s = "" + b;
            while (m_s.Length < 3)
            {
                m_s = '0' + m_s;
            }
            this.CodeBox.SelectionStart = GetCodeStart(index);
            this.CodeBox.SelectionLength = 3;
            this.CodeBox.SelectionColor = System.Drawing.Color.Brown;
            this.CodeBox.SelectedText = m_s;
        }

        /// <summary>
        /// Когда основное поле становится не активным, то установка текущего байта
        /// редактирования=-1 и изменени положения ползунка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeaveFocus(object sender, EventArgs e)
        {
            int m_index = GetCodeIndex(this.CodeBox.SelectionStart);
            this._CurrentLocationByte = m_index;
            this.CodeBox.SelectionStart = GetCodeStart(m_index);
            this.PresentationBox.SelectionStart = m_index + (m_index / this._SegmentSize); ;
            this.AddressBox.SelectionStart = (m_index / this._SegmentSize + 1) * 9 - 1;
            ChangePosition();
            this._CurrentByte = -1;
            this._LastByte = -2;
        }

        /// <summary>
        /// При передачи активности полю адреса, возвращение ее основному полю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressBoxGotFocus(object sender, EventArgs e)
        {
            this.CodeBox.Focus();
            ChangePosition();
        }

        /// <summary>
        /// Изменение положения ползунка
        /// </summary>
        private void ChangePosition()
        {
            if ((this._CurrentLocationByte / this._SegmentSize + 1) * this._SymbolHeight >= this._Position + this.Size.Height - this._ScrollWidth - 4)
                this._Position = (this._CurrentLocationByte / this._SegmentSize + 1) * this._SymbolHeight - this.Size.Height + this._ScrollWidth + 4;
            if ((this._CurrentLocationByte / this._SegmentSize) * this._SymbolHeight <= this._Position)
                this._Position = (this._CurrentLocationByte / this._SegmentSize) * this._SymbolHeight;
            if (this._Position>this.MainScrollBar.Maximum) this._Position=this.MainScrollBar.Maximum;
            if (this._Position < this.MainScrollBar.Minimum) this._Position = this.MainScrollBar.Minimum;
            this.MainScrollBar.Value = this._Position;
            if (_CursorInCode)
            {
                if (((this._CurrentLocationByte % this._SegmentSize + 1) * this.CodeBox.Size.Width / this._SegmentSize + this._SymbolWidth + this.AddressBox.Size.Width) > (this._PositionX + this.Size.Width - this._ScrollWidth - 4))
                    this._PositionX = (this._CurrentLocationByte % this._SegmentSize + 1) * this.CodeBox.Size.Width / this._SegmentSize + this._SymbolWidth + this.AddressBox.Size.Width - this.Size.Width + this._ScrollWidth + 4;
                if (((this._CurrentLocationByte % this._SegmentSize - 1) * this.CodeBox.Size.Width / this._SegmentSize + this._SymbolWidth + this.AddressBox.Size.Width) < this._PositionX)
                    this._PositionX = (this._CurrentLocationByte % this._SegmentSize - 1) * this.CodeBox.Size.Width / this._SegmentSize + this._SymbolWidth + this.AddressBox.Size.Width;
            }
            else
            {
                if (((this._CurrentLocationByte % this._SegmentSize + 1) * this.PresentationBox.Size.Width / this._SegmentSize + this._SymbolWidth + this.AddressBox.Size.Width + this.CodeBox.Size.Width) > (this._PositionX + this.Size.Width - this._ScrollWidth - 4))
                    this._PositionX = ((this._CurrentLocationByte % this._SegmentSize + 1) * this.PresentationBox.Size.Width / this._SegmentSize + this._SymbolWidth + this.AddressBox.Size.Width + this.CodeBox.Size.Width) - this.Size.Width + this._ScrollWidth + 4;
                if (((this._CurrentLocationByte % this._SegmentSize - 1) * this.PresentationBox.Size.Width / this._SegmentSize + this._SymbolWidth + this.AddressBox.Size.Width + this.CodeBox.Size.Width) < this._PositionX)
                    this._PositionX = (this._CurrentLocationByte % this._SegmentSize - 1) * this.PresentationBox.Size.Width / this._SegmentSize + this._SymbolWidth + this.AddressBox.Size.Width;
            }
            if (this._PositionX > this.HorizontalScrollBar.Maximum) this._PositionX = this.HorizontalScrollBar.Maximum;
            if (this._PositionX < this.HorizontalScrollBar.Minimum) this._PositionX = this.HorizontalScrollBar.Minimum;
            this.HorizontalScrollBar.Value = this._PositionX;
            this.AddressBox.Location = new Point(- this.HorizontalScrollBar.Value, -this.MainScrollBar.Value);
            this.CodeBox.Location = new Point(this.AddressBox.Size.Width - this.HorizontalScrollBar.Value, -this.MainScrollBar.Value);
            this.PresentationBox.Location = new Point(this.AddressBox.Size.Width + this.CodeBox.Size.Width - this.HorizontalScrollBar.Value, -this.MainScrollBar.Value);
        }


        /// <summary>
        /// Перемещение курсора по основному полю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBoxKeyDown(object sender, KeyEventArgs e)
        {
            _CursorInCode = true;
            if ((e.KeyCode == Keys.Home) || (e.KeyCode == Keys.End) || (e.KeyCode == Keys.PageUp) || (e.KeyCode == Keys.PageDown) || (e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right))
            {
                int m_index = GetCodeIndex(this.CodeBox.SelectionStart);
                CursorMove(m_index, e);
                _CurrentByte = -1;
                _LastByte = -2;
                this.CodeBox.Focus();
                ChangePosition();
            }
        }

        /// <summary>
        /// Изменение положения курсора во всех тектсовых полях
        /// </summary>
        /// <param name="index">индекс нового активного бита</param>
        private void ChangeLocation(int index)
        {
            //Изменение положения курсора на нулевую строк для дальнейшего выравнивания текстовых полей
            if (index >= this._ArraySize)
            {
                this.PresentationBox.SelectionStart = 0;
                if (this.CodeBox.Focused)
                {
                    this.PresentationBox.Focus();
                }
                this.AddressBox.SelectionStart = 0;
                if (this.CodeBox.Focused || this.PresentationBox.Focused)
                {
                    this.AddressBox.Focus();
                }
                this.CodeBox.SelectionStart = 0;
                index = this._ArraySize - 1;
            }
            this._CurrentLocationByte = index;
            //изменение положения курсора на указанный байт
            this.PresentationBox.SelectionStart = index + (index / this._SegmentSize); ;
            this.PresentationBox.SelectionLength = 0;
            if (this.CodeBox.Focused)
            {
                this.PresentationBox.Focus();
            }
            this.AddressBox.SelectionStart = (index / this._SegmentSize + 1) * 9 - 1;
            this.AddressBox.SelectionLength = 0;
            if (this.CodeBox.Focused || this.PresentationBox.Focused)
            {
                this.AddressBox.Focus();
            }
            this.CodeBox.SelectionStart = GetCodeStart(index);
            this.CodeBox.SelectionLength = 0;
        }

        /// <summary>
        /// Изменение положения курсора на правильное при нажатии 1 раз левой
        /// клавиши мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CodeBoxMouseDown(object sender, MouseEventArgs e)
        {
            _CursorInCode = true;
            _CurrentByte = -1;
            this._LastByte = -2;
            int m_index = ((e.X / _SymbolWidth - (e.X / _SymbolWidth) / (8 * (_CodingType % 7 + 1)) - 1) / (_CodingType % 7 + 1)) + _SegmentSize * (e.Y / _SymbolHeight);
            ChangeLocation(m_index);
            this.CodeBox.Focus();
            ChangePosition();
        }

        /// <summary>
        /// Прокрутка ползунка при повороте колеса мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeMouseRotate(object sender, MouseEventArgs e)
        {
            this._Position = this._Position - e.Delta;
            if (this._Position > this.AddressBox.Size.Height - this.Size.Height)
                this._Position = this.AddressBox.Size.Height - this.Size.Height;
            if (this._Position < 0)
                this._Position = 0;
            this.MainScrollBar.Value = this._Position;
            this.AddressBox.Location = new Point(this.AddressBox.Location.X, -this.MainScrollBar.Value);
            this.CodeBox.Location = new Point(this.CodeBox.Location.X, -this.MainScrollBar.Value);
            this.PresentationBox.Location = new Point(this.PresentationBox.Location.X, -this.MainScrollBar.Value);
        }

        /// <summary>
        /// Выделение текущего бита информации при двойном щелчке мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBoxDoubleClick(object sender, EventArgs e)
        {
            _CursorInCode = true;
            int m_index = GetCodeIndex(this.CodeBox.SelectionStart);
            this.CodeBox.SelectionStart = GetCodeStart(m_index);
            this.CodeBox.SelectionLength = this._CodingType % 7;
        }

        /// <summary>
        /// Изменение положения курсора во всех полях на правильное 
        /// при нажатии 1 раз левой клавиши мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PresentationBoxMouseDown(object sender, MouseEventArgs e)
        {
            _CursorInCode = false;
            int m_index = ((e.X / _SymbolWidth)) + _SegmentSize * (e.Y / _SymbolHeight);
            ChangeLocation(m_index);
            this.PresentationBox.Focus();
            ChangePosition();
            _CurrentByte = -1;
            this._LastByte = -2;
        }

        /// <summary>
        /// Установка ползунка при получении полем представления или основным фокуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextGotFocus(object sender, EventArgs e)
        {
            ChangePosition();
        }

        /// <summary>
        /// Перемещение курсора по полю представления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PresentationBoxKeyDown(object sender, KeyEventArgs e)
        {
            _CursorInCode = false;
            if ((e.KeyCode == Keys.Home) || (e.KeyCode == Keys.End) || (e.KeyCode == Keys.PageUp) || (e.KeyCode == Keys.PageDown) || (e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right))
            {
                int m_index = (this.PresentationBox.SelectionStart - this.PresentationBox.SelectionStart / (this._SegmentSize + 1));
                CursorMove(m_index, e);
                this.PresentationBox.Focus();
                ChangePosition();
            }
        }


        /// <summary>
        /// Перемещение курсора в зависимости от индекса текущего бит и нажатой клавиши
        /// </summary>
        /// <param name="index"></param>
        /// <param name="e"></param>
        private void CursorMove(int index, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.PageDown:
                    index = index + this._SegmentSize * 10;
                    if (index >= this._ArraySize)
                        index = this._ArraySize - 1;
                    e.Handled = true;
                    break;
                case Keys.PageUp:
                    index = index - this._SegmentSize * 10;
                    if (index < 0)
                        index = 0;
                    e.Handled = true;
                    break;
                case Keys.End:
                    if (e.Control)
                        index = this._ArraySize - 1;
                    else
                        index = (index + this._SegmentSize) / this._SegmentSize * this._SegmentSize - 1;
                    e.Handled = true;
                    break;
                case Keys.Home:
                    if (e.Control)
                        index = 0;
                    else
                        index = index / this._SegmentSize * this._SegmentSize;
                    e.Handled = true;
                    break;
                case Keys.Up:
                    index = index - this._SegmentSize;
                    if (index < 0)
                        index = 0;
                    e.Handled = true;
                    break;
                case Keys.Down:
                    index = index + this._SegmentSize;
                    if (index >= this._ArraySize)
                        index = this._ArraySize - 1;
                    e.Handled = true;
                    break;
                case Keys.Left:
                    index = index - 1;
                    if (index < 0)
                        index = 0;
                    e.Handled = true;
                    break;
                case Keys.Right:
                    index = index + 1;
                    if (index >= this._ArraySize)
                        index = this._ArraySize - 1;
                    e.Handled = true;
                    break;
                default:
                    break;
            }
            ChangeLocation(index);
        }

        /// <summary>
        /// Реакция на нажатие клавиши при курсоре в основном окне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            int m_index = GetCodeIndex(this.CodeBox.SelectionStart);
            if (this._CodingType == 10)
            {
                //Изменение бита если десятичная система счисления
                switch (e.KeyChar)
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
                        this._LastByte = this._CurrentByte;
                        this._CurrentByte = m_index;
                        if (this._LastByte != this._CurrentByte)
                        {
                            this.ChangeByte10(Convert.ToByte("" + e.KeyChar), m_index);
                            this._CurrentByteEnter = 1;
                        }
                        else
                        {
                            //Если продолжает изменяться тот же байт
                            int m_ByteStart = GetCodeStart(m_index);
                            string m_Byte = "" + this.CodeBox.Text[m_ByteStart] + this.CodeBox.Text[m_ByteStart + 1] + this.CodeBox.Text[m_ByteStart + 2];
                            string m_s = "";
                            if ((this._CurrentByteEnter < 3) && (Convert.ToInt16(m_Byte + e.KeyChar) < 256))
                            {
                                m_s = m_Byte + e.KeyChar;
                                this.ChangeByte10(Convert.ToByte(m_s), m_index);
                                this._CurrentByteEnter = this._CurrentByteEnter + 1;
                            }
                            else
                            {
                                m_index++;
                                if (m_index < this._ArraySize)
                                {
                                    this._LastByte = this._CurrentByte;
                                    this._CurrentByte = m_index;
                                    this.ChangeByte10(Convert.ToByte("" + e.KeyChar), m_index);
                                    this._CurrentByteEnter = 1;
                                }
                                else { m_index--; }
                            }
                        }
                        this.CodeBox.Focus();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //Изменение бита если шестнадцатиричная система счисления
                switch (e.KeyChar)
                {
                    case 'ф':
                    case 'Ф':
                        e.KeyChar = 'a';
                        break;
                    case 'и':
                    case 'И':
                        e.KeyChar = 'b';
                        break;
                    case 'с':
                    case 'С':
                        e.KeyChar = 'c';
                        break;
                    case 'в':
                    case 'В':
                        e.KeyChar = 'd';
                        break;
                    case 'у':
                    case 'У':
                        e.KeyChar = 'e';
                        break;
                    case 'а':
                    case 'А':
                        e.KeyChar = 'f';
                        break;
                    default:
                        break;
                }
                switch (e.KeyChar)
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
                    case 'a':
                    case 'A':
                    case 'b':
                    case 'B':
                    case 'c':
                    case 'C':
                    case 'd':
                    case 'D':
                    case 'e':
                    case 'E':
                    case 'f':
                    case 'F':
                        this._LastByte = this._CurrentByte;
                        this._CurrentByte = m_index;
                        if (this._LastByte != this._CurrentByte)
                        {
                            this.ChangeByte16(Convert.ToByte(TransformationHexToDec("" + e.KeyChar)), m_index);
                            this._CurrentByteEnter = 1;
                        }
                        else
                        {
                            //Если продолжает изменяться тот же байт
                            int m_ByteStart = GetCodeStart(m_index);
                            string m_Byte = "" + this.CodeBox.Text[m_ByteStart] + this.CodeBox.Text[m_ByteStart + 1];
                            if ((this._CurrentByteEnter < 2) && (Convert.ToInt16(TransformationHexToDec(m_Byte + e.KeyChar)) < 256))
                            {
                                m_Byte = m_Byte + e.KeyChar;
                                this.ChangeByte16(Convert.ToByte(TransformationHexToDec(m_Byte)), m_index);
                                this._CurrentByteEnter = this._CurrentByteEnter + 1;
                            }
                            else
                            {
                                m_index++;
                                if (m_index < this._ArraySize)
                                {
                                    this._LastByte = this._CurrentByte;
                                    this._CurrentByte = m_index;
                                    this.ChangeByte16(Convert.ToByte(TransformationHexToDec("" + e.KeyChar)), m_index);
                                    this._CurrentByteEnter = 1;
                                }
                                else { m_index--; }
                            }
                        }
                        if (this.PresentationBox.Focused)
                        {
                            this.CodeBox.Focus();
                        }
                        break;
                    default:
                        break;
                }
            }
            e.Handled = true;
        }



        /// <summary>
        /// Смена значения байта если меняется тектст в поле представления(нажатиа клавиша)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PresentationBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            int m_index;
            m_index = (this.PresentationBox.SelectionStart - this.PresentationBox.SelectionStart / (this._SegmentSize + 1));
            try
            {
                if (m_index < this._ArraySize)
                {
                    string b = "" + e.KeyChar + e.KeyChar;
                    byte[] BB = Encoding.GetEncoding(866).GetBytes(b);
                    if (this._CodingType == 10)
                        ChangeByte10(BB[0], m_index);
                    else
                        ChangeByte16(BB[0], m_index);
                }
                if (m_index < this._ArraySize)
                { m_index = m_index + 1; }
                ChangeLocation(m_index);
                this.PresentationBox.Focus();
            }
            catch
            {
            }
            e.Handled = true;
        }

        /// <summary>
        /// Изменение позиции при прокрутке ползунка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this._Position = this.MainScrollBar.Value;
            this.AddressBox.Location = new Point(this.AddressBox.Location.X, -this.MainScrollBar.Value);
            this.CodeBox.Location = new Point(this.CodeBox.Location.X, -this.MainScrollBar.Value);
            this.PresentationBox.Location = new Point(this.PresentationBox.Location.X, -this.MainScrollBar.Value);
        }

        private void HexEditior_Enter(object sender, EventArgs e)
        {
            ChangeLocation(_CurrentLocationByte);
        }

        private void HorizontalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this._PositionX = this.HorizontalScrollBar.Value;
            this.AddressBox.Location = new Point(-this.HorizontalScrollBar.Value, this.AddressBox.Location.Y);
            this.CodeBox.Location = new Point(this.AddressBox.Size.Width - this.HorizontalScrollBar.Value, this.CodeBox.Location.Y);
            this.PresentationBox.Location = new Point(this.AddressBox.Size.Width + this.CodeBox.Size.Width - this.HorizontalScrollBar.Value, this.PresentationBox.Location.Y);
        }

        private void HexEditior_ClientSizeChanged(object sender, EventArgs e)
        {
            this.MainScrollBar.Size = new System.Drawing.Size(this._ScrollWidth, this.Size.Height);
            this.HorizontalScrollBar.Size = new System.Drawing.Size(this.Size.Width, this._ScrollWidth);
            this.MainScrollBar.Maximum = this._SymbolHeight * (this._SegmentAmount + 1) + 2 - this.Size.Height + this._ScrollWidth;
            this.MainScrollBar.Minimum = 0;
            this.HorizontalScrollBar.Maximum = this.AddressBox.Size.Width + this.CodeBox.Size.Width + this.PresentationBox.Size.Width + this._ScrollWidth + 10 - this.Size.Width;
            this.HorizontalScrollBar.Minimum = 0;
            if (this.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.MainScrollBar.Size = new System.Drawing.Size(this._ScrollWidth, this.Size.Height - 4);
                this.HorizontalScrollBar.Size = new System.Drawing.Size(this.Size.Width - 4, this._ScrollWidth);
            }
            if (this.Size.Height <= this._SymbolHeight * (this._SegmentAmount + 1) + 2)
                this.MainScrollBar.Visible = true;
            else
            {
                this.MainScrollBar.Visible = false;
                this.AddressBox.Location = new System.Drawing.Point(this.AddressBox.Location.X, 0);
                this.CodeBox.Location = new System.Drawing.Point(this.CodeBox.Location.X, 0);
                this.PresentationBox.Location = new System.Drawing.Point(this.PresentationBox.Location.X, 0);
            }
            if (this.Size.Width <= (this.AddressBox.Size.Width + this.CodeBox.Size.Width + this.PresentationBox.Size.Width + this._ScrollWidth))
                this.HorizontalScrollBar.Visible = true;
            else
            {
                this.HorizontalScrollBar.Visible = false;
                this.AddressBox.Location = new System.Drawing.Point(0, this.AddressBox.Location.Y);
                this.CodeBox.Location = new System.Drawing.Point(this.AddressBox.Size.Width, this.CodeBox.Location.Y);
                this.PresentationBox.Location = new System.Drawing.Point(this.AddressBox.Size.Width + this.CodeBox.Size.Width, this.PresentationBox.Location.Y);
            }
        }

    }
}
