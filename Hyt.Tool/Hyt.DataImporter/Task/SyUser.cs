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
    public class SyUser :BaseTask
    {
        public override void Read()
        {

            //string ssql = "select distinct " +
            //              "sysno," +
            //               "userid as account," +
            //               "pwd as password," +
            //               "username as username," +
            //               "mobilephone as mobilephonenumber," +
            //               "email as emailaddress, " +
            //                "(case when status=0 then 1 " +
            //                        "when status=-1 then  0 " +
            //                "end) as status," +
            //               "getdate() as createddate," +
            //               "null as createdby," +
            //               "null as lastupdatedate," +
            //                "null as lastupdateby " +
            //               "from [hyt-v2].dbo.sys_user " +
            //               "order by sysno";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_SyUser",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "SyUser");

            }
        }

        /*sysno, account, password, username, mobilephonenumber, emailaddress, status, createddate, createdby, lastupdatedate,lastupdateby*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("account", "account");
            bcp.ColumnMappings.Add("password", "password");
            bcp.ColumnMappings.Add("username", "username");
            bcp.ColumnMappings.Add("mobilephonenumber", "mobilephonenumber");
            bcp.ColumnMappings.Add("emailaddress", "emailaddress");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");

        }
    }
}
