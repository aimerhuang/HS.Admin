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
    public class FnPaymentVoucherItem :BaseTask
    {
        public override void Read()
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_FnPaymentVoucherItem",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FnPaymentVoucherItem");

            }
        }

        /*SysNo, PaymentVoucherSysNo, TransactionSysNo, OriginalPaymentTypeSysNo, OriginalVoucherNo,
            PaymentType, Amount, VoucherNo, RefundBank, RefundAccountName,
            RefundAccount,PaymentToType, PaymentToSysNo, Status, CreatedBy,
             CreatedDate, PayerSysNo, PayDate, LastUpdateBy, LastUpdateDate */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("PaymentVoucherSysNo", "PaymentVoucherSysNo");
            bcp.ColumnMappings.Add("TransactionSysNo", "TransactionSysNo");
            bcp.ColumnMappings.Add("OriginalPaymentTypeSysNo", "OriginalPaymentTypeSysNo");
            bcp.ColumnMappings.Add("OriginalVoucherNo", "OriginalVoucherNo");

            bcp.ColumnMappings.Add("PaymentType", "PaymentType");
            bcp.ColumnMappings.Add("Amount", "Amount");
            bcp.ColumnMappings.Add("VoucherNo", "VoucherNo");
            bcp.ColumnMappings.Add("RefundBank", "RefundBank");
            bcp.ColumnMappings.Add("RefundAccountName", "RefundAccountName");

            bcp.ColumnMappings.Add("RefundAccount", "RefundAccount");
            bcp.ColumnMappings.Add("PaymentToType", "PaymentToType");
            bcp.ColumnMappings.Add("PaymentToSysNo", "PaymentToSysNo");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");

            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("PayerSysNo", "PayerSysNo");
            bcp.ColumnMappings.Add("PayDate", "PayDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
        }
    }
}
