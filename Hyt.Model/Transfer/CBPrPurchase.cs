using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 采购单查询实体
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class CBPrPurchase : PrPurchase
    {
        /// <summary>
        /// 仓库后台显示名称
        /// </summary>
        public string BackWarehouseName { get; set; }
        /// <summary>
        /// 仓库前台显示名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string FName { get; set; }
    }

    /// <summary>
    /// 采购单查询实体
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class CBPrPurchaseDetails : CBPrPurchase
    {
        public string ProductName { get; set; }
        public string OneCategory { get; set; }
        public string Price { get; set; }
        public string SecondCategory { get; set; }
        public string ErpCode { get; set; }
        public string ProQuantity { get; set; }
        public string ProEnterQuantity { get; set; }
        public string ProMoney { get; set; }
        public string ProTotalMoney { get; set; }
        public string ProductRemarks { get; set; }
    }

    /// <summary>
    /// 采购单查询实体
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class CBPrPurchaseDetailsOutput 
    {
        public string 采购单代码 { get; set; }
        public string 供应商 { get; set; }
        public string 仓库 { get; set; }
        public string 采购数量 { get; set; }
        public string 已入库数量 { get; set; }
        public string 采购总额 { get; set; }
        public string 创建时间 { get; set; }
        //public string 制单人 { get; set; }
        public string 备注 { get; set; }
        public string 付款状态 { get; set; }
        public string 状态 { get; set; }
        public string 商品编码 { get; set; }
        public string 商品名称 { get; set; }
        public string 一级分类 { get; set; }
        public string 二级分类 { get; set; }
        public string 会员价 { get; set; }
        public string 商品数量 { get; set; }
        public string 商品已入库数量 { get; set; }
        public string 商品单价 { get; set; }
        public string 商品总金额 { get; set; }
        public string 商品备注 { get; set; }
       
    }
}
