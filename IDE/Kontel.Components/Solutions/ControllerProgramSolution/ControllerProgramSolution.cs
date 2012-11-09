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

namespace Kontel.Relkon.Solutions 
{
    /// <summary>
    /// ������ ��������� �� ����������
    /// </summary>
    [XmlInclude(typeof(STM32F107Solution))]
    public abstract class ControllerProgramSolution : Solution
    {
        private static string CurrentVersion = "6.0.0"; // ������ ��������� �������
        private string programFileName = ""; // ��� ����� ���������
        private string pultFileName = ""; // ��� ����� �������
        //private string fbdFileName = ""; // ��� ����� ����-��������
        private string debugerFileName = ""; // ��� ����� ���������
        private CompilationParams compilationParams = new CompilationParams(); // ��������� ����������
        private PultParams pultParams = new PultParams(); // ��������� �������
        private ProcessorParams processorParams = new ProcessorParams(); // ��������� ���������
        private ControllerVarCollection vars = new ControllerVarCollection(); // ��� ���������� �������
        internal RelkonCodeModel codeModel; // �� ������������� ���� ���������
        private List<string> searchList = new List<string>(); // ������ ����� ��� ������
        private List<string> replaceList = new List<string>(); // ������ ����� ��� ������
        
        

        // ��������� �����������
        private int controllerAddress = 1; // ������� �����
        private int searchedControllerAddress = 0; // ��� ���������������� ����� ������������� ����� ����������� � ���� �������
        /// <summary>
        /// ��������� ��� ���������� ������ � �������� ����� ������
        /// </summary>
        public virtual event RelkonDataReceivedEventHandler OutputDataReceived;

        public ControllerProgramSolution()
        {
            this.IntitalizeParams();
            this.Processes = new List<ProjectProcess>();
            this.CreateNotRemovedExtensionsList();
            this.Version = ControllerProgramSolution.CurrentVersion;
        }
        /// <summary>
        /// ������ ��������� �������
        /// </summary>
        public List<ProjectProcess> Processes { get; set; }
        /// <summary>
        /// ���������� ������� �� ��� ��������
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
        /// ���������� ��� ������������� ������� ����� �����������
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
        /// ��� ���������������� ����� ������������� ����� ����������� � ���� �������
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
        /// ���������� ������ ���������� �������
        /// </summary>
        public ControllerVarCollection Vars
        {
            get
            {
                return this.vars;
            }
        }
        /// <summary>
        /// ���������� ������ ��� ������
        /// </summary>
        public List<string> SearchList
        {
            get
            {
                return this.searchList;
            }
        }
        /// <summary>
        /// ���������� ������ ��� ������
        /// </summary>
        public List<string> ReplaceList
        {
            get
            {
                return this.replaceList;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ��� ����� ���������
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
        /// ���������� ��� ������������� ��� ����� �������
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
        /// ���������� ��� ������������� ��� ����� �������
        /// </summary>
        //public string FbdFileName
        //{
        //    get
        //    {
        //        return this.fbdFileName;
        //    }
        //    set
        //    {
        //        if (value != this.fbdFileName)
        //        {
        //            this.fbdFileName = value;
        //        }
        //    }
        //}
        /// <summary>
        /// ���������� ��� ������������� ��� ����� �������� ���������
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
        /// ���������� ����� ���������� ���������� �������
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
        /// ���������� ����� ���������� ������� �������
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
        /// ���������� ����� ���������� ���������� �������
        /// </summary>
        [XmlIgnore]
        public ProcessorParams ProcessorParams
        {
            get
            {
                return this.processorParams;
            }
        }
        
        public override string Extension
        {
            get 
            {
                return ".rp6";
            }
        }

        public override string FileDialogFilter
        {
            get 
            {
                return "������� Relkon (.rp6)|*.rp6";
            }
        }
        /// <summary>
        /// ������� ������ ���������� ������, ������� �� ������ ������������ ��� �������������� ������� � ������ �����
        /// </summary>
        private void CreateNotRemovedExtensionsList()
        {
            this.notRemovedExtensios = new List<string>();
            this.notRemovedExtensios.Add(".epj");
        }

        #region Abstract and virtual methods
        /// <summary>
        /// ��������� ��������� ����������� �� ������� �������
        /// </summary>
        /// <param name="solution"></param>
        public abstract void LoadControllerParamsFromAnotherSolution(ControllerProgramSolution solution);
        /// <summary>
        /// ������� ������ ����� ���������� ������
        /// </summary>
        /// <returns></returns>
        protected abstract ControllerProgramSolution Clone();
        /// <summary>
        /// �������������� ��������� ����������, ����������
        /// </summary>
        protected abstract void IntitalizeParams();
        /// <summary>
        /// ���������� ������ ���������� ���������� ���� (� ������)
        /// </summary>
        /// <param name="Type">��� ���� (char, int � �.�.)</param>
        protected abstract int TypeSize(string Type);
        /// <summary>
        /// ��������� ������ �������������� ��������� �� ����������� ����������
        /// </summary>
        protected abstract void CreatePostcompileMessages();
        /// <summary>
        /// ���������, �������� �� ����� ������ ���������� 
        /// �������� ��� ������� ���� ���������
        /// </summary>
        public abstract bool IsValidPultVarMask(string Mask);
        /// <summary>
        /// ����������� ������
        /// </summary>
        public abstract void Compile();
        /// <summary>
        /// ������� ������ ������ ����������
        /// </summary>
        protected abstract void CreateErrorsList();
        /// <summary>
        /// ��������� ������ ���������� �� ����� Flash.map
        /// </summary>
        public abstract void LoadVarsAddressesFromFlashMap(ControllerVarCollection Vars);
        /// <summary>
        /// ��������� ���������������� �������� ����� ����������
        /// </summary>
        protected virtual void PrepareToCompile()
        {
            this.CompilationParams.ErrorsFileNotCreated = false;
            this.CompilationParams.WaitForCompilationErrors = false;
            this.CompilationParams.Errors.Clear();
            this.CompilationParams.CompilationCreatedFilesNames.Clear();
            this.CompilationParams.PostcompileMessages.Clear();
            this.UpdateCodeModel();
        }
        /// <summary>
        /// ���������� ������ ����������� ���������� �����-������
        /// </summary>
        public abstract List<ControllerIOVar> GetDefaultIOVarsList();
        /// <summary>
        /// ������� ������ ��������� ���������� ����������
        /// </summary>
        public abstract List<ControllerSystemVar> GetSystemVarsList();
        /// <summary>
        /// ������� ������ ��������� (����������) ���������� ����������
        /// </summary>
        public virtual List<ControllerEmbeddedVar> GetEmbeddedVarsList()
        {
            List<ControllerEmbeddedVar> res = new List<ControllerEmbeddedVar>();
            int wxyAddress = 0;
            for (int i = 0; i < 4; i++)
            {               
                for (int j = 0; j < 64; j++)
                {                    
                    res.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 64 + j), Size = this.TypeSize("char"), Memory = MemoryType.XRAM, Value = 255, Address = wxyAddress });
                    if (j % 2 == 0)
                    {
                        res.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 64 + j) + "i", Size = this.TypeSize("int"), Memory = MemoryType.XRAM, Value = 0xFFFF, Address = wxyAddress });
                    }
                    if (j % 4 == 0)
                    {                        
                        res.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 64 + j) + "l", Size = this.TypeSize("long"), Memory = MemoryType.XRAM, Value = 0xFFFFFFFF, Address = wxyAddress });
                    }
                    wxyAddress++;
                }
            }
            return res;
        }

        public abstract List<ControllerDispatcheringVar> GetDispatcheringVarsList();
        
        /// <summary>
        /// ������� ������ ���������, ���������� �� ���� ��������
        /// </summary>
        internal virtual void FillUserVarsFromCodeModel(RelkonCodeModel CodeModel)
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
                    throw new Exception("GetVarFromRelkonCodeModel: �� ��������� ��������� �������� ���� " + this.GetType());
                this.vars.UserVars.Add(v);

                // ������������� ������ ����� ���������������� ����������
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
                    throw new Exception("GetVarFromRelkonCodeModel: �� ��������� ��������� �������� ���� " + this.GetType());
                this.vars.UserVars.Add(ctrStructVar);

                // ������������� ������ ����� ���������������� ����������
                ControllerUserVar nv = OldVars.Find(new Predicate<ControllerUserVar>(delegate(ControllerUserVar UserVar) { return UserVar.Name == ctrStructVar.Name; }));
                if (nv != null)
                    ctrStructVar.Address = nv.Address;
            }            
        }
        #endregion

        #region Static methods
        /// <summary>
        /// ������� ����� ������ ��� ��������� ��� ����������
        /// </summary>
        /// <param name="Name">��� �������</param>
        /// <param name="Directory">������� �������</param>
        /// <param name="Processor">��� ����������, ��� ������� ��������� ������</param>
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

            res.Vars.DispatcheringVars.Clear();
            res.Vars.DispatcheringVars.AddRange(res.GetDispatcheringVarsList());

            File.WriteAllBytes(res.programFileName, Kontel.Components.Properties.Resources.ControllerProgramTemplate);
            using (RelkonPultModel pm = new RelkonPultModel(res.pultParams.DefaultPultType))
            {
                pm.Save(res.pultFileName);
            }
                        
            res.Save();
            return res;
        }
        /// <summary>
        /// ������� ����� ������ ��� ��������� ��� ���������� ��� ���������� �� ����
        /// </summary>
        /// <param name="Name">��� �������</param>
        /// <param name="Directory">������� �������</param>
        /// <param name="Processor">��� ����������, ��� ������� ��������� ������</param>
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
            res.Vars.DispatcheringVars.Clear();
            res.Vars.DispatcheringVars.AddRange(res.GetDispatcheringVarsList());
            return res;
        }
        /// <summary>
        /// ��������� ������ �� �����
        /// </summary>
        public static ControllerProgramSolution FromFile(string SolutionFileName)
        {
            string s = File.ReadAllText(SolutionFileName, Encoding.Default);
            bool is50 = false; // ����������, �������� �� ���� ������� � ������ 5.0
            ControllerProgramSolution res = null;
            if (s.Contains("<RelkonSolution"))
            {
                // ���� ������� � ������� 5.0; ����������� � 5.0.1
                ControllerProgramSolution.ConvertSolutionFrom50FormatTo501(SolutionFileName);
                is50 = true;
            }
            else if (s.Contains("Version=\"5.0.1\""))
            {
                // ���� � ������� 5.0.1
                res = ControllerProgramSolution.From501File(SolutionFileName);
                res.SaveAs(SolutionFileName);
            }
            else
            {
                // ���� � ������� ������
                // ������� ������� �� �����
                XmlSerializer xs = new XmlSerializer(typeof(ControllerProgramSolution));
                using (StreamReader sr = new StreamReader(SolutionFileName, Encoding.Default))
                {
                    res = (ControllerProgramSolution)xs.Deserialize(sr);
                }
            }
            // ��������� ����� � ������
            res.SolutionFileName = SolutionFileName;
            if (Path.GetDirectoryName(res.programFileName) != Path.GetDirectoryName(SolutionFileName))
                res.ChangeFilesPath(Path.GetDirectoryName(SolutionFileName));
            

            // �������� ��������� ����������, � ������ �������������
            if (res.vars.SystemVars.Count == 0 || (res.vars.SystemVars.GetVarByName("RX_0") == null))
            {
                res.vars.SystemVars.Clear();
                res.vars.EmbeddedVars.Clear();
                res.vars.SystemVars.AddRange(res.GetSystemVarsList());
                res.vars.EmbeddedVars.AddRange(res.GetEmbeddedVarsList());              
            }

            if (res.vars.DispatcheringVars.Count == 0)
            {
                res.Vars.DispatcheringVars.AddRange(res.GetDispatcheringVarsList());              
            }

            var yearVar = res.vars.SystemVars.GetVarByName("YEAR");

            if (yearVar == null)
                res.vars.SystemVars.Add(new ControllerSystemVar() { Name = "YEAR", SystemName = "_Sys4x_Year", Memory = MemoryType.XRAM, Size = 1 });
          

            //if (res.vars.EmbeddedVars[0].Name == "W0")
            //{              
            //    string file = File.ReadAllText(res.ProgramFileName, Encoding.Default);
            //    string pult = File.ReadAllText(res.PultFileName, Encoding.Default);

            //    ControllerEmbeddedVar var = null;
                       
            //    for (int i = 0; i < 16; i++)
            //    {
            //        var = res.vars.EmbeddedVars.GetVarByName("W" + i);
            //        if (var != null)
            //        {
            //            var.Name = "EE" + (i + 64).ToString();
            //            file = Regex.Replace(file, @"\b" + "W" + i + @"\b", "EE" + (i + 64).ToString());
            //            pult = Regex.Replace(pult, @"\b" + "W" + i + @"\b", "EE" + (i + 64).ToString());
            //        }

            //        var = res.vars.EmbeddedVars.GetVarByName("X" + i);
            //        if (var != null)
            //        {                        
            //            var.Name = "EE" + (i + 80).ToString();
            //            file = Regex.Replace(file, @"\b" + "X" + i + @"\b", "EE" + (i + 80).ToString());
            //            pult = Regex.Replace(pult, @"\b" + "X" + i + @"\b", "EE" + (i + 80).ToString());
            //        }

            //        var = res.vars.EmbeddedVars.GetVarByName("Y" + i);
            //        if (var != null)
            //        {
            //            var.Name = "EE" + (i + 96).ToString();
            //            file = Regex.Replace(file, @"\b" + "Y" + i + @"\b", "EE" + (i + 96).ToString());
            //            pult = Regex.Replace(pult, @"\b" + "Y" + i + @"\b", "EE" + (i + 96).ToString());
            //        }

            //        var = res.vars.EmbeddedVars.GetVarByName("Z" + i);
            //        if (var != null)
            //        {
            //            var.Name = "EE" + (i + 112).ToString();
            //            file = Regex.Replace(file, @"\b" + "Z" + i + @"\b", "EE" + (i + 112).ToString());
            //            pult = Regex.Replace(pult, @"\b" + "Z" + i + @"\b", "EE" + (i + 112).ToString());
            //        }

            //    }

            //    for (int i = 0; i < 16; i = i + 2)
            //    {
            //        res.vars.EmbeddedVars.GetVarByName("W" + i + "i").Name = "EE" + (i + 32).ToString() + "i";
            //        file = Regex.Replace(file, @"\b" + "W" + i + "i" + @"\b", "EE" + (i + 32).ToString() + "i");
            //        pult = Regex.Replace(pult, @"\b" + "W" + i + "i" + @"\b", "EE" + (i + 32).ToString() + "i");                    

            //        res.vars.EmbeddedVars.GetVarByName("X" + i + "i").Name = "EE" + (i + 40).ToString() + "i";
            //        file = Regex.Replace(file, @"\b" + "X" + i + "i" + @"\b", "EE" + (i + 40).ToString() + "i");
            //        pult = Regex.Replace(pult, @"\b" + "X" + i + "i" + @"\b", "EE" + (i + 40).ToString() + "i");

            //        res.vars.EmbeddedVars.GetVarByName("Y" + i + "i").Name = "EE" + (i + 48).ToString() + "i";
            //        file = Regex.Replace(file, @"\b" + "Y" + i + "i" + @"\b", "EE" + (i + 48).ToString() + "i");
            //        pult = Regex.Replace(pult, @"\b" + "Y" + i + "i" + @"\b", "EE" + (i + 48).ToString() + "i");  

            //        res.vars.EmbeddedVars.GetVarByName("Z" + i + "i").Name = "EE" + (i + 56).ToString() + "i";
            //        file = Regex.Replace(file, @"\b" + "Z" + i + "i" + @"\b", "EE" + (i + 56).ToString() + "i");
            //        pult = Regex.Replace(pult, @"\b" + "Z" + i + "i" + @"\b", "EE" + (i + 56).ToString() + "i"); 
            //    }
            //    for (int i = 0; i < 16; i = i + 4)
            //    {
            //        res.vars.EmbeddedVars.GetVarByName("W" + i + "l").Name = "EE" + (i + 16).ToString() + "l";
            //        file = Regex.Replace(file, @"\b" + "W" + i + "l" + @"\b", "EE" + (i + 16).ToString() + "l");
            //        pult = Regex.Replace(pult, @"\b" + "W" + i + "l" + @"\b", "EE" + (i + 16).ToString() + "l"); 

            //        res.vars.EmbeddedVars.GetVarByName("X" + i + "l").Name = "EE" + (i + 20).ToString() + "l";
            //        file = Regex.Replace(file, @"\b" + "X" + i + "l" + @"\b", "EE" + (i + 20).ToString() + "l");
            //        pult = Regex.Replace(pult, @"\b" + "W" + i + "l" + @"\b", "EE" + (i + 16).ToString() + "l"); 

            //        res.vars.EmbeddedVars.GetVarByName("Y" + i + "l").Name = "EE" + (i + 24).ToString() + "l";
            //        file = Regex.Replace(file, @"\b" + "Y" + i + "l" + @"\b", "EE" + (i + 24).ToString() + "l");
            //        pult = Regex.Replace(pult, @"\b" + "W" + i + "l" + @"\b", "EE" + (i + 16).ToString() + "l"); 
                   
            //        res.vars.EmbeddedVars.GetVarByName("Z" + i + "l").Name = "EE" + (i + 28).ToString() + "l";
            //        file = Regex.Replace(file, @"\b" + "Z" + i + "l" + @"\b", "EE" + (i + 28).ToString() + "l");
            //        pult = Regex.Replace(pult, @"\b" + "W" + i + "l" + @"\b", "EE" + (i + 16).ToString() + "l"); 

            //    }

            //    File.WriteAllText(res.ProgramFileName, file, Encoding.Default);
            //    File.WriteAllText(res.PultFileName, pult,  Encoding.Unicode);

            //    int wxyAddress = 0;
            //    for (int i = 0; i < 4; i++)
            //    {
            //        for (int j = 0; j < 64; j++)
            //        {
            //            if (res.vars.EmbeddedVars.GetVarByName("EE" + (i * 64 + j).ToString()) == null)
            //                res.vars.EmbeddedVars.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 64 + j).ToString(), Size = 1, Memory = MemoryType.XRAM, Value = 255, Address = wxyAddress });

            //            if (j % 2 == 0 && res.vars.EmbeddedVars.GetVarByName("EE" + (i * 64 + j).ToString() + "i") == null)                            
            //                res.vars.EmbeddedVars.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 64 + j) + "i", Size = 2, Memory = MemoryType.XRAM, Value = 0xFFFF, Address = wxyAddress });

            //            if (j % 4 == 0 && res.vars.EmbeddedVars.GetVarByName("EE" + (i * 64 + j).ToString() + "l") == null)                            
            //                res.vars.EmbeddedVars.Add(new ControllerEmbeddedVar() { Name = "EE" + (i * 64 + j) + "l", Size = 4, Memory = MemoryType.XRAM, Value = 0xFFFFFFFF, Address = wxyAddress });                            
            //            wxyAddress++;
            //        }
            //    }
            //}


            if(is50)
                res.ComputeMultibyteEmbeddedVarsValues();
            //if (res is MB90F347Solution)
            //{
            //    ((MB90F347Solution)res).Uarts[2].BufferSize = 0x80;
            //}
            return res;
        }
        /// <summary>
        /// ��������� ���� ������� �� ����� � ������� 5.0.1
        /// </summary>
        private static ControllerProgramSolution From501File(string SolutionFileName)
        {
            XPathDocument xpDoc = new XPathDocument(SolutionFileName);
            XPathNavigator xpNav = ((IXPathNavigable)xpDoc).CreateNavigator();
            // ���������� ����� ���������� �������
            XPathNodeIterator xpList = xpNav.Select("/ControllerProgramSolution");
            xpList.MoveNext();
            ControllerProgramSolution res = ControllerProgramSolution.Create(xpList.Current.OuterXml.Contains("AT89C51ED2Solution") ? ProcessorType.AT89C51ED2 : ProcessorType.MB90F347);
            res.ControllerAddress = int.Parse(xpList.Current.GetAttribute("ControllerAddress", ""));
            int SearchedControllerAddress = 0;
            if (int.TryParse(xpList.Current.GetAttribute("SearchedControllerAddres", ""), out SearchedControllerAddress))
                res.SearchedControllerAddress = SearchedControllerAddress;
            res.fileName = SolutionFileName;
            res.ID = Guid.NewGuid();
            //if (res is AT89C51ED2Solution)
            //{
            //    ((AT89C51ED2Solution)res).BaudRate = int.Parse(xpList.Current.GetAttribute("BaudRate", ""));
            //    if(xpList.Current.GetAttribute("Protocol", "") != "")
            //        ((AT89C51ED2Solution)res).Protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), xpList.Current.GetAttribute("Protocol", ""));
            //    ((AT89C51ED2Solution)res).ReadPassword = xpList.Current.GetAttribute("ReadPassword", "");
            //    ((AT89C51ED2Solution)res).WritePassword = xpList.Current.GetAttribute("WritePassword", "");
            //}
            // ���������� ��������� ����� �������
            xpList = xpNav.Select("/ControllerProgramSolution/ActiveFileName");
            xpList.MoveNext();
            res.ActiveFileName = xpList.Current.Value;
            // ���������� ����� ������ �������
            //xpList = xpNav.Select("/ControllerProgramSolution/IsNewSolution");
            //xpList.MoveNext();
            //res.IsNewSolution = bool.Parse(xpList.Current.Value);
            res.IsNewSolution = false;
            // ���������� ������ ������ �������
            xpList = xpNav.Select("/ControllerProgramSolution/Files/string");
            while (xpList.MoveNext())
            {
                res.Files.Add(xpList.Current.Value);
            }
            // ���������� ������ �������� ������ �������
            xpList = xpNav.Select("/ControllerProgramSolution/OpenedFiles/string");
            while (xpList.MoveNext())
            {
                res.OpenedFiles.Add(xpList.Current.Value);
            }
            // ���������� ������ ���������� �������
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
            // ���������� ����� ��������� �������
            xpList = xpNav.Select("/ControllerProgramSolution/ProgramFileName");
            xpList.MoveNext();
            res.ProgramFileName = xpList.Current.Value;
            // ���������� ����� ������� �������
            xpList = xpNav.Select("/ControllerProgramSolution/PultFileName");
            xpList.MoveNext();
            res.PultFileName = xpList.Current.Value;
            //if (res is MB90F347Solution)
            //{
            //    // ���������� ����� ������������� LCD-������
            //    xpList = xpNav.Select("/ControllerProgramSolution/UseLCDPanel");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).UseLCDPanel = bool.Parse(xpList.Current.Value);
            //    // ���������� ���������� UART
            //    xpList = xpNav.Select("/ControllerProgramSolution/Uarts/UartOptions");
            //    int i = 0;
            //    while (xpList.MoveNext())
            //    {
            //        // ���������� ���� ���������
            //        XPathNodeIterator xpUart = xpList.Current.Select("Protocol");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].Protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), xpUart.Current.Value);
            //        // ���������� ��������
            //        xpUart = xpList.Current.Select("BaudRate");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].BaudRate = int.Parse(xpUart.Current.Value);
            //        // ���������� ������ �� ������
            //        xpUart = xpList.Current.Select("ReadPassword");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].ReadPassword = xpUart.Current.Value;
            //        // ���������� ������ �� ������
            //        xpUart = xpList.Current.Select("WritePassword");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].WritePassword = xpUart.Current.Value;
            //        // ���������� ������� ������ ������
            //        xpUart = xpList.Current.Select("ReadBufferSize");
            //        xpUart.MoveNext();
            //        ((MB90F347Solution)res).Uarts[i].BufferSize = int.Parse(xpUart.Current.Value);
            //    }
            //    // ���������� �������� ���������� 1
            //    xpList = xpNav.Select("/ControllerProgramSolution/DispetcherPhone");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).DispatcherPhone1 = xpList.Current.Value;
            //    // ���������� �������� ���������� 2
            //    xpList = xpNav.Select("/ControllerProgramSolution/DispetcherPhone2");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).DispatcherPhone2 = xpList.Current.Value;
            //    // ���������� �������� ���������� 3
            //    xpList = xpNav.Select("/ControllerProgramSolution/DispetcherPhone3");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).DispatcherPhone3 = xpList.Current.Value;
            //    // ���������� �������� sms 1
            //    xpList = xpNav.Select("/ControllerProgramSolution/SmsPhone1");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).SmsPhone1 = xpList.Current.Value;
            //    // ���������� �������� sms 2
            //    xpList = xpNav.Select("/ControllerProgramSolution/SmsPhone2");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).SmsPhone2 = xpList.Current.Value;
            //    // ���������� �������� sms 3
            //    xpList = xpNav.Select("/ControllerProgramSolution/SmsPhone3");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).SmsPhone3 = xpList.Current.Value;
            //    // ���������� ������ ������������� ������
            //    xpList = xpNav.Select("/ControllerProgramSolution/ModemInitializationString");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).ModemInitializationString = xpList.Current.Value;
            //    // ���������� ����� ���������� ���������������� �� ���������
            //    xpList = xpNav.Select("/ControllerProgramSolution/DenyProgrammingThrowProtocol");
            //    xpList.MoveNext();
            //    ((MB90F347Solution)res).DenyProgrammingThrowProtocol = bool.Parse(xpList.Current.Value);
            //}
            return res;
        }
        /// <summary>
        /// ����������� ���� ������� �� ������� 5.0 � ��������
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
        /// ���������� ����� ������, ���������� ����� ��������� ���������� ���������� ������� �� ������
        /// </summary>
        //public static ControllerProgramSolution ChangeSolutionProcessor(ControllerProgramSolution solution, ProcessorType ProcessorType)
        //{
        //    solution.SaveSolutionBackup();
        //    ControllerProgramSolution res = null;
        //    if (solution is AT89C51ED2Solution)
        //    {
        //        if (ProcessorType == ProcessorType.MB90F347)
        //            res = ControllerProgramSolution.ConvertAT89C51ED2toMB90F347(solution as AT89C51ED2Solution);
        //        else
        //            throw new Exception("���������� ������������� ������ ��� ��������� ��� ���������");
        //    }
        //    if (solution is MB90F347Solution)
        //        throw new Exception("���������� ������������� ������ ��� ��������� ��� ���������");
        //    return res;
        //}
        /// <summary>
        /// ����������� ������ ��� ��������� AT89C51ED2 � ������ ��� ��������� MB90F347 
        /// </summary>
        //private static MB90F347Solution ConvertAT89C51ED2toMB90F347(AT89C51ED2Solution solution)
        //{
        //    MB90F347Solution res = (MB90F347Solution)Create(typeof(MB90F347Solution));
        //    res.SolutionFileName = solution.fileName;
        //    res.programFileName = solution.programFileName;
        //    res.pultFileName = solution.pultFileName;
        //    //res.fbdFileName = solution.fbdFileName;
        //    //new FbdEditor().Save(res.fbdFileName);
        //    res.ActiveFileName = solution.ActiveFileName;

        //    //res.OpenedFiles.Add(res.fbdFileName);
        //    //foreach (string FileName in solution.OpenedFiles)
        //    //{
        //    //    res.OpenedFiles.Add(FileName);
        //    //}

        //    //res.Files.Add(res.fbdFileName);
        //    foreach (string FileName in solution.Files)
        //    {
        //        if (Path.GetExtension(FileName) == ".plt")
        //        {
        //            RelkonPultModel model = RelkonPultModel.FromFile(FileName);
        //            model.ChangePultType(res.PultParams.DefaultPultType);
        //            model.Save(FileName);
        //        }
        //        string s = Path.GetFileName(FileName).ToLower();
        //        if (!(s == "fc_u.c" || s == "flash.map" || s == "pult.asm" || s == "eepred2.asm"))
        //            res.Files.Add(FileName);
        //        else
        //            res.OpenedFiles.Remove(FileName);
        //    }
            
        //    foreach (MB90F347Solution.UartOptions uart in res.Uarts)
        //    {
        //        uart.Protocol = solution.Protocol;
        //        uart.BaudRate = solution.BaudRate;
        //        uart.ReadPassword = solution.ReadPassword;
        //        uart.WritePassword = solution.WritePassword;
        //    }
        //    res.ControllerAddress = solution.ControllerAddress;
        //    res.SearchedControllerAddress = solution.SearchedControllerAddress;
        //    if(res.ProgramFileName!="")
        //        res.ConvertProgram(res.programFileName);
        //    res.vars.EmbeddedVars.Clear();
        //    res.vars.EmbeddedVars.AddRange(solution.vars.EmbeddedVars);
        //    return res;
        //}
        /// <summary>
        /// ������� ��������� ������ ���������� ����
        /// </summary>
        private static ControllerProgramSolution Create(Type type)
        {
            ControllerProgramSolution res = null;          
            if (type == typeof(STM32F107Solution))
                res = new STM32F107Solution();
            else
                throw new Exception("������� ���� " + type + " �� �������������");
            res.Vars.SystemVars.AddRange(res.GetSystemVarsList());
            res.Vars.IOVars.AddRange(res.GetDefaultIOVarsList());
            res.Vars.EmbeddedVars.AddRange(res.GetEmbeddedVarsList());
            res.Vars.DispatcheringVars.AddRange(res.GetDispatcheringVarsList());
            return res;
        }
        /// <summary>
        /// ������� ��������� ������ ���������� ����
        /// </summary>
        public static ControllerProgramSolution Create(ProcessorType processor)
        {
            ControllerProgramSolution res = null;
            switch (processor)
            {
                //case ProcessorType.AT89C51ED2:
                //    res = Create(typeof(AT89C51ED2Solution));
                //    break;
                //case ProcessorType.MB90F347:
                //    res = Create(typeof(MB90F347Solution));
                //    break;
                case ProcessorType.STM32F107:
                    res = Create(typeof(STM32F107Solution));
                    break;
                default:
                    throw new Exception("���������� ���� " + processor + " �� ��������������");
            }
            return res;
        }
        /// <summary>
        /// ��������� ������ ������� �� rpj-����� 
        /// </summary>
        public static ControllerProgramSolution FromRelkon4PrjFile(string FileName)
        {
            XPathDocument xpDoc = new XPathDocument(FileName);
            XPathNavigator xpNav = ((IXPathNavigable)xpDoc).CreateNavigator();
            XPathNodeIterator xpList = xpNav.Select("/Project");
            xpList.MoveNext();
            string SolutionName = xpList.Current.GetAttribute("projectName", "");
            ControllerProgramSolution res = ControllerProgramSolution.Create((int.Parse(xpList.Current.GetAttribute("processor", "")) == 2) ? ProcessorType.AT89C51ED2 : ProcessorType.MB90F347);
            res.SolutionFileName = SolutionName;
            xpList = xpNav.Select("/Project/var");
            while (xpList.MoveNext())
            {
                string name = xpList.Current.GetAttribute("name", "");
                if (res.vars.GetSystemVar(name) != null || res.vars.GetEmbeddedVar(name) != null)
                    continue;
                ControllerUserVar var = new ControllerUserVar();
                var.Name = name;
                var.Address = Convert.ToInt32(xpList.Current.GetAttribute("address", ""), 16);
                var.Memory = (int.Parse(xpList.Current.GetAttribute("memoryType", "")) == 0) ? MemoryType.RAM : MemoryType.XRAM;
                var.Size = int.Parse(xpList.Current.GetAttribute("size", ""));
                res.vars.Add(var);
            }
            return res;
        }

        #endregion

        #region Override methods
        /// <summary>
        /// �������� ��� ���������� ����� �������
        /// </summary>
        /// <param name="OldFileName">������ ��� �����</param>
        /// <param name="NewFileName">����� ��� �����</param>
        public override void ChangeCustomFileName(string OldFileName, string NewFileName)
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

        public override void SaveAs(string FileName)
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
        /// �������� ���� ���� ������ ������� �� ���������
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
        /// ��������� ����� ������� ����� ������ ���������
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
                Utils.ErrorMessage("������ �������� ��������� ����� �������: " + Utils.FirstLetterToLower(ex.Message));
            }
        }
        /// <summary>
        /// ����������� ������ ���������� �� ��� ������� ��� LCD-�������
        /// </summary>
        public void ConvertAllLCDPanelProjects()
        {
            foreach (string FileName in this.Files)
            {
                if (Path.GetExtension(FileName) != ".epj")
                    continue;
                this.ConvertLCDPanelProject(FileName, this.vars);
            }
        }
        /// <summary>
        /// ����������� ������ ���������� � ������ ��� LCD-������
        /// </summary>
        public void ConvertLCDPanelProject(string FileName, ControllerVarCollection Vars)
        {
            this.RaisedOutputDataReceivedEvent("����������� ������� " + Path.GetExtension(FileName) + "...");
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
                        this.compilationParams.Errors.Add(new CompilationError("���������� " + VarName + " ���������� � �������", FileName, -1, true));
                        continue;
                    }
                }
                int BitNumber = (m.Groups[2].Value != "") ? int.Parse(m.Groups[2].Value) : -1;
                if (BitNumber > 15)
                {
                    this.compilationParams.Errors.Add(new CompilationError("���������� " + VarName + "." + m.Groups[2].Value + ": �������� ����� ���� (������ ���� �� 0 �� 15)", FileName, -1, true));
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
                        this.compilationParams.Errors.Add(new CompilationError("���������� " + VarName + ": ����������� ������ ����������� ����������", FileName, -1, true));
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
        /// �� ������ ������ ������ � fc_u.c ���������� ����� ������
        /// � ����� ��������� Relkon
        /// </summary>
        /// <param name="FCLineNumber">����� ������ � ����� fc_u.c</param>
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
        /// ���������� ������� OutputDataReceived
        /// </summary>
        protected void RaisedOutputDataReceivedEvent(string Data)
        {
            if (this.OutputDataReceived != null)
                this.OutputDataReceived(this, new RelkonDataReceivedEventArgs(Data));
        }
        /// <summary>
        /// ��������� ������ ���� � ������ ���������������� ���������� �������  �� ������ ����� ��������� �������
        /// </summary>
        public void UpdateCodeModel()
        {
            this.codeModel = RelkonCodeModel.ParseFromFile(this.programFileName, true);
            this.FillUserVarsFromCodeModel(this.codeModel);
        }
        /// <summary>
        /// ��������� ������ ���� � ������ ���������������� ���������� ������� �� ������ ���������� ���� ���������
        /// </summary>
        public void UpdateCodeModel(string code)
        {
            this.codeModel = RelkonCodeModel.ParseFromCode(code);
            this.FillUserVarsFromCodeModel(this.codeModel);
        }
        /// <summary>
        /// �������� �������� ��������� ��������� �� ������ � ��������� ����� ������ ������;
        /// � ������ ������� - ������� ��������� �� �������
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
                this.RaisedOutputDataReceivedEvent("�� ������� �������� ������ � ���� ������: " + Utils.FirstLetterToLower(ex.Message));
                this.RaisedOutputDataReceivedEvent(Message);
            }
        }
        /// <summary>
        /// ��������� ������ �� ������� �������
        /// </summary>
        protected void LoadFromAnotherSolution(ControllerProgramSolution solution)
        {
            if (!(solution.GetType().Equals(this.GetType())))
                throw new Exception("������ ������ ���� ���� " + this.GetType());
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
        /// ��������� �������� ����- � �������������� ���������� ��������� ��������� �� ������
        /// �������� ����������� ���������� ��� ���������� �������
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

        public override void UploadToDevice(bool onlyProgram, bool onlyParams, bool readEmbVars)
        { }

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
    }

    public enum ProcessorType
    {
        AT89C51ED2,
        MB90F347,
        STM32F107
    }
}
