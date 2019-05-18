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
    public class FnPaymentVoucher :BaseTask
    {
        public override void Read()
        {

        //    string sSql = "SELECT " +  
        //                        "fso.SysNo," +
        //                        //"NULL AS  TransactionSysNo," +
        //                        "50 AS Source, " +
        //                        "NULL AS  TransactionSysNo, " +
        //                        "OrderSysno AS SourceSysNo, " +
        //                        "IncomeAmt as PayableAmount," +									    //应付金额
        //                        "(CASE WHEN fso.Status=0 THEN 0 " +
        //                            "WHEN fso.Status=1 THEN IncomeAmt " +						
        //                            "END ) AS PaidAmount, " +										//已付金额
        //                        "rmam.CustomerSysNo, " +
        //                        "rmam.RefundBank," +
        //                        "rmam.RefundAccountName, " +
        //                        "rmam.RefundAccount," +
        //                        "(CASE WHEN fso.status=-1 THEN -10 " +
        //                            "WHEN fso.status=0 THEN  10 " +
        //                            "WHEN fso.status =1 THEN 20 " +
        //                        "END) AS STATUS, " +												//状态:待付款(10),部分付款(15),已付款(20),作废(-10)
        //                        "rom.Note AS Remarks," +
        //                        "rom.CreateTime AS CreatedDate," +
        //                        "rom.CreateUserSysNo AS CreatedBy," +
        //                        "fso.ConfirmUserSysNo AS PayerSysNo," +
        //                        "fso.ConfirmTime AS PayDate," +
        //                        "NULL AS LastUpdateBy," +
        //                        "NULL AS LastUpdateDate " +
        //"FROM dbo.Finance_SOIncome fso LEFT JOIN dbo.RO_Master rom ON fso.OrderSysNo=rom.SysNo " +
        //                            "LEFT JOIN dbo.RMA_Master rmam ON rmam.SysNo=rom.RMASysNo " +
        //"WHERE ordertype= 2 "; 

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_FnPaymentVoucher", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FnPaymentVoucher");

            }
        }
        /*SysNo, TransactionSysNo, Source, SourceSysNo, PayableAmount, PaidAmount, CustomerSysNo, RefundBank, RefundAccountName, RefundAccount, Status, Remarks, CreatedDate, CreatedBy, PayerSysNo, PayDate, LastUpdateBy, LastUpdateDate   */
        /*SysNo, TransactionSysNo, Source, SourceSysNo, PayableAmount, 
            PaidAmount, CustomerSysNo, RefundBank, RefundAccountName, RefundAccount, 
            Status, Remarks, CreatedDate, CreatedBy, PayerSysNo, PayDate, LastUpdateBy, LastUpdateDate */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("TransactionSysNo", "TransactionSysNo");
            bcp.ColumnMappings.Add("Source", "Source");
            bcp.ColumnMappings.Add("SourceSysNo", "SourceSysNo");
            bcp.ColumnMappings.Add("PayableAmount", "PayableAmount");
            bcp.ColumnMappings.Add("PaidAmount", "PaidAmount");
            bcp.ColumnMappings.Add("CustomerSysNo", "CustomerSysNo");
            bcp.ColumnMappings.Add("RefundBank", "RefundBank");
            bcp.ColumnMappings.Add("RefundAccountName", "RefundAccountName");
            bcp.ColumnMappings.Add("RefundAccount", "RefundAccount");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("Remarks", "Remarks");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("PayerSysNo", "PayerSysNo");
            bcp.ColumnMappings.Add("PayDate", "PayDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
        }

    }
}
