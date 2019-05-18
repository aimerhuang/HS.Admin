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
    public class FeCommentSupport :BaseTask
    {
        public override void Read()
        {

            //type 10 新闻 20 帮助  ;status 20 已审核
            //string sSql = "SELECT " +
            //                    "a.SysNo," +
            //                    "ReviewID as productcommentsysno, " +
            //                    "b.CreateCustomerSysNo as customersysno," +
            //                    "ApplyYesCount as supportcount," +
            //                    "ApplyNoCount as unsupportcount," +
            //                    "CreateUser  AS createdby," +
            //                    "a.CreateDate  AS createdate," +
            //                    "NULL as lastupdateby," +
            //                    "NULL AS  lastupdatedate " +
            //                "from Review_Apply a INNER JOIN dbo.Review_Master b ON a.ReviewID=b.SysNo " +
            //                "ORDER BY a.sysno "; 
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_FeCommentSupport", myConn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FeCommentSupport");

            }
        }

        /*sysno,productcommentsysno, customersysno, supportcount, unsupportcount, createdby, createdate, lastupdateby, lastupdatedate */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("productcommentsysno", "productcommentsysno");
            bcp.ColumnMappings.Add("customersysno", "customersysno");
            bcp.ColumnMappings.Add("supportcount", "supportcount");
            bcp.ColumnMappings.Add("unsupportcount", "unsupportcount");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createdate", "createdate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");

        }
    }
}
