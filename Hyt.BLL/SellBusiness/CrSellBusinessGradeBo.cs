using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.SellBusiness
{
    /// <summary>
    /// 分销商
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class CrSellBusinessGradeBo : BOBase<CrSellBusinessGradeBo>
    {

        #region 分销商
        /// <summary>
        /// 分页获取分销商等级
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<CrSellBusinessGrade> GetCrSellBusinessGradeList(ParaSellBusinessGradeFilter filter)
        {
            return ICrSellBusinessGradeDao.Instance.GetCrSellBusinessGradeList(filter);
        }

        /// <summary>
        /// 保存分销商等级
        /// </summary>
        /// <param name="model">分销商等级</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SaveCrSellBusinessGrade(CrSellBusinessGrade model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            CrSellBusinessGrade entity = ICrSellBusinessGradeDao.Instance.GetEntity(model.SysNo);
            if(entity != null)
            {
                model.SysNo = entity.SysNo;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                ICrSellBusinessGradeDao.Instance.Update(model);
                r.Status = true;
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                ICrSellBusinessGradeDao.Instance.Insert(model);
                r.Status = true;
            }
            return r;
        }
        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public CrSellBusinessGrade GetEntity(int SysNo)
        {
            return ICrSellBusinessGradeDao.Instance.GetEntity(SysNo);
        }
        /// <summary>
        /// 删除分销商等级
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = ICrSellBusinessGradeDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }
        #endregion
    }
}
