using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Kontel.Relkon.Solutions;
using Kontel.Relkon.Debugger;

namespace Kontel.Relkon.Components.Documents
{
    public partial class ViewSituationsTabbedDocument : DebuggerTabbedDocument
    {
        private ControllerProgramSolution _solution = null;//Текущий проект
        private DebuggerEngine _engine = null; // движок отладчика
        private bool _IsReading = false;//Происходит ли опрос(видна ли вкладака)

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="solution"></param>
        public ViewSituationsTabbedDocument(ControllerProgramSolution solution, DebuggerEngine engine)
            : base(solution, engine)
        {
            InitializeComponent();
            _engine = engine;
            _solution = solution;
            if (_solution == null) { _solution = ControllerProgramSolution.Create(_engine.Parameters.ProcessorType); }
        }

        protected override string ProtectedTabText
        {
            get
            {
                return "Активные ситуации";
            }
        }

        private void ViewSituationsTabbedDocument_Load(object sender, EventArgs e)
        {
            _engine.EngineStatusChanged += new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
            DebuggerParametersList_ChangeStatusEngine(null, new Debugger.DebuggerEngineStatusChangedEventArgs(_engine.EngineStatus, null));
            ////Загрузка параметров
            this.Update(_solution, _engine);

        }

        /// <summary>
        /// Событие при изменении состояния отладчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DebuggerParametersList_ChangeStatusEngine(object sender, DebuggerEngineStatusChangedEventArgs e)
        {
            if ((_engine.EngineStatus == DebuggerEngineStatus.Stopped))
            {
                for (int i = 0; i < this.dgProcess.Rows.Count; i++)
                {
                    this.dgProcess.Rows[i].Cells[1].Value = "***";
                }
            }
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
            //Оснатовка чтения
            this.RemoveReadItems();
            //Удаление строк из таблицы
            this.dgProcess.Rows.Clear();
            //Добавление процессов из проекта в таблицу
            foreach (ProjectProcess ps in _solution.Processes)
            {
                this.dgProcess.Rows.Add(ps.Name,"***");
            }
            //Сортировка записий в таблице
            this.dgProcess.Sort(this.dgProcess.Columns[0], ListSortDirection.Ascending);
            //Запуск чтения
            this.AddedReadItems();
        }

        /// <summary>
        /// Добавление результатов проченитя в таблицу
        /// </summary>
        /// <param name="sender"></param>
        private void CallBack(object Marker, byte[] Buffer, bool Error)
        {
            System.Windows.Forms.DataGridViewRow m_marker = (System.Windows.Forms.DataGridViewRow)Marker;
            if (Buffer != null && !Error)
            {
                if (_engine.Parameters.InverseByteOrder) 
                    Array.Reverse(Buffer);
                int Value = Kontel.Relkon.AppliedMath.BytesToInt(Buffer);
                try 
                { 
                    //m_marker.Cells[1].Value = (_solution.GetProcessByName((string)m_marker.Cells[0].Value)).GetSituationByAddress(Value).Name; 
                    m_marker.Cells[1].Value = "SIT" + Value; ; 
                }
                catch 
                { 
                    m_marker.Cells[1].Value = "***"; 
                }
            }
            else
            {
                m_marker.Cells[1].Value = "***";
            }
        }

        /// <summary>
        /// Составление запросов на опрос процессов
        /// </summary>
        private void AddedReadItems()
        {
            if (!_IsReading)
            {
                for (int i = 0; i < this.dgProcess.Rows.Count; i++)
                {
                    ProjectProcess m_p = this._solution.GetProcessByName((string)this.dgProcess.Rows[i].Cells[0].Value);
                    //_engine.AddReadItem(_engine.Parameters.ProcessorType == ProcessorType.MB90F347 ? (m_p.Address + 6) : (m_p.Address + 5), MemoryType.XRAM, _engine.Parameters.ProcessorType == ProcessorType.MB90F347 ? 4 : 2, this.dgProcess.Rows[i], null, CallBack);
                    _engine.AddReadItem(m_p.Address + 4, MemoryType.XRAM, 4, this.dgProcess.Rows[i], null, CallBack);
                }
                _IsReading = true;
            }
        }

        /// <summary>
        /// Удаление запросов на опрос процессов
        /// </summary>
        private void RemoveReadItems()
        {
            if (_IsReading)
            {
                for (int i = 0; i < this.dgProcess.Rows.Count; i++)
                {
                    ProjectProcess m_p = this._solution.GetProcessByName((string)this.dgProcess.Rows[i].Cells[0].Value);
                    try { _engine.RemoveReadItem(m_p.Address + 4, MemoryType.XRAM, this.dgProcess.Rows[i]); }
                    catch { }
                }
                _IsReading = false;
            }
        }

        private void ViewSituationsTabbedDocument_VisibleChanged(object sender, EventArgs e)
        {
            //IsOpen-показывает виден ли документ
            if ((this.IsOpen) && (!this._IsReading))
            {
                this.AddedReadItems();
            }
            else if ((!this.IsOpen) && (this._IsReading))
            {
                this.RemoveReadItems();
            }
        }

        private void ViewSituationsTabbedDocument_Closed(object sender, EventArgs e)
        {
            _engine.EngineStatusChanged -= new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
            this.RemoveReadItems();
        }

    }
}
