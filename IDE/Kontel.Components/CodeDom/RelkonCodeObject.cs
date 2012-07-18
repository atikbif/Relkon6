using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    public class RelkonCodeObject
    {
        private int lineNumber; // номер строке в тексте программы, с которой начинается элемент
        private string code; // код программы, описывающий элемент
        /// <summary>
        /// Возвращает номер строки в тексте программы, с которой начинается элемент
        /// </summary>
        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }
        /// <summary>
        /// Код программы, описывающий элемент
        /// </summary>
        public string Code
        {
            get
            {
                return this.code;
            }
        }
        /// <summary>
        /// Создает новый экземпяр класса
        /// </summary>
        /// <param name="LineNumber">Номер стоки в программе, с которой начинается элемент</param>
        /// <param name="Code">Код программы, описывающий элемент</param>
        public RelkonCodeObject(int LineNumber, string Code)
        {
            this.code = Code;
            this.lineNumber = LineNumber;
        }
    }
}
