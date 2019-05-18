using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Infrastructure.Pager;
using Hyt.Model.Parameter;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 会员可用积分日志业务逻辑层
    /// </summary>
    /// <remarks>2013－08-05 苟治国 创建</remarks>
    public class CrAvailablePointLogBo : BOBase<CrAvailablePointLogBo>
    {
        /// <summary>
        /// 会员可用积分日志
        /// </summary>
        /// <param name="pageIndex">索引</param>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>可用积分日志列表</returns>
        /// <remarks>2013-12-01 苟治国 创建</remarks>
        public PagedList<Model.CrAvailablePointLog> GetPager(int pageIndex, int customerSysNo, DateTime beginTime, DateTime endTime)
        {
            var list = new PagedList<CrAvailablePointLog>();
            var pager = new Pager<CrAvailablePointLog>();

            var apl = new ParaCrAvailablePointLogFilter();
            apl.CustomerSysNo = customerSysNo;
            apl.BeginDate = beginTime;
            apl.EndDate = endTime;

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new CrAvailablePointLog
            {
                CustomerSysNo = customerSysNo
            };
            pager.PageSize = list.PageSize;
            pager = ICrAvailablePointLogDao.Instance.GetPager(pager, apl);
            list = new PagedList<CrAvailablePointLog>
            {
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                IsLoading = false,
                Style = PagedList.StyleEnum.WebSmall
            };
            return list;
        }

        public List<CrAvailablePointLog> GetAll()
        {
            return ICrAvailablePointLogDao.Instance.GetAll();
        }

        public void DeleteData(int p)
        {
            ICrAvailablePointLogDao.Instance.DeleteData(p);
        }
    }
}
