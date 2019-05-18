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
    public class CrCustomerQuestion : BaseTask
    {
        public override void Read()
        {
       
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_CrCustomerQuestion",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "CrCustomerQuestion");

            }
        }
        /* sysno, customersysno, productsysno, questiontype, question, questiondate, answersysno, answer, answerdate, status   */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("customersysno", "customersysno");
            bcp.ColumnMappings.Add("productsysno", "productsysno");
            bcp.ColumnMappings.Add("questiontype", "questiontype");
            bcp.ColumnMappings.Add("question", "question");
            bcp.ColumnMappings.Add("questiondate", "questiondate");
            bcp.ColumnMappings.Add("answersysno", "answersysno");
            bcp.ColumnMappings.Add("answer", "answer");
            bcp.ColumnMappings.Add("answerdate", "answerdate");
            bcp.ColumnMappings.Add("status", "status");
        }
    }
}
