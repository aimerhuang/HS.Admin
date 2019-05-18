using Hyt.BLL.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Cart
{
    /// <summary>
    /// 数据库购物车操作
    /// </summary>
    /// <remarks>2013-10-16 黄波 创建</remarks>
    public class DataBaseCart : ICart
    {
        private int _customerSysNo;
        /// <summary>
        /// 创建用于操作DataBaseCart的新实例
        /// </summary>
        /// <param name="customerSysNo">用户系统编号</param>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public DataBaseCart(int customerSysNo)
        {
            _customerSysNo = customerSysNo;
        }

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override Model.CrShoppingCart GetShoppingCart(bool isChecked = false)
        {
            return CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.PC商城 }, _customerSysNo, isChecked);
        }

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
        public override Model.CrShoppingCart GetShoppingCart(int[] sysNo, int? areaSysNo, int? deliveryTypeSysNo, string promotionCode, string couponCode, bool isChecked = false)
        {
            return CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.PC商城 }, _customerSysNo, sysNo, areaSysNo, deliveryTypeSysNo, promotionCode, couponCode, isChecked);
        }

        /// <summary>
        /// 添加商品至购物车
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="source">购物车商品来源</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void Add(int productSysNo, int quantity, Model.WorkflowStatus.CustomerStatus.购物车商品来源 source)
        {
            CrShoppingCartBo.Instance.Add(_customerSysNo, productSysNo, quantity, source);
        }

        /// <summary>
        /// 添加促销商品至购物车
        /// </summary>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void Add(int groupSysNo, int quantity, int promotionSysNo, Model.WorkflowStatus.CustomerStatus.购物车商品来源 source)
        {
            CrShoppingCartBo.Instance.Add(_customerSysNo, groupSysNo, quantity, promotionSysNo, source);
        }

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UpdateQuantity(int[] sysNo, int quantity)
        {
            CrShoppingCartBo.Instance.UpdateQuantity(_customerSysNo, sysNo, quantity);
        }

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UpdateQuantity(string groupCode, string promotionSysNo, int quantity)
        {
            CrShoppingCartBo.Instance.UpdateQuantity(_customerSysNo, groupCode, promotionSysNo, quantity);
        }

        /// <summary>
        /// 选择购物车所有明细项目
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void CheckedAll()
        {
            CrShoppingCartBo.Instance.CheckedAll(_customerSysNo);
        }

        /// <summary>
        /// 取消选择购物车所有明细项目
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UncheckedAll()
        {
            CrShoppingCartBo.Instance.UncheckedAll(_customerSysNo);
        }

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void CheckedItem(int[] itemSysNo)
        {
            CrShoppingCartBo.Instance.CheckedItem(_customerSysNo, itemSysNo);
        }

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UncheckedItem(int[] itemSysNo)
        {
            CrShoppingCartBo.Instance.UncheckedItem(_customerSysNo, itemSysNo);
        }

        /// <summary>
        /// 选择购物车组明细项目
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void CheckedItem(string groupCode, string promotionSysNo)
        {
            CrShoppingCartBo.Instance.CheckedItem(_customerSysNo, groupCode, promotionSysNo);
        }

        /// <summary>
        /// 取消选择购物车组明细项目
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UncheckedItem(string groupCode, string promotionSysNo)
        {
            CrShoppingCartBo.Instance.UncheckedItem(_customerSysNo, groupCode, promotionSysNo);
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void Remove(int[] sysNo)
        {
            CrShoppingCartBo.Instance.Remove(_customerSysNo, sysNo);
        }

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void Remove(string groupCode, string promotionSysNo)
        {
            CrShoppingCartBo.Instance.Remove(_customerSysNo, groupCode, promotionSysNo);
        }

        /// <summary>
        /// 删除购物车选中的明细
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void RemoveCheckedItem()
        {
            CrShoppingCartBo.Instance.RemoveCheckedItem(_customerSysNo);
        }

        /// <summary>
        /// 删除购物车所有明细
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void RemoveAll()
        {
            CrShoppingCartBo.Instance.RemoveAll(_customerSysNo);
        }

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void AddGift(int productSysNo, int promotionSysNo, Model.WorkflowStatus.CustomerStatus.购物车商品来源 source)
        {
            CrShoppingCartBo.Instance.AddGift(_customerSysNo, productSysNo, promotionSysNo, source);
        }

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void RemoveGift(int productSysNo, int promotionSysNo)
        {
            CrShoppingCartBo.Instance.RemoveGift(_customerSysNo, productSysNo, promotionSysNo);
        }
    }
}
