using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 用户收货地址扩展类
    /// </summary>
    /// <remarks>
    /// 2013-08-06 杨晗 创建
    /// 2013-08-12 唐永勤 修改
    /// </remarks>
    public class CBCrReceiveAddress : CrReceiveAddress
    {

        /// <summary>
        /// 拼接联系电话由多个或1个手机和座机号码组成
        /// </summary>
        /// <remarks>
        /// 2013-08-06 杨晗 创建
        /// </remarks>
        public string Tel
        {
            get
            {
                string seg = string.Empty;
                if (!string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(MobilePhoneNumber))
                {
                    seg = ",";
                }
                return string.Format("{0} {1}"
                                     ,
                                     string.IsNullOrEmpty(PhoneNumber)
                                         ? string.Empty
                                         : PhoneNumber + seg
                                     ,
                                     string.IsNullOrEmpty(MobilePhoneNumber)
                                         ? string.Empty
                                         : MobilePhoneNumber
                    );

            }
        }

        /// <summary>
        /// 省名称
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public string Province { get; set; }

        /// <summary>
        /// 省编号
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public int ProvinceSysNo { get; set; }

        /// <summary>
        /// 市名称
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public string City { get; set; }

        /// <summary>
        /// 市编号
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public int CitySysNo { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public string Region { get; set; }

        /// <summary>
        /// 区域编号
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public int RegionSysNo { get; set; }
       
    }
}
