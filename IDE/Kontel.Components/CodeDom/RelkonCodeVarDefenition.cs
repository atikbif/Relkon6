using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    /// <summary>
    /// Представляет определение переменной программы
    /// </summary>
    public class RelkonCodeVarDefenition: RelkonCodeObject
    {
        protected string name; // имя переменной
        protected string type; // тип переменной
        private bool far; // если true, то переменная объявлена через far
        protected bool hasSign; // имеет ли переменная знак        

        /// <summary>
        /// Возвращает имя переменной
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        /// <summary>
        /// Возвращает тип переменной
        /// </summary>
        public string Type
        {
            get
            {
                return this.type;
            }
        }
        /// <summary>
        /// Если true, то переменная объявлена через far
        /// </summary>
        public bool Far
        {
            get
            {
                return this.far;
            }
        }

        /// <summary>
        /// Показывает, имеет ли переменная знак
        /// </summary>
        public bool HasSign
        {
            get
            {
                return this.hasSign;
            }
        }

        public RelkonCodeVarDefenition(int LineNumber, string Code, string Name, string Type, bool HasSign, bool Far): base(LineNumber, Code)
        {
            this.name = Name;
            this.type = Type;
            this.far = Far;
            this.hasSign = HasSign;
        }
    }
}
