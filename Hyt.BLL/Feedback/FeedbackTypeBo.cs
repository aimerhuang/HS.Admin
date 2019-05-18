using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Feedback;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Feedback
{
    /// <summary>
    /// 意见反馈类别
    /// </summary>
    /// <remarks>
    /// 2013-08-19 周唐炬 创建
    /// </remarks>
    public class FeedbackTypeBo : BOBase<FeedbackTypeBo>
    {
        /// <summary>
        /// 通过来源获取意见反馈类别
        /// </summary>
        /// <param name="source">来源</param>
        /// <returns>意见反馈类别</returns>
        /// <remarks>2013-08-19 周唐炬 创建</remarks>
        public IList<CrFeedbackType> GetFeedbackTypeList(CustomerStatus.意见反馈类型来源 source)
        {
            return ICrFeedbackTypeDao.Instance.GetFeedbackTypeList(source);
        }

        /// <summary>
        /// 获取全部意见反馈类别名称列表
        /// </summary>
        /// <returns>意见反馈类别名称集合</returns>
        /// <remarks>2013-09-03 沈强 创建</remarks>
        public IList<string> GetFeedbackTypeNameList()
        {
            var feedbackTypeNames = ICrFeedbackTypeDao.Instance.GetFeedbackTypeList().Select(f => f.Name).ToList();
            var tmp = new List<string>();
            foreach (var item in feedbackTypeNames)
            {
                if (!tmp.Contains(item))
                {
                    tmp.Add(item);
                }
            }
            return tmp;
        }
    }
}
