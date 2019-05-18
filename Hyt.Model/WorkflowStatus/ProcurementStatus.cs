using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    public class ProcurementStatus
    {
        public enum 采购申请单状态
        {
            作废 = -1,
            保存 = 0,
            已推至机构 = 1,
            已分单至厂家 = 2,
        }
        public enum 采购申请单发货状态
        {
            未发货 = 0,
            部分发货 = 1,
            全部发货 = 2,
            完成 = 3
        }
        public enum 采购申请单收货状态
        {
            未收货 = 0,
            部分入库 = 1,
            已全部入库 = 2,
            完成 = 3
        }

        public enum 采购分货单状态
        {
            保存 = 0,
            厂家生产 = 1,
            产品配送 = 2,
            配送完成在途运输 = 3,
            完成 = 4
        }
        public enum 商品配送状态
        { 
            未配送 = 0,
            配送中 = 1,
            配送完成 = 2
        }
        public enum 采购货品配送
        {
            保存 = 0,
            配送出库 = 1,
            配送完成 = 2
        }
    }
}
