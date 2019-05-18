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
    public class FnInvoice :BaseTask
    {
        public override void Read()
        {

            //string sSql = "SELECT distinct " +
            //                    "inv.SysNo, " +
            //                    "1 AS InvoiceTypeSysNo," +					//普通发票
            //                    "(case when len(InvoiceCode)<=20 then InvoiceCode " +
            //                            "when len(InvoiceCode)>20 then '' " +
            //                    " end) as InvoiceCode," +
            //                    "(case when len(InvoiceNum)<=20 then  InvoiceNum " +
            //                            "when len(InvoiceNum)>20 then '' " +
            //                    " end) as InvoiceNo," +
            //                    "Amount as InvoiceAmount, " +
            //                    "NULL AS InvoiceRemarks," +
            //                    "sm.ReceiveName AS InvoiceTitle," +
            //                    "(CASE dom.InvoiceStatus  " +
            //                        "WHEN 1 THEN 20	" +
            //                        "WHEN 2 THEN 10 " +
            //                        "WHEN 3 THEN -10 " +
            //                        "ELSE NULL " +
            //                    "END ) AS STATUS,	" +						        //状态:待开票(10),已开票(20),已取回(30),作废(-10)
            //                    "dom.InvoiceTime as CreatedDate, " +
            //                    "dom.InvoiceUpdateUserSysNo as CreatedBy, " +
            //                    "NULL AS  LastUpdateBy, " +
            //                    "NULL AS LastUpdateDate, " +
            //                    "NULL AS TransactionSysNo " +
            //                "FROM DO_Invoice inv LEFT JOIN DO_Master dom ON inv.DoSysNo=dom.SysNo " +
            //                    "LEFT JOIN DO_Item doi ON doi.DoSysNo=dom.SysNo " +
            //                    "LEFT JOIN SO_Master sm ON sm.SysNo=doi.ReferSysNo ";
							

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_FnInvoice", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FnInvoice");

            }
        }

        /*SysNo, InvoiceTypeSysNo, InvoiceCode, InvoiceNo, InvoiceAmount, InvoiceRemarks,
            InvoiceTitle, Status, CreatedDate, CreatedBy, LastUpdateBy,
            LastUpdateDate, TransactionSysNo*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("InvoiceTypeSysNo", "InvoiceTypeSysNo");
            bcp.ColumnMappings.Add("InvoiceCode", "InvoiceCode");
            bcp.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
            bcp.ColumnMappings.Add("InvoiceAmount", "InvoiceAmount");
            bcp.ColumnMappings.Add("InvoiceRemarks", "InvoiceRemarks");
            bcp.ColumnMappings.Add("InvoiceTitle", "InvoiceTitle");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("TransactionSysNo", "TransactionSysNo");
        }
    }
}
