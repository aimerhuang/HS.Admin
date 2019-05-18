using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.ExpressList
{
    /// <summary>
    /// 快递单查询
    /// </summary>
    /// <remarks>2017-11-29 廖移凤 创建</remarks>
   public class KuaiDiNumQuery
    {   
      /// <summary>
      /// 快递单号
      /// </summary>
       public string KuaidiNo { get; set; }
       /// <summary>
       /// 出库单号
       /// </summary>
       public string WhStocklnNo { get; set; }
       /// <summary>
       /// 订单号
       /// </summary>
       public string OrderNo { get; set; }
       /// <summary>
       /// 收货人
       /// </summary>
       public string Name { get; set; }
       /// <summary>
       /// 价格
       /// </summary>
       public string Money { get; set; }
       /// <summary>
       /// 状态
       /// </summary>
       public string Status { get; set; }
       /// <summary>
       /// 配送方式
       /// </summary>
       public string DeliveryTypeName { get; set; }
    }
}
