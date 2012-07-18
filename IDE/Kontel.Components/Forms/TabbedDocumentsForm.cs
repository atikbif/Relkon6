using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TD.SandDock;
using Kontel.Relkon;
using System.Threading;

namespace Kontel.TabbedDocumentsForm
{
    public partial class TabbedDocumentsForm : Form
    {
        private ScanTimerToolStripItemsDelegate scanTimerToolStripItems = null; // ��������� ������ ��������� ������� ���������� � �������� ���� �� ������� �������

        /// <summary>
        /// ��������� ��� �������� ������ ��� ���������� ����������
        /// </summary>
        [Browsable(true)]
        public event EventHandler<ClosingDocumentsEventArgs> DocumentsClosing;
        
        public TabbedDocumentsForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ��������� ������ ��������� ������� ���������� � 
        /// �������� ���� �� ������� �������
        /// </summary>
        public ScanTimerToolStripItemsDelegate ScanTimerToolStripItems
        {
            get
            {
                return this.scanTimerToolStripItems;
            }
            set
            {
                this.scanTimerToolStripItems = value;
                this.WatchForChangesTimer.Enabled = (value != null);
            }
        }
        /// <summary>
        /// ����������� ������ ���� DockControl � ������� ���� TabbedDocument
        /// </summary>
        private TabbedDocument DockControlToTabbedDocument(DockControl control)
        {
            return control as TabbedDocument;
        }
        /// <summary>
        /// ���������� ������ �������� ����������
        /// </summary>
        public TabbedDocument[] Documents
        {
            get
            {
                return Array.ConvertAll<DockControl, TabbedDocument>(this.DocumentManager.GetDockControls(DockSituation.Document), new Converter<DockControl,TabbedDocument>(this.DockControlToTabbedDocument));
            }
        }
        /// <summary>
        /// ���������� �������� ��������
        /// </summary>
        public TabbedDocument ActiveDocument
        {
            get
            {
                return this.DocumentManager.ActiveTabbedDocument as TabbedDocument;
            }
        }
        /// <summary>
        /// ��������� ��� ���������, �� ����������� ����������
        /// </summary>
        internal void CloseAllExcept(TabbedDocument document)
        {
            List<TabbedDocument> ClosedDocuments = new List<TabbedDocument>();
            foreach (TabbedDocument doc in this.DocumentManager.GetDockControls(DockSituation.Document))
            {
                if (doc != document)
                    ClosedDocuments.Add(doc);
            }
            this.CloseDocuments(ClosedDocuments.ToArray());

        }

        /// <summary>
        /// �������� ������� ��������� ���������
        /// </summary>
        /// <returns>true, ���� ��������� ���� �������</returns>
        public bool CloseDocuments(TabbedDocument[] documents)
        {
            if (this.DocumentsClosing != null)
            {
                ClosingDocumentsEventArgs e = new ClosingDocumentsEventArgs(documents);
                this.DocumentsClosing(this, e);
                if (e.Cancel)
                    return false;
            }
            foreach (TabbedDocument doc in documents)
                doc.Close();
            return true;
        }
        /// <summary>
        /// ��������� ��������� ���������
        /// </summary>
        public void SaveDocuments(TabbedDocument[] documents)
        {
            List<IEditableTabbedDocument> docs = new List<IEditableTabbedDocument>();
            foreach (TabbedDocument doc in documents)
            {
                if (doc is IEditableTabbedDocument)
                    docs.Add((IEditableTabbedDocument)doc);
            }
            if (docs.Count == 0)
                return;
            while (this.SaveBackgroundWorker.IsBusy)
            {
                Application.DoEvents();
            }
            this.StartShowingAnimation(this.Images.Images[0]);
            this.SaveBackgroundWorker.RunWorkerAsync(docs.ToArray());
            this.InformationStatusLabel.Text = "����������...";
        }
        /// <summary>
        /// ��������� ��������� ��������
        /// </summary>
        public void SaveDocument(TabbedDocument document)
        {
            this.SaveDocuments(new TabbedDocument[] { document });
        }
        /// <summary>
        /// ��������� ����������� ��������� �������� �� ������ ���������
        /// </summary>
        /// <param name="AnimationImagesStrip">������ �����������, ���������� ��������</param>
        public void StartShowingAnimation(Image AnimationImagesStrip)
        {
            ImageList il = new ImageList();
            il.ColorDepth = ColorDepth.Depth24Bit;
            il.Images.AddStrip(AnimationImagesStrip);
            il.Tag = 0;
            this.AnimationTimer.Tag = il;
            this.AnimationTimer.Start();
        }
        /// <summary>
        /// ������������� ����������� �������� �� ������ ���������
        /// </summary>
        public void StopShowingAnimation()
        {
            this.AnimationTimer.Stop();
            this.AnimationStatusLabel.Image = null;
        }
        /// <summary>
        /// ����������������� ���������� ��������� �� ���������� ����������
        /// </summary>
        public void WaitWhileSaving()
        {
            while (this.SaveBackgroundWorker.IsBusy)
            {
                Application.DoEvents();
            }
        }
        /// <summary>
        /// ��������� �� ����� ��������� ��������
        /// </summary>
        public void OpenDocument(TabbedDocument document)
        {
            document.Manager = this.DocumentManager;
            document.Open();
            document.Invalidate();
        }

        private void DocumentManager_DockControlClosing(object sender, TD.SandDock.DockControlClosingEventArgs e)
        {
            if (e.DockControl is TabbedDocument && this.DocumentsClosing != null)
            {
                ClosingDocumentsEventArgs ce = new ClosingDocumentsEventArgs(new TabbedDocument[] { (TabbedDocument)e.DockControl });
                this.DocumentsClosing(this, ce);
                e.Cancel = ce.Cancel;
            }
        }

        private void WatchForChangesTimer_Tick(object sender, EventArgs e)
        {
            this.WatchForChangesTimer.Stop();
            this.scanTimerToolStripItems();
            this.WatchForChangesTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            this.AnimationTimer.Stop();
            if (this.AnimationTimer.Tag == null)
                return;
            ImageList AnimationImages = this.AnimationTimer.Tag as ImageList;
            int i = (int)(AnimationImages.Tag);
            this.AnimationStatusLabel.Image = AnimationImages.Images[i];
            AnimationImages.Tag = (++i < AnimationImages.Images.Count) ? i : 0;
            this.AnimationTimer.Start();
        }

        private void SaveBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if(e.Argument == null)
                return;
            IEditableTabbedDocument[] Documents = (IEditableTabbedDocument[])e.Argument;
            for (int i = 0; i < Documents.Length; i++)
            {
                try
                {
                    this.Invoke(new ThreadStart(Documents[i].Save));
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage("SaveBackgroundWorker_DoWork: " + ex.Message);
                }
            }
        }

        private void SaveBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.StopShowingAnimation();
            this.InformationStatusLabel.Text = "";
        }

        private void DocumentManager_ShowControlContextMenu(object sender, ShowControlContextMenuEventArgs e)
        {
            if (e.DockControl.DockSituation == DockSituation.Document)
                this.TabbedDocumentContextMenu.Show(e.DockControl, e.Position);
        }

        private void TabbedDocumentContextMenu_Opening(object sender, CancelEventArgs e)
        {
            this.miCloseAllButThis.Enabled = this.DocumentManager.GetDockControls(DockSituation.Document).Length > 1;
            this.miNewHorizontalTabGroup.Enabled = this.miNewVerticalTabGroup.Enabled = this.ActiveDocument.LayoutSystem.Controls.Count > 1;
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            if(this.ActiveDocument is IEditableTabbedDocument)
                ((IEditableTabbedDocument)this.ActiveDocument).Save();
        }

        private void miClose_Click(object sender, EventArgs e)
        {
            this.ActiveDocument.Close();
        }

        private void miNewHorizontalTabGroup_Click(object sender, EventArgs e)
        {
            this.DocumentManager.ActiveTabbedDocument.Split(DockSide.Top);
        }

        private void miNewVerticalTabGroup_Click(object sender, EventArgs e)
        {
            this.DocumentManager.ActiveTabbedDocument.Split(DockSide.Left);
        }

        private void miCloseAllButThis_Click(object sender, EventArgs e)
        {
            this.CloseAllExcept(this.ActiveDocument);
        }
    }

    public delegate void ScanTimerToolStripItemsDelegate();

    public class ClosingDocumentsEventArgs : EventArgs
    {
        private TabbedDocument[] documents; // ����������� ���������
        private bool cancel = false; // true, ���� �������� ���� ��������

        /// <summary>
        /// ����������� ���������
        /// </summary>
        public TabbedDocument[] Documents
        {
            get
            {
                return this.documents;
            }
        }
        /// <summary>
        /// true, ���� ���� �������� ����������
        /// </summary>
        public bool Cancel
        {
            get
            {
                return this.cancel;
            }
            set
            {
                this.cancel = value;
            }
        }

        public ClosingDocumentsEventArgs(TabbedDocument[] documents)
        {
            this.documents = documents;
        }
    }
}