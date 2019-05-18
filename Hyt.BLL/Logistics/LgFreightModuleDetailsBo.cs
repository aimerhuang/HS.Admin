using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Logistics;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 运费模板
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class LgFreightModuleDetailsBo : BOBase<LgFreightModuleDetailsBo>
    {

        #region 运费模板
        ///// <summary>
        ///// 分页获取运费模板
        ///// </summary>
        ///// <param name="filter">筛选条件</param>
        ///// <returns>分页列表</returns>
        ///// <remarks>2015-08-06 王耀发 创建</remarks>
        //public Pager<LgFreightModule> GetLgFreightModuleList(ParaFreightModule filter)
        //{
        //    return ILgFreightModuleDao.Instance.GetLgFreightModuleList(filter);
        //}

        ///// <summary>
        ///// 获取运费模板信息
        ///// </summary>
        ///// <param name="sysNo">费模板编号</param>
        ///// <returns></returns>
        ///// <remarks>2015-08-06 王耀发 创建</remarks>
        //public LgFreightModule GetEntity(int sysNo)
        //{
        //    return Hyt.DataAccess.Logistics.ILgFreightModuleDao.Instance.GetEntity(sysNo);
        //}

        /// <summary>
        /// 保存运费模板
        /// </summary>
        /// <param name="model">运费模板</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SaveFreightModuleDetails(LgFreightModuleDetails model, SyUser user)
        {
            Result result = new Result()
            {
                Status = false
            };
            if (model.SysNo > 0)
            {
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                UpdateFreightModuleDetails(model);
                result.StatusCode = model.SysNo;
            }
            else
            {
                //新增数据
                model.Status = (int)Hyt.Model.WorkflowStatus.PromotionStatus.促销规则状态.待审;
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                result.StatusCode = Hyt.DataAccess.Logistics.IFreightModuleDetailsDao.Instance.Insert(model);
            }
            result.Status = true;
            result.Message = "保存成功";
            return result;
        }

        /// <summary>
        /// 更新运费模板详情
        /// </summary>
        /// <param name="entity">运费模板详情实体</param>
        /// <returns></returns>
        /// <remarks>2015-11-22 杨浩 创建</remarks>
        public int UpdateFreightModuleDetails(LgFreightModuleDetails entity)
        {
            return Hyt.DataAccess.Logistics.IFreightModuleDetailsDao.Instance.UpdateFreightModuleDetails(entity);
        }
        /// <summary>
        /// 获取促销规则列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public List<LgFreightModuleDetails> GetFreightModuleDetailsBy(int FreightModuleSysNo, int IsPost, int ValuationStyle, int DeliveryStyle)
        {
            return Hyt.DataAccess.Logistics.IFreightModuleDetailsDao.Instance.GetFreightModuleDetailsBy(FreightModuleSysNo, IsPost, ValuationStyle, DeliveryStyle);
        }
        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public void Delete(int sysNo)
        {
            Hyt.DataAccess.Logistics.IFreightModuleDetailsDao.Instance.Delete(sysNo);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-22 杨浩 创建</remarks>
        public void DeleteBySysNos(string sysNos)
        {
            Hyt.DataAccess.Logistics.IFreightModuleDetailsDao.Instance.DeleteBySysNos(sysNos);
        }
        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="FreightModuleSysNo">运费模板编号</param>
        public void DeleteByFreightModuleSysNo(int FreightModuleSysNo)
        {
            Hyt.DataAccess.Logistics.IFreightModuleDetailsDao.Instance.DeleteByFreightModuleSysNo(FreightModuleSysNo);
        }
        #endregion
    }
}
