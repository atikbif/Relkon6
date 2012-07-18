using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kontel.Relkon.Classes;
using Kontel.Relkon;

namespace Kontel.Relkon.Solutions
{
    /// <summary>
    /// —одержит параметры компил€ции дл€ проекта контроллера
    /// </summary>
    public sealed class CompilationParams
    {
        private List<string> compilationCreatedFilesNames = new List<string>(); // список файлов, созданных в результате компил€ции
        private List<string> postcompileMessages = new List<string>(); // список информационных сообщений по результатам компил€ции
        private List<CompilationError> errors = new List<CompilationError>(); // список ошибок и предупреждений, вы€вленых после компил€ции
        internal StreamWriter CompilationErrorsWriter; // поток записи ошибок компил€ции
        private bool errorsFileNotCreated; // если true, то не удалось создать файл ошибок
        private bool emulationMode; // флаг режима эмул€ции
        private bool emulationMode2; // флаг режима эмул€ции входов
        private string sdkDirectory; // каталог SDK
        private string compilerDirectory;
        internal string CompilationErrrosFilePath; // им€ файла ошибок компил€ции начина€ от каталога SDK
        internal bool WaitForCompilationErrors; // если true, то в программу из стандартного потока должны идти сообщени€ об ошибках компил€ции
        /// <summary>
        /// ¬озвращает список файлов, созданных в результате компил€ции
        /// </summary>
        public List<string> CompilationCreatedFilesNames
        {
            get
            {
                return this.compilationCreatedFilesNames;
            }
        }
        /// <summary>
        /// ¬озвращает список информационных сообщений по результатам компил€ции
        /// </summary>
        public List<string> PostcompileMessages
        {
            get
            {
                return this.postcompileMessages;
            }
        }
        /// <summary>
        /// ¬озвращает список ошибок и предупреждений, вы€вленых в результате компил€ции
        /// </summary>
        public List<CompilationError> Errors
        {
            get
            {
                return this.errors;
            }
        }
        /// <summary>
        /// ≈сли true, то не удалось создать файл ошибок
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
        /// ¬озвращает или устанавливает флаг режима эмул€ции
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
        /// ¬озвращает или устанавливает флаг режима эмул€ции входов
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
        /// ѕоказывает, были ли ошибки либо предупреждени€ при компил€ции
        /// </summary>
        public bool HasErrorsOrWarnings
        {
            get
            {
                return this.errors.Count > 0;
            }
        }
        /// <summary>
        /// ѕоказывает, были ли ошибки при компил€ции
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
        /// ¬озвращает или устанавливает каталог SDK компил€тора
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
        /// ¬озвращает им€ файла ошибок компил€ции
        /// </summary>
        public string CompilationErrorsFileName
        {
            get
            {
                return this.sdkDirectory + "\\" + this.CompilationErrrosFilePath;
            }
        }
        /// <summary>
        /// —оздает потоки записи ошибок компил€ции и линковки
        /// </summary>
        internal void CreateErrorWriter()
        {
            this.CompilationErrorsWriter = new StreamWriter(this.CompilationErrorsFileName, false, Encoding.GetEncoding(866));
        }
        /// <summary>
        /// «акрыает потоки записи ошибок компил€ции и линковки
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
