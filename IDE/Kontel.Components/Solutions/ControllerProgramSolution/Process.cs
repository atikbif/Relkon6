using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kontel.Relkon.Solutions
{
    public sealed class ProjectProcess
    {
        /// <summary>
        /// Список ситуаций процесса
        /// </summary>
        public List<ProjectSituation> Situations { get; set; }
        /// <summary>
        /// Индекс процесса
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// Адрес процесса
        /// </summary>
        [XmlAttribute]
        public int Address { get; set; }

        public ProjectProcess(string name, int address)
        {
            this.Situations = new List<ProjectSituation>();
            this.Name = name;
            this.Address = address;
        }
        public ProjectProcess() 
        {
            this.Situations = new List<ProjectSituation>();
        }

        public ProjectSituation GetSituationByAddress(int address)
        {
            foreach (ProjectSituation p in this.Situations)
            {
                if (p.Address == address) return p;
            }
            return null;
        }
  }

    public class ProjectSituation
    {
        /// <summary>
        /// Индекс ситуации
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// Адрес ситуации
        /// </summary>
        [XmlAttribute]
        public int Address { get; set; }

        public ProjectSituation(){}

        public ProjectSituation(string name, int address)
        {
            this.Name = name;
            this.Address = address;
        }
    }

}
