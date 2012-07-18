using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Kontel.Relkon;

namespace Kontel.Relkon
{
    /// <summary>
    /// Активная часть вкладки (которая отображает надпись)
    /// </summary>
    public sealed partial class TabPageHeader : Label
    {
        private TabPageState state = TabPageState.Normal; // определяет текущее состояние объекта
        /// <summary>
        /// Возникает изменения состояния компонента
        /// </summary>
        public event EventHandler<EventArgs<TabPageState>> OnStateChanged;

        public TabPageHeader()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Возвращает или устанавливает текущее состояние компонента
        /// (только для внутреннего использования)
        /// </summary>
        internal TabPageState State
        {
            get
            {
                return this.state;
            }
            set
            {
                if (this.state == value)
                    return;
                this.state = value;
                if (this.OnStateChanged != null)
                    this.OnStateChanged(this, new EventArgs<TabPageState>(this.state));
                this.Invalidate();
            }
        }

        #region Paint methods
        /// <summary>
        /// Отрисовывает нормальное состояние компонента
        /// </summary>
        private void PaintNormalMode(Graphics graphics)
        {
            //Отрисовка фона
            LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 255, 246), Color.FromArgb(242, 236, 219), 0f);
            graphics.FillRectangle(brush, this.ClientRectangle);
            // Отрисовка нижней границы
            brush = new LinearGradientBrush(new Point(4, this.Height - 1), new Point(this.Width - 7, this.Height - 1), Color.FromArgb(255, 255, 250), Color.FromArgb(249, 246, 239));
            graphics.DrawLine(new Pen(brush), 4, this.Height - 1, this.Width - 7, this.Height - 1);
            // Отрисовка верхней границы
            brush = new LinearGradientBrush(new Point(4, this.Height - 1), new Point(this.Width - 7, 0), Color.FromArgb(239, 238, 228), Color.FromArgb(233, 229, 216));
            graphics.DrawLine(new Pen(brush), 4, 0, this.Width - 7, 0);
        }
        /// <summary>
        /// Отрисовка левой границы компонента для сстояний Pushed или Popuped
        /// </summary>
        private void PaintPushedPopupedLeftBorder(Graphics graphics)
        {
            //Отрисовка внешней рамки
            Pen pen = new Pen(Color.FromArgb(207, 114, 37));
            graphics.DrawLine(pen, 2, 0, 0, 2);
            graphics.DrawLine(pen, 2, this.Height-1, 0, this.Height-3);
            graphics.DrawLine(pen, 0, 2, 0, this.Height - 3);
            // Заполнение внутренней области
            pen.Color = Color.FromArgb(227, 147, 84);
            graphics.DrawLine(pen, 1, 2, 1, this.Height - 3);
            graphics.DrawLine(pen, 2, 1, 2, this.Height - 2);
        }
        /// <summary>
        /// Отрисовка общих границ компонента для состояний Pushed и Popuped
        /// </summary>
        private void PaintPushedPopupedBorder(Graphics graphics)
        {
            SolidBrush sbrush = new SolidBrush(Color.FromArgb(145, 155, 156));
            // Отрисовка нижней границы
            graphics.DrawLine(new Pen(sbrush), 3, this.Height - 1, this.Width - 1, this.Height - 1);
            // Отрисовка верхней границы
            graphics.DrawLine(new Pen(sbrush), 3, 0, this.Width - 1, 0);
            // Отрисовка левой границы
            this.PaintPushedPopupedLeftBorder(graphics);
        }
        /// <summary>
        /// Отрисовка компонента в состоянии Popuped
        /// </summary>
        private void PaintPopupedMode(Graphics graphics)
        {
            // Отрисовка фона
            LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 255, 246), Color.FromArgb(242, 236, 219), 0f);
            graphics.FillRectangle(brush, this.ClientRectangle);
            // Отрисовка границ
            this.PaintPushedPopupedBorder(graphics);
        }
        /// <summary>
        /// Отрисовка компонента в состоянии Pushed
        /// </summary>
        private void PaintPushedMode(Graphics graphics)
        {
            // Отрисовка фона
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 246));
            graphics.FillRectangle(brush, this.ClientRectangle);
            // Отрисовка левой, верхней и нижней границ
            this.PaintPushedPopupedBorder(graphics);
            // Отрисовка правой границы
            //graphics.DrawLine(new Pen(Color.FromArgb(207, 211, 194)), new Point(this.Width, 1), new Point(this.Width, this.Height - 2));
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            switch (this.State)
            {
                case TabPageState.Normal:
                    this.PaintNormalMode(pevent.Graphics);
                    break;
                case TabPageState.Popuped:
                    this.PaintPopupedMode(pevent.Graphics);
                    break;
                case TabPageState.Pushed:
                    this.PaintPushedMode(pevent.Graphics);
                    break;
            }
        }
        #endregion

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (this.State != TabPageState.Pushed)
            {
                this.State = TabPageState.Pushed;
                this.Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if(this.State==TabPageState.Normal)
            {
                this.State = TabPageState.Popuped;
                this.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (this.State == TabPageState.Popuped)
            {
                this.State = TabPageState.Normal;
                this.Invalidate();
            }
        }


    }

    #region TabPageState definition
    /// <summary>
    /// Определяет возможные состояния компонента
    /// </summary>
    public enum TabPageState
    {
        /// <summary>
        /// Нормальное состояние компонента
        /// </summary>
        Normal,
        /// <summary>
        /// На компонент наведен указатель мыши
        /// </summary>
        Popuped,
        /// <summary>
        /// Компонент нажат
        /// </summary>
        Pushed
    }
    #endregion
}
