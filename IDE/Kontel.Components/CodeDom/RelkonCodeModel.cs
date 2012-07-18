using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Kontel.Relkon;
using Kontel.Relkon.Solutions;
using System.Collections;

namespace Kontel.Relkon.CodeDom
{
    public sealed class RelkonCodeModel
    {
        private List<RelkonCodeDefine> defines; // #define-объявления
        private List<RelkonCodeVarDefenition> vars; // объявления переменных
        private RelkonCodeObject initFunction; // инициализирующая функция программы
        private List<RelkonCodeProcess> processes; // определения процессов программы
        private List<RelkonCodeStruct> structs; // объявление структур
        private Hashtable modules; // список внешнх модулей ВВ, используемых в программе; ключ - физический номер мдуля, значение - сам модуль
        private StringBuilder codeRemainder; // оставшийся код после выделения всех конструкций программы
        private string fileName; // имя файла, по которому создана модель

        /// <summary>
        /// Имя файла, по которому создана модель
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }
        /// <summary>
        /// Возвращает список #define-объявлений программы
        /// </summary>
        public List<RelkonCodeDefine> Defines
        {
            get
            {
                return this.defines;
            }
        }
        /// <summary>
        /// Возвращает список объявлений перменных программы
        /// </summary>
        public List<RelkonCodeVarDefenition> Vars
        {
            get
            {
                return this.vars;
            }
        }
        /// <summary>
        /// Возвращает список модулей ВВ, используемых в программе;
        /// ключ - физический номер мдуля, значение - сам модуль
        /// </summary>
        public Hashtable IOModules
        {
            get
            {
                return this.modules;
            }
        }
        /// <summary>
        /// Возвращает функцию инициализации программы
        /// </summary>
        public RelkonCodeObject InitFunction
        {
            get
            {
                return this.initFunction;
            }
        }
        /// <summary>
        /// Возвращает список процессов программы
        /// </summary>
        public List<RelkonCodeProcess> Processes
        {
            get
            {
                return this.processes;
            }
        }
        /// <summary>
        /// Возвращает список процессов программы
        /// </summary>
        public List<RelkonCodeStruct> Structs
        {
            get
            {
                return this.structs;
            }
        }
        /// <summary>
        /// Возвращает остаток кода после выделения всех конструкций программы
        /// </summary>
        public StringBuilder CodeRemainder
        {
            get
            {
                return this.codeRemainder;
            }
        }
        /// <summary>
        /// Удаляет все комментарии из текста программы и устанавливает перед 
        /// каждой строкой ее номер
        /// </summary>
        /// <param name="Buffer">Буфер, содержащий текст программы</param>
        /// <param name="reader">Поток для чтения кода программы</param>
        private static void RemoveComments(TextReader reader, StringBuilder Buffer)
        {
            int lineNumber = 0;
            bool isComment = false;
            string s = null;
            while ((s = reader.ReadLine()) != null)
            {
                bool isRecord = false;
                bool lineStart = true;
                if (isComment)
                    Buffer.Append("\r\n");
                for (int i = 0; i < s.Length; i++)
                {
                    if ((s[i] == '/' && i == s.Length - 1) || (s[i] != '/' || s[i + 1] != '/') || isComment)
                    {
                        if (i < s.Length - 1 && s[i] == '/' && s[i + 1] == '*' && !isComment)
                        {
                            isRecord = false;
                            isComment = true;
                            i += 2;
                        }
                        if ((i < s.Length - 1 && s[i] == '*' && s[i + 1] == '/' && isComment))
                        {
                            isRecord = true;
                            isComment = false;
                            i += 2;
                            if (i >= s.Length)
                            {
                                Buffer.Append("\r\n");
                                isRecord = false;
                                break;
                            }
                        }
                        if (i < s.Length && s[i] > 32 && !isComment)
                            isRecord = true;
                        if (isRecord)
                        {
                            if (lineStart)
                            {
                                Buffer.Append("/*" + (lineNumber + 1) + "*/");
                                lineStart = false;
                            }
                            Buffer.Append(s[i]);
                        }
                    }
                    else
                    {
                        if (isRecord)
                        {
                            if (lineStart)
                            {
                                Buffer.Append("/*" + (lineNumber + 1) + "*/");
                                lineStart = false;
                            }
                            Buffer.Append("\r\n");
                            isRecord = false;
                        }
                        break;
                    }
                }
                if (isRecord)
                    Buffer.Append("\r\n");
                lineNumber++;
            }
            Buffer.Append("\r\n");
            reader.Close();
        }
        /// <summary>
        /// Возвращает представление инициализирующей функции программы; код функции удаляется
        /// из кода программы
        /// </summary>
        /// <param name="CodeRemainder">Строка, содержащая код программы</param>
        private static RelkonCodeObject GetInitFunction(StringBuilder CodeRemainder)
        {
            string Code = CodeRemainder.ToString();
            Match m = Regex.Match(Code, @"(/\*(\d+)\*/\s*#INIT\s+)(?:.*\n)*");
            if (!m.Success)
                return new RelkonCodeObject(-1, "");
            int endIdx = Utils.GetIndexOfAnySubString(Code, new string[] { "#DATA", "#INPUT", "#OUTPUT", "#TIME", "#PROCESS" }, m.Index, @"/\*\d+\*/\s*");
            if (endIdx == -1)
                endIdx = Code.Length;
            RelkonCodeObject res = new RelkonCodeObject(int.Parse(m.Groups[2].Value), Code.Substring(m.Index + m.Groups[1].Value.Length, endIdx - m.Index - m.Groups[1].Value.Length));
            CodeRemainder.Remove(m.Index, endIdx - m.Index);
            return res;
        }
        /// <summary>
        /// Возвращает список процесоов, объявленных в программе; код процессов удаляется
        /// из кода программы
        /// </summary>
        /// <param name="CodeRemainder">Строка, содержащая код программы</param>
        private static List<RelkonCodeProcess> GetProcessesList(StringBuilder CodeRemainder)
        {
            List<RelkonCodeProcess> res = new List<RelkonCodeProcess>();
            StringBuilder ProcessesCodeRemainder = new StringBuilder();
            string Code = CodeRemainder.ToString();
            MatchCollection mc1 = Regex.Matches(Code, @"(/\*(\d+)\*/\s*#PROCESS\s*(\d+)\s+)(?:.*\n)");
            int k = 0;
            for (int i = 0; i < mc1.Count; i++)
            {
                int LineNumber = int.Parse(mc1[i].Groups[2].Value);
                int ProcessIdx = int.Parse(mc1[i].Groups[3].Value);
                string ProcessCode = "";
                int endIdx = -1;
                if (i < mc1.Count - 1)
                    endIdx = mc1[i + 1].Index;
                else
                {
                    endIdx = Utils.GetIndexOfAnySubString(Code, new string[] { "#DATA", "#INPUT", "#OUTPUT", "#TIME", "#INIT" }, mc1[i].Index, @"/\*\d+\*/\s*");
                    if (endIdx == -1)
                        endIdx = Code.Length;
                }
                ProcessCode = Code.Substring(mc1[i].Index + mc1[i].Groups[1].Value.Length, endIdx - mc1[i].Index - mc1[i].Groups[1].Value.Length);
                RelkonCodeProcess p = new RelkonCodeProcess(LineNumber, ProcessCode, ProcessIdx);
                res.Add(p);
                CodeRemainder.Remove(mc1[i].Index - k, endIdx - mc1[i].Index);
                k += endIdx - mc1[i].Index;
                ProcessesCodeRemainder.Append(p.OtherCode);
            }
            CodeRemainder.Append(ProcessesCodeRemainder);
            return res;
        }
        /// <summary>
        /// Удаляет определения структур из кода программы
        /// </summary>
        private static List<RelkonCodeStruct> GetStructsDefenitions(StringBuilder VarsAndStructs)
        {
            List<RelkonCodeStruct> res = new List<RelkonCodeStruct>();
            MatchCollection mc = Regex.Matches(VarsAndStructs.ToString(), @"(struct[^\{]*\{[^\}]*\})([^;]*);");
            int k = 0;
            bool far = false;
            foreach (Match mc1 in mc)
            {
                string strNames = mc1.Groups[2].Value;
                if (strNames.Contains("far"))
                {
                    far = true;
                    strNames = strNames.Replace("far", "");                    
                }                
                string[] names = strNames.Split(',');               
                foreach (string name in names)
                {                    
                    List<RelkonCodeVarDefenition> vrs = GetVarsList(new StringBuilder(mc1.Groups[1].Value.ToString()));
                    res.Add(new RelkonCodeStruct(-1, "", name.Trim(), far, vrs));
                }
                VarsAndStructs = VarsAndStructs.Remove(mc1.Index - k, mc1.Value.Length);
                k += mc1.Value.Length;
            }          
            return res;
        }
        /// <summary>
        /// Удалаяет определения типов через typedef из кода программы
        /// </summary>
        private static string RemoveTypedefDefinitions(string Code)
        {
            string value = Code;
            MatchCollection mc1 = Regex.Matches(value, "typedef[^;]*;");
            int k = 0;
            foreach (Match m in mc1)
            {
                value = value.Remove(m.Index - k, m.Value.Length);
                k += m.Value.Length;
            }
            return value;
        }
        /// <summary>
        /// Удалает директивы Relkon из кода программы
        /// </summary>
        private static void RemoveRelkonDirectives(StringBuilder CodeRemainder)
        {
            string[] RelkonDirectives = new[] { "#RELKON-C.52", "#DATA", "#INIT", "#INPUT", "#OUTPUT", "#TIME(0.1)", "#TIME(1)" };
            for (int i = 0; i < RelkonDirectives.Length; i++)
            {
                CodeRemainder.Replace(RelkonDirectives[i], "");
            }
        }
        /// <summary>
        /// Проверяет, не является ли слово зарезервированной командой
        /// </summary>
        private static bool IsReservedWord(string value)
        {
            List<string> ReserveWords = new List<string>(new string[] { "unsigned", "char", "short", "int", "long", "float", "double", "void", "far", "if", "do", "while", "break", "continue", "goto", "struct" });
            return ReserveWords.Contains(value);
        }
        /// <summary>
        /// Возвращает представление массива
        /// </summary>
        /// <param name="TypeDefenition">Тип массива</param>
        /// <param name="ArrayName">Имя переменной</param>
        /// <param name="Dimesion">Размернось массива ([10], [5][6] и т.д.</param>
        private static RelkonCodeArrayDefenition GetArrayDefenition(string ArrayName, string TypeDefenition, string Dimesion, bool Far)
        {
            MatchCollection mc = Regex.Matches(Dimesion, "\\d+");
            int ItemsCount = 0;
            foreach (Match m in mc)
            {
                ItemsCount += int.Parse(m.Value);
            }
            return new RelkonCodeArrayDefenition(-1, "", ArrayName, TypeDefenition, Far, ItemsCount);
        }
        /// <summary>
        /// Возвращает массив переменных программы (включая структуры); объявления переменны 
        /// из кода программы не удаляются
        /// </summary>
        /// <param name="VarsAndStructs">Код программы</param>
        private static List<RelkonCodeVarDefenition> GetVarsList(StringBuilder VarsAndStructs)
        {
            List<RelkonCodeVarDefenition> res = new List<RelkonCodeVarDefenition>();          
            string Code = RemoveTypedefDefinitions(VarsAndStructs.ToString());
            Code = Regex.Replace(Code, "\".*\"", "");
            string pattern = @"(?:(?:(\b(?:__)?far\s+)?(?:volatile\s+)?(?:const\s+)?(unsigned\s+)?)|(?:\s+))((\bchar\b)|(\bint\b)|(\bshort\b)|(\blong\b)|(\bfloat\b)|(\bdouble\b)|(\bClock\b))[^;]";
            MatchCollection mc = Regex.Matches(Code, pattern);
            for(int i = 0; i < mc.Count; i++)
            {
                int endIdx = (i < mc.Count - 1) ? mc[i + 1].Index : Code.Length;
                string VarsDefenition = Code.Substring(mc[i].Index + mc[i].Value.Length, endIdx - mc[i].Index - mc[i].Value.Length);
                string VarType = mc[i].Groups[3].Value;
                bool HasSign = !mc[i].Groups[2].Success;
                bool far = mc[i].Groups[1].Success && mc[i].Groups[1].Value.Trim() == "far";
                MatchCollection words = Regex.Matches(VarsDefenition, "\\b([a-zA-Z_][a-zA-Z0-9_]*)((?:\\[\\d+\\])*)(\\s*=\\s*((\"[^\"]*\")|([a-zA-Z_0-9]+)))?");
                foreach (Match word in words)
                {
                    if (word.Value == "at" || word.Value == "far")
                        continue;
                    string VarName = word.Groups[1].Value;
                    string Dimension = word.Groups[2].Value;
                    if (IsReservedWord(word.Groups[1].Value))
                        break;
                    if (Dimension != "")
                    {
                        if(VarType!="Clock")
                            res.Add(GetArrayDefenition(VarName, VarType, Dimension, far));
                    }
                    else
                    {
                        if (VarType == "Clock")
                        {
                            if (VarName == "date")
                            {
                                res.Add(new RelkonCodeVarDefenition(-1, "", "date_Date", "char", false, far));
                                res.Add(new RelkonCodeVarDefenition(-1, "", "date_Month", "char", false, far));
                                res.Add(new RelkonCodeVarDefenition(-1, "", "date_Year", "char", false, far));
                            }
                            if (VarName == "time")
                            {
                                res.Add(new RelkonCodeVarDefenition(-1, "", "time_Second", "char", false, far));
                                res.Add(new RelkonCodeVarDefenition(-1, "", "time_Minute", "char", false, far));
                                res.Add(new RelkonCodeVarDefenition(-1, "", "time_Hour", "char", false, far));
                            }
                        }
                        else
                            res.Add(new RelkonCodeVarDefenition(-1, "", VarName, VarType, HasSign, far));
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Возвращает список дирекив #define программы; код дирекив удаляется
        /// из кода программы
        /// </summary>
        /// <param name="CodeRemainder">Код программы</param>
        public static List<RelkonCodeDefine> GetDefines(StringBuilder CodeRemainder)
        {
            List<RelkonCodeDefine> res = new List<RelkonCodeDefine>();
            string Code = CodeRemainder.ToString();
            string Pattern = @"/\*(\d+)\*/\s*#define\s+([_a-zA-Z0-9]+)\s+([^\r\n]*)";
            MatchCollection DefineMatches = Regex.Matches(Code, Pattern);
            int k = 0;
            foreach (Match DefineMatche in DefineMatches)
            {
                res.Add(new RelkonCodeDefine(int.Parse(DefineMatche.Groups[1].Value), DefineMatche.Value, DefineMatche.Groups[2].Value, DefineMatche.Groups[3].Value));
                CodeRemainder.Remove(DefineMatche.Index - k, DefineMatche.Length);
                k += DefineMatche.Length;
            }
            return res;
        }
        /// <summary>
        /// Преобразует код программы в его программное представление
        /// </summary>
        /// <param name="FileName">Имя файла программы</param>
        /// <param name="HasExternalIOModules">Показывает, распарсивать ли код с поддержкой внешних модулей ВВ</param>
        public static RelkonCodeModel ParseFromFile(string FileName, bool HasExternalIOModules)
        {
            RelkonCodeModel res = RelkonCodeModel.ParseFromCode(File.ReadAllText(FileName, Encoding.Default));
            res.fileName = FileName;
            return res;
        }
        /// <summary>
        /// Преобразует код программы в его программное представление
        /// </summary>
        /// <param name="Code">Код программы</param>
        /// <param name="HasExternalIOModules">Показывает, распарсивать ли код с поддержкой внешних модулей ВВ</param>
        public static RelkonCodeModel ParseFromCode(string Code)
        {
            RelkonCodeModel res = new RelkonCodeModel();
            res.codeRemainder = new StringBuilder();
            RemoveComments(new StringReader(Code), res.codeRemainder);
            res.initFunction = GetInitFunction(res.codeRemainder);
            res.processes = GetProcessesList(res.codeRemainder);
            RemoveRelkonDirectives(res.codeRemainder);
            res.defines = GetDefines(res.codeRemainder);
            StringBuilder VarsAndStructs = new StringBuilder(res.codeRemainder.ToString());
            res.structs = GetStructsDefenitions(VarsAndStructs);
            res.vars = GetVarsList(VarsAndStructs);                       
            res.FillIOModulesList();
            return res;
        }
        /// <summary>
        /// Проверяет содержится ли в программепроцессор с указанным индексом
        /// </summary>
        public RelkonCodeProcess GetProcessByIndex(int index)
        {
            foreach (RelkonCodeProcess process in this.processes)
            {
                if (process.Index == index)
                    return process;
            }
            return null;
        }        

        #region IOModules functions
        /// <summary>
        /// Возвращает список модулей, использующихся в указанном фрагменте кода
        /// </summary>
        private List<IOModule> GetIOModules(string code, Hashtable DefinesWModules)
        {
            List<IOModule> res = new List<IOModule>();
            // Поиск IN-модулей
            string pattern = @"\bIN(\d+)\b";
            MatchCollection matches = Regex.Matches(code, pattern);
            foreach (Match m in matches)
            {
                int index = int.Parse(m.Groups[1].Value);
                if (index >= DINModule.MinIndex && index <= DINModule.MaxIndex) // IN-модули начинаются с IN4
                {
                    res.Add(DINModule.Create(index));
                }
            }
            // Поиск OUT-модулей
            pattern = @"\bOUT(\d+)\b";
            matches = Regex.Matches(code, pattern);
            foreach (Match m in matches)
            {
                int index = int.Parse(m.Groups[1].Value);
                if (index >= DOUTModule.MinIndex && index <= DINModule.MaxIndex) // OUT-модули начинаются с OUT4
                {
                    res.Add(DOUTModule.Create(index));
                }
            }
            // Поиск ADC-модулей
            pattern = @"\bAD[C|H](\d+)\b";
            matches = Regex.Matches(code, pattern);
            foreach (Match m in matches)
            {
                int index = int.Parse(m.Groups[1].Value);
                if (index >= ADCModule.MinIndex && index <= ADCModule.MaxIndex) // ADC-модули начинаются с ADC9
                {
                    res.Add(ADCModule.Create(index));
                }
            }
            // Поиск DAC-модулей
            pattern = @"\bDA[C|H](\d+)\b";
            matches = Regex.Matches(code, pattern);
            foreach (Match m in matches)
            {
                int index = int.Parse(m.Groups[1].Value);
                if (index >= DACModule.MinIndex && index<= DACModule.MaxIndex) // DAC-модули начинаются с DAC5
                {
                    res.Add(DACModule.Create(index));
                }
            }
            if (DefinesWModules != null)
            {
                foreach (string define in DefinesWModules.Keys)
                {
                    Match m = Regex.Match("\b" + define + "\b", code);
                    if (m.Success)
                    {
                        res.AddRange((List<IOModule>)DefinesWModules[define]);
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Проверяет, занесен ли модуль в список используемых модулей, и если нет, то добавляет его в список
        /// </summary>
        private void AddModulesToModulesHashtable(List<IOModule> modules, int Period)
        {
            if (Period == 0)
                Period = 1000;
            foreach (IOModule module in modules)
            {
                if (!this.modules.ContainsKey(module.ModuleNumber))
                {
                    module.Period = Period;
                    this.modules.Add(module.ModuleNumber, module);
                }
                else if (Period < ((IOModule)this.modules[module.ModuleNumber]).Period)
                    ((IOModule)this.modules[module.ModuleNumber]).Period = Period;
            }
        }
        /// <summary>
        /// Заполняет список внешних модулей ВВ, используемых в программе 
        /// </summary>
        private void FillIOModulesList()
        {
            this.modules = new Hashtable();
            Hashtable definesWModules = new Hashtable(); // ключ - имя #define-переменной, знас\чение - список модулей ВВ, в нем определенных
            foreach (RelkonCodeDefine define in this.defines)
            {
                List<IOModule> defineModules = this.GetIOModules(define.Value, null);
                if (defineModules.Count > 0 && !definesWModules.ContainsKey(define.Name))
                    definesWModules.Add(define.Name, defineModules);
            }
            foreach(RelkonCodeProcess process in this.processes)
            {
                foreach(RelkonCodeSituation situation in process.Situations)
                {
                    this.AddModulesToModulesHashtable(this.GetIOModules(situation.Code, definesWModules), situation.Period);
                }
            }
            this.AddModulesToModulesHashtable(this.GetIOModules(this.initFunction.Code, definesWModules), 1000);
        }

        #endregion
    }
}
