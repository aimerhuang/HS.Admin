using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Favorite;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 商家获取收藏列表
	/// </summary>
	public class FavoriteSearchResponse 
		: YhdResponse 
	{
		/**收藏信息 */
		[XmlElement("yhd_favorites")]
		public OpenFavoriteVoList  Yhd_favorites{ get; set; }

		/**错误码 */
		[XmlElement("error_code")]
			public string  Error_code{ get; set; }

		/**子错误描述 */
		[XmlElement("sub_msg")]
			public string  Sub_msg{ get; set; }

		/**错误描述 */
		[XmlElement("msg")]
			public string  Msg{ get; set; }

		/**子错误码 */
		[XmlElement("sub_code")]
			public string  Sub_code{ get; set; }

	}
}
