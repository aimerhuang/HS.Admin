using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    /// <summary>
    /// 生产厂家实体
    /// </summary>
    public class PmManufacturer
    {
        public int SysNo { get; set; }
        /// <summary>
        /// 生产厂家名称
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// 生产厂家描述备注
        /// </summary>
        public string FDisInfo { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string FContact { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string FTelephone { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string FAddress { get; set; }
        /// <summary>
        /// 生产商品分类
        /// </summary>
        public string FCategory { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行卡
        /// </summary>
        public string BankIDCard { get; set; }
        /// <summary>
        /// 生产厂家编号
        /// </summary>
        public string ManufacturerCode { get;set; }

    }
}
