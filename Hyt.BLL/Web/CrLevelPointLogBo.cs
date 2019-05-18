using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Model.Parameter;
using Hyt.Infrastructure.Pager;

namespace Hyt.BLL.Web
{
    /// <summary>
    ///等级积分日志业务层
    /// </summary>
    /// <remarks>2013－08-22 苟治国 创建</remarks>
    public class CrLevelPointLogBo : BOBase<CrLevelPointLogBo>
    {
        /// <summary>
        /// 根据条件获取等级积分日志列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="customersysno">客户编号</param>
        /// <param name="type">类型</param>
        /// <returns>等级积分日志列表</returns>
        /// <remarks>2013-08-22 苟治国 创建</remarks>
        public PagedList<Model.CrLevelPointLog> SeachPager(int pageIndex, int customersysno, int type)
        {
            var list = new PagedList<CrLevelPointLog>();
            var pager = new Pager<CrLevelPointLog>();

            pager.PageFilter = new CrLevelPointLog
            {
                CustomerSysNo = customersysno
            };

            pager.CurrentPage = pageIndex;
            pager.PageSize = list.PageSize;
            pager = ICrLevelPointLogDao.Instance.SeachPager(pager,type);
            list = new PagedList<CrLevelPointLog>
            {
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                IsLoading = false,
                Style = PagedList.StyleEnum.WebSmall
            };
            return list;
        }
    }
}
