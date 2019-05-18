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
    public class LgSettlementItem :BaseTask
    {
        public override void Read()
        {

           
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_LgSettlementItem", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "LgSettlementItem");

            }
        }

        /*SysNo, SettlementSysNo, TransactionSysNo, DeliverySysNo, StockOutSysNo, PayType,
         * PayAmount, PayNo, Status, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("SettlementSysNo", "SettlementSysNo");
            bcp.ColumnMappings.Add("TransactionSysNo", "TransactionSysNo");
            bcp.ColumnMappings.Add("DeliverySysNo", "DeliverySysNo");
            bcp.ColumnMappings.Add("StockOutSysNo", "StockOutSysNo");
            bcp.ColumnMappings.Add("PayType", "PayType");
            bcp.ColumnMappings.Add("PayAmount", "PayAmount");
            bcp.ColumnMappings.Add("PayNo", "PayNo");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
           
        }
    }
}
