using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.MallEhking
{
    public class MEProductSearch
    {
        /// <summary>
        /// 1.商户编号
        /// </summary>
        /// <must>非空</must>
        /// <remarks>调用接口的商户所对应的商户号，商户在易汇金系统的唯一身份标识，商户完成易汇金系统注册后可登录商户后台商户服务查看。</remarks>
        public string merchantId
        {
            get;
            set;
        }

        /// <summary>
        /// 2.商户编号
        /// </summary>
        /// <must>可为空</must>
        /// <remarks>该商品所对应的商户号，如为空则使用merchantId</remarks>
        public string customerId
        {
            get;
            set;
        }

        /// <summary>
        /// 3.商品id
        /// </summary>
        /// <must>可为空</must>
        /// <remarks>上传商品时返回的id</remarks>
        public string goodsId
        {
            get;
            set;
        }

        /// <summary>
        /// 4.商品编码
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品编码</remarks>
        public string goodsCode
        {
            get;
            set;
        }

        /// <summary>
        /// 5.开始日期
        /// </summary>
        /// <must>可为空</must>
        /// <remarks>查询数据的开始时间，可以根据开始日期和结束日期进行批量查询（时间格式yyyy-MM-dd HH:mm:ss）</remarks>
        public string startDate
        {
            get;
            set;
        }

        /// <summary>
        /// 6.结束日期
        /// </summary>
        /// <must>可为空</must>
        /// <remarks>查询数据的结束时间，可以根据开始日期和结束日期进行批量查询（时间格式yyyy-MM-dd HH:mm:ss）</remarks>
        public string endDate
        {
            get;
            set;
        }

        /// <summary>
        /// 参数签名
        /// </summary>
        /// <must>非空</must>
        /// <remarks>使用商户密钥进行签名，获得hmac的方法见签名算法,参数顺序按照表格中顺序,但不包括本参数</remarks>
        public string hmac
        {
            get;
            set;
        }
    }
}
