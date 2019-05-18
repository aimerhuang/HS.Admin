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
    public class SyPrivilege : BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM ImportData.dbo.SyPrivilege ORDER BY sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "SyPrivilege");

            }
        }

        /*sysno,name, code, status, description*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("name", "name");
            bcp.ColumnMappings.Add("code", "code");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("description", "description");
     
        }
    }
}
