using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
    /// <summary>
    /// 海带接口返回
    /// </summary>
    /// <remarks>2017-6-13 罗勤尧 创建</remarks>
    public class HaiDaiResultLogin
    {

        /// <summary>
        /// 结果代码
        /// </summary>
        public int result { set; get; }
        /// <summary>
        /// toke
        /// </summary>
        public string  token { set; get; }
        /// <summary>
        /// 消息
        /// </summary>
        public string message { set; get; }
        /// <summary>
        /// 会员信息
        /// </summary>
        public Member member {set;get;}
        /// <summary>
        /// 结果
        /// </summary>
        public resultL data { set; get; }
    }
}
