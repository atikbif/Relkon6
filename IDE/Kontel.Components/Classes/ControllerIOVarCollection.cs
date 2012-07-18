using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.Classes
{
    public sealed class ControllerIOVarCollection : List<ControllerIOVar>
    {
        public ControllerIOVar GetVarByName(string Name)
        {
            foreach (ControllerIOVar var in this)
            {
                if (var.Name == Name)
                    return var;
            }
            return null;
        }
    }
}
