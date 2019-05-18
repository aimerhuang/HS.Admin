using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionApiStoreInfoOuterList 
	{	
		/// <summary>
		/// 网盟店铺信息列表
		/// </summary>
		[XmlElement("union_api_store_info_outer")]
		public List<UnionApiStoreInfoOuter>  Union_api_store_info_outer{ get; set; }
	}
}
