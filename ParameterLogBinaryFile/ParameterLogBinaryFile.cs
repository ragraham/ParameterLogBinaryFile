//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;

//namespace ParameterLogBinaryFile
//{
//    public class ParameterLogBinaryFile
//    {
//        public ParameterLogBinaryFile(TaskFactory taskfactory)
//        {
//            f = taskfactory;
//        }

//        TaskFactory f;

//        string filename = "LogTest1.dat";

//        Header header = new Header();

//        public bool AddParameters(Parameter[] parameters)
//        {
//            try
//            {
//                header.AddParameters(parameters);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public BlockingCollection<byte[]> RowsToWrite = new BlockingCollection<byte[]>();

//        public List<Row> RowsRead = new List<Row>();
        

//        public bool LoggingActive = false;

//        public void WriteHeader()
//        {
//            header.Write(filename);
//        }

//        public void ReadHeader()
//        {
//            header.Read(filename);
//        }

//        public bool AddRow(byte[] row)
//        {
//            try
//            {
//                RowsToWrite.Add(row);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public void StartLogging()
//        {
//            LoggingActive = true;
//            f.StartNew(() =>
//            {
//                BinaryWriter writer = new BinaryWriter(File.OpenWrite(filename));
//                writer.BaseStream.Position = header.DataPosition;

//                foreach (var item in RowsToWrite.GetConsumingEnumerable())
//                {
//                    //Write Data
//                    writer.Write(item);
//                }

//                writer.Flush();
//                writer.Close();
//                LoggingActive = false;
//            });
//        }

//        public void StopLogging()
//        {
//            RowsToWrite.CompleteAdding();
//        }

//        public void ReadData()
//        {
//            BinaryReader reader = new BinaryReader(File.OpenRead(filename));

//            reader.BaseStream.Position = header.DataPosition;

//            while (reader.BaseStream.Position < reader.BaseStream.Length)
//            {
//                Row row = new Row();

//                //./ Read Time
//                var ticks = reader.ReadInt64();
//                DateTime TimeTemp = new DateTime(ticks);
//                row.Time = TimeTemp;

//                row.Data = new object[header.ParametersCount()];

//                for (int i = 0; i < header.ParametersCount(); i++)
//                {
//                    switch (header.ParameterType(i))
//                    {
//                        case ParameterType.BYTE: break;
//                        case ParameterType.SBYTE: break;
//                        case ParameterType.UINT16: break;
//                        case ParameterType.UINT32: break;
//                        case ParameterType.UINT64: break;
//                        case ParameterType.INT16: break;
//                        case ParameterType.INT32: break;
//                        case ParameterType.INT64: break;
//                        case ParameterType.FLOAT: break;
//                        case ParameterType.DOUBLE:
//                            var tempData = reader.ReadDouble();
//                            row.Data[i] = tempData;
//                            break;
//                        default: break;
//                    }
//                }

//                RowsRead.Add(row);
//            }
//        }
//    }
//}
