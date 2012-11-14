using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Solutions;
using TD.SandDock;
using Kontel.TabbedDocumentsForm;

namespace Kontel.Relkon.Components.Documents
{
    /// <summary>
    /// Базовый класс для документов, отображающих свойства различных проектов
    /// </summary>
    public class PropertiesTabbedDocument: RelkonTabbedDocument, IEditableTabbedDocument
    {
        private bool successSave = false; // показывает, было ли сохранение успешным

        public PropertiesTabbedDocument(ControllerProgramSolution Solution)
            : base(Solution)
        {
            this.Padding = new System.Windows.Forms.Padding(5);
        }

        protected override string ProtectedTabText
        {
            get
            {
                if (this.Solution != null)
                    return Solution.Name;
                else
                    return "";
            }
        }

        public PropertiesTabbedDocument()
        {

        }

        protected override void OnClosing(DockControlClosingEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = !this.LoadToSolution();
            if (!e.Cancel)
                this.Solution.Save();
        }
        /// <summary>
        /// Показывает, было ли сохранение настроек успешным
        /// </summary>
        public bool SuccessSave
        {
            get
            {
                return this.successSave;
            }
        }

        #region Virtual methods
        /// <summary>
        /// Загружает данные в документ из проекта документа;
        /// обязательно должен быть перекрыт в потомке класса PropertiesTabbedDocument
        /// </summary>
        protected virtual void LoadFromSolution()
        {
            throw new Exception("method PropertiesTabbedDocument.LoadFromSolution must be overloaded in child classes");
        }
        /// <summary>
        /// Обновляет заначения переменных заводских установок;
        /// обязательно должен быть перекрыт в потомке класса PropertiesTabbedDocument
        /// </summary>
        public virtual void UpdateEmbeddedVarsValues()
        {
            throw new Exception("method PropertiesTabbedDocument.UpdateEmbeddedVarsValues must be overloaded in child classes");
        }
        /// <summary>
        /// Выполняет сохранение данных документа; обязательно должен быть перекрыт 
        /// в потомке класса PropertiesTabbedDocument
        /// </summary>
        protected virtual bool LoadToSolution()
        {
            throw new Exception("method PropertiesTabbedDocument.LoadToSolution must be overloaded in child classes");
        }
        /// <summary>
        /// Перезагружает данные, отображаемые документом
        /// </summary>
        public virtual void Reload()
        {
            this.LoadFromSolution();
        }
        #endregion

        #region IEditableTabbedDocument Members

        public bool SaveRequired
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public bool CanUndo
        {
            get 
            {
                return false;
            }
        }

        public bool CanRedo
        {
            get
            {
                return false;
            }
        }

        public bool CanPaste
        {
            get 
            {
                return false;
            }
        }

        public void Save()
        {
            if (this.successSave = this.LoadToSolution())
            {
                this.Solution.Save();
                this.DocumentModified = false;
            }
        }

        public void Save(string FileName)
        {
            
        }

        public void Undo()
        {
            
        }

        public void Redo()
        {
            
        }

        public void Cut()
        {
        }

        public void Copy()
        {
            
        }

        public void Paste()
        {
            
        }

        public void Delete()
        {
            
        }

        public void SelectAll()
        {
            
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PropertiesTabbedDocument
            // 
            this.Name = "PropertiesTabbedDocument";
            this.ResumeLayout(false);

        }

        #region IEditableTabbedDocument Members


        public void SaveAs(string FileName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
