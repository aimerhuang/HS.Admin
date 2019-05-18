using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品价格维护数据接口
    /// </summary>
    /// <remarks>2013-06-26 邵斌 创建</remarks>
    public abstract class IPdPriceDao : DaoBase<IPdPriceDao>
    {

        /// <summary>
        /// 创建商品价格信息
        /// </summary>
        /// <param name="model">价格信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2013-06-27 黄波 创建</remarks>
        public abstract int Create(PdPrice model);

        /// <summary>
        /// 根据价格编号获取商品价格详细信息
        /// </summary>
        /// <param name="sysNo">价格系统编号</param>
        /// <returns>价格详细信息</returns>
        /// <remarks>2013-06-27 黄波 创建</remarks>
        public abstract PdPrice GetPrice(int sysNo);

        /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2016-06-28 王耀发 创建</remarks>
        public abstract IList<PdPrice> GetProductPriceByStatus(int productSysNo);

        /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        public abstract IList<PdPrice> GetProductPrice(int productSysNo);

        /// <summary>
        /// 获取价格来源类型列表
        /// </summary>
        /// <returns>价格来源类型列表</returns>
        /// <remarks>2013-07-17 黄波 创建</remarks>
        public abstract IList<PdPriceType> GetPriceTypeItems();

        /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="priceType">用于显示的价格类型</param>
        /// <param name="status">价格状态</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2016-09-12 杨浩 创建</remarks>
        public abstract IList<PdPrice> GetProductPrice(int productSysNo, int priceType, int status);

        /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="priceType">用于显示的价格类型</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        public abstract IList<PdPrice> GetProductPrice(int productSysNo, ProductStatus.产品价格来源 priceType);
         /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="priceType">用于显示的价格类型</param>
        /// <param name="status">价格状态</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2016-09-12 杨浩 创建</remarks>
        public abstract IList<PdPrice> GetProductPrices(int productSysNo, int priceType);
        /// <summary>
        /// 获取指定商品的会员等级商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>        
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2013-07-17 郑荣华 创建</remarks>
        public abstract IList<CBPdPrice> GetProductLevelPrice(int productSysNo);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract int Update(PdPrice model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sysNo">价格系统编码</param>
        /// <param name="price">新价格</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract int Update(int sysNo, decimal price);

        /// <summary>
        /// 更新单个商品价格状态
        /// </summary>
        /// <param name="priceSysNo">商品价格系统编号</param>
        /// <param name="status">商品状态</param>
        /// <returns>成功 true 失败 false</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract bool UpdatePriceStatus(int priceSysNo, ProductStatus.产品价格状态 status);

        /// <summary>
        /// 获取指定商品的分销商等级商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="sourceSysNo">来源系统编号</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2013-09-17 周瑜 创建</remarks>
        public abstract IList<CBPdPrice> GetDealerProductLevelPrice(int productSysNo, int sourceSysNo);
        /// <summary>
        /// 2015-12-31 王耀发 创建
        /// </summary>
        /// <param name="ProductSysNo">商品编号</param>
        /// <param name="PriceSource">价格源</param>
        /// <returns></returns>
        public abstract PdPrice GetSalesPrice(int ProductSysNo, int PriceSource);
        /// <summary>
        /// 获取商品的价格
        /// </summary>
        /// <param name="productSysNoList"></param>
        /// <param name="PriceSource"></param>
        /// <returns></returns>
        public abstract List<PdPrice> GetProductPrices(List<int> productSysNoList, int PriceSource);
        /// <summary>
        /// 删除产品价格
        /// </summary>
        /// <param name="priceSysNoList">价格系统编号列表</param>
        /// <returns></returns>
        /// <remarks>2017-3-11 杨浩 创建</remarks>
        public abstract bool DeleltePrice(string priceSysNoList);
        /// <summary>
        /// 删除产品重复的价格
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-3-11 杨浩 创建</remarks>
        public abstract bool DeleleRepeatPrice(int productSysNo);
    }
}
