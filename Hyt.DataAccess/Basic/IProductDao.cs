using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using System.Data;

namespace Hyt.DataAccess.Basic
{    
    /// <summary>
    /// 商品
    /// </summary>
    /// <remarks>2013-06-18 黄志勇 创建</remarks>
    public abstract class IProduct : DaoBase<IProduct>
    {
        /// <summary>
        /// 根据商品父分类ID获取子分类列表
        /// </summary>
        /// <param name="parentSysNo">父级分类编号</param>
        /// <returns>商品类别列表</returns>
        /// <remarks>2013-06-18 黄志勇 创建</remarks>
        public abstract DataTable SearchProductCategory(int parentSysNo);        
    }
}
