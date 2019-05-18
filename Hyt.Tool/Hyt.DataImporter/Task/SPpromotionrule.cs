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
    public class SPpromotionrule :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM  ImportData.dbo.SPpromotionrule";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "SPpromotionrule");

            }
        }

        /*sysno,             name,             description,             fronttext,             truetext, 
            falsetext,             rulescript,             adminhtml,             adminscript,             promotiontype, 
            status,             auditorsysno,             auditdate,             createdby,             createddate, 
            lastupdateby,             lastupdatedate,             ruletype*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("name", "name");
            bcp.ColumnMappings.Add("description", "description");

            bcp.ColumnMappings.Add("fronttext", "fronttext");
            bcp.ColumnMappings.Add("truetext", "truetext");
            bcp.ColumnMappings.Add("falsetext", "falsetext")
                ;
            bcp.ColumnMappings.Add("rulescript", "rulescript");
            bcp.ColumnMappings.Add("adminhtml", "adminhtml");
            bcp.ColumnMappings.Add("adminscript", "adminscript");

            bcp.ColumnMappings.Add("promotiontype", "promotiontype");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("auditorsysno", "auditorsysno");

            bcp.ColumnMappings.Add("auditdate", "auditdate");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("ruletype", "ruletype");

        }
    }
}
