using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Provider
{
    /// <summary>
    /// 备注内容
    /// </summary>
    public abstract class IRemarksParameters
    {
        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string MallOrderId { get; set; }

        private string _remarksContent = "";
        /// <summary>
        /// 备注信息
        /// </summary>
        public string RemarksContent { get { return _remarksContent; } set { _remarksContent = value; } }
    }
}
