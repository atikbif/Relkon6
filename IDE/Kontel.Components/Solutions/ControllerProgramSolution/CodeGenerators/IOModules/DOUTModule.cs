using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Kontel.Relkon.Solutions
{
    public sealed class DOUTModule : DigitalModule
    {
        /// <summary>
        /// Минимальный физический номер модуля
        /// </summary>
        public const int MinModuleNumber = 65;
        /// <summary>
        /// Максимальный физический номер модуля
        /// </summary>
        public const int MaxModuleNumber = 128;

        /// <summary>
        /// Создает модуль по указанному номеру переменной
        /// </summary>        public DOUTModule(int index)
        public static new DOUTModule Create(int index)
        {
            if (index < DOUTModule.MinIndex || index > DOUTModule.MaxIndex)
                throw new Exception("Невеный индекс переменной. Допускаются переменные из диапазона OUT0 - OUT" + DOUTModule.MaxIndex);
            DOUTModule res = new DOUTModule();
            res.Init(res.ComputeModuleNumber(index));
            return res;
        }

        public DOUTModule(int ModuleNumber)
        {
            if (ModuleNumber < DOUTModule.MinModuleNumber || ModuleNumber > DOUTModule.MaxModuleNumber)
                throw new Exception(String.Format("Неверный номер модуля. Адреса модулей цифровых входов лежат в диапазоне {0} - {0}", DOUTModule.MinModuleNumber, DOUTModule.MaxModuleNumber));
            this.Init(ModuleNumber);
        }

        public DOUTModule()
        {

        }

        public override int RequestSize
        {
            get
            {
                return 4;
            }
        }

        public override double RequestTime
        {
            get
            {
                return 0.2;
            }
        }

        public override double ReplyTime
        {
            get
            {
                return 0.3;
            }
        }

        protected override int ComputeModuleNumber(int index)
        {
            return index + 0x3D;
        }

        protected override void GenerateVarsNames(int ModuleNumber)
        {
            this.VarNames = new ModuleVarDescription[] { new ModuleVarDescription("OUT" + (ModuleNumber - 0x3D), "OUT[" + (ModuleNumber - 0x41) + ']') };
        }
    }
}
