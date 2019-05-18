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
    public class RcReturnItem :BaseTask
    {
        public override void Read()
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_RcReturnItem", myConn);
                command.CommandType=CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "RcReturnItem");

            }
        }

        /*sysno,returnsysno, transactionsysno, productsysno, rmaquantity, returntype, returnpricetype, originprice, 
refundproductamount, stockoutitemsysno, rmareason, productname*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("returnsysno", "returnsysno");
            bcp.ColumnMappings.Add("transactionsysno", "transactionsysno");
            bcp.ColumnMappings.Add("productsysno", "productsysno");
            bcp.ColumnMappings.Add("rmaquantity", "rmaquantity");
            bcp.ColumnMappings.Add("returntype", "returntype");
            bcp.ColumnMappings.Add("returnpricetype", "returnpricetype");
            bcp.ColumnMappings.Add("originprice", "originprice");
            bcp.ColumnMappings.Add("refundproductamount", "refundproductamount");
            bcp.ColumnMappings.Add("stockoutitemsysno", "stockoutitemsysno");
            bcp.ColumnMappings.Add("rmareason", "rmareason");
            bcp.ColumnMappings.Add("productname", "productname");

        }
    }
}
