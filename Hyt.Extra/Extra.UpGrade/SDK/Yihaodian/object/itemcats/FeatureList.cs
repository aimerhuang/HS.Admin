using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Itemcats
{
	[Serializable]
	public class FeatureList 
	{	
		/// <summary>
		/// Feature对象列表 目前已有的属性： 若Attr_key为 udsaleprop，attr_value为1 则允许卖家在改类目新增自定义销售属性,不然为不允许（暂不提供）
		/// </summary>
		[XmlElement("feature")]
		public List<Feature>  Feature{ get; set; }
	}
}
