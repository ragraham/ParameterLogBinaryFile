using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace ParameterLogBinaryFile.Helpers
{
    public class ParameterLogBinaryFileWriter
    {
        public ParameterLogBinaryFileWriter(TaskFactory taskfactory, string filename, Parameter[] parameters)
        {
            f = taskfactory;
            this.filename = filename;
            this.Parameters = parameters;
        }

        private TaskFactory f;
        private string filename;

        private Parameter[] Parameters = new Parameter[0];

        public BlockingCollection<byte[]> RowsToWrite = new BlockingCollection<byte[]>();

        public bool LoggingActive = false;

        Int64 DataPosition = 0;

        public void WriteHeader()
        {
            DataPosition = HeaderTools.Write(filename, Parameters);
        }

        public bool AddRow(byte[] row)
        {
            try
            {
                RowsToWrite.Add(row);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void StartLogging()
        {
            LoggingActive = true;
            f.StartNew(() =>
            {
                BinaryWriter writer = new BinaryWriter(File.OpenWrite(filename));
                writer.BaseStream.Position = DataPosition;                

                foreach (var item in RowsToWrite.GetConsumingEnumerable())
                {
                    //Write Data
                    writer.Write(item);
                }

                writer.Flush();
                writer.Close();
                LoggingActive = false;
            });
        }

        public void StopLogging()
        {
            RowsToWrite.CompleteAdding();
        }
    }
}
