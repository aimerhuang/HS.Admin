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
    public class WhStockIn:BaseTask
    {
        public override void Read()
        {
            
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_WhStockIn",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "WhStockIn");

            }
        }

        /*sysno,transactionsysno, warehousesysno, sourcetype, sourcesysno, deliverytype, remarks, status, 
createdby, createddate, lastupdateby, lastupdatedate, isprinted, stamp*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("transactionsysno", "transactionsysno");
            bcp.ColumnMappings.Add("warehousesysno", "warehousesysno");
            bcp.ColumnMappings.Add("sourcetype", "sourcetype");
            bcp.ColumnMappings.Add("sourcesysno", "sourcesysno");
            bcp.ColumnMappings.Add("deliverytype", "deliverytype");
            bcp.ColumnMappings.Add("remarks", "remarks");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("isprinted", "isprinted");
            bcp.ColumnMappings.Add("stamp", "stamp");
        }
    }
}
