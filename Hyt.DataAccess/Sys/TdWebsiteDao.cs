using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 网站购物状态管理
    /// </summary>
    /// <remarks>2016-07-01 周 创建</remarks>
    public abstract class TdWebsiteDao : DaoBase<TdWebsiteDao>
    {
         /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public abstract TdWebsiteManagement SelectWebsiteManagement(int sysNo);
         /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public abstract bool UpdateWebsiteManagement(TdWebsiteManagement model);
        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public abstract TdWebsiteState SelectWebsiteState(int sysNo);
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2016-07-01 周海鹏 创建</remarks>
        public abstract int InsertWebsiteState(TdWebsiteState model);
         /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public abstract bool UpdateWebsiteState(TdWebsiteState model);
        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public abstract IList<TdWebsiteState> SelectAllWebsiteState();
         /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">状态</param>
        /// <returns>分页</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public abstract Pager<TdWebsiteState> SelectAllWebsiteState(int currentPage, int pageSize, int? status, int? ProvinceSysno, int? CitySysno, int? AreaSysNo, int? WarehouseSysNo, string WarehouseName);
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public abstract bool DeleteWebsiteState(int sysNo);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">状态</param>
        /// <returns>分页</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public abstract Pager<TdWebsiteManagement> SelectWebsiteManagement(int currentPage, int pageSize, int? status, int? ProvinceSysno, int? CitySysno, int? AreaSysNo, int? WarehouseSysNo, string WarehouseName);
         /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public abstract int InsertWebsiteManagement(TdWebsiteManagement model);
         /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <returns></returns>
        public abstract int ExitWebsiteManagement(int WarehouseSysNo);

    }
}
