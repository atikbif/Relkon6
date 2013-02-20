using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kontel.Relkon.Solutions
{
    [XmlInclude(typeof(Pult2x16)), XmlInclude(typeof(Pult4x20)), XmlInclude(typeof(Pult2x12))]
    public abstract class PultType
    {
        protected int rowCount = 0;
        protected int symbolsInRow = 0;
        protected string caption = "";
        /// <summary>
        /// Возвращает число строк в пульте
        /// </summary>
        public int RowCount
        {
            get
            {
                return this.rowCount;
            }
        }
        /// <summary>
        /// Возвращает число символов в строке пульта
        /// </summary>
        public int SymbolsInRow
        {
            get
            {
                return this.symbolsInRow;
            }
        }

        public static Pult2x16 Pult2x16
        {
            get
            {
                return new Pult2x16();
            }
        }

        public static Pult4x20 Pult4x20
        {
            get
            {
                return new Pult4x20();
            }
        }

        public static Pult2x12 Pult2x12
        {
            get
            {
                return new Pult2x12();
            }
        }

        public override string ToString()
        {
            return this.caption;
        }

        public override bool Equals(object obj)
        {
            return ((obj is PultType) && (((PultType)obj).caption == this.caption));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Возвращает класс, описывающий пульт с указанным числом
        /// строк  символов в каждой из них
        /// </summary>
        public static PultType GetPultType(int RowCount, int SymbolsInRow)
        {
            PultType res = PultType.Pult2x16;
            switch(RowCount)
            {
                case 2:
                    if (SymbolsInRow == 12)
                        res = PultType.Pult2x12;
                    break;
                case 4:
                    res = PultType.Pult4x20;
                    break;
            }
            return res;
        }
    }

    public sealed class Pult2x16 : PultType
    {
        public Pult2x16()
        {
            this.rowCount = 2;
            this.symbolsInRow = 16;
            this.caption = "ПУ-102";
        }
    }

    public sealed class Pult4x20 : PultType
    {
        public Pult4x20()
        {
            this.rowCount = 4;
            this.symbolsInRow = 20;
            this.caption = "ПУ-132Щ";
        }
    }

    public sealed class Pult2x12 : PultType
    {
        public Pult2x12()
        {
            this.rowCount = 2;
            this.symbolsInRow = 12;
            this.caption = "ПУ-212";
        }
    }
}
