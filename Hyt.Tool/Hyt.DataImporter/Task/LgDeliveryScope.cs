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
    public class LgDeliveryScope :BaseTask
    {
        public override void Read()
        {

            //string sSql = " SELECT 	ROW_NUMBER() OVER(ORDER BY areasysno) AS SysNO," +
            //                        "areasysno, " +
            //                        "mapscope," +
            //                        "DESCRIPTION," +
            //                        "createdby," +
            //                        "createddate," +
            //                        "lastupdateby," +
            //                        "lastupdatedate " +
            //                "FROM  " +
            //                    "( " +	
            //                        " SELECT   " +
            //                               "CitySysNo AS areasysno," + 
            //                               "position AS mapscope," + 
            //                               "comment AS DESCRIPTION, " +
            //                               "NULL AS  createdby, " +
            //                               "NULL AS createddate, " +
            //                               "NULL AS lastupdateby, " +
            //                               "NULL AS lastupdatedate "   +
            //              "FROM dbo.City_Map "+
            //            "UNION " +
            //            "SELECT " +
            //                "sysno AS  areasysno," +
            //                "NULL AS   mapscope," +
            //                "NULL AS DESCRIPTION," +
            //                "NULL AS  createdby, " +
            //                "NULL AS createddate, " +
            //                "NULL AS lastupdateby, " +
            //                "NULL AS lastupdatedate " +                 
            //            "FROM area " +
            //            "WHERE   DistrictName IS NOT null and DistrictName <>'' and  CityName LIKE '成都%'  AND status=0 ) a";
                        

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_LgDeliveryScope", myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "LgDeliveryScope");

            }
        }

        /*sysno, areasysno, mapscope, description, createdby, createddate,lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("areasysno", "areasysno");
            bcp.ColumnMappings.Add("mapscope", "mapscope");
            bcp.ColumnMappings.Add("description", "description");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
        }
    }
}
