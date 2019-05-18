using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 微信咨询客服回复查询实体
    /// </summary>
    /// <remarks>
    /// 2013-11-07 郑荣华 创建
    /// </remarks>
    public class CBMkWeixinQuestion : MkWeixinQuestion
    {
        /// <summary>
        /// 新消息数目
        /// </summary>
        public int NewsNum { get; set; }

        /// <summary>
        /// 显示名，客服回复显示客服账号，否则显示微信号
        /// </summary>
        public string ShowName { get; set; }
    }
}
