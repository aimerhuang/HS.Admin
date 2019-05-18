using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 利嘉取消订单
    /// </summary>
    /// <remarks>2017-5-18 罗勤尧 创建</remarks>
   public class LiJiaOrderCancel
    {
        /// <summary>
        /// 第三方订单号
        /// </summary>
        [Description("第三方订单号")]
        public string SourceOrderNo { get; set; }
    }
}
