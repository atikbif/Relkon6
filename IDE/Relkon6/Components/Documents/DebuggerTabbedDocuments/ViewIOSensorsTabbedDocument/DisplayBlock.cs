using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon.Solutions;
using Kontel.Relkon.Debugger;
using System.Text.RegularExpressions;
using Kontel.Relkon.Classes;
using Kontel.Relkon;

namespace Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument
{
    public partial class DisplayBlock : UserControl
    {
        private ControllerProgramSolution _solution;//Текущий проект
        private bool _isNewSolution=true;//Евляется ли новый проект автоматически сзданным
        private DebuggerEngine _engine = null; // движок отладчика
        int _pageNumber = 0;//идентификатор странички, на которой расположен компонент
        bool _isModulRemoved = false;//показывает запущенна ли процедура удаления аналогого модуля
        //Список цифровых входов
        private SortedList<int, Kontel.Relkon.Classes.ControllerVar> _inVarsDigital = new SortedList<int, Kontel.Relkon.Classes.ControllerVar>();
        //Список цифровых выходов
        private SortedList<int, Kontel.Relkon.Classes.ControllerVar> _outVarsDigital = new SortedList<int, Kontel.Relkon.Classes.ControllerVar>();
        //Список аналоговых входов
        private SortedList<int, Kontel.Relkon.Classes.ControllerVar> _inVarsAnalog = new SortedList<int, Kontel.Relkon.Classes.ControllerVar>();
        //Список аналоговых выходов
        private SortedList<int, Kontel.Relkon.Classes.ControllerVar> _outVarsAnalog = new SortedList<int, Kontel.Relkon.Classes.ControllerVar>();

        //Компоненты для аналоговых входов
        Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl[] ascInputs;
        //Компоненты для аналоговых выходов
        Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl[] ascOutputs;

        public DisplayBlock(ControllerProgramSolution solution, DebuggerEngine engine, int PageNumber)
        {
            InitializeComponent();
            _pageNumber = PageNumber;
            if (solution != null) this.digitalIO.NewComponents("Не выбрано ни одного датчика для отображения.");
                else this.digitalIO.NewComponents("Нет проекта для определения адресов датчиков!!!");
        }

        /// <summary>
        /// Загрузка новых параметров, загрузка меток из параметров отладчика
        /// </summary>
        public void Update_presentation(ControllerProgramSolution solution, DebuggerEngine engine/*,bool IsNewSolution*/)
        {
            _engine = engine;
            if (solution != null)
                _solution = solution;
            else
                _solution = Kontel.Relkon.Solutions.ControllerProgramSolution.Create(_engine.Parameters.ProcessorType);
            _isNewSolution = (solution == null);
            _engine.EngineStatusChanged += new EventHandler<DebuggerEngineStatusChangedEventArgs>(DebuggerParametersList_ChangeStatusEngine);
            CreateComponents();
            DebuggerParametersList_ChangeStatusEngine(engine, null);
        }

        /// <summary>
        /// Создание компонентов для отображения цифровых входов и выходов, блоков для чтения, полос прокрутки
        /// </summary>
        private void CreateComponents()
        {
            _inVarsDigital.Clear();
            _outVarsDigital.Clear();
            _inVarsAnalog.Clear();
            _outVarsAnalog.Clear();
            this.clb_moduls.Items.Clear();
            //Создание списка модулей
            for (int i = 0; i < _solution.Vars.IOVars.Count; i++)
            {
                if (_solution.Vars.IOVars[i].Address == 0)
                    continue;
                Match m;
                m = Regex.Match(_solution.Vars.IOVars[i].Name, "ADL(\\d+)");
                if (((m != null) && (m.Length != 0))) continue;
                m = Regex.Match(_solution.Vars.IOVars[i].Name, "ADH(\\d+)");
                if (((m != null) && (m.Length != 0))) continue;
                m = Regex.Match(_solution.Vars.IOVars[i].Name, "DAL(\\d+)");
                if (((m != null) && (m.Length != 0))) continue;
                m = Regex.Match(_solution.Vars.IOVars[i].Name, "DAH(\\d+)");
                if (((m != null) && (m.Length != 0))) continue;

                m = Regex.Match(_solution.Vars.IOVars[i].Name, "ADC(\\d+)");
                if (((m != null) && (m.Length != 0)) && !_solution.Vars.IOVars[i].ExternalModule) continue;
                m = Regex.Match(_solution.Vars.IOVars[i].Name, "DAC(\\d+)");
                if (((m != null) && (m.Length != 0)) && !_solution.Vars.IOVars[i].ExternalModule) continue;
                
                m = Regex.Match(_solution.Vars.IOVars[i].Name, "IN(\\d+)");
                if (((m != null) && (m.Length != 0)) && !_solution.Vars.IOVars[i].ExternalModule) continue;
                m = Regex.Match(_solution.Vars.IOVars[i].Name, "OUT(\\d+)");
                if (((m != null) && (m.Length != 0)) && !_solution.Vars.IOVars[i].ExternalModule) continue;
                this.clb_moduls.Items.Add(_solution.Vars.IOVars[i]);
            }
            List<ControllerIOVar> ContollerVarList = new List<ControllerIOVar>();
            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription ad in _engine.Parameters.ADCSensors)
            {
                ControllerIOVar var = new ControllerIOVar();
                var.Address = ad.Address;
                var.HasSign = ad.HasSign;
                var.Memory = ad.MemoryType;
                var.Name = ad.Name;
                var.Size = ad.Size;
                ContollerVarList.Add(var);
            }
            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription ad in _engine.Parameters.DACSensors)
            {
                ControllerIOVar var = new ControllerIOVar();
                var.Address = ad.Address;
                var.HasSign = ad.HasSign;
                var.Memory = ad.MemoryType;
                var.Name = ad.Name;
                var.Size = ad.Size;
                ContollerVarList.Add(var);
            }
            foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription ad in _engine.Parameters.DINSensors)
            {
                ControllerIOVar var = new ControllerIOVar();
                var.Address = ad.Address;
                var.HasSign = ad.HasSign;
                var.Memory = ad.MemoryType;
                var.Name = ad.Name;
                var.Size = ad.Size;
                ContollerVarList.Add(var);
            }
            foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription ad in _engine.Parameters.DOUTSensors)
            {
                ControllerIOVar var = new ControllerIOVar();
                var.Address = ad.Address;
                var.HasSign = ad.HasSign;
                var.Memory = ad.MemoryType;
                var.Name = ad.Name;
                var.Size = ad.Size;
                ContollerVarList.Add(var);
            }

            if (this._isNewSolution)
            {
                //Дополнить сиписок датчиками, которых нет в проекте, но есть в сохраненных параметрах
                foreach (ControllerIOVar ad in ContollerVarList)
                {
                    bool fined = false;
                    for (int i = 0; i < this.clb_moduls.Items.Count; i++)
                        if (((ControllerVar)(this.clb_moduls.Items[i])).Name == ad.Name)
                        {
                            fined = true;
                            break;
                        }
                    if (!fined)
                        this.clb_moduls.Items.Add(ad);
                }
            }
            else
            {
                //Удаление из параметров датчиков, которых нет в проекте
                foreach (ControllerIOVar ad in ContollerVarList)
                {
                    bool fined = false;
                    for (int i = 0; i < this.clb_moduls.Items.Count; i++)
                        if (((ControllerVar)(this.clb_moduls.Items[i])).Name == ad.Name)
                        {
                            fined = true;
                            break;
                        }
                    if (!fined)
                    {
                        if (ad.ExternalModule)
                        {
                            this._engine.Parameters.ADCSensors.Remove(this.GetAnalogVar(ad.Name, this._engine.Parameters.ADCSensors));
                            this._engine.Parameters.DACSensors.Remove(this.GetAnalogVar(ad.Name, this._engine.Parameters.DACSensors));
                            this._engine.Parameters.DINSensors.Remove(this.GetDigitalVar(ad.Name, this._engine.Parameters.DINSensors));
                            this._engine.Parameters.DOUTSensors.Remove(this.GetDigitalVar(ad.Name, this._engine.Parameters.DOUTSensors));
                        }
                    }
                }
            }
            SortList();

            if (_solution != null)
            {
                //Создание компонентов
                Kontel.Relkon.DebuggerParameters.Block BL = null;
                foreach (Kontel.Relkon.DebuggerParameters.Block bl in _engine.Parameters.ModulBlocks)
                    if (bl.Number == _pageNumber)
                    {
                        BL = bl;
                        break;
                    }
                //загрузка датчиков на опрос
                for (int j = BL.Vars.Count - 1; j >= 0; j--)
                {
                    bool finde = false;
                    for (int i = 0; i < this.clb_moduls.Items.Count; i++)
                    {
                        if (BL.Vars[j] == (((ControllerIOVar)(this.clb_moduls.Items[i])).Name))
                        {
                            this.clb_moduls.SelectedItem = this.clb_moduls.Items[i];
                            this.clb_moduls.SetItemChecked(i, true);
                            finde = true;
                            break;
                        }
                    }
                    if (!finde)
                        BL.Vars.Remove(BL.Vars[j]);
                }
                //перестроение представления датчиков
                this.ascInputs = RebuildAnalog(4, true, _inVarsAnalog, this.ascInputs, this.pInput);
                this.ascOutputs = RebuildAnalog(2, false, _outVarsAnalog, this.ascOutputs, this.pOutput);
                if (_inVarsDigital.Count + _outVarsDigital.Count <= 0)
                    this.digitalIO.NewComponents("Не выбрано ни одного датчика для отображения.");
            }
            else
            {
                this.digitalIO.NewComponents("Нет проекта для определения адресов датчиков!!!");
            }
            //Установка полжений разделителей
            if (_engine.Parameters.ModulBlocks[GetPageIndex(_pageNumber)].FirstSpliter>0)
            this.splitter1.SplitPosition = _engine.Parameters.ModulBlocks[GetPageIndex(_pageNumber)].FirstSpliter;
            if (_engine.Parameters.ModulBlocks[GetPageIndex(_pageNumber)].SecondSpliter>0)
                this.splitter2.SplitPosition = _engine.Parameters.ModulBlocks[GetPageIndex(_pageNumber)].SecondSpliter;
        }

        /// <summary>
        /// Сортировка списка
        /// </summary>
        private void SortList()
        {
            for (int i = 0; i < this.clb_moduls.Items.Count; i++)
            {
                Match m = Regex.Match(((ControllerIOVar)(this.clb_moduls.Items[i])).Name.ToString(), "([A-Z]*)(\\d+)");
                for (int j = i + 1; j < this.clb_moduls.Items.Count; j++)
                {
                    Match m1 = Regex.Match(((ControllerIOVar)(this.clb_moduls.Items[j])).Name.ToString(), "([A-Z]*)(\\d+)");
                    if ((m.Groups[1].Value.CompareTo(m1.Groups[1].Value) == 1) || (m.Groups[1].Value == m1.Groups[1].Value && int.Parse(m.Groups[2].Value) > int.Parse(m1.Groups[2].Value)))
                    {
                        ControllerIOVar cv = (ControllerIOVar)(this.clb_moduls.Items[j]);
                        this.clb_moduls.Items.RemoveAt(j);
                        this.clb_moduls.Items.Insert(i, cv);
                        m = m1;
                    }
                }
            }
        }
        /// <summary>
        /// Добавление или удалени датчика в зависимости от выбора его в списке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clb_moduls_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ControllerIOVar CurentValue = (ControllerIOVar)(((CheckedListBox)sender).SelectedItem);
            if (e.CurrentValue == e.NewValue) return;

            Match m = Regex.Match(CurentValue.Name.ToString(), "([A-Z]*)(\\d+)");
            //добавление или датчика на опрос
            if (m.Groups[1].Value == "IN" || m.Groups[1].Value == "OUT"|| !_isModulRemoved)
            {
                RemoveReadItems();
            }
            switch (m.Groups[1].Value)
            {
                case "IN":
                    if (e.NewValue == CheckState.Checked)
                    {
                        if (_inVarsDigital.ContainsKey(Convert.ToInt32(m.Groups[2].Value))) return;
                        _inVarsDigital.Add(Convert.ToInt32(m.Groups[2].Value), CurentValue);
                        Kontel.Relkon.DebuggerParameters.DigitalSensorDescription vd = GetDigitalVar(CurentValue.Name, _engine.Parameters.DINSensors);
                        if (vd == null)
                        {
                            //Добавление нового датчика цифрового входа в параметры отладчика
                            vd = new DebuggerParameters.DigitalSensorDescription();
                            vd.Name = CurentValue.Name;
                            vd.Address = CurentValue.Address;
                            vd.BitNumber = 4;
                            vd.HasSign = CurentValue.HasSign;
                            vd.MemoryType = CurentValue.Memory;
                            vd.Size = CurentValue.Size;
                            _engine.Parameters.DINSensors.Add(vd);
                            this.digitalIO.RefreshComponents(_inVarsDigital.Keys, _outVarsDigital.Keys,  SystemColors.ControlLightLight, SystemColors.Control);
                        }
                        else
                        {
                            vd.Address = CurentValue.Address;
                            vd.BitNumber = 4;
                            vd.HasSign = CurentValue.HasSign;
                            vd.MemoryType = CurentValue.Memory;
                            vd.Size = CurentValue.Size;
                            this.digitalIO.RefreshComponents(_inVarsDigital.Keys, _outVarsDigital.Keys,  SystemColors.ControlLightLight, SystemColors.Control);
                            //Закгрузка меток выбранного датчика
                            for (int j = 0; j < vd.Labels.Count; j++)
                            {
                                try { this.digitalIO.ChangeLabel(true, Convert.ToInt32(m.Groups[2].Value), vd.Labels[j].Number, vd.Labels[j].Caption); }
                                catch { vd.Labels.Remove(vd.Labels[j]); }
                            }
                        }
                    }
                    else
                    {
                        _inVarsDigital.Remove(Convert.ToInt32(m.Groups[2].Value));
                        Kontel.Relkon.DebuggerParameters.DigitalSensorDescription vd = GetDigitalVar(CurentValue.Name, _engine.Parameters.DINSensors);
                        if (_inVarsDigital.Count + _outVarsDigital.Count > 0)
                            this.digitalIO.RefreshComponents(_inVarsDigital.Keys, _outVarsDigital.Keys,  SystemColors.ControlLightLight, SystemColors.Control);
                        else this.digitalIO.NewComponents("Не выбрано ни одного датчика для отображения.");
                    }
                    break;
                case "OUT":
                    if (e.NewValue == CheckState.Checked)
                    {
                        _outVarsDigital.Add(Convert.ToInt32(m.Groups[2].Value), CurentValue);
                        Kontel.Relkon.DebuggerParameters.DigitalSensorDescription vd = GetDigitalVar(CurentValue.Name, _engine.Parameters.DOUTSensors);
                        if (vd == null)
                        {
                            //Добавление нового датчика цифрового выхода в параметры отладчика
                            vd = new DebuggerParameters.DigitalSensorDescription();
                            vd.Name = CurentValue.Name;
                            vd.Address = CurentValue.Address;
                            vd.BitNumber = 4;
                            vd.HasSign = CurentValue.HasSign;
                            vd.MemoryType = CurentValue.Memory;
                            vd.Size = CurentValue.Size;
                            _engine.Parameters.DOUTSensors.Add(vd);
                            this.digitalIO.RefreshComponents(_inVarsDigital.Keys, _outVarsDigital.Keys,  SystemColors.ControlLightLight, SystemColors.Control);
                        }
                        else
                        {
                            vd.Address = CurentValue.Address;
                            vd.BitNumber = 4;
                            vd.HasSign = CurentValue.HasSign;
                            vd.MemoryType = CurentValue.Memory;
                            vd.Size = CurentValue.Size;
                            this.digitalIO.RefreshComponents(_inVarsDigital.Keys, _outVarsDigital.Keys,  SystemColors.ControlLightLight, SystemColors.Control);
                            //Закгрузка меток выбранного датчика
                            for (int j = 0; j < vd.Labels.Count; j++)
                            {
                                try { this.digitalIO.ChangeLabel(false, Convert.ToInt32(m.Groups[2].Value), vd.Labels[j].Number, vd.Labels[j].Caption); }
                                catch { vd.Labels.Remove(vd.Labels[j]); }
                            }
                        }
                    }
                    else
                    {
                        _outVarsDigital.Remove(Convert.ToInt32(m.Groups[2].Value));
                        Kontel.Relkon.DebuggerParameters.DigitalSensorDescription vd = GetDigitalVar(CurentValue.Name, _engine.Parameters.DOUTSensors);
                        if (_inVarsDigital.Count + _outVarsDigital.Count > 0)
                            this.digitalIO.RefreshComponents(_inVarsDigital.Keys, _outVarsDigital.Keys,  SystemColors.ControlLightLight, SystemColors.Control);
                        else this.digitalIO.NewComponents("Не выбрано ни одного датчика для отображения.");
                    }
                    break;
                case "ADC":
                    if (e.NewValue == CheckState.Checked)
                    {
                        AddModule((Convert.ToInt32(m.Groups[2].Value) - 1) / 4 * 4 + 1, 4, true);
                        Kontel.Relkon.DebuggerParameters.AnalogSensorDescription vd = GetAnalogVar(CurentValue.Name, _engine.Parameters.ADCSensors);
                        if (vd == null)
                        {
                            //Добавление нового датчика аналогого входа в параметры отладчика
                            vd = new DebuggerParameters.AnalogSensorDescription();
                            vd.Name = CurentValue.Name;
                            vd.Address = CurentValue.Address;
                            vd.Caption = CurentValue.Name;
                            vd.DisplayOneByte = true;
                            vd.HasSign = CurentValue.HasSign;
                            vd.MemoryType = CurentValue.Memory;
                            vd.Size = CurentValue.Size;
                            _engine.Parameters.ADCSensors.Add(vd);
                        }
                        else
                        {
                            vd.Address = CurentValue.Address;
                            vd.DisplayOneByte = true;
                            vd.HasSign = CurentValue.HasSign;
                            vd.MemoryType = CurentValue.Memory;
                            vd.Size = CurentValue.Size;
                        }
                    }
                    else
                    {
                        RemoveModule((Convert.ToInt32(m.Groups[2].Value) - 1) / 4 * 4 + 1, 4, true);
                        Kontel.Relkon.DebuggerParameters.AnalogSensorDescription vd = GetAnalogVar(CurentValue.Name, _engine.Parameters.ADCSensors);
                    }
                    break;
                case "DAC":
                    if (e.NewValue == CheckState.Checked)
                    {
                        AddModule((Convert.ToInt32(m.Groups[2].Value) - 1) / 2 * 2 + 1, 2, false);
                        Kontel.Relkon.DebuggerParameters.AnalogSensorDescription vd = GetAnalogVar(CurentValue.Name, _engine.Parameters.DACSensors);
                        if (vd == null)
                        {
                            //Добавление нового датчика аналогого выхода в параметры отладчика
                            vd = new DebuggerParameters.AnalogSensorDescription();
                            vd.Name = CurentValue.Name;
                            vd.Address = CurentValue.Address;
                            vd.Caption = CurentValue.Name;
                            vd.DisplayOneByte = false;
                            vd.HasSign = CurentValue.HasSign;
                            vd.MemoryType = CurentValue.Memory;
                            vd.Size = CurentValue.Size;
                            _engine.Parameters.DACSensors.Add(vd);
                        }
                        else
                        {
                            vd.Address = CurentValue.Address;
                            vd.DisplayOneByte = true;
                            vd.HasSign = CurentValue.HasSign;
                            vd.MemoryType = CurentValue.Memory;
                            vd.Size = CurentValue.Size;
                        }
                    }
                    else
                    {
                        RemoveModule((Convert.ToInt32(m.Groups[2].Value) - 1) / 2 * 2 + 1, 2, false);
                        Kontel.Relkon.DebuggerParameters.AnalogSensorDescription vd = GetAnalogVar(CurentValue.Name, _engine.Parameters.DACSensors);
                    }
                    break;
            }
            //Получение текущей странички в параметрах отладчика
            foreach (Kontel.Relkon.DebuggerParameters.Block bl in _engine.Parameters.ModulBlocks)
            {
                if (bl.Number == _pageNumber)
                {
                    if (e.NewValue == CheckState.Checked)
                    {
                        if (!bl.Vars.Contains(CurentValue.Name))
                            //Добавление переменной к текущей страничке
                            bl.Vars.Add(CurentValue.Name);
                    }
                    else
                    {
                        if (bl.Vars.Contains(CurentValue.Name))
                            //Удаление переменной с текущей страничке
                            bl.Vars.Remove(CurentValue.Name);
                    }

                    break;
                }
            }
            //добавление или датчика на опрос
            if (m.Groups[1].Value == "IN" || m.Groups[1].Value == "OUT" )
            {
                AddedReadItems();
            }
        }

        /// <summary>
        /// Возварщает значение индеска вкладки в списке по ее номеру
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int GetPageIndex(int number)
        {
            foreach (Kontel.Relkon.DebuggerParameters.Block bl in _engine.Parameters.ModulBlocks)
                if (bl.Number == number) return _engine.Parameters.ModulBlocks.IndexOf(bl);
            return -1;
        }

        /// <summary>
        /// Ищет в списке дискретных переменных переменную с заданным именем
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="ListForFound">список в котором производится поиск</param>
        /// <returns></returns>
        private Kontel.Relkon.DebuggerParameters.DigitalSensorDescription GetDigitalVar(string name, List<Kontel.Relkon.DebuggerParameters.DigitalSensorDescription> ListForFound)
        {
            foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription vd in ListForFound)
                if (vd.Name == name) return vd;
            return null;
        }

        /// <summary>
        /// Ищет в списке аналоговых переменных переменную с заданным именем
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="ListForFound">список в котором производится поиск</param>
        /// <returns></returns>
        private Kontel.Relkon.DebuggerParameters.AnalogSensorDescription GetAnalogVar(string name, List<Kontel.Relkon.DebuggerParameters.AnalogSensorDescription> ListForFound)
        {
            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription vd in ListForFound)
                if (vd.Name == name) return vd;
            return null;
        }

        /// <summary>
        /// Добавление блока аналоговых входов/выходов
        /// </summary>
        /// <param name="StartNumber"></param>
        /// <param name="Size"></param>
        /// <param name="IsInput"></param>
        private void AddModule(int StartNumber, int Size, bool IsInput)
        {
            //Добавление аналогого входа
            for (int i = 0; i < this.clb_moduls.Items.Count; i++)
                if (IsInput)
                {
                    Match m;
                    m = Regex.Match(((ControllerIOVar)(this.clb_moduls.Items[i])).Name.ToString(), "ADC(\\d+)");
                    if (((m != null) && (m.Length != 0)) && ((Convert.ToInt32(m.Groups[1].Value) >= StartNumber) && (Convert.ToInt32(m.Groups[1].Value) < StartNumber + Size) && !_inVarsAnalog.ContainsKey(Convert.ToInt32(m.Groups[1].Value))))
                    {
                        _inVarsAnalog.Add(Convert.ToInt32(m.Groups[1].Value), (ControllerIOVar)(this.clb_moduls.Items[i]));
                        this.clb_moduls.SelectedItem = this.clb_moduls.Items[i];
                        this.clb_moduls.SetItemCheckState(i, CheckState.Checked);
                        if (Convert.ToInt32(m.Groups[1].Value) == StartNumber + Size - 1)
                            this.ascInputs = RebuildAnalog(Size, true, _inVarsAnalog, this.ascInputs, this.pInput);
                    }
                }
                else
                {
                    Match m;
                    m = Regex.Match(((ControllerIOVar)(this.clb_moduls.Items[i])).Name.ToString(), "DAC(\\d+)");
                    if (((m != null) && (m.Length != 0)) && ((Convert.ToInt32(m.Groups[1].Value) >= StartNumber) && (Convert.ToInt32(m.Groups[1].Value) < StartNumber + Size) && !_outVarsAnalog.ContainsKey(Convert.ToInt32(m.Groups[1].Value))))
                    {
                        _outVarsAnalog.Add(Convert.ToInt32(m.Groups[1].Value), (ControllerIOVar)(this.clb_moduls.Items[i]));
                        this.clb_moduls.SetSelected(i, true);
                        this.clb_moduls.SetItemCheckState(i, CheckState.Checked);
                        if (Convert.ToInt32(m.Groups[1].Value) == StartNumber + Size - 1)
                            this.ascOutputs = RebuildAnalog(Size, false, _outVarsAnalog, this.ascOutputs, this.pOutput);
                    }
                }
            RemoveReadItems();
            AddedReadItems();
        }

        /// <summary>
        /// Уделение блока аналоговых входов/выходов из предствления
        /// </summary>
        /// <param name="StartNumber"></param>
        /// <param name="Size"></param>
        /// <param name="IsInput"></param>
        private void RemoveModule(int StartNumber, int Size, bool IsInput)
        {
            _isModulRemoved = true;
            for (int i = 0; i < this.clb_moduls.Items.Count; i++)
                if (IsInput)
                {
                    //Удаление аналогого входа
                    Match m;
                    m = Regex.Match(((ControllerIOVar)(this.clb_moduls.Items[i])).Name.ToString(), "ADC(\\d+)");
                    if (((m != null) && (m.Length != 0)) && ((Convert.ToInt32(m.Groups[1].Value) >= StartNumber) && (Convert.ToInt32(m.Groups[1].Value) < StartNumber + Size) && _inVarsAnalog.ContainsKey(Convert.ToInt32(m.Groups[1].Value))))
                    {
                        _inVarsAnalog.Remove(Convert.ToInt32(m.Groups[1].Value));
                        this.clb_moduls.SetSelected(i, true);
                        this.clb_moduls.SetItemCheckState(i, CheckState.Unchecked);
                        if (Convert.ToInt32(m.Groups[1].Value) == StartNumber + Size - 1)
                        {
                            this.ascInputs = RebuildAnalog(Size, true, _inVarsAnalog, this.ascInputs, this.pInput);
                            AddedReadItems();
                        }
                    }
                }
                else
                {
                    //Удаление аналогого выхода
                    Match m;
                    m = Regex.Match(((ControllerIOVar)(this.clb_moduls.Items[i])).Name.ToString(), "DAC(\\d+)");
                    if (((m != null) && (m.Length != 0)) && ((Convert.ToInt32(m.Groups[1].Value) >= StartNumber) && (Convert.ToInt32(m.Groups[1].Value) < StartNumber + Size) && _outVarsAnalog.ContainsKey(Convert.ToInt32(m.Groups[1].Value))))
                    {
                        _outVarsAnalog.Remove(Convert.ToInt32(m.Groups[1].Value));
                        this.clb_moduls.SetSelected(i, true);
                        this.clb_moduls.SetItemCheckState(i, CheckState.Unchecked);
                        if (Convert.ToInt32(m.Groups[1].Value) == StartNumber + Size - 1)
                        {
                            this.ascOutputs = RebuildAnalog(Size, false, _outVarsAnalog, this.ascOutputs, this.pOutput);
                            AddedReadItems();
                        }
                    }
                }
            _isModulRemoved = false;
        }


        /// <summary>
        /// Создание компонентов аналоговых датчиков
        /// </summary>
        private AnalogSensorControl[] RebuildAnalog(int BlockSize, bool IsInput, SortedList<int, ControllerVar> WorkList, AnalogSensorControl[] ComponentArray, Panel ParentPanel)
        {
            int n = 0;
            int y = 0;
            int x = 0;
            //ParentPanel.Controls.Clear();
            AnalogSensorControl[] oldVars = ComponentArray == null ? new AnalogSensorControl[0] : new AnalogSensorControl[ComponentArray.Length];
            if (ComponentArray != null) Array.Copy(ComponentArray, oldVars, ComponentArray.Length);
            ComponentArray = new Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl[WorkList.Count];
            IList<int> m_key = WorkList.Keys;
            if (WorkList == null || WorkList.Count == 0)
                //удаление лишних датчиков
                for (int j = WorkList.Count; j < oldVars.Length; j++)
                    ParentPanel.Controls.Remove(oldVars[j]);

            for (int i = 0; i < WorkList.Count; i++)
            {
                if (i >= 0 && i < oldVars.Length && oldVars[i].SensorName == WorkList[m_key[i]].Name)
                {
                    ComponentArray[i] = oldVars[i];
                    //удаление лишних датчиков
                    if (i == WorkList.Count - 1 && oldVars.Length > WorkList.Count)
                        for (int j = WorkList.Count; j < oldVars.Length; j++)
                            ParentPanel.Controls.Remove(oldVars[j]);
                }
                else if (i - BlockSize >= 0 && i - BlockSize < oldVars.Length && (oldVars[i - BlockSize].SensorName == WorkList[m_key[i]].Name))
                {
                    ComponentArray[i] = oldVars[i - BlockSize];
                }
                else if (i + BlockSize >= 0 && i + BlockSize < oldVars.Length && (oldVars[i + BlockSize].SensorName == WorkList[m_key[i]].Name))
                {
                    //удаление i датчика
                    ParentPanel.Controls.Remove(oldVars[i]);
                    ComponentArray[i] = oldVars[i + BlockSize];
                }
                else
                {
                    ComponentArray[i] = new Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl(2);
                    ParentPanel.Controls.Add(ComponentArray[i]);
                    ComponentArray[i].BackColor = System.Drawing.SystemColors.Control;
                    ComponentArray[i].InverseByteOrder = _engine.Parameters.InverseByteOrder;
                    ComponentArray[i].SensorName = WorkList[m_key[i]].Name;
                    ComponentArray[i].SensorLabel = "";
                    ComponentArray[i].EnabledMouseClick = _engine.EngineStatus == DebuggerEngineStatus.Started;
                    if (IsInput)
                    {
                        ComponentArray[i].ValueFieldColor = Color.FromArgb(102, 254, 51);
                        foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription asd in _engine.Parameters.ADCSensors)
                            if (ComponentArray[i].SensorName == asd.Name)
                            {
                                ComponentArray[i].SensorLabel = asd.Caption;
                                break;
                            }
                    }
                    else
                    {
                        ComponentArray[i].ValueFieldColor = Color.FromArgb(255, 121, 75);
                        ComponentArray[i].ChangeSigleByte = false;
                        foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription asd in _engine.Parameters.DACSensors)
                            if (ComponentArray[i].SensorName == asd.Name)
                            {
                                ComponentArray[i].SensorLabel = asd.Caption;
                                break;
                            }
                    }
                    ComponentArray[i].ValueChanged += new System.EventHandler(this.ViewIOSensorsTabbedDocument_ValueChanged);
                    ComponentArray[i].LabelChanged += new System.EventHandler(this.ViewIOSensorsTabbedDocument_LabelChanged);
                    //ComponentArray[i].OneByteChanged += new System.EventHandler(this.ViewIOSensorsTabbedDocument_OneByteChanged);
                }
                ComponentArray[i].Location = new System.Drawing.Point(x, y);

                y = y + ComponentArray[i].Size.Height;
                n = (n + 1) % (BlockSize * 2);
                if (n == BlockSize)
                {
                    x = x + ComponentArray[i].Size.Width + 10;
                    y = y - ComponentArray[i].Size.Height * BlockSize;
                }
                if (n == 0)
                {
                    y = y + 10;
                    x = 0;
                }
            }
            return ComponentArray;
        }

        /// <summary>
        /// Изменение метки аналогого датчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewIOSensorsTabbedDocument_LabelChanged(object sender, EventArgs e)
        {
            //В параметрах отладчика ищется компонент с таким именем, если он находится,
            //то метка заменяетя иначе ничего не происходит
            Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl Sender = (Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl)sender;
            if (Sender.SensorName.Contains("ADC"))
            {
                for (int i = 0; i < _engine.Parameters.ADCSensors.Count; i++)
                    if (_engine.Parameters.ADCSensors[i].Name == Sender.SensorName)
                    {
                        _engine.Parameters.ADCSensors[i].Caption = Sender.SensorLabel;
                        return;
                    }
            }
            else
            {
                for (int i = 0; i < _engine.Parameters.DACSensors.Count; i++)
                    if (_engine.Parameters.DACSensors[i].Name == Sender.SensorName)
                    {
                        _engine.Parameters.DACSensors[i].Caption = Sender.SensorLabel;
                        return;
                    }
            }
        }

        /// <summary>
        /// Смена метки цифрового датчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void digitalIO_LabelChange(object sender, DigitalIO.LabelChangeEventArgs e)
        {
            if (e.IsInput)
            {
                for (int i = 0; i < _engine.Parameters.DINSensors.Count; i++)
                {
                    if (_engine.Parameters.DINSensors[i].Name == ("IN" + e.Key))
                    {
                        bool m_exit = false;
                        for (int j = 0; j < _engine.Parameters.DINSensors[i].Labels.Count; j++)
                            if (_engine.Parameters.DINSensors[i].Labels[j].Number == e.Index)
                            {
                                _engine.Parameters.DINSensors[i].Labels[j].Caption = e.Text; m_exit = true;
                                break;
                            }
                        if (!m_exit)
                        {
                            Kontel.Relkon.DebuggerParameters.SensorLabels m_caption = new DebuggerParameters.SensorLabels();
                            m_caption.Number = e.Index;
                            m_caption.Caption = e.Text;
                            _engine.Parameters.DINSensors[i].Labels.Add(m_caption);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _engine.Parameters.DOUTSensors.Count; i++)
                {
                    if (_engine.Parameters.DOUTSensors[i].Name == ("OUT" + e.Key))
                    {
                        bool m_exit = false;
                        for (int j = 0; j < _engine.Parameters.DOUTSensors[i].Labels.Count; j++)
                            if (_engine.Parameters.DOUTSensors[i].Labels[j].Number == e.Index)
                            {
                                _engine.Parameters.DOUTSensors[i].Labels[j].Caption = e.Text; m_exit = true;
                                break;
                            }
                        if (!m_exit)
                        {
                            Kontel.Relkon.DebuggerParameters.SensorLabels m_caption = new DebuggerParameters.SensorLabels();
                            m_caption.Number = e.Index;
                            m_caption.Caption = e.Text;
                            _engine.Parameters.DOUTSensors[i].Labels.Add(m_caption);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Изменение меток на текущей вкладке
        /// </summary>
        public void ChangeLabels()
        {
            foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription vd in _engine.Parameters.DINSensors)
            {            //Закгрузка меток дискретных входов
                Match m = Regex.Match(vd.Name.ToString(), "([A-Z]*)(\\d+)");
                for (int j = 0; j < vd.Labels.Count; j++)
                {
                    try { this.digitalIO.ChangeLabel(true, Convert.ToInt32(m.Groups[2].Value), vd.Labels[j].Number, vd.Labels[j].Caption); }
                    catch { }
                }
            }
            foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription vd in _engine.Parameters.DOUTSensors)
            {            //Закгрузка меток дискретных выходов
                Match m = Regex.Match(vd.Name.ToString(), "([A-Z]*)(\\d+)");
                for (int j = 0; j < vd.Labels.Count; j++)
                {
                    try { this.digitalIO.ChangeLabel(false, Convert.ToInt32(m.Groups[2].Value), vd.Labels[j].Number, vd.Labels[j].Caption); }
                    catch { }
                }
            }
            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription vd in _engine.Parameters.ADCSensors)
            {            //Закгрузка меток аналоговых входов
                foreach (AnalogSensorControl ac in this.ascInputs)
                    if (ac.SensorName == vd.Name)
                        try { ac.SensorLabel = vd.Caption; }
                        catch { }
            }
            foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription vd in _engine.Parameters.DACSensors)
            {            //Закгрузка меток аналоговых выходов
                foreach (AnalogSensorControl ac in this.ascOutputs)
                    if (ac.SensorName == vd.Name)
                        try { ac.SensorLabel = vd.Caption; }
                        catch { }
            }
        }

        /// <summary>
        /// Изменение положения делителя 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //if (_engine.Parameters.ModulBlocks[GetPageIndex(_pageNumber)].FirstSpliter != this.splitter1.SplitPosition)
                _engine.Parameters.ModulBlocks[GetPageIndex(_pageNumber)].FirstSpliter = this.splitter1.SplitPosition;
        }

        /// <summary>
        /// Изменение положения делителя 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //if (_engine.Parameters.ModulBlocks[GetPageIndex(_pageNumber)].SecondSpliter != this.splitter2.SplitPosition)
                _engine.Parameters.ModulBlocks[GetPageIndex(_pageNumber)].SecondSpliter = this.splitter2.SplitPosition;
        }

        #region Опрос датчиков
        /// <summary>
        /// Составление опросов датчиков
        /// </summary>
        public void AddedReadItems()
        {
            IList<int> m_key;
            //Запуск на четние цифровых датчиков
            m_key = _inVarsDigital.Keys;
            for (int i = 0; i < _inVarsDigital.Count; i++)
            {
                try { _engine.AddReadItem(_inVarsDigital[m_key[i]].Address, _inVarsDigital[m_key[i]].Memory, _inVarsDigital[m_key[i]].Size, "DIN_" + m_key[i], null, RefreshInterfaseDigital); }
                catch { }
            }
            m_key = _outVarsDigital.Keys;
            for (int i = 0; i < _outVarsDigital.Count; i++)
            {
                try { _engine.AddReadItem(_outVarsDigital[m_key[i]].Address, _outVarsDigital[m_key[i]].Memory, _outVarsDigital[m_key[i]].Size, "DOUT_" + m_key[i], null, RefreshInterfaseDigital); }
                catch { }
            }
            //Запуск на чтение аналоговых датчиков
            Kontel.Relkon.Classes.ControllerVar m_var;
            m_key = _inVarsAnalog.Keys;
            if (this.ascInputs != null)
                for (int i = 0; i < this.ascInputs.Length; i++)
                {
                    m_var = _inVarsAnalog[int.Parse(Regex.Match(this.ascInputs[i].SensorName, "[A-Z]*(\\d+)").Groups[1].Value)];
                    try { _engine.AddReadItem(_inVarsAnalog[m_key[i]].Address, _inVarsAnalog[m_key[i]].Memory, _inVarsAnalog[m_key[i]].Size, this.ascInputs[i], null, RefreshInterfaseAnalog); }
                    catch { }
                }
            m_key = _outVarsAnalog.Keys;
            if (this.ascOutputs != null)
                for (int i = 0; i < this.ascOutputs.Length; i++)
                {
                    m_var = _outVarsAnalog[int.Parse(Regex.Match(this.ascOutputs[i].SensorName, "[A-Z]*(\\d+)").Groups[1].Value)];
                    try { _engine.AddReadItem(_outVarsAnalog[m_key[i]].Address, _outVarsAnalog[m_key[i]].Memory, _outVarsAnalog[m_key[i]].Size, this.ascOutputs[i], null, RefreshInterfaseAnalog); }
                    catch { }
                }
        }

        /// <summary>
        /// Остановка опросов датчиков
        /// </summary>
        public void RemoveReadItems()
        {
            //Остановка чтения цифровых датчиков
            IList<int> m_key = _inVarsDigital.Keys;
            for (int i = 0; i < _inVarsDigital.Count; i++)
            {
                try { _engine.RemoveReadItem(_inVarsDigital[m_key[i]].Address, _inVarsDigital[m_key[i]].Memory, "DIN_" + m_key[i]); }
                catch {  }
            }
            m_key = _outVarsDigital.Keys;
            for (int i = 0; i < _outVarsDigital.Count; i++)
            {
                try { _engine.RemoveReadItem(_outVarsDigital[m_key[i]].Address, _outVarsDigital[m_key[i]].Memory, "DOUT_" + m_key[i]); }
                catch { }
            }
            //Остановка чения аналоговых датчиков
            m_key = _inVarsAnalog.Keys;
            Kontel.Relkon.Classes.ControllerVar m_var;
            if (this.ascInputs != null)
                for (int i = 0; i < this.ascInputs.Length; i++)
                {
                    try
                    {
                        m_var = _inVarsAnalog[int.Parse(Regex.Match(this.ascInputs[i].SensorName, "[A-Z]*(\\d+)").Groups[1].Value)];
                        _engine.RemoveReadItem(_inVarsAnalog[m_key[i]].Address, _inVarsAnalog[m_key[i]].Memory, this.ascInputs[i]);
                    }
                    catch { }
                }
            m_key = _outVarsAnalog.Keys;
            if (this.ascOutputs != null)
                for (int i = 0; i < this.ascOutputs.Length; i++)
                {
                    try
                    {
                        m_var = _outVarsAnalog[int.Parse(Regex.Match(this.ascOutputs[i].SensorName, "[A-Z]*(\\d+)").Groups[1].Value)];
                        _engine.RemoveReadItem(_outVarsAnalog[m_key[i]].Address, _outVarsAnalog[m_key[i]].Memory, this.ascOutputs[i]);
                    }
                    catch { }
                }
        }

        /// <summary>
        /// Обновление входов и выходов аналоговых
        /// </summary>
        /// <param name="Sender"></param>
        private void RefreshInterfaseAnalog(object Marker, byte[] Buffer, bool Error)
        {
            if (Buffer != null && !Error)
            {
                try
                {
                    Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl m_sender = (Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl)Marker;
                    if (!m_sender.Edited)
                    {
                        m_sender.InverseByteOrder = _engine.Parameters.InverseByteOrder;
                        m_sender.SetData(Buffer);
                        foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription dsd in _engine.Parameters.ADCSensors)
                            if (dsd.Name == m_sender.SensorName)
                            {
                                dsd.Value = Buffer;
                                break;
                            }
                        foreach (Kontel.Relkon.DebuggerParameters.AnalogSensorDescription dsd in _engine.Parameters.DACSensors)
                            if (dsd.Name == m_sender.SensorName)
                            {
                                dsd.Value = Buffer;
                                break;
                            }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Обновление входов и выходов цифровых
        /// </summary>
        /// <param name="Sender"></param>
        private void RefreshInterfaseDigital(object Marker, byte[] Buffer, bool Error)
        {
            if (Buffer != null)
            {
                Match m;
                m = Regex.Match((string)Marker, "DIN_(\\d+)");
                if (((m != null) && (m.Length != 0)) && (_inVarsDigital.ContainsKey(Convert.ToInt32(m.Groups[1].Value))))
                {
                    if (digitalIO.InVars[Convert.ToInt32(m.Groups[1].Value)].PrimaryValue != Buffer[0])
                    {
                        digitalIO.ChangeStatePictures(true, Convert.ToInt32(m.Groups[1].Value), Buffer[0]);
                        foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription dsd in _engine.Parameters.DINSensors)
                            if (dsd.Name == "IN" + Convert.ToInt32(m.Groups[1].Value))
                            {
                                dsd.Value = Buffer;
                                break;
                            }
                    }
                }
                m = Regex.Match((string)Marker, "DOUT_(\\d+)");
                if (((m != null) && (m.Length != 0)) && (_outVarsDigital.ContainsKey(Convert.ToInt32(m.Groups[1].Value))))
                {
                    if (digitalIO.OutVars[Convert.ToInt32(m.Groups[1].Value)].PrimaryValue != Buffer[0])
                    {
                        digitalIO.ChangeStatePictures(false, Convert.ToInt32(m.Groups[1].Value), Buffer[0]);
                        foreach (Kontel.Relkon.DebuggerParameters.DigitalSensorDescription dsd in _engine.Parameters.DOUTSensors)
                            if (dsd.Name == "OUT" + Convert.ToInt32(m.Groups[1].Value))
                            {
                                dsd.Value = Buffer;
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Изменение значения аналогого датчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewIOSensorsTabbedDocument_ValueChanged(object sender, EventArgs e)
        {
            Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl Sender = (Kontel.Relkon.Components.Documents.DebuggerTabbedDocuments.ViewIOSensorsTabbedDocument.AnalogSensorControl)sender;
            if (_engine.EngineStatus == DebuggerEngineStatus.Started)
            {
                Kontel.Relkon.Classes.ControllerVar m_var = _solution.Vars.GetVarByName(Sender.SensorName);
                byte[] bbb = Sender.GetData();
                _engine.AddWriteItem(m_var.Address, m_var.Memory, bbb/*Sender.GetData()*/, "Analog_W_" + m_var.Name, null, null);
            }

        }

        /// <summary>
        /// Запись цифровых входов/выходов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void digitalIO_StateChange(object sender, DigitalIO.StateChangeEventArgs e)
        {
            if (_engine.EngineStatus == DebuggerEngineStatus.Started)
                try
                {
                    if (!e.IsInput)
                    {
                        _engine.AddWriteItem(_outVarsDigital[e.Key].Address, _outVarsDigital[e.Key].Memory, new Byte[] { e.New_value }, "DOUT_W_" + _outVarsDigital[e.Key].Name + "_" + e.Index, null, WriteFinish);
                    }
                    else
                    {
                        _engine.AddWriteItem(_inVarsDigital[e.Key].Address, _inVarsDigital[e.Key].Memory, new Byte[] { e.New_value }, "DIN_W_" + _inVarsDigital[e.Key].Name + "_" + e.Index, null, WriteFinish);
                    }
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage("digitalIO_StateChange:" + ex.Message);
                    return;
                }
        }

        /// <summary>
        /// Удаление признака чтения дактчика
        /// </summary>
        /// <param name="Marker"></param>
        /// <param name="Buffer"></param>
        /// <param name="Error"></param>
        private void WriteFinish(object Marker, byte[] Buffer, bool Error)
        {
            if (!Error)
            {
                Match m = Regex.Match((string)Marker, "IN(\\d+)_(\\d+)");
                if (m.Success)
                {
                    this.digitalIO.InVars[int.Parse(m.Groups[1].Value)].WriteSensors.Remove(int.Parse(m.Groups[2].Value));
                    return;
                }
                m = Regex.Match((string)Marker, "OUT(\\d+)_(\\d+)");
                if (m.Success)
                {
                    this.digitalIO.OutVars[int.Parse(m.Groups[1].Value)].WriteSensors.Remove(int.Parse(m.Groups[2].Value));
                    return;
                }
            }
        }

        /// <summary>
        /// Смена режима работы отладчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DebuggerParametersList_ChangeStatusEngine(object sender, DebuggerEngineStatusChangedEventArgs e)
        {
            if (((DebuggerEngine)sender).EngineStatus == DebuggerEngineStatus.Stopped)
            {
                //Установка полей недоступными
                this.digitalIO.EnabledMouseClick = false;
                for (int i = 0; i < this.ascInputs.Length; i++)
                    this.ascInputs[i].EnabledMouseClick = false;
                for (int i = 0; i < this.ascOutputs.Length; i++)
                    this.ascOutputs[i].EnabledMouseClick = false;
            }
            else
            {
                this.digitalIO.EnabledMouseClick = true;
                for (int i = 0; i < this.ascInputs.Length; i++)
                    this.ascInputs[i].EnabledMouseClick = true;
                for (int i = 0; i < this.ascOutputs.Length; i++)
                    this.ascOutputs[i].EnabledMouseClick = true;
            }
        }

        #endregion


   }
}
