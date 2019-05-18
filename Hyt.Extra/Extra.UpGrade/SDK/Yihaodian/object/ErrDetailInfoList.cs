using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object
{
    [Serializable]
    public class ErrDetailInfoList
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        [XmlElement("errDetailInfo")]
        public List<ErrDetailInfo> ErrDetailInfo { get; set; }
    }
}
