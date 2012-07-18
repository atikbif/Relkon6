using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon
{
    /// <summary>
    /// EventArgs, передающий значение определенного типа
    /// </summary>
    /// <typeparam name="T">Тип значения EventArgs.Value</typeparam>
    public sealed class EventArgs<T> : EventArgs
    {
        private T value;

        public T Value
        {
            get
            {
                return this.value;
            }
        }
        public EventArgs(T Value)
        {
            this.value = Value;
        }
    }
}
