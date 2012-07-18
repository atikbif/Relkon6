using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.Solutions
{
    /// <summary>
    /// Содержит параметры пультов для 
    /// </summary>
    public sealed class PultParams
    {
        private int maxVarMaskLength = 0; // максимально возможный размер маски переменной пульта
        private int maxVarMaskDigitsCountAfterComma; // максимально возможное число цифр в маске переменной после точки
        private PultType defaultPultType; // тип пульта по-умолчанию
        private PultType[] availablePultTypes; // доступные для проекта типы пульта

        /// <summary>
        /// Возващает максимально возможный размер маски переменной пульта
        /// </summary>
        public int MaxVarMaskLength
        {
            get
            {
                return this.maxVarMaskLength;
            }
            internal set
            {
                this.maxVarMaskLength = value;
            }
        }
        /// <summary>
        /// Возващает максимально возможное число цифр в маске переменной после точки
        /// </summary>
        public int MaxVarMaskDigitsCountAfterComma
        {
            get
            {
                return this.maxVarMaskDigitsCountAfterComma;
            }
            internal set
            {
                this.maxVarMaskDigitsCountAfterComma = value;
            }
        }
        /// <summary>
        /// Возвращает тип пульта по-умолчанию
        /// </summary>
        public PultType DefaultPultType
        {
            get
            {
                return this.defaultPultType;
            }
            internal set
            {
                this.defaultPultType = value;
            }
        }
        /// <summary>
        /// Возвращает массив доступных для проекта типов пультов
        /// </summary>
        public PultType[] AvailablePultTypes
        {
            get
            {
                return this.availablePultTypes;
            }
            internal set
            {
                this.availablePultTypes = value;
            }
        }
        /// <summary>
        /// Проверяет, является ли указанный тип пульта допустимым
        /// для текущего процессора
        /// </summary>
        public bool PultTypeAllowed(PultType type)
        {
            bool res = false;
            for (int i = 0; i < this.AvailablePultTypes.Length; i++)
            {
                if (this.AvailablePultTypes[i].Equals(type))
                    res = true;
            }
            return res;
        }
    }
}
