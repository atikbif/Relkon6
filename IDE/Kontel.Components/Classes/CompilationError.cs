using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Solutions;


namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// �������� �������� ������ ����������� ��� ���������� ��������� ��� �����������
    /// </summary>
    public class CompilationError
    {
        /// <summary>
        /// �������� ������
        /// </summary>
        public string Description = "";
        /// <summary>
        /// ��� �����, � ������� ���������� ������
        /// </summary>
        public string FileName = "";
        /// <summary>
        /// ������, �� ������� ����������� ������
        /// </summary>
        public int LineNumber = -1;
        /// <summary>
        /// ����������, �������� �� ������ ������ ���������������
        /// </summary>
        public bool Warning = false;

        public CompilationError(string Description, string FileName, int LineNumber, bool Warning)
        {
            this.Description = Description;
            this.FileName = FileName;
            this.LineNumber = LineNumber;
            this.Warning = Warning;
        }

        public CompilationError()
        {

        }
    }
    /// <summary>
    /// �������� �������� ������, ��������� ��� ���������� ����� �������
    /// </summary>
    public sealed class PultTranslationError : CompilationError
    {
        /// <summary>
        /// ����� ������ ������, ���������� ������
        /// </summary>
        public int Row;
        /// <summary>
        /// ����� ����, ����������� ������
        /// </summary>
        public int View;
        /// <summary>
        /// ����� ������� ������, � �������� ���������� ������
        /// </summary>
        public int Symbol;

        public PultTranslationError(string Description, string FileName, int Row, int View, int Symbol, bool Warning)
        {
            this.Description = Description;
            this.FileName = FileName;
            this.Row = Row;
            this.View = View;
            this.Symbol = Symbol;
            this.Warning = Warning;
            this.LineNumber = -1;
        }
    }
    /// <summary>
    /// �������� �������� ������, ��������� ��� ���������� �������� ���������������
    /// </summary>  
    public sealed class DispatcheringSolutionError : CompilationError
    {
        /// <summary>
        /// ������ �� ����������, ��������� ������
        /// </summary>
        public int ControllerIndex;

        public DispatcheringSolutionError(string Description, string FileName, int ControllerIndex, bool Warning)
            : base(Description, FileName, -1, Warning)
        {
            this.ControllerIndex = ControllerIndex;
        }
    }
}
