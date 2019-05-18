using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 搜索关键字
    /// </summary>
    /// <remarks>2013-08-07 黄波 创建</remarks>
    public abstract class IFeSearchKeywordDao : DaoBase<IFeSearchKeywordDao>
    {
        /// <summary>
        /// 获取搜索热词
        /// </summary>
        /// <returns>热词列表</returns>
        /// <remarks>2013-08-07 黄波 创建</remarks>
        public abstract IList<FeSearchKeyword> GetSearchKeys();
    }
}
