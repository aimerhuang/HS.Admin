using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 第三方快递打印包裹单实体类
    /// </summary>
    /// <remarks>
    /// 2013-07-12 郑荣华 创建
    /// </remarks>
    public class PrtPack
    {
        /// <summary>
        /// 出库单表系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 寄件人姓名
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// 寄件城市
        /// </summary>
        public string FromCity { get; set; }

        /// <summary>
        /// 寄件单位 固定商城
        /// </summary>
        //public string FromCompanyName { get; set; }

        /// <summary>
        /// 寄件地址
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// 寄件人电话（固定电话）
        /// </summary>
        public string FromTel { get; set; }

        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// 收件人单位
        /// </summary>
       // public string ToCompanyName { get; set; }

        /// <summary>
        /// 收件城市
        /// </summary>
        public string ToCity { get; set; }

        /// <summary>
        /// 收件地址
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// 收件人手机号码
        /// </summary>
        public string ToMobile { get; set; }

        /// <summary>
        /// 收件人固定电话
        /// </summary>
        public string ToTel { get; set; }

        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 升舱订单号
        /// </summary>
        public string DsOrderSysNo { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        /// <remarks>2016-8-2 杨浩 添加</remarks>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 街道地址
        /// </summary>
        /// <remarks>2016-8-2 杨浩 添加</remarks>
        public string StreetAddress { get; set; }

    }
}
