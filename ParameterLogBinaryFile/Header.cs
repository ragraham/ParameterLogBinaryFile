using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParameterLogBinaryFile
{
    public static class HeaderTools
    {
        public static long Write(string filename, Parameter[] parameters)
        {
            string HeaderStr = "";
            for (int i = 0; i < parameters.Length; i++)
            {
                HeaderStr += parameters[i].Name + ",";
            }
            HeaderStr += "\r\n";
            for (int i = 0; i < parameters.Length; i++)
            {
                HeaderStr += parameters[i].ParamType.ToString() + ",";
            }
            HeaderStr += "\r\n";
            for (int i = 0; i < parameters.Length; i++)
            {
                HeaderStr += parameters[i].Units + ",";
            }
            HeaderStr += "\r\n";
            for (int i = 0; i < parameters.Length; i++)
            {
                HeaderStr += parameters[i].Description + ",";
            }
            HeaderStr += "\r\n";

            Int64 LengthXml = (Int64)HeaderStr.Length;

            Int64 DataPosition = 8 + LengthXml;

            BinaryWriter writer = new BinaryWriter(File.Create(filename));
            writer.Write(LengthXml);
            writer.Flush();
            writer.Close();

            StreamWriter writer2 = new StreamWriter(filename, true);
            writer2.Write(HeaderStr);
            writer2.Flush();
            writer2.Close();

            return DataPosition;
        }

        public static (Parameter[] Parameters, Int64 DataPosition) Read(string filename)
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(filename));
            Int64 LengthXml = reader.ReadInt64();
            Int64 DataPosition = 8 + LengthXml;
            reader.Close();

            StreamReader reader2 = new StreamReader(filename);
            char[] buffer = new char[LengthXml];
            reader2.BaseStream.Position = 8;
            reader2.Read(buffer, 0, buffer.Length);
            reader2.Close();

            string HeaderStr = new string(buffer);

            List<Parameter> ParamList = new List<Parameter>();
            var lines = HeaderStr.Replace("\r","").Split(new char[] { '\n' }, StringSplitOptions.None);
            string[] Names = new string[0];
            string[] ParamTypes = new string[0];
            string[] Units = new string[0];
            string[] Descriptions = new string[0];
            if (lines.Length >= 1)
            {
                Names = lines[0].Split(new char[] { ',' });
            }
            if (lines.Length >= 2)
            {
                ParamTypes = lines[1].Split(new char[] { ',' });
            }
            if (lines.Length >= 3)
            {
                Units = lines[2].Split(new char[] { ',' });
            }
            if (lines.Length >= 4)
            {
                Descriptions = lines[3].Split(new char[] { ',' });
            }

            for (int i = 0; i < Names.Length; i++)
            {
                if (!String.IsNullOrEmpty(Names[i]))
                {
                    string Name = Names[i];
                    ParameterType ParamType = ParameterType.BYTE;
                    if (ParamTypes.Length > i)
                    {
                        if (Enum.TryParse(ParamTypes[i], out ParameterType TypeOut))
                        {
                            ParamType = TypeOut;
                        }
                    }
                    string Unit = "";
                    if (Units.Length > i)
                    {
                        Unit = Units[i];
                    }
                    string Description = "";
                    if (Descriptions.Length > i)
                    {
                        Description = Descriptions[i];
                    }
                    ParamList.Add(new Parameter()
                    {
                        Name = Name,
                        ParamType = ParamType,
                        Units = Unit,
                        Description = Description,
                    });
                }
            }

            return (ParamList.ToArray(), DataPosition);
        }

        //public int ParametersCount()
        //{
        //    return Parameters.Length;
        //}

        //public ParameterType ParameterType(int index)
        //{
        //    return Parameters[index].parameterType;
        //}
    }
}
