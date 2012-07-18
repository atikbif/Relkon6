using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.Solutions
{
    public abstract class DigitalModule: IOModule
    {
        /// <summary>
        /// Минимальное значение, которое может принимать переменная модуля
        /// </summary>
        public const int MinIndex = 4;
        /// <summary>
        /// Максимальное значение, которое может принимать переменная модуля
        /// </summary>
        public const int MaxIndex = 67;
    }
}
