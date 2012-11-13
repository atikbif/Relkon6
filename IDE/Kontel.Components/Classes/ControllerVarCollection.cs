using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Kontel.Relkon;
using System.Text.RegularExpressions;
using Kontel;

namespace Kontel.Relkon.Classes
{
    public sealed class ControllerVarCollection : IList<ControllerVar>
    {
        #region Enumerator
        private class ControllerVarCollectionEnumerator : IEnumerator<ControllerVar>
        {
            private ControllerVarCollection collection;
            private int index = -1;

            public ControllerVarCollectionEnumerator(ControllerVarCollection Collection)
            {
                this.collection = Collection;
            }
            #region IEnumerator<RelkonVar> Members

            public ControllerVar Current
            {
                get 
                {
                    return this.collection[this.index]; 
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {

            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get 
                {
                    return this.collection[this.index];
                }
            }

            public bool MoveNext()
            {
                return (++this.index < this.collection.Count);
            }

            public void Reset()
            {
                this.index = 0;
            }

            #endregion
        }
        #endregion

        // ¬ коллекции переменные хран€тьс€ в нижеследующим пор€дке
        private ControllerSystemVarCollection systemVars; // системные переменные
        private ControllerIOVarCollection ioVars; // переменные датчиков ввода-вывода
        private ControllerEmbeddedVarCollection embeddedVars; //встроенные переменные (заводские установки)
        private List<ControllerDispatcheringVar> dispatcheringVars; 
        private List<ControllerUserVar> userVars; // пользовательские переменные       

        public ControllerVarCollection()
        {
            this.systemVars = new ControllerSystemVarCollection();
            this.ioVars = new ControllerIOVarCollection();
            this.userVars = new List<ControllerUserVar>();
            this.embeddedVars = new ControllerEmbeddedVarCollection();
            this.dispatcheringVars = new List<ControllerDispatcheringVar>();
        }
        /// <summary>
        /// ¬озвращает список системных переменных
        /// </summary>
        public ControllerSystemVarCollection SystemVars
        {
            get
            {
                return this.systemVars;
            }
        }
        /// <summary>
        /// ¬озвращает список переменных ввода-вывода
        /// </summary>
        public ControllerIOVarCollection IOVars
        {
            get
            {
                return this.ioVars;
            }
        }
        /// <summary>
        /// ¬озвращает список встроенных переменных
        /// </summary>
        public ControllerEmbeddedVarCollection EmbeddedVars
        {
            get
            {
                return this.embeddedVars;
            }
        }

        public List<ControllerDispatcheringVar> DispatcheringVars
        {
            get
            {
                return this.dispatcheringVars;
            }
        }
        /// <summary>
        /// ¬озвращает список пользовательских переменных
        /// </summary>
        public List<ControllerUserVar> UserVars
        {
            get
            {
                return this.userVars;
            }
        }
        /// <summary>
        /// ¬озвращает список пользовательских структурных переменных
        /// </summary>
        public List<ControllerStructVar> StructVars
        {            
            get
            {
                List<ControllerStructVar> res = new List<ControllerStructVar>();
                foreach (ControllerUserVar csv in userVars)
                {
                    if (csv is ControllerStructVar)
                    {
                        res.Add(csv as ControllerStructVar);
                    }
                }
                return res;
            }
        }
        /// <summary>
        /// ¬озвращает переменную по ее имени (если такой переменной нет, то
        /// возвращаетс€ null
        /// </summary>
        /// <param name="Name">»м€ переменной</param>
        public ControllerVar GetVarByName(string Name)
        {
            foreach (ControllerVar var in this)
            {
                if (var.Name == Name)
                    return var;
            }
            return null;
        }
        /// <summary>
        /// ”станавливает параметры размещени€ переменной в пам€ти контроллера
        /// (адрес, тип пам€ти)
        /// </summary>
        /// <param name="Name">»м€ переменной</param>
        /// <param name="Address">јдрес переменной</param>
        /// <param name="Memory">“ип пам€ти</param>
        public void SetVarMemoryParameters(string Name, int Address, MemoryType Memory)
        {
            ControllerVar var = this.GetVarByName(Name);
            if (var != null)
            {
                var.Address = Address;
                var.Memory = Memory;
            }
        }
        /// <summary>
        /// ¬озвращает переменную заводских установок по ее имени
        /// </summary>
        public ControllerEmbeddedVar GetEmbeddedVar(string Name)
        {
            return this.embeddedVars.GetVarByName(Name);
        }

        //public ControllerDispatcheringVar GetDispatcheringVar(string Name)
        //{
        //    return this.dispatcheringVars.GetVarByName(Name);
        //}

        /// <summary>
        /// ¬озвращает системную переменную по ее имени
        /// </summary>
        public ControllerSystemVar GetSystemVar(string Name)
        {
            foreach (ControllerSystemVar var in this.systemVars)
            {
                if (var.Name == Name)
                    return var;
            }
            return null;
        }
        /// <summary>
        /// ¬озвращает переменную ввода-вывода по ее имени
        /// </summary>
        public ControllerIOVar GetIOVar(string Name)
        {
            foreach (ControllerIOVar var in this.ioVars)
            {
                if (var.Name == Name)
                    return var;
            }
            return null;
        }
        /// <summary>
        /// ¬озвращает пользовательскую переменную по ее имени
        /// </summary>
        public ControllerUserVar GetUserVar(string Name)
        {
            foreach (ControllerUserVar var in this.userVars)
            {
                if (var.Name == Name)
                    return var;
            }
            return null;
        }        

        #region IList<RelkonVar> Members

        public int IndexOf(ControllerVar item)
        {
            if (item is ControllerEmbeddedVar)
                return this.embeddedVars.IndexOf((ControllerEmbeddedVar)item);
            if (item is ControllerDispatcheringVar)
                return this.dispatcheringVars.IndexOf((ControllerDispatcheringVar)item);
            if (item is ControllerSystemVar)
                return this.systemVars.IndexOf((ControllerSystemVar)item);
            if (item is ControllerIOVar)
                return this.ioVars.IndexOf((ControllerIOVar)item);
            else
                return this.userVars.IndexOf((ControllerUserVar)item);
        }

        public void Insert(int index, ControllerVar item)
        {
            throw new Exception("The method or operation is not allowed.");
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count + this.dispatcheringVars.Count + this.userVars.Count)
                throw new IndexOutOfRangeException("»ндекс находитс€ вне допустимого диапазона");
            if (index < this.systemVars.Count)
                this.systemVars.RemoveAt(index);
            if (index >= this.systemVars.Count && index < (this.systemVars.Count + this.ioVars.Count))
                this.ioVars.RemoveAt(index - this.systemVars.Count);
            if (index >= (this.ioVars.Count + this.systemVars.Count) && index < (this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count))
                this.embeddedVars.RemoveAt(index - this.ioVars.Count - this.systemVars.Count);
            if (index >= (this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count) && index < (this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count + this.userVars.Count))
                this.RemoveAt(index - this.systemVars.Count - this.ioVars.Count - this.embeddedVars.Count);
            if (index >= (this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count + this.userVars.Count))
                this.RemoveAt(index - this.systemVars.Count - this.ioVars.Count - this.embeddedVars.Count - this.userVars.Count);
        }

        public ControllerVar this[int index]
        {
            get
            {
                ControllerVar res = null;
                if (index < 0 || index >= this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count + this.dispatcheringVars.Count + this.userVars.Count)
                    throw new IndexOutOfRangeException("»ндекс находитс€ вне допустимого диапазона");
                if (index < this.systemVars.Count)
                    res =  this.systemVars[index];
                if (index >= this.systemVars.Count && index < (this.systemVars.Count + this.ioVars.Count))
                    res = this.ioVars[index - this.systemVars.Count];
                if (index >= (this.systemVars.Count + this.ioVars.Count) && index < (this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count))
                    res = this.embeddedVars[index - this.ioVars.Count - this.systemVars.Count];
                if (index >= (this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count) && index < (this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count + this.userVars.Count))
                    res = this.userVars[index - this.systemVars.Count - this.ioVars.Count - this.embeddedVars.Count];
                if (index >= (this.systemVars.Count + this.ioVars.Count + this.embeddedVars.Count + this.userVars.Count))
                    res = this.dispatcheringVars[index - this.systemVars.Count - this.ioVars.Count - this.embeddedVars.Count - this.userVars.Count];

                return res;
            }
            set
            {
                throw new InvalidOperationException("ƒанный способ изменени€ элементов коллекции недопустим. »спользуйте свойства SystemVars, EmbeddedVars и UserVars.");
            }
        }

        #endregion

        #region ICollection<RelkonVar> Members

        public void Add(ControllerVar item)
        {
            if (item is ControllerEmbeddedVar)
                this.embeddedVars.Add((ControllerEmbeddedVar)item);
            else if (item is ControllerDispatcheringVar)
                this.dispatcheringVars.Add((ControllerDispatcheringVar)item);
            else if (item is ControllerIOVar)
                this.ioVars.Add((ControllerIOVar)item);
            else if (item is ControllerSystemVar)
                this.systemVars.Add((ControllerSystemVar)item);
            else
                this.userVars.Add((ControllerUserVar)item);
        }

        public void AddRange(ControllerVarCollection vars)
        {
            this.embeddedVars.AddRange(vars.embeddedVars);
            this.dispatcheringVars.AddRange(vars.dispatcheringVars);
            this.systemVars.AddRange(vars.systemVars);
            this.ioVars.AddRange(vars.ioVars);
            this.userVars.AddRange(vars.userVars);
        }

        public void Clear()
        {
            this.systemVars.Clear();
            this.userVars.Clear();
            this.ioVars.Clear();
            this.embeddedVars.Clear();
            this.dispatcheringVars.Clear();
        }

        public bool Contains(ControllerVar item)
        {
            if (item is ControllerEmbeddedVar)
                return this.embeddedVars.Contains((ControllerEmbeddedVar)item);
            if (item is ControllerDispatcheringVar)
                return this.dispatcheringVars.Contains((ControllerDispatcheringVar)item);
            if (item is ControllerIOVar)
                return this.ioVars.Contains((ControllerIOVar)item);
            if(item is ControllerSystemVar)
                return this.systemVars.Contains((ControllerSystemVar)item);
            else
                return this.userVars.Contains((ControllerUserVar)item);
        }

        public void CopyTo(ControllerVar[] array, int arrayIndex)
        {
            for (int i = 0; i < this.Count; i++)
                array[i + arrayIndex] = this[i];
        }

        public int Count
        {
            get 
            {
                return this.systemVars.Count + this.ioVars.Count + this.userVars.Count + this.embeddedVars.Count + this.dispatcheringVars.Count;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        public bool Remove(ControllerVar item)
        {
            if (item is ControllerEmbeddedVar)
                return this.embeddedVars.Remove((ControllerEmbeddedVar)item);
            if (item is ControllerDispatcheringVar)
                return this.dispatcheringVars.Remove((ControllerDispatcheringVar)item);
            if(item is ControllerIOVar)
                return this.ioVars.Remove((ControllerIOVar)item);
            if(item is ControllerSystemVar)
                return this.systemVars.Remove((ControllerSystemVar)item);
            else
                return this.userVars.Contains((ControllerUserVar)item);
        }

        #endregion

        #region IEnumerable<RelkonVar> Members

        public IEnumerator<ControllerVar> GetEnumerator()
        {
            return new ControllerVarCollectionEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ControllerVarCollectionEnumerator(this);
        }

        #endregion
    }
}
