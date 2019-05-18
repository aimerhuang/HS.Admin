using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.CustomProductAPI
{
	[Serializable]
	public class JdProductPriceList 
	{	
		/// <summary>
		/// 产品价格列表
		/// </summary>
		[XmlElement("jdProductPrice")]
		public List<JdProductPrice>  JdProductPrice{ get; set; }
	}
}
