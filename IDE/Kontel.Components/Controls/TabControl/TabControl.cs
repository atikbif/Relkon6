using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Windows.Forms.Design.Behavior;
using System.Collections;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;

namespace Kontel.Relkon
{
    /// <summary>
    /// Элемент управления, аналогичный используемому в Visual Studio 
    /// для отображения свойств проекта.
    /// 
    /// Очень глючный. Желательно полностью переписать
    /// </summary>
    [DefaultProperty("TabPages"), Designer(typeof(TabControlDesigner)), DefaultEvent("SelectedIndexChanged")]
    public sealed partial class TabControl : UserControl
    {
        private TabPageCollection tabPages; // список вкладок компонента
        private int selectedIndex = -1; // индекс активной вкладки
        // Возникает при изменении индекса активной вкладки
        public event EventHandler SelectedIndexChanged;

        public TabControl()
        {
            InitializeComponent();
            this.tabPages = new TabPageCollection(this);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
        }
        /// <summary>
        /// Возвращает или устанавливает индекс активной вкладки
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }
            set
            {
                if (this.selectedIndex == value)
                    return;
                if (this.selectedIndex != -1 && this.tabPages.Count>this.selectedIndex)
                {
                    this.SelectedTab.Deselect();
                    this.SelectedTab.Visible = false;
                }
                this.selectedIndex = value;
                if (value != -1)
                {
                    this.SelectedTab.Header.State = TabPageState.Pushed;
                    this.SelectedTab.Visible = true;
                }
                if (this.SelectedIndexChanged != null)
                    this.SelectedIndexChanged(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Возвращает активную вкладку компонента
        /// </summary>
        public TabPage SelectedTab
        {
            get
            {
                return this.tabPages[this.SelectedIndex];
            }
        }

        #region overrides

        public override Rectangle DisplayRectangle
        {
            get
            {
                return new Rectangle(1, 5, base.DisplayRectangle.Width - 5, base.DisplayRectangle.Height - 10);
            }
        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (!(e.Control is TabPage))
            {
                return;
            }
            TabPage page = e.Control as TabPage;
            page.Header.Location = new Point(0, this.TabHeadersPanel.Controls.Count * page.Header.Height);
            //page.Header.Width = this.panel4.Width;
            page.Header.Dock = DockStyle.Top;
            page.OnSelect += new EventHandler(TabPage_OnSelect);
            this.TabHeadersPanel.Controls.Add(page.Header);
            //page.Location = new Point(this.TabHeadersPanel.Width + 1, 1);////////////
            if (!this.tabPages.Contains(page))
                this.tabPages.Add(page);
            if (this.tabPages.Count == 1)
                this.SelectedIndex = 0;
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (!(e.Control is TabPage))
            {
                return;
            }
            TabPage page = (TabPage)e.Control;
            if (this.tabPages.Contains(page))
                this.tabPages.Add(page);
            this.TabHeadersPanel.Controls.Remove(page);
            if (this.tabPages.Count > 0)
                this.SelectedIndex = 1;
            else
                this.SelectedIndex = -1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Отрисовка рамки комонента
            Pen pen = new Pen(Color.FromArgb(166, 161, 166));
            e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            // Отрисовка области перед верхней вкладкой
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(1,1,this.panel4.Width - 1, this.Height - 2), Color.FromArgb(255, 255, 246), Color.FromArgb(242, 236, 219), 0f);
            e.Graphics.FillRectangle(brush, new Rectangle(1, 1, this.panel4.Width - 1, this.Height - 2));
            // Отрисовка верхней границы самого верхнего компонента
            brush = new LinearGradientBrush(new Point(4, this.panel4.Top - 1), new Point(this.panel4.Width - 7, this.panel4.Top - 1), Color.FromArgb(255, 255, 250), Color.FromArgb(249, 246, 239));
            e.Graphics.DrawLine(new Pen(brush), 4, this.panel4.Top - 1, this.panel4.Width - 7, this.panel4.Top - 1);
        }
        #endregion

        private void TabPage_OnSelect(object sender, EventArgs e)
        {
            this.SelectedIndex = this.tabPages.IndexOf((TabPage)sender);
        }
        /// <summary>
        /// Возращает список вкладок компонента
        /// </summary>
        public TabPageCollection TabPages
        {
            get
            {
                return this.tabPages;
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            LinearGradientBrush brush = new LinearGradientBrush(this.panel4.Bounds, Color.FromArgb(255, 255, 246), Color.FromArgb(242, 236, 219), 0f);
            e.Graphics.FillRectangle(brush, new Rectangle(-1,0,this.panel4.Width+1,this.panel4.Height));
        }
    }

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    internal class TabControlDesigner : ParentControlDesigner
    {
        private DesignerVerb removeVerb;
        private DesignerVerbCollection verbs;
        private bool addingOnInitialize;

        public TabControlDesigner()
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (this.verbs == null)
                {
                    this.removeVerb = new DesignerVerb("Удалить вкладку", new EventHandler(this.OnRemove));
                    this.verbs = new DesignerVerbCollection();
                    this.verbs.Add(new DesignerVerb("Добавить вкладку", new EventHandler(this.OnAdd)));
                    this.verbs.Add(this.removeVerb);
                }
                if (this.Control != null)
                {
                    this.removeVerb.Enabled = this.Control.Controls.Count > 0;
                }
                return this.verbs;
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            base.AutoResizeHandles = true;
            TabControl control1 = component as TabControl;
            ISelectionService service1 = (ISelectionService)this.GetService(typeof(ISelectionService));
            
            if (service1 != null)
            {
                service1.SelectionChanged += new EventHandler(this.OnSelectionChanged);
            }
            if (control1 != null)
            {
                control1.SelectedIndexChanged += new EventHandler(OnTabSelectedIndexChanged);
            }
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            ISelectionService service1 = (ISelectionService)this.GetService(typeof(ISelectionService));
            if (service1 != null)
            {
                ICollection collection1 = service1.GetSelectedComponents();
                TabControl control1 = (TabControl)base.Component;
                foreach (object obj1 in collection1)
                {
                    TabPage page1 = TabControlDesigner.GetTabPageOfComponent(obj1);
                    if ((page1 != null) && (page1.Parent == control1))
                    {
                        control1.SelectedIndex = control1.TabPages.IndexOf(page1);
                        return;
                    }
                }
            }
        }

        internal static TabPage GetTabPageOfComponent(object comp)
        {
            if (!(comp is Control))
            {
                return null;
            }
            Control control1 = (Control)comp;
            while ((control1 != null) && !(control1 is TabPage))
            {
                control1 = control1.Parent;
            }
            return (TabPage)control1;
        }

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            try
            {
                this.addingOnInitialize = true;
                this.OnAdd(this, EventArgs.Empty);
                this.OnAdd(this, EventArgs.Empty);
            }
            finally
            {
                this.addingOnInitialize = false;
            }
            MemberDescriptor descriptor1 = TypeDescriptor.GetProperties(base.Component)["Controls"];
            base.RaiseComponentChanging(descriptor1);
            base.RaiseComponentChanged(descriptor1, null, null);
        }

        private void OnAdd(object sender, EventArgs eevent)
        {
            TabControl control1 = (TabControl)base.Component;
            IDesignerHost host1 = (IDesignerHost)this.GetService(typeof(IDesignerHost));
            if (host1 != null)
            {
                DesignerTransaction transaction1 = null;
                try
                {
                    try
                    {
                        transaction1 = host1.CreateTransaction("TabControlAddTab");
                    }
                    catch (CheckoutException exception1)
                    {
                        if (exception1 != CheckoutException.Canceled)
                        {
                            throw exception1;
                        }
                        return;
                    }
                    MemberDescriptor descriptor1 = TypeDescriptor.GetProperties(control1)["Controls"];
                    TabPage page1 = (TabPage)host1.CreateComponent(typeof(TabPage));
                    if (!this.addingOnInitialize)
                    {
                        base.RaiseComponentChanging(descriptor1);
                    }
                    string text1 = null;
                    PropertyDescriptor descriptor2 = TypeDescriptor.GetProperties(page1)["Name"];
                    if ((descriptor2 != null) && (descriptor2.PropertyType == typeof(string)))
                    {
                        text1 = (string)descriptor2.GetValue(page1);
                    }
                    if (text1 != null)
                    {
                        PropertyDescriptor descriptor3 = TypeDescriptor.GetProperties(page1)["Text"];
                        if (descriptor3 != null)
                        {
                            descriptor3.SetValue(page1, text1);
                        }
                    }
                    control1.Controls.Add(page1);
                    if (!this.addingOnInitialize)
                    {
                        base.RaiseComponentChanged(descriptor1, null, null);
                    }
                }
                finally
                {
                    if (transaction1 != null)
                    {
                        transaction1.Commit();
                    }
                }
            }
        }

        private void OnRemove(object sender, EventArgs eevent)
        {
            TabControl control1 = (TabControl)base.Component;
            if ((control1 != null) && (control1.TabPages.Count != 0))
            {
                MemberDescriptor descriptor1 = TypeDescriptor.GetProperties(base.Component)["TabPages"];
                TabPage page1 = control1.SelectedTab;
                IDesignerHost host1 = (IDesignerHost)this.GetService(typeof(IDesignerHost));
                if (host1 != null)
                {
                    DesignerTransaction transaction1 = null;
                    try
                    {
                        try
                        {
                            transaction1 = host1.CreateTransaction("TabControlRemoveTab");
                        }
                        catch (CheckoutException exception1)
                        {
                            if (exception1 != CheckoutException.Canceled)
                            {
                                throw exception1;
                            }
                            return;
                        }
                        control1.TabPages.Remove(page1);
                        host1.DestroyComponent(page1);
                    }
                    finally
                    {
                        if (transaction1 != null)
                        {
                            transaction1.Commit();
                        }
                    }
                }
            }
        }

        private void OnTabSelectedIndexChanged(object sender, EventArgs e)
        {
            ISelectionService service1 = (ISelectionService)this.GetService(typeof(ISelectionService));
            if (service1 != null)
            {
                ICollection collection1 = service1.GetSelectedComponents();
                TabControl control1 = (TabControl)base.Component;
                bool flag1 = false;
                foreach (object obj1 in collection1)
                {
                    TabPage page1 = TabControlDesigner.GetTabPageOfComponent(obj1);
                    if (((page1 != null) && (page1.Parent == control1)) && (page1 == control1.SelectedTab))
                    {
                        flag1 = true;
                        break;
                    }
                }
                if (!flag1)
                {
                    service1.SetSelectedComponents(new object[] { base.Component });
                }
            }
        }
    }

    internal class GradientPanel : Panel
    {
        public GradientPanel()
        {

        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                return new Rectangle(0,0, this.Width, this.Height);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
           
        }
    }

}
