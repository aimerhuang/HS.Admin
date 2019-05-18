using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.MallEhking
{
    public class MEProductAdd
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
        /// 3.商品名
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品名称</remarks>
        public string goodsName
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
        /// 5.商品价格
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品价格（单位：分）</remarks>
        public int goodsAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 6.商品类别
        /// </summary>
        /// <must>非空</must>
        /// <remarks>商品类别（参考下面的商品类别说明）</remarks>
        public string goodsType
        {
            get;
            set;
        }

        /// <summary>
        /// 7.库存数量
        /// </summary>
        /// <must>非空</must>
        /// <remarks>库存</remarks>
        public int inventoryCount
        {
            get;
            set;
        }

        /// <summary>
        /// 8.商品状态
        /// </summary>
        /// <must>可为空</must>
        /// <remarks>商品状态（1：上架0：下架，默认为1）</remarks>
        public int status
        {
            get;
            set;
        }

        /// <summary>
        /// 商品描述
        /// </summary>
        /// <must>可为空</must>
        /// <remarks>商品描述</remarks>
        public string goodsDescribe
        {
            get;
            set;
        }

        /// <summary>
        /// 图片列表
        /// </summary>
        /// <must>可为空</must>
        /// <remarks>商品图片，最多12张，显示顺序与上传顺序一致</remarks>
        public List<MEProductAddPicture> pictureList
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

    /// <summary>
    /// 图片信息
    /// </summary>
    public class MEProductAddPicture
    {
        /// <summary>
        /// 图片
        /// </summary>
        /// <must>非空</must>
        /// <remarks>每个图片流通过base64编码（最多12张,单个图片最大3M，显示顺序与上传顺序一致）</remarks>
        public string baseCode
        {
            get;
            set;
        }

        /// <summary>
        /// 图片类型
        /// </summary>
        /// <must>非空</must>
        /// <remarks></remarks>
        public string fileType
        {
            get;
            set;
        }
    }
}
