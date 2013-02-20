using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Kontel.Relkon.Solutions;
using System.Data;
using Kontel.Relkon.Components.Documents;
using Kontel.Relkon;
using System.Windows.Forms;
using System.Collections;
using Kontel.Relkon.Classes;
using Kontel.Relkon.Debugger;
using System.Text.RegularExpressions;

namespace Kontel.Relkon.Components.Documents
{
    public sealed partial class ViewVarsTabbedDocument : DebuggerTabbedDocument
    {
        private ControllerProgramSolution _solution = null;//Текущий проект
        private DebuggerEngine _engine = null; // движок отладчика
        private SortedList<String, Kontel.Relkon.Classes.ControllerVar> _vars = new SortedList<string, Kontel.Relkon.Classes.ControllerVar>();//Список опрашиваемых переменных
        private List<Kontel.Relkon.Classes.ControllerVar> _AddedVars = new List<Kontel.Relkon.Classes.ControllerVar>();//Список добвленных в проект переменных
        private List<Kontel.Relkon.Classes.ControllerVar>[] _RemovedVars = new List<Kontel.Relkon.Classes.ControllerVar>[4];//Список удаленных из проекта переменных
        private bool _IsHeader = false;//Шелкнуто на заголовок или на ячейку в таблице
        private String _EditVar = "";//Изменяемая в данный момент переменная
        private string _EditCellText = "0";//Значение на которое заменятеся значение в ячейке редактирования при неправлиьном вводе данных
        private System.Windows.Forms.OpenFileDialog _OpenFileDialog = new System.Windows.Forms.OpenFileDialog();//Окно открытия файла проекта
        private bool _IsReading = false;//Опрашиваются ли переменные(видна ли вкладака)
        private bool _EmptyProgect;//Является проект сгенерированным


        private System.Data.DataTable Table
        {
            get
            {
                //return null;
                return (System.Data.DataTable)this.viewVarsDataSet.Tables[0];
                //return (VeiwVarsDataSet.VarsDataTable)this.veiwVarsDataSet.Tables[0];
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="solution"></param>
        public ViewVarsTabbedDocument(ControllerProgramSolution solution, DebuggerEngine engine)
            : base(solution, engine)
        {
          
            InitializeComponent();
            _engine = engine;
            _solution = solution;
            if (_solution == null) 
            { 
                _solution = ControllerProgramSolution.Create(_engine.Parameters.ProcessorType); 
                _EmptyProgect = true; 
            }
            else 
                _EmptyProgect = false;
            for (int i = 0; i < _RemovedVars.Length; i++)
                _RemovedVars[i] = new List<ControllerVar>();            
            RebuildTree();
            this.dgVars.Columns[1].ReadOnly = false;
            
        }

        protected override string ProtectedTabText
        {
            get
            {
                return "Переменные контроллера";
            }
        }


        /// <summary>
        /// Установка начальных значений при создании формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewVarsTabbedDocument_Load(object sender, EventArgs e)
        {
            _engine.EngineStatusChanged += new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
            if ((_engine.EngineStatus == DebuggerEngineStatus.Started) && (_solution != null))
            {
                this.dgVars.Columns[1].ReadOnly = false;
            }
            else
            {
                this.dgVars.Columns[1].ReadOnly = true;
            }
            ////Загрузка параметров
            this.Update(_solution, _engine);

            //this.dgVars.Sort(this.dgVars.Columns["nameDataGridViewTextBoxColumn"], ListSortDirection.Ascending);
            this.dgVars.Sort(this.dgVars.Columns[0], ListSortDirection.Ascending);
        }

        /// <summary>
        /// Загрузка новых параметров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Update(ControllerProgramSolution solution, DebuggerEngine engine)
        {
            //очистка значений
            //_AddedVars.Clear();
            //_RemovedVars[0].Clear();
            //_RemovedVars[1].Clear();
            //_RemovedVars[2].Clear();
            //_RemovedVars[3].Clear();
            //_solution = null;
            //_engine = null;
            //_vars.Clear();
            //this.Table.Rows.Clear();
            //RemoveNewVars();
            if (_EmptyProgect)
            {
                this.Table.Rows.Clear();
                _vars.Clear();
            }

            _engine = engine;
            if (solution != null/* && _solution != solution*/)
            {
                if (_solution != solution) _EmptyProgect = false;
                _solution = solution;
            }
            else
            {
                RemoveNewVars();
                _solution = ControllerProgramSolution.Create(_engine.Parameters.ProcessorType);
            }
            if (_solution != null)
            {
                Kontel.Relkon.DebuggerParameters.VarDescription[] m_Vars = this._engine.Parameters.ReadingVars.ToArray();
                for (int i = 0; i < dgVars.Rows.Count; )
                {
                    this.dgVars_CellMouseClick(null, new DataGridViewCellMouseEventArgs(0, 0, 0, 0, new MouseEventArgs(MouseButtons.Left, 2, 0, 0, 0)));
                    this.dgVars_DoubleClick(null, new EventArgs());
                }
                //Добавление в проект недостающийх переменных
                for (int j = 0; j < m_Vars.Length; j++)
                {
                    //Надо проверить на наличие такой переменнйо в проекте
                    if (_solution.Vars.GetVarByName(m_Vars[j].Name) == null || _solution.Vars.GetVarByName(m_Vars[j].Name).Address==0)
                    {
                        if (_solution.Vars.GetSystemVar(m_Vars[j].Name) != null && _solution.Vars.GetSystemVar(m_Vars[j].Name).Address == 0)
                        {
                            _RemovedVars[0].Add(_solution.Vars.GetVarByName(m_Vars[j].Name));
                            _solution.Vars.SystemVars.Remove((Kontel.Relkon.Classes.ControllerSystemVar)_solution.Vars.GetVarByName(m_Vars[j].Name));
                        }
                        if (_solution.Vars.GetEmbeddedVar(m_Vars[j].Name) != null && _solution.Vars.GetEmbeddedVar(m_Vars[j].Name).Address == 0)
                        {
                            _RemovedVars[1].Add(_solution.Vars.GetVarByName(m_Vars[j].Name));
                            _solution.Vars.EmbeddedVars.Remove((Kontel.Relkon.Classes.ControllerEmbeddedVar)_solution.Vars.GetVarByName(m_Vars[j].Name));
                        }
                        if (_solution.Vars.GetIOVar(m_Vars[j].Name) != null && _solution.Vars.GetIOVar(m_Vars[j].Name).Address == 0)
                        {
                            _RemovedVars[2].Add(_solution.Vars.GetVarByName(m_Vars[j].Name));
                            _solution.Vars.IOVars.Remove((Kontel.Relkon.Classes.ControllerIOVar)_solution.Vars.GetVarByName(m_Vars[j].Name));
                        }
                        if (_solution.Vars.GetUserVar(m_Vars[j].Name) != null && _solution.Vars.GetUserVar(m_Vars[j].Name).Address == 0)
                        {
                            _RemovedVars[3].Add(_solution.Vars.GetVarByName(m_Vars[j].Name));
                            _solution.Vars.UserVars.Remove((Kontel.Relkon.Classes.ControllerUserVar)_solution.Vars.GetVarByName(m_Vars[j].Name));
                        }
                        switch (m_Vars[j].Type)
                        {
                            case 0:
                                Kontel.Relkon.Classes.ControllerSystemVar m_var1 = new Kontel.Relkon.Classes.ControllerSystemVar();
                                m_var1.Address = m_Vars[j].Address;
                                m_var1.HasSign = m_Vars[j].HasSign;
                                m_var1.Memory = m_Vars[j].MemoryType;
                                m_var1.Name = m_Vars[j].Name;
                                m_var1.Size = m_Vars[j].Size;
                                _solution.Vars.SystemVars.Add(m_var1);
                                _AddedVars.Add((Kontel.Relkon.Classes.ControllerVar)m_var1);
                                break;
                            case 1:
                                Kontel.Relkon.Classes.ControllerEmbeddedVar m_var2 = new Kontel.Relkon.Classes.ControllerEmbeddedVar();
                                m_var2.Address = m_Vars[j].Address;
                                m_var2.HasSign = m_Vars[j].HasSign;
                                m_var2.Memory = m_Vars[j].MemoryType;
                                m_var2.Name = m_Vars[j].Name;
                                m_var2.Size = m_Vars[j].Size;
                                _solution.Vars.EmbeddedVars.Add(m_var2);
                                _AddedVars.Add((Kontel.Relkon.Classes.ControllerVar)m_var2);
                                break;
                            case 2:
                                Kontel.Relkon.Classes.ControllerIOVar m_var3 = new Kontel.Relkon.Classes.ControllerIOVar();
                                m_var3.Address = m_Vars[j].Address;
                                m_var3.HasSign = m_Vars[j].HasSign;
                                m_var3.Memory = m_Vars[j].MemoryType;
                                m_var3.Name = m_Vars[j].Name;
                                m_var3.Size = m_Vars[j].Size;
                                _solution.Vars.IOVars.Add(m_var3);
                                _AddedVars.Add((Kontel.Relkon.Classes.ControllerVar)m_var3);
                                break;
                            case 3:
                                Kontel.Relkon.Classes.ControllerUserVar m_var4 = new Kontel.Relkon.Classes.ControllerUserVar();
                                m_var4.Address = m_Vars[j].Address;
                                m_var4.HasSign = m_Vars[j].HasSign;
                                m_var4.Memory = m_Vars[j].MemoryType;
                                m_var4.Name = m_Vars[j].Name;
                                m_var4.Size = m_Vars[j].Size;
                                _solution.Vars.UserVars.Add(m_var4);
                                _AddedVars.Add((Kontel.Relkon.Classes.ControllerVar)m_var4);
                                break;
                            default:
                                break;
                        }
                    }
                }
                RebuildTree();
                //Создание списка опрашиваемых переменных
                for (int j = 0; j < m_Vars.Length; j++)
                {
                    bool m_IsFined = false;
                    for (int k = 0; k < 4; k++)
                    {
                        for (int i = 0; i < this.tvVars.Nodes[k].Nodes.Count; i++)
                        {
                            if (this.tvVars.Nodes[k].Nodes[i].Text.CompareTo(m_Vars[j].Name) == 0)
                            {
                                try
                                {
                                    this.tvVars_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(this.tvVars.Nodes[k].Nodes[i], MouseButtons.Left, 2, 0, 0));
                                    m_IsFined = true;
                                }
                                catch { }
                                break;
                            }
                        }
                        if (m_IsFined) break;
                    }
                    _engine.Parameters.ReadingVars.Remove(m_Vars[j]);
                    for (int i = 0; i < _engine.Parameters.ReadingVars.Count; i++)
                        if (_engine.Parameters.ReadingVars[i].Name == m_Vars[j].Name) m_Vars[j] = _engine.Parameters.ReadingVars[i];
                    if ((m_IsFined) && (!this._engine.Parameters.ReadingVars.Contains(m_Vars[j])))
                        this._engine.Parameters.ReadingVars.Add(m_Vars[j]);
                }
                _IsReading = true;
            }
        }

        /// <summary>
        /// Удаление из проекта добавленых туда переменных
        /// </summary>
        private void RemoveNewVars()
        {
            for (int i = _AddedVars.Count-1; i>=0; i--)
            {
                try { _solution.Vars.SystemVars.Remove((Kontel.Relkon.Classes.ControllerSystemVar)_AddedVars[i]); }
                catch { }
                try { _solution.Vars.UserVars.Remove((Kontel.Relkon.Classes.ControllerUserVar)_AddedVars[i]); }
                catch { }
                try { _solution.Vars.IOVars.Remove((Kontel.Relkon.Classes.ControllerIOVar)_AddedVars[i]); }
                catch { }
                try { _solution.Vars.EmbeddedVars.Remove((Kontel.Relkon.Classes.ControllerEmbeddedVar)_AddedVars[i]); }
                catch { }
            }
            _AddedVars.Clear();
            for (int i = 0; i < _RemovedVars[0].Count; i++)
            {
                _solution.Vars.SystemVars.Add((Kontel.Relkon.Classes.ControllerSystemVar)_RemovedVars[0][i]);
            }
            _RemovedVars[0].Clear();
            for (int i = 0; i < _RemovedVars[1].Count; i++)
            {
                _solution.Vars.EmbeddedVars.Add((Kontel.Relkon.Classes.ControllerEmbeddedVar)_RemovedVars[1][i]);
            }
            _RemovedVars[1].Clear();
            for (int i = 0; i < _RemovedVars[2].Count; i++)
            {
                _solution.Vars.IOVars.Add((Kontel.Relkon.Classes.ControllerIOVar)_RemovedVars[2][i]);
            }
            _RemovedVars[2].Clear();
            for (int i = 0; i < _RemovedVars[3].Count; i++)
            {
                _solution.Vars.UserVars.Add((Kontel.Relkon.Classes.ControllerUserVar)_RemovedVars[3][i]);
            }
            _RemovedVars[3].Clear();
        }

        /// <summary>
        /// Событие при изменении состояния отладчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DebuggerParametersList_ChangeStatusEngine(object sender, DebuggerEngineStatusChangedEventArgs e)
        {
            if ((_engine.EngineStatus == DebuggerEngineStatus.Started) && (_solution != null) && (this._IsReading))
            {
                this.dgVars.Columns[1].ReadOnly = false;
            }
            else
            {
                this.dgVars.Columns[1].ReadOnly = true;
            }
        }

        /// <summary>
        /// Событтие при двойном щелчке по дереву не прочитаннх переменных
        /// (формировани запроса на их чтение)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvVars_NodeMouseDoubleClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            if ((e.Node.Nodes.Count == 0) && ((e.Node.Text.CompareTo("Системные переменные") != 0) && (e.Node.Text.CompareTo("Пользовательские переменные") != 0) && (e.Node.Text.CompareTo("Заводские установки") != 0)))
            {
                Kontel.Relkon.Classes.ControllerVar m_newVar = _solution.Vars.GetVarByName(e.Node.Text);
                if (m_newVar == null) return;

                if (!_vars.ContainsKey(e.Node.Text))
                {
                    _vars.Add(e.Node.Text, m_newVar);

                    //Добавление записи с читаемой переменной
                  

                    Int64 m_Max = 0;
                    Int64 m_Min = 0;
                    if (m_newVar.HasSign)
                    {
                        switch (m_newVar.Size)
                        {
                            case 1: m_Max = sbyte.MaxValue; m_Min = sbyte.MinValue; break;
                            case 2: m_Max = Int16.MaxValue; m_Min = Int16.MinValue; break;
                            case 4: m_Max = Int32.MaxValue; m_Min = Int32.MinValue; break;
                            default: m_Max = sbyte.MaxValue; m_Min = sbyte.MinValue; break;
                        }
                    }
                    else
                    {
                        switch (m_newVar.Size)
                        {
                            case 1: m_Max = Byte.MaxValue; m_Min = Byte.MinValue; break;
                            case 2: m_Max = UInt16.MaxValue; m_Min = UInt16.MinValue; break;
                            case 4: m_Max = UInt32.MaxValue; m_Min = UInt32.MinValue; break;
                            default: m_Max = Byte.MaxValue; m_Min = Byte.MinValue; break;
                        }
                    }

                    DataRow m_row = this.Table.NewRow();
                    m_row[0] = m_newVar.Name;
                    m_row[2] = m_Max;
                    m_row[3] = m_Min;
                    m_row[4] = m_newVar.Size;
                    m_row[5] = m_newVar.Address;
                    m_row[6] = m_newVar.Memory;                  
                    m_row[7] = ("0x00000000".Insert("0x00000000".Length - ("" + AppliedMath.DecToHex(m_newVar.Address)).Length, "" + AppliedMath.DecToHex(m_newVar.Address))).Substring(0, 10);
                    m_row[8] = new byte[m_newVar.Size];
                    m_row[9] = m_newVar.Real;
                    this.BuildDisplayValue(m_row);

                    this.Table.Rows.Add(m_row);
                    //Запоминаем что читаем эту переменную в параметры отладчика
                    Kontel.Relkon.DebuggerParameters.VarDescription m_VarDescription = new Kontel.Relkon.DebuggerParameters.VarDescription();
                    m_VarDescription.Address = m_newVar.Address;
                    m_VarDescription.MemoryType = m_newVar.Memory;
                    m_VarDescription.Name = m_newVar.Name;
                    m_VarDescription.Size = m_newVar.Size;
                    m_VarDescription.Real = m_newVar.Real;
                    m_VarDescription.Type = e.Node.Parent.Index;
                    _engine.Parameters.ReadingVars.Add(m_VarDescription);
                    //Добавление переменной на опрос
                    if (this._IsReading)
                    {
                        for (int i = 0; i < this.Table.Rows.Count; i++)
                        {
                            if (this.Table.Rows[i][0].ToString() == m_newVar.Name)
                            {
                                try { _engine.AddReadItem(m_newVar.Address, m_newVar.Memory, m_newVar.Size, this.Table.Rows[i], null, CallBack); }
                                catch { }
                            }
                        }
                    }
                }
                this.tvVars.Nodes.Remove(e.Node);
            }
        }

        /// <summary>
        /// Добавление переменной в таблицу, когда она прочитана
        /// </summary>
        /// <param name="sender"></param>
        private void CallBack(object Marker, byte[] Buffer, bool Error)
        {
            try
            {
                DataRow m_row = ((DataRow)Marker);
                if (m_row.RowState != System.Data.DataRowState.Detached && m_row[0].ToString().CompareTo(this._EditVar) != 0)
                {
                    if (Buffer.Length == 0 || Error)
                    {
                        return;
                    }
                    m_row[8] = Buffer;
                    BuildDisplayValue(m_row);
                }
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Построение прочитанног значения в зависимости от кодировки
        /// </summary>
        /// <param name="row"></param>
        private void BuildDisplayValue(DataRow row)
        {
            DataRow m_row = row;
            byte[] Buffer = (byte[])m_row.ItemArray[8];

            bool real = (bool)row.ItemArray[9];
            int size = (int)row.ItemArray[4];

            long Value = 0;
            double dValue = 0;

            if (_engine.Parameters.InverseByteOrder && !real)
                Array.Reverse(Buffer);

            if (real)
            {

                if (size == 8)
                    dValue = BitConverter.ToDouble(Buffer, 0);
                else
                    dValue = (double)BitConverter.ToSingle(Buffer, 0);
            }
            else
            {
                Value = AppliedMath.BytesToLong(Buffer);


                if (_vars[m_row.ItemArray[0].ToString()].HasSign)
                {
                    switch ((int)m_row.ItemArray[4])
                    {
                        case 1: Value = (sbyte)Value; break;
                        case 2: Value = (Int16)Value; break;
                        case 4: Value = (Int32)Value; break;
                    }
                }
                else
                {
                    switch ((int)m_row.ItemArray[4])
                    {
                        case 1: Value = (Byte)Value; break;
                        case 2: Value = (UInt16)Value; break;
                        case 4: Value = (UInt32)Value; break;
                    }
                }
            }

            //Добавление прочитанного значения
            String m_StrVar = this._EditCellText;
            String varName = m_row.ItemArray[0].ToString();
            bool array = false;

            array = _vars[varName] is ControllerUserVar && ((ControllerUserVar)_vars[varName]).Array;

            if (!array)
                array = _vars[varName] is ControllerSystemVar && ((ControllerSystemVar)_vars[varName]).Array;

            if (array)
            {//обработка значения если это массив              
                switch (codingType)
                {
                    case 16:
                        String m_Str = "";
                        String m_Byte = "";
                        foreach (Byte m_byte in Buffer)
                        {
                            m_Byte = Convert.ToString(m_byte, codingType);
                            switch (m_Byte.Length)
                            {
                                case 0: m_Byte = "00" + m_Byte; break;
                                case 1: m_Byte = "0" + m_Byte; break;
                                default: break;
                            }
                            m_Str += m_Byte + " ";
                        }
                        m_StrVar = m_Str.ToUpper();
                        break;
                    default:
                        String m_Str1 = "";
                        foreach (Byte m_byte in Buffer)
                            m_Str1 += Convert.ToString(m_byte, codingType) + " ";
                        m_StrVar = m_Str1.ToUpper();
                        break;
                }
            }
            else
            {
                switch (codingType)
                {
                    case 16:
                        String m_Str = Convert.ToString(Value, codingType);
                        if (m_Str.Length > (int)m_row.ItemArray[4] * 2)
                        {
                            int m_CutIndex = m_Str.Length - (int)m_row.ItemArray[4] * 2;
                            m_Str = m_Str.Substring(m_CutIndex);
                        }
                        m_Str = "0x" + Utils.AddChars('0', m_Str, 8/*(int)m_row.ItemArray[4] * 2*/);
                        m_StrVar = m_Str.ToUpper();
                        break;
                    default:
                        if (real)
                            m_StrVar = dValue.ToString("F2");
                        else
                            m_StrVar = Convert.ToString(Value, codingType).ToUpper();
                        break;
                }
            }
            m_row[1] = m_StrVar.Replace('X', 'x');
        }

        /// <summary>
        /// Событие при двойном щелчке по таблице прочитанных переменных
        /// (перенос их в дерево)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgVars_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (_IsHeader) { _IsHeader = false; return; }

                //Если поле не редактируемое, то переместить переменную
                if ((this.dgVars.RowCount > 0) && (this.dgVars.CurrentCell.ReadOnly == true))
                {
                    //Добавление переменной в дерево
                    String m_name = (String)this.dgVars.CurrentRow.Cells[0].Value;
                    Kontel.Relkon.Classes.ControllerVar m_newVar = _solution.Vars.GetVarByName(m_name);
                    if (m_newVar == null)
                    {
                        //если написать строку,то не отображаются переменные, которых нет в _solution, но есть в _enging
                        //иначевозникает ошибка при открытии проекта, послеоткрытия файла отладчика
                        //this.Table.Rows.Remove(this.Table.Rows[this.dgVars.CurrentRow.Index]);
                        return;
                    }
                    DataRow m_CurrentRow = (DataRow)this.Table.Rows[this.dgVars.CurrentRow.Index];
                    for (int i = 0; i < this.Table.Rows.Count; i++)
                    {
                        if ((String)this.Table.Rows[i][0] == m_name)
                        {
                            m_CurrentRow = (DataRow)this.Table.Rows[i];
                            break;
                        }
                    }

                    if (m_newVar is ControllerSystemVar)
                        this.tvVars.Nodes["sys"].Nodes.Add(m_newVar.Name);
                    else if (m_newVar is ControllerEmbeddedVar)
                        this.tvVars.Nodes["emb"].Nodes.Add(m_newVar.Name);
                    else if (m_newVar is ControllerIOVar)
                        this.tvVars.Nodes["io"].Nodes.Add(m_newVar.Name);
                    else if (m_newVar is ControllerUserVar)
                        this.tvVars.Nodes["usr"].Nodes.Add(m_newVar.Name);                  
                    else
                        return;                                             
                  
                    //Удаление переменной из списка опрашиваемых
                    if (_vars.ContainsKey((String)m_newVar.Name))
                    {
                        for (int i = 0; i < _engine.Parameters.ReadingVars.Count; i++)
                        {
                            if (_engine.Parameters.ReadingVars[i].Name == m_newVar.Name)
                            {
                                Kontel.Relkon.DebuggerParameters.VarDescription m_VarDescription = _engine.Parameters.ReadingVars[i];
                                _engine.Parameters.ReadingVars.Remove(m_VarDescription);
                            }
                        }
                        _engine.RemoveReadItem(m_newVar.Address, m_newVar.Memory, m_CurrentRow/*.ItemArray[0].ToString()*/);
                        _vars.Remove((String)m_newVar.Name);
                    }
                    //Удаление переменной из таблицы

                    this.Table.Rows.Remove(m_CurrentRow);
                }
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("dgVars_DoubleClick:" + ex.Message);
                return;
            }
        }

        /// <summary>
        /// Перестроение дерева переменных по проекту
        /// </summary>
        private void RebuildTree()
        {                           
            if (_solution != null)
            {
                this.tvVars.Nodes.Clear();
                TreeNode tn = new TreeNode();

                this.tvVars.Nodes.Add("sys", "Системные переменные");
                this.tvVars.Nodes.Add("emb", "Заводские установки");               
                this.tvVars.Nodes.Add("io", "Датчики ввода-вывода");
                this.tvVars.Nodes.Add("usr", "Пользовательские переменные");
                
                this.tvVars.Nodes[0].ImageIndex = 0;
                this.tvVars.Nodes[1].ImageIndex = 0;
                this.tvVars.Nodes[2].ImageIndex = 0;
                this.tvVars.Nodes[3].ImageIndex = 0;
              
                this.tvVars.Nodes[0].SelectedImageIndex = 0;
                this.tvVars.Nodes[1].SelectedImageIndex = 0;
                this.tvVars.Nodes[2].SelectedImageIndex = 0;
                this.tvVars.Nodes[3].SelectedImageIndex = 0;
                     
               
                //Заполнения дерева переменных
                for (int i = 0; i < _solution.Vars.SystemVars.Count; i++)
                {//Заполнение системных переменных
                    if (_solution.Vars.SystemVars[i].Address > -1)
                        this.tvVars.Nodes["sys"].Nodes.Add(_solution.Vars.SystemVars[i].Name);
                }                

                for (int i = 0; i < _solution.Vars.UserVars.Count; i++)
                {//Заполнение пользовательских переменных
                    if (_solution.Vars.UserVars[i].Address > -1 && !(_solution.Vars.UserVars[i] is ControllerStructVar))
                    {
                        this.tvVars.Nodes["usr"].Nodes.Add(_solution.Vars.UserVars[i].Name);
                    }
                }
                              
                for (int i = 0; i < _solution.Vars.EmbeddedVars.Count; i++)
                {//Заполнение заводских установок
                    this.tvVars.Nodes["emb"].Nodes.Add(_solution.Vars.EmbeddedVars[i].Name);               
                }

              
    
                for (int i = 0; i < _solution.Vars.IOVars.Count; i++)
                {//Заполнение входов/выходов
                    if (_solution.Vars.IOVars[i].Address > -1)
                        this.tvVars.Nodes["io"].Nodes.Add(_solution.Vars.IOVars[i].Name);                    
                }                                    
            }

            this.tvVars.Sort();

            //Удаление запросов на опрос переменных
            Kontel.Relkon.Classes.ControllerVar m_var;
            for (int i = 0; i < this.Table.Rows.Count; i++)
            {
                m_var = _solution.Vars.GetVarByName(this.Table.Rows[i].ItemArray[0].ToString());
                try
                {
                    //Удаление переменной из списка опрашиваемых
                    if (_vars.ContainsKey(this.Table.Rows[i].ItemArray[0].ToString()))
                    {
                        _vars.Remove((String)dgVars.Rows[i].Cells[0].Value);
                    }
                    _engine.RemoveReadItem(m_var.Address, m_var.Memory, this.Table.Rows[i]);
                }
                catch { }
            }
            //Очистка таблицы опрашиваемых переменных
            this.Table.Clear();          
        }              


        /// <summary>
        /// Событие при оканчании редактирования значения переменной
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgVars_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Определение правильности введенного значения
                bool m_error = false;
                //Определение изменяемой переменной
                Kontel.Relkon.Classes.ControllerVar m_newVar = _solution.Vars.GetVarByName((String)this.dgVars[0, e.RowIndex].Value);
                DataRow m_CurrentRow = (DataRow)this.Table.Rows[this.dgVars.CurrentRow.Index];
                for (int i = 0; i < this.Table.Rows.Count; i++)
                    if ((String)this.Table.Rows[i][0] == this._EditVar)
                    {
                        m_CurrentRow = (DataRow)this.Table.Rows[i];
                        break;
                    }
                //Определения правльности входных данных. Формирование массива для записи
                Byte[] m_Vars = new Byte[4];
                Byte[] m_WriteVars = new Byte[m_newVar.Size];
                int m_SignValue = 0;
                if (m_newVar is ControllerUserVar && ((ControllerUserVar)m_newVar).Array)
                {
                    String m_ArrayVar = (String)this.dgVars[e.ColumnIndex, e.RowIndex].Value;
                    String m_CurrentVar = "";
                    for (int i = 0; i < m_newVar.Size; i++)
                    {
                        if (m_ArrayVar.IndexOf(' ') > 0)
                        {
                            m_CurrentVar = m_ArrayVar.Substring(0, m_ArrayVar.IndexOf(' '));
                            m_ArrayVar = m_ArrayVar.Substring(m_ArrayVar.IndexOf(' ') + 1);
                        }
                        else
                        {
                            m_CurrentVar = m_ArrayVar;
                            m_ArrayVar = "";
                        }
                        if (this.codingType == 16)
                        {
                            try { if (!((Convert.ToInt64(Convert.ToByte(AppliedMath.HexToDec(m_CurrentVar))) <= long.Parse(m_CurrentRow[2].ToString())) && ((Convert.ToInt64(Convert.ToByte(AppliedMath.HexToDec(m_CurrentVar)))) >= long.Parse(m_CurrentRow[3].ToString())))) { m_error = true; } }
                            catch { m_error = true; }
                            if ((AppliedMath.IsValidHexNumber(m_CurrentVar)) && (!m_error))
                            {
                                m_WriteVars[i] = Convert.ToByte(AppliedMath.HexToDec(m_CurrentVar));
                            }
                            else { m_error = true; break; }
                        }
                        else
                        {
                            if ((AppliedMath.IsValidDecNumber(m_CurrentVar)) && ((Convert.ToInt64(m_CurrentVar) <= long.Parse(m_CurrentRow[2].ToString())) && (Convert.ToInt64(m_CurrentVar) >= long.Parse(m_CurrentRow[3].ToString()))))
                            {
                                m_WriteVars[i] = Convert.ToByte(m_CurrentVar);
                            }
                            else { m_error = true; break; }
                        }
                        if ((m_ArrayVar.Trim() == "") && (i < m_newVar.Size - 1))
                        {
                            m_error = true;
                            break;
                        }
                    }
                }
                else
                {
                    String m_CurrentValue = "";
                    if (this.codingType == 10)
                        m_CurrentValue = (String)(this.dgVars[e.ColumnIndex, e.RowIndex].Value);
                    else
                    {
                        if (m_newVar.Size == 4) m_CurrentValue = Convert.ToString((UInt32)(AppliedMath.HexToDec((String)this.dgVars[e.ColumnIndex, e.RowIndex].Value)));
                        else m_CurrentValue = Convert.ToString(AppliedMath.HexToDec((String)this.dgVars[e.ColumnIndex, e.RowIndex].Value));
                    }
                    try { long i = long.Parse(m_CurrentValue); }
                    catch { m_error = true; }
                    if ((!m_error) && ((Convert.ToInt64(m_CurrentValue) <= long.Parse(m_CurrentRow[2].ToString())) && (Convert.ToInt64(m_CurrentValue) >= long.Parse(m_CurrentRow[3].ToString()))))
                    {
                        //Преобразование
                        if (this.codingType == 10)
                        {
                            if (m_newVar.HasSign)
                            {
                                m_SignValue = Convert.ToInt32(this.dgVars[e.ColumnIndex, e.RowIndex].Value);
                            }
                            else
                            {
                                if (m_newVar.Size == 4)
                                {
                                    UInt32 m_Value = 0;
                                    m_Value = Convert.ToUInt32(this.dgVars[e.ColumnIndex, e.RowIndex].Value);
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
                                    m_SignValue = Convert.ToInt32(this.dgVars[e.ColumnIndex, e.RowIndex].Value);
                                }
                            }
                        }
                        else
                        {
                            m_SignValue = Convert.ToInt32(AppliedMath.HexToDec((String)this.dgVars[e.ColumnIndex, e.RowIndex].Value));
                        }
                        m_Vars = AppliedMath.IntToBytes(m_SignValue);
                        //закоментированный вариант корректно опрашивает только с обратным порядокм байт
                        //if (_engine.Parameters.InverseByteOrder) Array.Reverse(m_Vars);
                        //for (int i = 0; i < m_WriteVars.Length; i++) m_WriteVars[i] = m_Vars[i];
                        for (int i = 0; i < m_WriteVars.Length; i++) m_WriteVars[i] = m_Vars[i + (4 - m_WriteVars.Length)];
                        if (_engine.Parameters.InverseByteOrder) Array.Reverse(m_WriteVars);
                    }
                    else { m_error = true; }
                }
                if (m_error)
                {
                    this.dgVars[e.ColumnIndex, e.RowIndex].Value = this._EditCellText;
                }
                else
                {
                    //Запись переменной в память
                    _engine.AddWriteItem(m_newVar.Address, m_newVar.Memory, m_WriteVars, "vars_write_" + m_newVar.Name, null, null);
                }
                this._EditVar = "";
            }
            catch (Exception ex)
            {
                this.dgVars[e.ColumnIndex, e.RowIndex].Value = this._EditCellText;
                this._EditVar = "";
                Utils.ErrorMessage("dgVars_CellEndEdit: " + ex.Message);
                return;
            }
        }

        /// <summary>
        /// Событие при начале редактирования значения переменной
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgVars_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int i = this.dgVars.CurrentRow.Index;
            this._EditCellText = (String)dgVars.Rows[i].Cells[1].Value;
            this._EditVar = (String)dgVars.Rows[i].Cells[0].Value;
        }

        /// <summary>
        /// Событие при закрытии вкладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewVarsTabbedDocument_Closed(object sender, EventArgs e)
        {
            _engine.EngineStatusChanged -= new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
            try
            {
                RemoveNewVars();
                _AddedVars.Clear();
                //Удаление запроса на опрос переменной
                Kontel.Relkon.Classes.ControllerVar m_var;
                for (int i = 0; i < this.Table.Rows.Count; i++)
                {
                    m_var = _solution.Vars.GetVarByName(this.Table.Rows[i].ItemArray[0].ToString());
                    try { _engine.RemoveReadItem(m_var.Address, m_var.Memory, this.Table.Rows[i]); }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Если щелкнули по заголовку, то выставляется флаг запрещения переноса переменной из таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgVars_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _IsHeader = true;
        }
        /// <summary>
        /// Если щелкнули по ячейке, то выставляется флаг разрешения переноса переменной из таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgVars_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            _IsHeader = false;
        }
        /// <summary>
        /// Если щелкнули по ячейке, то выставляется флаг разрешения переноса переменной из таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgVars_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _IsHeader = false;
        }
        /// <summary>
        /// Событие при сортировки таблицы по имение переменной
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgVars_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgVars.RowCount; i++)
                try
                {
                    if ((String)this.dgVars.Rows[i].Cells[1].Value == "Ошибка")
                        dgVars.Rows[i].Cells[1].Style.ForeColor = System.Drawing.Color.Red;
                    else dgVars.Rows[i].Cells[1].Style.ForeColor = System.Drawing.Color.Black;
                }
                catch { }

        }

        /// <summary>
        /// Смена кодировки представления
        /// </summary>
        /// <param name="HEX"></param>
        public override void UpdateDataPresentation(bool HEX)
        {
            base.UpdateDataPresentation(HEX);
            for (int i = 0; i < this.Table.Rows.Count; i++)
            {
                this.BuildDisplayValue(this.Table.Rows[i]);
            }
        }

        /// <summary>
        /// Разрешение или запрещение опроса переменных, в зависимости от видимости вкладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewVarsTabbedDocument_Leave(object sender, EventArgs e)
        {
            //IsOpen-возможно показывает виден ли документ
            if ((this.IsOpen) && (!this._IsReading))
            {
                this._IsReading = true;
                //Запуск всех опросов
                Kontel.Relkon.Classes.ControllerVar m_var;
                for (int i = 0; i < this.Table.Rows.Count; i++)
                {
                    m_var = _solution.Vars.GetVarByName(this.Table.Rows[i].ItemArray[0].ToString());
                    try { _engine.AddReadItem(m_var.Address, m_var.Memory, m_var.Size, this.Table.Rows[i], null, CallBack); }
                    catch { }
                }
            }
            else
            {
                if ((this._IsReading) && (!this.IsOpen))
                {
                    this._IsReading = false;
                    //Остановка опроса всех переменных
                    try
                    {
                        Kontel.Relkon.Classes.ControllerVar m_var;
                        for (int i = 0; i < this.Table.Rows.Count; i++)
                        {
                            m_var = _solution.Vars.GetVarByName(this.Table.Rows[i].ItemArray[0].ToString());
                            try { _engine.RemoveReadItem(m_var.Address, m_var.Memory, this.Table.Rows[i]); }
                            catch { }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.ErrorMessage(ex.Message);
                        return;
                    }
                }
            }

            DebuggerParametersList_ChangeStatusEngine(null, null);
        }

    }
}
