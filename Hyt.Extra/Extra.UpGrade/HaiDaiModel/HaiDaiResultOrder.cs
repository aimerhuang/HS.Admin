using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
    /// <summary>
    /// 海带接收订单接口结果集
    /// </summary>
    /// <remarks>2017-6-13 罗勤尧 创建</remarks>
    public class HaiDaiResultOrder
    {
        /// <summary>
        /// 结果代码
        /// </summary>
        public int result { set; get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string message { set; get; }

        /// <summary>
        /// 结果
        /// </summary>
        public Data data { set; get; }
    }

    public class Data
    {
        /// <summary>
        ///不存在订单
        /// </summary>
        public string noOrderIds { set; get; }
        /// <summary>
        /// 用户保关
        /// </summary>
        public int userBaoguan { set; get; }
        /// <summary>
        /// 无仓库订单
        /// </summary>
        public string noDepots { set; get; }
        /// <summary>
        /// 错误订单id
        /// </summary>
        public string errOrderIds { set; get; }

        /// <summary>
        /// 错误数量
        /// </summary>
        public int errCount { set; get; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errorMsg { set; get; }

        /// <summary>
        /// 成功数量
        /// </summary>
        public int sucCount { set; get; }
    }
}
