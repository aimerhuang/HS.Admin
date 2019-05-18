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
    public class FeProductCommentReply : BaseTask
    {
        public override void Read()
        {

            //type 10 新闻 20 帮助  ;status 20 已审核
                //string sSql = "select " +
                //                "a.sysno, " +
                //                "ReviewSysNo as commentsysno," +
                //                "b.CreateCustomerSysNo as customersysno," +
                //                "replycontent," +
                //                "a.CreateDate as replydate," +
                //                "(CASE WHEN a.status=0 THEN 20 " +
                //                "WHEN a.Status=1 THEN  -10 " +
                //                "WHEN a.Status=2 THEN 10 " +
                //           "ELSE " +
                //                "-10 " +
                //           "END ) AS status " +
                //        "from Review_Reply a INNER JOIN dbo.Review_Master b ON a.ReviewSysNo=b.SysNo " +
                //            "ORDER BY a.sysno ";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.PROC_FeProductCommentReply",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FeProductCommentReply");

            }
        }

        /*sysno, commentsysno, customersysno, replycontent, replydate, status */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("commentsysno", "commentsysno");
            bcp.ColumnMappings.Add("customersysno", "customersysno");
            bcp.ColumnMappings.Add("replycontent", "replycontent");
            bcp.ColumnMappings.Add("replydate", "replydate");
            bcp.ColumnMappings.Add("status", "status");
        }
    }
}
