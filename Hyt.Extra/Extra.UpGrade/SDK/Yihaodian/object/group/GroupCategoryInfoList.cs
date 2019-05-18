using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Group
{
	[Serializable]
	public class GroupCategoryInfoList 
	{	
		/// <summary>
		/// 团购类目信息
		/// </summary>
		[XmlElement("groupCategoryInfo")]
		public List<GroupCategoryInfo>  GroupCategoryInfo{ get; set; }
	}
}
