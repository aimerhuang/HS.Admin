using System;
using System.Collections.Generic;
using System.Text;

namespace Extra.Logistics
{
    /// <summary>
    /// 各接口url
    /// </summary>
    public static class ApiUrl
    {
        /// <summary>
        /// 创建一号仓包裹预报单接口
        /// </summary>
        public static string InboundCreate = "http://api.1hcang.com/Inbound/Create";

        /// <summary>
        /// 查询包裹入库状态接口
        /// </summary>
        public static string InboundQuery = "http://api.1hcang.com/Inbound/Query";

        /// <summary>
        /// 创建一号仓创建预报接口
        /// </summary>
        public static string OutboundCreate = "http://api.1hcang.com/Outbound/Create";

        /// <summary>
        /// 查询运单转运状态接口
        /// </summary>
        public static string OutboundQuery = "http://api.1hcang.com/Outbound/Query";

        /// <summary>
        /// 查询运单转运状态
        /// </summary>
        public static string OutboundQueryDetail = "http://api.1hcang.com/Outbound/QueryDetail";
    }
}