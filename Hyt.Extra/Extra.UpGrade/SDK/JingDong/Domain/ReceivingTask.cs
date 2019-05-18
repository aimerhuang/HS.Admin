using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReceivingTask : JdObject{


         [XmlElement("goods_no")]
public  		string
  goodsNo { get; set; }


         [XmlElement("goods_status")]
public  		string
  goodsStatus { get; set; }


         [XmlElement("qty")]
public  		string
  qty { get; set; }


         [XmlElement("expected_qty")]
public  		string
  expectedQty { get; set; }


         [XmlElement("difference_remark")]
public  		string
  differenceRemark { get; set; }


}
}
