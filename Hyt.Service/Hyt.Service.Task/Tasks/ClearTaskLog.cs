using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.Service.Task.Core;
using Hyt.Model.Pos;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 定时移除微信JsTicket缓存
    /// </summary>
    /// <remarks>2016-5-3 杨浩 创建</remarks>
    [Description("定时移除微信JsTicket缓存")]
    public class JsTicketTask : ITask
    {
        /// <summary>
        /// 定时清理任务日志
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2016-5-3 杨浩 添加</remarks>
        public void Execute(object state)
        {
            List<DBDsPosOrderItemData> saveDataList = Hyt.BLL.Pos.DsPosOrderBo.Instance.GetPosOrderListByNoBindSale();
        }
    }
}
