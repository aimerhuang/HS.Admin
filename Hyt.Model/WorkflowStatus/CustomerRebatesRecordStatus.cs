using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 返利记录状态
    /// </summary>
    /// <remarks>2017-1-16 杨浩 创建</remarks>
    public  class CustomerRebatesRecordStatus
    {
        /// <summary>
        /// 返利记录状态
        /// 数据表:CrCustomerRebatesRecord 字段:Status
        /// </summary>
        public enum 返利状态
        {
            冻结=0,
            完结=1,
            作废=-1,
        }
        /// <summary>
        /// 提现审核状态
        /// 数据表:CrPredepositCash 字段:Status
        /// </summary>
        public enum 提现审核状态
        {
            未审核=0,
            已审核=1,
            作废=-1,       
        }

    }
}
