using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiPay.QianDai
{
    /// <summary>
    /// 会员卡信息
    /// </summary>
    /// <remarks>2017-03-31 杨浩 创建</remarks>
    public class VipCardInfo : IAccount 
    {
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 身份证号,仅支持 18 位格式的身份证号
        /// </summary>
        public string IdNo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

    }
}
