using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.RongEgouModel
{
    /// <summary>
    /// 商品详情查询接口成功返回参数
    /// </summary>
    /// <remarks>2018-03-19 罗熙 创建</remarks>
    public class RegProductlistSelectResultPara
    {
        public Products products { get; set; }
    }


    /// <summary>
    /// 商品主体
    /// </summary>
    public class Products
    {
        public List<Product> product { get; set; }
    }

    /// <summary>
    /// 商品列表
    /// </summary>
    public class Product
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
        /// 商品市场价
        /// </summary>
        public decimal product_market_price { get; set; }

        /// <summary>
        /// 商城商品价格
        /// </summary>
        public decimal product_emall_price { get; set; }

        /// <summary>
        /// 商品库存
        /// </summary>
        public int product_storage { get; set; }

        /// <summary>
        /// 商品扣减方式
        /// </summary>
        public int deduct_mode { get; set; }

        /// <summary>
        /// 商品条形码
        /// </summary>
        public string product_bar_code { get; set; }

        /// <summary>
        /// 商品单位
        /// </summary>
        public string product_unit { get; set; }

        /// <summary>
        /// 商品品牌名称
        /// </summary>
        public string product_brand { get; set; }

        /// <summary>
        /// 商品品牌编号
        /// </summary>
        public string product_brand_id { get; set; }

        /// <summary>
        /// 商品允许购买渠道 （0 不限渠道，1 PC，2 手机，3 平板）
        /// </summary>
        public string buy_channel { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        public string product_status { get; set; }

        /// <summary>
        /// 商户商品编码
        /// </summary>
        public string product_merchant_id { get; set; }

        /// <summary>
        /// 商品重量
        /// </summary>
        public decimal product_weight { get; set; }

        /// <summary>
        /// 商品体积
        /// </summary>
        public decimal product_bulk { get; set; }

        /// <summary>
        /// 上架方式
        /// </summary>
        public string puton_type { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        public string puton_time { get; set; }

        /// <summary>
        /// 是否支持七天无理由退换货
        /// </summary>
        public string is_sev_refund { get; set; }

        /// <summary>
        /// 基本属性列表节点
        /// </summary>
        public Basicproperties basicproperties { get; set; }

        /// <summary>
        /// 商品sku编码列表
        /// </summary>
        public Skuproducts skuproducts { get; set; }

        /// <summary>
        /// 销售属性列表
        /// </summary>
        public Saleproperties saleproperties { get; set; }

        /// <summary>
        /// 搭售商品列表
        /// </summary>
        public Tringproducts tringproducts { get; set; }

        /// <summary>
        /// 逻辑仓列表节点
        /// </summary>
        public Logstors logstors { get; set; }

        /// <summary>
        /// 库存列表节点
        /// </summary>
        public Storages storages { get; set; }
    }

    /// <summary>
    /// 基本属性列表节点
    /// </summary>
    public class Basicproperties
    {
        public List<Basicproperty> basicproperty { get; set; }
    }

    /// <summary>
    /// 基本属性单个节点
    /// </summary>
    public class Basicproperty
    {
        /// <summary>
        /// 基本属性名称
        /// </summary>
        public string basic_prop_name { get; set; }

        /// <summary>
        /// 基本属性值
        /// </summary>
        public string basic_prop_value { get; set; }
    }

    /// <summary>
    /// 商品sku编码列表
    /// </summary>
    public class Skuproducts
    {
        /// <summary>
        /// 单个sku编码节点
        /// </summary>
        public List<Skuproduct> skuproduct { get; set; }
    }

    /// <summary>
    /// 单个sku编码节点
    /// </summary>
    public class Skuproduct
    {
        /// <summary>
        /// 商品sku编码
        /// </summary>
        public string product_sku_id { get; set; }

        /// <summary>
        /// 商户商品编码
        /// </summary>
        public string product_merchant_id { get; set; }

        /// <summary>
        /// 商品sku市场价格
        /// </summary>
        public decimal product_market_price { get; set; }

        /// <summary>
        /// 商品sku商城价格
        /// </summary>
        public decimal product_emall_price { get; set; }

        /// <summary>
        /// 商品库存
        /// </summary>
        public int product_storage { get; set; }

        /// <summary>
        /// 商品条形码
        /// </summary>
        public string product_bar_code { get; set; }
    }

    /// <summary>
    /// 销售属性列表
    /// </summary>
    public class Saleproperties
    {
        /// <summary>
        /// 单个销售属性
        /// </summary>
        public List<Saleproperty> saleproperty { get; set; }
    }

    /// <summary>
    /// 单个销售属性
    /// </summary>
    public class Saleproperty
    {
        /// <summary>
        /// 销售属性编码
        /// </summary>
        public string sale_prop_name_id { get; set; }

        /// <summary>
        /// 销售属性名称
        /// </summary>
        public string sale_prop_name { get; set; }

        /// <summary>
        /// 销售属性值编码
        /// </summary>
        public string sale_prop_value_id { get; set; }

        /// <summary>
        /// 销售属性值
        /// </summary>
        public string sale_prop_value { get; set; }
    }

    /// <summary>
    /// 搭售商品列表
    /// </summary>
    public class Tringproducts
    {
        /// <summary>
        /// 单个搭售商品
        /// </summary>
        public List<Tringproduct> tringproduct { get; set; }
    }

    /// <summary>
    /// 单个搭售商品
    /// </summary>
    public class Tringproduct
    {
        /// <summary>
        /// 搭售商品编码
        /// </summary>
        public string product_sku_id { get; set; }

        /// <summary>
        /// 搭售商品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 商户商品名称
        /// </summary>
        public string product_merchant_id { get; set; }

        /// <summary>
        /// 搭售商品sku商城价格
        /// </summary>
        public decimal product_emall_price { get; set; }

        /// <summary>
        /// 搭售商品sku商城优惠价格
        /// </summary>
        public decimal product_emall_discount_price { get; set; }
    }

    /// <summary>
    /// 逻辑仓列表节点
    /// </summary>
    public class Logstors
    {
        /// <summary>
        /// 逻辑仓单个节点
        /// </summary>
        public List<Logstor> logstor { get; set; }
    }

    /// <summary>
    /// 逻辑仓单个节点
    /// </summary>
    public class Logstor
    {
        /// <summary>
        /// 商品逻辑仓编号
        /// </summary>
        public string logstor_id { get; set; }

        /// <summary>
        /// 商品商户逻辑仓编号
        /// </summary>
        public string logstor_mer_id { get; set; }

    }

    /// <summary>
    /// 库存列表节点
    /// </summary>
    public class Storages
    {
        /// <summary>
        /// 仓库单个节点
        /// </summary>
        public List<Storage> storage { get; set; }
    }

    /// <summary>
    /// 仓库单个节点
    /// </summary>
    public class Storage
    {
        /// <summary>
        /// 商品逻辑仓编号
        /// </summary>
        public string logstor_id { get; set; }

        /// <summary>
        /// 商品sku编码
        /// </summary>
        public string product_sku_id { get; set; }

        /// <summary>
        /// 库存（返回该商品该sku、该逻辑仓的库存，如果共享了其它sku的库存，则默认为0）
        /// </summary>
        public int prod_sku_log_sto { get; set; }

        /// <summary>
        /// 共享库存的skuID
        /// </summary>
        public string share_prod_sku_id { get; set; }
    }
}
