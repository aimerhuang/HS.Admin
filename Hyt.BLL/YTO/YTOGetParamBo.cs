using Hyt.DataAccess.YTO;
using Hyt.Model.TYO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.YTO
{
    /// <summary>
    /// 查询 圆通接口 参数
    /// </summary>
    /// <remarks> 2017-12-12 廖移凤 创建</remarks>
    public class YTOGetParamBo : BOBase<YTOGetParamBo>
    {
        /// <summary>
        /// 查询圆通电子面单接口参数
        /// </summary>
        /// <param name="StockOutSysNo">出库单号</param>
        /// <returns></returns>
        /// <remarks> 2017-12-12 廖移凤 </remarks>
        public RequestOrder GetRequestOrder(int StockOutSysNo) {

            return YTOGetParamDao.Instance.GetRequestOrder(StockOutSysNo);
        }

        /// <summary>
        /// 查询圆通电子面单接口参数 收件人
        /// </summary>
        /// <param name="StockOutSysNo">出库单号</param>
        /// <returns></returns>
        /// <remarks> 2017-12-12 廖移凤 </remarks>
        public Receiver GetReceiver(int StockOutSysNo) {

            return YTOGetParamDao.Instance.GetReceiver(StockOutSysNo);
        }

        /// <summary>
        /// 查询圆通电子面单接口参数 收件人
        /// </summary>
        /// <returns></returns>
        /// <param name="StockOutSysNo">出库单号</param>
        /// <remarks> 2017-12-12 廖移凤 </remarks>
        public Sender GetSender(int StockOutSysNo) {

            return YTOGetParamDao.Instance.GetSender(StockOutSysNo);
        }
    }
}
