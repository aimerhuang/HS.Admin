using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 促销规则操作
    /// </summary>
    /// <remarks>2013-08-26 朱成果 创建</remarks>
    public class SpPromotionRuleBo : BOBase<SpPromotionRuleBo>
    {
        /// <summary>
        /// 判断规则名称是否存在
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="excludesysNo">排除的规则编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        public bool ExistsRule(string ruleName, int excludesysNo)
        {
            return Hyt.DataAccess.Promotion.ISpPromotionRuleDao.Instance.ExistsRule(ruleName, excludesysNo);
        }

        /// <summary>
        /// 获取规则信息
        /// </summary>
        /// <param name="sysNo">规则编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        public SpPromotionRule GetEntity(int sysNo)
        {
            return Hyt.DataAccess.Promotion.ISpPromotionRuleDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 获取促销规则列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public List<SpPromotionRule> GetListByPromotionSysNo(int promotionSysNo)
        {
            return Hyt.DataAccess.Promotion.ISpPromotionRuleDao.Instance.GetListByPromotionSysNo(promotionSysNo);
        }

        /// <summary>
        /// 审核促销规则
        /// </summary>
        /// <param name="sysNo">促销规则编号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        public Result AuditPromotionRule(int sysNo, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            var entity = GetEntity(sysNo);
            if (entity != null)
            {
                if (entity.Status == (int)Hyt.Model.WorkflowStatus.PromotionStatus.促销规则状态.待审)
                {
                    entity.LastUpdateBy = user.SysNo;
                    entity.LastUpdateDate = DateTime.Now;
                    entity.Status = (int)Hyt.Model.WorkflowStatus.PromotionStatus.促销规则状态.已审;
                    entity.AuditDate = DateTime.Now;
                    entity.AuditorSysNo = user.SysNo;
                    Hyt.DataAccess.Promotion.ISpPromotionRuleDao.Instance.Update(entity);
                    r.Status = true;
                }
                else
                {
                    r.Message = "当前规则状态非待审核状态";
                }
            }
            else
            {
                r.Message = "规则数据不存在";
            }
            return r;
        }

        /// <summary>
        /// 作废促销规则
        /// </summary>
        /// <param name="sysNo">促销规则编号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        public Result CancelPromotionRule(int sysNo, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            var entity = GetEntity(sysNo);
            if (entity != null)
            {

                entity.LastUpdateBy = user.SysNo;
                entity.LastUpdateDate = DateTime.Now;
                entity.Status = (int)Hyt.Model.WorkflowStatus.PromotionStatus.促销规则状态.作废;
                Hyt.DataAccess.Promotion.ISpPromotionRuleDao.Instance.Update(entity);
                r.Status = true;

            }
            else
            {
                r.Message = "规则数据不存在";
            }
            return r;
        }

        /// <summary>
        /// 保存促销规则
        /// </summary>
        /// <param name="model">促销规则</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        public Result SavePromotionRule(SpPromotionRule model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            if (model.SysNo > 0)
            {
                //编辑数据
                var entity = GetEntity(model.SysNo);
                if (entity != null)
                {
                    model.Status = entity.Status;
                    model.CreatedDate = entity.CreatedDate;
                    model.CreatedBy = entity.CreatedBy;
                    model.AuditDate = entity.AuditDate;
                    model.AuditorSysNo = entity.AuditorSysNo;
                    model.LastUpdateBy = user.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    Hyt.DataAccess.Promotion.ISpPromotionRuleDao.Instance.Update(model);
                    r.StatusCode = model.SysNo;
                    r.Status = true;
                }
                else
                {
                    r.Status = false;
                    r.Message = "规则数据不存在";
                }
            }
            else
            {
                //新增数据
                model.Status = (int)Hyt.Model.WorkflowStatus.PromotionStatus.促销规则状态.待审;
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                r.StatusCode = Hyt.DataAccess.Promotion.ISpPromotionRuleDao.Instance.Insert(model);
                r.Status = true;
            }
            return r;
        }
    }
}
