using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 获取物流编号和物流编码实体
    /// </summary>
    /// <remarks>2015-09-30 杨云奕 添加</remarks>
    public class LgExpressModel
    {
        /// <summary>
        /// 物流编码
        /// </summary>
        public string OverseaCarrier { get; set; }
        /// <summary>
        /// 物流号
        /// </summary>
        public string ExpressNo { get; set; }
    }
}
