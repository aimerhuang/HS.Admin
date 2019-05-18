using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Goods.CUSREC
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
        [XmlElement(ElementName = "CusGoodsNo")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string CusGoodsNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "OpResult")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string OpResult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "CustomsNotes")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string CustomsNotes { get; set; }
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
