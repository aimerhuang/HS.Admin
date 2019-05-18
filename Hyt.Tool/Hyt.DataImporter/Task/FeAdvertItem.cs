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
    public class FeAdvertItem :BaseTask
    {
        public override void Read()
        {

            string sSql = "SELECT *FROM ImportData.dbo.FeAdvertItem";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlCommand command = new SqlCommand(sSql, myConn);
                command.CommandType = CommandType.Text;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "FeAdvertItem");

            }
        }

        /* SysNo, GroupSysNo, Name, Content, ImageUrl, LinkUrl, LinkTitle, OpenType, BeginDate, EndDate, DisplayOrder, Status, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("GroupSysNo", "GroupSysNo");
            bcp.ColumnMappings.Add("Name", "Name");
            bcp.ColumnMappings.Add("Content", "Content");
            bcp.ColumnMappings.Add("ImageUrl", "ImageUrl");
            bcp.ColumnMappings.Add("LinkUrl", "LinkUrl");
            bcp.ColumnMappings.Add("LinkTitle", "LinkTitle");
            bcp.ColumnMappings.Add("OpenType", "OpenType");
            bcp.ColumnMappings.Add("BeginDate", "BeginDate");
            bcp.ColumnMappings.Add("EndDate", "EndDate");
            bcp.ColumnMappings.Add("DisplayOrder", "DisplayOrder");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
         
        }
    }
}
