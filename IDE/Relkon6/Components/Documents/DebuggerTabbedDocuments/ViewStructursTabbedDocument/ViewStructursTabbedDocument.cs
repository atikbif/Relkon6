using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon.Solutions;
using Kontel.Relkon.Debugger;
using System.Data.SqlClient;
using Kontel.Relkon.Classes;
using Kontel.Relkon;

namespace Kontel.Relkon.Components.Documents
{
    public partial class ViewStructursTabbedDocument : DebuggerTabbedDocument
    {
        private ControllerProgramSolution _solution = null;//Текущий проект
        private DebuggerEngine _engine = null; // движок отладчика
        private bool _IsReading = false;//Опрашиваются ли переменные(видна ли вкладака)
        private string _EditCellText = "0";//Значение на которое заменятеся значение в ячейке редактирования при неправлиьном вводе данных
        private DataRow _EditRow = null;//Изменяемая в данный момент переменная
        private List<ControllerStructVar> _freeStructs = new List<ControllerStructVar>();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="solution"></param>
        public ViewStructursTabbedDocument(ControllerProgramSolution solution, DebuggerEngine engine)
            : base(solution, engine)
        {
            InitializeComponent();
            _engine = engine;
            _solution = solution;
        }
    

        protected override string ProtectedTabText
        {
            get
            {
                return "Структуры контроллера";
            }
        }

        private System.Data.DataTable StructsTable
        {
            get
            {
                return (System.Data.DataTable)this.structsDataSet.Tables[2];

            }
        }

        private System.Data.DataTable VarsTable
        {
            get
            {
                return (System.Data.DataTable)this.structsDataSet.Tables[1];
            }
        }

        private System.Data.DataTable DisplayVarsTable
        {
            get
            {
                return (System.Data.DataTable)this.structsDataSet.Tables[0];
            }
        }

        /// <summary>
        /// Добавление значения переменной в таблицу, когда она прочитана
        /// </summary>
        /// <param name="sender"></param>
        private void CallBack(object Marker, byte[] Buffer, bool Error)
        {
            DataRow marker = (DataRow)Marker;
            try { long i = (long)marker[9]; }
            catch { return; }
            if (Buffer != null && !Error)
            {
                if (_EditRow != marker)
                {
                    marker[2] = Buffer;
                    marker[4] = BuildDisplayValue(Buffer, Buffer.Length, (bool)((long)marker[9] < 0));
                }
            }
            else
                marker[4] = BuildDisplayValue(null, 0, false);
        }

        /// <summary>
        /// Событие при открытии вкладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewStructursTabbedDocument_Load(object sender, EventArgs e)
        {
            _engine.EngineStatusChanged += new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
            DebuggerParametersList_ChangeStatusEngine(null,null);
            //Загрузка параметров
            this.Update(_solution, _engine);
        }

        /// <summary>
        /// Событие при изменении состояния отладчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DebuggerParametersList_ChangeStatusEngine(object sender, DebuggerEngineStatusChangedEventArgs e)
        {
            if ((_engine.EngineStatus == DebuggerEngineStatus.Started) && (_solution != null) && (this._IsReading))
                this.dgv_askStructs.Columns[2].ReadOnly = false;
            else
                this.dgv_askStructs.Columns[2].ReadOnly = true;
        }

        /// <summary>
        /// Загрузка новых параметров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Update(ControllerProgramSolution solution, DebuggerEngine engine)
        {
            _engine = engine;
            if (solution != null) _solution = solution;
            else
            {
                _solution = ControllerProgramSolution.Create(_engine.Parameters.ProcessorType);
            }
            //Очистка значений переменных
            this.StructsTable.Rows.Clear();
            this.VarsTable.Rows.Clear();
            this.DisplayVarsTable.Rows.Clear();
            this._freeStructs = new List<ControllerStructVar>();
            this.clb_structurs.Items.Clear();

            FillStuctursTables();
            //Добваление структур, которых нет в проекте
            ControllerStructVar m_struct;
            ControllerUserVar m_var;
            foreach (DebuggerParameters.StructDescription sd in _engine.Parameters.ReadingStructs)
            {
                bool m_fined = false;
                foreach (DataRow dr in this.StructsTable.Rows)
                    if (dr[0].ToString() == sd.Name)
                    {
                        m_fined = true;
                        break;
                    }
                if (m_fined) continue;
                m_struct = new ControllerStructVar();
                m_struct.Address = sd.Address;
                m_struct.HasSign = sd.HasSign;
                m_struct.Memory = sd.MemoryType;
                m_struct.Name = sd.Name;
                m_struct.Size = sd.Size;
                foreach (DebuggerParameters.VarDescription vd in sd.Vars)
                {
                    m_var = new ControllerUserVar();
                    m_var.Address = vd.Address;
                    m_var.HasSign = vd.HasSign;
                    m_var.Array = false;
                    m_var.Memory = vd.MemoryType;
                    m_var.Name = vd.Name;
                    m_var.Size = vd.Size;
                    m_struct.Vars.Add(m_var);
                }
                AddStructurToTable(m_struct);
            }
            FillCheckedList();
            //Выбор структур для опроса
            List<DebuggerParameters.StructDescription> readStructs = new List<DebuggerParameters.StructDescription>(_engine.Parameters.ReadingStructs);
            _engine.Parameters.ReadingStructs = new List<DebuggerParameters.StructDescription>();
            for (int i = 0; i < this.clb_structurs.Items.Count; i++)
            {
                foreach (DebuggerParameters.StructDescription sd in readStructs)
                {
                    if (this.clb_structurs.Items[i].ToString() == sd.Name)
                    {
                        //this.clb_structurs_ItemCheck(this.clb_structurs, new ItemCheckEventArgs(i, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked));
                        this.clb_structurs.SetItemCheckState(i, CheckState.Checked);
                    }
                }
            }
        }

        /// <summary>
        /// Формирования таблиц структур и переменных структур
        /// </summary>
        private void FillStuctursTables()
        {
            //Формирование списка структур
            foreach (ControllerStructVar v in _solution.Vars.StructVars)
            {
                AddStructurToTable(v);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="structure"></param>
        private void AddStructurToTable(ControllerStructVar structure)
        {
            DataRow m_row;
            m_row = this.StructsTable.NewRow();
            m_row[0] = structure.Name;
            m_row[1] = structure.Address;
            m_row[2] = structure.Memory;
            m_row[3] = structure.Size;
            int m_addrass = structure.Address;
            this.StructsTable.Rows.Add(m_row);
            foreach (ControllerUserVar p in structure.Vars)
            {
                m_row = this.VarsTable.NewRow();
                m_row[0] = p.Name;
                m_row[1] = structure.Name;
                m_row[2] = structure.Memory;
                m_row[3] = m_addrass + p.Address;
                m_row[4] = p.Size;
                m_row[5] = p.HasSign;
                this.VarsTable.Rows.Add(m_row);
            }
        }

        /// <summary>
        /// Заполненеие списка структур
        /// </summary>
        private void FillCheckedList()
        {
            foreach (DataRow dr in this.StructsTable.Rows)
            {
                this.clb_structurs.Items.Add(dr[0].ToString());
            }
        }

        /// <summary>
        /// Добавление структуры в таблицу опрашиваемых
        /// </summary>
        /// <param name="sructName"></param>
        private void AddStruct(string structName)
        {
            DataRow m_row;
            //Запоминаем что читаем эту переменную в параметры отладчика
            Kontel.Relkon.DebuggerParameters.StructDescription m_StructDescription = new Kontel.Relkon.DebuggerParameters.StructDescription();
            foreach(DataRow dr in this.StructsTable.Rows)
            {
                if (dr[0].ToString() == structName)
                {
                    m_StructDescription.Address = (int)dr[1];
                    m_StructDescription.MemoryType =GetMemoryType(dr[2].ToString());
                    m_StructDescription.Name = dr[0].ToString();
                    m_StructDescription.Size = (int)dr[3];
                    m_StructDescription.Type = 0;
                    break;
                }
            }

            foreach (DataRow dr in this.VarsTable.Rows)
            {
                if (dr[1].ToString() == structName)
                {
                    //Добавление переменной структуры в таблицу опрашиваемых
                    m_row = this.DisplayVarsTable.NewRow();
                    m_row[0] = dr[0];
                    m_row[1] = dr[1];
                    m_row[2] = 0;
                    m_row[5] = dr[2];
                    m_row[6] = dr[3];
                    m_row[7] = dr[4];
                    if ((bool)dr[5])
                    {
                        switch ((int)dr[4])
                        {
                            case 1:
                                m_row[8] = sbyte.MaxValue;
                                m_row[9] = sbyte.MinValue;
                                break;
                            case 2:
                                m_row[8] = short.MaxValue;
                                m_row[9] = short.MinValue;
                                break;
                            case 4:
                                m_row[8] = int.MaxValue;
                                m_row[9] = int.MinValue;
                                break;
                        }
                    }
                    else
                    {
                        switch ((int)dr[4])
                        {
                            case 1:
                                m_row[8] = byte.MaxValue;
                                m_row[9] = byte.MinValue;
                                break;
                            case 2:
                                m_row[8] = ushort.MaxValue;
                                m_row[9] = ushort.MinValue;
                                break;
                            case 4:
                                m_row[8] = uint.MaxValue;
                                m_row[9] = uint.MinValue;
                                break;
                        }
                    }
                    m_row[3] = BuildDisplayAddress((int)dr[3]);
                    m_row[4] = BuildDisplayValue((byte[])m_row[2], (int)m_row[7], (long)m_row[9] < 0);
                    this.DisplayVarsTable.Rows.Add(m_row);
                    AddRowToAsk(m_row);
                    //Добавление переменной к структуре
                    Kontel.Relkon.DebuggerParameters.VarDescription m_VarDescription = new Kontel.Relkon.DebuggerParameters.VarDescription();
                    m_VarDescription.Address = (int)dr[3];
                    m_VarDescription.MemoryType = GetMemoryType(dr[2].ToString());
                    m_VarDescription.Name = dr[0].ToString();
                    m_VarDescription.Size = (int)dr[4];
                    m_StructDescription.Vars.Add(m_VarDescription);
                }
            }
            _engine.Parameters.ReadingStructs.Add(m_StructDescription);
            
            //Добавление пустой строки для разделения структур
            m_row = this.DisplayVarsTable.NewRow();
            this.DisplayVarsTable.Rows.Add(m_row);
        }

        /// <summary>
        /// Добавление переменной структуры на опрос
        /// </summary>
        /// <param name="m_row"></param>
        private void AddRowToAsk(DataRow m_row)
        {
            if (this._IsReading)
            {
                try { _engine.AddReadItem((int)m_row[6], GetMemoryType(m_row[5].ToString()), (int)m_row[7], m_row, null, CallBack); }
                catch { }
            }
        }

        /// <summary>
        /// Удаление переменной структуры с опроса
        /// </summary>
        /// <param name="m_row"></param>
        private void RemoveRowFromAsk(DataRow m_row)
        {
            try { _engine.RemoveReadItem((int)m_row[6], GetMemoryType(m_row[5].ToString()), m_row); }
            catch { }
        }

        /// <summary>
        /// Возвращает тип памяти по ее текстовому описанию
        /// </summary>
        /// <param name="memory"></param>
        /// <returns></returns>
        private MemoryType GetMemoryType(string memory)
        {
            if (MemoryType.Clock.ToString() == memory)
                return MemoryType.Clock;
            else if (MemoryType.EEPROM.ToString() == memory)
                return MemoryType.EEPROM;
            else if (MemoryType.Flash.ToString() == memory)
                return MemoryType.Flash;
            else if (MemoryType.FRAM.ToString() == memory)
                return MemoryType.FRAM;
            else if (MemoryType.RAM.ToString() == memory)
                return MemoryType.RAM;
            else if (MemoryType.SDCard.ToString() == memory)
                return MemoryType.SDCard;
            else if (MemoryType.XRAM.ToString() == memory)
                return MemoryType.XRAM;
            return MemoryType.RAM;
        }

        /// <summary>
        /// Формирование адреса для отображения
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string BuildDisplayAddress(int address)
        {
            return ("0x00000000".Insert("0x00000000".Length - ("" + AppliedMath.DecToHex(address)).Length, "" + AppliedMath.DecToHex(address))).Substring(0, 10);
        }

        /// <summary>
        /// Формирование значения переменной для отображения
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string BuildDisplayValue(byte[] buffer,int size,bool hasSign)
        {
            if (buffer == null || size==0)
                return "Ошибка";

            byte[] Buffer = buffer;
            long Value = 0;

            if (_engine.Parameters.InverseByteOrder) Array.Reverse(Buffer);
            Value = AppliedMath.BytesToLong(Buffer);
            //Определение типа переменной
            if (hasSign)
            {
                switch (size)
                {
                    case 1: Value = (sbyte)Value; break;
                    case 2: Value = (Int16)Value; break;
                    case 4: Value = (Int32)Value; break;
                }
            }
            else
            {
                switch (size)
                {
                    case 1: Value = (Byte)Value; break;
                    case 2: Value = (UInt16)Value; break;
                    case 4: Value = (UInt32)Value; break;
                }
            }
            //Формирвание прочитанного значения
            String m_StrVar = this._EditCellText;
            switch (codingType)
                {
                    case 16:
                        String m_Str = Convert.ToString(Value, codingType);
                        if (m_Str.Length > size * 2)
                        {
                            int m_CutIndex = m_Str.Length - size * 2;
                            m_Str = m_Str.Substring(m_CutIndex);
                        }
                        m_Str = "0x" + Utils.AddChars('0', m_Str, 8/*(int)m_row.ItemArray[4] * 2*/);
                        m_StrVar = m_Str.ToUpper();
                        break;
                    default:
                        m_StrVar = Convert.ToString(Value, codingType).ToUpper();
                        break;
                }
            return m_StrVar.Replace('X', 'x');
        }

        /// <summary>
        /// Смена кодировки представления
        /// </summary>
        /// <param name="HEX"></param>
        public override void UpdateDataPresentation(bool HEX)
        {
            base.UpdateDataPresentation(HEX);
            foreach (DataRow dr in this.DisplayVarsTable.Rows)
            {
                if (dr[0].ToString().Trim() != "")
                    dr[4] = BuildDisplayValue((byte[])dr[2], (int)dr[7], (long)dr[9] < 0);
            }
        }

        /// <summary>
        /// Удаление структуры в таблицу опрашиваемых
        /// </summary>
        /// <param name="sructName"></param>
        private void RemoveStruct(string sructName)
        {
            DataRow dr;
            for (int i = this.DisplayVarsTable.Rows.Count - 1; i >= 0; i--)
            {
                dr = this.DisplayVarsTable.Rows[i];
                RemoveRowFromAsk(dr);
                //удаление переменной из параметров отладчика
                foreach (Kontel.Relkon.DebuggerParameters.StructDescription sd in _engine.Parameters.ReadingStructs)
                    if (sd.Name == sructName)
                    {
                        _engine.Parameters.ReadingStructs.Remove(sd);
                        break;
                    }
                //удаление переменной с опроса
                if (dr[1].ToString() == sructName)
                {
                    this.DisplayVarsTable.Rows.Remove(dr);
                    if (i<this.DisplayVarsTable.Rows.Count && this.DisplayVarsTable.Rows[i][1].ToString().Trim() == "")
                        this.DisplayVarsTable.Rows.Remove(this.DisplayVarsTable.Rows[i]);
                }
            }
        }

        /// <summary>
        /// Реакция на изменение выбранных для опроса структур
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clb_structurs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //добавление новых переменных
            if (e.NewValue == CheckState.Checked)
            {
                AddStruct(((CheckedListBox)sender).Items[e.Index].ToString());
                return;
            }
            //удаление лишних переменных
            if (e.NewValue == CheckState.Unchecked)
            {
                RemoveStruct(((CheckedListBox)sender).Items[e.Index].ToString());
                return;
            }
        }

        /// <summary>
        /// Удаление запросов с опроса, когда вкладка не видимая
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewStructursTabbedDocument_Leave(object sender, EventArgs e)
        {
            //IsOpen-возможно показывает виден ли документ
            if ((this.IsOpen) && (!this._IsReading))
            {
                this._IsReading = true;
                //Запуск всех опросов
                foreach (DataRow dr in this.DisplayVarsTable.Rows)
                {
                    if (dr[0].ToString().Trim() != "")
                        AddRowToAsk(dr);
                }
            }
            else
            {
                if ((this._IsReading) && (!this.IsOpen))
                {
                    this._IsReading = false;
                    //Удаление всех запросов
                    foreach (DataRow dr in this.DisplayVarsTable.Rows)
                    {
                        if (dr[0].ToString().Trim() != "")
                            RemoveRowFromAsk(dr);
                    }
                }
            }
        }

        /// <summary>
        /// Событие при закрытии вкладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewStructursTabbedDocument_Closed(object sender, EventArgs e)
        {
            _engine.EngineStatusChanged -= new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
        }

        /// <summary>
        /// Запоминание предыдущего значения при начале редактирования ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_askStructs_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this._EditCellText = (String)this.dgv_askStructs.Rows[this.dgv_askStructs.CurrentRow.Index].Cells[2].Value;
            this._EditRow = this.DisplayVarsTable.Rows[this.dgv_askStructs.CurrentRow.Index];
        }

        /// <summary>
        /// Записть измененного знаячения при завершении редактирования ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_askStructs_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Определение правильности введенного значения
            bool m_error = false;
            //Определение изменяемой переменной
            DataRow m_CurrentRow = (DataRow)this.DisplayVarsTable.Rows[this.dgv_askStructs.CurrentRow.Index];
            //Определения правльности входных данных. Формирование массива для записи
            Byte[] m_Vars = new Byte[4];
            Byte[] m_WriteVars = new Byte[(int)m_CurrentRow[7]];
            int m_SignValue = 0;
            String m_CurrentValue = "";
            if (this.dgv_askStructs[e.ColumnIndex, e.RowIndex].Value.ToString().Trim() != "")
            {
                if (this.codingType == 10)
                    m_CurrentValue = (String)(this.dgv_askStructs[e.ColumnIndex, e.RowIndex].Value);
                else
                {
                    if ((int)m_CurrentRow[7] == 4) m_CurrentValue = Convert.ToString((UInt32)(AppliedMath.HexToDec((String)this.dgv_askStructs[e.ColumnIndex, e.RowIndex].Value)));
                    else m_CurrentValue = Convert.ToString(AppliedMath.HexToDec((String)this.dgv_askStructs[e.ColumnIndex, e.RowIndex].Value));
                    if (this.dgv_askStructs[e.ColumnIndex, e.RowIndex].Value.ToString().Contains("-")) m_error = true;
                }
                try { long i = long.Parse(m_CurrentValue); }
                catch { m_error = true; }
            }
            else m_error = true;
            if ((!m_error) && ((Convert.ToInt64(m_CurrentValue) <= (long)m_CurrentRow[8]) && (Convert.ToInt64(m_CurrentValue) >= (long)m_CurrentRow[9])))
            {
                //Преобразование
                if (!((long)m_CurrentRow[9] < 0 || (int)m_CurrentRow[7] == 4))
                {
                    UInt32 m_Value = 0;
                    m_Value = Convert.ToUInt32(m_CurrentValue);
                    for (int i = 0; i < 4; i++)
                    {
                        m_Vars[i] = (Byte)(m_Value % 256);
                        m_Value = m_Value / 256;
                    }
                    Array.Reverse(m_Vars);
                    m_SignValue = AppliedMath.BytesToInt(m_Vars);
                }
                else
                {
                    m_SignValue = Convert.ToInt32(m_CurrentValue);
                }
                m_Vars = AppliedMath.IntToBytes(m_SignValue);
                Array.Reverse(m_Vars);
                for (int i = 0; i < m_WriteVars.Length; i++) m_WriteVars[i] = m_Vars[i];
                if (!_engine.Parameters.InverseByteOrder) Array.Reverse(m_WriteVars);
            }
            else { m_error = true; }
            if (m_error)
            {
                this.dgv_askStructs[e.ColumnIndex, e.RowIndex].Value = this._EditCellText;
            }
            else
            {
                //Запись переменной в память
                _engine.AddWriteItem((int)m_CurrentRow[6], GetMemoryType(m_CurrentRow[5].ToString()), m_WriteVars, "vars_write_" + m_CurrentRow[1].ToString() + "_" + m_CurrentRow[0].ToString(), null, null);
            }
            this._EditRow = null;
        }


    }
}
