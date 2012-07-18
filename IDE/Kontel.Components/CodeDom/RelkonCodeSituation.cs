using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    public sealed class RelkonCodeSituation : RelkonCodeObject
    {
        private int index; // номер ситуации
        private int period; // период ситуации в мс

        /// <summary>
        /// Возвращает номер ситуации
        /// </summary>
        public int Index
        {
            get
            {
                return this.index;
            }
        }
        /// <summary>
        /// Возвращает период ситуации в мс
        /// </summary>
        public int Period
        {
            get
            {
                return this.period;
            }
        }

        public RelkonCodeSituation(int LineNumber, string Code, int Index, int Period)
            : base(LineNumber, Code)
        {
            this.index = Index;
            this.period = Period;
        }
    }
}
