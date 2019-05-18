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
    /// 广告日志
    /// </summary>
    /// <remarks>2013-10-14 吴文强 创建</remarks>
    public abstract class IUnAdvertisementLogDao : DaoBase<IUnAdvertisementLogDao>
    {
        /// <summary>
        /// 新增广告日志
        /// </summary>
        /// <param name="advertisementLog">广告日志</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public abstract void Insert(UnAdvertisementLog advertisementLog);

        /// <summary>
        /// 更新广告日志
        /// </summary>
        /// <param name="advertisementLog">广告日志</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public abstract void Update(UnAdvertisementLog advertisementLog);

        /// <summary>
        /// 根据Cookie标识码获取广告日志集合
        /// </summary>
        /// <param name="cookieIdentity">Cookie标识码</param>
        /// <returns>Cookie标识码集合</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public abstract List<UnAdvertisementLog> GetListByCookieIdentity(string cookieIdentity);

        /// <summary>
        /// 查询联盟广告日志
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>查询数据</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract Pager<UnAdvertisementLog> GetList(ParaUnAdvertisementLogFilter filter);

        /// <summary>
        /// 获取联盟广告日志实体
        /// </summary>
        /// <param name="sysNo">联盟广告日志系统编号</param>
        /// <returns>联盟广告日志</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract UnAdvertisementLog GetModel(int sysNo);

        /// <summary>
        /// 根据订单号获取联盟广告日志
        /// </summary>
        /// <param name="validData">有效数据</param>
        /// <returns>联盟广告日志</returns>
        /// <remarks>2014-02-25 吴文强 创建</remarks>
        public abstract UnAdvertisementLog GetModelByValidData(string validData);

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>Excel数据</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract List<Model.Transfer.CBUnAdvertisementLog> GetExcelList(ParaUnAdvertisementLogFilter filter);
    }
}
