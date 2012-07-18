using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon.Solutions;

namespace Kontel.Relkon
{
    public sealed partial class UpdateControllerForm : Form
    {
        private string selectedFileName = "";// имя проекта, данными которого д. б. обновлен контроллер

        public UpdateControllerForm()
        {
            InitializeComponent();
        }

        public UpdateControllerForm(List<string> ControllerProgramSolutionFilesNames)
        {
            InitializeComponent();
            this.FillTable(ControllerProgramSolutionFilesNames);
        }
        /// <summary>
        /// Возвращает имя файла выбранного проекта
        /// </summary>
        public string SelectedFileName
        {
            get
            {
                return this.selectedFileName;
            }
        }
        /// <summary>
        /// Заполняет таблицу данными проекта
        /// </summary>
        private void FillTable(List<string> SolutionFilesNames)
        {
            foreach (string FileName in SolutionFilesNames)
            {
                ControllerProgramSolution sln = null;
                try
                {
                    sln = ControllerProgramSolution.FromFile(FileName);
                }
                catch
                {
                    continue;
                }
                this.dgSolutions.Rows.Add();
                this.dgSolutions[0, this.dgSolutions.RowCount - 1].Value = sln.Name;
                this.dgSolutions[1, this.dgSolutions.RowCount - 1].Value = FileName;
            }
        }

        private void dgSolutions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.bOk.PerformClick();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Проекты Relkon (*.rproj, *.rpj)|*.rproj; *.rpj";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.selectedFileName = dlg.FileName;
                this.bOk.PerformClick();
            }
        }

        private void dgSolutions_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgSolutions.SelectedRows.Count > 0)
            {
                this.selectedFileName = (string)this.dgSolutions[1, this.dgSolutions.SelectedRows[0].Index].Value;
            }
        }
    }
}