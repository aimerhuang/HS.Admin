using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.ExpressList
{

    /// <summary>
    /// 快递100接口 返回数据类
    /// </summary>
    ///<remarks>2017-11-15 廖移凤 创建</remarks>
  [Serializable]
    public  class KdOrderNums
    {

      
      /// <summary>
      /// 编号
      /// </summary>
        public int sysNo { get; set; }
      /// <summary>
        /// 快递业务类型编码
      /// </summary>
        public string expressCode { get; set; }
      /// <summary>
      /// 
      /// </summary>
        public string payaccount { get; set; }
      /// <summary>
        /// 目的地区域编码
      /// </summary>
        public string destCode { get; set; }
      /// <summary>
        /// 条形码
      /// </summary>
        public string kuaidinum { get; set; }
      /// <summary>
        /// 快递单号
      /// </summary>
        public string kdOrderNum { get; set; }
      /// <summary>
        /// 目的分栋编码
      /// </summary>
        public string destSortingCode { get; set; }

      /// <summary>
        /// 网址（电子面单）
      /// </summary>
        public string templateurl { get; set; }
 

    }
}
