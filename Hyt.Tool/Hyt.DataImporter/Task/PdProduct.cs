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
    public class PdProduct : BaseTask
    {
        public override void Read()
        {

            //SysNo, BrandSysNo, ErpCode, Barcode, QRCode, ProductType, ProductName, ProductSubName, NameAcronymy, ProductSummary, ProductSlogan, PackageDesc, ProductDesc, ImageCount, Thumbnail, ViewCount, SeoTitle, SeoKeyword, SeoDescription, Status, DisplayOrder, CanFrontEndOrder, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate, Stamp
            //string sSql = "SELECT  " +
            //                    "SysNo as sysno," +
            //                    "1 AS brandsysno," +
            //                    "ProductID AS ErpCode," +
            //                    "Barcode," +
            //                    "null AS QRCode," +
            //                    "(case when ProductName like '上门%'  then 20 " +
            //                    "else 10 " +
            //                    " end ) as ProductType," +
            //                    "ProductName," +
            //                    "BriefName AS ProductSubName," +
            //                    "''" + " AS NameAcronymy," +
            //                    "Performance AS ProductSummary," +
            //                    "''" + " AS ProductSlogan," +
            //                    "PackageList AS PackageDesc," +
            //                    "ProductDesc," +
            //                    "'/ImageServer/v1formal1/ProductImg800/' + ProductID  + '.jpg'   as ProductImage, " +
            //                    "0 AS ViewCount," +
            //                    "SEOTitle," +
            //                    "SEOKeyword," +
            //                    "SEODescription," +
            //                    "(CASE WHEN   Status=1 THEN 1 " +
            //                        "ELSE " +
            //                            "0 " +
            //                    "END) AS STATUS, " +
            //                    "1 AS displayorder," +
            //                    "1 AS CanFrontEndOrder," +
            //                    "CreateUserSysNo as createdby," +
            //                    "CreateTime as  CreatedDate," +
            //                    "null AS LastUpdateBy," +
            //                    "GETDATE() AS LastUpdateDate, "  +
            //                    "Null as productshorttitle," +
            //                    "Null as stamp " +
            //                    "FROM product " +
            //                    "ORDER BY SysNo";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlCommand command = new SqlCommand("ImportData.dbo.proc_Pdproduct", myConn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "PdProduct");

            }
        }

        /*SysNo, BrandSysNo, ErpCode, EasName, Barcode, QRCode, ProductType, ProductName,
            ProductSubName, NameAcronymy, ProductShortTitle, ProductSummary, ProductSlogan, 
            PackageDesc, ProductDesc, ProductImage, ViewCount, SeoTitle, SeoKeyword, SeoDescription, 
            Status, DisplayOrder, CanFrontEndOrder, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate, Stamp*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("SysNo", "SysNo");
            bcp.ColumnMappings.Add("BrandSysNo", "BrandSysNo");
            bcp.ColumnMappings.Add("ErpCode", "ErpCode");
            bcp.ColumnMappings.Add("EasName", "EasName");
            bcp.ColumnMappings.Add("Barcode", "Barcode");
            bcp.ColumnMappings.Add("QRCode", "QRCode");
            bcp.ColumnMappings.Add("ProductType", "ProductType");
            bcp.ColumnMappings.Add("ProductName", "ProductName");

            bcp.ColumnMappings.Add("ProductSubName", "ProductSubName");
            bcp.ColumnMappings.Add("NameAcronymy", "NameAcronymy");
            bcp.ColumnMappings.Add("ProductShortTitle", "ProductShortTitle");
            bcp.ColumnMappings.Add("ProductSummary", "ProductSummary");
            bcp.ColumnMappings.Add("ProductSlogan", "ProductSlogan");

            bcp.ColumnMappings.Add("PackageDesc", "PackageDesc");
            bcp.ColumnMappings.Add("ProductDesc", "ProductDesc");
            bcp.ColumnMappings.Add("ProductImage", "ProductImage");
            bcp.ColumnMappings.Add("ViewCount", "ViewCount");
            bcp.ColumnMappings.Add("SeoTitle", "SeoTitle");
            bcp.ColumnMappings.Add("SeoKeyword", "SeoKeyword");
            bcp.ColumnMappings.Add("SeoDescription", "SeoDescription");

            bcp.ColumnMappings.Add("Status", "Status");
            bcp.ColumnMappings.Add("DisplayOrder", "DisplayOrder");
            bcp.ColumnMappings.Add("CanFrontEndOrder", "CanFrontEndOrder");
            bcp.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bcp.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bcp.ColumnMappings.Add("LastUpdateBy", "LastUpdateBy");
            bcp.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
            bcp.ColumnMappings.Add("Stamp", "Stamp");
         
        }
    }
}
