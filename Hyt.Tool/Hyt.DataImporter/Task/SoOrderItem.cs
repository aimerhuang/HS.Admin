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
    public class SoOrderItem :BaseTask
    {
        public override void Read()
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_SoOrderItem", myConn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "SoOrderItem");

            }
        }

        /*SysNo, OrderSysNo, TransactionSysNo, ProductSysNo, ProductName,
            Quantity, OriginalPrice, SalesUnitPrice, SalesAmount, DiscountAmount, 
            ChangeAmount, RealStockOutQuantity, ProductSalesType,
             ProductSalesTypeSysNo, GroupCode, GroupName, UsedPromotions*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("OrderSysNo", "OrderSysNo");

            bcp.ColumnMappings.Add("ProductSysNo", "ProductSysNo");
            bcp.ColumnMappings.Add("ProductName", "ProductName");
            bcp.ColumnMappings.Add("Quantity", "Quantity");

            bcp.ColumnMappings.Add("OriginalPrice", "OriginalPrice");
            bcp.ColumnMappings.Add("SalesAmount", "SalesAmount");
            bcp.ColumnMappings.Add("RealStockOutQuantity", "RealStockOutQuantity");

            bcp.ColumnMappings.Add("TransactionSysNo", "TransactionSysNo");
            bcp.ColumnMappings.Add("ProductSalesType", "ProductSalesType");
            bcp.ColumnMappings.Add("ProductSalesTypeSysNo", "ProductSalesTypeSysNo");

            bcp.ColumnMappings.Add("SalesUnitPrice", "SalesUnitPrice");
            bcp.ColumnMappings.Add("DiscountAmount", "DiscountAmount");
            bcp.ColumnMappings.Add("ChangeAmount", "ChangeAmount");
          
            bcp.ColumnMappings.Add("GroupCode", "GroupCode");
            bcp.ColumnMappings.Add("GroupName", "GroupName");
            bcp.ColumnMappings.Add("UsedPromotions", "UsedPromotions");

        }
    }
}
