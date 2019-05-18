using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class PmPriceList 
	{	
		/// <summary>
		/// 产品价格列表
		/// </summary>
		[XmlElement("pmPrice")]
		public List<PmPrice>  PmPrice{ get; set; }
	}
}
