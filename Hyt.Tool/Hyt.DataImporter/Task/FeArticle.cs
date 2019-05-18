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
    public class FeArticle :BaseTask
    {
        public override void Read()
        {

            //type 10 新闻 20 帮助  ;status 20 已审核
            //string sSql = "SELECT *FROM  ImportData.dbo.FeArticle";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlCommand command = new SqlCommand("ImportData.dbo.Proc_FeArticle",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FeArticle");

            }
        }

        /* sysno, categorysysno, title, content, createdby, createddate, lastupdateby, lastupdatedate, status */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("categorysysno", "categorysysno");
            bcp.ColumnMappings.Add("title", "title");
            bcp.ColumnMappings.Add("content", "content");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("status", "status");
          
        }
    }
}
