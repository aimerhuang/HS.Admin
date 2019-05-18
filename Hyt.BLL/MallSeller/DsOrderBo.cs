using System;
using System.Collections.Generic;
using Hyt.BLL.Log;
using Hyt.DataAccess.Distribution;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using System.Linq;
using Hyt.Util;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.MallSeller
{
    public class DsOrderBo : BOBase<DsOrderBo>
    {
        /// <summary>
        /// 锁定对象用户预存款表
        /// </summary>
        //private  static object obj = new object();

        #region 根据开始日期获取指定状态的升舱订单
        /// <summary>
        /// 根据开始日期获取指定状态的升舱订单
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="dearerMallSysNo">商城系统编号</param>
        /// <param name="status">订单状态</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2014-04-08 黄波 创建</reamrks>
        public List<CBDsOrder> GetSuccessedOrder(DateTime startDate, DateTime endDate, int dearerMallSysNo, Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态 status)
        {
            return IDsOrderDao.Instance.GetSuccessedOrder(startDate, endDate, dearerMallSysNo, status);
        }
        #endregion

        #region 升舱订单
        /// <summary>
        /// 升舱订单分页
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-03 朱家宏 创建</remarks>
        public Pager<CBDsOrder> GetPagerList(ParaDsOrderFilter filter)
        {
            return IDsOrderDao.Instance.Query(filter);
        }

        public Pager<CBDsOrder> GetPagerSuccessedList(ParaDsOrderFilter filter)
        {
            filter.HytOrderStatus = (int)Model.WorkflowStatus.OrderStatus.销售单状态.已完成;
            return IDsOrderDao.Instance.Query(filter);
        }

        /// <summary>
        /// 获取升舱订单实体
        /// </summary>
        /// <param name="mallOrderId">淘宝订单编号</param>
        /// <returns>实体</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public DsOrder GetDsOrderByMallOrderId(string mallOrderId)
        {
            if (string.IsNullOrWhiteSpace(mallOrderId))
                throw new ArgumentNullException();

            return IDsOrderDao.Instance.SelectByMallOrderId(mallOrderId);
        }

        /// <summary>
        /// 获取升舱订单明细
        /// </summary>
        /// <param name="dsOrderSysNo">升舱编号</param>
        /// <returns>明细列表</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public IList<DsOrderItem> GetDsOrderItems(int dsOrderSysNo)
        {
            return IDsOrderDao.Instance.SelectItems(dsOrderSysNo);
        }
        #endregion

        #region 登录及账户管理
        /// <summary>
        /// 根据店铺账号获取分销商商城
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        public DsDealerMall GetDsDealerMallByShopAccount(string shopAccount, int mallTypeSysNo)
        {
            return IDsOrderDao.Instance.GetDsDealerMallByShopAccount(shopAccount, mallTypeSysNo);
        }

        /// <summary>
        /// 根据分销商系统编号获取授权账号绑定表
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        ///<returns>授权账号绑定表</returns>
        /// <remarks>2013-09-13 黄志勇 创建</remarks>
        public List<DsDealerMall> GetDsAuthorizations(int dealerSysNo)
        {
            return IDsOrderDao.Instance.GetDsAuthorizations(dealerSysNo);
        }

        /// <summary>
        /// 通过名称得到分销商商城旗舰店
        /// </summary>
        /// <returns>分销商商城旗舰店列表</returns>
        /// <remarks>2014-05-20 余勇 创建</remarks>
        public List<DsDealerMall> GetAllFlagShip()
        {
            return IDsOrderDao.Instance.GetAllFlagShip();
        }

        /// <summary>
        /// 更新分销商商城
        /// </summary>
        /// <param name="model">分销商商城</param>
        ///<returns>受影响行数</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        public int UpdateDsAuthorization(DsDealerMall model)
        {
            return IDsOrderDao.Instance.UpdateDsAuthorization(model);
        }

        /// <summary>
        /// 设置登录信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public MallSellerAuthorization SetLoginInfo(string shopAccount, int mallTypeSysNo)
        {
            return IDsOrderDao.Instance.SetLoginInfo(shopAccount, mallTypeSysNo);
        }

        /// <summary>
        /// 根据系统用户系统编号获取分销商商城
        /// </summary>
        /// <param name="userId">系统用户系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public DsDealerMall GetAuthorizationByUserID(int userId)
        {
            var model = IDsOrderDao.Instance.GetAuthorizationByUserID(userId);
            if (model != null)
            {
                model.LastUpdateBy = userId;
                model.LastUpdateDate = DateTime.Now;
                UpdateDsAuthorization(model);
            }
            return model;
        }

        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-06 黄志勇 创建</remarks>
        public CBAccountInfo GetAccountInfo(string shopAccount, int mallTypeSysNo)
        {
            return IDsOrderDao.Instance.GetAccountInfo(shopAccount, mallTypeSysNo);
        }

        /// <summary>
        /// 分页查询分销商预存款往来账明细
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-06 黄志勇 创建</remarks>
        public Pager<DsPrePaymentItem> QueryPrePaymentItem(ParaDsPrePaymentItemFilter filter)
        {
            return IDsOrderDao.Instance.QueryPrePaymentItem(filter);
        }

        #endregion

        #region 分销商订单升舱
        /// <summary>
        /// 分销商订单升舱
        /// </summary>
        /// <param name="order">升舱订单</param>
        /// <param name="items">升舱订单明细</param>
        /// <param name="hytorderID">商城订单编号</param>
        /// <remarks>2013-09-09 朱成果 创建</remarks>
        public void SaveDsOrder(DsOrder order, List<CBDsOrderItem> items, int hytorderID)
        {
            //在事物里面检查
            if (ExistsDsOrder(order.DealerMallSysNo, order.MallOrderId))
            {
                throw new Exception("存在已经升舱的订单:" + order.MallOrderId);
            }
            order.UpgradeTime = DateTime.Now;
            order.Status =(int) Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.升舱中;
            order.OrderTransactionSysNo = items[0].CurrectDsOrderItemAssociations.OrderTransactionSysNo;
            order.SysNo = Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.InsertOrder(order);//添加主表
            foreach (CBDsOrderItem pp in items)
            {
                DsOrderItem p = pp.BaseInstance;
                p.DsOrderSysNo = order.SysNo;
                p.OrderTransactionSysNo = order.OrderTransactionSysNo;
                pp.CurrectDsOrderItemAssociations.DsOrderItemSysNo = Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.InsertOrderItem(p);//添加子表
                Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.InsertAssociation(pp.CurrectDsOrderItemAssociations);//添加关联表
            }
        }

        /// <summary>
        /// 获取分销商升舱订单
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">升舱完成</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public List<DsOrder> GetGetDsOrderInfo(string shopAccount, int mallTypeSysNo, int top, bool? isFinish)
        {
            return IDsOrderDao.Instance.GetDsOrderInfo(shopAccount, mallTypeSysNo, top, isFinish);
        }

        /// <summary>
        /// 分销商预存款主表
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public DsPrePayment GetPrePayment(string shopAccount, int mallTypeSysNo)
        {
            return IDsOrderDao.Instance.GetPrePayment(shopAccount, mallTypeSysNo);
        }

        ///// <summary>
        ///// 获取指定账户最后登录时间
        ///// </summary>
        ///// <param name="shopAccount">账户</param>
        ///// <param name="mallTypeSysNo">类型</param>
        ///// <returns></returns>
        ///// <remarks>2013-09-10 黄志勇 创建</remarks>
        //public DateTime? GetLastLoginDate(string shopAccount, int mallTypeSysNo)
        //{
        //    return IDsOrderDao.Instance.GetLastLoginDate(shopAccount, mallTypeSysNo);
        //}

        #endregion

        #region 订单升舱冻结预存款

        /// <summary>
        /// 订单升舱冻结预存款
        /// </summary>
        /// <param name="hytorderid">商城订单编号</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="money">冻结金额</param>
        /// <param name="userSysno">操作人员编号</param>
        /// <remarks>2013-09-10 朱成果 创建
        /// 2014-4-30 杨文兵 取消冻结金额的更新
        /// </remarks>
        public bool FreezeDsPrePayment(int hytorderid, int dealerSysNo, decimal money, int userSysno,DateTime?  payDate=null)
        {
            if (money <= 0) return true;
            bool flg = false;
            //lock (obj)
            //{
            var prePayment = Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.GetEntityByDealerSysNo(dealerSysNo);
            if (prePayment != null && prePayment.AvailableAmount >= money)
            {
                prePayment.AvailableAmount -= money;
                var item = new DsPrePaymentItem()
                {
                    CreatedDate =payDate!=null?(DateTime)payDate:DateTime.Now,
                    Decreased = money,
                    Surplus = prePayment.AvailableAmount,
                    Source = (int)Hyt.Model.WorkflowStatus.DistributionStatus.预存款明细来源.订单消费,
                    SourceSysNo = hytorderid,
                    Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.预存款明细状态.完结,
                    PrePaymentSysNo = prePayment.SysNo,
                    LastUpdateDate = DateTime.Now
                };
                Hyt.DataAccess.Distribution.IDsPrePaymentItemDao.Instance.Insert(item);
                //Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.Update(prePayment);
                IDsPrePaymentDao.Instance.SubtractAvailableAmount(dealerSysNo, money, userSysno);
                flg = true;
            }
            //}
            if (!flg)
            {
                throw new Exception("分销商【" + dealerSysNo + "】预付款金额不足");
                //Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(hytorderid, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.未支付);
            }
            return flg;
        }

        /// <summary>
        /// 预存款是否满足支付
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="paymoney">支付金额</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 朱成果 创建</remarks>
        public bool IsDsPrePaymentCanPay(int dealerSysNo, decimal paymoney)
        {
            bool flg = false;
            //lock (obj)
            //{
            var prePayment = Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.GetEntityByDealerSysNo(dealerSysNo);
            if (prePayment != null && prePayment.AvailableAmount >= paymoney)
            {
                flg = true;
            }
            //}
            return flg;
        }
        #endregion

        #region 订单完成，扣除冻结的预存款
        /// <summary>
        /// 订单完成扣除预存款
        /// </summary>
        /// <param name="hytorderid">商城订单编号</param>
        /// <param name="isPrePayment">是否使用预付款支付</param>
        /// <returns></returns>
        /// <remarks>2013-09-11 朱成果 创建</remarks>
        public void CompleteOrder(int hytorderid, bool isPrePayment = true)
        {
            var orderlist = Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.GetEntityByHytOrderID(hytorderid);//更新分销商订单状态
            if (orderlist != null)
            {
                foreach (DsOrder p in orderlist)
                {
                    p.SignTime = DateTime.Now;
                    if (p.DeliveryTime == DateTime.MinValue)
                    {
                        p.DeliveryTime = DateTime.Now;
                    }
                    p.Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.已完成;
                    Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.UpdateOrder(p);//更新分销商订单状态
                }
            }
        }
        #endregion

        #region 订单作废，增加预存款

        /// <summary>
        /// 订单作废，增加预存款可用余额
        /// </summary>
        /// <param name="hytorderid">商城订单编号</param>
        /// <param name="isPrePayment">是否使用预付款支付</param>
        /// <param name="operateSysno">操作者编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-11 朱成果 创建
        /// 
        /// </remarks>
        public void CancelOrder(int hytorderid, int operateSysno, bool isPrePayment = true, int dealerSysNo = 0)
        {
            var orderlist = Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.GetEntityByHytOrderID(hytorderid);//更新分销商订单状态
            if (orderlist != null)
            {
                foreach (DsOrder p in orderlist)
                {
                    p.Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.失败;
                    Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.UpdateOrder(p);//更新分销商订单状态
                }
            }
            //不使用预付款支付
            if (!isPrePayment) return;
            int _dealerSysNo = Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.GetHytOrderDealerSysno(hytorderid);//获取商城订单对应的分销商编号

            if (_dealerSysNo > 0)
                dealerSysNo = _dealerSysNo;
            
            var lst = Hyt.DataAccess.Distribution.IDsPrePaymentItemDao.Instance.GetListBySource(dealerSysNo, (int)Hyt.Model.WorkflowStatus.DistributionStatus.预存款明细来源.订单消费, hytorderid);
            var item = lst.FirstOrDefault(m => m.Status == (int)Hyt.Model.WorkflowStatus.DistributionStatus.预存款明细状态.完结 && m.Decreased > 0);
            if (item == null)
            {
                //没有预存款不执行
                return;
            }
            //lock (obj)
            //{
            var moneyData = Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.GetEntity(item.PrePaymentSysNo);
            moneyData.AvailableAmount += item.Decreased;
            Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.AddAvailableAmount(dealerSysNo, item.Decreased, operateSysno);
            var insertItem = new DsPrePaymentItem()
            {
                CreatedDate = DateTime.Now,
                Source = (int)Hyt.Model.WorkflowStatus.DistributionStatus.预存款明细来源.订单作废,
                SourceSysNo = hytorderid,
                Increased = item.Decreased,
                PrePaymentSysNo = item.PrePaymentSysNo,
                Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.预存款明细状态.完结,
                Surplus = moneyData.AvailableAmount,
                LastUpdateDate=(DateTime)System.Data.SqlTypes.SqlDateTime.MinValue
            };
            Hyt.DataAccess.Distribution.IDsPrePaymentItemDao.Instance.Insert(insertItem);
            //}
        }

        #endregion

        #region 退换货退款

        /// <summary>
        /// 退预存款
        /// </summary>
        /// <param name="hytorderid">商城订单编号</param>
        /// <param name="rmaid">退换货编号</param>
        /// <param name="refundamount">金额</param>
        /// <param name="operateSysno">操作人员编号</param>
        /// <remarks>2013-09-27 朱成果 创建</remarks>
        public void RmaRefund(int hytorderid, int rmaid, decimal refundamount, int operateSysno)
        {
            if (refundamount > 0)
            {
                int dealerSysNo = Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.GetHytOrderDealerSysno(hytorderid);//获取商城订单对应的分销商编号
                if (dealerSysNo <= 0) return;
                var model = Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.GetEntityByDealerSysNo(dealerSysNo);
                if (model == null)
                {
                    throw new ArgumentException("分销商预存款数据不存在：分销商编号：" + dealerSysNo);
                }
                else
                {
                    model.AvailableAmount += refundamount;
                    model.LastUpdateDate = DateTime.Now;
                    //Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.Update(model);
                    Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.AddAvailableAmount(dealerSysNo, refundamount, operateSysno);
                }
                var insertItem = new DsPrePaymentItem()
                {
                    CreatedDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    Source = (int)Hyt.Model.WorkflowStatus.DistributionStatus.预存款明细来源.退款,
                    SourceSysNo = rmaid,
                    Increased = refundamount,
                    PrePaymentSysNo = model.SysNo,
                    Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.预存款明细状态.完结,
                    Surplus = model.AvailableAmount
                };
                Hyt.DataAccess.Distribution.IDsPrePaymentItemDao.Instance.Insert(insertItem);
            }
        }
        #endregion

        #region 分销商价格信息
        /// <summary>
        /// 获取分销商价格信息
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 朱成果 创建</remarks>
        public decimal GetDsSpecialPrice(int dealerSysNo, int productSysNo)
        {
            var info = Hyt.DataAccess.Distribution.IDsSpecialPriceDao.Instance.GetEntity(dealerSysNo, productSysNo);
            if (info != null)
            {
                return info.Price;
            }
            else
            {
                var model = Hyt.DataAccess.Distribution.IDsDealerDao.Instance.GetDsDealer(dealerSysNo);
                if (model == null)
                {
                    throw new Exception("分销商信息不存在");
                }
                else
                {
                    var priceinfo = Hyt.DataAccess.Product.IPdPriceDao.Instance.GetProductPrice(productSysNo, Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.分销商等级价)
                       .FirstOrDefault(m => m.SourceSysNo == model.LevelSysNo && m.Status == (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格状态.有效);
                    if (priceinfo != null)
                    {

                        return priceinfo.Price;
                    }
                    else
                    {
                        throw new Exception("分销商等级价格信息不存在,请联系客户处理.");

                    }
                }
            }
        }
        #endregion

        #region 分销商退换货

        /// <summary>
        /// 获取分销商退换货单
        /// </summary>
        /// <param name="shopAccount">账户</param>
        /// <param name="mallTypeSysNo">类型</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">退款完成</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public List<CBDsReturn> GetReturn(string shopAccount, int mallTypeSysNo, int top, bool? isFinish)
        {
            return IDsReturnDao.Instance.GetReturn(shopAccount, mallTypeSysNo, top, isFinish);
        }
        #endregion

        #region 根据商城订单获取分销商订单信息
        /// <summary>
        /// 根据商城订单事物编号获取分销商订单信息
        /// </summary>
        /// <param name="orderTransactionSysNo">订单事物编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-12 朱成果 创建</remarks>
        public List<DsOrder> GetEntityByTransactionSysNo(string orderTransactionSysNo)
        {
            return Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.GetEntityByTransactionSysNo(orderTransactionSysNo);

        }
        /// <summary>
        /// 根据商城订单编号获取分销商订单信息
        /// </summary>
        /// <param name="hytOrderID">商城订单编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-12 朱成果 创建</remarks>
        public List<DsOrder> GetEntityByHytOrderID(int hytOrderID)
        {
            return Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.GetEntityByHytOrderID(hytOrderID);
        }
        #endregion

        #region 分销产品与商城产品映射
        /// <summary>
        /// 分销产品与商城产品映射
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallProductId">商城商品编码</param>
        /// <param name="mallProductAttr">商城商品属性</param>
        /// <param name="hytProductSysNo">商城商品编码</param>
        /// <returns>void</returns>
        /// <remarks>2013-09-13 朱成果 创建</remarks>
        public void SetProductAssociation(int dealerMallSysNo, string mallProductId, string mallProductAttr, int hytProductSysNo)
        {
            var model = Hyt.DataAccess.Distribution.IDsProductAssociationDao.Instance.GetEntity(dealerMallSysNo, mallProductId);
            if (model != null)
            {
                if (model.HytProductSysNo != hytProductSysNo)
                {
                    model.HytProductSysNo = hytProductSysNo;
                    model.MallProductAttr = mallProductAttr;
                    Hyt.DataAccess.Distribution.IDsProductAssociationDao.Instance.Update(model);
                }
            }
            else
            {
                model = new DsProductAssociation()
                {
                    DealerMallSysNo = dealerMallSysNo,
                    HytProductSysNo = hytProductSysNo,
                    MallProductAttr = mallProductAttr,
                    MallProductId = mallProductId
                };
                Hyt.DataAccess.Distribution.IDsProductAssociationDao.Instance.Insert(model);
            }
        }

        /// <summary>
        /// 获取关联的商城产品详情
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallProductId">商城商品编码</param>
        /// <returns>商品关联</returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public CBDsProductAssociation GetAssociationHytProduct(int dealerMallSysNo, string mallProductId)
        {
            return Hyt.DataAccess.Distribution.IDsProductAssociationDao.Instance.GetHytProduct(dealerMallSysNo, mallProductId);
        }

        /// <summary>
        /// 获取关联的商城产品详情
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallProductId">商城商品编码</param>
        /// <returns>关联的商城产品详情</returns>
        /// <remarks>2013-09-17  朱家宏 创建</remarks>
        public DsProductAssociation GetAssociationByMall(int dealerMallSysNo, string mallProductId)
        {
            return DataAccess.Distribution.IDsProductAssociationDao.Instance.GetEntity(dealerMallSysNo, mallProductId);
        }

        /// <summary>
        /// 根据出库单明细获取关联
        /// </summary>
        /// <param name="outStockItemNo">出库单明细编号</param>
        /// <returns>出库单明细关联</returns>
        /// <remarks>2013-09-17  朱家宏 创建</remarks>
        public List<DsOrderItemAssociation> GetDsOrderItemAssociationByOutStockItemNo(int outStockItemNo)
        {
            return Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.GetDsOrderItemAssociationByOutStockItemNo(outStockItemNo);
        }
        #endregion

        /// <summary>
        /// 通过系统编号获取分销商商城信息
        /// </summary>
        /// <param name="sysNo">分销商商城系统编号</param>
        /// <returns>分销商商城信息</returns>
        /// <remarks>2013-09-17 朱家宏 创建</remarks>
        /// <remarks>2014-05-13余勇 修改 从缓存获取数据</remarks>
        public DsDealerMall GetDsDealerMall(int sysNo)
        {
            return MemoryProvider.Default.Get(string.Format(KeyConstant.DsDealerMall, sysNo), () => IDsOrderDao.Instance.SelectDsDealerMall(sysNo));
            //return IDsOrderDao.Instance.SelectDsDealerMall(sysNo);
        }

        /// <summary>
        /// 是否已经成功升舱
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallOrderId">商城订单号</param>
        /// <returns>bool</returns>
        /// <remarks>2013-9-17 朱成果 创建</remarks>
        public bool ExistsDsOrder(int dealerMallSysNo, string mallOrderId)
        {
            return IDsOrderDao.Instance.ExistsDsOrder(dealerMallSysNo, mallOrderId);
        }

        /// <summary>
        /// 根据订单升舱编号获取
        /// </summary>
        /// <param name="sysNo">订单升舱编号</param>
        /// <returns>升舱订单</returns>
        /// <remarks>2013-09-26 朱成果 创建</remarks>
        public DsOrder SelectBySysNo(int sysNo)
        {
            return IDsOrderDao.Instance.SelectBySysNo(sysNo);
        }

        /// <summary>
        /// 查询可退换货升舱订单
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-26 朱成果 创建</remarks>
        public Pager<CBDsOrder> QueryForRma(ParaDsOrderFilter filter)
        {
            return IDsOrderDao.Instance.QueryForRma(filter);

        }

        /// <summary>
        /// 根据系统编号获取分销商
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-28 黄志勇 创建</remarks>
        public DsDealer GetDealer(int dealerSysNo)
        {
            return IDsOrderDao.Instance.GetDealer(dealerSysNo);
        }

        /// <summary>
        /// 获取商城订单和商城类型
        /// </summary>
        /// <param name="orderTransactionSysNos">OrderTransactionSysNo</param>
        /// <returns>商城订单和商城类型列表</returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public List<CBOrderMallType> GetMallType(string orderTransactionSysNos)
        {
            return IDsOrderDao.Instance.GetMallType(orderTransactionSysNos);
        }

        /// <summary>
        /// 设置升舱订单的商城类型
        /// </summary>
        /// <param name="pager">订单查询结果</param>
        /// <remarks>2013-11-28 黄志勇 创建</remarks>
        public void SetMallType(Pager<CBSoOrder> pager)
        {
            if (pager != null && pager.Rows.Count > 0 &&
                pager.Rows.Any(row => row.OrderSource == (int)OrderStatus.销售单来源.分销商升舱))
            {
                var list = pager.Rows.Where(j => j.OrderSource == (int)OrderStatus.销售单来源.分销商升舱)
                         .Select(i => string.Format("'{0}'", i.TransactionSysNo));
                if (list.Any())
                {
                    var orderTransactionSysNos = FormatUtil.IEnumerableToString(list, ",");
                    var mallTypes = GetMallType(orderTransactionSysNos);
                    if (mallTypes.Count > 0)
                    {
                        pager.Rows.ApplyParallel(item =>
                            {
                                var mallType = mallTypes.FirstOrDefault(i => i.OrderTransactionSysNo == item.TransactionSysNo);
                                if (mallType != null) item.MallTypeSysNo = mallType.MallTypeSysNo;
                            });
                    }
                }
            }
        }

        /// <summary>
        /// 根据商城订单事务编号返回分销商升舱订单扩展信息
        /// </summary>
        /// <param name="orderTransactionSysNo">商城订单事务编号</param>
        /// <returns>分销商升舱订单和分销商商城</returns>
        /// <remarks>2013-12-13 黄志勇 创建</remarks>
        public Tuple<DsOrder, DsDealerMall> GetDsOrderInfoEx(string orderTransactionSysNo)
        {
            var dsOrders = GetEntityByTransactionSysNo(orderTransactionSysNo);
            DsOrder dsOrder = null;
            DsDealerMall dealerMall = null;
            if (dsOrders != null && dsOrders.Count > 0)
            {
                dsOrder = dsOrders.First();
                dealerMall = GetDsDealerMall(dsOrder.DealerMallSysNo);
            }
            return Tuple.Create(dsOrder, dealerMall);
        }

        /// <summary>
        /// 取得订单图片标识
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="sellerMessage">第三方卖家备注</param>
        /// <param name="sellerFlag">卖家标识</param>
        /// <param name="upGradOrderItems">商城订单明细</param>
        /// <returns>订单图片标识</returns>
        /// <remarks>2014-05-20 余勇 创建
        /// 将类型名称用自定义类型代替 2014-07-09 余勇 修改
        /// </remarks>
        public string GetImgFlag(int dealerMallSysNo, string sellerMessage, string sellerFlag, List<Hyt.Model.UpGrade.UpGradeOrderItem> upGradOrderItems)
        {
            string result;
            var dsDealerMall = GetDsDealerMall(dealerMallSysNo);

            if (dsDealerMall != null)
            {
                switch (dsDealerMall.MallTypeSysNo)
                {
                    case (int)DistributionStatus.商城类型预定义.天猫商城:
                        result = GetTmallImgFlag(dealerMallSysNo, sellerMessage, sellerFlag, upGradOrderItems);
                        break;
                    case (int)DistributionStatus.商城类型预定义.淘宝分销:
                        result = MallTypeFlag.淘宝分销; // "taobao";
                        break;
                    case (int)DistributionStatus.商城类型预定义.拍拍网购:
                        result = MallTypeFlag.拍拍网购; // "paipai";
                        break;
                    case (int)DistributionStatus.商城类型预定义.亚马逊:
                        result = MallTypeFlag.亚马逊; // "yamaxun";
                        break;
                    case (int)DistributionStatus.商城类型预定义.百度众测:
                        result = MallTypeFlag.百度众测; // "baidugongce";
                        break;
                    case (int)DistributionStatus.商城类型预定义.一号店:
                        result = MallTypeFlag.一号店; // "yihaodian";
                        break;
                    case (int)DistributionStatus.商城类型预定义.国美在线:
                        result = MallTypeFlag.国美在线; // "guomeizaixian";
                        break;
                    case (int)DistributionStatus.商城类型预定义.百度微购:
                        result = MallTypeFlag.百度微购; // "baiduweigou";
                        break;
                    case (int)DistributionStatus.商城类型预定义.阿里巴巴经销批发:
                        result = MallTypeFlag.阿里巴巴经销批发; // "alibaba";
                        break;
                    case (int)DistributionStatus.商城类型预定义.京东商城:
                        result = MallTypeFlag.京东商城; // "jd";
                        break;
                    case (int)DistributionStatus.商城类型预定义.有赞:
                        result = MallTypeFlag.有赞; // "youzan";
                        break;
                    case (int)DistributionStatus.商城类型预定义.苏宁易购:
                        result = MallTypeFlag.苏宁易购; // "suningyigou";
                        break;

                    case (int)DistributionStatus.商城类型预定义.海拍客:
                        result = MallTypeFlag.海拍客; // "HaiPaiKe";
                        break;
                    case (int)DistributionStatus.商城类型预定义.海带网: //HaiDai
                        result = MallTypeFlag.海带网;
                        break;
                    default:
                        result = string.Empty;
                        break;
                }
            }
            else
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 取得天猫订单图片标识（是否购买升舱）
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="sellerMessage">第三方卖家备注</param>
        /// <param name="sellerFlag">卖家标识</param>
        /// <param name="upGradOrderItems">商城订单明细</param>
        /// <returns>订单图片标识</returns>
        /// <remarks>2014-05-20 余勇 创建
        /// 将类型名称用自定义类型代替 2014-07-09 余勇 修改
        /// </remarks>
        private string GetTmallImgFlag(int dealerMallSysNo, string sellerMessage, string sellerFlag, List<Hyt.Model.UpGrade.UpGradeOrderItem> upGradOrderItems)
        {
            //直营旗舰店 -------- 全部默认订单均为升舱服务订单，不用任何标示
            //直营非旗舰店 -------靠三个依据：旗帜、备注、商品属性（淘宝商品属性）。 
            //                1、订单添加蓝色旗帜为升舱服务订单，其他颜色旗帜为非升舱服务订单
            //                2、备注中有升舱开头字样的订单
            //                3、商品属性中有“升舱服务”的订单
            var flagShip = GetAllFlagShip().Select(p => p.SysNo); //获取所有直营旗舰店

            if (flagShip.Contains(dealerMallSysNo))
            {
                return MallTypeFlag.天猫商城旗舰店; // "tmall" 直营旗舰店
            }
            if (!string.IsNullOrEmpty(sellerFlag) &&
                int.Parse(sellerFlag) == (int)Hyt.Model.WorkflowStatus.DistributionStatus.淘宝订单旗帜.蓝)
            {
                return MallTypeFlag.天猫商城非旗舰店已升舱; // "tmallup" 购买升舱的非直营店
            }
            if (!string.IsNullOrEmpty(sellerMessage) && sellerMessage.Contains("升舱"))
            {
                return MallTypeFlag.天猫商城非旗舰店已升舱;  //"tmallup"购买升舱的非直营店
            }
            if (upGradOrderItems != null && upGradOrderItems.Any())
            {
                var list = upGradOrderItems.Where(x => x != null && x.MallProductAttrs != null && x.MallProductAttrs.Contains("升舱"));

                if (list != null && list.Any())
                {
                    return MallTypeFlag.天猫商城非旗舰店已升舱; //"tmallup"购买升舱的非直营店
                }
            }
            return MallTypeFlag.天猫商城未升舱; // "tmallnoup";
        }


        #region 升舱明细
        /// <summary>
        /// 根据商城订单明细获取升舱明细
        /// </summary>
        /// <param name="soOrderItemSysNo">事物编号</param>
        /// <returns>获取升舱明细</returns>
        /// <remarks>2014-07-04 朱成果 创建</remarks>
        public DsOrderItem GetDsOrderItemsByHytItems(int  soOrderItemSysNo)
        {
            return Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.GetDsOrderItemsByHytItems(soOrderItemSysNo);
        }

        /// <summary>
        /// 删除升舱明细
        /// </summary>
        /// <param name="soOrderItemSysNo">商城升舱订单编号</param>
        /// <remarks>2014-07-04 朱成果 创建</remarks>
        public void DeleteDsOrderItemByHytItems(int  soOrderItemSysNo)
        {
            Hyt.DataAccess.Order.ISoOrderItemDao.Instance.Delete(soOrderItemSysNo);
            var item = GetDsOrderItemsByHytItems(soOrderItemSysNo);
            if(item!=null)
            {
                Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.DeleteAssociationByItmeSysNo(item.SysNo);
                Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.DeleteItemByItemSysNo(item.SysNo);
            }
        }

        /// <summary>
        /// 插入升舱明细
        /// </summary>
        /// <param name="item">商城明细</param>
        /// <remarks>2014-07-04 朱成果 创建</remarks>
        public void InsertDsOrderItemByHytItems(CBSoOrderItem item)
        {
            item.SysNo=Hyt.DataAccess.Order.ISoOrderItemDao.Instance.Insert(item);
            var dsorder = GetEntityByHytOrderID(item.OrderSysNo).FirstOrDefault();
            if(dsorder!=null)
            {
                DsOrderItem ditem = new DsOrderItem()
                {
                     Quantity=item.Quantity,
                     Price=item.SalesUnitPrice,
                     OrderTransactionSysNo=item.TransactionSysNo,
                     MallProductId=item.ErpCode,
                     MallProductName=item.ProductName,
                     DsOrderSysNo=dsorder.SysNo
                };
                ditem.SysNo = Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.InsertOrderItem(ditem);
                DsOrderItemAssociation dia = new DsOrderItemAssociation()
                {
                     SoOrderItemSysNo=item.SysNo,
                     OrderTransactionSysNo=item.TransactionSysNo,
                     DsOrderItemSysNo = ditem.SysNo
                };
                Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.InsertAssociation(dia);
            }
        }
        #endregion


    }
}
