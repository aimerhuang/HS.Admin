using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util.JPush
{
    /// <summary>
    /// 极光推送返回数据格式
    /// </summary>
    /// <remarks>2014-01-17 邵斌 创建</remarks>
    /// <remarks>改格式有JPush（极光推送决定），参数命名请不要以当前系统命名来检查</remarks>
    public class JPushResponse
    {
        /// <summary>
        /// 业务系统唯一标示
        /// </summary>
        public int sendno { get; set; }

        /// <summary>
        /// 推送销售返回唯一标示
        /// </summary>
        public int msg_id { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string errmsg { get; set; }
    }
}
