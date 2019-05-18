using Hyt.DataAccess.YTO;
using Hyt.Model.TYO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.YTO
{
    /// <summary>
    /// 圆通KK海外接口 广州跨境创建订单下发到快递接口
    /// </summary>
    /// <param name="StockOutSysNo">出库单号</param>
    /// <returns></returns>
    /// <remarks> 2017-12-12 廖移凤 创建</remarks>
    public class TmsWayBillNotifyImplBo : BOBase<TmsWayBillNotifyImplBo>
    {
         public tmsWayBillNotifyRequest GetTmsWayBillNotifyRequest(int StockOutSysNo)
         {

             return TmsWayBillNotifyDao.Instance.GetTmsWayBillNotifyRequest(StockOutSysNo);
         }
    }
}
