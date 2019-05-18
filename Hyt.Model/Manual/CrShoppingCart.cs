using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Common;

namespace Hyt.Model
{
    /// <summary>
    /// 购物车
    /// </summary>
    /// <remarks>2013-08-12 吴文强 创建</remarks>
    //[Serializable]
    public class CrShoppingCart
    {
        /// <summary>
        /// 当前购物车中所有促销
        /// </summary>
        public List<CBSpPromotion> AllPromotions { get; set; }

        /// <summary>
        /// 商品销售合计金额(优惠前的金额)
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 商品优惠金额(优惠的金额)
        /// </summary>
        public decimal ProductDiscountAmount { get; set; }

        /// <summary>
        /// 订单优惠金额(优惠的金额)
        /// </summary>
        /// 
        public decimal OrderDiscountAmount { get; set; }
        /// <summary>
        /// 运费金额(优惠前的金额)
        /// </summary>
        public decimal FreightAmount { get; set; }

        /// <summary>
        /// 税费金额
        /// </summary>
        public decimal TaxFee { get; set; }

        /// <summary>
        /// 运费优惠金额(运费的优惠金额)
        /// </summary>
        public decimal FreightDiscountAmount { get; set; }

        /// <summary>
        /// 结算优惠金额(订单总金额的优惠金额，不包括商品,运费优惠金额)
        /// </summary>
        public decimal SettlementDiscountAmount { get; set; }

        /// <summary>
        /// 优惠券代码
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// 促销码（使用成功的促销码）
        /// </summary>
        public string PromotionCode { get; set; }

        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 结算金额(=商品销售合计金额+运费金额-商品优惠金额-运费优惠金额-订单优惠金额)
        /// SettlementAmount = ProductAmount+FreightAmount-ProductDiscountAmount-FreightDiscountAmount-SettlementDiscountAmount-CouponAmount
        /// </summary>
        public decimal SettlementAmount { get; set; }

        /// <summary>
        /// 购物车组
        /// </summary>
        public IList<CrShoppingCartGroup> ShoppingCartGroups { get; set; }

        /// <summary>
        /// 购物车促销信息
        /// </summary>
        public IList<CrShoppingCartGroupPromotion> GroupPromotions { get; set; }

        private List<object> Debug { get; set; }
        public void WriteDebug(object obj)
        {
            if (Debug == null)
            {
                Debug = new List<object>();
            }
            Debug.Add(obj);
        }
    }

    /// <summary>
    /// 购物车组
    /// </summary>
    /// <remarks>2013-08-12 吴文强 创建</remarks>
    public class CrShoppingCartGroup
    {
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 组代码
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 合计金额(优惠前的金额)
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }

        /// <summary>
        /// 是否过期重置
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// 促销系统编号(多个用";"分隔)
        /// </summary>
        public string Promotions { get; set; }

        /// <summary>
        /// 购物车商品信息
        /// </summary>
        public IList<CBCrShoppingCartItem> ShoppingCartItems { get; set; }

        /// <summary>
        /// 组使用促销信息
        /// </summary>
        public IList<CrShoppingCartGroupPromotion> GroupPromotions { get; set; }
    }

    /// <summary>
    /// 购物车组促销
    /// </summary>
    /// <remarks>2013-08-12 吴文强 创建</remarks>
    public class CrShoppingCartGroupPromotion
    {
        /// <summary>
        /// 促销系统编号
        /// </summary>
        public int PromotionSysNo { get; set; }

        /// <summary>
        /// 促销描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 规则类型:团购(20),秒杀(30),抢购(40),拍卖(50),组合(60),满减(100),满赠(110),满折(120),其他(200)
        /// </summary>
        public int RuleType { get; set; }

        /// <summary>
        /// 规则图标
        /// </summary>
        public string RuleIcon
        {
            //TODO:须实现
            get
            {
                string Icon = "http://image.huiyuanti.com/b2capp/";
                switch (RuleType)
                {
                    case (int)PromotionStatus.促销规则类型.满减:
                        Icon = Icon + "cx_mj.png";
                        break;
                    case (int)PromotionStatus.促销规则类型.满赠:
                        Icon = Icon + "cx_zp.png";
                        break;
                    case (int)PromotionStatus.促销规则类型.团购:
                        Icon = Icon + "cx_tuan.png";
                        break;
                    case (int)PromotionStatus.促销规则类型.组合:
                        Icon = Icon + "cx_zu.png";
                        break;
                }
                return Icon;
            }
            set { }
        }

        /// <summary>
        /// 是否已满足
        /// </summary>
        public bool IsSatisfy { get; set; }

        /// <summary>
        /// 是否已使用
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// 促销赠品
        /// </summary>
        public IList<CBSpPromotionGift> GiftProducts { get; set; }

        /// <summary>
        /// 已选择赠品
        /// </summary>
        public IList<CBSpPromotionGift> UsedGiftProducts { get; set; }
    }

    /// <summary>
    /// Json格式的购物车明细
    /// </summary>
    /// <remarks>2013-09-26 吴文强 创建</remarks>
    public class JsonCartItem 
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 是否选中:是(1),否(0)
        /// </summary>
        public int IsChecked { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public int IsLock { get; set; }

        /// <summary>
        /// 组代码
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 促销系统编号
        /// </summary>
        public string Promotions { get; set; }

        /// <summary>
        /// 商品销售类型
        /// </summary>
        public int ProductSalesType { get; set; }

        /// <summary>
        /// 商品Erp编号 余勇 添加 2014-03-07
        /// </summary>
        public string ProductErpCode { get; set; }

    }
}
