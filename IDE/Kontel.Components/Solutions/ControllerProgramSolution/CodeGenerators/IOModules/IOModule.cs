using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Kontel.Relkon;

namespace Kontel.Relkon.Solutions
{
    /// <summary>
    /// Базовый класс для описания подключаемых модулей ВВ 
    /// </summary>
    public abstract class IOModule
    {
        /// <summary>
        /// Возвращает описание модуля ВВ; тип модуля определяется по имени переменной
        /// </summary>
        /// <param name="VarName">Имя переменной модуля ВВ</param>
        public static IOModule Create(string VarName)
        {
            Match m = Regex.Match(VarName, @"^([A-Z]+)(\d+)$");
            IOModule res = null;
            int index = 0;
            if (!int.TryParse(m.Groups[2].Value, out index))
                throw new Exception("Неверное имя переменной: " + VarName + " не является корректным именем переменной модуля ввода-вывода");
            string prefix = m.Groups[1].Value;
            if (prefix == "IN")
                res = DINModule.Create(index);
            else if (prefix == "OUT")
                res = DOUTModule.Create(index);
            else if (prefix == "ADC" || prefix == "ADH")
                res = ADCModule.Create(index);
            else if (prefix == "DAC" || prefix == "DAH")
                res = DACModule.Create(index);
            else
                throw new Exception("Неверное имя переменной: " + VarName + " не является корректным именем переменной модуля ввода-вывода");
            return res;
        }
        /// <summary>
        /// Возвращает описание модуля ВВ; тип модуля определяется по имени переменной
        /// </summary>
        /// <param name="VarName">Имя переменной модуля ВВ</param>
        public static IOModule Create(int ModuleNumber)
        {
            IOModule res = null;
            if (ModuleNumber >= DINModule.MinModuleNumber && ModuleNumber <= DINModule.MaxModuleNumber)
                res = new DINModule(ModuleNumber);
            else if (ModuleNumber >= DOUTModule.MinModuleNumber && ModuleNumber <= DOUTModule.MaxModuleNumber)
                res = new DOUTModule(ModuleNumber);
            else if (ModuleNumber >= ADCModule.MinModuleNumber && ModuleNumber <= ADCModule.MaxModuleNumber)
                res = new ADCModule(ModuleNumber);
            else if (ModuleNumber >= DACModule.MinModuleNumber && ModuleNumber <= DACModule.MaxModuleNumber)
                res = new DACModule(ModuleNumber);
            else
                throw new Exception("Неверный номер модуля ввода-вывода");
            return res;
        }
        /// <summary>
        /// Выполняет инициализацию полей класса
        /// </summary>
        /// <param name="ModuleNumber">Физический номер модуля</param>
        protected void Init(int ModuleNumber)
        {
            this.ModuleNumber = ModuleNumber;
            this.GenerateVarsNames(this.ModuleNumber);
            this.Req10msDefinition = "0x" + Utils.AddChars('0', AppliedMath.DecToHex(this.ModuleNumber), 4);
            foreach (ModuleVarDescription VarDescription in this.VarNames)
            {
                this.Req10msDefinition += ", (unsigned int)&" + VarDescription.DisplayName;
            }
            this.Req10msDefinition += ", (unsigned int)&E" + this.VarNames[0].DisplayName;
        }
        /// <summary>
        /// Возвращает строку, задающую модуль в массиве _Sys4x_Req10ms
        /// </summary>
        public string Req10msDefinition { get; private set; }
        /// <summary>
        /// Генерирует список имен переменных, соответствующих модулю ВВ
        /// </summary>
        /// <param name="ModuleNumber">Физический номер модуля ВВ</param>
        protected abstract void GenerateVarsNames(int ModuleNumber);
        /// <summary>
        /// Возвращает список имен переменных, соответствующих модулю
        /// </summary>
        public ModuleVarDescription[] VarNames { get; protected set; }
        /// <summary>
        /// Возвращает физический номер модуля
        /// </summary>
        public int ModuleNumber { get; protected set; }
        /// <summary>
        /// Вычисляет физический номер модуля
        /// </summary>
        protected abstract int ComputeModuleNumber(int varIndex);
        /// <summary>
        /// Возвращает или устанавливает минимальный период ситуации, в которой вызывается модуль
        /// </summary>
        public int Period { get; set; }
        /// <summary>
        /// Возвращает или устанавливает фактический период опроса модуля
        /// </summary>
        public int RealPeriod { get; set; }
        /// <summary>
        /// Возвращает суммарный размер в байтах запроса к модулю и ответа модуля
        /// </summary>
        public abstract int RequestSize { get; }
        /// <summary>
        /// Возвращает время в мс на запрос к модулю
        /// </summary>
        public abstract double RequestTime { get; }
        /// <summary>
        /// Возвращает время в мс на ответ от модуля
        /// </summary>
        public abstract double ReplyTime { get; }
    }
    /// <summary>
    /// Содержит описание переменной модуля
    /// </summary>
    public class ModuleVarDescription
    {
        /// <summary>
        /// Возвращает отображаемое пользователю имя переменной модуля
        /// </summary>
        public string DisplayName { get; private set; }
        /// <summary>
        /// Возвраща
        /// </summary>
        public string SystemName { get; private set; }

        public ModuleVarDescription(string DisplayName, string SystemName)
        {
            this.DisplayName = DisplayName;
            this.SystemName = SystemName;
        }
    }
}
