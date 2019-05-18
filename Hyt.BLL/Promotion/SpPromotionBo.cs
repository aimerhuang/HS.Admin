using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 促销业务
    /// </summary>
    /// <remarks>2013-08-13 吴文强 创建</remarks>
    public class SpPromotionBo : BOBase<SpPromotionBo>
    {
        /// <summary>
        /// 获取有效促销
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <returns>有效促销集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public List<CBSpPromotion> GetValidPromotions(PromotionStatus.促销使用平台[] platformType)
        {
            var promotions = ISpPromotionDao.Instance.GetValidPromotions(platformType);
            return PromotionAssociation(promotions);
        }

        /// <summary>
        /// 获取有效促销
        /// </summary>
        /// <param name="promotionCode">促销代码</param>
        /// <returns>有效促销集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public List<CBSpPromotion> GetValidPromotions(string[] promotionCode)
        {
            var promotions = ISpPromotionDao.Instance.GetValidPromotions(promotionCode);
            return PromotionAssociation(promotions);
        }

        /// <summary>
        /// 获取指定促销编号的促销
        /// </summary>
        /// <param name="sysNo">促销编号</param>
        /// <returns>促销集合</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public List<CBSpPromotion> GetPromotions(int[] sysNo = null)
        {
            if (sysNo == null || sysNo.Count() == 0)
            {
                return new List<CBSpPromotion>();
            }

            var promotions = ISpPromotionDao.Instance.GetPromotions(sysNo);
            return PromotionAssociation(promotions);
        }

        /// <summary>
        /// 促销属性关联
        /// </summary>
        /// <param name="promotions">促销集合</param>
        /// <returns>促销集合</returns>
        /// <remarks>2013-08-14 吴文强 创建</remarks>
        private List<CBSpPromotion> PromotionAssociation(List<SpPromotion> promotions)
        {
            if (promotions == null || promotions.Count == 0)
            {
                return new List<CBSpPromotion>();
            }

            var listPromotionSysNo = promotions.Select(p => p.SysNo).ToArray();
            var allPromotionRuleConditions = ISpPromotionDao.Instance.GetPromotionRuleConditions(listPromotionSysNo);
            var allPromotionRules =
                ISpPromotionDao.Instance.GetPromotionRules(
                    allPromotionRuleConditions.Select(p => p.PromotionRuleSysNo).ToArray());
            var allPromotionOverlays = ISpPromotionDao.Instance.GetPromotionOverlays(listPromotionSysNo);
            var allPromotionGifts = ISpPromotionDao.Instance.GetPromotionGifts(listPromotionSysNo);
            //给实体赋方法(获取赠品缩略图)
            foreach (var p in allPromotionGifts)
            {
                p.GetThumbnail = () => Hyt.BLL.Web.ProductImageBo.Instance.GetProductImagePath(Web.ProductThumbnailType.Image100, p.ProductSysNo);
            }

            var allPromotionRuleKeyValues = ISpPromotionDao.Instance.GetPromotionRuleKeyValues(listPromotionSysNo);

            var listCbSpPromotion = new List<CBSpPromotion>();

            foreach (var spPromotion in promotions)
            {
                var cbSpPromotion = new CBSpPromotion();
                SpPromotion promotion = spPromotion;

                #region SpPromotion转CBSpPromotion
                cbSpPromotion.SysNo = spPromotion.SysNo;
                cbSpPromotion.Description = spPromotion.Description;
                cbSpPromotion.DisplayPrefix = spPromotion.DisplayPrefix;
                cbSpPromotion.SubjectUrl = spPromotion.SubjectUrl;
                cbSpPromotion.PromotionType = spPromotion.PromotionType;
                cbSpPromotion.StartTime = spPromotion.StartTime;
                cbSpPromotion.EndTime = spPromotion.EndTime;
                cbSpPromotion.PromotionCode = spPromotion.PromotionCode;
                cbSpPromotion.IsUsePromotionCode = spPromotion.IsUsePromotionCode;
                cbSpPromotion.PromotionUseQuantity = spPromotion.PromotionUseQuantity;
                cbSpPromotion.PromotionUsedQuantity = spPromotion.PromotionUsedQuantity;
                cbSpPromotion.UserUseQuantity = spPromotion.UserUseQuantity;
                cbSpPromotion.Priority = spPromotion.Priority;
                cbSpPromotion.Status = spPromotion.Status;
                cbSpPromotion.AuditDate = spPromotion.AuditDate;
                cbSpPromotion.CreatedDate = spPromotion.CreatedDate;
                cbSpPromotion.LastUpdateDate = spPromotion.LastUpdateDate;
                #endregion

                var prc = allPromotionRuleConditions
                    .FirstOrDefault(rc => rc.PromotionSysNo == promotion.SysNo);

                cbSpPromotion.PromotionRule = allPromotionRules.FirstOrDefault(pr => prc != null && pr.SysNo == prc.PromotionRuleSysNo);

                var promotionOverlay = allPromotionOverlays.FirstOrDefault(po => po.PromotionSysNo == spPromotion.SysNo);
                if (promotionOverlay != null)
                {
                    var overlayCode = promotionOverlay.OverlayCode;
                    cbSpPromotion.PromotionOverlays = allPromotionOverlays.Where(po => po.OverlayCode == overlayCode).Select(n => n.PromotionSysNo).ToArray();
                }

                cbSpPromotion.PromotionGifts = allPromotionGifts.Where(pg => pg.PromotionSysNo == spPromotion.SysNo).ToList();

                cbSpPromotion.PromotionRuleKeyValues = allPromotionRuleKeyValues.Where(prkv => prkv.PromotionSysNo == spPromotion.SysNo).ToList();

                listCbSpPromotion.Add(cbSpPromotion);
            }

            return listCbSpPromotion;
        }
        /// <summary>
        /// 促销类型
        /// </summary>
        /// <param name="PromotionType"></param>
        /// <returns></returns>
        public List<SpPromotion> GetSpPromotionList(int PromotionType)
        {
            return ISpPromotionDao.Instance.GetSpPromotionList(PromotionType);
        } /// 促销类型
        /// </summary>
        /// <param name="PromotionType"></param>
        /// <returns></returns>
        public List<SpPromotion> GetSpPromotionAllList(int PromotionType)
        {
            return ISpPromotionDao.Instance.GetSpPromotionAllList(PromotionType);
        }

    }
}
