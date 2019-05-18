using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.RongEgouModel
{
    /// <summary>
    /// 商品列表查询接口成功返回参数（N为非必填）
    /// </summary>
    /// <remarks>2018-03-19 罗熙 创建</remarks>
    public class RegProductSelectResultPara
    {
        public PdProducts products { get; set; }
    }

    /// <summary>
    /// 商品主体
    /// </summary>
    public class PdProducts
    {
        public List<PdProduct> product { get; set; }
    }

    /// <summary>
    /// 商品列表
    /// </summary>
    public class PdProduct
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        public string product_status { get; set; }

        /// <summary>
        /// 商户商品编码
        /// </summary>
        public string product_merchant_id { get; set; }
    }
}
