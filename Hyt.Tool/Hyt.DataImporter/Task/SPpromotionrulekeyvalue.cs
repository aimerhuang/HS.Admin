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
    public class SPpromotionrulekeyvalue :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM  ImportData.dbo.SPpromotionrulekeyvalue";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "SPpromotionrulekeyvalue");

            }
        }

        /*sysno, promotionsysno, rulekey, rulevalue, description */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("promotionsysno", "promotionsysno");
            bcp.ColumnMappings.Add("rulekey", "rulekey");
            bcp.ColumnMappings.Add("rulevalue", "rulevalue");
            bcp.ColumnMappings.Add("description", "description");

        }
    }
}
