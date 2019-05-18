using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Extra.UpGrade.SDK.JingDong.Response
{





    public class AddressCompleteServiceResponse : JdResponse
    {


        [XmlElement("result")]
        public string
          result { get; set; }


    }
}
