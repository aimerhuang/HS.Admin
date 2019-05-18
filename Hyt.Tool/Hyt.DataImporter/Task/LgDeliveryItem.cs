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
    public class LgDeliveryItem :BaseTask
    {
        public override void Read()
        {
                  
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command =new  SqlCommand("ImportData.dbo.proc_LgDeliveryItem",myConn);
                  command.CommandType = CommandType.StoredProcedure;
                  SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "LgDeliveryItem");

            }
        }

        /*SysNo, DeliverySysNo, TransactionSysNo, NoteType, NoteSysNo, IsCOD, 
        StockOutAmount, Receivable, PaymentType, PayNo, AddressSysNo, ExpressNo, Status, Remarks, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("DeliverySysNo", "DeliverySysNo");
            bcp.ColumnMappings.Add("TransactionSysNo", "TransactionSysNo");
            bcp.ColumnMappings.Add("NoteType", "NoteType");
            bcp.ColumnMappings.Add("NoteSysNo", "NoteSysNo");
            bcp.ColumnMappings.Add("IsCOD", "IsCOD");
            bcp.ColumnMappings.Add("StockOutAmount", "StockOutAmount");
            bcp.ColumnMappings.Add("Receivable", "Receivable");
            bcp.ColumnMappings.Add("PaymentType", "PaymentType");
            bcp.ColumnMappings.Add("PayNo", "PayNo");
            bcp.ColumnMappings.Add("AddressSysNo", "AddressSysNo");
            bcp.ColumnMappings.Add("ExpressNo", "ExpressNo");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("Remarks", "Remarks");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");

           
        }
    }
}
