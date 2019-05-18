using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	[Serializable]
	public class MerchantCategoryInfoList 
	{	
		/// <summary>
		/// 商家产品类别信息列表
		/// </summary>
		[XmlElement("merchantCategoryInfo")]
		public List<MerchantCategoryInfo>  MerchantCategoryInfo{ get; set; }
	}
}
