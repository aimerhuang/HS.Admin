using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 代理商预存款来往明细扩展类
    /// </summary>
    /// <remarks> 2016-04-18 刘伟豪 创建 </remarks>
    public class CBDsAgentPrePaymentItem : DsAgentPrePaymentItem
    {
        /// <summary>
        /// 代理商名称
        /// </summary>
        public string AgentName { get; set; }
    }
}