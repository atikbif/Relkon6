using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Classes;
using Kontel;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using Kontel.Relkon.CodeDom;

namespace Kontel.Relkon.Solutions
{
    public sealed class STM32F107Solution : ControllerProgramSolution
    {                           
        internal STM32F107Solution()
        {
           
        }
       
        protected override ControllerProgramSolution Clone()
        {
            STM32F107Solution res = new STM32F107Solution();
            res.LoadFromAnotherSolution(this);
            return res;
        }                      
    }
}
