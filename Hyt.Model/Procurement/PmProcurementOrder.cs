using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
 
    /// <summary>
    /// 采购申请单
    /// 2016-1-7 杨云奕 添加
    /// </summary>
    public class PmProcurementOrder
    {
        public int SysNo { get; set; }
        public string Po_Number { get; set; }
        public DateTime Po_CreateTime { get; set; }
        public int Po_CreateBy { get; set; }
        public DateTime Po_UpdateTime { get; set; }
        public int Po_UpdateBy { get; set; }
        public int Po_Status { get; set; }
        /// <summary>
        /// 发货状态
        /// </summary>
        public int Po_SendStatus { get; set; }
        /// <summary>
        /// 收货状态
        /// </summary>
        public int Po_GetStatus { get; set; }
    }
}
