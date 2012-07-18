using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.CodeDom;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using Kontel.Relkon.Classes;
using System.IO;

namespace Kontel.Relkon.Solutions
{
    public sealed class STM32F107CodeGenerator : CodeGenerator
    {

        public STM32F107CodeGenerator(ControllerProgramSolution Solution)
            : base(Solution)
        {
            //this.availablePeriods = this.GetAvailablePeriodsList(); // периоды в массиве обязательно должны идти по убыванию, т.к. это используется в методе ComputeDispetcherPeriodAndPeriodsList()
            //this.ComputeDispetcherPeriodAndPeriodsList();
        }        

        protected override bool ValidateSituationPeriod(RelkonCodeSituation Situation)
        {           
            return true;
        }

        public override uint MaxDelay
        {
            get
            {
                return (uint) 60000;
            }
        }
        
        public override void GeneratePultCode()
        {
            StringBuilder code = new StringBuilder();
            IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(code));
            writer.WriteLine("/***********PultDataCI***************/");
            for (int i = 0; i < this.pultModel.Rows.Count; i++)
            {
                writer.WriteLine("const unsigned char str" + (i + 1).ToString() + "[][" + this.pultModel.Type.SymbolsInRow + "] = {");
                writer.Indent++;
                foreach (View view in this.pultModel.Rows[i].Views)
                {
                    if (!view.Enabled)
                        continue;
                    writer.WriteLine("\"" + this.ConvertStringToSTM32F107Format(view) + "\",");
                }

                if (this.pultModel.Rows[i].Views.Count > 0)
                {
                    View view = this.pultModel.Rows[i].Views[this.pultModel.Rows[i].Views.Count - 1];
                    writer.WriteLine("\"" + this.ConvertStringToSTM32F107Format(view) + "\"");
                }
                writer.Indent--;
                writer.WriteLine("};");
            }


            writer.WriteLine("void print_var(void)");
            writer.WriteLine("{");
            writer.Indent++;
            for (int i = 1; i < this.pultModel.Rows.Count + 1; i++)
            {                
                writer.WriteLine("switch(_Sys.S" + i + ")");
                writer.WriteLine("{");
                writer.Indent++;
                for (int j = 0; j < this.pultModel.Rows[i - 1].EnabledViews.Count; j++)
                {
                    View view = this.pultModel.Rows[i - 1].EnabledViews[j];
                    if (view.Vars.Count == 0)
                        continue;
                    view.Vars.Sort(PultVar.CompareByPosition);
                    writer.WriteLine("case " + j + ":");
                    writer.Indent++;
                    foreach (PultVar v in view.Vars)
                    {
                        int afterDot = Math.Min(3, GetNumberOfDigitsAfterDotInMask(v.Mask));
                        int s = afterDot == 0 ? v.Mask.Length : v.Mask.Length - 1;
                        s = Math.Min(7, s);
                        RelkonCodeVarDefenition var = solution.GetRelkonCodeVarDefenitionByName(v.Name);
                        if (var != null)
                        {                                                      
                            if (v.ReadOnly)
                            {
                                if (var.Type == "float")
                                {
                                    writer.WriteLine("print_float(" + v.Name + ", " + i + ", " + (v.Position + 1).ToString() + ", "
                                                    + s + ", " + afterDot + ");");
                                }
                                else
                                    writer.WriteLine("print_long((long)" + v.Name + ", " + i + ", " + (v.Position + 1).ToString() + ", "
                                                    + s
                                                    + ", " + afterDot + ");");
                            }
                            else
                            {
                                writer.WriteLine("print_edit((void*)&" + var.Name + ", " + i
                                                                       + ", " + ++v.Position + ", "
                                                                       + s
                                                                       + ", " + afterDot + ", 0x" + AppliedMath.DecToHex(GetTypeCodeOfVar(var, v.HasSign)) + ");");
                            }                            
                        }
                        else
                        {
                            if (v.Name == "HOUR" ||
                                v.Name == "MIN" ||
                                v.Name == "SEC" ||
                                v.Name == "DATE" ||
                                v.Name == "MONTH" ||
                                v.Name == "YEAR")
                            {
                                if (v.ReadOnly)
                                    writer.WriteLine("print_long((long)" + "times." + v.Name.ToLower() + ", " + i + ", " + (v.Position + 1).ToString() + ", "
                                                   + s
                                                   + ", " + afterDot + ");");                                 
                                else
                                    writer.WriteLine("print_time(" + i + "," + (v.Position + 1).ToString() + "," + v.Name + "_TYPE);");
                                continue;
                            }
                            Match m = Regex.Match(v.Name, @"\bEE(\d+)");
                            if (m.Success)
                            {
                                if (v.ReadOnly)
                                    writer.WriteLine("print_long((long)" + v.Name + ", " + i + ", " + (v.Position + 1).ToString() + ", "
                                                   + s
                                                   + ", " + afterDot + ");");
                                else
                                    writer.WriteLine("print_edit_ee(" + int.Parse(m.Groups[1].Value) + ", " + i
                                                                       + ", " + ++v.Position + ", "
                                                                       + s
                                                                       + ", " + afterDot + ", 0x" + AppliedMath.DecToHex(GetTypeCodeOfEEVar(v.Name, v.HasSign)) + ");");
                                continue;
                            }

                            for (int k = 1; k < 9; k++)
                            {
                                v.Name = Regex.Replace(v.Name, @"\bADC" + k + "\b", "_Sys_ADC[" + (k - 1).ToString() + "]");
                                //Match m = Regex.Match(v.Name, );
                                //v.Name = v.Name.Replace("ADC" + k, "_Sys_ADC[" + (k - 1).ToString() + "]");
                            }

                            if (v.ReadOnly)
                            {
                                writer.WriteLine("print_long((long)" + v.Name + ", " + i + ", " + (v.Position + 1).ToString() + ", "
                                                   + (afterDot == 0 ? v.Mask.Length : v.Mask.Length - 1).ToString()
                                                   + ", " + afterDot + ");");                                
                            }
                            else 
                            {
                                if (v.Name.Contains("ADC") || v.Name.Contains("DAC"))
                                {
                                    writer.WriteLine("print_edit((void*)&" + v.Name + ", " + i
                                                                          + ", " + ++v.Position + ", "
                                                                          + (afterDot == 0 ? v.Mask.Length : v.Mask.Length - 1).ToString()
                                                                          + ", " + afterDot + ", 0x02);");
                                }
                                else
                                {
                                    writer.WriteLine("print_edit((void*)&" + v.Name + ", " + i
                                                                          + ", " + ++v.Position + ", "
                                                                          + (afterDot == 0 ? v.Mask.Length : v.Mask.Length - 1).ToString()
                                                                          + ", " + afterDot + ", 0x01);");
                                }
                            }


                        }
                    }
                    writer.WriteLine("break;");
                    writer.Indent--;
                }
                writer.WriteLine("default: break;");
                writer.Indent--;
                writer.WriteLine("}");               
            }
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.Close();
            try
            {
                using (StreamWriter wr = new StreamWriter(this.solution.DirectoryName + "\\fc_u.c", true, Encoding.Default))
                {
                    wr.Write(code.ToString());
                }
            }
            catch (Exception ex)
            {
                this.Errors.Add(new Kontel.Relkon.Classes.CompilationError(ex.Message, this.solution.DirectoryName + "\\fc_u.c", -1, false));
            }
        }

        private int GetNumberOfDigitsAfterDotInMask(string mask)
        {
            int res = 0;
            bool dotFinded = false;
            for (int i = 0; i < mask.Length; i++)
            {
                if (!dotFinded)
                {
                    if (mask[i] == '.' || mask[i] == ',')
                        dotFinded = true;
                }
                else                
                    res++;                                
            }
            return res;
        }

        private int GetTypeCodeOfVar(RelkonCodeVarDefenition var, bool signed)
        {
            int type = 0x83;
            switch (var.Type)
            {
                case "char":
                    type = signed ? 0x81 : 0x01;    
                    break;
                case "short":
                    type = signed ? 0x82 : 0x02;    
                    break;
                case "int":
                    type = signed ? 0x83 : 0x03; 
                    break;              
                case "long":
                    type = signed ? 0x83 : 0x03;
                    break;
            }

            return type;
        }

        private int GetTypeCodeOfEEVar(string ee_var, bool signed)
        {
            int type;
            string lst = ee_var.Substring(ee_var.Length - 1, 1);
            if (lst == "l")            
                type = signed ? 0x83 : 0x03;            
            else if (lst == "i")            
                type = signed ? 0x82 : 0x02;            
            else             
                type = signed ? 0x81 : 0x01;            

            return type;
        }

        protected override void GenerateInitFunction(IndentedTextWriter CodeWriter)
        {
            CodeWriter.WriteLine("void Relkon_init()");
            CodeWriter.WriteLine("{");
            CodeWriter.Indent++;
            //for (int i = 0; i < 4; i++)
            //    CodeWriter.WriteLine("_Sys4x_IN" + i + " = (struct iobit*)&IN" + i + ";\r\n");
            //for (int i = 0; i < 4; i++)
            //    CodeWriter.WriteLine("_Sys4x_OUT" + i + " = (struct iobit*)&OUT" + i + ";\r\n");
            //CodeWriter.WriteLine("_Sys4x_disp_counter=0;\r\n");


            CodeWriter.Write(this.TranslateCodeFragment(this.codeModel.InitFunction.Code));
            CodeWriter.Indent--;
            CodeWriter.WriteLine("}");
                     
        }           

        protected override void GenerateHeader(IndentedTextWriter CodeWriter)
        {
            CodeWriter.WriteLine("#include \"fc_u.h\"");
            CodeWriter.WriteLine("#include \"additional.h\"\n");                     
            CodeWriter.WriteLine("extern unsigned long sd_fl;");

            CodeWriter.WriteLine("//");
                               
            int c = 0;
            for (int i = 0; i < this.pultModel.Rows[this.pultModel.Rows.Count - 1].Views.Count; i++)            
                if (this.pultModel.Rows[this.pultModel.Rows.Count - 1].Views[i].Enabled)
                    c++;

            CodeWriter.WriteLine(string.Format("const unsigned short S4_max = {0};", c.ToString()));
            CodeWriter.WriteLine("//");

            CodeWriter.WriteLine(GenerateModulesHeader());
            CodeWriter.WriteLine("//");
        }

        protected override void GenerateDefines(IndentedTextWriter CodeWriter)
        {
            foreach (RelkonCodeDefine define in this.codeModel.Defines)
            {
                Match m = Regex.Match(define.Value, @"\(\s*\*\s*\(\s*\(([^\*]*)\s*\*\s*\)\s*(0x6[0-9A-F])\s*\)\s*\)", RegexOptions.IgnoreCase);
                if (!m.Success)
                {
                    // Преобразовываем определения цифровых датчиков
                    StringBuilder sb = new StringBuilder(define.Value);
                    this.ReplaceIO(sb);
                    CodeWriter.WriteLine("/*" + define.LineNumber + "*/#define " + define.Name + " " + sb.ToString());
                }
                else
                {
                    // замена #define ramADR (*((unsigned char *)0x0000))
                    string s = "/*" + define.LineNumber + "*/" + "#define " + define.Name + " (*((" + m.Groups[1].Value + " *)(" + m.Groups[2].Value + " + 0x250)" + "))";
                    CodeWriter.WriteLine(s);
                }
            }
        }

        protected override void GenerateProcessStructDefenition(IndentedTextWriter CodeWriter)
        {
            CodeWriter.WriteLine("struct process");
            CodeWriter.WriteLine("{");
            CodeWriter.Indent++;
            CodeWriter.WriteLine("unsigned long DELAY;");
            CodeWriter.WriteLine("unsigned int SIT;");
            CodeWriter.Indent--;
            CodeWriter.Write("}");            
        }


        public override void GenerateProgramCode()
        {
            base.GenerateProgramCode();
            if (this.codeModel.IOModules.Count > 31)
                this.Errors.Add(new CompilationError("На данный момент допускается использование не более 31 модуля ввода-вывода", this.codeModel.FileName, -1, false));           
        }

        /// <summary>
        /// Генерирует h-файл с определениями модулей
        /// </summary>
        private string GenerateModulesHeader()
        {          
            int count = 0;

            
            int[] groupesReqTime = new int[8];

            List<IOModule>[] groupes = new List<IOModule>[8];

            List<IOModule> Req1msMod = new List<IOModule>();
            List<IOModule> Req5msMod = new List<IOModule>();
            List<IOModule> Req10msMod = new List<IOModule>();
            List<IOModule> Req100msMod = new List<IOModule>();

            for (int i = 0; i < 8; i++)            
                groupes[i] = new List<IOModule>();
            

            foreach (IOModule mod in this.codeModel.IOModules.Values)
            {
                switch (mod.Period)
                {
                    case 1:
                        Req1msMod.Add(mod);		            
                        break;
                    case 5:
                        Req5msMod.Add(mod);		            
                        break;
                    case 10:
                        Req10msMod.Add(mod);		            
                        break;
                    case 100:
                        Req100msMod.Add(mod);		            
                        break;
                }
            }

            foreach (IOModule mod in Req1msMod)            
                for (int i = 0; i < groupes.Length; i++)                
                    groupes[i].Add(mod);

            count = 0;
            foreach (IOModule mod in Req5msMod)
            {
                if (count == 0)
                    for (int i = 0; i < 8; i = i + 2)
                        groupes[i].Add(mod);
                else
                    for (int i = 1; i < 8; i = i + 2)
                        groupes[i].Add(mod);
                count++;
                if (count == 2) count = 0;  
            }

            count = 0;
            foreach (IOModule mod in Req10msMod)
            {
                groupes[count].Add(mod);
                groupes[count + 4].Add(mod);
                count++;
                if (count == 4) count = 0; 
            }

            count = 0;            
            foreach (IOModule mod in Req100msMod)
            {                             
                groupes[count].Add(mod);
                count++;
                if (count == 8) count = 0;
            }

            for (int i = 0; i < groupes.Length; i++)
            {
                double totalReqTime = 0.2;
                double totalRepTime = 2;

                foreach (IOModule m in groupes[i])
                {
                    totalReqTime += m.RequestTime;
                    totalRepTime += m.ReplyTime;
                }
                int t1 = totalReqTime > (int)totalReqTime ? ((int)totalReqTime) + 1 : (int)totalReqTime;
                int t2 = totalRepTime > (int)totalRepTime ? ((int)totalRepTime) + 1 : (int)totalRepTime;
                groupesReqTime[i] = t1 + t2;
            }

            int d = MaxValueInArray(groupesReqTime);

            foreach (IOModule mod in Req1msMod)            
                mod.RealPeriod = d;

            int[] p = new int[4];

            p[0] = groupesReqTime[0] + groupesReqTime[1];
            p[1] = groupesReqTime[2] + groupesReqTime[3];
            p[2] = groupesReqTime[4] + groupesReqTime[5];
            p[3] = groupesReqTime[6] + groupesReqTime[7];

            d = MaxValueInArray(p);

            foreach (IOModule mod in Req5msMod)
                mod.RealPeriod = d;

            p = new int[2];

            p[0] = groupesReqTime[0] + groupesReqTime[1] + groupesReqTime[2] + groupesReqTime[3];
            p[1] = groupesReqTime[4] + groupesReqTime[5] + groupesReqTime[6] + groupesReqTime[7];

            d = MaxValueInArray(p);

            foreach (IOModule mod in Req10msMod)
                mod.RealPeriod = d;

            d = 0;
            for (int i = 0; i < 8; i++)            
                d += groupesReqTime[i];

            foreach (IOModule mod in Req100msMod)
                mod.RealPeriod = d;

            StringBuilder modulesh = new StringBuilder();
            StringBuilder iodefinesh = new StringBuilder(); 
 
            StringBuilder DIODefines = new StringBuilder();
            StringBuilder AIODefines = new StringBuilder();        

            foreach (IOModule module in this.codeModel.IOModules.Values)
            {
                if (module is DigitalModule)
                {                   
                    DIODefines.AppendLine("#define " + module.VarNames[0].DisplayName + " " + module.VarNames[0].SystemName);
                }
                else
                {
                    AnalogModule amodule = (AnalogModule)module;
                    for (int i = 0; i < amodule.VarNames.Length; i++)
                    {                       
                        AIODefines.AppendLine("#define " + amodule.VarNames[i].DisplayName + " " + amodule.VarNames[i].SystemName);
                        AIODefines.AppendLine("#define " + amodule.SingleByteVarNames[i].DisplayName + " (" + amodule.VarNames[i].SystemName + ">>8)");
                    }
                }
            }

            iodefinesh.AppendLine(DIODefines.ToString());
            iodefinesh.AppendLine(AIODefines.ToString());

            modulesh.AppendLine("const unsigned char mod_table[]={");            
            for (int i = 0; i < groupes.Length; i++)
            {
                if (groupes[i].Count != 0)
                {
                    foreach (IOModule mod in groupes[i])
                    {
                        modulesh.Append("0x" + Utils.AddChars('0', AppliedMath.DecToHex(mod.ModuleNumber), 2) + ", ");
                    }
                    modulesh.AppendLine("0x00,");                   
                }               
            }
            modulesh.Append("0x00");
            modulesh.AppendLine("};");
                      
            try
            {
                File.WriteAllText(this.solution.DirectoryName + "\\iodefines.h", iodefinesh.ToString());
            }
            catch (Exception ex)
            {
                this.Errors.Add(new CompilationError(ex.Message, this.codeModel.FileName, -1, false));
            }

            return modulesh.ToString();
        }


        private int MaxValueInArray(int[] arr)
        {
            int res = arr[0];
            for (int i = 1; i < arr.Length; i++)            
                if (arr[i] > res)
                    res = arr[i];
            return res;
        }

        protected override string GetDINDefinition(string ByteNumber, string BitNumber)
        {
            int bn = int.Parse(ByteNumber);            
            if (bn >= DINModule.MaxIndex)
                throw new Exception(string.Format("Неверная переменная. Переменные внешнего цифрового модуля принимают диапазон IN{0} - IN{1}", DINModule.MinIndex, DINModule.MaxIndex));
            if (bn >= DigitalModule.MinIndex && int.Parse(BitNumber) >= 4)
                throw new Exception("Неверная переменная. Внешний цифровой модуль содержит только четыре датчика, нумеруемые с нуля");
            return (bn >= DINModule.MinIndex) ? "(*(iostruct*)&IN[" + (bn - 4) + "]).bit" + BitNumber : "(*(iostruct*)&_Sys_IN[" + ByteNumber + "]).bit" + BitNumber;
        }

        protected override string GetDOUTDefinition(string ByteNumber, string BitNumber)
        {
            int bn = int.Parse(ByteNumber);
            if (bn >= DOUTModule.MaxIndex)
                throw new Exception(string.Format("Неверная переменная. Переменные внешнего цифрового модуля принимают диапазон OUT{0} - OUT{1}", DOUTModule.MinIndex, DOUTModule.MaxIndex));
            if (bn >= DigitalModule.MinIndex && int.Parse(BitNumber) >= 4)
                throw new Exception("Неверная переменная. Внешний цифровой модуль содержит только четыре датчика, нумеруемые с нуля");
            return (bn >= DINModule.MinIndex) ? "(*(iostruct*)&OUT[" + (bn - 4) + "]).bit" + BitNumber : "(*(iostruct*)&_Sys_OUT[" + ByteNumber + "]).bit" + BitNumber;
        }

        protected override string GetSituationName(RelkonCodeProcess Process, RelkonCodeSituation Situation)
        {
            throw new NotImplementedException();
        }
        
        protected override string GetJumpToSituationCode(Kontel.Relkon.CodeDom.RelkonCodeProcess Process, Kontel.Relkon.CodeDom.RelkonCodeSituation Situation, uint Delay)
        {
            string pn = this.GetProcessName(Process);
            return pn + ".SIT=" + Situation.Index + "; " + pn + ".DELAY=" + (Delay == 0 ? Delay : Delay - 1) + "; break";
        }

        protected override string GetRestartSituationCode(Kontel.Relkon.CodeDom.RelkonCodeProcess Process, uint Delay)
        {
            string pn = this.GetProcessName(Process);
            return pn + ".DELAY=" + (Delay == 0 ? Delay : Delay - 1) + "; break";            
        }


        protected override string GetStartProcessCode(Kontel.Relkon.CodeDom.RelkonCodeProcess Process)
        {
            if (Process.Situations.Count == 0)
                return "";
            else
            {
                string pn = this.GetProcessName(Process);
                return pn + ".SIT = 1; " + pn + ".DELAY = 0";
            }
        }

        protected override string GetStopProcessCode(Kontel.Relkon.CodeDom.RelkonCodeProcess Process)
        {
            if (Process.Situations.Count == 0)
                return "";
            else
            {
                string pn = this.GetProcessName(Process);
                return pn + ".SIT = 0;";
            }
        }

        protected override string GetProcessName(RelkonCodeProcess Process)
        {
            return "_Sys4x_p" + Process.Index;
        }   

        protected override void GenerateDispetcher(IndentedTextWriter CodeWriter)
        {                     
            GenerateDelayBlockCode(CodeWriter, 100);
            GenerateDelayBlockCode(CodeWriter, 1);
            GenerateDelayBlockCode(CodeWriter, 5);
            GenerateDelayBlockCode(CodeWriter, 10);
            GenerateDelayBlockCode(CodeWriter, 1000);
        }
       

        protected override void GenerateSituations(IndentedTextWriter CodeWriter)
        {
            //base.GenerateSituations(CodeWriter);
                     

            foreach (RelkonCodeProcess process in this.codeModel.Processes)
            {
                foreach (RelkonCodeSituation situation in process.Situations)
                {
                    
                }
            }
        }

        private void GenerateDelayBlockCode(IndentedTextWriter CodeWriter, int delay)
        { 
            CodeWriter.WriteLine("void R" + delay + "Task( void *pvParameters ) ");
            CodeWriter.WriteLine("{");
            CodeWriter.Indent++;
            CodeWriter.WriteLine("portTickType xLastExecutionTime;");                        
            if (delay == 100)
            {            
                CodeWriter.WriteLine("IWDG_WriteAccessCmd(IWDG_WriteAccess_Enable);");
                CodeWriter.WriteLine("IWDG_SetPrescaler(IWDG_Prescaler_64); // IWDG counter clock: 40KHz(LSI) / 64  (1.6 ms) ");
                CodeWriter.WriteLine("IWDG_SetReload(150); //Set counter reload value to 6 s");
                CodeWriter.WriteLine("IWDG_ReloadCounter();");
                CodeWriter.WriteLine("IWDG_Enable();");
            }
          
            CodeWriter.WriteLine("xLastExecutionTime = xTaskGetTickCount();");
            CodeWriter.WriteLine("for( ;; )");
            CodeWriter.WriteLine("{");
            CodeWriter.Indent++;
            if (delay == 1)
            {
                foreach (RelkonCodeProcess process in this.codeModel.Processes)
                {
                    string pn = this.GetProcessName(process);
                    CodeWriter.WriteLine("if ( " + pn + ".DELAY != 0 )");
                    CodeWriter.Indent++;
                    CodeWriter.WriteLine(pn + ".DELAY" + "--;");
                    CodeWriter.Indent--;
                }
            }
                    
            foreach (RelkonCodeProcess process in this.codeModel.Processes)
            {
                if (process.Periods.Contains(delay))
                {
                    string pn = this.GetProcessName(process);
                    CodeWriter.WriteLine("if ( " + pn + ".DELAY == 0 )");
                    CodeWriter.Indent++;                  
                    CodeWriter.WriteLine("switch (" + pn + ".SIT)");
                    CodeWriter.WriteLine("{");
                    
                    foreach (RelkonCodeSituation situation in process.Situations)
                    {
                        if (situation.Period == delay)
                        {
                            CodeWriter.Indent++;
                            CodeWriter.WriteLine("case " + situation.Index + ':');

                            CodeWriter.Indent++;
                            StringBuilder code = new StringBuilder(situation.Code);
                            this.ReplaceIO(code);
                            this.ReplaceStartStopProcessCommands(code);
                            this.ReplaceJumpSituationCommand(code, process);
                            this.ReplaceRestartSituationCommands(code, process);
                            this.PerfomCustomReplacing(code);
                            CodeWriter.WriteLine(code.ToString());

                            CodeWriter.WriteLine("break;");
                            CodeWriter.Indent -= 2;
                        }
                    }
                   
                    CodeWriter.WriteLine("}");
                    CodeWriter.Indent--;
                }
            }
            if (delay == 100)            
                CodeWriter.WriteLine("IWDG_ReloadCounter();");               
            
            CodeWriter.WriteLine("r" + delay + "++;");
                    
                                      
            CodeWriter.WriteLine("vTaskDelayUntil( &xLastExecutionTime, R" + delay + "_DELAY );");
            CodeWriter.Indent--;
            CodeWriter.WriteLine("}");
            CodeWriter.Indent--;
            CodeWriter.WriteLine("}");                      
        }

        protected override void GenerateProcessesDefenitions(IndentedTextWriter CodeWriter)
        {
            base.GenerateProcessesDefenitions(CodeWriter);
        }

        protected override void GenerateSituationPrototypes(IndentedTextWriter CodeWriter)
        {
            //base.GenerateSituationPrototypes(CodeWriter);
        }        

        /// <summary>
        /// Преобразует строку к формату процессора STM32F107
        /// (русские и специальные символы заменяются на их коды,
        /// записанные в восьмеричной системе)
        /// </summary>
        private string ConvertStringToSTM32F107Format(View view)
        {
            string s = view.Text + new string(' ', this.pultModel.Type.SymbolsInRow - view.Text.Length);
            foreach (PultVar var in view.Vars)
            {
                string t = "";
                for (int i = 0; i < var.Mask.Length; i++)
                    t += (var.Mask[i] == '0') ? "0" : " ";
                s = s.Remove(var.Position, var.Mask.Length).Insert(var.Position, t);
            }
            string res = "";
            byte[] b = EpsonPultEncoding.GetBytes(s);
            for (int i = 0; i < b.Length; i++)
            {
                res += "\\" + Convert.ToString(b[i], 8);
            }
            return res;
        }       
    }
}
