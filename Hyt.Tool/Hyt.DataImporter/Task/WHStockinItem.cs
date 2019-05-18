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
    public class WHStockinItem :BaseTask
    {
        public override void Read()
        {

           
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_WHStockinItem", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "WHStockinItem");

            }
        }

        /*sysno, stockinsysno, productsysno, productname, stockinquantity, realstockinquantity, remarks, 
createdby, createddate, lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("stockinsysno", "stockinsysno");
            bcp.ColumnMappings.Add("productsysno", "productsysno");
            bcp.ColumnMappings.Add("productname", "productname");
            bcp.ColumnMappings.Add("stockinquantity", "stockinquantity");
            bcp.ColumnMappings.Add("realstockinquantity", "realstockinquantity");
            bcp.ColumnMappings.Add("remarks", "remarks");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
          
        }
    }
}
