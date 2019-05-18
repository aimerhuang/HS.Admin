using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.MallEhking
{
    public class MEProductReturn
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
        /// 2.商品信息
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品相关信息</remarks>
        public List<GoodsInfo> goodsList
        {
            get;
            set;
        }

        /// <summary>
        /// 3.请求状态
        /// </summary>
        /// <must>非空</must>
        /// <remarks>成功SUCCESS,错误ERROR</remarks>
        public string status
        {
            get;
            set;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <must>可为空</must>
        /// <remarks></remarks>
        public string errorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 参数签名
        /// </summary>
        /// <must>非空</must>
        /// <remarks>获得hmac的方法见签名算法,参数顺序按照表格中顺序,但不包括本参数</remarks>
        public string hmac
        {
            get;
            set;
        }
    }

    public class GoodsInfo
    {
        /// <summary>
        /// 1.商品id
        /// </summary>
        /// <must>非空</must>
        /// <remarks>创建商品返回的id</remarks>
        public string id
        {
            get;
            set;
        }

        /// <summary>
        /// 2.商品编码
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品编码</remarks>
        public string goodsCode
        {
            get;
            set;
        }

        /// <summary>
        /// 3.商品名称
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品编码</remarks>
        public string goodsName
        {
            get;
            set;
        }

        /// <summary>
        /// 4.商品金额
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品金额（单位：分）</remarks>
        public int goodsAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 5.库存数量
        /// </summary>
        /// <must>非空</must>
        /// <remarks>目前商品的剩余数量</remarks>
        public int inventoryCount
        {
            get;
            set;
        }

        /// <summary>
        /// 6.已售数量
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品已经销售的数量</remarks>
        public int sellCount
        {
            get;
            set;
        }

        /// <summary>
        /// 7.商品类别
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品类别（参考文档下方的商品类别说明）</remarks>
        public string goodsType
        {
            get;
            set;
        }

        /// <summary>
        /// 8.商品描述
        /// </summary>
        /// <must>非空</must>
        /// <remarks>配送地址，格式为JSONObject（下单及支付完成通知没有此项）</remarks>
        public string goodsDescribe
        {
            get;
            set;
        }

        /// <summary>
        /// 9.商品状态
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品状态（1：上架0：下架，默认为1）</remarks>
        public string status
        {
            get;
            set;
        }

        /// <summary>
        /// 10.创建时间
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品创建时间（时间格式yyyy-MM-dd HH:mm:ss）</remarks>
        public string createDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 参数签名
        /// </summary>
        /// <must>非空</must>
        /// <remarks>获得hmac的方法见签名算法,参数顺序按照表格中顺序,但不包括本参数</remarks>
        public string hmac
        {
            get;
            set;
        }
    }
}
