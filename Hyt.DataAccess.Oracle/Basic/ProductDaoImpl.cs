using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Basic;
using Hyt.Model;
using System.Data;

namespace Hyt.DataAccess.Oracle.Basic
{
    /// <summary>
    /// 商品管理
    /// </summary>
    /// <remarks>2013-06-18 黄志勇 创建</remarks>
    public class ProductDaoImpl : IProduct
    {
        /// <summary>
        /// 根据商品父分类ID获取子分类列表
        /// </summary>
        /// <param name="parentSysNo">父级分类编号</param>
        /// <returns>商品类别列表</returns>
        /// <remarks>2013-06-18 黄志勇 创建</remarks>
        public override DataTable SearchProductCategory(int parentSysNo)
        {
            #region sql
            var command = @"Select b.SysNo,b.CategoryName,count(a.SysNo) Total from PdProductCategory  a
                            Right Join 
                            (
                            Select sysno,CategoryName from PdProductCategory where ParentSysNo = @0
                            ) b
                            on a.PARENTSYSNO = b.SYSNO
                            GROUP BY b.SYSNO,b.CategoryName
                            ORDER BY SYSNO";
            #endregion

            return Context.Sql(command, parentSysNo).QuerySingle<DataTable>();
        }
    }
}
