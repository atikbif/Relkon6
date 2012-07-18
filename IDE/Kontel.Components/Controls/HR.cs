using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Kontel.Relkon
{
    /// <summary>
    /// Элемент управления, представляющий собой горизонтальную линию
    /// </summary>
    public sealed class HR : Panel
    {
        public HR()
        {
            base.MaximumSize = new Size(100000, 1);
            this.DoubleBuffered = true;
        }
        /*[Browsable(false)]
        public override System.Drawing.Size MaximumSize
        {
            get
            {
                return new System.Drawing.Size(base.MaximumSize.Width, 1);
            }
        }*/

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.White), 0, 0, this.Width - 1, 0);
            if (this.Width > 2)
                e.Graphics.DrawLine(new Pen(Color.FromArgb(172, 168, 163)), 1, 0, this.Width - 2, 0);
        }
    }
}
