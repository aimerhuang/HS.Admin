using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Basic;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Basic
{
    /// <summary>
    /// 库存
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class OriginBo : BOBase<OriginBo>
    {

        #region 国家
        /// <summary>
        /// 分页获取国家
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<Origin> GetOriginList(ParaOriginFilter filter)
        {
            return IOriginDao.Instance.GetOriginList(filter);
        }
        /// <summary>
        /// 分页获取国家
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public List<Origin> GetOrigin()
        {
            return IOriginDao.Instance.GetOrigin();
        }
        /// <summary>
        /// 保存国家
        /// </summary>
        /// <param name="model">国家</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SaveOrigin(Origin model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            Origin entity = IOriginDao.Instance.GetEntity(model.SysNo);
            if(entity != null)
            {
                model.SysNo = entity.SysNo;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IOriginDao.Instance.Update(model);
                r.Status = true;
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IOriginDao.Instance.Insert(model);
                r.Status = true;
            }
            return r;
        }
        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public Origin GetEntity(int SysNo)
        {
            return IOriginDao.Instance.GetEntity(SysNo);
        }
        /// <summary>
        /// 删除国家
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = IOriginDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }
        /// <summary>
        /// 获取指定名称的国家信息
        /// </summary>
        /// <param name="name">国家名称</param>
        /// <returns>国家实体信息</returns>
        /// <remarks>2015-12-5 王耀发 创建</remarks>
        public Origin GetEntityByName(string Origin_Name)
        {
            return IOriginDao.Instance.GetEntityByName(Origin_Name);
        }
        #endregion
    }
}
