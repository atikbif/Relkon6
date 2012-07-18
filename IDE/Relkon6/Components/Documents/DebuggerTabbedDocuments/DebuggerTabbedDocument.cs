using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Solutions;
using Kontel.Relkon.Debugger;

namespace Kontel.Relkon.Components.Documents
{
    /// <summary>
    /// Базовый класс для документов отладчика
    /// </summary>
    public class DebuggerTabbedDocument : RelkonTabbedDocument
    {
        protected int codingType = 16; // система счисления, в которой отобража
        protected DebuggerEngine debuggerEngine = null; // движок отладчика, который обслуживает компонент

        /// <summary>
        /// Обновляет данные, отображаемые документом на основани передаваемого проекта
        /// </summary>
        public virtual void Update(ControllerProgramSolution solution, DebuggerEngine engine)
        {
            //throw new Exception("Метод DebuggerTabbedDocument.Update() надо перегрузить в потомке");
        }
        /// <summary>
        /// Изменяет отображение данных в соответствии с новой системой счисления
        /// </summary>
        public virtual void UpdateDataPresentation(bool HEX)
        {
            this.codingType = HEX ? 16 : 10;
        }

        public DebuggerTabbedDocument(ControllerProgramSolution solution, DebuggerEngine Engine)
            : base(solution)
        {
            this.codingType = MainForm.MainFormInstance.Hex ? 16 : 10;
            this.debuggerEngine = Engine;
        }

        public DebuggerTabbedDocument()
            : base()
        {

        }
    }
}
