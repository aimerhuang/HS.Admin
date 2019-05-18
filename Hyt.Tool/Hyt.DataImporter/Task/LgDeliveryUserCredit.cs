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
    public class LgDeliveryUserCredit :BaseTask
    {
        public override void Read()
        {

            //string sSql = "SELECT  SysNo AS deliveryusersysno," +
            //                "10000 AS deliverycredit ," +
            //                "10000 AS remainingdeliverycredit," +
            //                "10000 AS borrowingcredit," +
            //                "10000 AS remainingborrowingcredit," +
           //                 "1 AS isallowborrow," +
           //                 "1  AS isallowdelivery," +
           //                 "2 AS createdby," +
           //                 "GETDATE() AS createddate," +
           //                 "2 AS lastupdateby," +
           //                 "GETDATE() AS lastupdatedate," +
           //                 "2 AS warehousesysno " +							
           //             "FROM    Sys_User " +
           //             "WHERE   UserName IN ( '肖雪', '刘伟', '亢超', '陈琛', '陈宇', '向用', '刘芳', '吴艳波', '钟伦清', '孙斌', '赵佳' ) ";
                    

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_LgDeliveryUserCredit",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);
                
                myAdapter.Fill(Common.RDS, "LgDeliveryUserCredit");

            }
        }

        /*deliveryusersysno, deliverycredit, remainingdeliverycredit, borrowingcredit, remainingborrowingcredit, 
isallowborrow, isallowdelivery, createdby, createddate, lastupdateby, lastupdatedate, warehousesysno*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("deliveryusersysno", "deliveryusersysno");
            bcp.ColumnMappings.Add("deliverycredit", "deliverycredit");
            bcp.ColumnMappings.Add("remainingdeliverycredit", "remainingdeliverycredit");
            bcp.ColumnMappings.Add("borrowingcredit", "borrowingcredit");
            bcp.ColumnMappings.Add("remainingborrowingcredit", "remainingborrowingcredit");
            bcp.ColumnMappings.Add("isallowborrow", "isallowborrow");
            bcp.ColumnMappings.Add("isallowdelivery", "isallowdelivery");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("warehousesysno", "warehousesysno");
        
        }
    }
}
