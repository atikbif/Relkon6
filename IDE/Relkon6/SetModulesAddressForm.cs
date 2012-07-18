using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using Kontel.Relkon.Solutions;
using Kontel.Relkon;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Kontel.Relkon
{
    public sealed partial class SetModulesAddressForm : Form
    {
        private List<IOModule> modules = new List<IOModule>(); // список обнаруженных модулей
        private List<string> dinVarsList = new List<string>(); // список всех возможных переменных модулей цифровых входов
        private List<string> doutVarsList = new List<string>(); // список всех возможных переменных модулей цифровых выходов
        private List<string> adcVarsList = new List<string>(); // список всех возможных переменных модулей аналоговых входов
        private List<string> dacVarsList = new List<string>(); // список всех возможных переменных модулей аналоговых выходов

        public SetModulesAddressForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Считывает адреса модулей и заполняет таблицу
        /// </summary>
        private void ReadModulesAddresses()
        {
            using (SerialPort port = new SerialPort((string)this.ddlComPortName.SelectedItem, 115200, Parity.Odd))
            {
                port.Open();
                port.Write(new byte[] { 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x55, 0x55, 0x55, 0xAA, 0xAA, 0xAA }, 0, 12);
                Thread.Sleep(200);
                byte[] res = new byte[port.BytesToRead];
                port.Read(res, 0, res.Length);
                this.dgModules.Rows.Clear();
                this.modules.Clear();
                for (int i = 0; i < res.Length / 2  * 2; i += 2)
                {
                    if (res[i] == res[i + 1])
                    {
                        try
                        {
                            modules.Add(IOModule.Create(res[i]));
                            this.AddModuleToDataGrid(modules[modules.Count - 1]);
                        }
                        catch { }
                    }
                }
            }
        }
        /// <summary>
        /// Добавляет указанный модуль в таблицу отображения модулей
        /// </summary>
        private void AddModuleToDataGrid(IOModule module)
        {
            this.dgModules.Rows.Add();
            DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)this.dgModules[0, this.dgModules.RowCount - 1];
            List<string> varsList = null;
            if (module is DINModule)
                varsList = dinVarsList;
            else if (module is DOUTModule)
                varsList = doutVarsList;
            else if (module is ADCModule)
                varsList = adcVarsList;
            else if (module is DACModule)
                varsList = dacVarsList;
            if (varsList.Count == 0)
            {
                int n0 = (int)module.GetType().GetField("MinModuleNumber").GetValue(module);
                int n1 = (int)module.GetType().GetField("MaxModuleNumber").GetValue(module);
                for (int i = n0; i < n1; i++)
                {
                    IOModule tm = IOModule.Create(i);
                    varsList.Add(this.GetModuleCellLabel(tm));
                }
            }
            foreach (string var in varsList)
            {
                cell.Items.Add(var);
            }
            cell.Value = this.GetModuleCellLabel(module);
        }
        /// <summary>
        /// Для заданного модуля возвращает строку, отображающую этот модуль в таблице
        /// </summary>
        private string GetModuleCellLabel(IOModule module)
        {
           return (module is DigitalModule ? module.VarNames[0].DisplayName : String.Format("{0} - {1}", module.VarNames[0].DisplayName, module.VarNames[module.VarNames.Length - 1].DisplayName));
        }
        /// <summary>
        /// Считывает новые адреса из таблицы и записывает изменившиеся в модули
        /// </summary>
        private void WriteModulesAddresses(string PortName)
        {
            using (SerialPort port = new SerialPort(PortName, 115200, Parity.Odd))
            {
                port.Open();
                for (int i = 0; i < this.modules.Count; i++)
                {
                    if (this.GetModuleCellLabel(modules[i]) == (string)this.dgModules[0, i].Value)
                        continue;
                    // Создаем описание нового модуля (на основани выбора пользователя)
                    IOModule newModule = IOModule.Create(Regex.Match((string)this.dgModules[0, i].Value, "[A-Z]+\\d+").Value);
                    // Запись данных в модуль
                    port.Write(new byte[] { 0x00, 0xE1, (byte)this.modules[i].ModuleNumber, (byte)newModule.ModuleNumber }, 0, 4);
                    Thread.Sleep(100);
                    byte[] res = new byte[port.BytesToRead];
                    port.Read(res, 0, res.Length);
                    // Анализ ответа модуля
                    if (res.Length < 2 || res[0] != res[1] || (res[0] == res[1] && res[0] != newModule.ModuleNumber))
                    {
                        // Неверный ответ
                        this.Invoke(new SimpleDelegate(delegate { this.lbErrors.Items.Add(String.Format("Модулю {0} не удалось присвоить адрес {1}", this.GetModuleCellLabel(this.modules[i]), this.dgModules[0, i].Value)); }));
                        this.dgModules[0, i].Value = this.GetModuleCellLabel(this.modules[i]);
                    }
                    else
                        this.modules[i] = newModule;
                    // Отобрвжение процента зваершния записи
                    this.bwRW.ReportProgress((int)Math.Round(1.0 * i / this.modules.Count * this.pbWriteProgrss.Maximum));
                }
            }
        }

        private void SetModulesAddressForm_Load(object sender, EventArgs e)
        {
            this.ddlComPortName.Items.AddRange(SerialPort.GetPortNames());
            int idx = this.ddlComPortName.Items.IndexOf(Program.Settings.ModulesCOMPort);
            this.ddlComPortName.SelectedIndex = (idx == -1) ? 0 : idx;
            this.Height = Program.Settings.ModulesFormHeight;
            this.splitContainer1.SplitterDistance = Program.Settings.ModulesFormSplitterDistance;
            this.lbErrors.Width = this.splitContainer1.Panel1.Width;
        }

        private void SetModulesAddressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Settings.ModulesCOMPort = (string)this.ddlComPortName.SelectedItem;
            Program.Settings.ModulesFormHeight = this.Height;
            Program.Settings.ModulesFormSplitterDistance = this.splitContainer1.SplitterDistance;
            Program.Settings.Save();
        }

        private void ibReadModulesAddresses_Click(object sender, EventArgs e)
        {
            try
            {
                this.ReadModulesAddresses();
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
            }
        }

        private void ibWriteModulesAddresses_Click(object sender, EventArgs e)
        {
            this.dgModules.EndEdit();
            this.lbErrors.Items.Clear();
            for (int i = 0; i < this.dgModules.RowCount - 1; i++)
            {
                string s = (string)this.dgModules[0, i].Value;
                for (int j = i + 1; j < this.dgModules.RowCount; j++)
                {
                    if (s == (string)this.dgModules[0, j].Value)
                        this.lbErrors.Items.Add(String.Format("Обнаружены модули с одинаковыми адресами ({0})", this.dgModules[0, j].Value));
                }
            }
            if (this.lbErrors.Items.Count != 0)
                return;
            this.ibReadModulesAddresses.Enabled = this.ibWriteModulesAddresses.Enabled = false;
            this.pbWriteProgrss.Value = 0;
            this.toolStripStatusLabel1.Visible = this.pbWriteProgrss.Visible = true;
            this.bwRW.RunWorkerAsync((string)this.ddlComPortName.SelectedItem);
        }

        private void bwRW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.WriteModulesAddresses((string)e.Argument);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
            }
        }

        private void bwRW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.pbWriteProgrss.Value = e.ProgressPercentage;
        }

        private void bwRW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.toolStripStatusLabel1.Visible = this.pbWriteProgrss.Visible = false;
            this.ibReadModulesAddresses.Enabled = this.ibWriteModulesAddresses.Enabled = true;
            if (this.lbErrors.Items.Count == 0)
                this.lbErrors.Items.Add("Операция прошла успешно");
        }

        private void SetModulesAddressForm_SizeChanged(object sender, EventArgs e)
        {
            this.lbErrors.Height = this.splitContainer1.Panel2.Height;
        }

        private void dgModules_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            this.dgModules.CurrentCell = this.dgModules[e.ColumnIndex, e.RowIndex];
        }

        private void lbErrors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.lbErrors.SelectedItem == null)
                return;
            string ModuleLabel = Regex.Match((string)this.lbErrors.SelectedItem, "[A-Z]+\\d+").Value;
            if (ModuleLabel == "")
                return;
            int n = 0;
            byte counter = 0; // счетчик, показывающий сколько раз встретился ошибочный модуль
            for (int i = 0; i < this.dgModules.RowCount; i++)
            {
                if (((string)this.dgModules[0, i].Value).Contains(ModuleLabel))
                {
                    n = i;
                    // Ищем до второго вхождения, чтобы в случае ошибки дублирования модулей показать не первый модуль, а второй
                    if (++counter >= 2)
                        break;
                }
            }
            this.dgModules.Select();
            this.dgModules.CurrentCell = this.dgModules[0, n];
        }
    }
}
