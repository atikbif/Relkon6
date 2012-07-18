using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using Kontel.Relkon;

namespace Kontel.Relkon
{
    /// <summary>
    /// TreeView, поддерживающий отображение индексов узлов в списке,
    /// выделение узла левой кнопкой мыши и двойную буфферизацию
    /// </summary>
    public sealed class TreeViewEx : TreeView
    {
        private bool showSubNodesNumbers = false; // показывает, требуетс€ ли отображать индексы дочерних узлов 
        private bool flag = false; // устанавливаетс€ в true после срабатывани€ OnKeyPress
        private TreeNode sn = null; // инициализируетс€ SelectedNode после срабатывани€ OnKeyPress
        private List<TreeNode> nonCheckedNodes = new List<TreeNode>(); // узлы, у которых не должны отображатьс€ checkbox'ы при установке свойства CheckBoxes=true
        private bool snDrawed = false; // используетс€ чтобы прорисовка выбранного узла при смене текста осуществл€лась только 1 раз
        internal bool NodeTextChanging = false; // устанавливаетс€ в true перед сменой текста какого-либо узла

        public TreeViewEx()
        {
            this.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.AdvancedTreeView_DrawNode);
        }

        /// <summary>
        /// ѕоказывает, требуетс€ ли отображать индексы дочерних узлов 
        /// </summary>
        public bool ShowSubNodesNumbers
        {
            get
            {
                return this.showSubNodesNumbers;
            }
            set
            {
                this.showSubNodesNumbers = value;
                base.DrawMode = this.showSubNodesNumbers ? TreeViewDrawMode.OwnerDrawText : TreeViewDrawMode.Normal;
            }
        }
        /// <summary>
        /// ¬озвращает список узлов, у которых не должны отображатьс€ checkbox'ы при установке свойства CheckBoxes=true
        /// </summary>
        public List<TreeNode> NonCheckedNodes
        {
            get
            {
                return this.nonCheckedNodes;
            }
        }
        /// <summary>
        /// ¬ычисл€ет границы указанного узла
        /// </summary>
        private Rectangle GetNodeBounds(TreeNode node)
        {
            Rectangle bounds = node.Bounds;
            Graphics g = this.CreateGraphics();
            int IndexWidth = (int)g.MeasureString(node.Index.ToString() + ". " + node.Text, this.Font).Width;
            bounds = new Rectangle(node.Bounds.Location, new Size(Math.Max(IndexWidth, node.Bounds.Width) + 2, node.Bounds.Height));
            bounds.Offset(-2, 0);
            return bounds;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
        }
        /// <summary>
        /// ¬озвращает индекс узла без учета неактивных узлов
        /// </summary>
        /// <param name="parent">–одительский узел</param>
        /// <param name="node">”зел, дл€ которого вычисл€етс€ индекс</param>
        private int GetNodeIndexWODisabledNodes(TreeNode parent, TreeNode node)
        {
            int i = 0;
            foreach (TreeNode n in parent.Nodes)
            {
                if (n == node)
                    break;
                if ((n is TreeNode && (!(n is TreeNodeEx))) || (n is TreeNodeEx && ((TreeNodeEx)n).Enabled))
                    i++;
            }
            return i;
        }
        /// <summary>
        /// ќтрисовывает TreeView в соответствии с концепцией DoubleBuffered
        /// </summary>
        private void DrawDoubleBufferedTreeView(ref Message m)
        {
            using (Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
            {
                Win32.PAINTSTRUCT ps;
                IntPtr dc = Win32.BeginPaint(Handle, out ps);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    m.WParam = g.GetHdc();
                    base.WndProc(ref m);
                    g.ReleaseHdc();
                }
                using (Graphics g = Graphics.FromHdc(dc))
                    g.DrawImage(bmp, 0, 0);
                Win32.EndPaint(Handle, ref ps);
            }
        }
        /// <summary>
        /// ѕровер€ет, содержит ли сгобщение флаг CUSTOMDRAW и если
        /// да, то обрабатывает его
        /// </summary>
        private void CheckAndProceedCustomDrawMsg(ref Message m)
        {
            // ќбработка сообщени€ NMTVCUSTOMDRAW, а именно - не осуществл€етс€ отрисовка checkbox'ов у
            // узлов, наход€щихс€ в nonCheckedNodes
            Win32.NMHDR nmHeader = (Win32.NMHDR)m.GetLParam(typeof(Win32.NMHDR));
            if (nmHeader.code == (int)Win32.NM_CUSTOMDRAW)
            {
                Win32.NMTVCUSTOMDRAW tvDraw = (Win32.NMTVCUSTOMDRAW)m.GetLParam(typeof(Win32.NMTVCUSTOMDRAW));
                if ((int)Win32.CDDS.CDDS_PREPAINT == tvDraw.nmcd.dwDrawStage)
                {
                    m.Result = new IntPtr((int)Win32.CDRF.CDRF_NOTIFYITEMDRAW);
                }
                else if ((int)Win32.CDDS.CDDS_ITEMPREPAINT == tvDraw.nmcd.dwDrawStage)
                {
                    TreeViewCustomDraw(ref m, tvDraw);
                }
            }
        }
        /// <summary>
        /// ќсуществл€ет Custom Draw дл€ TreeView применително к checkbox
        /// </summary>
        private void TreeViewCustomDraw(ref Message msg, Win32.NMTVCUSTOMDRAW tvDraw)
        {
            try
            {
                int hTreeNode = (int)tvDraw.nmcd.dwItemSpec;
                if (hTreeNode == 0)
                {
                    msg.Result = new IntPtr((int)Win32.CDRF.CDRF_DODEFAULT);
                    return;
                }
                TreeNode curNode = TreeNode.FromHandle(this, new IntPtr(hTreeNode));
                if (curNode == null)
                {
                    msg.Result = new IntPtr((int)Win32.CDRF.CDRF_DODEFAULT);
                    return;
                }
                TreeView tree = curNode.TreeView;
                if (tree != null)
                {
                    if (this.nonCheckedNodes.Contains(curNode))
                    {
                        UnckeckNode(curNode.Handle.ToInt32());
                    }
                }
            }
            finally
            {
                msg.Result = new IntPtr((int)Win32.CDRF.CDRF_DODEFAULT);
            }
        }
        /// <summary>
        /// —нимает флаг прорисовки checkbox'а у узла с заданным дескриптором
        /// </summary>
        private void UnckeckNode(int hItem)
        {
            if (hItem > 0)
            {
                Win32.TVITEM tvi = new Win32.TVITEM();
                tvi.mask = (uint)Win32.TVIF.TVIF_HANDLE | (uint)Win32.TVIF.TVIF_STATE;
                tvi.hItem = new IntPtr(hItem);
                tvi.stateMask = (uint)Win32.TVIS.TVIS_STATEIMAGEMASK;
                tvi.state = 0;

                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(tvi));

                try
                {
                    Marshal.StructureToPtr(tvi, ptr, false);
                    Message msg = Message.Create(this.Handle, (int)Win32.TVM_SETITEMA, IntPtr.Zero, ptr);
                    DefWndProc(ref msg);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || (e.Button == MouseButtons.Left && !this.LabelEdit))
            {
                TreeNode node = this.GetNodeAt(e.Location);
                if (node != null && this.SelectedNode != node)
                {
                    this.SelectedNode = node;
                    base.OnAfterSelect(new TreeViewEventArgs(this.SelectedNode, TreeViewAction.ByMouse)); // если это убрать, то в AfterSelect будет TreeViewAction.Unknown
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                flag = true;
                sn = this.SelectedNode;
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            flag = true;
            sn = this.SelectedNode;
            base.OnKeyPress(e);
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (!flag)
                base.OnAfterSelect(e);
            else
            {
                this.SelectedNode = sn;
                flag = false;
            }
        }
       
        private void AdvancedTreeView_DrawNode(object sender, System.Windows.Forms.DrawTreeNodeEventArgs e)
        {
            if (this.NodeTextChanging)
            {
                if (e.Node != this.SelectedNode || this.snDrawed)
                    return;
                this.snDrawed = true;
            }
            else
                this.snDrawed = false;
            Color TextColor = this.ForeColor;
            Rectangle NodeBounds = this.GetNodeBounds(e.Node);
            if ((e.State & TreeNodeStates.Selected) != 0 || e.Node == this.SelectedNode)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, NodeBounds);
                TextColor = SystemColors.HighlightText;
                using (Pen FocusPen = new Pen(Color.Black))
                {
                    FocusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    NodeBounds.Size = new Size(NodeBounds.Width - 1, NodeBounds.Height - 1);
                    e.Graphics.DrawRectangle(FocusPen, NodeBounds);
                }
            }
            else if (e.Node.Parent != null && !((TreeNodeEx)e.Node).Enabled)
            {
                TextColor = Color.Gray;
            }
            string index_string = "";
            if (e.Node.Parent != null)
                index_string = ((TreeNodeEx)e.Node).Enabled ? this.GetNodeIndexWODisabledNodes(e.Node.Parent, e.Node) + ". " : "    ";
            e.Graphics.DrawString(index_string + e.Node.Text, this.Font, new SolidBrush(TextColor),e.Bounds.Location);
        }

        protected override void WndProc(ref Message m)
        {
            if (!DesignMode)
            {
                switch (m.Msg)
                {
                    case Win32.WM_ERASEBKGND:
                        m.Result = IntPtr.Zero;
                        return;
                    case Win32.WM_PAINT:
                        this.DrawDoubleBufferedTreeView(ref m);
                        return;
                    case Win32.WM_USER + 7246:
                        this.CheckAndProceedCustomDrawMsg(ref m);
                        break;
                }
            }
            base.WndProc(ref m);
        }
        /// <summary>
        /// ѕредоставл€ет доступ к необходимым константам и функци€м WinAPI
        /// </summary>
        private class Win32
        {
            #region Methods

            [DllImport("User32.dll")]
            public static extern bool SendMessage(IntPtr hWnd, int msg, int wParam, ref TVITEM tvItem);

            [DllImport("user32.dll")]
            public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

            [DllImport("user32.dll")]
            public static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);

            #endregion

            #region Structs

            [StructLayout(LayoutKind.Sequential)]
            public struct NMHDR
            {
                public IntPtr hwndFrom;
                public int idFrom;
                public int code;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct PAINTSTRUCT
            {
                public IntPtr hdc;
                public bool fErase;
                public RECT rcPaint;
                public bool fRestore;
                public bool fIncUpdate;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
                public byte[] rgbReserved;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
                public RECT(int left_, int top_, int right_, int bottom_)
                {
                    Left = left_;
                    Top = top_;
                    Right = right_;
                    Bottom = bottom_;
                }
                public int Height { get { return Bottom - Top; } }
                public int Width { get { return Right - Left; } }
                public Size Size { get { return new Size(Width, Height); } }
                public Point Location { get { return new Point(Left, Top); } }
                // Handy method for converting to a System.Drawing.Rectangle
                public Rectangle ToRectangle()
                {
                    return Rectangle.FromLTRB(Left, Top, Right, Bottom);
                }
                public static RECT FromRectangle(Rectangle rectangle)
                {
                    return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
                }
                public override int GetHashCode()
                {
                    return Left ^ ((Top << 13) | (Top >> 0x13))
                      ^ ((Width << 0x1a) | (Width >> 6))
                      ^ ((Height << 7) | (Height >> 0x19));
                }
                #region Operator overloads
                public static implicit operator Rectangle(RECT rect)
                {
                    return rect.ToRectangle();
                }
                public static implicit operator RECT(Rectangle rect)
                {
                    return FromRectangle(rect);
                }
                #endregion
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct NMCUSTOMDRAW
            {
                public NMHDR hdr;
                public int dwDrawStage;
                public IntPtr hdc;
                public RECT rc;
                public int dwItemSpec;
                public int uItemState;
                public int lItemlParam;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct NMTVCUSTOMDRAW
            {
                public NMCUSTOMDRAW nmcd;
                public uint clrText;
                public uint clrTextBk;
                public int iSubItem;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
            public struct TVITEM
            {
                public uint mask;
                public IntPtr hItem;
                public uint state;
                public uint stateMask;
                public IntPtr pszText;
                public int cchTextMax;
                public int iImage;
                public int iSelectedImage;
                public int cChildren;
                public IntPtr lParam;
            }

            #endregion

            #region Messages & Enums

            // Windows messages
            public const int WM_NOTIFY       = 0x004E;
            public const int WM_USER         = 0x0400;
            public const int WM_ERASEBKGND   = 0x0014;
            public const int WM_PAINT        = 0x000F;
            //TreeView messages
            public const int TV_FIRST        = 0x1100;
            public const int TVM_DELETEITEM  = (TV_FIRST + 1);
            public const int TVM_INSERTITEMA = (TV_FIRST + 0);
            public const int TVM_INSERTITEMW = (TV_FIRST + 50);

            public const int TVM_SETITEMA    = (TV_FIRST + 13);
            public const int TVM_SETITEMW    = (TV_FIRST + 63);
            // Notification messages
            public const int NM_FIRST        = (0 - 0);
            public const int NM_CUSTOMDRAW   = (NM_FIRST - 12);
            /// <summary>
            /// Custom draw draw state
            /// </summary>
            public enum CDDS : int
            {
                CDDS_PREPAINT = 0x00000001,
                CDDS_POSTPAINT = 0x00000002,
                CDDS_PREERASE = 0x00000003,
                CDDS_POSTERASE = 0x00000004,
                CDDS_ITEM = 0x00010000,
                CDDS_ITEMPREPAINT = (CDDS_ITEM | CDDS_PREPAINT),
                CDDS_ITEMPOSTPAINT = (CDDS_ITEM | CDDS_POSTPAINT),
                CDDS_ITEMPREERASE = (CDDS_ITEM | CDDS_PREERASE),
                CDDS_ITEMPOSTERASE = (CDDS_ITEM | CDDS_POSTERASE),
                CDDS_SUBITEM = 0x00020000
            }
            /// <summary>
            /// Custom draw return flags
            /// </summary>
            public enum CDRF : int
            {
                CDRF_DODEFAULT = 0x00000000,
                CDRF_NEWFONT = 0x00000002,
                CDRF_SKIPDEFAULT = 0x00000004,
                CDRF_NOTIFYPOSTPAINT = 0x00000010,
                CDRF_NOTIFYITEMDRAW = 0x00000020
            }

            public enum TVIF : int
            {
                TVIF_TEXT = 0x0001,
                TVIF_IMAGE = 0x0002,
                TVIF_PARAM = 0x0004,
                TVIF_STATE = 0x0008,
                TVIF_HANDLE = 0x0010,
                TVIF_SELECTEDIMAGE = 0x0020,
                TVIF_CHILDREN = 0x0040,
                TVIF_INTEGRAL = 0x0080
            }

            public enum TVIS : int
            {
                TVIS_STATEIMAGEMASK = 0xF000
            }

            #endregion
        }
    }

    public class TreeNodeEx : TreeNode
    {
        private bool enabled = true; // активен или неактивен узел
        
        public new string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    TreeViewEx treeView = this.TreeView as TreeViewEx;
                    if (treeView != null)
                        treeView.NodeTextChanging = true;
                    base.Text = value;
                    if (treeView != null)
                        treeView.NodeTextChanging = false;
                }
            }
        }
        /// <summary>
        /// ¬озвращает или устанавливает флаг активности узла
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }
    }
}
