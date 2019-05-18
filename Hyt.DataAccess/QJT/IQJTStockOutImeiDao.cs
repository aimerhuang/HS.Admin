using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.QJT
{
    /// <summary>
    /// 千机团串码记录
    /// </summary>
    /// <remarks>2016-02-17 谭显锋 创建</remarks>    
    public abstract class IQJTStockOutImeiDao : DaoBase<IQJTStockOutImeiDao>
    {
        /// <summary>
        /// 添加千机团串码记录
        /// </summary>
        /// <param name="model">实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2016-02-17 谭显锋 创建</remarks>
        public abstract int Create(QJTStockOutImei model);

        /// <summary>
        /// 获取千机团串码记录
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>实体</returns>       
        /// <remarks>2016-02-17 谭显锋 创建</remarks>
        public abstract QJTStockOutImei Get(int sysno);

        /// <summary>
        /// 根据商品编号获取商品的串码
        /// </summary>
        /// <param name="stockOutItemSysNo">出库单明细编号</param>
        /// <returns>商品的串码实体</returns>
        /// <remarks>2016-02-17 谭显锋 创建</remarks>
        public abstract IList<QJTStockOutImei> GetImeiByStockOutItemSysNo(int stockOutItemSysNo);

        /// <summary>
        /// 根据出库单编号获取串码列表
        /// </summary>
        /// <param name="stockoutsysno">出库单编号</param>
        /// <returns></returns>
        /// <remarks>2016-02-17 谭显锋 创建</remarks>
        public abstract IList<QJTStockOutImei> GetImeiByStockOutSysNo(int stockoutsysno);

        /// <summary>
        /// 更新商品的串码
        /// </summary>
        /// <param name="model">千机团串码记录实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2016-02-19 谭显锋 创建</remarks>
        public abstract int Update(QJTStockOutImei model);

    }
}
