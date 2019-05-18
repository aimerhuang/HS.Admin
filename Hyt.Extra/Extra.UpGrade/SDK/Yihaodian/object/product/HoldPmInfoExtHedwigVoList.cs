using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class HoldPmInfoExtHedwigVoList 
	{	
		/// <summary>
		/// 区域商家商品信息，限集团商家
		/// </summary>
		[XmlElement("holdPmInfoExtHedwigVo")]
		public List<HoldPmInfoExtHedwigVo>  HoldPmInfoExtHedwigVo{ get; set; }
	}
}
