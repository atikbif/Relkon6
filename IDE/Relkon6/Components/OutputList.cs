using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TD.SandDock;
using System.IO;

namespace Kontel.Relkon.Components
{
    public sealed partial class OutputList : UserDockableWindow
    {
        private StringWriter stream = new StringWriter();

        public OutputList()
        {
            InitializeComponent();
            this.TabImage = this.imageList1.Images[0];
        }
        /// <summary>
        /// Добавляет строку к списку вывода
        /// </summary>
        /// <param name="Line"></param>
        public void WriteLine(string Line)
        {
            this.list.Text += Line.Replace("\r\n", "") + "\r\n";
            this.list.SelectionStart = this.list.Text.Length - 1;
            this.list.SelectionLength = 0;
            this.list.ScrollToCaret();
        }
        /// <summary>
        /// Очищает список
        /// </summary>
        public void Clear()
        {
            this.list.Text = "";
        }
        /// <summary>
        /// Добавляет массив строк к списку
        /// </summary>
        /// <param name="Line"></param>
        public void WriteLines(string[] Lines)
        {
            for (int i = 0; i < Lines.Length; i++)
            {
                this.WriteLine(Lines[i]);
            }
        }
    }
}
