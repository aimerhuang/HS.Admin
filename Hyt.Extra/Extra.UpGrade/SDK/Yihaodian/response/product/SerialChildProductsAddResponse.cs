using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 新增系列产品子品
	/// </summary>
	public class SerialChildProductsAddResponse 
		: YhdResponse 
	{
		/**更新成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

		/**更新失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
