using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// 简单商品模型
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class SimplProduct
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 当前用户等级价格
        /// </summary>
        public decimal LevelPrice { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 商品促销图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 推荐展示规则
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 会员价格名
        /// </summary>
        public string LevelName { get; set; }
    }

    /// <summary>
    /// 订购明细
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class SimplProductItem : SimplProduct
    {
        /// <summary>
        /// 订购数量
        /// </summary>
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 商品详细模型
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ProductDetail
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 商品原价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 会员价格等级组
        /// </summary>
        public IList<Prices> MemberPrices { get; set; }

        /// <summary>
        /// 当前价格等级名称
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// 当前用户等级价格
        /// </summary>
        public decimal LevelPrice { get; set; }

        /// <summary>
        /// 如果商品为团购，就存在团购价
        /// </summary>
        public decimal GroupShoppingPrice { get; set; }

        /// <summary>
        /// 为当前用户节省价格
        /// </summary>
        public decimal SavePrice { get; set; }

        /// <summary>
        /// 商品评分
        /// </summary>
        public double Grade { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片地址集合
        /// </summary>
        public IList<Images> ImgList { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 商品广告语
        /// </summary>
        public string ProductSlogan { get; set; }

        /// <summary>
        /// 晒单数
        /// </summary>
        public int ShowOrderNum { get; set; }

        /// <summary>
        /// 商品咨询数
        /// </summary>
        public int ConsultingNum { get; set; }

        /// <summary>
        /// 关联商品属性组
        /// </summary>
        public IList<ProductAttribute> Attributes { get; set; }

        /// <summary>
        /// 推荐展示规则
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 是否有组合
        /// </summary>
        public bool HasGroup { get; set; }

        /// <summary>
        /// 是否有团购
        /// </summary>
        public bool HasTuan { get; set; }

        /// <summary>
        /// 是否已关注
        /// </summary>
        public bool IsAttention { get; set; }
    }

    /// <summary>
    /// 商品价格组
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class Prices
    {
        /// <summary>
        /// 等级系统编号
        /// </summary>
        public int LevelSysNo;
        /// <summary>
        /// 会员等级价格
        /// </summary>
        public string PriceName { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }
    }

    /// <summary>
    /// 商品图片地址集合
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class Images
    {
        /// <summary>
        /// 图片缩略图地址
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 大图片地址
        /// </summary>
        public string OriginalImg { get; set; }
    }

    /// <summary>
    /// 商品组
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ProductGroup
    {
        /// <summary>
        /// 商品组图标
        /// </summary>
        public string ProductGroupIcon { get; set; }

        /// <summary>
        /// 颜色值
        /// </summary>
        public string NameColor { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 组所对应的商品明细
        /// </summary>
        public IList<SimplProduct> Items { get; set; }
    }

    /// <summary>
    /// 商品属性
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ProductAttribute
    {
        /// <summary>
        /// 商品属性系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 属性名称(前台显示用)
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 商品属性选项
        /// </summary>
        public IList<AttributeOption> AttributeOptions { get; set; }
    }

    /// <summary>
    /// 商品属性选项
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class AttributeOption
    {
        /// <summary>
        /// 商品属性选项编号
        /// </summary>
        public int AttributeOptionSysNo { get; set; }

        /// <summary>
        /// 产品编号(产品详情用)
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 属性文本值
        /// </summary>
        public string AttributeText { get; set; }

        /// <summary>
        /// 属性值图片
        /// </summary>
        public string Image { get; set; }
    }

    /// <summary>
    /// 产品规格
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class Specification
    {
        /// <summary>
        /// 属性名称(前台显示用)
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 属性文本值
        /// </summary>
        public string AttributeText { get; set; }
    }

    #region 评论

    /// <summary>
    /// 评论评分
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class PdCommentTotal
    {
        /// <summary>
        /// 总评数
        /// </summary>
        public int TotalComments { get; set; }
        /// <summary>
        /// 满意数
        /// </summary>
        public int Satisfaction { get; set; }

        /// <summary>
        /// 一般数量
        /// </summary>
        public int General { get; set; }

        /// <summary>
        /// 不满意数量
        /// </summary>
        public int Dissatisfied { get; set; }

        /// <summary>
        ///  总体平均评分
        /// </summary>
        public double OverallScore { get; set; }

        /// <summary>
        /// 用户参与数量
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// 评论列表
        /// </summary>
        public IList<PdCommentList> CommentList { get; set; }
    }

    /// <summary>
    /// 产品评论列表项
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class PdCommentList
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 会员账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CommentTime { get; set; }

        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime BuyTime { get; set; }

    }

    /// <summary>
    /// 评论
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class PdComment : PdCommentList
    {

        /// <summary>
        /// 会员编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 评论标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 优点
        /// </summary>
        public string Advantage { get; set; }

        /// <summary>
        /// 缺点
        /// </summary>
        public string Disadvantage { get; set; }
    }

    #endregion

    /// <summary>
    /// 搜索模型
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class PdSearch
    {
        /// <summary>
        /// 产品列表
        /// </summary>
        public IList<SimplProduct> Products { get; set; }

        /// <summary>
        /// 返回一个分类系统号(用于属性检索)
        /// </summary>
        public int CategorySysNo { get; set; }
    }

    #region 前台晒单

    /// <summary>
    /// 晒单列表
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ShareOrderList
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 会员账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 回复数量
        /// </summary>
        public int ReplyCount { get; set; }

        /// <summary>
        /// 晒单时间
        /// </summary>
        public DateTime ShareTime { get; set; }

        /// <summary>
        /// 晒单标题
        /// </summary>
        public string ShareTitle { get; set; }

    }

    /// <summary>
    /// 晒单详细
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ShareOrderDetails : ShareOrderList
    {
        /// <summary>
        /// 晒单内容
        /// </summary>
        public string ShareContent { get; set; }

        /// <summary>
        /// 晒单图片
        /// </summary>
        public IList<ProductCommentImage> Images { get; set; }

        /// <summary>
        /// 晒单回复
        /// </summary>
        public IList<ProductCommentReply> Replies { get; set; }
    }

    /// <summary>
    /// 用于新增晒单
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class PostShareOrder
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 晒单标题
        /// </summary>
        public string ShareTitle { get; set; }
        /// <summary>
        /// 晒单内容
        /// </summary>
        public string ShareContent { get; set; }

        /// <summary>
        /// 要上传的晒单图片
        /// </summary>
        public string[] Pictures { get; set; }
    }

    /// <summary>
    /// 晒单回复
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ProductCommentReply
    {
        /// <summary>
        /// 会员账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime ReplyDate { get; set; }
    }

    /// <summary>
    /// 晒单图片
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ProductCommentImage
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath { get; set; }
    }

    #endregion

    #region 个人中心晒单

    /// <summary>
    /// 个人中心晒单、评价商品列表
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ShowOrComment
    {
        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Quantity { get; set; }

        public string ProductName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal LevelPrice { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 可操作项
        /// </summary>
        public string OperateStatus { get; set; }

        /// <summary>
        /// 推荐展示规则
        /// </summary>
        public string Specification { get; set; }
    }

    /// <summary>
    /// 关注商品模型
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class AttentionProduct
    {
        /// <summary>
        /// 关注系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 当前用户等级价格
        /// </summary>
        public decimal LevelPrice { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 推荐展示规则
        /// </summary>
        public string Specification { get; set; }
    }

    #endregion

    #region 商品咨询

    /// <summary>
    /// 商品咨询列表
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class CustomerQuestion
    {
        /// <summary>
        /// 用户咨询昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 咨询内容
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 咨询时间
        /// </summary>
        public DateTime QuestionDate { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime AnswerDate { get; set; }
    }

    /// <summary>
    /// 咨询添加模型
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class CustomerQuestionAdd
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 咨询类型：商品（10）、支付（20）、配送（30）、其它
        /// </summary>
        public int QuestionType { get; set; }
        /// <summary>
        /// 咨询内容
        /// </summary>
        public string Question { get; set; }
    }

    #endregion

}
