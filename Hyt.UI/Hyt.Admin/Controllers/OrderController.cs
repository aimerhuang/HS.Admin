using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Extra.Erp.Model;
using Hyt.BLL.Authentication;
using Hyt.BLL.Basic;
using Hyt.BLL.Log;
using Hyt.BLL.MallSeller;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Admin.Models;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Order;
using Hyt.Model.Parameter;
using Hyt.Util;
using Hyt.BLL.CRM;
using Hyt.Model.Common;
using Hyt.Model.SystemPredefined;
using Result = Hyt.Model.Result;
using Hyt.BLL.Cart;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Hyt.Model.Transfer;
using System.Web.Script.Serialization;
using Hyt.BLL.Product;
using Hyt.BLL.Distribution;
using LitJson;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.Text;
using System.Net;
using Hyt.Model.Order;
using Hyt.Admin;
using Hyt.Model.Generated;
using Hyt.Model.LiJiaModel;
using Extra.UpGrade.UpGrades;
using Hyt.BLL.Logistics;
//using Hyt.Model.DouShabaoModel;
namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 订单Controller层
    /// </summary>
    /// <remarks>2013－06-27 黄志勇 创建</remarks>
    public class OrderController : BaseController
    {



        #region 订单创建

        /// <summary>
        /// 客服下单
        /// </summary>
        /// <param name="orderComplex">销售单复合实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013－06-27 黄志勇 创建</remarks>
        [HttpPost]
        [ValidateInput(false)]
        [Privilege(PrivilegeCode.SO1002201)]
        public JsonResult SaveSoOrder(OrderComplex orderComplex)
        {
            try
            {
                var soOrder = orderComplex.SoOrder;
                var soReceiveAddress = orderComplex.SoReceiveAddress;
                SoOrderItem[] product = orderComplex.Product;
                var invoice = orderComplex.Invoice;
                if (invoice.InvoiceTypeSysNo == 0)
                {
                    invoice = null;
                }
                else
                {
                    invoice.CreatedBy = CurrentUser.Base.SysNo;
                    invoice.CreatedDate = DateTime.Now;
                    invoice.LastUpdateBy = CurrentUser.Base.SysNo;
                    invoice.LastUpdateDate = DateTime.Now;
                    invoice.Status = (int)FinanceStatus.发票状态.待开票;
                }

                using (var tran = new TransactionScope())
                {
                    var result = SoOrderBo.Instance.CreateSoOrder(soOrder.CustomerSysNo
                                                                  , soReceiveAddress
                                                                  , soOrder.DeliveryTypeSysNo
                                                                  , soOrder.PayTypeSysNo
                                                                  , soOrder.DefaultWarehouseSysNo
                                                                  , soOrder.DeliveryRemarks
                                                                  , soOrder.DeliveryTime
                                                                  , soOrder.CustomerMessage
                                                                  , soOrder.InternalRemarks
                                                                  , soOrder.ContactBeforeDelivery
                                                                  , orderComplex.CouponCode
                                                                  , product, invoice,
                                                                  CurrentUser.Base, (int)soOrder.CoinPay, soOrder.DealerSysNo);
                    if (result.Status)
                    {
                        tran.Complete();
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "客服-创建销售单", LogStatus.系统日志目标类型.订单,
                                             result.StatusCode, CurrentUser.Base.SysNo);
                        return Json(new Result { Status = true, Message = "保存成功", StatusCode = result.StatusCode },
                                    JsonRequestBehavior.AllowGet);
                    }
                    return Json(new Result { Status = false, Message = result.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "客服下单错误", LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, ex);
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public static bool _starting;
        /// <summary>
        /// 导入订单
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult ImportExcel()
        {
            //frm load
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Result();
                if (!_starting)
                {
                    _starting = true;
                    try
                    {
                        result = SoOrderBo.Instance.ImportExcel(httpPostedFileBase.InputStream, CurrentUser.Base.SysNo);
                    }
                    catch (Exception ex)
                    {
                        result.Message = ex.Message;
                        result.Status = false;
                    }
                    finally
                    {
                        _starting = false;
                    }
                }
                else
                {
                    result.Message = string.Format("正在导入数据，请稍后再操作");
                    result.Status = false;

                }
                ViewBag.result = HttpUtility.UrlEncode(result.Message);
            }

            return View();

        }
        #endregion

        #region 创建订单 购物车相关 add by ywb

        /// <summary>
        /// 添加商品到购物车，并返回购物车html
        /// </summary>
        /// <param name="customerSysNo">顾客sysno.</param>
        /// <param name="products">商品json数组.</param>
        /// <param name="isShopCoupon">是否门店使用优惠券</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>
        /// 2013－9-24 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddProductToCartAndReturnHtml(int customerSysNo, string products, bool? isShopCoupon)
        {
            /* products数据格式如下
             [{ProductSysNo:1},{ProductSysNo:2},{GroupSysNo:"组系统编号(组合,团购主表系统编号)",PromotionSysNo:"促销系统编号"}]
             */
            var productList = Hyt.Util.Serialization.JsonUtil.ToObject<IList<AddProductToCartAndReturnHtmlModel>>(products);
            foreach (var p in productList)
            {
                if (p.ProductSysNo > 0 && p.GroupSysNo == 0 && p.PromotionSysNo == 0)
                {
                    Hyt.BLL.CRM.CrShoppingCartBo.Instance.Add(customerSysNo, p.ProductSysNo, 1, CustomerStatus.购物车商品来源.客服下单);
                }
                else if (p.ProductSysNo == 0 && p.GroupSysNo > 0 && p.PromotionSysNo > 0)
                {
                    Hyt.BLL.CRM.CrShoppingCartBo.Instance.Add(customerSysNo, p.GroupSysNo, 1, p.PromotionSysNo, CustomerStatus.购物车商品来源.客服下单);
                }
            }

            //2014-1-2 杨文兵 注释  原因：添加产品时返回购物车数据改为添加产品不返回购物车数据由客户端另外发起一个请求获取购物车数据。
            //Hyt.BLL.CRM.CrShoppingCartBo.Instance.CheckedAll(customerSysNo);
            //var model = Hyt.BLL.CRM.CrShoppingCartBo.Instance.GetShoppingCart(customerSysNo);
            //var platformType = PromotionStatus.优惠券使用平台类型.Web;
            //if (isShopCoupon==true)//门店使用优惠券
            //{
            //    platformType = PromotionStatus.优惠券使用平台类型.门店;
            //}
            //ViewBag.CouponList = Hyt.BLL.Promotion.SpCouponBo.Instance.GetCurrentCartValidCoupons(customerSysNo, model, platformType);//重现计算优惠券
            //return View("ShoppingCartHtml", model);

            return Json(new Result { Status = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 此视图返回会员的购物车HTML
        /// 主要作用是：后台客服创建订单时，显示购物车数据
        /// </summary>
        /// <param name="customerSysNo">会员系统编号.</param>
        /// <param name="areaSysNo">区域系统编号.</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号.</param>
        /// <param name="couponCode">优惠券代码.</param>
        /// <param name="isShopCoupon">是否门店使用优惠券.</param>
        /// <param name="isChangePrice">是否调价.</param>
        /// <param name="warehouseSysNo">发货仓库.</param>
        /// <returns>返回会员的购物车HTML</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ShoppingCartHtml(int customerSysNo, int? areaSysNo, int? deliveryTypeSysNo, string couponCode, bool? isShopCoupon, bool isChangePrice = false, int? warehouseSysNo = null)
        {
            if (string.IsNullOrWhiteSpace(deliveryTypeSysNo.ToString()))
            {
                deliveryTypeSysNo = 0;
            }
            Hyt.BLL.CRM.CrShoppingCartBo.Instance.CheckedAll(customerSysNo);
            var platformType = new[] { PromotionStatus.促销使用平台.PC商城 };
            if (isShopCoupon == true)//门店使用优惠券
            {
                platformType = new[] { PromotionStatus.促销使用平台.门店 };
            }
            var model = Hyt.BLL.CRM.CrShoppingCartBo.Instance.GetShoppingCart(platformType, customerSysNo, null, areaSysNo, deliveryTypeSysNo, null, couponCode, false, false, warehouseSysNo);
            //给购物车赠品信息中添加EAS编号
            AddProductEasCode(model);
            ViewBag.WarehouseSysNo = warehouseSysNo;
            ViewBag.CouponList = Hyt.BLL.Promotion.SpCouponBo.Instance.GetCurrentCartValidCoupons(customerSysNo, model, platformType);
            if (isChangePrice)
            {
                return View("ShoppingCartHtmlForChangePrice", model);
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// 门店创建订单调价
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="model">调价信息</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult ChangePrice(int customerSysNo, List<SelectListItem> model)
        {
            Result res = new Result();
            res.Status = true;
            res.Message = "0";
            if (model != null)
            {
                try
                {
                    decimal ShopAdjustPrice = 20;//门店调价范围
                    var config = SyConfigBo.Instance.GetModel("ShopAdjustPrice", SystemStatus.系统配置类型.常规配置);
                    if (config == null)
                    {
                        config = new SyConfig();
                        config.CreatedBy = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                        config.CategoryId = (int)SystemStatus.系统配置类型.常规配置;
                        config.CreatedDate = DateTime.Now;
                        config.Description = "门店下单调价范围";
                        config.Key = "ShopAdjustPrice";
                        config.LastUpdateBy = config.CreatedBy;
                        config.LastUpdateDate = config.CreatedDate;
                        config.Value = ShopAdjustPrice.ToString();
                        SyConfigBo.Instance.Create(config);
                    }
                    else
                    {
                        if (!decimal.TryParse(config.Value, out ShopAdjustPrice))
                        {
                            ShopAdjustPrice = 20;//默认值20
                            config.Value = ShopAdjustPrice.ToString();
                            SyConfigBo.Instance.Update(config);
                        }
                    }
                    decimal sumval = model.Where(m => !string.IsNullOrEmpty(m.Value)).Sum(m => decimal.Parse(m.Value));
                    if (Math.Abs(sumval) > ShopAdjustPrice)
                    {
                        TempData.Remove("ChangePriceitem");
                        res.Status = false;
                        res.Message = "超过允许的调价范围(&yen;" + ShopAdjustPrice + "),请重新调价.";
                    }
                    else
                    {
                        res.Status = true;
                        TempData["ChangePriceitem"] = model;//保存在mvc 的临时对象中
                        res.Message = sumval.ToString();
                    }
                }
                catch (Exception ex)
                {
                    res.Status = false;
                    res.Message = ex.Message;
                }
            }
            return Json(res);

        }


        /// <summary>
        /// 门店创建订单调价
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="model">调价信息</param>
        /// <returns>2017 06 07添加 停止使用上面优惠方法 改为修改商品单价</returns>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult ChangePriceNew(int customerSysNo, List<SelectListItemNew> model)
        {
            Result res = new Result();
            res.Status = true;
            res.Message = "0";
            if (model != null)
            {
                try
                {
                    decimal ShopAdjustPrice = 20;//门店调价范围
                    var config = SyConfigBo.Instance.GetModel("ShopAdjustPrice", SystemStatus.系统配置类型.常规配置);
                    if (config == null)
                    {
                        config = new SyConfig();
                        config.CreatedBy = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                        config.CategoryId = (int)SystemStatus.系统配置类型.常规配置;
                        config.CreatedDate = DateTime.Now;
                        config.Description = "门店下单调价范围";
                        config.Key = "ShopAdjustPrice";
                        config.LastUpdateBy = config.CreatedBy;
                        config.LastUpdateDate = config.CreatedDate;
                        config.Value = ShopAdjustPrice.ToString();
                        SyConfigBo.Instance.Create(config);
                    }
                    else
                    {
                        if (!decimal.TryParse(config.Value, out ShopAdjustPrice))
                        {
                            ShopAdjustPrice = 20;//默认值20
                            config.Value = ShopAdjustPrice.ToString();
                            SyConfigBo.Instance.Update(config);
                        }
                    }
                    //foreach (var item in model)
                    //{
                    //    var product = PdProductBo.Instance.GetProduct(int.Parse(item.Text));
                    //    decimal val = decimal.Parse(item.Value) == 0m ? product.PdPrice : decimal.Parse(item.Value);
                    //}

                    decimal sumval = model.Where(m => !string.IsNullOrEmpty(m.Value)).Sum(m => decimal.Parse(m.Value) * int.Parse(m.Pcs));
                    //if (Math.Abs(sumval) > ShopAdjustPrice)
                    //{
                    //    TempData.Remove("ChangePriceitem");
                    //    res.Status = false;
                    //    res.Message = "超过允许的调价范围(&yen;" + ShopAdjustPrice + "),请重新调价.";
                    //}
                    //else
                    //{
                    res.Status = true;
                    //修改优惠信息为空值
                    TempData["ChangePriceitem"] = null;//保存在mvc 的临时对象中
                    //增加商品单价修改
                    TempData["ChangeDanPriceitem"] = model;//保存在mvc 的临时对象中
                    res.Message = sumval.ToString();
                    //}
                }
                catch (Exception ex)
                {
                    res.Status = false;
                    res.Message = ex.Message;
                }
            }
            return Json(res);

        }

        /// <summary>
        /// 判断订单商品库存
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="model">调价信息</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult JudgeOrderStock(int OrderSysNo, int WarehouseSysNo)
        {
            Result result = new Result();
            result.Status = true;
            return Json(result);

            result.Status = true;
            IList<CBSoOrderItem> datao = SoOrderBo.Instance.GetCBOrderItemsByOrderId(OrderSysNo);

            foreach (CBSoOrderItem item in datao)
            {
                if (result.Status)
                {
                    int ProductSysNo = item.ProductSysNo;
                    string EasName = item.EasName;
                    int Quantity = item.Quantity;
                    PdProductStock entity = Hyt.BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(WarehouseSysNo, ProductSysNo);
                    if (entity != null)
                    {
                        if (entity.StockQuantity < Quantity)
                        {
                            result.Status = false;
                            result.Message = EasName + ":在该仓库中库存为" + entity.StockQuantity.ToString("0") + ",库存不足";
                            break;
                        }
                        else
                        {
                            result.Status = true;
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = EasName + ":不存在该仓库中";
                        break;
                    }
                }
            }
            return Json(result);

        }

        /// <summary>
        /// 判断订单商品库存
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="model">调价信息</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult JudgeOrderProductStock(int ProductSysNo, int WarehouseSysNo, decimal Quantity)
        {
            Result result = new Result();
            PdProductStock entity = Hyt.BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(WarehouseSysNo, ProductSysNo);
            if (entity != null)
            {
                if (entity.StockQuantity < Quantity)
                {
                    result.Status = false;
                    string EasName = Hyt.BLL.Product.PdProductBo.Instance.GetProductEasName(ProductSysNo);
                    result.Message = EasName + ":在该仓库中库存为" + entity.StockQuantity.ToString("0") + ",库存不足";
                }
                else
                {
                    result.Status = true;
                }
            }
            else
            {
                result.Status = false;
                string EasName = Hyt.BLL.Product.PdProductBo.Instance.GetProductEasName(ProductSysNo);
                result.Message = EasName + ":不存在该仓库中";
            }
            return Json(result);

        }

        /// <summary>
        /// 判断订单商品库存
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="model">调价信息</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        public ActionResult JudgeShopCartProductStock(string ProductList)
        {
            Result result = new Result();
            string[] productArray = ProductList.Split(';');
            int ProductSysNo; int WarehouseSysNo; decimal Quantity;
            foreach (string p in productArray)
            {
                ProductSysNo = int.Parse(p.Split(',')[0]);
                WarehouseSysNo = int.Parse(p.Split(',')[1]);
                Quantity = decimal.Parse(p.Split(',')[2]);


                PdProductStock entity = Hyt.BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(WarehouseSysNo, ProductSysNo);
                if (entity != null)
                {
                    if (entity.StockQuantity < Quantity)
                    {
                        result.Status = false;
                        string EasName = Hyt.BLL.Product.PdProductBo.Instance.GetProductEasName(ProductSysNo);
                        result.Message = EasName + ":在该仓库中库存为" + entity.StockQuantity.ToString("0") + ",库存不足";
                        break;
                    }
                    else
                    {
                        result.Status = true;
                    }
                }
                else
                {
                    result.Status = false;
                    string EasName = Hyt.BLL.Product.PdProductBo.Instance.GetProductEasName(ProductSysNo);
                    result.Message = EasName + ":不存在该仓库中";
                    break;
                }
            }
            return Json(result);

        }

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateGroupQuantity(int customerSysNo, string groupCode, string promotionSysNo, int quantity)
        {
            if (quantity > 0)
            {
                CrShoppingCartBo.Instance.UpdateQuantity(customerSysNo, groupCode, promotionSysNo, quantity);
            }
            else
            {
                CrShoppingCartBo.Instance.Remove(customerSysNo, groupCode, promotionSysNo);
            }

            return Json(new Result { Status = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateItemQuantity(int customerSysNo, int sysNo, int quantity)
        {
            if (quantity > 0)
            {
                CrShoppingCartBo.Instance.UpdateQuantity(customerSysNo, new int[] { sysNo }, quantity);
            }
            else
            {
                CrShoppingCartBo.Instance.Remove(customerSysNo, new int[] { sysNo });
            }
            return Json(new Result { Status = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult RemoveGroup(int customerSysNo, string groupCode, string promotionSysNo)
        {
            CrShoppingCartBo.Instance.Remove(customerSysNo, groupCode, promotionSysNo);
            return Json(new Result { Status = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除购物车明细商品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013－9-20 杨文兵 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult RemoveItem(int customerSysNo, int sysNo)
        {
            CrShoppingCartBo.Instance.Remove(customerSysNo, new int[] { sysNo });
            return Json(new Result { Status = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加购物车赠品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddGift(int customerSysNo, int productSysNo, int promotionSysNo)
        {
            CrShoppingCartBo.Instance.AddGift(customerSysNo, productSysNo, promotionSysNo, CustomerStatus.购物车商品来源.客服下单);
            return Json(new Result { Status = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除购物车赠品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201)]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult RemoveGift(int customerSysNo, int productSysNo, int promotionSysNo)
        {
            CrShoppingCartBo.Instance.RemoveGift(customerSysNo, productSysNo, promotionSysNo);
            return Json(new Result { Status = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改订单 购物车相关

        /// <summary>
        /// 获取订单修改时的购物车HTMl。
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单修改时的购物车HTMl</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        [HttpPost]
        public ActionResult CartHtmlForEditOrder(int orderSysNo)
        {
            var order = SoOrderBo.Instance.GetEntity(orderSysNo);
            return View(BuildEditOrderModel(order));
        }

        /// <summary>
        /// 构建页面显示编辑对象
        /// </summary>
        /// <param name="order">订单实体</param>
        /// <returns>页面显示编辑对象HTMl</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        private EditOrderModel BuildEditOrderModel(SoOrder order)
        {
            var orderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(order.SysNo);
            var cart = Hyt.BLL.CRM.CrShoppingCartConvertBo.Instance.GetCartByOrder(new[] { order.GetPromotionPlatformType() }, order.CustomerSysNo, order.SysNo);

            var model = new EditOrderModel()
            {
                Order = order,
                OrderItems = orderItems,
                ShoppingCart = cart,
                JsonCartItem = cart.ConvertJson()
            };

            if (model.Order.ReceiveAddressSysNo > 0)
            {
                var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.Order.ReceiveAddressSysNo);
                if (receiveAddress != null) model.AreaSysNo = receiveAddress.AreaSysNo;
            }
            model.DeliveryTypeSysNo = model.Order.DeliveryTypeSysNo;
            if (string.IsNullOrEmpty(model.ShoppingCart.CouponCode))
            {
                //从数据库里面获取正在使用的优惠券
                var ordercoupon = SoOrderBo.Instance.GetCouponByOrderSysNo(order.SysNo).FirstOrDefault();
                if (ordercoupon != null)
                {
                    model.CouponCode = ordercoupon.CouponCode;
                    model.ShoppingCart.CouponAmount = ordercoupon.CouponAmount;
                }
            }
            else
            {
                model.CouponCode = model.ShoppingCart.CouponCode;
            }
            model.UsedPromotionSysNo = model.Order.GetUsedPromotionSysNo();

            //给购物车赠品信息中添加EAS编号
            AddProductEasCode(model.ShoppingCart);
            return model;
        }

        /// <summary>
        /// 给购物车赠品信息中添加EAS编号
        /// </summary>
        /// <param name="model">购物车信息</param>
        /// <returns>购物车信息</returns>
        /// <remarks>
        /// 2014-03-07 余勇 创建
        /// </remarks>
        private CrShoppingCart AddProductEasCode(CrShoppingCart model)
        {
            if (model.GroupPromotions != null)
            {
                foreach (var group in model.GroupPromotions)
                {
                    if (group.GiftProducts != null)
                    {
                        group.GiftProducts.ForEach(
                            g =>
                            g.ProductErpCode = Hyt.BLL.Product.PdProductBo.Instance.GetProductErpCode(g.ProductSysNo));
                    }
                }
            }
            if (model.ShoppingCartGroups != null)
            {
                foreach (var group in model.ShoppingCartGroups)
                {
                    if (group.GroupPromotions != null)
                    {
                        foreach (var list in group.GroupPromotions)
                        {
                            if (list.GiftProducts != null)
                            {
                                list.GiftProducts.ForEach(
                                    g =>
                                    g.ProductErpCode =
                                    Hyt.BLL.Product.PdProductBo.Instance.GetProductErpCode(g.ProductSysNo));
                            }
                        }
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 编辑优惠券
        /// </summary>
        /// <param name="editOrderModel"></param>
        /// <param name="newcouponCode">新的购物券</param>
        /// <param name="newDeliveryTypeSysNo">新的配送方式</param>
        /// <param name="newAreaNo">新的地区编号</param>
        /// <param name="isUpdataDB">是否更新数据库</param>
        /// <returns>编辑优惠券HTMl</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditOrderChangeCouponCode(string editOrderModel, string newcouponCode, decimal expensesAmount, int? newDeliveryTypeSysNo, int? newAreaNo, bool isUpdataDB = true)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOrderModel>(editOrderModel);
            var editOrderCart = new EditOrderCart(model.Order.CustomerSysNo, model.JsonCartItem
                , model.AreaSysNo
                , model.DeliveryTypeSysNo
                , model.CouponCode
                , model.UsedPromotionSysNo
                , model.Order
            );

            editOrderCart.SetCoupon(newcouponCode);
            model.CouponCode = newcouponCode;        // 2014-1-3 杨文兵 注释 解决不能取消优惠券的问题
            //if (string.IsNullOrEmpty(newcouponCode) == false)
            //{
            //    editOrderCart.SetCoupon(newcouponCode);
            //    //model.CouponCode = newcouponCode; 
            //}

            if (newDeliveryTypeSysNo.HasValue == true)
            {
                editOrderCart.SetDeliveryTypeSysNo(newDeliveryTypeSysNo.Value);
                model.DeliveryTypeSysNo = newDeliveryTypeSysNo.Value;
            }
            if (newAreaNo.HasValue == true)
            {
                editOrderCart.SetAreaSysNo(newAreaNo.Value);
                model.AreaSysNo = newAreaNo.Value;
            }
            model.ShoppingCart = editOrderCart.ToCrShoppingCart(new[] { model.Order.GetPromotionPlatformType() }, expensesAmount);
            model.JsonCartItem = model.ShoppingCart.ConvertJson();
            //是否是显示编辑 2016-7-2 王耀发 创建
            ViewBag.IsShowEdit = true;
            return PartialView("CartHtmlForEditOrder", model);
        }

        /// <summary>
        /// 修改订单时 添加商品
        /// </summary>
        /// <param name="editOrderModel">订单修改交互界面Model</param>
        /// <param name="products">添加商品json数据</param>
        /// <returns>添加商品HTMl</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditOrderAddProduct(string editOrderModel, string products)
        {
            //var model = Hyt.Util.Serialization.JsonUtil.ToObject<EditOrderModel>(editOrderModel);
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOrderModel>(editOrderModel); //使用上面语句反序列化得到的IList对象调用List.Add方法会报异常。
            var productList = Hyt.Util.Serialization.JsonUtil.ToObject<IList<AddProductToCartAndReturnHtmlModel>>(products);
            //var cackeKey = "EditOrder" + model.Order.SysNo;

            //SoOrderBo.Instance.EditOrderBefore(model);

            var editOrderCart = new EditOrderCart(model.Order.CustomerSysNo, model.JsonCartItem
                                                  , model.AreaSysNo
                                                  , model.DeliveryTypeSysNo
                                                  , model.CouponCode
                                                  , model.UsedPromotionSysNo
                                                  , model.Order
                );

            var source = (CustomerStatus.购物车商品来源)model.Order.OrderSource;

            foreach (var p in productList)
            {
                if (p.ProductSysNo > 0 && p.GroupSysNo == 0 && p.PromotionSysNo == 0)
                {
                    var jsonCartItem = editOrderCart.Add(p.ProductSysNo, 1, source);
                    if (jsonCartItem != null)
                    {
                        model.OrderItems.Add(new SoOrderItem()
                        {
                            SysNo = jsonCartItem.SysNo,
                            ProductSysNo = p.ProductSysNo,
                            ChangeAmount = 0
                        });
                    }
                }
                else if (p.ProductSysNo == 0 && p.GroupSysNo > 0 && p.PromotionSysNo > 0)
                {
                    editOrderCart.Add(p.GroupSysNo.ToString(), 1, p.PromotionSysNo.ToString(), source);
                }
            }
            model.ShoppingCart = editOrderCart.ToCrShoppingCart(new[] { model.Order.GetPromotionPlatformType() });
            model.JsonCartItem = model.ShoppingCart.ConvertJson();
            //是否是显示编辑 2016-7-2 王耀发 创建
            ViewBag.IsShowEdit = true;
            return View("CartHtmlForEditOrder", model);
        }


        /// <summary>
        /// 修改订单时 更新商品数量 调价
        /// </summary>
        /// <param name="editOrderModel">订单修改交互界面Model.</param>         
        /// <param name="groupCode">组代码.</param>
        /// <param name="promotionSysNo">促销系统编号.</param>
        /// <param name="quantity">商品数量.</param>
        /// <param name="productSysNo">普通商品sysno.</param>
        /// <param name="changeAmount">调价金额.</param>
        /// <returns>购物车商品编辑HTMl</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditOrderUpdateQuantity(string editOrderModel, string groupCode, string promotionSysNo, int quantity, int productSysNo, decimal changeAmount)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOrderModel>(editOrderModel);
            //var cacheKey = "EditOrder" + model.Order.SysNo;
            //SoOrderBo.Instance.EditOrderBefore(model);

            var editOrderCart = new EditOrderCart(model.Order.CustomerSysNo, model.JsonCartItem
                , model.AreaSysNo
                , model.DeliveryTypeSysNo
                , model.CouponCode
                , model.UsedPromotionSysNo
                , model.Order
                );

            if (productSysNo > 0 && string.IsNullOrEmpty(groupCode) && string.IsNullOrEmpty(promotionSysNo))
            {
                editOrderCart.UpdateQuantity(productSysNo, quantity);

                if (productSysNo > 0)
                {
                    model.OrderItems.First(p => p.ProductSysNo == productSysNo).ChangeAmount = changeAmount;//调价
                }
            }
            else if (productSysNo < 1 && string.IsNullOrEmpty(groupCode) == false && string.IsNullOrEmpty(promotionSysNo) == false)
            {
                editOrderCart.UpdateQuantity(groupCode, promotionSysNo, quantity);
            }

            model.ShoppingCart = editOrderCart.ToCrShoppingCart(new[] { model.Order.GetPromotionPlatformType() });
            model.JsonCartItem = model.ShoppingCart.ConvertJson();

            //是否是显示编辑 2016-7-2 王耀发 创建
            ViewBag.IsShowEdit = true;
            return View("CartHtmlForEditOrder", model);
        }

        /// <summary>
        /// 修改订单时 删除商品.
        /// </summary>
        /// <param name="editOrderModel">订单修改交互界面Model.</param>
        /// <param name="productSysNo">普通商品系统编号.</param>
        /// <param name="groupCode">组代码.</param>
        /// <param name="promotionSysNo">促销系统编号.</param>        
        /// <returns>购物车商品编辑HTMl</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditOrderRemoveProduct(string editOrderModel, int productSysNo, string groupCode, string promotionSysNo)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOrderModel>(editOrderModel);

            var editOrderCart = new EditOrderCart(model.Order.CustomerSysNo, model.JsonCartItem
                , model.AreaSysNo
                , model.DeliveryTypeSysNo
                , model.CouponCode
                , model.UsedPromotionSysNo
                , model.Order
                );

            if (productSysNo > 0 && string.IsNullOrEmpty(groupCode) && string.IsNullOrEmpty(promotionSysNo))
            {
                model.OrderItems.Remove(model.OrderItems.First(p => p.ProductSysNo == productSysNo));
                editOrderCart.Remove(productSysNo);
            }
            else if (productSysNo < 1 && string.IsNullOrEmpty(groupCode) == false && string.IsNullOrEmpty(promotionSysNo) == false)
            {
                editOrderCart.Remove(groupCode, promotionSysNo);
            }

            model.ShoppingCart = editOrderCart.ToCrShoppingCart(new[] { model.Order.GetPromotionPlatformType() });
            model.JsonCartItem = model.ShoppingCart.ConvertJson();
            //是否是显示编辑 2016-7-2 王耀发 创建
            ViewBag.IsShowEdit = true;
            return View("CartHtmlForEditOrder", model);
        }

        /// <summary>
        /// 重新选择赠品.
        /// （系统设计目前只支持一个赠品）
        /// </summary>
        /// <param name="editOrderModel">订单修改交互界面Model.</param>
        /// <param name="delGift">删除赠品.</param>
        /// <param name="addGift">增加赠品.</param>
        /// <param name="promotionSysNo">促销系统编号.</param>
        /// <returns>购物车商品编辑HTMl</returns>
        /// <remarks>
        /// 2013－9-20 杨文兵 创建
        /// </remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChangeGift(string editOrderModel, int delGift, int addGift, int promotionSysNo)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOrderModel>(editOrderModel);
            var editOrderCart = new EditOrderCart(model.Order.CustomerSysNo, model.JsonCartItem
                , model.AreaSysNo
                , model.DeliveryTypeSysNo
                , model.CouponCode
                , model.UsedPromotionSysNo
                , model.Order
                );

            if (delGift > 0)
            {
                editOrderCart.RemoveGift(delGift, promotionSysNo.ToString());
            }
            if (addGift > 0)
            {
                var source = (CustomerStatus.购物车商品来源)model.Order.OrderSource;
                editOrderCart.AddGift(addGift, promotionSysNo.ToString(), source);
            }

            model.ShoppingCart = editOrderCart.ToCrShoppingCart(new[] { model.Order.GetPromotionPlatformType() });
            model.JsonCartItem = model.ShoppingCart.ConvertJson();
            //是否是显示编辑 2016-7-2 王耀发 创建
            ViewBag.IsShowEdit = true;
            return View("CartHtmlForEditOrder", model);
        }

        #endregion

        #region 显示订单详情

        /// <summary>
        /// 销售单创建产品分页
        /// </summary>
        /// <param name="customerSysNo">会员编号.</param>
        /// <returns>视图</returns>
        /// <remarks>2013－06-27 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult OrderCreate(int? customerSysNo)
        {
            ViewBag.FnInvoiceType = Hyt.BLL.Finance.FnInvoiceBo.Instance.GetFnInvoiceTypeList();
            if (customerSysNo != null)
            {
                var model = CrCustomerBo.Instance.GetModel(customerSysNo.Value);
                ViewBag.CustomerName = model.Name;
                ViewBag.Account = model.Account;
            }
            ViewBag.CustomerSysNo = customerSysNo;
            ViewBag.CreatorSysNo = CurrentUser.Base.SysNo;
            return View();
        }

        /// <summary>
        /// 获取订单时间搓
        /// </summary>
        /// <param name="sysNo">订单号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013－06-27 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult GetOrderStamp(int sysNo)
        {
            Result r = new Result { Status = false };
            try
            {
                r.Message = SoOrderBo.Instance.GetOrderStamp(sysNo).Ticks.ToString();
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单详情(可操作视图)
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="logPageIndex">订单日志分页</param>
        /// <returns>视图</returns>
        /// <remarks>2013-06-09 朱成果 创建</remarks>
        /// <remarks>2013-11-6 黄志勇 修改</remarks>
        [HttpGet]
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult OrderDetail(int id, int? logPageIndex)
        {
            int pageSize = 10;
            //ajax分页
            if (Request.IsAjaxRequest())
            {
                //订单日志分页
                if (logPageIndex.HasValue)
                {
                    return PartialView("_OrderLog", SoOrderBo.Instance.GetTransactionLogPageData(id, logPageIndex.Value, pageSize));
                }
            }
            var model = SoOrderBo.Instance.GetEntity(id);
            if (model == null)
            {
                return RedirectToAction("OrderCreate");
            }
            LoadOtherOrderData(model, pageSize);//加载其他数据
            //是否是显示编辑 2016-7-2 王耀发 创建
            ViewBag.IsShowEdit = false;
            //是否是可操作状态
            ViewBag.IsCanEdit = true;
            //输出相关提示
            if (Request.QueryString["success"] != null)
            {
                ViewBag.Flg = Request.QueryString["success"];
            }
            ProcJobPool(model);

            var text = BsCodeBo.Instance.GetCancelReasonCode().Select(s => s.CodeName).ToArray();
            ViewBag.CancelOrderText = string.Join("|", text);

            return View(model);
        }
        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// 2016-01-07 王耀发 创建
        [HttpGet]
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult DoOrderItemQuery(ParaOrderItemRecordFilter filter)
        {
            filter.PageSize = 100;
            var pager = SoOrderItemBo.Instance.GetOrderItemsRecordList(filter);
            var list = new PagedList<CBSoOrderItem>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                PageSize = 100,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_OrderItemRecordPager", list);
        }

        /// <summary>
        /// 任务池处理
        /// </summary>
        /// <param name="order">订单实体</param>
        /// <returns>空</returns>
        /// <remarks>2013-11-6 黄志勇 修改</remarks>
        private void ProcJobPool(SoOrder order)
        {
            int taskType = 0;
            switch (order.Status)
            {
                case (int)OrderStatus.销售单状态.待审核:
                    taskType = (int)SystemStatus.任务对象类型.客服订单审核;
                    break;
                case (int)OrderStatus.销售单状态.部分创建出库单:
                case (int)OrderStatus.销售单状态.待创建出库单:
                    taskType = (int)SystemStatus.任务对象类型.客服订单提交出库;
                    break;
            }
            SyUser user = null;
            var jobPool = SyJobPoolManageBo.Instance.GetByTask(order.SysNo, taskType);
            if (jobPool != null)
            {
                if (jobPool.Status == (int)SystemStatus.任务池状态.已锁定)
                {
                    ViewBag.IsLocked = true;
                    ViewBag.IsCanEdit = false;
                }
                user = SyUserBo.Instance.GetSyUser(jobPool.ExecutorSysNo);
            }
            ViewBag.CurrentSysNo = CurrentUser.Base.SysNo;
            ViewBag.TaskType = taskType;
            ViewBag.ExecutorSysNo = user != null ? user.SysNo : 0;
            ViewBag.ExecutorName = user != null ? user.UserName : ""; //任务执行者
            //若订单已出库而任务池仍有记录，则删除该任务
            if (taskType == 0 && (order.Status == (int)OrderStatus.销售单状态.作废 || order.Status == (int)OrderStatus.销售单状态.已创建出库单 || order.Status == (int)OrderStatus.销售单状态.已完成))
            {
                jobPool = SyJobPoolManageBo.Instance.GetByTask(order.SysNo, (int)SystemStatus.任务对象类型.客服订单审核);
                if (jobPool != null)
                {
                    SyJobPoolManageBo.Instance.DeleteJobPool(jobPool.SysNo);
                }
                jobPool = SyJobPoolManageBo.Instance.GetByTask(order.SysNo, (int)SystemStatus.任务对象类型.客服订单提交出库);
                if (jobPool != null)
                {
                    SyJobPoolManageBo.Instance.DeleteJobPool(jobPool.SysNo);
                }
            }

        }

        /// <summary>
        /// 加载订单相关信息
        /// </summary>
        /// <param name="model">订单实体</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>空</returns>
        /// <remarks>2013-06-09 朱成果 创建</remarks>
        /// <remarks>2013-12-13 黄志勇 修改（增加升舱信息）</remarks>
        private void LoadOtherOrderData(SoOrder model, int pageSize)
        {
            ViewBag.IsPay = (model.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付);//是否已付款
            //收货地址
            model.ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);
            //显示省市区
            if (model.ReceiveAddress != null)
            {
                //城市
                Hyt.Model.BsArea cEntity;
                //地区
                Hyt.Model.BsArea aEntity;
                var pEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetProvinceEntity(model.ReceiveAddress.AreaSysNo, out cEntity, out aEntity);
                string addressDetatil = string.Empty;
                if (pEntity != null)
                {
                    addressDetatil = pEntity.AreaName;
                }
                if (cEntity != null)
                {
                    addressDetatil = addressDetatil + "  " + cEntity.AreaName;
                }
                if (aEntity != null)
                {
                    addressDetatil = addressDetatil + "  " + aEntity.AreaName;
                }
                ViewBag.AddressDetatil = addressDetatil;
            }
            //会员信息
            model.Customer = SoOrderBo.Instance.SearchCustomer(model.CustomerSysNo);
            //默认仓库名
            if (model.DefaultWarehouseSysNo > 0)
            {
                var defaultWarehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseEntity(model.DefaultWarehouseSysNo);
                if (defaultWarehouse != null)
                {
                    ViewBag.DefaultWarehouseName = defaultWarehouse.BackWarehouseName;
                    ViewBag.DefaultWarehouseSyo = defaultWarehouse.SysNo;
                }
            }

            //发票信息
            model.OrderInvoice = SoOrderBo.Instance.GetFnInvoice(model.InvoiceSysNo);
            //订单日志默认加载第一页
            ViewData["LogList"] = SoOrderBo.Instance.GetTransactionLogPageData(model.SysNo, 1, pageSize);

            //订购商品数量
            ViewData["OrderItems"] = SoOrderBo.Instance.GetOrderItemsWithErpCodeByOrderSysNo(model.SysNo);

            #region 修改订单购物车相关数据 2013-9-26 杨文兵
            var showmodel = BuildEditOrderModel(model);
            ViewData["EditOrderModel"] = showmodel;
            SoOrderBo.Instance.EditOrderBefore(showmodel);//缓存
            #endregion

            //获取出库单
            //ViewData["WhStockOut"] = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhStockOutListByOrderID(model.SysNo);
            ViewBag.InvoiceTypeList = GetInvoiceTypeForDropDownList();//发票类型选择
            //记录订单状态
            ViewData["Status"] = model.Status;
            //是否分销商升舱订单
            ViewBag.IsMallSeller = model.OrderSource == (int)OrderStatus.销售单来源.分销商升舱;
            //
            if (ViewBag.IsMallSeller)
            {
                //不允许调整商品明细
                ViewBag.EditOrderItem = false;
                var dsOrderEx = DsOrderBo.Instance.GetDsOrderInfoEx(model.TransactionSysNo);
                if (dsOrderEx.Item1 != null && dsOrderEx.Item2 != null)
                {
                    ViewBag.DsOrder = dsOrderEx.Item1;
                    ViewBag.DealerMall = dsOrderEx.Item2;
                }
            }
        }

        /// <summary>
        /// 获取发票类型选择下拉数据
        /// </summary>
        /// <returns>发票类型选择下拉数据</returns>
        /// <remarks>2013-06-09 朱成果 创建</remarks>
        private List<SelectListItem> GetInvoiceTypeForDropDownList()
        {
            var fnInvoiceTypeList = Hyt.BLL.Finance.FnInvoiceBo.Instance.GetFnInvoiceTypeList();
            List<SelectListItem> showInvoiceTypeList = new List<SelectListItem>();
            showInvoiceTypeList.Add(new SelectListItem { Text = "无", Value = "0" });
            if (showInvoiceTypeList != null)
            {
                foreach (FnInvoiceType f in fnInvoiceTypeList)
                {
                    showInvoiceTypeList.Add(new SelectListItem { Text = f.Name, Value = f.SysNo.ToString() });
                }
            }
            return showInvoiceTypeList;
        }

        /// <summary>
        /// 订单详情(只读视图)
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="logPageIndex">订单日志分页</param>
        /// <returns>视图</returns>
        /// <remarks>2013-06-09 朱成果 创建</remarks>
        [HttpGet]
        [Privilege(PrivilegeCode.CM1005802)]
        public ActionResult OrderView(int id, int? logPageIndex)
        {
            int pageSize = 10;
            //ajax分页
            if (Request.IsAjaxRequest())
            {
                //订单日志分页
                if (logPageIndex.HasValue)
                {
                    return PartialView("_OrderLog", SoOrderBo.Instance.GetTransactionLogPageData(id, logPageIndex.Value, pageSize));
                }
            }
            var model = SoOrderBo.Instance.GetEntity(id);
            if (model == null)
            {
                return RedirectToAction("OrderCreate");
            }
            LoadOtherOrderData(model, pageSize);//加载其他数据
            //是否是可操作状态
            ViewBag.IsCanEdit = false;
            ViewBag.ReadOnly = true;
            return View("OrderDetail", model);
        }

        /// <summary>
        /// 选择仓库
        /// </summary>
        /// <param name="defaultWarehouse">默认仓库编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-06-09 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.CM1005808)]
        [HttpGet]
        public ActionResult SelectWarehouse(int? defaultWarehouse)
        {
            Hyt.Model.WhWarehouse model = new Model.WhWarehouse();
            if (defaultWarehouse.HasValue)
            {
                var entity = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseEntity(defaultWarehouse.Value);
                if (entity != null)
                {
                    model = entity;
                    Model.BsArea cityEntity;
                    Model.BsArea areaEntity;
                    var provinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetProvinceEntity(model.AreaSysNo, out cityEntity, out areaEntity);
                    if (cityEntity != null && areaEntity != null && provinceEntity != null)
                    {
                        ViewBag.LocalArea = provinceEntity.AreaName + " > " + cityEntity.AreaName + " > " + areaEntity.AreaName;
                    }
                }
            }

            return View(model);
        }

        /// <summary>
        /// 多选仓库
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-02 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.CM1005804)]
        [HttpGet]
        public ActionResult MultipleSelectWarehouse()
        {
            return View();
        }
        #endregion

        #region 订单保存，审核，作废，出库
        /// <summary>
        /// 保存订单信息
        /// </summary>
        /// <param name="entity">订单实体</param>
        /// <param name="item">表单参数</param>
        /// <returns>订单详情视图</returns>
        /// <remarks>2013-06-19 朱成果 创建</remarks>
        /// <remarks>2013-11-15 黄志勇 修改</remarks>
        /// <remarks>2013-11-18 杨文兵 修改</remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveOrder(Hyt.Model.SoOrder entity, FormCollection item)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改订单", LogStatus.系统日志目标类型.订单, entity.SysNo,
                                    CurrentUser.Base.SysNo);
            //-1 失败 1 成功 2 订单数据已经更改
            int flg = -1;
            var model = SoOrderBo.Instance.GetEntity(entity.SysNo);
            //时间搓一致
            if (model.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核 && item["Ticks"].ToString() == model.Stamp.Ticks.ToString())
            {
                flg = 0;
                model.ContactBeforeDelivery = entity.ContactBeforeDelivery;
                model.CouponAmount = entity.CouponAmount;
                model.CustomerMessage = entity.CustomerMessage;
                model.DefaultWarehouseSysNo = entity.DefaultWarehouseSysNo;
                model.DeliveryRemarks = entity.DeliveryRemarks;
                model.DeliveryTime = entity.DeliveryTime;
                model.DeliveryTypeSysNo = entity.DeliveryTypeSysNo;
                //model.OrderDiscountAmount = entity.OrderDiscountAmount;
                //model.FreightAmount = entity.FreightAmount;
                //model.TaxFee = entity.TaxFee;
                model.InternalRemarks = entity.InternalRemarks; //对内备注
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;
                //model.OrderAmount = entity.OrderAmount;
                //model.CashPay = entity.CashPay;
                model.PayTypeSysNo = entity.PayTypeSysNo;
                model.Stamp = DateTime.Now;

                model.ReceiveAddress = entity.ReceiveAddress;

                #region 修改发票信息
                if (entity.OrderInvoice.InvoiceTypeSysNo == 0)
                {
                    model.InvoiceSysNo = 0;
                }
                else //开发票
                {
                    //以前没有创建发票
                    if (model.InvoiceSysNo == 0)
                    {
                        model.OrderInvoice = new FnInvoice
                        {
                            CreatedBy = CurrentUser.Base.SysNo,
                            CreatedDate = DateTime.Now,
                            InvoiceRemarks = entity.OrderInvoice.InvoiceRemarks,
                            InvoiceTitle = entity.OrderInvoice.InvoiceTitle,
                            InvoiceTypeSysNo = entity.OrderInvoice.InvoiceTypeSysNo,
                            InvoiceAmount = entity.CashPay,
                            Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.发票状态.待开票,
                            TransactionSysNo = model.TransactionSysNo
                        };
                        model.InvoiceSysNo = SoOrderBo.Instance.InsertOrderInvoice(model.OrderInvoice);
                    }
                    else  //以前有发票
                    {
                        model.OrderInvoice = SoOrderBo.Instance.GetFnInvoice(model.InvoiceSysNo);
                        model.OrderInvoice.LastUpdateBy = CurrentUser.Base.SysNo;
                        model.OrderInvoice.LastUpdateDate = DateTime.Now;
                        model.OrderInvoice.InvoiceTitle = entity.OrderInvoice.InvoiceTitle;
                        model.OrderInvoice.InvoiceRemarks = entity.OrderInvoice.InvoiceRemarks;
                        model.OrderInvoice.InvoiceTypeSysNo = entity.OrderInvoice.InvoiceTypeSysNo;
                        model.OrderInvoice.InvoiceAmount = entity.CashPay;
                        SoOrderBo.Instance.UpdateOrderInvoice(model.OrderInvoice);
                    }
                }
                #endregion
                //太平洋保险
                decimal expensesAmount = 0M;
                expensesAmount = Hyt.BLL.Order.SoOrderOtherExpensesBo.Instance.GetExpensesFee(entity.SysNo);
                //修改订单 和订单明细  2013-11-15 杨文兵
                if (model.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付)
                {
                    flg = SoOrderBo.Instance.SaveOrder(model) ? 1 : -1;
                }
                else
                {
                    var editOrderModel = Hyt.Util.Serialization.JsonUtil.ToObject<EditOrderModel>(item["txtEditOrderModel"].ToString());
                    using (var tran = new TransactionScope())
                    {
                        flg = SoOrderBo.Instance.EditOrder(model, editOrderModel, expensesAmount) ? 1 : -1;
                        tran.Complete();
                    }
                }
            }
            else
            {
                flg = -2;
            }
            return RedirectToAction("OrderDetail", new { id = entity.SysNo, success = flg });
            //return Json(new { success = flg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 审核订单
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-06-20 朱成果 创建</remarks>
        /// <remarks>2013-11-15 黄志勇 修改</remarks>
        /// <remarks>2017-05-18 罗勤尧 修改添加利嘉订单同步</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1003601)]
        public ActionResult AuditOrder(int orderID)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "审核订单", LogStatus.系统日志目标类型.订单, orderID,
                                    CurrentUser.Base.SysNo);
            Result r = new Result
            {
                Status = false
            };
            if (Request["Ticks"] != null)//页面传入的时间戳
            {
                //获取页面传入的时间戳
                var dt = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderStamp(orderID);
                if (dt != null && dt.Ticks.ToString() != Request["Ticks"].ToString())
                {
                    r.Message = "订单数据已经被其他人员修改，请先刷新订单数据！";
                    return Json(r, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {
                using (var tran = new TransactionScope())
                {
                    r.Status = SoOrderBo.Instance.AuditSoOrder(orderID, CurrentUser.Base.SysNo);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            //订单审核通过，同步利嘉订单接口
            if (r.Status)
            {
                SynchronizeOrder(orderID);
            }
            //太平洋保险费
            decimal expensesAmount = 0M;
            expensesAmount = Hyt.BLL.Order.SoOrderOtherExpensesBo.Instance.GetExpensesFee(orderID);
            if (expensesAmount > 0 && r.Status)
            {
                //DouShabaoOrder(orderID);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        //private ActionResult DouShabaoOrder(int orderID)
        //{
        //    //根据订单获取订单信息
        //    var model = SoOrderBo.Instance.GetEntity(orderID);
        //    #region 获取参数
        //    var sparameter = SoOrderBo.Instance.GetSignparameter(orderID);  //获取签名参数
        //    var douShabaoParameter = SoOrderBo.Instance.Getotherparameter(orderID);  //获取配送方式，身份证，总价，(运费)，下单时间
        //    ProductList productlist = SoOrderBo.Instance.GetProductlist(orderID); //获取信营订单
        //    string orderNo = orderID.ToString();
        //    string totalAmount = sparameter.TotalAmount;
        //    string buyerName = sparameter.BuyerName;
        //    string buyerMobile = sparameter.BuyerMobile;
        //    string sign = Hyt.BLL.DouShabao.DouShabaoSign.Instance.GetSign(orderNo, totalAmount, buyerName, buyerMobile); //获取签名
        //    //豆沙包传入参数
        //    douShabaoParameter.OrderNo = orderID.ToString();
        //    string expressChannel = douShabaoParameter.ExpressChannel; //运输方式
        //    string idCard = douShabaoParameter.IdCard; //身份证
        //    string orderTime = douShabaoParameter.OrderTime; //订单时间
        //    string purchasOrderNo = douShabaoParameter.PurchasOrderNo; //订单号
        //    string expressNo = douShabaoParameter.ExpressNo; //物流单号
        //    double totalWeight = douShabaoParameter.TotalWeight; //总重量
        //    string transitLine = "EMS清关路线";
        //    string purchasOrderAmount = sparameter.TotalAmount;
        //    if (expressNo==null)
        //    {
        //        expressNo = "1234567890"; //若配送单为空，默认为1234567890
        //    }
        //    string purchasOrderTime = DateTime.Now.ToString();
        //    Hyt.BLL.DouShabao.DouShabaoSign.Instance.DoushabaoRealize(orderNo, expressChannel, totalAmount, buyerName, buyerMobile, idCard, expressNo, transitLine, orderTime, totalWeight,sign,purchasOrderNo, purchasOrderAmount, purchasOrderTime, productlist);
        //    #endregion

        //    return null;
        //}
        /// <summary>
        /// 利嘉订单订单同步
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public ActionResult SynchronizeOrder(int orderID)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "利嘉订单同步操作", LogStatus.系统日志目标类型.订单, orderID,
                                    CurrentUser.Base.SysNo);
            Result r = new Result
            {
                Status = false
            };
            //根据订单获取分销商信息
            var dsdealer = BLL.Distribution.DsDealerBo.Instance.GetCBDsDealerByOrderId(orderID);

            //创建新增分销商对象
            DsDealerLiJia dslijia = null;

            if (dsdealer != null)
            {

                dslijia = new DsDealerLiJia();
                dslijia.MemberName = dsdealer.MemberName;
                dslijia.PhoneNumber = dsdealer.PhoneNumber;

                //如果省为空值
                if (string.IsNullOrWhiteSpace(dsdealer.Province))
                {
                    //添加一个默认值
                    dslijia.Province = "广东省";
                }
                else
                {
                    dslijia.Province = dsdealer.Province;
                }
                dslijia.QQ = dsdealer.QQ;
                //如果会员地址为空值
                if (string.IsNullOrWhiteSpace(dsdealer.AddressLine))
                {
                    //添加一个默认值
                    dslijia.AddressLine = "默认地址";
                }
                else
                {
                    dslijia.AddressLine = dsdealer.AddressLine;
                }
                dslijia.BankAccount = dsdealer.BankAccount;
                dslijia.CellPhone = dsdealer.CellPhone;

                //如果市为空值
                if (string.IsNullOrWhiteSpace(dsdealer.City))
                {
                    //添加一个默认值
                    dslijia.City = "深圳市";
                }
                else
                {
                    dslijia.City = dsdealer.City;
                }
                //如果联系人为空值
                if (string.IsNullOrWhiteSpace(dsdealer.Contact))
                {
                    //添加一个默认值
                    dslijia.Contact = dsdealer.MemberName;
                }
                else
                {
                    dslijia.Contact = dsdealer.Contact;
                }

                //如果区为空值
                if (string.IsNullOrWhiteSpace(dsdealer.District))
                {
                    //添加一个默认值
                    dslijia.District = "罗湖区";
                }
                else
                {
                    dslijia.District = dsdealer.District;
                }
                dslijia.Email = dsdealer.Email;
                dslijia.Fax = dsdealer.Fax;
                dslijia.UserName = dsdealer.UserName;
            }
            else
            {
                r.Message = "分销商不存在";
                return Json(r, JsonRequestBehavior.AllowGet);
            }

            try
            {
                //查询本系统是否保存有利嘉分销商会员id    
                var dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealerByOrderSysNo(orderID);
                if (dealer.LiJiaSysNo > 0)
                {
                    //获取同步订单数据
                    LiJiaOrderModel lijiamodel = BLL.Order.SoOrderBo.Instance.GetLiJiaOrderByOrderNo(orderID);
                    lijiamodel.MemberId = dealer.LiJiaSysNo;
                    //上线请注释测试
                    //lijiamodel.Memo = "测试订单";
                    var res = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaOrder(lijiamodel);
                    if (res.Success)
                    {
                        //返回的利嘉erp订单号
                        r.Message = "订单同步成功";
                        r.Status = true;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }
                    else if (!res.Success && res.Message.IndexOf("商品编码不存在或已禁用") > -1)
                    {
                        //有商品不存在
                        //添加商品接口待实现
                        //取出商品的erpID
                        string perpid = res.Message.Replace("商品编码不存在或已禁用", "").Trim();
                        //获取商品信息
                        //var PdProductsLijia = BLL.Product.PdProductBo.Instance.GetEntityLiJiaByErpCode(perpid);
                    }
                    else
                    {
                        r.Message = "订单同步失败";
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    //调接口新增会员
                    var Result = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaMemerber(dslijia);
                    if (Result.Success)
                    {
                        //新增分销商会员成功
                        //查询分销商
                        var dealerinfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealerByOrderSysNo(orderID);
                        if (dealerinfo != null)
                        {
                            dealerinfo.LiJiaSysNo = Result.MemberId;
                            //保存对方返回的会员id
                            int i = BLL.Distribution.DsDealerBo.Instance.UpdateLiJiaSysNo(Result.MemberId, dealerinfo.SysNo);
                        }
                        //获取同步订单数据
                        LiJiaOrderModel lijiamodel = BLL.Order.SoOrderBo.Instance.GetLiJiaOrderByOrderNo(orderID);
                        lijiamodel.MemberId = Result.MemberId;
                        //lijiamodel.MemberId = 348;
                        //上线请注释测试
                        //lijiamodel.Memo = "测试订单";
                        var res = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaOrder(lijiamodel);
                        if (res.Success)
                        {
                            //返回的利嘉erp订单号
                            r.Message = "订单同步成功";
                            r.Status = true;
                            return Json(r, JsonRequestBehavior.AllowGet);
                        }
                        else if (!res.Success && res.Message.IndexOf("商品编码不存在或已禁用") > -1)
                        {
                            //有商品不存在
                            //添加商品接口待实现
                            //取出商品的erpID
                            string perpid = res.Message.Replace("商品编码不存在或已禁用", "").Trim();
                            //获取商品信息
                            //var PdProductsLijia = BLL.Product.PdProductBo.Instance.GetEntityLiJiaByErpCode(perpid);

                        }
                        else
                        {
                            r.Message = "订单同步失败";
                            return Json(r, JsonRequestBehavior.AllowGet);
                        }


                    }
                    else if (!Result.Success && Result.Message.IndexOf("手机号已存在") > -1)
                    {
                        //根据手机号查询利嘉会员信息
                        LiJiaMemberSearch serch = new LiJiaMemberSearch();
                        serch.rules = new List<LiJiaSearch>();
                        LiJiaSearch se = new LiJiaSearch();
                        se.data = dslijia.CellPhone;
                        se.field = "CellPhone";
                        serch.rules.Add(se);
                        LiJiaSearch sels = new LiJiaSearch();
                        sels.data = dslijia.MemberName;
                        sels.field = "MemberName";
                        serch.rules.Add(sels);
                        var res = BLL.Order.LiJiaSoOrderSynchronize.SeachLiJiaMemerber(serch);
                        if (res.Success)
                        {
                            if (res.rows.Count <= 0)
                            {
                                r.Message = "分销商手机号码必须唯一";
                                r.Status = false;
                                return Json(r, JsonRequestBehavior.AllowGet);
                            }
                            var me = res.rows.First(i => i.CellPhone.Contains(dslijia.CellPhone));
                            //查询分销商
                            var dealerinfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealerByOrderSysNo(orderID);
                            if (dealerinfo != null)
                            {
                                dealerinfo.LiJiaSysNo = Result.MemberId;
                                //保存对方返回的会员id
                                int i = BLL.Distribution.DsDealerBo.Instance.UpdateLiJiaSysNo(me.MemberId, dealerinfo.SysNo);
                            }
                            //获取同步订单数据
                            LiJiaOrderModel lijiamodel = BLL.Order.SoOrderBo.Instance.GetLiJiaOrderByOrderNo(orderID);
                            lijiamodel.MemberId = me.MemberId;
                            //lijiamodel.MemberId = 348;
                            //上线请注释测试
                            //lijiamodel.Memo = "测试订单";
                            var ret = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaOrder(lijiamodel);
                            if (ret.Success)
                            {
                                //返回的利嘉erp订单号
                                r.Message = "订单同步成功";
                                r.Status = true;
                                return Json(r, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            r.Message = "分销商不存在";
                            r.Status = false;
                            return Json(r, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        r.Message = "会员创建失败";
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        public string MainHttpT(string host, string path, string method, string Token, string querys, string bodys)
        {
            //String querys = "preCarNum=%E4%BA%AC&province=%E5%8C%97%E4%BA%AC";
            //String bodys = "";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;
            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }
            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            //httpRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            httpRequest.ContentType = "application/json; charset=utf-8";
            httpRequest.Method = method;
            httpRequest.Headers.Add("Token", Token);
            //httpRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            //Console.WriteLine(httpResponse.StatusCode);
            //Console.WriteLine(httpResponse.Method);
            //Console.WriteLine(httpResponse.Headers);
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            string retString = reader.ReadToEnd();
            //Console.WriteLine(reader.ReadToEnd());
            //Console.WriteLine("\n");
            reader.Close();
            st.Close();
            return retString;

        }


        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        /// <summary>
        /// 批量审核订单
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2016-10-11 罗远康 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1003601)]
        public ActionResult BatchAuditOrder(string orderSysNos = "")
        {
            Result r = new Result
            {
                Status = false
            };
            List<int> sysNos = null;
            string AuditFailOrder = "";
            if (orderSysNos != "")
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(orderSysNos);
                foreach (int orderID in sysNos)
                {
                    var model = SoOrderBo.Instance.GetEntity(orderID);
                    if (model != null)
                    {
                        if (model.PayStatus == 20 && model.Status == 10)//已支付待审核
                        {
                            #region 修改任务执行者
                            var i = 0;
                            var job = SyJobPoolManageBo.Instance.GetByTask(orderID, 10);
                            if (job != null)
                            {
                                if (job.ExecutorSysNo != CurrentUser.Base.SysNo)//执行者相同不更新
                                {
                                    i = BLL.Sys.SyJobPoolManageBo.Instance.UpdateExecutorSysNo(orderID, 10, CurrentUser.Base.SysNo);
                                    SyJobDispatcherBo.Instance.WriteJobLog(EnumUtil.GetDescription(typeof(SystemStatus.任务对象类型), 10) + "被" + CurrentUser.Base.UserName + "强制执行,任务对象编号:" + orderID + ",原执行人是：" + SyUserBo.Instance.GetUserName(job.ExecutorSysNo), orderID, null, CurrentUser.Base.SysNo);
                                }
                            }
                            else
                            {
                                i = SyJobPoolManageBo.Instance.AssignJobByTaskType(10, orderID, CurrentUser.Base.SysNo);
                                SyJobDispatcherBo.Instance.WriteJobLog(EnumUtil.GetDescription(typeof(SystemStatus.任务对象类型), 10) + "被" + CurrentUser.Base.UserName + "强制执行,任务对象编号:" + orderID + ",原执行人为空", orderID, null, CurrentUser.Base.SysNo);
                            }
                            SoOrderBo.Instance.UpdateOnlineStatusByOrderID(model.SysNo, model.OnlineStatus);//目的是刷新时间戳
                            #endregion

                            #region 审核订单
                            try
                            {
                                using (var tran = new TransactionScope())
                                {
                                    r.Status = SoOrderBo.Instance.AuditSoOrder(orderID, CurrentUser.Base.SysNo);
                                    tran.Complete();
                                }
                            }
                            catch (Exception ex)
                            {
                                r.Status = false;
                                r.Message = ex.Message;
                                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "审核订单：" + r.Message, LogStatus.系统日志目标类型.订单, orderID,
                                    CurrentUser.Base.SysNo);
                            }
                            #endregion
                            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "审核订单", LogStatus.系统日志目标类型.订单, orderID,
                                 CurrentUser.Base.SysNo);
                        }
                        else
                        {
                            r.Status = false;
                        }
                        if (r.Status == false)//将未审核成功的订单号保存起来
                        {
                            AuditFailOrder += orderID + ":";
                        }
                        if (r.Status)
                        {
                            Task.Factory.StartNew(() =>
                            {  
                                //审核成功同步订单
                                SynchronizeOrder(orderID);
                            });
                           
                        }
                    }
                }
                if (AuditFailOrder != "")
                {
                    r.Message = "审核失败订单：" + AuditFailOrder;
                }
            }
            else
            {
                sysNos = new List<int>();
                r.Message = "请选择订单";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量备注
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <param name="pass">备注信息</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2017-5-5 罗勤尧 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult BatchBeizhuOrder(string pass, string orderSysNos = "")
        {
            Result r = new Result
            {
                Status = false
            };
            List<int> sysNos = null;
            string AuditFailOrder = "";
            if (orderSysNos != "")
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(orderSysNos);
                foreach (int orderID in sysNos)
                {
                    var model = SoOrderBo.Instance.GetEntity(orderID);
                    if (model != null)
                    {
                        if (string.IsNullOrWhiteSpace(model.DeliveryRemarks))//已支付待审核
                        {
                            #region 修改任务执行者
                            var i = 0;
                            var job = SyJobPoolManageBo.Instance.GetByTask(orderID, 10);
                            if (job != null)
                            {
                                if (job.ExecutorSysNo != CurrentUser.Base.SysNo)//执行者相同不更新
                                {
                                    i = BLL.Sys.SyJobPoolManageBo.Instance.UpdateExecutorSysNo(orderID, 10, CurrentUser.Base.SysNo);
                                    SyJobDispatcherBo.Instance.WriteJobLog(EnumUtil.GetDescription(typeof(SystemStatus.任务对象类型), 10) + "被" + CurrentUser.Base.UserName + "强制执行,任务对象编号:" + orderID + ",原执行人是：" + SyUserBo.Instance.GetUserName(job.ExecutorSysNo), orderID, null, CurrentUser.Base.SysNo);
                                }
                            }
                            else
                            {
                                i = SyJobPoolManageBo.Instance.AssignJobByTaskType(10, orderID, CurrentUser.Base.SysNo);
                                SyJobDispatcherBo.Instance.WriteJobLog(EnumUtil.GetDescription(typeof(SystemStatus.任务对象类型), 10) + "被" + CurrentUser.Base.UserName + "强制执行,任务对象编号:" + orderID + ",原执行人为空", orderID, null, CurrentUser.Base.SysNo);
                            }
                            SoOrderBo.Instance.UpdateOnlineStatusByOrderID(model.SysNo, model.OnlineStatus);//目的是刷新时间戳
                            #endregion

                            #region 备注订单
                            try
                            {
                                using (var tran = new TransactionScope())
                                {
                                    var s = SoOrderBo.Instance.UpdateOrderDeliveryRemarks(orderID, pass);
                                    if (s > 0)
                                    {
                                        r.Status = true;
                                        tran.Complete();
                                    }
                                    else
                                    {
                                        r.Status = false;
                                        r.Message = "更新失败";
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                r.Status = false;
                                r.Message = ex.Message;
                                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单配送备注：" + r.Message, LogStatus.系统日志目标类型.订单, orderID,
                                    CurrentUser.Base.SysNo);
                            }
                            #endregion
                            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单配送备注", LogStatus.系统日志目标类型.订单, orderID,
                                 CurrentUser.Base.SysNo);
                        }
                        else
                        {
                            #region 修改任务执行者
                            var i = 0;
                            var job = SyJobPoolManageBo.Instance.GetByTask(orderID, 10);
                            if (job != null)
                            {
                                if (job.ExecutorSysNo != CurrentUser.Base.SysNo)//执行者相同不更新
                                {
                                    i = BLL.Sys.SyJobPoolManageBo.Instance.UpdateExecutorSysNo(orderID, 10, CurrentUser.Base.SysNo);
                                    SyJobDispatcherBo.Instance.WriteJobLog(EnumUtil.GetDescription(typeof(SystemStatus.任务对象类型), 10) + "被" + CurrentUser.Base.UserName + "强制执行,任务对象编号:" + orderID + ",原执行人是：" + SyUserBo.Instance.GetUserName(job.ExecutorSysNo), orderID, null, CurrentUser.Base.SysNo);
                                }
                            }
                            else
                            {
                                i = SyJobPoolManageBo.Instance.AssignJobByTaskType(10, orderID, CurrentUser.Base.SysNo);
                                SyJobDispatcherBo.Instance.WriteJobLog(EnumUtil.GetDescription(typeof(SystemStatus.任务对象类型), 10) + "被" + CurrentUser.Base.UserName + "强制执行,任务对象编号:" + orderID + ",原执行人为空", orderID, null, CurrentUser.Base.SysNo);
                            }
                            SoOrderBo.Instance.UpdateOnlineStatusByOrderID(model.SysNo, model.OnlineStatus);//目的是刷新时间戳
                            #endregion

                            #region 备注订单
                            try
                            {
                                using (var tran = new TransactionScope())
                                {
                                    var s = SoOrderBo.Instance.UpdateOrderDeliveryRemarks(orderID, pass + model.DeliveryRemarks);
                                    if (s > 0)
                                    {
                                        r.Status = true;
                                        tran.Complete();
                                    }
                                    else
                                    {
                                        r.Status = false;
                                        r.Message = "更新失败";
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                r.Status = false;
                                r.Message = ex.Message;
                                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单配送备注：" + r.Message, LogStatus.系统日志目标类型.订单, orderID,
                                    CurrentUser.Base.SysNo);
                            }
                            #endregion
                            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单配送备注", LogStatus.系统日志目标类型.订单, orderID,
                                 CurrentUser.Base.SysNo);
                            r.Status = true;
                        }
                        if (r.Status == false)//将未备注成功的订单号保存起来
                        {
                            AuditFailOrder += orderID + ":";
                        }
                    }
                }
                if (AuditFailOrder != "")
                {
                    r.Message = "配送备注失败订单：" + AuditFailOrder;
                }
            }
            else
            {
                sysNos = new List<int>();
                r.Message = "请选择订单";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 汇付批量支付
        /// </summary>
        /// <returns>2017-11-21 罗熙 创建</returns>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult HuiFuPay(string orderSysNos = "")
        {
            Result r = new Result
            {
                Status = false
            };
            List<int> sysNos = null;
            string AuditFailOrder = "";
            if (orderSysNos != "")
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(orderSysNos);
                foreach (int orderID in sysNos)
                {
                    var model = SoOrderBo.Instance.GetEntity(orderID);
                    if (model.PayStatus == 10)  //未支付
                    {
                        BLL.ApiPay.Chinapnr.PayProvider a = new BLL.ApiPay.Chinapnr.PayProvider();
                        r = a.ApplyToCustoms(model);
                    }
                    else
                    {
                        r.Message = "被勾选的队列中必须选择未支付的订单！";
                        break;
                    }
                    //if (r.Status==false)
                    //{   
                    //    r.Message = "支付失败";
                    //}
                }
            }
            else
            {
                sysNos = new List<int>();
                r.Message = "请选择订单";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取海带订单
        /// </summary>
        /// <remarks>2017-7-5 罗勤尧 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult GetHaiDaiOrder(int? id, ParaDsDealerMallFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                var model = new PagedList<CBDsDealerMall>();

                var modelRef = new Pager<CBDsDealerMall> { CurrentPage = id ?? 1, PageSize = 999 };
                DsDealerMallBo.Instance.GetDsDealerMallList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                var result = new MyResult<IList<CBDsDealerMall>>();
                result.Status = 1;
                result.Data = modelRef.Rows;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            //获取所有商城类型
            var modelDs = DsMallTypeBo.Instance.GetDsMallTypeList("", null, null);
            ViewBag.dsMallType = modelDs;
            return View();
        }
        /// <summary>
        /// 取消审核订单
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-07-03 朱成果 创建</remarks>
        /// <remarks>2013-11-15 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1003601)]
        public ActionResult UnAuditOrder(int orderID)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "取消审核订单", LogStatus.系统日志目标类型.订单, orderID, CurrentUser.Base.SysNo);
            bool flg = SoOrderBo.Instance.CancelAuditedOrder(orderID, CurrentUser.Base.SysNo);
            return Json(new { success = flg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作废订单
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-06-20 朱成果 创建</remarks>
        /// <remarks>2013-11-15 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1003501)]
        public ActionResult CancelOrder(int orderID)
        {
            var obsoleteReason = Request["obsoleteReason"];

            if (string.IsNullOrWhiteSpace(obsoleteReason))
            {
                return Json(new { success = false, message = "请输入作废原因。" }, JsonRequestBehavior.AllowGet);
            }


            string msg = string.Empty;
            bool flg = false;
            using (var tran = new TransactionScope())
            {
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "作废订单。作废原因:" + obsoleteReason, LogStatus.系统日志目标类型.订单, orderID, CurrentUser.Base.SysNo);
                flg = SoOrderBo.Instance.CancelSoOrder(orderID, CurrentUser.Base.SysNo, OrderStatus.销售单作废人类型.后台用户,
                    ref msg, obsoleteReason);
                tran.Complete();
            }
            //作废的已支付的订单需要创建退款单 （待实现)
            return Json(new { success = flg, message = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建出库单
        /// </summary>
        /// <param name="data">出库商品列表</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="deliveryTypeSysNo">出库单配送方式</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-06-20 朱成果 创建</remarks>
        /// <remarks>2013-11-15 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult CreateOutStock(IList<SoOrderItem> data, int warehouseSysNo, int? deliveryTypeSysNo)
        {
            Result res = new Result { Status = true };
            var identity = string.Format("创建订单{0}的出库单", data[0].OrderSysNo);
            if (YwbUtil.Enter(identity) == false)
            {
                res.Status = false;
                res.Message = "其它人正在处理这个订单，请稍后重试";
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (var tran = new TransactionScope())
                {
                    var outStock = SoOrderBo.Instance.CreateOutStock(data, warehouseSysNo, CurrentUser.Base, deliveryTypeSysNo);
                    tran.Complete();
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建出库单", LogStatus.系统日志目标类型.出库单, outStock.SysNo, CurrentUser.Base.SysNo);
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
            }
            //如果创建出库单成功
            //20170524添加
            if (res.Status)
            {
                //同步利嘉出库单

            }
            YwbUtil.Exit(identity);

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 利嘉出库单同步
        /// </summary>
        /// <param name="data">出库商品列表</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="deliveryTypeSysNo">出库单配送方式</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public ActionResult OrderInfoTracking(IList<SoOrderItem> data, int warehouseSysNo, int? deliveryTypeSysNo)
        {
            //SysLog.Instance.Info(LogStatus.系统日志来源.后台, "利嘉出库单同步操作", LogStatus.系统日志目标类型.订单, orderID,
            //                        CurrentUser.Base.SysNo);
            Result r = new Result
            {
                Status = false
            };

            //根据仓库编号获取仓库信息
            var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo);

            //创建新增仓库对象
            LiJiaStoreAdd StoreAdd = null;

            if (warehouse != null)
            {

                StoreAdd = new LiJiaStoreAdd();
                StoreAdd.StoreName = warehouse.WarehouseName;
            }
            else
            {
                r.Message = "仓库不存在";
                return Json(r, JsonRequestBehavior.AllowGet);
            }

            try
            {
                //本系统是否保存有利嘉仓库id    
                //var dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealerByOrderSysNo(data[0].OrderSysNo);
                if (!string.IsNullOrWhiteSpace(warehouse.LiJiaStoreCode))
                {
                    //获取同步出库数据
                    //LiJiaPurchaseInfo lijiamodel = BLL.Order.SoOrderBo.Instance.GetLiJiaOrderByOrderNo(data[0].OrderSysNo);
                    //lijiamodel.MemberId = dealer.LiJiaSysNo;
                    ////上线请注释测试
                    ////lijiamodel.Memo = "测试订单";
                    //var res = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaOrder(lijiamodel);
                    //if (res)
                    //{
                    //    //返回的利嘉erp订单号
                    //    r.Message = "订单同步成功";
                    //    r.Status = true;
                    //    return Json(r, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    //    r.Message = "订单同步失败";
                    //    return Json(r, JsonRequestBehavior.AllowGet);
                    //}

                }
                else
                {
                    //调接口新增仓库
                    string StoreCode = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaStore(StoreAdd);
                    if (!string.IsNullOrWhiteSpace(StoreCode))
                    {
                        //新增仓库成功
                        //保存对方返回的仓库编号
                        BLL.Warehouse.WhWarehouseBo.Instance.UpdateLiJiaStoreCode(warehouse.SysNo, StoreCode);



                    }
                }


                //else
                //{
                //    //查询分销商会员接口
                //    string searchpath = "member.search";
                //    //构建查询参数
                //    //LiJiaMemberSearch lijiasearch = new LiJiaMemberSearch();
                //    //lijiasearch.page = 1;
                //    //lijiasearch.rows = 20;
                //    //lijiasearch.rules = new List<LiJiaSearch>();
                //    //LiJiaSearch search = new LiJiaSearch();
                //    //search.field = "CellPhone";
                //    //search.data = dsdealer.CellPhone;
                //    //lijiasearch.rules.Add(search);
                //    //LiJiaSearch searchT = new LiJiaSearch();
                //    //searchT.field = "UserName";
                //    //searchT.data = dsdealer.UserName;
                //    //lijiasearch.rules.Add(searchT);
                //    //string searchjosn = JsonHelper.ToJson(lijiasearch);
                //    ////访问查询分销商接口
                //    //ResultLiJia searchDsDealer = BLL.Order.LiJiaSoOrderSynchronize.SeachLiJiaMemerber(lijiasearch);
                //    //if (!string.IsNullOrEmpty(searchDsDealer))
                //    //{
                //    //var searchreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(searchDsDealer);
                //    //if (searchreturn.Success)
                //    //{

                //    //查询本系统保存的利嘉分销商会员id    
                //    var dealerinfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealerByOrderSysNo(orderID);
                //    if (dealerinfo != null)
                //    {
                //        //获取同步订单数据
                //        LiJiaOrderModel lijiamodel = BLL.Order.SoOrderBo.Instance.GetLiJiaOrderByOrderNo(orderID);
                //        lijiamodel.MemberId = dealerinfo.LiJiaSysNo;
                //        //上线请注释测试
                //        //lijiamodel.Memo = "测试订单";
                //        var res = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaOrder(lijiamodel);
                //        if (res)
                //        {
                //            //返回的利嘉erp订单号
                //            r.Message = "订单同步成功";
                //            r.Status = true;
                //            return Json(r, JsonRequestBehavior.AllowGet);
                //        }
                //        else
                //        {
                //            r.Message = "订单同步失败";
                //            return Json(r, JsonRequestBehavior.AllowGet);
                //        }
                //}
                // 
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除订单的商品明细
        /// </summary>
        /// <param name="sysNo">销售单销售明细编号</param>
        /// <param name="orderID">销售单编号</param>
        /// <returns>操作结果json</returns>
        /// <remarks>2013-06-24 朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult OrderItemDelete(int sysNo, int orderID)
        {
            bool flg = true;
            string strMessage = string.Empty;
            //业务逻辑实现
            using (var scope = new TransactionScope())
            {
                flg = SoOrderBo.Instance.DeleteOrderItem(orderID, sysNo);
                scope.Complete();
            }
            //获取订单实体
            var data = SoOrderBo.Instance.GetSoOrderAmount(orderID);
            return Json(new { success = flg, message = strMessage, orderData = data, ticks = data.Stamp.Ticks.ToString() }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单商品数量修改
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <param name="quantity">数量</param>
        /// <param name="orderID">订单编号</param>
        /// <param name="changeAmount">调价金额</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-06-24 朱成果 创建</remarks>
        /// <remarks>2013-09-17 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult OrderItemEdit(int sysNo, int quantity, int orderID, decimal changeAmount)
        {
            string strMessage = string.Empty;
            //业务逻辑实现
            bool flg = false;
            using (var scope = new TransactionScope())
            {
                flg = SoOrderBo.Instance.UpdateOrderItemQuantity(sysNo, quantity, orderID, changeAmount);
                scope.Complete();
            }
            //获取订单实体
            var data = SoOrderBo.Instance.GetSoOrderAmount(orderID);
            return Json(new { ticks = data.Stamp.Ticks.ToString(), success = flg, message = strMessage, orderData = data }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 订购商品明细
        /// </summary>
        /// <param name="data">订单详情列表</param>
        /// <returns>操作结果json</returns>
        /// <remarks>2013-06-24 朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1002201)]
        public ActionResult InsertOrderItem(IEnumerable<SoOrderItem> data)
        {
            bool flg = false;
            using (var scope = new TransactionScope())
            {
                flg = SoOrderBo.Instance.InsertOrderItems(data);
                scope.Complete();
            }
            int orderId = data.FirstOrDefault().OrderSysNo;
            //重新获取订购商品明细，并返回
            var list = SoOrderBo.Instance.GetOrderItemsWithErpCodeByOrderSysNo(orderId);
            //获取订单实体
            var order = SoOrderBo.Instance.GetSoOrderAmount(orderId);
            return Json(new { success = flg, data = list, order = order, ticks = order.Stamp.Ticks.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 订单查询
        /// <summary>
        /// 我的订单列表查询
        /// </summary>
        /// <returns>订单列表</returns>
        /// <remarks>2013-06-19 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult MyOrder()
        {
            return View();
        }

        /// <summary>
        /// 加载订单日志信息
        /// </summary>
        /// <param name="soSysNo">订单编号</param>
        /// <returns>订单日志页面</returns>
        /// <remarks>2013-06-19 余勇 创建</remarks> 
        [Privilege(PrivilegeCode.SO1001101, PrivilegeCode.SO1004101)]
        [HttpPost]
        public ActionResult GetOrderLogs(int soSysNo)
        {
            //查询前10条记录
            var list = SoOrderBo.Instance.GetTransactionLogPageData(soSysNo, 1, 10).TData;
            ViewBag.List = list.OrderBy(x => x.OperateDate).ToList();
            SoOrder soOrder = SoOrderBo.Instance.GetEntity(soSysNo);
            ViewBag.OrderImgUrl = SoOrderBo.Instance.GetOrderProductImgUrl(soSysNo);
            ViewBag.CustomerMessage = soOrder.CustomerMessage;
            ViewBag.OrderSource = EnumUtil.GetDescription(typeof(Hyt.Model.WorkflowStatus.OrderStatus.销售单来源), soOrder.OrderSource);
            return PartialView("_MyOrderDetail");
        }
        /// <summary>
        /// 加载备注信息
        /// </summary>
        /// <param name="soSysNo">订单编号</param>
        /// <returns>备注信息页面</returns>
        /// <remarks>2017-05-4 罗勤尧创建 创建</remarks> 
        [Privilege(PrivilegeCode.SO1001101, PrivilegeCode.SO1004101)]
        [HttpPost]
        public ActionResult GetBeizhuLogs(int soSysNo)
        {
            //查询记录

            //Result result = new Result();
            SoOrder soOrder = SoOrderBo.Instance.GetEntity(soSysNo);
            ViewBag.Message = soOrder.DeliveryRemarks;
            return PartialView("_Beizhu");
        }

        /// <summary>
        /// 编辑备注信息
        /// </summary>
        /// <param name="soSysNo">订单编号</param>
        /// <returns>编辑备注信息</returns>
        /// <remarks>2017-05-4 罗勤尧创建 创建</remarks> 
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1002201)]
        [HttpPost]
        public ActionResult EditBeizhuLogs(int soSysNo, string pass)
        {
            Result result = new Result();
            SoOrder soOrder = SoOrderBo.Instance.GetEntity(soSysNo);
            if (soOrder != null)
            {
                soOrder.DeliveryRemarks = pass;
                var b = Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderDeliveryRemarks(soSysNo, pass);
                if (b > 0)
                {
                    result.Status = true;
                    result.Message = "备注成功";
                }
                else
                {
                    result.Status = false;
                    result.Message = "备注失败";
                }
            }
            else
            {
                result.Status = false;
                result.Message = "订单不存在";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询我的订单
        /// </summary>
        /// <param name="model">传入的实体参数</param>
        /// <returns>返回查询列表</returns>
        /// <remarks>2013-06-18 余勇 创建</remarks>
        /// <remarks>2013-06-20 朱家宏 修改了业务层调用方式</remarks>
        [Privilege(PrivilegeCode.SO1003101)]
        public ActionResult SearchMyOrder(CBSoOrder model)
        {
            Pager<CBSoOrder> pager = new Pager<CBSoOrder>();
            pager.CurrentPage = model.id;
            var filter = new ParaOrderFilter { Keyword = model.Condition, ExecutorSysNo = CurrentUser.Base.SysNo, HasAllWarehouse = true };

            switch (model.Status)
            {
                case 1://待审核
                    filter.OrderStatus = (int)OrderStatus.销售单状态.待审核;
                    SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
                    break;
                case 2://待出库订单
                    filter.OrderStatus = (int)OrderStatus.销售单状态.待创建出库单;
                    SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
                    break;
                case 3://今日订单
                    SoOrderBo.Instance.DoSoOrderQueryForToday(ref pager, filter);
                    break;
                case 4://缺货订单
                    SoOrderBo.Instance.DoSoOrderQueryForOutOfStock(ref pager, filter);
                    break;
                default://所有订单
                    SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
                    break;
            }

            return PartialView("_MyOrderPager", pager.Map());
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>订单列表</returns>
        /// <remarks>2013-06-20 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult Query(int? customerSysNo)
        {
            ViewBag.CustomerSysNo = customerSysNo;

            ViewBag.canAudit = false;
            if (AdminAuthenticationBo.Instance.Current.PrivilegeList.HasPrivilege(PrivilegeCode.SO1002201))
            {
                ViewBag.canAudit = true;
            }

            return View();
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="model">传入的实体参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2013-06-20 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult DoQuery(CBSoOrder model)
        {
            var pager = new Pager<CBSoOrder> { CurrentPage = model.id, PageSize = 10, IdRows = new List<int>() };
            var filter = new ParaOrderFilter { Keyword = model.Condition, CustomerSysNo = model.CustomerSysNo, SelectedDealerSysNo = model.SelectedDealerSysNo, SelectedAgentSysNo = model.SelectedAgentSysNo };

            //当前用户对应仓库权限，2016-6-13 王耀发 创建
            filter.HasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            filter.Warehouses = CurrentUser.Warehouses;

            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            switch (model.Status)
            {
                case 1://待审核
                    filter.OrderStatus = (int)OrderStatus.销售单状态.待审核;
                    SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
                    break;
                case 2://待出库订单
                    filter.OrderStatus = (int)OrderStatus.销售单状态.待创建出库单;
                    SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
                    break;
                case 3://今日订单
                    SoOrderBo.Instance.DoSoOrderQueryForToday(ref pager, filter);
                    break;
                case 4://缺货订单
                    SoOrderBo.Instance.DoSoOrderQueryForOutOfStock(ref pager, filter);
                    break;
                case 5://未支付订单
                    filter.PayStatus = (int)OrderStatus.销售单支付状态.未支付;
                    SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
                    break;
                case 6://已支付订单
                    filter.PayStatus = (int)OrderStatus.销售单支付状态.已支付;
                    SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
                    break;
                default://所有订单
                    SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
                    break;
            }

            return PartialView("_OrderQueryPager", pager.Map());
        }

        /// <summary>
        /// 订单高级查询
        /// </summary>
        /// <param name="filter">传入的查询参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2013-06-24 朱家宏 创建</remarks>
        /// <remarks>2017-05-02 罗勤尧 修改</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult DoComplexQuery(ParaOrderFilter filter)
        {
            //当前用户对应仓库权限，2016-6-13 王耀发 创建
            filter.HasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            filter.Warehouses = CurrentUser.Warehouses;

            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            var pager = new Pager<CBSoOrder> { CurrentPage = filter.Id, PageSize = 10 };
            SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
            return PartialView("_OrderQueryPager", pager.Map());
        }

        /// <summary>
        /// 订单高级查询
        /// </summary>
        /// <param name="filter">传入的查询参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2017-09-12 罗勤尧 修改</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult DoComplexQueryNew(ParaOrderFilter filter)
        {
            //当前用户对应仓库权限，2016-6-13 王耀发 创建
            filter.HasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            filter.Warehouses = CurrentUser.Warehouses;

            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            var pager = new Pager<CBSoOrder> { CurrentPage = filter.Id, PageSize = 10 };
            SoOrderBo.Instance.DoSoOrderQueryNew(ref pager, filter);
            return PartialView("_OrderQueryPager", pager.Map());
        }
        /// <summary>
        /// 新增推送订单
        /// </summary>
        /// <param name="id">推送订单</param>
        /// <returns>视图</returns>
        /// <remarks>2015-09-2 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult SendOrderDialog()
        {
            ViewBag.soOrderSysNo = this.Request["soOrderSysNo"];
            ViewBag.OverseaCarrier = this.Request["OverseaCarrier"];
            return View();
        }

        //获取推送运单返回消息
        public string return_msg = "";
        /// <summary>
        /// 推送订单
        /// </summary>
        /// <returns>2015-09-2 王耀发 创建</returns>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult SendOrder()
        {
            int soOrderSysNo = int.Parse(this.Request["soOrderSysNo"]);
            //string OverseaCarrier = this.Request["OverseaCarrier"];
            string OverseaTrackingNo = this.Request["OverseaTrackingNo"];
            string ServiceCode = this.Request["ServiceCode"];
            Result result = new Result();
            Hyt.Model.SendOrderReturn entity = SendOrderReturnBo.Instance.GetEntityByOTrackingNo(OverseaTrackingNo);
            if (entity != null && entity.Code == "0")
            {
                string OverseaCarrier = entity.OverseaCarrier;
                string strJson = "{\"OverseaCarrier\":\"" + OverseaCarrier + "\",\"OverseaTrackingNo\":\"" + OverseaTrackingNo + "\",\"ServiceCode\":\"" + ServiceCode + "\"}";
                var jsonObject = JObject.Parse(strJson);
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("data", strJson);
                ///获得接口返回值
                var sAPIResult = "";
                try
                {
                    sAPIResult = Extra.Logistics.Client.Post(Extra.Logistics.ApiUrl.InboundQuery, data);
                    var json = JObject.Parse(sAPIResult);
                    string Status = json["data"]["Status"].ToString();
                    if (Status == "2") //入库状态：已入库
                    {
                        //推送运单
                        string OutboundOrderNo = OutboundCreate(soOrderSysNo, jsonObject);
                        if (OutboundOrderNo != "")
                        {
                            result.Status = true;
                            result.Message = "推送成功！";
                            Hyt.BLL.Order.SoOrderBo.UpdateOrderSendStatus(soOrderSysNo, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单推送状态.已推送);
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = return_msg;
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "包裹未入库！";
                    }
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                }
            }
            else
            {
                result.Status = false;
                result.Message = "包裹单不存在，请先推送包裹！";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 创建运单
        /// </summary>
        /// <param name="soOrderSysNo"></param>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        /// <remarks>2015-09-2 王耀发 创建</remarks>
        public string OutboundCreate(int soOrderSysNo, JObject jsonObject)
        {
            SoReceiveAddress srEnity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(soOrderSysNo);
            string IDCardNo = "";
            string IDCardImgs = "";
            string IdentityCardImageURL1 = "";
            string IdentityCardImageURL2 = "";
            if (!string.IsNullOrEmpty(srEnity.IDCardNo))
            {
                IDCardNo = srEnity.IDCardNo;
            }
            if (!string.IsNullOrEmpty(srEnity.IDCardImgs))
            {
                IDCardImgs = srEnity.IDCardImgs;
                string[] img = IDCardImgs.Split(';');
                //获得对应身份证，正反面
                string FileServer = Hyt.BLL.Config.Config.Instance.GetAttachmentConfig().FileServer;
                if (!string.IsNullOrEmpty(img[0]))
                {
                    IdentityCardImageURL1 = FileServer + img[0];
                }
                if (!string.IsNullOrEmpty(img[1]))
                {
                    IdentityCardImageURL2 = FileServer + img[1];
                }
            }

            BsArea DistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(srEnity.AreaSysNo);
            string District = DistrictEntity.AreaName;
            BsArea CityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(DistrictEntity.ParentSysNo);
            string City = CityEntity.AreaName;
            BsArea ProvinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(CityEntity.ParentSysNo);
            string Province = ProvinceEntity.AreaName;

            SoOrder SData = Hyt.BLL.Order.SoOrderBo.Instance.GetEntityNoCache(soOrderSysNo);

            WhWarehouse WDate = Hyt.BLL.Warehouse.WhWarehouseBo.GetEntity(SData.DefaultWarehouseSysNo);
            string WarehouseId = WDate.ErpCode;

            string PhoneNumber = "";
            if (!string.IsNullOrEmpty(srEnity.PhoneNumber))
            {
                PhoneNumber = srEnity.PhoneNumber;
            }
            else
            {
                PhoneNumber = "";
            }

            string MobilePhoneNumber = "";
            if (!string.IsNullOrEmpty(srEnity.MobilePhoneNumber))
            {
                MobilePhoneNumber = srEnity.MobilePhoneNumber;
            }
            else
            {
                MobilePhoneNumber = "";
            }

            string ZipCode = "";
            if (!string.IsNullOrEmpty(srEnity.ZipCode))
            {
                ZipCode = srEnity.ZipCode;
            }
            else
            {
                ZipCode = "";
            }
            string ServiceCode = jsonObject["ServiceCode"].ToString();

            string strJson = "{\"WarehouseId\":\"" + WarehouseId + "\",\"CustomerReference\":\"\",\"ServiceCode\":\"" + ServiceCode + "\",\"ServiceChannelCode\":\"\",\"IsExtractInvoice\":false,\"Consignee\":\"" + srEnity.Name + "\",\"Phone\":\"" + (PhoneNumber + "    " + MobilePhoneNumber) + "\",\"Province\":\"" + Province + "\",";
            strJson += "\"City\":\"" + City + "\",\"District\":\"" + District + "\",\"Postcode\":\"" + ZipCode + "\",\"Address\":\"" + srEnity.StreetAddress + "\",\"IdentityCard\":\"" + IDCardNo + "\",\"IdentityCardImageURL1\":\"" + IdentityCardImageURL1 + "\",\"IdentityCardImageURL2\":\"" + IdentityCardImageURL2 + "\",\"Remark\":\"\",";

            string OverseaCarrier = jsonObject["OverseaCarrier"].ToString();
            string OverseaTrackingNo = jsonObject["OverseaTrackingNo"].ToString();
            strJson += "\"CommodityList\":";
            strJson += "[";
            string str = "";
            int i = 0;

            IList<CBSoOrderItem> datao = SoOrderBo.Instance.GetCBOrderItemsByOrderId(soOrderSysNo);

            foreach (CBSoOrderItem item in datao)
            {
                if (i > 0)
                {
                    str += ",{";
                }
                else
                {
                    str += "{";
                }
                str += "\"SKU\":\"" + item.ErpCode.ToString();
                str += "\",\"UPC\":\"" + item.BarCode.ToString();
                str += "\",\"CommodityName\":\"" + item.ProductName;
                str += "\",\"Quantity\":" + item.Quantity.ToString();
                str += ",\"CustomerReference\":\"\",\"OverseaTrackingNo\":\"" + OverseaTrackingNo + "\",\"OverseaCarrier\":\"" + OverseaCarrier + "\"";
                str += "}";
                i++;
            }
            strJson += str;
            strJson += "]}";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("data", strJson);
            //获得接口返回值
            string OutboundOrderNo = "";
            string Message = "";
            var sAPIResult = "";
            try
            {
                sAPIResult = Extra.Logistics.Client.Post(Extra.Logistics.ApiUrl.OutboundCreate, data);
                var json = JObject.Parse(sAPIResult);
                OutboundReturn m = new OutboundReturn();
                m.soOrderSysNo = soOrderSysNo;
                m.Code = json["code"].ToString();
                m.Msg = json["msg"].ToString();
                if (m.Code == "0")
                {
                    m.OutboundOrderNo = json["OutboundOrderNo"].ToString();
                    Hyt.BLL.Order.OutboundReturnBo.Instance.InsertOutboundReturn(m, 1);
                }
                if (m.Code == "1")
                {
                    m.OutboundOrderNo = "";
                }
                //获取返回值
                return_msg = m.Msg;
                OutboundOrderNo = m.OutboundOrderNo;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return OutboundOrderNo;
        }
        #endregion

        #region 推送订单返回
        /// <summary>
        /// 推送订单返回查询
        /// </summary>
        /// <returns>推送订单返回列表</returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SOR1001)]
        public ActionResult SendOrderReturnList()
        {
            return View();
        }
        /// <summary>
        /// 分页获取推送订单返回
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>推送订单返回列表</returns>
        /// <remarks>2015-09-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SOR1001)]
        public ActionResult DoSendOrderReturnQuery(ParaSendOrderReturnFilter filter)
        {
            filter.PageSize = 10;
            var pager = SendOrderReturnBo.Instance.GetSendOrderReturnList(filter);
            var list = new PagedList<SendOrderReturn>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_SendOrderReturnPager", list);
        }
        /// <summary>
        /// 查询推送订单的物流情况
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-09-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SOR1001)]
        public ActionResult SendORQueryDetail()
        {
            string OverseaCarrier = this.Request["OverseaCarrier"];
            string OverseaTrackingNo = this.Request["OverseaTrackingNo"];
            string strJson = "{\"OverseaCarrier\":\"" + OverseaCarrier + "\",\"OverseaTrackingNo\":\"" + OverseaTrackingNo + "\"}";
            Result result = new Result();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("data", strJson);
            ///获得接口返回值
            var sAPIResult = "";
            try
            {
                sAPIResult = Extra.Logistics.Client.Post(Extra.Logistics.ApiUrl.InboundQuery, data);
                result.Message = sAPIResult;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询返回的明细记录
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.SOR1001)]
        public ActionResult QueryDetailDialog()
        {
            ViewBag.OverseaCarrier = this.Request["OverseaCarrier"];
            ViewBag.OverseaTrackingNo = this.Request["OverseaTrackingNo"];
            return View();
        }
        #endregion

        #region 一号仓包裹追踪
        [Privilege(PrivilegeCode.SOR1002)]
        public ActionResult OutboundReturnList()
        {
            return View();
        }
        /// <summary>
        /// 一号仓包裹追踪
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.SOR1002)]
        public ActionResult QueryOutboundDetailDialog()
        {
            ViewBag.OutboundOrderNo = this.Request["OutboundOrderNo"];
            return View();
        }
        [Privilege(PrivilegeCode.SOR1002)]
        public ActionResult DoOutboundReturnQuery(ParaOutboundReturnFilter filter)
        {
            filter.PageSize = 10;
            var pager = OutboundReturnBo.Instance.GetOutboundReturnList(filter);
            var list = new PagedList<OutboundReturn>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_OutboundReturnPager", list);
        }
        /// <summary>
        /// 一号仓包裹追踪
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-09-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SOR1001)]
        public ActionResult OutboundQueryDetail()
        {
            string OutboundOrderNo = this.Request["OutboundOrderNo"];
            string strJson = "{\"OutboundOrderNo\":\"" + OutboundOrderNo + "\"}";
            Result result = new Result();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("data", strJson);
            ///获得接口返回值
            var sAPIResult = "";
            try
            {
                sAPIResult = Extra.Logistics.Client.Post(Extra.Logistics.ApiUrl.OutboundQueryDetail, data);
                result.Message = sAPIResult;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 订单选择 朱家宏 创建

        /// <summary>
        /// 订单选择
        /// </summary>
        /// <param  name="onlinePay">是否在线支付</param>
        /// <returns>订单视图</returns>
        /// <remarks>2013-06-24 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005803)]
        public ActionResult Selector(bool? onlinePay)
        {
            ViewBag.onlinePay = onlinePay;
            return View("SelectOrder");
        }

        /// <summary>
        /// 订单选择 分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="onlinePay">是否在线支付</param>
        /// <returns>分页视图</returns>
        /// <remarks>2013-06-24 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005803)]
        public ActionResult SelectorQuery(ParaOrderFilter filter, string onlinePay)
        {
            if (!string.IsNullOrEmpty(onlinePay) && onlinePay.ToLower() == "true")
            {
                filter.PaymentType = (int)BasicStatus.支付方式类型.预付;
                //filter.OrderStatus = (int)Model.WorkflowStatus.OrderStatus.销售单状态.已完成;
            }
            var pager = new Pager<CBSoOrder>() { CurrentPage = filter.Id };
            filter.NonInvalidStatus = 1;   //过滤作废的订单 余勇 2014/1/2
            filter.HasAllWarehouse = true;
            //是否绑定所有经销商 王耀发 2016-2-22 创建
            filter.IsBindAllDealer = true;
            //选中所有代理商  王耀发 2016-2-22 创建
            filter.SelectedAgentSysNo = -1;
            SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);

            var list = new PagedList<CBSoOrder>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };

            return PartialView("_SelectOrderPager", list);
        }


        #endregion

        #region 私有类，用于反序列化json数据，
        /// <summary>
        /// 该类用于OrderController.AddProductToCartAndReturnHtml  OrderController.EditOrderAddProduct方法序列化json数据
        /// </summary>
        /// <remarks>2013-06-24 朱家宏 创建</remarks>
        private class AddProductToCartAndReturnHtmlModel
        {
            /// <summary>
            /// 商品系统编号
            /// </summary>
            public int ProductSysNo { get; set; }
            /// <summary>
            /// 组系统编号(组合,团购主表系统编号)
            /// </summary>
            public int GroupSysNo { get; set; }
            /// <summary>
            /// 促销系统编号
            /// </summary>
            public int PromotionSysNo { get; set; }

        }
        #endregion

        #region 仓库库存
        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="erpCode">商品编码</param>
        /// <param name="erpWarehouseSysNo">仓库编号</param>
        /// <returns>操作结果json</returns>
        /// <remarks>2013-10-14 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1003601)]
        public ActionResult GetInventory(string[] erpCode, string erpWarehouseSysNo)
        {
            var result = new List<Extra.Erp.Model.Inventory>();
            System.Threading.CancellationTokenSource tks = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken token = tks.Token;
            Task t = new Task(() =>
            {
                //System.Threading.Thread.Sleep(20000);
                result = SoOrderBo.Instance.GetErpInventory(erpCode, erpWarehouseSysNo);
            }, token);
            t.Start();
            if (!t.Wait(4000)) //超时任务取消4 秒
            {
                tks.Cancel();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 配送地图
        /// <summary>
        /// 订单配送地图
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="cityNo">城市编号</param>
        /// <param name="x">经度</param>
        /// <param name="y">纬度</param>
        /// <returns></returns>
        /// <remarks>2014-05-29 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.CM1005802)]
        public ActionResult ShowBaiduMap(string cityName, int? cityNo, double? x, double? y)
        {
            Hyt.Model.Transfer.CBBaiduMap model = new Hyt.Model.Transfer.CBBaiduMap();
            model.CityName = cityName;
            model.CityNo = cityNo;
            model.LocalX = x;
            model.LocalY = y;
            return View(model);
        }
        #endregion

        #region 升仓赠品管理
        /// <summary>
        /// 升仓赠品管理
        /// </summary>
        /// <param name="orderid">订单编号</param>
        /// <returns></returns>
        /// <remarks>2014-07-03 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SO1003601)]
        public ActionResult MallGifts(int orderid)
        {
            ViewBag.OrderID = orderid;
            ViewBag.TransactionSysNo = Request.QueryString["TransactionSysNo"];
            var items = Hyt.BLL.Order.SoOrderBo.Instance.GetMallGiftItems(orderid);
            return View("MallGifts", items);
        }

        /// <summary>
        /// 选择赠品
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        /// <remarks>2014-07-03 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SO1003601)]
        public ActionResult SearchMallGift(string keyword)
        {
            List<Hyt.Model.UpGrade.UpGradeProduct> lst = new List<Hyt.Model.UpGrade.UpGradeProduct>();
            //返回结果
            var pager = new Hyt.Model.Pager<ParaProductSearchFilter>();
            pager.CurrentPage = 1;
            pager.PageSize = 13;
            //设置查询条件
            pager.PageFilter = new ParaProductSearchFilter
            {
                ProductName = keyword,
                ErpCode = keyword
            };

            PagedList<ParaProductSearchFilter> list; //分页结果集
            Hyt.BLL.Product.PdProductBo.Instance.ProductSelectorProductSearch(ref pager, out list); //执行查询
            if (list != null && list.TData != null)
            {
                lst = list.TData.Select(m => new Hyt.Model.UpGrade.UpGradeProduct()
                {
                    HytProductCode = m.ErpCode,
                    HytProductSysNo = m.SysNo,
                    HytProductName = m.ProductName,
                    HytPrice = m.Price

                }).ToList();
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存升舱赠品
        /// </summary>
        /// <param name="orderid">订单编号</param>
        /// <param name="transactionSysNo">事物编号</param>
        /// <param name="items">明细编号</param>
        /// <returns></returns>
        /// <remarks>2014-07-03 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SO1003601)]
        [HttpPost]
        public ActionResult SaveMallGift(int orderid, string transactionSysNo, List<Hyt.Model.Transfer.CBSoOrderItem> items)
        {
            Result res = new Result() { Status = true };
            try
            {
                using (var tran = new TransactionScope())
                {
                    var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(orderid);
                    if (order == null || order.Status != (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核)
                    {
                        throw new HytException("销售单当前状态不是待审核状态");
                    }
                    Hyt.BLL.Order.SoOrderBo.Instance.SaveMallGift(orderid, transactionSysNo, items, CurrentUser.Base);
                    tran.Complete();
                }
            }
            catch (HytException e)
            {
                res.Status = false;
                res.Message = e.Message;
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = "发生错误,操作失败，请联系系统管理员";
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "编辑升舱订单信息出错", LogStatus.系统日志目标类型.订单, orderid, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 导出订单
        /// <summary>
        /// 导出订单模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public void ExportTemplate()
        {
            ExportExcel(@"\Templates\Excel\SoOrderNew.xls", "订单导入模板");
        }
        /// <summary>
        /// 导出excel模板文件
        /// </summary>
        /// <param name="tmpPath">模板路径</param>
        /// <param name="excelFileName">导出文件名</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        private void ExportExcel(string tmpPath, string excelFileName)
        {
            ExcelUtil.ExportFromTemplate(new List<object>(), tmpPath, 1, excelFileName, null, false);
        }
        /// <summary>
        /// 导出订单
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <remarks>2016-2-21 王耀发 创建</remarks>
        //[HttpPost]
        [Privilege(PrivilegeCode.SO1001101)]
        public void ExportOrders(string orderSysNos = "")
        {
            List<int> sysNos = null;
            if (orderSysNos != "")
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(orderSysNos);
            }
            else
            {
                sysNos = new List<int>();
            }
            if (sysNos != null)
            {
                SoOrderBo.Instance.ExportOrdersPic(sysNos, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
            }
        }

        /// <summary>
        /// 导出订单-普通查询
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <remarks>2016-3-16 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public void ExportOrdersByDoSearch()
        {
            int Status = int.Parse(this.Request["Status"]);

            string Condition = this.Request["Condition"];
            string SelectedAgentSysNo = this.Request["SelectedAgentSysNo"];
            string SelectedDealerSysNo = this.Request["SelectedDealerSysNo"];
            var filter = new ParaOrderFilter
            {
                Keyword = Condition,
                SelectedDealerSysNo = int.Parse(SelectedDealerSysNo),
                SelectedAgentSysNo = int.Parse(SelectedAgentSysNo)
            };
            switch (Status)
            {
                case 1://待审核
                    filter.OrderStatus = (int)OrderStatus.销售单状态.待审核;
                    break;
                case 2://待出库订单
                    filter.OrderStatus = (int)OrderStatus.销售单状态.待创建出库单;
                    break;
                case 3://今日订单
                    filter.BeginDate = DateTime.Today;
                    break;
                case 4://缺货订单
                    //为0表示为缺货订单查询
                    if (filter.StoreSysNoList == null)
                        filter.StoreSysNoList = new List<int>();
                    filter.StoreSysNoList.Add(0);
                    break;
                case 5://未支付订单
                    filter.PayStatus = (int)OrderStatus.销售单支付状态.未支付;
                    break;
                case 6://已支付订单
                    filter.PayStatus = (int)OrderStatus.销售单支付状态.已支付;
                    break;
                default://所有订单
                    break;
            }

            //当前用户对应仓库权限，2016-6-13 王耀发 创建
            filter.HasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            filter.Warehouses = CurrentUser.Warehouses;

            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            SoOrderBo.Instance.ExportOrdersByDoSearchPic(filter, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }

        /// <summary>
        /// 导出订单-高级查询
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <remarks>2016-3-16 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public void ExportOrdersByDoComplexSearch()
        {
            string ErpCode = this.Request["ErpCode"];
            string BeginDate = this.Request["BeginDate"];
            DateTime? dateBeginDate;
            if (BeginDate == "")
            {
                dateBeginDate = null;
            }
            else
            {
                dateBeginDate = DateTime.Parse(BeginDate);
            }
            string EndDate = this.Request["EndDate"];
            DateTime? dateEndDate;
            if (EndDate == "")
            {
                dateEndDate = null;
            }
            else
            {
                dateEndDate = DateTime.Parse(EndDate);
            }
            string PayBeginDate = this.Request["PayBeginDate"];
            DateTime? datePayBeginDate;
            if (PayBeginDate == "")
            {
                datePayBeginDate = null;
            }
            else
            {
                datePayBeginDate = DateTime.Parse(PayBeginDate);
            }
            string PayEndDate = this.Request["PayEndDate"];
            DateTime? datePayEndDate;
            if (PayEndDate == "")
            {
                datePayEndDate = null;
            }
            else
            {
                datePayEndDate = DateTime.Parse(PayEndDate);
            }

            string SelectedAgentSysNo = this.Request["SelectedAgentSysNo"];
            string SelectedDealerSysNo = this.Request["SelectedDealerSysNo"];
            string OrderSource = this.Request["OrderSource"];
            int? dOrderSource;
            if (OrderSource == "")
            {
                dOrderSource = null;
            }
            else
            {
                dOrderSource = int.Parse(OrderSource);
            }

            string OrderStatus = this.Request["OrderStatus"];
            int? dOrderStatus;
            if (OrderStatus == "")
            {
                dOrderStatus = null;
            }
            else
            {
                dOrderStatus = int.Parse(OrderStatus);
            }

            string PayTypeSysNo = this.Request["PayTypeSysNo"];
            int? dPayTypeSysNo;
            if (PayTypeSysNo == "")
            {
                dPayTypeSysNo = null;
            }
            else
            {
                dPayTypeSysNo = int.Parse(PayTypeSysNo);
            }
            string DeliveryTypeSysNo = this.Request["DeliveryTypeSysNo"];
            int? dDeliveryTypeSysNo;
            if (DeliveryTypeSysNo == "")
            {
                dDeliveryTypeSysNo = null;
            }
            else
            {
                dDeliveryTypeSysNo = int.Parse(DeliveryTypeSysNo);
            }

            string ReceiveName = this.Request["ReceiveName"];
            if (ReceiveName == "")
            {
                ReceiveName = null;
            }

            string ReceiveTel = this.Request["ReceiveTel"];
            if (ReceiveTel == "")
            {
                ReceiveTel = null;
            }

            string Auditor = this.Request["Auditor"];
            if (Auditor == "")
            {
                Auditor = null;
            }

            string MinOrderAmount = this.Request["MinOrderAmount"];
            decimal dAmount = 0;
            decimal? dMinOrderAmount = 0;

            if (decimal.TryParse(MinOrderAmount, out dAmount))
            {
                dMinOrderAmount = decimal.Parse(MinOrderAmount);
            }
            else
            {
                dMinOrderAmount = null;
            }
            string MaxOrderAmount = this.Request["MaxOrderAmount"];
            decimal? dMaxOrderAmount = 0;

            if (decimal.TryParse(MaxOrderAmount, out dAmount))
            {
                dMaxOrderAmount = decimal.Parse(MaxOrderAmount);
            }
            else
            {
                dMaxOrderAmount = null;
            }
            string WarehouseSysNo = this.Request["WarehouseSysNo"];
            int? dWarehouseSysNo;
            if (WarehouseSysNo == "")
            {
                dWarehouseSysNo = null;
            }
            else
            {
                dWarehouseSysNo = int.Parse(WarehouseSysNo);
            }
            var filter = new ParaOrderFilter
            {
                ErpCode = ErpCode,
                BeginDate = dateBeginDate,
                EndDate = dateEndDate,
                PayBeginDate = datePayBeginDate,
                PayEndDate = datePayEndDate,
                SelectedDealerSysNo = int.Parse(SelectedDealerSysNo),
                SelectedAgentSysNo = int.Parse(SelectedAgentSysNo),
                OrderSource = dOrderSource,
                OrderStatus = dOrderStatus,
                PayTypeSysNo = dPayTypeSysNo,
                DeliveryTypeSysNo = dDeliveryTypeSysNo,
                ReceiveName = ReceiveName,
                ReceiveTel = ReceiveTel,
                Auditor = Auditor,
                MinOrderAmount = dMinOrderAmount,
                MaxOrderAmount = dMaxOrderAmount,
                WarehouseSysNo = dWarehouseSysNo
            };

            //当前用户对应仓库权限，2016-6-13 王耀发 创建
            filter.HasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            filter.Warehouses = CurrentUser.Warehouses;

            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            SoOrderBo.Instance.ExportOrderListByDoComplexSearchPic(filter, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }
        #endregion

        /// <summary>
        /// 获取流配送信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-04-09 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.CM1005902)]
        public ActionResult LogisticsTracking(int orderId, int warehouseId)
        {
            var result = new Hyt.Model.Result<string>
            {
                Status = false
            };

            try
            {
                int logistics = 0;
                var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(warehouseId);
                if (warehouse != null)
                {
                    logistics = warehouse.Logistics;
                }
                ViewBag.Logistics = logistics;
                //var logisticeIns = BLL.ApiFactory.ApiProviderFactory.GetLogisticsInstance(logistics);
                //result = logisticeIns.GetLogisticsTracking(orderId);
                //if (result.Status == true)
                //{
                //    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(1, 3, orderId);
                //}
                //BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单" + orderId + "推送到物流：" + result.Message, LogStatus.系统日志目标类型.订单, orderId,
                //    CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "订单" + orderId + "查询物流跟踪信息失败。", ex);
            }

            return View("_LogisticsTracking", result);
        }

        /// <summary>
        /// 加载订单物流
        /// </summary>
        /// <param name="soSysNo">订单编号</param>
        /// <returns>订单物流</returns>
        /// <remarks>2016-04-29 王耀发 创建</remarks> 
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult QueryLogistics(int soSysNo)
        {
            ViewBag.OrderSysNo = soSysNo;
            return View("_QueryLogistics");
        }
        /// <summary>
        /// 追踪
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-04-29 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult QueryLogisticsDetail()
        {
            string soSysNo = this.Request["soSysNo"];
            Result result = new Result();
            using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Express.Contract.IExpressService>())
            {
                try
                {
                    var response = service.Channel.GetExpressDeliveryLocusByOrderSysNo(int.Parse(soSysNo));
                    if (response.IsError)
                    {
                        result.Status = !response.IsError;
                        result.Message = response.ErrMsg;
                    }
                    else
                    {
                        result.Status = !response.IsError;
                        result.Message = response.Data;
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="id">订单发货</param>
        /// <returns>视图</returns>
        /// <remarks>2016-05-14 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult OrderShipDialog()
        {
            ViewBag.soOrderSysNo = this.Request["soOrderSysNo"];
            ViewBag.deliveryTypeName = this.Request["deliveryTypeName"];
            return View();
        }
        /// <summary>
        /// 推送订单到高捷物流
        /// </summary>
        /// <param name="orderId">订单发货</param>
        /// <param name="code">物流类型编码</param>
        /// <returns>视图</returns>
        /// <remarks>2016-05-27 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult OrderLogDialog()
        {
            ViewBag.orderId = this.Request["orderId"];
            ViewBag.code = this.Request["code"];
            SoOrder oEntity = SoOrderBo.Instance.GetEntityNoCache(int.Parse(this.Request["orderId"]));
            CBDsDealer model = DsDealerBo.Instance.GetDsDealer(oEntity.DealerSysNo);
            return View("OrderLogDialog", model);
        }

        /// <summary>
        /// 查看高捷物流
        /// </summary>
        /// <param name="orderId">订单发货</param>
        /// <param name="code">物流类型编码</param>
        /// <returns>订单物流</returns>
        /// <remarks>2016-04-29 王耀发 创建</remarks> 
        [Privilege(PrivilegeCode.SO1001101)]
        public ActionResult QueryGaoJieLogistics()
        {
            ViewBag.orderId = this.Request["orderId"];
            ViewBag.code = this.Request["code"];
            return View("_QueryGaoJieLogistics");
        }

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="id">订单发货</param>
        /// <returns>视图</returns>
        /// <remarks>2016-05-14 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SO1001101)]
        public JsonResult Ship(int orderId, string expressNo)
        {
            var result = new Result
            {
                Status = false
            };

            result = BLL.Order.SoOrderBo.Instance.Ship(orderId, expressNo);
            if (result.Status)
            {
                result.Message = "发货成功！";
                #region 利嘉（你他购）推送订单状态
                int type = 2;//绑数据类型：2：订单
                string state = "7";//订单状态，7：已经发货待确认中
                var model = BLL.Web.MKNitagoDateBindBo.Instance.Select(type, orderId);
                if (model != null)
                {
                    NitagaPostOrderState orderstate = new NitagaPostOrderState();
                    string sAPIResult = orderstate.OrderState(model.NitagoDateSysNo, state);
                    JsonData returnDate = JsonMapper.ToObject(sAPIResult);
                    int backcode = 0;
                    if (((System.Collections.IDictionary)returnDate).Contains("backcode"))
                    {
                        backcode = Convert.ToInt32(returnDate["backcode"].ToString());
                        if (backcode == 0)
                        {
                            if (((System.Collections.IDictionary)returnDate).Contains("error"))
                            {
                                string error = (string)returnDate["error"];
                                BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.外部应用, "推送订单状态：" + orderId + "到你他购失败！" + error, new Exception());
                            }
                        }
                    }
                }
                #endregion

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除跨境物流订单
        /// </summary>
        /// <param name="soOrderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-06 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201)]
        public JsonResult CancelCBOrder(string sysNos)
        {
            Result result = new Result();
            result.Status = true;
            result.Message = "操作失败";

            try
            {
                string[] sysNoList = sysNos.Split(',');
                foreach (string sysno in sysNoList)
                {
                    int logistics = 0;
                    SoOrder order = BLL.Order.SoOrderBo.Instance.GetEntity(Convert.ToInt32(sysno));
                    if (order == null)
                    {
                        result.Message = "订单" + sysno + "不存在";
                        return Json(result);
                    }
                    if (order.CBLogisticsSendStatus == 0)
                    {
                        result.Message = "订单" + sysno + "不是已推送状态";
                        return Json(result);
                    }
                    var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
                    if (warehouse != null)
                    {
                        logistics = warehouse.Logistics;
                    }
                    if (logistics <= 0)
                    {
                        result.Message = "订单" + sysno + "对应的仓库(" + order.DefaultWarehouseSysNo + ")没有选择物流";
                        return Json(result);
                    }
                    // 调用接口
                    result = BLL.ApiFactory.ApiProviderFactory.GetLogisticsInstance(logistics).CancelOrderTrade(Convert.ToInt32(sysno));
                    if (result.Status == true)
                    {
                        // 更新为未推送状态
                        BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(0, 3, Convert.ToInt32(sysno));
                    }
                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单" + sysno + "删除对应的跨境物流订单：" + result.Message, LogStatus.系统日志目标类型.订单, Convert.ToInt32(sysno),
                        CurrentUser.Base.SysNo);
                }
            }
            catch (Exception ex)
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "删除跨境物流订单失败。", ex);
            }

            return Json(result);
        }
    }
}
