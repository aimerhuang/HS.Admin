using Hyt.BLL.ScheduledEvents;
using Hyt.Infrastructure.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Service.Event
{
    /// <summary>
    /// 定时清除微信AccessToken缓存
    /// </summary>
    /// <remarks>2016-5-14 杨浩 添加</remarks>
    [Description("定时清除微信AccessToken缓存")]
    public class ClearAccessTokenEvent : IEvent
    {
        /// <summary>
        /// 定时清除微信JsTicket缓存
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2016-5-14 杨浩 添加</remarks>
        public void Execute(object state)
        {
            MemoryProvider.Default.Remove(string.Format(KeyConstant.WeixinAccessToken_, state.ToString()));
            BLL.Log.LocalLogBo.Instance.Write(DateTime.Now.ToString() + "  定时清除微信AccessToken缓存\r\n", "ClearAccessTokenEventLog");
        }
    }
}
