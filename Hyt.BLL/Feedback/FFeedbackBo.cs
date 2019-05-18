using Hyt.DataAccess.Feedback;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Feedback
{
    public class FFeedbackBo : BOBase<FFeedbackBo>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="State">状态</param>
        /// <param name="pager">分页对象</param>
        /// <returns>2016-03-30 周海鹏 创建</returns>
        public IList<FFeedBack> GetFeedBackList(int State, PagedList<FFeedBack> pager)
        {

            Pager<FFeedBack> transPager = new Pager<FFeedBack>();
            transPager.CurrentPage = pager.CurrentPageIndex;
            transPager.PageSize = pager.PageSize;
            FeedBackDao.Instance.Seach(State, transPager);
            pager.TData = transPager.Rows;
            pager.TotalItemCount = transPager.TotalRows;
            return pager.TData;
        }
         /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNoItems">ID组</param>
        /// <returns></returns>
        public int Update(string sysNoItems)
        {
            return FeedBackDao.Instance.Update(sysNoItems);
        }
    }
}
