using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon;

namespace Kontel.Relkon.Forms
{
    /// <summary>
    /// Форма, отображащая долю завершения какого-либо процесса
    /// </summary>
    public sealed partial class ProgressForm : Kontel.Relkon.Forms.ShadowedForm
    {
        private enum ButtonState
        {
            Normal,
            Pushed,
            Poped
        }
        
        private ButtonState state = ButtonState.Normal; // флаг, показывающий текущее состояние формы
        private Color activeCrossColor = Color.Black;
        private Color inactiveCrossColor = Color.Gray;

        /// <summary>
        /// Текст сообщения на форме
        /// </summary>
        public string Message
        {
            get
            {
                return this.lMessage.Text;
            }
            set
            {
                this.lMessage.Text = value;
            }
        }
        /// <summary>
        /// Текущее значение индикатора прогресса
        /// </summary>
        public int ProgressPercentage
        {
            get
            {
                return (int)Math.Round(100.0 * this.pbProgress.Value / this.pbProgress.Maximum);
            }
            set
            {
                if (value < 0 || value > 100)
                    throw new Exception(String.Format("Неверное значение: {0}. ProgressForm.ProgressPercentage может принимать значения из интервала 0..100", value));
                this.pbProgress.Value = (int)Math.Round(value/100.0 * this.pbProgress.Maximum);
            }
        }
        
        public ProgressForm(Form owner)
        {
            InitializeComponent();
            this.Owner = owner;
            this.pCloseButton.BorderColor = this.BackColor;
            this.state = ButtonState.Normal;
        }
        /// <summary>
        /// Отрисовывает крест на кнопке закрытия
        /// </summary>
        private void DrawCross(Graphics graphcs, Color color)
        {
            Rectangle r = new Rectangle(3,3,1,1);
            Brush b = new SolidBrush(color);
            for (int i = 0; i < 14; i++)
            {
                graphcs.FillRectangle(b, r);
                if (i % 2 == 0)
                    r.X++;
                else
                    r.Y++;
            }
            r = new Rectangle(3, 9, 1, 1);
            for (int i = 0; i < 14; i++)
            {
                graphcs.FillRectangle(b, r);
                if (i % 2 == 0)
                    r.X++;
                else
                    r.Y--;
            }
        }
        /// <summary>
        /// Устанавливает новое состояние кнопки закрытия
        /// </summary>
        private void SetState(ButtonState State)
        {
            if (this.state != State)
            {
                this.state = State;
                this.pCloseButton.Invalidate();
            }
        }

        private void DrawNormalState(Graphics graphics)
        {
            this.DrawCross(graphics, this.inactiveCrossColor);
        }

        private void DrawPopedState(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(244, 244, 238)), this.ClientRectangle);
            graphics.DrawRectangle(new Pen(this.panel1.BackColor), new Rectangle(0, 0, this.pCloseButton.Width - 1, this.pCloseButton.Height - 1));
            this.DrawCross(graphics, this.activeCrossColor);
        }

        private void DrawPushedState(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(227, 227, 219)), this.ClientRectangle);
            graphics.DrawRectangle(new Pen(this.panel1.BackColor), new Rectangle(0, 0, this.pCloseButton.Width - 1, this.pCloseButton.Height - 1));
            this.DrawCross(graphics, this.activeCrossColor);
        }

        private void pCloseButton_Paint(object sender, PaintEventArgs e)
        {
            switch (this.state)
            {
                case ButtonState.Normal:
                    this.DrawNormalState(e.Graphics);
                    break;
                case ButtonState.Poped:
                    this.DrawPopedState(e.Graphics);
                    break;
                case ButtonState.Pushed:
                    this.DrawPushedState(e.Graphics);
                    break;
            }
        }

        private void pCloseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.SetState(ButtonState.Pushed);
        }

        private void pCloseButton_MouseEnter(object sender, EventArgs e)
        {
            this.SetState(ButtonState.Poped);
        }

        private void pCloseButton_MouseLeave(object sender, EventArgs e)
        {
            this.SetState(ButtonState.Normal);
        }

        private void pCloseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.SetState(ButtonState.Poped);
        }

        private void pCloseButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}