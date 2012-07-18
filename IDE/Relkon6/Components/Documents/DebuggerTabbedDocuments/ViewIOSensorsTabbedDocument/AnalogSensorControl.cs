using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon;

namespace Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument
{
    public sealed partial class AnalogSensorControl : UserControl
    {
        private byte[] data = new byte[2]; // значение датчика в том виде, в котором оно храниться в контроллере 
        private Color valueFieldNormalColor = Color.White; // Цвет поля, отображающего значение датчика
        private Color valueFieldEditedColor = Color.FromArgb(255, 255, 0); // Цвет поля, отображающего значение датчика, после изменения этого значения пользователем
        private bool valueChangedProgrammativaly = false; // Показывает, что значение поля было изменено програмно
        private bool singleByte = true;//отображение 8 бит(или 12)
        private bool changeSigleByte = true;//возможность изменить число отображаемых бит

        private bool inverseByteOrder = false;//обратный порядок бит в контроллере
        private bool mouseClick = true;//Разрешена ли смена состояния датчиков

        public event EventHandler ValueChanged; // Возникает, когда пользователь поменял значение датчика
        public event EventHandler LabelChanged; // Возникает, когда пользователь поменял метку датчика
        public event EventHandler OneByteChanged; // Возникает, когда пользователь поменял число отображаемых байт

        /// <summary>
        /// Изменение возможности изменения числа отображаемых бит
        /// </summary>
        public bool ChangeSigleByte
        {
            get { return changeSigleByte; }
            set
            {
                changeSigleByte = value;
                this.rb8b.Enabled = value;
                this.rb12b.Enabled = value;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="DataLength"></param>
        public AnalogSensorControl(int DataLength)
        {
            InitializeComponent();
            data = new byte[DataLength];
            switch (data.Length)
            {
                case 1:
                    ChangeSigleByte = false;
                    break;
                case 2:
                    ChangeSigleByte = true;
                    break;
            }
            this.Width = this.tbarValue.Location.X + this.tbarValue.Size.Width + 5;
            this.SigleByte = true;
        }

        /// <summary>
        /// Изменение цвета при изменение цвета родительского компонента
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if(this.Parent != null)
                this.BackColor = this.Parent.BackColor;
        }

        /// <summary>
        /// Изменение цвета фона при изменение фона родительского компонента
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            if(this.Parent != null)
                this.BackColor = Parent.BackColor;
        }

        /// <summary>
        /// Поле отражающее возможность смены состнияния датчиков
        /// </summary>
        public bool EnabledMouseClick
        {
            get { return mouseClick; }
            set { 
                mouseClick = value;
                this.nudValue.Enabled = mouseClick;
                //this.nudValue.BackColor = System.Drawing.SystemColors.Window;
                this.tbarValue.Enabled = mouseClick;
            }
        }

        /// <summary>
        /// Возвращает или устанавливает имя датчика
        /// </summary>
        [Browsable(true)]
        public string SensorName
        {
            get
            {
                return this.label1.Text;
            }
            set
            {
                this.label1.Text = value;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает текстовое описание датчика
        /// </summary>
        [Browsable(true)]
        public string SensorLabel
        {
            get
            {
                return this.tbDescription.Text;
            }
            set
            {
                this.tbDescription.Text = value;
            }
        }
        /// <summary>
        /// Возращает или устанавливает флаг, определяющий порядок следования байт значении датчика
        /// </summary>
        [Browsable(true)]        
        public bool InverseByteOrder 
        {
            get
            {
                return this.inverseByteOrder;    
            }
            set
            {
                if (this.inverseByteOrder != value)
                {
                    this.inverseByteOrder = value;
                    this.UpdateDataPresentation();
                }
            }
        }
        /// <summary>
        /// Если true, то будет обрабатываться только старший байт датчика
        /// </summary>
        [Browsable(true)]
        public bool SigleByte 
        {
            get
            {
                return this.singleByte;
            }
            set
            {
                if (this.singleByte != value)
                {
                    this.singleByte = value;
                    this.rb8b.Checked = value;
                    this.rb12b.Checked = !value;
                    this.UpdateDataPresentation();
                }
            }
        }

        /// <summary>
        /// Возвращает цвет поля ввода значений датчика
        /// </summary>
        [Browsable(true)]
        public Color ValueFieldColor
        {
            get
            {
                return this.valueFieldNormalColor;
            }
            set
            {
                if (this.valueFieldNormalColor != value)
                {
                    this.valueFieldNormalColor = value;
                    if (!this.Edited)
                        this.nudValue.BackColor = value;
                }
            }
        }
        /// <summary>
        /// Показывает, что компонент находится в режиме редатирования
        /// </summary>
        public bool Edited { get; private set; }
        /// <summary>
        /// Обновляет отображение значения датчика
        /// </summary>
        private void UpdateDataPresentation()
        {
            this.valueChangedProgrammativaly = true;
            this.nudValue.Maximum = this.tbarValue.Maximum = this.singleByte ? 255 : 65519;
            this.tbarValue.TickFrequency = this.tbarValue.Maximum / 10;
            this.nudValue.Value = this.tbarValue.Value = this.ComputeValue();
            this.valueChangedProgrammativaly = false;
        }
        /// <summary>
        /// Устанавливает новое значение, отображаемое компонентом
        /// </summary>
        /// <param name="Data">Значение датчика в том виде, в котором оно храниться в контроллере</param>
        public void SetData(byte[] Data)
        {
            if (Data == null || Data.Length != this.data.Length)
                throw new Exception("Inavalid argument");
            Array.Copy(Data, this.data, this.data.Length);
            this.UpdateDataPresentation();
        }
        /// <summary>
        /// Возвращает значение датчика в том виде, в котором оно храниться в контроллере
        /// </summary>
        public byte[] GetData()
        {
            byte[] res = new byte[this.data.Length];
            Array.Copy(this.data, res, res.Length);
            return this.data;
        }
        /// <summary>
        /// По массиву data вычисляет значение датчика
        /// </summary>
        private int ComputeValue()
        {
            byte[] b = this.InverseByteOrder ? Utils.ReflectArray<byte>(this.data) : this.data;
            return this.SigleByte ? b[0] : AppliedMath.BytesToInt(b);
        }
        /// <summary>
        /// Вычисление массива data по значению датчика
        /// </summary>
        private void ComputeData()
        {
            byte[] b = new byte[2];
            b = AppliedMath.IntToBytes((int)this.nudValue.Value);
            switch (data.Length)
            {
                case 1:
                    this.data[0] = b[3];
                    break;
                case 2:
                    if (this.singleByte)
                    {
                        this.data[0] = b[3];
                        this.data[1] = 0;
                    }
                    else
                    {
                        b = AppliedMath.IntToBytes((int)(this.nudValue.Value));
                        this.data[0] = b[2];
                        this.data[1] = b[3];
                        //this.data[0] = b[3];
                        //this.data[1] = b[2];
                    }
                    break;
            }
            if (this.InverseByteOrder) Array.Reverse(this.data);
        }       
        /// <summary>
        /// Переводит компонент в режим редактирования
        /// </summary>
        private void SetEditedModeOn()
        {
            this.nudValue.BackColor = this.valueFieldEditedColor;
            this.Edited = true;
        }
        /// <summary>
        /// Выводит компонент из режима программирования
        /// </summary>
        private void SetEditedModeOff()
        {
            this.Edited = false;
            this.nudValue.BackColor = valueFieldNormalColor;
        }

        /// <summary>
        /// Вызов события изменения значения переменной
        /// </summary>
        private void RaiseValueChangedEvent()
        {
            if (this.Edited)
            {
                ComputeData();
                if (this.ValueChanged != null)
                    this.ValueChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Изменение значения переменной пользователем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudValue_ValueChanged(object sender, EventArgs e)
        {
            if (!this.valueChangedProgrammativaly)
            {
                if (!this.Edited)
                    this.SetEditedModeOn();
                //////////////////////////
                //запись каждой единицы
                int tbv = this.tbarValue.Value;
                decimal nuv = this.nudValue.Value;
                //////////////////////////
                if (sender.Equals(this.tbarValue))
                {
                    if (((TrackBar)sender).Value != (int)this.nudValue.Value)
                        this.nudValue.Value = ((TrackBar)sender).Value;
                }
                else if (sender.Equals(this.nudValue))
                {
                    int value = (int)((NumericUpDown)sender).Value;
                    if (value != this.tbarValue.Value)
                        this.tbarValue.Value = value;
                }
                ////////////////////////
                //запись каждой единицы
                if (this.tbarValue.Value != tbv || nuv != this.nudValue.Value)
                {
                    Console.WriteLine("-" + tbv.ToString() + "-" + nuv.ToString() + "-");
                    this.RaiseValueChangedEvent();
                }
                ///////////////////////
            }
        }

        private void tbDescription_Leave(object sender, EventArgs e)
        {
            if (!this.tbarValue.Focused && !this.nudValue.Focused)
            {
                this.RaiseValueChangedEvent();
                this.SetEditedModeOff();
            }
        }

        private void tbarValue_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.Enter)
            {
                this.RaiseValueChangedEvent();
                this.SetEditedModeOff();
            }
            else*/ if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                this.SetEditedModeOn();
        }

        private void AnalogSensorControl_Leave(object sender, EventArgs e)
        {
            this.RaiseValueChangedEvent();
            this.SetEditedModeOff();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            this.SensorLabel = this.tbDescription.Text;
            if (this.LabelChanged != null)
                this.LabelChanged(this, EventArgs.Empty);
        }

        private void nudValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
                this.SetEditedModeOn();
        }

        private void rb8b_CheckedChanged(object sender, EventArgs e)
        {
            this.SigleByte = this.rb8b.Checked;
            if (this.OneByteChanged != null && this.rb8b.Checked)
                this.OneByteChanged(this, EventArgs.Empty);
        }

        private void rb12b_CheckedChanged(object sender, EventArgs e)
        {
            this.SigleByte = !this.rb12b.Checked;
            if (this.OneByteChanged != null && this.rb12b.Checked)
                this.OneByteChanged(this, EventArgs.Empty);
        }

        private void tbDescription_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2 && e.Button == MouseButtons.Left)
            {
                this.tbDescription.ReadOnly = false;
                this.tbDescription.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void tbDescription_Leave_1(object sender, EventArgs e)
        {
            this.tbDescription.ReadOnly = true;
            this.tbDescription.BorderStyle = BorderStyle.None;
        }

        private void tbDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.tbDescription.ReadOnly = true;
                this.tbDescription.BorderStyle = BorderStyle.None;
            }
        }


    }
}
