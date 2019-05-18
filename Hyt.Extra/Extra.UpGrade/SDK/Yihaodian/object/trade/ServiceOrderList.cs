using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Trade
{
	[Serializable]
	public class ServiceOrderList 
	{	
		/// <summary>
		/// 服务子订单列表（暂不提供 ）
		/// </summary>
		[XmlElement("service_order")]
		public List<ServiceOrder>  Service_order{ get; set; }
	}
}
