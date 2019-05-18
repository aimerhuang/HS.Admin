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
    public class FeProductGroup :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM ImportData.dbo.FeProductGroup";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlCommand command = new SqlCommand(sSql, myConn);
                command.CommandType = CommandType.Text;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FeProductGroup");

            }
        }

        /* SysNo, PlatformType, ProductGroupIcon, Name, NameColor,
        Code, DisplayOrder, Status, CreatedBy, CreatedDate,
        LastUpdateBy, LastUpdateDate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("PlatformType", "PlatformType");
            bcp.ColumnMappings.Add("ProductGroupIcon", "ProductGroupIcon");
            bcp.ColumnMappings.Add("Name", "Name");
            bcp.ColumnMappings.Add("NameColor", "NameColor");
            bcp.ColumnMappings.Add("Code", "Code");
            bcp.ColumnMappings.Add("DisplayOrder", "DisplayOrder");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
        
        }
    }
}
