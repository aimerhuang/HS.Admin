using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Picture
{
	[Serializable]
	public class MerchantPictureSpacePicInfoList 
	{	
		/// <summary>
		/// 图片空间图片列表信息
		/// </summary>
		[XmlElement("merchantPictureSpacePicInfo")]
		public List<MerchantPictureSpacePicInfo>  MerchantPictureSpacePicInfo{ get; set; }
	}
}
