using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZNanSha.CustomsResult.CommodityAudit
{
    public class Declaration
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "GoodsRegRecList")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public GoodsRegRecList GoodsRegRecList { get; set; }
    }
}
