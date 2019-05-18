using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    public class IcpStatus
    {
        /// <summary>
        /// 商品商检推送状态
        /// 数据表:SpCombo 字段:Status
        /// </summary>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public enum 商品商检推送状态
        {
            待推送 = 0,
            已推送 = 1,
            已接收 = 2,
            已通过 = 3,
            申报失败 = -1
        }
    }
}
