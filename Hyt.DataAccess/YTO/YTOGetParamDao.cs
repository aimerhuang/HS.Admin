using Hyt.DataAccess.Base;
using Hyt.Model.TYO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.YTO
{
    /// <summary>
    /// 查询 圆通接口 参数
    /// </summary>
    /// <remarks> 2017-12-12 廖移凤 创建</remarks>
    public abstract class YTOGetParamDao : DaoBase<YTOGetParamDao>
    {
        /// <summary>
        /// 查询圆通电子面单接口参数
        /// </summary>
        /// <param name="StockOutSysNo">出库单号</param>
        /// <returns></returns>
        /// <remarks> 2017-12-12 廖移凤 </remarks>
        public abstract RequestOrder GetRequestOrder(int StockOutSysNo);
      
       /// <summary>
       /// 查询圆通电子面单接口参数 收件人
       /// </summary>
        /// <param name="StockOutSysNo">出库单号</param>
       /// <returns></returns>
       /// <remarks> 2017-12-12 廖移凤 </remarks>
        public abstract Receiver GetReceiver(int StockOutSysNo);

       /// <summary>
       /// 查询圆通电子面单接口参数 收件人
       /// </summary>
       /// <returns></returns>
        /// <param name="StockOutSysNo">出库单号</param>
       /// <remarks> 2017-12-12 廖移凤 </remarks>
        public abstract Sender GetSender(int StockOutSysNo);
    }
}
