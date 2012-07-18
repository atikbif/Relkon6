using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Kontel.Relkon.Solutions;
using System.Windows.Forms;
using System.Threading;
using Kontel.Relkon;
using Kontel.Relkon.Debugger;
using System.Drawing;

namespace Kontel.Relkon.Components.Documents
{

    public sealed partial class ViewMemoryTabbedDocument : DebuggerTabbedDocument
    {
        private bool _cycleReading = false;//Происходит ли циклическое чтение в данный момент
        private bool _changeData = true;//В предыдущий раз считывались те же данные
        private int _readAddress = 0;//Адрес опрашиваемой или записываемой в данный момент памяти
        private int _readSize = 0;//Размер опрашиваемой или записываемой в данный момент памяти
        private int _Address = 0;//Адрес корректно введенный в поле
        private bool _HexAddressCoding = true;
        private bool _HexSizeCoding = true;
        private int _Size = 0;//Размер корректно введенный в поле
        private MemoryType _readMemory = MemoryType.Clock;//Тип опрашиваемой или записываемой в данный момент памяти
        private DebuggerEngine _engine = null; // движок отладчика
        private bool _IsReading = false;//Возможен ли опрос памяти(зависит от видимости окна)

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="solution"></param>
        public ViewMemoryTabbedDocument(ControllerProgramSolution solution, DebuggerEngine engine)
            : base(solution, engine)
        {
            InitializeComponent();
            _engine = engine;
        }

        protected override string ProtectedTabText
        {
            get
            {
                return "Память контроллера";
            }
        }


        private void StatusCallBack(object Marker, double PercentRead, byte[] Buffer)
        {
            Int32 m_value = (Int32)(PercentRead * pbStatus.Maximum);
            this.pbStatus.Value = m_value;
            this.pbStatus.Refresh();
        }

        /// <summary>
        /// Определение размера текущей памяти
        /// </summary>
        /// <returns></returns>
        private int MaxMemoryValue()
        {
            int m_max = 8;
            switch (this.ddlMemoryType.Text.ToUpper())
            {
                case "CLOCK":
                    m_max = 8;
                    break;
                case "RAM":
                    m_max = 256;
                    break;               
                case "FRAM":
                    m_max = 66560;
                    break;
                case "XRAM":                   
                    m_max = 66560;
                    break;
                case "FLASH":
                    m_max = 66560;
                    break;               
                default:
                    m_max = 8;
                    break;
            }
            return m_max;
        }

        private MemoryType GetMemoryType(string Name)
        {
            switch (Name.ToUpper())
            {
                case "CLOCK":
                    return MemoryType.Clock;
                case "RAM":
                    return MemoryType.RAM;               
                case "FRAM":
                    return MemoryType.FRAM;
                case "XRAM":
                    return MemoryType.XRAM;
                case "FLASH":
                    return MemoryType.Flash;                
            }
            return MemoryType.Clock;
        }

        /// <summary>
        /// Однократное чтение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bAdd_Click(object sender, EventArgs e)
        {
                try
                {
                    this.bWrite.Enabled = false;
                    this.bRead.Enabled = false;
                    this.bTimerRead.Enabled = false;
                    this.ddlMemoryType.Enabled = false;
                    this.tbStartAddress.Enabled = false;
                    this.tbReadingSize.Enabled = false;

                    _readAddress = _Address;
                    _readSize = _Size;
                    _readMemory = GetMemoryType(this.ddlMemoryType.Text);
                    _engine.AddReadItem(_readAddress, _readMemory, _readSize, "memory_one", StatusCallBack, CallBack);
                    this.pbStatus.Visible = true;
                }
                catch
                {
                    this.bWrite.Enabled = true;
                    this.bTimerRead.Enabled = true;
                    this.bRead.Enabled = true;
                    this.ddlMemoryType.Enabled = true;
                    this.tbStartAddress.Enabled = true;
                    this.tbReadingSize.Enabled = true;
                    this.pbStatus.Visible = false;
                }
                finally
                {
                }
        }

        /// <summary>
        /// Передача прочитанных данных в HexEditor
        /// </summary>
        /// <param name="sender"></param>
        private void CallBack(object Marker, byte[] Buffer, bool Error)
        {
            try
            {
                if (Buffer!=null && Buffer.Length != 0)
                {
                    this.heMemory.ChangeValues(Buffer, _readAddress, (this._changeData || (string)Marker == "memory_one"));
                    this._changeData = false;
                    this.heMemory.AddressColor = Color.DarkSlateGray;
                    this.heMemory.CodeColor = Color.FromArgb(0, 102, 0);
                    this.heMemory.PresentationColor = Color.FromArgb(0, 102, 204);
                }
                else
                {
                    //Установка исходного вида при ошибке чтения
                    byte[] m_array;
                    m_array = new byte[this.heMemory.SegmentSize];
                    for (int i = 0; i < this.heMemory.SegmentSize; i++)
                        m_array[i] = 0;
                    this.heMemory.ChangeValues(m_array, 0, true);
                    this.heMemory.AddressColor = System.Drawing.SystemColors.GrayText;
                    this.heMemory.CodeColor = System.Drawing.SystemColors.GrayText;
                    this.heMemory.PresentationColor = System.Drawing.SystemColors.GrayText;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                this.bWrite.Enabled = true;
                if (!_cycleReading)
                {
                    _engine.RemoveReadItem(_readAddress, _readMemory, "memory_one");
                    this.bTimerRead.Enabled = true;
                    this.bRead.Enabled = true;
                    this.ddlMemoryType.Enabled = true;
                    this.tbStartAddress.Enabled = true;
                    this.tbReadingSize.Enabled = true;
                    this.pbStatus.Visible = false;
                    this.pbStatus.Value = 0;
                }
            }
        }


        /// <summary>
        /// Событие при потере фокуса полем "Размер считывания"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadingSize_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.tbReadingSize.Text[0] == '0' && this.tbReadingSize.Text[1] == 'x')
                {
                    this._Size = AppliedMath.HexToDec(this.tbReadingSize.Text);
                    _HexSizeCoding = true;
                }
                else
                {
                    _Size = int.Parse(this.tbReadingSize.Text);
                    _HexSizeCoding = false;
                }
            }
            catch
            {
                SetValues();
                return;
            }
            if (this._Size == 0)
                this._Size = 1;
            int m_size = 32767 / this.heMemory.SymbolHeight * this.heMemory.SegmentSize;
            if (this._Size > m_size)
                this._Size = m_size;
            if (this._Size + _Address > MaxMemoryValue())
                this._Size = MaxMemoryValue() - this._Address;
            _engine.Parameters.ReadingSize = _Size;
            this.SetValues();
        }
        /// <summary>
        /// Событие при потере фокуса полем "Начальный адрес"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartAddress_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.tbStartAddress.Text[0] == '0' && this.tbStartAddress.Text[1] == 'x')
                {
                    _Address = AppliedMath.HexToDec(this.tbStartAddress.Text);
                    _HexAddressCoding = true;
                }
                else
                {
                    _Address = int.Parse(this.tbStartAddress.Text);
                    _HexAddressCoding = false;
                }
            }
            catch
            {
                SetValues();
                return;
            }
            if (this._Address == 0 && this.ddlMemoryType.Text == "SD Card")
                this._Address = 1;
            if (this._Address >= MaxMemoryValue())
                this._Address = MaxMemoryValue() - 1;
            if (this._Size + this._Address > MaxMemoryValue())
                this._Size = MaxMemoryValue() - this._Address;
            _engine.Parameters.ViewMemoryAddress = _Address;
            this.SetValues();
        }
        /// <summary>
        /// Установка значений в поля размер считывания и начальный адрес
        /// </summary>
        private void SetValues()
        {
            if (_HexAddressCoding)
                if (this._Address == 0) this.tbStartAddress.Text = "0x00";
                else this.tbStartAddress.Text = "0x" + AppliedMath.DecToHex(this._Address);
            else
                this.tbStartAddress.Text = "" + this._Address;
            if (_HexSizeCoding)
                this.tbReadingSize.Text = "0x" + AppliedMath.DecToHex(this._Size);
            else
                this.tbReadingSize.Text = "" + this._Size;
        }

        /// <summary>
        /// Событие при смене типа памяти
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemoryType_SelectedValueChanged(object sender, EventArgs e)
        {
            _Address = AppliedMath.HexToDec(this.tbStartAddress.Text);
            if (this._Address == 0 && this.ddlMemoryType.Text == "SD Card")
                this._Address = 1;
            if (this._Address >= MaxMemoryValue())
                this._Address = MaxMemoryValue() - 1;
            if (this._Size + this._Address > MaxMemoryValue())
                this._Size = MaxMemoryValue() - this._Address;
            this.SetValues();
            _engine.Parameters.ViewMemoryType = GetMemoryType(this.ddlMemoryType.Text);
        }

        /// <summary>
        /// Циклическое чтение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (_cycleReading)
            {
                //Остановка циклического чтения
                this._IsReading = false;
                _engine.RemoveReadItem(_readAddress, _readMemory, "memory_cycle");
                this.bTimerRead.Text = "Циклическое чтение";
                this._cycleReading = false;
                this.bRead.Enabled = true;
                this.bWrite.Enabled = true;
                this.ddlMemoryType.Enabled = true;
                this.tbStartAddress.Enabled = true;
                this.tbReadingSize.Enabled = true;
            }
            else
            {
                //Запуск циклического чтения
                this._IsReading = true;
                _readAddress = this._Address;
                _readSize = this._Size;
                _readMemory = GetMemoryType(this.ddlMemoryType.Text);
                _engine.AddReadItem(_readAddress, _readMemory, _readSize, "memory_cycle", StatusCallBack, CallBack);
                this.bTimerRead.Text = "Остановить";
                this._cycleReading = true;
                this.bRead.Enabled = false;
                this.bWrite.Enabled = false;
                this.ddlMemoryType.Enabled = false;
                this.ddlMemoryType.ForeColor = System.Drawing.SystemColors.WindowText;
                this.tbStartAddress.Enabled = false;
                this.tbReadingSize.Enabled = false;
            }
        }

        public override void UpdateDataPresentation(bool HEX)
        {
            base.UpdateDataPresentation(HEX);
            this.heMemory.CodingType = this.codingType;
        }

        /// <summary>
        /// Событие при создании формы просмотра памяти
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewMemoryTabbedDocument_Load(object sender, EventArgs e)
        {
            _engine.EngineStatusChanged += new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
            this.bRead.Enabled = (_engine.EngineStatus == DebuggerEngineStatus.Started ? true : false);
            this.bTimerRead.Enabled = _engine.EngineStatus == DebuggerEngineStatus.Started ? true : false;
            this.bWrite.Enabled = _engine.EngineStatus == DebuggerEngineStatus.Started ? true : false;
            byte[] m_array;
            m_array = new byte[this.heMemory.SegmentSize];
            for (int i = 0; i < this.heMemory.SegmentSize; i++)
                m_array[i] = 0;
            this.heMemory.ChangeValues(m_array, 0, true);
            this.heMemory.AddressColor = System.Drawing.SystemColors.GrayText;
            this.heMemory.CodeColor = System.Drawing.SystemColors.GrayText;
            this.heMemory.PresentationColor = System.Drawing.SystemColors.GrayText;
            //Загрузка параметров
            Update(null, _engine);
        }


        /// <summary>
        /// Загрузка новых параметров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Update(ControllerProgramSolution solution, DebuggerEngine engine)
        {
            try
            {
                _engine = engine;
                this._Address = _engine.Parameters.ViewMemoryAddress;
                this._Size = _engine.Parameters.ReadingSize;
                this.SetValues();
                switch (engine.Parameters.ProcessorType)
                {
                    case ProcessorType.MB90F347:
                        this.ddlMemoryType.Items[5] = "SD Card";
                        break;
                    case ProcessorType.AT89C51ED2:
                        this.ddlMemoryType.Items[5] = "Flash";
                        break;
                    default:
                        break;
                }
                switch (_engine.Parameters.ViewMemoryType)
                {
                    case MemoryType.Clock: this.ddlMemoryType.Text = "Clock"; break;
                    case MemoryType.RAM: this.ddlMemoryType.Text = "RAM"; break;
                    case MemoryType.EEPROM: this.ddlMemoryType.Text = "Eprom"; break;
                    case MemoryType.FRAM: this.ddlMemoryType.Text = "FRAM"; break;
                    case MemoryType.XRAM: this.ddlMemoryType.Text = "XRAM"; break;
                    case MemoryType.Flash: this.ddlMemoryType.Text = "FLash"; break;
                    default: this.ddlMemoryType.Text = "SD Card"; break;
                }
                this.StartAddress_Leave(null, null);
                this.ReadingSize_Leave(null, null);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("Update:" + ex.Message);
                return;
            }
        }


        /// <summary>
        /// Изменение состояния отладчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DebuggerParametersList_ChangeStatusEngine(object sender, DebuggerEngineStatusChangedEventArgs e)
        {
            //при осатновке отладчика циклическое чтение прерывается
            this.bRead.Enabled = _engine.EngineStatus == DebuggerEngineStatus.Started;
            this.bTimerRead.Enabled = _engine.EngineStatus == DebuggerEngineStatus.Started;
            this.bWrite.Enabled = _engine.EngineStatus == DebuggerEngineStatus.Started;
            if (_cycleReading && e.Status != DebuggerEngineStatus.Started)
                //_cycleReading = false;
                this.button2_Click(null, null);

            this.bTimerRead.Text = "Циклическое чтение";
            this.tbStartAddress.Enabled = true;
            this.tbReadingSize.Enabled = true;
            this.ddlMemoryType.Enabled = true;
            this.tbStartAddress.BackColor = System.Drawing.SystemColors.Window;
            this.tbReadingSize.BackColor = System.Drawing.SystemColors.Window;
            this.pbStatus.Visible = false;

            //поля ввода данных не активны при остановленном отладчике
            //this.bRead.Enabled = (_engine.EngineStatus == DebuggerEngineStatus.Started) && !_cycleReading && this.pbStatus.Value == 0;
            //this.bTimerRead.Enabled = _engine.EngineStatus == DebuggerEngineStatus.Started && (_cycleReading || !_cycleReading && this.pbStatus.Value == 0);
            //this.bWrite.Enabled = (_engine.EngineStatus == DebuggerEngineStatus.Started) && !_cycleReading && this.pbStatus.Value == 0;

            //this.tbStartAddress.Enabled = (_engine.EngineStatus == DebuggerEngineStatus.Started) && !_cycleReading && this.pbStatus.Value == 0;
            //this.tbReadingSize.Enabled = (_engine.EngineStatus == DebuggerEngineStatus.Started) && !_cycleReading && this.pbStatus.Value == 0;
            //this.ddlMemoryType.Enabled = (_engine.EngineStatus == DebuggerEngineStatus.Started) && !_cycleReading && this.pbStatus.Value == 0;
            //this.tbStartAddress.BackColor = System.Drawing.SystemColors.Window;
            //this.tbReadingSize.BackColor = System.Drawing.SystemColors.Window;
            //this.pbStatus.Visible = this.pbStatus.Value > 0 && !_cycleReading;

     }

        /// <summary>
        /// Изменение параметров чтения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Data_TextChanged(object sender, EventArgs e)
        {
            this._changeData = true;
        }

        /// <summary>
        /// Запись измененных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bWrite_Click(object sender, EventArgs e)
        {
            if (this._cycleReading)
            {
                try
                {
                    //Удаление запроса на циклическое чтение памяти
                    _engine.RemoveReadItem(_readAddress, _readMemory, "memory_cycle");
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                    return;
                }
            }
            int m_Address = this.tbStartAddress.Text.Contains("x") ? Convert.ToInt32(this.tbStartAddress.Text.Substring(2), 16) : Convert.ToInt32(this.tbStartAddress.Text, 10);
            SortedList<int, byte> ValueseToWrite = this.heMemory.GetValues();
            int m_StartAddress = 0;
            List<Byte> m_alist = new List<Byte>();
            ushort m_Size = 0;
            for (int i = this.heMemory.GetStartAddress(); i < this.heMemory.GetStartAddress() + this.heMemory.GetArraySize(); i++)
            {
                if (ValueseToWrite.ContainsKey(i))
                {
                    m_alist.Add(ValueseToWrite[i]);
                    m_Size = Convert.ToUInt16(m_Size + 1);
                    if ((i == 0) || (!ValueseToWrite.ContainsKey(i - 1)))
                    {
                        m_StartAddress = i;
                    }
                    if ((i + 1 == this.heMemory.GetArraySize()) || (!ValueseToWrite.ContainsKey(i + 1)))
                    {
                        _engine.AddWriteItem(m_StartAddress,_readMemory,m_alist.ToArray(),m_Size,null,null);
                        m_alist.Clear();
                        m_Size = 0;
                    }
                }
                else
                {
                }
            }
            this.heMemory.SaveChanges();
            if (this._cycleReading)
            {
                //Запуск циклического чтения памяти
                _engine.AddReadItem(_readAddress, _readMemory, _readSize, "memory_cycle", StatusCallBack, CallBack);
            }
        }

        /// <summary>
        /// Событие при прокрутке ползунка представления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Memory_Scroll(object sender, ScrollEventArgs e)
        {
            this.borderPanel1.Refresh();
        }

        private void ViewMemoryTabbedDocument_Closed(object sender, EventArgs e)
        {
            _engine.EngineStatusChanged -= new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
                   }

        private void ViewMemoryTabbedDocument_VisibleChanged(object sender, EventArgs e)
        {
            if ((this.IsOpen) && (!this._IsReading) && (this._cycleReading))
            {
                this._IsReading = true;
                //Запуск циклического чтения памяти
                _engine.AddReadItem(_readAddress, _readMemory, _readSize, "memory_cycle", StatusCallBack, CallBack);
            }
            else
            {
                if ((this._IsReading) && (!this.IsOpen) && (this._cycleReading))
                {
                    this._IsReading = false;
                    try
                    {
                        //Удаление запроса на циклическое чтение памяти
                    _engine.RemoveReadItem(_readAddress, _readMemory, "memory_cycle");
                    }
                    catch (Exception ex)
                    {
                        Utils.ErrorMessage(ex.Message);
                        return;
                    }
                }
            }
        }

        private void tbStartAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                this.StartAddress_Leave(null,null);
        }

        private void tbReadingSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                this.ReadingSize_Leave(null, null);
        }

    }
}
