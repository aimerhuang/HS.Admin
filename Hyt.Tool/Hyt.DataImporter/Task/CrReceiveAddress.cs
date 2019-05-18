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
    public class CrReceiveAddress :BaseTask
    {
        public override void Read()
        {
            
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_CrReceiveAddress", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "CrReceiveAddress");

            }
        }

        /* SysNo, AreaSysNo, CustomerSysNo, Title, Name, Gender, PhoneNumber,
            MobilePhoneNumber, FaxNumber, EmailAddress, StreetAddress, ZipCode, IsDefault */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("AreaSysNo", "AreaSysNo");
            bcp.ColumnMappings.Add("CustomerSysNo", "CustomerSysNo");
            bcp.ColumnMappings.Add("Title", "Title");
            bcp.ColumnMappings.Add("Name", "Name");
            bcp.ColumnMappings.Add("Gender", "Gender");
            bcp.ColumnMappings.Add("PhoneNumber", "PhoneNumber");
            bcp.ColumnMappings.Add("MobilePhoneNumber", "MobilePhoneNumber");
            bcp.ColumnMappings.Add("FaxNumber", "FaxNumber");
            bcp.ColumnMappings.Add("EmailAddress", "EmailAddress");
            bcp.ColumnMappings.Add("StreetAddress", "StreetAddress");
            bcp.ColumnMappings.Add("ZipCode", "ZipCode");
            bcp.ColumnMappings.Add("IsDefault", "IsDefault");
          
          
        }
    }
}
