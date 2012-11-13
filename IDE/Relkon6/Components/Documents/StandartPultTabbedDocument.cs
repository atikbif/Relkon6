using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon;
using System.IO;
using Kontel.Relkon.Classes;
using Kontel.Relkon.Solutions;
using System.Drawing.Printing;
using Kontel.TabbedDocumentsForm;
using Kontel.Relkon.CodeDom;

namespace Kontel.Relkon.Components.Documents
{
    public sealed partial class StandartPultTabbedDocument : FileTabbedDocument, IEditableTabbedDocument, IPrintableTabbedDocument
    {
        /// <summary>
        /// Стек изменений
        /// </summary>
        private class UndoRedoStack
        {
            #region Actions
            public abstract class Action
            {
                private int row; // индекс активной строки
                private int view; // индекс активного вида
                private int symbol; // индекс активного символа

                /// <summary>
                /// Возвращает или устанавливает индекс текущей строки
                /// </summary>
                public int Row
                {
                    get
                    {
                        return this.row;
                    }
                    set
                    {
                        this.row = value;
                    }
                }
                /// <summary>
                /// Возвращает или устанавливает индекс текущего вида
                /// </summary>
                public int View
                {
                    get
                    {
                        return this.view;
                    }
                    set
                    {
                        this.view = value;
                    }
                }
                /// <summary>
                /// Возвращает или устанавливает индекс текущего символа
                /// </summary>
                public int Symbol
                {
                    get
                    {
                        return this.symbol;
                    }
                    set
                    {
                        this.symbol = value;
                    }
                }

                public Action(int Row, int View, int Symbol)
                {
                    this.row = Row;
                    this.view = View;
                    this.symbol = Symbol;
                }
                /// <summary>
                /// Отменяет указанное действие
                /// </summary>
                public abstract void Undo(StandartPultTabbedDocument Document);
                /// <summary>
                /// Возвращает указанное действие
                /// </summary>
                public abstract void Redo(StandartPultTabbedDocument Document);
            }
            /// <summary>
            /// Добавление вида
            /// </summary>
            public class AddingViewAction : Action
            {
                private Kontel.Relkon.Solutions.View addedView;

                public AddingViewAction(int Row, int View, int Symbol, Kontel.Relkon.Solutions.View AddedView)
                    : base(Row, View, Symbol)
                {
                    this.addedView = AddedView;
                }

                public override void Undo(StandartPultTabbedDocument Document)
                {
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                    Document.DeleteView(this.View);
                }

                public override void Redo(StandartPultTabbedDocument Document)
                {
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                    Document.InsertViewToCurrentRow(this.View, this.addedView);
                }
            }
            /// <summary>
            /// Удаление вида
            /// </summary>
            public class DeletingViewAction : Action
            {
                private Kontel.Relkon.Solutions.View deletedView;

                public DeletingViewAction(int Row, int View, int Symbol, Kontel.Relkon.Solutions.View DeletedView)
                    : base(Row, View, Symbol)
                {
                    this.deletedView = DeletedView;
                }

                public override void Undo(StandartPultTabbedDocument Document)
                {
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                    Document.InsertViewToCurrentRow(this.View, this.deletedView);
                }

                public override void Redo(StandartPultTabbedDocument Document)
                {
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                    Document.DeleteView(this.View);
                }
            }
            /// <summary>
            /// В вид вставлена строка
            /// </summary>
            public class InsertingTextAction : Action
            {
                private string text; // вставленный текст

                public InsertingTextAction(int Row, int View, int Symbol, string Text)
                    : base(Row, View, Symbol)
                {
                    this.text = Text;
                }

                public override void Undo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.text.Length);
                    Document.lcdIndicator.DeleteSelection();
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.text.Length);
                }

                public override void Redo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                    Document.lcdIndicator.InsertString(this.text);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.text.Length);
                }
            }
            /// <summary>
            /// Из текста вид удалена строка
            /// </summary>
            public class DeletingTextAction : Action
            {
                private string text; // вставленный текст

                public DeletingTextAction(int Row, int View, int Symbol, string Text)
                    : base(Row, View, Symbol)
                {
                    this.text = Text;
                }

                public override void Undo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                    Document.lcdIndicator.InsertString(this.text);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.text.Length);
                }

                public override void Redo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.text.Length);
                    Document.lcdIndicator.DeleteSelection();
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.text.Length);
                }
            }
            /// <summary>
            /// В тексте вид 1 строка заменена на другую
            /// </summary>
            public class ReplacingTextAction : Action
            {
                private string textBefore; // заменяемая строка
                private string textAfter; // строка замены
                public ReplacingTextAction(int Row, int View, int Symbol, string TextBefore, string TextAfter)
                    : base(Row, View, Symbol)
                {
                    this.textBefore = TextBefore;
                    this.textAfter = TextAfter;
                }

                public override void Undo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.textAfter.Length);
                    Document.lcdIndicator.ReplaceSelection(this.textBefore);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.textBefore.Length);
                }

                public override void Redo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.textBefore.Length);
                    Document.lcdIndicator.ReplaceSelection(this.textAfter);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), this.textAfter.Length);
                }
            }
            /// <summary>
            /// Добавление переменной
            /// </summary>
            public class AddingVarAction: Action
            {
                private PultVar var;

                public AddingVarAction(int Row, int View, int Symbol, PultVar Var)
                    : base(Row, View, Symbol)
                {
                    this.var = Var;
                }

                public override void Undo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.pult[this.Row][this.View].Vars.Remove(this.var);
                    Document.lcdIndicator[this.Row].RemoveObject(this.var);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                }

                public override void Redo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.pult[this.Row][this.View].Vars.Add(this.var);
                    Document.lcdIndicator[this.Row].AddExistingObject(this.var);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                }
            }
            /// <summary>
            /// Удаление переменной
            /// </summary>
            public class DeletingVarAction : Action
            {
                private PultVar var;

                public DeletingVarAction(int Row, int View, int Symbol, PultVar Var)
                    : base(Row, View, Symbol)
                {
                    this.var = Var;
                }

                public override void Undo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.pult[this.Row][this.View].Vars.Add(this.var);
                    Document.lcdIndicator[this.Row].AddExistingObject(this.var);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                }

                public override void Redo(StandartPultTabbedDocument Document)
                {
                    Document.SelectView(this.Row, this.View, true);
                    Document.pult[this.Row][this.View].Vars.Remove(this.var);
                    Document.lcdIndicator[this.Row].RemoveObject(this.var);
                    Document.lcdIndicator.SetSelection(new Point(this.Symbol, this.Row), 1);
                }
            }
            #endregion

            private StandartPultTabbedDocument document;
            
            private List<Action> actions = new List<Action>(); // список отменяемых действий
            private int sp = -1; // вершина стека
            private bool denyAddingAction = false; // если true, то нельзя добавлять действие в стек

            public UndoRedoStack(StandartPultTabbedDocument Document)
            {
                this.document = Document;
            }
            /// <summary>
            /// Показывает, можно ли отменить последнее действие
            /// </summary>
            public bool CanUndo
            {
                get
                {
                    return this.sp >= 0;
                }
            }
            /// <summary>
            /// Показывает, можно ли вернуть последнее действие
            /// </summary>
            public bool CanRedo
            {
                get
                {
                    return this.sp < this.actions.Count - 1;
                }
            }
            /// <summary>
            /// Добавляет указанное действие в стек
            /// </summary>
            public void AddAction(Action Action)
            {
                if (this.denyAddingAction)
                    return;
                for (int i = this.actions.Count - 1; i > sp && i > -1; i--)
                {
                    this.actions.RemoveAt(i);
                }
                this.actions.Add(Action);
                sp++;
            }
            /// <summary>
            /// Отменяет последнее изменене
            /// </summary>
            public void Undo()
            {
                if (!this.CanUndo)
                    return;
                this.denyAddingAction = true;
                this.actions[sp].Undo(this.document);
                this.denyAddingAction = false;
                this.sp--;
                if (sp > -1 && ((this.actions[sp + 1] is InsertingTextAction && this.actions[this.sp] is AddingVarAction) ||
                    (this.actions[this.sp + 1] is DeletingTextAction && this.actions[this.sp] is DeletingVarAction)))
                    this.Undo();
            }
            /// <summary>
            /// Возвращает последнее изменение
            /// </summary>
            public void Redo()
            {
                if (!this.CanRedo)
                    return;
                this.sp++;
                this.denyAddingAction = true;
                this.actions[sp].Redo(this.document);
                this.denyAddingAction = false;
                if (sp <this.actions.Count-1 && ((this.actions[sp + 1] is InsertingTextAction && this.actions[this.sp] is AddingVarAction) ||
                    (this.actions[this.sp + 1] is DeletingTextAction && this.actions[this.sp] is DeletingVarAction)))
                    this.Redo();
            }
            /// <summary>
            /// Очищает стек от всех действий
            /// </summary>
            public void Clear()
            {
                this.actions.Clear();
                this.sp = -1;
            }
        }

        private RelkonPultModel pult = null;
        //private FbdEditor fbdeditor;
        private int[] selectedViews; // индексы текущих видов для каждой из строк
        private string af = ""; // хранит автоматически выбранный формат
        private int st = -1; // начало размещения переменной при автоматическом выборе формата
        private UndoRedoStack urStack; // стек для отката сделанных ранее изменений
        private bool selectIndicator = true; // Если false, то после нажатия кнопки клавиатуры на дереве видов выделения индикатора не произойдет
        private bool shift = false; // показывает, нажата ли в данный момент клавиша shift

        private int pageNumber = 0;
        private int printingLines = 0;
        private int nCopies = 1;
        private float linesPerPage = 0;
        private Font printingFont = new Font("Microsoft Sans Serif", 14);
        private List<string> printingViews = null;

        public StandartPultTabbedDocument(ControllerProgramSolution Solution, string FileName)
            : base(Solution, FileName)
        {          
            this.Initialize();
            this.LoadFromFile(FileName);
            this.initialized = true;            
        }

        //public FbdEditor ActiveFbdEditor
        //{
        //    set { fbdeditor = value; }
        //}
        /// <summary>
        /// Возвращает проект к которому относится документ, преобразованный к проекту Relkon
        /// </summary>
        public ControllerProgramSolution ControllerProgramSolution
        {
            get
            {
                return this.Solution as ControllerProgramSolution;
            }
        }

        #region FileTabbedDocument members

        protected override Encoding FileEncoding
        {
            get
            {
                return this.pult.FileEncoding;
            }
        }

        protected override void PerformSaving(string FileName)
        {
            this.pult.Save(FileName);
        }

        public bool CanUndo
        {
            get
            {
                return this.urStack.CanUndo;
            }
        }

        public bool CanRedo
        {
            get
            {
                return this.urStack.CanRedo;
            }
        }

        public bool CanPaste
        {
            get 
            {
                return false;
            }
        }

        public void Cut()
        {
        }

        public void Copy()
        {
        }

        public void Paste()
        {
        }

        public void Undo()
        {
            this.urStack.Undo();
        }

        public void Redo()
        {
            this.urStack.Redo();
        }

        public void SelectAll()
        {
        }

        public void Delete()
        {
        }
        #endregion

        /// <summary>
        /// Возвращает выделенную переменную, или null, если
        /// ни одна переменная не выделена
        /// </summary>
        private PultVar SelectedVar
        {
            get
            {
                return ((PultVar)this.lcdIndicator.SelectedObject);
            }
        }
        /// <summary>
        /// Возвращает модель оболочк пульта
        /// </summary>
        public RelkonPultModel Pult
        {
            get
            {
                return this.pult;
            }
        }
        /// <summary>
        /// Возвращает текущую строку индикатора
        /// </summary>
        private int CurrentRow
        {
            get
            {
                return this.lcdIndicator.CurrentRow;
            }
        }

        public LCDIndicator LcdIndicator
        {
            get
            {
                return this.lcdIndicator;
            }
        }

        /// <summary>
        /// Выводит на панель состяния режим ввода символов в редактор (вставка / перезапись)
        /// </summary>
        public void SetInputModeString()
        {
            MainForm.MainFormInstance.PositionStatusLabel.Text = "Режим ввода: " + (this.lcdIndicator.InsertMode ? "Вставка" : "Перезапись");
        }
        /// <summary>
        /// Возвращает число сток и столбцов для указанного типа пульта
        /// (Width - число столбцов, Height - число строк)
        /// </summary>
        private Size GetPultTypeSize(PultType Type)
        {
            return new Size(Type.SymbolsInRow, Type.RowCount);
        }
        /// <summary>
        /// Изменяет тип индикатора
        /// </summary>
        private void ChangeIndicatorType(PultType NewPultType)
        {
            this.gbIndicator.Controls.Remove(this.lcdIndicator);
            Size size = this.GetPultTypeSize(NewPultType);
            this.lcdIndicator = new LCDIndicator(size.Height, size.Width);
            this.lcdIndicator.Location = new Point(10,10);
            this.lcdIndicator.Dock = DockStyle.Fill;
            this.gbIndicator.Controls.Add(this.lcdIndicator);
            this.ComputeDimensions();
            if (NewPultType.Equals(PultType.Pult2x16))
                this.lcdIndicator.ShowGrid = true;
            else
                this.lcdIndicator.ShowGrid = false;
            this.SetIndicatorEvents();
            this.SetInputModeString();
        }
        /// <summary>
        /// Устанавливает события пульта
        /// </summary>
        private void SetIndicatorEvents()
        {
            this.lcdIndicator.InputModeChanged += new EventHandler(lcdIndicator_InputModeChanged);
            this.lcdIndicator.RowTextChanged += new EventHandler<EventArgs<int>>(lcdIndicator_RowTextChanged);
            this.lcdIndicator.RowTextInserted += new EventHandler<LCDIndicator.RowTextIDREventArgs>(lcdIndicator_RowTextInserted);
            this.lcdIndicator.RowTextDeleted += new EventHandler<LCDIndicator.RowTextIDREventArgs>(lcdIndicator_RowTextDeleted);
            this.lcdIndicator.RowTextReplaced += new EventHandler<LCDIndicator.RowTextIDREventArgs>(lcdIndicator_RowTextReplaced);
            this.lcdIndicator.SelectionChanged += new EventHandler(lcdIndicator_SelectionChanged);
            this.lcdIndicator.KeyDown += new KeyEventHandler(lcdIndicator_KeyDown);
            this.lcdIndicator.KeyUp += new KeyEventHandler(lcdIndicator_KeyUp);
            this.lcdIndicator.LCDIndicatorRowObjectLostFocus += new EventHandler(lcdIndicator_LCDIndicatorRowObjectLostFocus);
            this.lcdIndicator.LCDIndicatorRowObjectSelected += new EventHandler<EventArgs<ILCDIndicatorRowObject>>(lcdIndicator_LCDIndicatorRowObjectSelected);
            this.lcdIndicator.LCDIndicatorRowObjectDeleted += new EventHandler<EventArgs<ILCDIndicatorRowObject>>(lcdIndicator_LCDIndicatorRowObjectDeleted);
            this.lcdIndicator.MouseWheel += new MouseEventHandler(lcdIndicator_MouseWheel);
        }
        /// <summary>
        /// Вычисляет размеры и положение некоторых элементов компонента
        /// </summary>
        private void ComputeDimensions()
        {
            this.gbIndicator.Width = this.lcdIndicator.Width + this.gbIndicator.Padding.Left * 2;
            this.gbVar.Width = this.bAddVar.Left + this.bAddVar.Width + 10;
            this.pIndicator.Height = this.lcdIndicator.Height + this.gbIndicator.Padding.Bottom * 3;
            this.gbVar.Top = this.gbIndicator.Top + this.gbIndicator.Height + 5;
            int i = Math.Max(this.gbIndicator.Width, this.gbVar.Width);
            this.gbIndicator.Width = i;
            this.gbVar.Width = i;
        }
        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        private void Initialize()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetInputModeString();
            this.ComputeDimensions();
            this.pult = RelkonPultModel.Default;
            this.selectedViews = new int[this.pult.Rows.Count];
            this.SetIndicatorEvents();
            this.urStack = new UndoRedoStack(this);
        }
        /// <summary>
        /// Проверяет, может ли файл пультов указанного типа быть загружен под текущий тип процессора
        /// </summary>
        private bool CheckForValidPultType(PultType Type)
        {
            if (this.ControllerProgramSolution == null)
                return true;
            bool res = true;            
            return res;
        }
        /// <summary>
        /// Загружает модель пульта из файла
        /// </summary>
        protected override void LoadFromFile(string FileName)
        {
            RelkonPultModel pm = null;
            try
            {
                pm = RelkonPultModel.FromFile(FileName);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("Ошибка: " + Utils.FirstLetterToLower(ex.Message));
                pm = this.pult;
            }
            if(this.Solution!=null)
                pm.ConvertViewsTextsForEmbeddedVars(this.ControllerProgramSolution.Vars.EmbeddedVars);
            this.LoadPultModel(pm);
            this.fileHash = this.GetFileHash(FileName);
        }
        /// <summary>
        /// Загружает в документ указанную модель пультов
        /// </summary>
        public void LoadPultModel(RelkonPultModel Model)
        {
            this.urStack.Clear();
            if (!this.CheckForValidPultType(Model.Type))
            {
                Utils.ErrorMessage("Тип пульта несовместим с текущим типом процессора");
                Model = this.pult;
            }
            this.pult = Model;
            this.selectedViews = new int[this.pult.Rows.Count];
            MainForm.MainFormInstance.SelectedPultType = this.pult.Type;
            this.ChangeIndicatorType(this.pult.Type);
            this.FillTreeView();
            for (int i = 0; i < this.pult.Rows.Count; i++)
            {
                this.lcdIndicator[i].Text = this.pult[i][0].Text;
            }
            for (int i = 0; i < this.pult.Rows.Count; i++)
            {
                this.selectedViews[i] = -1;
                this.SelectView(i, 0, true);
            }
            this.lcdIndicator.SetSelection(new Point(0, 0), 1);
            this.lcdIndicator.Focus();
            if (this.initialized)
                this.DocumentModified = true;
        }
        /// <summary>
        /// заполняет дерево строк и видов значениями из текущего значения пульта
        /// </summary>
        private void FillTreeView()
        {
            this.tvRows.Nodes.Clear();
            for (int i = 0; i < this.pult.Rows.Count; i++)
            {
                this.tvRows.Nodes.Add("Строка " + i);
                for (int j = 0; j < this.pult[i].Views.Count; j++)
                {
                    this.tvRows.Nodes[i].Nodes.Add(this.CreateViewTreeNode(this.pult[i][j]));
                }
            }
            this.tvRows.ExpandAll();
            this.tvRows.SelectedNode = this.tvRows.Nodes[0].Nodes[0];
        }
        /// <summary>
        /// Создает узел дерева для указанного вида
        /// </summary>
        private TreeNode CreateViewTreeNode(Kontel.Relkon.Solutions.View view)
        {
            TreeNodeEx node = new TreeNodeEx();
            node.Text = view.Text;
            node.ImageIndex = 1;
            node.SelectedImageIndex = 1;
            node.Enabled = view.Enabled;
            return node;
        }
        /// <summary>
        /// Устанавливает текст текущего вида для указанной строки
        /// </summary>
        private void SetCurrentViewText(int row, string text)
        {
            ((TreeNodeEx)this.tvRows.Nodes[row].Nodes[this.selectedViews[row]]).Text = text;
            this.pult[row][this.selectedViews[row]].Text = text;
        }
        /// <summary>
        /// Устанавливает новый активный вид для указанной строки
        /// </summary>
        /// <param name="row">Индекс строки, для которой надо установить новый активный вид</param>
        /// <param name="view">Индекс вида, который надо сделать активным</param>
        /// <param name="active">True, если вид должен быть активным видом индикатора</param>
        private void SelectView(int row, int view, bool active)
        {
            if (view >= this.pult.Rows[row].Views.Count)
                view = this.pult.Rows[row].Views.Count - 1;
            if (view < 0)
                view = 0;
            if (this.selectedViews[row] == view)
                return;
            this.selectedViews[row] = view;
            this.lcdIndicator[row].Clear();
            this.lcdIndicator[row].Text = this.pult[row][view].Text;
            foreach (PultVar var in this.pult[row][view].Vars)
            {
                this.lcdIndicator[row].AddExistingObject(var);
            }
            if (active)
            {
                if (this.tvRows.SelectedNode != this.tvRows.Nodes[row].Nodes[view])
                    this.tvRows.SelectedNode = this.tvRows.Nodes[row].Nodes[view];
                int dx = (this.lcdIndicator[row].LastNonSpaceSymbolIndex != this.lcdIndicator[0].Length - 1 && this.lcdIndicator[row].LastNonSpaceSymbolIndex != 0) ? 1 : 0;
                this.lcdIndicator.SetSelection(new Point(this.lcdIndicator[row].LastNonSpaceSymbolIndex + dx, row), 1);
            }
        }
        /// <summary>
        /// Устанавливает курсор индикатора на указанный символ указанного вида указанной строки
        /// </summary>
        public void SelectSymbol(int row, int view, int symbol)
        {
            this.SelectView(row, view, true);
            this.lcdIndicator.SetSelection(new Point(symbol, row), 1);
            this.lcdIndicator.Select();
        }
        /// <summary>
        /// Вставляет указанный вид в указанную позицию в списке видов текущей строки
        /// </summary>
        private void InsertViewToCurrentRow(int index, Kontel.Relkon.Solutions.View view)
        {
            this.InsertView(this.CurrentRow, index, view);
        }
        /// <summary>
        /// Вставляет вид в указанную позицию указанной строки
        /// </summary>
        private void InsertView(int RowIndex, int ViewIndex, Kontel.Relkon.Solutions.View view)
        {
            this.pult[RowIndex].Views.Insert(ViewIndex, view);
            TreeNode node = this.CreateViewTreeNode(view);
            this.tvRows.Nodes[RowIndex].Nodes.Insert(ViewIndex, node);
            this.tvRows.SelectedNode = node;
            this.selectedViews[RowIndex] = -1;
            this.SelectView(RowIndex, ViewIndex, true);
            this.lcdIndicator.Select();
            this.DocumentModified = true;
            this.urStack.AddAction(new UndoRedoStack.AddingViewAction(RowIndex, ViewIndex, this.lcdIndicator.CurrentSymbol, view));
        }
        /// <summary>
        /// Удаляет вид по указанному индексу в текущей строке
        /// </summary>
        private void DeleteView(int index)
        {
            this.urStack.AddAction(new UndoRedoStack.DeletingViewAction(this.CurrentRow, index, this.lcdIndicator.CurrentSymbol, this.pult.Rows[this.CurrentRow].Views[index]));
            this.pult.Rows[this.CurrentRow].Views.RemoveAt(index);
            this.tvRows.Nodes[this.CurrentRow].Nodes.RemoveAt(index);
            int idx = (index == this.tvRows.Nodes[this.CurrentRow].Nodes.Count) ? index - 1 : index;
            this.selectedViews[this.CurrentRow] = -1;
            this.SelectView(this.CurrentRow, idx, true);
            this.DocumentModified = true;
        }
        /// <summary>
        /// Копирует вид
        /// </summary>
        private void CopyView(int index)
        {
            Clipboard.SetDataObject(this.pult[this.CurrentRow][index].Copy(), true);
        }
        /// <summary>
        /// Копирует текст вида c указанным индексом
        /// </summary>
        private void CopyViewAsText(int index)
        {
            Clipboard.SetText(this.pult[this.CurrentRow][index].Text);
        }
        /// <summary>
        /// Возвращает вид, содержащийся в буфере обмена
        /// </summary>
        private Kontel.Relkon.Solutions.View GetViewFromClipboard()
        {
            return (Kontel.Relkon.Solutions.View)Clipboard.GetDataObject().GetData(typeof(Kontel.Relkon.Solutions.View));
        }
        /// <summary>
        /// Возвращает границы области, в которой будет искаться
        /// строка цифр
        /// </summary>
        /// <param name="row">Строка пульта</param>
        /// <param name="view">Выбранный вид</param>
        /// <param name="pos">Позиция, определяющая область</param>
        /// <param name="lb">Левая граница</param>
        /// <param name="rb">Правая граница</param>
        private void GetBorders(int row, int view, int pos, ref int lb, ref int rb)
        {
            lb = 0;
            rb = lcdIndicator[0].Length - 1;
            List<PultVar> vl1 = this.pult[row][view].Vars; // список переменных в виде view
            for (int i = 0; i < vl1.Count; i++)
            {
                int k = vl1[i].Position + vl1[i].Mask.Length - 1; // правая граница переменной
                if (k > lb && k <= pos)
                    lb = k + 1; // переменная лежит слева и правее границы
                if (vl1[i].Position > pos && vl1[i].Position <= rb)
                    rb = vl1[i].Position - 1; // переменная лежит справа и левее границы
            }
        }
        /// <summary>
        /// Возвращает возможную маску переменной на основании символов в текущей позиции курсора
        /// </summary>
        /// <param name="pos">Позция, в которой ищется маска</param>
        /// <param name="startPos">В этот параметер заносится стартовая позиция найденой маски</param>
        private string GetFormat(int pos, ref int startPos)
        {
            int r = this.CurrentRow;
            int c = pos;
            PultVar v = new PultVar();
            if (!this.lcdIndicator.ContainsObjectInCursorPos() && Char.IsDigit(this.lcdIndicator[r][c].Symbol)
                || this.lcdIndicator[r][c].Symbol == '.' || this.lcdIndicator[r][c].Symbol == ',' || this.lcdIndicator[r][c].Symbol == '-')
            {
                //текущий символ - цифра, точка или запятая 
                int lb = 0; //левая граница
                int rb = lcdIndicator[0].Length - 1; // правая граница
                GetBorders(r, this.selectedViews[r], c, ref lb, ref rb);
                if (lb > pos)
                {
                    startPos = -1;
                    return "";
                }
                int k = 1; //число цифр после текущей позиции
                int p = 0; // флаг точки; 0-не было точки, 1-точка среди цифр, 2-точка - последний символ
                int mc = this.Solution == null ? 4 : this.ControllerProgramSolution.PultParams.MaxVarMaskLength; // максимальное число символов для переменной
                int tt = mc; // максимально возможное число символов справа
                if (this.lcdIndicator[r][c].Symbol == '.' || this.lcdIndicator[r][c].Symbol == ',')
                {
                    if (c == lb || c == rb || !Char.IsDigit(this.lcdIndicator[r][c - 1].Symbol) || !Char.IsDigit(this.lcdIndicator[r][c + 1].Symbol))
                        return "";
                    p = 2;
                    tt = this.Solution == null ? 2 : this.ControllerProgramSolution.PultParams.MaxVarMaskDigitsCountAfterComma;
                }
                if (this.lcdIndicator[r][c].Symbol == '-')
                {
                    //tt = this.RelkonSolution.Processor.MaxVarMaskLength - 1;
                    lb = c;
                    //k++;
                }
                startPos = c;
                // считаем число от текущей позиции вправо
                for (int i = 1; c + i <= rb && i < tt; i++)
                {
                    if (Char.IsDigit(this.lcdIndicator[r][c + i].Symbol))
                    {
                        k++;
                        if (p == 2)
                            p = 1;
                        continue;
                    }
                    if (this.lcdIndicator[r][c + i].Symbol == '.' || this.lcdIndicator[r][c + i].Symbol == ',')
                        if (p == 0 && i < tt && c + i < rb) // Точки еще не было
                        {
                            p = 2;
                            k++;
                        }
                        else
                            break; // точка уже была
                    else
                        break; // цифры кончились
                }
                if (p == 2)
                {
                    k--;
                    p = 0;
                }
                // Если достигли предела по размеру переменной
                if (k == (this.Solution == null ? 4 : this.ControllerProgramSolution.PultParams.MaxVarMaskLength))
                {
                    return this.lcdIndicator[r].Text.Substring(c, this.Solution == null ? 4 : this.ControllerProgramSolution.PultParams.MaxVarMaskLength);
                }
                // считаем число от текущей позиции влево
                int kk = k; // число цифр справа
                for (int i = 1; c - i >= lb && i <= mc - kk; i++)
                {
                    if (Char.IsDigit(this.lcdIndicator[r][c - i].Symbol))
                    {
                        k++;
                        if (p == 2)
                            p = 1;
                        startPos = c - i;
                        continue;
                    }
                    if (this.lcdIndicator[r][c - i].Symbol == '.' || lcdIndicator[r][c - i].Symbol == ',')
                    {
                        if (p == 0 && i < mc - kk && c - i > lb) // Точки еще не было и символ не последний
                        {
                            p = 2;
                            startPos = c - i;
                            k++;
                        }
                        else
                            break; // точка уже была
                    }
                    else if (this.lcdIndicator[r][c - i].Symbol == '-')
                    {
                        startPos = c - i;
                        k++;
                        break;
                    }
                    else
                        break; // цифры кончились
                }
                if (p == 2)
                {
                    k--;
                    startPos++;
                }
                return this.lcdIndicator[r].Text.Substring(startPos, k);
            }
            else
                return "";
        }
        /// <summary>
        /// Заполняет список имен переменных, которые могут выводится на пульт
        /// </summary>
        public void FillVarNamesList(ControllerVarCollection Vars)
        {
            this.ddlVarNames.Items.Clear();
            this.ddlVarNames.AutoCompleteCustomSource.Clear();

            foreach (ControllerVar var in Vars.SystemVars)
            {
                this.ddlVarNames.AutoCompleteCustomSource.Add(var.Name);
            }
            foreach (ControllerVar var in Vars.IOVars)
            {
                this.ddlVarNames.AutoCompleteCustomSource.Add(var.Name);
            }
            foreach (ControllerVar var in Vars.EmbeddedVars)
            {
                this.ddlVarNames.AutoCompleteCustomSource.Add(var.Name);
            }
            for (int i = 0; i < Vars.DispatcheringVars.Count; i++)
            {
                this.ddlVarNames.AutoCompleteCustomSource.Add(Vars.DispatcheringVars[i].Name);
            }        
            
            for (int i = 0; i < Vars.UserVars.Count; i++)
            {                
                if (!(Vars.UserVars[i] is ControllerStructVar) && Vars.UserVars[i].Array != true)
                {
                    this.ddlVarNames.Items.Add(Vars.UserVars[i].Name);
                    this.ddlVarNames.AutoCompleteCustomSource.Add(Vars.UserVars[i].Name);
                }
            }

            this.ddlVarNames.Items.Add("HOUR");
            this.ddlVarNames.Items.Add("MIN");
            this.ddlVarNames.Items.Add("SEC");
            this.ddlVarNames.Items.Add("DATE");
            this.ddlVarNames.Items.Add("MONTH");
            this.ddlVarNames.Items.Add("YEAR");
        }
        /// <summary>
        /// Заполняет список имен ФБД - переменных, которые могут выводится на пульт
        /// </summary>
        //public void FillFBDVarNamesList(FbdEditor editor)
        //{
        //    fbdeditor = editor;
        //    ddlFbdVarNames.Items.Clear();
        //    for (int i = 0; i < editor.Elements.FBlocks.Count; i++)
        //    {
        //        for (int j = 0; j < editor.Elements.FBlocks[i].Pins.Count; j++)
        //            ddlFbdVarNames.Items.Add("Блок " + editor.Elements.FBlocks[i].Index + " | " + editor.Elements.FBlocks[i].Name + " | "
        //                                     + editor.Elements.FBlocks[i].Pins[j].Label);
                
        //        for (int k = 0; k < (editor.Elements.FBlocks[i]).EmbVars.Count; k++)                
        //            ddlFbdVarNames.Items.Add("Блок " + editor.Elements.FBlocks[i].Index + " | " +
        //                                     editor.Elements.FBlocks[i].Name + " | "
        //                                     + editor.Elements.FBlocks[i].EmbVars[k].NameInDisplay);                                
        //    }
            
        //}

        /// <summary>
        /// Если переменная var отображает переменную заводских установок,
        /// то ее значение становится равным маске var
        /// </summary>
        private void SetEmbeddedVarValue(int row, int view, PultVar var)
        {
            ControllerEmbeddedVar ev = this.ControllerProgramSolution.Vars.GetEmbeddedVar(var.Name);
            if (ev != null)
            {
                int i = int.Parse(var.Mask.Replace(".", "").Replace(",", ""));
                bool MustChangedMask = false;
                // Проверяем, больше ли значение маски переменной, максимально возможного значения переменной 
                // заводских установок, на которую она ссылается
                if (i > Math.Pow(2, ev.Size * 8) - 1)
                {
                    // Если больше, то значение маски берется по модулю максимально возможного значения переменной заодских установок
                    i = i % (int)Math.Pow(2, ev.Size * 8); 
                    MustChangedMask = true;
                }
                this.ControllerProgramSolution.Vars.EmbeddedVars.SetEmbeddedVarValue(ev.Name, i, this.ControllerProgramSolution.ProcessorParams.InverseByteOrder);
                if(MustChangedMask)
                    this.SetPultVarMaskFromEmbeddedVar(row, view, var, ev);
            }
        }
        /// <summary>
        /// Устанавивает маску переменной пульта на основании значения 
        /// соответствующей ей переменной заводских установок 
        /// </summary>
        private void SetPultVarMaskFromEmbeddedVar(int row, int view, PultVar pv, ControllerEmbeddedVar ev)
        {
            PultVar var = pv.Copy();
            var.Mask = "";
            string s = ev.Value.ToString();
            int rs = pv.Mask.Length - ((pv.Mask.IndexOfAny(new char[] { '.', ',' }) != -1) ? 1 : 0);
            if (rs < s.Length)
                return;
            int j = pv.Mask.Length - 1;
            for (int i = s.Length - 1; i >= 0; i--,j--)
            {
                if (pv.Mask[j] != '.' && pv.Mask[j] != ',')
                    var.Mask = s[i].ToString() + var.Mask;
                else
                    var.Mask += pv.Mask[j].ToString();
            }
            var.Mask = new string('0', pv.Mask.Length - s.Length) + var.Mask;
            this.ModifyVar(row, view, pv, var);
        }
        /// <summary>
        /// Добавляет переменную в указанный вид указанной строки
        /// </summary>
        private void AddVarToView(int row, int view, PultVar var)
        {
            if (st > -1)
            {
                var.Position = st;
                this.lcdIndicator.SetSelection(new Point(st, row), this.tbVarMask.Text.Length);
                this.lcdIndicator.DeleteSelection();
                st = -1;
            }         
            this.lcdIndicator[row].AddObject(var);
            this.pult[row][view].Vars.Add(var);            
            this.urStack.AddAction(new UndoRedoStack.AddingVarAction(this.CurrentRow, this.selectedViews[this.CurrentRow], this.lcdIndicator.CurrentSymbol, var));
            this.SetEmbeddedVarValue(row, view, var);
            this.DocumentModified = true;
        }
        /// <summary>
        /// Заменяет параметры переменной var указанного вида указанной строки
        /// параметрами переменной NewVar
        /// </summary>
        private void ModifyVar(int row, int view, PultVar var, PultVar NewVar)
        {
            if (var == null)
                return;
            this.lcdIndicator.SetSelection(new Point(var.Position, row), var.Mask.Length);
            this.lcdIndicator.DeleteSelection();
            try
            {
                this.AddVarToView(row, view, NewVar);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
                this.AddVarToView(row, view, var);
            }
        }
        /// <summary>
        /// Преобразует пульт к указанному типу
        /// </summary>
        public void ConvertPultModel(PultType type)
        {
            try
            {
                if (this.pult.Type.Equals(type))
                    return;
                if(!this.pult.IsEmpty)
                    File.Copy(this.FileName, Path.GetDirectoryName(this.FileName) + "\\" + Path.GetFileNameWithoutExtension(this.FileName) + "_" + this.pult.Type.ToString() + ".plt", true);
                this.Pult.ChangePultType(type);
                this.LoadPultModel(this.Pult);
            }
            catch (IOException ex)
            {
                Utils.ErrorMessage("Ошибка создания страховочной копии: " + ex.Message);
            }
        }
        /// <summary>
        /// Очищает документ
        /// </summary>
        public void Clear()
        {
            RelkonPultModel pm = new RelkonPultModel(this.pult.Type);
            this.LoadPultModel(pm);
        }

        #region Selection events

        private void lcdIndicator_InputModeChanged(object sender, EventArgs e)
        {
            this.SetInputModeString();
        }

        private void lcdIndicator_RowTextChanged(object sender, EventArgs<int> e)
        {
            this.SetCurrentViewText(e.Value, this.lcdIndicator[e.Value].Text);
            // Автоматическое получение формата пременной
            int pos = this.lcdIndicator.CurrentSymbol;
            if (this.lcdIndicator[this.CurrentRow][pos].Symbol == ' ' && pos>0)
                pos--;
            this.tbVarMask.Text = this.GetFormat(pos, ref st);
            af = this.tbVarMask.Text;
            if (this.tbVarMask.Text == "")
                st = -1;
            this.DocumentModified = true;
        }

        private void lcdIndicator_RowTextReplaced(object sender, LCDIndicator.RowTextIDREventArgs e)
        {
            this.urStack.AddAction(new UndoRedoStack.ReplacingTextAction(e.RowIndex, this.selectedViews[e.RowIndex], e.Position, e.Text1, e.Text2));
        }

        private void lcdIndicator_RowTextDeleted(object sender, LCDIndicator.RowTextIDREventArgs e)
        {
            this.urStack.AddAction(new UndoRedoStack.DeletingTextAction(e.RowIndex, this.selectedViews[e.RowIndex], e.Position, e.Text1));
        }

        private void lcdIndicator_RowTextInserted(object sender, LCDIndicator.RowTextIDREventArgs e)
        {
            this.urStack.AddAction(new UndoRedoStack.InsertingTextAction(e.RowIndex, this.selectedViews[e.RowIndex], e.Position, e.Text1));
        }

        private void lcdIndicator_MouseWheel(object sender, MouseEventArgs e)
        {
            int dx = e.Delta > 0 ? -1 : 1;
            for(int i = 0; i<this.pult.Rows.Count; i++)
            {
                if (i == this.CurrentRow)
                    this.SelectView(i, this.selectedViews[i] + dx, true);
                else if (this.shift)
                    this.SelectView(i, this.selectedViews[i] + dx, false);
            }
        }

        private void tvRows_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
                return;
            this.SelectView(e.Node.Parent.Index, e.Node.Index, true);
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                int dx = (this.lcdIndicator[e.Node.Parent.Index].LastNonSpaceSymbolIndex != this.lcdIndicator[0].Length - 1 && this.lcdIndicator[e.Node.Parent.Index].LastNonSpaceSymbolIndex != 0) ? 1 : 0;
                this.lcdIndicator.SetSelection(new Point(this.lcdIndicator[e.Node.Parent.Index].LastNonSpaceSymbolIndex + dx, e.Node.Parent.Index), 1);
            }
        }

        private void lcdIndicator_SelectionChanged(object sender, EventArgs e)
        {
            if (this.tvRows.SelectedNode.Parent == null || this.lcdIndicator.CurrentRow != this.tvRows.SelectedNode.Parent.Index)
                this.tvRows.SelectedNode = this.tvRows.Nodes[this.lcdIndicator.CurrentRow].Nodes[this.selectedViews[this.lcdIndicator.CurrentRow]];
            if (!this.lcdIndicator.ContainsObjectInCursorPos())
            {
                st = this.lcdIndicator.CurrentSymbol;
                this.tbVarMask.Text = GetFormat(lcdIndicator.CurrentSymbol, ref st);
                af = this.tbVarMask.Text;
                if (this.tbVarMask.Text == "")
                    st = -1;
            }
        }

        private void tvRows_KeyDown(object sender, KeyEventArgs e)
        {
            this.selectIndicator = true;
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (this.tvRows.SelectedNode.Parent != null)
                        this.lcdIndicator.Select();
                    break;
            }
        }

        private void tvRows_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.selectIndicator)
            {
                this.lcdIndicator.Select();
                SendKeys.Send(e.KeyChar.ToString());
            }
        }

        private void lcdIndicator_KeyDown(object sender, KeyEventArgs e)
        {
            this.shift = e.Shift;
            switch (e.KeyCode)
            {
                case Keys.PageDown:
                    this.SelectView(this.CurrentRow, this.selectedViews[this.CurrentRow] + 1, true);
                    this.lcdIndicator.Select();
                    break;
                case Keys.PageUp:
                    this.SelectView(this.CurrentRow, this.selectedViews[this.CurrentRow] - 1, true);
                    this.lcdIndicator.Select();
                    break;
                case Keys.Enter:
                    int idx = this.selectedViews[this.CurrentRow];
                    if (idx == this.pult[this.CurrentRow].Views.Count - 1)
                    {
                        this.InsertViewToCurrentRow(idx + 1, new Kontel.Relkon.Solutions.View());
                        this.tvRows.SelectedNode = this.tvRows.Nodes[this.CurrentRow].Nodes[this.selectedViews[this.CurrentRow]];
                        this.lcdIndicator.SetSelection(new Point(0, this.CurrentRow), 1);
                        this.lcdIndicator.Select();
                    }
                    break;
            }
        }

        private void lcdIndicator_KeyUp(object sender, KeyEventArgs e)
        {
            this.shift = false;
        }

        private void lcdIndicator_LCDIndicatorRowObjectSelected(object sender, EventArgs<ILCDIndicatorRowObject> e)
        {           
            PultVar var = (PultVar)e.Value;
            this.ddlVarNames.Text = var.Name;
            this.tbVarMask.Text = var.Mask;
            //if (fbdeditor != null)
            //    this.ddlFbdVarNames.Text = fbdeditor.GetFullVarNameByVar(var.Name);                      
            this.cbHasSign.Checked = var.HasSign;
            this.cbReadOnly.Checked = var.ReadOnly;
            this.bAddVar.Text = "Изменить";
            //this.bFbdAddVar.Text = "Изменить";          

        }

        private void lcdIndicator_LCDIndicatorRowObjectLostFocus(object sender, EventArgs e)
        {
            this.ddlVarNames.Text = "";
            //this.ddlFbdVarNames.Text = "";
            this.tbVarMask.Text = "";
            this.bAddVar.Text = "Добавить";
            //this.bFbdAddVar.Text = "Добавить";
        }

        private void lcdIndicator_LCDIndicatorRowObjectDeleted(object sender, EventArgs<ILCDIndicatorRowObject> e)
        {
            this.pult[this.CurrentRow][this.selectedViews[this.CurrentRow]].Vars.Remove((PultVar)e.Value);
            this.urStack.AddAction(new UndoRedoStack.DeletingVarAction(this.CurrentRow, this.selectedViews[this.CurrentRow], this.lcdIndicator.CurrentSymbol, (PultVar)e.Value));
            this.DocumentModified = true;
        }

        #endregion

        private void ViewContextMenu_Opening(object sender, CancelEventArgs e)
        {
            this.ViewContextMenu.Items.Clear();
            if (this.tvRows.SelectedNode.Parent == null)
            {
                this.ViewContextMenu.Items.Add(this.miAddView);
            }
            else
            {
                this.ViewContextMenu.Items.Add(this.miAddViewFromTop);
                this.ViewContextMenu.Items.Add(this.miAddViewFromBottom);
                this.ViewContextMenu.Items.Add(this.miDeleteView);
                this.ViewContextMenu.Items.Add(new ToolStripSeparator());
                this.ViewContextMenu.Items.Add(this.pult[this.CurrentRow][this.selectedViews[this.CurrentRow]].Enabled ? this.miDisableView : this.miEnableView);
                if (this.pult[this.CurrentRow][this.selectedViews[this.CurrentRow]].Enabled)
                {
                    this.ViewContextMenu.Items.Add(this.miDisableView);
                    this.miDisableView.Enabled = (this.pult.Rows[this.CurrentRow].EnabledViews.Count > 1);
                }
                else
                    this.ViewContextMenu.Items.Add(this.miEnableView);
                this.ViewContextMenu.Items.Add(new ToolStripSeparator());
                this.ViewContextMenu.Items.Add(this.miCutView);
                this.miDeleteView.Enabled = (this.miCutView.Enabled = this.pult[this.CurrentRow].Views.Count > 1);
                this.ViewContextMenu.Items.Add(this.miCopyView);
                this.ViewContextMenu.Items.Add(this.miCopyAsText);
                this.ViewContextMenu.Items.Add(this.miPasteView);
                this.ViewContextMenu.Items.Add(this.miReplaceView);
                if (Clipboard.GetDataObject().GetDataPresent(typeof(Kontel.Relkon.Solutions.View)))
                {
                    Kontel.Relkon.Solutions.View view = (Kontel.Relkon.Solutions.View)Clipboard.GetDataObject().GetData(typeof(Kontel.Relkon.Solutions.View));
                    this.miPasteView.Enabled = this.miReplaceView.Enabled = view.Text.Length <= this.pult.Type.SymbolsInRow;
                }
            }
        }

        private void miAddView_Click(object sender, EventArgs e)
        {
            int row = this.tvRows.SelectedNode.Parent == null ? this.tvRows.SelectedNode.Index : this.lcdIndicator.CurrentRow;
            this.InsertView(row, this.pult[row].Views.Count, new Kontel.Relkon.Solutions.View());
        }

        private void miAddViewFromTop_Click(object sender, EventArgs e)
        {
            this.InsertViewToCurrentRow(this.selectedViews[this.lcdIndicator.CurrentRow], new Kontel.Relkon.Solutions.View());
        }

        private void miAddViewFromBottom_Click(object sender, EventArgs e)
        {
            this.InsertViewToCurrentRow(this.selectedViews[this.CurrentRow] + 1, new Kontel.Relkon.Solutions.View());
        }

        private void miDeleteView_Click(object sender, EventArgs e)
        {
            if (this.pult.Rows[this.CurrentRow].Views.Count == 1)
                return;
            this.DeleteView(this.selectedViews[this.CurrentRow]);
        }

        private void miDisableView_Click(object sender, EventArgs e)
        {
            if (this.pult.Rows[this.CurrentRow].EnabledViews.Count > 1)
            {
                this.pult.Rows[this.CurrentRow][this.selectedViews[this.CurrentRow]].Enabled = false;
                ((TreeNodeEx)this.tvRows.Nodes[this.CurrentRow].Nodes[this.selectedViews[this.CurrentRow]]).Enabled = false;
                this.tvRows.Invalidate();
            }
            if (this.selectedViews[this.CurrentRow] < this.pult.Rows[this.CurrentRow].Views.Count - 1)
            {
                this.SelectView(this.CurrentRow, this.selectedViews[this.CurrentRow] + 1, true);
            }
            this.DocumentModified = true;
        }

        private void miEnableView_Click(object sender, EventArgs e)
        {
            this.pult.Rows[this.CurrentRow][this.selectedViews[this.CurrentRow]].Enabled = true;
            ((TreeNodeEx)this.tvRows.Nodes[this.CurrentRow].Nodes[this.selectedViews[this.CurrentRow]]).Enabled = true;
            this.tvRows.Invalidate();
            if (this.selectedViews[this.CurrentRow] < this.pult.Rows[this.CurrentRow].Views.Count - 1)
            {
                this.SelectView(this.CurrentRow, this.selectedViews[this.CurrentRow] + 1, true);
            }
            this.DocumentModified = true;
        }

        private void miCutView_Click(object sender, EventArgs e)
        {
            this.CopyView(this.selectedViews[this.CurrentRow]);
            this.DeleteView(this.selectedViews[this.CurrentRow]);
        }

        private void miCopyView_Click(object sender, EventArgs e)
        {
            this.CopyView(this.selectedViews[this.CurrentRow]);
        }

        private void miPasteView_Click(object sender, EventArgs e)
        {
            if (!Clipboard.GetDataObject().GetDataPresent(typeof(Kontel.Relkon.Solutions.View)))
                return;
            Kontel.Relkon.Solutions.View view = this.GetViewFromClipboard();
            this.InsertViewToCurrentRow(this.selectedViews[this.CurrentRow], view);
            if (this.Solution != null)
            {
                foreach (PultVar v in view.Vars)
                {
                    ControllerEmbeddedVar var = this.ControllerProgramSolution.Vars.GetEmbeddedVar(v.Name);
                    if (var != null)
                    {
                        var.Value = int.Parse(v.Mask.Replace(".", "").Replace(",", ""));
                    }
                }
            }
        }

        private void miReplaceView_Click(object sender, EventArgs e)
        {
            if (!Clipboard.GetDataObject().GetDataPresent(typeof(Kontel.Relkon.Solutions.View)))
                return;
            Kontel.Relkon.Solutions.View view = this.GetViewFromClipboard();
            int idx = this.selectedViews[this.CurrentRow];
            this.DeleteView(idx);
            if (this.pult[this.CurrentRow].EnabledViews.Count == 0)
                view.Enabled = true;
            this.InsertViewToCurrentRow(idx, view);
            if (this.Solution != null)
            {
                foreach (PultVar v in view.Vars)
                {
                    ControllerEmbeddedVar var = this.ControllerProgramSolution.Vars.GetEmbeddedVar(v.Name);
                    if (var != null)
                        var.Value = int.Parse(v.Mask);
                }
            }
        }

        private void ddlVarNames_TextChanged(object sender, EventArgs e)
        {
            this.bAddVar.Enabled = (this.Solution != null && this.ControllerProgramSolution.IsValidPultVarMask(this.tbVarMask.Text) && this.ControllerProgramSolution.Vars.GetVarByName(this.ddlVarNames.Text) != null);
            //this.st = -1;
        }

        private void bAddVar_Click(object sender, EventArgs e)
        {
            PultVar var = new PultVar(this.ddlVarNames.Text, this.tbVarMask.Text, this.lcdIndicator.CurrentSymbol, this.cbHasSign.Checked, this.cbReadOnly.Checked);
            if (this.bAddVar.Text == "Добавить")
            {
                try
                {
                    RelkonCodeVarDefenition varDev = (Solution as ControllerProgramSolution).GetRelkonCodeVarDefenitionByName(this.ddlVarNames.Text);
                    if (varDev != null && varDev.Type == "float")
                    {
                        var.ReadOnly = true;
                        this.cbReadOnly.Checked = true;
                    }
                    
                    this.AddVarToView(this.CurrentRow, this.selectedViews[this.CurrentRow], var);
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                }
            }
            else if (this.bAddVar.Text == "Изменить")
                this.ModifyVar(this.CurrentRow, this.selectedViews[this.CurrentRow], this.SelectedVar, var);
        }

        private void tbVarMask_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.st = -1;
        }

        private void ddlVarNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.bAddVar.PerformClick();
        }

        private void cbReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (this.SelectedVar != null)
            {
                this.SelectedVar.ReadOnly = this.cbReadOnly.Checked;
            }
        }

        private void cbHasSign_CheckedChanged(object sender, EventArgs e)
        {
            if (this.SelectedVar != null)
            {
                this.SelectedVar.HasSign = this.cbHasSign.Checked;
            }
        }
        /// <summary>
        /// Подготавливает документ к печати
        /// </summary>
        private void PreparingToPrint(PrintDocument Document)
        {
            pageNumber = 0;
            this.printingLines = 0;
            nCopies = Document.PrinterSettings.Copies;
            Document.PrintPage += new PrintPageEventHandler(Document_PrintPage);
            Document.EndPrint += new PrintEventHandler(Document_EndPrint);
            this.printingViews = this.CreatePrintingViewsList();
        }
        /// <summary>
        /// Завершает печать
        /// </summary>
        private void EndPrintig(PrintDocument doc)
        {
            this.printingViews = null;
            doc.PrintPage -= new PrintPageEventHandler(Document_PrintPage);
            doc.EndPrint -= new PrintEventHandler(Document_EndPrint);
        }

        public void PrintPreview(PrintPreviewDialog dlg)
        {
            this.PreparingToPrint(dlg.Document);
            try
            {
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
                this.EndPrintig(dlg.Document);
            }
        }

        public void Print(PrintDialog dlg)
        {
            this.PreparingToPrint(dlg.Document);
            dlg.AllowSelection = false;
            dlg.PrinterSettings.PrintRange = PrintRange.AllPages;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dlg.Document.Print();
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                    this.EndPrintig(dlg.Document);
                }
            }
        }

        private int GetNodeIndexWODisabledNodes(int RowIndex, Kontel.Relkon.Solutions.View View)
        {
            int i = 0;
            foreach (Kontel.Relkon.Solutions.View n in this.pult.Rows[RowIndex].Views)
            {
                if (n == View)
                    break;
                if (n.Enabled)
                    i++;
            }
            return i;
        }
        /// <summary>
        /// Создает список видов для печати
        /// </summary>
        private List<string> CreatePrintingViewsList()
        {
            List<string> res = new List<string>();
            for (int i = 0; i < this.pult.Rows.Count; i++)
            {
                res.Add("Строка " + i);
                foreach (Kontel.Relkon.Solutions.View v in this.pult.Rows[i].Views)
                {
                    if (v.Enabled)
                    {
                        int idx = this.GetNodeIndexWODisabledNodes(i, v);
                        res.Add("        " + (v.Enabled ? idx.ToString() + "." : "    ") + v.Text);
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Печатает виды в файл, позволяет выбрать имя файла
        /// </summary>
        public void PrintViewsToFile()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (this.Solution != null)
            {
                dlg.InitialDirectory = this.Solution.DirectoryName;
                dlg.FileName = Path.GetFileName(this.FileName) + "_виды.txt";
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(dlg.FileName, false, Encoding.Default))
                    {
                        List<string> views = this.CreatePrintingViewsList();
                        for (int i = 0; i < views.Count; i++)
                        {
                            writer.WriteLine(views[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                }
            }
        }

        private void Document_PrintPage(object sender, PrintPageEventArgs e)
        {
            float yPos = 0;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            int count = 0;
            string line = "";
            pageNumber++;
            if (printingLines == 0)
            {
                linesPerPage = e.MarginBounds.Height / printingFont.GetHeight(e.Graphics) - 1;
            }
            string ss = DateTime.Now.ToString("dd.MM.yy");
            Font f = new Font(printingFont.Name, 10);
            e.Graphics.DrawString(this.FileName, f, Brushes.Black, e.MarginBounds.X, e.MarginBounds.Y - 10 - f.Height);
            e.Graphics.DrawString(ss, f, Brushes.Black, e.MarginBounds.X
                + e.MarginBounds.Width - e.Graphics.MeasureString(ss, f).Width, e.MarginBounds.Y - 10 - f.Height);
            e.Graphics.DrawLine(new Pen(Brushes.Black, 1), e.MarginBounds.X, e.MarginBounds.Y - 8, e.MarginBounds.X + e.MarginBounds.Width, e.MarginBounds.Y - 8);
            string s = pageNumber.ToString();
            e.Graphics.DrawString(s, f, Brushes.Black, e.MarginBounds.X + e.MarginBounds.Width / 2, e.MarginBounds.Y + 12 + e.MarginBounds.Height);
            e.Graphics.DrawLine(new Pen(Brushes.Black, 1), e.MarginBounds.X, e.MarginBounds.Y + e.MarginBounds.Height + 10, e.MarginBounds.X + e.MarginBounds.Width, e.MarginBounds.Y + e.MarginBounds.Height + 10);
            while (count < linesPerPage && printingLines < this.printingViews.Count)
            {
                line = this.printingViews[printingLines++];
                yPos = topMargin + (count * printingFont.GetHeight(e.Graphics));
                StringFormat sf1 = new StringFormat();
                e.Graphics.DrawString(line, printingFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }
            if (printingLines < this.printingViews.Count)
                e.HasMorePages = true;
            else
            {
                if ((--nCopies) == 0)
                    e.HasMorePages = false;
                else
                {
                    printingLines = 0;
                    e.HasMorePages = true;
                }
            }
        }

        private void Document_EndPrint(object sender, PrintEventArgs e)
        {
            this.EndPrintig((PrintDocument)sender);
        }

        private void miCopyAsText_Click(object sender, EventArgs e)
        {
            this.CopyViewAsText(this.selectedViews[this.CurrentRow]);
        }

        private void ViewContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (!this.ViewContextMenu.Items.Contains(this.miDisableView))
                this.ViewContextMenu.Items.Add(this.miDisableView);
            if (!this.ViewContextMenu.Items.Contains(this.miEnableView))
                this.ViewContextMenu.Items.Add(this.miEnableView);
            this.miEnableView.Enabled = true;
        }

        //private void ddlFbdVarNames_TextChanged(object sender, EventArgs e)
        //{
        //    string var = fbdeditor.GetVarByFullVarName(this.ddlFbdVarNames.Text);
        //    ControllerVar cVar = this.ControllerProgramSolution.Vars.GetVarByName(var);
        //    bool b = cVar != null;
        //    bool k = this.Solution != null;
        //    bool v = this.ControllerProgramSolution.IsValidPultVarMask(this.tbVarMask.Text);
        //    this.bFbdAddVar.Enabled = (k && v && b);
        //}

        private void ddlFbdVarNames_KeyDown(object sender, KeyEventArgs e)
        {

        }

        //private void bAddFbdVar_Click(object sender, EventArgs e)
        //{
        //    PultVar var = new PultVar(fbdeditor.GetVarByFullVarName(this.ddlFbdVarNames.Text), this.tbVarMask.Text, this.lcdIndicator.CurrentSymbol, this.cbHasSign.Checked, this.cbReadOnly.Checked);
        //    if (this.bFbdAddVar.Text == "Добавить")
        //    {
        //        try
        //        {
        //            this.AddVarToView(this.CurrentRow, this.selectedViews[this.CurrentRow], var);
        //        }
        //        catch (Exception ex)
        //        {
        //            Utils.ErrorMessage(ex.Message);
        //        }
        //    }
        //    else if (this.bFbdAddVar.Text == "Изменить")
        //        this.ModifyVar(this.CurrentRow, this.selectedViews[this.CurrentRow], this.SelectedVar, var);
        //}

       
    }
}
