using Hyt.DataAccess.Sys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 升级数据库
    /// </summary>
    /// <remarks>2017-1-10 杨浩 创建</remarks>
    public class SyUpgradeTheDatabaseDaoImpl : ISyUpgradeTheDatabaseDao
    {
        /// <summary>
        /// 升级版本
        /// 注：每次数据库有变更需要累加版本值。
        /// </summary>
        public override decimal? Version
        {
            get { return 8; }
        }

        #region 升级脚本

        /// <summary>
        /// 升级新增表脚本数组
        /// </summary>
        private string[] UpgradeCreateTableSqlArry
        {
            get
            {
                return new string[] 
                {            
                    #region 库存变动日志表
                     GenerateCreateTableScript("WhWarehouseChangeLog",@"
                            CREATE TABLE [dbo].[WhWarehouseChangeLog]
                            (
	                                [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                                [WareSysNo] [int] NULL,
	                                [ProSysNo] [int] NULL,
	                                [ChangeQuantity] [int] NULL,
	                                [Quantity] [int] NULL,
	                                [BusinessTypes] [varchar](100) NULL,
	                                [LogData] [text] NULL,
	                                [ChageDate] [datetime] NULL,
	                                [CreateDate] [datetime] NULL
                            ) 
                    "),//库存变动日志表
                    #endregion
                    #region 会员卡表
                    GenerateCreateTableScript("CrCustomerShipCard",@"
                           CREATE TABLE [dbo].[CrCustomerShipCard](
	                            [CustomerSysNo] [int] NOT NULL,
	                            [CardNumber] [nvarchar](20) NOT NULL,
                                CONSTRAINT [PK_CrCustomerShipCard] PRIMARY KEY CLUSTERED 
                                (
	                                [CustomerSysNo] ASC,
	                                [CardNumber] ASC
                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]
                    "),//会员卡表
                    #endregion
                    #region 提现返利记录关联表
                     GenerateCreateTableScript("CrPredepositCashRebatesRecordAssociation",@"
                           CREATE TABLE [dbo].[CrPredepositCashRebatesRecordAssociation]
                           (
	                            [CustomerSysNo] [int] NULL,
	                            [CrCustomerRebatesRecordSysNos] [nvarchar](4000) NULL,
	                            [CrPredepositCashSysNo] [int] NULL
                            ) ON [PRIMARY]
                    "),//提现返利记录关联表
                    #endregion
                    #region 退款表
                    GenerateCreateTableScript("RcRefundReturn",@"CREATE TABLE [dbo].[RcRefundReturn](
	                            [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                            [TransactionSysNo] [varchar](32) NULL,
	                            [RmaId] [varchar](20) NULL,
	                            [OrderSysNo] [int] NULL,
	                            [CustomerSysNo] [int] NULL,
	                            [Source] [int] NULL,
	                            [HandleDepartment] [int] NULL,
	                            [RefundType] [int] NULL,
	                            [OrginAmount] [numeric](10, 2) NULL,
	                            [OrginPoint] [int] NULL,
	                            [OrginCoin] [numeric](10, 2) NULL,
	                            [CouponAmount] [numeric](10, 2) NULL,
	                            [RedeemAmount] [numeric](10, 2) NULL,
	                            [RefundPoint] [int] NULL,
	                            [RefundCoin] [numeric](10, 2) NULL,
	                            [RefundTotalAmount] [numeric](10, 2) NULL,
	                            [InternalRemark] [varchar](500) NULL,
	                            [RMARemark] [varchar](500) NULL,
	                            [RefundBank] [nvarchar](50) NULL,
	                            [RefundAccountName] [nvarchar](10) NULL,
	                            [RefundAccount] [varchar](30) NULL,
	                            [Status] [int] NULL,
	                            [CreateBy] [int] NULL,
	                            [CreateDate] [datetime] NULL,
	                            [CancelBy] [int] NULL,
	                            [CancelDate] [datetime] NULL,
	                            [AuditorBy] [int] NULL,
	                            [AuditorDate] [datetime] NULL,
	                            [RefundBy] [int] NULL,
	                            [RefundDate] [datetime] NULL,
	                            [LastUpdateBy] [int] NULL,
	                            [LastUpdateDate] [datetime] NULL,
                             CONSTRAINT [PK_RCREFUNDRETURN] PRIMARY KEY NONCLUSTERED 
                            (
	                            [SysNo] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]"),
                    #endregion
                    #region 物流公司
                    GenerateCreateTableScript("PmLogisticsCompany",@"
                          CREATE TABLE [dbo].[PmLogisticsCompany](
	                        [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                        [LCName] [varchar](500) NULL,
	                        [LCType] [varchar](500) NULL,
	                        [LCDis] [text] NULL
                        ) ON [PRIMARY]
                    "),//物流公司表
                    #endregion
                    #region 生产厂家表
                    GenerateCreateTableScript("PmManufacturer",@"
                          CREATE TABLE [dbo].[PmManufacturer](
	                        [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                        [FName] [varchar](500) NULL,
	                        [FDisInfo] [text] NULL,
	                        [FContact] [varchar](200) NULL,
	                        [FTelephone] [varchar](200) NULL,
	                        [FAddress] [varchar](500) NULL,
	                        [FCategory] [varchar](8000) NULL,
	                        [BankName] [varchar](4000) NULL,
	                        [BankIDCard] [varchar](4000) NULL,
	                        [ManufacturerCode] [varchar](1000) NULL
                        ) ON [PRIMARY]
                    "),//生产厂家表
                    #endregion
                    #region 集装箱表
                    GenerateCreateTableScript("PmContainer",@"
                          CREATE TABLE [dbo].[PmContainer](
	                        [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                        [CName] [varchar](500) NULL,
	                        [CType] [varchar](500) NULL,
	                        [CSHeight] [varchar](50) NULL,
	                        [CSWidth] [varchar](50) NULL,
	                        [CSLong] [varchar](50) NULL,
	                        [CDis] [text] NULL,
	                        [CubeType] [varchar](50) NULL
                        ) ON [PRIMARY]
                    "),//集装箱表
                    #endregion
                    #region 采购单
                    GenerateCreateTableScript("PrPurchase",@"
                         CREATE TABLE [dbo].[PrPurchase](
	                    [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                    [WarehouseSysNo] [int] NULL,
	                    [PurchaseCode] [nvarchar](100) NULL,
	                    [ManufacturerSysNo] [int] NULL,
	                    [Quantity] [int] NULL,
	                    [EnterQuantity] [int] NULL,
	                    [TotalMoney] [numeric](10, 2) NULL,
	                    [Status] [int] NULL,
	                    [PaymentStatus] [int] NULL,
	                    [WarehousingStatus] [int] NULL,
	                    [Remarks] [nvarchar](200) NULL,
	                    [CreatedBy] [int] NULL,
	                    [CreatedDate] [datetime] NULL,
                     CONSTRAINT [PK_PRPURCHASE] PRIMARY KEY NONCLUSTERED 
                    (
	                    [SysNo] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                    "),//采购单
                    #endregion
                    #region 采购详情
                    GenerateCreateTableScript("PrPurchaseDetails",@"
                        CREATE TABLE [dbo].[PrPurchaseDetails](
	                    [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                    [PurchaseSysNo] [int] NULL,
	                    [ProductSysNo] [int] NULL,
	                    [ProductName] [nvarchar](100) NULL,
	                    [ErpCode] [nvarchar](20) NULL,
	                    [Quantity] [int] NULL,
	                    [EnterQuantity] [int] NULL,
	                    [Money] [numeric](10, 2) NULL,
	                    [TotalMoney] [numeric](10, 2) NULL,
	                    [Remarks] [nvarchar](200) NULL,
                     CONSTRAINT [PK_PRPURCHASEDETAILS] PRIMARY KEY NONCLUSTERED 
                    (
	                    [SysNo] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                    "),//采购单关联表
                    #endregion
                    #region 采购退货表
                    GenerateCreateTableScript("PrPurchaseReturn",@"
                        CREATE TABLE [dbo].[PrPurchaseReturn](
	                    [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                    [WarehouseSysNo] [int] NULL,
	                    [PurchaseSysNo] [int] NULL,
	                    [Status] [int] NULL,
	                    [ReturnQuantity] [int] NULL,
	                    [OutQuantity] [int] NULL,
	                    [ReturnTotalMoney] [numeric](10, 2) NULL,
	                    [Remarks] [nvarchar](200) NULL,
	                    [CreatedBy] [int] NULL,
	                    [CreatedDate] [datetime] NULL,
                     CONSTRAINT [PK_PRPURCHASERETURN] PRIMARY KEY NONCLUSTERED 
                    (
	                    [SysNo] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                    "),//采购退货表
                    #endregion
                    #region 采购退货明细表
                    GenerateCreateTableScript("PrPurchaseReturnDetails",@"
                        CREATE TABLE [dbo].[PrPurchaseReturnDetails](
	                    [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                    [PurchaseReturnSysNo] [int] NULL,
	                    [ProductSysNo] [int] NULL,
	                    [ReturnQuantity] [int] NULL,
	                    [OutQuantity] [int] NULL,
	                    [Payment] [numeric](10, 2) NULL,
	                    [ReturnTotalMoney] [numeric](10, 2) NULL,
                     CONSTRAINT [PK_PRPURCHASERETURNDETAILS] PRIMARY KEY NONCLUSTERED 
                    (
	                    [SysNo] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                    "),//采购退货关联表
                    #endregion
                    #region 采购商品出库
                    GenerateCreateTableScript("WhInventoryOut",@"
                        CREATE TABLE [dbo].[WhInventoryOut](
	                    [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                    [TransactionSysNo] [varchar](32) NULL,
	                    [WarehouseSysNo] [int] NULL,
	                    [SourceType] [int] NULL,
	                    [SourceSysNO] [int] NULL,
	                    [DeliveryType] [int] NULL,
	                    [Remarks] [nvarchar](200) NULL,
	                    [IsPrinted] [int] NULL,
	                    [Status] [int] NULL,
	                    [CreatedBy] [int] NULL,
	                    [CreatedDate] [datetime] NULL,
	                    [LastUpdateBy] [int] NULL,
	                    [LastUpdateDate] [datetime] NULL,
	                    [Stamp] [datetime] NULL,
                     CONSTRAINT [PK_WHINVENTORYOUT] PRIMARY KEY NONCLUSTERED 
                    (
	                    [SysNo] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                    "),//采购商品出库
                    #endregion
                    #region 采购商品出库明细
                    GenerateCreateTableScript("WhInventoryOutItem",@"
                        CREATE TABLE [dbo].[WhInventoryOutItem](
	                    [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                    [InventoryOutSysNo] [int] NULL,
	                    [ProductSysNo] [int] NULL,
	                    [ProductName] [nvarchar](100) NULL,
	                    [StockOutQuantity] [int] NULL,
	                    [RealStockOutQuantity] [int] NULL,
	                    [SourceItemSysNo] [int] NULL,
	                    [Remarks] [nvarchar](200) NULL,
	                    [CreatedBy] [int] NULL,
	                    [CreatedDate] [datetime] NULL,
	                    [LastUpdateBy] [int] NULL,
	                    [LastUpdateDate] [datetime] NULL,
                     CONSTRAINT [PK_WHINVENTORYOUTITEM] PRIMARY KEY NONCLUSTERED 
                    (
	                    [SysNo] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                    "),//采购商品出库明细
                    #endregion
                };
            }
        }


        /// <summary>
        /// 升级表结构Sql脚本数组
        /// </summary>
        private string[] UpgradeAlterTableSqlArray
        {
            get
            {
                #region sql脚本数组
                return new string[]
                {
                    GenerateAddTableFieldScript("CrCustomerRebatesRecord","Flag","ALTER TABLE [CrCustomerRebatesRecord] ADD [Flag] char(1) NOT NULL DEFAULT ('0')"),//增加提现标示字段
                    GenerateAddTableFieldScript("CrPredepositCash","Status","ALTER TABLE [CrPredepositCash] ADD [Status] char(1) NOT NULL DEFAULT ('0')"),//增加审核状态字段                              
                    GenerateAddTableFieldScript("SoOrder","EhkingCipStatus","ALTER TABLE [SoOrder] ADD EhkingCipStatus int NOT NULL DEFAULT (0)"),//增加订单表支付商检推送状态字段
                    GenerateAddTableFieldScript("SoOrder","BalancePay","ALTER TABLE [SoOrder] ADD BalancePay decimal(18,2) NOT NULL DEFAULT (0)"),//增加订单表余额支付字段
                    GenerateAddTableFieldScript("RcReturnItem","DeductRebates","alter table [RcReturnItem] add DeductRebates numeric(10,2) NOT NULL DEFAULT (0)"),//增加退货商品列表扣除返点
                };
                #endregion
            }
        }

        /// <summary>
        /// 升级函数脚本数组
        /// 注：必须先删除函数再创建，一次只能执行一批语句
        /// </summary>
        private string[] UpgradeFunctionSqlArray
        {
            get
            {
                return new string[] 
                { 
                    #region 更根据价格源获取价格函数func_GetPriceSource
                    GenerateDropFunctionScript("func_GetPriceSource"),
                    @"
                        CREATE FUNCTION [dbo].[func_GetPriceSource]
                        (
	                        @SysNo int,
	                        @Status int	
                        )
                        RETURNS varchar(8000) 
                        AS
                        BEGIN
	                       DECLARE @str varchar(8000) 

                           SET @str = '' 
	
	                       SELECT @str = @str + ',' +CONVERT(varchar(20),[Price])+':'+CONVERT(varchar(20),[PriceSource])+':'+CONVERT(varchar(20),[SourceSysNo])
     
                           FROM [PdPrice] WHERE [ProductSysNo]=@SysNo and [Status]=@Status ORDER BY  SysNo 

                           SET @str =STUFF(@str,1,1,'')   --去掉@str中的第一个逗号

                           if @str<>''
	                        begin
	                           SET @str = ','+@str+','
	                        end
	
                          RETURN @str
                       END
                    ", 
                    #endregion
                };
            }
        }
        /// <summary>
        /// 升级存储过程脚本数组
        /// </summary>
        private string[] UpgradeProcedureSqlArray
        {
            get
            {
                return new string[] { };
            }
        }

        #region 私有方法
        /// <summary>
        /// 生成修改或添加表字段脚本
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">表字段</param>
        /// <param name="alterTableScript">更改表字段Sql脚本</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        private string GenerateAddTableFieldScript(string tableName, string fieldName, string alterTableScript)
        {
            return @"
                    IF EXISTS (SELECT * FROM DBO.SYSOBJECTS WHERE id = object_id(N'[" + tableName + @"]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
                    BEGIN
                            IF NOT EXISTS(
                            SELECT  *  FROM  syscolumns  WHERE   id = ( SELECT   id
                               FROM     sysobjects
                               WHERE    name = '" + tableName + @"'
                             )                  
                            AND name = '" + fieldName + @"')
                            BEGIN
                                " + alterTableScript + @" 
                            END
                    END

            ";
        }
        /// <summary>
        /// 生成创建表脚本
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="createTableScript">创建表Sql脚本</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        private string GenerateCreateTableScript(string tableName, string createTableScript)
        {
            return @"
                     IF NOT EXISTS (SELECT * FROM DBO.SYSOBJECTS WHERE id = object_id(N'[" + tableName + @"]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
                     BEGIN
                        " + createTableScript + @"
                     END

            ";
        }
        /// <summary>
        /// 生成删除函数脚本
        /// </summary>
        /// <param name="functionName">函数名称</param>
        /// <returns></returns>
        /// <remarks>2017-1-11 杨浩 创建</remarks>
        private string GenerateDropFunctionScript(string functionName)
        {
            return @"
                    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + functionName + @"]') and xtype in (N'FN', N'IF', N'TF'))
                    BEGIN   
                      drop function [dbo].[" + functionName + @"] -- 删除函数
                    END
                 
                    ";
        }
        /// <summary>
        /// 生成删除存储过程脚本
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <returns></returns>
        /// <remarks>2017-1-11 杨浩 创建</remarks>
        private string GenerateDropProcedureScript(string procedureName)
        {
            return @"
                        if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + procedureName + @"]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                        BEGIN 
                            drop procedure [dbo].[" + procedureName + @"]  -- 删除存储过程
                        END
                        
                    ";
        }
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="scriptContent">脚本内容</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        private bool ExecuteScript(string[] sqlArray)
        {
            //string[] sqlArray = scriptContent
            //  .Split(new string[] { "GO\r\n", "go\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string sql in sqlArray)
            {
                try
                {
                    if (sql.Trim() == string.Empty)
                        continue;
                    Context.Sql(sql).Execute();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #endregion

        /// <summary>
        /// 升级
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        public override bool Upgrade()
        {
            var result = true;

            result = ExecuteScript(UpgradeCreateTableSqlArry); //升级新增表

            if (result)
                result = ExecuteScript(UpgradeAlterTableSqlArray); //升级表结构

            if (result)
                result = ExecuteScript(UpgradeFunctionSqlArray); //升级函数

            if (result)
                result = ExecuteScript(UpgradeProcedureSqlArray); //升级存储过程

            return result;
        }

    }
}
