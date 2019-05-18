using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 拍拍接口返回结果
    /// </summary>
    /// <remarks>2014-01-07 陶辉 重构</remarks>
    public class PaipaiResult
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string errorCode { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string errorMessage { get; set; }

        /// <summary>
        /// 当前是第几页
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        /// 总页面
        /// </summary>
        public int pageTotal { get; set; }

        /// <summary>
        /// 满足查询条件的记录总数
        /// </summary>
        public int countTotal { get; set; }

        /// <summary>
        /// 订单列表
        /// </summary>
        public List<DealInfo> dealList { get; set; }
    }
}
