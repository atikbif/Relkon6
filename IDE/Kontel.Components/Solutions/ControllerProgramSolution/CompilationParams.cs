using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kontel.Relkon.Classes;
using Kontel.Relkon;

namespace Kontel.Relkon.Solutions
{
    /// <summary>
    /// �������� ��������� ���������� ��� ������� �����������
    /// </summary>
    public sealed class CompilationParams
    {
        private List<string> compilationCreatedFilesNames = new List<string>(); // ������ ������, ��������� � ���������� ����������
        private List<string> postcompileMessages = new List<string>(); // ������ �������������� ��������� �� ����������� ����������
        private List<CompilationError> errors = new List<CompilationError>(); // ������ ������ � ��������������, ��������� ����� ����������
        internal StreamWriter CompilationErrorsWriter; // ����� ������ ������ ����������
        private bool errorsFileNotCreated; // ���� true, �� �� ������� ������� ���� ������
        private bool emulationMode; // ���� ������ ��������
        private bool emulationMode2; // ���� ������ �������� ������
        private string sdkDirectory; // ������� SDK
        private string compilerDirectory;
        internal string CompilationErrrosFilePath; // ��� ����� ������ ���������� ������� �� �������� SDK
        internal bool WaitForCompilationErrors; // ���� true, �� � ��������� �� ������������ ������ ������ ���� ��������� �� ������� ����������
        /// <summary>
        /// ���������� ������ ������, ��������� � ���������� ����������
        /// </summary>
        public List<string> CompilationCreatedFilesNames
        {
            get
            {
                return this.compilationCreatedFilesNames;
            }
        }
        /// <summary>
        /// ���������� ������ �������������� ��������� �� ����������� ����������
        /// </summary>
        public List<string> PostcompileMessages
        {
            get
            {
                return this.postcompileMessages;
            }
        }
        /// <summary>
        /// ���������� ������ ������ � ��������������, ��������� � ���������� ����������
        /// </summary>
        public List<CompilationError> Errors
        {
            get
            {
                return this.errors;
            }
        }
        /// <summary>
        /// ���� true, �� �� ������� ������� ���� ������
        /// </summary>
        public bool ErrorsFileNotCreated
        {
            get
            {
                return this.errorsFileNotCreated;
            }
            internal set
            {
                this.errorsFileNotCreated = value;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ���� ������ ��������
        /// </summary>
        public bool EmulationMode
        {
            get
            {
                return this.emulationMode;
            }
            set
            {
                this.emulationMode = value;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ���� ������ �������� ������
        /// </summary>
        public bool EmulationMode2
        {
            get
            {
                return this.emulationMode2;
            }
            set
            {
                this.emulationMode2 = value;
            }
        }
        /// <summary>
        /// ����������, ���� �� ������ ���� �������������� ��� ����������
        /// </summary>
        public bool HasErrorsOrWarnings
        {
            get
            {
                return this.errors.Count > 0;
            }
        }
        /// <summary>
        /// ����������, ���� �� ������ ��� ����������
        /// </summary>
        public bool HasErrors
        {
            get
            {
                foreach (CompilationError error in this.errors)
                {
                    if (!error.Warning)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ������� SDK �����������
        /// </summary>
        public string SDKDirectory
        {
            get
            {
                return this.sdkDirectory;
            }
            set
            {
                this.sdkDirectory = value;
            }
        }

        public string CompilerDirectory
        {
            get
            {
                return this.compilerDirectory;
            }
            set
            {
                this.compilerDirectory = value;
            }
        }
        

        /// <summary>
        /// ���������� ��� ����� ������ ����������
        /// </summary>
        public string CompilationErrorsFileName
        {
            get
            {
                return this.sdkDirectory + "\\" + this.CompilationErrrosFilePath;
            }
        }
        /// <summary>
        /// ������� ������ ������ ������ ���������� � ��������
        /// </summary>
        internal void CreateErrorWriter()
        {
            this.CompilationErrorsWriter = new StreamWriter(this.CompilationErrorsFileName, false, Encoding.GetEncoding(866));
        }
        /// <summary>
        /// �������� ������ ������ ������ ���������� � ��������
        /// </summary>
        internal void CloseErrorWriter()
        {
            if (!this.errorsFileNotCreated)
            {
                this.CompilationErrorsWriter.Close();
            }
        }
    }
}
