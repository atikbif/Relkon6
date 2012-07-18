using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Kontel.Relkon
{
    /// <summary>
    /// Реализует все возможнности панели, но кроме того
    /// еще окружает компонент рамкой указанного цвета
    /// </summary>
    public sealed class BorderPanel : Panel
    {
        private Color borderColor = Color.FromKnownColor(KnownColor.ActiveBorder);
        private Graphics graphics = null;

        public BorderPanel()
        {
            this.DoubleBuffered = true;
            this.Padding = new Padding(1);
            this.graphics = this.CreateGraphics();
        }
        /// <summary>
        /// Возвращает или устанавливает цвет рамки компонента
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                this.borderColor = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //g.FillRectangle(new SolidBrush(this.BackColor), this.Bounds);
            e.Graphics.DrawRectangle(new Pen(this.borderColor, 1), new Rectangle(0, 0, this.Width - 1, this.Height - 1));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            this.Invalidate();
        }
    }
}
