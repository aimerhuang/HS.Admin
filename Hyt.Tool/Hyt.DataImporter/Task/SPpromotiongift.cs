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
    public class SPpromotiongift :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM  ImportData.dbo.SPpromotiongift";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "SPpromotiongift");

            }
        }

        /*sysno,promotionsysno, productsysno, productname, purchaseprice, 
        requirementmaxamount, maxsalequantity, usedsalequantity, requirementminamount*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("promotionsysno", "promotionsysno");
            bcp.ColumnMappings.Add("productsysno", "productsysno");
            bcp.ColumnMappings.Add("productname", "productname");
            bcp.ColumnMappings.Add("purchaseprice", "purchaseprice");
            bcp.ColumnMappings.Add("requirementmaxamount", "requirementmaxamount");
            bcp.ColumnMappings.Add("maxsalequantity", "maxsalequantity");
            bcp.ColumnMappings.Add("usedsalequantity", "usedsalequantity");
            bcp.ColumnMappings.Add("requirementminamount", "requirementminamount");
           

        }
    }
}
