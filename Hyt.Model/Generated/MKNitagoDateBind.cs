using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 利嘉（你他购）分销商、会员、订单绑定
    /// </summary>
    public class MKNitagoDateBind
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 类型：0:分销商；1:会员；2:订单
        /// </summary>
        public int BindDateTepy { get; set; }

        /// <summary>
        /// 你他购（利嘉）数据
        /// </summary>
        public int NitagoDateSysNo { get; set; }

        /// <summary>
        /// 信营数据
        /// </summary>
        public int XinyingDateSysNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
