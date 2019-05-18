using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Warehouse;
using Hyt.Model.WorkflowStatus;
using System.IO;
using System.Data;
using Hyt.Util;
using Hyt.BLL.Product;
using Hyt.BLL.Log;
using Hyt.DataAccess.Promotion;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 仓库免运费
    /// </summary>
    /// <remarks>2016-04-20 王耀发 创建</remarks>
    public class WhouseFreightFreeBo : BOBase<WhouseFreightFreeBo>
    {
        /// <summary>
        /// 分页获取仓库免邮费
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2016-04-20 王耀发 创建</remarks>
        public Pager<CBWhouseFreightFree> GetWhouseFreightFreeList(ParaWhouseFreightFreeFilter filter)
        {
            return IWhouseFreightFreeDao.Instance.GetWhouseFreightFreeList(filter);
        }

        /// <summary>
        /// 保存仓库免邮费
        /// </summary>
        /// <param name="model">仓库免邮费</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2016-04-20 王耀发 创建</remarks>
        public Result SaveWhouseFreightFree(WhouseFreightFree model, SyUser user)
        {
            Result r = new Result();
            try
            {
                if (model.SysNo > 0)
                {
                    WhouseFreightFree Entity = IWhouseFreightFreeDao.Instance.GetEntity(model.SysNo);
                    model.CreatedBy = Entity.CreatedBy;
                    model.CreatedDate = Entity.CreatedDate;
                    model.LastUpdateBy = user.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    IWhouseFreightFreeDao.Instance.Update(model);
                    r.Status = true;
                    r.Message = "操作成功";
                }
                else
                {
                    model.CreatedBy = user.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = user.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    IWhouseFreightFreeDao.Instance.Insert(model);
                    r.Status = true;
                    r.Message = "操作成功";
                }
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return r;
        }
    }
}
