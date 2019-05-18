
using System;
using System.Collections.Generic;

namespace Hyt.Model.B2CApp
{
    #region Model

    /// <summary>
    /// 分享
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class Share
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }
    }
    
    /// <summary>
    /// 反馈类型
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class FeedbackType
    {
        /// <summary>
        /// 编号
        /// </summary>
        /// 
        public int SysNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 反馈信息
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class Feedback
    {
        public int SysNo { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 反馈类型编号
        /// </summary>
        public int FeedbackTypeSysNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 反馈信息
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 来源:商城(10),IphoneApp(20),AndroidApp(30)
        /// </summary>
        public int Source { get; set; }
    }

    /// <summary>
    /// 支付宝配置信息
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class AlipayConfig
    {
        /// <summary>
        /// 合作商户ID。用签约支付宝账号登录ms.alipay.com后，在账户信息页面获取。
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// 商户收款的支付宝账号
        /// </summary>
        public string Seller { get; set; }
        /// <summary>
        /// 商户（RSA）私钥
        /// </summary>
        public string RsaPrivate { get; set; }
        /// <summary>
        /// 支付宝（RSA）公钥 用签约支付宝账号登录ms.alipay.com后，在密钥管理页面获取。
        /// </summary>
        public string RsaAlipayPublic { get; set; }

        /// <summary>
        /// 淘宝服务器异步通知页面路径
        /// </summary>
        public string AliNotifyUrl { get; set; }
    }

    /// <summary>
    /// 支付类型
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class PaymentType
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 支付名称
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public int Type { get; set; }
        
    }

    #endregion

    #region Model

    /// <summary>
    /// App首页模型
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class Home
    {
        /// <summary>
        /// 首页团购推荐
        /// </summary>
        public GroupShopping TopGroupBuy { get; set; }
        /// <summary>
        /// 首页轮换广告展示
        /// </summary>
        public AdvertGroup TopAdverts { get; set; }
        /// <summary>
        /// 首页商品推荐展示
        /// </summary>
        public IList<ProductGroup> RecommendProducts { get; set; }
        /// <summary>
        /// New首页商品推荐展示
        /// </summary>
        public IList<AdvertGroup> NewRecommendProducts { get; set; }
        /// <summary>
        /// 首页底部商品推荐展示
        /// </summary>
        public ProductGroup BottomProducts { get; set; }
    }

    /// <summary>
    /// App首页模型
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class HomeMore
    {
        /// <summary>
        /// 首页更多轮换广告展示
        /// </summary>
        public AdvertGroup TopAdverts { get; set; }

        /// <summary>
        /// 首页更多底部商品推荐展示
        /// </summary>
        public ProductGroup BottomProducts { get; set; }
    }

    #region 团购

    /// <summary>
    /// 团购
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class GroupShopping
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 促销系统编号
        /// </summary>
        public int PromotionSysNo { get; set; }

        /// <summary>
        /// 团购标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 团购小图标
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// 团购剩余时间（毫秒）
        /// </summary>
        public double RemainingTime { get; set; }

        /// <summary>
        /// 已团数量
        /// </summary>
        public int HaveQuantity { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int RemainingQuantity { get; set; }   

        /// <summary>
        /// 限购数量
        /// </summary>
        public int LimitQuantity { get; set; }

        /// <summary>
        /// 总价值
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 团购价
        /// </summary>
        public decimal GroupPrice { get; set; }

        /// <summary>
        /// 节省价格
        /// </summary>
        public decimal SavePrice { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }

    /// <summary>
    /// 团购商品明细
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class GroupShoppingItem
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 产品名字
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }
    }

    /// <summary>
    /// 团购详细对象
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class GroupShoppingDetails:GroupShopping
    {
        public IList<GroupShoppingItem> GroupShoppingItems { get; set; }
    }

    #endregion

    #region 组合

    /// <summary>
    /// 组合商品列表
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class Combo
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 促销系统编号
        /// </summary>
        public int PromotionSysNo { get; set; }

        /// <summary>
        /// 套餐名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 组合价
        /// </summary>
        public decimal ComboPrice { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal BasicPrice { get; set; }

        /// <summary>
        /// 节省多少
        /// </summary>
        public decimal SavePrice { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public IList<ComboItem> ComboItem { get; set; }
    }

    /// <summary>
    /// 组合商品明细
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class ComboItem 
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 价格(等级价格-优惠)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }       
    }

    #endregion

    #endregion
}
