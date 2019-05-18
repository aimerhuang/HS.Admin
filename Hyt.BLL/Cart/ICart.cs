using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Cart
{
    /// <summary>
    /// 购物车业务
    /// </summary>
    /// <remarks>2013-10-16 黄波 创建</remarks>
    public abstract class ICart
    {
        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract CrShoppingCart GetShoppingCart(bool isChecked = false);

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="sysNo">购物车明细系统编号(null:购物车为服务器选中的对象)</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract CrShoppingCart GetShoppingCart(int[] sysNo, int? areaSysNo, int? deliveryTypeSysNo, string promotionCode, string couponCode, bool isChecked = false);

        /// <summary>
        /// 添加商品至购物车
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="source">购物车商品来源</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void Add(int productSysNo, int quantity, CustomerStatus.购物车商品来源 source);

        /// <summary>
        /// 添加促销商品至购物车
        /// </summary>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void Add(int groupSysNo, int quantity, int promotionSysNo, CustomerStatus.购物车商品来源 source);

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void UpdateQuantity(int[] sysNo, int quantity);

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void UpdateQuantity(string groupCode, string promotionSysNo, int quantity);

        /// <summary>
        /// 选择购物车所有明细项目
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void CheckedAll();

        /// <summary>
        /// 取消选择购物车所有明细项目
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void UncheckedAll();

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void CheckedItem(int[] itemSysNo);

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void UncheckedItem(int[] itemSysNo);

        /// <summary>
        /// 选择购物车组明细项目
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void CheckedItem(string groupCode, string promotionSysNo);

        /// <summary>
        /// 取消选择购物车组明细项目
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void UncheckedItem(string groupCode, string promotionSysNo);

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void Remove(int[] sysNo);

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void Remove(string groupCode, string promotionSysNo);

        /// <summary>
        /// 删除购物车选中的明细
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void RemoveCheckedItem();

        /// <summary>
        /// 删除购物车所有明细
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void RemoveAll();

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void AddGift(int productSysNo, int promotionSysNo, CustomerStatus.购物车商品来源 source);

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public abstract void RemoveGift(int productSysNo, int promotionSysNo);

    }
}
