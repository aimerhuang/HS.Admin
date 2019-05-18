using System.Collections.Generic;
using Hyt.DataAccess.Union;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Union
{
    /// <summary>
    /// 广告日志
    /// </summary>
    /// <remarks>2013-10-14 吴文强 创建</remarks>
    public class UnAdvertisementLogBo : BOBase<UnAdvertisementLogBo>
    {
        /// <summary>
        /// 新增广告日志
        /// </summary>
        /// <param name="advertisementLog">广告日志</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public void Insert(UnAdvertisementLog advertisementLog)
        {
            IUnAdvertisementLogDao.Instance.Insert(advertisementLog);
        }

        /// <summary>
        /// 更新广告日志
        /// </summary>
        /// <param name="advertisementLog">广告日志</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public void Update(UnAdvertisementLog advertisementLog)
        {
            IUnAdvertisementLogDao.Instance.Update(advertisementLog);
        }

        /// <summary>
        /// 根据Cookie标识码获取广告日志集合
        /// </summary>
        /// <param name="cookieIdentity">Cookie标识码</param>
        /// <returns>Cookie标识码集合</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public List<UnAdvertisementLog> GetListByCookieIdentity(string cookieIdentity)
        {
            return IUnAdvertisementLogDao.Instance.GetListByCookieIdentity(cookieIdentity);
        }

        /// <summary>
        /// 查询联盟广告日志
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>查询数据</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public PagedList<UnAdvertisementLog> GetList(ParaUnAdvertisementLogFilter filter)
        {
            if (filter != null)
            {
                var model = new PagedList<UnAdvertisementLog>();
                filter.PageSize = model.PageSize;
                var pager = IUnAdvertisementLogDao.Instance.GetList(filter);
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
        /// 获取联盟广告日志实体
        /// </summary>
        /// <param name="sysNo">联盟广告日志系统编号</param>
        /// <returns>联盟广告日志</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public UnAdvertisementLog GetModel(int sysNo)
        {
            return IUnAdvertisementLogDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据订单号获取联盟广告日志
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>联盟广告日志</returns>
        /// <remarks>2014-02-25 吴文强 创建</remarks>
        public UnAdvertisementLog GetModelByOrder(int orderSysNo)
        {
            return IUnAdvertisementLogDao.Instance.GetModelByValidData(string.Format("orders:{0},", orderSysNo));
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>Excel数据</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public List<CBUnAdvertisementLog> ExportExcel(ParaUnAdvertisementLogFilter filter)
        {
            return IUnAdvertisementLogDao.Instance.GetExcelList(filter);
        }
    }
}
