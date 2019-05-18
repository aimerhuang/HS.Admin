using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询商家活动列表信息
	/// </summary>
	public class UnionBusinessActivityGetResponse 
		: YhdResponse 
	{
		/**商家活动信息列表 */
		[XmlElement("business_activity_info_outer")]
		public BusinessActivityInfoOuterList  Business_activity_info_outer{ get; set; }

	}
}
