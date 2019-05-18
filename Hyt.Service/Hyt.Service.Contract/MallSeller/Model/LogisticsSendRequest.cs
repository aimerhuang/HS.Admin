using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Service.Contract.MallSeller.Model
{
    public class LogisticsSendRequest : BaseRequest
    {
        /// <summary>
        /// 分销商编号
        /// </summary>
        public int DealerSysNo { get; set; }

        /// <summary>
        /// 分销商商城编号
        /// </summary>
        public int DealerMallSysNo { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 物流公司名称/代码
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressCode { get; set; }
    }
}
