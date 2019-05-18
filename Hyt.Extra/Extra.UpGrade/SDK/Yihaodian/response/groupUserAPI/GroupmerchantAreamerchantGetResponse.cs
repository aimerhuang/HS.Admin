using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.GroupUserAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询集团商家下区域商家信息
	/// </summary>
	public class GroupmerchantAreamerchantGetResponse 
		: YhdResponse 
	{
		/**区域商家列表 */
		[XmlElement("areaMerchantList")]
		public MerchantVOList  AreaMerchantList{ get; set; }

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
