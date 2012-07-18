using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Kontel.Relkon.Forms
{
    /// <summary>
    /// Форма, отбрасывающая тень
    /// </summary>
    public partial class ShadowedForm : Form
    {
        public ShadowedForm()
        {
            this.InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (System.Environment.OSVersion.Version.Major > 5 || (System.Environment.OSVersion.Version.Major == 5 && System.Environment.OSVersion.Version.Minor >= 1))
                    cp.ClassStyle |= 0x20000;
                return cp;
            }
        }
    }
}