using System;
using System.Collections.Generic;
using System.Text;
using TD.SandDock;
using Kontel.Relkon.Solutions;

namespace Kontel.Relkon.Components.Documents
{
    /// <summary>
    /// Базовый класс для всех документов Relkon
    /// </summary>
    public class RelkonTabbedDocument: UserTabbedDocument
    {
        private ControllerProgramSolution solution; // проект, к которому относится документ; если документ не принадлежт ни одному проекту, то null 
        private bool documentModified = false; // показывает, был ли документ изменен (нужно ли добавлять '*' в конце TabText
        protected bool initialized = false; // показывает, закончилась ли инициализация докуменита

        public RelkonTabbedDocument(ControllerProgramSolution solution)
        {
            this.solution = solution;
        }

        public RelkonTabbedDocument()
        {

        }
        /// <summary>
        /// Возвращает проект, связанный с документом
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
        /// Возвращает или устанавливает флаг того, что документ был изменен
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
        /// Возвращает собственно заголовок документа (без '*' в конце) 
        /// </summary>
        protected virtual string ProtectedTabText
        {
            get
            {
                if (!DesignMode)
                    throw new Exception("Свойство RelkonTabbedDocument.ProtectedTabText долно быть перегружено в потомке");
                return "";
            }
        }
        #endregion
    }
}
