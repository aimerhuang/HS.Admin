using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.BasicAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取省级市级区级地址列表
	/// </summary>
	public class BasicAreaGetResponse 
		: YhdResponse 
	{
		/**操作成功1或者失败0 */
		[XmlElement("isSuccess")]
			public int?  IsSuccess{ get; set; }

		/**所有省级列表结果 */
		[XmlElement("areaList")]
		public BasicAreaList  AreaList{ get; set; }

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
