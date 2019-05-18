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
    public class PdCategory :BaseTask
    {

        private DataTable table;
        public DataTable Table
        {
            get;
            set;
        }

        public override void Read()
        {
            
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_pdcategory",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "PdCategory");

            }
        }

        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            //SYSNO, PARENTSYSNO, CATEGORYNAME, CODE, SEOTITLE, SEOKEYWORD, SEODESCRIPTION, TEMPLATESYSNO, ISONLINE, DISPLAYORDER, REMARKS, CREATEDBY, CREATEDDATE, LASTUPDATEBY, LASTUPDATEDATE, STATUS
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("ParentSysNo", "ParentSysNo");
            bcp.ColumnMappings.Add("CategoryName", "CategoryName");
            bcp.ColumnMappings.Add("Code", "Code");
            bcp.ColumnMappings.Add("CategoryImage", "CategoryImage");
            bcp.ColumnMappings.Add("SeoTitle", "SeoTitle");
            bcp.ColumnMappings.Add("SeoKeyword", "SeoKeyword");
            bcp.ColumnMappings.Add("SeoDescription", "SeoDescription");
            bcp.ColumnMappings.Add("templatesysno", "templatesysno");
            bcp.ColumnMappings.Add("ISONLINE", "ISONLINE");
            bcp.ColumnMappings.Add("DisplayOrder", "DisplayOrder");
            bcp.ColumnMappings.Add("remarks", "remarks");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
            bcp.ColumnMappings.Add("status", "status");
           
        }
    }
}
