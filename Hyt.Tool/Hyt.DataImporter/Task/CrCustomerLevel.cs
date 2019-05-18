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
    public class CrCustomerLevel :BaseTask
    {

        public override void Read()
        {
            string sSql = "SELECT *FROM ImportData.dbo.CrCustomerLevel ORDER BY SYSNO";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "CrCustomerLevel");

            }
        }

        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            //sysno,levelname,leveldescription,lowerlimit,upperlimit,canpayforproduct,productpaymentupperlimit,productpaymentpercentage,
            //canpayforservice,servicepaymentupperlimit, servicepaymentpercentage, createdby,createddate, lastupdateby, lastupdatedate
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("levelname", "levelname");
            bcp.ColumnMappings.Add("leveldescription", "leveldescription");
            bcp.ColumnMappings.Add("lowerlimit", "lowerlimit");
            bcp.ColumnMappings.Add("upperlimit", "upperlimit");
            bcp.ColumnMappings.Add("canpayforproduct", "canpayforproduct");
            bcp.ColumnMappings.Add("productpaymentupperlimit", "productpaymentupperlimit");
            bcp.ColumnMappings.Add("productpaymentpercentage", "productpaymentpercentage");
            bcp.ColumnMappings.Add("canpayforservice", "canpayforservice");
            bcp.ColumnMappings.Add("servicepaymentupperlimit", "servicepaymentupperlimit");
            bcp.ColumnMappings.Add("servicepaymentpercentage", "servicepaymentpercentage");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
            
        }
    }
}
