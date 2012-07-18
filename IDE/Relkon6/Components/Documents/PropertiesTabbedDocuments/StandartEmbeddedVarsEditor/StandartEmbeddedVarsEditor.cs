using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon.Classes;
using Kontel.Relkon.Solutions;
using Kontel.Relkon;


namespace Kontel.Relkon.Components.Documents
{
    public sealed partial class StandartEmbeddedVarsEditor : UserControl
    {
        private ControllerProgramSolution solution = null; // список редактируемых переменных
        private string lastModifyEmbeddedVarValue; // значение последней измененной переменной       
        private EmbeddedVarsDataSet.EESingleByteVarDataTable tEESingleByte;
        private EmbeddedVarsDataSet.EEDoubleByteVarDataTable tEEDoubleByte;
        private EmbeddedVarsDataSet.EETetraByteVarDataTable tEETetraByte;

        public StandartEmbeddedVarsEditor()
        {
            this.InitializeComponent();
            this.CreateDataTables();
            this.rbSingleByte.Checked = true;
        }
        /// <summary>
        /// ¬озвращает или устанавливает проект, встроеннные переменные
        /// которого редактирует компонент
        /// </summary>
        public ControllerProgramSolution Solution
        {
            get
            {
                return this.solution;
            }
            set
            {
                this.solution = value;
                if (value != null)
                {
                    this.Update();
                }
            }
        }
        /// <summary>
        /// «авершает редактирование переменных
        /// </summary>
        public void EndEdit()
        {         
            this.dgEESingleByte.EndEdit();
            this.dgEEDoubleByte.EndEdit();
            this.dgEETetraByte.EndEdit();
        }
        /// <summary>
        /// ќбновл€ет содержимое таблиц
        /// </summary>
        public new void Update()
        {
            this.FillSingleByteVarsTables();
            this.FillDoubleByteVarsTables();
            this.FillTetraByteVarsTables();
        }

        #region Displaying embedded vars
        /// <summary>
        /// ѕровер€ет, €вл€етс€ ли значение валидным дл€ указанной таблицы
        /// в случае успеха возвращает 0
        /// </summary>
        private long IsValidCellValue(DataGridView table, long value)
        {
            long res = 0;
            if (table == this.dgEESingleByte)
            {
                if (value < -0x7F)
                    res = -0x7F;
                if (value > 0xFF)
                    res = 0xFF;
            }
            else if (table == this.dgEEDoubleByte)
            {
                if (value < -0x7FFF)
                    res = -0x7FFF;
                if (value > 0xFFFF)
                    res = 0xFFFF;
            }
            else if (table == this.dgEETetraByte)
            {
                if (value < -0x7FFFFFFF)
                    res = -0x7FFFFFFF;
                if (value > 0xFFFFFFFF)
                    res = 0xFFFFFFFF;
            }
            return res;
        }

        private void DataGrids_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView Sender = (DataGridView)sender;
            try
            {
                long i = long.Parse(Sender[e.ColumnIndex, e.RowIndex].Value.ToString());
                long l = this.IsValidCellValue(Sender, i);
                if (l != 0)
                {
                    i = l;
                    ((DataTable)Sender.DataSource).Rows[e.RowIndex][e.ColumnIndex] = i;
                }
                string VarName = ((Sender.RowCount == 4) ? Sender[0, e.RowIndex].Value.ToString() : "") + Sender.Columns[e.ColumnIndex].HeaderText;
                this.solution.Vars.EmbeddedVars.SetEmbeddedVarValue(VarName, i, this.solution.ProcessorParams.InverseByteOrder);
                // »зменение значений в DataTables дл€ измененных переменных
                List<ControllerEmbeddedVar> vars = this.solution.Vars.EmbeddedVars.GetAllAssignedEmbeddedVars(VarName);
                foreach (ControllerEmbeddedVar v in vars)
                {
                    if (v.Name != VarName)
                    {

                        Point p = this.GetVarTableLocation(v);
                        this.GetVarDataTable(v).Rows[p.X][p.Y] = v.Value;
                    }
                }
            }
            catch
            {
                Sender[e.ColumnIndex, e.RowIndex].Value = this.lastModifyEmbeddedVarValue;
            }
        }
        /// <summary>
        /// ¬озвращает DataTable, который отображает указанную переменную
        /// </summary>
        private DataTable GetVarDataTable(ControllerEmbeddedVar var)
        {
            DataTable res = null;           
            if (var.Name.Contains("i"))
                res = this.tEEDoubleByte;
            else if (var.Name.Contains("l"))
                res = this.tEETetraByte;
            else
                res = this.tEESingleByte;            
            return res;
        }
        /// <summary>
        /// ¬озвращает позицию (строка, столбец) в DataTable, в которой
        /// выводитс€ указанна€ переменна€
        /// </summary>
        private Point GetVarTableLocation(ControllerEmbeddedVar var)
        {
            Point res = new Point();
            if (var.Name[0] != 'E')
            {
                res.X = (byte)var.Name[0] - (byte)'W';
                if (var.Name.Contains("i"))
                    res.Y = this.solution.Vars.EmbeddedVars.GetEmbeddedVarIndex(var.Name) / 2 + 1;
                else if (var.Name.Contains("l"))
                    res.Y = this.solution.Vars.EmbeddedVars.GetEmbeddedVarIndex(var.Name) / 4 + 1;
                else
                    res.Y = this.solution.Vars.EmbeddedVars.GetEmbeddedVarIndex(var.Name) + 1;
            }
            else
            {
                res.X = 0;
                if (var.Name.Contains("i"))
                    res.Y = this.solution.Vars.EmbeddedVars.GetEmbeddedVarIndex(var.Name) / 2;
                else if (var.Name.Contains("l"))
                    res.Y = this.solution.Vars.EmbeddedVars.GetEmbeddedVarIndex(var.Name) / 4;
                else
                    res.Y = this.solution.Vars.EmbeddedVars.GetEmbeddedVarIndex(var.Name);
            }
            return res;
        }

        private void DataGrids_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.lastModifyEmbeddedVarValue = ((DataGridView)sender)[e.ColumnIndex, e.RowIndex].Value.ToString();
        }
        /// <summary>
        /// ¬озвращает общую ширину колонок табицы
        /// </summary>
        /// <param name="table"></param>
        private int GetTotalColumnsWidth(DataGridView table)
        {
            return table.Columns[0].Width + table.Columns[1].Width * (table.Columns.Count - 2) + table.Columns[table.Columns.Count - 1].Width;
        }

        private void rbSingleByte_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowDataGrids(1);
        }

        private void rbDoubleByte_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowDataGrids(2);
        }

        private void rbTetraByte_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowDataGrids(4);
        }

        private void DataGrids_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Utils.ErrorMessage(e.Exception.Message);
            e.Cancel = true;
        }

        private void DataGridViews_SizeChanged(object sender, EventArgs e)
        {
            if (!((DataGridView)sender).Visible)
                return;
            int width = this.GetTotalColumnsWidth((DataGridView)sender);
            //this.ComputeTablesColumnsWidth((DataGridView)sender);
            this.ComputeTablesHeight((DataGridView)sender);
        }

        private void DataGrids_DataSourceChanged(object sender, EventArgs e)
        {
            //this.ComputeTablesHeight((DataGridView)sender);
        }
        /// <summary>
        /// ¬ычисл€ет высоту указанной таблицы
        /// </summary>
        private void ComputeTablesHeight(DataGridView table)
        {
            if (table.Rows.Count == 0)
                return;
            table.Parent.Height = table.ColumnHeadersHeight + table.Rows[0].Height * table.Rows.Count + table.Parent.Padding.Top + table.Parent.Padding.Bottom + 19;
            int flag = (!System.Windows.Forms.VisualStyles.VisualStyleInformation.IsEnabledByUser) ? 1 : 0;
            if (this.GetTotalColumnsWidth(table) > table.Width - 3 - flag)
                table.Parent.Height += 22;
            table.Height = table.Parent.Height - table.Parent.Padding.Top - table.Parent.Padding.Bottom - 17;
        }
        /// <summary>
        /// —оздает таблицы дл€ отображени€ заводских установок
        /// </summary>
        private void CreateDataTables()
        {           
            this.tEESingleByte = new EmbeddedVarsDataSet.EESingleByteVarDataTable();
            this.dgEESingleByte.DataSource = this.tEESingleByte;

            this.tEEDoubleByte = new EmbeddedVarsDataSet.EEDoubleByteVarDataTable();
            this.dgEEDoubleByte.DataSource = this.tEEDoubleByte;

            this.tEETetraByte = new EmbeddedVarsDataSet.EETetraByteVarDataTable();
            this.dgEETetraByte.DataSource = this.tEETetraByte;
        }
        /// <summary>
        /// ¬ычисл€ет ширину колонок указанной таблицы
        /// </summary>
        //private void ComputeTablesColumnsWidth(DataGridView table)
        //{
        //    bool b = (table == this.dgWXYZSingleByte || table == this.dgWXYZDoubleByte || table == this.dgWXYZTetraByte);
        //    int TotalWidth = (b ? table.Width - table.Columns[0].Width : table.Width);
        //    int TotalCount = (b ? table.Columns.Count - 1 : table.Columns.Count);
        //    int width = (int)Math.Round(1.0 * TotalWidth / TotalCount);
        //    if (width < table.Columns[1].MinimumWidth)
        //        width = table.Columns[1].MinimumWidth;
        //    int LastColumnWidth = table.Width - table.Columns[0].Width - width * (table.Columns.Count - 2) - 3;
        //    if (table.Columns[1].Width != width)
        //    {
        //        foreach (DataGridViewColumn c in table.Columns)
        //        {
        //            if ((b && c.Index == 0) || c.Index == table.Columns.Count - 1)
        //                continue;
        //            else
        //            {
        //                if (LastColumnWidth < table.Columns[table.Columns.Count - 1].MinimumWidth && width > table.Columns[1].MinimumWidth)
        //                {
        //                    LastColumnWidth++;
        //                    c.Width = width - 1;
        //                }
        //                else
        //                    c.Width = width;
        //            }
        //        }
        //    }
        //    if (table.Columns[table.Columns.Count - 1].Width != LastColumnWidth)
        //        table.Columns[table.Columns.Count - 1].Width = LastColumnWidth;
        //}
        /// <summary>
        /// ѕоказывает таблицы переменных с указанным размером
        /// </summary>
        /// <param name="NumberOfBytes"></param>
        private void ShowDataGrids(int NumberOfBytes)
        {
            this.EndEdit();
            this.dgEESingleByte.Hide();
            this.dgEEDoubleByte.Hide();
            this.dgEETetraByte.Hide();
            switch (NumberOfBytes)
            {
                case 1:
                    this.dgEESingleByte.Show();
                    break;
                case 2:
                    this.dgEEDoubleByte.Show();
                    break;
                case 4:
                    this.dgEETetraByte.Show();
                    break;
            }
        }
        /// <summary>
        /// «аполн€ет таблицы однобайтовыми значени€ми
        /// </summary>
        private void FillSingleByteVarsTables()
        {            
            this.tEESingleByte.Rows.Clear();
            var EEParams = new object[1024];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 256; j++)
                    EEParams[i * 256 + j] = this.solution.Vars.GetEmbeddedVar("EE" + (i * 256 + j)).Value;
            this.tEESingleByte.Rows.Add(EEParams);            
        }
        /// <summary>
        /// «аполн€ет таблицы двухбайтными значени€ми
        /// </summary>
        private void FillDoubleByteVarsTables()
        {    
            this.tEEDoubleByte.Rows.Clear();     
            var EEParams = new object[512];
            for (int i = 0; i < 4; i++)                           
                for (int j = 0; j < 256; j += 2)
                    EEParams[(i * 256 + j) / 2] = this.solution.Vars.GetEmbeddedVar("EE" + (i * 256 + j) + "i").Value;                                            
            this.tEEDoubleByte.Rows.Add(EEParams);
        }
        /// <summary>
        /// «аполн€ет таблицы четырех байтными значени€ми
        /// </summary>
        private void FillTetraByteVarsTables()
        {          
            this.tEETetraByte.Rows.Clear();          
            var EEParams = new object[256];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 256; j += 4)
                    EEParams[(i * 256 + j) / 4] = this.solution.Vars.GetEmbeddedVar("EE" + (i * 256 + j) + "l").Value;                                         
            this.tEETetraByte.Rows.Add(EEParams);
        }
        #endregion

        
    }
}
