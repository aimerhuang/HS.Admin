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
    public class PdProductStatistics : BaseTask
    {
        public override void Read()
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_PdProductStatistics", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "PdProductStatistics");

            }
        }

        /*SysNo, ProductSysNo, Sales, Liking, Favorites, Comments, Shares, Question, TotalScore, AverageScore*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("ProductSysNo", "ProductSysNo");
            bcp.ColumnMappings.Add("Sales", "Sales");
            bcp.ColumnMappings.Add("Liking", "Liking");
            bcp.ColumnMappings.Add("Favorites", "Favorites");
            bcp.ColumnMappings.Add("Comments", "Comments");
            bcp.ColumnMappings.Add("Shares", "Shares");
            bcp.ColumnMappings.Add("Question", "Question");
            bcp.ColumnMappings.Add("TotalScore", "TotalScore");
            bcp.ColumnMappings.Add("AverageScore", "AverageScore");
         
        }
    }
}
