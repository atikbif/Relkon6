using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Kontel.Relkon;

namespace Kontel.Relkon.Classes
{
    public sealed class ControllerEmbeddedVarCollection : List<ControllerEmbeddedVar>
    {
        /// <summary>
        /// Возвращает переменную заводских установок по ее имени
        /// </summary>
        public ControllerEmbeddedVar GetVarByName(string Name)
        {
            foreach (ControllerEmbeddedVar var in this)
            {
                if (var.Name == Name)
                    return var;
            }
            return null;
        }
        /// <summary>
        /// По имени переменной возвращает ее индекс,
        /// например: для Z5 - 5 и т.д.
        /// </summary>
        public int GetEmbeddedVarIndex(string VarName)
        {
            return int.Parse(Regex.Match(VarName, "\\d+").Value);
        }
        /// <summary>
        /// Возвращает все возможные переменные всех размеров, которые перекрывают 
        /// или перекрываются указанной переменной
        /// </summary>
        public List<ControllerEmbeddedVar> GetAllAssignedEmbeddedVars(string VarName)
        {
            string prefix = Regex.Match(VarName, @"^[^\d]+").Value;
            int StartIndex = this.GetEmbeddedVarIndex(VarName) / 4 * 4;
            int[] index = { StartIndex, StartIndex + 1, StartIndex + 2, StartIndex + 3 };
            List<ControllerEmbeddedVar> res = new List<ControllerEmbeddedVar>();
            for (int i = 0; i < 4; i++)
            {
                res.Add(this.GetVarByName(prefix + index[i]));
                if (i % 2 == 0)
                    res.Add(this.GetVarByName(prefix + index[i] + "i"));
                if (i % 4 == 0)
                    res.Add(this.GetVarByName(prefix + index[i] + "l"));
            }
            return res;
        }
        /// <summary>
        /// Возвращает масив байт который обрузует блок из 4 переменных
        /// </summary>
        /// <param name="Vars">Список всех переменных, которые попадают в заданый блок</param>
        /// <param name="VarSize">Размер переменной, относительно который вычисляется блок</param>
        private int[] CreateVarBytesArray(List<ControllerEmbeddedVar> Vars, int VarSize, bool InverseByteOrder)
        {
            int[] res = new int[4];
            switch (VarSize)
            {
                case 1:
                    res[0] = (int)Vars[0].Value;
                    res[1] = (int)Vars[3].Value;
                    res[2] = (int)Vars[4].Value;
                    res[3] = (int)Vars[6].Value;
                    break;
                case 2:
                    res[0] = !InverseByteOrder ? AppliedMath.Hi((int)Vars[1].Value) : AppliedMath.Low((int)Vars[1].Value);
                    res[1] = !InverseByteOrder ? AppliedMath.Low((int)Vars[1].Value) : AppliedMath.Hi((int)Vars[1].Value);
                    res[2] = !InverseByteOrder ? AppliedMath.Hi((int)Vars[5].Value) : AppliedMath.Low((int)Vars[5].Value);
                    res[3] = !InverseByteOrder ? AppliedMath.Low((int)Vars[5].Value) : AppliedMath.Hi((int)Vars[5].Value);
                    break;
                case 4:
                    byte[] tmp = InverseByteOrder ? Utils.ReflectArray(AppliedMath.IntToBytes((int)Vars[2].Value)) : AppliedMath.IntToBytes((int)Vars[2].Value);
                    for (int i = 0; i < 4; i++)
                        res[i] = tmp[i];
                    break;
            }
            return res;
        }
        /// <summary>
        /// Устанавливает новые значения переменных заводских установок при изменении значения какой либо одной
        /// </summary>
        public void SetEmbeddedVarValue(string VarName, long value, bool InverseByteOrder)
        {
            ControllerEmbeddedVar WorkingVar = this.GetVarByName(VarName);
            if (WorkingVar == null)
                return;
            WorkingVar.Value = value;
            List<ControllerEmbeddedVar> vars = this.GetAllAssignedEmbeddedVars(VarName);
            int[] ibytes = this.CreateVarBytesArray(vars, WorkingVar.Size, InverseByteOrder);
            byte[] bytes = { (byte)ibytes[0], (byte)ibytes[1], (byte)ibytes[2], (byte)ibytes[3] };
            // Установка значений однобайтных переменных
            vars[0].Value = ibytes[0];
            vars[3].Value = ibytes[1];
            vars[4].Value = ibytes[2];
            vars[6].Value = ibytes[3];
            // Установка значений двухбайтных переменных
            if (WorkingVar != vars[1])
                vars[1].Value = AppliedMath.BytesToInt(InverseByteOrder ? Utils.ReflectArray<byte>(Utils.GetSubArray<byte>(bytes, 0, 2)) : Utils.GetSubArray<byte>(bytes, 0, 2));
            if (WorkingVar != vars[5])
                vars[5].Value = AppliedMath.BytesToInt(InverseByteOrder ? Utils.ReflectArray<byte>(Utils.GetSubArray<byte>(bytes, 2)) : Utils.GetSubArray<byte>(bytes, 2));
            // Установка значений четырехбайтной переменной
            if (WorkingVar != vars[2])
                vars[2].Value = (uint)AppliedMath.BytesToInt(InverseByteOrder ? Utils.ReflectArray<byte>(bytes) : bytes);

        }
    }
}
