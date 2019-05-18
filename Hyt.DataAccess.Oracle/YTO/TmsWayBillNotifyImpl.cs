using Hyt.DataAccess.YTO;
using Hyt.Model.TYO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.YTO
{
    /// <summary>
    /// 圆通KK海外接口 广州跨境创建订单下发到快递接口
    /// </summary>
    /// <remarks> 2017-12-12 廖移凤 创建</remarks>
   public class TmsWayBillNotifyImpl:TmsWayBillNotifyDao
    {
        /// <summary>
        /// 圆通KK海外接口 广州跨境创建订单下发到快递接口 参数
        /// </summary>
        /// <param name="StockOutSysNo">出库单号</param>
        /// <returns></returns>
        /// <remarks> 2017-12-12 廖移凤 创建</remarks>
       public override tmsWayBillNotifyRequest GetTmsWayBillNotifyRequest(int StockOutSysNo) {


           return Context.Sql(@"select  c.name consignee ,c.mobilephonenumber consigneeTel,w.ProductName goodsName,w.OrderSysNo tradeId,ww.StreetAddress ,
                                c.streetaddress consigneeAddress from whstockout t ,WhStockOutItem w,WhWarehouse ww,BsArea b,
                                soreceiveaddress c where   t.receiveaddresssysno=c.sysno and t.SysNo=w.StockOutSysNo and t.WarehouseSysNo=ww.SysNo
                                and ww.AreaSysNo=b.SysNo and t.sysno=@0", StockOutSysNo).QuerySingle<tmsWayBillNotifyRequest>();
       }
    }
}
