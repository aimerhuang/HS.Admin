using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Union
{
    /// <summary>
    /// 联盟网站
    /// </summary>
    /// <remarks>2013-10-14 吴文强 创建</remarks>
    public abstract class IUnUnionSiteDao : DaoBase<IUnUnionSiteDao>
    {
        /// <summary>
        /// 根据系统编号获取联盟网站
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>联盟网站</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public abstract UnUnionSite GetModel(int sysNo);

        /// <summary>
        /// 创建联盟网站
        /// </summary>
        /// <param name="model">联盟网站实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract int Create(UnUnionSite model);

        /// <summary>
        /// 修改联盟网站
        /// </summary>
        /// <param name="model">联盟网站实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract int Update(UnUnionSite model);

        /// <summary>
        /// 获取所有联盟网站
        /// </summary>
        /// <param></param>
        /// <returns>所有联盟网站</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract List<UnUnionSite> GetAll();

        /// <summary>
        /// 通过条件获取联盟网站列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>联盟网站列表</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract Pager<UnUnionSite> GetUnionSiteList(ParaUnUnionSiteFilter filter);

        /// <summary>
        /// 联盟网站名称验证
        /// </summary>
        /// <param name="siteName">联盟网站名称</param>
        /// <param name="sysNo">联盟网站系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public abstract int UnionSiteVerify(string siteName,int? sysNo);
    }
}
