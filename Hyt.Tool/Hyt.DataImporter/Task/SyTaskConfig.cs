using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Hyt.Model;
using Hyt.ProductImport;

namespace Hyt.DataImporter.Task
{
    public class SyTaskConfig :BaseTask
    {
        public override void Read()
        {
            string sSql = "SELECT *FROM  ImportData.dbo.SyTaskConfig";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "SyTaskConfig");

            }
        }

        /*SysNo, TypeName, TaskName, TaskDescription, Timetype, TimeQuantum, 
         * DayOfWeek, Month, ExecuteTime, StartTime, EndTime, EnabledEndTime, LastExecuteTime, CreateTime, IsAgain,
         * MaxAgainCount, FailureCount, LastMessage, Status*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("TypeName", "TypeName");
            bcp.ColumnMappings.Add("TaskName", "TaskName");
            bcp.ColumnMappings.Add("TaskDescription", "TaskDescription");
            bcp.ColumnMappings.Add("Timetype", "Timetype");
            bcp.ColumnMappings.Add("TimeQuantum", "TimeQuantum");
            bcp.ColumnMappings.Add("DayOfWeek", "DayOfWeek");
            bcp.ColumnMappings.Add("Month", "Month");
            bcp.ColumnMappings.Add("ExecuteTime", "ExecuteTime");
            bcp.ColumnMappings.Add("StartTime", "StartTime");
            bcp.ColumnMappings.Add("EndTime", "EndTime");
            bcp.ColumnMappings.Add("EnabledEndTime", "EnabledEndTime");
            bcp.ColumnMappings.Add("LastExecuteTime", "LastExecuteTime");
            bcp.ColumnMappings.Add("CreateTime", "CreateTime");
            bcp.ColumnMappings.Add("IsAgain", "IsAgain");
            bcp.ColumnMappings.Add("MaxAgainCount", "MaxAgainCount");
            bcp.ColumnMappings.Add("FailureCount", "FailureCount");
            bcp.ColumnMappings.Add("LastMessage", "LastMessage");
            bcp.ColumnMappings.Add("Status", "Status");

        }
    }
}
