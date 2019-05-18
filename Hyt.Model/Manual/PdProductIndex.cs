using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 商品索引实体
    /// </summary>
    /// <remarks>2013-08-02 黄波 创建</remarks>
    /// 
    [DataContract]
    [Serializable]
    public class PdProductIndex
    {
        /// <summary>
        /// 商品系统编号
        /// </summary>
        /// 
        [DataMember]
        public int SysNo { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        /// 
        [DataMember]
        public string Barcode { get; set; }
        /// <summary>
        /// ERP编码
        /// </summary>/// 
        [DataMember]
        public string ErpCode { get; set; }
        /// <summary>
        /// 商品二维码
        /// </summary>
        [DataMember]
        public string QRCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }
        /// <summary>
        /// 商品副名称
        /// </summary>
        [DataMember]
        public string ProductSubName { get; set; }
        /// <summary>
        /// 商品品牌
        /// </summary>
        [DataMember]
        public int BrandSysNo { get; set; }
        /// <summary>
        /// 商品分类
        /// </summary>
        [DataMember]
        public int Category { get; set; }
        /// <summary>
        /// 商品关联分类
        /// ,分类编号,....,分类编号,
        /// </summary>
        [DataMember]
        public string AssociationCategory { get; set; }
        /// <summary>
        /// 商品名称拼音
        /// </summary>
        [DataMember]
        public string NameAcronymy { get; set; }
        /// <summary>
        /// 商品主图格式化地址
        /// </summary>
        [DataMember]
        public string ProductImage { get; set; }
        /// <summary>
        /// 商品价格
        /// ,价格:价格来源,来源编号,...,价格:价格来源,来源编号,
        /// </summary>
        [DataMember]
        public string Prices { get; set; }
        /// <summary>
        /// 分销商编号
        /// ,编号1,编号2,...,编号x,
        /// </summary>
        public string DealerSysNos { get; set; }
        /// <summary>
        /// 仓库编号
        /// ,编号1,编号2,...,编号x,
        /// </summary>
        [DataMember]
        public string WarehouseSysNos { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        [DataMember]
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 经销商价格
        /// </summary>
        [DataMember]
        public decimal DealerPrice { get; set; }
        /// <summary>
        /// 经销商产品状态
        /// </summary>
        [DataMember]
        public int DealerProductStatus { get; set; }

        /// <summary>
        /// 该商品所具有可作为搜索属性的键值集合
        /// 可作为搜索属性的属性类型始终为选项类型
        /// 属性编号,属性值编号;...属性编号,属性值编号
        /// </summary>
        [DataMember]
        public string Attributes { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        [DataMember]
        public int SalesCount { get; set; }
        /// <summary>
        /// 评论总数
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 喜欢总数
        /// </summary>
        [DataMember]
        public int Liking { get; set; }

        /// <summary>
        /// 收藏总数
        /// </summary>
        [DataMember]
        public int Favorites{get;set;}

        /// <summary>
        /// 评论总数
        /// </summary>
        [DataMember]
        public int Comments{get;set;}
        
        /// <summary>
        /// 晒单总数
        /// </summary>
        [DataMember]
        public int Shares{get;set;}

        /// <summary>
        /// 咨询总数
        /// </summary>
        [DataMember]
        public int Question{get;set;}

        /// <summary>
        /// 平均评分
        /// </summary>
        [DataMember]
        public double AverageScore { get; set; }

        /// <summary>
        /// 总评分
        /// </summary>
        [DataMember]
        public double TotalScore { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// 前台显示标志(仅在搜索广告商品组是有值)
        /// 不存在索引
        /// </summary>
        [DataMember]
        public int DispalySymbol { get; set; }
        /// <summary>
        /// 商品基础价格
        /// 不存在索引
        /// </summary>
        [DataMember]
        public decimal BasicPrice { get; set; }
        /// <summary>
        /// 商品客户等级价格
        /// 登录后会返回客户等级价
        /// 未登录返回基础价格
        /// 不存在索引
        /// </summary>
        [DataMember]
        public decimal RankPrice { get; set; }

        /// <summary>
        /// 商品创建时间
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 是否前台下单
        /// <remarks>2013-20-23 邵斌 添加字段</remarks>
        /// </summary>
        [DataMember]
        public int CanFrontEndOrder { get; set; }

        /// <summary>
        /// 商品组
        /// </summary>
        [DataMember]
        public string ProductGroupCode { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [DataMember]
        public int OriginSysNo { get; set; }

        /// <summary>
        /// 前台显示
        /// </summary>
        [DataMember]
        public int IsFrontDisplay { get; set; }

        /// <summary>
        /// 商品会员价格
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// 商品类型：普通商品 = 10,虚拟商品 = 20,保税商品 = 30,直邮商品 = 40,完税商品 = 50
        /// </summary>
        [DataMember]
        public int ProductType { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        [DataMember]
        public decimal Tax { get; set; }

    }
}
