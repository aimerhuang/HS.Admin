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
    public class BsOrganization :BaseTask
    {
        public override void Read()
        {
            string sSql = "SELECT *FROM ImportData.dbo.BsOrganization";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "BsOrganization");

            }
        }

        /*SysNo, ParentSysNo, Code, Name, Description, DisplayOrder, Status, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate  */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            //SysNo, PaymentSysNo, DeliverySysNo, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("ParentSysNo", "ParentSysNo");
            bcp.ColumnMappings.Add("Code", "Code");
            bcp.ColumnMappings.Add("Name", "Name");
            bcp.ColumnMappings.Add("Description", "Description");
            bcp.ColumnMappings.Add("DisplayOrder", "DisplayOrder");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");

        }
    }
}
