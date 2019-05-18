using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 前台广告
    /// </summary>
    /// <remarks>2013-08-06 黄波 创建</remarks>
    public abstract class IFeAdvertItemDao : DaoBase<IFeAdvertItemDao>
    {
        /// <summary>
        /// 根据广告组代码获取广告内容
        /// </summary>
        /// <param name="platformType">广告平台类型</param>
        /// <param name="groupCode">组代码</param>
        /// <returns>广告项</returns>
        /// <remarks>2013-08-06 黄波 创建</remarks>
        public abstract IList<FeAdvertItem> GetAdvertItems(ForeStatus.广告组平台类型 platformType, string groupCode);
        /// <summary>
        /// 获取广告图片数据对象
        /// </summary>
        /// <param name="groupSysNo"></param>
        /// <returns></returns>
        public abstract IList<FeAdvertItem> GetWebAdvertItemsByGroupSysNo(int groupSysNo);
    }
}
