using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    /// <summary>
    /// Представляет массив элементов определеного типа
    /// </summary>
    public sealed class RelkonCodeArrayDefenition : RelkonCodeVarDefenition
    {
        private int itemsCount; // число элементов массива
        /// <summary>
        /// Возвращает число элементов массива
        /// </summary>
        public int ItemsCount
        {
            get
            {
                return this.itemsCount;
            }
        }

        public RelkonCodeArrayDefenition(int LineNumber, string Code, string Name, string Type, bool Far, int ItemsCount)
            : base(LineNumber, Code, Name, Type, false, Far)
        {
            this.itemsCount = ItemsCount;
        }
    }
}
