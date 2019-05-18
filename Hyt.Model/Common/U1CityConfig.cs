using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    public class U1CityConfig
    {
        /// <summary>
        /// Api_Key
        /// </summary>
        public string Api_Key { get; set; }
        /// <summary>
        /// Api_Serect
        /// </summary>
        public string Api_Serect { get; set; }
        /// <summary>
        /// 异步地址
        /// </summary>
        public string Api_Url { get; set; }
        /// <summary>
        /// 返回类型  1：json,2:xml
        /// </summary>
        public string ApiResultType { get; set; }
    }
}
