using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Order
{
    public class OrderWaybillRel
    {
        /// <summary>
        /// 物流企业代码
        /// </summary>
        [XmlElement(ElementName = "EHSEntNo")]
        public string EHSEntNo { get; set; }

        /// <summary>
        /// 物流企业名称
        /// </summary>
        [XmlElement(ElementName = "EHSEntName")]
        public string EHSEntName { get; set; }

        /// <summary>
        /// 电子运单编号
        /// </summary>
        [XmlElement(ElementName = "WaybillNo")]
        public string WaybillNo { get; set; }

        /// <summary>
        /// 备注 可空
        /// </summary>
        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }
    }
}
