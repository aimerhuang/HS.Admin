using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Data;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询水晶商家数据
	/// </summary>
	public class DataCrystalGetResponse 
		: YhdResponse 
	{
		/**数据详情 */
		[XmlElement("merchantDataList")]
		public MerchantDataList  MerchantDataList{ get; set; }

		/**成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
