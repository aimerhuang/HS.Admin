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
    public class PdBrand :BaseTask
    {
        public override void Read()
        {

            string sSql = "select *from ImportData.dbo.PdBrand order by sysno ";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "PdBrand");

            }
        }

        /*sysno, name, logo, displayorder, status*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("name", "name");
            bcp.ColumnMappings.Add("logo", "logo");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("status", "status");
       
        }
    }
}
