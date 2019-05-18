using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Web;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 前台商品分类
    /// </summary>
    /// <remarks>2013-08-22 黄波 创建</remarks>
    public class PdCategoryDaoImpl : IPdCategoryDao
    {
        /// <summary>
        /// 获取所有可用分类信息
        /// 部分数据
        /// </summary>
        /// <returns>分类信息</returns>
        /// <remarks>2013-08-06 黄波 创建</remarks>
        /// <remarks>2013-10-08 邵斌 修改：数据库停用online字段</remarks>
        public override IList<CBPdCategory> GetAllCategory()
        {
            //读取商品分类
            string sql = @"select 
                                        t.SysNo,t.Parentsysno,t.categoryname,t.seotitle,t.seokeyword,t.seodescription,t.displayorder,t.categoryimage 
                                    from 
                                        PdCategory t
                                    where
                                         t.Status = @Status and t.isonline=@IsOnLine
                                    order by
                                         displayorder asc    
                                  ";
            return Context.Sql(sql)
                .Parameter("Status", ProductStatus.商品分类状态.有效)
                .Parameter("IsOnLine", ProductStatus.是否前端展示.是)
                .QueryMany<CBPdCategory>();
        }
    }
}
