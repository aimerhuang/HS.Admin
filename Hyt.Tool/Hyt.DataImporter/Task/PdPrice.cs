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
    public class PdPrice :BaseTask
    {
        public override void Read()
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_PdPrice", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "PdPrice");

            }
        }

        /*sysno, productsysno, price, pricesource, sourcesysno, status*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("productsysno", "productsysno");
            bcp.ColumnMappings.Add("price", "price");
            bcp.ColumnMappings.Add("pricesource", "pricesource");
            bcp.ColumnMappings.Add("sourcesysno", "sourcesysno");
            bcp.ColumnMappings.Add("status", "status");
          
        }
    }
}
