using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 物流配送信息查询实体类
    /// </summary>
    /// <remarks>
    /// 2014-04-10 余勇 创建
    /// </remarks>
    public class CBLgExpressInfo
    {
        /// <summary>
        //物流配送信息
        /// </summary>
        public LgExpressInfo LgExpressInfo { get; set; }

        /// <summary>
        /// 物流配送日志
        /// </summary>
        public IList<LgExpressLog> LgExpressLog { get; set; }
    }
}
