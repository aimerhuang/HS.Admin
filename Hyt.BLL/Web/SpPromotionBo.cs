using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 前台商品促销操作
    /// </summary>
    /// <remarks>2013-08-13 绍斌 创建</remarks>
    public class SpPromotionBo : BOBase<SpPromotionBo>
    {
        /// <summary>
        /// 根据商品系统编号获取商品促销信息
        /// </summary>
        /// <param name="platformType">促销使用平台</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="containGroup">是否包含团购</param>
        /// <returns>返回单个商品促销</returns>
        /// <remarks>2013-08-13 绍斌 创建</remarks>
        public IList<SpPromotionHint> GetSpPromotionByProductSysNo(PromotionStatus.促销使用平台[] platformType, int productSysNo, bool containGroup = false)
        {
            IList<SpPromotionHint> result = CacheManager.Get<IList<SpPromotionHint>>(CacheKeys.Items.SingleProductSpPromotionInfo_,
                                                                                productSysNo.ToString(),
                                                                                delegate()
                                                                                {
                                                                                    return BLL.Promotion
                                                                                           .SpPromotionEngineBo
                                                                                           .Instance
                                                                                           .CheckPromotionHints(platformType,
                                                                                               productSysNo, containGroup);
                                                                                });

            //if (isUseProductDetial)
            //{
            //    return result.Where(s => s.RuleType != (int)PromotionStatus.促销规则类型.团购 && s.RuleType != (int)PromotionStatus.促销规则类型.组合)
            //          .ToList();
            //}

            return result;
        }
    }
}
