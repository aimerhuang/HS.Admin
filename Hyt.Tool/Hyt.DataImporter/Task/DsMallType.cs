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
    public class DsMallType :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT * FROM ImportData.dbo.DsMallType ORDER BY SYSNO";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "DsMallType");

            }
        }

        /*sysno, mallcode, mallname, ispredeposit, status */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("mallcode", "mallcode");
            bcp.ColumnMappings.Add("mallname", "mallname");
            bcp.ColumnMappings.Add("ispredeposit", "ispredeposit");
            bcp.ColumnMappings.Add("status", "status");
           
        }
    }
}
