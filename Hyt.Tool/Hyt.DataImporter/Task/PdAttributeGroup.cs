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
    public class PdAttributeGroup : BaseTask 
    {
        public override void Read()
        {

            string sSql = "SELECT  " +
                            "a.sysno," +
                            "a.Attribute1Name AS Name ," +
                            "b.C3Name AS backendname ," +
                            "a.OrderNum AS displayorder ," +
                            "(CASE WHEN  a.Status=0 THEN 1 " +
				                "WHEN a.Status=-1 THEN 0 " +
		                     "END) AS status " +
                        "FROM    [db-hytformal].dbo.Category_Attribute1 a  LEFT JOIN [db-hytformal].dbo.Category3 b ON a.C3SysNo=b.SysNo";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "PdAttributeGroup");

            }
        }

        /*sysno, name, backendname, displayorder,status*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("name", "name");
            bcp.ColumnMappings.Add("backendname", "backendname");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("status", "status");
          
        }
    }
}
