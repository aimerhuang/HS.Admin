using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class CBDsPosManage : DsPosManage
    {
        public string DealerName { get; set; }
    }

    /// <summary>
    /// 收银机管理表
    /// </summary>
    /// <remarks>
    /// 2016-02-15 杨云奕 添加
    /// </remarks>
    public class DsPosManage
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int pos_DsSysNo { get; set; }
        /// <summary>
        /// Mac地址
        /// </summary>
        public string pos_MacData { get; set; }
        /// <summary>
        /// 收银机名称
        /// </summary>
        public string pos_posName { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime pos_dateTime { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime pos_BindTime { get; set; }
        /// <summary>
        /// 秘钥的Key
        /// </summary>
        public string pos_KeyData { get; set; }

        /// <summary>
        /// 终端机器编码
        /// </summary>
        public string Pos_TLTermid { get; set; }
    }
}
