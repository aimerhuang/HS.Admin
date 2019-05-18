using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.UpGrade;
using Hyt.Service.Contract.MallSeller;
using Hyt.Util.Net;

namespace Hyt.Service.Implement.MallSeller
{
    /// <summary>
    /// 分销商升舱订单退货查询实现类
    /// </summary>
    /// <remarks>2013-8-29 陶辉 创建</remarks>
    public class MallRma : BaseService, IMallRma
    {
        /// <summary>
        /// /// <summary>
        /// 获取分销商所有已申请退货订单
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns>已申请退货列表</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        /// <remarks>2013-09-10 余勇 实现</remarks>
        public Result<PagedList<UpGradeRma>> GetMallRmaList(MallRmaParameters param)
        {

            var filter = new ParaDsReturnFilter
            {
                PageIndex = param.PageIndex <= 0 ? 1 : param.PageIndex,
                PageSize = param.PageSize,
                MallProductName = param.ProductName,
                MallProductId = param.ProductCode,
                MallOrderId = param.OrderId,
                BuyerNick = param.BuyerName,
                BeginDate = param.StartDate,
                EndDate = param.EndDate,
                DealerMallSysNo=param.DealerMallSysNo
            };

            var dsOrders = BLL.MallSeller.DsReturnBo.Instance.GetPagerList(filter);

            var list = new List<UpGradeRma>();

            foreach (var order in dsOrders.Rows)
            {
                var mallRmaInfo = new UpGradeRma
                {
                    MallOrderId = order.MallOrderId,
                    ApplyTime = order.ApplicationTime,
                    BuyerRmaMessage = order.BuyerRemark,
                    BuyerRmaReason = order.RmaRemark,
                    HytRmaStatus = Hyt.Util.EnumUtil.GetDescription(typeof(Hyt.Model.WorkflowStatus.RmaStatus.退换货状态), order.Status),
                    MallBuyerName = order.BuyerNick,
                    MallRefundFee = order.MallReturnAmount,
                    MallRmaId = order.MallReturnId,
                    MallRmaMessage = "",
                    MallType = 1,
                    HytRmaId = order.RcReturnSysNo
                };
                list.Add(mallRmaInfo);
            }

            var result = new Result<PagedList<UpGradeRma>>
            {
                Data = new PagedList<UpGradeRma>
                {
                    TotalItemCount = dsOrders.TotalRows,
                    CurrentPageIndex = dsOrders.CurrentPage,
                    TData = list
                },
                Status = true
            };

            return result;
        }

        /// <summary>
        /// 获取退货单详情
        /// </summary>
        /// <param name="rmaId">商城退货单编号</param>
        /// <returns>退货单详情</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        /// <remarks>2013-09-10 朱家宏 实现</remarks>
        /// <remarks>2013-12-3 黄志勇 修改 处理hytRma抛异常</remarks>
        public Result<UpGradeRma> GetMallRma(string rmaId)
        {
            var rmaOrder = IDsReturnDao.Instance.SelectByRmaSysNo(int.Parse(rmaId));

            var rmaOrderItems = IDsReturnDao.Instance.SelectItems(rmaOrder.SysNo);

            //HYT订单数据
            var dsOrder = BLL.MallSeller.DsOrderBo.Instance.GetDsOrderByMallOrderId(rmaOrder.MallOrderId);

            var mallRmaItems = new List<UpGradeRmaItem>();
            var hytRma = BLL.RMA.RmaBo.Instance.GetRMA(rmaOrder.RcReturnSysNo);
            if (hytRma != null && hytRma.RMAItems != null)
            {
                foreach (var item in hytRma.RMAItems)
                {

                    var myid = BLL.MallSeller.DsOrderBo.Instance.GetDsOrderItemAssociationByOutStockItemNo(item.StockOutItemSysNo).Select(m => m.DsOrderItemSysNo).FirstOrDefault();
                    var mallRmaItem = rmaOrderItems.FirstOrDefault(o => o.SysNo == myid);
                    if (mallRmaItem == null) mallRmaItem = new DsReturnItem();
                    mallRmaItems.Add(new UpGradeRmaItem
                    {
                        DiscountFee = 0,
                        HytProductCode = item.ProductSysNo.ToString(),
                        HytRmaAmount =item.RefundProductAmount,
                        MallOrderItemId = mallRmaItem.MallItemNo.ToString(),
                        MallProductAttrs = mallRmaItem.MallProductAttribute,
                        MallProductName = mallRmaItem.MallProductName,
                        MallProductCode = mallRmaItem.MallProductId,
                        MallQuantity = item.RmaQuantity,
                        HytProductName = item.ProductName,
                        HytProductErpCode = BLL.Product.PdProductBo.Instance.GetProductErpCode(item.ProductSysNo)
                    });
                }
            }
            

            var mallRmaInfo = new UpGradeRma
                {
                    ApplyTime = rmaOrder.ApplicationTime,
                    BuyerRmaMessage = rmaOrder.BuyerRemark,
                    BuyerRmaReason = rmaOrder.RmaRemark,
                    MallBuyerName = rmaOrder.BuyerNick,
                    MallOrderId = rmaOrder.MallOrderId,
                    MallRefundFee = rmaOrder.MallReturnAmount,
                    HytRmaId = rmaOrder.RcReturnSysNo,
                    MallRmaMessage = rmaOrder.RmaRemark,
                    RmaItems = mallRmaItems
                };

            var result = new Result<UpGradeRma>
                {
                    Data = mallRmaInfo,
                    Status = true
                };
            return result;
        }

        /// <summary>
        /// 获取退货单处理进度
        /// </summary>
        /// <param name="rmaId">商城退货单编号</param>
        /// <returns>退货处理进度</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        /// <remarks>2013-09-10 朱家宏 实现</remarks>
        public Result<List<HytRmaLog>> GetMallRmaLogs(int rmaId)
        {
            var list = new List<HytRmaLog>();

            var logs = BLL.RMA.RmaBo.Instance.GetLogList(rmaId);
            foreach (var log in logs)
            {
                list.Add(new HytRmaLog
                    {
                        LogContent = log.LogContent,
                        OperateDate = log.OperateDate,
                        Operator = log.Operator,
                        OperatorName = log.UserName,
                        ReturnSysNo = log.ReturnSysNo,
                        SysNo = log.SysNo,
                        TransactionSysNo = log.TransactionSysNo
                    });
            }
            var result = new Result<List<HytRmaLog>>
                {
                    Data = list,
                    Status = true
                };

            return result;
        }

        /// <summary>
        /// 根据第三方订单号获取可退货的商品信息
        /// </summary>
        /// <param name="dsOrderSysNo">订单升舱编号</param>
        /// <returns>可退货商品信息</returns>
        /// <remarks>2013-9-12 陶辉 创建</remarks>
        /// <remarks>2013-9-12 朱家宏 实现</remarks>
        public Result<UpGradeRma> BuildMallRma(int dsOrderSysNo)
        {
            //HYT订单数据
            var dsOrder = BLL.MallSeller.DsOrderBo.Instance.SelectBySysNo(dsOrderSysNo);
            var soOrder = DataAccess.Order.ISoOrderDao.Instance.GetByTransactionSysNo(dsOrder.OrderTransactionSysNo);
            var orderSysNo = soOrder.SysNo;
            var hytStockOutItems = new List<UpGradeRmaItem>(); //页面出库单显示列表
            var stockOutList = BLL.Warehouse.WhWarehouseBo.Instance.GetWhStockOutListByOrderID(orderSysNo);
            //获取该订单的出库单列表（包含出库单明细)
            foreach (var stockOut in stockOutList)
            {
                if (BLL.RMA.RmaBo.Instance.CanReturn(stockOut))
                {
                    foreach (var item in stockOut.Items)
                    {
                        var ritem = new UpGradeRmaItem
                        {
                            DiscountFee = 0,
                            HytProductCode = item.ProductSysNo.ToString(),
                            HytProductErpCode = Hyt.BLL.Product.PdProductBo.Instance.GetProductErpCode(item.ProductSysNo),
                            HytProductName = item.ProductName,
                            HytProductPrice = Math.Round(item.RealSalesAmount / item.ProductQuantity, 2),//计算单价
                            MallQuantity = item.ProductQuantity - BLL.RMA.RmaBo.Instance.GetAllRmaQuantity(orderSysNo, item.SysNo, 0),//可退数量
                            ProductQuantity = item.ProductQuantity,
                            StockOutItemSysNo = item.SysNo
                        };
                        ritem.HytRmaAmount = Math.Round(item.RealSalesAmount * ((decimal)ritem.MallQuantity / (decimal)item.ProductQuantity), 2);//根据可退数量计算可退金额
                        hytStockOutItems.Add(ritem);
                    }
                }
            }
            var rmaInfo = new UpGradeRma
            {
                    MallOrderId = dsOrder.MallOrderId,
                    ApplyTime = DateTime.Now,
                    MallBuyerName = dsOrder.BuyerNick,
                    RmaItems = hytStockOutItems
            };
            var result = new Result<UpGradeRma>
            {
                    Data = rmaInfo,
                    Status = true
             };
            return result;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="img">图片信息</param>
        /// <returns>上传结果</returns>
        /// <remarks>2013-09-10 朱成果 实现</remarks>
        public Result SaveRmaImg(UpGradeRmaImage img)
        {
            var result = new Result() { Status = true };
            try
            {
                if (img.FileData != null)
                {
                    using (var ms = new MemoryStream(img.FileData))
                    {
                        string filePath = "RMA/" + img.FileName;
                        var ftp = new FtpUtil(FtpImageServer, FtpUserName, FtpPassword);
                        var flg= ftp.Upload(ms, FtpImageServer + filePath);//FTP上传
                        if (flg)
                        {
                            result.Message=img.ImageUrl = filePath;//图片存相对路径方便迁移
                        }
                    }
                }
                ////退换货图片
                //if (!string.IsNullOrEmpty(img.ImageUrl))
                //{
                //    BLL.RMA.RmaBo.Instance.InsertRMAImg(new RcReturnImage()
                //    {
                //        ImageUrl = img.ImageUrl,
                //        ReturnSysNo = img.ReturnSysNo
                //    });
                //}
            }
            catch (Exception ex)
            {
                result.Message = "图片上传错误";
                result.Status = false;
            }
            return result;
        }

        /// <summary>
        /// 商城退货单导入商城
        /// </summary>
        /// <param name="mallRma">退货单实体</param>
        /// <returns>处理结果</returns>
        /// <remarks>2013-8-29 陶辉 创建</remarks>
        /// <remarks>2013-09-10 朱家宏 实现</remarks>
        /// <remarks>2013-10-21 黄志勇 修改退换货子表实退商品金额</remarks>
        public Result ImportMallRma(UpGradeRma mallRma)
        {
            //直接申请退换单，根据原订单匹配成功的商品明细进行退货
            var result = new Result();

            try
            {


                //升舱订单数据
                var dsOrder = BLL.MallSeller.DsOrderBo.Instance.GetDsOrderByMallOrderId(mallRma.MallOrderId);
                var dsOrderItems = BLL.MallSeller.DsOrderBo.Instance.GetDsOrderItems(dsOrder.SysNo);


                //HYT订单主表数据
                var soOrder =
                    DataAccess.Order.ISoOrderDao.Instance.GetByTransactionSysNo(dsOrder.OrderTransactionSysNo);

                if (soOrder.Status != (int)Model.WorkflowStatus.OrderStatus.销售单状态.已完成)
                {
                    result.Message = "未完成的订单不允许做此操作。";
                    result.Status = false;
                    result.StatusCode = -1;
                    return result;
                }

                //订单明细数据
                var soOrderItems = BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(soOrder.SysNo);

                if (soOrderItems == null) throw new ArgumentNullException();

                //退换货明细
                var rcReturnItems = new List<RcReturnItem>();
                foreach (var item in mallRma.RmaItems)
                {
                    var outitem = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhStockOutItem(item.StockOutItemSysNo);
                    SoOrderItem soOrderItem = null;
                    if (outitem != null)
                    {
                        soOrderItem = soOrderItems.FirstOrDefault(m => m.SysNo == outitem.OrderItemSysNo);
                    }
                    if (soOrderItem != null)
                    {
                        var rcReturnItem = new RcReturnItem
                            {
                                OriginPrice = soOrderItem.OriginalPrice,
                                ProductName = soOrderItem.ProductName,
                                ProductSysNo = int.Parse(item.HytProductCode),
                                ReturnPriceType = (int)Model.WorkflowStatus.RmaStatus.商品退款价格类型.自定义价格,
                                ReturnType = (int)Model.WorkflowStatus.RmaStatus.商品退换货类型.新品,
                                RmaQuantity = item.MallQuantity,
                                RmaReason = "",
                                StockOutItemSysNo = item.StockOutItemSysNo,
                                RefundProductAmount = Math.Round(outitem.RealSalesAmount * ((decimal)item.MallQuantity / (decimal)outitem.ProductQuantity), 2)
                            };
                        rcReturnItems.Add(rcReturnItem);
                    }
                }

                var refundProductAmount = rcReturnItems.Sum(o => o.RefundProductAmount); //退款金额合计

                var rcReturn = new CBRcReturn
                    {
                        CreateBy = soOrder.CustomerSysNo,
                        CreateDate = DateTime.Now,
                        CustomerSysNo = soOrder.CustomerSysNo,
                        HandleDepartment = (int)Model.WorkflowStatus.RmaStatus.退换货处理部门.客服中心,
                        InvoiceSysNo = soOrder.InvoiceSysNo,
                        LastUpdateBy = soOrder.OrderCreatorSysNo,
                        LastUpdateDate = DateTime.Now,
                        OrderSysNo = soOrder.SysNo,
                        ReceiveAddressSysNo = soOrder.ReceiveAddressSysNo,
                        RMARemark = "",
                        RmaType = (int)Model.WorkflowStatus.RmaStatus.RMA类型.售后退货,
                        Source = (int)Model.WorkflowStatus.RmaStatus.退换货申请单来源.分销商,
                        Status = (int)Model.WorkflowStatus.RmaStatus.退换货状态.待审核,
                        WarehouseSysNo = soOrder.DefaultWarehouseSysNo,
                        RMAItems = rcReturnItems,
                        DeductedInvoiceAmount = 0,
                        InternalRemark = mallRma.BuyerRmaReason,//退款说明
                        IsPickUpInvoice = 0,
                        OrginAmount = refundProductAmount,
                        OrginPoint = (int)refundProductAmount,
                        RefundPoint = (int)refundProductAmount,
                        PickUpAddressSysNo = 0,
                        PickUpTime = "",
                        PickupTypeSysNo = 0,
                        RedeemAmount = 0,
                        RefundAccount = "",
                        RefundAccountName = "",
                        RefundBank = "",
                        RefundBy = 0,
                        ShipTypeSysNo = 0,
                        RefundDate = DateTime.Now,
                        RefundProductAmount = refundProductAmount,
                        RefundTotalAmount = refundProductAmount,
                        RefundType = (int)Hyt.Model.WorkflowStatus.RmaStatus.退换货退款方式.分销商预存
                    };

                //当前操作用户
                var htyUserSysNo = IDsOrderDao.Instance.GetDealer(mallRma.DealerSysNo).UserSysNo;
                var syUser = BLL.Sys.SyUserBo.Instance.GetSyUser(htyUserSysNo);
                var pickaddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(soOrder.ReceiveAddressSysNo);//收货地址变成取件地址
                pickaddress.SysNo = 0;
                using (var tran = new TransactionScope())
                {
                    //创建rma单
                    var rmaSysNo = BLL.RMA.RmaBo.Instance.InsertRMA(rcReturn, pickaddress, null, syUser);
                    var htyRma = BLL.RMA.RmaBo.Instance.GetRcReturnEntity(rmaSysNo);
                    //分销商退换货单
                    var dsReturn = new DsReturn
                        {
                            ApplicationTime = mallRma.ApplyTime,
                            BuyerNick = mallRma.MallBuyerName,
                            RmaRemark = mallRma.BuyerRmaReason,
                            DealerMallSysNo = mallRma.DealerMallSysNo,
                            MallOrderId = mallRma.MallOrderId,
                            MallReturnAmount = mallRma.MallRefundFee,
                            MallReturnId = mallRma.MallRmaId,
                            BuyerRemark = mallRma.MallRmaMessage,
                            RmaType = (int)Model.WorkflowStatus.RmaStatus.RMA类型.售后退货,
                            RcReturnSysNo = rmaSysNo,
                            ReturnTransactionSysNo = htyRma.TransactionSysNo
                        };



                    var dsReturnItems = new List<DsReturnItem>();
                    foreach (var rmaItem in mallRma.RmaItems)
                    {

                        var myid = BLL.MallSeller.DsOrderBo.Instance.GetDsOrderItemAssociationByOutStockItemNo(rmaItem.StockOutItemSysNo).Select(m => m.DsOrderItemSysNo).FirstOrDefault();
                        DsOrderItem dsOrderItem = dsOrderItems.FirstOrDefault(m => m.SysNo == myid);
                        if (dsOrderItem == null) dsOrderItem = new DsOrderItem();
                         dsReturnItems.Add(new DsReturnItem
                                    {
                                        Amount = rmaItem.HytRmaAmount,
                                        MallProductAttribute = dsOrderItem.MallProductAttribute,
                                        MallProductId = dsOrderItem.MallProductId,
                                        MallProductName = dsOrderItem.MallProductName,
                                        Quantity = rmaItem.MallQuantity
                                    });
                        
                    }

                    //创建『分销商退换货单』
                    BLL.MallSeller.DsReturnBo.Instance.Create(dsReturn, dsReturnItems);

                    if (!string.IsNullOrEmpty(mallRma.ImgPaths))
                    {
                        string[] paths = mallRma.ImgPaths.Split(',');
                        foreach (string str in paths)
                        {
                              BLL.RMA.RmaBo.Instance.InsertRMAImg(new RcReturnImage()
                                {
                                    ImageUrl = str,
                                    ReturnSysNo = rmaSysNo
                                });

                        }
                    }

                    result.Message = "操作成功。";
                    result.Status = true;
                    result.StatusCode = rmaSysNo;

                    tran.Complete();
                }
            }
            catch
            {
                result.Message = "操作失败。";
                result.Status = false;
                result.StatusCode = -1;
            }

            return result;
        }

        /// <summary>
        /// 获取分销商退换货单
        /// </summary>
        /// <param name="shopAccount">账户</param>
        /// <param name="mallTypeSysNo">类型</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">退款完成</param>
        /// <returns>分销商退换货单列表</returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public Result<List<DsReturn>> GetReturnInfo(string shopAccount, int mallTypeSysNo, int top, bool? isFinish)
        {
            var model = BLL.MallSeller.DsOrderBo.Instance.GetReturn(shopAccount, mallTypeSysNo, top, isFinish);
            var list = new List<DsReturn>();
            if (model != null && model.Count > 0)
            {
                foreach (var cbDsReturn in model)
                {
                    var info = new DsReturn();
                    Hyt.Util.Reflection.ReflectionUtils.Transform(cbDsReturn, info);
                    list.Add(info);
                }
            }
            var result = new Result<List<DsReturn>>
                {
                    Data = list,
                    Status = true
                };
            return result;
        }

        /// <summary>
        /// 查询可退换货升舱订单
        /// </summary>
        /// <param name="param">订单查询参数</param>
        /// <returns>第三方订单实体分页数据</returns>
        /// <remarks>2014-1-8 黄志勇 代码规范</remarks>
        public Result<PagedList<UpGradeOrder>> GetCanRmaOrderList(MallOrderParameters param)
        {
            Pager<CBDsOrder> dsOrders;
            var filter = new ParaDsOrderFilter
            {
                PageIndex = param.PageIndex <= 0 ? 1 : param.PageIndex,
                PageSize = param.PageSize,
                MallProductName = param.ProductName,
                MallProductId = param.ProductCode,
                MallOrderId = param.OrderID,
                BuyerNick = param.BuyerName,
                BeginDate = param.StartDate,
                EndDate = param.EndDate,
                DealerMallSysNo=param.DealerMallSysNo
            };
            dsOrders = Hyt.BLL.MallSeller.DsOrderBo.Instance.QueryForRma(filter);
            var list = new List<UpGradeOrder>();
            foreach (var order in dsOrders.Rows)
            {
                var mallOrderInfo = new UpGradeOrder
                {
                    HytOrderDealer = new HytOrderDealerInfo
                    {
                        HytOrderTime = order.UpgradeTime,
                        LogisticsTime = order.DeliveryTime,
                        DsOrderSysNo = order.SysNo,
                        HytPayStatus = order.PayStatus,
                        DeliveryStatus = order.Status.ToString(),
                        HytPayTime = order.PayTime
                    },
                    MallOrderBuyer = new MallOrderBuyerInfo
                    {
                        MallOrderId = order.MallOrderId,
                        BuyerNick = order.BuyerNick
                    },
                    MallOrderPayment = new MallOrderPaymentInfo(),
                    MallOrderReceive = new MallOrderReceiveInfo()
                };
                list.Add(mallOrderInfo);
            }
            var result = new Result<PagedList<UpGradeOrder>>
            {
                Data = new PagedList<UpGradeOrder>
                {
                    TotalItemCount = dsOrders.TotalRows,
                    CurrentPageIndex = dsOrders.CurrentPage,
                    TData = list
                },
                Status = true
            };
            return result;
        }
    }
}
