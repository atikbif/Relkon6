using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Kontel.Relkon.Solutions
{
    public sealed class DACModule: AnalogModule
    {
        /// <summary>
        /// Минимальное значение, которое может принимать переменная модуля
        /// </summary>
        public const int MinIndex = 5;
        /// <summary>
        /// Максимальное значение, которое может принимать переменная модуля
        /// </summary>
        public const int MaxIndex = 68;
        /// <summary>
        /// Минимальный физический номер модуля
        /// </summary>
        public const int MinModuleNumber = 161;
        /// <summary>
        /// Максимальный физический номер модуля
        /// </summary>
        public const int MaxModuleNumber = 192;

        /// <summary>
        /// Создает модуль по указанному номеру переменной
        /// </summary>
        public static new DACModule Create(int index)
        {
            if (index < DACModule.MinIndex || index > DACModule.MaxIndex)
                throw new Exception("Невеный индекс переменной. Допускаются переменные из диапазона DAC(DAH)1 - DAC(DAH)" + DACModule.MaxIndex);
            DACModule res = new DACModule();
            res.Init(res.ComputeModuleNumber(index));
            return res;
        }

        public DACModule(int ModuleNumber)
        {
            if (ModuleNumber < DACModule.MinModuleNumber || ModuleNumber > DACModule.MaxModuleNumber)
                throw new Exception(String.Format("Неверный номер модуля. Адреса модулей цифровых входов лежат в диапазоне {0} - {0}", DACModule.MinModuleNumber, DACModule.MaxModuleNumber));
            this.Init(ModuleNumber);
        }

        public DACModule()
        {

        }

        public override int RequestSize
        {
            get 
            {
                return 7;
            }
        }

        public override double RequestTime
        {
            get
            {
                return 0.5;
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
            return (index - MinIndex) / 2 + MinModuleNumber;
        }

        protected override void GenerateVarsNames(int ModuleNumber)
        {          
            this.VarNames = new ModuleVarDescription[2];
            this.SingleByteVarNames = new ModuleVarDescription[2];
            for (int i = 0; i < 2; i++)
            {
                int n = (ModuleNumber - MinModuleNumber) * 2 + MinIndex + i;
                int n1 = (ModuleNumber - MinModuleNumber);
                this.VarNames[i] = new ModuleVarDescription("DAC" + n, "_DAC.D" + (i + 1) + '[' + n1 + ']');
                this.SingleByteVarNames[i] = new ModuleVarDescription("DAH" + n, "_Sys4x_" + "_DAH" + n + ".BYTE");
            }
        }
    }
}
