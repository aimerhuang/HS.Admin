using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 系统配置功能Bo
    /// </summary>
    /// <remarks>2014-01-20 周唐炬 创建</remarks>
    public class SyConfigBo : BOBase<SyConfigBo>
    {
        /// <summary>
        /// 获取系统配置功能实体
        /// </summary>
        /// <param name="sysNo">系统配置功能系统编号</param>
        /// <returns>系统配置</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public SyConfig GetModel(int sysNo)
        {
            return ISyConfigDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 获取系统配置功能实体
        /// </summary>
        /// <param name="key">系统配置功能key</param>
        /// <param name="category">分类系统</param>
        /// <returns>系统配置</returns>
        /// <remarks>2014-02-11 周唐炬 创建</remarks>
        public SyConfig GetModel(string key, SystemStatus.系统配置类型 category)
        {
            return ISyConfigDao.Instance.GetModel(key, category);
        }

        /// <summary>
        /// 创建系统配置功能
        /// </summary>
        /// <param name="model">系统配置功能实体</param>
        /// <returns>系统配置功能实体系统编号</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public int Create(SyConfig model)
        {
            return ISyConfigDao.Instance.Create(model);
        }

        /// <summary>
        /// 更新系统配置功能
        /// </summary>
        /// <param name="model">系统配置功能实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public int Update(SyConfig model)
        {
            return ISyConfigDao.Instance.Update(model);
        }

        /// <summary>
        /// 删除系统配置项
        /// </summary>
        /// <param name="sysNo">系统配置系统编号</param>
        /// <returns>受影响行</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public int Remove(int sysNo)
        {
            return ISyConfigDao.Instance.Remove(sysNo);
        }

        /// <summary>
        /// 更根条件获取系统配置功能列表
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>系统配置功能列表</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public PagedList<SyConfig> GetList(ParaSyConfigFilter filter)
        {
            if (filter != null)
            {
                var model = new PagedList<SyConfig>();
                filter.PageSize = model.PageSize;
                var pager = ISyConfigDao.Instance.GetList(filter);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.CurrentPage;
                }
                return model;
            }
            return null;
        }

        /// <summary>
        /// 验证系统配置功能是否已经存在
        /// </summary>
        /// <param name="key">系统配置功能key</param>
        /// <param name="categoryId">分类</param>
        /// <param name="sysNo">系统配置功能系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public bool SyConfigVerify(string key, int categoryId, int? sysNo)
        {
            var count = ISyConfigDao.Instance.SyConfigVerify(key, categoryId, sysNo);
            return count <= 0;
        }
    }
}
