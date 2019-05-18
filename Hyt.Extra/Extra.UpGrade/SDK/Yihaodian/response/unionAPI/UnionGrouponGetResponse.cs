using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询网盟团购信息
	/// </summary>
	public class UnionGrouponGetResponse 
		: YhdResponse 
	{
		/**查询成功记录数 */
		[XmlElement("total_count")]
			public int?  Total_count{ get; set; }

		/**查询失败记录数 */
		[XmlElement("error_count")]
			public int?  Error_count{ get; set; }

		/**错误信息列表 */
		[XmlElement("err_info_list")]
		public ErrDetailInfoList  Err_info_list{ get; set; }

		/**网盟团购信息列表 */
		[XmlElement("union_api_groupon_info_outer_list")]
		public UnionApiGrouponInfoOuterList  Union_api_groupon_info_outer_list{ get; set; }

	}
}
