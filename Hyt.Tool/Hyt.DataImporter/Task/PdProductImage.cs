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
    public class PdProductImage :BaseTask
    {
        public override void Read()
        {
           string sSql = "select " +
                                "ROW_NUMBER() OVER(ORDER BY sysno) AS sysno," +
                                "sysno AS  productsysno," +
                                "('\\ImageServer\\v1formal1\\ProductImg800\\' + convert(varchar(20),sysno)) + '.jpg' AS imageurl, " +
                                "1 as status, " +
                                "ordernum AS  displayorder " +
                                "from [db-hytformal].dbo.product " +
                                "where status<>-1 order by sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "PdProductImage");

            }
        }

        /*sysno, productsysno, imageurl, status, displayorder*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("productsysno", "productsysno");
            bcp.ColumnMappings.Add("imageurl", "imageurl");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("displayorder", "displayorder");

        }
    }
}
