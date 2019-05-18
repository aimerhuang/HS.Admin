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
    public class FeArticleCategory :BaseTask
    {
        public override void Read()
        {

            //type 10 新闻 20 帮助  ;status 20 已审核
            //string sSql = "select ROW_NUMBER() OVER(ORDER BY type ) AS SysNo,type,name,DESCRIPTION,displayorder,STATUS " +
              //            "from  " +
                //                "(select " +
                  //                  "20 AS type,title AS  name,note as description,ordernum as displayorder,20 AS STATUS from dbo.ArticleTheme " +
                    //              "UNION all " +
                      //              "select 10 AS type,'商城公告' AS  name,'商城公告' as description,1 as displayorder,20 AS STATUS " +
                        //          "UNION all " +
                          //          "select 10 AS type,'促销活动' AS  name,'促销活动' as description,2 as displayorder,20 AS STATUS " +
                            //    ") a"; 

            //string sSql = "select *from ImportData.dbo.FeArticleCategory";
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.Proc_FeArticleCategory", myConn);
                command.CommandType=CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FeArticleCategory");

            }
        }

        /* sysno, type, name, description, displayorder, status */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("type", "type");
            bcp.ColumnMappings.Add("name", "name");
            bcp.ColumnMappings.Add("description", "description");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("status", "status");
          
        }
    }
}
