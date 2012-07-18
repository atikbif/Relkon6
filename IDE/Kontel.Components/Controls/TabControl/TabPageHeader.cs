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
    /// �������� ����� ������� (������� ���������� �������)
    /// </summary>
    public sealed partial class TabPageHeader : Label
    {
        private TabPageState state = TabPageState.Normal; // ���������� ������� ��������� �������
        /// <summary>
        /// ��������� ��������� ��������� ����������
        /// </summary>
        public event EventHandler<EventArgs<TabPageState>> OnStateChanged;

        public TabPageHeader()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ���������� ��� ������������� ������� ��������� ����������
        /// (������ ��� ����������� �������������)
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
        /// ������������ ���������� ��������� ����������
        /// </summary>
        private void PaintNormalMode(Graphics graphics)
        {
            //��������� ����
            LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 255, 246), Color.FromArgb(242, 236, 219), 0f);
            graphics.FillRectangle(brush, this.ClientRectangle);
            // ��������� ������ �������
            brush = new LinearGradientBrush(new Point(4, this.Height - 1), new Point(this.Width - 7, this.Height - 1), Color.FromArgb(255, 255, 250), Color.FromArgb(249, 246, 239));
            graphics.DrawLine(new Pen(brush), 4, this.Height - 1, this.Width - 7, this.Height - 1);
            // ��������� ������� �������
            brush = new LinearGradientBrush(new Point(4, this.Height - 1), new Point(this.Width - 7, 0), Color.FromArgb(239, 238, 228), Color.FromArgb(233, 229, 216));
            graphics.DrawLine(new Pen(brush), 4, 0, this.Width - 7, 0);
        }
        /// <summary>
        /// ��������� ����� ������� ���������� ��� �������� Pushed ��� Popuped
        /// </summary>
        private void PaintPushedPopupedLeftBorder(Graphics graphics)
        {
            //��������� ������� �����
            Pen pen = new Pen(Color.FromArgb(207, 114, 37));
            graphics.DrawLine(pen, 2, 0, 0, 2);
            graphics.DrawLine(pen, 2, this.Height-1, 0, this.Height-3);
            graphics.DrawLine(pen, 0, 2, 0, this.Height - 3);
            // ���������� ���������� �������
            pen.Color = Color.FromArgb(227, 147, 84);
            graphics.DrawLine(pen, 1, 2, 1, this.Height - 3);
            graphics.DrawLine(pen, 2, 1, 2, this.Height - 2);
        }
        /// <summary>
        /// ��������� ����� ������ ���������� ��� ��������� Pushed � Popuped
        /// </summary>
        private void PaintPushedPopupedBorder(Graphics graphics)
        {
            SolidBrush sbrush = new SolidBrush(Color.FromArgb(145, 155, 156));
            // ��������� ������ �������
            graphics.DrawLine(new Pen(sbrush), 3, this.Height - 1, this.Width - 1, this.Height - 1);
            // ��������� ������� �������
            graphics.DrawLine(new Pen(sbrush), 3, 0, this.Width - 1, 0);
            // ��������� ����� �������
            this.PaintPushedPopupedLeftBorder(graphics);
        }
        /// <summary>
        /// ��������� ���������� � ��������� Popuped
        /// </summary>
        private void PaintPopupedMode(Graphics graphics)
        {
            // ��������� ����
            LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 255, 246), Color.FromArgb(242, 236, 219), 0f);
            graphics.FillRectangle(brush, this.ClientRectangle);
            // ��������� ������
            this.PaintPushedPopupedBorder(graphics);
        }
        /// <summary>
        /// ��������� ���������� � ��������� Pushed
        /// </summary>
        private void PaintPushedMode(Graphics graphics)
        {
            // ��������� ����
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 246));
            graphics.FillRectangle(brush, this.ClientRectangle);
            // ��������� �����, ������� � ������ ������
            this.PaintPushedPopupedBorder(graphics);
            // ��������� ������ �������
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
    /// ���������� ��������� ��������� ����������
    /// </summary>
    public enum TabPageState
    {
        /// <summary>
        /// ���������� ��������� ����������
        /// </summary>
        Normal,
        /// <summary>
        /// �� ��������� ������� ��������� ����
        /// </summary>
        Popuped,
        /// <summary>
        /// ��������� �����
        /// </summary>
        Pushed
    }
    #endregion
}
