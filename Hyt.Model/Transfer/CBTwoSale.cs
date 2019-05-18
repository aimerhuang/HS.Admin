using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 
    /// 二次销售对账报表
    /// </summary>
    /// <remarks>2014-9-22 朱成果 创建</remarks>
   public  class CBTwoSale
    {
       /// <summary>
       /// 用户编号
       /// </summary>
       public int UserID { get; set; }

       /// <summary>
       /// 用户名称
       /// </summary>
       public string UserName { get; set; }

       /// <summary>
       /// 仓库编号
       /// </summary>
       public int WarehouseSysNo { get; set; }

       /// <summary>
       /// 仓库
       /// </summary>
       public string WarehouseName { get; set; }

       /// <summary>
       /// 下单日期
       /// </summary>
       public string CreateDate { get; set; }

       /// <summary>
       /// 订单数量
       /// </summary>
       public int OrderCount { get; set; }

       /// <summary>
       /// 订单合计金额
       /// </summary>
       public decimal OrderCash { get; set; }
    }
}
