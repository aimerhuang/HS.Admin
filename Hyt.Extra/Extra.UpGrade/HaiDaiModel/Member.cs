using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
    /// <summary>
    /// 海带会员
    /// </summary>
    /// <remarks>2017-6-13 罗勤尧 创建</remarks>
    public class Member
    {

        /// <summary>
        /// 
        /// </summary>
        public int lv_id { set; get; }
        public string face { set; get; }
        public string uname { set; get; }
        public string lvname { set; get; }
        public string mobile { set; get; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string member_id { set; get; }
    }
}
