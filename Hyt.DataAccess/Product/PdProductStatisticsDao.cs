using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 产品统计Dao
    /// </summary>
    /// <remarks>2013-08-30 周唐炬 创建</remarks>
    public abstract class PdProductStatisticsDao : DaoBase<PdProductStatisticsDao>
    {
        /// <summary>
        /// 创建产品统计
        /// </summary>
        /// <param name="model">产品统计实体</param>
        /// <returns>实体系统编号</returns>
        /// <remarks>2013-08-30 周唐炬 创建</remarks>
        public abstract int Create(PdProductStatistics model);

        /// <summary>
        /// 更新产品统计信息
        /// </summary>
        /// <param name="model">产品统计实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-08-30 周唐炬 创建</remarks>
        public abstract int Update(PdProductStatistics model);

        /// <summary>
        /// 获取商品统计信息
        /// </summary>
        /// <param name="ProductSysNo">商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-11-14 邵斌 创建</remarks>
        public abstract PdProductStatistics Get(int ProductSysNo);

        /// <summary>
        /// 更新销售量
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="Sales"></param>
        /// <remarks>2016-06-14 王耀发 创建</remarks>
        public abstract bool UpdatePdProStatisticsSales(int productSysNo, int Sales);
    }
}
