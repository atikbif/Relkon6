using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.Solutions
{
    /// <summary>
    /// Содежит настройки процесора проекта контроллера
    /// </summary>
    public sealed class ProcessorParams
    {
        private string sdkDirectory = ""; // каталог SDK процессора проекта
        private ProcessorType type = ProcessorType.AT89C51ED2; // тип процессора проекта
        private bool inverseByteOrder = false; // порядок представления чисел в памятю процессора

        /// <summary>
        /// Возвращает или устанавливает каталог SDK процессора проекта
        /// </summary>
        public string SDKDirectory
        {
            get
            {
                return this.sdkDirectory;
            }
            set
            {
                this.sdkDirectory = value;
            }
        }
        /// <summary>
        /// Возвращает тип процессора проекта
        /// </summary>
        public ProcessorType Type
        {
            get
            {
                return this.type;
            }
            internal set
            {
                this.type = value;
            }
        }
        /// <summary>
        /// Возвращает порядок следования байт в памяти процессора (прямой - сначала идет 
        /// старший байт, а за ним младший, или обратный - сначала младший, а за ним - старший)
        /// </summary>
        public bool InverseByteOrder
        {
            get
            {
                return this.inverseByteOrder;
            }
            internal set
            {
                this.inverseByteOrder = value;
            }
        }
    }
}
