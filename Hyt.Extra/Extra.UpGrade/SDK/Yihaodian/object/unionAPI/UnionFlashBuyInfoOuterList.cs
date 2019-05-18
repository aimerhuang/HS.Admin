using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionFlashBuyInfoOuterList 
	{	
		/// <summary>
		/// 名品卖场对象集合
		/// </summary>
		[XmlElement("union_flash_buy_info_outer")]
		public List<UnionFlashBuyInfoOuter>  Union_flash_buy_info_outer{ get; set; }
	}
}
