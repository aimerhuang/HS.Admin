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
    /// 惠源币日志业务层
    /// </summary>
    /// <remarks>2013－08-21 苟治国 创建</remarks>
    public class CrExperienceCoinLogBo : BOBase<CrExperienceCoinLogBo>
    {
        /// <summary>
        /// 根据条件获取惠源币的列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="customersysno">客户编号</param>
        /// <param name="type">类型</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>惠源币列表</returns>
        /// <remarks>2013-08-21 苟治国 创建</remarks>
        public PagedList<Model.CrExperienceCoinLog> SeachPager(int pageIndex, int customersysno, int type, DateTime beginTime, DateTime endTime)
        {
            var list = new PagedList<CrExperienceCoinLog>();
            var pager = new Pager<CrExperienceCoinLog>();

            var exp = new ParaCrExperienceCoinLogFilter();
            exp.CustomerSysNo = customersysno;
            exp.BeginDate = beginTime;
            exp.EndDate = endTime;
            exp.Type = type;

            pager.CurrentPage = pageIndex;
            pager.PageSize = list.PageSize;
            pager = ICrExperienceCoinLogDao.Instance.SeachPager(pager, exp);
            list = new PagedList<CrExperienceCoinLog>
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
