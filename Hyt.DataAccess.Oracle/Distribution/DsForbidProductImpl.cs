using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 禁止升舱管理接口层
    /// </summary>
    /// <remarks>
    /// 2014-03-21 余勇 创建
    /// </remarks>
    public class DsForbidProductImpl : IDsForbidProductDao
    {
        #region 操作

        /// <summary>
        /// 创建禁止升舱商品
        /// </summary>
        /// <param name="model">禁止升舱商品</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2014-03-21 余勇 创建
        /// </remarks>
        public override int Create(DsForbidProduct model)
        {
            return Context.Insert("DsForbidProduct", model)
                         .AutoMap(x => x.SysNo)
                         .ExecuteReturnLastId<int>("SysNo");
        }

       /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>
        /// 2014-03-21 余勇 创建
        /// </remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("DsForbidProduct").Where("SysNo", sysNo).Execute();
        }

         /// <summary>
         /// 通过商品编号取得实体
         /// </summary>
        /// <param name="productSysNo">商品编号</param>
         /// <returns>禁止升舱商品</returns>
         /// <remarks>
         /// 2014-03-21 余勇 创建
         /// </remarks>
        public override DsForbidProduct GetByProductSysNo(int productSysNo)
         {
             return Context.Sql("select * from DsForbidProduct where ProductSysNo=@ProductSysNo")
               .Parameter("ProductSysNo", productSysNo)
               .QuerySingle<DsForbidProduct>();
         }
        #endregion

        #region 查询
        /// <summary>
        /// 通过过滤条件获取禁止升舱商品列表
        /// </summary>
        /// <param name="product">商品名称或编号</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>分页列表</returns>
        ///<remarks>2014-03-21 余勇 创建</remarks>
        public override Pager<DsForbidProduct> Query(string product, int currentPage, int pageSize)
        {
            const string sql = @"(SELECT A.*
                                FROM DsForbidProduct a inner join Pdproduct b on a.ProductSysNo=b.SysNo
                                WHERE (@0 is null or charindex(lower(b.EasName),lower(@0)) > 0 or charindex(lower(b.ErpCode),lower(@0)) > 0)
                                    ) tb";

            var dataList = Context.Select<DsForbidProduct>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var paras = new object[]
                {
                   product
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<DsForbidProduct>()
            {
                CurrentPage = currentPage,
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(currentPage, pageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }
        #endregion
    }
}
