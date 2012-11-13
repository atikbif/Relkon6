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
        /// Полученная строка
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
    /// Базовый класс проектов системы
    /// </summary>
    public abstract class Solution
    {
        private List<string> files = new List<string>(); // файлы проекта
        private List<string> openedFiles = new List<string>(); // открытые файлы
        private bool isNewSolution = false; // показывает, является ли проект только что созданным
        private string version = ""; // версия Relkon, в которой создан проект
        private string activeFileName = ""; // возвращает имя активного файла проекта на момент закрытия последнего
        private Guid solutionID = Guid.NewGuid(); // уникальный идентификатор проекта
        protected List<string> notRemovedExtensios; // расширения файлов, которые нельзя перемещать при перемещении проекта
        protected string fileName = ""; // имя файла проекта
        protected STM32F107UploadMgr uploadMgr = null;

        /// <summary>
        /// Периодичски возникает в процессе загузки данных проекта в контроллер
        /// </summary>
        public event UploadMgrProgressChangedEventHandler UploadingToDeviceProgressChanged;
        /// <summary>
        /// Генерируется по завершении загрузки данных проекта в контроллер
        /// </summary>
        public event AsyncCompletedEventHandler UploadingToDeviceCompleted;

        /// <summary>
        /// Версия Relkon, в которой создан проект
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
        /// Возвращает или устанавливает уникальный идентификатор проекта
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
        /// Возвращает или устанавливает имя активного файла проекта
        /// на момент закрытия последнего
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
        /// Показывает, является ли проект только что созданным
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
        /// Возвращает или устанавливает каталог проекта
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
        /// Возвращает или устанавливает имя файла проекта
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
        /// Возвращает расширение файла проекта
        /// </summary>
        public abstract string Extension
        {
            get;
        }
        /// <summary>
        /// Возвращает строку Filter для компонентов OpenFileDialog и SaveFileDialog
        /// </summary>
        public abstract string FileDialogFilter
        {
            get;
        }
        /// <summary>
        /// Возвращает или устанавливает имя проекта
        /// </summary>
        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.fileName);
            }
        }
        /// <summary>
        /// Возвращает список файлов проекта
        /// </summary>
        public List<string> Files
        {
            get
            {
                return this.files;
            }
        }
        /// <summary>
        /// Возвращает список файлов проекта
        /// </summary>
        public List<string> OpenedFiles
        {
            get
            {
                return this.openedFiles;
            }
        }
        /// <summary>
        /// Сохраняет проект в указанный файл
        /// </summary>
        public abstract void SaveAs(string FileName);
        /// <summary>
        /// Сохраняет проект в его текущий файл
        /// </summary>
        public void Save()
        {
            this.SaveAs(this.fileName);
        }
        /// <summary>
        /// Заменяет имя одного из файлов, входящих в проект на новое
        /// </summary>
        /// <param name="OldFileName">Имя файла, которое требуется изменить</param>
        /// <param name="NewFileName">Новое имя файла</param>
        public abstract void ChangeCustomFileName(string OldFileName, string NewFileName);
        /// <summary>
        /// Проверяет на валидность имя файла или проекта
        /// </summary>
        public static bool IsValidIdentifier(string Name)
        {
            return (Name.IndexOfAny(new char[] { '/', '?', ':', '&', '\\', '*', '"', '<', '>', '|', '#', '%'}) == -1 && Name != "CON" &&
                    Name != "AUX" && Name != "PRN" && Name != "COM1" && Name != "LPT2" && Name != "." && Name != ".." && Name != "");
        }
        /// <summary>
        /// Преименовывает проект (в том числе и файл проекта, старый файл удаляется)
        /// </summary>
        public void Rename(string NewSolutionName)
        {
            if (!IsValidIdentifier(NewSolutionName))
            {
                throw new Exception("Идентификаторы и имена файлов не могут:\r\n" +
                                    "- содержать любые из следующих символов: / ? : & \\ * \" < > | # %\r\n" +
                                    "- содержать управляющие символы Unicode\r\n" +
                                    "- быть зарезервированными системными именами, например 'CON', 'AUX', 'PRN','COM1', LPT2 и т.д.\r\n" +
                                    "- быть '.' или '..' или пустым\r\n\r\n" +
                                    "Пожалуйста введите правильное имя");
            }
            string s = this.SolutionFileName;
            this.SaveAs(this.DirectoryName + "\\" + NewSolutionName + this.Extension);
            if (s != this.SolutionFileName)
                File.Delete(s);
        }
        /// <summary>
        /// Возвращает проект, который содержит указанный файл
        /// </summary>
        public virtual Solution GetSolutionThatContainsFile(string FileName)
        {
            return (this.files.Contains(FileName) ? this : null);
        }
        /// <summary>
        /// Программирует контроллер, используя внутренние средства релкона
        /// </summary>
        public abstract void UploadToDevice();

        public abstract void UploadToDevice(bool onlyProgram, bool onlyParams, bool readEmbVars);
    

        /// <summary>
        /// Останавливает процесс программирования контроллера
        /// </summary>
        public void StopUploading()
        {
            this.uploadMgr.StopUploading();
        }
        /// <summary>
        /// Показывает, происходит ли в данный момент процесс загрузки
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
