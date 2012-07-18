using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon.Solutions;
using Kontel.Relkon.Forms;

namespace Kontel.Relkon
{
    public sealed partial class ModifiedFilesForm : Form
    {
        public ModifiedFilesForm(List<string> ModifiedFiles)
        {
            InitializeComponent();
            for (int i = 0; i < ModifiedFiles.Count; i++)
                this.lbFileNames.Items.Add(ModifiedFiles[i]);
        }

        private void bYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void bNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }        
    }
}