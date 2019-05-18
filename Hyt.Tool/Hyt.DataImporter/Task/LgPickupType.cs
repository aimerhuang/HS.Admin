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
    public class LgPickupType : BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT * from ImportData.dbo.LgPickupType order by Sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "LgPickupType");

            }
        }

        //sysno,parentsysno, pickuptypename, pickuptypedescription, pickuplevel, pickuptime, 
        //traceurl,displayorder, provider, isonlinevisible, freight, status, 
        //createdby, createddate, lastupdateby, lastupdatedate

        
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("parentsysno", "parentsysno");
            bcp.ColumnMappings.Add("pickuptypename", "pickuptypename");
            bcp.ColumnMappings.Add("pickuptypedescription", "pickuptypedescription");
            bcp.ColumnMappings.Add("pickuplevel", "pickuplevel");
            bcp.ColumnMappings.Add("pickuptime", "pickuptime");
            bcp.ColumnMappings.Add("traceurl", "traceurl");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("provider", "provider");
            bcp.ColumnMappings.Add("isonlinevisible", "isonlinevisible");
            bcp.ColumnMappings.Add("freight", "freight");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
        }
    }
}
