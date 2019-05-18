using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 促销状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class PromotionStatus
    {
        /// <summary>
        /// 促销应用类型
        /// 数据表:SpPromotion 字段:PromotionType
        /// </summary>
        /// <remarks>2013-08-15 吴文强 创建</remarks>
        public enum 促销应用类型
        {
            应用到分类 = 10,
            应用到商品 = 20,
            应用到商品合计 = 50,
            应用到订单合计 = 60,
            应用到运费 = 70,
            优惠券 = 100,
            组合套餐 = 110,
            团购 = 200,
            秒杀 = 250
        }

        /// <summary>
        /// 是否使用促销码
        /// 数据表:SpPromotion 字段:IsUsePromotionCode
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 是否使用促销码
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 促销应用平台
        /// 数据表:SpPromotion 字段:PromotionPlatform
        /// </summary>
        /// <remarks>2013-08-30 吴文强 创建</remarks>
        public enum 促销应用平台
        {
            全部平台 = 0,
            PC网站 = 10,
            手机商城 = 30,
        }

        /// <summary>
        /// 促销状态
        /// 数据表:SpPromotion 字段:Status
        /// </summary>
        /// <remarks>2013-08-26 朱家宏 创建</remarks>
        public enum 促销状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 促销规则类型:团购(20),秒杀(30),抢购(40),拍卖(50),组合(60),满减(100),满赠(110),满折(120),其他(200)
        ///  数据表:SpPromotionRule 字段:RuleType
        /// </summary>
        /// <remarks>2013-08-30 吴文强 创建</remarks>
        public enum 促销规则类型
        {
            团购 = 20,
            秒杀 = 30,
            抢购 = 40,
            拍卖 = 50,
            组合 = 60,
            满减 = 100,
            满赠 = 110,
            满折 = 120,
            其他 = 200,
        }

        /// <summary>
        /// 促销规则状态
        ///  数据表:SpPromotionRule 字段:Status
        /// </summary>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        public enum 促销规则状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 优惠券类型
        /// 数据表:SpCoupon 字段:Type
        /// </summary>
        /// <remarks>2013-08-27 黄志勇 创建</remarks>
        public enum 优惠券类型
        {
            系统 = 10,
            私有 = 20
        }

        /// <summary>
        /// 优惠券状态
        /// 数据表:SpCoupon 字段:Status
        /// </summary>
        /// <remarks>2013-08-27 黄志勇 创建</remarks>
        public enum 优惠券状态
        {
            待审核 = 10,
            已审核 = 20,
            已使用 = 30,
            作废 = -10
        }

        /// <summary>
        /// 是否优惠卡
        /// 数据表:SpCoupon 字段:IsCouponCard
        /// </summary>
        /// <remarks>2013-12-27 吴文强 创建</remarks>
        public enum 是否优惠卡
        {
            是 = 1,
            否 = 0
        }

        /// <summary>
        /// 组合套餐状态
        /// 数据表:SpCombo 字段:Status
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 组合套餐状态
        {
            待审核 = 10,
            已审核 = 20,
            作废 = -10
        }
        public enum 秒杀状态
        {
            待审核 = 10,
            已审核 = 20,
            作废 = -10
        }
        /// <summary>
        /// 是否是套餐主商品
        /// 数据表:SpComboItem 字段:IsMaster
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 是否是套餐主商品
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 商城使用
        /// 数据表:SpPromotion 字段:WebPlatform
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 商城使用
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 门店使用
        /// 数据表:SpPromotion 字段:ShopPlatform
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 门店使用
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 手机商城使用
        /// 数据表:SpPromotion 字段:MallAppPlatform
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 手机商城使用
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 物流App使用
        /// 数据表:SpPromotion 字段:LogisticsAppPlatform
        /// </summary>
        /// <remarks>2014-01-13 吴文强 创建</remarks>
        public enum 物流App使用
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 优惠卡类型状态
        /// 数据表:SpCouponCardType 字段:Status
        /// </summary>
        /// <remarks>2014-01-08 朱成果 创建</remarks>
        public enum 优惠券卡类型状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 优惠卡状态
        /// 数据表:SpCouponCard 字段:Status
        /// </summary>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public enum 优惠券卡状态
        {
            启用 = 1,
            禁用 = 0,
        }
        #region "用户自定义"
        /// <summary>
        /// 促销/优惠券使用平台(读取优惠券时使用)
        /// </summary>
        /// <remarks>2013-12-30 吴文强 创建</remarks>
        public enum 促销使用平台
        {
            PC商城 = 1,
            门店 = 2,
            手机商城 = 3,
            物流App = 4,
        }
        #endregion
    }
}
