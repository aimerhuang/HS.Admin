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
    public class FnOnlinePayment :BaseTask
    {
        public override void Read()
        {

            //string sSql = "SELECT " +
            //                    "SysNo, " +
            //                    "10 AS Source,"  +                                  // 10订单
            //                    "sosysno AS SourceSysNo," +
            //                    "payAmount AS  Amount," +
            //                    "(CASE WHEN PayTypeSysNo=1 THEN 7 " +
            //                    "WHEN PayTypeSysNo=2 THEN 6 " +
            //                    "WHEN PayTypeSysNo=4 THEN 5 " +
            //                    "ELSE 1 " +
            //                    "END ) AS PaymentTypeSysNo, " +
            //                    "NULL AS  VoucherNo, " +
            //                    "(CASE WHEN status=-1 THEN 0 " +
            //                    "ELSE 1  " +
            //                    "END ) AS Status," +
            //                    "InputUserSysNo AS  CreatedBy, " +
            //                    "InputTime AS CreatedDate," +
            //                    "ApproveUserSysNo AS Operator," +
            //                    "ApproveTime AS  OperatedDate, " +
            //                    "NULL AS  LastUpdateBy," +
            //                    "NULL AS  LastUpdateDate " +
            //                "FROM dbo.Finance_NetPay"; 

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.PROC_FnOnlinePayment",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FnOnlinePayment");

            }
        }

        /*SysNo, Source, SourceSysNo, Amount, PaymentTypeSysNo, VoucherNo, Status, CreatedBy, CreatedDate, Operator, OperatedDate, LastUpdateBy, LastUpdateDate */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("Source", "Source");
            bcp.ColumnMappings.Add("SourceSysNo", "SourceSysNo");
            bcp.ColumnMappings.Add("Amount", "Amount");
            bcp.ColumnMappings.Add("PaymentTypeSysNo", "PaymentTypeSysNo");
            bcp.ColumnMappings.Add("VoucherNo", "VoucherNo");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("Operator", "Operator");
            bcp.ColumnMappings.Add("OperatedDate", "OperatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
        }
    }
}
