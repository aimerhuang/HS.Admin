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
    public class WhWarehouse : BaseTask
    {
        public override void Read()
        {

            //string sSql = "SELECT   DISTINCT " +
            //                "a.sysno , " +
            //                "a.stockid AS erpcode ," +
            //                "b.StockID AS erprmacode ," +
            //                "StockName AS warehousename ," +
            //                " ( CASE WHEN a.DistrictSysNo IN ( SELECT  sysno " +
            //                                " FROM    ImportData.dbo.area_1)" +
            //                               "THEN c.NewAreaSysNo " +
            //                  "ELSE a.DistrictSysNo " +
            //                  "END ) AS AreaSysNo ," +
            //                "Address AS streetaddress ," +
            //                "contact ," +
            //                "phone ," +
            //                "( CASE WHEN a.status = 0 THEN 1 " +
            //                    "WHEN a.status = -1 THEN 0 " +
            //                    "END ) AS status ," +
            //                "(CASE WHEN StockType=1 THEN 10 " +
            //                    "WHEN StockType=3 THEN 20 " +
            //                    "END) AS warehousetype ," +
            //                "Xpoint AS latitude ," +
            //                "ypoint AS longitude ," +
            //                "img AS imgurl ," +
            //                "2 AS createdby ," +
            //                "GETDATE() AS createddate ," +
            //                "2 AS lastupdateby ," +
            //                "GETDATE() AS lastupdatedate " +
            //        "FROM    [hyt-v2].dbo.Stock AS a LEFT JOIN (SELECT StockID,substring(StockID,4,LEN(StockID)-3) AS stockid1 FROM [hyt-v2].dbo.Stock WHERE  status=0 and stockid LIKE 'RMA%') b " +
            //                                            "ON a.StockID=b.stockid1 " +
            //                                " LEFT JOIN ImportData.dbo.area_1 c ON a.DistrictSysNo=c.sysno" +
            //    " WHERE  a.status=0 and  a.stockid NOT LIKE 'RMA%'";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlCommand command = new SqlCommand("ImportData.dbo.proc_WhWarehouse",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "WhWarehouse");

            }
        }

        /*sysno, erpcode, erprmacode, warehousename, areasysno, streetaddress, contact, phone, status, 
        warehousetype, latitude, longitude, imgurl, createdby, createddate, lastupdateby, lastupdatedate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("erpcode", "erpcode");
            bcp.ColumnMappings.Add("erprmacode", "erprmacode");
            bcp.ColumnMappings.Add("warehousename", "warehousename");
            bcp.ColumnMappings.Add("areasysno", "areasysno");
            bcp.ColumnMappings.Add("streetaddress", "streetaddress");
            bcp.ColumnMappings.Add("contact", "contact");
            bcp.ColumnMappings.Add("phone", "phone");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("warehousetype", "warehousetype");
            bcp.ColumnMappings.Add("latitude", "latitude");
            bcp.ColumnMappings.Add("longitude", "longitude");
            bcp.ColumnMappings.Add("imgurl", "imgurl");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");

        }
    }
}
