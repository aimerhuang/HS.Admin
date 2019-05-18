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
    public class PdAttributeGroupAssociation :BaseTask
    {
        public override void Read()
        {
             
            string sSql = "select " +
                           "ROW_NUMBER() over (order by a.sysno) as SysNo," + 
                           "b.orderNum AS displayorder," +
                           "a.SysNo AS  attributesysno," +
                           "b.SysNo AS attributegroupsysno," +
                           "2 as createdby," +
                           "GETDATE() as createddate," +
                           "2 as lastupdateby," +
                           "GETDATE() as lastupdatedate " +
                       " from [db-hytformal].dbo.Category_Attribute2 a inner join  [db-hytformal].dbo.Category_Attribute1 b on a.A1SysNo=b.sysno ";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "PdAttributeGroupAssociation");

            }
        }

        /*displayorder, sysno, attributesysno, attributegroupsysno, createdby, createddate, lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("attributesysno", "attributesysno");
            bcp.ColumnMappings.Add("attributegroupsysno", "attributegroupsysno");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
        }
    }
}
