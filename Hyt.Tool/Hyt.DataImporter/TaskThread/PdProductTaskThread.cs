using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyt.DataImporter.TaskThread;
using Hyt.Model;
using Hyt.ProductImport;

namespace Hyt.DataImporter
{
    public class PdProductTaskThread : BaseTaskThread
    {
        logRecord log = new logRecord();

        public PdProductTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
            : base(OnTaskBegin, OnTaskGoing)
        {
            Read();
        }

        public override int order
        {
            get { return 1; }
        }
        
        public override string name
        {
            get { return "商品"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdProduct> list;

        protected override void Read()
        {
            //SysNo, BrandSysNo, ErpCode, Barcode, QRCode, ProductType, ProductName, ProductSubName, NameAcronymy, ProductSummary, ProductSlogan, PackageDesc, ProductDesc, ImageCount, Thumbnail, ViewCount, SeoTitle, SeoKeyword, SeoDescription, Status, DisplayOrder, CanFrontEndOrder, CreatedBy, CreatedDate, LastUpdateBy, LastUpdateDate, Stamp
            string sSql = "SELECT  " +  
                                "SysNo as sysno," + 
                                "1 AS brandsysno," +
                                "ProductID AS ErpCode," +
                                "Barcode," +
                                "null AS QRCode," +
                                "(case when ProductName like '上门%'  then 20 " +
                                "else 10 " +
                                " end ) as ProductType," +
                                "ProductName," +
                                "BriefName AS ProductSubName," +
                                "''" +  " AS NameAcronymy," +
                                "Performance AS ProductSummary," +
                                "''" +  " AS ProductSlogan," +
                                "PackageList AS PackageDesc," +
                                "ProductDesc," +
                                "'/ImageServer/v1formal1/ProductImg800/' + ProductID  + '.jpg'   as ProductImage, " +
                                "0 AS ViewCount," +
                                "SEOTitle," +
                                "SEOKeyword," +
                                "SEODescription," +
                                "Status," +
                                "1 AS displayorder," +
                                "1 AS CanFrontEndOrder," +
                                "CreateUserSysNo as createdby," +
                                "CreateTime as  CreatedDate," +
                                "null AS LastUpdateBy," +
                                "GETDATE() AS LastUpdateDate " +
                                "FROM product " +
                                "ORDER BY SysNo";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdProduct>();
        }

        protected override void Write(int rowIndex)
        {
            try
            {

                string sql = @"INSERT INTO PdProduct
                           (SysNo
                           ,BrandSysNo
                           ,ErpCode
                           ,Barcode
                           ,QRCode
                           ,ProductType
                           ,ProductName
                           ,ProductSubName
                           ,NameAcronymy
                           ,ProductSummary
                           ,ProductSlogan
                           ,PackageDesc
                           ,ProductDesc
                           ,ProductImage
                           ,ViewCount
                           ,SeoTitle
                           ,SeoKeyword
                           ,SeoDescription
                           ,Status
                           ,DisplayOrder
                           ,CanFrontEndOrder
                           ,CreatedBy
                           ,CreatedDate
                           ,LastUpdateBy
                           ,LastUpdateDate)
                         VALUES
                               (
                                    :SysNo,
                                   :BrandSysNo,
                                   :ErpCode,
                                   :Barcode, 
                                   :QRCode,
                                   :ProductType,
                                   :ProductName, 
                                   :ProductSubName,
                                   :NameAcronymy, 
                                   :ProductSummary, 
                                   :ProductSlogan,
                                   :PackageDesc,
                                   :ProductDesc,
                                   :ProductImage,
                                   :ViewCount,
                                   :SeoTitle,
                                   :SeoKeyword,
                                   :SeoDescription, 
                                   :Status,
                                   :DisplayOrder,
                                   :CanFrontEndOrder,
                                   :CreatedBy,
                                   :CreatedDate,
                                   :LastUpdateBy,
                                   :LastUpdateDate)";

               DataProvider.OracleInstance.Sql(sql, new object[] { 
               list[rowIndex].SysNo,
               list[rowIndex].BrandSysNo,
               list[rowIndex].ErpCode,
               list[rowIndex].Barcode, 
               list[rowIndex].QRCode,
               list[rowIndex].ProductType,
               list[rowIndex].ProductName, 
               list[rowIndex].ProductSubName,
               list[rowIndex].NameAcronymy, 
               list[rowIndex].ProductSummary, 
               list[rowIndex].ProductSlogan,
               list[rowIndex].PackageDesc,
               list[rowIndex].ProductDesc,
               list[rowIndex].ProductImage,
               list[rowIndex].ViewCount,
               list[rowIndex].SeoTitle,
               list[rowIndex].SeoKeyword,
               list[rowIndex].SeoDescription, 
               list[rowIndex].Status,
               list[rowIndex].DisplayOrder,
               list[rowIndex].CanFrontEndOrder,
               list[rowIndex].CreatedBy,
               list[rowIndex].CreatedDate,
               list[rowIndex].LastUpdateBy,
               list[rowIndex].LastUpdateDate}).Execute();
                //DataProvider.OracleInstance.Insert<PdProduct>("PdProduct", list[rowIndex]).AutoMap().Execute();
            }
            catch (Exception ex)
            {
                log.CheckLog("导入表【PdProduct】失败，系统号：" + list[rowIndex].SysNo.ToString() + ";错误信息："+ ex.Message );
            }
            
         
           
        }
    }
}
