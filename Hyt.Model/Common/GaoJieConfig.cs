using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 高捷物流配置
    /// </summary>
    /// <remarks>2016-7-1 王耀发 创建</remarks>
    public class GaoJieConfig : ConfigBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string seller { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string api_key { get; set; }
        /// <summary>
        /// 订单网址
        /// </summary>
        public string pweb { get; set; }

        /// <summary>
        /// 网址名称
        /// </summary>
        public string web_name { get; set; }

        /// <summary>
        /// 商家备案号
        /// </summary>
        public string re_no { get; set; }

        /// <summary>
        /// 商家备案名称
        /// </summary>
        public string re_name { get; set; }
        /// <summary>
        /// 申报地海关
        /// </summary>
        public string DeclareCustoms { get; set; }
        /// <summary>
        /// 进出口口岸
        /// </summary>
        public string IePort { get; set; }
        
        /// <summary>
        /// 进出口口岸
        /// </summary>
        public string SendUserCardID { get; set; }
    }
}
