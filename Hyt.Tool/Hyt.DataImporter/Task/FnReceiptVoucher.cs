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
    public class FnReceiptVoucher : BaseTask
    {
        public override void Read()
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_FnReceiptVoucher",myConn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FnReceiptVoucher");

            }
        }

        /*SysNo, TransactionSysNo, IncomeType, Source, SourceSysNo, IncomeAmount, ReceivedAmount, Status, Remark, CreatedBy, CreatedDate, ConfirmedBy, ConfirmedDate,
         * LastUpdateBy, LastUpdateDate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("TransactionSysNo", "TransactionSysNo");
            bcp.ColumnMappings.Add("IncomeType", "IncomeType");
            bcp.ColumnMappings.Add("Source", "Source");
            bcp.ColumnMappings.Add("SourceSysNo", "SourceSysNo");
            bcp.ColumnMappings.Add("IncomeAmount", "IncomeAmount");
            bcp.ColumnMappings.Add("ReceivedAmount", "ReceivedAmount");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("Remark", "Remark");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("ConfirmedBy", "ConfirmedBy");
            bcp.ColumnMappings.Add("ConfirmedDate", "ConfirmedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
        }
    }
}
