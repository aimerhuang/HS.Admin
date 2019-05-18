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
    public class SoReceiveAddress :BaseTask
    {
        public override void Read()
        {

            //area_1 事先生成的区域对应表，由于取消了在第三级个区域有不同的位置，现在只取最小的那个区域编号
            //string sSql = "SELECT  " +
            //                    "distinct " +
            //                    "b.sysno," +
            //                    "name," +
            //                    "sex AS gender," +
            //    //"customersysno," +
            //                    "phone AS phonenumber," +
            //                    "(CASE WHEN LEN(CellPhone)>11 THEN NULL " +
            //                        "ELSE CellPhone " +
            //                     "END) AS  mobilephonenumber," +
            //                    "fax AS faxnumber," +
            //                    "email AS emailaddress," +
            //                    "(CASE WHEN  a.ReceiveAreaSysNo IN (SELECT sysno FROM ImportData.dbo.area_1) THEN c.NewAreaSysNo " +
            //                    "ELSE " +
            //                    "a.ReceiveAreaSysNo " +
            //                    "END ) AS AreaSysNo ," +
            //                    "address AS streetaddress," +
            //                    "(CASE WHEN LEN(zip)>6 THEN NULL " +
            //                         "ELSE zip " +
            //                    "END) AS zipcode " +

            //                "   FROM SO_Master  a " +
            //                    "INNER JOIN  Customer_Address b ON a.CustomerSysNo=b.CustomerSysNo " +
            //                                                        "AND a.ReceiveContact=b.Name " +
            //                                                        "AND a.ReceiveAddress=b.Address " +
            //                                                        "AND a.ReceiveAreaSysNo=b.AreaSysNo " +
            //                                                        "AND a.ReceiveCellPhone=b.CellPhone " +
            //                    "INNER JOIN ImportData.dbo.area_1 c ON a.ReceiveAreaSysNo=c.sysno ";
                              

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_SoReceiveAddress",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "SoReceiveAddress");

            }
        }

        /*SysNo, Name, Gender, PhoneNumber, MobilePhoneNumber, FaxNumber, EmailAddress, AreaSysNo, StreetAddress, ZipCode*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("Name", "Name");
            bcp.ColumnMappings.Add("Gender", "Gender");
            bcp.ColumnMappings.Add("PhoneNumber", "PhoneNumber");
            bcp.ColumnMappings.Add("MobilePhoneNumber", "MobilePhoneNumber");
            bcp.ColumnMappings.Add("FaxNumber", "FaxNumber");
            bcp.ColumnMappings.Add("EmailAddress", "EmailAddress");
            bcp.ColumnMappings.Add("AreaSysNo", "AreaSysNo");
            bcp.ColumnMappings.Add("StreetAddress", "StreetAddress");
            bcp.ColumnMappings.Add("ZipCode", "ZipCode");
           
        }
    }
}
