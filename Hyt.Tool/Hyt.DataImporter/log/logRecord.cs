using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Hyt.ProductImport;

namespace Hyt.DataImporter
{
    public class logRecord
    {
        string LogFile = @"d:\log.txt";

        private void CreateLog()
        {
            StreamWriter SW;
            SW = File.CreateText(LogFile);
            SW.WriteLine("Log created at: " +
                                 DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            SW.Close();
        }

        private void WriteLog(string Log)
        {
            using (StreamWriter SW = File.AppendText(LogFile))
            {
                SW.WriteLine(Log);
                SW.Close();
            }
        }

        public void CheckLog(string Log)
        {
            if (File.Exists(LogFile))
            {
                WriteLog(Log);
            }

            else
            {
                CreateLog();
                WriteLog(Log);
            }
        }

    }
}
