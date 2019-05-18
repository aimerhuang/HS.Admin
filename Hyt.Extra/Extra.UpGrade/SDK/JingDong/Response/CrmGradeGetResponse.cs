using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class CrmGradeGetResponse : JdResponse{


         [XmlElement("grade_promotions")]
public  		string
  gradePromotions { get; set; }


}
}
