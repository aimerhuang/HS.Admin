using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Goods.CIQREC
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
        public List<GoodsRegRecList> GoodsRegRecList { get; set; }
    }
}
