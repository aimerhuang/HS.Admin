using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 商品状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class ProductStatus
    {
        /// <summary>
        /// 商品类型
        /// 数据表:PdProduct 字段:ProductType
        /// </summary>
        /// <remarks>
        /// 2013-06-18 吴文强 创建
        /// 2015-09-04 杨浩 添加 保税商品 直邮商品
        /// </remarks>
        
        public enum 商品类型
        {
            普通商品 = 10,
            虚拟商品 = 20,
            保税商品 = 30,
            直邮商品 = 40,
            完税商品 = 50,
        }

        /// <summary>
        /// 商品状态
        /// 数据表:PdProduct 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品状态
        {
            上架 = 1,
            下架 = 0,
            作废 = 2,
        }

        /// <summary>
        /// 商品是否前台下单
        /// 数据表:PdProduct 字段:CanFrontEndOrder
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品是否前台下单
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 品牌状态
        /// 数据表:PdBrand 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 品牌状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 产品上线状态
        /// 数据表:PdOnlineSchedule 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 产品上线状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 产品价格来源
        /// 数据表:PdPrice 字段:PriceSource
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 产品价格来源
        {
            /// <summary>
            /// 基础价格的价格来源编号为0
            /// </summary>
            基础价格 = 0,
            会员等级价 = 10,
            线下门店价 = 20,
            线下批发价 = 25,
            分销商等级价 = 40,
            /// <summary>
            /// 配送员进货价的价格来源编号为0
            /// </summary>
            配送员进货价 = 70,
            业务员销售限价=80,
            门店销售价=90,
        }

        /// <summary>
        /// 产品价格状态
        /// 数据表:PdPrice 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 产品价格状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 产品价格变更状态
        /// 数据表:PdPriceHistory 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 产品价格变更状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 是否是主分类
        /// 数据表:PdCategoryAssociation 字段:IsMaster
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否是主分类
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 是否前端展示
        /// 数据表:PdCategory 字段:IsOnline
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否前端展示
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 商品分类状态
        /// 数据表:PdCategory 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品分类状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 属性参数模板状态
        /// 数据表:PdAttributeTemplate 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 属性参数模板状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 商品属性关联状态
        /// 数据表:PdProductAttribute 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品属性关联状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 是否用做关联属性
        /// 数据表:PdAttribute 字段:IsRelationFlag
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否用做关联属性
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 是否作为搜索条件
        /// 数据表:PdAttribute 字段:IsSearchKey
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否作为搜索条件
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 前台属性值呈现类型
        /// (主要用户产品详细页面的产品规格显示。具体参数详细中任然用文本。)
        /// 数据表:PdAttribute 字段:FrontValueShowType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 前台属性值呈现类型
        {
            文本 = 1,
            图标 = 2,
        }

        /// <summary>
        /// 商品属性状态
        /// 数据表:PdAttribute 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品属性状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 商品属性类型
        /// 数据表:PdAttribute 字段:AttributeType
        /// </summary>
        /// <remarks>2013-06-28 吴文强 创建</remarks>
        public enum 商品属性类型
        {
            文本类型 = 10,
            图片类型 = 20,
            选项类型 = 30,
        }

        /// <summary>
        /// 商品属性选项状态
        /// 数据表:PdAttributeOption 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品属性选项状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 商品属性分类状态
        /// 数据表:PdAttributeCategory 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品属性分类状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 商品属性分组状态
        /// 数据表:PdAttributeGroup 字段:Status
        /// </summary>
        /// <remarks>2013-06-27 吴文强 创建</remarks>
        public enum 商品属性分组状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 商品描述模板类型
        /// 数据表:PdTemplate 字段:Type
        /// </summary>
        /// <remarks>2013-06-27 吴文强 创建</remarks>
        public enum 商品描述模板类型
        {
            模板 = 10,
            模块 = 20,
        }

        /// <summary>
        /// 商品图片状态
        /// 数据表:PdProductImage 字段:Status
        /// </summary>
        /// <remarks>2013-12-05 吴文强 创建</remarks>
        public enum 商品图片状态
        {
            显示 = 1,
            隐藏 = 0,
        }

        /// <summary>
        /// 定制商品状态
        /// 数据表:PdProductPrivate 字段:Status
        /// </summary>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        public enum 定制商品状态
        {
            已处理 = 1,
            未处理 = 0,
            作废 = -1,
        }

        /// <summary>
        /// 是否前台显示
        /// 数据表:PdProduct 字段:IsFrontDisplay
        /// </summary>
        /// <remarks>2015-12-19 王耀发 创建</remarks>
        public enum 前台显示
        {
            是 = 1,
            否 = 0,

        }
        #region 自定义
        /// <summary>
        /// 商品前台网站展示类型
        /// </summary>
        /// <remarks>2013-09-10 吴文强 创建</remarks>
        /// <remarks>2013-11-26 苟治国 迁移</remarks>
        public enum ProductOnlineShowType
        {
            /// <summary>
            /// 正常
            /// </summary>
            [Description("正常")]
            Normal = 10,
            /// <summary>
            /// 新品
            /// </summary>
            [Description("新品")]
            New = 20,
            /// <summary>
            /// 热销
            /// </summary>
            [Description("热销")]
            Hot = 30,
            /// <summary>
            /// 特价
            /// </summary>
            [Description("特价")]
            Recommend = 40,
        }
        #endregion
    }
}
