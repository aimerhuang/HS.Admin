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
    public class CrCustomer :BaseTask
    {
        public override void Read()
        {
           
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlCommand command = new SqlCommand("ImportData.dbo.proc_CrCustomer", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);
                myAdapter.Fill(Common.RDS, "CrCustomer");
            }
        }
        /*  sysno,account, password, name, nickname, headimage, emailaddress, emailstatus, mobilephonenumber, mobilephonestatus, gender, idcardno, 
areasysno, streetaddress, birthday, maritalstatus, monthlyincome, hobbies, registersource, registerdate, lastloginip, lastlogindate, status, 
levelsysno, levelpoint, experiencepoint, experiencecoin, registersourcesysno, islevelfixed, isexperiencepointfixed, isexperiencecoinfixed  */
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("account", "account");
            bcp.ColumnMappings.Add("password", "password");
            bcp.ColumnMappings.Add("name", "name");
            bcp.ColumnMappings.Add("nickname", "nickname");
            bcp.ColumnMappings.Add("headimage", "headimage");
            bcp.ColumnMappings.Add("emailaddress", "emailaddress");
            bcp.ColumnMappings.Add("emailstatus", "emailstatus");
            bcp.ColumnMappings.Add("mobilephonenumber", "mobilephonenumber");
            bcp.ColumnMappings.Add("mobilephonestatus", "mobilephonestatus");
            bcp.ColumnMappings.Add("gender", "gender");
            bcp.ColumnMappings.Add("idcardno", "idcardno");
            bcp.ColumnMappings.Add("areasysno", "areasysno");
            bcp.ColumnMappings.Add("streetaddress", "streetaddress");
            bcp.ColumnMappings.Add("birthday", "birthday");
            bcp.ColumnMappings.Add("maritalstatus", "maritalstatus");
            bcp.ColumnMappings.Add("monthlyincome", "monthlyincome");
            bcp.ColumnMappings.Add("hobbies", "hobbies");
            bcp.ColumnMappings.Add("registersource", "registersource");
            bcp.ColumnMappings.Add("registerdate", "registerdate");
            bcp.ColumnMappings.Add("lastloginip", "lastloginip");
            bcp.ColumnMappings.Add("lastlogindate", "lastlogindate");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("levelsysno", "levelsysno");
            bcp.ColumnMappings.Add("levelpoint", "levelpoint");
            bcp.ColumnMappings.Add("experiencepoint", "experiencepoint");
            bcp.ColumnMappings.Add("experiencecoin", "experiencecoin");
            bcp.ColumnMappings.Add("AvailablePoint", "AvailablePoint");
            bcp.ColumnMappings.Add("registersourcesysno", "registersourcesysno");
            bcp.ColumnMappings.Add("islevelfixed", "islevelfixed");
            bcp.ColumnMappings.Add("isexperiencepointfixed", "isexperiencepointfixed");
            bcp.ColumnMappings.Add("isexperiencecoinfixed", "isexperiencecoinfixed");

        }
    }
}
