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
    public class LgDelivery : BaseTask
    {
        public override void Read()
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_lgdelivery", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "LgDelivery");

            }
        }

        /*sysno, stocksysno, deliveryusersysno, paidamount, codamount, status, isenforceallow, deliverytypesysno, 
        createdby, createddate, lastupdateby, lastupdatedate, stamp*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("stocksysno", "stocksysno");
            bcp.ColumnMappings.Add("deliveryusersysno", "deliveryusersysno");
            bcp.ColumnMappings.Add("paidamount", "paidamount");
            bcp.ColumnMappings.Add("codamount", "codamount");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("isenforceallow", "isenforceallow");
            bcp.ColumnMappings.Add("deliverytypesysno", "deliverytypesysno");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("stamp", "stamp");
        }
    }
}
