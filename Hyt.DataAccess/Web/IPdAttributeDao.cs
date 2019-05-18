using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 商品属性
    /// </summary>
    /// <remarks>2013-08-22 黄波 创建</remarks>
    public abstract class IPdAttributeDao : DaoBase<IPdAttributeDao>
    {
        /// <summary>
        /// 根据分类编号获取分类下所有可作为搜索选项的属性以及属性选项列表
        /// </summary>
        /// <param name="categoryList">分类列表</param>
        /// <return>属性及属性选项列表</return>
        /// <remarks>2013-08-22 黄波 创建</remarks>
        public abstract IList<Hyt.Model.SearchAttributeAndOptions> GetSearchAttributeAndOptions(int categoryList);
    }
}
