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
    public class WHwarehouseDeliveryType :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM ImportData.dbo.whwarehousedeliverytype ORDER BY sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "WHwarehouseDeliveryType");

            }
        }

        /*sysno, deliverytypesysno, warehousesysno, status, createdby, createddate, lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("deliverytypesysno", "deliverytypesysno");
            bcp.ColumnMappings.Add("warehousesysno", "warehousesysno");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
          
        }
    }
}
