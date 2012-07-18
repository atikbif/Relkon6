using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.CodeDom;
using Kontel.Relkon.Classes;
using System.IO;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Kontel.Relkon.Solutions
{
    public abstract class CodeGenerator
    {
        protected RelkonCodeModel codeModel;
        protected RelkonPultModel pultModel;
        private List<CompilationError> errors = new List<CompilationError>();
        protected List<int> availablePeriods; // список доступных периодов для ситуаций
        protected string workingDirectory; // рабоий каталог, в который будут сохранятся сгенерированные файлы
        protected ControllerProgramSolution solution; // проект, для которого генерируется код
        public int DispetcherPeriod; // период работы диспетчера

        public CodeGenerator(ControllerProgramSolution Solution)
        {
            this.solution = Solution;
            this.codeModel = Solution.codeModel;
            if (!String.IsNullOrEmpty(Solution.PultFileName))
                this.pultModel = RelkonPultModel.FromFile(Solution.PultFileName);
        }
        /// <summary>
        /// Возвращает или устанавливает максимально возможнуя задержку выполнения процесса
        /// </summary>
        public abstract uint MaxDelay
        {
            get;
        }
        /// <summary>
        /// Поверяет период ситуации на валидность; в случае ошибки заносит сообщение в список ошибок
        /// и возвращает false, иначе возвращает true
        /// </summary>
        protected abstract bool ValidateSituationPeriod(RelkonCodeSituation Situation);
        /// <summary>
        /// Поверяет интервал задержки на валидность; в случае ошибки заносит сообщение в список ошибок
        /// и возвращает false, иначе возвращает true
        /// </summary>
        private bool ValidateDelayInterval(uint delay, int LineNumber)
        {
            bool res = ((delay == 0) || (delay <= this.MaxDelay && delay >= this.DispetcherPeriod));
            if (!res)
                this.errors.Add(new CompilationError("Неверное значение интервала задержки; интервал задержки может принимать значения в интервале " + Math.Round(1.0 * this.DispetcherPeriod / 1000, 3) + " - " + Math.Round(1.0 * this.MaxDelay / 1000, 3) + " с.", this.codeModel.FileName, LineNumber, false));
            if (this.DispetcherPeriod != 0)
                if(delay % this.DispetcherPeriod != 0)
                    this.errors.Add(new CompilationError("Рекомендуется, чтобы значение интервала задержки было кратным периоду работы диспетчера (" + this.DispetcherPeriod + " мс.)", this.codeModel.FileName, LineNumber, false));
            return res;
        }
        /// <summary>
        /// Генерирует код программы на основе модели кода Relkon
        /// </summary>
        public virtual void GenerateProgramCode()
        {
            StringBuilder res = new StringBuilder();
            IndentedTextWriter CodeWriter = new IndentedTextWriter(new StringWriter(res));
            this.GenerateHeader(CodeWriter);
            this.GenerateDefines(CodeWriter);
            CodeWriter.Write(this.codeModel.CodeRemainder);
            this.GenerateProcessesDefenitions(CodeWriter);
            this.GenerateDispetcher(CodeWriter);
            this.GenerateSituationPrototypes(CodeWriter);
            this.GenerateInitFunction(CodeWriter);
            this.GenerateSituations(CodeWriter);                      
            CodeWriter.Close();
            this.RemoveEmptyLines(res);
            try
            {
                File.WriteAllText(this.solution.DirectoryName + "\\fc_u.c", res.ToString(), Encoding.Default);
            }
            catch (Exception ex)
            {
                this.errors.Add(new CompilationError(ex.Message, this.codeModel.FileName, -1, false));
            }
        }
        /// <summary>
        /// Генерирует код программы на основе модели кода Relkon в StringBuilder
        /// </summary>
        public StringBuilder GenerateProgramCodeToStringBuilder()
        {
            StringBuilder res = new StringBuilder();
            IndentedTextWriter CodeWriter = new IndentedTextWriter(new StringWriter(res));
            CodeWriter.WriteLine("#include \"fc_u.h\"");
            CodeWriter.WriteLine("#include \"io.h\"");            
            CodeWriter.WriteLine("int _Sys4x_disp_counter = -1;");
            this.GenerateDefines(CodeWriter);
            CodeWriter.Write(this.codeModel.CodeRemainder);
            this.GenerateProcessesDefenitions(CodeWriter);
            this.GenerateDispetcher(CodeWriter);
            this.GenerateSituationPrototypes(CodeWriter);
            //this.GenerateInitFunction(CodeWriter);
            this.GenerateSituations(CodeWriter);
            CodeWriter.Close();
            this.RemoveEmptyLines(res);
            return res;
        }

        /// <summary>
        /// Генериует код пультов проекта
        /// </summary>
        public abstract void GeneratePultCode();
        /// <summary>
        /// Генерирует код функции инициализации
        /// </summary>
        protected abstract void GenerateInitFunction(IndentedTextWriter CodeWriter);
        /// <summary>
        /// Возвращает имя переменной процесса
        /// </summary>
        protected abstract string GetProcessName(RelkonCodeProcess Process);
        /// <summary>
        /// Возвращает имя функции ситуации
        /// </summary>
        protected abstract string GetSituationName(RelkonCodeProcess Process, RelkonCodeSituation Situation);
        /// <summary>
        /// Создает заголовок программы
        /// </summary>
        protected abstract void GenerateHeader(IndentedTextWriter CodeWriter);
        /// <summary>
        /// Генерирует код для #define-определений
        /// </summary>
        protected abstract void GenerateDefines(IndentedTextWriter CodeWriter);
        /// <summary>
        /// Генерирует определение структуры процесса
        /// </summary>
        protected abstract void GenerateProcessStructDefenition(IndentedTextWriter CodeWriter);
        /// <summary>
        /// Генерирует объявления процессов и ситуаций
        /// </summary>
        protected virtual void GenerateProcessesDefenitions(IndentedTextWriter CodeWriter)
        {
            this.GenerateProcessStructDefenition(CodeWriter);
            for(int i = 0; i<this.codeModel.Processes.Count; i++)
            {
                CodeWriter.Write(this.GetProcessName(this.codeModel.Processes[i]));
                if (i < this.codeModel.Processes.Count - 1)
                    CodeWriter.Write(",");
                if(this.codeModel.Processes[i].Situations.Count==0)
                    this.errors.Add(new CompilationError("В процессе отсутствуют ситуации", this.codeModel.FileName, this.codeModel.Processes[i].LineNumber, true));
            }
            CodeWriter.WriteLine(";");
        }
        /// <summary>
        /// Генерирует прототипы всех ситуаций программы 
        /// </summary>
        protected virtual void GenerateSituationPrototypes(IndentedTextWriter CodeWriter)
        {
            foreach (RelkonCodeProcess process in this.codeModel.Processes)
            {
                foreach (RelkonCodeSituation situation in process.Situations)
                {
                    CodeWriter.WriteLine("void " + this.GetSituationName(process, situation) + "();");
                }
            }
            CodeWriter.WriteLine("void empty(){}");
        }
        /// <summary>
        /// Генерирует определения всех ситуаций
        /// </summary>
        protected virtual void GenerateSituations(IndentedTextWriter CodeWriter)
        {
            foreach (RelkonCodeProcess process in this.codeModel.Processes)
            {
                foreach (RelkonCodeSituation situation in process.Situations)
                {
                    if (!this.ValidateSituationPeriod(situation))
                        break;
                    CodeWriter.WriteLine("/*"+situation.LineNumber+"*/void " + this.GetSituationName(process, situation) + "()");
                    CodeWriter.WriteLine("{");
                    CodeWriter.Indent++;
                    this.GenerateSituationCode(process, situation, CodeWriter);
                    CodeWriter.Indent--;
                    CodeWriter.WriteLine("}");
                }
            }
        }
        /// <summary>
        /// Генерирует код ситуации на основе исходного кода Relkon (заменяет команды Relkon
        /// на команды C
        /// </summary>
        /// <param name="Situation">Ситуация, чей код генерируется</param>
        /// <param name="Process">Процесс, к которому относится ситуация</param>
        private void GenerateSituationCode(RelkonCodeProcess Process, RelkonCodeSituation Situation, IndentedTextWriter CodeWriter)
        {
            StringBuilder code = new StringBuilder(Situation.Code);
            this.ReplaceIO(code);
            this.ReplaceStartStopProcessCommands(code);
            this.ReplaceJumpSituationCommand(code, Process);
            this.ReplaceRestartSituationCommands(code, Process);
            this.PerfomCustomReplacing(code);
            CodeWriter.Write(code.ToString());
        }
        /// <summary>
        /// По указанной позиции символа возвращает номер строки
        /// в kon-файле
        /// </summary>
        protected int GetLineNumber(StringBuilder Code, int CharIndex)
        {
            int index = 0;
            for (int i = CharIndex; i >= 0; i--)
            {
                if (Code[i] == '\r')
                {
                    index = i;
                    break;
                }
            }
            Match m = Regex.Match(Code.ToString().Substring(index, CharIndex - index + 1), @"/\*(\d+)\*/");
            return (m.Success ? int.Parse(m.Groups[1].Value) : -1);
        }
        /// <summary>
        /// Заменяет определения входных и выходных датчиков (#IN0.5, #OUT2.7 и т.д.)
        /// </summary>
        protected virtual void ReplaceIO(StringBuilder Code)
        {
            for (int i = 1; i < 9; i++)
            {
                //Code = new StringBuilder();

                //Code.Replace("ADC" + i, "_Sys_ADC[" + (i - 1).ToString() + "]");
            }
            // "_Sys_ADC[" + (i - 1).ToString() + "]"
            int k = 0;
            MatchCollection mc = Regex.Matches(Code.ToString(), @"\bADC(\d+)\b");
            for (int j = 0; j < mc.Count; j++)
            {                
                int ByteNumber = int.Parse(mc[j].Groups[1].Value);
                if (ByteNumber < 9)
                {
                    string str = "_Sys_ADC[" + (ByteNumber - 1).ToString() + "]";
                    Code.Replace(mc[j].Value, str, mc[j].Index + k, mc[j].Value.Length);
                    k += str.Length - mc[j].Value.Length;
                }
            }


            for (int i = 0; i < 2; i++)
            {
                string pattern = (i == 0) ? @"#IN(\d+)\.([0-7])" : @"#OUT(\d+)\.([0-7])";
                mc = Regex.Matches(Code.ToString(), pattern);
                k = 0;
                for (int j = 0; j < mc.Count; j++)
                {
                    string ByteNumber = mc[j].Groups[1].Value;
                    string BitNumber = mc[j].Groups[2].Value;
                    string s = "";
                    try
                    {
                        s = (i == 0) ? this.GetDINDefinition(ByteNumber, BitNumber) : this.GetDOUTDefinition(ByteNumber, BitNumber);
                        Code.Replace(mc[j].Value, s, mc[j].Index + k, mc[j].Value.Length);
                        k += s.Length - mc[j].Value.Length;
                    }
                    catch (Exception ex)
                    {
                        this.errors.Add(new CompilationError(ex.Message, this.codeModel.FileName, this.GetLineNumber(Code, mc[j].Index + k), false));
                    }
                }
            }
        }
        /// <summary>
        /// Возвращает определение цифровых входов на языке C
        /// </summary>
        /// <param name="ByteNumber">Номер байта, к которому относися вход</param>
        /// <param name="BitNumber">Номер бита в байте, которому соответствует вход</param>
        protected abstract string GetDINDefinition(string ByteNumber, string BitNumber);
        /// <summary>
        /// Возвращает определение цифровых выходов на языке C
        /// </summary>
        /// <param name="ByteNumber">Номер байта, к которому относися выход</param>
        /// <param name="BitNumber">Номер бита в байте, которому соответствует выход</param>
        protected abstract string GetDOUTDefinition(string ByteNumber, string BitNumber);
        /// <summary>
        /// Заменяет команды #START, #STOP
        /// </summary>
        protected void ReplaceStartStopProcessCommands(StringBuilder Code)
        {
            for (int j = 0; j < 2; j++)
            {
                string pattern = (j == 0) ? @"/\*(\d+)\*/.*(#STARTp(\d+))" : @"/\*(\d+)\*/.*(#STOPp(\d+))";
                do
                {
                    MatchCollection mc = Regex.Matches(Code.ToString(), pattern);
                    int k = 0;
                    for (int i = 0; i < mc.Count; i++)
                    {
                        int LineNumber = int.Parse(mc[i].Groups[1].Value);
                        RelkonCodeProcess p = this.codeModel.GetProcessByIndex(int.Parse(mc[i].Groups[3].Value));
                        string s = "";
                        if (p != null)
                             s = (j == 0) ? this.GetStartProcessCode(p) : this.GetStopProcessCode(p);
                        else
                            this.errors.Add(new CompilationError("Указаного процесса не существует", this.codeModel.FileName, LineNumber, false));
                        string RelkonCode = mc[i].Groups[2].Value;
                        Code.Replace(RelkonCode, s, mc[i].Groups[2].Index + k, RelkonCode.Length);
                        k += s.Length - RelkonCode.Length;
                    }
                }
                while (Regex.IsMatch(Code.ToString(), pattern));
            }
        }
        /// <summary>
        /// Возвращает код запуска процесса
        /// </summary>
        protected abstract string GetStartProcessCode(RelkonCodeProcess Process);
        /// <summary>
        /// Возвращает код остановки процесса
        /// </summary>
        protected virtual string GetStopProcessCode(RelkonCodeProcess Process)
        {
            return this.GetProcessName(Process) + ".W = empty";
        }
        /// <summary>
        /// Заменяет команды перехода (#\s2, #\0.1\s2 и т.д.) 
        /// </summary>
        /// <param name="Process">Процесс, ситуации которого рассматриваются</param>
        protected void ReplaceJumpSituationCommand(StringBuilder Code, RelkonCodeProcess Process)
        {
            string pattern = @"/\*(\d+)\*/.*(#(?:/(\d+\.\d+))?/s(\d+))";
            do
            {
                int k = 0;
                MatchCollection mc = Regex.Matches(Code.ToString(), pattern);
                for (int i = 0; i < mc.Count; i++)
                {
                    int LineNumber = int.Parse(mc[i].Groups[1].Value);
                    string RelkonCode = mc[i].Groups[2].Value;
                    uint delay = (mc[i].Groups[3].Value != "") ? (uint)(double.Parse(mc[i].Groups[3].Value, NumberFormatInfo.InvariantInfo) * 1000) : 0;
                    string s = "";
                    if (this.ValidateDelayInterval(delay, LineNumber))
                    {
                        RelkonCodeSituation Situation = Process.GetSituationByIndex(int.Parse(mc[i].Groups[4].Value));
                        if (Situation != null)
                            s = this.GetJumpToSituationCode(Process, Situation, delay);
                        else
                            this.errors.Add(new CompilationError("Указанной ситуации не существует", this.codeModel.FileName, LineNumber, false));
                    }
                    Code.Replace(RelkonCode, s, mc[i].Groups[2].Index + k, RelkonCode.Length);
                    k += s.Length - RelkonCode.Length;
                }
            }
            while (Regex.IsMatch(Code.ToString(), @"#(?:/(\d+\.\d+))?/s(\d+)"));
        }
        /// <summary>
        /// Создает код операции перехода на другую ситуацию
        /// </summary>
        /// <param name="Process">Процесс, в котором осуществляется работа</param>
        /// <param name="Situation">Ситуация, на которую осуществляется переход</param>
        /// <param name="Delay">Задержка в мс перед переходом</param>
        protected abstract string GetJumpToSituationCode(RelkonCodeProcess Process, RelkonCodeSituation Situation, uint Delay);
        /// <summary>
        /// Заменяет команды перехода (#\s2, #\0.1\s2 и т.д.);
        /// </summary>
        /// <param name="Process">Процесс, ситуации которого рассматриваются</param>
        protected virtual void ReplaceRestartSituationCommands(StringBuilder Code, RelkonCodeProcess Process)
        {
            string pattern = @"/\*(\d+)\*/.*(#(?:/(\d+\.\d+))?/R)";
            do
            {
                int k = 0;
                MatchCollection mc = Regex.Matches(Code.ToString(), pattern);
                for (int i = 0; i < mc.Count; i++)
                {
                    int LineNumber = int.Parse(mc[i].Groups[1].Value);
                    string RelkonCode = mc[i].Groups[2].Value;
                    uint delay = (mc[i].Groups[3].Value != "") ? (uint)(double.Parse(mc[i].Groups[3].Value, NumberFormatInfo.InvariantInfo) * 1000) : 0;
                    string s = "";
                    if (this.ValidateDelayInterval(delay, LineNumber))
                        s = this.GetRestartSituationCode(Process, delay);
                    Code.Replace(RelkonCode, s, mc[i].Groups[2].Index + k, RelkonCode.Length);
                    k += s.Length - RelkonCode.Length;
                }
            }
            while (Regex.IsMatch(Code.ToString(), pattern));
        }
        /// <summary>
        /// Создает код запуска нового цикла ситуации
        /// </summary>
        /// <param name="Process">Процесс, в котором осуществляется работа</param>
        /// <param name="Delay">Задержка в мс перед запуском нового цикла</param>
        protected abstract string GetRestartSituationCode(RelkonCodeProcess Process, uint Delay);
        /// <summary>
        /// Выполняет специфические для каждого процессора замены
        /// </summary>
        protected virtual void PerfomCustomReplacing(StringBuilder Code)
        {
            return;
        }
        /// <summary>
        /// Генерирует диспетчер процессов
        /// </summary>
        protected abstract void GenerateDispetcher(IndentedTextWriter CodeWriter);
        /// <summary>
        /// Показывает, были ли обнаружены ошибки при генерации кода программы
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
        /// Возвращает список ошибок, обнаруженных при генерации кода программы
        /// </summary>
        public List<CompilationError> Errors
        {
            get
            {
                return this.errors;
            }
        }
        /// <summary>
        /// Удаляет из кода пустые строки
        /// </summary>
        private void RemoveEmptyLines(StringBuilder Code)
        {
            MatchCollection mc = Regex.Matches(Code.ToString(), @"^\s*(/\*\d+\*/)?\s*\r\n", RegexOptions.Multiline);
            int k = 0;
            foreach (Match m in mc)
            {
                Code.Remove(m.Index - k, m.Value.Length);
                k += m.Value.Length;
            }
        }
        /// <summary>
        /// Транслирует указанный фрагмент кода Relkon
        /// в код на C
        /// </summary>
        protected virtual string TranslateCodeFragment(string code)
        {
            StringBuilder res = new StringBuilder(code);
            this.ReplaceIO(res);
            this.ReplaceStartStopProcessCommands(res);
            this.PerfomCustomReplacing(res);
            return res.ToString();
        }
    }
}
