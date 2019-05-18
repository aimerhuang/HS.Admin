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
    public class SyMenu :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM ImportData.dbo.symenu ORDER BY sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "SyMenu");

            }
        }

        /*sysno, parentsysno, menuname, menuurl, displayorder, createdby, createddate, lastupdateby, lastupdatedate, 
        status, menuimage, innavigator*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("parentsysno", "parentsysno");
            bcp.ColumnMappings.Add("menuname", "menuname");
            bcp.ColumnMappings.Add("menuurl", "menuurl");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("menuimage", "menuimage");
            bcp.ColumnMappings.Add("innavigator", "innavigator");
        }
    }
}
