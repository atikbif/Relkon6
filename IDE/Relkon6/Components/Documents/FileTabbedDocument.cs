using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Forms;
using System.IO;
using System.Security.Cryptography;
using Kontel.Relkon.Solutions;
using Kontel.TabbedDocumentsForm;

namespace Kontel.Relkon.Components.Documents
{
    /// <summary>
    /// ������� ����� ��� ����������, ���������� ������� ����������� �� ������
    /// </summary>
    public class FileTabbedDocument: RelkonTabbedDocument
    {
        private string fileName = ""; // ��� �����, ������������ � ���������
        protected bool saveRequired = true; // ���� false, �� �������� �� ����� ��������
        protected string fileHash = ""; // ������ ����������� ���������

        public FileTabbedDocument(ControllerProgramSolution Solution, string FileName)
            : base(Solution)
        {
            this.FileName = FileName;
        }

        public FileTabbedDocument()
        {

        }

        protected override string ProtectedTabText
        {
            get
            {
                return Path.GetFileName(this.fileName);
            }
        }
        /// <summary>
        /// ���������� ��� �����, ������������ � ���������
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
                this.ToolTipText = this.fileName;
            }
        }
        /// <summary>
        /// ��������� hash-������� �������� ����������� ���������
        /// </summary>
        private string GetDocumentHash()
        {
            string s = Path.GetTempFileName();
            this.PerformSaving(s);
            string res = this.GetFileHash(s);
            File.Delete(s);
            return res;
        }
        /// <summary>
        /// ��������� hash-������� ���������� �����
        /// </summary>
        protected string GetFileHash(string FileName)
        {
            SHA1 sh = SHA1.Create();
            return Encoding.Default.GetString(sh.ComputeHash(File.ReadAllBytes(FileName)));
        }

        #region Virtual methods
        /// <summary>
        /// ���������� ���������, � ������� ����������� ���������� ���������;
        /// ����������� ������ ���� �������� � ������� ������ FileTabbedDocument
        /// </summary>
        protected virtual Encoding FileEncoding
        {
            get
            {
                throw new Exception("property FileTabbedDocument.FileEncoding must be overloaded in child classes");
            }
        }
        /// <summary>
        /// ���������� �������� �� ���������� �����; ����������� ������ ���� �������� 
        /// � ������� ������ FileTabbedDocument
        /// </summary>
        protected virtual void LoadFromFile(string FileName)
        {
            throw new Exception("method FileTabbedDocument.LoadFromFile must be overloaded in child classes");
        }
        /// <summary>
        /// ��������� ���������� ��������� � ���� � ��������� ������;
        /// ����������� ������ ���� �������� � ������� ������ FileTabbedDocument
        /// </summary>
        protected virtual void PerformSaving(string FileName)
        {
            if(!DesignMode)
                throw new Exception("method FileTabbedDocument.PerformSaving must be overloaded in child classes");
        }
        #endregion

        #region IEditableTabbedDocument Members

        public bool SaveRequired
        {
            get 
            {
                try
                {
                    return (this.saveRequired && this.fileHash != this.GetDocumentHash());
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                this.saveRequired = value;
            }
        }

        public void Save()
        {
            this.Save(this.fileName);
            this.DocumentModified = false;
        }

        private void Save(string FileName)
        {
            this.PerformSaving(FileName);
            this.FileName = FileName;
            this.fileHash = this.GetFileHash(FileName);
        }

        public void SaveAs(String FileName)
        {
            this.PerformSaving(FileName);
        }

        #endregion
    }
}
