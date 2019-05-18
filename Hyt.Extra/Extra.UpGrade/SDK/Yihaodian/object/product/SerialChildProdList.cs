using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class SerialChildProdList 
	{	
		/// <summary>
		/// 系列产品子产品信息
		/// </summary>
		[XmlElement("serialChildProd")]
		public List<SerialChildProd>  SerialChildProd{ get; set; }
	}
}
