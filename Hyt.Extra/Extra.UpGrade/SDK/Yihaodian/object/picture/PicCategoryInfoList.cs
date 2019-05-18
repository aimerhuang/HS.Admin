using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Picture
{
	[Serializable]
	public class PicCategoryInfoList 
	{	
		/// <summary>
		/// 图片空间分类信息列表
		/// </summary>
		[XmlElement("picCategoryInfo")]
		public List<PicCategoryInfo>  PicCategoryInfo{ get; set; }
	}
}
