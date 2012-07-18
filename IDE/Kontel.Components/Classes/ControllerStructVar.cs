using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// Описывает переменные, соответствующие стуктурам пользователя    
    /// </summary>
    public sealed class ControllerStructVar : ControllerUserVar
    {
        private List<ControllerUserVar> vars = new List<ControllerUserVar>();
        public List<ControllerUserVar> Vars
        {
            get
            {
                return this.vars;
            }
        }

    }
}
