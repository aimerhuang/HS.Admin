using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;

namespace Hyt.Model.Manual
{
    public class SoOrderMods:SoOrder
    {
        
        /// <summary>
        /// 收款单
        /// </summary>
        /// <remarks> 2015-10-15 杨云奕 添加</remarks>
        public FnReceiptVoucher ReceiptVoucher { get; set; }
        /// <summary>
        /// 收款单明细
        /// </summary>
        ///  <remarks> 2015-10-15 杨云奕 添加</remarks>
        public List<Hyt.Model.Transfer.CBFnReceiptVoucherItem> ReceiptVoucherItemList { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        /// <remarks> 2015-10-15 王耀发 添加</remarks>
        public string ReceiverCountry { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        /// <remarks> 2015-10-15 杨云奕 添加</remarks>
        public string ReceiverProvince { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        /// <remarks> 2015-10-15 杨云奕 添加</remarks>
        public string ReceiverCity { get; set; }
        /// <summary>
        /// 区/县名称
        /// </summary>
        /// <remarks> 2015-10-15 杨云奕 添加</remarks>
        public string ReceiverArea { get; set; }
        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <remarks> 2016-04-12 王耀发 添加</remarks>
        public CBCrCustomer Customer { get; set; }
    }
}
