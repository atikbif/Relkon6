using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Kontel.Relkon.Solutions
{
    public sealed class DINModule : DigitalModule
    {
        /// <summary>
        /// Минимальный физический номер модуля
        /// </summary>
        public const int MinModuleNumber = 1;
        /// <summary>
        /// Максимальный физический номер модуля
        /// </summary>
        public const int MaxModuleNumber = 64;
        /// <param name="index">Индекс переменной модуля</param>
        public static new DINModule Create(int index)
        {
            if (index < DINModule.MinIndex || index > DINModule.MaxIndex)
                throw new Exception("Невеный индекс переменной. Допускаются переменные из диапазона IN0 - IN" + DINModule.MaxIndex);
            DINModule res = new DINModule();
            res.Init(res.ComputeModuleNumber(index));
            return res;
        }

        public DINModule(int ModuleNumber)
        {
            if (ModuleNumber < DINModule.MinModuleNumber || ModuleNumber > DINModule.MaxModuleNumber)
                throw new Exception(String.Format("Неверный номер модуля. Адреса модулей цифровых входов лежат в диапазоне {0} - {0}", DINModule.MinModuleNumber, DINModule.MaxModuleNumber));
            this.Init(ModuleNumber);
        }

        public DINModule()
        {

        }

        public override int RequestSize
        {
            get
            {
                return 1;
            }
        }

        public override double RequestTime
        {
            get
            {
                return 0.1;
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
            return index - 0x03;
        }

        protected override void GenerateVarsNames(int ModuleNumber)
        {
            this.VarNames = new ModuleVarDescription[] { new ModuleVarDescription("IN" + (ModuleNumber + 0x03), "IN[" + (ModuleNumber - 1) + ']') };
        }
    }
}
