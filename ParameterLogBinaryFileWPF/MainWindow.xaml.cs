using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParameterLogBinaryFileWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        TaskFactory f = new TaskFactory();

        ParameterLogBinaryFile.Helpers.ParameterLogBinaryFileWriter LogWriter;
        ParameterLogBinaryFile.Helpers.ParameterLogBinaryFileReader LogReader;

        List<ParameterLogBinaryFile.Parameter> ParameterList = new List<ParameterLogBinaryFile.Parameter>();

        private void ButtonStartClicked(object sender, RoutedEventArgs e)
        {


            for (int i = 0; i < 10; i++)
            {
                ParameterList.Add(new ParameterLogBinaryFile.Parameter()
                {
                    Name = "Param" + (i + 1).ToString(),
                    ParamType = ParameterLogBinaryFile.ParameterType.DOUBLE,
                });
            }

            Stopwatch swWrite = new Stopwatch();
            swWrite.Start();
            LogWriter = new ParameterLogBinaryFile.Helpers.ParameterLogBinaryFileWriter(f, "LogTest1.dat", ParameterList.ToArray());
            LogWriter.WriteHeader();
            LogWriter.StartLogging();


            for (int i = 0; i < 11000000; i++)
            {
                List<byte> RowBuffer = new List<byte>();

                DateTime NOW = DateTime.UtcNow;
                AddToByteList(NOW.Ticks, RowBuffer);

                for (int j = 0; j < ParameterList.Count; j++)
                {
                    AddToByteList(j / 10.0, RowBuffer);
                }

                LogWriter.AddRow(RowBuffer.ToArray());

                //System.Threading.Thread.Sleep(100);
            }

            LogWriter.StopLogging();

            while (LogWriter.LoggingActive)
            {
                System.Threading.Thread.Sleep(100);
            }
            swWrite.Stop();

            Stopwatch swRead = new Stopwatch();
            swRead.Start();
            LogReader = new ParameterLogBinaryFile.Helpers.ParameterLogBinaryFileReader(f, "LogTest1.dat");
            LogReader.ReadHeader();
            LogReader.ReadData();
            swRead.Stop();
        }

        private void AddToByteList(double data, List<byte> buffer)
        {
            byte[] temp = BitConverter.GetBytes(data);
            for (int i = 0; i < temp.Length; i++)
            {
                buffer.Add(temp[i]);
            }
        }

        private void AddToByteList(long data, List<byte> buffer)
        {
            byte[] temp = BitConverter.GetBytes(data);
            for (int i = 0; i < temp.Length; i++)
            {
                buffer.Add(temp[i]);
            }
        }
    }
}
