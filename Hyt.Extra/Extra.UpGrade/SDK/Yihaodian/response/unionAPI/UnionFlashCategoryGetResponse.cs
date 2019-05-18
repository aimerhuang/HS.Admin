using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询名品类目信息
	/// </summary>
	public class UnionFlashCategoryGetResponse 
		: YhdResponse 
	{
		/**名品类目集合对象 */
		[XmlElement("union_flash_category_list")]
		public UnionFlashCategoryInfoOuterList  Union_flash_category_list{ get; set; }

	}
}
