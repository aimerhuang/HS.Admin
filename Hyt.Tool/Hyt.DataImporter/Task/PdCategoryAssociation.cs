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
    public class PdCategoryAssociation :BaseTask
    {
        public override void Read()
        {

            //string sSql = "SELECT " +
            //              "ROW_NUMBER() over(order by C3SysNo,SysNo) as SysNo," +  
            //             "C3SysNo AS categorysysno," +
            //             "sysno AS ProductSysNo, " +
            //             "OrderNum AS displayorder," +
            //             "1 AS IsMaster," +
            //             "2 AS createdby," +
            //             "GETDATE() AS createddate," +
            //             "2 AS lastupdateby," +
            //             "GETDATE() AS lastupdatedate " +
            //             "FROM [hyt-v2].dbo.Product ";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlCommand command = new SqlCommand("ImportData.dbo.proc_PdCategoryAssociation",myConn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "PdCategoryAssociation");

            }
        }

        /*sysno, categorysysno, productsysno, displayorder, ismaster, createdby, createddate, lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("categorysysno", "categorysysno");
            bcp.ColumnMappings.Add("productsysno", "productsysno");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("ismaster", "ismaster");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
        }
    }
}
