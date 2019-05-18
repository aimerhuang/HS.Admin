using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Hyt.BLL.Authentication;
using Hyt.BLL.CRM;
using Hyt.BLL.Logistics;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Admin.Models;
using Hyt.BLL.RMA;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Warehouse;
using Hyt.Util.Validator;
namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 退换货
    /// </summary>
    /// <remarks></remarks>
    public class RMAController : BaseController
    {
        #region 退换货页面 2013-07-11 余勇 创建

        #region 退货
        /// <summary>
        /// 新建退货单
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="department">退换货处理部门：客服10，门店20</param>
        /// <returns>新建退货单页面</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-07-15 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult RmaReturn(int id, int department = 10)
        {
            return RmaCreate(id, department, RmaStatus.RMA类型.售后退货);
        }

        /// <summary>
        /// 新建退货单编辑控件
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="department">退换货处理部门：客服10，门店20</param>
        /// <returns>退货单编辑控件</returns>
        /// <remarks>视图</remarks>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-07-12 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult ReturnEdit(int id, int department)
        {
            var model = GetReturnEditByOrder(id, department);
            return PartialView("_ReturnEdit", model);
        }

        /// <summary>
        /// 退货单审核
        /// </summary>
        /// <param name="id">退换货主表编号</param>
        /// <returns>新建退货单页面</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult ReturnToAudit(int id)
        {
            ViewBag.SysNo = id;
            var rmaReturn = RmaBo.Instance.GetRMA(id);
            if (rmaReturn != null)
            {
                ViewBag.Prompt = (rmaReturn.HandleDepartment == (int)RmaStatus.退换货处理部门.门店 ? "门店" : "") + "退货单审核";
                ViewBag.IsShop = rmaReturn.HandleDepartment == (int)RmaStatus.退换货处理部门.门店;
            }

            return View();
        }

        /// <summary>
        /// 审核退货单编辑控件
        /// </summary>
        /// <param name="id">退货单编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-07-15 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult ReturnEditAudit(int id)
        {
            var model = GetReturnEditByRMA(id);
            return PartialView("_ReturnEdit", model);
        }

        /// <summary>
        /// 退货单查看(待入库、待退款、已完成、作废)
        /// </summary>
        /// <param name="id">退换货主表编号</param>
        /// <returns>退货单查看页面</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-07-15 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult RmaReturnView(int id)
        {
            return RMAView(id);
        }

        /// <summary>
        /// 退货单查看控件
        /// </summary>
        /// <param name="id">退货单编号</param>
        /// <returns>退货单页面实体</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-07-15 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult ReturnView(int id)
        {
            var model = GetReturnViewByRMA(id);

            return PartialView("_ReturnView", model);
        }

        /// <summary>
        /// 退货单订单信息控件
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="rmaid">退换货编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-07-12 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101, PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult RmaOrderInfo(int id, int? rmaid)
        {
            var model = new ReturnOrder();
            var order = SoOrderBo.Instance.GetEntity(id);
            if (order != null)
            {

                var custome = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(order.CustomerSysNo);
                if (custome != null)
                {
                    model.CustomerName = custome.Name;
                    model.MobilePhoneNumber = custome.Account;
                }
                model.CustomerSysNo = order.CustomerSysNo;
                model.SysNo = order.SysNo;
                model.OrderSource = Util.EnumUtil.GetDescription(typeof(OrderStatus.销售单来源), order.OrderSource);
                model.Status = Util.EnumUtil.GetDescription(typeof(OrderStatus.销售单状态), order.Status);
                model.CashPay = order.CashPay;
                model.OrderAmount = order.OrderAmount;
                var invoice = SoOrderBo.Instance.GetFnInvoice(order.InvoiceSysNo);
                if (invoice != null)
                {
                    model.InvoiceTypeSysNo = invoice.InvoiceTypeSysNo;
                    model.InvoiceType = Util.EnumUtil.GetDescription(typeof(OrderStatus.发票类型), invoice.InvoiceTypeSysNo);
                    model.InvoiceTitle = invoice.InvoiceTitle;
                    model.InvoiceRemarks = invoice.InvoiceRemarks;
                    model.InvoiceCode = invoice.InvoiceCode;
                    model.InvoiceNo = invoice.InvoiceNo;
                }
                else
                {
                    model.InvoiceType = "无";
                }
            }
            if (rmaid.HasValue)//退换货编号
            {
                model.RmaOrderSysNo = Hyt.BLL.RMA.RmaBo.Instance.GetRMAOrderSysNo(rmaid.Value);//销售单号
            }
            return PartialView("_RmaOrderInfo", model);
        }
        #endregion

        #region 换货
        /// <summary>
        /// 新建换货单
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="department">退换货处理部门：客服10，门店20</param>
        /// <returns>新建换货单页面</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-07-16 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult RmaExchanges(int id, int department = 10)
        {
            return RmaCreate(id, department, RmaStatus.RMA类型.售后换货);
        }

        /// <summary>
        /// 新建换货单编辑控件
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="department">退换货处理部门：客服10，门店20</param>
        /// <returns>退货单编辑控件</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult ExchangesEdit(int id, int department)
        {
            var model = GetReturnEditByOrder(id, department);
            return PartialView("_ExchangesEdit", model);
        }

        /// <summary>
        /// 换货单审核
        /// </summary>
        /// <param name="id">换货单主表编号</param>
        /// <returns>新建换货单页面</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult ExchangesToAudit(int id)
        {
            ViewBag.SysNo = id;
            var rma = RmaBo.Instance.GetRcReturnEntity(id);
            ViewBag.Prompt = (rma.HandleDepartment == (int)RmaStatus.退换货处理部门.门店 ? "门店" : "") + "换货单审核";
            ViewBag.IsShop = rma.HandleDepartment == (int)RmaStatus.退换货处理部门.门店;
            return View();
        }

        /// <summary>
        /// 审核换货单编辑控件
        /// </summary>
        /// <param name="id">换货单编号</param>
        /// <returns>换货单编辑控件</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult ExchangesAudit(int id)
        {
            var model = GetReturnEditByRMA(id);
            return PartialView("_ExchangesEdit", model);
        }

        /// <summary>
        /// 换货单查看
        /// </summary>
        /// <param name="id">退换货主表编号</param>
        /// <returns>换货单查看页面</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-07-15 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult RmaExchangesView(int id)
        {
            return RMAView(id);
        }

        /// <summary>
        /// 换货单查看控件
        /// </summary>
        /// <param name="id">换货单编号</param>
        /// <returns>换货单页面实体</returns>
        /// <remarks>2013-07-11 余勇 创建</remarks>
        /// <remarks>2013-07-15 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult ExchangesView(int id)
        {
            var model = GetReturnViewByRMA(id);
            return PartialView("_ExchangesView", model);
        }
        #endregion

        #region 退款
        /// <summary>
        /// 新建退款单
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="department">退换货处理部门：客服10，门店20</param>
        /// <returns>新建退款单页面</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        /// <remarks>2017-10-15 罗勤瑶 修改</remarks>
        [Privilege(PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult RmaRefunds(int id, int department = 10)
        {
            return RmaCreate(id, department, RmaStatus.RMA类型.售后退货);
        }

        /// <summary>
        /// 新建退款单编辑控件
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="department">退换货处理部门：客服10，门店20</param>
        /// <returns>退款单编辑控件</returns>
        /// <remarks>视图</remarks>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        /// <remarks>2017-10-15 罗勤瑶 修改</remarks>
        [Privilege(PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult RefundsEdit(int id, int department)
        {
            var model = GetReturnEditByOrder(id, department);
            return PartialView("_ReturnEdit", model);
        }

        /// <summary>
        /// 退款单审核
        /// </summary>
        /// <param name="id">退换货主表编号</param>
        /// <returns>新建退款单页面</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        /// <remarks>2017-10-15 罗勤瑶 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult RefundsToAudit(int id)
        {
            ViewBag.SysNo = id;
            var rmaReturn = RmaBo.Instance.GetRMA(id);
            if (rmaReturn != null)
            {
                ViewBag.Prompt = (rmaReturn.HandleDepartment == (int)RmaStatus.退换货处理部门.门店 ? "门店" : "") + "退货单审核";
                ViewBag.IsShop = rmaReturn.HandleDepartment == (int)RmaStatus.退换货处理部门.门店;
            }

            return View();
        }

        /// <summary>
        /// 审核退款单编辑控件
        /// </summary>
        /// <param name="id">退货单编号</param>
        /// <returns>视图</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        /// <remarks>2017-10-15 罗勤瑶 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult RefundsEditAudit(int id)
        {
            var model = GetReturnEditByRMA(id);
            return PartialView("_ReturnEdit", model);
        }

        /// <summary>
        /// 退款单查看(待入库、待退款、已完成、作废)
        /// </summary>
        /// <param name="id">退换货主表编号</param>
        /// <returns>退款单查看页面</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        /// <remarks>2017-10-15 罗勤瑶 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult RmaRefundsView(int id)
        {
            return RMAView(id);
        }

        /// <summary>
        /// 退款单查看控件
        /// </summary>
        /// <param name="id">退货单编号</param>
        /// <returns>退款单页面实体</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        /// <remarks>2017-10-15 罗勤瑶 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult RefundsView(int id)
        {
            var model = GetReturnViewByRMA(id);

            return PartialView("_ReturnView", model);
        }

        /// <summary>
        /// 退款单订单信息控件
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="rmaid">退款编号</param>
        /// <returns>视图</returns>
        /// <remarks>2017-10-11 罗勤瑶 创建</remarks>
        /// <remarks>2017-10-15 罗勤瑶 修改</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101, PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult RmaRefundsOrderInfo(int id, int? rmaid)
        {
            var model = new ReturnOrder();
            var order = SoOrderBo.Instance.GetEntity(id);
            if (order != null)
            {

                var custome = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(order.CustomerSysNo);
                if (custome != null)
                {
                    model.CustomerName = custome.Name;
                    model.MobilePhoneNumber = custome.Account;
                }
                model.CustomerSysNo = order.CustomerSysNo;
                model.SysNo = order.SysNo;
                model.OrderSource = Util.EnumUtil.GetDescription(typeof(OrderStatus.销售单来源), order.OrderSource);
                model.Status = Util.EnumUtil.GetDescription(typeof(OrderStatus.销售单状态), order.Status);
                model.CashPay = order.CashPay;
                model.OrderAmount = order.OrderAmount;
                var invoice = SoOrderBo.Instance.GetFnInvoice(order.InvoiceSysNo);
                if (invoice != null)
                {
                    model.InvoiceTypeSysNo = invoice.InvoiceTypeSysNo;
                    model.InvoiceType = Util.EnumUtil.GetDescription(typeof(OrderStatus.发票类型), invoice.InvoiceTypeSysNo);
                    model.InvoiceTitle = invoice.InvoiceTitle;
                    model.InvoiceRemarks = invoice.InvoiceRemarks;
                    model.InvoiceCode = invoice.InvoiceCode;
                    model.InvoiceNo = invoice.InvoiceNo;
                }
                else
                {
                    model.InvoiceType = "无";
                }
            }
            if (rmaid.HasValue)//退换货编号
            {
                model.RmaOrderSysNo = Hyt.BLL.RMA.RmaBo.Instance.GetRMAOrderSysNo(rmaid.Value);//销售单号
            }
            return PartialView("_RmaOrderInfo", model);
        }
        #endregion
        #endregion

        #region 退换货业务操作 2013-07-12  余勇 创建
        #region 退货

        /// <summary>
        /// 新建退货单
        /// </summary>
        /// <param name="model">退换货单编辑实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-12 余勇 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult InsertRmaReturn(ReturnEdit model)
        {
            var result = new Result<int>();
            CBRcReturn entity = null;
            SoReceiveAddress pickUpAddress = null;
            entity = GetInsetCBRcReturn(model);
            //取得自动计算字段的值
            // entity = CalRcReturn(entity, model);
            if (entity != null)
            {
                entity.RmaType = (int)RmaStatus.RMA类型.售后退货; //退换货类型:售后换货(10),售后退货(20)
                if (entity.RMAItems.Count == 0)
                {
                    result.StatusCode = -1;
                    result.Message = "退货商品不能为空";
                }
            }
            else
            {
                result.StatusCode = -1;
                result.Message = "该订单不存在";
            }
            if (result.StatusCode != -1)
            {
                try
                {
                    //获取取件地址
                    if (model.PickUpShipTypeSysNo == PickupType.百城当日取件 || model.PickUpShipTypeSysNo == PickupType.普通取件
                        || model.PickUpShipTypeSysNo == PickupType.加急取件 || model.PickUpShipTypeSysNo == PickupType.定时取件)
                    {
                        pickUpAddress = model.PickUpAddress;

                    }

                    using (var tran = new TransactionScope())
                    {
                        result.Data = RmaBo.Instance.InsertRMA(entity, pickUpAddress, null, CurrentUser.Base);
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 新建换货单时取得换货实体
        /// </summary>
        /// <param name="model">退换货单编辑实体</param>
        /// <returns>换货单实体</returns>
        /// <remarks>2016-06-06 余勇 创建</remarks> 
        private CBRcReturn GetInsetRcExchanges(ReturnEdit model)
        {
            CBRcReturn entity = null;
            var rmaItems = new List<RcReturnItem>();
            var order = SoOrderBo.Instance.GetEntity(model.SysNo);
            if (order != null)
            {
                rmaItems.AddRange(from list in model.ReturnEditOutStore
                                  from item in list.ReturnWhStockOutItemEx
                                  where item.ProductSysNo > 0 && item.RmaQuantity > 0
                                  let pdModel = PdProductBo.Instance.GetProductBySysNo(item.ProductSysNo)
                                  select new RcReturnItem
                                  {
                                      ProductSysNo = item.ProductSysNo, //商品编号
                                      RmaQuantity = item.RmaQuantity, //实际退换数量
                                      RmaReason = item.RmaReason, //退换原因
                                      OriginPrice = item.OriginalPrice, //商品原价
                                      RefundProductAmount = item.RefundAmount, //退款金额
                                      StockOutItemSysNo = item.StockOutItemSysNo, //出库单明细编号
                                      ProductName = pdModel.ProductName,
                                      ReturnPriceType = item.ReturnPriceType
                                  });
                //申请单来源
                int source = model.HandleDepartment == (int)RmaStatus.退换货处理部门.门店 ? (int)RmaStatus.退换货申请单来源.门店 : (int)RmaStatus.退换货申请单来源.客服;
                entity = new CBRcReturn
                {
                    OrderSysNo = model.SysNo,                           //订单编号
                    RefundType = model.RefundType,                      //退款方式
                    WarehouseSysNo = model.WarehouseSysNo,              //入库仓库
                    PickupTypeSysNo = model.PickUpShipTypeSysNo,        //取件方式
                    RMARemark = model.RMARemark,                        //会员备注
                    InternalRemark = model.InternalRemark,              //对内备注
                    Status = (int)WarehouseStatus.退换货单状态.待审核,  //状态
                    CreateBy = CurrentUser.Base.SysNo,                  //创建人
                    CreateDate = DateTime.Now,                          //创建时间
                    LastUpdateBy = CurrentUser.Base.SysNo,              //更新人
                    LastUpdateDate = DateTime.Now,                      //更新时间
                    PickUpTime = model.PickUpTime,                      //取件时间段
                    CustomerSysNo = order.CustomerSysNo,                //会员系统编号
                    Source = source,                                    //申请单来源
                    InvoiceSysNo = order.InvoiceSysNo,                  //发票系统编号
                    HandleDepartment = model.HandleDepartment,            //处理部门
                    ShipTypeSysNo = order.DeliveryTypeSysNo,              //配送配送方式编号
                    RefundAccount = model.RefundAccount,                 //退款开户帐号
                    RefundAccountName = model.RefundAccountName,         //退款开户人
                    RefundBank = model.RefundBank,                      //退款开户行
                    RMAItems = rmaItems,                                 //退换货商品明细

                    CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    RefundDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                };
            }
            return entity;
        }

        /// <summary>
        /// 新建退换货单时取得退换货实体
        /// </summary>
        /// <param name="model">退换货单编辑实体</param>
        /// <returns>退换货实体</returns>
        /// <remarks>2013-07-12 余勇 创建</remarks> 
        private CBRcReturn GetInsetCBRcReturn(ReturnEdit model)
        {
            CBRcReturn cbReturn = null;
            decimal fundAmount = 0;
            var rmaItems = new List<RcReturnItem>();
            var order = SoOrderBo.Instance.GetEntity(model.SysNo);
            if (order != null)
            {
                rmaItems.AddRange(from list in model.ReturnEditOutStore
                                  from item in list.ReturnWhStockOutItemEx
                                  where item.ProductSysNo > 0 && item.RmaQuantity > 0
                                  let pdModel = PdProductBo.Instance.GetProductBySysNo(item.ProductSysNo)
                                  select new RcReturnItem
                                  {
                                      ProductSysNo = item.ProductSysNo, //商品编号
                                      RmaQuantity = item.RmaQuantity, //实际退换数量
                                      RmaReason = item.RmaReason, //退换原因
                                      OriginPrice = item.OriginalPrice, //商品原价
                                      RefundProductAmount = item.RefundAmount, //退款金额
                                      StockOutItemSysNo = item.StockOutItemSysNo, //出库单明细编号
                                      ProductName = pdModel.ProductName,
                                      ReturnPriceType = item.ReturnPriceType
                                  });
                //申请单来源
                int source = model.HandleDepartment == (int)RmaStatus.退换货处理部门.门店 ? (int)RmaStatus.退换货申请单来源.门店 : (int)RmaStatus.退换货申请单来源.客服;
                cbReturn = new CBRcReturn
                {
                    OrderSysNo = model.SysNo,                           //订单编号
                    RefundType = model.RefundType,                      //退款方式
                    WarehouseSysNo = model.WarehouseSysNo,              //入库仓库
                    PickupTypeSysNo = model.PickUpShipTypeSysNo,        //取件方式
                    RMARemark = model.RMARemark,                        //会员备注
                    InternalRemark = model.InternalRemark,              //对内备注
                    Status = (int)WarehouseStatus.退换货单状态.待审核,  //状态
                    CreateBy = CurrentUser.Base.SysNo,                  //创建人
                    CreateDate = DateTime.Now,                          //创建时间
                    LastUpdateBy = CurrentUser.Base.SysNo,                  //更新人
                    LastUpdateDate = DateTime.Now,                          //更新时间
                    PickUpTime = model.PickUpTime,                      //取件时间段
                    CustomerSysNo = order.CustomerSysNo,                //会员系统编号
                    Source = source,                                    //申请单来源
                    InvoiceSysNo = order.InvoiceSysNo,                  //发票系统编号
                    HandleDepartment = model.HandleDepartment,            //处理部门
                    ShipTypeSysNo = order.DeliveryTypeSysNo,              //配送配送方式编号
                    RefundAccount = model.RefundAccount,                 //退款开户帐号
                    RefundAccountName = model.RefundAccountName,         //退款开户人
                    RefundBank = model.RefundBank,                      //退款开户行

                    CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    RefundDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                };
                #region 重新计算明细退款金额
                var calRmaItem = RmaBo.Instance.CalculateRmaAmountByStockOutItem(cbReturn.OrderSysNo, rmaItems.ToDictionary(x => x.StockOutItemSysNo, x => x.RmaQuantity));
                foreach (var rcReturnItem in rmaItems)
                {
                    //如果是自定义价格就不重新赋值
                    if (calRmaItem != null && calRmaItem.StockOutItemAmount != null && rcReturnItem.ReturnPriceType != (int)Hyt.Model.WorkflowStatus.RmaStatus.商品退款价格类型.自定义价格)
                    {
                        var dicSoi = calRmaItem.StockOutItemAmount.FirstOrDefault(x => x.Key == rcReturnItem.StockOutItemSysNo);
                        rcReturnItem.RefundProductAmount = dicSoi.Value;
                    }
                    fundAmount += rcReturnItem.RefundProductAmount;
                }
                var aRmaItem = RmaBo.Instance.CalculateRefundRmaAmount(cbReturn.OrderSysNo, fundAmount, false);
                if (calRmaItem != null && aRmaItem != null)
                {
                    cbReturn.OrginPoint = calRmaItem.OrginPoint;
                    cbReturn.OrginAmount = calRmaItem.OrginAmount;
                    cbReturn.OrginCoin = calRmaItem.OrginCoin;
                    cbReturn.CouponAmount = calRmaItem.CouponAmount;
                    cbReturn.DeductedInvoiceAmount = model.DeductedInvoiceAmount;
                    cbReturn.RefundProductAmount = aRmaItem.RefundProductAmount;
                    cbReturn.RedeemAmount = aRmaItem.RedeemAmount;
                    cbReturn.RefundCoin = aRmaItem.RefundCoin;
                    cbReturn.RefundPoint = aRmaItem.RefundPoint;
                    cbReturn.RefundTotalAmount = fundAmount - aRmaItem.RedeemAmount - model.DeductedInvoiceAmount - aRmaItem.RefundCoin;//实退总金额(实退商品金额-发票扣款金额-现金补偿金额-实退惠源币)
                }
                cbReturn.RMAItems = rmaItems;

                #endregion
            }
            return cbReturn;
        }

        /// <summary>
        /// 退货保存
        /// </summary>
        /// <param name="model">退换货单编辑实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-17 余勇 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.RC1002201, PrivilegeCode.RC1004201)]
        public ActionResult SaveRmaReturn(ReturnEdit model)
        {
            Result result = new Result();
            SoReceiveAddress pickUpAddress = null;
            CBRcReturn cbReturn = GetCBRcReturn(model);
            //取得自动计算字段的值
            //cbReturn = CalRcReturn(cbReturn, model);
            //审核退换货
            if (cbReturn == null)
            {
                result.StatusCode = -1;
                result.Message = "退换货单不存在";
            }
            if (result.StatusCode != -1)
            {
                //获取取件地址
                if (model.PickUpShipTypeSysNo == PickupType.百城当日取件 || model.PickUpShipTypeSysNo == PickupType.普通取件
                            || model.PickUpShipTypeSysNo == PickupType.加急取件 || model.PickUpShipTypeSysNo == @PickupType.定时取件)
                {
                    pickUpAddress = model.PickUpAddress;
                    pickUpAddress.SysNo = cbReturn.PickUpAddressSysNo;
                }
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        RmaBo.Instance.SaveRMA(cbReturn, pickUpAddress, null, CurrentUser.Base);
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    result.Message = ex.Message;
                }
            }
            return Json(result);

        }

        ///// <summary>
        ///// 计算退款中的自动计算字段
        ///// </summary>
        ///// <param name="cbReturn"></param>
        ///// <param name="model"></param>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //private CBRcReturn CalRcReturn(CBRcReturn cbReturn, ReturnEdit model)
        //{
        //    CBRcReturnCalculate calReturn = RmaBo.Instance.CalculateRefundRmaAmount(cbReturn.OrderSysNo, model.RefundProductAmount, false);//TODO:false:杨文文本组需要从界面上获取
        //    cbReturn.RedeemAmount = calReturn.RedeemAmount;
        //    cbReturn.RefundCoin = calReturn.RefundCoin;
        //    cbReturn.RefundPoint = calReturn.RefundPoint;
        //    cbReturn.RefundTotalAmount = calReturn.RefundTotalAmount;
        //    return cbReturn;
        //}

        /// <summary>
        /// 退货审核
        /// </summary>
        /// <param name="model">退换货单编辑实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-12 余勇 创建</remarks> 
        /// <remarks>2013-09-24 黄志勇 修改</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.RC1002601, PrivilegeCode.RC1004601)]
        public ActionResult AuditRmaReturn(ReturnEdit model)
        {
            Result result = new Result();
            SoReceiveAddress pickUpAddress = null;
            CBRcReturn cbReturn = GetCBRcReturn(model);
            //取得自动计算字段的值
            //cbReturn = CalRcReturn(cbReturn, model);
            //审核退换货
            if (cbReturn == null)
            {
                result.StatusCode = -1;
                result.Message = "退换货单不存在";
            }
            if (cbReturn.Status != (int)RmaStatus.退换货状态.待审核)
            {
                result.StatusCode = -1;
                result.Message = "退换货单目前不是待审核状态";
            }
            if (result.StatusCode != -1)
            {
                //获取取件地址
                if (model.PickUpShipTypeSysNo == PickupType.百城当日取件 || model.PickUpShipTypeSysNo == PickupType.普通取件
                            || model.PickUpShipTypeSysNo == PickupType.加急取件 || model.PickUpShipTypeSysNo == PickupType.定时取件)
                {
                    pickUpAddress = model.PickUpAddress;
                    pickUpAddress.SysNo = cbReturn.PickUpAddressSysNo;

                }

                try
                {
                    WhStockIn stockInModel = null;
                    using (var tran = new TransactionScope())
                    {
                        RmaBo.Instance.SaveRMA(cbReturn, pickUpAddress, null, CurrentUser.Base);
                        stockInModel=RmaBo.Instance.AuditRMA(cbReturn.SysNo, CurrentUser.Base);
                        tran.Complete();
                    }

                    #region 库存入库平台

                    if (stockInModel != null)
                    {
                        var productList = stockInModel.ItemList.Select(x => x.ProductSysNo).ToList();
                        var products = BLL.Product.PdProductBo.Instance.GetProductListBySysnoList(productList);
                        if (products.Count != productList.Count)
                        {
                            throw new HytException("产品信息有误！");
                        }

                        var data = stockInModel.ItemList.Select(x => new WhStockInItem()
                        {
                            RealStockInQuantity = x.RealStockInQuantity,
                            ProductErpCode = products.Where(y => y.SysNo == x.ProductSysNo).FirstOrDefault().ErpCode,
                        }).ToList();

                        var reslut = BLL.Warehouse.WhWarehouseBo.Instance.ReduceStock(1,stockInModel.WarehouseSysNo, null, data);
                        if (!reslut.Status)
                            throw new HytException(reslut.Message);
                    }
                 
                    #endregion

                    if (model.PickUpShipTypeSysNo == (int)PickupType.快递至仓库)
                    {
                        var mobile = string.Empty;
                        if (pickUpAddress != null) mobile = pickUpAddress.MobilePhoneNumber;
                        if (string.IsNullOrEmpty(mobile))
                        {
                            var obj = CrCustomerBo.Instance.GetModel(model.CustomerSysNo);
                            if (obj != null) mobile = obj.MobilePhoneNumber;
                        }
                        if (VHelper.Do(mobile, VType.Mobile))
                        {
                            var warehouse = WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo);
                            BLL.Extras.SmsBO.Instance.发送退货受理审核通过短信(mobile, model.SysNo.ToString(),
                                                                  warehouse.StreetAddress, warehouse.Contact);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    result.Message = ex.Message;
                }
            }
            return Json(result);

        }

        /// <summary>
        /// 取得退货单实体
        /// </summary>
        /// <param name="model">退换货单编辑实体</param>
        /// <returns>退货单实体</returns>
        /// <remarks>2013-07-17 余勇 创建</remarks> 
        /// <remarks>2014-06-06 余勇 修改 重新计算退款金额</remarks> 
        private CBRcReturn GetCBRcReturn(ReturnEdit model)
        {
            CBRcReturn cbReturn = RmaBo.Instance.GetRMA(model.SysNo);
            if (cbReturn != null)
            {
                decimal fundAmount = 0;
                var rmaItems = (from list in model.ReturnEditOutStore
                                from item in list.ReturnWhStockOutItemEx
                                where item.ProductSysNo > 0 && item.RmaQuantity > 0
                                let pdModel = PdProductBo.Instance.GetProductBySysNo(item.ProductSysNo)
                                select new RcReturnItem
                                {
                                    ProductSysNo = item.ProductSysNo, //商品编号
                                    RmaQuantity = item.RmaQuantity, //实际退换数量
                                    RmaReason = item.RmaReason, //退换原因
                                    OriginPrice = item.OriginalPrice, //商品原价
                                    RefundProductAmount = item.RefundAmount, //退款金额
                                    StockOutItemSysNo = item.StockOutItemSysNo, //出库单明细编号
                                    ProductName = pdModel.ProductName,
                                    ReturnSysNo = cbReturn.SysNo,
                                    TransactionSysNo = cbReturn.TransactionSysNo,
                                    ReturnPriceType = item.ReturnPriceType //商品退款价格类型
                                }).ToList();
                cbReturn.RefundType = model.RefundType;                      //退款方式
                cbReturn.WarehouseSysNo = model.WarehouseSysNo;              //入库仓库
                cbReturn.PickupTypeSysNo = model.PickUpShipTypeSysNo;        //取件方式
                cbReturn.RMARemark = model.RMARemark;                        //会员备注
                cbReturn.InternalRemark = model.InternalRemark;              //对内备注
                //cbReturn.Status = (int)WarehouseStatus.退换货单状态.待入库;  //状态
                cbReturn.PickUpTime = model.PickUpTime;                      //取件时间段
                cbReturn.RefundBank = model.RefundBank;                       //退款开户行
                cbReturn.RefundAccount = model.RefundAccount;                 //退款开户帐号
                cbReturn.RefundAccountName = model.RefundAccountName;         //退款开户人

                #region 重新计算明细退款金额
                var calRmaItem = RmaBo.Instance.CalculateRmaAmountByStockOutItem(cbReturn.OrderSysNo, rmaItems.ToDictionary(x => x.StockOutItemSysNo, x => x.RmaQuantity));
                foreach (var rcReturnItem in rmaItems)
                {
                    //如果是自定义价格就不重新对实退金额赋值
                    if (calRmaItem != null && calRmaItem.StockOutItemAmount != null && rcReturnItem.ReturnPriceType != (int)Hyt.Model.WorkflowStatus.RmaStatus.商品退款价格类型.自定义价格)
                    {
                        var dicSoi = calRmaItem.StockOutItemAmount.FirstOrDefault(x => x.Key == rcReturnItem.StockOutItemSysNo);
                        rcReturnItem.RefundProductAmount = dicSoi.Value;
                    }
                    fundAmount += rcReturnItem.RefundProductAmount;
                }
                var aRmaItem = RmaBo.Instance.CalculateRefundRmaAmount(cbReturn.OrderSysNo, fundAmount, false);
                if (calRmaItem != null && aRmaItem != null)
                {
                    cbReturn.OrginPoint = calRmaItem.OrginPoint;
                    cbReturn.OrginAmount = calRmaItem.OrginAmount;
                    cbReturn.OrginCoin = calRmaItem.OrginCoin;
                    cbReturn.CouponAmount = calRmaItem.CouponAmount;

                    cbReturn.DeductedInvoiceAmount = model.DeductedInvoiceAmount;
                    cbReturn.RefundProductAmount = aRmaItem.RefundProductAmount;
                    cbReturn.RedeemAmount = aRmaItem.RedeemAmount;
                    cbReturn.RefundCoin = aRmaItem.RefundCoin;
                    cbReturn.RefundPoint = aRmaItem.RefundPoint;
                    cbReturn.RefundTotalAmount = fundAmount - aRmaItem.RedeemAmount - model.DeductedInvoiceAmount - aRmaItem.RefundCoin;//实退总金额(实退商品金额-发票扣款金额-现金补偿金额-实退惠源币)
                }
                #endregion

                cbReturn.RMAItems = rmaItems;
            }
            return cbReturn;
        }

        /// <summary>
        /// 取得换货单实体
        /// </summary>
        /// <param name="model">换货单编辑实体</param>
        /// <returns>换货单实体</returns>
        /// <remarks>2014-06-06 余勇 创建</remarks> 
        private CBRcReturn GetRcExchanges(ReturnEdit model)
        {
            CBRcReturn cbReturn = RmaBo.Instance.GetRMA(model.SysNo);
            if (cbReturn != null)
            {
                decimal fundAmount = 0;
                var rmaItems = new List<RcReturnItem>();
                foreach (var list in model.ReturnEditOutStore)
                {
                    foreach (var item in list.ReturnWhStockOutItemEx)
                    {
                        if (item.ProductSysNo > 0 && item.RmaQuantity > 0)
                        {
                            //fundAmount += item.RefundAmount;
                            var pdModel = PdProductBo.Instance.GetProductBySysNo(item.ProductSysNo);
                            rmaItems.Add(new RcReturnItem
                            {
                                ProductSysNo = item.ProductSysNo, //商品编号
                                RmaQuantity = item.RmaQuantity, //实际退换数量
                                RmaReason = item.RmaReason, //退换原因
                                OriginPrice = item.OriginalPrice, //商品原价
                                RefundProductAmount = item.RefundAmount, //退款金额
                                StockOutItemSysNo = item.StockOutItemSysNo, //出库单明细编号
                                ProductName = pdModel.ProductName,
                                ReturnSysNo = cbReturn.SysNo,
                                TransactionSysNo = cbReturn.TransactionSysNo,
                                ReturnPriceType = item.ReturnPriceType //商品退款价格类型
                            });
                        }
                    }
                }
                cbReturn.RefundType = model.RefundType;                      //退款方式
                cbReturn.WarehouseSysNo = model.WarehouseSysNo;              //入库仓库
                cbReturn.PickupTypeSysNo = model.PickUpShipTypeSysNo;        //取件方式
                cbReturn.RMARemark = model.RMARemark;                        //会员备注
                cbReturn.InternalRemark = model.InternalRemark;              //对内备注
                //cbReturn.Status = (int)WarehouseStatus.退换货单状态.待入库;  //状态
                cbReturn.PickUpTime = model.PickUpTime;                      //取件时间段
                cbReturn.RefundBank = model.RefundBank;                       //退款开户行
                cbReturn.RefundAccount = model.RefundAccount;                 //退款开户帐号
                cbReturn.RefundAccountName = model.RefundAccountName;         //退款开户人

                //cbReturn.OrginPoint = model.OrginPoint;
                //cbReturn.OrginAmount = model.OrginAmount;
                //cbReturn.OrginCoin = model.OrginCoin;
                //cbReturn.CouponAmount = model.CouponAmount;
                //cbReturn.DeductedInvoiceAmount = model.DeductedInvoiceAmount;    //发票扣款金额;

                //cbReturn.RefundProductAmount = model.RefundProductAmount;
                //cbReturn.RedeemAmount = model.RedeemAmount;
                //cbReturn.RefundCoin = model.RefundCoin;
                //cbReturn.RefundPoint = model.RefundPoint;
                //cbReturn.RefundTotalAmount = fundAmount - model.RedeemAmount - model.DeductedInvoiceAmount - model.RefundCoin;//实退总金额(实退商品金额-发票扣款金额-现金补偿金额-实退惠源币)
                cbReturn.RMAItems = rmaItems;
            }
            return cbReturn;
        }

        /// <summary>
        /// 计算退货对象
        /// </summary>
        /// <param name="orderSysNo">销售单系统编号</param>
        /// <param name="stockOutItemRmaQuantity">当前退货销售单明细编号和数量</param>
        /// <returns>退货计算对象</returns>
        /// <remarks>2013-09-17 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1002201, PrivilegeCode.RC1001201, PrivilegeCode.RC1003201, PrivilegeCode.RC1004201, PrivilegeCode.RC1002601, PrivilegeCode.RC1004601)]
        public JsonResult CalculateRmaAmount(int orderSysNo, List<StockOutItemRmaQuantity> stockOutItemRmaQuantity)
        {
            var result = new Result<CBRcReturnCalculate>();
            try
            {
                result.Data = RmaBo.Instance.CalculateRmaAmountByStockOutItem(orderSysNo, stockOutItemRmaQuantity.ToDictionary(x => x.StockOutItemSysNo, x => x.RmaQuantity));
                if (result.Data != null)//计算实际退款，发票等信息
                {
                    result.Data = RmaBo.Instance.CalculateRefundRmaAmount(orderSysNo, result.Data, false);
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 计算实际退货对象
        /// </summary>
        /// <param name="orderSysNo">销售单系统编号</param>
        /// <param name="refundAmount">退款商品总额</param>
        /// <returns>退货计算对象</returns>
        /// <remarks>2013-09-17 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1002201, PrivilegeCode.RC1001201, PrivilegeCode.RC1003201, PrivilegeCode.RC1004201, PrivilegeCode.RC1002601, PrivilegeCode.RC1004601)]
        public JsonResult CalculateTotalAmount(int orderSysNo, decimal refundAmount)
        {
            var result = new Result<CBRcReturnCalculate>();
            try
            {
                result.Data = RmaBo.Instance.CalculateRefundRmaAmount(orderSysNo, refundAmount, false);//TODO:false,杨文兵组需从界面上获取
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 退货转门店
        /// </summary>
        /// <param name="sysNo">退换货编号</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-09-17 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1002201)]
        public JsonResult ReturnToShop(int sysNo)
        {
            var result = new Result<CBRcReturnCalculate>();
            try
            {
                RmaBo.Instance.ReturnToShop(sysNo);
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  换货
        /// <summary>
        /// 新建换货单
        /// </summary>
        /// <param name="model">退换货单编辑实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-15 余勇 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult InsertRmaExchanges(ReturnEdit model)
        {
            Result<int> result = new Result<int>();
            CBRcReturn entity = null;
            SoReceiveAddress pickUpAddress = null;
            entity = GetInsetRcExchanges(model);
            if (entity != null)
            {
                entity.RmaType = (int)RmaStatus.RMA类型.售后换货;          //退换货类型:售后换货(10),售后退货(20)
                if (entity.RMAItems.Count == 0)
                {
                    result.StatusCode = -1;
                    result.Message = "换货商品不能为空";
                }
            }
            else
            {
                result.StatusCode = -1;
                result.Message = "该订单不存在";
            }
            if (result.StatusCode != -1)
            {
                //获取取件地址
                if (model.PickUpShipTypeSysNo == PickupType.百城当日取件 || model.PickUpShipTypeSysNo == PickupType.普通取件
                            || model.PickUpShipTypeSysNo == PickupType.加急取件 || model.PickUpShipTypeSysNo == @PickupType.定时取件)
                {
                    pickUpAddress = model.PickUpAddress;

                }
                SoReceiveAddress SoReceiveAddress = model.SoReceiveAddress;
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        result.Data = RmaBo.Instance.InsertRMA(entity, pickUpAddress, SoReceiveAddress, CurrentUser.Base);
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 新建换货单
        /// </summary>
        /// <param name="model">退换货单编辑实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-15 余勇 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.RC1002201, PrivilegeCode.RC1004201)]
        public ActionResult SaveRmaExchanges(ReturnEdit model)
        {
            Result result = new Result();
            SoReceiveAddress pickUpAddress = null;
            CBRcReturn cbReturn = GetRcExchanges(model);
            //审核退换货
            if (cbReturn == null)
            {
                result.StatusCode = -1;
                result.Message = "退换货单不存在";
            }
            if (cbReturn.Status != (int)RmaStatus.退换货状态.待审核)
            {
                result.StatusCode = -1;
                result.Message = "退换货单目前不是待审核状态";
            }
            //获取取件地址
            if (model.PickUpShipTypeSysNo == PickupType.百城当日取件 || model.PickUpShipTypeSysNo == PickupType.普通取件
                        || model.PickUpShipTypeSysNo == PickupType.加急取件 || model.PickUpShipTypeSysNo == @PickupType.定时取件)
            {
                pickUpAddress = model.PickUpAddress;
                pickUpAddress.SysNo = cbReturn.PickUpAddressSysNo;

            }
            var soReceiveAddress = model.SoReceiveAddress;
            soReceiveAddress.SysNo = cbReturn.ReceiveAddressSysNo;

            try
            {
                using (var tran = new TransactionScope())
                {
                    RmaBo.Instance.SaveRMA(cbReturn, pickUpAddress, soReceiveAddress, CurrentUser.Base);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 换货审核
        /// </summary>
        /// <param name="model">退换货单编辑实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-12 余勇 创建</remarks> 
        /// <remarks>2013-11-6 黄志勇 修改</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.RC1002601, PrivilegeCode.RC1004601)]
        public ActionResult AuditRmaExchanges(ReturnEdit model)
        {
            Result result = new Result();
            SoReceiveAddress pickUpAddress = null;
            CBRcReturn cbReturn = GetRcExchanges(model);
            //审核退换货
            if (cbReturn == null)
            {
                result.StatusCode = -1;
                result.Message = "退换货单不存在";
            }
            if (cbReturn.Status != (int)RmaStatus.退换货状态.待审核)
            {
                result.StatusCode = -1;
                result.Message = "退换货单目前不是待审核状态";
            }
            //获取取件地址
            if (model.PickUpShipTypeSysNo == PickupType.百城当日取件 || model.PickUpShipTypeSysNo == PickupType.普通取件
                        || model.PickUpShipTypeSysNo == PickupType.加急取件 || model.PickUpShipTypeSysNo == PickupType.定时取件)
            {
                pickUpAddress = model.PickUpAddress;
                pickUpAddress.SysNo = cbReturn.PickUpAddressSysNo;

            }
            var soReceiveAddress = model.SoReceiveAddress;
            soReceiveAddress.SysNo = cbReturn.ReceiveAddressSysNo;

            try
            {
                WhStockIn stockInModel = null;
                using (var tran = new TransactionScope())
                {
                    RmaBo.Instance.SaveRMA(cbReturn, pickUpAddress, soReceiveAddress, CurrentUser.Base);
                    stockInModel=RmaBo.Instance.AuditRMA(cbReturn.SysNo, CurrentUser.Base);
                    tran.Complete();
                }

                #region 库存入库平台

                //if (stockInModel != null)
                //{
                //    var productList = stockInModel.ItemList.Select(x => x.ProductSysNo).ToList();
                //    var products = BLL.Product.PdProductBo.Instance.GetProductListBySysnoList(productList);
                //    if (products.Count != productList.Count)
                //    {
                //        throw new HytException("产品信息有误！");
                //    }

                //    var data = stockInModel.ItemList.Select(x => new WhStockInItem()
                //    {
                //        RealStockInQuantity = x.RealStockInQuantity,
                //        ProductErpCode = products.Where(y => y.SysNo == x.ProductSysNo).FirstOrDefault().ErpCode,
                //    }).ToList();

                //    var reslut = BLL.Warehouse.WhWarehouseBo.Instance.ReduceStock(1, stockInModel.WarehouseSysNo, null, data);
                //    if (!reslut.Status)
                //        throw new HytException(reslut.Message);
                //}

                #endregion

                if (model.PickUpShipTypeSysNo == PickupType.快递至仓库)
                {
                    var mobile = model.SoReceiveAddress.MobilePhoneNumber;
                    if (VHelper.Do(mobile, VType.Mobile))
                    {
                        var warehouse = WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo);
                        BLL.Extras.SmsBO.Instance.发送换货受理审核通过短信(mobile, model.SysNo.ToString(),
                                                              warehouse.StreetAddress, warehouse.Contact);
                    }
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        #endregion
        /// <summary>
        /// 作废退换货单
        /// </summary>
        /// <param name="sysNo">退换货单号</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-12 黄志勇 创建</remarks> 
        [Privilege(PrivilegeCode.RC1002601, PrivilegeCode.RC1004601)]
        public JsonResult CancelReturn(int sysNo)
        {
            Result result = new Result();
            var rcReturn = RmaBo.Instance.GetRcReturnEntity(sysNo);
            if (rcReturn == null)
            {
                result.Message = "未找到退换货单";
            }
            else
            {
                if (rcReturn.Status == (int)RmaStatus.退换货状态.待审核)
                {
                    using (var tran = new TransactionScope())
                    {
                        RmaBo.Instance.CancelRMA(sysNo, CurrentUser.Base);
                        tran.Complete();
                        result.Status = true;
                        result.Message = "作废成功";
                    }
                    #region 发送短信、邮件
                    var order = SoOrderBo.Instance.GetEntity(rcReturn.OrderSysNo);
                    var address = SoOrderBo.Instance.GetOrderReceiveAddress(rcReturn.ReceiveAddressSysNo);
                    if (address != null && VHelper.Do(address.MobilePhoneNumber, VType.Mobile))
                    {
                        if (rcReturn.RmaType == (int)RmaStatus.RMA类型.售后退货)
                            BLL.Extras.SmsBO.Instance.发送退货受理审核未通过短信(address.MobilePhoneNumber);
                        else
                            BLL.Extras.SmsBO.Instance.发送换货受理审核未通过短信(address.MobilePhoneNumber);
                    }
                    var customer = CrCustomerBo.Instance.GetCrCustomerItem(rcReturn.CustomerSysNo);
                    if (customer != null && VHelper.Do(customer.EmailAddress, VType.Email))
                    {
                        BLL.Extras.EmailBo.Instance.发送退换货单未通过审核邮件(customer.EmailAddress, order.CustomerSysNo.ToString(),
                                                                  rcReturn.SysNo.ToString(), rcReturn.CreateDate);
                    }
                    #endregion
                }
                else
                {
                    result.StatusCode = -1;
                    result.Message = "作废失败，退换货单状态为：" + Hyt.Util.EnumUtil.GetDescription(typeof(RmaStatus.退换货状态), rcReturn.Status);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 分页查询

        /// <summary>
        /// 退换货列表
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="clickSource">点击来源</param>
        /// <returns>退换货列表页面</returns>
        /// <remarks>2013-07-11 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RC1001101)]
        public ActionResult RmaNewList(int? customerSysNo, string clickSource)
        {
            ViewBag.CustomerSysNo = customerSysNo;
            ViewBag.clickSource = clickSource;
            return View();
        }

        /// <summary>
        /// 退换货列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2013-07-11 朱家宏 创建</remarks> 
        [Privilege(PrivilegeCode.RC1001101, PrivilegeCode.RC1003101)]
        public ActionResult DoRmaNewQuery(ParaOrderFilter filter)
        {
            filter.HasAllWarehouse = true;
            filter.IsBindAllDealer = true;
            filter.SelectedAgentSysNo = -1;
            var pager = RmaBo.Instance.GetRmaSoOrders(filter);

            var list = new PagedList<CBSoOrder>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            ViewBag.HandleDepartment = filter.HandleDepartment;
            return PartialView("_RmaNewListPager", list);
        }

        /// <summary>
        /// 退换货维护列表
        /// </summary>
        /// <param name="customersysno">会员编号</param>
        /// <returns>退换货维护列表页面</returns>
        /// <remarks>2013-07-12 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RC1002101)]
        public ActionResult RmaEditList(int? customersysno)
        {
            var typeList = new List<SelectListItem>();
            var statusList = new List<SelectListItem>();
            Util.EnumUtil.ToListItem<RmaStatus.RMA类型>(ref typeList);
            Util.EnumUtil.ToListItem<RmaStatus.退换货状态>(ref statusList);
            if (typeList.Count > 0) ViewBag.typeList = typeList;
            if (statusList.Count > 0) ViewBag.statusList = statusList;
            ViewBag.CustomerSysNo = customersysno;
            return View();
        }

        /// <summary>
        /// 退换货维护列表(客服)
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>退换货维护列表</returns>
        /// <remarks>2013-07-12 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult DoRmaEditQuery(ParaRmaFilter filter)
        {

            var pager = RmaBo.Instance.GetRmasForCallCenter(filter);

            var list = new PagedList<CBRcReturn>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            ViewBag.ActionFrom = "CallCenter";
            return PartialView("_RmaEditListPager", list);
        }

        /// <summary>
        /// 门店退换货查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>视图</returns>
        /// <remarks>2013-07-12 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult DoShopRmaEditQuery(ParaRmaFilter filter)
        {
            if (filter.StoreSysNoList == null || !filter.StoreSysNoList.Any())
            {
                if (CurrentUser.Warehouses != null && CurrentUser.Warehouses.Any())
                {
                    filter.StoreSysNoList =
                        CurrentUser.Warehouses.Where(o => o.WarehouseType == (int)WarehouseStatus.仓库类型.门店)
                                   .Select(o => o.SysNo)
                                   .ToList();
                }
            }

            var pager = RmaBo.Instance.GetRmasForShop(filter);

            var list = new PagedList<CBRcReturn>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            ViewBag.ActionFrom = "Shop";
            return PartialView("_RmaEditListPager", list);
        }


        #region 退款
        /// <summary>
        /// 退款维护列表
        /// </summary>
        /// <param name="customersysno">会员编号</param>
        /// <returns>退款维护列表页面</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        [Privilege(PrivilegeCode.RC1005101)]
        public ActionResult RefundEditList(int? customersysno)
        {
            var typeList = new List<SelectListItem>();
            var statusList = new List<SelectListItem>();
            Util.EnumUtil.ToListItem<RmaStatus.RMA类型>(ref typeList);
            Util.EnumUtil.ToListItem<RmaStatus.退换货状态>(ref statusList);
            if (typeList.Count > 0) ViewBag.typeList = typeList;
            if (statusList.Count > 0) ViewBag.statusList = statusList;
            ViewBag.CustomerSysNo = customersysno;
            return View();
        }

        /// <summary>
        /// 退款维护列表
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>退款维护列表页面</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        [Privilege(PrivilegeCode.RC1005101)]
        public ActionResult DoRefundEditQuery(ParaRefundFilter filter)
        {
            var pager = RefundReturnDao.Instance.GetAll(filter);

            var list = new PagedList<RcRefundReturn>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            ViewBag.ActionFrom = "CallCenter";
            return PartialView("_RefundEditListPager", list);
        }


        /// <summary>
        /// 退款详情
        /// </summary>
        /// <param name="sysNo">退款编号</param>
        /// <returns>视图</returns>
        /// <remarks>2016-08-29 罗远康 创建</remarks>
        [HttpGet]
        [Privilege(PrivilegeCode.RC1005101)]
        public ActionResult RefundDetail(int sysNo)
        {
            ViewBag.SysNo = sysNo;
            ViewBag.RMAUrl = "RefundToAudit";
            return View();
        }

        /// <summary>
        /// 退款审核
        /// </summary>
        /// <param name="id">退款主表编号</param>
        /// <returns>新建退货单页面</returns>
        /// <remarks>2016-08-29 罗远康 创建</remarks>
        [Privilege(PrivilegeCode.RC1005101)]
        public ActionResult RefundToAudit(int id)
        {
            var model = RefundReturnDao.Instance.GetEntity(id);
            ViewBag.Status = 0;
            ViewBag.payment = 0;
            if (model != null)
            {
                Hyt.Model.Transfer.CBFnPaymentVoucher payment = Hyt.BLL.Finance.FinanceBo.Instance.GetPayment((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.销售单, model.OrderSysNo);
                if (payment != null)
                {
                    ViewBag.payment = payment.SysNo;
                }
                ViewBag.Status = model.Status;
            }
            ViewBag.SysNo = id;
            return View();
        }
        /// <summary>
        /// 审核退款单编辑控件
        /// </summary>
        /// <param name="id">退款单编号</param>
        /// <returns>视图</returns>
        /// <remarks>2016-08-29 罗远康 创建</remarks>
        [Privilege(PrivilegeCode.RC1005101)]
        public ActionResult RefundEditAudit(int id)
        {
            var model = RefundReturnDao.Instance.GetEntity(id);
            if (model != null)
            {
                var order = SoOrderBo.Instance.GetEntity(model.OrderSysNo);
                if (order != null)
                {
                    ViewBag.OrderList = order.OrderItemList;
                }
            }
            return PartialView("_RefundEdit", model);
        }

        /// <summary>
        /// 退款审核
        /// </summary>
        /// <param name="model">退款单编辑实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2016-08-29 罗远康 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.RC1005101)]
        public ActionResult AuditRefundReturn(RcRefundReturn model)
        {
            Result result = new Result();
            RcRefundReturn RefundReturn = RefundReturnDao.Instance.GetEntity(model.SysNo);
            //审核退换货
            if (RefundReturn == null)
            {
                result.StatusCode = -1;
                result.Message = "退款单不存在";
            }
            if (RefundReturn.Status != (int)RmaStatus.退换货状态.待审核)
            {
                result.StatusCode = -1;
                result.Message = "退款单目前不是待审核状态";
            }
            if (result.StatusCode != -1)
            {
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        RefundReturn.RefundTotalAmount = model.RefundTotalAmount;
                        RefundReturn.RMARemark = model.RMARemark;
                        RefundReturn.InternalRemark = model.InternalRemark;
                        RefundReturn.AuditorDate = DateTime.Now;
                        RefundReturn.LastUpdateDate = DateTime.Now;
                        RefundReturn.Status = (int)RmaStatus.退换货状态.待退款;
                        RefundReturnDao.Instance.Update(RefundReturn);
                        //调用订单审核作废订单功能，操作后自动作废订单、生成付款单、退回优惠券、积分、惠源币等
                        SoOrderBo.Instance.CancelSoOrder(RefundReturn.OrderSysNo, CurrentUser.Base.SysNo, OrderStatus.销售单作废人类型.后台用户, ref result.Message);
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    result.Message = ex.Message;
                }
            }
            return Json(result);

        }

        /// <summary>
        /// 作废退款单
        /// </summary>
        /// <param name="sysNo">退款单号</param>
        /// <returns>操作结果</returns>
        /// <remarks>2016-08-29 罗远康 创建</remarks> 
        [Privilege(PrivilegeCode.RC1005101)]
        public JsonResult CancelRefundReturn(int sysNo)
        {
            Result result = new Result();
            var RefundReturn = RefundReturnDao.Instance.GetEntity(sysNo);
            if (RefundReturn == null)
            {
                result.Message = "未找到退款单";
            }
            else
            {
                if (RefundReturn.Status == (int)RmaStatus.退换货状态.待审核)
                {
                    using (var tran = new TransactionScope())
                    {
                        RefundReturn.CancelBy = CurrentUser.Base.SysNo;
                        RefundReturn.CancelDate = DateTime.Now;
                        RefundReturn.Status = (int)RmaStatus.退换货状态.作废;
                        RefundReturnDao.Instance.Update(RefundReturn);
                        tran.Complete();
                        result.Status = true;
                        result.Message = "作废成功";
                    }
                }
                else
                {
                    result.StatusCode = -1;
                    result.Message = "作废失败，退款单状态为：" + Hyt.Util.EnumUtil.GetDescription(typeof(RmaStatus.退换货状态), RefundReturn.Status);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 退款保存
        /// </summary>
        /// <param name="model">退款单编辑实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2016-08-29 罗远康 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.RC1005101)]
        public ActionResult SaveRefundReturn(ReturnEdit model)
        {
            Result result = new Result();
            RcRefundReturn RefundReturn = RefundReturnDao.Instance.GetEntity(model.SysNo);

            //审核退换货
            if (RefundReturn == null)
            {
                result.StatusCode = -1;
                result.Message = "退款单不存在";
            }
            if (result.StatusCode != -1)
            {
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        RefundReturn.RefundTotalAmount = model.RefundTotalAmount;
                        RefundReturn.RMARemark = model.RMARemark;
                        RefundReturn.InternalRemark = model.InternalRemark;
                        RefundReturn.LastUpdateDate = DateTime.Now;
                        RefundReturnDao.Instance.Update(RefundReturn);
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    result.Message = ex.Message;
                }
            }
            return Json(result);

        }
        #endregion
        #endregion

        #region 门店退换货页面
        /// <summary>
        /// 门店退换货列表
        /// </summary>
        /// <returns>门店退换货列表页面</returns>
        /// <remarks>2013-07-16 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1003101)]
        public ActionResult ShopRmaNewList()
        {
            return View();
        }

        /// <summary>
        /// 门店退换货维护列表
        /// </summary>
        /// <returns>门店退换货维护列表页面</returns>
        /// <remarks>2013-07-16 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1004101)]
        public ActionResult ShopRmaEditList()
        {
            ViewBag.Shops = ShopOrderBo.Instance.GetShopsFromUserSession(CurrentUser.Warehouses);
            return View();
        }

        #endregion

        #region 退换货公共方法
        /// <summary>
        /// 根据订单获取退货单编辑控件
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="department">退换货处理部门：客服10，门店20</param>
        /// <returns>退换货编辑实体</returns>
        /// <remarks>2013-07-17 黄志勇 创建</remarks>
        private ReturnEdit GetReturnEditByOrder(int id, int department = 10)
        {
            var model = new ReturnEdit();
            model.SysNo = id;
            model.OrderSysNo = id;
            model.HandleDepartment = department;
            var isShop = department == (int)RmaStatus.退换货处理部门.门店;  //是否门店
            var order = SoOrderBo.Instance.GetEntity(id);
            //出库仓库
            WhWarehouse defaultWhWarehouse = null;
            if (order != null)
            {
                var rmaItemList = RmaBo.Instance.GetItemListByOrder(id);//退换货明细 (2013-08-07 朱成果)
                if (order.DefaultWarehouseSysNo > 0)
                {
                    defaultWhWarehouse = WhWarehouseBo.Instance.GetWarehouseEntity(order.DefaultWarehouseSysNo);
                    if (defaultWhWarehouse != null)
                    {
                        model.WarehouseSysNo = defaultWhWarehouse.SysNo;
                        model.BackWarehouseName = defaultWhWarehouse.BackWarehouseName;
                    }
                }
                model.ReturnAble = CurrentUser.PrivilegeList.HasPrivilege(isShop ? PrivilegeCode.RC1003801 : PrivilegeCode.RC1001801); //有强制退换货权限判断
                model.ModifyAmountAble = CurrentUser.PrivilegeList.HasPrivilege(isShop ? PrivilegeCode.RC1003802 : PrivilegeCode.RC1001802); //有修改合计金额权限
                var outStoreList = new List<ReturnEditOutStore>(); //页面出库单显示列表
                var stockOutList = WhWarehouseBo.Instance.GetWhStockOutListByOrderID(id); //获取该订单的出库单列表（包含出库单明细)
                foreach (var stockOut in stockOutList)
                {
                    if (RmaBo.Instance.CanReturn(stockOut))
                    {
                        var returnEditOutStore = new ReturnEditOutStore();
                        returnEditOutStore.SysNo = stockOut.SysNo;
                        returnEditOutStore.StockOutAmount = stockOut.StockOutAmount;
                        returnEditOutStore.SignTime = stockOut.SignTime;
                        returnEditOutStore.StatusName = Util.EnumUtil.GetDescription(
                            typeof(WarehouseStatus.出库单状态), stockOut.Status);
                        returnEditOutStore.BackWarehouseName =
                            WhWarehouseBo.Instance.GetWarehouseEntity(stockOut.WarehouseSysNo)
                               .BackWarehouseName;

                        returnEditOutStore.ReturnWhStockOutItemEx = new List<ReturnWhStockOutItemEx>();
                        foreach (var item in stockOut.Items)
                        {
                            var stockOutItemEx = new ReturnWhStockOutItemEx();
                            stockOutItemEx.StockOutItemSysNo = item.SysNo;
                            stockOutItemEx.ProductSysNo = item.ProductSysNo;
                            stockOutItemEx.ProductName = item.ProductName;
                            stockOutItemEx.OriginalPrice = item.OriginalPrice;
                            stockOutItemEx.ProductQuantity = item.ProductQuantity;
                            stockOutItemEx.RealSalesAmount = item.RealSalesAmount;
                            stockOutItemEx.OrderItemSysNo = item.OrderItemSysNo;
                            //stockOutItemEx.ProductQuantityAble = item.ProductQuantity - item.ReturnQuantity;
                            stockOutItemEx.ProductQuantityAble = item.ProductQuantity -
                                                                 RmaBo.Instance.GetAllRmaQuantity(
                                                                     rmaItemList, item.SysNo, 0);

                            var orderItem = SoOrderBo.Instance.GetOrderItem(item.OrderItemSysNo);
                            stockOutItemEx.ProductSalesType = orderItem.ProductSalesType;
                            //退换货明细 (2013-08-07 朱成果)
                            returnEditOutStore.ReturnWhStockOutItemEx.Add(stockOutItemEx);


                        }
                        outStoreList.Add(returnEditOutStore);
                    }
                }
                //获取入库仓库列表
                if (department == (int)WarehouseStatus.仓库类型.门店)
                {
                    var list = new List<WhWarehouse>();
                    list.AddRange(AdminAuthenticationBo.Instance.Current.Warehouses.Where(m => !string.IsNullOrEmpty(m.ErpRmaCode)));
                    model.WhWarehouseList = list;
                    var list2 = ShopOrderBo.Instance.GetShopsFromUserSession(CurrentUser.Warehouses);
                    if (list2 != null)
                    {
                        foreach (WhWarehouse itx in list2)
                        {
                            if (!list.Exists(m => m.SysNo == itx.SysNo))
                            {
                                model.WhWarehouseList.Add(itx);
                            }
                        }
                    }

                    if (!(defaultWhWarehouse != null && defaultWhWarehouse.WarehouseType == (int)WarehouseStatus.仓库类型.门店))
                    {
                        //门店退，默认选门店 朱成果
                        var defaultshop = model.WhWarehouseList.Where(m => m.WarehouseType == (int)WarehouseStatus.仓库类型.门店).FirstOrDefault();
                        if (defaultshop != null)
                        {
                            model.WarehouseSysNo = defaultshop.SysNo;
                            model.BackWarehouseName = defaultshop.BackWarehouseName;
                        }
                    }
                }
                //获取取件地址
                var pickUpAddress = SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                model.PickUpAddress = pickUpAddress;
                model.SoReceiveAddress = pickUpAddress;
                model.ReturnEditOutStore = outStoreList;
                model.InvoiceSysNo = order.InvoiceSysNo;
                var invoice = SoOrderBo.Instance.GetFnInvoice(order.InvoiceSysNo);
                if (invoice != null)
                {
                    model.InvoiceType = invoice.InvoiceTypeSysNo;
                }
            }
            return model;
        }

        /// <summary>
        /// 根据退货单获取退货单编辑控件
        /// </summary>
        /// <param name="id">退货单编号</param>
        /// <returns>退换货编辑实体</returns>
        /// <remarks>2013-07-17 黄志勇 创建</remarks>
        private ReturnEdit GetReturnEditByRMA(int id)
        {
            var model = new ReturnEdit();
            var rma = RmaBo.Instance.GetRMA(id);
            if (rma != null)
            {

                var rmaItemList = RmaBo.Instance.GetItemListByOrder(rma.OrderSysNo);//退换货明细 (2013-08-07 朱成果)
                var isShop = rma.HandleDepartment == (int)RmaStatus.退换货处理部门.门店;
                model.SysNo = id;
                model.OrderSysNo = rma.OrderSysNo;
                model.ReturnAble = CurrentUser.PrivilegeList.HasPrivilege(isShop ? PrivilegeCode.RC1003801 : PrivilegeCode.RC1001801); //有强制退换货权限判断
                model.ModifyAmountAble = CurrentUser.PrivilegeList.HasPrivilege(isShop ? PrivilegeCode.RC1003802 : PrivilegeCode.RC1001802); //有修改合计金额权限
                model.SaveAble = true; //todo: 有保存权限
                model.CancelAble = true;  //todo: 有作废权限
                model.AuditAble = CurrentUser.PrivilegeList.HasPrivilege(isShop ? PrivilegeCode.RC1004601 : PrivilegeCode.RC1002601);  //有审核通过权限
                model.PickUpTime = rma.PickUpTime;  //预约时间 
                var order = SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
                if (order != null)
                {
                    model.OrderSysNo = order.SysNo;
                    var outStoreList = new List<ReturnEditOutStore>(); //页面出库单显示列表
                    var stockOutList = WhWarehouseBo.Instance.GetWhStockOutListByOrderID(order.SysNo); //获取可退换货的出库单列表（包含出库单明细)
                    foreach (var stockOut in stockOutList)
                    {
                        if (RmaBo.Instance.CanReturn(stockOut))
                        {
                            var returnEditOutStore = new ReturnEditOutStore();
                            returnEditOutStore.SysNo = stockOut.SysNo;
                            returnEditOutStore.StockOutAmount = stockOut.StockOutAmount;
                            returnEditOutStore.SignTime = stockOut.SignTime;
                            returnEditOutStore.StatusName =
                                Hyt.Util.EnumUtil.GetDescription(typeof(WarehouseStatus.出库单状态), stockOut.Status);
                            returnEditOutStore.BackWarehouseName =
                                WhWarehouseBo.Instance.GetWarehouseEntity(stockOut.WarehouseSysNo)
                                   .BackWarehouseName;
                            returnEditOutStore.ReturnWhStockOutItemEx = new List<ReturnWhStockOutItemEx>();
                            foreach (var item in stockOut.Items)
                            {
                                var stockOutItemEx = new ReturnWhStockOutItemEx();
                                stockOutItemEx.ProductSysNo = item.ProductSysNo;
                                stockOutItemEx.ProductName = item.ProductName;
                                stockOutItemEx.OriginalPrice = item.OriginalPrice;
                                stockOutItemEx.ProductQuantity = item.ProductQuantity;
                                stockOutItemEx.RealSalesAmount = item.RealSalesAmount;
                                // stockOutItemEx.ProductQuantityAble = item.ProductQuantity - item.ReturnQuantity;
                                stockOutItemEx.ProductQuantityAble = item.ProductQuantity -
                                                                     RmaBo.Instance.GetAllRmaQuantity(
                                                                         rmaItemList, item.SysNo, id);
                                //退换货明细 (2013-08-07 朱成果)
                                var rmaDetail = rma.RMAItems.Find(i => i.StockOutItemSysNo == item.SysNo);
                                if (rmaDetail != null)
                                {
                                    stockOutItemEx.RmaQuantity = rmaDetail.RmaQuantity; //退(换)货数量
                                    stockOutItemEx.RmaReason = rmaDetail.RmaReason;
                                    stockOutItemEx.RefundAmount = rmaDetail.RefundProductAmount; //合计退款金额
                                    stockOutItemEx.ReturnPriceType = rmaDetail.ReturnPriceType;

                                }
                                stockOutItemEx.StockOutItemSysNo = item.SysNo;
                                stockOutItemEx.OrderItemSysNo = item.OrderItemSysNo;

                                var orderItem = SoOrderBo.Instance.GetOrderItem(item.OrderItemSysNo);
                                stockOutItemEx.ProductSalesType = orderItem.ProductSalesType;
                                returnEditOutStore.ReturnWhStockOutItemEx.Add(stockOutItemEx);
                            }
                            outStoreList.Add(returnEditOutStore);
                        }
                    }
                    model.ReturnEditOutStore = outStoreList;
                }
                //获取入库仓库列表
                if (isShop)
                {
                    model.WhWarehouseList = AdminAuthenticationBo.Instance.Current.Warehouses.ToList();
                }
                model.WarehouseSysNo = rma.WarehouseSysNo;
                var warehouse = WhWarehouseBo.Instance.GetWarehouseEntity(model.WarehouseSysNo);
                model.BackWarehouseName = warehouse != null ? warehouse.BackWarehouseName : "";
                model.RefundProductAmount = rma.RefundProductAmount;  //合计应退款金额
                model.RefundType = rma.RefundType;  //退款方式
                model.RefundBank = rma.RefundBank;  //退款开户行
                model.RefundAccount = rma.RefundAccount;  //退款开户帐号
                model.HandleDepartment = rma.HandleDepartment;  //退款单处理部门：客服（10），门店（20）
                model.RefundAccountName = rma.RefundAccountName;  //退款开户人
                model.LgPickupType = WhWarehouseBo.Instance.GetPickupTypeListByWarehouse(rma.WarehouseSysNo) as List<LgPickupType>;
                model.PickUpShipTypeSysNo = rma.PickupTypeSysNo;  //取件方式
                model.RMARemark = rma.RMARemark;  //顾客备注
                model.InternalRemark = rma.InternalRemark;  //对内备注
                model.PickUpAddress = SoOrderBo.Instance.GetOrderReceiveAddress(rma.PickUpAddressSysNo);  //取件地址
                model.SoReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(rma.ReceiveAddressSysNo);  //收货地址
                model.PickUpTime = rma.PickUpTime;          //取件预约时间
                model.DeductedInvoiceAmount = rma.DeductedInvoiceAmount;    //发票扣款金额;

                model.OrginPoint = rma.OrginPoint;
                model.OrginAmount = rma.OrginAmount;
                model.OrginCoin = rma.OrginCoin;
                model.CouponAmount = rma.CouponAmount;
                model.RedeemAmount = rma.RedeemAmount;
                model.RefundProductAmount = rma.RefundProductAmount;
                model.RefundCoin = rma.RefundCoin;
                model.RefundPoint = rma.RefundPoint;
                model.RefundTotalAmount = rma.RefundTotalAmount;

                if (order != null) model.InvoiceSysNo = order.InvoiceSysNo;
            }

            var rmaImgs = RmaBo.Instance.GetRmaImages(id);
            model.RmaImages = rmaImgs.ToList();

            return model;
        }

        /// <summary>
        /// 根据退货单获取退货单查看控件
        /// </summary>
        /// <param name="id">换货单编号</param>
        /// <returns>退换货显示实体</returns>
        /// <remarks>2013-07-17 黄志勇 创建</remarks>
        private ReturnView GetReturnViewByRMA(int id)
        {
            var model = new ReturnView();
            var rma = RmaBo.Instance.GetRMA(id);
            if (rma != null)
            {
                var rmaItemList = RmaBo.Instance.GetItemListByOrder(rma.OrderSysNo);//退换货明细 (2013-08-07 朱成果)
                var order = SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
                if (order != null)
                {
                    var outStoreList = new List<ReturnEditOutStore>(); //页面出库单显示列表
                    var stockOutList = WhWarehouseBo.Instance.GetWhStockOutListByOrderID(order.SysNo); //获取可退换货的出库单列表（包含出库单明细)
                    foreach (var stockOut in stockOutList)
                    {
                        if (RmaBo.Instance.CanReturn(stockOut))
                        {
                            var returnEditOutStore = new ReturnEditOutStore();
                            returnEditOutStore.SysNo = stockOut.SysNo;
                            returnEditOutStore.StockOutAmount = stockOut.StockOutAmount;
                            returnEditOutStore.SignTime = stockOut.SignTime;
                            returnEditOutStore.StatusName =
                                Hyt.Util.EnumUtil.GetDescription(typeof(WarehouseStatus.出库单状态), stockOut.Status);

                            //空仓库判断 2013-11-20 朱家宏
                            var stockOutWarehouse = WhWarehouseBo.Instance.GetWarehouseEntity(stockOut.WarehouseSysNo);
                            returnEditOutStore.BackWarehouseName = stockOutWarehouse != null ? stockOutWarehouse.BackWarehouseName : "";

                            returnEditOutStore.ReturnWhStockOutItemEx = new List<ReturnWhStockOutItemEx>();
                            foreach (var item in stockOut.Items)
                            {
                                var stockOutItemEx = new ReturnWhStockOutItemEx();
                                stockOutItemEx.ProductSysNo = item.ProductSysNo;
                                stockOutItemEx.ProductName = item.ProductName;
                                stockOutItemEx.OriginalPrice = item.OriginalPrice;
                                stockOutItemEx.ProductQuantity = item.ProductQuantity;
                                stockOutItemEx.RealSalesAmount = item.RealSalesAmount;
                                //stockOutItemEx.ProductQuantityAble = item.ProductQuantity - item.ReturnQuantity;
                                stockOutItemEx.ProductQuantityAble = item.ProductQuantity -
                                                                     RmaBo.Instance.GetAllRmaQuantity(
                                                                         rmaItemList, item.SysNo, id);
                                //退换货明细 (2013-08-07 朱成果)
                                var rmaDetail = rma.RMAItems.Find(i => i.StockOutItemSysNo == item.SysNo);
                                if (rmaDetail != null)
                                {
                                    stockOutItemEx.RmaQuantity = rmaDetail == null ? 0 : rmaDetail.RmaQuantity;
                                    //退(换)货数量
                                    stockOutItemEx.RmaReason = rmaDetail == null ? "" : rmaDetail.RmaReason;
                                    stockOutItemEx.RefundAmount = rmaDetail == null ? 0 : rmaDetail.RefundProductAmount;
                                    //合计退款金额
                                    returnEditOutStore.ReturnWhStockOutItemEx.Add(stockOutItemEx);
                                }
                            }
                            if (returnEditOutStore.ReturnWhStockOutItemEx.Count > 0)
                            {
                                outStoreList.Add(returnEditOutStore);
                            }
                        }
                    }
                    model.ReturnEditOutStore = outStoreList;
                }
                model.SysNo = rma.SysNo;
                model.OrderSysNo = rma.OrderSysNo;
                model.HandleDepartment = rma.HandleDepartment;  //退款单处理部门：客服（10），门店（20）
                model.RefundAmount = rma.OrginAmount;  //合计应退款金额
                model.ActualRefundAmount = rma.RefundTotalAmount;//实退金额
                model.OrginAmount = rma.OrginAmount;    //应退金额

                model.OrginCoin = rma.OrginCoin;    //应退惠源币
                model.OrginPoint = rma.OrginPoint;    //应退金额
                model.CouponAmount = rma.CouponAmount;    //扣回优惠卷
                model.RefundCoin = rma.RefundCoin;    //实退惠源币
                model.RefundPoint = rma.RefundPoint;    //实扣回积分
                model.RedeemAmount = rma.RedeemAmount;    //积分现金补偿金额

                model.RefundType = rma.RefundType;  //退款方式
                model.RefundTypeName = Hyt.Util.EnumUtil.GetDescription(typeof(RmaStatus.退换货退款方式), rma.RefundType);  //退款方式名称
                model.RefundBank = rma.RefundBank;  //退款开户行
                model.RefundAccount = rma.RefundAccount;  //退款开户帐号
                model.RefundAccountName = rma.RefundAccountName;  //退款开户人
                var warehouse = WhWarehouseBo.Instance.GetWarehouseEntity(rma.WarehouseSysNo);
                model.WhStockInName = warehouse != null ? warehouse.BackWarehouseName : "";
                var pickupType = LgPickUpTypeBo.Instance.GetPickupType(rma.PickupTypeSysNo);
                model.LgPickupTypeName = pickupType != null ? pickupType.PickupTypeName : "";  //取件方式
                model.RMARemark = rma.RMARemark;  //顾客备注
                model.InternalRemark = rma.InternalRemark;  //对内备注
                var pickUpAddress = SoOrderBo.Instance.GetOrderReceiveAddress(rma.PickUpAddressSysNo);  //取件地址
                var soAddress = SoOrderBo.Instance.GetOrderReceiveAddress(rma.ReceiveAddressSysNo);  //取件地址
                model.PickUpAddress = pickUpAddress;  //取件地址
                model.SoReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(rma.ReceiveAddressSysNo);  //收货地址
                model.PickUpAddressFullName = pickUpAddress != null ? string.Format("{0} {1}", GetFullAreaName(pickUpAddress.AreaSysNo), pickUpAddress.StreetAddress) : "";  //取件地址全称
                model.ReceiveAddressFullName = soAddress != null ? string.Format("{0} {1}", GetFullAreaName(soAddress.AreaSysNo), soAddress.StreetAddress) : "";  //收货地址全称

                //取件预约时间
                if (!string.IsNullOrEmpty(rma.PickUpTime))
                {
                    //控制日期显示 2013-11-20 朱家宏
                    if (DateTime.Parse(rma.PickUpTime) != DateTime.MinValue)
                    {
                        model.PickUpTime = rma.PickUpTime;
                    }
                }

                model.DeductedInvoiceAmount = rma.DeductedInvoiceAmount;     //发票扣款金额
                model.IsPickUpInvoice = rma.IsPickUpInvoice > 0 ? "是" : "否";
            }

            var rmaImgs = RmaBo.Instance.GetRmaImages(id);
            model.RmaImages = rmaImgs.ToList();

            return model;
        }

        /// <summary>
        /// 根据区县编号获取省市区全称
        /// </summary>
        /// <param name="sysNo">区县编号</param>
        /// <returns>地址全称</returns>
        /// <remarks>2013-07-4 黄志勇 创建</remarks>
        private string GetFullAreaName(int sysNo)
        {
            BsArea area;
            BsArea city;
            BsArea province = Hyt.BLL.Basic.BasicAreaBo.Instance.GetProvinceEntity(sysNo, out city, out area);
            return (province != null ? province.AreaName : "") + (city != null ? city.AreaName : "") + (area != null ? area.AreaName : "");
        }

        /// <summary>
        /// 退换货详情
        /// </summary>
        /// <param name="sysNo">退换货编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-07-22 黄志勇 创建</remarks>
        [HttpGet]
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult RMADetail(int sysNo)
        {
            var rma = RmaBo.Instance.GetRMA(sysNo);
            CBRmaRelations model = null;
            if (rma != null)
            {
                var type = rma.RmaType == (int)RmaStatus.RMA类型.售后退货 ? "退货单" : "换货单";
                var status = Util.EnumUtil.GetDescription(typeof(RmaStatus.退换货状态), rma.Status);
                ViewBag.Prompt = type + status;
                ViewBag.SysNo = sysNo;
                if (rma.RmaType == (int)RmaStatus.RMA类型.售后退货)
                {
                    //退货
                    if (rma.Status == (int)RmaStatus.退换货状态.待审核)
                        ViewBag.RMAUrl = "ReturnToAudit";
                    else
                        ViewBag.RMAUrl = "RmaReturnView";
                }
                else
                {
                    //换货
                    if (rma.Status == (int)RmaStatus.退换货状态.待审核)
                        ViewBag.RMAUrl = "ExchangesToAudit";
                    else
                        ViewBag.RMAUrl = "RmaExchangesView";
                }
                model = RmaBo.Instance.GetRmaRelationsBySysNo(rma.SysNo);
            }
            return View(model);
        }

        /// <summary>
        /// 可退换货订单判断
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>是否可退换货</returns>
        /// <remarks>2013-07-23 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public JsonResult CanReturn(int orderSysNo)
        {
            var result = RmaBo.Instance.CanReturn(orderSysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查看退换货单详情
        /// </summary>
        /// <param name="id">退换货单号</param>
        /// <returns>退换货单详情查看页面</returns>
        /// <remarks>2013-07-31 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RC1002101, PrivilegeCode.RC1004101)]
        public ActionResult RMAView(int id)
        {
            var rma = RmaBo.Instance.GetRcReturnEntity(id);
            if (rma != null)
            {
                ViewBag.SysNo = id;
                ViewBag.Status = rma.Status;
                ViewBag.Prompt = (rma.HandleDepartment == (int)RmaStatus.退换货处理部门.门店 ? "门店" : "") + "退货单" + ((RmaStatus.退换货状态)rma.Status).ToString();
                if (rma.RmaType == (int)RmaStatus.RMA类型.售后退货) return View("RmaReturnView");
                return View("RmaExchangesView");
            }
            throw new ArgumentException("未找到退换货单：" + id);
        }

        /// <summary>
        /// 新建退换货单
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <param name="department">退换货处理部门：客服10，门店20</param>
        /// <param name="type">退换货类型</param>
        /// <returns>新建退换货单页面</returns>
        /// <remarks>
        /// 2013-08-1 黄志勇 创建
        /// 2013-08-19 朱家宏 区分退换货的限制期限(退货15天内，换货1年内)
        /// </remarks>
        [Privilege(PrivilegeCode.RC1001201, PrivilegeCode.RC1003201)]
        public ActionResult RmaCreate(int id, int department, RmaStatus.RMA类型 type)
        {
            var isReturn = type == RmaStatus.RMA类型.售后退货;  //是否退货

            var deadline = 0;
            var deadlineText = "";
            switch (type)
            {
                case RmaStatus.RMA类型.售后退货:
                    deadline = 15;      //15天
                    deadlineText = deadline + "天";
                    break;
                case RmaStatus.RMA类型.售后换货:
                    deadline = 365;     //一年
                    deadlineText = "一年";
                    break;
            }

            var isShop = department == (int)RmaStatus.退换货处理部门.门店;  //是否门店
            ViewBag.IsShop = isShop;
            ViewBag.Prompt = string.Format("{0}新建{1}货单", isShop ? "门店" : "", isReturn ? "退" : "换");
            ViewBag.OrderSysNo = id;
            ViewBag.department = department;

            var model = new ReturnEdit();
            var order = SoOrderBo.Instance.GetEntity(id);  //订单
            var outStores = WhWarehouseBo.Instance.GetWhStockOutListByOrderID(id); //订单下所有出库单列表
            var hasExpireOutStore = false; //有超过15天已签收的出库单
            if (order != null && outStores.Count > 0)
            {
                ViewBag.Source = order.OrderSource; // 申请单来源:会员(10),客服(20),门店(30)
                var sign = outStores.Where(i => i.Status == (int)WarehouseStatus.出库单状态.已签收 || i.Status == (int)WarehouseStatus.出库单状态.部分退货); //已签收出库单
                if (sign.Count() > 0)
                {
                    var minSign = sign.Min(i => i.SignTime); //最早签收时间
                    if ((DateTime.Now - minSign).Days > deadline)
                    {
                        model.Title += string.Format("<li>业务流程规定只允许" + deadlineText + "以内的订单执行{0}货操作，但是可以继续执行。</li>", isReturn ? "退" : "换");
                        hasExpireOutStore = true;
                    }
                }
                else
                {
                    model.Title += string.Format("<li>出库单没有签收不允许{0}货</li>", isReturn ? "退" : "换");
                    ViewBag.CannotSave = true;
                }
                if (outStores.Count(i => i.Items.Count(j => ((j.ProductQuantity) - j.ReturnQuantity) > 0) > 0) == 0)
                {
                    model.Title += string.Format("<li>该订单无商品可{0}货。</li>", isReturn ? "退" : "换");
                    ViewBag.CannotSave = true;
                }

                model.ReturnAble = CurrentUser.PrivilegeList.HasPrivilege(isShop ? PrivilegeCode.RC1003801 : PrivilegeCode.RC1001801); //有强制退换货权限判断
                ViewBag.HasExpireOutStore = hasExpireOutStore;
                ViewBag.IsRma = order.OrderSource == (int)OrderStatus.销售单来源.RMA下单;
                if (isReturn && ViewBag.IsRma)
                {
                    model.Title += "<li>该订单来源为RMA，业务规定不能退货，但可以强制执行。</li>";
                }

                var pendReturn = RmaBo.Instance.GetPendWithReturn(order.SysNo);
                if (pendReturn != null)
                {
                    model.Title += string.Format("<li>订单号：{0}，存在未审核的退货申请，请先审核处理，跳转到审核页面？【<a href='/rma/RMADetail/?sysNo=" + pendReturn.SysNo + "'><span class='blue'>跳转</span></a>】</li>", order.SysNo);
                    ViewBag.CannotSave = true;
                }
            }
            else
            {
                return View("RmaNewList");
            }
            return View(isReturn ? "RmaReturn" : "RmaExchanges", model);

        }
        #endregion
    }
}
