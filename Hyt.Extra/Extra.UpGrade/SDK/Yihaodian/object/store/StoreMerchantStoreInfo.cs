using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Store
{
	/// <summary>
	/// 店铺基础信息
	/// </summary>
	[Serializable]
	public class StoreMerchantStoreInfo 
	{
		/**店铺ID */
		[XmlElement("storeId")]
			public long?  StoreId{ get; set; }

		/**店铺名称 */
		[XmlElement("storeName")]
			public string  StoreName{ get; set; }

		/**店铺网址 */
		[XmlElement("storeAddress")]
			public string  StoreAddress{ get; set; }

		/**开店时间 */
		[XmlElement("storeOpenTime")]
			public string  StoreOpenTime{ get; set; }

		/**店铺所属的类目编号 */
		[XmlElement("storeCategoryCode")]
			public long?  StoreCategoryCode{ get; set; }

		/**卖家昵称 */
		[XmlElement("storeNickName")]
			public string  StoreNickName{ get; set; }

		/**店铺评分(服务态度) */
		[XmlElement("attitudeExactExpPoint")]
			public double?  AttitudeExactExpPoint{ get; set; }

		/**店铺评分(描述相符) */
		[XmlElement("descriptExactExpPoint")]
			public double?  DescriptExactExpPoint{ get; set; }

		/**店铺评分(发货速度) */
		[XmlElement("logisticeExactExpPoint")]
			public double?  LogisticeExactExpPoint{ get; set; }

		/**主营类目 */
		[XmlElement("storeOperateCategory")]
			public string  StoreOperateCategory{ get; set; }

		/**店铺模式（自配送、供应商） */
		[XmlElement("storeMode")]
			public string  StoreMode{ get; set; }

	}
}
