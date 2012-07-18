using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TD.SandDock;
using Kontel.Relkon.Classes;
using Kontel.Relkon;

namespace Kontel.Relkon.Components
{
    public sealed partial class ErrorList : UserDockableWindow
    {
        private int lastModifySizeColumnIndex = -1; // индекс колонки таблицы, ширина которой изменялась последний раз
        /// <summary>
        /// Возникает при двойном щелчке на строке ошибки
        /// </summary>
        public event RelkonCompilationErrorEventHandler RelkonCompilationErrorShow;
        /// <summary>
        /// Возникает при щелчке на кнопке открытия файла 
        /// </summary>
        public event FileEventHandler ErrorFileMustOpen;
        private string errorsFileName = "";

        public ErrorList()
        {
            InitializeComponent();
            this.TabImage = this.imageList1.Images[0];
        }

        public override string TabText
        {
            get
            {
                return "Список ошибок";
            }
        }

        public override string Text
        {
            get
            {
                return "Список ошибок";
            }
        }

        /// <summary>
        /// Выводит ошибки
        /// </summary>
        /// <param name="Errors">Список ошибок</param>
        public void PrintErrors(List<CompilationError> Errors, string ErrorsFileName)
        {
            this.errorsFileName = ErrorsFileName;
            this.tsbOpenErrorFile.Enabled = System.IO.File.Exists(ErrorsFileName);
            this.dgErrors.Rows.Clear();
            int ErrorsCount = 0;
            int WarningsCount = 0;
            foreach (CompilationError error in Errors)
            {
                if (error.Warning)
                    WarningsCount++;
                else
                    ErrorsCount++;
                object[] values = new object[4];
                values[0] = error.Warning ? this.imageList1.Images[2] : this.imageList1.Images[1];
                values[1] = error.Description;
                values[2] = error.FileName;
                values[3] = error.LineNumber>0 ? error.LineNumber.ToString() : "";
                this.dgErrors.Rows.Add(values);
                this.dgErrors.Rows[this.dgErrors.Rows.Count - 1].Tag = error;
            }
            this.tsbShowErrors.Text = this.GetErrorsButtonCaption(ErrorsCount);
            this.tsbShowWarnings.Text = this.GetWarningsButtonCaption(WarningsCount);
        }
        /// <summary>
        /// Возвращает подпись к кнопке показа ошибок
        /// </summary>
        /// <param name="ErrorsCount"></param>
        private string GetErrorsButtonCaption(int ErrorsCount)
        {
            string res = ErrorsCount.ToString() + " ";
            string ec = ErrorsCount.ToString();
            string s = "Ошибок";
            if (ec.Length == 1 || ec[ec.Length - 2] != '1')
            {
                
                if (ec[ec.Length - 1] == '1')
                    s = "Ошибка";
                if (ec[ec.Length - 1] == '2' || ec[ec.Length - 1] == '3' || ec[ec.Length - 1] == '4')
                    s = "Ошибки";
            }
            return res + s;
        }
        /// <summary>
        /// Возвращает подпись к кнопке показа предупреждений
        /// </summary>
        /// <param name="WarningsCount"></param>
        private string GetWarningsButtonCaption(int WarningsCount)
        {
            string res = WarningsCount.ToString() + " ";
            string ec = WarningsCount.ToString();
            string s = "Предупреждений";
            if (ec.Length == 1 || ec[ec.Length - 2] != '1')
            {

                if (ec[ec.Length - 1] == '1')
                    s = "Предупреждение";
                if (ec[ec.Length - 1] == '2' || ec[ec.Length - 1] == '3' || ec[ec.Length - 1] == '4')
                    s = "Предупреждения";
            }
            return res + s;
        }

        #region Columns width change
        /// <summary>
        /// Возвращает или устанавливает ширину колонки описания ошибки
        /// </summary>
        public int DescriptionColumnWidth
        {
            get
            {
                return this.dgErrors.Columns[1].Width;
            }
            set
            {
                this.dgErrors.Columns[1].Width = value;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает ширину колонки файла ошибки
        /// </summary>
        public int FileColumnWidth
        {
            get
            {
                return this.dgErrors.Columns[2].Width;
            }
            set
            {
                this.dgErrors.Columns[2].Width = value;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает ширину колонки файла ошибки
        /// </summary>
        public int LineColumnWidth
        {
            get
            {
                return this.dgErrors.Columns[3].Width;
            }
            set
            {
                this.dgErrors.Columns[3].Width = value;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает индекс последней колонки, у которой изменялась ширина
        /// </summary>
        public int LastModifySizeColumnIndex
        {
            get
            {
                return this.lastModifySizeColumnIndex;
            }
            set
            {
                this.lastModifySizeColumnIndex = value;
            }
        }
        private bool b = false;
        /// <summary>
        /// Пересчитывает размеры колонок после изменения размеров одной из них или размеров компонента
        /// </summary>
        private void ComputeColumnsWidth()
        {
            b = true;
            if (this.lastModifySizeColumnIndex == -1)
                return;
            int width = 0;
            for (int i = 0; i < this.dgErrors.Columns.Count; i++)
            {
                if (i != this.lastModifySizeColumnIndex)
                    width += this.dgErrors.Columns[i].Width;
            }
            this.dgErrors.Columns[this.lastModifySizeColumnIndex].Width = this.Width - width - 4;
            b = false;
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.ComputeColumnsWidth();
        }

        private void dgErrors_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Index == 3 || this.lastModifySizeColumnIndex == -1 || b)
                return;
            this.lastModifySizeColumnIndex = e.Column.Index + 1;
            this.ComputeColumnsWidth();
            this.lastModifySizeColumnIndex = e.Column.Index;
        }
        /// <summary>
        /// Вычисляет ширину колонки строки ошибки так, чтобы сумма размеров 
        /// всех колонок была равна шиине компонента
        /// </summary>
        public void ComputeLineNumberWidth()
        {
            this.dgErrors.Columns[3].Width = this.Width - this.dgErrors.Columns[0].Width - this.dgErrors.Columns[1].Width - this.dgErrors.Columns[2].Width - 4;
        }
        #endregion

        private void dgErrors_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgErrors.RowCount == 0 || e.RowIndex == -1)
                return;
            if (this.RelkonCompilationErrorShow != null)
                this.RelkonCompilationErrorShow(this, new CompilationErrorEventArgs((CompilationError)this.dgErrors.Rows[e.RowIndex].Tag));
        }

        private void tsbShowErrors_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dgErrors.Rows)
            {
                if (!((CompilationError)row.Tag).Warning)
                    row.Visible = (this.tsbShowErrors.Checked);
            }
        }

        private void tsbShowWarnings_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dgErrors.Rows)
            {
                if (((CompilationError)row.Tag).Warning)
                    row.Visible = this.tsbShowWarnings.Checked;
            }
        }

        private void tsbOpenErrorFile_Click(object sender, EventArgs e)
        {
            if (this.ErrorFileMustOpen != null)
                this.ErrorFileMustOpen(this, new FileEventArgs(this.errorsFileName));
        }
        /// <summary>
        /// Очищает список ошибок
        /// </summary>
        public void Clear()
        {
            this.dgErrors.Rows.Clear();
        }
        /// <summary>
        /// Сбрасывает компонент в состояние по-умолчанию
        /// </summary>
        public void SetToDefaultState()
        {
            this.tsbShowErrors.Text = this.GetErrorsButtonCaption(0);
            this.tsbShowWarnings.Text = this.GetWarningsButtonCaption(0);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.miCopy.Enabled = this.dgErrors.RowCount > 0;
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            string text = "";
            for(int i = 1; i<this.dgErrors.SelectedRows[0].Cells.Count; i++)
            {
                text += this.dgErrors.SelectedRows[0].Cells[i].Value.ToString() + "\t";
            }
            Clipboard.SetText(text);
        }
    }

    #region Events declare

    #region RelkonCompilationError
    public class CompilationErrorEventArgs : EventArgs
    {
        private CompilationError error;

        public CompilationError Error
        {
            get
            {
                return this.error;
            }
        }

        public CompilationErrorEventArgs(CompilationError Error)
        {
            this.error = Error;
        }
    }

    public delegate void RelkonCompilationErrorEventHandler(object sender, CompilationErrorEventArgs e);
    #endregion

    #region ErrorFileOpen event

    public class FileEventArgs : EventArgs
    {
        private string fileName;

        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        public FileEventArgs(string FileName)
        {
            this.fileName = FileName;
        }
    }

    public delegate void FileEventHandler(object sender, FileEventArgs e);

    #endregion
    #endregion

}
