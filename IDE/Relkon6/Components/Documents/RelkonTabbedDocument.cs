using System;
using System.Collections.Generic;
using System.Text;
using TD.SandDock;
using Kontel.Relkon.Solutions;

namespace Kontel.Relkon.Components.Documents
{
    /// <summary>
    /// ������� ����� ��� ���� ���������� Relkon
    /// </summary>
    public class RelkonTabbedDocument: UserTabbedDocument
    {
        private ControllerProgramSolution solution; // ������, � �������� ��������� ��������; ���� �������� �� ���������� �� ������ �������, �� null 
        private bool documentModified = false; // ����������, ��� �� �������� ������� (����� �� ��������� '*' � ����� TabText
        protected bool initialized = false; // ����������, ����������� �� ������������� ����������

        public RelkonTabbedDocument(ControllerProgramSolution solution)
        {
            this.solution = solution;
        }

        public RelkonTabbedDocument()
        {

        }
        /// <summary>
        /// ���������� ������, ��������� � ����������
        /// </summary>
        public ControllerProgramSolution Solution
        {
            get
            {
                return this.solution;
            }
            protected set
            {
                this.solution = value;
            }

        }
        /// <summary>
        /// ���������� ��� ������������� ���� ����, ��� �������� ��� �������
        /// </summary>
        protected bool DocumentModified
        {
            get
            {
                return this.documentModified;
            }
            set
            {
                if (this.documentModified != value)
                {
                    this.documentModified = value;
                    base.TabText = this.TabText;
                }
            }
        }

        public override string TabText
        {
            get
            {
                return this.ProtectedTabText + (this.documentModified ? "*" : "");
            }
            set
            {

            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RelkonTabbedDocument
            // 
            this.Name = "RelkonTabbedDocument";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ResumeLayout(false);

        }

        #region Virtual methods
        /// <summary>
        /// ���������� ���������� ��������� ��������� (��� '*' � �����) 
        /// </summary>
        protected virtual string ProtectedTabText
        {
            get
            {
                if (!DesignMode)
                    throw new Exception("�������� RelkonTabbedDocument.ProtectedTabText ����� ���� ����������� � �������");
                return "";
            }
        }
        #endregion
    }
}
