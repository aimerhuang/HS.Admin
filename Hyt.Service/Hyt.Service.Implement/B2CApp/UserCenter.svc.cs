using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Instrumentation;
using System.Transactions;
using Hyt.BLL.Basic;
using Hyt.BLL.CRM;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using System.Linq;
using Hyt.BLL.Promotion;
using Hyt.BLL.RMA;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.BLL.Web;
using Hyt.DataAccess.Log;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.B2CApp;
using Hyt.Util.Net;
using IsolationLevel = System.Transactions.IsolationLevel;
using SoOrderBo = Hyt.BLL.Order.SoOrderBo;
using TransactionLog = Hyt.Model.B2CApp.TransactionLog;
using WhWarehouseBo = Hyt.BLL.Warehouse.WhWarehouseBo;

namespace Hyt.Service.Implement.B2CApp
{
    /// <summary>
    /// 个人中心管理接口相关
    /// </summary>
    /// <remarks> 2013-7-1 杨浩 创建</remarks>
    public class UserCenter : BaseService, IUserCenter
    {
        #region 地址管理

        /// <summary>
        /// 新增用户收货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns>返回新地址的系统号</returns>
        /// <remarks>
        /// 2013-8-1 杨浩 创建
        /// 2013-08-20 郑荣华 实现
        /// </remarks>
        public Result<int> AddAddress(CrReceiveAddress address)
        {
            address.IsDefault = (int)CustomerStatus.是否默认地址.否;
            address.Gender = (int)CustomerStatus.性别.保密;
            var rSysNo = CrReceiveAddressBo.Instance.InsertReceiveAddress(address);
            return new Result<int>
                {
                    Data = rSysNo,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 更新收货地址
        /// </summary>
        /// <param name="address">收货地址</param>
        /// <returns>执行结果</returns>
        /// <remarks>
        /// 2013-8-1 杨浩 创建
        /// 2013-08-20 郑荣华 实现
        /// </remarks>
        public Result UpdateAddress(CrReceiveAddress address)
        {
            var oldAddress = CrReceiveAddressBo.Instance.GetCrReceiveAddress(address.SysNo);
            oldAddress.MobilePhoneNumber = address.MobilePhoneNumber;
            oldAddress.EmailAddress = address.EmailAddress;
            oldAddress.Title = address.Title;
            oldAddress.StreetAddress = address.StreetAddress;
            oldAddress.AreaSysNo = address.AreaSysNo;
            oldAddress.Name = address.Name;
            oldAddress.ZipCode = address.ZipCode;

            if (SoOrderBo.Instance.UpdateReceiveAddress(oldAddress) > 0)
                return new Result
                {
                    Status = true,
                    StatusCode = 1
                };
            return new Result
            {
                Status = false,
                StatusCode = -1
            };
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="sysNo">地址编号</param>
        /// <returns>执行结果</returns>
        /// <remarks>
        /// 2013-8-1 杨浩 创建
        /// 2013-08-20 郑荣华 实现
        /// </remarks>
        public Result DeleteAddress(int sysNo)
        {
            if (CrReceiveAddressBo.Instance.Delete(sysNo))
                return new Result
                {
                    Status = true,
                    StatusCode = 1
                };
            return new Result
            {
                Status = false,
                StatusCode = -1
            };
        }

        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="sysNo">地址编号</param>
        /// <returns>执行结果</returns>
        /// <remarks>
        /// 2013-8-1 杨浩 创建
        /// 2013-08-20 郑荣华 实现
        /// 2013-08-28 周瑜 修改
        /// </remarks>
        public Result SetDefaultAddress(int sysNo)
        {
            var customerSysNo = CurrentUser.SysNo;
            if (CrReceiveAddressBo.Instance.SetDefaultAddress(sysNo, customerSysNo))
                return new Result
                {
                    Status = true,
                    StatusCode = 1
                };

            return new Result
            {
                Status = false,
                StatusCode = -1
            };
        }

        #endregion

        #region Home

        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <param name="imgBase64"></param>
        /// <returns>状态</returns>
        /// <remarks>2013-8-29 杨浩 添加</remarks>
        public Result UploadAvatar(string imgBase64)
        {
            byte[] buffer = Convert.FromBase64String(imgBase64);
            var ms = new MemoryStream(buffer);

            //上传到FTP的路径
            string ftpPath = string.Format("{0}/{1}/{2}.jpg", FtpImageServer, BLL.Web.ProductThumbnailType.CustomerFace, CurrentUser.SysNo);
            //上传
            var ftp = new FtpUtil(FtpImageServer, FtpUserName, FtpPassword);
            ftp.Upload(ms, ftpPath);
            ms.Dispose();

            return new Result
            {
                Status = true,
                StatusCode = 1,
                Message = "头像上传成功"
            };

        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns>头像地址</returns>
        /// <remarks>2013-8-29 杨浩 添加</remarks>
        public Result<string> GetAvatar()
        {
            var headImagePath = BLL.Web.ProductImageBo.Instance.GetHeadImagePath(ProductThumbnailType.CustomerFace, CurrentUser.SysNo);
            return new Result<string>
                {
                    Data = headImagePath,
                    Status = true
                };
        }

        /// <summary>
        /// 获取用户中心首页的所有提示数
        /// </summary>
        /// <returns>气泡数</returns>
        /// <remarks>杨浩 添加</remarks>
        /// <remarks>2013-08-27 周瑜 实现逻辑</remarks>
        public Result<Tips> GetAllTips()
        {

            //获取站内信的总数
            var messageNum = CrSiteMessageBo.Instance.GetCount(CurrentUser.SysNo)[(int)CustomerStatus.站内信状态.未读];
            //获取商品关注数
            var attentionNum = BLL.CRM.CrFavoritesBo.Instance.GetAttentionCount(CurrentUser.SysNo);
            var deliveriesNum = LgDeliveryBo.Instance.GetDeliveryingCount(CurrentUser.SysNo);
            var obligationNum = SoOrderBo.Instance.GetUnPaidOrderCount(CurrentUser.SysNo);
            var noEvaluationNum = SoOrderBo.Instance.GetUnCommentsCount(CurrentUser.SysNo);
            var couponNum = SpCouponBo.Instance.GetCustomerCoupons(CurrentUser.SysNo, PromotionStatus.优惠券状态.已审核).Count; //优惠券数
            return new Result<Tips>
                {
                    Data = new Tips
                            {
                                AttentionNum = attentionNum,//商品关注数
                                CouponNum = couponNum, //优惠券数
                                MessageNum = messageNum, //消息数量
                                NoEvaluationNum = noEvaluationNum, //待评价数
                                ObligationNum = obligationNum, //待付款数量
                                DeliveriesNum = deliveriesNum //配送中数
                            },
                    Status = true,
                    StatusCode = 1
                };
        }

        #endregion

        #region 商品列表

        /// <summary>
        /// 晒单或评价商品列表 
        /// </summary>
        /// <param name="filter">过滤条件(30 为待评价 ， 0所有)</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>晒单或评价商品列表</returns>
        /// <remarks>
        /// 2013-08-01 杨浩 添加
        /// 2013-08-29 郑荣华 实现
        /// </remarks>
        public ResultPager<IList<ShowOrComment>> GetProductShowOrComment(int filter, int pageIndex)
        {
            var pager = new Pager<CBSoOrderItem>
                {
                    CurrentPage = pageIndex,
                    PageSize = 10
                };
            Hyt.BLL.Front.FeProductCommentBo.Instance.GetProductShowOrComment(null, CurrentUser.SysNo, ref pager);
            if ((int)AppEnum.OrderFilter.待评价 == filter)
            {
                // pager.Rows = pager.Rows.Where(x => x.CommentStatus == 0).ToList();
            }
            var data = pager.Rows.ToList().Select(item => new ShowOrComment()
                {
                    LevelPrice = item.SalesUnitPrice,
                    ProductName = item.ProductName,
                    ProductSysNo = item.ProductSysNo,
                    OrderSysNo = item.OrderSysNo,
                    Quantity = item.RealStockOutQuantity,
                    Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, item.ProductSysNo),
                    OperateStatus = GetOperateStatus(item.OrderSysNo, item.ProductSysNo).ToString()
                }).ToList();

            return new ResultPager<IList<ShowOrComment>>
                {
                    HasMore = pager.TotalPages > pageIndex,
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取评论晒单操作状态
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="productSysNo"></param>
        /// <returns>显示评论晒单操作状态</returns>
        /// <remarks>
        /// 2013-08-29 郑荣华 创建
        /// 2013-12-06 杨浩 修改
        /// 2014-06-17 陶辉 修改
        /// </remarks>
        private AppEnum.OperateStatus GetOperateStatus(int sysNo, int productSysNo)
        {
            var operateStatus = AppEnum.OperateStatus.CommentAndShow;
            var list = Hyt.BLL.Front.FeProductCommentBo.Instance.GetProductCommentList(sysNo, productSysNo,
                CurrentUser.SysNo);

            if (list != null)
            {
                var hascomment = list.Any(m => m.IsComment == (int)ForeStatus.是否评论.是);
                var hasshare = list.Any(m => m.IsShare == (int)ForeStatus.是否晒单.是);

                if (!hascomment && !hasshare)
                {
                    operateStatus = AppEnum.OperateStatus.CommentAndShow;
                }
                if (!hascomment && hasshare)
                {
                    operateStatus = AppEnum.OperateStatus.CommentAndShowed;
                }
                if (hascomment && hasshare)
                {
                    operateStatus = AppEnum.OperateStatus.CommentedAndShowed;
                }
                if (hascomment && !hasshare)
                {
                    operateStatus = AppEnum.OperateStatus.CommentedAndShow;
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 获取用户关注商品
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <returns>关注的商品列表</returns>
        /// <remarks>
        /// 杨浩 添加
        /// 2013-08-26 郑荣华 实现
        /// </remarks>
        public ResultPager<IList<AttentionProduct>> GetAttentionProduct(int pageIndex)
        {
            var crSysNo = CurrentUser.SysNo;
            var levelSysNo = CurrentUser.LevelSysNo;
            var pager = new Pager<CBCrFavorites>
                {
                    CurrentPage = pageIndex,
                    PageSize = 8
                };

            BLL.CRM.CrFavoritesBo.Instance.GetCrFavoritesList(crSysNo, null, ref pager);

            var data = pager.Rows.ToList().Select(item =>
                {
                    var levelPrice = Hyt.BLL.Product.PdPriceBo.Instance.GetProductPrice(item.ProductSysNo).SingleOrDefault(x => x.PriceSource == (int)ProductStatus.产品价格来源.会员等级价 && x.SourceSysNo == levelSysNo);
                    return new AttentionProduct
                        {
                            ProductName = item.ProductName,
                            Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, item.ProductSysNo),
                            LevelPrice = levelPrice == null ? 99999 : levelPrice.Price, //查询不到设为99999
                            ProductSysNo = item.ProductSysNo,
                            SysNo = item.SysNo
                        };
                }).ToList();

            return new ResultPager<IList<AttentionProduct>>
                {
                    Data = data,
                    HasMore = pager.TotalPages > pageIndex,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 删除用户关注商品
        /// </summary>
        /// <param name="sysNo">关注系统编号</param>
        /// <returns>操作结果</returns>
        /// <remarks>
        /// 2013-08-01 杨浩 添加
        /// 2013-08-26 郑荣华 实现
        /// </remarks>
        public Result DeleteAttentionProduct(int sysNo)
        {
            if (BLL.CRM.CrFavoritesBo.Instance.Delete(CurrentUser.SysNo, sysNo) > 0)
                return new Result //成功
                    {
                        Status = true,
                        StatusCode = 1
                    };
            return new Result
            {
                Status = false,
                StatusCode = -1
            };
        }

        /// <summary>
        /// 添加商品关注(收藏)已关注则删除关注，未关注则添加关注
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>操作结果</returns>
        /// <remarks>
        /// 2013-08-01 杨浩 添加
        /// 2013-08-26 郑荣华 实现
        /// </remarks>
        public Result AddAttention(int productSysNo)
        {

            var isAttention = BLL.CRM.CrFavoritesBo.Instance.IsAttention(CurrentUser.SysNo, productSysNo);
            var message = string.Empty;
            if (isAttention)
            {
                //已关注删除关注
                if (BLL.CRM.CrFavoritesBo.Instance.Delete(CurrentUser.SysNo, productSysNo) > 0)
                    message = "收藏已取消";
                else
                    message = "收藏取消失败";
            }
            else
            {
                var rSysNo = BLL.CRM.CrFavoritesBo.Instance.Create(CurrentUser.SysNo, productSysNo);

                if (rSysNo > 0)
                    message = "收藏成功";
            }

            return new Result { Status = true, Message = message };
        }

        #endregion

        #region 基础实现

        /// <summary>
        /// 获取用户消息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <returns>用户消息列表</returns>
        /// <remarks>2013-08-29 周唐炬 实现</remarks>
        public ResultPager<IList<CrSiteMessage>> GetMessages(int pageIndex)
        {
            var result = new ResultPager<IList<CrSiteMessage>>() { StatusCode = -1 };
            var status = new CustomerStatus.站内信状态[]
                {
                    CustomerStatus.站内信状态.已读,
                    CustomerStatus.站内信状态.未读
                };
            var list = CrSiteMessageBo.Instance.GetPage(CurrentUser.SysNo, pageIndex, status);
            if (list != null && list.TData.Any())
            {
                result.Data = list.TData;
                result.StatusCode = 0;
                result.Status = true;
            }
            else
            {
                result.Message = "暂无消息!";
            }
            return result;
        }

        #endregion

        #region 订单列表

        #region  private
        /// <summary>
        /// 订单可操作转换
        /// </summary>
        /// <param name="status">销售单状态</param>
        /// <param name="payStatus">销售单支付状态</param>
        /// <param name="payTypeSysNo">支付方式</param>
        /// <returns>订单状态</returns>
        /// <remarks>2013-10-28 杨浩 添加</remarks>
        private AppEnum.OperateStatus GetOperateStatus(OrderStatus.销售单状态 status, OrderStatus.销售单支付状态 payStatus, int payTypeSysNo)
        {
            var operateStatus = AppEnum.OperateStatus.None;
            switch (status)
            {
                case OrderStatus.销售单状态.待审核:
                    if (payStatus == OrderStatus.销售单支付状态.未支付 &&
                        (Model.SystemPredefined.PaymentType.网银 == payTypeSysNo ||
                         Model.SystemPredefined.PaymentType.支付宝 == payTypeSysNo))
                        operateStatus = AppEnum.OperateStatus.CancelAndPay;
                    else if (payStatus == OrderStatus.销售单支付状态.已支付)
                        operateStatus = AppEnum.OperateStatus.None;
                    else
                        operateStatus = AppEnum.OperateStatus.Cancel;
                    break;
                case OrderStatus.销售单状态.待支付:
                    if ((Model.SystemPredefined.PaymentType.网银 == payTypeSysNo ||
                         Model.SystemPredefined.PaymentType.支付宝 == payTypeSysNo))
                        operateStatus = AppEnum.OperateStatus.CancelAndPay;
                    else
                        operateStatus = AppEnum.OperateStatus.Cancel;
                    break;
                case OrderStatus.销售单状态.已创建出库单:
                    operateStatus = AppEnum.OperateStatus.Logistics;
                    break;
                case OrderStatus.销售单状态.已完成:
                    operateStatus = AppEnum.OperateStatus.Logistics;
                    break;
                case OrderStatus.销售单状态.作废:
                    operateStatus = AppEnum.OperateStatus.None;
                    break;
            }
            return operateStatus;
        }

        /// <summary>
        /// 判断订单支付状态
        /// </summary>
        /// <param name="payStatus"></param>
        /// <param name="orderStatus"></param>
        /// <param name="onlineStatus"></param>
        /// <returns>状态文本</returns>
        /// <remarks>2013-08-01 杨浩 添加</remarks>
        private string GetOnlineStatus(int payStatus, int orderStatus, string onlineStatus)
        {
            if (payStatus == (int)OrderStatus.销售单支付状态.已支付 && orderStatus == (int)OrderStatus.销售单状态.待审核)
            {
                onlineStatus = "已支付";
            }
            return onlineStatus;
        }

        #endregion

        /// <summary>
        /// 用户订单列表
        /// </summary>
        /// <param name="filter">状态：5（All）、配送中（10）、待支付（20）待评价(30)</param>
        /// <param name="month">1:一个月内订单,-1:一个月前订单</param>
        /// <param name="pageIndex"></param>
        /// <returns>订单列表</returns>
        /// <remarks>2013-08-01 杨浩 添加</remarks>
        /// <remarks>2013-08-29 沈  强 实现</remarks>
        /// <remarks>2013-10-28 杨浩 重写</remarks>
        public ResultPager<IList<Order>> GetAllOrders(AppEnum.OrderFilter filter, int month, int pageIndex)
        {
            #region 过期方法 (杨浩 标注)

            //var result = new ResultPager<IList<Order>>();
            //bool hasMore = true;
            //IList<Order> data = null;
            //bool status = true;
            //try
            //{
            //    data = DataAccess.Order.ISoOrderDao.Instance.GetAllOrders(filter, month, pageIndex, out hasMore, CurrentUser.SysNo);//CurrentUser.SysNo
            //}
            //catch
            //{
            //    status = false;
            //}
            //result.Data = data;
            //result.HasMore = hasMore;
            //result.Status = status;
            //result.StatusCode = 1;
            //return result;

            #endregion

            //2013-10-28 杨浩 重写

            #region 条件过滤

            var pager = new Pager<SoOrder>
            {
                PageFilter = { CustomerSysNo = CurrentUser.SysNo },
                CurrentPage = pageIndex,
                PageSize = 5
            };
            DateTime? start = (month == 1 ? DateTime.Now.AddMonths(-1) : DateTime.Now.AddYears(-1));
            DateTime? end = (month == 1 ? DateTime.Now.AddYears(1) : DateTime.Now.AddMonths(-1));
            bool unPay = false;

            if (AppEnum.OrderFilter.Obligation == filter)
            {
                unPay = true;
                start = null;
                end = null;
            }
            if (AppEnum.OrderFilter.待评价 == filter)
            {
                pager.PageFilter.Status = (int)OrderStatus.销售单状态.已完成;
                start = null;
                end = null;
            }
            if (AppEnum.OrderFilter.Deliveries == filter)
            {
                pager.PageFilter.Status = (int)OrderStatus.销售单状态.已创建出库单;
            }

            #endregion

            Hyt.BLL.Web.SoOrderBo.Instance.GetMyOrderList(pager, start, end, unPay);
            var list = pager.Rows.Select(order => new  Order
                {
                    SysNo = order.SysNo,
                    OnlineStatus = GetOnlineStatus(order.PayStatus, order.Status, order.OnlineStatus),
                    CreateDate = order.CreateDate,
                    OrderAmount = order.OrderAmount,
                    OperateStatus = GetOperateStatus((OrderStatus.销售单状态)order.Status, (OrderStatus.销售单支付状态)order.PayStatus, order.PayTypeSysNo).ToString(),
                    Products = order.OrderItemList.Select(item => new OrderItem
                        {
                            ProductSysNo = item.ProductSysNo,
                            Quantity = item.Quantity,
                            ProductName = item.ProductName,
                            LevelPrice = item.OriginalPrice,
                            Specification = string.Empty,
                            Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, item.ProductSysNo)
                        }).ToList()
                }).ToList();

            foreach (var l in list)
            {
                foreach (var p in l.Products)
                {
                    l.OrderItemCount += p.Quantity;
                }
            }

            return new ResultPager<IList<Order>>
                {
                    Data = list,
                    Message = "订单获取成功",
                    Status = true,
                    HasMore = pager.TotalPages > pageIndex
                };
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns>返回订单详情</returns>
        /// <remarks>2013-08-10 杨浩 创建</remarks>
        /// <remarks>2013-09-10 沈强 实现</remarks>
        public Result<OrderDetail> GetOrderDetail(string sysNo)
        {
            OrderDetail data = null;
            data = SoOrderBo.Instance.GetOrderDetail(int.Parse(sysNo), CurrentUser.SysNo);
            data.OnlineStatus = GetOnlineStatus(data.PayStatus, data.Status, data.OnlineStatus);
            return new Result<OrderDetail>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>状态</returns>
        /// <remarks>2013-9-12 沈强 实现</remarks>
        /// <remarks>2013-9-16 杨浩 修改</remarks>
        /// <remarks>
        /// 2013-10-29 黄波 修改了取消订单方法
        /// 2014-01-03 朱家宏 修改了作废订单方法，增加作废人类型
        /// </remarks>
        public Result CancelOrder(string sysNo)
        {
            var message = string.Empty;
            using (var tran = new TransactionScope())
            {
                Hyt.BLL.Order.SoOrderBo.Instance.CancelSoOrder(int.Parse(sysNo), CurrentUser.SysNo, OrderStatus.销售单作废人类型.前台用户, ref message);
                tran.Complete();
            }
            return new Result
                {
                    Message = "订单已取消",
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取订单去支付信息
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns>支付信息</returns>
        /// <remarks>2013-9-16 杨浩 创建</remarks>
        public Result<OrderResult> GetOrderPayResult(string sysNo)
        {
            var temp = BLL.Order.SoOrderBo.Instance.GetEntity(int.Parse(sysNo));
            //去支付 始终为 在线支付(淘宝)
            var paymentType = new Model.B2CApp.PaymentType
            {
                SysNo = Model.SystemPredefined.PaymentType.支付宝,
                Type = Model.SystemPredefined.PaymentType.支付宝,
                PaymentName = "支付宝"
            };
            var data = new OrderResult
                {
                    Subject = "商城订单号：" + sysNo,
                    Body = "",
                    CreateDate = temp.CreateDate,
                    OrderSysNo = temp.SysNo.ToString(),
                    PaymentType = paymentType,
                    SettlementAmount = temp.CashPay,
                };
            return new Result<OrderResult>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        #endregion

        #region 退换货

        /// <summary>
        /// 查看退换货历史
        /// </summary>
        /// <returns>返回退换货历史列表</returns>
        /// <remarks>
        /// 2013-09-13 杨浩 创建
        /// 2013-09-13 沈  强 实现
        /// </remarks>
        public Result<IList<ReturnHistory>> GetReturnHistory()
        {
            IList<ReturnHistory> returnHistories = null;
            bool status = true;
            try
            {
                returnHistories = BLL.RMA.RmaBo.Instance.GetRcReturnByCustomerSysNo(CurrentUser.SysNo);
            }
            catch (Exception)
            {
                status = false;
            }
            return new Result<IList<ReturnHistory>>
            {
                Data = returnHistories,
                Status = status
            };
        }

        /// <summary>
        /// 查看退换货进度日志
        /// </summary>
        /// <param name="sysNo">退换货编号</param>
        /// <returns>退换货进度日志</returns>
        /// <remarks>2013-9-13 杨浩 创建</remarks>
        /// <remarks>2013-9-13 周唐炬 实现</remarks>
        public Result<ReturnHistorySub> GetReturnSchedule(string sysNo)
        {
            int rmaSysNo = 0;
            if (!int.TryParse(sysNo, out rmaSysNo))
            {
                return new Result<ReturnHistorySub> { Status = false, Data = null, Message = "参数错误,请输入正确退换货编号" };
            }

            var rma = RmaBo.Instance.GetRMA(rmaSysNo);
            if (rma == null)
            {
                return new Result<ReturnHistorySub> { Status = false, Data = null, Message = "找不到指定的退/换货单" };
            }

            var data = new ReturnHistorySub
                {
                    CreateDate = rma.CreateDate,
                    Items = (from item in rma.RMAItems
                             select new ReturnHistoryItem
                                 {
                                     ProductName = item.ProductName,
                                     ProductSysNo = item.ProductSysNo,
                                     RmaQuantity = item.RmaQuantity,
                                     Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, item.ProductSysNo),
                                 }).ToList(),
                    RefundTotalAmount = (rma.Status == (int)RmaStatus.退换货状态.待审核) ? "审核中" : "¥" + rma.OrginAmount.ToString(),
                    RefundPoint = (rma.Status == (int)RmaStatus.退换货状态.待审核) ? "审核中" : rma.OrginPoint.ToString(),
                    RmaType = rma.RmaType,
                    Status = rma.Status,
                    StatusText = ((RmaStatus.退换货状态)rma.Status).ToString(),
                    SysNo = rma.SysNo
                };

            var list = RmaBo.Instance.GetLogList(rma.TransactionSysNo);
            //data.Logs = list.ConvertAll(x => (RcReturnLog)x);
            data.Logs = list.Select(x => new RcReturnLog
                {
                    LogContent = x.LogContent,
                    OperateDate = x.OperateDate,
                    SysNo = x.SysNo
                }).ToList();

            return new Result<ReturnHistorySub> { Status = true, Data = data, Message = "", StatusCode = 1 };
        }

        /// <summary>
        /// 获取可退换列表
        /// </summary>
        /// <returns>返回退换货明细列表</returns>
        /// <remarks>2013-08-13 杨浩 创建</remarks>
        /// <remarks>2013-09-12 沈  强 实现</remarks>
        public ResultPager<IList<ReturnDetail>> GetReturnsList()
        {
            IList<ReturnDetail> returnDetails = null;
            var status = true;
            try
            {
                returnDetails = BLL.Warehouse.WhWarehouseBo.Instance.GetReturnsList(CurrentUser.SysNo);
            }
            catch
            {
                status = false;
            }
            return new ResultPager<IList<ReturnDetail>>
                {
                    Data = returnDetails,
                    Status = status,
                    HasMore = false
                };
        }

        /// <summary>
        /// 获取可选自提仓库
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <param name="pickupTypeSysNo">取件方式编号</param>
        /// <returns>可选自提门店列表</returns>
        /// <remarks>2013-09-12 周唐炬 实现</remarks>
        public Result<IList<Warehouse>> GetWarehouse(string stockOutSysNo, int pickupTypeSysNo)
        {
            var result = new Result<IList<Warehouse>>() { StatusCode = -1 };
            int stockOutID;
            if (!string.IsNullOrWhiteSpace(stockOutSysNo) && int.TryParse(stockOutSysNo, out stockOutID))
            {
                var stockOut = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.Get(stockOutID);
                if (stockOut != null)
                {
                    var address = SoOrderBo.Instance.GetOrderReceiveAddress(stockOut.ReceiveAddressSysNo);
                    if (address != null)
                    {
                        var warehouseType = 0;
                        if (pickupTypeSysNo == PickupType.送货至门店)
                        {
                            warehouseType = (int)WarehouseStatus.仓库类型.门店;
                        }
                        else
                            warehouseType = (int)WarehouseStatus.仓库类型.仓库;

                        var warehouses = BLL.Warehouse.WhWarehouseBo.Instance.GetWhWarehouseList(address.AreaSysNo, warehouseType, pickupTypeSysNo);
                        warehouses.Add(WhWarehouseBo.Instance.GetWarehouseEntity(stockOut.WarehouseSysNo));
                        if (warehouses.Any())
                        {
                            var list = warehouses.Select(x => new Warehouse
                                {
                                    SysNo = x.SysNo,
                                    WarehouseName = x.WarehouseName,
                                    Contact = x.Contact,
                                    Phone = x.Phone,
                                    StreetAddress = x.StreetAddress
                                }).ToList();
                            result.Data = list;
                            result.Status = true;
                            result.StatusCode = 0;
                        }
                    }
                    else
                    {
                        result.Message = "未指定收货地址,请联系客服!";
                    }
                }
                else
                {
                    result.Message = "不存在相关出库单!";
                }
            }
            return result;
        }

        /// <summary>
        /// 获取可退换商品列表
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <returns>可退换商品列表</returns>
        /// <remarks>2013-09-12 周唐炬 添加注释</remarks>
        public Result<ReturnDetail> GetReturnInfo(string stockOutSysNo)
        {
            ReturnDetail returnDetail = null;
            var status = true;

            try
            {
                returnDetail = BLL.Warehouse.WhWarehouseBo.Instance.GetReturnInfo(int.Parse(stockOutSysNo));
            }
            catch (Exception)
            {
                status = false;
            }

            return new Result<ReturnDetail>
                {
                    Data = returnDetail,
                    Status = status
                };
        }

        /// <summary>
        /// 新建换货单
        /// </summary>
        /// <returns>结果</returns>
        /// <remarks>2013-8-15 杨浩 创建</remarks>
        /// <remarks>2013-09-12 周唐炬 实现</remarks>
        public Result AddExchanges(ExchangeOrders exchange)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (exchange != null)
                {
                    var model = GetExchangesModel(exchange, RmaStatus.RMA类型.售后换货);
                    result = CreateReceiptTransaction(model, exchange);
                }
                else
                {
                    result.Message = "换货单数据不能为空!";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.商城AndroidApp, "新建换货单" + ex.Message, LogStatus.系统日志目标类型.用户,
                                      CurrentUser.SysNo, ex);
            }
            return result;
        }

        /// <summary>
        /// 新建退货单
        /// </summary>
        /// <returns>结果</returns>
        /// <remarks>2013-8-15 杨浩 创建</remarks>
        /// <remarks>2013-09-12 周唐炬 实现</remarks>
        public Result AddRejectedOrders(RejectedOrders rejectedOrders)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (rejectedOrders != null)
                {
                    var model = GetRejectedOrdersModel(rejectedOrders);
                    result = CreateReceiptTransaction(model, rejectedOrders);
                }
                else
                {
                    result.Message = "退货单数据不能为空!";
                }
            }
            catch (Exception ex)
            {
                if (ex is HytException)
                {
                    result.Message = ex.Message;
                }
                SysLog.Instance.Error(LogStatus.系统日志来源.商城AndroidApp, "新建退货单" + ex.Message, LogStatus.系统日志目标类型.用户, CurrentUser.SysNo, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取取件方式
        /// </summary>
        /// <returns>取件方式</returns>
        /// <remarks>2013-9-12 杨浩 创建</remarks>
        public Result<IList<RePickupType>> GetPickupType()
        {
            var data = new List<RePickupType>
                {
                    new RePickupType{ SysNo=PickupType.送货至门店, Name="客户送货至商城门店",Type=PickupType.送货至门店},
                    new RePickupType{ SysNo=PickupType.快递至仓库, Name="客户邮寄回商城仓库",Type=PickupType.快递至仓库},
                    new RePickupType{ SysNo=PickupType.百城当日取件, Name="普通上门取件",Type=PickupType.百城当日取件},
                };
            return new Result<IList<RePickupType>>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        #region 私有方法

        /// <summary>
        /// 封装退换货单实体
        /// </summary>
        /// <param name="exchange">客户提交退换货信息</param>
        /// <param name="rmaType">RMA类型</param>
        /// <returns>换货单实体</returns>
        /// <remarks>2013-09-12 周唐炬 创建</remarks>
        private CBRcReturn GetExchangesModel(ExchangeOrders exchange, RmaStatus.RMA类型 rmaType)
        {
            //换货单实体
            var model = new CBRcReturn()
            {
                RMAItems = new List<RcReturnItem>(),
                CustomerSysNo = exchange.CustomerSysNo,
                OrderSysNo = exchange.OrderSysNo,
                RMARemark = exchange.RmaReason,
                PickupTypeSysNo = exchange.PickupTypeSysNo,
                WarehouseSysNo = exchange.WarehouseSysNo,
                PickUpAddressSysNo = exchange.PickUpAddressSysNo,
                ReceiveAddressSysNo = exchange.ReceiveAddressSysNo,
                Status = (int)WarehouseStatus.退换货单状态.待审核,
                Source = (int)RmaStatus.退换货申请单来源.会员,
                RmaType = rmaType.GetHashCode(),
                HandleDepartment = (int)RmaStatus.退换货处理部门.客服中心
            };
            model.CreateBy = model.LastUpdateBy = User.SystemUser;
            model.CreateDate = model.LastUpdateDate = DateTime.Now;
            //出库单编号
            var stockOutSysNo = 0;
            if (exchange.ProductReturns != null && exchange.ProductReturns.Any())
            {
                #region 退换货商品明细

                foreach (var item in exchange.ProductReturns)
                {
                    var whStockOutItem = WhWarehouseBo.Instance.GetWhStockOutItem(item.StockOutItemSysNo);
                    if (whStockOutItem == null)
                    {
                        throw new HytException("商品出库信息有误,请联系客服!");
                    }
                    stockOutSysNo = whStockOutItem.StockOutSysNo;
                    model.RMAItems.Add(new RcReturnItem
                    {
                        StockOutItemSysNo = item.StockOutItemSysNo,
                        ProductSysNo = item.ProductSysNo,
                        ProductName = whStockOutItem.ProductName,
                        OriginPrice = whStockOutItem.OriginalPrice,
                        RmaQuantity = item.RmaQuantity,
                        RmaReason = exchange.RmaReason
                    });
                    #region 重新计算明细退款金额

                    if (rmaType != RmaStatus.RMA类型.售后退货) continue;
                    var rmaItem = RmaBo.Instance.CalculateRmaAmountByStockOutItem(model.OrderSysNo, model.RMAItems.ToDictionary(x => x.StockOutItemSysNo, x => x.RmaQuantity));
                    decimal fundAmount = 0;
                    foreach (var returnItem in model.RMAItems)
                    {
                        //如果是自定义价格就不重新赋值
                        if (rmaItem != null && rmaItem.StockOutItemAmount != null && returnItem.ReturnPriceType != (int)RmaStatus.商品退款价格类型.自定义价格)
                        {
                            var soi = rmaItem.StockOutItemAmount.FirstOrDefault(x => x.Key == returnItem.StockOutItemSysNo);
                            returnItem.RefundProductAmount = soi.Value;
                        }
                        fundAmount += returnItem.RefundProductAmount;
                    }
                    var rmaItemList = RmaBo.Instance.CalculateRefundRmaAmount(model.OrderSysNo, fundAmount, false);
                    if (rmaItem == null || rmaItemList == null) continue;
                    model.OrginPoint = rmaItem.OrginPoint;
                    model.OrginAmount = rmaItem.OrginAmount;
                    model.OrginCoin = rmaItem.OrginCoin;
                    model.CouponAmount = rmaItem.CouponAmount;
                    model.DeductedInvoiceAmount = model.DeductedInvoiceAmount;
                    model.RefundProductAmount = rmaItemList.RefundProductAmount;
                    model.RedeemAmount = rmaItemList.RedeemAmount;
                    model.RefundCoin = rmaItemList.RefundCoin;
                    model.RefundPoint = rmaItemList.RefundPoint;
                    //实退总金额(实退商品金额-发票扣款金额-现金补偿金额-实退惠源币)
                    model.RefundTotalAmount = fundAmount - rmaItemList.RedeemAmount - model.DeductedInvoiceAmount - rmaItemList.RefundCoin;

                    #endregion
                }

                #endregion
                //默认退换货仓库
                if (model.WarehouseSysNo == 0)
                {
                    model.WarehouseSysNo = WhWarehouseBo.Instance.Get(stockOutSysNo).WarehouseSysNo;
                }
            }
            else
            {
                throw new HytException("换货商品明细数量应该大于0!");
            }
            return model;
        }

        /// <summary>
        /// 生成退货单实体
        /// </summary>
        /// <param name="rejectedOrders">客户提交退换货信息</param>
        /// <returns>退货单实体</returns>
        /// <remarks>2013-09-12 周唐炬 创建</remarks>
        private CBRcReturn GetRejectedOrdersModel(RejectedOrders rejectedOrders)
        {
            var model = GetExchangesModel(rejectedOrders, RmaStatus.RMA类型.售后退货);
            model.IsPickUpInvoice = rejectedOrders.IsPickUpInvoice;
            model.RefundBank = rejectedOrders.RefundBank;
            model.RefundAccountName = rejectedOrders.RefundAccountName;
            model.RefundAccount = rejectedOrders.RefundAccount;
            return model;
        }

        /// <summary>
        /// 创建退换货事务
        /// </summary>
        /// <param name="model">退换货信息</param>
        /// <param name="exchange">客户提交退换货信息</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-09-12 周唐炬 创建</remarks>
        private static Result CreateReceiptTransaction(CBRcReturn model, ExchangeOrders exchange)
        {
            var result = new Result() { StatusCode = -1, Status = false };
            var syUser = SyUserBo.Instance.GetSyUser(User.SystemUser);

            #region 地址
            //取件地址
            SoReceiveAddress pickAddress = null;
            if (exchange.PickupTypeSysNo == PickupType.百城当日取件)
            {
                var address = BLL.Web.CrCustomerBo.Instance.GetCustomerReceiveAddressBySysno(exchange.PickUpAddressSysNo);
                if (address != null)
                {
                    pickAddress = new SoReceiveAddress
                    {
                        AreaSysNo = address.AreaSysNo,
                        EmailAddress = address.EmailAddress,
                        FaxNumber = address.FaxNumber,
                        Gender = address.Gender,
                        MobilePhoneNumber = address.MobilePhoneNumber,
                        Name = address.Name,
                        PhoneNumber = address.PhoneNumber,
                        StreetAddress = address.StreetAddress,
                        ZipCode = address.ZipCode
                    };
                }

            }
            //收货地址
            SoReceiveAddress receiveAddress = null;
            if (exchange.ReceiveAddressSysNo == -10)
            {
                var soOrder = SoOrderBo.Instance.GetEntity(exchange.OrderSysNo);
                if (soOrder != null)
                {
                    exchange.ReceiveAddressSysNo = soOrder.ReceiveAddressSysNo;
                    receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(exchange.ReceiveAddressSysNo);
                }

            }
            if (exchange.ReceiveAddressSysNo > 0)
            {
                var address =
                    BLL.Web.CrCustomerBo.Instance.GetCustomerReceiveAddressBySysno(exchange.ReceiveAddressSysNo);
                if (address != null)
                {
                    receiveAddress = new SoReceiveAddress
                    {
                        AreaSysNo = address.AreaSysNo,
                        EmailAddress = address.EmailAddress,
                        FaxNumber = address.FaxNumber,
                        Gender = address.Gender,
                        MobilePhoneNumber = address.MobilePhoneNumber,
                        Name = address.Name,
                        PhoneNumber = address.PhoneNumber,
                        StreetAddress = address.StreetAddress,
                        ZipCode = address.ZipCode
                    };
                }

            }
            #endregion

            var options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TransactionManager.DefaultTimeout };
            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                var id = RmaBo.Instance.InsertRMA(model, pickAddress, receiveAddress, syUser);
                if (id > 0)
                {
                    result.Status = true;
                    result.StatusCode = 1;
                    result.Message = "退换货申请成功!";
                }
                else
                {
                    result.Message = "系统错误,请稍后重试！";
                }
                scope.Complete();
            }
            return result;
        }

        #endregion

        #endregion

        #region 订单查询

        /// <summary>
        /// 获取订单日志
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单日志</returns>
        /// <remarks>2013-9-12 杨浩 创建</remarks>
        /// <remarks>2013-09-13 周唐炬 实现</remarks>
        /// <remarks>2014-04-11 苟治国 修改增加第三方快递</remarks>
        public Result<TransactionLog> GetTransactionLog(string orderSysNo)
        {
            var result = new Result<TransactionLog>() { StatusCode = -1 };
            int orderId;
            if (!string.IsNullOrWhiteSpace(orderSysNo) && int.TryParse(orderSysNo, out orderId))
            {
                var order = SoOrderBo.Instance.GetEntity(orderId);
                if (order != null)
                {
                    var model = new TransactionLog();
                    //获取配送方式
                    var deliveryType = DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);
                    if (deliveryType != null)
                    {
                        model.DeliverySysNo = deliveryType.SysNo.ToString(CultureInfo.InvariantCulture);
                        model.DeliveryTypeName = deliveryType.DeliveryTypeName;
                    }
                    //获取支付方式
                    var paymentType = PaymentTypeBo.Instance.GetEntity(order.PayTypeSysNo);
                    if (paymentType != null)
                    {
                        model.PayTypeName = paymentType.PaymentName;
                    }
                    var pager = new Pager<SoTransactionLog>();
                    ISoTransactionLogDao.Instance.GetPageDataByOrderID(orderId, ref pager);
                    foreach (var item in pager.Rows)
                    {
                        item.LogContent = Hyt.Util.WebUtil.StripHTML(item.LogContent);
                    }


                    model.Transactions = pager.Rows;
                    //三方快递
                    //var expressList = Hyt.BLL.Logistics.LgExpressBo.Instance.GetLgExpressLogByTransactionSysNo(order.TransactionSysNo);
                    //if (expressList != null)
                    //{
                    //    var list = from item in expressList select new SoTransactionLog { LogContent = item.LogContext, OperateDate = item.LogTime };
                    //    var oldlist = model.Transactions.ToList();
                    //    oldlist.AddRange(list);
                    //    model.Transactions = oldlist;
                    //}   
                    result.Data = model;
                    result.Status = true;
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = "未找到该订单!";
                }
            }
            else
            {
                result.Message = "订单号有误,请检查重试!";
            }

            return result;
        }
        public static List<T> ConvertIListToList<T>(IList<T> gbList) where T : class
        {
            if (gbList != null && gbList.Count >= 1)
            {
                List<T> list = new List<T>();
                for (int i = 0; i < gbList.Count; i++)
                {
                    T temp = gbList[i] as T;
                    if (temp != null)
                        list.Add(temp);
                }
                return list;
            }
            return null;
        }
        #endregion

        #region 优惠券

        /// <summary>
        /// 获取用户优惠券
        /// </summary>
        /// <param name="status">status: 20 已审核(当前可用)，30 已使用</param>
        /// <returns>优惠券</returns>
        /// <remarks> 2013-9-16 杨浩 创建</remarks>
        public Result<IList<Coupon>> GetCoupons(PromotionStatus.优惠券状态 status)
        {
            int count = 0;
            int type = 1;
            if (status == PromotionStatus.优惠券状态.已使用) type = 2;
            var temp = Hyt.DataAccess.Promotion.ISpCouponDao.Instance.Seach(1, 20, CurrentUser.SysNo, status, null, null, type, out count);
            //var temp = Hyt.BLL.Promotion.SpCouponBo.Instance.GetCustomerCoupons(CurrentUser.SysNo, status);
            var data = temp.Select(t => new Coupon
            {
                SysNo = t.SysNo,
                CouponAmount = t.CouponAmount,
                CouponCode = t.CouponCode,
                CustomerSysNo = t.CustomerSysNo,
                Description = t.Description,
                PromotionSysNo = t.PromotionSysNo,
                RequirementAmount = t.RequirementAmount,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                Color = "#3390d0"//bc33d0
            }).ToList();
            return new Result<IList<Coupon>>
            {
                Data = data,
                Status = true,
                StatusCode = 1
            };
        }

        #endregion


     
    }
}
