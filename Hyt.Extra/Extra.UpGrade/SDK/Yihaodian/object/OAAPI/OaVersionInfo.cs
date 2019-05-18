using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.OAAPI
{
	/// <summary>
	/// OA版本信息
	/// </summary>
	[Serializable]
	public class OaVersionInfo 
	{
		/**OA APP的版本号 */
		[XmlElement("oaVer")]
			public string  OaVer{ get; set; }

		/**是否强制升级。1：是， 0：否 */
		[XmlElement("isMustUpdate")]
			public int?  IsMustUpdate{ get; set; }

		/**版本描述 */
		[XmlElement("versionDesc")]
			public string  VersionDesc{ get; set; }

	}
}
