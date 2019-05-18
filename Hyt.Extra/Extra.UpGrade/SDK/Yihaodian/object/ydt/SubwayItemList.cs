using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	[Serializable]
	public class SubwayItemList 
	{	
		/// <summary>
		/// 商品列表
		/// </summary>
		[XmlElement("subway_item")]
		public List<SubwayItem>  Subway_item{ get; set; }
	}
}
