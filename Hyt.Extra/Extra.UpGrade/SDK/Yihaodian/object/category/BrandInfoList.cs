using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	[Serializable]
	public class BrandInfoList 
	{	
		/// <summary>
		/// 商家品牌信息列表
		/// </summary>
		[XmlElement("brandInfo")]
		public List<BrandInfo>  BrandInfo{ get; set; }
	}
}
