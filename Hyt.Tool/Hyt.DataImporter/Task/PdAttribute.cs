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
    public class PdAttribute :BaseTask
    {
        public override void Read()
        {

            //string sSql = "SELECT " +
            //                  "SysNo," +
            //                 "Attribute2Name as AttributeName," +
            //                  "Attribute2Name  as backendname," +
            //                  "1 AS isrelationflag, " +
            //                  "(case Attribute2Type when 0 then 10 " +
            //                  "when 1 then 30 " +
            //                  "when 2 then 30 " +
            //                  " end) as attributetype," +
            //                  "1 AS issearchkey, " +
            //                  "(case when status=0 then 1 " +
            //                         "when status=-1 then 0 " +
            //                    "end) AS status,"  +
            //                  "2 as createdby, " +
            //                  "GETDATE() AS  createddate, " +
            //                  "2 AS lastupdateby, " +
            //                  "GETDATE() lastupdatedate " +

            //                  "FROM [hyt-v2].dbo.Category_Attribute2 " +
            //                  "ORDER BY sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_PdAttribute",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "PdAttribute");

            }
        }

        /*sysno, attributename, backendname, isrelationflag, issearchkey, status, createdby, 
createddate, lastupdateby, lastupdatedate, attributetype*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("attributename", "attributename");
            bcp.ColumnMappings.Add("backendname", "backendname");
            bcp.ColumnMappings.Add("isrelationflag", "isrelationflag");
            bcp.ColumnMappings.Add("issearchkey", "issearchkey");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("attributetype", "attributetype");
        }
    }
}
