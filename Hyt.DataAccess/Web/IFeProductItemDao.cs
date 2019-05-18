using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 商品组
    /// </summary>
    /// <remarks>2013-08-06 黄波 创建</remarks>
    public abstract class IFeProductItemDao : DaoBase<IFeProductItemDao>
    {
        /// <summary>
        /// 根据商品组代码获取商品
        /// 返回部分列数据
        /// </summary>
        /// <param name="platformType">平台类型</param>
        /// <param name="groupCode">组代码</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-06 黄波 创建</remarks>
        public abstract IList<Model.FeProductItem> GetFeProductItems(ForeStatus.商品组平台类型 platformType, string groupCode);

        /// <summary>
        /// 根据商品组系统编号获取商品
        /// 返回部分列数据
        /// </summary>
        /// <param name="groupSysNo">组系统编号</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-21 周瑜 创建</remarks>
        public abstract IList<CBFeProductItem> GetFeProductItems(int groupSysNo);
    }
}
