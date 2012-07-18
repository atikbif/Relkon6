using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    public sealed  class RelkonCodeDefine: RelkonCodeObject
    {
        private string name; // имя определяемого #define выражения
        private string value; // значение определяемого #define выражения

        /// <summary>
        /// Возвращает имя определяемого через #define выражения
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        /// <summary>
        /// Возвращает значение определяемого через #define выражения
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }
        }

        public RelkonCodeDefine(int LineNumber, string Code, string Name, string Value)
            : base(LineNumber, Code)
        {
            this.name = Name;
            this.value = Value;
        }
    }
}
