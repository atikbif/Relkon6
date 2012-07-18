using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.Solutions
{
    public abstract class AnalogModule: IOModule
    {
        /// <summary>
        /// Возвращает список однобайтовых переменных модуля ВВ
        /// </summary>
        public ModuleVarDescription[] SingleByteVarNames { get; set; }
    }
}

