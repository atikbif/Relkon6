using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon.Solutions;
using Kontel.Relkon;

namespace Kontel.Relkon.Components.Documents
{
    public sealed partial class DigitalIO : UserControl
    {
        private System.Drawing.Size _sensorSize = new System.Drawing.Size(90, 22);//Размер одного датчика
        private System.Drawing.Size _lampSize = new System.Drawing.Size(15, 16);//Размер лампочки
        private int _interval = 30;//Интервал между цифровыми входами и выходами
        private int _intervalX = 11;//Ширина интервала между блоками датчиков
        private int _intervalY = 20;//Высота интервала между блоками датичков
        private Bitmap _bitmap = new Bitmap(Kontel.Relkon.Properties.Resources.lamp_green);//Область отображения
        private Sensor _curentSensor;//Датчик на котором редактируется метка
        private bool _mouseClick = true;//Разрешена ли смена состояния датчиков
        //private ProcessorType _typeProcessor;//Тип процессора для которого создан компонент
        private Color _componentColor;
        private Color _backColor;

        //Изменение интервала между входами и выходами
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        //Поле отражающее возможность смены состнияния датчиков
        public bool EnabledMouseClick
        {
            get { return _mouseClick; }
            set { _mouseClick = value; }
        }
        /// <summary>
        /// Список блоков, отражающих стостояния входов
        /// </summary>
        private SortedList<int, Variable> _inVars = new SortedList<int, Variable>();
        public SortedList<int, Variable> InVars
        {
            get { return _inVars; }
        }
        /// <summary>
        /// Список блоков, отражающих стостояния выходов
        /// </summary>
        private SortedList<int, Variable> _outVars = new SortedList<int, Variable>();
        public SortedList<int, Variable> OutVars
        {
            get { return _outVars; }
        }

        /// <summary>
        /// Показывать стандартыне датчики
        /// </summary>
        //private bool _displayDefault = true;
        //public bool DisplayDefaultSensors
        //{
        //    get { return _displayDefault; }
        //    set
        //    {
        //        IList<int> m_key = InVars.Keys;
        //        for (int i = 0; i < m_key.Count; i++)
        //        {
        //            if (_displayDefault && !value)
        //                InVars[m_key[i]].Location = new Point(InVars[m_key[i]].Location.X, InVars[m_key[i]].Location.Y - BlokSize().Y - 2);
        //            if (!_displayDefault && value)
        //                InVars[m_key[i]].Location = new Point(InVars[m_key[i]].Location.X, InVars[m_key[i]].Location.Y + BlokSize().Y + 2);
        //        }
        //        m_key = OutVars.Keys;
        //        for (int i = 0; i < m_key.Count; i++)
        //        {
        //            if (_displayDefault && !value)
        //                OutVars[m_key[i]].Location = new Point(OutVars[m_key[i]].Location.X, OutVars[m_key[i]].Location.Y - BlokSize().Y - 2);
        //            if (!_displayDefault && value)
        //                OutVars[m_key[i]].Location = new Point(OutVars[m_key[i]].Location.X, OutVars[m_key[i]].Location.Y + BlokSize().Y + 2);
        //        }
        //        int m_x = _bitmap.Size.Width;
        //        int m_y = _bitmap.Size.Height;
        //        if (_displayDefault && !value)
        //            _bitmap = new Bitmap(m_x, (m_y - this.BlokSize().Y < 0) ? 30 : m_y - this.BlokSize().Y);
        //        if (!_displayDefault && value)
        //            _bitmap = new Bitmap(m_x, (m_y ==30) ? this.BlokSize().Y+2*_intervalY : m_y + this.BlokSize().Y);
        //        _displayDefault = value;
        //        DrawConponents();
        //        //Обновление картинки не дожидаясь OnPaint
        //        using (Graphics g = this.CreateGraphics())
        //        {
        //            g.DrawImage(_bitmap, 0, 0);
        //        }
        //    }
        //}


        /// <summary>
        /// Класс определяющий датчик
        /// </summary>
        public class Sensor
        {
            public bool IsInput;//Датчик входа
            public int Key;//Номер блока(переменной IN)
            public int Index;//Номер дотчика в блоке

            public Sensor() { }
            public Sensor(bool isInput, int key, int index)
            {
                IsInput = isInput;
                Key = key;
                Index = index;
            }
        }

        /// <summary>
        /// Класс для блока дотчиков
        /// </summary>
        public class Variable
        {
            private String _name = "";//Имя переменной
            public string Name
            {
                get { return _name; }
            }
            private System.Drawing.Point _location = new System.Drawing.Point(0, 0);//Положение болка на форме
            public System.Drawing.Point Location
            {
                get { return _location; }
                set { _location = value; }
            }
            private Int16 _sensorNumber = 8;//Количество датчиков в этой переменной
            public short SensorNumber
            {
                get { return _sensorNumber; }
            }
            private Byte _primaryValue = 0;//Значение переменной 
            public byte PrimaryValue
            {
                get { return _primaryValue; }
                set { _primaryValue = value; }
            }
            private string[] _stringCollection;//Массив меток
            public String[] StringCollection
            {
                get { return _stringCollection; }
                set { _stringCollection = value; }
            }
            private List<int> _writeSensors;//Список записываемых датчиков
            public List<int> WriteSensors
            {
                get { return _writeSensors; }
                set { _writeSensors = value; }
            }

            public Variable() { _writeSensors = new List<int>(); }

            public Variable(string name, System.Drawing.Point location, short sensorNumber, byte primaryValue, /*bool isInput,*/ string[] stringCollection)
            {
                _name = name;
                _location = new System.Drawing.Point(location.X, location.Y);
                _sensorNumber = sensorNumber;
                _primaryValue = primaryValue;
                _stringCollection = stringCollection;
                _writeSensors = new List<int>();
            }
        }

        /// <summary>
        /// Событие смены метки одного датчика
        /// </summary>
        public event EventHandler<LabelChangeEventArgs> LabelChange;

        private void OnLabelChange(LabelChangeEventArgs e)
        {
            EventHandler<LabelChangeEventArgs> temp = LabelChange;
            if (temp != null) temp(this, e);
        }

        public sealed class LabelChangeEventArgs : EventArgs
        {
            private readonly string text;//Новое знчение метки
            public string Text
            {
                get { return text; }
            }
            private readonly bool isInput;//Датчик входа?
            public bool IsInput
            {
                get { return isInput; }
            }
            private readonly int key;//Номер блока
            public int Key
            {
                get { return key; }
            }
            private readonly int index;//Номер датчика в блоке
            public int Index
            {
                get { return index; }
            }

            public LabelChangeEventArgs(string p_text, bool p_isInput, int p_key, int p_index)
            {
                text = p_text;
                isInput = p_isInput;
                key = p_key;
                index = p_index;
            }
        }

        /// <summary>
        /// Событие попытки смены состояния одного датчика
        /// срабатывает при однократном щелчке левой кнопкой мышки по датчику
        /// </summary>
        public event EventHandler<StateChangeEventArgs> StateChange;
        private void OnStateChange(StateChangeEventArgs e)
        {
            EventHandler<StateChangeEventArgs> temp = StateChange;
            if (temp != null) temp(this, e);
        }
        public sealed class StateChangeEventArgs : EventArgs
        {
            private readonly byte new_value;//Новое знчение переменной отражающей состояние блока
            public byte New_value
            {
                get { return new_value; }
            }
            private readonly bool isInput;//Датчик входа?
            public bool IsInput
            {
                get { return isInput; }
            }
            private readonly int key;//Номер блока
            public int Key
            {
                get { return key; }
            }
            private readonly int index;//Номер блока
            public int Index
            {
                get { return index; }
            }

            public StateChangeEventArgs(byte p_new_value, bool p_isInput, int p_key, int p_index)
            {
                new_value = p_new_value;
                isInput = p_isInput;
                key = p_key;
                index = p_index;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DigitalIO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Отрисовка
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(_bitmap, 0, 0);
        }


        /// <summary>
        /// Формирование пустого окна цифровых датчиков
        /// </summary>
        public void NewComponents(string Message)
        {
            //Очистка переменных
            _inVars.Clear();
            _outVars.Clear();
            int m_width;
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                m_width = (int)g.MeasureString(Message, this.Font).Width;
            }
            _bitmap = new Bitmap(m_width + 30, 30);
            this.Size = new System.Drawing.Size(_bitmap.Size.Width, _bitmap.Size.Height);
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Size.Width, this.Size.Height);
                g.DrawRectangle(new Pen(System.Drawing.Color.Blue), 0, 0, this.Size.Width - 1, this.Size.Height - 1);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawString(Message, this.Font, new SolidBrush(Color.Black), 5, 5);
            }
            //Обновление картинки не дожидаясь OnPaint
            using (Graphics g = this.CreateGraphics())
            {
                g.DrawImage(_bitmap, 0, 0);
            }
        }

        /// <summary>
        /// Формирование новых данных на основе списка входных и выходных переменных
        /// </summary>
        /// <param name="IN">Список номеров входных переменных</param>
        /// <param name="OUT">Список номеров выходных переменных</param>
        public void NewComponents(IList<int> IN, IList<int> OUT, Color back, Color component)
        {
         
            _backColor = back;
            _componentColor = component;
            //Очистка переменных
            _inVars.Clear();
            _outVars.Clear();
            //Заполнение входов
            int x = _intervalX;
            int y = _intervalY;
            for (int i = 0; i < IN.Count; i++)
            {
                if (_inVars.ContainsKey(IN[i]))
                    _inVars[IN[i]] = new Variable("IN" + IN[i], new Point(x, y), 8, (Byte)0, new String[8]);
                else _inVars.Add(IN[i], new Variable("IN" + IN[i], new Point(x, y), 8, (Byte)0, new String[8]));
                x = (x + _sensorSize.Width + _intervalX) % ((_sensorSize.Width + _intervalX) * 4);
                if (x == _intervalX) y = y + _sensorSize.Height * 8 + _intervalY;
            }
            //Заполнение выходов
            int m_start = (_sensorSize.Width + _intervalX) * 4 + _interval;
            x = m_start;
            y = _intervalY;
            for (int i = 0; i < OUT.Count; i++)
            {
                if (_outVars.ContainsKey(OUT[i]))
                    _outVars[OUT[i]] = new Variable("OUT" + OUT[i], new Point(x, y), 8, (Byte)0, new String[8]);
                else _outVars.Add(OUT[i], new Variable("OUT" + OUT[i], new Point(x, y), 8, (Byte)0, new String[8]));
                x = m_start + (x - m_start + (_sensorSize.Width + _intervalX)) % ((_sensorSize.Width + _intervalX) * 4);
                if (x == m_start) y = y + _sensorSize.Height * 8 + _intervalY;
            }
            //_typeProcessor = typeProcessor;
            _bitmap = new Bitmap((_sensorSize.Width + _intervalX) * 8 + _interval + _intervalX, (_sensorSize.Height) * (8 * (((Math.Max(_inVars.Count, _outVars.Count) - 1) / 4) + 1)) + _intervalY * ((Math.Max(_inVars.Count, _outVars.Count) - 1) / 4 + 2));
            DrawConponents();
        }

        /// <summary>
        /// Формирование новых данных на основе списка входных и выходных переменных
        /// </summary>
        /// <param name="IN">Список номеров входных переменных</param>
        /// <param name="OUT">Список номеров выходных переменных</param>
        public void RefreshComponents(IList<int> IN, IList<int> OUT, Color back, Color component)
        {
            _backColor = back;
            _componentColor = component;        
            //Дополнение входов
            int x = _intervalX;
            int y = _intervalY;
            int bitmapHeight = 30;
            SortedList<int, Variable> inVars = _inVars!=null?_inVars:new SortedList<int, Variable>();
            _inVars=new SortedList<int,Variable>();
            for (int i = 0; i < IN.Count; i++)
            {
                if (y + _sensorSize.Height * 8 + _intervalY > bitmapHeight)
                    bitmapHeight = y + _sensorSize.Height * 8 + _intervalY;
                Variable v = new Variable();
                if (inVars.ContainsKey(IN[i]))
                {
                    v = inVars[IN[i]];
                    v.Location = new Point(x, y);
                }
                else
                    v = new Variable("IN" + IN[i], new Point(x, y), 8, (Byte)0, new String[8]);
                _inVars.Add(IN[i], v);
                x = (x + _sensorSize.Width + _intervalX) % ((_sensorSize.Width + _intervalX) * 4);
                if (x == _intervalX)
                {
                    y = y + _sensorSize.Height * 8 + _intervalY;
                }
            }
            //Дополнение выходов
            int m_start = (_sensorSize.Width + _intervalX) * 4 + _interval;
            x = m_start;
            y = _intervalY;
            SortedList<int, Variable> outVars = _outVars != null ? _outVars : new SortedList<int, Variable>();
            _outVars = new SortedList<int, Variable>();
            for (int i = 0; i < OUT.Count; i++)
            {
                if (y + _sensorSize.Height * (OUT[i] < 4 ? (Int16)8 : (Int16)4) + _intervalY > bitmapHeight)
                    bitmapHeight = y + _sensorSize.Height * (OUT[i] < 4 ? (Int16)8 : (Int16)4) + _intervalY;
                Variable v = new Variable();
                if (outVars.ContainsKey(OUT[i]))
                {
                    v = outVars[OUT[i]];
                    v.Location = new Point(x, y);
                }
                else
                    v = new Variable("OUT" + OUT[i], new Point(x, y), OUT[i] < 4 ? (Int16)8 : (Int16)4, (Byte)0, new String[OUT[i] < 4 ? (Int16)8 : (Int16)4]);
                _outVars.Add(OUT[i], v);
                x = m_start + (x - m_start + (_sensorSize.Width + _intervalX)) % ((_sensorSize.Width + _intervalX) * 4);
                if (x == m_start)
                {
                    y = y + _sensorSize.Height * (OUT[i] < 4 ? (Int16)8 : (Int16)4) + _intervalY;
                }
            }

            _bitmap = new Bitmap((_sensorSize.Width + _intervalX) * 8 + _interval + _intervalX, bitmapHeight);
            DrawConponents();
        }

        /// <summary>
        /// Отрисовка компонентов
        /// </summary>
        private void DrawConponents()
        {
            IList<int> IN = _inVars.Keys;
            IList<int> OUT = _outVars.Keys;
            this.Size = new System.Drawing.Size(_bitmap.Size.Width, _bitmap.Size.Height);
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                    
                g.FillRectangle(new SolidBrush(this._componentColor), 0, 0, this.Size.Width, this.Size.Height);
                g.FillRectangle(new SolidBrush(this._backColor), _bitmap.Width / 2 - _interval / 6, 0, _interval / 3, this.Size.Height);
                g.DrawRectangle(new Pen(System.Drawing.Color.Blue), 0, 0, this.Size.Width / 2 - _interval / 6 - 1, this.Size.Height - 1);
                g.DrawRectangle(new Pen(System.Drawing.Color.Blue), this.Size.Width / 2 + _interval / 6, 0, this.Size.Width / 2 - _interval / 6, this.Size.Height - 1);
                    
                Boolean[] m_value;
                //Отрисовка входов
                for (int i = 0; i < _inVars.Count; i++)
                {                    
                    g.DrawRectangle(new Pen(System.Drawing.Color.White), _inVars[IN[i]].Location.X + 1, _inVars[IN[i]].Location.Y + (_sensorSize.Height - _lampSize.Height) - 3 - g.MeasureString("T", this.Font).Height / 2 + 1, _sensorSize.Width + 3, _sensorSize.Height * _inVars[IN[i]].SensorNumber - (_sensorSize.Height - _lampSize.Height) + g.MeasureString("T", this.Font).Height / 2 + 3);
                    g.DrawRectangle(new Pen(System.Drawing.Color.FromArgb(0, 225, 0)), _inVars[IN[i]].Location.X, _inVars[IN[i]].Location.Y + (_sensorSize.Height - _lampSize.Height) - 3 - g.MeasureString("T", this.Font).Height / 2, _sensorSize.Width + 3, _sensorSize.Height * _inVars[IN[i]].SensorNumber - (_sensorSize.Height - _lampSize.Height) + g.MeasureString("T", this.Font).Height / 2 + 3);
                    g.FillRectangle(new SolidBrush(this._componentColor), _inVars[IN[i]].Location.X + 5, _inVars[IN[i]].Location.Y + (_sensorSize.Height - _lampSize.Height) - 4 - g.MeasureString("T", this.Font).Height, g.MeasureString(this._inVars[IN[i]].Name, this.Font).Width + 2, g.MeasureString("T", this.Font).Height + 2);
                    g.DrawString(_inVars[IN[i]].Name, this.Font, new SolidBrush(Color.Black), _inVars[IN[i]].Location.X + 6, _inVars[IN[i]].Location.Y + (_sensorSize.Height - _lampSize.Height) - 3 - g.MeasureString("T", this.Font).Height);
                    m_value = FromByteToBool(_inVars[IN[i]].PrimaryValue);
                    for (int j = 0; j < _inVars[IN[i]].SensorNumber; j++)
                    {
                        g.DrawString(_inVars[IN[i]].StringCollection[j], this.Font, new SolidBrush(Color.Black), _inVars[IN[i]].Location.X + _sensorSize.Width - _lampSize.Width - g.MeasureString(_inVars[IN[i]].StringCollection[j], this.Font).Width, (j + 1) * _sensorSize.Height + _inVars[IN[i]].Location.Y - _lampSize.Height + 2);
                        switch (m_value[j])
                        {
                            case false: g.DrawImage(Kontel.Relkon.Properties.Resources.lamp_gray, _inVars[IN[i]].Location.X + _sensorSize.Width - _lampSize.Width, (j + 1) * _sensorSize.Height + _inVars[IN[i]].Location.Y - _lampSize.Height, _lampSize.Width, _lampSize.Height); break;
                            case true: g.DrawImage(Kontel.Relkon.Properties.Resources.lamp_green, _inVars[IN[i]].Location.X + _sensorSize.Width - _lampSize.Width, (j + 1) * _sensorSize.Height + _inVars[IN[i]].Location.Y - _lampSize.Height, _lampSize.Width, _lampSize.Height); break;
                        }
                    }
                }
                //Отрисовка выходов
                for (int i = 0; i < _outVars.Count; i++)
                {                    
                    g.DrawRectangle(new Pen(System.Drawing.Color.White), _outVars[OUT[i]].Location.X + 1, _outVars[OUT[i]].Location.Y + (_sensorSize.Height - _lampSize.Height) - 3 - g.MeasureString("T", this.Font).Height / 2 + 1, _sensorSize.Width + 3, _sensorSize.Height * _outVars[OUT[i]].SensorNumber - (_sensorSize.Height - _lampSize.Height) + g.MeasureString("T", this.Font).Height / 2 + 3);
                    g.DrawRectangle(new Pen(System.Drawing.Color.FromArgb(235, 0, 0)), _outVars[OUT[i]].Location.X, _outVars[OUT[i]].Location.Y + (_sensorSize.Height - _lampSize.Height) - 3 - g.MeasureString("T", this.Font).Height / 2, _sensorSize.Width + 3, _sensorSize.Height * _outVars[OUT[i]].SensorNumber - (_sensorSize.Height - _lampSize.Height) + g.MeasureString("T", this.Font).Height / 2 + 3);
                    g.FillRectangle(new SolidBrush(this._componentColor), _outVars[OUT[i]].Location.X + 5, _outVars[OUT[i]].Location.Y + (_sensorSize.Height - _lampSize.Height) - 4 - g.MeasureString("T", this.Font).Height, g.MeasureString(this._outVars[OUT[i]].Name, this.Font).Width + 2, g.MeasureString("T", this.Font).Height + 2);
                    g.DrawString(_outVars[OUT[i]].Name, this.Font, new SolidBrush(Color.Black), _outVars[OUT[i]].Location.X + 6, _outVars[OUT[i]].Location.Y + (_sensorSize.Height - _lampSize.Height) - 3 - g.MeasureString("T", this.Font).Height);
                    m_value = FromByteToBool(_outVars[OUT[i]].PrimaryValue);
                    for (int j = 0; j < _outVars[OUT[i]].SensorNumber; j++)
                    {
                        g.DrawString(_outVars[OUT[i]].StringCollection[j], this.Font, new SolidBrush(Color.Black), _outVars[OUT[i]].Location.X + _sensorSize.Width - _lampSize.Width - g.MeasureString(_outVars[OUT[i]].StringCollection[j], this.Font).Width, (j + 1) * _sensorSize.Height + _outVars[OUT[i]].Location.Y - _lampSize.Height + 2);
                        switch (m_value[j])
                        {
                            case false: g.DrawImage(Kontel.Relkon.Properties.Resources.lamp_gray, _outVars[OUT[i]].Location.X + _sensorSize.Width - _lampSize.Width, (j + 1) * _sensorSize.Height + _outVars[OUT[i]].Location.Y - _lampSize.Height, _lampSize.Width, _lampSize.Height); break;
                            case true: g.DrawImage(Kontel.Relkon.Properties.Resources.lamp_Red, _outVars[OUT[i]].Location.X + _sensorSize.Width - _lampSize.Width, (j + 1) * _sensorSize.Height + _outVars[OUT[i]].Location.Y - _lampSize.Height, _lampSize.Width, _lampSize.Height); break;
                        }
                    }
                }
            }
            //Обновление картинки не дожидаясь OnPaint
            using (Graphics g = this.CreateGraphics())
            {
                g.DrawImage(_bitmap, 0, 0);
            }
        }

        private Point BlokSize()
        {
            return new System.Drawing.Point(this._sensorSize.Width, this._sensorSize.Height * 8 + this._intervalY);
        }

        /// <summary>
        /// Преобразование знчения типа byte в массив логических переменных
        /// </summary>
        /// <param name="buf">Входное значение</param>
        /// <returns>Логический массив</returns>
        private bool[] FromByteToBool(byte buf)
        {
            bool[] result = new bool[8];
            result[0] = Convert.ToBoolean(buf & 0x01);
            result[1] = Convert.ToBoolean((buf & 0x02) >> 1);
            result[2] = Convert.ToBoolean((buf & 0x04) >> 2);
            result[3] = Convert.ToBoolean((buf & 0x08) >> 3);
            result[4] = Convert.ToBoolean((buf & 0x10) >> 4);
            result[5] = Convert.ToBoolean((buf & 0x20) >> 5);
            result[6] = Convert.ToBoolean((buf & 0x40) >> 6);
            result[7] = Convert.ToBoolean((buf & 0x80) >> 7);
            return result;
        }
        /// <summary>
        /// Пробразование логического массива в значений типа byte
        /// </summary>
        /// <param name="buf">Логический массив</param>
        /// <returns>значений типа byte</returns>
        private byte FromBoolToByte(bool[] buf)
        {
            byte result;
            if (buf.Length != 8) return 0;
            byte[] buf2 = new byte[8];
            for (int i = 0; i < 8; i++)
                if (buf[i] == true) buf2[i] = 0xFF;
                else buf2[i] = 0x00;
            result = (byte)(buf2[0] & 0x01 | buf2[1] & 0x02 | buf2[2] & 0x04 | buf2[3] & 0x08 | buf2[4] & 0x10 | buf2[5] & 0x20 | buf2[6] & 0x40 | buf2[7] & 0x80);
            return result;
        }

        /// <summary>
        /// Смена цвета лампочек при щелчке мышки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DigitalIO_MouseClick(object sender, MouseEventArgs e)
        {
            if (_mouseClick)
            {
                if (e.X > _bitmap.Width || e.Y > _bitmap.Height) return;
                IList<int> m_key = new List<int>();
                m_key = _inVars.Keys;
                for (int i = 0; i < m_key.Count; i++)
                {
                    for (int j = 0; j < _inVars[m_key[i]].SensorNumber; j++)
                    {
                        //Определени на какую лампочку нажали по координатам
                        if (_inVars[m_key[i]].Location.X + _sensorSize.Width - _lampSize.Width < e.X &&
                            _inVars[m_key[i]].Location.Y + j * _sensorSize.Height < e.Y &&
                            _inVars[m_key[i]].Location.X + _sensorSize.Width > e.X &&
                            _inVars[m_key[i]].Location.Y + (j + 1) * _sensorSize.Height > e.Y)
                        {
                            bool[] m_value = FromByteToBool(_inVars[m_key[i]].PrimaryValue);//преобразование значенй дактчиков в логический массив
                            m_value[j] = !m_value[j];//Изменение значение датчика на противоложенный
                            ChangeStatePicture(true, m_key[i], j, m_value[j]);//изменение цвета лампочки
                            _inVars[m_key[i]].WriteSensors.Add(j);
                            _inVars[m_key[i]].PrimaryValue = FromBoolToByte(m_value);//запись измененног значения в однобайтную переменную
                            OnStateChange(new StateChangeEventArgs(_inVars[m_key[i]].PrimaryValue, true, m_key[i], j));//генерация события измнения значения датчика
                            return;
                        }
                    }
                }
                m_key = _outVars.Keys;
                for (int i = 0; i < m_key.Count; i++)
                {
                    for (int j = 0; j < _outVars[m_key[i]].SensorNumber; j++)
                    {
                        if (_outVars[m_key[i]].Location.X + _sensorSize.Width - _lampSize.Width < e.X &&
                            _outVars[m_key[i]].Location.Y + j * _sensorSize.Height < e.Y &&
                            _outVars[m_key[i]].Location.X + _sensorSize.Width > e.X &&
                            _outVars[m_key[i]].Location.Y + (j + 1) * _sensorSize.Height > e.Y)
                        {
                            bool[] m_value = FromByteToBool(_outVars[m_key[i]].PrimaryValue);
                            m_value[j] = !m_value[j];
                            _outVars[m_key[i]].WriteSensors.Add(j);
                            ChangeStatePicture(false, m_key[i], j, m_value[j]);
                            _outVars[m_key[i]].PrimaryValue = FromBoolToByte(m_value);
                            OnStateChange(new StateChangeEventArgs(_outVars[m_key[i]].PrimaryValue, false, m_key[i], j));
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Смена картинки на одном датчике
        /// </summary>
        /// <param name="isInput">входной ли датчик</param>
        /// <param name="key">номер меняемой переменной</param>
        /// <param name="index">номер датчика в меняемой переменной</param>
        /// <param name="value">новое зачение датчика</param>
        private void ChangeStatePicture(bool isInput, int key, int index, bool value)
        {
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                if (value)
                {
                    if (isInput)
                        g.DrawImage(Kontel.Relkon.Properties.Resources.lamp_green, _inVars[key].Location.X + _sensorSize.Width - _lampSize.Width, (index + 1) * _sensorSize.Height + _inVars[key].Location.Y - _lampSize.Height, _lampSize.Width, _lampSize.Height);
                    else
                        g.DrawImage(Kontel.Relkon.Properties.Resources.lamp_Red, _outVars[key].Location.X + _sensorSize.Width - _lampSize.Width, (index + 1) * _sensorSize.Height + _outVars[key].Location.Y - _lampSize.Height, _lampSize.Width, _lampSize.Height);
                }
                else
                {
                    if (isInput)
                        g.DrawImage(Kontel.Relkon.Properties.Resources.lamp_gray, _inVars[key].Location.X + _sensorSize.Width - _lampSize.Width, (index + 1) * _sensorSize.Height + _inVars[key].Location.Y - _lampSize.Height, _lampSize.Width, _lampSize.Height);
                    else
                        g.DrawImage(Kontel.Relkon.Properties.Resources.lamp_gray, _outVars[key].Location.X + _sensorSize.Width - _lampSize.Width, (index + 1) * _sensorSize.Height + _outVars[key].Location.Y - _lampSize.Height, _lampSize.Width, _lampSize.Height);
                }
            }
            //Обновление картинки не дожидаясь OnPaint
            using (Graphics g = this.CreateGraphics())
            {
                g.DrawImage(_bitmap, 0, 0);
            }
        }


        /// <summary>
        /// Смена метки на одном датчике
        /// </summary>
        /// <param name="isInput">входной ли датчик</param>
        /// <param name="key">номер меняемой переменной</param>
        /// <param name="index">номер датчика в меняемой меткой</param>
        /// <param name="value">новое зачение vtnrb</param>
        public void ChangeLabel(bool isInput, int key, int index, String value)
        {
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                if (isInput)
                {
                    _inVars[key].StringCollection[index] = value;
                    g.FillRectangle(new SolidBrush(this._componentColor), _inVars[key].Location.X + 2, index * _sensorSize.Height + _inVars[key].Location.Y, _sensorSize.Width - _lampSize.Width - 2, _sensorSize.Height);
                    //g.DrawString(_inVars[key].StringCollection[index], this.Font, new SolidBrush(Color.Black), _inVars[key].Location.X + _sensorSize.Width - _lampSize.Width - g.MeasureString(value, this.Font).Width, (index + 1) * _sensorSize.Height + _inVars[key].Location.Y - _lampSize.Height);
                    g.DrawString(_inVars[key].StringCollection[index], this.Font, new SolidBrush(Color.Black), _inVars[key].Location.X+2, (index + 1) * _sensorSize.Height + _inVars[key].Location.Y - _lampSize.Height);
                }
                else
                {
                    _outVars[key].StringCollection[index] = value;
                    g.FillRectangle(new SolidBrush(this._componentColor), _outVars[key].Location.X + 2, index * _sensorSize.Height + _outVars[key].Location.Y, _sensorSize.Width - _lampSize.Width - 2, _sensorSize.Height);
                    //g.DrawString(_outVars[key].StringCollection[index], this.Font, new SolidBrush(Color.Black), _outVars[key].Location.X + _sensorSize.Width - _lampSize.Width - g.MeasureString(value, this.Font).Width, (index + 1) * _sensorSize.Height + _outVars[key].Location.Y - _lampSize.Height);
                    g.DrawString(_outVars[key].StringCollection[index], this.Font, new SolidBrush(Color.Black), _outVars[key].Location.X + 2, (index + 1) * _sensorSize.Height + _outVars[key].Location.Y - _lampSize.Height);
                }
            }
            //Обновление картинки не дожидаясь OnPaint
            try
            {
                using (Graphics g = this.CreateGraphics())
                {
                    g.DrawImage(_bitmap, 0, 0);
                }
            }
            catch { }
        }

        /// <summary>
        /// Замена всех меток пробелами
        /// </summary>
        public void ClearLabels()
        {
            try
            {
                using (Graphics g = Graphics.FromImage(_bitmap))
                {

                    for (int i = 0; i < _inVars.Count; i++)
                        for (int j = 0; j < _inVars[i].SensorNumber; j++)
                        {
                            _inVars[i].StringCollection[j] = "";
                            g.FillRectangle(new SolidBrush(this._componentColor), _inVars[i].Location.X + 2, j * _sensorSize.Height + _inVars[i].Location.Y, _sensorSize.Width - _lampSize.Width - 2, _sensorSize.Height);
                        }
                    for (int i = 0; i < _outVars.Count; i++)
                        for (int j = 0; j < _outVars[i].SensorNumber; j++)
                        {
                            _outVars[i].StringCollection[j] = "";
                            g.FillRectangle(new SolidBrush(this._componentColor), _outVars[i].Location.X + 2, j * _sensorSize.Height + _outVars[i].Location.Y, _sensorSize.Width - _lampSize.Width - 2, _sensorSize.Height);
                        }
                }
            }
            catch { }
            try
            {
                using (Graphics g = this.CreateGraphics())
                {
                    g.DrawImage(_bitmap, 0, 0);
                }
            }
            catch { }
        }

        /// <summary>
        /// Смена картинки на блоке
        /// </summary>
        /// <param name="isInput">входной ли датчик</param>
        /// <param name="key">номер меняемой переменной</param>
        /// <param name="value">новое зачение датчика</param>
        public void ChangeStatePictures(bool isInput, int key, byte value)
        {
            bool[] m_value = FromByteToBool(value);
            if (isInput)
            {
                for (int i = 0; i < _inVars[key].SensorNumber; i++)
                {
                    if (!_inVars[key].WriteSensors.Contains(i))
                        ChangeStatePicture(true, key, i, m_value[i]);
                    else
                    {
                        bool[] m_primaryValue = FromByteToBool(_inVars[key].PrimaryValue);
                        m_value[i] = m_primaryValue[i];
                    }
                }
                _inVars[key].PrimaryValue = FromBoolToByte(m_value);
            }
            else
            {
                for (int i = 0; i < _outVars[key].SensorNumber; i++)
                {
                    if (!_outVars[key].WriteSensors.Contains(i))
                        ChangeStatePicture(false, key, i, m_value[i]);
                    else
                    {
                        bool[] m_primaryValue = FromByteToBool(_outVars[key].PrimaryValue);
                        m_value[i] = m_primaryValue[i];
                    }
                }
                _outVars[key].PrimaryValue = FromBoolToByte(m_value);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Определение текущего сенсора для редактирования меток
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DigitalIO_MouseDown(object sender, MouseEventArgs e)
        {
            //this.ContextMenuStrip = this.cmsEditName;
            //if (e.Clicks == 2 && e.Button == MouseButtons.Right)
            //{
            //    if (e.X > _bitmap.Width || e.Y > _bitmap.Height)
            //    {
            //        this.ContextMenuStrip = null;
            //        return;
            //    }
            //    IList<int> m_key = new List<int>();
            //    m_key = _inVars.Keys;
            //    for (int i = 0; i < m_key.Count; i++)
            //    {
            //        for (int j = 0; j < _inVars[m_key[i]].SensorNumber; j++)
            //        {
            //            if (_inVars[m_key[i]].Location.X  < e.X &&
            //                _inVars[m_key[i]].Location.Y + j * _sensorSize.Height < e.Y &&
            //                _inVars[m_key[i]].Location.X + _sensorSize.Width - _lampSize.Width > e.X &&
            //                _inVars[m_key[i]].Location.Y + (j + 1) * _sensorSize.Height > e.Y)
            //            {
            //                _curentSensor = new Sensor(true, m_key[i], j);
            //                return;
            //            }
            //        }
            //    }
            //    m_key = _outVars.Keys;
            //    for (int i = 0; i < m_key.Count; i++)
            //    {
            //        for (int j = 0; j < _outVars[m_key[i]].SensorNumber; j++)
            //        {
            //            if (_outVars[m_key[i]].Location.X  < e.X &&
            //                _outVars[m_key[i]].Location.Y + j * _sensorSize.Height < e.Y &&
            //                _outVars[m_key[i]].Location.X + _sensorSize.Width - _lampSize.Width > e.X &&
            //                _outVars[m_key[i]].Location.Y + (j + 1) * _sensorSize.Height > e.Y)
            //            {
            //                _curentSensor = new Sensor(false, m_key[i], j);
            //                return;
            //            }
            //        }
            //    }
            //}

            //Редактирование при помощи окошечка
            this.tbEditLabel.Visible = false;
            if (e.Clicks == 2 && e.Button == MouseButtons.Left)
            {
                if (e.X > _bitmap.Width || e.Y > _bitmap.Height)
                    return;
                IList<int> m_key = new List<int>();
                m_key = _inVars.Keys;
                for (int i = 0; i < m_key.Count; i++)
                {
                    for (int j = 0; j < _inVars[m_key[i]].SensorNumber; j++)
                    {
                        if (_inVars[m_key[i]].Location.X < e.X &&
                            _inVars[m_key[i]].Location.Y + j * _sensorSize.Height < e.Y &&
                            _inVars[m_key[i]].Location.X + _sensorSize.Width - _lampSize.Width > e.X &&
                            _inVars[m_key[i]].Location.Y + (j + 1) * _sensorSize.Height > e.Y)
                        {
                            _curentSensor = new Sensor(true, m_key[i], j);
                            this.tbEditLabel.Visible = true;
                            return;
                        }
                    }
                }
                m_key = _outVars.Keys;
                for (int i = 0; i < m_key.Count; i++)
                {
                    for (int j = 0; j < _outVars[m_key[i]].SensorNumber; j++)
                    {
                        if (_outVars[m_key[i]].Location.X < e.X &&
                            _outVars[m_key[i]].Location.Y + j * _sensorSize.Height < e.Y &&
                            _outVars[m_key[i]].Location.X + _sensorSize.Width - _lampSize.Width > e.X &&
                            _outVars[m_key[i]].Location.Y + (j + 1) * _sensorSize.Height > e.Y)
                        {
                            _curentSensor = new Sensor(false, m_key[i], j);
                            this.tbEditLabel.Visible = true;
                            return;
                        }
                    }
                }
            }

            //this.ContextMenuStrip = null;
        }

        ///// <summary>
        ///// Загрузка описания текущего датчика в поле для редактирования меток 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void cmsEditName_Opening(object sender, CancelEventArgs e)
        //{
        //    if (_curentSensor.IsInput)
        //        this.ts1tbName.Text = _inVars[_curentSensor.Key].StringCollection[_curentSensor.Index];
        //    else
        //        this.ts1tbName.Text = _outVars[_curentSensor.Key].StringCollection[_curentSensor.Index];
        //}

        ///// <summary>
        ///// Если при редактировании меток нажата Enter, то завершение редактирования
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ts1tbName_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyData == Keys.Enter)
        //    {
        //        ChangeLabel(_curentSensor.IsInput, _curentSensor.Key, _curentSensor.Index, ts1tbName.Text);
        //        this.cmsEditName.Close();
        //    }
        //}

        ///// <summary>
        ///// Изменеие описания датчика при смене текста в поле редактирования меток
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ts1tbName_TextChanged(object sender, EventArgs e)
        //{
        //    ChangeLabel(_curentSensor.IsInput, _curentSensor.Key, _curentSensor.Index, ts1tbName.Text);
        //    OnLabelChange(new LabelChangeEventArgs(ts1tbName.Text, _curentSensor.IsInput, _curentSensor.Key, _curentSensor.Index));
        //}

        ///// <summary>
        ///// Разрешение ввода в поле редактирования меток
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ts1tbName_KeyUp(object sender, KeyEventArgs e)
        //{
        //    ts1tbName.ReadOnly = false;
        //}

        //private void ts1tbName_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    float m_width = 0;
        //    using (Graphics g = Graphics.FromImage(_bitmap))
        //    {
        //        m_width = g.MeasureString(ts1tbName.Text + e.KeyChar, this.Font).Width;
        //    }
        //    if ((m_width > _sensorSize.Width - _lampSize.Width - 2) && e.KeyChar != '\b') e.Handled = true;
        //}

        /// <summary>
        /// Изменеие описания датчика при смене текста в поле редактирования меток
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbEditLabel_TextChanged(object sender, EventArgs e)
        {
            ChangeLabel(_curentSensor.IsInput, _curentSensor.Key, _curentSensor.Index, tbEditLabel.Text);
            OnLabelChange(new LabelChangeEventArgs(tbEditLabel.Text, _curentSensor.IsInput, _curentSensor.Key, _curentSensor.Index));
        }

        /// <summary>
        /// Загрузка текста когда окно становится видемым
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbEditLabel_VisibleChanged(object sender, EventArgs e)
        {
            if (this.tbEditLabel.Visible)
            {
                this.tbEditLabel.Size = new Size(_sensorSize.Width - _lampSize.Width - 2, _sensorSize.Height - 1);
                if (_curentSensor.IsInput)
                {
                    this.tbEditLabel.Location = new Point(_inVars[_curentSensor.Key].Location.X + 2, _inVars[_curentSensor.Key].Location.Y + _curentSensor.Index * _sensorSize.Height + 1);
                    this.tbEditLabel.Text = _inVars[_curentSensor.Key].StringCollection[_curentSensor.Index];
                }
                else
                {
                    this.tbEditLabel.Location = new Point(_outVars[_curentSensor.Key].Location.X + 2, _outVars[_curentSensor.Key].Location.Y + _curentSensor.Index * _sensorSize.Height + 1);
                    this.tbEditLabel.Text = _outVars[_curentSensor.Key].StringCollection[_curentSensor.Index];
                }
            }
        }
        /// <summary>
        /// делать невидимой tbEditLabel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbEditLabel_Leave(object sender, EventArgs e)
        {
            this.tbEditLabel.Visible = false;
        }

        /// <summary>
        /// Если при редактировании меток нажата Enter, то завершение редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbEditLabel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ChangeLabel(_curentSensor.IsInput, _curentSensor.Key, _curentSensor.Index, tbEditLabel.Text);
                tbEditLabel.Visible = false;
            }
        }
        /// <summary>
        /// Ввод символа только при условии что он влезет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbEditLabel_KeyPress(object sender, KeyPressEventArgs e)
        {
            float m_width = 0;
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                m_width = g.MeasureString(tbEditLabel.Text + e.KeyChar, this.Font).Width;
            }
            if ((m_width > _sensorSize.Width - _lampSize.Width - 2) && e.KeyChar != '\b') e.Handled = true;
        }
        /// <summary>
        /// Разрешение ввода в поле редактирования меток
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbEditLabel_KeyUp(object sender, KeyEventArgs e)
        {
            tbEditLabel.ReadOnly = false;
        }

    }
}
