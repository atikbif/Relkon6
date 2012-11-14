using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using QWhale.Editor;
using System.Drawing.Printing;
using Kontel.Relkon;
using Kontel.Relkon.Solutions;
using Kontel.TabbedDocumentsForm;
using System.Text.RegularExpressions;
using QWhale.Syntax;
using System.Collections.Generic;
using QWhale.Common;

namespace Kontel.Relkon.Components.Documents
{
    /// <summary>
    /// Описывает документ редактирования текста
    /// </summary>
    public sealed partial class EditorTabbedDocument : FileTabbedDocument, IEditableTabbedDocument, IPrintableTabbedDocument
    {
        public  delegate void SetPositionStringDelegate(string PostionString);
        private readonly SetPositionStringDelegate setPositionFunction; // функция, устанавливающая строку с текущей позицие курсора

        private string wholeLineCopied = ""; // буфер для записи целиком скопированной строки

        private int pageNumber = 0; // номер печатуемой страницы
        private int printedLines = 0; // число напечптанных строк
        private int maxPrintedLines = 0; // число строк, которые нужно напечатать
        private int nCopies = 1; // чисо копий
        private float linesPerPage = 0; // число строк на странице
        private string[] printingLines; // массив строк для печати

        public EditorTabbedDocument(ControllerProgramSolution Solution, string FileName, SetPositionStringDelegate SetPositionFunction)
            : base(Solution, FileName)
        {
            InitializeComponent();
            this.editor.Source.UndoOptions = QWhale.Editor.UndoOptions.AllowUndo;
            this.editor.UseDefaultMenu = false;
            this.ReloadFont();
            this.setPositionFunction = SetPositionFunction;
            if (string.IsNullOrEmpty(FileName))
                return;
            this.LoadFromFile(FileName);
            this.initialized = true;
        }
        /// <summary>
        /// По новой загружет шрифт редактора из настроек программы
        /// </summary>
        public void ReloadFont()
        {
            this.editor.Font = Program.Settings.EditorFont;
        }
        /// <summary>
        /// Возвращает число строк в документе
        /// </summary>
        public int LinesCount
        {
            get
            {
                return this.editor.Lines.Count;
            }
        }
        /// <summary>
        /// Устанавливает в редакторе подсветку синтаксиса под соответствующий тип файла
        /// </summary>
        /// <param name="fileName">Имя файла, для которого надо установит подсветку синтаксиса</param>
        private void SetLanguage(string fileName)
        {
            string Extension = Path.GetExtension(fileName);
            switch (Extension)
            {
                case ".asm":
                    this.LanguageParser.Language = QWhale.Syntax.Schemes.Languages.Assembler;
                    break;
                case ".c":
                case ".h":
                    this.LanguageParser.Language = QWhale.Syntax.Schemes.Languages.Custom;
                    this.LanguageParser.LoadScheme(Utils.ApplicationDirectory + "\\c.xml");
                    break;
                case ".kon":
                    this.LanguageParser.Language = QWhale.Syntax.Schemes.Languages.Custom;
                    this.LanguageParser.LoadScheme(Utils.ApplicationDirectory + "\\Relkon.xml");
                    break;
                case ".map":
                    this.LanguageParser.Language = QWhale.Syntax.Schemes.Languages.Custom;
                    this.LanguageParser.LoadScheme(Utils.ApplicationDirectory + "\\Map.xml");
                    break;
                case ".xml":
                    this.LanguageParser.Language = QWhale.Syntax.Schemes.Languages.Xml;
                    break;
                default:
                    this.editor.Lexer = null;
                    break;
            }
        }
        /// <summary>
        /// Загружает указанный файл
        /// </summary>
        protected override void LoadFromFile(string fileName)
        {
            this.SetLanguage(fileName);
            this.editor.Text = File.ReadAllText(fileName, this.FileEncoding);
            this.fileHash = this.GetFileHash(fileName);
        }
        /// <summary>
        /// Запускает поиск указанного текста в документе
        /// </summary>
        /// <returns>true, если текст был найден, в противном случае - false</returns>
        public bool Find(string SearchingText, bool MatchCase, bool WholeWord, bool SearchUp)
        {
            return this.editor.Find(SearchingText, this.GetSearchOptions(MatchCase, WholeWord, SearchUp));
        }
        /// <summary>
        /// Ищет следующее вхождение текста, указанного в функции Find
        /// </summary>
        /// <returns>true, если текст был найден, в противном случае - false</returns>
        public bool FindNext()
        {
            return ((this.editor.SearchOptions & SearchOptions.BackwardSearch) == SearchOptions.BackwardSearch) ? this.editor.FindPrevious() : this.editor.FindNext();
        }
        /// <summary>
        /// Заменяет ищет первое вхождение указанного текста в документе и заменяет его новым
        /// </summary>
        /// <returns>true, если текст был найден, в противном случае - false</returns>
        public bool Replace(string SearchingText, string ReplacingText, bool MatchCase, bool WholeWord, bool SearchUp)
        {
            return this.editor.Replace(SearchingText, ReplacingText, this.GetSearchOptions(MatchCase, WholeWord, SearchUp));
        }
        /// <summary>
        /// Заменяет все вхождения заданного текста в документе новым значением
        /// </summary>
        /// <returns>Число произведенных замен</returns>
        public int ReplaceAll(string SearchingText, string ReplacingText, bool MatchCase, bool WholeWord, bool SearchUp)
        {
            int c;
            this.editor.ReplaceAll(SearchingText, ReplacingText, this.GetSearchOptions(MatchCase, WholeWord, SearchUp), out c);
            return c;
            //return this.editor.ReplaceAll(SearchingText, ReplacingText, this.GetSearchOptions(MatchCase, WholeWord, SearchUp));
        }
        /// <summary>
        /// Перезагружает файл
        /// </summary>
        public void Reload()
        {
            this.LoadFromFile(this.FileName);
            this.DocumentModified = false;
        }
        /// <summary>
        /// Выделяет строку с указанным номером
        /// </summary>
        public void SelectLine(int LineNumber)
        {
            this.editor.Select();
            this.editor.MoveToLine(LineNumber - 1);
            this.editor.Selection.SelectLine();
        }
        /// <summary>
        /// Перемещает указатель на строку с указанным номером
        /// </summary>
        public void MoveToLine(int LineNumer)
        {
            this.editor.Select();
            this.editor.MoveToLine(LineNumer - 1);
        }
        /// <summary>
        /// Возвращает текущую строку документа
        /// </summary>
        public int CurrentLineNumer
        {
            get
            {
                return this.editor.Position.Y + 1;
            }
        }
        /// <summary>
        /// Возвращает редактор, связанный с документом
        /// </summary>
        public SyntaxEdit Editor
        {
            get
            {
                return this.editor;
            }
        }
        /// <summary>
        /// Подгтавливает документ к печати
        /// </summary>
        private void PreparingToPrint(PrintDocument Document)
        {
            printedLines = 0;
            pageNumber = 0;
            nCopies = Document.PrinterSettings.Copies;
            if (this.editor.Selection.SelectedText != "")
            {
                int startLine = this.editor.Source.GetPositionFromCharIndex(this.editor.Selection.SelectionStart).Y;
                int endLine = this.editor.Source.GetPositionFromCharIndex(this.editor.Selection.SelectionStart + this.editor.Selection.SelectionLength - 1).Y;
                printingLines = new string[endLine - startLine + 1];
                for (int i = 0; i < printingLines.Length; i++)
                    printingLines[i] = this.editor.Lines[i + startLine];
                printingLines[0] = printingLines[0].Substring(this.editor.Source.GetPositionFromCharIndex(this.editor.Selection.SelectionStart).X);
                printingLines[printingLines.Length - 1] = (printingLines[printingLines.Length - 1] + " ").Remove(this.editor.Source.GetPositionFromCharIndex(this.editor.Selection.SelectionStart + this.editor.Selection.SelectionLength - 1).X);
            }
            else
                printingLines = null;
            Document.PrintPage += new PrintPageEventHandler(Document_PrintPage);
            Document.EndPrint += new PrintEventHandler(Document_EndPrint);
        }
        /// <summary>
        /// Завершает печать
        /// </summary>
        private void EndPrintig(PrintDocument doc)
        {
            doc.PrintPage -= new PrintPageEventHandler(Document_PrintPage);
            doc.EndPrint -= new PrintEventHandler(Document_EndPrint);
        }
        /// <summary>
        /// Обрежает строку,если она выходит за границы печати
        /// </summary>
        private int WrapLine(string line, PrintPageEventArgs e)
        {
            Graphics gr1 = e.Graphics;
            SizeF w = gr1.MeasureString(line, this.editor.Font);
            if (w.Width < e.MarginBounds.Width)
                return -1;
            char[] pattern = { ' ', '\t', ',', ';', '+', '-', ':', '*', '&', '%', '$', '#', '!', '\\', '|' };
            int i = (int)(((double)e.MarginBounds.Width) / w.Width * line.Length);
            int j = line.LastIndexOfAny(pattern, i);
            if (j != -1)
                i = j;
            return i;
        }

        private QWhale.Editor.SearchOptions GetSearchOptions(bool MatchCase, bool WholeWord, bool SearchUp)
        {
            //QWhale.Editor.SearchOptions res = QWhale.Editor.SearchOptions.EntireScope;
            QWhale.Editor.SearchOptions res = QWhale.Editor.SearchOptions.CycledSearch;
            if (MatchCase)
                res |= QWhale.Editor.SearchOptions.CaseSensitive;
            if (WholeWord)
                res |= QWhale.Editor.SearchOptions.WholeWordsOnly;
            if (SearchUp)
                res |= QWhale.Editor.SearchOptions.BackwardSearch;
            return res;
        }

        public override string Text
        {
            get
            {
                if (this.editor != null)
                    return this.editor.Text;
                else
                    return "";
            }
            set
            {
                if (this.editor != null)
                    this.editor.Text = value;
            }
        }

        protected override Encoding FileEncoding
        {
            get
            {
                return Encoding.Default;
            }
        }

        protected override void PerformSaving(string FileName)
        {
            File.WriteAllText(FileName, this.editor.Text, this.FileEncoding);
        }

        public bool CanUndo
        {
            get
            {
                return this.editor.Source.CanUndo();
            }
        }

        public bool CanRedo
        {
            get
            {
                return this.editor.Source.CanRedo();
            }
        }

        public bool CanPaste
        {
            get
            {
                return editor.Selection.CanPaste();
            }
        }

        public void Cut()
        {
            if (!this.editor.Selection.IsEmpty)
                this.editor.Selection.Cut();
            else
                this.editor.Selection.CutLine();
        }

        public void Copy()
        {
            if (this.editor.Selection.SelectedText == "")
            {
                this.wholeLineCopied = this.editor.Lines[this.editor.Position.Y] + "\r\n";
                Clipboard.SetText(this.wholeLineCopied);
            }
            else
                this.editor.Selection.Copy();
        }

        public void Paste()
        {
            if (Clipboard.GetText() == this.wholeLineCopied)
            {
                int idx = 0;
                for (idx = this.editor.Selection.SelectionStart; (idx >= 0 && this.editor.Text[idx] != '\n'); idx--) ;
                this.editor.Selection.SelectionStart = idx;
                this.editor.Selection.Paste();
            }
            else
            {
                this.editor.Selection.Paste();
                this.wholeLineCopied = "";
            }
        }

        public void Undo()
        {
            this.editor.Source.Undo();
        }

        public void Redo()
        {
            this.editor.Source.Redo();
        }

        public void SelectAll()
        {
            this.editor.Selection.SelectAll();
        }

        public void Delete()
        {
            if (this.editor.Selection.IsEmpty)
                this.editor.Selection.DeleteRight();
            else
                this.editor.Selection.Delete();
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
            if (this.editor.Selection.SelectedText != "")
            {
                dlg.AllowSelection = true;
                dlg.PrinterSettings.PrintRange = PrintRange.Selection;
            }
            else
            {
                dlg.AllowSelection = false;
                dlg.PrinterSettings.PrintRange = PrintRange.AllPages;
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.PreparingToPrint(dlg.Document);
                if (!(dlg.AllowSelection && dlg.PrinterSettings.PrintRange == PrintRange.Selection))
                    printingLines = null;
                try
                {
                    dlg.Document.Print();
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage("Ошибка печати: " + ex.Message);
                    this.EndPrintig(dlg.Document);
                }
            }
        }

        private void Document_PrintPage(object sender, PrintPageEventArgs e)
        {
            float yPos = 0;
            int count = 0;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            string line = "";
            pageNumber++;
            maxPrintedLines = printingLines == null ? this.editor.Lines.Count : printingLines.Length;
            linesPerPage = e.MarginBounds.Height / this.editor.Font.GetHeight(e.Graphics);
            ///////////////////////////////////////////////////////
            string ss = DateTime.Now.ToString("dd.MM.yy");
            Font f = new Font(this.editor.Font.Name, 10);
            e.Graphics.DrawString(this.FileName, f, Brushes.Black, e.MarginBounds.X, e.MarginBounds.Y - 10 - f.Height);
            e.Graphics.DrawString(ss, f, Brushes.Black, e.MarginBounds.X + e.MarginBounds.Width - e.Graphics.MeasureString(ss, f).Width, e.MarginBounds.Y - 10 - f.Height);
            e.Graphics.DrawLine(new Pen(Brushes.Black, 1), e.MarginBounds.X, e.MarginBounds.Y - 8, e.MarginBounds.X + e.MarginBounds.Width, e.MarginBounds.Y - 8);
            string s = pageNumber.ToString();
            e.Graphics.DrawString(s, f, Brushes.Black, e.MarginBounds.X + e.MarginBounds.Width / 2, e.MarginBounds.Y + 12 + e.MarginBounds.Height);
            e.Graphics.DrawLine(new Pen(Brushes.Black, 1), e.MarginBounds.X, e.MarginBounds.Y + e.MarginBounds.Height + 10, e.MarginBounds.X + e.MarginBounds.Width, e.MarginBounds.Y + e.MarginBounds.Height + 10);
            ////////////////////////////////////////////////////////
            while (count < linesPerPage && printedLines < maxPrintedLines)
            {
                if (line == "")
                    line = printingLines == null ? this.editor.Lines[printedLines++] : printingLines[printedLines++];
                yPos = topMargin + (count * f.GetHeight(e.Graphics));
                int i = WrapLine(line, e);
                StringFormat sf1 = new StringFormat();
                if (i != -1)
                {
                    e.Graphics.DrawString(line.Substring(0, i), f, Brushes.Black, leftMargin, yPos, new StringFormat());
                    line = line.Substring(i);
                }
                else
                {
                    e.Graphics.DrawString(line, f, Brushes.Black, leftMargin, yPos, new StringFormat());
                    line = "";
                }
                count++;
            }
            if (printedLines < maxPrintedLines)
                e.HasMorePages = true;
            else
            {
                if ((--nCopies) == 0)
                    e.HasMorePages = false;
                else
                {
                    printedLines = 0;
                    e.HasMorePages = true;
                }
            }
        }

        private void Document_EndPrint(object sender, PrintEventArgs e)
        {
            this.EndPrintig((PrintDocument)sender);
        }

        private void editor_DragDrop(object sender, DragEventArgs e)
        {
            this.editor.Invalidate();
        }

        private void miFind_Click(object sender, EventArgs e)
        {
            MainForm.MainFormInstance.RunFindReplaceForm(false);
        }

        private void miReplace_Click(object sender, EventArgs e)
        {
            MainForm.MainFormInstance.RunFindReplaceForm(true);
        }

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            if(this.initialized)
                this.DocumentModified = true;
        }

        private void Editor_SourceStateChanged(object sender, QWhale.Editor.NotifyEventArgs e)
        {
            if ((e.State & QWhale.Editor.NotifyState.PositionChanged) == QWhale.Editor.NotifyState.PositionChanged)
                this.setPositionFunction("Cтрока " + (this.editor.Position.Y + 1) + new string(' ', 6) + "Cимвол " + (this.editor.Position.X + 1));           
        }

        private void miCut_Click(object sender, EventArgs e)
        {
            this.Cut();
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            this.Copy();
        }

        private void miPaste_Click(object sender, EventArgs e)
        {
            this.Paste();
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            this.Delete();
        }

        private void miUndo_Click(object sender, EventArgs e)
        {
            this.Undo();
        }

        private void miRedo_Click(object sender, EventArgs e)
        {
            this.Redo();
        }

        private void miSelectAll_Click(object sender, EventArgs e)
        {
            this.SelectAll();
        }

        private void EditiorContextMenu_Opening(object sender, CancelEventArgs e)
        {
            this.miUndo.Enabled = this.CanUndo;
            this.miRedo.Enabled = this.CanRedo;
            this.miPaste.Enabled = this.CanPaste;
        }


        private Point PrevPosition(Point position)
        {
            Point pos = position;
            if (pos.Y > 0)
            {
                pos.Y--;
                pos.X = Math.Max(0, this.editor.Strings[pos.Y].Length - 1);
            }
            else
                pos.X--;
            return pos;
        }

        //private void DoCustomOutlining()
        //{
        //    Point oldPos = this.editor.Position;
        //    this.editor.Source.BeginUpdate();
        //    try
        //    {
        //        string regexp = @"#PROCESS.*$";
        //        if (this.editor.Find(regexp, SearchOptions.EntireScope | SearchOptions.RegularExpressions, new Regex(regexp, RegexOptions.Singleline)))
        //        {
        //            IList<IRange> ranges = new List<IRange>();
        //            Point start = this.editor.Position;
        //            while (this.editor.FindNext())
        //            {
        //                ranges.Add(new OutlineRange(start, PrevPosition(this.editor.Position), 0, "..."));
        //                start = this.editor.Position;
        //            }
        //            ranges.Add(new OutlineRange(start, new Point(this.editor.Lines[this.editor.Lines.Count - 1].Length, this.editor.Lines.Count - 1), 0, "..."));
        //            this.editor.Outlining.SetOutlineRanges(ranges, false);
        //        }
        //        this.editor.Selection.Clear();
        //    }
        //    finally
        //    {
        //        this.editor.MoveTo(oldPos);
        //        this.editor.Source.EndUpdate();
        //    }
        //}

    }
}
