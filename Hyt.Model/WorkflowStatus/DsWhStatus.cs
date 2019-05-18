using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    public class DsWhStatus
    {
        public enum 货物操作状态
        {
            已签收 = 70,
            已出库 = 60,
            准备出库 = 50,
            运输 = 40,
            打包 = 30,
            打印 = 20,
            扫描 = 18,
            入库 = 15,
            通过 = 10,
            创建 = 1,
            导入 = 0,
            未通过 = -10
        }

        public enum 包裹状态
        {
            正在打包=1,
            完成打包=10,
            运输 = 20
        }
        public enum 航运状态
        {
            创建 = 1,
            发货 = 10,
            抵达 = 20,
            准备清单 = 30,
            清关单已生成 = 40,
            海关存单 = 50,
            交付 = 60
        }

        public enum 出库单状态
        {
            保存=1,
            完成出库 = 10,
            完成出库未结账=20,
            已结账未完成出库=30,
            已结账已完成出库=40
            
        }
    }
}
