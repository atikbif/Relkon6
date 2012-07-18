using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// Описывает переменную заводских установок контроллера
    /// </summary>
    public sealed class ControllerEmbeddedVar : ControllerVar
    {
        /// <summary>
        /// Значение переменной
        /// </summary>
        [XmlAttribute]
        public long Value = 0;
    }
}
