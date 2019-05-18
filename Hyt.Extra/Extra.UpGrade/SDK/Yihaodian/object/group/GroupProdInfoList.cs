using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Group
{
	[Serializable]
	public class GroupProdInfoList 
	{	
		/// <summary>
		/// 团购报名信息
		/// </summary>
		[XmlElement("groupProdInfo")]
		public List<GroupProdInfo>  GroupProdInfo{ get; set; }
	}
}
