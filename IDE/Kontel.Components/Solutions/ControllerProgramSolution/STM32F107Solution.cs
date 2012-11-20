using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Classes;
using Kontel;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using Kontel.Relkon.CodeDom;

namespace Kontel.Relkon.Solutions
{
    public sealed class STM32F107Solution : ControllerProgramSolution
    {
        private string label = "";

        public sealed class Uart
        {
            [XmlAttribute]
            public ProtocolType Protocol = ProtocolType.RC51BIN; // протокол передачи данных
            [XmlAttribute]
            public int BaudRate = 19200; // скорость передачи данных  

            public Uart() { }
        }

        private Uart[] uarts = new Uart[2]; // настройки портов процессора 
        private byte[] ipAdress = {0, 0, 0, 0};
        private byte[] ipMask = { 255, 255, 255, 255 };
        private byte[] ipGateway = { 0, 0, 0, 0 };
        private string macAdress = "000000000000";
        private bool pultEnable = true;
        private bool _SDEnable = false;

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

        internal STM32F107Solution()
        {
            this.uploadMgr = new UploadMgr(this);
            this.uploadMgr.ProgressChanged += new UploadMgrProgressChangedEventHandler(mgr_ProgressChanged);
            this.uploadMgr.UploadingCompleted += new AsyncCompletedEventHandler(mgr_UploadingCompleted);
        }

        public override void LoadControllerParamsFromAnotherSolution(ControllerProgramSolution solution)
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

        protected override ControllerProgramSolution Clone()
        {
            STM32F107Solution res = new STM32F107Solution();
            res.LoadFromAnotherSolution(this);
            return res;
        }

        protected override void IntitalizeParams()
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

       

        protected override void CreatePostcompileMessages()
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

       

        public override void Compile()
        {
            //try
            //{
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
            //}
            //catch (Exception ex)
            //{
            //    this.CompilationParams.Errors.Add(new CompilationError(ex.Message, "", -1, false));
            //}
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

        

        public override void LoadVarsAddressesFromFlashMap(Kontel.Relkon.Classes.ControllerVarCollection Vars)
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
                    if (var != null)
                    {
                        var.Address = adress;
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

            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_ADC");
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

             m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+IN");
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


             m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+OUT");
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
                m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+" + z.SystemName + "\\b");
                if (m.Groups[1].Success)
                    z.Address = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);

                z = Vars.SystemVars.GetVarByName("Z50");
                m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+" + z.SystemName + "\\b");
                if (m.Groups[1].Success)
                    z.Address = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);


                m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys\b");
                if (m.Groups[1].Success)
                {
                    adr = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16) + 1024 + 256 + 5;
                    z = Vars.SystemVars.GetVarByName("Z30");
                    z.Address = adr;
                    adr += z.Size;
                    z = Vars.SystemVars.GetVarByName("Z31");
                    z.Address = adr;
                    adr += z.Size;
                    z = Vars.SystemVars.GetVarByName("Z32");
                    z.Address = adr;
                    adr += z.Size;
                    z = Vars.SystemVars.GetVarByName("Z33");
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

        internal override void FillUserVarsFromCodeModel(Kontel.Relkon.CodeDom.RelkonCodeModel CodeModel)
        {
            base.FillUserVarsFromCodeModel(CodeModel);
            List<ControllerIOVar> OldVars = new List<ControllerIOVar>(this.Vars.IOVars);
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
                        ControllerIOVar var = OldVars.Find(new Predicate<ControllerIOVar>(delegate(ControllerIOVar v) { return v.Name == description.DisplayName; }));
                        this.Vars.IOVars.Add(new ControllerIOVar() { Name = description.DisplayName, Memory = MemoryType.XRAM, ExternalModule = true, SystemName = description.SystemName, Size = 1 });
                    }
                }
            }
            foreach (ControllerIOVar OldVar in OldVars)
            {
                ControllerIOVar var = this.Vars.GetIOVar(OldVar.Name);
                if (var != null)
                    var.Address = OldVar.Address;
            }
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
    }
}
