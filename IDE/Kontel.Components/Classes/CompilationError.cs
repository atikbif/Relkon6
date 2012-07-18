using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Solutions;


namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// Содержит описание ошибок возникающих при компиляции программы для контроллера
    /// </summary>
    public class CompilationError
    {
        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string Description = "";
        /// <summary>
        /// Имя файла, в котором обнаружена ошибка
        /// </summary>
        public string FileName = "";
        /// <summary>
        /// Строка, на которой гобнаружена ошибка
        /// </summary>
        public int LineNumber = -1;
        /// <summary>
        /// Показывает, является ли ошибка только предупреждением
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
    /// Содержит описание ошибок, выявленых при трансляции файла пультов
    /// </summary>
    public sealed class PultTranslationError : CompilationError
    {
        /// <summary>
        /// Номер строки пульта, содержащей ошибку
        /// </summary>
        public int Row;
        /// <summary>
        /// Номер вида, содержащего ошибку
        /// </summary>
        public int View;
        /// <summary>
        /// Номер символа строки, с которого начинается ошибка
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
    /// Содержит описания ошибок, выявленых при трансляции проектов диспетчеризации
    /// </summary>  
    public sealed class DispatcheringSolutionError : CompilationError
    {
        /// <summary>
        /// Ссылка на контроллер, вызвавший ошибку
        /// </summary>
        public int ControllerIndex;

        public DispatcheringSolutionError(string Description, string FileName, int ControllerIndex, bool Warning)
            : base(Description, FileName, -1, Warning)
        {
            this.ControllerIndex = ControllerIndex;
        }
    }
}
