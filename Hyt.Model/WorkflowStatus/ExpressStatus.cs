using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 快递状态
    /// </summary>
    /// <remarks>2017-12-13 杨浩 创建</remarks>
    public class ExpressStatus
    {
        /// <summary>
        /// 配送类型
        /// 数据表:LgDeliveryType 字段:SysNo
        /// </summary>
        /// <remarks>2017-12-13 杨浩 创建</remarks>
        public enum 快递类型预定义
        {
            快递100=0,
            圆通快递=12,
            顺丰快递=14,
        } 
    }
}
