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
    public class LgDeliveryType : BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT * FROM  ImportData.dbo.LgDeliveryType ORDER BY sysno ";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "LgDeliveryType");

            }
        }

        /*sysno, parentsysno, deliverytypename, deliverytypedescription, deliverylevel, deliverytime, traceurl, displayorder, 
        provider, isonlinevisible, freight, isthirdpartyexpress, status, createdby, createddate, lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("parentsysno", "parentsysno");
            bcp.ColumnMappings.Add("deliverytypename", "deliverytypename");
            bcp.ColumnMappings.Add("deliverytypedescription", "deliverytypedescription");
            bcp.ColumnMappings.Add("deliverylevel", "deliverylevel");
            bcp.ColumnMappings.Add("deliverytime", "deliverytime");
            bcp.ColumnMappings.Add("traceurl", "traceurl");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("provider", "provider");
            bcp.ColumnMappings.Add("isonlinevisible", "isonlinevisible");
            bcp.ColumnMappings.Add("freight", "freight");
            bcp.ColumnMappings.Add("isthirdpartyexpress", "isthirdpartyexpress");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
        }
    }
}
