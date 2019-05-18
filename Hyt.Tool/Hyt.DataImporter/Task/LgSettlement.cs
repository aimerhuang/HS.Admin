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
    public class LgSettlement : BaseTask
    {
        public override void Read()
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_LgSettlement", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "LgSettlement");

            }
        }

        /**/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("WarehouseSysNo", "WarehouseSysNo");
            bcp.ColumnMappings.Add("deliveryusersysno", "deliveryusersysno");
            bcp.ColumnMappings.Add("totalamount", "totalamount");
            bcp.ColumnMappings.Add("paidamount", "paidamount");
            bcp.ColumnMappings.Add("codamount", "codamount");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("remarks", "remarks");
            bcp.ColumnMappings.Add("auditorsysno", "auditorsysno");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("stamp", "stamp");
            bcp.ColumnMappings.Add("auditdate", "auditdate");
        }
    }
}
