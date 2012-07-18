using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon.Components.Documents;

namespace Kontel.Relkon
{
    public sealed partial class GotoLineForm : Form
    {
        private int lineNumber;
        private int maxLineNumber;
        /// <summary>
        /// Возвращает номер строки, на которую требуется перейти
        /// </summary>
        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }

        public GotoLineForm(int MaxLineNumber, int CurrentLineNumber)
        {
            InitializeComponent();
            this.maxLineNumber = MaxLineNumber;
            this.label1.Text = "Номер строки (1 " + ((char)8211).ToString() + " " + MaxLineNumber + "):";
            this.tbLineNumber.Text = CurrentLineNumber.ToString();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            this.lineNumber = Math.Min(int.Parse(this.tbLineNumber.Text), this.maxLineNumber);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void tbLineNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
                e.Handled = false;
        }

        private void tbLineNumber_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    bOk.PerformClick();
                    break;
                case Keys.Escape:
                    bCancel.PerformClick();
                    break;
            }
        }

        private void GotoLineForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.tbLineNumber;
            this.tbLineNumber.Select(0, this.tbLineNumber.Text.Length);
        }
    }
}