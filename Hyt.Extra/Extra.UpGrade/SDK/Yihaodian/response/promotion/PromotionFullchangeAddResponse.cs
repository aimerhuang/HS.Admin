using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 新增满就换购促销
	/// </summary>
	public class PromotionFullchangeAddResponse 
		: YhdResponse 
	{
		/**成功为促销的id值，失败无此字段 */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**更新成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

		/**查询失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
