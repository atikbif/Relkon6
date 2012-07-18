using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon;

namespace Kontel.Relkon
{
    /// <summary>
    /// Вывода на индикатор специальных символов
    /// </summary>
    public sealed partial class SpecialSymbolsForm : Form
    {
        public event EventHandler<EventArgs<char>> SymbolSelected;
        private LCDIndicator indicator;
        public SpecialSymbolsForm(LCDIndicator Indicator)
        {
            InitializeComponent();
            this.indicator = Indicator;
        }

        private void Symbol_Clicked(object sender, EventArgs e)
        {
            this.indicator.Focus();
            ToolStripButton Sender = (ToolStripButton)sender;
            int code = 0xFF00 + AppliedMath.HexToDec(Sender.Tag.ToString());
            if (this.SymbolSelected != null)
                this.SymbolSelected(this, new EventArgs<char>((char)code));
            this.Focus();
        }

        private void SpecialSymbolsForm_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(this.Width, this.Height);
        }

        private void SpecialSymbolsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void toolStripButton1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!this.Focused)
            //    this.Focus();
        }

    }
}