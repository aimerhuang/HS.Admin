using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 利嘉查询会员返回数据
    /// </summary>
    /// <remarks>
    /// 2017-05-18 罗勤尧 生成
    /// </remarks>
    public class MemberSerch
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember(Name = "Success")]
        public bool Success { get; set; }

        /// <summary>
        /// 查询结果总记录
        /// </summary>
        [DataMember(Name = "total")]
        public int total { get; set; }

        /// <summary>
        /// accessToken
        /// </summary>
        [DataMember(Name = "Message")]
        public string Message { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        [DataMember(Name = "rows")]
        public List<MemberModel> rows { get; set; }
    }
}
