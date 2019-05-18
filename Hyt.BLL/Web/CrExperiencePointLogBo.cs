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
    /// 经验积分日志业务层
    /// </summary>
    /// <remarks>2013－11-1 苟治国 创建</remarks>
    public class CrExperiencePointLogBo : BOBase<CrExperiencePointLogBo>
    {
        /// <summary>
        /// 根据条件获取经验积分的列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="customersysno">客户编号</param>
        /// <param name="type">类型</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>经验积分列表</returns>
        /// <remarks>2013-11-1 苟治国 创建</remarks>
        public PagedList<Model.CrExperiencePointLog> SeachPager(int pageIndex, int customersysno, int type, DateTime beginTime, DateTime endTime)
        {
            var list = new PagedList<CrExperiencePointLog>();
            var pager = new Pager<CrExperiencePointLog>();

            var exp = new ParaCrExperiencePointLogFilter();
            exp.CustomerSysNo = customersysno;
            exp.BeginDate = beginTime;
            exp.EndDate = endTime;
            exp.Type = type;

            pager.CurrentPage = pageIndex;
            pager.PageSize = list.PageSize;
            pager = ICrExperiencePointLogDao.Instance.SeachPager(pager, exp);
            list = new PagedList<CrExperiencePointLog>
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
