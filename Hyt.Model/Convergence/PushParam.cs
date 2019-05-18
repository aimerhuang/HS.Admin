using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Convergence
{
   public class PushParam
    {
        /// <summary>
        /// 商户编号
        /// </summary>
        public string Pc01_MerchantNo { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string Pc02_OrderNo { get; set; }
        /// <summary>
        /// 海关代码
        /// </summary>
        public string Pc03_CustomsCode { get; set; }
        /// <summary>
        /// 申报类型
        /// </summary>
        public string Pc04_FunctionCode { get; set; }
        /// <summary>
        /// 电商平台互联网域名
        /// </summary>
        public string Pc06_DomainName { get; set; }
        /// <summary>
        /// 电商平台代码
        /// </summary>
        public string Pc07_TmallCode { get; set; }
        /// <summary>
        /// 电商平台名称
        /// </summary>
        public string Pc08_TmallName { get; set; }
        /// <summary>
        /// 付款人姓名
        /// </summary>
        public string Pc09_PayerName { get; set; }
        /// <summary>
        /// 付款人证件类型  1 身份证
        /// </summary>
        public int Pc10_PayerIdType { get; set; }
        /// <summary>
        /// 付款人证件号 
        /// </summary>
        public string Pc11_PayerIdNo { get; set; }
        /// <summary>
        /// 付款人手机号
        /// </summary>
        public string Pc12_PayerTel { get; set; }
    }
}
