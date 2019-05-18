using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Feedback
{
    /// <summary>
    /// 意见反馈类别
    /// </summary>
    /// <remarks>
    /// 2013-08-19 周唐炬 创建
    /// </remarks>
    public abstract class ICrFeedbackTypeDao : DaoBase<ICrFeedbackTypeDao>
    {
        /// <summary>
        /// 通过来源获取意见反馈类别
        /// </summary>
        /// <param name="source">来源</param>
        /// <returns>意见反馈类别</returns>
        /// <remarks>2013-08-19 周唐炬 创建</remarks>
        public abstract IList<CrFeedbackType> GetFeedbackTypeList(CustomerStatus.意见反馈类型来源 source);

        /// <summary>
        /// 获取全部意见反馈类别列表
        /// </summary>
        /// <returns>意见反馈类别</returns>
        /// <remarks>2013-09-03 沈强 创建</remarks>
        public abstract IList<CrFeedbackType> GetFeedbackTypeList();
    }
}
