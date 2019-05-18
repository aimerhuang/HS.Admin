using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.SupplierCategory;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 批量获取新品品牌名称列表（含ID）
	/// </summary>
	public class SupplierBrandsIdGetResponse 
		: YhdResponse 
	{
		/**新品品牌名称列表 */
		[XmlElement("brandList")]
		public BrandList  BrandList{ get; set; }

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
