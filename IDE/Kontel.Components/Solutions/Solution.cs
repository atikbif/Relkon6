using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using Kontel.Relkon.Forms;
using System.Threading;
using Kontel;
using System.ComponentModel;

namespace Kontel.Relkon
{
    #region RelkonDataReceivedEventHandler
    public delegate void RelkonDataReceivedEventHandler(object sender, RelkonDataReceivedEventArgs e);

    public class RelkonDataReceivedEventArgs : EventArgs
    {
        private string data;
        /// <summary>
        /// ���������� ������
        /// </summary>
        public string Data
        {
            get
            {
                return this.data;
            }
        }

        public RelkonDataReceivedEventArgs(string Data)
        {
            this.data = Data;
        }
    }
    #endregion
}

namespace Kontel.Relkon.Solutions
{
    /// <summary>
    /// ������� ����� �������� �������
    /// </summary>
    public abstract class Solution
    {
        private List<string> files = new List<string>(); // ����� �������
        private List<string> openedFiles = new List<string>(); // �������� �����
        private bool isNewSolution = false; // ����������, �������� �� ������ ������ ��� ���������
        private string version = ""; // ������ Relkon, � ������� ������ ������
        private string activeFileName = ""; // ���������� ��� ��������� ����� ������� �� ������ �������� ����������
        private Guid solutionID = Guid.NewGuid(); // ���������� ������������� �������
        protected List<string> notRemovedExtensios; // ���������� ������, ������� ������ ���������� ��� ����������� �������
        protected string fileName = ""; // ��� ����� �������
        protected STM32F107UploadMgr uploadMgr = null;

        /// <summary>
        /// ����������� ��������� � �������� ������� ������ ������� � ����������
        /// </summary>
        public event UploadMgrProgressChangedEventHandler UploadingToDeviceProgressChanged;
        /// <summary>
        /// ������������ �� ���������� �������� ������ ������� � ����������
        /// </summary>
        public event AsyncCompletedEventHandler UploadingToDeviceCompleted;

        /// <summary>
        /// ������ Relkon, � ������� ������ ������
        /// </summary>
        [XmlAttribute]
        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ���������� ������������� �������
        /// </summary>
        [XmlAttribute]
        public Guid ID
        {
            get
            {
                return this.solutionID;
            }
            set
            {
                this.solutionID = value;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ��� ��������� ����� �������
        /// �� ������ �������� ����������
        /// </summary>
        public string ActiveFileName
        {
            get
            {
                return this.activeFileName;
            }
            set
            {
                this.activeFileName = value;
            }
        }
        /// <summary>
        /// ����������, �������� �� ������ ������ ��� ���������
        /// </summary>
        public bool IsNewSolution
        {
            get
            {
                return this.isNewSolution;
            }
            set
            {
                this.isNewSolution = value;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ������� �������
        /// </summary>
        public string DirectoryName
        {
            get
            {
                string res = "";
                try
                {
                    res = Path.GetDirectoryName(this.fileName);
                }
                catch
                {
                    res = "";
                }
                return res;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ��� ����� �������
        /// </summary>
        [XmlIgnore]
        public string SolutionFileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }
        /// <summary>
        /// ���������� ���������� ����� �������
        /// </summary>
        public abstract string Extension
        {
            get;
        }
        /// <summary>
        /// ���������� ������ Filter ��� ����������� OpenFileDialog � SaveFileDialog
        /// </summary>
        public abstract string FileDialogFilter
        {
            get;
        }
        /// <summary>
        /// ���������� ��� ������������� ��� �������
        /// </summary>
        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.fileName);
            }
        }
        /// <summary>
        /// ���������� ������ ������ �������
        /// </summary>
        public List<string> Files
        {
            get
            {
                return this.files;
            }
        }
        /// <summary>
        /// ���������� ������ ������ �������
        /// </summary>
        public List<string> OpenedFiles
        {
            get
            {
                return this.openedFiles;
            }
        }
        /// <summary>
        /// ��������� ������ � ��������� ����
        /// </summary>
        public abstract void SaveAs(string FileName);
        /// <summary>
        /// ��������� ������ � ��� ������� ����
        /// </summary>
        public void Save()
        {
            this.SaveAs(this.fileName);
        }
        /// <summary>
        /// �������� ��� ������ �� ������, �������� � ������ �� �����
        /// </summary>
        /// <param name="OldFileName">��� �����, ������� ��������� ��������</param>
        /// <param name="NewFileName">����� ��� �����</param>
        public abstract void ChangeCustomFileName(string OldFileName, string NewFileName);
        /// <summary>
        /// ��������� �� ���������� ��� ����� ��� �������
        /// </summary>
        public static bool IsValidIdentifier(string Name)
        {
            return (Name.IndexOfAny(new char[] { '/', '?', ':', '&', '\\', '*', '"', '<', '>', '|', '#', '%'}) == -1 && Name != "CON" &&
                    Name != "AUX" && Name != "PRN" && Name != "COM1" && Name != "LPT2" && Name != "." && Name != ".." && Name != "");
        }
        /// <summary>
        /// �������������� ������ (� ��� ����� � ���� �������, ������ ���� ���������)
        /// </summary>
        public void Rename(string NewSolutionName)
        {
            if (!IsValidIdentifier(NewSolutionName))
            {
                throw new Exception("�������������� � ����� ������ �� �����:\r\n" +
                                    "- ��������� ����� �� ��������� ��������: / ? : & \\ * \" < > | # %\r\n" +
                                    "- ��������� ����������� ������� Unicode\r\n" +
                                    "- ���� ������������������ ���������� �������, �������� 'CON', 'AUX', 'PRN','COM1', LPT2 � �.�.\r\n" +
                                    "- ���� '.' ��� '..' ��� ������\r\n\r\n" +
                                    "���������� ������� ���������� ���");
            }
            string s = this.SolutionFileName;
            this.SaveAs(this.DirectoryName + "\\" + NewSolutionName + this.Extension);
            if (s != this.SolutionFileName)
                File.Delete(s);
        }
        /// <summary>
        /// ���������� ������, ������� �������� ��������� ����
        /// </summary>
        public virtual Solution GetSolutionThatContainsFile(string FileName)
        {
            return (this.files.Contains(FileName) ? this : null);
        }
        /// <summary>
        /// ������������� ����������, ��������� ���������� �������� �������
        /// </summary>
        public abstract void UploadToDevice();

        public abstract void UploadToDevice(bool onlyProgram, bool onlyParams, bool readEmbVars);
    

        /// <summary>
        /// ������������� ������� ���������������� �����������
        /// </summary>
        public void StopUploading()
        {
            this.uploadMgr.StopUploading();
        }
        /// <summary>
        /// ����������, ���������� �� � ������ ������ ������� ��������
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.uploadMgr.IsBusy;
            }
        }

        protected void mgr_UploadingCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (this.UploadingToDeviceCompleted != null)
                this.UploadingToDeviceCompleted(this, e);
        }

        protected void mgr_ProgressChanged(object sender, UploadMgrProgressChangedEventArgs e)
        {
            if (this.UploadingToDeviceProgressChanged != null)
                this.UploadingToDeviceProgressChanged(this, e);
        }
    }
}
