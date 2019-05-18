using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.GeGeJiaModel
{
    #region 格格家接口请求参数

    /// <summary>
    /// 格格家接口请求参数
    /// </summary>
    /// <remarks>2017-08-22 黄杰 创建</remarks>
    public class GeGeJiaParameter
    {
        /// <summary>
        /// 商家身份标识
        /// </summary>
        public string partner { get; set; }

        /// <summary>
        /// 时间戳，格式为yyyy-mm-dd HH:mm:ss。误差不超过10分钟。
        /// </summary>
        public string timestamp { get; set; }

        public GeGeJiaOrderFormParams paramss { get; set; }

    }
    
    /// <summary>
    /// 具体业务参数（json格式的字符串）
    /// </summary>
    public class GeGeJiaOrderFormParams
    {
        /// <summary>
        /// 订单状态；1：未付款，2：待发货，3：已发货，4：交易成功，5：用户取消（待退款团购），6：超时取消（已退款团购），7：团购进行中(团购)
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 付款起始时间
        /// </summary>
        /// <remarks>starTime必须小于endTime，并且起始时间跟结束时间差不得超过30天</remarks>
        public string startTime { get; set; }

        /// <summary>
        /// 付款结束时间
        /// </summary>
        /// <remarks>starTime必须小于endTime，并且起始时间跟结束时间差不得超过30天</remarks>
        public string endTime { get; set; }

        /// <summary>
        /// 页码，不传默认展示第一页
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页数量，不传默认一页50条
        /// </summary>
        public int pageSize { get; set; }
    }

    #endregion
}
