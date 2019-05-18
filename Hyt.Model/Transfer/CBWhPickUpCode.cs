using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 提货码及验证码分页查询
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    public class CBWhPickUpCode
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 状态待发(10),已发(20),作废(-10)
        /// </summary>
        public int Status { get; set; }

        public string StockOutSysNo { get; set; }

    }
}
