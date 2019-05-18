using Hyt.DataAccess.Order;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 二次销售实现
    /// </summary>
    /// <remarks>2014-9-23  朱成果 创建</remarks>
    public class TwoSaleDaoImpl : ITwoSaleDao
    {
        /// <summary>
        /// 添加业务员二次销售收款记录
        /// </summary>
        /// <param name="model">模型</param>
        /// <remarks>2014-9-17  朱成果 创建</remarks>
        public override  void InsertTwoSaleCashHistory(Rp_业务员二次销售 model)
        {
            string sql = @"  insert into 
                              Rp_业务员二次销售(DeliveryUserSysNo,DeliveryUserName,StockSysNo,StockName,OrderSysNo,OrderAmount,CreateDate) 
                              values(@DeliveryUserSysNo,@DeliveryUserName,@StockSysNo,@StockName,@OrderSysNo,@OrderAmount,@CreateDate) 
                         ";
            Context.Sql(sql)
                .Parameter("DeliveryUserSysNo", model.DeliveryUserSysNo)
                .Parameter("DeliveryUserName", model.DeliveryUserName)
                .Parameter("StockSysNo", model.StockSysNo)
                .Parameter("StockName", model.StockName)
                .Parameter("OrderSysNo", model.OrderSysNo)
                .Parameter("OrderAmount", model.OrderAmount)
                .Parameter("CreateDate", model.CreateDate)
                .Execute();
        }
    }
}
