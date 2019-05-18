using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Sys;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 网站购物状态管理
    /// </summary>
    /// <remarks>2016-07-01 周 创建</remarks>
    public class TdWebsiteBo : BOBase<TdWebsiteBo>
    {
        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public TdWebsiteManagement SelectWebsiteManagement(int sysNo)
        {
            return TdWebsiteDao.Instance.SelectWebsiteManagement(sysNo);
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public bool UpdateWebsiteManagement(TdWebsiteManagement model)
        {
            return TdWebsiteDao.Instance.UpdateWebsiteManagement(model);
        }
        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public TdWebsiteState SelectWebsiteState(int sysNo)
        {
            return TdWebsiteDao.Instance.SelectWebsiteState(sysNo);
        }
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2016-07-01 周海鹏 创建</remarks>
        public int InsertWebsiteState(TdWebsiteState model)
        {
            return TdWebsiteDao.Instance.InsertWebsiteState(model);
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public bool UpdateWebsiteState(TdWebsiteState model)
        {
            return TdWebsiteDao.Instance.UpdateWebsiteState(model);
        }
        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public IList<TdWebsiteState> SelectAllWebsiteState()
        {
            return TdWebsiteDao.Instance.SelectAllWebsiteState();
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">状态</param>
        /// <returns>分页</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public Pager<TdWebsiteState> SelectAllWebsiteState(int currentPage, int pageSize, int? status, int? ProvinceSysno, int? CitySysno, int? AreaSysNo, int? WarehouseSysNo, string WarehouseName)
        {
            return TdWebsiteDao.Instance.SelectAllWebsiteState(currentPage, pageSize, status, ProvinceSysno, CitySysno, AreaSysNo, WarehouseSysNo, WarehouseName);
        }
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public bool DeleteWebsiteState(int sysNo)
        {
            return TdWebsiteDao.Instance.DeleteWebsiteState(sysNo);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">状态</param>
        /// <returns>分页</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public Pager<TdWebsiteManagement> SelectWebsiteManagement(int currentPage, int pageSize, int? status, int? ProvinceSysno, int? CitySysno, int? AreaSysNo, int? WarehouseSysNo, string WarehouseName)
        {
            return TdWebsiteDao.Instance.SelectWebsiteManagement(currentPage, pageSize, status, ProvinceSysno, CitySysno, AreaSysNo, WarehouseSysNo, WarehouseName);
        }
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public int InsertWebsiteManagement(TdWebsiteManagement model)
        {
            return TdWebsiteDao.Instance.InsertWebsiteManagement(model);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <returns></returns>
        public int ExitWebsiteManagement(int WarehouseSysNo)
        {
            return TdWebsiteDao.Instance.ExitWebsiteManagement(WarehouseSysNo);
        }

    }
}
