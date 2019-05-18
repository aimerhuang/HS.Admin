using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Service.Contract.MallSeller.Model
{
    public class UpdateMemoRequest : BaseRequest
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string MemoContent { get; set; }

        /// <summary>
        /// 备注类型
        /// 淘宝:旗帜
        /// </summary>
        public string MemoType { get; set; }
    }
}
