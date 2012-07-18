using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Kontel.Relkon;

namespace Kontel.Relkon
{
    /// <summary>
    /// ������� ����������, ������������ ������ LCD-���������� �������
    /// ��������� � ����������
    /// </summary>
    public sealed partial class LCDIndicator : ContainerControl
    {
        #region Selection defenition
        private class Selection
        {
            /// <summary>
            /// ��������� ������� ��������� (������� �������)
            /// </summary>
            public Point Start = new Point(0, 0);
            /// <summary>
            /// ����� ���������� ��������; ���� ������ 0, �� ���������� ������� ���� �����
            /// �� ��������� �������
            /// </summary>
            public int Length = 1;
            /// <summary>
            /// ����������� ���������: 1 - ������,
            /// -1 - �����
            /// </summary>
            public int Direction
            {
                get
                {
                    return Math.Sign(this.Length);
                }
            }
            /// <summary>
            /// ���������� ������ �����
            /// </summary>
            public int AbsLength
            {
                get
                {
                    return Math.Abs(this.Length);
                }
            }
            /// <summary>
            /// ���������� ������ ����� ���������
            /// </summary>
            public Point End
            {
                get
                {
                    return new Point(this.Start.X + this.Length - this.Direction, this.Start.Y);
                }
            }
        }
        #endregion

        #region Indicator row defenition 

        public class LCDIndicatorRow
        {
            private List<ILCDIndicatorRowObject> objects = null; // ������ ��������, ������� ���������� � ������
            private LCDIndicator parent; // ���������, � ������� ��������� ������
            private int index; // ������ ������ � ����������

            public LCDIndicatorRow(int Index, LCDIndicator Parent)
            {
                this.parent = Parent;
                this.index = Index;
                this.objects = new List<ILCDIndicatorRowObject>();
            }
            /// <summary>
            /// ���������� ����� ������� � ������
            /// </summary>
            public int Length
            {
                get
                {
                    return this.parent.symbolsInRow;
                }
            }
            /// <summary>
            /// ���������� ������ ������ �� ��������� �������
            /// </summary>
            public LCDIndicatorSymbol this[int index]
            {
                get
                {
                    return this.parent.symbols[this.index, index];
                }
            }
            /// <summary>
            /// ���������� ������ ���������� �������, ������� �� ����� �������
            /// </summary>
            public int LastNonSpaceSymbolIndex
            {
                get
                {
                    int res = 0;
                    for (int i = this.Length - 1; i >= 0; i--)
                    {
                        res = i;
                        if (this[i].Symbol != ' ')
                            break;
                    }
                    return res;
                }
            }
            /// <summary>
            /// ���������� ������ ������ � ������ ����� ����������
            /// </summary>
            public int Index
            {
                get
                {
                    return this.index;
                }
            }
            /// <summary>
            /// ���������� ��������� ������������� ������ ����������
            /// </summary>
            public string Text
            {
                get
                {
                    string res = "";
                    for (int i = 0; i < parent.symbolsInRow; i++)
                        res += parent.symbols[this.index, i].Symbol.ToString();
                    return res;
                }
                set
                {
                    for (int i = 0; i < value.Length; i++)
                        this[i].Symbol = value[i];
                    for (int i = value.Length; i < this.Length; i++)
                        this[i].Symbol = ' ';
                }
            }
            /// <summary>
            /// ��������� ��������� ������
            /// </summary>
            public void AddObject(ILCDIndicatorRowObject obj)
            {
                int FreeSpace = this.Length - this.LastNonSpaceSymbolIndex - 1;
                if (obj.Position < this.LastNonSpaceSymbolIndex + 1)
                {
                    for (int i = obj.Position; i < this.Length && this[i].Symbol == ' '; i++)
                        FreeSpace++;
                }
                else
                    FreeSpace = this.Length - obj.Position;
                if (FreeSpace < obj.Mask.Length)
                    throw new Exception("������������ ����� �� �������");
                this.parent.SetSelection(new Point(obj.Position, this.Index), 1);
                this.parent.InsertString(obj.Mask);
                this.AddExistingObject(obj);
                this.parent.selectedObject = null;
                int position = obj.Mask.Length + obj.Position < this.Length ? obj.Mask.Length + obj.Position : obj.Mask.Length + obj.Position - 1;
                this.parent.SetSelection(new Point(position, this.index), 1);
                this.parent.RaiseSelectionChangedEvent();
            }
            /// <summary>
            /// ���������� ��� ������� �� �� ������� � ������
            /// </summary>
            private int CompareObjects(ILCDIndicatorRowObject o1, ILCDIndicatorRowObject o2)
            {
                return (o1.Position - o2.Position);
            }
            /// <summary>
            /// ��������� ������ � ������ ��� �������������� ������
            /// (����� ������� ��� ���������� � ������� ������ �������)
            /// </summary>
            public void AddExistingObject(ILCDIndicatorRowObject obj)
            {
                this.objects.Add(obj);
                System.Comparison<ILCDIndicatorRowObject> comparer = new Comparison<ILCDIndicatorRowObject>(this.CompareObjects);
                this.objects.Sort(comparer);
                this.SetObjectSymbolsColor(obj);
            }
            /// <summary>
            /// ������������� ���� ��������, � ������� ��������� ��������� ������
            /// </summary>
            private void SetObjectSymbolsColor(ILCDIndicatorRowObject obj)
            {
                for (int i = obj.Position; i < obj.Position + obj.Mask.Length; i++)
                {
                    this[i].ActivePixelColor = this.parent.objectSymbolsColor;
                }
            }
            /// <summary>
            /// �������� ���������, ������������ � ��������� ������� �� ��������� ����� �������� ������
            /// </summary>
            public void MoveSubstringRight(int position, int count)
            {
                for (int i = this.LastNonSpaceSymbolIndex; i >= position; i--)
                    this[i + count].Symbol = this[i].Symbol;
                for (int j = this.objects.Count - 1; j >= 0; j--)
                {
                    ILCDIndicatorRowObject obj = this.objects[j];
                    if (obj.Position >= position)
                    {
                        for (int i = obj.Position; i < obj.Position + Math.Min(obj.Mask.Length, count); i++)
                            this[i].ActivePixelColor = Color.Black;
                        obj.Position += count;
                        this.SetObjectSymbolsColor(obj);
                    }
                }
            }
            /// <summary>
            /// �������� ���������, ������������ � ��������� ������� �� ��������� ����� �������� �����
            /// </summary>
            public void MoveSubstringLeft(int position, int count)
            {
                for (int i = position - count; i < this.Length; i++)
                    this[i].Symbol = (i + count < this.Length) ? this[i + count].Symbol : ' ';
                foreach (ILCDIndicatorRowObject obj in this.objects)
                {
                    if (obj.Position >= position)
                    {
                        for (int i = obj.Position + obj.Mask.Length - 1; i >= obj.Position + obj.Mask.Length - Math.Min(obj.Mask.Length, count) - 1; i--)
                            this[i].ActivePixelColor = Color.Black;
                        obj.Position -= count;
                        this.SetObjectSymbolsColor(obj);
                    }
                }
            }
            /// <summary>
            /// ���������� true, ���� � ��������� ������� ������ ��������� ������
            /// </summary>
            internal bool ContainsObjectInPosition(int Position)
            {
                bool res = false;
                foreach (ILCDIndicatorRowObject obj in this.objects)
                {
                    if (obj.Position <= Position && obj.Position + obj.Mask.Length > Position)
                    {
                        res = true;
                        break;
                    }
                }
                return res;
            }
            /// <summary>
            /// ���������� ������ � ��������� ������� ������, ��� null, ���� ��� ��� �������
            /// </summary>
            public ILCDIndicatorRowObject GetObject(int Position)
            {
                ILCDIndicatorRowObject res = null;
                foreach (ILCDIndicatorRowObject obj in this.objects)
                {
                    if (obj.Position <= Position && obj.Position + obj.Mask.Length > Position)
                    {
                        res = obj;
                        break;
                    }
                }
                return res;
            }
            /// <summary>
            /// ������� ������, ������� ��� �������
            /// </summary>
            public void Clear()
            {
                this.Text = "";
                this.objects.Clear();
                for (int i = 0; i < this.Length; i++)
                    this[i].ActivePixelColor = Color.Black;
            }
            /// <summary>
            /// ���������� ������ ���� ��������, ���������� � ��������� ������� ������
            /// </summary>
            /// <param name="position">��������� ������� ���������</param>
            /// <param name="length">������ ���������</param>
            internal List<ILCDIndicatorRowObject> GetObjectsInInteval(int position, int length)
            {
                List<ILCDIndicatorRowObject> res = new List<ILCDIndicatorRowObject>();
                foreach (ILCDIndicatorRowObject obj in this.objects)
                {
                    if (obj.Position >= position && obj.Position < position + length)
                        res.Add(obj);
                }
                return res;
            }
            /// <summary>
            /// ������� ��������� ������ �� ������
            /// </summary>
            /// <param name="obj">��������� ������</param>
            public void RemoveObject(ILCDIndicatorRowObject obj)
            {
                this.objects.Remove(obj);
                for (int i = obj.Position; i < obj.Position + obj.Mask.Length; i++)
                {
                    this[i].ActivePixelColor = Color.Black;
                }
            }
        }

        #endregion

        private LCDIndicatorSymbol[,] symbols; // ������ �������� ����������
        private LCDIndicatorRow[] rows;
        private int rowCount = 4; // ����� ����� ������
        private int symbolsInRow = 20; // ����� �������� � ������
        private int horizontalMargin = 10; // ������� �� ������ ���������� ������ � �����
        private int verticalMargin = 10; // ������� �� ������ ���������� ������ � �����
        private int sizeBetweenSymbols = 4; // ���������� ����� ��������� (������������� ������ ��������)
        private Selection selection = new Selection(); // ������� ��������� ��������
        private bool showGrid = true; // �������� �� ������������ �����, ������� ������� �� ����� �� 4
        private bool insertMode = true; // ����� ����� ��������: true - �������, false - ����������
        private Color backColor = Color.FromKnownColor(KnownColor.Gainsboro); // ���� ���� ����������
        private Color objectSymbolsColor = Color.FromArgb(0, 102,204); // ���� �������� �������
        private SpecialSymbolsForm ssf; // ����� ��� ����� ������������
        private ILCDIndicatorRowObject selectedObject = null; // ������ ������, �� ������� ���������� ������ (��� null, ���� ������ �� ����� �� �������)
        private bool denyRaiseRowTextIDEvents = false; // ���� true, �� ������� RowTextInserted � RowTextDeleted �� ������������

        #region Events
        /// <summary>
        /// ��������, ����� ���������� ������� ��� ����� ���������
        /// </summary>
        public event EventHandler SelectionChanged;
        /// <summary>
        /// ���������, ����� ��������� ����� ������, ������ ������ ������������ � EventArgs
        /// </summary>
        public event EventHandler<EventArgs<int>> RowTextChanged;
        /// <summary>
        /// ���������, ����� ����� ��� �������� � ������
        /// </summary>
        public event EventHandler<RowTextIDREventArgs> RowTextInserted;
        /// <summary>
        /// ��������, ����� ����� ��� ������ �� ������
        /// </summary>
        public event EventHandler<RowTextIDREventArgs> RowTextDeleted;
        /// <summary>
        /// ���������, ����� ����� � ������ ��� ������� �� ������
        /// </summary>
        public event EventHandler<RowTextIDREventArgs> RowTextReplaced;
        /// <summary>
        /// �������� ��������� ������� RowTextInserted, RowTextDeleted, RowTextReplaced
        /// </summary>
        public class RowTextIDREventArgs : EventArgs
        {
            private string text1;
            private string text2;
            private int rowIndex;
            private int position;
            /// <summary>
            /// ��� ������� RowTextInserted - ����������� ������,
            /// ��� ������� RowTextDeleted - ��������� ������,
            /// ��� ������� RowTextReplaced - ���������� ������
            /// </summary>
            public string Text1
            {
                get
                {
                    return text1;
                }
            }
            /// <summary>
            /// ��� ������� RowTextInserted � RowTextDeleted - ������ ������,
            /// ��� ������� RowTextReplaced - ������ ������
            /// </summary>
            public string Text2
            {
                get
                {
                    return text2;
                }
            }
            /// <summary>
            /// ������ ������, � ������� ���� ������������� �������
            /// </summary>
            public int RowIndex
            {
                get
                {
                    return this.rowIndex;
                }
            }
            /// <summary>
            /// ������� � ������, � ������� ���� ������������� �������
            /// </summary>
            public int Position
            {
                get
                {
                    return this.position;
                }
            }

            public RowTextIDREventArgs(int RowIndex, int Position, string Text1, string Text2)
            {
                this.rowIndex = RowIndex;
                this.position = Position;
                this.text1 = Text1;
                this.text2 = Text2;
            }
        }
        /// <summary>
        /// ���������, ����� ���������� ����� ����� ������
        /// </summary>
        public event EventHandler InputModeChanged;
        /// <summary>
        /// ���������, ����� ������ ��������������� �� ������ ������
        /// </summary>
        public event EventHandler<EventArgs<ILCDIndicatorRowObject>> LCDIndicatorRowObjectSelected;
        /// <summary>
        /// ���������, ����� ������ ������ � ������� ������
        /// </summary>
        public event EventHandler LCDIndicatorRowObjectLostFocus;
        /// <summary>
        /// ���������, ����� ������ ���������
        /// </summary>
        public event EventHandler<EventArgs<ILCDIndicatorRowObject>> LCDIndicatorRowObjectDeleted;
        public new event MouseEventHandler MouseWheel;
        #endregion

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (this.MouseWheel != null)
                this.MouseWheel(this, e);
        }

        public LCDIndicator()
        {
            InitializeComponent();
            this.BackColor = backColor;
            this.Size = this.MaximumSize;
            this.CreateSymbols();
            this.CreateRows();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetSelection(new Point(0, 0), 1);
        }

        public LCDIndicator(int RowCount, int SymbolsInRow)
        {
            this.rowCount = RowCount;
            this.symbolsInRow = SymbolsInRow;
            InitializeComponent();
            this.BackColor = backColor;
            this.Size = this.MaximumSize;
            this.CreateSymbols();
            this.CreateRows();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetSelection(new Point(0, 0), 1);
        }
        /// <summary>
        /// ���������� ���������� ������ ������ ��� null,
        /// ���� �� ������ ������� �� ��������
        /// </summary>
        public ILCDIndicatorRowObject SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
        }
        /// <summary>
        /// ���������� ������ ���������� � ��������� ��������
        /// </summary>
        public LCDIndicatorRow this[int index]
        {
            get
            {
                return this.rows[index];
            }
            set
            {
                this.rows[index] = value;
            }
        }
        /// <summary>
        /// True, ���� Insert, False - ���� Overwrite
        /// </summary>
        public bool InsertMode
        {
            get
            {
                return this.insertMode;
            }
        }
        /// <summary>
        /// ���������� ���������� ����� � ����������
        /// </summary>
        public string SelectedText
        {
            get
            {
                string res = "";
                int start = Math.Min(this.selection.Start.X, this.selection.End.X);
                for (int i = start; i < start + this.selection.AbsLength; i++)
                    res += this.symbols[this.selection.Start.Y, i].Symbol.ToString();
                return res;
            }
        }
        /// <summary>
        /// ���������� ������� ������� �������
        /// </summary>
        public Point CursorPos
        {
            get
            {
                return this.selection.Start;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ����, ������������, ��������� �� ������������ ����� �� ����������
        /// </summary>
        public bool ShowGrid
        {
            get
            {
                return this.showGrid;
            }
            set
            {
                this.showGrid = value;
            }
        }
        /// <summary>
        /// ���������� ������� ������ ����������
        /// </summary>
        public int CurrentRow
        {
            get
            {
                return this.selection.Start.Y;
            }
        }
        /// <summary>
        /// ���������� ������ ������, �� ������� � ������ ������ ���������� ������
        /// </summary>
        public int CurrentSymbol
        {
            get
            {
                return this.selection.Start.X;
            }
        }
        /// <summary>
        /// ������ ������ ����� ����������
        /// </summary>
        public void CreateRows()
        {
            this.rows = new LCDIndicatorRow[this.rowCount];
            for (int i = 0; i < this.rowCount; i++)
                this.rows[i] = new LCDIndicatorRow(i, this);

        }
        /// <summary>
        /// ������� � �������������� ������ �������� ����������
        /// </summary>
        private void CreateSymbols()
        {
            this.symbols = new LCDIndicatorSymbol[this.rowCount, this.symbolsInRow];
            for (int i = 0; i < this.rowCount; i++)
            {
                for (int j = 0; j < this.symbolsInRow; j++)
                {
                    LCDIndicatorSymbol symbol = new LCDIndicatorSymbol();
                    symbol.PositionInIndicator = new Point(j, i);
                    symbol.Location = new Point(this.horizontalMargin + j * (symbol.Width + this.sizeBetweenSymbols),
                        this.verticalMargin + i * (symbol.Height + this.sizeBetweenSymbols));
                    this.Controls.Add(symbol);
                    this.symbols[i, j] = symbol;
                }
            }
        }

        /// <summary>
        /// ���������� ����, ������� ����� ���������� �������
        /// </summary>
        private Color SelectionColor
        {
            get
            {
                return SystemColors.HotTrack;
            }
        }
        /// <summary>
        /// ���������� ���� ����� ����������
        /// </summary>
        private Color BorderColor
        {
            get
            {
                return SystemColors.Highlight;
            }
        }
        /// <summary>
        /// ������������ ����� ��������� ������ �������
        /// </summary>
        private void DwawSymbolSelection(LCDIndicatorSymbol symbol, Graphics graphics)
        {
            graphics.DrawRectangle(new Pen(this.SelectionColor), new Rectangle(symbol.Left - 1, symbol.Top - 1, symbol.Width + 1, symbol.Height + 1));
        }
        /// <summary>
        /// ���������� ������ � ��������� ������� ��� null, ���� ������� ���
        /// </summary>
        private LCDIndicatorSymbol GetSymbolByPoint(Point p)
        {
            int dx = this.horizontalMargin - this.sizeBetweenSymbols/2;
            int dy = this.verticalMargin - this.sizeBetweenSymbols/2;
            if (p.X < dx || p.X >= this.Width - dx || p.Y < dy || p.Y >= this.Height - dy)
                return null;
            return this.symbols[(p.Y - dy) / (this.symbols[0, 0].Height + this.sizeBetweenSymbols), (p.X - dx) / (this.symbols[0, 0].Width + this.sizeBetweenSymbols)];
        }
        /// <summary>
        /// ���������� ������� SelectionChanged
        /// </summary>
        private void RaiseSelectionChangedEvent()
        {
            if (this.SelectionChanged != null)
                this.SelectionChanged(this, EventArgs.Empty);
        }
        /// <summary>
        /// ���������� ������� RowTextInsertedEventArgs
        /// </summary>
        /// <param name="Text">�������� �������� text1 ��� RowTextIDREventArgs</param>
        /// <param name="CursorPos">������� ������� ������� ����������: X - ������, Y - ������</param>
        private void RaiseRowTextInsertedEvent(Point CursorPos, string Text)
        {
            if (this.RowTextInserted != null && !this.denyRaiseRowTextIDEvents)
                this.RowTextInserted(this, new RowTextIDREventArgs(CursorPos.Y, CursorPos.X, Text, ""));
        }
        /// <summary>
        /// ���������� ������� RowTextDeletedEventArgs
        /// </summary>
        /// <param name="CursorPos">������� ������� ������� ����������: X - ������, Y - ������</param>
        /// <param name="Text">�������� �������� text1 ��� RowTextIDREventArgs</param>
        private void RaiseRowTextDeletedEvent(Point CursorPos, string Text)
        {
            if (this.RowTextDeleted != null && !this.denyRaiseRowTextIDEvents)
                this.RowTextDeleted(this, new RowTextIDREventArgs(CursorPos.Y, CursorPos.X, Text, ""));
        }
        /// <summary>
        /// ���������� ������� RowTextReplacedEventArgs
        /// </summary>
        /// <param name="CursorPos">������� ������� ������� ����������: X - ������, Y - ������</param>
        private void RaiseRowTextReplacedEvent(Point CursorPos, string Text1, string Text2)
        {
            if (this.RowTextReplaced != null && Text1 != Text2)
                this.RowTextReplaced(this, new RowTextIDREventArgs(CursorPos.Y, CursorPos.X, Text1, Text2));
        }
        /// <summary>
        /// ������������ ����� ����������
        /// </summary>
        private void DrawGrid(Graphics graphics)
        {
            for (int i = 1; i < this.rowCount; i++)
            {
                graphics.DrawLine(new Pen(this.BorderColor, 1), this.horizontalMargin, this.verticalMargin + i * (this.symbols[0, 0].Height + this.sizeBetweenSymbols) - this.sizeBetweenSymbols / 2,
                                    this.Width - this.horizontalMargin, this.verticalMargin + i * (this.symbols[0, 0].Height + this.sizeBetweenSymbols) - this.sizeBetweenSymbols / 2);
            }
            for (int i = 4; i < this.symbolsInRow - 1; i += 4)
            {
                graphics.DrawLine(new Pen(this.BorderColor, 1), this.horizontalMargin + i * (this.symbols[0, 0].Width + this.sizeBetweenSymbols) - this.sizeBetweenSymbols / 2, this.verticalMargin,
                                            this.horizontalMargin + i * (this.symbols[0, 0].Width + this.sizeBetweenSymbols) - this.sizeBetweenSymbols / 2, this.Height - this.verticalMargin);
            }
        }
        /// <summary>
        /// ���������� ������ �� 1 ������ ������
        /// </summary>
        private void MoveCursorUp()
        {
            if (this.selection.Start.Y != 0)
                this.SetSelection(new Point(this.selection.Start.X, this.selection.Start.Y - 1), 1);
        }
        /// <summary>
        /// ���������� ������ �� ���� ������ ����
        /// </summary>
        private void MoveCursorDown()
        {
            if (this.selection.Start.Y != this.rowCount - 1)
                this.SetSelection(new Point(this.selection.Start.X, this.selection.Start.Y + 1), 1);
        }
        /// <summary>
        /// ���������� ����� �� 1 ������ �����
        /// </summary>
        private void MoveCursorLeft(bool Shift)
        {
            int StartPosition = this.selection.Start.X;
            int Length = this.selection.Length;
            if (this.CursorPos.X == 0 && (this.selection.AbsLength == 1 || Shift))
                return;
            if (this.selection.AbsLength > 1 && !Shift)
            {
                // ���� �������� ����� 1 ������� � ����� ������ Left, �� ��������� ������ ������� � ������� ���������
                if (!this.ContainsObjectInCursorPos())
                    this.SetSelection((this.selection.Direction == 1 ? this.CursorPos : new Point(this.CursorPos.X + this.selection.Length + 1, this.CursorPos.Y)), 1);
                else if (this.CursorPos.X != 0)
                    this.SetSelection(new Point(this.CursorPos.X - 1, this.CursorPos.Y), 1);
                return;
            }
             // ���������� ������� ������������� ������������� ������ �� �������
            if (!Shift || (this.selection.Direction == -1 && this.selection.AbsLength!=1))
            {
                // ������ ������ ������� LEFT ��� ���������� ������� ����� �� �������, �� ������� � ���� ���������
                this.symbols[this.CursorPos.Y, this.CursorPos.X].Selected = false;
            }
            if (Shift)
                // ������ LEFT + Shift - ����������� ����� ���������
                Length = (this.selection.Length == -1) ? 2 : this.selection.Length + 1;
            // � � Shift'�� � ��� �������� ������ �����
            StartPosition = this.selection.Start.X - 1;
            // ������������� ��������� �� ������ ��� ��������
            this.SetSelection(new Point(StartPosition, this.selection.Start.Y), Length);
        }
        /// <summary>
        /// ���������� ������ �� 1 ������ ������
        /// </summary>
        private void MoveCursorRight(bool Shift)
        {
            int StartPosition = this.selection.Start.X;
            int Length = this.selection.Length;
            if (this.CursorPos.X == this.symbolsInRow - 1 && (this.selection.AbsLength == 1 || Shift))
                return;
            if (this.selection.AbsLength > 1 && !Shift)
            {
                ILCDIndicatorRowObject obj = this[this.CurrentRow].GetObject(StartPosition);
                if (obj != null)
                {
                    int x = (obj.Position + obj.Mask.Length) < this.symbolsInRow ? (obj.Position + obj.Mask.Length) : (this.symbolsInRow - 1); 
                    this.SetSelection(new Point(x, this.selection.Start.Y), 1);
                }
                else
                    // ���� �������� ����� 1 ������� � ����� ������ Right, �� ��������� ������ ������� � ������� ���������
                    this.SetSelection((this.selection.Direction == -1 ? this.CursorPos : new Point(this.CursorPos.X + this.selection.Length - 1, this.CursorPos.Y)), 1);
                return;
            }
            // ���������� ������� ������������� ������������� ������ �� �������
            if (!Shift || (this.selection.Direction == 1 && this.selection.AbsLength!=1 && !this.ContainsObjectInCursorPos()))
            {
                // ������ ������ ������� Right ��� ���������� ������� ������ �� �������, �� ������� � ���� ���������
                this.symbols[this.CursorPos.Y, this.CursorPos.X].Selected = false;
            }
            if (Shift)
                // ������ Right+Shift - ����������� ����� ���������
                Length = (this.selection.Length == 1) ? -2 : this.selection.Length - 1;
            // � � Shift'�� � ��� �������� ������ ������
            StartPosition = this.selection.Start.X + 1;
            ILCDIndicatorRowObject obj1 = this[this.CurrentRow].GetObject(StartPosition);
            if (obj1 != null && Shift)
            {
                Length -= obj1.Position + obj1.Mask.Length - StartPosition;
                StartPosition = (obj1.Position + obj1.Mask.Length) < this.symbolsInRow ? (obj1.Position + obj1.Mask.Length) : (this.symbolsInRow - 1);
            }
            this.SetSelection(new Point(StartPosition, this.selection.Start.Y), (Length==0) ? 1 : Length);
        }
        /// <summary>
        /// ������������� ��������� ������� � ����� ���������� ��������
        /// </summary>
        public void SetSelection(Point StartPosition, int Length)
        {
            Point p = this.CursorPos;
            if (Length == 0)
                Length = 1;
            // ������ �������� ���������
            for (int i = 0; i < this.selection.AbsLength; i++)
            {
                int x = this.selection.Start.X + i * this.selection.Direction;
                /*if (x > Math.Min(StartPosition.X, StartPosition.X + Length) && x < Math.Max(StartPosition.X, StartPosition.X + Length))
                    continue;*/
                this.symbols[this.selection.Start.Y, x].Selected = false;
            }
            // ��������� ��������� ������� ����������
            this.selection.Start = StartPosition;
            // �������� �� ������� ������� � ���������
            ILCDIndicatorRowObject obj = this[StartPosition.Y].GetObject(StartPosition.X);
            if (obj != null)
            {
                // ��������� �������� �� ������
                this.selection.Start = new Point(obj.Position, this.CurrentRow);
                Length += StartPosition.X - obj.Position;
                if (Length < obj.Mask.Length)
                    Length = obj.Mask.Length;
                // ��������� ������� ��������� �������
                if(this.selectedObject!=obj && this.LCDIndicatorRowObjectSelected!=null)
                {
                    this.selectedObject = obj;
                    this.LCDIndicatorRowObjectSelected(this, new EventArgs<ILCDIndicatorRowObject>(obj));
                }
            }
            else if(this.selectedObject!=null && this.LCDIndicatorRowObjectLostFocus!=null)
            {
                // ������ � ��������� �� �����, �� �� ����� �� �� ���
                // ��������� ������� ������ �������� ������
                this.LCDIndicatorRowObjectLostFocus(this, EventArgs.Empty);
                this.selectedObject = null;
            }
            this.selection.Length = Length;
            // ��������� ���� ����������� ��������
            for (int i = 0; i < this.selection.AbsLength; i++)
                this.symbols[this.selection.Start.Y, this.selection.Start.X + i * this.selection.Direction].Selected = true;
            this.Invalidate();
            // ��������� ������� ����� ���������
            if (p != this.CursorPos && this.SelectionChanged != null)
                this.SelectionChanged(this, EventArgs.Empty);
        }
        /// <summary>
        /// ��������� ����� � ������� ������� ����������. ����������:
        /// 0 - ������ �� ��� ��������,
        /// 1 - ������ ��� �������� � ������� ������� �������
        /// 2 - ������ ������� ������ � ������� ������� �������
        /// </summary>
        public int AddSymbol(char Symbol)
        {
            int res = 0;
            // �������� ������� �� ����������
            if (this.insertMode)
            {
                if (this[this.CursorPos.Y][this.symbolsInRow-1].Symbol == ' ')
                {
                    // ��������� ������ - ������, ������� ��������� ����� ������ � ������� �������,
                    // � ��������� �������� ������
                    this[this.CursorPos.Y].MoveSubstringRight(this.CursorPos.X, 1);
                    this.symbols[this.CursorPos.Y,this.CursorPos.X].Symbol = Symbol;
                    res = 1;
                }
                else
                {
                    // ��������� ������ ��������
                    if (this.symbols[this.CursorPos.Y,this.CursorPos.X].Symbol == ' ' && Symbol != ' ')
                    {
                        // ������� ������ - ������, ������ ��� �� �����
                        this.symbols[this.CursorPos.Y,this.CursorPos.X].Symbol = Symbol;
                        res = 2;
                    }
                    else
                        return res;
                }
            }
            else
            {
                this.symbols[this.CursorPos.Y, this.CursorPos.X].Symbol = Symbol;
                res = 2;
            }
            this.MoveCursorRight(false);
            return res;
        }
        /// <summary>
        /// ��������� ������ � ������� ������� �������
        /// </summary>
        public void InsertString(string value)
        {
            string s = this[this.CursorPos.Y].Text;
            if (this.selection.AbsLength != 1 && !this.ContainsObjectInCursorPos())
            {
                this.ReplaceSelection(value);
                return;
            }
            else
            {
                bool b = this.insertMode;
                string text1 = this[this.CurrentRow][this.CurrentSymbol].Symbol.ToString();
                Point cursorPos = (this.selection.Direction == 1) ? this.selection.Start : this.selection.End;
                if (this.ContainsObjectInCursorPos())
                    this.insertMode = true;
                int r = 0; // ��������� ������� �������
                for (int i = 0; i < value.Length; i++)
                    r = this.AddSymbol(value[i]);
                this.insertMode = b;
                if (r == 1)
                    this.RaiseRowTextInsertedEvent(cursorPos, value);
                else if (r == 2)
                    RaiseRowTextReplacedEvent(cursorPos, text1, value);
            }
            if (s != this[this.CursorPos.Y].Text && this.RowTextChanged != null)
                this.RowTextChanged(this, new EventArgs<int>(this.CursorPos.Y));
        }
        /// <summary>
        /// �������� ��������� ������ �����
        /// </summary>
        public void ReplaceSelection(string value)
        {
            this.denyRaiseRowTextIDEvents = true;
            Point cursorPos = (this.selection.Direction == 1) ? this.selection.Start : this.selection.End;
            string text1 = this.SelectedText;
            bool b = this.insertMode;
            this.insertMode = true;
            this.DeleteSelection();
            this.InsertString(value);
            this.insertMode = b;
            this.RaiseRowTextReplacedEvent(cursorPos, text1, value);
            this.denyRaiseRowTextIDEvents = false;
        }
        /// <summary>
        /// ������� ���������� ������� �� ������
        /// </summary>
        public void DeleteSelection()
        {
            string s = this[this.CurrentRow].Text;
            string text1 = this.SelectedText;
            int x = Math.Min(this.selection.Start.X, this.selection.End.X);
            int l = this.selection.AbsLength;
            List<ILCDIndicatorRowObject> objects = this[this.CurrentRow].GetObjectsInInteval(x, l);
            foreach (ILCDIndicatorRowObject obj in objects)
            {
                this[this.CurrentRow].RemoveObject(obj);
                if (this.LCDIndicatorRowObjectDeleted != null)
                    this.LCDIndicatorRowObjectDeleted(this, new EventArgs<ILCDIndicatorRowObject>(obj));
            }
            this.SetSelection(new Point(x, this.selection.Start.Y), 1);
            this[this.CursorPos.Y].MoveSubstringLeft(x + l, l);
            this.SetSelection(new Point(x, this.selection.Start.Y), 1);
            if (this[this.CurrentRow].Text != s)
            {
                this.RaiseRowTextDeletedEvent(new Point(x, this.CurrentRow), text1);
                if (this.RowTextChanged != null)
                    this.RowTextChanged(this, new EventArgs<int>(this.CurrentRow));
            }
        }
        /// <summary>
        /// ���������� �����, �� ������� �������� ���������
        /// </summary>
        private Form GetParentForm()
        {
            Control res = this.Parent;
            while (!(res is Form))
                res = res.Parent;
            return (Form)res;
        }
        /// <summary>
        /// ���������, ��������� �� ������ �� �������
        /// </summary>
        public bool ContainsObjectInCursorPos()
        {
            return this[this.CurrentRow].ContainsObjectInPosition(this.CurrentSymbol);
        }
        /// <summary>
        /// ������������� ����� ����� ������ ���������� �������
        /// </summary>
        public void SetSelectedObjectMask(string Mask)
        {
            ;
        }

        #region Overrides

        protected override void Dispose(bool disposing)
        {
            if (this.ssf != null && this.ssf.Visible)
                this.ssf.Close();
            base.Dispose(disposing);
        }

        public override Size MaximumSize
        {
            get
            {
                LCDIndicatorSymbol symbol = new LCDIndicatorSymbol();
                return new Size(this.horizontalMargin * 2 + this.symbolsInRow * (symbol.Width + this.sizeBetweenSymbols) - this.sizeBetweenSymbols, this.verticalMargin * 2 + this.rowCount * (symbol.Height + this.sizeBetweenSymbols) - this.sizeBetweenSymbols);
            }
        }

        public override Size MinimumSize
        {
            get
            {
                return this.MaximumSize;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.DwawSymbolSelection(this.symbols[this.selection.Start.Y, this.selection.Start.X], e.Graphics);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            pevent.Graphics.DrawRectangle(new Pen(this.BorderColor), new Rectangle(1, 1, this.Width - 2, this.Height - 2));
            if(this.showGrid)
                this.DrawGrid(pevent.Graphics);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            LCDIndicatorSymbol s = this.GetSymbolByPoint(new Point(e.X, e.Y));
            if (s == null)
                goto end;
            if (((LCDIndicator.ModifierKeys & Keys.Shift) == Keys.Shift) && s.PositionInIndicator.Y == this.CursorPos.Y)
            {
                int x = this.selection.Start.X + this.selection.Length - this.selection.Direction;
                int l = (Math.Abs(s.PositionInIndicator.X - x) + 1) * (s.PositionInIndicator.X > x ? -1 : 1);
                if (l < 0 && this[this.CurrentRow].ContainsObjectInPosition(s.PositionInIndicator.X + l + 1))
                    l++;
                this.SetSelection(s.PositionInIndicator, l);
            }
            else
                this.SetSelection(s.PositionInIndicator, 1);
        end:
            base.OnMouseDown(e);
            this.Focus();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                goto end;
            LCDIndicatorSymbol s = this.GetSymbolByPoint(new Point(e.X, e.Y));
            if (s == null || s.PositionInIndicator == this.selection.Start)
                goto end;
            if (s.PositionInIndicator.Y != this.CursorPos.Y)
                this.SetSelection(s.PositionInIndicator, 1);
            else
            {
                int l = -1 * ((s.PositionInIndicator.X - this.selection.End.X) + Math.Sign(s.PositionInIndicator.X - this.selection.End.X));
                if (l < 0 && this[this.CurrentRow].ContainsObjectInPosition(s.PositionInIndicator.X + l + 1))
                    l++;
                this.SetSelection(s.PositionInIndicator, l);
            }
        end:
            this.Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    this.MoveCursorUp();
                    break;
                case Keys.Down:
                    this.MoveCursorDown();
                    break;
                case Keys.Left:
                    this.MoveCursorLeft(e.Shift);
                    break;
                case Keys.Right:
                    this.MoveCursorRight(e.Shift);
                    break;
                case Keys.Home:
                    this.SetSelection(new Point(0, this.CursorPos.Y), e.Shift ? this.selection.End.X + 1: 1);
                    break;
                case Keys.End:
                    this.SetSelection(new Point(this.symbolsInRow - 1, this.CursorPos.Y), e.Shift ? -1 * (this.symbolsInRow - this.selection.End.X): 1);
                    break;
                case Keys.Delete:
                    this.DeleteSelection();
                    break;
                case Keys.Back:
                    if ((this.selection.AbsLength > 1 && !this.ContainsObjectInCursorPos()) || this.selection.Start.X == 0)
                        this.DeleteSelection();
                    else if (this.selection.Start.X != 0)
                    {
                        this.SetSelection(new Point(this.selection.Start.X - 1, this.selection.Start.Y), 1);
                        this.DeleteSelection();
                    }
                    break;
                case Keys.Insert:
                    this.insertMode = !this.insertMode;
                    if (this.InputModeChanged != null)
                        this.InputModeChanged(this, EventArgs.Empty);
                    break;
            }
            base.OnKeyDown(e);
        }

        protected override void  OnKeyPress(KeyPressEventArgs e)
        {
            if ((int)e.KeyChar < 0x20)
                return;
            this.InsertString(e.KeyChar.ToString());
            base.OnKeyPress(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            Keys key = keyData & Keys.KeyCode;
            if ((key == Keys.Up) || (key == Keys.Down) || (key == Keys.Left) || (key == Keys.Right) || (key == Keys.PageUp) || (key == Keys.PageDown))
                return true;
            else
                return base.IsInputKey(keyData);
        }

        #endregion

        #region ContextMenu events

        private void miCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.SelectedText);
        }

        private void miPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                if (this.selection.AbsLength > 1)
                    this.ReplaceSelection(Clipboard.GetText());
                else
                    this.InsertString(Clipboard.GetText());
            }
        }

        private void miCut_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.SelectedText);
            this.DeleteSelection();            
        }

        private void CreateSpecialSymbolsForm()
        {
            this.ssf = new SpecialSymbolsForm(this);
            Point p = this.symbols[this.selection.Start.Y, Math.Min(this.selection.Start.X, this.selection.End.X)].Location;
            this.ssf.Tag = this.PointToScreen(new Point(p.X - 20, p.Y + 38));
            this.ssf.Load += new EventHandler(ssf_Load);
            this.ssf.SymbolSelected += new EventHandler<Kontel.Relkon.EventArgs<char>>(ssf_SymbolSelected);
            this.ssf.Owner = this.GetParentForm();
            this.ssf.Show();
        }

        void ssf_Load(object sender, EventArgs e)
        {
            if (this.ssf.Tag != null)
            {
                Point p = (Point)this.ssf.Tag;
                this.ssf.Tag = null;
                this.ssf.Location = p;
            }
        }

        private void miSpecialSymbol_Click(object sender, EventArgs e)
        {
            if (this.ssf == null || !this.ssf.Visible)
            {
                this.CreateSpecialSymbolsForm();
            }
            this.ssf.Select();
        }

        private void ssf_SymbolSelected(object sender, Kontel.Relkon.EventArgs<char> e)
        {
            //this.AddSymbol(e.Value);
            this.InsertString(e.Value.ToString());
        }

        private void cm_Opening(object sender, CancelEventArgs e)
        {
            this.miSpecialSymbol.Enabled = !this.showGrid;
        }

        #endregion
    }
    /// <summary>
    /// ��������� ��������� ������, ������� ��������� � ������������ ������� ������ ����������
    /// </summary>
    public interface ILCDIndicatorRowObject
    {
        /// <summary>
        /// ������� ������� � ������ ���������
        /// </summary>
        int Position
        {
            get;
            set;
        }
        /// <summary>
        /// ����� ������ ������� �� ������� ����������
        /// </summary>
        string Mask
        {
            get;
            set;
        }
    }
}
