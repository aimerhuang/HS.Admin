using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Qc
{
	/// <summary>
	/// 品牌信息
	/// </summary>
	[Serializable]
	public class QcBrandInfo 
	{
		/**品牌名称 */
		[XmlElement("title")]
			public string  Title{ get; set; }

		/**品牌商标状态 */
		[XmlElement("brandStateTitle")]
			public string  BrandStateTitle{ get; set; }

		/**符合类目 */
		[XmlElement("accordCategory")]
			public string  AccordCategory{ get; set; }

		/**品牌授权结果：0：未通过；1：通过 */
		[XmlElement("certified")]
			public int?  Certified{ get; set; }

		/**授权截止时间 */
		[XmlElement("endDate")]
			public string  EndDate{ get; set; }

		/**备注1。最大1024个字符 */
		[XmlElement("remark1")]
			public string  Remark1{ get; set; }

		/**品牌授权层级数 */
		[XmlElement("brandLevel")]
			public string  BrandLevel{ get; set; }

		/**品牌授权核实链条 */
		[XmlElement("brandType")]
			public string  BrandType{ get; set; }

		/**品牌授权核实过程 */
		[XmlElement("brandCourse")]
			public string  BrandCourse{ get; set; }

		/**特殊资料是否通过。0：未通过；1：通过 */
		[XmlElement("specialCertified")]
			public string  SpecialCertified{ get; set; }

		/**备注2。最大1024个字符 */
		[XmlElement("remark2")]
			public string  Remark2{ get; set; }

	}
}
