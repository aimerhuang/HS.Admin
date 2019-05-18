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
    public class SPpromotionoverlay : BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM  ImportData.dbo.SPpromotionoverlay";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "SPpromotionoverlay");

            }
        }

        /*sysno, promotionsysno, overlaycode*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("promotionsysno", "promotionsysno");
            bcp.ColumnMappings.Add("overlaycode", "overlaycode");
          

        }
    }
}
