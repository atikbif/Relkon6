using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    public sealed class RelkonCodeStruct : RelkonCodeObject
    {
        //Элементы тела структуры
        private List<RelkonCodeVarDefenition> vars = new List<RelkonCodeVarDefenition>();
        private string name; //Имя структуры
        private bool far; //Если true, то переменная объявлена через far        
        /// <summary>
        /// Возвращает элементы (переменные) тела структуры
        /// </summary>
        public List<RelkonCodeVarDefenition> Vars
        {
            get 
            {
                return this.vars;
            }
        }
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
        /// Создаёт экземпляр данного класса
        /// </summary>
        public RelkonCodeStruct(int LineNumber, string Code, string Name, bool Far, List<RelkonCodeVarDefenition> Vars)
               : base(LineNumber, Code)
        {
            this.name = Name;            
            this.far = Far;
            this.vars = Vars;
        }
    }
}
