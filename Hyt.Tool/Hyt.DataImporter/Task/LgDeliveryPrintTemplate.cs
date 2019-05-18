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
    public class LgDeliveryPrintTemplate : BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT * FROM  ImportData.dbo.LgDeliveryPrintTemplate ORDER BY sysno ";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "LgDeliveryPrintTemplate");

            }
        }

        /*SysNo, DeliveryTypeSysNo, Name, BackgroundImage, Template, Status, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("DeliveryTypeSysNo", "DeliveryTypeSysNo");
            bcp.ColumnMappings.Add("Name", "Name");
            bcp.ColumnMappings.Add("BackgroundImage", "BackgroundImage");
            bcp.ColumnMappings.Add("Template", "Template");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
         
        }
    }
}
