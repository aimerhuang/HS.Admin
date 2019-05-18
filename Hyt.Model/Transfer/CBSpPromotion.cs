using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model
{
    /// <summary>
    /// 购物车
    /// </summary>
    /// <remarks>2013-08-12 吴文强 创建</remarks>
    public class CBSpPromotion : SpPromotion
    {
        /// <summary>
        /// 促销规则（跳过促销规则条件，1对1直接关联）
        /// </summary>
        public SpPromotionRule PromotionRule { get; set; }

        /// <summary>
        /// 可叠加的促销规则集合(规则系统编号)
        /// </summary>
        public int[] PromotionOverlays { get; set; }

        /// <summary>
        /// 促销可选商品集合(赠品/加购商品)
        /// </summary>
        public List<CBSpPromotionGift> PromotionGifts { get; set; }

        /// <summary>
        /// 促销规则值集合
        /// </summary>
        public List<SpPromotionRuleKeyValue> PromotionRuleKeyValues { get; set; }

        /// <summary>
        /// 促销,用于新建时保存实体,余勇添加,因为在数据层的get方法中只能获取到SpPromotion实体而不能直接得到CBSpPromotion实体，为获取SpPromotion实体数据，增加了该实体
        /// </summary>
        public SpPromotion Promotion { get; set; }
    }
}
