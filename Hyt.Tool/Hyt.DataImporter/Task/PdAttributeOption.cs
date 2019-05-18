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
    public class PdAttributeOption :BaseTask
    {
        public override void Read()
        {

            //string sSql = "SELECT " +
            //                    "ROW_NUMBER() over (order by sysno) as sysno," +
            //                    "Attribute2OptionName AS attributetext ," +
            //                    "OrderNum AS displayorder ," +
            //                    "1 as Status ," +
            //                    "2 AS createdby ," +
            //                    "GETDATE() AS createddate ," +
            //                    "2 AS lastupdateby , " +
            //                    "GETDATE() AS lastupdatedate ," +
            //                    "Attribute2SysNo AS attributesysno " +
            //                    "FROM    [hyt-v2].dbo.Category_Attribute2_Option ";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlCommand command = new SqlCommand("ImportData.dbo.proc_PdAttributeOption",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "PdAttributeOption");

            }
        }

        /*sysno, attributetext, displayorder, status, createdby, createddate, lastupdateby, lastupdatedate, attributesysno*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("attributetext", "attributetext");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("attributesysno", "attributesysno");
        }
    }
}
