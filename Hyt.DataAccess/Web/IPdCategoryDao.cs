using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 商品分类（前台）
    /// </summary>
    /// <remarks>2013-08-06 黄波 创建</remarks>
    public abstract class IPdCategoryDao : DaoBase<IPdCategoryDao>
    {
        /// <summary>
        /// 获取所有可用分类信息
        /// </summary>
        /// <returns>分类信息</returns>
        /// <remarks>2013-08-06 黄波 创建</remarks>
        public abstract IList<CBPdCategory> GetAllCategory();
    }
}
