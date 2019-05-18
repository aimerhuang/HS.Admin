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
    public class FeProductComment : BaseTask
    {
        public override void Read()
        {

            //string sSql = "SELECT *FROM ImportData.dbo.FeProductComment";
                           
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.PROC_FeProductComment",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FeProductComment");

            }
        }

        /*SysNo, OrderSysNo, ProductSysNo, CustomerSysNo, Title, Content, Score, Advantage, Disadvantage, IsBest, IsTop, SupportCount, UnSupportCount, 
         * ReplyCount, CommentTime, IsComment, IsShare, ShareTime, ShareTitle, ShareContent, CommentStatus, ShareStatus*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("OrderSysNo", "OrderSysNo");
            bcp.ColumnMappings.Add("ProductSysNo", "ProductSysNo");
            bcp.ColumnMappings.Add("CustomerSysNo", "CustomerSysNo");
            bcp.ColumnMappings.Add("Title", "Title");
            bcp.ColumnMappings.Add("Content", "Content");
            bcp.ColumnMappings.Add("Score", "Score");
            bcp.ColumnMappings.Add("Advantage", "Advantage");
            bcp.ColumnMappings.Add("Disadvantage", "Disadvantage");
            bcp.ColumnMappings.Add("IsBest", "IsBest");
            bcp.ColumnMappings.Add("IsTop", "IsTop");
            bcp.ColumnMappings.Add("SupportCount", "SupportCount");
            bcp.ColumnMappings.Add("UnSupportCount", "UnSupportCount");
            bcp.ColumnMappings.Add("ReplyCount", "ReplyCount");
            bcp.ColumnMappings.Add("CommentTime", "CommentTime");
            bcp.ColumnMappings.Add("IsComment", "IsComment");
            bcp.ColumnMappings.Add("IsShare", "IsShare");
            bcp.ColumnMappings.Add("ShareTime", "ShareTime");
            bcp.ColumnMappings.Add("ShareTitle", "ShareTitle");
            bcp.ColumnMappings.Add("ShareContent", "ShareContent");
            bcp.ColumnMappings.Add("CommentStatus", "CommentStatus");
            bcp.ColumnMappings.Add("ShareStatus", "ShareStatus");

        }
    }
}
