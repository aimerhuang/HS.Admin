using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.QJT;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.QJT
{
    /// <summary>
    /// 千机团串码记录
    /// </summary>
    /// <remarks>2016-02-17 谭显锋 创建</remarks>  
    public class QJTStockOutImeiDaoImpl : IQJTStockOutImeiDao
    {
        /// <summary>
        /// 添加千机团串码记录
        /// </summary>
        /// <param name="model">实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2016-02-17 谭显锋 创建</remarks>
        public override int Create(QJTStockOutImei model)
        {
            int sysno = 0;
            sysno = Context.Insert<QJTStockOutImei>("QJTStockOutImei", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("Sysno");
            return sysno;
        }

        /// <summary>
        /// 获取千机团串码记录
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>实体</returns>       
        /// <remarks>2016-02-17 谭显锋 创建</remarks>
        public override QJTStockOutImei Get(int sysno)
        {
            return Context.Sql("select * from qjtstockoutimei where sysno= @sysno")
                                      .Parameter("sysno", sysno)
                                      .QuerySingle<QJTStockOutImei>();
        }

        /// <summary>
        /// 根据商品编号获取商品的串码
        /// </summary>
        /// <param name="stockOutItemSysNo">出库单明细编号</param>
        /// <returns>商品的串码实体</returns>
        /// <remarks>2016-02-17 谭显锋 创建</remarks>
        public override IList<QJTStockOutImei> GetImeiByStockOutItemSysNo(int stockOutItemSysNo)
        {
            return Context.Sql("select * from qjtstockoutimei where stockOutItemSysNo= @stockOutItemSysNo order by sysno asc")
                                     .Parameter("stockOutItemSysNo", stockOutItemSysNo)
                                     .QueryMany<QJTStockOutImei>();
        }
  
        /// 根据出库单编号获取串码列表
        /// </summary>
        /// <param name="stockoutsysno">出库单编号</param>
        /// <returns></returns>
        /// <remarks>2016-02-17 谭显锋 创建</remarks>
        public override IList<QJTStockOutImei> GetImeiByStockOutSysNo(int stockoutsysno)
        {
            return Context.Sql("select * from qjtstockoutimei where StockOutSysNo= @StockOutSysNo order by sysno asc")
                                     .Parameter("StockOutSysNo", stockoutsysno)
                                     .QueryMany<QJTStockOutImei>();
        }


        /// <summary>
        /// 更新商品的串码
        /// </summary>
        /// <param name="model">千机团串码记录实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2016-02-19 谭显锋 创建</remarks>
        public override int Update(QJTStockOutImei model)
        {
           return Context.Update("QJTStockOutImei", model)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", model.SysNo)
                   .Execute();
        }
    }
}
