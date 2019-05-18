using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 网盟店铺信息出对象
	/// </summary>
	[Serializable]
	public class UnionApiStoreInfoOuter 
	{
		/**发货速度评分（5分值） */
		[XmlElement("delivery_score")]
			public double?  Delivery_score{ get; set; }

		/**发货速度与行业对比状态（1：高于；0：低于） */
		[XmlElement("delivery_status")]
			public long?  Delivery_status{ get; set; }

		/**店铺的链接地址 */
		[XmlElement("shop_url")]
			public string  Shop_url{ get; set; }

		/**店铺商品数 */
		[XmlElement("auction_count")]
			public int?  Auction_count{ get; set; }

		/**店铺logo的请求地址，暂为保留字段 */
		[XmlElement("pic_url")]
			public string  Pic_url{ get; set; }

		/**发货速度与行业对比，百分比，20就标示20% */
		[XmlElement("delivery_differ")]
			public double?  Delivery_differ{ get; set; }

		/**描述相符评分（5分制） */
		[XmlElement("item_score")]
			public double?  Item_score{ get; set; }

		/**描述相符与行业对比状态（1：高于；0：低于） */
		[XmlElement("item_status")]
			public long?  Item_status{ get; set; }

		/**店铺的所在省份名称 */
		[XmlElement("merchant_province_name")]
			public string  Merchant_province_name{ get; set; }

		/**卖家昵称，暂为保留字段 */
		[XmlElement("seller_nick")]
			public string  Seller_nick{ get; set; }

		/**商家名称 */
		[XmlElement("shop_title")]
			public string  Shop_title{ get; set; }

		/**店铺Id */
		[XmlElement("user_id")]
			public long?  User_id{ get; set; }

		/**服务态度评分（5分值） */
		[XmlElement("service_score")]
			public double?  Service_score{ get; set; }

		/**服务态度与行业对比状态（1：高于；0：低于） */
		[XmlElement("service_status")]
			public long?  Service_status{ get; set; }

		/**发货速度与行业对比，百分比，20就表示20% */
		[XmlElement("item_differ")]
			public double?  Item_differ{ get; set; }

		/**店铺的推广佣金率 */
		[XmlElement("commission_rate")]
			public string  Commission_rate{ get; set; }

		/**服务态度与行业对比，百分比，20就标示20% */
		[XmlElement("service_differ")]
			public double?  Service_differ{ get; set; }

		/**近期佣金支出量等级1-5 */
		[XmlElement("commision_level")]
			public int?  Commision_level{ get; set; }

		/**店铺经营的类目列表，数据格式为(json格式)[{"id":21266,"name":"大小家电、厨电、汽车"},{"id":34621,"name":"美容护理2"},{"id":25228,"name":"图书杂志"}] */
		[XmlElement("merchant_categorys")]
			public string  Merchant_categorys{ get; set; }

	}
}
