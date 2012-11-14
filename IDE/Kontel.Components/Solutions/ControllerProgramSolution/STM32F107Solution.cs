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

        protected override int TypeSize(string Type)
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

        public override bool IsValidPultVarMask(string Mask)
        {
            return Regex.IsMatch(Mask, @"^\d{1,7}([,\.]\d{1,7})?$");
        }

        public override void Compile()
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
                this.ConvertAllLCDPanelProjects(); // перенести для MB90F347

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

        protected override void CreateErrorsList()
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
                    var.Address = adress;
                    adress += var.Size;
                }

                for (int i = 4; i < 6; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("DIN" + i);
                    var.Address = adress;
                    adress += var.Size;
                }

            }

            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys_OUT");
            if (m.Success)
            {
                int adress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);
                for (int i = 0; i < 4; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("OUT" + i);
                    var.Address = adress;
                    adress += var.Size;
                }

                for (int i = 4; i < 6; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("DOUT" + i);
                    var.Address = adress;
                    adress += var.Size;
                }
            }

            m = Regex.Match(map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys_ADC");
            if (m.Success)
            {
                int adress = Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16);

                for (int i = 1; i < 9; i++)
                {
                    ControllerIOVar var = Vars.IOVars.GetVarByName("ADH" + i);                    
                    var.Address = adress;
                    var = Vars.IOVars.GetVarByName("ADC" + i);
                    var.Address = adress;
                    adress += var.Size;
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

            //res.Add(new ControllerSystemVar() { Name = "Z30", SystemName = "Z30", Memory = MemoryType.XRAM, Size = 1 });
            //res.Add(new ControllerSystemVar() { Name = "Z31", SystemName = "Z31", Memory = MemoryType.XRAM, Size = 1 });
            //res.Add(new ControllerSystemVar() { Name = "Z32", SystemName = "Z32", Memory = MemoryType.XRAM, Size = 1 });
            //res.Add(new ControllerSystemVar() { Name = "Z33", SystemName = "Z33", Memory = MemoryType.XRAM, Size = 1 });
         


            //foreach (ControllerIOVar var in Vars.IOVars)
            //{
            //    if (Regex.IsMatch(var.Name, @"\b(?:(?:ADH)|(?:DAH))\d+\b"))
            //        continue;
            //    Match m = Regex.Match(map, "\\b" + (var.ExternalModule ? "" : "_") + var.SystemName + @"\s+(?:(?:Var)|(?:Addr))\.\s+g\s0x([0-9A-F]+)");
            //    if (m.Success)
            //    {
            //        var.Address = Convert.ToInt32(m.Groups[1].Value, 16);
            //        Match ioMatch = Regex.Match(var.Name, @"\b((?:ADC)|(?:DAC))(\d+)\b");
            //        if (ioMatch.Success)
            //        {
            //            ControllerIOVar hVar = Vars.GetIOVar(ioMatch.Groups[1].Value.Remove(2) + "H" + ioMatch.Groups[2].Value);
            //            if (hVar != null)
            //                hVar.Address = var.Address + 1;
            //            else
            //                this.CompilationParams.Errors.Add(new CompilationError("Не удалость установить адрес переменной " + ioMatch.Groups[1].Value.Remove(2) + "H", this.ProgramFileName, -1, true));
            //        }
            //    }
            //    else
            //        this.CompilationParams.Errors.Add(new CompilationError("Не удалость установить адрес переменной " + var.Name, this.ProgramFileName, -1, true));
            //}

            //// Установка адресов пользовательских переменных
            //int dateAddress = -1;
            //int timeAddress = -1;
            //if (Vars.GetUserVar("date_Date") != null)
            //{
            //    string s = Regex.Match(map, "\\b" + @"date\s+(?:(?:Var)|(?:Addr))\.\s+g\s0x([0-9A-F]+)").Groups[1].Value;
            //    if (s != "")
            //        dateAddress = Convert.ToInt32(s, 16);
            //}
            //if (Vars.GetUserVar("time_Second") != null)
            //{
            //    string s = Regex.Match(map, "\\b" + @"time\s+(?:(?:Var)|(?:Addr))\.\s+g\s0x([0-9A-F]+)").Groups[1].Value;
            //    if (s != "")
            //        timeAddress = Convert.ToInt32(s, 16);
            //}

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
            //Определение списка процессов
            this.Processes.Clear();

            //MatchCollection mc = Regex.Matches(Map, "\\b" + @"_Sys4x_p(\d+)\s+(?:(?:Var)|(?:Addr))\.\s+g\s0x([0-9A-F]+)");
            //foreach (Match m in mc)
            //{
            //    ProjectProcess p = new ProjectProcess("PROCESS " + m.Groups[1].Value, AppliedMath.HexToDec(m.Groups[2].Value));
            //    MatchCollection mcps = Regex.Matches(Map, "\\b" + @"_Sys4x_sit_p" + (m.Groups[1].Value) + @"_(\d+)\s+(?:(?:Func)|(?:Addr))\.\s+g\s0x([0-9A-F]+)");
            //    foreach (Match m1 in mcps)
            //    {
            //        p.Situations.Add(new ProjectSituation("SIT" + m1.Groups[1].Value, AppliedMath.HexToDec(m1.Groups[2].Value)));
            //    }
            //    Match m2 = Regex.Match(Map, "\\b" + @"empty\s+(?:(?:Func)|(?:Addr))\.\s+g\s0x([0-9A-F]+)");
            //    p.Situations.Add(new ProjectSituation("Остановлен", AppliedMath.HexToDec(m2.Groups[1].Value)));
            //    this.Processes.Add(p);
            //}

            
            MatchCollection mc = Regex.Matches(Map, @"\b\s+0x([0-9a-fA-F]{8})\s+_Sys4x_p(\d+)\b");
            foreach (Match m in mc)
            {
                ProjectProcess p = new ProjectProcess("PROCESS " + m.Groups[2].Value, Convert.ToInt32(m.Groups[1].Value.Substring(4, 4), 16));
                //MatchCollection mcps = Regex.Matches(Map, "\\b" + @"_Sys4x_sit_p" + (m.Groups[1].Value) + @"_(\d+)\s+(?:(?:Func)|(?:Addr))\.\s+g\s0x([0-9A-F]+)");
                //foreach (Match m1 in mcps)
                //{
                //    p.Situations.Add(new ProjectSituation("SIT" + m1.Groups[1].Value, AppliedMath.HexToDec(m1.Groups[2].Value)));
                //}
                //Match m2 = Regex.Match(Map, "\\b" + @"empty\s+(?:(?:Func)|(?:Addr))\.\s+g\s0x([0-9A-F]+)");
                //p.Situations.Add(new ProjectSituation("Остановлен", AppliedMath.HexToDec(m2.Groups[1].Value)));
                this.Processes.Add(p);
            }
        }

        public override List<Kontel.Relkon.Classes.ControllerIOVar> GetDefaultIOVarsList()
        {
            List<ControllerIOVar> res = new List<ControllerIOVar>();
            // Цифровые входа
            res.Add(new ControllerIOVar() { Name = "IN0", SystemName = "_Sys_IN[0]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "IN1", SystemName = "_Sys_IN[1]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "IN2", SystemName = "_Sys_IN[2]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "IN3", SystemName = "_Sys_IN[3]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "DIN4", SystemName = "_Sys_IN[4]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "DIN5", SystemName = "_Sys_IN[5]", Memory = MemoryType.XRAM, Size = 1 });
            // Цифровые выхода
            res.Add(new ControllerIOVar() { Name = "OUT0", SystemName = "_Sys_OUT[0]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "OUT1", SystemName = "_Sys_OUT[1]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "OUT2", SystemName = "_Sys_OUT[2]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "OUT3", SystemName = "_Sys_OUT[3]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "DOUT4", SystemName = "_Sys_OUT[4]", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerIOVar() { Name = "DOUT5", SystemName = "_Sys_OUT[5]", Memory = MemoryType.XRAM, Size = 1 });
            // Аналоговые входа
            res.Add(new ControllerIOVar() { Name = "ADC1", SystemName = "_Sys_ADC[0]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "ADH1", SystemName = "ADH1", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerIOVar() { Name = "ADC2", SystemName = "_Sys_ADC[1]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "ADH2", SystemName = "ADH2", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerIOVar() { Name = "ADC3", SystemName = "_Sys_ADC[2]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "ADH3", SystemName = "ADH3", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerIOVar() { Name = "ADC4", SystemName = "_Sys_ADC[3]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "ADH4", SystemName = "ADH4", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerIOVar() { Name = "ADC5", SystemName = "_Sys_ADC[4]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "ADH5", SystemName = "ADH5", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerIOVar() { Name = "ADC6", SystemName = "_Sys_ADC[5]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "ADH6", SystemName = "ADH6", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerIOVar() { Name = "ADC7", SystemName = "_Sys_ADC[6]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "ADH7", SystemName = "ADH7", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerIOVar() { Name = "ADC8", SystemName = "_Sys_ADC[7]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "ADH8", SystemName = "ADH8", Memory = MemoryType.XRAM, Size = 1 });        


            res.Add(new ControllerIOVar() { Name = "DAC1", SystemName = "_Sys_DAC[0]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "DAH1", SystemName = "DAH1", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerIOVar() { Name = "DAC2", SystemName = "_Sys_DAC[1]", Memory = MemoryType.XRAM, Size = 2 });
            res.Add(new ControllerIOVar() { Name = "DAH2", SystemName = "DAH2", Memory = MemoryType.XRAM, Size = 1 });
      


            return res;
        }

        public override List<Kontel.Relkon.Classes.ControllerSystemVar> GetSystemVarsList()
        {
            List<ControllerSystemVar> res = new List<ControllerSystemVar>();
            res.Add(new ControllerSystemVar() { Name = "Z30", SystemName = "Z30", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "Z31", SystemName = "Z31", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "Z32", SystemName = "Z32", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "Z33", SystemName = "Z33", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "Z40", SystemName = "led", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "Z50", SystemName = "key", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "HOUR", SystemName = "_Sys4x_Hour", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "MIN", SystemName = "_Sys4x_Minute", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "SEC", SystemName = "_Sys4x_Second", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "DATE", SystemName = "_Sys4x_Date", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "MONTH", SystemName = "_Sys4x_Month", Memory = MemoryType.XRAM, Size = 1 });
            res.Add(new ControllerSystemVar() { Name = "YEAR", SystemName = "_Sys4x_Year", Memory = MemoryType.XRAM, Size = 1 });

            res.Add(new ControllerSystemVar() { Name = "TX_1", SystemName = "TX_1", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "TX_2", SystemName = "TX_2", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "TX_3", SystemName = "TX_3", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "TX_4", SystemName = "TX_4", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "TX_5", SystemName = "TX_5", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "TX_6", SystemName = "TX_6", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "TX_7", SystemName = "TX_7", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "TX_8", SystemName = "TX_8", Memory = MemoryType.XRAM, Size = 64, Array = true });

            res.Add(new ControllerSystemVar() { Name = "RX_1", SystemName = "RX_1", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "RX_2", SystemName = "RX_2", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "RX_3", SystemName = "RX_3", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "RX_4", SystemName = "RX_4", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "RX_5", SystemName = "RX_5", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "RX_6", SystemName = "RX_6", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "RX_7", SystemName = "RX_7", Memory = MemoryType.XRAM, Size = 64, Array = true });
            res.Add(new ControllerSystemVar() { Name = "RX_8", SystemName = "RX_8", Memory = MemoryType.XRAM, Size = 64, Array = true });
            return res;
        }

        public override List<ControllerEmbeddedVar> GetEmbeddedVarsList()
        {
            List<ControllerEmbeddedVar> res = new List<ControllerEmbeddedVar>();
            int wxyAddress = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    res.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 256 + j), Size = 1, Memory = MemoryType.XRAM, Value = 255, Address = wxyAddress });
                    if (j % 2 == 0)
                    {
                        res.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 256 + j) + "i", Size = 2, Memory = MemoryType.XRAM, Value = 0xFFFF, Address = wxyAddress });
                    }
                    if (j % 4 == 0)
                    {
                        res.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 256 + j) + "l", Size = 4, Memory = MemoryType.XRAM, Value = 0xFFFFFFFF, Address = wxyAddress });
                    }
                    wxyAddress++;
                }
            }
            return res;
        }

        public override List<ControllerDispatcheringVar> GetDispatcheringVarsList()
        {
            List<ControllerDispatcheringVar> res = new List<ControllerDispatcheringVar>();

            int address = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    res.Add(new ControllerDispatcheringVar() { Name = "mem" + (i * 64 + j), Size = 1, Memory = MemoryType.RAM, Address = address });
                    if (j % 2 == 0)
                        res.Add(new ControllerDispatcheringVar() { Name = "mem" + (i * 64 + j) + "i", Size = 2, Memory = MemoryType.RAM, Address = address });                    
                    if (j % 4 == 0)
                        res.Add(new ControllerDispatcheringVar() { Name = "mem" + (i * 64 + j) + "l", Size = 4, Memory = MemoryType.RAM, Address = address });                    
                    address++;
                }
            }

            return res;
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

        protected override void PrepareToCompile()
        {
            this.CompilationParams.ErrorsFileNotCreated = false;
            this.CompilationParams.WaitForCompilationErrors = false;
            this.CompilationParams.Errors.Clear();
            this.CompilationParams.CompilationCreatedFilesNames.Clear();
            this.CompilationParams.PostcompileMessages.Clear();

            this.codeModel = RelkonCodeModel.ParseFromFile(this.ProgramFileName, true);

            foreach (RelkonCodeProcess pr in this.codeModel.Processes)
            {
                foreach (RelkonCodeSituation sit in pr.Situations)
                {
                    if (sit.Period != 1 &&
                        sit.Period != 5 &&
                        sit.Period != 10 &&
                        sit.Period != 100 &&
                        sit.Period != 1000)
                    {
                        this.CompilationParams.Errors.Add(new CompilationError("Ситуация с периодом " + sit.Period + " не допустима. Возможные периоды для ситуаций 1, 5, 10 и 100 мс", "", sit.LineNumber, false));
                    }
                }
            }

            this.FillUserVarsFromCodeModel(this.codeModel);
        }

        private void TranslateProgram(STM32F107CodeGenerator CodeGenerator)
        {
            CodeGenerator.GenerateProgramCode();
            CodeGenerator.GeneratePultCode();
            this.CompilationParams.Errors.AddRange(CodeGenerator.Errors);
            if (!CodeGenerator.HasErrors)
                this.RaisedOutputDataReceivedEvent(this.ProgramFileName + ", " + this.PultFileName + " -> " + this.DirectoryName + "\\fc_u.c, " + this.DirectoryName + "\\modules.h");
            //this.SystemCycle = CodeGenerator.DispetcherPeriod;
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

        public override void UploadToDevice()
        {
            ((UploadMgr)this.uploadMgr).StartUploading(true, true, false);
        }

        public override void UploadToDevice(bool onlyProgram, bool onlyParams, bool readEmbVars)
        {
            ((UploadMgr)this.uploadMgr).StartUploading(onlyProgram, onlyParams, readEmbVars);            
        }

        
    }
}
