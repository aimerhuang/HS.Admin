using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Promotion;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.CRM;

namespace Hyt.BLL.Cart
{
    /// <summary>
    /// 订单编辑购物车数据处理
    /// </summary>
    /// <remarks>
    /// 2013-11-14 杨文兵 创建
    /// </remarks>
    public class EditOrderCart
    {
        /// <summary>
        /// Json格式的购物车明细
        /// </summary>
        private IList<JsonCartItem> _jsonCartItemList = new List<JsonCartItem>();
        ///// <summary>
        ///// 订单明细调价数据
        ///// </summary>
        //private Dictionary<int,decimal> _dictChangeAmount = new Dictionary<int,decimal>();

        private int _customerSysNo;     //会员SysNo.
        private int _areaSysNo;         //收货地址区域
        private int _deliveryTypeSysNo; //配送方式
        private string _couponCode;     //优惠券Code
        private int[] _usedPromotionSysNo; // 订单促销使用的系统编号集合
        private SoOrder _order; // 当前编辑的订单

        ///  <summary>
        ///  订单编辑购物车数据处理
        ///  </summary>
        ///  <param name="customerSysNo">会员SysNo.</param>
        ///  <param name="jsonCartItemList">购物车明细.</param>
        ///  <param name="areaSysNo">区域SysNo.</param>
        ///  <param name="deliveryTypeSysNo">配送方式SysNo.</param>
        ///  <param name="couponCode">优惠券SysNo.</param>
        ///  <param name="usedPromotionSysNo">订单促销使用的系统编号集合.</param>
        /// <param name="order">编辑的订单</param>
        /// <returns></returns>
        /// <remarks>2013-11-14 杨文兵 创建</remarks>
        public EditOrderCart(int customerSysNo, IList<JsonCartItem> jsonCartItemList, int areaSysNo, int deliveryTypeSysNo, string couponCode, int[] usedPromotionSysNo, SoOrder order)
        {
            _customerSysNo = customerSysNo;
            foreach (var item in jsonCartItemList)
            {
                _jsonCartItemList.Add(item);
            }
            _customerSysNo = customerSysNo;
            _areaSysNo = areaSysNo;
            _deliveryTypeSysNo = deliveryTypeSysNo;
            _couponCode = couponCode;
            _usedPromotionSysNo = usedPromotionSysNo;
            _order = order;
        }

        /// <summary>
        /// 订单编辑购物车数据处理
        /// </summary>
        /// <param name="customerSysNo">会员SysNo.</param>
        /// <param name="jsonCartItem">Json格式的购物车明细</param>
        /// <param name="areaSysNo">区域SysNo.</param>
        /// <param name="deliveryTypeSysNo">配送方式SysNo.</param>
        /// <param name="couponCode">优惠券SysNo.</param>
        /// <param name="usedPromotionSysNo">订单促销使用的系统编号集合.</param>
        /// <param name="order">编辑的订单</param>
        /// <exception cref="System.ArgumentException">Json格式的购物车明细 数据格式错误</exception>
        ///<returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks>
        public EditOrderCart(int customerSysNo, string jsonCartItem, int areaSysNo, int deliveryTypeSysNo, string couponCode, int[] usedPromotionSysNo, SoOrder order)
        {
            try
            {
                foreach (var item in Hyt.Util.Serialization.JsonUtil.ToObject<IList<JsonCartItem>>(jsonCartItem))
                {
                    _jsonCartItemList.Add(item);
                }
            }
            catch
            {
                throw new ArgumentException("Json格式的购物车明细 数据格式错误");
            }
            _customerSysNo = customerSysNo;
            _areaSysNo = areaSysNo;
            _deliveryTypeSysNo = deliveryTypeSysNo;
            _couponCode = couponCode;
            _usedPromotionSysNo = usedPromotionSysNo;
            _order = order;
        }

        /// <summary>
        /// 设置会员SysNo
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks>
        public void SetCustomerSysNo(int customerSysNo)
        {
            _customerSysNo = customerSysNo;
        }

        /// <summary>
        /// 设置区域SysNo
        /// </summary>
        /// <param name="areaSysNo"></param>
        /// <returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks>
        public void SetAreaSysNo(int areaSysNo)
        {
            _areaSysNo = areaSysNo;
        }

        /// <summary>
        /// 设置购物车的配送方式
        /// </summary>
        /// <param name="deliveryTypeSysNo"></param>
        ///<returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks>
        public void SetDeliveryTypeSysNo(int deliveryTypeSysNo)
        {
            _deliveryTypeSysNo = deliveryTypeSysNo;
        }

        /// <summary>
        /// 设置购物车使用的优惠券
        /// </summary>
        /// <param name="couponCode"></param>
        ///<returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks>
        public void SetCoupon(string couponCode)
        {
            _couponCode = couponCode;
        }

        /// <summary>
        /// 获取JsonCartItem
        /// </summary>
        /// <param name="sysno">sysno</param>
        /// <returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks>
        public JsonCartItem GetJsonCartItem(int sysno)
        {
            return _jsonCartItemList.FirstOrDefault(p => p.SysNo == sysno);
        }

        /// <summary>
        /// 添加商品至购物车
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks> 
        public JsonCartItem Add(int productSysNo, int quantity, CustomerStatus.购物车商品来源 source)
        {
            if (quantity < 1)
            {
                _jsonCartItemList = _jsonCartItemList.Where(p => p.ProductSysNo == productSysNo
                        && p.ProductSalesType == (int)CustomerStatus.商品销售类型.普通
                    ).ToList();
                return null;
            }

            var productItems = _jsonCartItemList.FirstOrDefault(o => o.ProductSysNo == productSysNo
                    && o.ProductSalesType == (int)CustomerStatus.商品销售类型.普通
                );
            if (productItems != null)
            {
                productItems.Quantity += quantity;
                return productItems;
            }
            else
            {
                productItems = new JsonCartItem
                {
                    //SysNo = CreateSysNo(),  //购物车对象之间互相转换时该属性值不能保持一致，不再依赖此属性做任何逻辑处理
                    GroupCode = "",
                    IsLock = (int)CustomerStatus.购物车是否锁定.否,
                    IsChecked = (int)CustomerStatus.是否选中.是,
                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                    ProductSysNo = productSysNo,
                    Promotions = "",
                    Quantity = quantity
                };
                _jsonCartItemList.Add(productItems);
                return productItems;
            }
        }

        /// <summary>
        /// 添加 组 至购物车
        /// </summary>
        /// <param name="groupCode">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns>购物车明细集合</returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks> 
        public void Add(string groupCode, int quantity, string promotionSysNo, CustomerStatus.购物车商品来源 source)
        {
            if (quantity < 1)
            {
                Remove(groupCode, promotionSysNo);
                return;
            }

            var item = _jsonCartItemList.FirstOrDefault(o => o.GroupCode == groupCode && o.Promotions == promotionSysNo);
            if (item == null)
            {
                _jsonCartItemList.Add(new JsonCartItem()
                {
                    //SysNo = CreateSysNo(), 该字段对于组而言没有任何意义
                    GroupCode = groupCode,
                    IsLock = (int)CustomerStatus.购物车是否锁定.是,
                    IsChecked = (int)CustomerStatus.是否选中.是,
                    ProductSysNo = 0,
                    Promotions = promotionSysNo,
                    Quantity = quantity
                });
            }
            else
            {
                foreach (var jsonCartItem in _jsonCartItemList)
                {
                    if (jsonCartItem.GroupCode == groupCode && jsonCartItem.Promotions == promotionSysNo)
                    {
                        jsonCartItem.Quantity++;
                    }
                }
            }

        }

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="productSysNo">商品SysNo</param>
        /// <param name="quantity">商品数量</param>
        /// <returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks> 
        public void UpdateQuantity(int productSysNo, int quantity)
        {
            if (quantity < 1)
            {
                Remove(productSysNo);
                return;
            }

            var item = _jsonCartItemList.FirstOrDefault(o => o.ProductSysNo == productSysNo
                && o.ProductSalesType == (int)CustomerStatus.商品销售类型.普通);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        ///<returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks> 
        public void UpdateQuantity(string groupCode, string promotionSysNo, int quantity)
        {
            if (quantity < 1)
            {
                Remove(groupCode, promotionSysNo);
                return;
            }

            foreach (var item in _jsonCartItemList)
            {
                if (item.GroupCode == groupCode && item.Promotions == promotionSysNo)
                {
                    item.Quantity = quantity;
                }
            }
        }

        /// <summary>
        /// 删除普通商品购物车明细
        /// </summary>
        /// <param name="productSysNo">普通商品SysNo</param>
        ///<returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks>  
        public void Remove(int productSysNo)
        {
            var item = _jsonCartItemList.FirstOrDefault(p => p.ProductSysNo == productSysNo
                && p.ProductSalesType == (int)CustomerStatus.商品销售类型.普通);
            if (item != null)
            {
                _jsonCartItemList.Remove(item);
            }
        }

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        ///<returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks> 
        public void Remove(string groupCode, string promotionSysNo)
        {
            var newList = new List<JsonCartItem>();

            foreach (var item in _jsonCartItemList)
            {
                if (item.GroupCode == groupCode && item.Promotions == promotionSysNo)
                {
                    //_jsonCartItemList.Remove(item);
                }
                else
                {
                    newList.Add(item);
                }
            }
            _jsonCartItemList = newList;
        }

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks> 
        public void AddGift(int productSysNo, string promotionSysNo, CustomerStatus.购物车商品来源 source)
        {
            var giftItem = _jsonCartItemList.FirstOrDefault(o =>
                                    o.ProductSysNo == productSysNo
                                    && o.Promotions == promotionSysNo.ToString()
                                    && o.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品
                                    );
            if ((giftItem == null))
            {
                _jsonCartItemList.Add(new JsonCartItem
                {
                    //SysNo = CreateSysNo(),  对修改订单功能无用的字段
                    GroupCode = "",
                    IsLock = (int)CustomerStatus.购物车是否锁定.是,
                    IsChecked = (int)CustomerStatus.是否选中.是,
                    ProductSalesType = (int)CustomerStatus.商品销售类型.赠品,
                    ProductSysNo = productSysNo,
                    Promotions = promotionSysNo,
                    Quantity = 1
                });
            }
        }

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks> 
        public void RemoveGift(int productSysNo, string promotionSysNo)
        {
            var item = _jsonCartItemList.FirstOrDefault(p => p.ProductSysNo == productSysNo
                        && p.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品
                        && p.Promotions == promotionSysNo
                    );
            if (item != null)
            {
                _jsonCartItemList.Remove(item);
            }
        }

        /// <summary>
        /// 转换成CrShoppingCart(购物车)对象
        /// </summary>
        /// <param name="platformType">促销使用平台</param>
        /// <param name="expensesAmount">太平洋保险</param>
        /// <returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks> 
        public CrShoppingCart ToCrShoppingCart(PromotionStatus.促销使用平台[] platformType, decimal expensesAmount = 0M)
        {
            var customer = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(_customerSysNo);
            var cartItemList = _jsonCartItemList.ConvertShoppingCartItems(customer.LevelSysNo); ;

            var promotionToPython = new SpPromotionToPython()
                {
                    Order = _order
                };

            return Hyt.BLL.CRM.CrShoppingCartBo.Instance.GetShoppingCart(platformType, customer, cartItemList
                                                                         , _usedPromotionSysNo
                                                                         , _couponCode
                                                                         , false
                                                                         , _areaSysNo, _deliveryTypeSysNo, null, false,
                                                                         promotionToPython,_order.DefaultWarehouseSysNo,expensesAmount);

        }

        ///// <summary>
        ///// 转换成CBCrShoppingCartItem(购物车明细)列表
        ///// </summary>
        //private IList<CBCrShoppingCartItem> ToCBCrShoppingCartItem()
        //{
        //    return //TODO:1客户等级系统编号
        //}

        /// <summary>
        /// 生成临时系统编号
        /// </summary>
        /// <returns></returns>
        ///<remarks>2013-11-14 杨文兵 创建</remarks> 
        private int CreateSysNo()
        {
            var sysNo = 1;
            while (_jsonCartItemList.Any(o => o.SysNo == sysNo))
            {
                sysNo = new Random().Next(0, 10000);
            }
            return sysNo;
        }

    }
}
