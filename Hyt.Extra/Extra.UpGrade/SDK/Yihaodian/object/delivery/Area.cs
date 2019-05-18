using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Delivery
{
	/// <summary>
	/// 地址区域结构
	/// </summary>
	[Serializable]
	public class Area 
	{
		/**标准行政区域代码.参考:http://www.stats.gov.cn/tjsj/tjbz/xzqhdm/ */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/** 	区域类型.area区域 1:country/国家;2:province/省/自治区/直辖市;3:city/地区(省下面的地级市);4:district/县/市(县级市)/区;abroad:海外. 比如北京市的area_type = 2,朝阳区是北京市的一个区,所以朝阳区的area_type = 4. */
		[XmlElement("type")]
			public int?  Type{ get; set; }

		/**地域名称.如北京市,杭州市,西湖区,每一个area_id 都代表了一个具体的地区. */
		[XmlElement("name")]
			public string  Name{ get; set; }

		/**父节点区域标识.如北京市的area_id是110100,朝阳区是北京市的一个区,所以朝阳区的parent_id就是北京市的area_id. */
		[XmlElement("parent_id")]
			public long?  Parent_id{ get; set; }

		/**具体一个地区的邮编 */
		[XmlElement("zip")]
			public string  Zip{ get; set; }

	}
}
