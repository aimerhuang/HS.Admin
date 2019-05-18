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
    public class PdProductAttribute :BaseTask
    {
        public override void Read()
        {

            //string sSql = "SELECT  " +
            //               "ROW_NUMBER() OVER(ORDER BY a.ProductSysNo,a.Attribute2SysNo,b.Attribute2Name,c.SysNo,d.SysNo) AS sysno," + 
            //               "a.Attribute2SysNo AS attributesysno," +
            //                "a.ProductSysNo AS productsysno, " +
            //                 "b.Attribute2Name AS attributename," +
            //                  "c.SysNo AS AttributeGroupSysNo," +
            //                   "d.SysNo AS attributeoptionsysno, " +
            //                   "d.Attribute2OptionName  AS attributetext," +
            //                   "NULL AS attributeimage," +
            //                   "(CASE b.Attribute2Type WHEN 0 THEN 10 " +
            //                   "WHEN 1 THEN 30 " +
            //                   "WHEN 2 THEN 30 " +
            //                   "end) AS attributetype," +
            //                   "d.OrderNum AS displayorder," +
            //                   "1 AS status," +
            //                   "2 AS createdby," +
            //                   "GETDATE() AS createddate," +
            //                   "2 AS lastupdateby," +
            //                   "GETDATE() AS lastupdatedate " +
            //                   "FROM [hyt-v2].dbo.Product_Attribute2 a INNER JOIN [hyt-v2].dbo.Category_Attribute2 AS b ON a.Attribute2SysNo=b.SysNo " +
            //                                               "INNER JOIN [hyt-v2].dbo.Category_Attribute1 AS c ON b.A1SysNo=c.SysNo " +
            //                                               "INNER JOIN [hyt-v2].dbo.Category_Attribute2_Option AS d ON a.Attribute2SysNo=d.Attribute2SysNo AND a.Attribute2OptionSysNo=d.SysNo ";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_PdProductAttribute",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);
                myAdapter.Fill(Common.RDS, "PdProductAttribute");

            }
        }

        /*sysno, attributesysno, productsysno,attributename, AttributeGroupSysNo, attributeoptionsysno, attributetext, 
            attributeimage, attributetype, displayorder, status, createdby, createddate, lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("attributesysno", "attributesysno");
            bcp.ColumnMappings.Add("productsysno", "productsysno");
            bcp.ColumnMappings.Add("attributename", "attributename");
            bcp.ColumnMappings.Add("AttributeGroupSysNo", "AttributeGroupSysNo");
            bcp.ColumnMappings.Add("attributeoptionsysno", "attributeoptionsysno");
            bcp.ColumnMappings.Add("attributetext", "attributetext");
            bcp.ColumnMappings.Add("attributeimage", "attributeimage");
            bcp.ColumnMappings.Add("attributetype", "attributetype");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
        }
    }
}
