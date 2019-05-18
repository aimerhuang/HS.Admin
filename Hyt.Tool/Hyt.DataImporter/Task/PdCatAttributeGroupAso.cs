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
    public class PdCatAttributeGroupAso : BaseTask
    {

        public override void Read()
        {

            //string sSql = "select " +
            //          "ROW_NUMBER() over (order by a.sysno) as sysno, " +  
            //          "b.orderNum AS displayorder," +
            //          "b.C3SysNo AS productcategorysysno," +
            //          " b.SysNo AS attributegroupsysno," +  
            //          "2 as createdby," +
            //          "GETDATE() as createddate," +
            //          "2 as lastupdateby," +
            //          "GETDATE() as lastupdatedate " +
            //    " from  [hyt-v2].dbo.Category_Attribute2 a inner join [hyt-v2].dbo.Category_Attribute1 b on a.A1SysNo=b.sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_PdCatAttributeGroupAso",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "PdCatAttributeGroupAso");

            }
        }

        /*displayorder, sysno, productcategorysysno, attributegroupsysno, createdby, createddate, lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("productcategorysysno", "productcategorysysno");
            bcp.ColumnMappings.Add("attributegroupsysno", "attributegroupsysno");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
        }

     
    }
}
