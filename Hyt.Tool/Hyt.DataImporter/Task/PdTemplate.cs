using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    public class PdTemplate :BaseTask
    {
        public override void Read()
        {
            string sSql = "select *from ImportData.dbo.PdTemplate";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "PdTemplate");

            }
        }

        /*SysNo, Type, Name, Icon, Remark, Content, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("Type", "Type");
            bcp.ColumnMappings.Add("Name", "Name");
            bcp.ColumnMappings.Add("Icon", "Icon");
            bcp.ColumnMappings.Add("Remark", "Remark");
            bcp.ColumnMappings.Add("Content", "Content");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
           
        }
    }
}
