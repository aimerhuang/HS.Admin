using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Icp.GZNanSha.RegRec
{
    public class Root
    {
        /// <summary>
        /// 头部信息
        /// </summary>
        public Head head { get; set; }
        /// <summary>
        /// 功能回调信息
        /// </summary>
        public Declaration declaration { get; set; }
    }
}
