using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Goods.CIQREC
{
    public class GoodsRegRecList
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "DeclEntNo")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string DeclEntNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "EntGoodsNo")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string EntGoodsNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "EportGoodsNo")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string EportGoodsNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "CIQGoodsNo")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string CIQGoodsNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "CIQGRegStatus")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string CIQGRegStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "CIQNotes")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string CIQNotes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "OpType")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string OpType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "OpTime")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string OpTime { get; set; }
    }
}
