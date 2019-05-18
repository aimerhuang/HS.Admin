using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class TryProductInfoOuterList 
	{	
		/// <summary>
		/// 试用中心商品数据信息列表
		/// </summary>
		[XmlElement("try_product_info_outer")]
		public List<TryProductInfoOuter>  Try_product_info_outer{ get; set; }
	}
}
