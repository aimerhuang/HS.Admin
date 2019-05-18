using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Sby
{
	/// <summary>
	/// 流量调用报表详细信息
	/// </summary>
	[Serializable]
	public class AppMerchantSummaryCost 
	{
		/**APPID */
		[XmlElement("appId")]
			public long?  AppId{ get; set; }

		/**app名称 */
		[XmlElement("appName")]
			public string  AppName{ get; set; }

		/**isvId */
		[XmlElement("isvId")]
			public string  IsvId{ get; set; }

		/**方法名称 */
		[XmlElement("invokeMethod")]
			public string  InvokeMethod{ get; set; }

		/**商家id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**商家名称 */
		[XmlElement("merchantName")]
			public string  MerchantName{ get; set; }

		/**类目id */
		[XmlElement("categoryId")]
			public long?  CategoryId{ get; set; }

		/**类目名称 */
		[XmlElement("categoryName")]
			public string  CategoryName{ get; set; }

		/**0:普通，1：增值 */
		[XmlElement("apiType")]
			public int?  ApiType{ get; set; }

		/**类型名称(普通/增值) */
		[XmlElement("apiTypeName")]
			public string  ApiTypeName{ get; set; }

		/**总调用次数 */
		[XmlElement("totalInvoke")]
			public long?  TotalInvoke{ get; set; }

		/**成功调用次数 */
		[XmlElement("succInvoke")]
			public long?  SuccInvoke{ get; set; }

		/**平均耗时 */
		[XmlElement("avgTimeCost")]
			public long?  AvgTimeCost{ get; set; }

		/**日期（yyyy-MM-dd),按日查询时存在 */
		[XmlElement("countTimeByDay")]
			public string  CountTimeByDay{ get; set; }

		/**日期（yyyy-MM),按月查询时存在 */
		[XmlElement("countTimeByMonth")]
			public string  CountTimeByMonth{ get; set; }

	}
}
