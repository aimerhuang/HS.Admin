using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupproductAPI
{
	[Serializable]
	public class PmStockList 
	{	
		/// <summary>
		/// 库存产品列表
		/// </summary>
		[XmlElement("pmStock")]
		public List<PmStock>  PmStock{ get; set; }
	}
}
