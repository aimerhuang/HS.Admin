using System;
using System.Data;
using System.Data.SqlClient;
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
    public class BsCode :BaseTask
    {
        public override void Read()
        {
            string sSql = "select *from  ImportData.dbo.bscode order by sysno;";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "BsCode");

            }
        }

        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            //SysNo, ParentSysNo, CodeName, Status
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("ParentSysNo", "ParentSysNo");
            bcp.ColumnMappings.Add("CodeName", "CodeName");
            bcp.ColumnMappings.Add("Status", "Status");
           
        }
    }
}
