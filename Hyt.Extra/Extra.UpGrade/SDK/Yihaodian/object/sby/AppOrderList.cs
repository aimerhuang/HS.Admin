using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Sby
{
	[Serializable]
	public class AppOrderList 
	{	
		/// <summary>
		/// app订单信息
		/// </summary>
		[XmlElement("appOrder")]
		public List<AppOrder>  AppOrder{ get; set; }
	}
}
