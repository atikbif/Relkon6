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
        protected List<int> availablePeriods; // ������ ��������� �������� ��� ��������
        protected string workingDirectory; // ������ �������, � ������� ����� ���������� ��������������� �����
        protected ControllerProgramSolution solution; // ������, ��� �������� ������������ ���
        public int DispetcherPeriod; // ������ ������ ����������

        public CodeGenerator(ControllerProgramSolution Solution)
        {
            this.solution = Solution;
            this.codeModel = Solution.codeModel;
            if (!String.IsNullOrEmpty(Solution.PultFileName))
                this.pultModel = RelkonPultModel.FromFile(Solution.PultFileName);
        }
        /// <summary>
        /// ���������� ��� ������������� ����������� ��������� �������� ���������� ��������
        /// </summary>
        public abstract uint MaxDelay
        {
            get;
        }
        /// <summary>
        /// �������� ������ �������� �� ����������; � ������ ������ ������� ��������� � ������ ������
        /// � ���������� false, ����� ���������� true
        /// </summary>
        protected abstract bool ValidateSituationPeriod(RelkonCodeSituation Situation);
        /// <summary>
        /// �������� �������� �������� �� ����������; � ������ ������ ������� ��������� � ������ ������
        /// � ���������� false, ����� ���������� true
        /// </summary>
        private bool ValidateDelayInterval(uint delay, int LineNumber)
        {
            bool res = ((delay == 0) || (delay <= this.MaxDelay && delay >= this.DispetcherPeriod));
            if (!res)
                this.errors.Add(new CompilationError("�������� �������� ��������� ��������; �������� �������� ����� ��������� �������� � ��������� " + Math.Round(1.0 * this.DispetcherPeriod / 1000, 3) + " - " + Math.Round(1.0 * this.MaxDelay / 1000, 3) + " �.", this.codeModel.FileName, LineNumber, false));
            if (this.DispetcherPeriod != 0)
                if(delay % this.DispetcherPeriod != 0)
                    this.errors.Add(new CompilationError("�������������, ����� �������� ��������� �������� ���� ������� ������� ������ ���������� (" + this.DispetcherPeriod + " ��.)", this.codeModel.FileName, LineNumber, false));
            return res;
        }
        /// <summary>
        /// ���������� ��� ��������� �� ������ ������ ���� Relkon
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
        /// ���������� ��� ��������� �� ������ ������ ���� Relkon � StringBuilder
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
        /// ��������� ��� ������� �������
        /// </summary>
        public abstract void GeneratePultCode();
        /// <summary>
        /// ���������� ��� ������� �������������
        /// </summary>
        protected abstract void GenerateInitFunction(IndentedTextWriter CodeWriter);
        /// <summary>
        /// ���������� ��� ���������� ��������
        /// </summary>
        protected abstract string GetProcessName(RelkonCodeProcess Process);
        /// <summary>
        /// ���������� ��� ������� ��������
        /// </summary>
        protected abstract string GetSituationName(RelkonCodeProcess Process, RelkonCodeSituation Situation);
        /// <summary>
        /// ������� ��������� ���������
        /// </summary>
        protected abstract void GenerateHeader(IndentedTextWriter CodeWriter);
        /// <summary>
        /// ���������� ��� ��� #define-�����������
        /// </summary>
        protected abstract void GenerateDefines(IndentedTextWriter CodeWriter);
        /// <summary>
        /// ���������� ����������� ��������� ��������
        /// </summary>
        protected abstract void GenerateProcessStructDefenition(IndentedTextWriter CodeWriter);
        /// <summary>
        /// ���������� ���������� ��������� � ��������
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
                    this.errors.Add(new CompilationError("� �������� ����������� ��������", this.codeModel.FileName, this.codeModel.Processes[i].LineNumber, true));
            }
            CodeWriter.WriteLine(";");
        }
        /// <summary>
        /// ���������� ��������� ���� �������� ��������� 
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
        /// ���������� ����������� ���� ��������
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
        /// ���������� ��� �������� �� ������ ��������� ���� Relkon (�������� ������� Relkon
        /// �� ������� C
        /// </summary>
        /// <param name="Situation">��������, ��� ��� ������������</param>
        /// <param name="Process">�������, � �������� ��������� ��������</param>
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
        /// �� ��������� ������� ������� ���������� ����� ������
        /// � kon-�����
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
        /// �������� ����������� ������� � �������� �������� (#IN0.5, #OUT2.7 � �.�.)
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
        /// ���������� ����������� �������� ������ �� ����� C
        /// </summary>
        /// <param name="ByteNumber">����� �����, � �������� �������� ����</param>
        /// <param name="BitNumber">����� ���� � �����, �������� ������������� ����</param>
        protected abstract string GetDINDefinition(string ByteNumber, string BitNumber);
        /// <summary>
        /// ���������� ����������� �������� ������� �� ����� C
        /// </summary>
        /// <param name="ByteNumber">����� �����, � �������� �������� �����</param>
        /// <param name="BitNumber">����� ���� � �����, �������� ������������� �����</param>
        protected abstract string GetDOUTDefinition(string ByteNumber, string BitNumber);
        /// <summary>
        /// �������� ������� #START, #STOP
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
                            this.errors.Add(new CompilationError("��������� �������� �� ����������", this.codeModel.FileName, LineNumber, false));
                        string RelkonCode = mc[i].Groups[2].Value;
                        Code.Replace(RelkonCode, s, mc[i].Groups[2].Index + k, RelkonCode.Length);
                        k += s.Length - RelkonCode.Length;
                    }
                }
                while (Regex.IsMatch(Code.ToString(), pattern));
            }
        }
        /// <summary>
        /// ���������� ��� ������� ��������
        /// </summary>
        protected abstract string GetStartProcessCode(RelkonCodeProcess Process);
        /// <summary>
        /// ���������� ��� ��������� ��������
        /// </summary>
        protected virtual string GetStopProcessCode(RelkonCodeProcess Process)
        {
            return this.GetProcessName(Process) + ".W = empty";
        }
        /// <summary>
        /// �������� ������� �������� (#\s2, #\0.1\s2 � �.�.) 
        /// </summary>
        /// <param name="Process">�������, �������� �������� ���������������</param>
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
                            this.errors.Add(new CompilationError("��������� �������� �� ����������", this.codeModel.FileName, LineNumber, false));
                    }
                    Code.Replace(RelkonCode, s, mc[i].Groups[2].Index + k, RelkonCode.Length);
                    k += s.Length - RelkonCode.Length;
                }
            }
            while (Regex.IsMatch(Code.ToString(), @"#(?:/(\d+\.\d+))?/s(\d+)"));
        }
        /// <summary>
        /// ������� ��� �������� �������� �� ������ ��������
        /// </summary>
        /// <param name="Process">�������, � ������� �������������� ������</param>
        /// <param name="Situation">��������, �� ������� �������������� �������</param>
        /// <param name="Delay">�������� � �� ����� ���������</param>
        protected abstract string GetJumpToSituationCode(RelkonCodeProcess Process, RelkonCodeSituation Situation, uint Delay);
        /// <summary>
        /// �������� ������� �������� (#\s2, #\0.1\s2 � �.�.);
        /// </summary>
        /// <param name="Process">�������, �������� �������� ���������������</param>
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
        /// ������� ��� ������� ������ ����� ��������
        /// </summary>
        /// <param name="Process">�������, � ������� �������������� ������</param>
        /// <param name="Delay">�������� � �� ����� �������� ������ �����</param>
        protected abstract string GetRestartSituationCode(RelkonCodeProcess Process, uint Delay);
        /// <summary>
        /// ��������� ������������� ��� ������� ���������� ������
        /// </summary>
        protected virtual void PerfomCustomReplacing(StringBuilder Code)
        {
            return;
        }
        /// <summary>
        /// ���������� ��������� ���������
        /// </summary>
        protected abstract void GenerateDispetcher(IndentedTextWriter CodeWriter);
        /// <summary>
        /// ����������, ���� �� ���������� ������ ��� ��������� ���� ���������
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
        /// ���������� ������ ������, ������������ ��� ��������� ���� ���������
        /// </summary>
        public List<CompilationError> Errors
        {
            get
            {
                return this.errors;
            }
        }
        /// <summary>
        /// ������� �� ���� ������ ������
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
        /// ����������� ��������� �������� ���� Relkon
        /// � ��� �� C
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
