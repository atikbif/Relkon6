using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Kontel.Relkon;
using System.Xml.Serialization;
using Kontel.Relkon.Classes;
using Kontel.Relkon.CodeDom;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using Kontel;
using System.ComponentModel;
using System.Diagnostics;


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
    /// Проект программы на контроллер
    /// </summary>
    [XmlInclude(typeof(STM32F107Solution))]
    public abstract class ControllerProgramSolution
    {
        public sealed class Uart
        {
            [XmlAttribute]
            public ProtocolType Protocol = ProtocolType.RC51BIN; // протокол передачи данных
            [XmlAttribute]
            public int BaudRate = 19200; // скорость передачи данных  

            public Uart() { }
        }

        private static string CurrentVersion = "6.0.0"; // версия структуры проекта
        private string programFileName = ""; // имя файла программы
        private string pultFileName = ""; // имя файла пультов
        //private string fbdFileName = ""; // имя файла блок-диаграмм
        private string debugerFileName = ""; // имя файла отладчика
        private CompilationParams compilationParams = new CompilationParams(); // параметры компиляции
        private PultParams pultParams = new PultParams(); // параметры пультов
        private ProcessorParams processorParams = new ProcessorParams(); // параметры процесора
        private ControllerVarCollection vars = new ControllerVarCollection(); // все переменные проекта
        internal RelkonCodeModel codeModel; // ОО представление кода программы
        private List<string> searchList = new List<string>(); // список строк для поиска
        private List<string> replaceList = new List<string>(); // список строк для замены

        protected Uart[] uarts = new Uart[2]; // настройки портов процессора 
        private byte[] ipAdress = { 0, 0, 0, 0 };
        private byte[] ipMask = { 255, 255, 255, 255 };
        private byte[] ipGateway = { 0, 0, 0, 0 };
        private string macAdress = "000000000000";
        private bool pultEnable = true;
        private bool _SDEnable = false;

        private string label = "";
        
        

        // Настройки контроллера
        private int controllerAddress = 1; // сетевой адрес
        private int searchedControllerAddress = 0; // при программировании будет осуществлятся поиск контроллера с этим адресом
        /// <summary>
        /// Возникает при добавлении строки в выходной поток данных
        /// </summary>
        public virtual event RelkonDataReceivedEventHandler OutputDataReceived;

        private List<string> files = new List<string>(); // файлы проекта
        private List<string> openedFiles = new List<string>(); // открытые файлы
        private bool isNewSolution = false; // показывает, является ли проект только что созданным
        private string version = ""; // версия Relkon, в которой создан проект
        private string activeFileName = ""; // возвращает имя активного файла проекта на момент закрытия последнего
        private Guid solutionID = Guid.NewGuid(); // уникальный идентификатор проекта
        protected List<string> notRemovedExtensios; // расширения файлов, которые нельзя перемещать при перемещении проекта
        protected string fileName = ""; // имя файла проекта
        protected UploadMgr uploadMgr = null;

        /// <summary>
        /// Периодичски возникает в процессе загузки данных проекта в контроллер
        /// </summary>
        public event UploadMgrProgressChangedEventHandler UploadingToDeviceProgressChanged;
        /// <summary>
        /// Генерируется по завершении загрузки данных проекта в контроллер
        /// </summary>
        public event AsyncCompletedEventHandler UploadingToDeviceCompleted;



        [XmlIgnore]
        public static string SDKDirectory = ""; // каталог SDK процессора
        [XmlIgnore]
        public static string CompilerDirectory = "";

        public Uart[] Uarts
        {
            get
            {
                return this.uarts;
            }
            set
            {
                this.uarts = value;
            }
        }

        public byte[] ControllerIPAdress
        {
            get
            {
                return this.ipAdress;
            }
            set
            {
                this.ipAdress = value;
            }
        }

        public byte[] ControllerIPMask
        {
            get
            {
                return this.ipMask;
            }
            set
            {
                this.ipMask = value;
            }
        }

        public byte[] ControllerIPGateway
        {
            get
            {
                return this.ipGateway;
            }
            set
            {
                this.ipGateway = value;
            }
        }

        public string ControllerMACAdress
        {
            get
            {
                return this.macAdress;
            }
            set
            {
                this.macAdress = value;
            }
        }

        public string Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }

        public bool PultEnable
        {
            get
            {
                return this.pultEnable;
            }
            set
            {
                this.pultEnable = value;
            }
        }

        public bool SDEnable
        {
            get
            {
                return _SDEnable;
            }
            set
            {
                _SDEnable = value;
            }
        }


        private SerialPort485 _lastWorkedPort = null;

        [XmlIgnore]
        public SerialPort485 LastWorkedPort
        {
            get
            {
                return _lastWorkedPort;
            }
            set
            {
                _lastWorkedPort = value;
            }
        }

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
        /// Список процессов проекта
        /// </summary>
        public List<ProjectProcess> Processes { get; set; }
        /// <summary>
        /// Возвращает процесс по его названию
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public ProjectProcess GetProcessByName(string Name)
        {
            for (int i = 0; i < this.Processes.Count; i++)
                if (this.Processes[i].Name == Name) return this.Processes[i];
            return null;
        }
        /// <summary>
        /// Возвращает или устанавливает сетевой адрес контроллера
        /// </summary>
        [XmlAttribute]
        public int ControllerAddress
        {
            get
            {
                return this.controllerAddress;
            }
            set
            {
                this.controllerAddress = value;
            }
        }
        /// <summary>
        /// При программировании будет осуществлятся поиск контроллера с этим адресом
        /// </summary>
        [XmlAttribute]
        public int SearchedControllerAddress
        {
            get
            {
                return this.searchedControllerAddress;
            }
            set
            {
                this.searchedControllerAddress = value;
            }
        }
        /// <summary>
        /// Возвращает список переменных проекта
        /// </summary>
        public ControllerVarCollection Vars
        {
            get
            {
                return this.vars;
            }
        }
        /// <summary>
        /// Возвращает список для поиска
        /// </summary>
        public List<string> SearchList
        {
            get
            {
                return this.searchList;
            }
        }
        /// <summary>
        /// Возвращает список для замены
        /// </summary>
        public List<string> ReplaceList
        {
            get
            {
                return this.replaceList;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает имя файла программы
        /// </summary>
        public string ProgramFileName
        {
            get
            {
                return this.programFileName;
            }
            set
            {
                this.programFileName = value;
            }
        }
        /// <summary>
        /// Возвращает или устанавливает имя файла пультов
        /// </summary>
        public string PultFileName
        {
            get
            {
                return this.pultFileName;
            }
            set
            {
                if (value != this.pultFileName)
                {
                    this.pultFileName = value;
                }
            }
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
     
        /// <summary>
        /// Возвращает или устанавливает имя файла настроек отладчика
        /// </summary>
        public string DebuggerFileName
        {
            get
            {
                return this.debugerFileName;
            }
            set
            {
                if (value != this.pultFileName)
                {
                    this.debugerFileName = value;
                }
            }
        }
        /// <summary>
        /// Возвращает набор параметров компиляции проекта
        /// </summary>
        [XmlIgnore]
        public CompilationParams CompilationParams
        {
            get
            {
                return this.compilationParams;
            }
        }
        /// <summary>
        /// Возвращает набор параметров пультов проекта
        /// </summary>
        [XmlIgnore]
        public PultParams PultParams
        {
            get
            {
                return this.pultParams;
            }
        }
        /// <summary>
        /// Возвращает набор параметров процессора проекта
        /// </summary>
        [XmlIgnore]
        public ProcessorParams ProcessorParams
        {
            get
            {
                return this.processorParams;
            }
        }
        
        public string Extension
        {
            get 
            {
                return ".rp6";
            }
        }

        public string FileDialogFilter
        {
            get 
            {
                return "Проекты Relkon (.rp6)|*.rp6";
            }
        }


        public ControllerProgramSolution()
        {
            this.IntitalizeParams();
            this.Processes = new List<ProjectProcess>();
            this.CreateNotRemovedExtensionsList();
            this.Version = ControllerProgramSolution.CurrentVersion;

            this.uploadMgr = new UploadMgr(this);
            this.uploadMgr.ProgressChanged += new UploadMgrProgressChangedEventHandler(mgr_ProgressChanged);
            this.uploadMgr.UploadingCompleted += new AsyncCompletedEventHandler(mgr_UploadingCompleted);
        }

        /// <summary>
        /// Создает список расширений файлов, которые не должны перемещаться при пересохранении проекта в другой папке
        /// </summary>
        private void CreateNotRemovedExtensionsList()
        {
            this.notRemovedExtensios = new List<string>();
            this.notRemovedExtensios.Add(".epj");
        }
        
        /// <summary>
        /// Загружает параметры контроллера из другого проекта
        /// </summary>
        /// <param name="solution"></param>
        public void LoadControllerParamsFromAnotherSolution(ControllerProgramSolution solution)
        {
            if (!(solution is STM32F107Solution))
                throw new Exception("Проект должнен быть типа STM32F107Solution");
            STM32F107Solution sln = solution as STM32F107Solution;
            for (int i = 0; i < this.uarts.Length; i++)
            {
                this.uarts[i].BaudRate = sln.uarts[i].BaudRate;
                this.uarts[i].Protocol = sln.uarts[i].Protocol;
            }
            this.ControllerAddress = sln.ControllerAddress;
            this.SearchedControllerAddress = sln.SearchedControllerAddress;
            this.ipAdress = sln.ControllerIPAdress;
        }
        /// <summary>
        /// Создает точную копию экземпляра класса
        /// </summary>
        /// <returns></returns>
        protected abstract ControllerProgramSolution Clone();
        /// <summary>
        /// Инициализирует параметры процессора, компиляции
        /// </summary>
        protected void IntitalizeParams()
        {
            this.ProcessorParams.InverseByteOrder = true;
            this.ProcessorParams.Type = ProcessorType.STM32F107;
            this.CompilationParams.SDKDirectory = STM32F107Solution.SDKDirectory;
            this.CompilationParams.CompilerDirectory = STM32F107Solution.CompilerDirectory;

            this.CompilationParams.CompilationErrrosFilePath = "fc_u.err";
            //this.CompilationParams.LinkingErrorsFilePath = "\\link.err";
            this.PultParams.AvailablePultTypes = new Kontel.Relkon.Solutions.PultType[] { PultType.Pult4x20, PultType.Pult2x12 };
            this.PultParams.DefaultPultType = PultType.Pult4x20;
            this.PultParams.MaxVarMaskDigitsCountAfterComma = 7;
            this.PultParams.MaxVarMaskLength = 15;
            for (int i = 0; i < this.uarts.Length; i++)
                this.uarts[i] = new Uart();
        }
        /// <summary>
        /// Возвращает размер переменной указанного типа (в байтах)
        /// </summary>
        /// <param name="Type">Имя типа (char, int и т.д.)</param>
        protected int TypeSize(string Type)
        {
            switch (Type)
            {
                case "char":
                    return 1;
                case "short":
                    return 2;
                case "int":
                    return 4;
                case "long":
                    return 4;
                case "float":
                    return 4;
                case "double":
                    return 8;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Заполняет список информационных сообщений по результатам компиляции
        /// </summary>
        protected void CreatePostcompileMessages()
        {
            // Добавлние периодов опроса модулей ВВ
            if (this.codeModel.IOModules.Count > 0)
            {
                List<int> addresses = new List<int>();
                foreach (int address in this.codeModel.IOModules.Keys)
                {
                    addresses.Add(address);
                }
                addresses.Sort();
                this.CompilationParams.PostcompileMessages.Add("Периоды опроса внешних модулей ввода-вывода:");
                foreach (int address in addresses)
                {
                    IOModule module = (IOModule)this.codeModel.IOModules[address];
                    string ts = "";
                    foreach (ModuleVarDescription description in module.VarNames)
                    {
                        ts += description.DisplayName + ",";
                    }
                    this.CompilationParams.PostcompileMessages.Add(ts.Remove(ts.Length - 1) + ": " + module.RealPeriod + " мс");
                }
            }
        }
        /// <summary>
        /// Проверяет, является ли маска вывода переменной 
        /// валидной для данного типа процесора
        /// </summary>
        public bool IsValidPultVarMask(string Mask)
        {
            return Regex.IsMatch(Mask, @"^\d{1,7}([,\.]\d{1,7})?$");
        }
        /// <summary>
        /// Компилирует проект
        /// </summary>
        public void Compile()
        {
            try
            {
                this.PrepareToCompile();

                STM32F107CodeGenerator codeGenerator = new STM32F107CodeGenerator(this);
                this.TranslateProgram(codeGenerator);
                if (this.CompilationParams.HasErrors)
                    return;

                this.CompilationParams.CompilationCreatedFilesNames.Add(this.DirectoryName + "\\fc_u.c");
                this.CompileProgram();
                if (this.CompilationParams.HasErrors)
                    return;

                this.LoadVarsAddressesFromFlashMap(this.Vars);
            }
            catch (Exception ex)
            {
                this.CompilationParams.Errors.Add(new CompilationError(ex.Message, "", -1, false));
            }
        }

        /// <summary>
        /// Создает командный файл для компиляции программы
        /// </summary>
        private void CreateCompilerCommandFile()
        {
            string s = File.ReadAllText(this.CompilationParams.SDKDirectory + "\\Compile.bat.pattern", Encoding.Default);
            File.WriteAllText(this.CompilationParams.SDKDirectory + "\\Compile.bat", String.Format(s, this.CompilationParams.SDKDirectory, this.DirectoryName, this.Name, this.CompilationParams.CompilerDirectory), Encoding.GetEncoding(866));
        }

        /// <summary>
        /// Запускает компилятор
        /// </summary>
        private void RunCompiler()
        {
            Process p1 = new Process();
            p1.StartInfo.FileName = this.CompilationParams.SDKDirectory + "\\Compile.bat";
            p1.StartInfo.WorkingDirectory = this.CompilationParams.CompilerDirectory;
            p1.StartInfo.RedirectStandardOutput = true;
            p1.StartInfo.RedirectStandardError = true;
            p1.StartInfo.CreateNoWindow = true;
            p1.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
            p1.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(866);
            p1.StartInfo.RedirectStandardError = true;
            p1.StartInfo.UseShellExecute = false;
            p1.ErrorDataReceived += new DataReceivedEventHandler(Compiler_ErrorDataReceived);
            p1.OutputDataReceived += new DataReceivedEventHandler(Compiler_OutputDataReceived);
            p1.Start();
            p1.BeginOutputReadLine();
            p1.BeginErrorReadLine();
            p1.WaitForExit();
        }


        /// <summary>
        /// Получение данных от компиляора в процессе компиляци программы
        /// </summary>
        private void Compiler_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                if ((!this.CompilationParams.WaitForCompilationErrors) || this.CompilationParams.ErrorsFileNotCreated)
                {
                    if (e.Data.Contains("arm-none-eabi-gcc.exe"))
                        this.CompilationParams.WaitForCompilationErrors = true;
                    this.RaisedOutputDataReceivedEvent(e.Data);
                }
                else if (this.CompilationParams.WaitForCompilationErrors)
                {
                    if (e.Data.Contains("arm-none-eabi-objcopy.exe"))
                    {
                        this.CompilationParams.WaitForCompilationErrors = false;
                        this.RaisedOutputDataReceivedEvent(e.Data);
                    }
                }
            }
        }

        private void Compiler_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                this.WriteErrorMessageToErrorStream(this.CompilationParams.CompilationErrorsWriter, e.Data);
        }


        private void TranslateProgram(STM32F107CodeGenerator CodeGenerator)
        {
            CodeGenerator.GenerateProgramCode();
            CodeGenerator.GeneratePultCode();
            this.CompilationParams.Errors.AddRange(CodeGenerator.Errors);
            if (!CodeGenerator.HasErrors)
                this.RaisedOutputDataReceivedEvent(this.ProgramFileName + ", " + this.PultFileName + " -> " + this.DirectoryName + "\\fc_u.c, " + this.DirectoryName + "\\modules.h");
        }

        /// <summary>
        /// Компилирует программу
        /// </summary>
        private void CompileProgram()
        {
            try
            {
                this.CompilationParams.CreateErrorWriter();
            }
            catch (Exception ex)
            {
                this.RaisedOutputDataReceivedEvent("Ошибка создания потоков для записи ошибок: " + Utils.FirstLetterToLower(ex.Message));
            }
            this.CreateCompilerCommandFile();
            this.RunCompiler();
            this.CompilationParams.CloseErrorWriter();
            if (File.Exists(this.CompilationParams.CompilationErrorsFileName))
                this.CreateErrorsList();
            if (this.CompilationParams.HasErrors)
                return;
            if (!File.Exists(this.DirectoryName + "\\" + this.Name + ".bin"))
                this.CompilationParams.Errors.Add(new CompilationError("Файл " + this.Name + ".bin не создан", "", -1, false));
            if (File.Exists(this.DirectoryName + "\\Flash.map"))
            {
                this.CreatePostcompileMessages();
            }
        }  
       
        /// <summary>
        /// Заполняет адреса переменных из файла Flash.map
        /// </summary>
        public void LoadVarsAddressesFromFlashMap(Kontel.Relkon.Classes.ControllerVarCollection Vars)
        {
            string map = File.ReadAllText(this.DirectoryName + "\\Flash.map", Encoding.Default);

            // Установка адресов переменных ввода-вывода (в том числе и внешних одулей ВВ)

            Match m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys_IN");
            if (m.Success)
            {
                string group = m.Groups[1].Value;
                int adress = Convert.ToInt32(group.Substring(4, 4), 16);
                for (int i = 0; i < 4; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("IN" + i);
                    ControllerIOVar var2 = Vars.IOVars.GetVarByName("DIN" + i);
                    if (var != null && var2 != null)
                    {
                        var.Address = adress;
                        var2.Address = adress;
                        adress += var.Size;
                    }
                }

                for (int i = 4; i < 6; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("DIN" + i);
                    if (var != null)
                    {
                        var.Address = adress;
                        adress += var.Size;
                    }
                }

            }

            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys_OUT");
            if (m.Success)
            {
                int adress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);
                for (int i = 0; i < 4; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("OUT" + i);
                    if (var != null)
                    {
                        var.Address = adress;
                        adress += var.Size;
                    }
                }

                for (int i = 4; i < 6; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("DOUT" + i);
                    if (var != null)
                    {
                        var.Address = adress;
                        adress += var.Size;
                    }
                }
            }

            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys_ADC");
            if (m.Success)
            {
                int adress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);

                for (int i = 1; i < 9; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("ADH" + i);
                    if (var != null)
                    {
                        var.Address = adress;
                        var = Vars.IOVars.GetVarByName("ADC" + i);
                        var.Address = adress;
                        adress += var.Size;
                    }
                }
            }

            int groupSize = 32 * 4;

            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_ADC\b");
            int adcStructureAdress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);

            foreach (ControllerIOVar var in Vars.IOVars)
            {
                if (var.Name.Contains("ADC") && var.ExternalModule)
                {
                    int index = 0;
                    if (int.TryParse(var.Name.Substring(3), out index))
                    {
                        int p = index - 9;
                        int k = (int)(p / 4);
                        int groupNum = p - k * 4;
                        int adress = groupNum * groupSize + k * 4 + adcStructureAdress;
                        var.Address = adress;
                        ControllerIOVar v = Vars.IOVars.GetVarByName("ADH" + index);
                        if (v != null)
                            v.Address = adress;
                    }
                }
            }

            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_DAC");
            int dacStructureAdress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);

            foreach (ControllerIOVar var in Vars.IOVars)
            {
                if (var.Name.Contains("DAC") && var.ExternalModule)
                {
                    int index = 0;
                    if (int.TryParse(var.Name.Substring(3), out index))
                    {
                        int p = index - 5;
                        int k = (int)(p / 2);
                        int groupNum = p - (k * 2);
                        int adress = groupNum * groupSize + k * 4 + dacStructureAdress;
                        var.Address = adress;
                        ControllerIOVar v = Vars.IOVars.GetVarByName("DAH" + index);
                        if (v != null)
                            v.Address = adress;
                    }
                }
            }


            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys_DAC");
            if (m.Success)
            {
                int adress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);
                for (int i = 1; i < 5; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("DAH" + i);
                    if (var != null)
                    {
                        var.Address = adress;
                        var = Vars.IOVars.GetVarByName("DAC" + i);
                        var.Address = adress;
                        adress += var.Size;
                    }
                }
            }

            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+IN\b");
            if (m.Success)
            {
                int adress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);

                foreach (ControllerIOVar var in Vars.IOVars)
                {
                    if (var.Name.Contains("IN") && var.ExternalModule)
                    {

                        m = Regex.Match(var.Name, @"IN(\d+)");
                        if (m.Groups[1].Success)
                            var.Address = (int.Parse(m.Groups[1].Value) - 4) + adress;
                    }
                }
            }


            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+OUT\b");
            if (m.Success)
            {
                int adress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);

                foreach (ControllerIOVar var in Vars.IOVars)
                {
                    if (var.Name.Contains("OUT") && var.ExternalModule)
                    {

                        m = Regex.Match(var.Name, @"OUT(\d+)");
                        if (m.Groups[1].Success)
                            var.Address = (int.Parse(m.Groups[1].Value) - 4) + adress;
                    }
                }
            }

            foreach (ControllerUserVar var in Vars.UserVars)
            {
                m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+" + var.Name + "\\b");
                if (m.Success)
                    var.Address = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);
                else
                    this.CompilationParams.Errors.Add(new CompilationError("Не удалость установить адрес переменной " + var.Name, this.ProgramFileName, -1, true));
            }

            Vars.SystemVars.Clear();
            Vars.SystemVars.AddRange(this.GetSystemVarsList());

            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+times");
            int adr = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);
            ControllerSystemVar iov = Vars.SystemVars.GetVarByName("SEC");
            iov.Address = adr;
            adr += iov.Size;
            iov = Vars.SystemVars.GetVarByName("MIN");
            iov.Address = adr;
            adr += iov.Size;
            iov = Vars.SystemVars.GetVarByName("HOUR");
            iov.Address = adr;
            adr += iov.Size;
            iov = Vars.SystemVars.GetVarByName("DATE");
            iov.Address = adr;
            adr += iov.Size;
            iov = Vars.SystemVars.GetVarByName("MONTH");
            iov.Address = adr;
            adr += iov.Size;
            iov = Vars.SystemVars.GetVarByName("YEAR");
            iov.Address = adr;


            ControllerSystemVar z = Vars.SystemVars.GetVarByName("Z40");
            if (z != null)
            {
                m = Regex.Match(map, z.SystemName + @"\s+0x([0-9a-fA-F]{8})\s+");                
                if (m.Groups[1].Success)
                {
                    adr = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);
                    z.Address = adr;
                    z = Vars.SystemVars.GetVarByName("led");
                    z.Address = adr;
                }

                z = Vars.SystemVars.GetVarByName("Z50");
                m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+" + z.SystemName + "\\b");
                if (m.Groups[1].Success)
                {
                    adr = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16); ;
                    z.Address = adr;
                    z = Vars.SystemVars.GetVarByName("key");
                    z.Address = adr;
                }


                m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys\b");
                if (m.Groups[1].Success)
                {
                    adr = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16) + 1024 + 5;
                    z = Vars.SystemVars.GetVarByName("Z30");
                    z.Address = adr;
                    z = Vars.SystemVars.GetVarByName("st1");
                    z.Address = adr;
                    adr += 4;
                    z = Vars.SystemVars.GetVarByName("Z31");
                    z.Address = adr;
                    z = Vars.SystemVars.GetVarByName("st2");
                    z.Address = adr;
                    adr += 4;
                    z = Vars.SystemVars.GetVarByName("Z32");
                    z.Address = adr;
                    z = Vars.SystemVars.GetVarByName("st3");
                    z.Address = adr;
                    adr += 4;
                    z = Vars.SystemVars.GetVarByName("Z33");
                    z.Address = adr;
                    z = Vars.SystemVars.GetVarByName("st4");
                    z.Address = adr;
                }
            }


            for (int i = 1; i < 9; i++)
            {
                ControllerSystemVar v = Vars.SystemVars.GetVarByName("RX_" + i);
                m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+" + v.SystemName + "\\b");
                if (m.Success)
                    v.Address = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);

                v = Vars.SystemVars.GetVarByName("TX_" + i);
                m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+" + v.SystemName + "\\b");
                if (m.Success)
                    v.Address = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);
            }

            ControllerSystemVar v2 = Vars.SystemVars.GetVarByName("RX");
            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+" + v2.SystemName + "\\b");
            if (m.Success)
                v2.Address = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);

            v2 = Vars.SystemVars.GetVarByName("TX");
            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+" + v2.SystemName + "\\b");
            if (m.Success)
                v2.Address = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);


            LoadEmbeddedVarsFromFlashMap(map);
            LoadProcessAddressesFromFlashMap(map);           
               

        }

        public void LoadEmbeddedVarsFromFlashMap(string Map)
        {
            Match m = Regex.Match(Map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys\b");
            if (m.Success)
            {
                int adress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);
                for (int i = 0; i < 1024; i++)
                {
                    Vars.EmbeddedVars.GetVarByName("EE" + i).Address = adress;
                    if (i % 2 == 0)
                        Vars.EmbeddedVars.GetVarByName("EE" + i + "i").Address = adress;
                    if (i % 4 == 0)
                        Vars.EmbeddedVars.GetVarByName("EE" + i + "l").Address = adress;
                    adress++;
                }
            }
            else
                this.CompilationParams.Errors.Add(new CompilationError("Не удалость установить адреса заводских уставок", this.ProgramFileName, -1, true));
        }

        public void LoadProcessAddressesFromFlashMap(string Map)
        {
            this.Processes.Clear();
            MatchCollection mc = Regex.Matches(Map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys4x_p(\d+)\b");
            foreach (Match m in mc)
            {
                ProjectProcess p = new ProjectProcess("PROCESS " + m.Groups[2].Value, Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16));
                this.Processes.Add(p);
            }
        }   

        public List<ControllerIOVar> GetDefaultIOVarsList()
        {
            List<ControllerIOVar> res = new List<ControllerIOVar>();

            // Цифровые входа
            for (int i = 0; i < 4; i++)
            {
                res.Add(new ControllerIOVar() { Name = "IN" + i, SystemName = "_Sys_IN[" + i + "]", Memory = MemoryType.XRAM, Size = 1 });
                res.Add(new ControllerIOVar() { Name = "OUT" + i, SystemName = "_Sys_OUT[" + i + "]", Memory = MemoryType.XRAM, Size = 1 });

                res.Add(new ControllerIOVar() { Name = "DIN" + i, SystemName = "_Sys_IN[" + i + "]", Memory = MemoryType.XRAM, Size = 1 });
            }

            for (int i = 4; i < 6; i++)
            {
                res.Add(new ControllerIOVar() { Name = "DIN" + i, SystemName = "_Sys_IN[" + i + "]", Memory = MemoryType.XRAM, Size = 1 });
                res.Add(new ControllerIOVar() { Name = "DOUT" + i, SystemName = "_Sys_OUT[" + i + "]", Memory = MemoryType.XRAM, Size = 1 });
            }

            for (int i = 1; i < 9; i++)
            {
                res.Add(new ControllerIOVar() { Name = "ADC" + i, SystemName = "_Sys_ADC[" + (i - 1).ToString() + "]", Memory = MemoryType.XRAM, Size = 2 });
                res.Add(new ControllerIOVar() { Name = "ADH" + i, SystemName = "ADH" + i, Memory = MemoryType.XRAM, Size = 1 });
            }
                    
            res.Add(new ControllerIOVar() { Name = "DAC1", SystemName = "_Sys_DAC[0]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "DAH1", SystemName = "DAH1", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerIOVar() { Name = "DAC2", SystemName = "_Sys_DAC[1]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "DAH2", SystemName = "DAH2", Memory = MemoryType.XRAM, Size = 1 });

            return res;
        }

        /// <summary>
        /// Создает список системных переменных процессора
        /// </summary>
        public List<Kontel.Relkon.Classes.ControllerSystemVar> GetSystemVarsList()
        {
            List<ControllerSystemVar> res = new List<ControllerSystemVar>();

            res.Add(new ControllerSystemVar() { Name = "Z30", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "Z31", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "Z32", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "Z33", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerSystemVar() { Name = "st1", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "st2", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "st3", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "st4", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerSystemVar() { Name = "Z40", SystemName = ".bss.led", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "Z50", SystemName = "_SysKey", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerSystemVar() { Name = "led", SystemName = ".bss.led", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "key", SystemName = "_SysKey", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerSystemVar() { Name = "HOUR", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "MIN", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "SEC", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "DATE", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "MONTH", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "YEAR", Memory = MemoryType.XRAM, Size = 1 });

            for (int i = 1; i < 9; i++)
            {
                res.Add(new ControllerSystemVar() { Name = "TX_" + i, SystemName = "TX_" + i, Memory = MemoryType.XRAM, Size = 64, Array = true });
                res.Add(new ControllerSystemVar() { Name = "RX_" + i, SystemName = "RX_" + i, Memory = MemoryType.XRAM, Size = 64, Array = true });
            }

            res.Add(new ControllerSystemVar() { Name = "TX", SystemName = "TX", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "RX", SystemName = "RX", Memory = MemoryType.XRAM, Size = 64, Array = true });

            int address = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    res.Add(new ControllerSystemVar() { Name = "mm" + (i * 64 + j), Size = 1, Memory = MemoryType.RAM, Address = address });
                    if (j % 2 == 0)
                        res.Add(new ControllerSystemVar() { Name = "mm" + (i * 64 + j) + "i", Size = 2, Memory = MemoryType.RAM, Address = address });
                    if (j % 4 == 0)
                        res.Add(new ControllerSystemVar() { Name = "mm" + (i * 64 + j) + "l", Size = 4, Memory = MemoryType.RAM, Address = address });
                    address++;
                }
            }

            return res;
        }

        /// <summary>
        /// Выполняет подготовительные действия после компиляции
        /// </summary>
        protected void PrepareToCompile()
        {
            this.CompilationParams.ErrorsFileNotCreated = false;
            this.CompilationParams.WaitForCompilationErrors = false;
            this.CompilationParams.Errors.Clear();
            this.CompilationParams.CompilationCreatedFilesNames.Clear();
            this.CompilationParams.PostcompileMessages.Clear();
            this.UpdateCodeModel();
        }

        /// <summary>
        /// Создает список заводских (встроенных) переменных процессора
        /// </summary>
        public List<ControllerEmbeddedVar> GetEmbeddedVarsList()
        {
            List<ControllerEmbeddedVar> res = new List<ControllerEmbeddedVar>();
            int wxyAddress = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    res.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 256 + j), Size = 1, Memory = MemoryType.XRAM, Value = 255, Address = wxyAddress });
                    if (j % 2 == 0)                    
                        res.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 256 + j) + "i", Size = 2, Memory = MemoryType.XRAM, Value = 0xFFFF, Address = wxyAddress });                    
                    if (j % 4 == 0)                    
                        res.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 256 + j) + "l", Size = 4, Memory = MemoryType.XRAM, Value = 0xFFFFFFFF, Address = wxyAddress });                    
                    wxyAddress++;
                }
            }
            return res;
        }
       
        
        /// <summary>
        /// Очищает список перменных, полученных из кода прогаммы
        /// </summary>
        internal void FillUserVarsFromCodeModel(RelkonCodeModel CodeModel)
        {
            List<ControllerUserVar> OldVars = new List<ControllerUserVar>(this.vars.UserVars);
            this.vars.UserVars.Clear();            
           
            foreach (RelkonCodeVarDefenition var in CodeModel.Vars)
            {
                ControllerUserVar v = new ControllerUserVar();
                v.Name = var.Name;
                if (var is RelkonCodeArrayDefenition)
                {
                    v.Size = ((RelkonCodeArrayDefenition)var).ItemsCount * this.TypeSize(var.Type);
                    v.Array = true;
                }
                else
                {
                    v.Size = this.TypeSize(var.Type);
                    if (var.Type == "float" || var.Type == "double")
                        v.Real = true;
                }
                v.HasSign = var.HasSign;
               
                if (this is STM32F107Solution)
                    v.Memory = MemoryType.XRAM;
                else
                    throw new Exception("GetVarFromRelkonCodeModel: не добавлена поддержка проектов типа " + this.GetType());
                this.vars.UserVars.Add(v);

                // Устанавливаем адреса ранее присутствовавших переменных
                ControllerUserVar nv = OldVars.Find(new Predicate<ControllerUserVar>(delegate(ControllerUserVar UserVar) { return UserVar.Name == v.Name; }));
                if (nv != null)
                    v.Address = nv.Address;
            }

            foreach (RelkonCodeStruct rkStructVar in CodeModel.Structs)
            {
                ControllerStructVar ctrStructVar = new ControllerStructVar();
                int struct_size = 0;
                ctrStructVar.Name = rkStructVar.Name;

                foreach (RelkonCodeVarDefenition var in rkStructVar.Vars)
                {
                    ControllerUserVar v = new ControllerUserVar();
                    v.Name = var.Name;
                    if (var is RelkonCodeArrayDefenition)
                    {
                        v.Size = ((RelkonCodeArrayDefenition)var).ItemsCount * this.TypeSize(var.Type);
                        v.Array = true;
                    }
                    else
                        v.Size = this.TypeSize(var.Type);
                    v.HasSign = var.HasSign;
                    v.Address = struct_size;
                    ctrStructVar.Vars.Add(v);
                    struct_size += this.TypeSize(var.Type);
                }

                ctrStructVar.Size = struct_size;
                ctrStructVar.HasSign = false;
               
                if (this is STM32F107Solution)
                    ctrStructVar.Memory = MemoryType.XRAM;
                else
                    throw new Exception("GetVarFromRelkonCodeModel: не добавлена поддержка проектов типа " + this.GetType());
                this.vars.UserVars.Add(ctrStructVar);

                // Устанавливаем адреса ранее присутствовавших переменных
                ControllerUserVar nv = OldVars.Find(new Predicate<ControllerUserVar>(delegate(ControllerUserVar UserVar) { return UserVar.Name == ctrStructVar.Name; }));
                if (nv != null)
                    ctrStructVar.Address = nv.Address;
            }


            List<ControllerIOVar> _OldVars = new List<ControllerIOVar>(this.Vars.IOVars);
            this.Vars.IOVars.Clear();
            this.Vars.IOVars.AddRange(this.GetDefaultIOVarsList());
            foreach (IOModule module in CodeModel.IOModules.Values)
            {
                foreach (ModuleVarDescription description in module.VarNames)
                {
                    this.Vars.IOVars.Add(new ControllerIOVar() { Name = description.DisplayName, Memory = MemoryType.XRAM, ExternalModule = true, SystemName = description.SystemName, Size = (module is DigitalModule ? 1 : 2) });
                }
                if (module is AnalogModule)
                {
                    foreach (ModuleVarDescription description in ((AnalogModule)module).SingleByteVarNames)
                    {
                        ControllerIOVar var = _OldVars.Find(new Predicate<ControllerIOVar>(delegate(ControllerIOVar v) { return v.Name == description.DisplayName; }));
                        this.Vars.IOVars.Add(new ControllerIOVar() { Name = description.DisplayName, Memory = MemoryType.XRAM, ExternalModule = true, SystemName = description.SystemName, Size = 1 });
                    }
                }
            }
            foreach (ControllerIOVar OldVar in _OldVars)
            {
                ControllerIOVar var = this.Vars.GetIOVar(OldVar.Name);
                if (var != null)
                    var.Address = OldVar.Address;
            }
        }
       

        protected void CreateErrorsList()
        {
            string ErrorString = File.ReadAllText(this.CompilationParams.CompilationErrorsFileName, Encoding.Default);
            string pattern = @":(\d+):\d+: error:([^\r\n]+)";
            MatchCollection mc = Regex.Matches(ErrorString, pattern, RegexOptions.IgnoreCase);
            for (int i = 0; i < mc.Count; i++)
            {
                CompilationError error = new CompilationError();
                error.Warning = false;
                error.Description = mc[i].Groups[2].Value;
                int ln = this.GetProgramLineNumber(int.Parse(mc[i].Groups[1].Value));
                if (ln == -1)
                {
                    error.LineNumber = int.Parse(mc[i].Groups[1].Value);
                    error.FileName = this.DirectoryName + "\\fc_u.c";
                }
                else
                {
                    error.LineNumber = ln;
                    error.FileName = this.ProgramFileName;
                }
                this.CompilationParams.Errors.Add(error);
            }


            pattern = @":(\d+):\d+: warning:([^\r\n]+)";
            mc = Regex.Matches(ErrorString, pattern, RegexOptions.IgnoreCase);            
            for (int i = 0; i < mc.Count; i++)
            {
                CompilationError error = new CompilationError();
                error.Warning = true;
                error.Description = mc[i].Groups[2].Value;
                if (error.Description.Contains("implicit declaration"))
                    error.Warning = false;
                int ln = this.GetProgramLineNumber(int.Parse(mc[i].Groups[1].Value));
                if (ln == -1)
                {
                    error.LineNumber = int.Parse(mc[i].Groups[1].Value);
                    error.FileName = this.DirectoryName + "\\fc_u.c";
                }
                else
                {
                    error.LineNumber = ln;
                    error.FileName = this.ProgramFileName;
                }
                this.CompilationParams.Errors.Add(error);
            }
        }

        #region Static methods
        /// <summary>
        /// Создает новый проект под указанный тип процессора
        /// </summary>
        /// <param name="Name">Имя проекта</param>
        /// <param name="Directory">Каталог проекта</param>
        /// <param name="Processor">Тип процессора, под который создается проект</param>
        public static ControllerProgramSolution CreateNewSolution(ProcessorType Processor, string Name, string Directory)
        {
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);

            ControllerProgramSolution res = ControllerProgramSolution.Create(Processor);
            res.fileName = Directory + "\\" + Name + ".rp6";
            res.Files.Add(Directory + "\\" + Name + ".kon");
            res.Files.Add(Directory + "\\" + Name + ".plt");            
            res.OpenedFiles.Add(Directory + "\\" + Name + ".kon");
            res.OpenedFiles.Add(Directory + "\\" + Name + ".plt");            
            res.ProgramFileName = Directory + "\\" + Name + ".kon";            
            res.PultFileName = Directory + "\\" + Name + ".plt";
            //res.FbdFileName = Directory + "\\" + Name + ".fbr";
            res.ActiveFileName = res.ProgramFileName;
            res.IsNewSolution = true;
            res.Vars.SystemVars.Clear();
            res.Vars.SystemVars.AddRange(res.GetSystemVarsList());
            res.Vars.EmbeddedVars.Clear();
            res.Vars.EmbeddedVars.AddRange(res.GetEmbeddedVarsList());      

            File.WriteAllBytes(res.programFileName, Kontel.Components.Properties.Resources.ControllerProgramTemplate);
            using (RelkonPultModel pm = new RelkonPultModel(res.pultParams.DefaultPultType))
            {
                pm.Save(res.pultFileName);
            }
                        
            res.Save();
            return res;
        }
        /// <summary>
        /// Создает новый проект под указанный тип процессора без сохранения на диск
        /// </summary>
        /// <param name="Name">Имя проекта</param>
        /// <param name="Directory">Каталог проекта</param>
        /// <param name="Processor">Тип процессора, под который создается проект</param>
        public static ControllerProgramSolution CreateNewSolutionWithoutSave(ProcessorType Processor, string Name, string Directory)
        {
            ControllerProgramSolution res = ControllerProgramSolution.Create(Processor);
            res.fileName = Directory + "\\" + Name + ".rp6";
            res.Files.Add(Directory + "\\" + Name + ".kon");
            res.Files.Add(Directory + "\\" + Name + ".plt");
            res.OpenedFiles.Add(Directory + "\\" + Name + ".kon");
            res.OpenedFiles.Add(Directory + "\\" + Name + ".plt");
            res.ProgramFileName = Directory + "\\" + Name + ".kon";
            res.ActiveFileName = res.ProgramFileName;
            res.PultFileName = Directory + "\\" + Name + ".plt";
            res.IsNewSolution = true;
            res.Vars.SystemVars.Clear();
            res.Vars.SystemVars.AddRange(res.GetSystemVarsList());
            res.Vars.EmbeddedVars.Clear();
            res.Vars.EmbeddedVars.AddRange(res.GetEmbeddedVarsList());        
            return res;
        }
        /// <summary>
        /// Загружает проект из файла
        /// </summary>
        public static ControllerProgramSolution FromFile(string SolutionFileName)
        {
            string s = File.ReadAllText(SolutionFileName, Encoding.Default);            


            bool is50 = false; // показывает, является ли файл проекта в фомате 5.0
            ControllerProgramSolution res = null;
            if (s.Contains("<RelkonSolution"))
            {
                // Файл проекта в формате 5.0; преобразуем в 5.0.1
                ControllerProgramSolution.ConvertSolutionFrom50FormatTo501(SolutionFileName);
                is50 = true;
            }
            else if (s.Contains("Version=\"5.0.1\""))
            {
                // Файл в формате 5.0.1
                res = ControllerProgramSolution.From501File(SolutionFileName);
                res.SaveAs(SolutionFileName);
            }
            else
            {
                if (s.Contains("ControllerDispatcheringVar"))
                {
                    List<string> newFile = new List<string>();
                    string[] strs = File.ReadAllLines(SolutionFileName, Encoding.Default);
                    for (int i = 0; i < strs.Length; i++)
                        if (!strs[i].Contains("ControllerDispatcheringVar"))
                            newFile.Add(strs[i]);

                    File.WriteAllLines(SolutionFileName, newFile.ToArray());
                }
            

                // Файл в текущей версии
                // Загузка проекта из файла
                XmlSerializer xs = new XmlSerializer(typeof(ControllerProgramSolution));
                using (StreamReader sr = new StreamReader(SolutionFileName, Encoding.Default))
                {
                    res = (ControllerProgramSolution)xs.Deserialize(sr);
                }
            }
            // Установка путей к файлам
            res.SolutionFileName = SolutionFileName;
            if (Path.GetDirectoryName(res.programFileName) != Path.GetDirectoryName(SolutionFileName))
                res.ChangeFilesPath(Path.GetDirectoryName(SolutionFileName));                            

            if(is50)
                res.ComputeMultibyteEmbeddedVarsValues();
           
            return res;
        }
        /// <summary>
        /// Загружает файл проекта из файла в формате 5.0.1
        /// </summary>
        private static ControllerProgramSolution From501File(string SolutionFileName)
        {
            XPathDocument xpDoc = new XPathDocument(SolutionFileName);
            XPathNavigator xpNav = ((IXPathNavigable)xpDoc).CreateNavigator();
            // Считывание общих параметров проекта
            XPathNodeIterator xpList = xpNav.Select("/ControllerProgramSolution");
            xpList.MoveNext();
            ControllerProgramSolution res = ControllerProgramSolution.Create(xpList.Current.OuterXml.Contains("AT89C51ED2Solution") ? ProcessorType.AT89C51ED2 : ProcessorType.MB90F347);
            res.ControllerAddress = int.Parse(xpList.Current.GetAttribute("ControllerAddress", ""));
            int SearchedControllerAddress = 0;
            if (int.TryParse(xpList.Current.GetAttribute("SearchedControllerAddres", ""), out SearchedControllerAddress))
                res.SearchedControllerAddress = SearchedControllerAddress;
            res.fileName = SolutionFileName;
            res.ID = Guid.NewGuid();
           
            // Считывание активного файла проекта
            xpList = xpNav.Select("/ControllerProgramSolution/ActiveFileName");
            xpList.MoveNext();
            res.ActiveFileName = xpList.Current.Value;
            // Считывание флага нового проекта
            //xpList = xpNav.Select("/ControllerProgramSolution/IsNewSolution");
            //xpList.MoveNext();
            //res.IsNewSolution = bool.Parse(xpList.Current.Value);
            res.IsNewSolution = false;
            // Считывание списка файлов проекта
            xpList = xpNav.Select("/ControllerProgramSolution/Files/string");
            while (xpList.MoveNext())
            {
                res.Files.Add(xpList.Current.Value);
            }
            // Считывание списка открытых файлов проекта
            xpList = xpNav.Select("/ControllerProgramSolution/OpenedFiles/string");
            while (xpList.MoveNext())
            {
                res.OpenedFiles.Add(xpList.Current.Value);
            }
            // Считывание списка переменных проекта
            xpList = xpNav.Select("/ControllerProgramSolution/Vars/ControllerVar");
            while (xpList.MoveNext())
            {
                string name = xpList.Current.GetAttribute("Name", "");
                int address = int.Parse(xpList.Current.GetAttribute("Address", ""));
                int size = int.Parse(xpList.Current.GetAttribute("Size", ""));
                bool HasSign = xpList.Current.GetAttribute("HasSign", "") != "" ? bool.Parse(xpList.Current.GetAttribute("HasSign", "")) : false;
                bool array = bool.Parse(xpList.Current.GetAttribute("Array", "") == "" ? "false" : xpList.Current.GetAttribute("Array", ""));
                MemoryType memory = (MemoryType)Enum.Parse(typeof(MemoryType), xpList.Current.GetAttribute("Memory", ""));
                ControllerVar var = res.vars.GetVarByName(name);
                if (var == null)
                {
                    res.vars.UserVars.Add(new ControllerUserVar() { Name = name, Address = address, Array = array, HasSign = HasSign, Memory = memory, Size = size });
                }
                else if (var is ControllerSystemVar || var is ControllerIOVar)
                {
                    var.Address = address;
                }
                else if(var is ControllerEmbeddedVar)
                {
                    var.Address = address;
                    ((ControllerEmbeddedVar)var).Value = long.Parse(xpList.Current.GetAttribute("Value", ""));
                }
            }
            // Считывание файла программы проекта
            xpList = xpNav.Select("/ControllerProgramSolution/ProgramFileName");
            xpList.MoveNext();
            res.ProgramFileName = xpList.Current.Value;
            // Считывание файла пультов проекта
            xpList = xpNav.Select("/ControllerProgramSolution/PultFileName");
            xpList.MoveNext();
            res.PultFileName = xpList.Current.Value;
            //if (res is MB90F347Solution)
            //{
            //    // Считывание флага использования LCD-панели
            //    xpList = xpNav.Select("/ControllerProgramSolution/UseLCDPanel");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).UseLCDPanel = bool.Parse(xpList.Current.Value);
            //    // Считывание параметров UART
            //    xpList = xpNav.Select("/ControllerProgramSolution/Uarts/UartOptions");
            //    int i = 0;
            //    while (xpList.MoveNext())
            //    {
            //        // Считывание типа протокола
            //        XPathNodeIterator xpUart = xpList.Current.Select("Protocol");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].Protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), xpUart.Current.Value);
            //        // Считывание скорости
            //        xpUart = xpList.Current.Select("BaudRate");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].BaudRate = int.Parse(xpUart.Current.Value);
            //        // Считывание пароля на чтение
            //        xpUart = xpList.Current.Select("ReadPassword");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].ReadPassword = xpUart.Current.Value;
            //        // Считывание пароля на запись
            //        xpUart = xpList.Current.Select("WritePassword");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].WritePassword = xpUart.Current.Value;
            //        // Считывание размера буфера чтения
            //        xpUart = xpList.Current.Select("ReadBufferSize");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].BufferSize = int.Parse(xpUart.Current.Value);
            //    }
            //    // Считывание телефона диспетчера 1
            //    xpList = xpNav.Select("/ControllerProgramSolution/DispetcherPhone");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).DispatcherPhone1 = xpList.Current.Value;
            //    // Считывание телефона диспетчера 2
            //    xpList = xpNav.Select("/ControllerProgramSolution/DispetcherPhone2");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).DispatcherPhone2 = xpList.Current.Value;
            //    // Считывание телефона диспетчера 3
            //    xpList = xpNav.Select("/ControllerProgramSolution/DispetcherPhone3");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).DispatcherPhone3 = xpList.Current.Value;
            //    // Считывание телефона sms 1
            //    xpList = xpNav.Select("/ControllerProgramSolution/SmsPhone1");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).SmsPhone1 = xpList.Current.Value;
            //    // Считывание телефона sms 2
            //    xpList = xpNav.Select("/ControllerProgramSolution/SmsPhone2");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).SmsPhone2 = xpList.Current.Value;
            //    // Считывание телефона sms 3
            //    xpList = xpNav.Select("/ControllerProgramSolution/SmsPhone3");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).SmsPhone3 = xpList.Current.Value;
            //    // Считывание строки инициализации модема
            //    xpList = xpNav.Select("/ControllerProgramSolution/ModemInitializationString");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).ModemInitializationString = xpList.Current.Value;
            //    // Считывание флага запрещения программирования по протоколу
            //    xpList = xpNav.Select("/ControllerProgramSolution/DenyProgrammingThrowProtocol");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).DenyProgrammingThrowProtocol = bool.Parse(xpList.Current.Value);
            //}
            return res;
        }
        /// <summary>
        /// Преобразует файл проекта из формата 5.0 к текущему
        /// </summary>
        private static void ConvertSolutionFrom50FormatTo501(string SolutionFileName)
        {
            string file = File.ReadAllText(SolutionFileName, Encoding.Default);
            StringBuilder res = new StringBuilder(file);
            
            string header = "<ControllerProgramSolution xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xsi:type=\"AT89C51ED2Solution\" Version=\"5.0.1\" ";
            header += Regex.Match(file, "ControllerAddress=\"\\d+\"").Value + " ";
            header += Regex.Match(file, "SearchedControllerAddress=\"\\d+\"").Value + " ";
            header += Regex.Match(file, "SearchedControllerAddress=\"\\d+\"").Value + " ";
            header += Regex.Match(file, "Protocol=\"[RC51ASCII|RC51BIN]\"").Value + " ";
            header += "BaudRate=\"" + Regex.Match(file, "Speed=\"(\\d+)\"").Groups[1].Value + "\" ";
            header += Regex.Match(file, "ReadPassword=\"[^\"]*\"").Value + " ";
            header += Regex.Match(file, "WritePassword=\"[^\"]*\"").Value + ">";

            res.Replace(Regex.Match(file, "<RelkonSolution[^>]*>").Value, header);
            res.Replace("RelkonVar", "ControllerVar");
            res.Replace("RelkonEmbeddedVar", "ControllerEmbeddedVar");
            res.Replace("RelkonSolution", "ControllerProgramSolution");
            File.WriteAllText(SolutionFileName, res.ToString());
        }
        
        /// <summary>
        /// Создает экземпляр проект указанного типа
        /// </summary>
        private static ControllerProgramSolution Create(Type type)
        {
            ControllerProgramSolution res = null;          
            if (type == typeof(STM32F107Solution))
                res = new STM32F107Solution();
            else
                throw new Exception("Проекты типа " + type + " не поддеживаются");
           
            res.Vars.IOVars.AddRange(res.GetDefaultIOVarsList());
            res.Vars.EmbeddedVars.AddRange(res.GetEmbeddedVarsList());    
    
            return res;
        }
        /// <summary>
        /// Создает экземпляр проект указанного типа
        /// </summary>
        public static ControllerProgramSolution Create(ProcessorType processor)
        {
            ControllerProgramSolution res = null;
            switch (processor)
            {
              
                case ProcessorType.STM32F107:
                    res = Create(typeof(STM32F107Solution));
                    break;
                default:
                    throw new Exception("Процессоры типа " + processor + " не поддерживаются");
            }
            return res;
        }
       

        #endregion

        #region Override methods
        /// <summary>
        /// Изменяет имя некоторого файла проекта
        /// </summary>
        /// <param name="OldFileName">Старое имя файла</param>
        /// <param name="NewFileName">Новое имя файла</param>
        public void ChangeCustomFileName(string OldFileName, string NewFileName)
        {
            int idx = this.Files.IndexOf(OldFileName);
            if(idx!=-1)
                this.Files[idx] = NewFileName;
            idx = this.OpenedFiles.IndexOf(OldFileName);
            if(idx!=-1)
                this.OpenedFiles[idx] = NewFileName;
            if (this.programFileName == OldFileName)
                this.programFileName = NewFileName;
            if (this.pultFileName == OldFileName)
                this.pultFileName = NewFileName;
            //if (this.fbdFileName == OldFileName)
            //    this.fbdFileName = NewFileName;
            if (this.ActiveFileName == OldFileName)
                this.ActiveFileName = NewFileName;
        }

        public void SaveAs(string FileName)
        {
            ControllerProgramSolution backup = null;
            try
            {
                if (FileName != this.SolutionFileName)
                {
                    backup = this.Clone();
                    string DirectoryName = Path.GetDirectoryName(FileName);
                    if (DirectoryName != this.DirectoryName && this.DirectoryName != "")
                    {
                        foreach (string name in this.Files)
                        {
                            File.Copy(name, DirectoryName + "\\" + Path.GetFileName(name), true);
                        }
                        this.ChangeFilesPath(DirectoryName);
                    }
                    if (Path.GetFileNameWithoutExtension(FileName) != this.Name)
                    {
                        string s = DirectoryName + "\\" + Path.GetFileNameWithoutExtension(FileName) + ".kon";
                        if (File.Exists(this.programFileName))
                            FileSystem.Rename(this.programFileName, s, true);
                        this.ChangeCustomFileName(this.programFileName, s);
                        //s = DirectoryName + "\\" + Path.GetFileNameWithoutExtension(FileName) + ".fbr";
                        //if (File.Exists(this.fbdFileName))
                        //    FileSystem.Rename(this.fbdFileName, s, true); 
                        //this.ChangeCustomFileName(this.fbdFileName, s);
                        int count = 0;
                        foreach (string fn in this.Files)
                        {
                            if (Path.GetExtension(fn) == ".plt" && (++count > 1))
                                break;
                        }
                        if (count < 2)
                        {
                            s = DirectoryName + "\\" + Path.GetFileNameWithoutExtension(FileName) + ".plt";
                            if (File.Exists(this.pultFileName))
                                FileSystem.Rename(this.pultFileName, s, true);
                            this.ChangeCustomFileName(this.pultFileName, s);
                        }
                        s = DirectoryName + "\\" + Path.GetFileNameWithoutExtension(FileName) + ".dbg";
                        this.ChangeCustomFileName(this.debugerFileName, s);
                        
                    }
                }
                this.fileName = FileName;
                XmlSerializer xs = new XmlSerializer(typeof(ControllerProgramSolution));
                using (StreamWriter sw = new StreamWriter(FileName, false, Encoding.Default))
                {
                    xs.Serialize(sw, this);
                }
            }
            catch (Exception ex)
            {
                if(backup != null)
                    this.LoadFromAnotherSolution(backup);
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Изменяет пути всех файлов проекта на указанный
        /// </summary>
        private void ChangeFilesPath(string NewDirectoryName)
        {
            if (this.Files.Contains(this.ActiveFileName))
                this.ActiveFileName = NewDirectoryName + "\\" + Path.GetFileName(this.ActiveFileName);
            for (int i = 0; i < this.OpenedFiles.Count; i++)
            {
                if (this.Files.Contains(this.OpenedFiles[i]))
                    this.OpenedFiles[i] = NewDirectoryName + "\\" + Path.GetFileName(this.OpenedFiles[i]);
            }
            for (int i = 0; i < this.Files.Count; i++)
                this.Files[i] = NewDirectoryName + "\\" + Path.GetFileName(this.Files[i]);
            this.ProgramFileName = NewDirectoryName + "\\" + Path.GetFileName(this.ProgramFileName);
            this.PultFileName = NewDirectoryName + "\\" + Path.GetFileName(this.PultFileName);
            //this.fbdFileName = NewDirectoryName + "\\" + Path.GetFileName(this.fbdFileName);
        }
        /// <summary>
        /// Сохраняет копию проекта перед сменой процесора
        /// </summary>
        private void SaveSolutionBackup()
        {
            try
            {
                string s = this.DirectoryName + "\\" + this.Name + "_" + this.processorParams.Type.ToString() + ".bak";
                if (!Directory.Exists(s))
                    Directory.CreateDirectory(s);
                ControllerProgramSolution solution = this.Clone();
                solution.LoadFromAnotherSolution(this);
                solution.SaveAs(s + "\\" + this.Name + ".rp6");
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("Ошибка создания резервной копии проекта: " + Utils.FirstLetterToLower(ex.Message));
            }
        }
       
        /// <summary>
        /// Подставляет адреса переменных в проект для LCD-панели
        /// </summary>
        public void ConvertLCDPanelProject(string FileName, ControllerVarCollection Vars)
        {
            this.RaisedOutputDataReceivedEvent("Конвертация проекта " + Path.GetExtension(FileName) + "...");
            string project = File.ReadAllText(FileName, Encoding.Default);
            int index = project.IndexOf("CTagObject");
            if (index == -1)
                return;
            string tags = project.Substring(index + 10);
            StringBuilder buf = new StringBuilder(tags);
            MatchCollection mc = Regex.Matches(tags, @"([a-z_][a-z0-9_]*)(?:\.(\d+))?(..).(\d#)?(\d+)", RegexOptions.IgnoreCase);
            int k = 0;
            foreach (Match m in mc)
            {
                string VarName = m.Groups[1].Value;
                ControllerVar v = Vars.GetSystemVar(VarName);
                bool SystemVar = true;
                if (v == null)
                {
                    SystemVar = false;
                    v = Vars.GetUserVar(VarName);
                    if (v == null)
                    {
                        this.compilationParams.Errors.Add(new CompilationError("Переменная " + VarName + " отсуствует в проекте", FileName, -1, true));
                        continue;
                    }
                }
                int BitNumber = (m.Groups[2].Value != "") ? int.Parse(m.Groups[2].Value) : -1;
                if (BitNumber > 15)
                {
                    this.compilationParams.Errors.Add(new CompilationError("Переменная " + VarName + "." + m.Groups[2].Value + ": неверный номер бита (должен быть от 0 до 15)", FileName, -1, true));
                    continue;
                }
                string NewDef = VarName + (BitNumber != -1 ? "." + BitNumber : "") + m.Groups[3].Value;
                string Address = "";
                if (BitNumber != -1)
                {
                    if (SystemVar)
                        Address = "" + Utils.AddChars('0', "" + ((v.Address - 0x250) * 8 + BitNumber + 1), 4);
                    else
                        Address = "" + (v.Address + 1) + BitNumber;
                }
                else
                {
                    if (v.Size == 1)
                    {
                        this.compilationParams.Errors.Add(new CompilationError("Переменная " + VarName + ": допускаются только двухбайтные переменные", FileName, -1, true));
                        continue;
                    }
                    else
                        Address += "" + (v.Address + 1);
                }
                NewDef += ((char)(Address.Length + 2)).ToString() + "1#" + Address;
                buf.Replace(m.Value, NewDef, m.Index + k, m.Value.Length);
                k += NewDef.Length - m.Value.Length;
            }
            if (mc.Count > 0)
            {
                project = project.Remove(index + 10);
                project += buf.ToString();
                File.WriteAllText(FileName, project, Encoding.Default);
            }
            this.RaisedOutputDataReceivedEvent("Ok");
        }
        /// <summary>
        /// По номеру строки ошибки в fc_u.c возвращает номер ошибки
        /// в файле программы Relkon
        /// </summary>
        /// <param name="FCLineNumber">номер строки в файле fc_u.c</param>
        protected int GetProgramLineNumber(int FCLineNumber)
        {
            StreamReader srFC = new StreamReader(this.DirectoryName + "\\fc_u.c", Encoding.Default);
            int CurrentLineNumber = 1;
            int res = -1;
            while (CurrentLineNumber <= FCLineNumber)
            {
                Match m = Regex.Match(srFC.ReadLine(), @"/\*(\d+)\*/");
                if (m.Success)
                    res = int.Parse(m.Groups[1].Value);
                else
                {
                    if (CurrentLineNumber != FCLineNumber)
                        res = -1;
                }
                CurrentLineNumber++;
            }
            srFC.Close();
            return res;
        }
        protected int GetProgramLineNumberForBlock(int FCLineNumber)
        {
            StreamReader srFC = new StreamReader(this.compilationParams.SDKDirectory + "\\src\\bk.c", Encoding.Default);
            int CurrentLineNumber = 1;
            int res = -1;
            while (CurrentLineNumber <= FCLineNumber)
            {
                Match m = Regex.Match(srFC.ReadLine(), @"/\*(\d+)\*/");
                if (m.Success)
                    res = int.Parse(m.Groups[1].Value);
                else
                {
                    if (CurrentLineNumber != FCLineNumber)
                        res = -1;
                }
                CurrentLineNumber++;
            }
            srFC.Close();
            return res;
        }
        /// <summary>
        /// Генерирует событие OutputDataReceived
        /// </summary>
        protected void RaisedOutputDataReceivedEvent(string Data)
        {
            if (this.OutputDataReceived != null)
                this.OutputDataReceived(this, new RelkonDataReceivedEventArgs(Data));
        }
        /// <summary>
        /// Обновляет модель кода и список пользовательских переменных проекта  на основе файла программы проекта
        /// </summary>
        public void UpdateCodeModel()
        {
            this.codeModel = RelkonCodeModel.ParseFromFile(this.programFileName, true);
            this.FillUserVarsFromCodeModel(this.codeModel);
        }
        /// <summary>
        /// Обновляет модель кода и список пользовательских переменных проекта на основе указанного кода программы
        /// </summary>
        public void UpdateCodeModel(string code)
        {
            this.codeModel = RelkonCodeModel.ParseFromCode(code);
            this.FillUserVarsFromCodeModel(this.codeModel);
        }
        /// <summary>
        /// Пытается записать указанное сообщение об ошибке в указанный поток записи ошибок;
        /// в случае неудачи - выводит сообщение на консоль
        /// </summary>
        protected void WriteErrorMessageToErrorStream(StreamWriter ErrorWriter, string Message)
        {
            try
            {
                ErrorWriter.WriteLine(Message);
            }
            catch (Exception ex)
            {
                this.compilationParams.ErrorsFileNotCreated = true;
                this.RaisedOutputDataReceivedEvent("Не удалось добавить запись в файл ошибок: " + Utils.FirstLetterToLower(ex.Message));
                this.RaisedOutputDataReceivedEvent(Message);
            }
        }
        /// <summary>
        /// Загружает проект из другого проекта
        /// </summary>
        protected void LoadFromAnotherSolution(ControllerProgramSolution solution)
        {
            if (!(solution.GetType().Equals(this.GetType())))
                throw new Exception("Проект должен быть типа " + this.GetType());
            this.ActiveFileName = solution.ActiveFileName;
            this.Files.Clear();
            this.Files.AddRange(solution.Files);
            this.IsNewSolution = solution.IsNewSolution;
            this.OpenedFiles.Clear();
            this.OpenedFiles.AddRange(solution.OpenedFiles);
            this.ProgramFileName = solution.ProgramFileName;
            this.PultFileName = solution.PultFileName;
            //this.fbdFileName = solution.FbdFileName;
            this.Vars.UserVars.Clear();
            this.Vars.UserVars.AddRange(solution.Vars.UserVars);
            this.fileName = solution.fileName;
            this.LoadControllerParamsFromAnotherSolution(solution);
        }
        /// <summary>
        /// Вычисляет значения двух- и четырехбайтных переменных заводских установок на основе
        /// значений однобайтных переменных для указанного проекта
        /// </summary>
        public void ComputeMultibyteEmbeddedVarsValues()
        {
            for (int count = 2; count < 5; count += 2)
            {
                string s = (count == 2) ? "i" : "l";
                for (char c = 'W'; c <= 'Z'; c++)
                {
                    for (int i = 0; i < 16; i += count)
                    {
                        byte[] bytes = new byte[count];
                        for (int j = 0; j < count; j++)
                        {
                            bytes[j] = (byte)this.Vars.GetEmbeddedVar(c.ToString() + (i + j)).Value;
                        }
                        if (this.ProcessorParams.InverseByteOrder)
                            bytes = Utils.ReflectArray<byte>(bytes);
                        this.Vars.GetEmbeddedVar(c.ToString() + i + s).Value = AppliedMath.BytesToLong(bytes);
                    }
                }
                for (int i = 0; i < 64; i += count)
                {
                    byte[] bytes = new byte[count];
                    for (int j = 0; j < count; j++)
                    {
                        bytes[j] = (byte)this.Vars.GetEmbeddedVar("EE" + (i + j)).Value;
                    }
                    if (this.ProcessorParams.InverseByteOrder)
                        bytes = Utils.ReflectArray<byte>(bytes);
                    this.Vars.GetEmbeddedVar("EE" + i + s).Value = AppliedMath.BytesToLong(bytes);
                }
            }
        }
      

        public RelkonCodeVarDefenition GetRelkonCodeVarDefenitionByName(string name)
        {
            for (int i = 0; i < codeModel.Vars.Count; i++)
            {
                if (codeModel.Vars[i].Name == name)
                {
                    return codeModel.Vars[i];
                }
            }
            return null;
        }

      
        /// <summary>
        /// Сохраняет проект в его текущий файл
        /// </summary>
        public void Save()
        {
            this.SaveAs(this.fileName);
        }
       
        /// <summary>
        /// Проверяет на валидность имя файла или проекта
        /// </summary>
        public static bool IsValidIdentifier(string Name)
        {
            return (Name.IndexOfAny(new char[] { '/', '?', ':', '&', '\\', '*', '"', '<', '>', '|', '#', '%' }) == -1 && Name != "CON" &&
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
        public virtual ControllerProgramSolution GetSolutionThatContainsFile(string FileName)
        {
            return (this.files.Contains(FileName) ? this : null);
        }
        /// <summary>      
     


        /// <summary>
        /// Останавливает процесс программирования контроллера
        /// </summary>
        public void StopUploading()
        {
            this.uploadMgr.StopUploading();
        }
        

        public void UploadToDevice()
        {
            this.uploadMgr.StartUploading(true, true, false);
        }

        public void UploadToDevice(bool onlyProgram, bool onlyParams, bool readEmbVars)
        {
            this.uploadMgr.StartUploading(onlyProgram, onlyParams, readEmbVars);
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

    public enum ProcessorType
    {
        AT89C51ED2,
        MB90F347,
        STM32F107
    }
}
