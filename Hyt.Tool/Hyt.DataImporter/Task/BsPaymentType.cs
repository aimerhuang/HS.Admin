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
    public class BsPaymentType : BaseTask
    {

        public override void Read()
        {

            string sSql = "select * from ImportData.dbo.BsPaymentType order by sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "BsPaymentType");

            }
        }

        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            //sysno,paymentname, paymentdescription, isonlinepay, isonlinevisible, 
            //paymenttype, displayorder, createdby, createddate, lastupdateby, lastupdatedate, 
            //status, requiredcardnumber

           
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("paymentname", "paymentname");
            bcp.ColumnMappings.Add("paymentdescription", "paymentdescription");
            bcp.ColumnMappings.Add("isonlinepay", "isonlinepay");
            bcp.ColumnMappings.Add("isonlinevisible", "isonlinevisible");
            bcp.ColumnMappings.Add("paymenttype", "paymenttype");
            bcp.ColumnMappings.Add("displayorder", "displayorder");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("requiredcardnumber", "requiredcardnumber");
          
        }
    }
}
