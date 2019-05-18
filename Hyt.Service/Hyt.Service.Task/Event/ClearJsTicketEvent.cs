using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.Service.Task.Core;
using Hyt.BLL.ScheduledEvents;
using Hyt.Infrastructure.Memory;

namespace Hyt.Service.Task.Event
{
    /// <summary>
    /// 定时清除微信JsTicket缓存
    /// </summary>
    /// <remarks>2016-5-14 杨浩 添加</remarks>
    [Description("定时清除微信JsTicket缓存")]
    public class ClearJsTicketEvent : IEvent
    {
        /// <summary>
        /// 定时清除微信JsTicket缓存
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2016-5-14 杨浩 添加</remarks>
        public void Execute(object state)
        {
            MemoryProvider.Default.Remove(string.Format(KeyConstant.WeixinJsTicket_,state.ToString()));      
        }
    }
}
