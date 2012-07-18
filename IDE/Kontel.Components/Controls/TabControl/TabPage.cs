using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Kontel.Relkon;

namespace Kontel.Relkon
{
    /// <summary>
    /// Клиентская часть вкладки TabControl
    /// </summary>
    public sealed partial class TabPage : Panel
    {
        private TabPageHeader header; // закладка для данной TabPage
        /// <summary>
        /// Возникает, когда TabPage становится активной вкладкой TabControl
        /// </summary>
        public event EventHandler OnSelect;

        public TabPage()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            base.Dock = DockStyle.Fill;
            this.Header = new TabPageHeader();
            this.BackColor = Color.FromKnownColor(KnownColor.Control);
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                return new Rectangle(this.header.Width + 2, 2, this.Width - this.header.Width - 3, this.Height - 3);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(155, 172, 156)), this.Header.Width, 0, this.Width - this.Header.Width - 1, this.Height - 1);
            if (this.header.State == TabPageState.Pushed)
                e.Graphics.DrawLine(new Pen(Color.FromArgb(207, 211, 194)), this.Header.Width, this.Header.Top, this.Header.Width, this.Header.Top + this.Header.Height);

        }

        /// <summary>
        /// Возвращает или устанавливает закладку для данной TabPage
        /// </summary>
        internal TabPageHeader Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = value;
                this.header.OnStateChanged += new EventHandler<EventArgs<TabPageState>>(header_OnStateChanged);
            }
        }

        private void header_OnStateChanged(object sender, EventArgs<TabPageState> e)
        {
            if (e.Value == TabPageState.Pushed && this.OnSelect != null)
                this.OnSelect(this, EventArgs.Empty);
        }
        [Browsable(true)]
        public override string  Text
        {
            get
            {
                return this.header.Text;
            }
            set
            {
                this.header.Text = value;
            }
        }
        /// <summary>
        /// Снимает выделение с заголовка вкладки
        /// </summary>
        internal void Deselect()
        {
            this.header.State = TabPageState.Normal;
        }
    }

    #region TabPageCollection declaration
    public class TabPageCollection : List<TabPage>
    {
        private TabControl owner; // компонент, вкладки которого содержит список

        public TabPageCollection(TabControl owner)
        {
            this.owner = owner;
        }

        public new void Add(TabPage item)
        {
            base.Add(item);
            if(!this.owner.Controls.Contains(item))
                this.owner.Controls.Add(item);
        }

        public new void Remove(TabPage item)
        {
            base.Remove(item);
            if (this.owner.Controls.Contains(item))
                this.owner.Controls.Remove(item);
        }

        public new void RemoveAt(int index)
        {
            TabPage Item = this[index];
            base.RemoveAt(index);
            this.owner.Controls.Remove(Item);
        }
    }
    #endregion

    #region TabPageCollectionEditor decalration

    public class TabPageCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        public TabPageCollectionEditor()
            : base(typeof(TabPageCollection))
        {

        }

        protected override object SetItems(object editValue, object[] value)
        {
            /*TabControl control1 = base.Context.Instance as TabControl;
            
            foreach (object obj1 in value)
            {
                control1.TabPages.Add(obj1 as TabPage);
            }
            object obj2 = base.SetItems(editValue, value);
            return obj2;*/
            TabControl control1 = base.Context.Instance as TabControl;
            if (control1 != null)
            {
                control1.SuspendLayout();
            }
            object obj2 = base.SetItems(editValue, value);
            if (control1 != null)
            {
                control1.ResumeLayout();
            }
            return obj2;

        }
    }
    #endregion
}
