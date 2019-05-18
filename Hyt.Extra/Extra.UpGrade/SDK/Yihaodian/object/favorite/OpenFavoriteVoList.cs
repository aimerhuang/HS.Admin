using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Favorite
{
	[Serializable]
	public class OpenFavoriteVoList 
	{	
		/// <summary>
		/// 收藏信息
		/// </summary>
		[XmlElement("open_favorite_vo")]
		public List<OpenFavoriteVo>  Open_favorite_vo{ get; set; }
	}
}
