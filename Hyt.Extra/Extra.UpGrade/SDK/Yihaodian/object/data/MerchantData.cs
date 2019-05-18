using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Data
{
	/// <summary>
	/// 商家数据
	/// </summary>
	[Serializable]
	public class MerchantData 
	{
		/**商家ID */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**店铺下单数 */
		[XmlElement("orderNum")]
			public long?  OrderNum{ get; set; }

		/**店铺下单商品数 */
		[XmlElement("orderProNum")]
			public long?  OrderProNum{ get; set; }

		/**店铺下单金额 */
		[XmlElement("orderMoney")]
			public double?  OrderMoney{ get; set; }

		/**店铺下单用户数 */
		[XmlElement("orderUser")]
			public long?  OrderUser{ get; set; }

		/**店铺成功下单数 */
		[XmlElement("payNum")]
			public long?  PayNum{ get; set; }

		/**店铺成功下单商品数 */
		[XmlElement("payProNum")]
			public long?  PayProNum{ get; set; }

		/**店铺成功下单金额 */
		[XmlElement("payMoney")]
			public double?  PayMoney{ get; set; }

		/**店铺成功下单用户数 */
		[XmlElement("payUser")]
			public long?  PayUser{ get; set; }

		/**店铺添加购物车次数 */
		[XmlElement("cartNum")]
			public long?  CartNum{ get; set; }

		/**店铺所有页浏览数 */
		[XmlElement("pv")]
			public long?  Pv{ get; set; }

		/**店铺所有页访客数 */
		[XmlElement("uv")]
			public long?  Uv{ get; set; }

		/**店铺详情页浏览数 */
		[XmlElement("detailPv")]
			public long?  DetailPv{ get; set; }

		/**店铺详情页访客数 */
		[XmlElement("detailUv")]
			public long?  DetailUv{ get; set; }

		/**店铺收藏次数 */
		[XmlElement("shopFavNum")]
			public long?  ShopFavNum{ get; set; }

		/**店铺商品收藏次数 */
		[XmlElement("proFavNum")]
			public long?  ProFavNum{ get; set; }

		/**日期 */
		[XmlElement("countDateStr")]
			public string  CountDateStr{ get; set; }

	}
}
