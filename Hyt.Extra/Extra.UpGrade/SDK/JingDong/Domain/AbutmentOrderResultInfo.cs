using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{

    [Serializable]
    public class AbutmentOrderResultInfo : JdObject
    {


        [XmlElement("isAuthorized")]
        public string
          isAuthorized { get; set; }


        [XmlElement("factoryAbutmentOrderDealInfoList")]
        public List<string>
          factoryAbutmentOrderDealInfoList { get; set; }


        [XmlElement("errorMessage")]
        public string
          errorMessage { get; set; }


    }
}
