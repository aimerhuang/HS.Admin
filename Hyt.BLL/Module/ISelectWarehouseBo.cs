using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;

namespace Hyt.BLL.Module
{
    /// <summary>
    /// 选择仓库
    /// </summary>
    /// <remarks>2013-06-19 吴文强 创建</remarks>
    public interface ISelectWarehouseBo
    {
        /// <summary>
        /// 根据筛选条件获取地区仓库集合
        /// </summary>
        /// <param name="filter">筛选条件(地区/仓库/拼音等 模糊查询)</param>
        /// <returns>仓库集合</returns>
        /// <remarks>2013-06-19 吴文强 创建</remarks>
        IList<WhWarehouse> GetWarehouse(string filter);
    }
}