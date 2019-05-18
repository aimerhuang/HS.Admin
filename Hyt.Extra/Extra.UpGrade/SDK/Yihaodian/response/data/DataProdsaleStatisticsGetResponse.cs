using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Data;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取产品的销售统计信息
	/// </summary>
	public class DataProdsaleStatisticsGetResponse 
		: YhdResponse 
	{
		/**产品销售统计详细信息 */
		[XmlElement("productStatisticsList")]
		public ProductStatisticsInfoList  ProductStatisticsList{ get; set; }

		/**查询成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**查询失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
