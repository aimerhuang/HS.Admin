using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class SerialProductList 
	{	
		/// <summary>
		/// 系列产品列表
		/// </summary>
		[XmlElement("serialProduct")]
		public List<SerialProduct>  SerialProduct{ get; set; }
	}
}
