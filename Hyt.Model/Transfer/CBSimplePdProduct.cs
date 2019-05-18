using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;

namespace Hyt.Model
{
    /// <summary>
    /// 简单商品信息（前台使用）
    /// </summary>
    /// <remarks>2013-08-14 邵斌 创建</remarks>
    [Serializable]
    public class CBSimplePdProduct
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        public int BrandSysNo { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 商品类型：普通商品（10）、虚拟商品（20）
        /// </summary>
        public int ProductType { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string EasName { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品副名称
        /// </summary>
        public string ProductSubName { get; set; }

        /// <summary>
        /// 商品名称拼音
        /// </summary>
        public string NameAcronymy { get; set; }

        /// <summary>
        /// 商品简称
        /// </summary>
        public string ProductShortTitle { get; set; }

        /// <summary>
        /// 商品简介
        /// </summary>
        public string ProductSummary { get; set; }

        /// <summary>
        /// 商品广告语
        /// </summary>
        public string ProductSlogan { get; set; }

        /// <summary>
        /// 包装清单
        /// </summary>
        public string PackageDesc { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string ProductDesc { get; set; }

        /// <summary>
        /// 商品图片地址
        /// </summary>
        public string ProductImage { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// SeoTitle
        /// </summary>
        public string SeoTitle { get; set; }

        /// <summary>
        /// SeoKeyword
        /// </summary>
        public string SeoKeyword { get; set; }

        /// <summary>
        /// SeoDescription
        /// </summary>
        public string SeoDescription { get; set; }

        /// <summary>
        /// 状态：有效（1）、无效（0）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 前台是否下单：有效（1）、无效（0）
        /// </summary>
        public int CanFrontEndOrder { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 团购系统编号
        /// </summary>
        public int GroupShoppingSysNo { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 关联商品关联关系码
        /// </summary>
        public string ProductAssociationRelationCode { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public IList<CBPdPrice> Prices { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        public IList<PdCategory> Categories { get; set; }

        /// <summary>
        /// 商品促销信息
        /// </summary>
        private IList<SpPromotionHint> PromotionInfo { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public IList<string> Images { get; set; }

        /// <summary>
        /// 商品评分
        /// </summary>
        public decimal CommentScore { get; set; }

        /// <summary>
        /// 商品评分总和
        /// </summary>
        public decimal CommentScoreTotal { get; set; }

        /// <summary>
        /// 商品评论次数
        /// </summary>
        public decimal CommentTimesCount { get; set; }

        /// <summary>
        /// 商品规格
        /// </summary>
        public string Volume { get; set; }

        /// <summary>
        /// 原产地
        /// </summary>
        public string Origin_Name { get; set; }
        /// <summary>
        /// 代理商系统编号
        /// </summary>
        public int AgentSysNo { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        public decimal GrosWeight { get; set; }
        /// <summary>
        /// 净重
        /// </summary>
        public decimal NetWeight { get; set; }
        /// <summary>
        /// 销售网址
        /// </summary>
        public string SalesAddress { get; set; }
    }
}
