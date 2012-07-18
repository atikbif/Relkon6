using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// Описывает переменную контроллера в проекте диспетчеризации (РС-400, TC-65)
    /// </summary>
    public sealed class DispatcheringControllerVar : ControllerVar
    {
        /// <summary>
        /// Имя контроллера, в который записывается переменная при 
        /// межконтроллерном обмене
        /// </summary>
        [XmlAttribute]
        public string ExchangeControllerName = null;
        /// <summary>
        /// Смещение переменной в общей структуре запросов контроллера
        /// </summary>
        [XmlIgnore]
        public int Offset = 0;

        /// <summary>
        /// Возвращает значение переменной, преобразованное из long в соответствие с ее типом;
        /// тип double возвращается, т.к. предполагается использовть функию при построении графиков значений переменной
        /// </summary>
        public double ConvertValueToVarType(int value)
        {
            double res = 0;
            switch (this.Size)
            {
                case 1:
                    if (this.HasSign)
                        res = (sbyte)value;
                    else
                        res = (byte)value;
                    break;
                case 2:
                    if (this.HasSign)
                        res = (short)value;
                    else
                        res = (ushort)value;
                    break;
                case 4:
                    if (this.HasSign)
                        res = (int)value;
                    else
                        res = (uint)value;
                    break;
            }
            return res;
        }
    }
}
