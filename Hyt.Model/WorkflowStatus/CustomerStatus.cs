using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 会员状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class CustomerStatus
    {
        /// <summary>
        /// 是否默认地址
        /// 数据表:CrReceiveAddress 字段:IsDefault
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否默认地址
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 大宗采购状态
        /// 数据表:CrBulkPurchase 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 大宗采购状态
        {
            待处理 = 10,
            已处理 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 邮箱状态
        /// 数据表:CrCustomer 字段:EmailStatus
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 邮箱状态
        {
            未验证 = 10,
            已验证 = 20,
        }

        /// <summary>
        /// 手机状态
        /// 数据表:CrCustomer 字段:MobilePhoneStatus
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 手机状态
        {
            未验证 = 10,
            已验证 = 20,
        }

        /// <summary>
        /// 性别
        /// 数据表:CrCustomer 字段:Gender
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 性别
        {
            保密 = 0,
            男 = 1,
            女 = 2,
        }

        /// <summary>
        /// 婚姻状况
        /// 数据表:CrCustomer 字段:MaritalStatus
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 婚姻状况
        {
            保密 = 0,
            未婚 = 1,
            已婚 = 2,
        }

        /// <summary>
        /// 注册来源
        /// 数据表:CrCustomer 字段:RegisterSource
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 注册来源
        {
            PC网站 = 10,
            信营全球购B2B2C3G网站 = 15,
            门店 = 20,
            商城IphoneApp = 31,
            商城AndroidApp = 32,
            物流App = 40,
            分销工具 = 50,
            商城客服注册 = 60,
            商城客户管理=70,
            三方商城=80,
        }

        /// <summary>
        /// 会员状态
        /// 数据表:CrCustomer 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 会员状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 等级是否固定
        /// 数据表:CrCustomer 字段:IsLevelFixed
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 等级是否固定
        {
            固定 = 1,
            不固定 = 0,
        }

        /// <summary>
        /// 经验积分是否固定
        /// 数据表:CrCustomer 字段:IsExperiencePointFixed
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 经验积分是否固定
        {
            固定 = 1,
            不固定 = 0,
        }

        /// <summary>
        /// 惠源币是否固定
        /// 数据表:CrCustomer 字段:IsExperienceCoinFixed
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 惠源币是否固定
        {
            固定 = 1,
            不固定 = 0,
        }

        /// <summary>
        /// 是否接收邮件
        /// 数据表:CrCustomer 字段:IsReceiveEmail
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 是否接收邮件
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 是否接收短信
        /// 数据表:CrCustomer 字段:IsReceiveShortMessage
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 是否接收短信
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 是否是公共账户
        /// 数据表:CrCustomer 字段:IsPublicAccount
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 是否是公共账户
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 会员投诉类型
        /// 数据表:CrComplaint 字段:ComplainType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 会员投诉类型
        {
            商品 = 10,
            订单 = 20,
            物流 = 30,
            售后 = 40,
            其他 = 50,
        }

        /// <summary>
        /// 会员投诉状态
        /// 数据表:CrComplaint 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 会员投诉状态
        {
            待处理 = 10,
            处理中 = 20,
            已处理 = 30,
            作废 = -10,
        }

        /// <summary>
        /// 会员投诉回复类型
        /// 数据表:CrComplaintReply 字段:ReplyerType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 会员投诉回复类型
        {
            客服回复 = 10,
            会员回复 = 20,

        }

        /// <summary>
        /// 会员咨询类型
        /// 数据表:CrCustomerQuestion 字段:QuestionType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 会员咨询类型
        {
            商品 = 10,
            支付 = 20,
            配送 = 30,
            其他 = 40,
        }

        /// <summary>
        /// 会员咨询状态
        /// 数据表:CrCustomerQuestion 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 会员咨询状态
        {
            待回复 = 10,
            已回复 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 商品浏览方式
        /// 数据表:CrBrowseHistory 字段:BrowseType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品浏览方式
        {
            PC网站 = 10,
            信营全球购B2B2C3G网站 = 15,
            手机商城 = 30,
            物流App = 40,
        }

        /// <summary>
        /// 通知类型
        /// 数据表:CrNotice 字段:NoticeType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 通知类型
        {
            降价通知 = 10,
            到货通知 = 20,
        }

        /// <summary>
        /// 通知方式
        /// 数据表:CrNotice 字段:NoticeWay
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 通知方式
        {
            短信 = 10,
            邮件 = 20,
        }

        /// <summary>
        /// 站内信状态
        /// 数据表:CrSiteMessage 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 站内信状态
        {
            未读 = 10,
            已读 = 20,
        }

        /// <summary>
        /// 惠源币是否可用于支付货款
        /// 数据表:CrCustomerLevel 字段:CanPayForProduct
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 惠源币是否可用于支付货款
        {
            支持 = 1,
            不支持 = 0,
        }

        /// <summary>
        /// 惠源币是否可用于支付服务
        /// 数据表:CrCustomerLevel 字段:CanPayForService
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 惠源币是否可用于支付服务
        {
            支持 = 1,
            不支持 = 0,
        }

        /// <summary>
        /// 惠源币变更类型
        /// 数据表:CrExperienceCoinLog 字段:ChangeType
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 惠源币变更类型
        {
            充值 = 10,
            积分兑换 = 15,
            交易变更 = 20,
        }

        /// <summary>
        /// 经验积分变更类型
        /// 数据表:CrExperiencePointLog 字段:PointType
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 经验积分变更类型
        {
            系统赠送 = 10,
            交易变更 = 20,
            参与活动 = 30,
            客服调整 = 90,
            过期调整 = 100,
            积分兑换 = 110,
        }

        /// <summary>
        /// 可用积分变更类型
        /// 数据表:CrAvailablePointLog 字段:PointType
        /// </summary>
        /// <remarks>2013-10-31 吴文强 创建</remarks>
        public enum 可用积分变更类型
        {
            系统赠送 = 10,
            交易变更 = 20,
            参与活动 = 30,
            门店交易 = 40,
            积分兑换 = 50,
            客服调整 = 90,
            过期调整 = 100,
        }

        /// <summary>
        /// 等级积分日志变更类型
        /// 数据表:CrLevelPointLog 字段:ChangeType
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 等级积分日志变更类型
        {
            交易变更 = 20,
            客服调整 = 90,
            过期调整 = 100,
        }

        /// <summary>
        /// 等级日志变更类型
        /// 数据表:CrLevelLog 字段:ChangeType
        /// </summary>
        /// <remarks>2013-06-21 吴文强 创建</remarks>
        public enum 等级日志变更类型
        {
            交易变更 = 20,
            客服调整 = 90,
            过期调整 = 100,
        }

        /// <summary>
        /// 购物车是否锁定
        /// 数据表:CrShoppingCartItem 字段:IsLock
        /// </summary>
        /// <remarks>2013-08-16 吴文强 创建</remarks>
        public enum 购物车是否锁定
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 购物车是否过期重置
        /// 数据表:CrShoppingCartItem 字段:IsExpireReset
        /// </summary>
        /// <remarks>2013-08-16 吴文强 创建</remarks>
        public enum 购物车是否过期重置
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 购物车商品来源
        /// 数据表:CrShoppingCartItem 字段:Source
        /// </summary>
        /// <remarks>2013-08-16 吴文强 创建</remarks>
        public enum 购物车商品来源
        {
            PC网站 = 10,
            信营全球购B2B2C3G网站 = 15,
            门店下单 = 20,
            手机商城 = 30,
            客服下单 = 50,
            配送员下单 = 60,
            换货销售单 = 70,
        }

        /// <summary>
        /// 商品销售类型
        /// 数据表:CrShoppingCartItem 字段:ProductSalesType
        /// </summary>
        /// <remarks>2013-08-16 吴文强 创建</remarks>
        public enum 商品销售类型
        {
            普通 = 10,
            团购 = 20,
            秒杀 = 30,
            抢购 = 40,
            拍卖 = 50,
            组合 = 60,
            赠品 = 90,
            格格家 = 91,
            众筹 = 100,
            搭配销售 = 110,
            预约 = 120,
            积分商城 = 130,
        }

        /// <summary>
        /// 是否选中
        /// 数据表:CrShoppingCartItem 字段:IsChecked
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 是否选中
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 意见反馈类型来源
        /// 数据表:CrFeedbackType 字段:Source
        /// </summary>
        /// <remarks>2013-08-16 吴文强 创建</remarks>
        /// <remarks>2014-05-04 余勇 添加 CRM客服系统</remarks>
        public enum 意见反馈类型来源
        {
            商城 = 10,
            IphoneApp = 20,
            AndroidApp = 30,
            Crm = 40
        }

        #region 自定义

        /// <summary>
        /// 积分更新目标对象
        /// 指定更新数据表
        /// </summary>
        /// <remarks>2013-07-03 吴文强 创建</remarks>
        public enum 积分更新目标
        {
            所有,
            惠源币日志,
            经验积分日志,
            等级积分日志
        }

        #endregion

        public enum 短信咨询状态
        {
            待回复=10,
            已回复=20,
            回复失败=-5,
            作废=-10
        }

        /// <summary>
        /// 会员分销商申请状态
        /// 数据表:CrDealerApply 字段:Status
        /// </summary>
        /// <remarks>2016-4-8 刘伟豪 创建</remarks>
        public enum 会员分销商申请状态
        {
            待审 = 10,
            通过 = 20,
            拒绝 = -10
        }
        /// <summary>
        /// 我的分销团队类型
        /// </summary>
        public enum 我的分销团队
        {
            直接推荐=1,
            间接推荐=2
        }
        /// <summary>
        /// O2O加盟申请状态
        /// </summary>
        public enum O2O加盟申请状态
        {
            作废 = -10,
            未审核 = 10,
            已审核 = 20
        }
    }
}