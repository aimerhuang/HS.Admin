using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Feedback;
using Hyt.Model;

namespace Hyt.BLL.Feedback
{
    /// <summary>
    /// 意见反馈
    /// </summary>
    /// <remarks>
    /// 2013-08-19 周唐炬 创建
    /// </remarks>
    public class FeedbackBo : BOBase<FeedbackBo>
    {
        /// <summary>
        /// 创建意见反馈
        /// </summary>
        /// <param name="model">意见反馈实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-08-19 周唐炬 创建</remarks>
        public int Create(CrFeedback model)
        {
            return ICrFeedbackDao.Instance.Create(model);
        }

        /// <summary>
        /// 更新意见反馈
        /// </summary>
        /// <param name="model">意见反馈实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-08-19 周唐炬 创建</remarks>
        public int Update(CrFeedback model)
        {
            return ICrFeedbackDao.Instance.Update(model);
        }

        /// <summary>
        /// 获取意见反馈集合
        /// </summary>
        /// <param name="pager">分页条件实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-03 沈强 创建</remarks>
        public void GetFeedbacks(ref Pager<CBCrFeedback> pager)
        {
            ICrFeedbackDao.Instance.GetFeedbacks(ref pager);
        }

    }
}
