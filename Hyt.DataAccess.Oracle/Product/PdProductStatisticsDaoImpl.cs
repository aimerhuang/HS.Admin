using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 产品统计Dao
    /// </summary>
    /// <remarks>2013-08-30 周唐炬 创建</remarks>
    public class PdProductStatisticsDaoImpl : PdProductStatisticsDao
    {
        /// <summary>
        /// 创建产品统计
        /// </summary>
        /// <param name="model">产品统计实体</param>
        /// <returns>实体系统编号</returns>
        /// <remarks>2013-08-30 周唐炬 创建</remarks>
        public override int Create(Model.PdProductStatistics model)
        {
            return Context.Insert<PdProductStatistics>("PdProductStatistics", model)
                                   .AutoMap(o => o.SysNo)
                                   .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新产品统计信息
        /// </summary>
        /// <param name="model">产品统计实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-08-30 周唐炬 创建</remarks>
        public override int Update(Model.PdProductStatistics model)
        {
            return Context.Update<PdProductStatistics>("PdProductStatistics", model)
                .AutoMap(o => o.SysNo)
                .Where("SysNo", model.SysNo)
                .Execute();
        }

        /// <summary>
        /// 获取商品统计信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品统计对象结果</returns>
        /// <remarks>2013-11-14 邵斌 创建</remarks>
        public override PdProductStatistics Get(int productSysNo)
        {
            return Context.Select<PdProductStatistics>("*")
                       .From("PdProductStatistics")
                       .Where("ProductSysNo=@ProductSysNo")
                       .Parameter("ProductSysNo", productSysNo)
                       .QuerySingle();
        }
        /// <summary>
        /// 更新销售量
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="Sales"></param>
        /// <remarks>2016-06-14 王耀发 创建</remarks>
        public override bool UpdatePdProStatisticsSales(int productSysNo, int Sales)
        {
            int result = Context.Sql("update PdProductStatistics set Sales = @0 where ProductSysNo = @1 ", Sales,
                                     productSysNo).Execute();
            return result > 0;
        }
    }
}
