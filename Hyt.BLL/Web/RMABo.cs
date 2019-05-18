using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Hyt.DataAccess.Web;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.BLL.RMA;
using Hyt.BLL.Warehouse;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 商品退换货操作
    /// </summary>
    /// <remarks>2013-08-27 邵斌 创建</remarks>
    public class RMABo : BOBase<RMABo>
    {

        /// <summary>
        /// 退还货历史查询
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="customer">用户系统编号</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="pager">分类对象</param>
        /// <return>退换货商品列表</return>
        /// <remarks>2013-08-27 邵斌 创建</remarks>
        public PagedList<CBWebRMA> Search(int pageIndex, int customer, DateTime endTime, string searchKeyWords)
        {
            Pager<CBWebRMA> pager = new Pager<CBWebRMA>();
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new CBWebRMA()
                {
                    SearchKeyWords = searchKeyWords
                };

            Hyt.DataAccess.Web.IRMADao.Instance.Search(customer, endTime, ref pager);

            PagedList<CBWebRMA> list = new PagedList<CBWebRMA>();
            list.TData = pager.Rows;
            list.CurrentPageIndex = pager.CurrentPage;
            list.TotalItemCount = pager.TotalRows;
            list.PageSize = pager.PageSize;
            return list;
        }

        /// <summary>
        /// 根据订单获取退货商品列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="returyType">退换货乐讯</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 邵斌 创建</remarks>
        public CBWebRMA GetReturnProductListByOrder(int orderSysNo, RmaStatus.RMA类型 returyType, int? productSysNo = null)
        {
            var result = new CBWebRMA();
            result.Items = new List<CBWebRMAItem>(); //页面出库单显示列表

            //判断订单是否存在
            var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);

            if (order != null)
            {
                result.CustomerSysNo = order.CustomerSysNo;
                result.ShipType = order.DeliveryTypeSysNo; //原订单配送方式

                var rmaItemList = RmaBo.Instance.GetItemListByOrder(orderSysNo);//退换货明细 (2013-08-07 朱成果)
                var stockOutList = Warehouse.WhWarehouseBo.Instance.GetWhStockOutListByOrderID(orderSysNo,true); //获取该订单的出库单列表（包含出库单明细)

                CBSimplePdProduct product = null;
                CBWebRMAItem stockOutItemEx;
                IList<WhStockOutItem> productItems;

                //遍历出库单，用出库单设置显示列表
                foreach (var stockOut in stockOutList)
                {

                    //过滤商品，只处理指定商品
                    if (productSysNo.HasValue)
                    {
                        productItems = stockOut.Items.Where(p => p.ProductSysNo == productSysNo.Value).ToList();
                    }
                    else
                    {
                        productItems = stockOut.Items;
                    }

                    //遍历每个出库单组合商品
                    foreach (var item in productItems)
                    {
                        stockOutItemEx = result.Items.FirstOrDefault(i => i.ProductSysNo == item.ProductSysNo);

                        //判断商品是否已经存在列表，如果不存在就读取数据
                        if (stockOutItemEx == null)
                        {
                            product = BLL.Web.PdProductBo.Instance.GetProduct(item.ProductSysNo);

                            //读取基本数据
                            stockOutItemEx = new CBWebRMAItem();
                            stockOutItemEx.StockOutItemSysNo = item.SysNo;
                            stockOutItemEx.ProductSysNo = item.ProductSysNo;
                            stockOutItemEx.ProductName = item.ProductName;
                            stockOutItemEx.OriginalPrice = item.OriginalPrice;
                            stockOutItemEx.PackageDesc = product.PackageDesc;   //商品基础信息
                            stockOutItemEx.Image = product.ProductImage;
                            stockOutItemEx.RealSalesAmount = item.RealSalesAmount;
                            stockOutItemEx.OrderItemSysNo = item.OrderItemSysNo;

                            //添加到退换货明细
                            result.Items.Add(stockOutItemEx);
                        }

                        //TODO 价格优惠暂时没有实现或确切的计算方式，等待确定业务逻辑和算法
                        stockOutItemEx.Preferential = 0;

                        //计算实际销售金额和消失数量
                        stockOutItemEx.RealSalesAmount += item.RealSalesAmount;
                        stockOutItemEx.ProductQuantity += item.ProductQuantity;

                        //判断商品是否可进行退换货操作
                        if (RmaBo.Instance.OrderRMARequest(stockOut.OrderSysNO, stockOutItemEx.ProductSysNo, (int)returyType))
                        {
                            //标记能否退换货
                            stockOutItemEx.EnableRMA = true;

                            //计算可退换货数量
                            stockOutItemEx.ProductQuantityAble += item.ProductQuantity -
                                                                 RmaBo.Instance.GetAllRmaQuantity(
                                                                     rmaItemList, item.SysNo, 0);
                        }
                    }
                }

                //获取取件地址
                result.PickUpAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);

                //获取发票信息
                var invoice = BLL.Order.SoOrderBo.Instance.GetFnInvoice(order.InvoiceSysNo);

                //是否有发票
                if (invoice != null)
                {
                    //如果有发票，并取得发票的状态用于前台判断是否需要退票
                    result.InvoiceSysNo = invoice.SysNo;
                    result.HasInvoice = true;
                    result.InvoiceStatus = (Hyt.Model.WorkflowStatus.FinanceStatus.发票状态)invoice.Status;
                }
            }

            //安能否退货排序
            result.Items = result.Items.OrderByDescending(i => i.EnableRMA).ToList();

            result.OrderSysNo = orderSysNo;

            return result;
        }

        /// <summary>
        /// 生成退换货申请单
        /// </summary>
        /// <param name="order">申请订单内容</param>
        /// <param name="customerSysNo">申请客户系统编号</param>
        /// <returns>创建是否成功的结果对象</returns>
        /// <remarks>2013-08-27 邵斌 创建</remarks>
        public Result CreateRMAOrder(CBWebRMA order, int customerSysNo)
        {
            Result result = new Result();
            RcReturn returnEntity = new RcReturn();
            IList<RcReturnItem> returnItems = new List<RcReturnItem>();

            //读取退化员订单信息，该信息将用于校验输入数据是否有效
            CBWebRMA oldOrder = GetReturnProductListByOrder(order.OrderSysNo, (RmaStatus.RMA类型)order.ReturnType);

            #region 数据验证

            //验证输入
            result = VaildRMAApplyOrderBaseInfo(order, oldOrder);

            //如果验证不通过将返回
            if (!result.Status)
                return result;

            #endregion

            #region 计算退款金额 并设置退换货明细

            CuCalculateCharge(order, oldOrder, ref returnEntity, ref returnItems);

            #endregion

            #region 退换货相关表数据设置

            returnEntity.RmaType = order.ReturnType;        //RMA类型
            returnEntity.OrderSysNo = oldOrder.OrderSysNo;  //销售单系统编号
            returnEntity.CustomerSysNo = customerSysNo;     //客户系统编号
            returnEntity.Source = (int)Model.WorkflowStatus.RmaStatus.退换货申请单来源.会员;  //申请单来源
            returnEntity.PickupTypeSysNo = order.PickUpType;    //取件方式系统编号
            returnEntity.PickUpTime = order.PickUpTime.ToString("yyyy-MM-dd HH:mm:ss");         //取件时间段
            returnEntity.InvoiceSysNo = oldOrder.InvoiceSysNo;  //发票系统编号

            //如果是换货将设置收货地址
            if (order.ReturnType == (int)RmaStatus.RMA类型.售后换货 || order.ReceiveAddress != null)
            {
                returnEntity.ShipTypeSysNo = order.ShipType; //收货地址系统编号
            }

            //读取订单实体，通过订单来判读是否是网络支付
            var orderEntity = BLL.Web.SoOrderBo.Instance.GetEntity(order.OrderSysNo);
            if (orderEntity.PayTypeSysNo == (int)PaymentType.网银 || orderEntity.PayTypeSysNo == (int)PaymentType.支付宝)
            {
                returnEntity.RefundType = (int)RmaStatus.退换货退款方式.原路返回;  //退款方式
            }
            else if (order.Bank != null)     //如果银行账户信息不为空则可以进行设置银行账号用于退款
            {
                returnEntity.RefundBank = order.Bank.Name;                         //开户行
                returnEntity.RefundAccount = order.Bank.Number;                    //开户账号
                returnEntity.RefundAccountName = order.Bank.Account;               //开户账户
                returnEntity.RefundType = (int)RmaStatus.退换货退款方式.至银行卡;  //退款方式
            }

            returnEntity.Status = (int)RmaStatus.退换货状态.待审核;                 //退换货状态
            returnEntity.CreateDate = DateTime.Now;                                 //退换货申请单创建时间
            returnEntity.LastUpdateDate = DateTime.Now;                             //退换货申请单最后更新时间
            returnEntity.RMARemark = order.Reason;                                  //用户备注

            // 退换货仓库
            if (order.WarehouseSysNo > 0)
            {
                returnEntity.WarehouseSysNo = order.WarehouseSysNo;
            }
            else
            {
                returnEntity.WarehouseSysNo = IWhWarehouseDao.Instance.GetDefaultWarehouse(true, order.PickUpType);
            }

            #endregion

            #region 数据入库

            
                //插入取货地址
                if (order.PickUpAddress != null)//取件地址
                {
                    order.PickUpAddress.PhoneNumber = order.PickUpAddress.MobilePhoneNumber;
                    returnEntity.PickUpAddressSysNo = Hyt.DataAccess.Order.ISoReceiveAddressDao.Instance.InsertEntity(order.PickUpAddress).SysNo;
                }

                if (order.ReturnType == (int)RmaStatus.RMA类型.售后换货 || order.ReceiveAddress != null)
                {
                    order.ReceiveAddress.PhoneNumber = order.ReceiveAddress.MobilePhoneNumber;
                    returnEntity.ReceiveAddressSysNo = Hyt.DataAccess.Order.ISoReceiveAddressDao.Instance.InsertEntity(order.ReceiveAddress).SysNo;
                }

                if (DataAccess.Web.IRMADao.Instance.InsertRMA(returnEntity, returnItems))
                {
                    result.Status = true;
                    result.StatusCode = 1;
                }
                else
                {
                    result.Status = false;
                    result.StatusCode = -3;
                    result.Message = "创建申请失败，请联系PC网站客服人员进行处理";
                }

            

            #endregion

            return result;
        }

        #region 私有辅助方法

        /// <summary>
        /// 检查申请退换货输入表单
        /// </summary>
        /// <param name="order">退换货申请实体</param>
        /// <param name="oldOrder">订单原实体</param>
        /// <returns>返回验证结果</returns>
        /// <remarks>2013-08-27 邵斌 创建</remarks>
        private Result VaildRMAApplyOrderBaseInfo(CBWebRMA order, CBWebRMA oldOrder)
        {
            Result result = new Result();
            //判断订单是否有效，订单为空表示订单编号无效 ,订单客户系统编号不等也视为无效
            if (oldOrder == null || oldOrder.CustomerSysNo != order.CustomerSysNo)
            {
                result.StatusCode = -1;
                result.Status = false;
                result.Message = "提交退换货申请的订单无效";
                return result;
            }

            result = CheckOrderProduct(order, oldOrder);

            //检查申请退换货商品是否有效,包括检查申请退换货的商品数量
            if (!result.Status)
            {
                result.Message = string.Format("{0}{1}", "提交退换货申请的订单无效<br/>", result.Message);
                return result;
            }

            //如果取件类型为百城当日取件，判断取货地址是否支持百城当日期间
            if (order.PickUpType == (int)Hyt.Model.SystemPredefined.PickupType.百城当日取件)
            {
                //判断取货地址是否支持百城当日期间
                if (!WhWarehouseBo.Instance.AroundHasWarehouseSupportPickUp(order.PickUpAddress.AreaSysNo, Model.SystemPredefined.PickupType.百城当日取件))
                {
                    result.Message = "在您填写的取件地址附近不支持上门取件，请更换其他地址再试试(如：办公地址)<br/>";
                    result.Status = false;
                }
            }

            //如果收件类型为百城当日达，判断收货地址是否支持百城当日达
            if (order.ShipType == (int)Hyt.Model.SystemPredefined.DeliveryType.百城当日达)
            {
                //判断取货地址是否支持百城当日期间
                if (!WhWarehouseBo.Instance.AroundHasWarehouseSupportDelivery(order.PickUpAddress.AreaSysNo, Model.SystemPredefined.DeliveryType.百城当日达))
                {
                    result.Message = "在您填写的收件地址附近不支持百城当日达，请更换其他地址再试试(如：办公地址)<br/>";
                    result.Status = false;
                }
            }

            #region 2013-08-07 允许同时多个退换货 (朱成果)
            //邵斌 2013-09-05 迁移

            var lstRMAItem = BLL.RMA.RmaBo.Instance.GetItemListByOrder(order.OrderSysNo);//已存在的退换货数据
            if (order.Items != null && lstRMAItem != null && lstRMAItem.Count > 0)
            {
                foreach (CBRmaReturnItem item in lstRMAItem)
                {
                    var newItem = order.Items.Where(m => m.StockOutItemSysNo == item.stockoutitemsysno).SingleOrDefault();
                    if (newItem != null)
                    {
                        if (item.ProductQuantity < newItem.ProductQuantity + BLL.RMA.RmaBo.Instance.GetAllRmaQuantity(lstRMAItem, item.stockoutitemsysno, 0))
                        {
                            throw new Exception("退换货数量之和超过了购买数量");
                        }
                    }
                }
            }

            #endregion

            //如果有错误信息将返回结果
            if (!result.Status)
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// 退换货申请有效性校验
        /// </summary>
        /// <param name="applyOrder">申请订单</param>
        /// <param name="oldOrder"></param>
        /// <returns>检查结果：是否通过检查</returns>
        /// <remarks>2013-08-27 邵斌 创建</remarks>
        private Result CheckOrderProduct(CBWebRMA applyOrder, CBWebRMA oldOrder)
        {
            var result = new Result();

            //查找所有申请的商品是否在员订单中并且是可退换货商品
            var queryProduct = from a in applyOrder.Items
                               join b in oldOrder.Items on a.ProductSysNo equals b.ProductSysNo
                               where b.EnableRMA.Equals(true)
                               select b;

            var targProductList = queryProduct.ToList<CBWebRMAItem>(); //查询商品个数

            //判断过滤后商品申请个数是否和申请商品个数不相等
            if (targProductList.Count != applyOrder.Items.Count)
            {
                result.StatusCode = -2;
                result.Status = false;
                result.Message = "申请的商品中有未符合退换货条件的商品，请刷新页面仔细检查后再提交申请<br/>";
                return result;
            }

            StringBuilder message = new StringBuilder();

            //检查可会换货商品数量是否有效
            foreach (var cbWebRmaItem in targProductList)
            {
                //读取申请退还货的商品数量
                int targProductNum =
                    applyOrder.Items.Where(p => p.ProductSysNo == cbWebRmaItem.ProductSysNo)
                              .Select(p => p.ProductQuantity)
                              .FirstOrDefault<int>();

                //申请退换货的商品数量是否超过了最大可退换货数量
                if (targProductNum > cbWebRmaItem.ProductQuantityAble)
                {
                    message.Append(string.Format("商品{0}的申请退换货数量超过了可退还商品数量<br/>", cbWebRmaItem.ProductName));
                }
            }

            //判断是否有错误信息
            if (message.Length > 0)
            {
                //返回错误信息
                result.StatusCode = -3;
                result.Status = false;
                result.Message = message.ToString();
                return result;
            }

            //验证通过
            result.Status = true;

            return result;
        }

        /// <summary>
        /// 计算退款金额
        /// </summary>
        /// <param name="applyOrder">申请退款表单</param>
        /// <param name="oldOrder">数据源表单</param>
        /// <param name="returnEntity">退换货对象</param>
        /// <param name="returnItems">退换货明细</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 邵斌 创建</remarks>
        public void CuCalculateCharge(CBWebRMA applyOrder, CBWebRMA oldOrder, ref RcReturn returnEntity, ref IList<RcReturnItem> returnItems)
        {

            //查找所有申请的商品是否在商品订单中并且是否是可退换货商品
            decimal price = 0;
            decimal totalReturnCharge = 0;

            #region 构造退换货明细对象

            #region 调用吴文强退换货金额计算方法

            var queryItems = from ri in applyOrder.Items
                             join oi in oldOrder.Items on ri.ProductSysNo equals oi.ProductSysNo
                             select new { OrderItemSysNo = oi.OrderItemSysNo, ProductQuantity = ri.ProductQuantity };

            Dictionary<int, int> items = queryItems.ToDictionary(p => p.OrderItemSysNo, p => p.ProductQuantity);
            CBRcReturnCalculate cReturn = Hyt.BLL.RMA.RmaBo.Instance.CalculateRmaAmount(applyOrder.OrderSysNo, items);
            KeyValuePair<int, decimal> productAmount;

            #endregion

            
            //遍历申请商品构造退换货明细对象
            foreach (var applyProductInfo in applyOrder.Items)
            {
                //查找申请的商品在原商品订单中的对应对象
                var cbWebRmaItem = oldOrder.Items.FirstOrDefault(p => p.ProductSysNo == applyProductInfo.ProductSysNo);

                //如果没有源对象表示这个商品是无效的将继续往下执行（在前面已经校对过可以保证数据时正确的）
                if (cbWebRmaItem == null)
                    continue;

                //构造退换货对象
                var rcReturnItem = new RcReturnItem()
                    {
                        StockOutItemSysNo = cbWebRmaItem.StockOutItemSysNo,
                        RmaReason = applyOrder.Reason,
                        OriginPrice = cbWebRmaItem.OriginalPrice,
                        ProductName = cbWebRmaItem.ProductName,
                        ProductSysNo = cbWebRmaItem.ProductSysNo,
                        ReturnType = (int)RmaStatus.商品退换货类型.新品,
                        RmaQuantity = applyProductInfo.ProductQuantity
                    };

               
                

                //如果是换货将不计算金额
                if (applyOrder.ReturnType == (int)RmaStatus.RMA类型.售后换货)
                {
                    rcReturnItem.RefundProductAmount = 0;
                    rcReturnItem.ReturnPriceType = 0;
                }
                else
                {
                    rcReturnItem.ReturnPriceType = (int)RmaStatus.商品退款价格类型.自定义价格;

                    //TODO
                    //从吴文强返回结果中查询对应商品的退换货单价
                    var queryPrice = from oi in oldOrder.Items
                                     join nri in cReturn.OrderItemAmount on oi.OrderItemSysNo equals nri.Key
                                     where oi.ProductSysNo == applyProductInfo.ProductSysNo
                                     select nri.Value;

                    price = queryPrice.Single();
                    rcReturnItem.RefundProductAmount = price;
                }

                returnItems.Add(rcReturnItem);

                //累计退款金额
                totalReturnCharge += rcReturnItem.RefundProductAmount;
            }

            #endregion

            if (applyOrder.ReturnType == (int)RmaStatus.RMA类型.售后退货)
            {

                #region 计算退款

                //应退
                returnEntity.OrginAmount = cReturn.OrginAmount; //应退商品金额
                returnEntity.OrginPoint = cReturn.OrginPoint;   //退款积分我为1块钱一个积分，所以和退款金额一致
                returnEntity.OrginCoin = cReturn.OrginCoin;     //应退惠源币

                //实退
                returnEntity.RefundProductAmount = cReturn.RefundProductAmount; //实退商品金额
                returnEntity.RefundPoint = cReturn.RefundPoint;                 //实退总积分
                returnEntity.RefundTotalAmount = cReturn.RefundTotalAmount;
                returnEntity.RefundCoin = cReturn.RefundCoin;                   //实退惠源币

                returnEntity.HandleDepartment = (int)Model.WorkflowStatus.RmaStatus.退换货处理部门.客服中心;

                //实体商品金额减去发票扣款=实退总金额
                returnEntity.IsPickUpInvoice = (oldOrder.InvoiceStatus == FinanceStatus.发票状态.已取回) ? 1 : 0;
                if (oldOrder.HasInvoice)
                {
                    if (oldOrder.InvoiceStatus != FinanceStatus.发票状态.已取回)
                    {
                        returnEntity.IsPickUpInvoice = 1;
                        returnEntity.DeductedInvoiceAmount = cReturn.DeductedInvoiceAmount;
                        returnEntity.RedeemAmount = cReturn.RedeemAmount;
                        returnEntity.CouponAmount = cReturn.CouponAmount;
                    }
                    else
                    {
                        returnEntity.IsPickUpInvoice = 0;
                        returnEntity.DeductedInvoiceAmount = 0;
                    }
                }

                #endregion
            }

            return;
        }

        #endregion

    }
}
