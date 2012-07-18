using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Kontel.Relkon.Solutions;
using System.Windows.Forms;
using System.Threading;
using ZedGraph;
using System.Collections;

using System.Data;
using Kontel.Relkon.Components.Documents;
using Kontel.Relkon;
using System.Drawing;
using Kontel.Relkon.Classes;
using System.Text.RegularExpressions;
using Kontel.Relkon.Debugger;

namespace Kontel.Relkon.Components.Documents
{
    public sealed partial class ViewGraphicsTabbedDocument : DebuggerTabbedDocument
    {
        private float Smooth = 0.3F;//Величина сглаживания графиков
        private int _VisibleTime = 10;//Время отображения графиков в секундах
        private Color[] GraphicsColors = { Color.Red, Color.Blue, Color.Black, Color.Green, Color.Brown, Color.DarkOrange }; // Массив цветов графиков проекта
        private List<ComboBox> VarsComboBoxes = null; // Массив ComboBox'ов, определяющих опрашиваемые переменные
        private long[] timeStamps = null; // Содержит моменты времени, в которые последний раз были добавлены точки на график
        private bool closed = false; // Показывает, что документ был закрыт
        private ControllerProgramSolution solutionFromEngine = null;

        /// <summary>
        /// Возвращает текущий проект, преобразованный к типу ControllerProgramSolution
        /// </summary>
        private ControllerProgramSolution ControllerProgramSolution
        {
            get
            {
                if (this.Solution is ControllerProgramSolution)
                    return this.Solution as ControllerProgramSolution;
                else if (this.debuggerEngine == null)
                    return null;
                else if (this.solutionFromEngine == null)
                    return (this.solutionFromEngine = ControllerProgramSolution.Create(this.debuggerEngine.Parameters.ProcessorType));
                else
                    return this.solutionFromEngine;
            }
        }

        protected override string ProtectedTabText
        {
            get
            {
                return "Графическое отображение данных";
            }
        }

        public ViewGraphicsTabbedDocument(ControllerProgramSolution solution, DebuggerEngine Engine)
            : base(solution, Engine)
        {
            InitializeComponent();
            // Инициализация ComboBox'ов
            this.VarsComboBoxes = new List<ComboBox>() { this.cbVar_0, this.cbVar_1, this.cbVar_2, this.cbVar_3, this.cbVar_4, this.cbVar_5 };
            this.timeStamps = new long[this.VarsComboBoxes.Count];
            for (int i = 0; i < this.VarsComboBoxes.Count; i++)
            {
                this.VarsComboBoxes[i].Leave += new EventHandler(VarComboBox_Leave);
                this.VarsComboBoxes[i].SelectedIndexChanged += new EventHandler(VarComboBox_Leave);
                this.VarsComboBoxes[i].KeyDown += new KeyEventHandler(VarComboBox_KeyDown);
                this.timeStamps[i] = 0;
            }
            this.FillVarsComboBoxes();
            // Инициализация графиков
            this.InitGraphControl();
            this.Update(solution, Engine);
        }

        /// <summary>
        /// Заполняет списки ComboBox'ов переменных
        /// </summary>
        private void FillVarsComboBoxes()
        {
            // Очистка списков переменных
            for (int i = 0; i < this.VarsComboBoxes.Count; i++)
            {
                this.VarsComboBoxes[i].Items.Clear();
            }
            List<object> displayedVars = new List<object>(); // список переменных, которые будут в выпадающем списке ComboBox'ов переменных
            List<string> autoCompleetVars = new List<string>(); // список переменных для списка автозаполнения ComboBox'ов
            // Заполяем определенные выше списки
            if (this.ControllerProgramSolution != null)
            {
                foreach (ControllerVar var in this.ControllerProgramSolution.Vars)
                {
                    if (var.Address == 0 && ! (var is ControllerEmbeddedVar))
                        continue;
                    if (var is ControllerIOVar && (var.Name.Contains("AD") || var.Name.Contains("DA")))
                        // В выпадающем списке отображаются аналоговые датчики
                        displayedVars.Add(var.Name);
                    // Остальные переменные,кроме структур заносятся в список автозаполнения
                    if (!(var is ControllerStructVar))
                        autoCompleetVars.Add(var.Name);
                }
            }
            // Инициализация списков ComboBox'ов
            for (int i = 0; i < this.VarsComboBoxes.Count; i++)
            {
                this.VarsComboBoxes[i].Items.AddRange(displayedVars.ToArray());
                this.VarsComboBoxes[i].AutoCompleteCustomSource.AddRange(autoCompleetVars.ToArray());
                
            }
        }
        /// <summary>
        /// Инициализирует графики
        /// </summary>
        private void InitGraphControl()
        {
            //Настройка графика
            GraphPane pane = this.plVarsView.GraphPane;
            pane.Title.Text = "";
            pane.XAxis.Title.Text = "Время";
            pane.YAxis.Title.Text = "Значение";
            pane.XAxis.Type = AxisType.Date;
            pane.XAxis.Scale.Format = "HH:mm:ss";
            pane.XAxis.Scale.FontSpec.Angle = 90;
            pane.XAxis.Scale.Min = new XDate(DateTime.Now.AddSeconds(-10));
            pane.XAxis.Scale.Max = new XDate(DateTime.Now);

            for (int i = 0; i < this.VarsComboBoxes.Count; i++)
            {
                PointPairList points = new PointPairList();
                LineItem curve = pane.AddCurve("Нет", points, this.GraphicsColors[i], SymbolType.None);
                curve.Line.Width = 1;
                curve.Line.SmoothTension = this.Smooth;
            }
            this.plVarsView.AxisChange();
            this.plVarsView.Refresh();
        }
        /// <summary>
        /// Для заданной переменной преобразует ее значение из массива байт в целое число
        /// </summary>
        private long GetVarValue(DebuggerParameters.VarDescription var, byte[] data)
        {
            if (this.debuggerEngine.Parameters.InverseByteOrder)
                data = Utils.ReflectArray<byte>(data);
            long res = AppliedMath.BytesToLong(data);
            switch (var.Size)
            {
                case 1:
                    if (var.HasSign)
                        res = (sbyte)res;
                    else
                        res = (byte)res;
                    break;
                case 2:
                    if (var.HasSign)
                        res = (short)res;
                    else
                        res = (ushort)res;
                    break;
                case 4:
                    if (var.HasSign)
                        res = (int)res;
                    else
                        res = (uint)res;
                    break;
            }
            return res;
        }
        /// <summary>
        /// Создает маркер, идентифицирующий переменную в очереди переменной
        /// </summary>
        /// <param name="var">Переменная</param>
        /// <param name="idx">Дополнительный идентификационный параметр</param>
        private string GetVarMarker(DebuggerParameters.VarDescription var, int idx)
        {
            return "graph_" + Utils.AddChars('0', idx.ToString(), 3 - idx.ToString().Length) + "_" + var.Name;
        }
        /// <summary>
        /// Выделяет из маркера имя переменной
        /// </summary>
        private string GetVarNameFromMarker(string Marker)
        {
            return Marker.Substring(9);
        }
        /// <summary>
        /// Вызывается движко отладчика по считывани значения переменной,
        /// добаляет считанное значение на график
        /// </summary>
        /// <param name="Buffer">Значение переменной ввиде массива байт</param>
        /// <param name="Error">Флаг, показывающий, что произошлда ошибка чтения переменной</param>
        /// <param name="Sender">Идентификатор считанной переменной</param>
        private void VarReaded(object Sender, byte[] Buffer, bool Error)
        {
            if (Error || this.closed)
                return;
            string varName = this.GetVarNameFromMarker((string)Sender); // Идентификатор является строкой вида graph_<имя_переменной>
            // Получаем описание считанной переменной
            int idx = this.debuggerEngine.Parameters.ChartVars.FindIndex(
                new Predicate<DebuggerParameters.VarDescription>(x => (x != null && x.Name == varName)));
            if (idx == -1)
                // Такой переменной уже нет в списке
                return;
            DateTime t = DateTime.Now;
            if ((t.Ticks - this.timeStamps[idx]) * 1.0 / 10e6 < 0.5)
                // Последний раз переменная была добавоена на график менее 500 мс назад
                // Переменные добавляются на график не чаще чем раз в 500 мс, иначе очень высока нагрузка на процессор
                return;
            long value = this.GetVarValue(this.debuggerEngine.Parameters.ChartVars[idx], Buffer); // значение переменной
            // Если необходимо, смещаем график по оси времени
            if (this.cbXScale.Checked)
            {
                this.plVarsView.AxisChange();
                this.plVarsView.GraphPane.XAxis.Scale.Min = new XDate(DateTime.Now.AddSeconds(-_VisibleTime));
                this.plVarsView.GraphPane.XAxis.Scale.Max = new XDate(DateTime.Now);
            }
            //Отображение нового значения переменной на графике
            this.plVarsView.GraphPane.CurveList[idx].AddPoint(new XDate(DateTime.Now), value);
            this.plVarsView.Refresh();
            // Запись нового момента времени
            timeStamps[idx] = t.Ticks;
        }
        /// <summary>
        /// Инициализирует поля ComboBox'ов переменных
        /// </summary>
        private void SetRequestedVarsToComboBoxes()
        {
            for(int i = 0; i<this.VarsComboBoxes.Count; i++)
            {
                if (i >= this.debuggerEngine.Parameters.ChartVars.Count)
                    this.debuggerEngine.Parameters.ChartVars.Add(null);
                else if (this.debuggerEngine.Parameters.ChartVars[i] != null)
                {
                    DebuggerParameters.VarDescription var = this.debuggerEngine.Parameters.ChartVars[i];
                    this.VarsComboBoxes[i].Text = var.Name;
                    this.debuggerEngine.AddReadItem(var.Address, var.MemoryType, var.Size, this.GetVarMarker(var, i), 
                        null, new ProceedingCompleetedDelegate(this.VarReaded));
                    this.plVarsView.GraphPane.CurveList[i].Label.Text = var.Name;
                }
            }
        }
        /// <summary>
        /// Обновляет содержимое документа на основании нового движка и проекта
        /// </summary>
        public override void Update(ControllerProgramSolution solution, DebuggerEngine engine)
        {
            if (solution == null)
            {
                this.solutionFromEngine = null;
                this.Solution = solution;
            }
            this.debuggerEngine = engine;
            this.Solution = solution;
            this.FillVarsComboBoxes();
            this.SetRequestedVarsToComboBoxes();
        }

        private void VarComboBox_Leave(object sender, EventArgs e)
        {
            ComboBox Sender = (ComboBox)sender;
            int idx = this.VarsComboBoxes.IndexOf(Sender); // индекс переменной ComboBox'а в списке DebuggerParameters.ChartVars 
            DebuggerParameters.VarDescription requestedVarDesc = this.debuggerEngine.Parameters.ChartVars[idx]; // текущая опрашиваемая переменная
            if (requestedVarDesc == null || requestedVarDesc.Name != Sender.Text)
            {
                // Выбранныя в ComboBox переменная не совпадает с опрашиваемой по тому же индексу
                if (Sender.Text != "")
                {
                    // В ComboBox не пустая строка
                    ControllerVar var = this.ControllerProgramSolution.Vars.GetVarByName(Sender.Text);
                    if (var == null || var.Address == 0 && (!(var is ControllerEmbeddedVar) && !(false)) || (var is ControllerStructVar) || (var is ControllerUserVar)&&((ControllerUserVar)var).Array)
                    {
                        // В ComboBox указано несуществующее имя переменной
                        Sender.Text = requestedVarDesc == null ? "" : requestedVarDesc.Name;
                        this.plVarsView.GraphPane.CurveList[idx].Label.Text = requestedVarDesc == null ? "Нет" : requestedVarDesc.Name;
                        this.Refresh();
                        return;
                    }
                    if (requestedVarDesc != null)
                    {
                        // По этому идексу переменная уже опрашивается => удаляем опрашиваемую переменную из очереди отладчика
                        this.debuggerEngine.RemoveReadItem(requestedVarDesc.Address, requestedVarDesc.MemoryType,
                            this.GetVarMarker(requestedVarDesc, idx));
                    }
                    // Создаем описание новой опрашиваемой переменной
                    DebuggerParameters.VarDescription varDesc =
                        requestedVarDesc == null ? new DebuggerParameters.VarDescription() : requestedVarDesc;
                    varDesc.Address = var.Address;
                    varDesc.HasSign = var.HasSign;
                    varDesc.Size = var.Size;
                    varDesc.Name = var.Name;
                    varDesc.MemoryType = var.Memory;
                    this.debuggerEngine.Parameters.ChartVars[idx] = varDesc;
                    // Добавляем переменную в очередь отладчика
                    this.debuggerEngine.AddReadItem(varDesc.Address, varDesc.MemoryType, varDesc.Size,
                        this.GetVarMarker(varDesc, idx), null, new ProceedingCompleetedDelegate(this.VarReaded));
                    this.plVarsView.GraphPane.CurveList[idx].Label.Text = varDesc.Name;
                    this.Refresh();
                }
                else
                {
                    // В ComboBox указана пустая строка
                    if (requestedVarDesc != null)
                    {
                        // Если по текущему индексу переменная уже опрашивалась, то удаляем ее из очереди отладчика и его параметров
                        this.debuggerEngine.RemoveReadItem(requestedVarDesc.Address, requestedVarDesc.MemoryType,
                             this.GetVarMarker(requestedVarDesc, idx));
                        this.debuggerEngine.Parameters.ChartVars[idx]=null;
                        this.plVarsView.GraphPane.CurveList[idx].Label.Text = "Нет";
                    }
                    this.Refresh();
                }
            }
        }

        private void VarComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.VarComboBox_Leave(sender, null);
        }

        private void ViewGraphicsTabbedDocument_Closed(object sender, EventArgs e)
        {
            this.closed = true;
            for (int i = 0; i < this.debuggerEngine.Parameters.ChartVars.Count; i++)
            {
                // Удаляем опрашиваемые переменные из списка отладчика
                DebuggerParameters.VarDescription var = this.debuggerEngine.Parameters.ChartVars[i];
                if (var != null)
                {
                    this.debuggerEngine.RemoveReadItem(var.Address, var.MemoryType, this.GetVarMarker(var, i));
                }
            }
        }

        private void nudVisibleTime_ValueChanged(object sender, EventArgs e)
        {
            this.debuggerEngine.Parameters.ViewGraphicsDisplayInterval = this._VisibleTime = (int)this.nudVisibleTime.Value;
        }
    }
}
