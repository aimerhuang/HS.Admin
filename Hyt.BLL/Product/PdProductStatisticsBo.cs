using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 产品统计BO
    /// </summary>
    /// <remarks>2013-08-30 周唐炬 创建</remarks>
    public class PdProductStatisticsBo : BOBase<PdProductStatisticsBo>
    {
        /// <summary>
        /// 创建产品统计,已有商品号则不创建
        /// </summary>
        /// <returns>实体系统编号</returns>
        /// <remarks>2013-08-30 周唐炬 创建</remarks>
        public int Create(PdProductStatistics model)
        {
            return PdProductStatisticsDao.Instance.Create(model);
        }

        /// <summary>
        /// 更新产品统计信息，还没有商品则创建
        /// </summary>
        /// <param name="model">产品统计实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-08-30 周唐炬 创建</remarks>
        public int Update(PdProductStatistics model)
        {
            var updateRows = PdProductStatisticsDao.Instance.Update(model);
            return updateRows > 0 ? updateRows : Create(model);
        }

        /// <summary>
        /// 更新产品统计信息，还没有商品则创建
        /// </summary>
        /// <param name="statisticsEnum">要变更的</param>
        /// <param name="isAdd">加1或减1，默认+1</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-08-30 周唐炬 创建</remarks>
        //public int Update(string statisticsEnum, bool isAdd = true)
        //{
        //    var updateRows = PdProductStatisticsDao.Instance.Update(model);
        //    return updateRows > 0 ? updateRows : Create(model);
        //}

        /// <summary>
        /// 获取商品统计信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-11-14 邵斌 创建</remarks>
        public PdProductStatistics Get(int productSysNo)
        {
            return PdProductStatisticsDao.Instance.Get(productSysNo);
        }
        /// <summary>
        /// 更新销售量
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="Sales"></param>
        /// <remarks>2016-06-14 王耀发 创建</remarks>
        public bool UpdatePdProStatisticsSales(int productSysNo, int Sales)
        {
            return PdProductStatisticsDao.Instance.UpdatePdProStatisticsSales(productSysNo, Sales);
        }
    }
}
