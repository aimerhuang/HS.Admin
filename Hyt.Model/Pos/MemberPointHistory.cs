using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class MemberPointHistory
    {
        /// <summary>
        ///  系统自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 获取订单编号
        /// </summary>
        public string mph_OrderNumber { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string mph_Text { get; set; }
        /// <summary>
        ///话费积分
        /// </summary>
        public long mph_Point { get; set; }
        /// <summary>
        /// 类型名
        /// </summary>
        public string mph_TypeName { get; set; }
        /// <summary>
        /// 时间日期
        /// </summary>
        public DateTime mph_Time { get; set; }
        /// <summary>
        /// 是否更新到的类型
        /// </summary>
        public int OnWebType { get; set; }
        /// <summary>
        /// 会员卡编号
        /// </summary>
        public string mph_CardNumber { get; set; }
        /// <summary>
        /// 会员卡名称
        /// </summary>
        public string mph_CardName { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int DsSysNo { get; set; }
    }
}
