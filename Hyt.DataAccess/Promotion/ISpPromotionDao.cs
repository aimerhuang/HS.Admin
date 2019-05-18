using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 促销
    /// </summary>
    /// <remarks>2013-08-13 吴文强 创建</remarks>
    public abstract class ISpPromotionDao : DaoBase<ISpPromotionDao>
    {
        /// <summary>
        /// 获取有效促销
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <returns>有效促销集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract List<SpPromotion> GetValidPromotions( PromotionStatus.促销使用平台[] platformType);

        /// <summary>
        /// 获取有效促销
        /// </summary>
        /// <returns>有效促销集合</returns>
        /// <param name="promotionCode">促销代码</param>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract List<SpPromotion> GetValidPromotions(string[] promotionCode);

        /// <summary>
        /// 获取促销集合
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <returns>促销集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract List<SpPromotion> GetPromotions(int[] sysNo = null);

        /// <summary>
        /// 获取促销规则关联集合
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销规则关联集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract List<SpPromotionRuleCondition> GetPromotionRuleConditions(int[] promotionSysNo);

        /// <summary>
        /// 获取促销规则集合
        /// </summary>
        /// <param name="sysNo">规则系统编号</param>
        /// <returns>促销规则集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract List<SpPromotionRule> GetPromotionRules(int[] sysNo);

        /// <summary>
        /// 获取促销叠加集合
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销叠加集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract List<SpPromotionOverlay> GetPromotionOverlays(int[] promotionSysNo);

        /// <summary>
        /// 获取促销赠品集合
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销赠品集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract List<CBSpPromotionGift> GetPromotionGifts(int[] promotionSysNo);

        /// <summary>
        /// 获取促销规则键值集合
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销规则键值集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract List<SpPromotionRuleKeyValue> GetPromotionRuleKeyValues(int[] promotionSysNo);

        /// <summary>
        /// 分页获取促销
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        public abstract Pager<SpPromotion> GetPromotion(ParaPromotion filter);
        /// <summary>
        /// 分页获取促销(有分销商)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2016-08-29 周 创建</remarks>
        public abstract Pager<ParaDealerPromotion> GetDealerPromotion(ParaPromotionpager filter);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  黄志勇 创建</remarks>
        public abstract int Insert(SpPromotion entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  黄志勇 创建</remarks>
        public abstract int Update(SpPromotion entity);

        /// <summary>
        /// 根据促销系统编号更新促销已使用次数
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public abstract void UpdateUsedQuantity(int sysNo);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-26  余勇 创建</remarks>
        public abstract SpPromotion Get(int sysNo);

        /// <summary>
        /// 通过促销码来获取数据
        /// </summary>
        /// <param name="promotionCode">促销码</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-12-9  朱家宏 创建</remarks>
        public abstract SpPromotion GetByPromotionCode(string promotionCode);

        /// <summary>
        /// 查询团购和组合以及促销规则中包含的所有商品
        /// </summary>
        /// <returns>促销商品</returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public abstract List<int> GetAllPromotionProduct();

        /// <summary>
        /// 获得已审核，在有效时间内的促销
        /// </summary>
        /// <returns></returns>
        /// 王耀发 2016-1-15 创建
        public abstract List<SpPromotion> GetSpPromotionList(int PromotionType);
        /// <summary>
        /// 所有促销
        /// </summary>
        /// <returns></returns>
        /// 王耀发 2016-1-15 创建
        public abstract List<SpPromotion> GetSpPromotionAllList(int PromotionType);
        /// <summary>
        /// 通过促销编号获取详情
        /// </summary>
        /// <param name="PromotionSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-08-29 周 创建</remarks>
        public abstract SpPromotionDealer GetByPromotionSysNo(int PromotionSysNo);
         /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract int InsertPromotionDealer(SpPromotionDealer entity);
         /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <remarks>2016-08-29 周 创建</remarks>
        public abstract void UpdatePromotionDealer(SpPromotionDealer entity);
    }
}
