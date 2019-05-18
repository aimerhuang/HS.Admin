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
    public class WhstockOutItem : BaseTask
    {
        public override void Read()
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_WhstockOutItem",myConn);
                command.CommandType = command.CommandType;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "WhstockOutItem");

            }
        }

        /*SysNo, StockOutSysNo, TransactionSysNo, OrderSysNo, OrderItemSysNo,
        ProductSysNo, ProductName, ProductQuantity, ReturnQuantity, Weight,
        Measurement, OriginalPrice, RealSalesAmount, Remarks, Status, 
        CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("StockOutSysNo", "StockOutSysNo");
            bcp.ColumnMappings.Add("TransactionSysNo", "TransactionSysNo");
            bcp.ColumnMappings.Add("OrderSysNo", "OrderSysNo");
            bcp.ColumnMappings.Add("OrderItemSysNo", "OrderItemSysNo");
            bcp.ColumnMappings.Add("ProductSysNo", "ProductSysNo");
            bcp.ColumnMappings.Add("ProductName", "ProductName");
            bcp.ColumnMappings.Add("ProductQuantity", "ProductQuantity");
            bcp.ColumnMappings.Add("ReturnQuantity", "ReturnQuantity");
            bcp.ColumnMappings.Add("Weight", "Weight");
            bcp.ColumnMappings.Add("Measurement", "Measurement");
            bcp.ColumnMappings.Add("OriginalPrice", "OriginalPrice");
            bcp.ColumnMappings.Add("RealSalesAmount", "RealSalesAmount");
            bcp.ColumnMappings.Add("Remarks", "Remarks");
            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
       
        }
    }
}
