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
    public class SPpromotion :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM  ImportData.dbo.SPpromotion";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "SPpromotion");

            }
        }

        /*sysno, name,  description, displayprefix, subjecturl, promotiontype, 
            starttime, endtime, promotioncode, isusepromotioncode, promotionusequantity, 
            promotionusedquantity, userusequantity, priority, status, auditorsysno, 
            auditdate, createdby, createddate, lastupdateby, lastupdatedate, promotionplatform*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("name", "name");
            bcp.ColumnMappings.Add("description", "description");
            bcp.ColumnMappings.Add("displayprefix", "displayprefix");
            bcp.ColumnMappings.Add("subjecturl", "subjecturl");
            bcp.ColumnMappings.Add("promotiontype", "promotiontype");
            bcp.ColumnMappings.Add("starttime", "starttime");
            bcp.ColumnMappings.Add("endtime", "endtime");
            bcp.ColumnMappings.Add("promotioncode", "promotioncode");
            bcp.ColumnMappings.Add("isusepromotioncode", "isusepromotioncode");
            bcp.ColumnMappings.Add("promotionusequantity", "promotionusequantity");
            bcp.ColumnMappings.Add("promotionusedquantity", "promotionusedquantity");
            bcp.ColumnMappings.Add("userusequantity", "userusequantity");
            bcp.ColumnMappings.Add("priority", "priority");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("auditorsysno", "auditorsysno");
            bcp.ColumnMappings.Add("auditdate", "auditdate");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("promotionplatform", "promotionplatform");

        }
    }
}
