using Hyt.DataAccess.YTO;
using Hyt.Model.TYO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.YTO
{
    /// <summary>
    /// 查询 圆通接口 参数
    /// </summary>
    /// <remarks> 2017-12-12 廖移凤 创建</remarks>
    public class YTOGetParamImpl : YTOGetParamDao
    {
        /// <summary>
        /// 查询圆通电子面单接口参数
        /// </summary>
        /// <returns></returns>
        /// <param name="StockOutSysNo">出库单号</param>
        /// <remarks> 2017-12-12 廖移凤 </remarks>
        public override RequestOrder GetRequestOrder(int StockOutSysNo){

            return Context.Sql(@"select w.ProductQuantity number,w.ProductName itemName,s.SysNo txLogisticID 
                                   from WhStockOutItem w,SoOrder s where s.SysNo=w.OrderSysNo 
                                   and StockOutSysNo=@0", StockOutSysNo).QuerySingle<RequestOrder>();
        }
        /// <summary>
        /// 查询圆通电子面单接口参数 收件人
        /// </summary>
        /// <returns></returns>
        /// <param name="StockOutSysNo">出库单号</param>
        /// <remarks> 2017-12-12 廖移凤 </remarks>
        public override Receiver GetReceiver(int StockOutSysNo)
        {

            return Context.Sql(@"select b.streetaddress  [address],b.phone mobile,b.WarehouseName name 
                                from whstockout t left join whwarehouse b on t.warehousesysno=b.sysno 
                                left join soreceiveaddress c on t.receiveaddresssysno=c.sysno 
                                where t.sysno=@0", StockOutSysNo).QuerySingle<Receiver>();
        }

        /// <summary>
        /// 查询圆通电子面单接口参数 收件人
        /// </summary>
        /// <returns></returns>
        /// <param name="StockOutSysNo">出库单号</param>
        /// <remarks> 2017-12-12 廖移凤 </remarks>
        public override Sender GetSender(int StockOutSysNo)
        {
            return Context.Sql(@" select  c.name name ,c.mobilephonenumber mobile,
                                c.streetaddress printAddr from whstockout t left join whwarehouse b on t.warehousesysno=b.sysno 
                                left join soreceiveaddress c on t.receiveaddresssysno=c.sysno  
                                where t.sysno=@0", StockOutSysNo).QuerySingle<Sender>();
        }
    }
}
