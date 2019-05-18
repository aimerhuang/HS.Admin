using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Favorite
{
	/// <summary>
	/// 收藏对象信息
	/// </summary>
	[Serializable]
	public class OpenFavoriteVo 
	{
		/**商品Id */
		[XmlElement("product_id")]
			public long?  Product_id{ get; set; }

		/**对应的商家ID，目前是商家维度收藏即商家店铺收藏 */
		[XmlElement("merchant_id")]
			public long?  Merchant_id{ get; set; }

		/**商家名称 */
		[XmlElement("merchant_name")]
			public string  Merchant_name{ get; set; }

		/**商品名称 */
		[XmlElement("product_name")]
			public string  Product_name{ get; set; }

		/**用户昵称 */
		[XmlElement("user_nick_name")]
			public string  User_nick_name{ get; set; }

	}
}
