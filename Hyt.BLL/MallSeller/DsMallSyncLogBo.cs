using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Generated;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Pager;

namespace Hyt.BLL.MallSeller
{
    public class DsMallSyncLogBo : BOBase<DsMallSyncLogBo>
    {
        /// <summary>
        /// 创建升舩商城同步日志表
        /// </summary>
        /// <param name="model">升舩商城同步日志表</param>
        /// <returns>创建的升舩商城同步日志表sysNo</returns>
        /// <remarks> 
        /// 2014-07-28 杨文兵 创建
        /// </remarks>
        public int Create(DsMallSyncLog model) {
            return Hyt.DataAccess.MallSeller.IDsMallSyncLogDao.Instance.Create(model);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>日志分页</returns>
        /// <remarks>2017-11-1 杨浩 创建</remarks>
        public PagedList<CBDsMallSyncLog> GetList(ParaDsMallSyncLogFilter filter)
        {
            if (filter != null)
            {
                var model = new PagedList<CBDsMallSyncLog>();
                filter.PageSize = model.PageSize;
                var pager=Hyt.DataAccess.MallSeller.IDsMallSyncLogDao.Instance.GetList(filter);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.Id;
                }
                return model;
            }
            return null;                   
        }
    }
}
