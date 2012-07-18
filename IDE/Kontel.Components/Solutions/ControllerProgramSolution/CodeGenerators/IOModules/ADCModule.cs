using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Kontel.Relkon.Solutions
{
    public sealed class ADCModule : AnalogModule
    {
        /// <summary>
        /// Минимальное значение, которое может принимать переменная модуля
        /// </summary>
        public const int MinIndex = 9;
        /// <summary>
        /// Максимальное значение, которое может принимать переменная модуля
        /// </summary>
        public const int MaxIndex = 136;
        /// <summary>
        /// Минимальный физический номер модуля
        /// </summary>
        public const int MinModuleNumber = 129;
        /// <summary>
        /// Максимальный физический номер модуля
        /// </summary>
        public const int MaxModuleNumber = 160;

        /// <summary>
        /// Создает модуль по указанному номеру переменной
        /// </summary>
        public static new ADCModule Create(int index)
        {
            if (index < ADCModule.MinIndex || index > ADCModule.MaxIndex)
                throw new Exception("Невеный индекс переменной. Допускаются переменные из диапазона ADC(ADH)1 - ADC (ADH)" + ADCModule.MaxIndex);
            ADCModule res = new ADCModule();
            res.Init(res.ComputeModuleNumber(index));
            return res;
        }
        
        public ADCModule(int ModuleNumber)
        {
            if (ModuleNumber < ADCModule.MinModuleNumber || ModuleNumber > ADCModule.MaxModuleNumber)
                throw new Exception(String.Format("Неверный номер модуля. Адреса модулей цифровых входов лежат в диапазоне {0} - {0}", ADCModule.MinModuleNumber, ADCModule.MaxModuleNumber));
            this.Init(ModuleNumber);
        }

        public ADCModule()
        {

        }

        public override int RequestSize
        {
            get 
            {
                return 10;
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
                return 1;
            }
        }

        protected override int ComputeModuleNumber(int index)
        {
            return (index - MinIndex) / 4 + MinModuleNumber;
        }

        protected override void GenerateVarsNames(int ModuleNumber)
        {
            this.VarNames = new ModuleVarDescription[4];
            this.SingleByteVarNames = new ModuleVarDescription[4];
            for (int i = 0; i < 4; i++)
            {
                int n = (ModuleNumber - 0x81) * 4 + 9 + i;
                int n1 = (ModuleNumber - 0x81);
                this.VarNames[i] = new ModuleVarDescription("ADC" + n, "_ADC.D" + (i + 1) + '[' + n1 + ']');
                this.SingleByteVarNames[i] = new ModuleVarDescription("ADH" + n, "_Sys4x_" + "_ADC" + n + ".BYTE");
            }
        }
    }
}
