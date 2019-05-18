using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Logis.XinYi
{
    /// <summary>
    /// 心怡商品类
    /// </summary>
    /// <remarks>2015-10-15 杨云奕 添加</remarks>
    [Serializable]
    public class Goods
    {
        /// <summary>
        /// 仓库编码（必填）
        /// 
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 货主编码（必填）
        /// </summary>
        public string OnNumber { get; set; }
        /// <summary>
        /// 商品序号 必选
        /// </summary>
        public string GNo { get; set; }
        /// <summary>
        /// 商品货号 必选
        /// </summary>
        public string CopGNo { get; set; }
        /// <summary>
        /// 厂商编号 可选
        /// </summary>
        public string ProGNo { get; set; }
        /// <summary>
        /// 商品名称 必选
        /// </summary>
        public string GName { get; set; }
        /// <summary>
        /// 规格型号 可选
        /// </summary>
        public string GModel { get; set; }
        /// <summary>
        /// 商品条码 可选
        /// </summary>
        public string BARCode { get; set; }
        /// <summary>
        /// 备注 可选
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// 标准计量单位 必选
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        ///第二计量单位 可选
        /// </summary>
        public string SecUnit { get; set; }
        /// <summary>
        /// 商品信息 报关检时标识，如皮包 可选
        /// </summary>
        public string GoodsMes { get; set; }
        /// <summary>
        /// 操作类型 新增、修改 必选
        /// </summary>
        public string OpType { get; set; }
        /// <summary>
        /// 简称 可选
        /// </summary> 
        public string ShortName { get; set; }
        /// <summary>
        /// 速记码 可选
        /// </summary>
        public string ShorthandCodes { get; set; }
        /// <summary>
        /// 第二条形码 可选
        /// </summary>
        public string SecBARCode { get; set; }
        /// <summary>
        /// 颜色 可选
        /// </summary>
        public string ItSkuColor { get; set; }
        /// <summary>
        /// 大小 可选
        /// </summary>
        public string ItSkuSize { get; set; }
        /// <summary>
        /// 生产厂家 必选
        /// </summary>
        public string Manufactory { get; set; }
        /// <summary>
        /// 品牌 必选
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 商品品质 可选
        /// </summary>
        public string Quality { get; set; }
        /// <summary>
        /// 原产国 如:110来源国别地区表 必选
        /// </summary>
        public string Original { get; set; }
        /// <summary>
        /// 采购地点  必选
        /// </summary>
        public string PurchasePlace { get; set; }
        /// <summary>
        /// 包装种类  可选
        /// </summary>
        public string PackageType { get; set; }
        /// <summary>
        /// 品质证明 可选
        /// </summary>
        public string QualityCertify { get; set; }
        /// <summary>
        /// 商品批次号 可选
        /// </summary>
        public string GoodsBatchNo { get; set; }
        /// <summary>  
        /// 商品类别 可选
        /// </summary>
        public string IttNumber { get; set; }
        /// <summary>
        /// 毛重 可选
        /// </summary>
        public decimal GrossWt { get; set; }
        /// <summary>
        /// 净重 可选
        /// </summary>
        public decimal NetWt { get; set; }
        /// <summary>
        /// 体积 可选
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// 有效期限 可选
        /// </summary>
        public int ExpirationDate { get; set; }
        /// <summary>
        /// 商品编码 可选
        /// </summary>
        public string CodeTS { get; set; }
        /// <summary>
        /// 申报备案价  必选
        /// </summary>
        public int DecPrice { get; set; }
        /// <summary>
        /// 行邮税号 可选
        /// </summary>
        public string PostTariffCode { get; set; }
        /// <summary>
        /// 进出口标志  (E-出口；I-进口；) 必选
        /// </summary>
        public string IEFlag { get; set; }
        /// <summary>
        /// 行邮税名称 可选
        /// </summary>
        public string PostTariffName { get; set; }
        /// <summary>
        /// 商品描述 必选
        /// </summary>
        public string GNote { get; set; }
        /// <summary>
        /// HS税率 可选
        /// </summary>
        public decimal HSTax { get; set; }
        /// <summary>
        /// 行邮税率 可选
        /// </summary>
        public decimal PostTax { get; set; }
        /// <summary>
        /// HS编码 可选
        /// </summary>
        public string HSCode { get; set; }
        /// <summary>
        /// 贸易国别 可选
        /// </summary>
        public string TradeCountry { get;set;}
    }
}
