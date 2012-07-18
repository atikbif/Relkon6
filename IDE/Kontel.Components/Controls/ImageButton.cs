using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Kontel.Relkon
{
    public sealed class ImageButton : Panel
    {
        private enum ButtonState
        {
            Normal,
            Pushed,
            Poped
        }

        private Image image = null;
        private Image inactiveImage = null;
        private ButtonState state = ButtonState.Normal;
        private Point imageLocation = new Point(0, 0);

        public ImageButton()
        {
            this.DoubleBuffered = true;
        }

        public Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                if (image != null)
                    this.imageLocation = this.ComputeImageLocation();
                this.Invalidate();
            }
        }

        public Image InactiveImage
        {
            get
            {
                return this.inactiveImage;
            }
            set
            {
                this.inactiveImage = value;
            }
        }

        private Point ComputeImageLocation()
        {
            if (this.image == null)
                return new Point(0, 0);
            int x = (int)Math.Round(1.0 * Math.Max(this.Width - this.image.Width, 0) / 2);
            int y = (int)Math.Round(1.0 * Math.Max(this.Height - this.image.Height, 0) / 2);
            return new Point(x, y);
        }

        private void SetState(ButtonState State)
        {
            if (this.state != State)
            {
                this.state = State;
                this.Invalidate();
            }
        }

        private void DrawNormalState(Graphics graphics)
        {
            if (this.image != null)
                graphics.DrawImageUnscaled(((!this.Enabled && this.inactiveImage != null) ? this.inactiveImage : this.image), this.imageLocation);
        }

        private void DrawPopedState(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(244, 244, 238)), this.ClientRectangle);
            graphics.DrawRectangle(new Pen(Color.FromArgb(127, 157, 185)), new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            if (this.image != null)
                graphics.DrawImageUnscaled(this.image, this.imageLocation);
        }
        
        private void DrawPushedState(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(227, 227, 219)), this.ClientRectangle);
            graphics.DrawRectangle(new Pen(Color.FromArgb(127, 157, 185)), new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            if (this.image != null)
                graphics.DrawImageUnscaled(this.image, new Point(this.imageLocation.X + 1, this.imageLocation.Y + 1));
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.SetState(ButtonState.Poped);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.SetState(ButtonState.Pushed);
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.SetState(ButtonState.Normal);
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.SetState(ButtonState.Poped);
            base.OnMouseUp(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.imageLocation = this.ComputeImageLocation();
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(this.image!=null)
                ((Bitmap)this.image).SetResolution(e.Graphics.DpiX, e.Graphics.DpiY);
            if (this.inactiveImage != null)
                ((Bitmap)this.inactiveImage).SetResolution(e.Graphics.DpiX, e.Graphics.DpiY);
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
    }
}
