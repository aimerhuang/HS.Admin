using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 购物车明细数
    /// </summary>
    /// <remarks>2013-08-16 吴文强 创建</remarks>
    public abstract class ICrShoppingCartItemDao : DaoBase<ICrShoppingCartItemDao>
    {

        /// <summary>
        /// 添加商品至购物车
        /// </summary>
        /// <param name="shoppingCartItems">购物车集合</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-08-16 吴文强 创建</remarks>
        public abstract List<CrShoppingCartItem> Add(List<CrShoppingCartItem> shoppingCartItems);

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract void Delete(int customerSysNo, int[] sysNo);

         /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract void DeleteByProductSysNo(int customerSysNo, int[] sysNo);

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isCheckedItem">是否是选择的明细,true:删除选中明细;false:删除该用户全部明细</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract void Delete(int customerSysNo, bool isCheckedItem);

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract void Delete(int customerSysNo, string groupCode, string promotionSysNo);

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract void Delete(int customerSysNo, int productSysNo, int promotionSysNo);

        /// <summary>
        /// 获取购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认查询全部</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract IList<CBCrShoppingCartItem> GetShoppingCartItems(int customerSysNo, int[] sysNo,
                                                                         bool isChecked = false);

        /// <summary>
        /// 获取购物车已选择赠品明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public abstract IList<CBCrShoppingCartItem> GetShoppingCartGiftItems(int customerSysNo);

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isChecked">是否选中(CustomerStatus.是否选中)</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public abstract void UpdateCheckedItem(int customerSysNo, string groupCode, string promotionSysNo, CustomerStatus.是否选中 isChecked);

        /// <summary>
        /// 根据顾客ID和产品Id查询购物车
        /// </summary>
        /// <param name="customerSysNo">The customer sys no.</param>
        /// <param name="productSysNo">The product sys no.</param>
        /// <returns></returns>
        /// <remarks>2013-08-05 唐永勤 创建</remarks>
        public abstract CrShoppingCart GetShoppingCart(int customerSysNo, int productSysNo);

        /// <summary>
        /// 查询顾客的购物车
        /// </summary>
        /// <param name="pager">购物车查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-08-05 唐永勤 创建</remarks>
        public abstract void GetPage(ref Pager<CrShoppingCartItem> pager);

        /// <summary>
        /// 选择/取消选择购物车所有明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isChecked">是否选中(CustomerStatus.是否选中)</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public abstract void UpdateCheckedItem(int customerSysNo, CustomerStatus.是否选中 isChecked);

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <param name="isChecked">是否选中(CustomerStatus.是否选中)</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public abstract void UpdateCheckedItem(int customerSysNo, int[] sysNo, CustomerStatus.是否选中 isChecked);

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public abstract void UpdateQuantity(int customerSysNo, int[] sysNo, int quantity);

        /// <summary>
        /// 更新购物车组商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public abstract void UpdateQuantity(int customerSysNo, string groupCode, string promotionSysNo, int quantity);

        /// <summary>
        /// 根据有效赠品移除无效赠品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="currContainSysNo">有效赠品列表</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public abstract void RemoveInvalidGift(int customerSysNo, int[] currContainSysNo);
    }
}
