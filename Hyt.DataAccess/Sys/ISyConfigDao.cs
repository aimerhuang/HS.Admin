using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 系统配置功能Dao
    /// </summary>
    /// <remarks>2014-01-20 周唐炬 创建</remarks>
    public abstract class ISyConfigDao : DaoBase<ISyConfigDao>
    {
        /// <summary>
        /// 获取系统配置功能实体
        /// </summary>
        /// <param name="sysNo">系统配置功能系统编号</param>
        /// <returns>系统配置</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public abstract SyConfig GetModel(int sysNo);

        /// <summary>
        /// 获取系统配置功能实体
        /// </summary>
        /// <param name="key">系统配置功能key</param>
        /// <param name="category">分类系统</param>
        /// <returns>系统配置</returns>
        /// <remarks>2014-02-11 周唐炬 创建</remarks>
        public abstract SyConfig GetModel(string key, SystemStatus.系统配置类型 category);

        /// <summary>
        /// 创建系统配置功能
        /// </summary>
        /// <param name="model">系统配置功能实体</param>
        /// <returns>系统配置功能实体系统编号</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public abstract int Create(SyConfig model);

        /// <summary>
        /// 更新系统配置功能
        /// </summary>
        /// <param name="model">系统配置功能实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public abstract int Update(SyConfig model);

        /// <summary>
        /// 删除系统配置项
        /// </summary>
        /// <param name="sysNo">系统配置系统编号</param>
        /// <returns>受影响行</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public abstract int Remove(int sysNo);

        /// <summary>
        /// 更根条件获取系统配置功能列表
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>系统配置功能列表</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public abstract Pager<SyConfig> GetList(ParaSyConfigFilter filter);

        /// <summary>
        /// 验证系统配置功能是否已经存在
        /// </summary>
        /// <param name="key">系统配置功能key</param>
        /// <param name="categoryId">分类</param>
        /// <param name="sysNo">系统配置功能系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2014-01-20 周唐炬 创建</remarks>
        public abstract int SyConfigVerify(string key, int categoryId, int? sysNo);
    }
}
