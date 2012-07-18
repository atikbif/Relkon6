using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.Classes
{
    public sealed class ControllerSystemVarCollection : List<ControllerSystemVar>
    {
        public ControllerSystemVar GetVarByName(string Name)
        {
            foreach (ControllerSystemVar var in this)
            {
                if (var.Name == Name)
                    return var;
            }
            return null;
        }
    }
}
