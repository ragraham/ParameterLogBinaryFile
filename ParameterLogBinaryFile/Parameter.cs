using System;
using System.Collections.Generic;
using System.Text;

namespace ParameterLogBinaryFile
{
    public class Parameter
    {
        public string Name;
        public ParameterType ParamType;
        public string Units;
        public string Description;
    }

    public enum ParameterType
    {
        BYTE,
        SBYTE,
        UINT16,
        UINT32,
        UINT64,
        INT16,
        INT32,
        INT64,
        FLOAT,
        DOUBLE,
    }

    public static class ParameterTools
    {
        public static int RowBytes(Parameter[] Parameters)
        {
            int Bytes = 8; //Ticks is long
            for (int i = 0; i < Parameters.Length; i++)
            {
                switch (Parameters[i].ParamType)
                {
                    case ParameterType.BYTE: Bytes++; break;
                    case ParameterType.SBYTE: Bytes++; break;
                    case ParameterType.UINT16: Bytes += 2; break;
                    case ParameterType.UINT32: Bytes += 4; break;
                    case ParameterType.UINT64: Bytes += 8; break;
                    case ParameterType.INT16: Bytes += 2; break;
                    case ParameterType.INT32: Bytes += 4; break;
                    case ParameterType.INT64: Bytes += 8; break;
                    case ParameterType.FLOAT: Bytes += 4; break;
                    case ParameterType.DOUBLE: Bytes += 8; break;
                }
            }
            return Bytes;
        }
    }
}
