using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace ParameterLogBinaryFile.Helpers
{
    public class ParameterLogBinaryFileReader
    {
        public ParameterLogBinaryFileReader(TaskFactory taskfactory, string filename)
        {
            f = taskfactory;
            this.filename = filename;
        }

        private TaskFactory f;
        private string filename;

        private Parameter[] Parameters;
        long DataPosition = 0;
        public List<Row> RowsRead = new List<Row>();

        public void ReadHeader()
        {
            var returnTemp = HeaderTools.Read(filename);
            Parameters = returnTemp.Parameters;
            DataPosition = returnTemp.DataPosition;
        }

        public void ReadData()
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(filename));

            reader.BaseStream.Position = DataPosition;

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                //var RowBytes = ParameterTools.RowBytes(Parameters);
                //var bytesTemp = reader.ReadBytes(RowBytes);

                Row row = new Row();

                //Read Time
                var ticks = reader.ReadInt64();
                DateTime TimeTemp = new DateTime(ticks);
                row.Time = TimeTemp;

                row.Data = new object[Parameters.Length];

                for (int i = 0; i < Parameters.Length; i++)
                {
                    switch (Parameters[i].ParamType)
                    {
                        case ParameterType.BYTE: break;
                        case ParameterType.SBYTE: break;
                        case ParameterType.UINT16: break;
                        case ParameterType.UINT32: break;
                        case ParameterType.UINT64: break;
                        case ParameterType.INT16: break;
                        case ParameterType.INT32: break;
                        case ParameterType.INT64: break;
                        case ParameterType.FLOAT: break;
                        case ParameterType.DOUBLE:
                            var tempData = reader.ReadDouble();
                            row.Data[i] = tempData;
                            break;
                        default: break;
                    }
                }

                RowsRead.Add(row);
            }
        }
    }
}
