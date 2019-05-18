using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	[Serializable]
	public class RefundOrderInfoList 
	{	
		/// <summary>
		/// 返回的异常订单退款信息
		/// </summary>
		[XmlElement("refundOrderInfo")]
		public List<RefundOrderInfo>  RefundOrderInfo{ get; set; }
	}
}
