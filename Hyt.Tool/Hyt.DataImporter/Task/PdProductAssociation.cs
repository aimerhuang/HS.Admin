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
    public class PdProductAssociation :BaseTask
    {
        public override void Read()
        {
            //string sSql = "SELECT ROW_NUMBER() OVER(ORDER BY p.SysNo, pai.Attribute2SysNo, pai.PASysNo) AS SYSNO," +
            //                    "pai.Attribute2SysNo AS attributesysno," +
            //                    "p.SysNo AS productsysno," +
            //                    "pai.PASysNo AS relationcode," +
            //                    "ca2.OrderNum AS displayorder," +
            //                    "NULL  AS createdby," +
            //                    "OnlineDateTime AS createddate," +
            //                    "NULL AS lastupdateby," +
            //                    "UpdateNewProductTime AS lastupdatedate " +
            //              "from [hyt-v2].dbo.Product AS p " +
            //                    "INNER JOIN [hyt-v2].dbo.Product_Associate_Item AS pai ON p.ProductAssociateSysNo = pai.PASysNo " +
            //                    "INNER JOIN [hyt-v2].dbo.Category_Attribute2 AS ca2 ON ca2.SysNo = pai.Attribute2SysNo "; 
                         
 
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_PdProductAssociation",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "PdProductAssociation");

            }
        }

        /*sysno, attributesysno, productsysno, relationcode, displayorder, createdby, createddate, lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("attributesysno", "attributesysno");
            bcp.ColumnMappings.Add("productsysno", "productsysno");
            bcp.ColumnMappings.Add("relationcode", "relationcode");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
        }
     
    }
}
