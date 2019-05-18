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
    public class spcoupon :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM  ImportData.dbo.spcoupon";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "spcoupon");

            }
        }
        
        /*sysno, promotionsysno, couponcode, couponamount, requirementamount, 
            starttime, endtime, type, sourcedescription, usequantity, 
            usedquantity, customersysno, status, description, auditorsysno, 
            auditdate, createdby, createddate, lastupdateby, lastupdatedate  */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("promotionsysno", "promotionsysno");
            bcp.ColumnMappings.Add("couponcode", "couponcode");
            bcp.ColumnMappings.Add("couponamount", "couponamount");
            bcp.ColumnMappings.Add("requirementamount", "requirementamount");
            bcp.ColumnMappings.Add("starttime", "starttime");
            bcp.ColumnMappings.Add("endtime", "endtime");
            bcp.ColumnMappings.Add("type", "type");
            bcp.ColumnMappings.Add("sourcedescription", "sourcedescription");
            bcp.ColumnMappings.Add("usequantity", "usequantity");
            bcp.ColumnMappings.Add("usedquantity", "usedquantity");
            bcp.ColumnMappings.Add("customersysno", "customersysno");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("description", "description");
            bcp.ColumnMappings.Add("auditorsysno", "auditorsysno");
            bcp.ColumnMappings.Add("auditdate", "auditdate");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
         
        }
    }
}
