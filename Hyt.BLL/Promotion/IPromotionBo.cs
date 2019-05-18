using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 促销业务
    /// </summary>
    /// <remarks>2013-06-26 吴文强 创建</remarks>
    public interface IPromotionBo
    {
        ///// <summary>
        ///// 根据商品分类系统编号获取促销信息
        ///// </summary>
        ///// <param name="categorySysNo">商品分类系统编号</param>
        ///// <returns>应用至商品分类的促销信息</returns>
        ///// <remarks>2013-06-26 吴文强 创建</remarks>
        //List<SpPromotion> GetPromotionByCategory(int categorySysNo);

        ///// <summary>
        ///// 根据商品系统编号获取促销信息
        ///// </summary>
        ///// <param name="productSysNo">商品系统编号</param>
        ///// <returns>应用至商品的促销信息</returns>
        ///// <remarks>2013-06-26 吴文强 创建</remarks>
        //List<SpPromotion> GetPromotionByProduct(int productSysNo);

        ///// <summary>
        ///// 计算购物车
        ///// 根据购物车对象获取商品包含的促销活动和可选购商品
        ///// </summary>
        ///// <param name="customerSysNo">用户系统编号</param>
        ///// <param name="shoppingCart">购物车对象</param>
        ///// <returns>包含促销活动和可选购商品的购物车对象</returns>
        ///// <remarks>2013-06-26 吴文强 创建</remarks>
        //ShoppingCart CalculatePromotion(int? customerSysNo, ShoppingCart shoppingCart);
    }

}
