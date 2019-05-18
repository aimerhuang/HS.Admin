using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 物流APP商品
    /// </summary>
    /// <remarks>2013-07-31 周唐炬 创建</remarks>
    [DataContract]
    public class AppProduct
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public int SysNo { get; set; }

        /// <summary>
        /// 商品原价格
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// 当前用户等级价格
        /// </summary>
        [DataMember]
        public decimal LevelPrice { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片地址集合
        /// </summary>
        [DataMember]
        public IList<ProductImage> ImgList { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        [DataMember]
        public string Thumbnail { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string ProductDesc { get; set; }

        /// <summary>
        /// 商品评分
        /// </summary>
        [DataMember]
        public double ProductCommentScore { get; set; }

        /// <summary>
        /// 商品评论次数
        /// </summary>
        [DataMember]
        public int CommentTimesCount { get; set; }

        /// <summary>
        /// 是否允许前台下单：是（1）、否（0）
        /// </summary>
        [DataMember]
        public int CanFrontEndOrder { get; set; }

        /// <summary>
        /// 销量 余勇 添加 用于列表排序 2014-06-05
        /// </summary>
        public int SalesCount { get; set; }

        /// <summary>
        /// 商品属性组
        /// </summary>
        [DataMember]
        public IList<AppProductAttributeGroup> AttributeGroupList { get; set; }
    }

    /// <summary>
    /// 商品图片
    /// </summary>
    /// <remarks>2013-07-31 周唐炬 创建</remarks>
   [DataContract]
    public class ProductImage
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public int SysNo { get; set; }
        /// <summary>
        /// 商品系统编号
        /// </summary>
        [DataMember]
       public int ProductSysNo { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [DataMember]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 状态:显示(1),隐藏(0)
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// 物流APP商品属性
    /// </summary>
   /// <remarks>2013-07-31 周唐炬 创建</remarks>
    public class AppProductAttribute
    {
        /// <summary>
        /// 属性编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 属性文本值
        /// </summary>
        public string AttributeText { get; set; }

        /// <summary>
        /// 属性图片值
        /// </summary>
        public string AttributeImage { get; set; }

        /// <summary>
        /// 属性类型：文本类型（10）、图片类型（20）、选项类型
        /// </summary>
        public int AttributeType { get; set; }
    }

    /// <summary>
    /// 物流APP商品属性组
    /// </summary>
    /// <remarks>2013-07-31 周唐炬 创建</remarks>
    public class AppProductAttributeGroup
    {
        /// <summary>
        /// 属性组编号
        /// </summary>
        public int GroupSysNo { get; set; }

        /// <summary>
        /// 属性组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 属性集合
        /// </summary>
        public IList<AppProductAttribute> AttributeList { get; set; }
    }
}
