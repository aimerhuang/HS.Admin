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
    public class BsDeliveryPayment : BaseTask
    {
        public override void Read()
        {
            string sSql = "select * from ImportData.dbo.BsDeliveryPayment order by sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "BsDeliveryPayment");

            }
        }

        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            //SysNo, PaymentSysNo, DeliverySysNo, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("PaymentSysNo", "PaymentSysNo");
            bcp.ColumnMappings.Add("DeliverySysNo", "DeliverySysNo");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
           
        }
    }
}
