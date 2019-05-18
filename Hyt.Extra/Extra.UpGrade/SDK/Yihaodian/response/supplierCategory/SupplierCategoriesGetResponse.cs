using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.SupplierCategory;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 批量获取新品类别列表
	/// </summary>
	public class SupplierCategoriesGetResponse 
		: YhdResponse 
	{
		/**新品类别名称列表 */
		[XmlElement("categoryList")]
		public CategoryList  CategoryList{ get; set; }

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
