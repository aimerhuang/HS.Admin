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
    public class FeProductCommentImage :BaseTask
    {
        public override void Read()
        {

            //type 10 新闻 20 帮助  ;status 20 已审核
            //string sSql = "select " +
            //                    " a.sysno," +
            //                    "ReviewSysNo AS commentsysno, " +
            //                    "b.CreateCustomerSysNo as customersysno," +
            //                    "Photo AS imagepath," +
            //                    "(CASE WHEN a.Status=-1 THEN -10 " +
            //                "ELSE " +
            //                    "20 " +
            //            "END) AS  status " +
            //            "from ReviewPhoto a INNER JOIN Review_Master b ON a.ReviewSysNo=b.SysNo";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_FeProductCommentImage", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FeProductCommentImage");

            }
        }

        /*sysno, commentsysno, customersysno, imagepath, status */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("commentsysno", "commentsysno");
            bcp.ColumnMappings.Add("customersysno", "customersysno");
            bcp.ColumnMappings.Add("imagepath", "imagepath");
            bcp.ColumnMappings.Add("status", "status");
          
        }
    }
}
