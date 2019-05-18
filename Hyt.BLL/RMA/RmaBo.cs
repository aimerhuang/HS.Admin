using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Hyt.BLL.CRM;
using Hyt.BLL.Finance;
using Hyt.BLL.LevelPoint;
using Hyt.BLL.Logistics;
using Hyt.BLL.Order;
using Hyt.BLL.Promotion;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Order;
using Hyt.DataAccess.RMA;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util.Extension;
using Hyt.Util.Validator;
using PaymentType = Hyt.Model.SystemPredefined.PaymentType;

namespace Hyt.BLL.RMA
{
    /// <summary>
    /// 退换货业务
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public class RmaBo : BOBase<RmaBo>, IRmaBo
    {

        #region 加载退换货相关数据
        /// <summary>
        /// 获取退换货明细
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns>退换货明细列表</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks> 
        public List<CBRmaReturnItem> GetItemListByOrder(int orderNo)
        {
            return IRcReturnItemDao.Instance.GetListByOrder(orderNo);
        }

        /// <summary>
        /// 通过订单编号获得退换货列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>退换货列表</returns>
        /// <remarks>
        /// 2013-12-06 余勇 创建
        /// </remarks>
        public List<CBRcReturn> GetRmaReturnListByOrderSysNo(int orderSysNo)
        {
            return IRcReturnDao.Instance.GetRmaReturnListByOrderSysNo(orderSysNo);
        }

        /// <summary>
        /// 获取已经退换货数量
        /// </summary>
        /// <param name="lst">所有退换货明细数据</param>
        /// <param name="stockOutItemSysNo">出库单明细编号</param>
        /// <param name="rmaID">退换货单号</param>
        /// <returns>已经退换货数量</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks> 
        public int GetAllRmaQuantity(List<CBRmaReturnItem> lst, int stockOutItemSysNo, int rmaID)
        {
            int q = 0;
            if (lst == null || lst.Count < 1) return 0;
            var ls = lst.Where(m => m.stockoutitemsysno == stockOutItemSysNo).ToList();
            if (ls == null || ls.Count < 1) return 0;
            foreach (CBRmaReturnItem item in ls)
            {
                if (item.RMAStatus == (int)RmaStatus.退换货状态.作废)//作废的退货单数量不统计
                {
                    continue;
                }
                else if (item.RMAStatus == (int)RmaStatus.退换货状态.待审核 && item.RMAID == rmaID)//当前待审核的不统计
                {
                    continue;
                }
                else
                {
                    q += item.RmaQuantity;
                }
            }
            return q;
        }

        /// <summary>
        /// 获取已经退换货数量
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="stockOutItemSysNo">出库单明细编号</param>
        /// <param name="rmaID">退换货单号</param>
        /// <returns>已经退换货数量</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks> 
        public int GetAllRmaQuantity(int orderNo, int stockOutItemSysNo, int rmaID)
        {
            int q = 0;
            var lst = GetItemListByOrder(orderNo);
            if (lst == null || lst.Count < 1) return 0;
            var ls = lst.Where(m => m.stockoutitemsysno == stockOutItemSysNo).ToList();
            if (ls == null || ls.Count < 1) return 0;
            foreach (CBRmaReturnItem item in ls)
            {
                if (item.RMAStatus == (int)RmaStatus.退换货状态.作废)//作废的退货单数量不统计
                {
                    continue;
                }
                else if (item.RMAStatus == (int)RmaStatus.退换货状态.待审核 && item.RMAID == rmaID)//当前待审核的退换货单不统计
                {
                    continue;
                }
                else
                {
                    q += item.RmaQuantity;
                }
            }
            return q;
        }

        /// <summary>
        /// 获取退换货单实体(包含退货商品明细）
        /// </summary>
        /// <param name="sysNo">退换货单编号</param>
        /// <returns>退换货单实体(包含退货商品明细）</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public CBRcReturn GetRMA(int sysNo)
        {
            var model = Hyt.DataAccess.RMA.IRcReturnDao.Instance.GetEntity(sysNo);
            if (model != null)
            {
                model.RMAItems = Hyt.DataAccess.RMA.IRcReturnItemDao.Instance.GetListByReturnSysNo(sysNo);
                //2013-11-07 朱家宏 修改：初始OrginAmount
                //model.OrginAmount = model.RefundProductAmount;
            }
            return model;
        }

        /// <summary>
        /// 获取当前订单待处理的退货单（不包括，作废和已完成的退换货单)
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>当前订单待处理的退货单</returns>
        /// <remarks>2013-07-16 朱成果 创建</remarks>
        public RcReturn GetDealWithRMA(int orderID)
        {
            return IRcReturnDao.Instance.GetDealWithRMA(orderID);
        }

        /// <summary>
        /// 获取当前订单待审核的退货单
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>获取当前订单待审核的退货单</returns>
        /// <remarks>2014-04-24 余勇 创建</remarks>
        public RcReturn GetPendWithReturn(int orderID)
        {
            return IRcReturnDao.Instance.GetPendWithReturn(orderID);
        }

        /// <summary>
        /// 获取退换货实体（不包括明细)
        /// </summary>
        /// <param name="sysNo">退换货单编号</param>
        /// <returns> 获取退换货实体（不包括明细)</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public RcReturn GetRcReturnEntity(int sysNo)
        {
            return IRcReturnDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 获取可退换货的出库单列表（包含出库单明细)
        /// </summary>
        /// <param name="orderID">订单明细</param>
        /// <param name="afterDays">多少天内签收的</param>
        /// <returns>可退换货的出库单列表（包含出库单明细)</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public IList<WhStockOut> GetCanReturnStockOutList(int orderID, int afterDays)
        {
            return WhWarehouseBo.Instance.GetWhStockOutListByOrderID(orderID).Where(m => m.SignTime > DateTime.Now.AddDays(-afterDays) && m.Status == (int)WarehouseStatus.出库单状态.已签收).ToList();
        }

        /// <summary>
        /// 获取退换货日志
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <returns>退换货日志列表</returns>
        /// <remarks>2013-07-17 朱成果 创建</remarks>
        public List<CBRcReturnLog> GetLogList(int returnSysNo)
        {
            return IRcReturnLogDao.Instance.GetListByReturnSysNo(returnSysNo);
        }

        /// <summary>
        /// 获取退换货日志
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        /// <returns>退换货日志列表</returns>
        /// <remarks>2013-07-17 朱成果 创建</remarks>
        public List<CBRcReturnLog> GetLogList(string transactionSysNo)
        {
            return IRcReturnLogDao.Instance.GetListByTransactionSysNo(transactionSysNo);
        }
        #endregion

        #region 退换货数据插入，更新
        /// <summary>
        /// 写退换货日志
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <param name="transactionSysNo">事物编号</param>
        /// <param name="logContent">日志内容</param>
        /// <param name="user">操作人</param>
        /// <returns>写退换货日志编号</returns>
        /// <remarks>2013-07-17 朱成果 创建</remarks>
        public int WriteRMALog(int returnSysNo, string transactionSysNo, string logContent, SyUser user)
        {
            var sysNo = Hyt.DataAccess.RMA.IRcReturnLogDao.Instance.Insert
                   (
                    new RcReturnLog
                    {
                        OperateDate = DateTime.Now,
                        Operator = user.SysNo,
                        TransactionSysNo = transactionSysNo,
                        LogContent = logContent,
                        ReturnSysNo = returnSysNo
                    }
                   );
            return sysNo;
        }

        /// <summary>
        /// 添加退换货单(方法内无事物处理，调用时请嵌套事物)
        /// </summary>
        /// <param name="entity">退换货（包括明细)</param>
        /// <param name="pickUpAddress">取件地址</param>
        /// <param name="receiveAddress">换货后的送货地址</param>
        /// <param name="user">操作人</param>
        /// <returns>退换货编号</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public int InsertRMA(CBRcReturn entity, SoReceiveAddress pickUpAddress, SoReceiveAddress receiveAddress, SyUser user)
        {
            //1.添加退换货单
            //2.换货后的送货地址
            //3.添加取件地址
            int sysNO = 0;
            if (null == entity)//不允许明细为空
            {
                throw new HytException("退换货数据不能为空");
            }
            if (null == entity.RMAItems)//不允许明细为空
            {
                throw new HytException("退换货明细不能为空");
            }
            //if (Hyt.DataAccess.RMA.IRcReturnDao.Instance.GetDealWithCount(entity.OrderSysNo) > 0)//只允许一条待处理的退换货数据
            //{
            //    throw new Exception("当前订单存在待处理的退换货单");
            //}
            #region 2013-08-07 允许同时多个退换货 (朱成果)

            var lstRMAItem = GetItemListByOrder(entity.OrderSysNo);//已存在的退换货数据
            if (entity.RMAItems != null && lstRMAItem != null && lstRMAItem.Count > 0)
            {
                foreach (CBRmaReturnItem item in lstRMAItem)
                {
                    var newItem = entity.RMAItems.Where(m => m.StockOutItemSysNo == item.stockoutitemsysno).SingleOrDefault();
                    if (newItem != null)
                    {
                        if (item.ProductQuantity < newItem.RmaQuantity + GetAllRmaQuantity(lstRMAItem, item.stockoutitemsysno, 0))
                        {
                            throw new HytException("退换货数量之和超过了购买数量");
                        }
                    }
                }
            }
            #endregion
            entity.Status = (int)RmaStatus.退换货状态.待审核;//待审核
            entity.TransactionSysNo = Guid.NewGuid().ToString().Replace("-", ""); //生成事务编号
            if (entity.PickUpAddressSysNo < 1 && pickUpAddress != null)//取件地址
            {
                entity.PickUpAddressSysNo = ISoReceiveAddressDao.Instance.InsertEntity(pickUpAddress).SysNo;
            }
            if (entity.ReceiveAddressSysNo < 1 && receiveAddress != null)//换货后的送货地址
            {
                entity.ReceiveAddressSysNo = ISoReceiveAddressDao.Instance.InsertEntity(receiveAddress).SysNo;
            }
            sysNO = IRcReturnDao.Instance.Insert(entity);//插入主表
            if (sysNO > 0)//插入主表成功
            {
                if (entity.RMAItems != null)
                {
                    foreach (RcReturnItem item in entity.RMAItems)
                    {
                        item.TransactionSysNo = entity.TransactionSysNo;
                        item.ReturnSysNo = sysNO;
                        IRcReturnItemDao.Instance.Insert(item);//插入明细表
                    }
                }
                WriteRMALog(sysNO, entity.TransactionSysNo, string.Format(Constant.RMA_CREATE, sysNO), user);
            }

            return sysNO;
        }

        /// <summary>
        /// 更新退换货信息(方法内无事物处理，调用时请嵌套事物)
        /// </summary>
        /// <param name="entity">退换货（包括明细)</param>
        /// <param name="pickUpAddress">取件地址</param>
        /// <param name="receiveAddress">换货后的送货地址</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public void SaveRMA(CBRcReturn entity, SoReceiveAddress pickUpAddress, SoReceiveAddress receiveAddress, SyUser user)
        {
            //更新主表
            //更新明细
            //更新取件地址
            //更新换货后的送货地址
            if (null == entity)//不允许明细为空
            {
                throw new Exception("退换货数据不能为空");
            }
            if (null == entity.RMAItems)//不允许明细为空
            {
                throw new Exception("退换货明细不能为空");
            }
            #region 2013-08-07 允许同时多个退换货 (朱成果)

            var lstRMAItem = GetItemListByOrder(entity.OrderSysNo);//已存在的退换货数据
            if (entity.RMAItems != null && lstRMAItem != null && lstRMAItem.Count > 0)
            {
                foreach (CBRmaReturnItem item in lstRMAItem)
                {
                    var newItem = entity.RMAItems.Where(m => m.StockOutItemSysNo == item.stockoutitemsysno).SingleOrDefault();
                    if (newItem != null)
                    {
                        if (item.ProductQuantity < newItem.RmaQuantity + GetAllRmaQuantity(lstRMAItem, item.stockoutitemsysno, entity.SysNo))
                        {
                            throw new Exception("退换货数量之和超过了购买数量");
                        }
                    }
                }
            }
            #endregion

            if (pickUpAddress != null)//取件地址
            {
                if (entity.PickUpAddressSysNo < 1) //取件地址
                {
                    entity.PickUpAddressSysNo =
                        ISoReceiveAddressDao.Instance.InsertEntity(pickUpAddress).SysNo;
                }
                else
                {
                    SoOrderBo.Instance.SaveSoReceiveAddress(pickUpAddress);
                }
            }
            else
            {
                entity.PickUpAddressSysNo = 0;
            }
            if (receiveAddress != null)//换货后的送货地址
            {
                if (entity.ReceiveAddressSysNo < 1) //换货后的送货地址
                {
                    entity.ReceiveAddressSysNo =
                        ISoReceiveAddressDao.Instance.InsertEntity(receiveAddress).SysNo;
                }
                else
                {
                    SoOrderBo.Instance.SaveSoReceiveAddress(receiveAddress);
                }
            }
            else
            {
                entity.ReceiveAddressSysNo = 0;
            }

            entity.LastUpdateBy = user.SysNo;
            entity.LastUpdateDate = DateTime.Now;
            IRcReturnDao.Instance.Update(entity);
            IRcReturnItemDao.Instance.DeleteByReturnSysNo(entity.SysNo);
            if (entity.RMAItems != null)
            {
                foreach (RcReturnItem item in entity.RMAItems)
                {
                    IRcReturnItemDao.Instance.Insert(item);//插入明细表
                }
            }
        }
        #endregion

        #region 退换货作废，审核

        /// <summary>
        /// 审核退换货单(方法内无事物处理，调用时请嵌套事物)
        /// </summary>
        /// <param name="sysNo">退换货单编号</param>
        /// <param name="user">用户</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        /// <remarks>2013-09-25 朱成果 创建</remarks>
        public WhStockIn AuditRMA(int sysNo, SyUser user)
        {
            //1.审核退换货
            //2.创建入库单
            //3.创建取件单
            var RMA = GetRMA(sysNo);
            if (RMA == null)
            {
                throw new Exception("退换货单不存在");
            }
            if (RMA.Status != (int)RmaStatus.退换货状态.待审核)
            {
                throw new Exception("退换货单目前不是待审核状态!");
            }
            var order = SoOrderBo.Instance.GetEntity(RMA.OrderSysNo);//订单
            bool autoInstock = RMA.HandleDepartment == (int)RmaStatus.退换货处理部门.门店;//门店退货自动入库
            RMA.Status = (int)Hyt.Model.WorkflowStatus.RmaStatus.退换货状态.待入库; //待入库
            RMA.AuditorBy = user.SysNo;
            RMA.AuditorDate = DateTime.Now;
            Hyt.DataAccess.RMA.IRcReturnDao.Instance.Update(RMA);
            MallSeller.DsReturnBo.Instance.HytRMAAuditCallBack(sysNo, RMA.OrderSysNo, RMA.RefundTotalAmount);//更新分销相关退货信息 2013/09/25 朱成果
            //创建入库单
            WhStockIn inEntity = new WhStockIn
            {
                CreatedBy = user.SysNo,
                CreatedDate = DateTime.Now,
                DeliveryType = GetStockInDeliveryType(RMA.PickupTypeSysNo),
                IsPrinted = (int)WarehouseStatus.是否已经打印拣货单.否,
                SourceSysNO = sysNo,
                SourceType = (int)WarehouseStatus.入库单据类型.RMA单,
                Status = autoInstock ? (int)WarehouseStatus.入库单状态.已入库 : (int)WarehouseStatus.入库单状态.待入库,
                TransactionSysNo = RMA.TransactionSysNo,
                WarehouseSysNo = RMA.WarehouseSysNo,
                Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                LastUpdateBy = user.SysNo,
                LastUpdateDate = DateTime.Now
            };
            if (autoInstock)
            {
                inEntity.Remarks = "门店退货,系统自动入库.";
            }
            inEntity.ItemList = new List<WhStockInItem>();
            //入库明细
            foreach (RcReturnItem item in RMA.RMAItems)
            {
                //WhStockInItem p = null;   DEFAULT
                //if (inEntity.ItemList != null && inEntity.ItemList.Count > 0)
                //{
                //    p = inEntity.ItemList.Where(m => m.ProductSysNo == item.ProductSysNo).FirstOrDefault();
                //}
                //if (p == null)
                //{
                    inEntity.ItemList.Add(new WhStockInItem
                    {
                        CreatedBy = user.SysNo,
                        CreatedDate = DateTime.Now,
                        ProductName = item.ProductName,
                        ProductSysNo = item.ProductSysNo,
                        StockInQuantity = item.RmaQuantity,
                        RealStockInQuantity = autoInstock ? item.RmaQuantity : 0,
                        LastUpdateBy = user.SysNo,
                        LastUpdateDate = DateTime.Now,
                        
                        SourceItemSysNo = item.SysNo //记录入库单明细来源单号（退换货明细编号)
                    });
                //}
                //else
                //{
                //    p.StockInQuantity += item.RmaQuantity; //相同产品累加入库数量
                //    if (autoInstock)
                //    {
                //        p.RealStockInQuantity += item.RmaQuantity;
                //    }
                //}
            }
            var inSysNo = InStockBo.Instance.CreateStockIn(inEntity); //保存入库单数据
            if (
                RMA.PickupTypeSysNo == (int)PickupType.百城当日取件 ||
                RMA.PickupTypeSysNo == (int)PickupType.定时取件 ||
                RMA.PickupTypeSysNo == (int)PickupType.加急取件 ||
                RMA.PickupTypeSysNo == (int)PickupType.普通取件) //上门取件
            {
                #region 创建取件单
                LgPickUp main = new LgPickUp
                {
                    CreatedBy = user.SysNo,
                    PickupAddressSysNo = RMA.PickUpAddressSysNo,
                    Remarks = "预约时间:" + RMA.PickUpTime,
                    StockInSysNo = inSysNo,
                    WarehouseSysNo = RMA.WarehouseSysNo,
                    CreatedDate = DateTime.Now,
                    PickupTypeSysNo = RMA.PickupTypeSysNo,
                    LastUpdateDate = DateTime.Now,
                    LastUpdateBy = user.SysNo,
                    Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    Status = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.取件单状态.待取件
                };
                main.SysNo = Hyt.DataAccess.RMA.ILgPickUpDao.Instance.Insert(main);
                if (main.SysNo > 0)
                {
                    foreach (RcReturnItem item in RMA.RMAItems)
                    {
                        var priceInfo = Hyt.BLL.Product.PdPriceBo.Instance.GetProductPrice(item.ProductSysNo).FirstOrDefault(m => m.PriceSource == (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.配送员进货价);
                        LgPickUpItem pitem = new LgPickUpItem
                        {
                            CreatedBy = user.SysNo,
                            CreatedDate = DateTime.Now,
                            PickUpSysNo = main.SysNo,
                            ProductName = item.ProductName,
                            ProductSysNo = item.ProductSysNo,
                            ProductQuantity = item.RmaQuantity,
                            LastUpdateDate = DateTime.Now,
                            LastUpdateBy = user.SysNo,
                            OriginalPrice = priceInfo == null ? 0 : priceInfo.Price
                        };
                        main.TotalAmount += pitem.OriginalPrice * pitem.ProductQuantity;//累计金额
                        Hyt.DataAccess.RMA.ILgPickUpItemDao.Instance.Insert(pitem);
                    }
                    if (main.TotalAmount > 0)//更新总价
                    {
                        Hyt.DataAccess.RMA.ILgPickUpDao.Instance.Update(main);
                    }
                }
                #endregion
            }
            //创建日志
            WriteRMALog(sysNo, RMA.TransactionSysNo, string.Format(Constant.RMA_Checked, sysNo), user);
            WriteRMALog(sysNo, RMA.TransactionSysNo, string.Format(Constant.RMA_InStock, inSysNo), user);

            //自动入库
            if (autoInstock)
            {
                //门店退款自动退款
                bool autocomplete = (RMA.RefundType == RmaStatus.退换货退款方式.门店退款.GetHashCode());
                RMAInStockCallBack(inEntity, RMA, user, autocomplete, false);
              
            }
            var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
            if (receiveAddress != null && !string.IsNullOrEmpty(receiveAddress.EmailAddress))
            {
                if (VHelper.Do(receiveAddress.EmailAddress, VType.Email))
                {
                    var warehouse = WhWarehouseBo.Instance.GetWarehouse(RMA.WarehouseSysNo);
                    BLL.Extras.EmailBo.Instance.发送退换货单通过审核邮件(receiveAddress.EmailAddress, order.CustomerSysNo.ToString(),
                                                            RMA.SysNo.ToString(), RMA.CreateDate,
                                                            warehouse.StreetAddress, warehouse.Contact, warehouse.Phone);
                }
            }

            if (autoInstock)
                return inEntity;
            return null;
        }

        /// <summary>
        /// 获取对应的入库物流方式
        /// </summary>
        /// <param name="PickupType">取件方式</param>
        /// <returns>入库物流方式</returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        private int GetStockInDeliveryType(int PickupType)
        {
            if (PickupType == (int)Hyt.Model.SystemPredefined.PickupType.快递至仓库)
            {
                return (int)WarehouseStatus.入库物流方式.快递入库;
            }
            else if (PickupType == (int)Hyt.Model.SystemPredefined.PickupType.送货至门店)
            {
                return (int)WarehouseStatus.入库物流方式.送至仓库;
            }
            else
            {
                return (int)WarehouseStatus.入库物流方式.上门取货;
            }
        }

        /// <summary>
        /// 作废退换货单(方法内无事物处理，调用时请嵌套事物)
        /// </summary>
        /// <param name="sysNo">退换货单编号</param>
        /// <param name="user">用户</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        /// <remarks>2013-09-25 黄志勇 创建</remarks>
        public void CancelRMA(int sysNo, SyUser user)
        {
            //更新作废状态
            //保存日志
            var RMA = IRcReturnDao.Instance.GetEntity(sysNo);
            if (RMA == null)
            {
                throw new Exception("退换货单不存在");
            }
            if (RMA.Status != (int)RmaStatus.退换货状态.待审核)
            {
                throw new Exception("当前状态退换货单不能作废!");
            }
            RMA.CancelBy = user.SysNo;
            RMA.CancelDate = DateTime.Now;
            RMA.Status = (int)RmaStatus.退换货状态.作废;
            Hyt.DataAccess.RMA.IRcReturnDao.Instance.Update(RMA);
            WriteRMALog(sysNo, RMA.TransactionSysNo, string.Format(Constant.RMA_Cancel, sysNo), user);
        }

        /// <summary>
        /// 退货转门店
        /// </summary>
        /// <param name="sysNo">退换货编号</param>
        /// <returns>影响行数</returns> 
        /// <remarks>2013-09-17 余勇 创建</remarks>
        public int ReturnToShop(int sysNo)
        {
            return IRcReturnDao.Instance.UpdateRcReturnToShop(sysNo, (int)RmaStatus.退换货处理部门.门店);
        }

        #endregion

        #region 入库单入库后，更新退换货单信息，(创建付款单或者创建RMA销售单)
        /// <summary>
        /// 入库单入库后，更新退货单,(创建退款单或者创建RMA销售单)
        /// </summary>
        /// <param name="inStockSysNo">入库单编号</param>
        /// <param name="isPickUpInvoice">发票是否取回</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public void RMAInStockCallBack(int inStockSysNo, SyUser user, bool isPickUpInvoice = false)
        {
            //1.更新退换货状态
            //2.如果退货，创建退款单
            //2.如果换货，创建RMA销售单
            var entity = InStockBo.Instance.GetStockIn(inStockSysNo);
            if (entity == null)
            {
                throw new HytException("入库单信息不存在");
            }
            var RMA = GetRMA(entity.SourceSysNO);
            RMAInStockCallBack(entity, RMA, user, false, isPickUpInvoice);
        }

        /// <summary>
        /// 入库完成后激发
        /// </summary>
        /// <param name="entity">入库单</param>
        /// <param name="rma">退换货单</param>
        /// <param name="user">操作人</param>
        /// <param name="autocomplete">自动完成付款</param>
        /// <param name="isPickUpInvoice">发票是否取回</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 朱成果 创建</remarks>
        private void RMAInStockCallBack(WhStockIn entity, CBRcReturn rma, SyUser user, bool autocomplete = false, bool isPickUpInvoice = false)
        {
            if (entity == null)
            {
                throw new HytException("入库单信息不存在");
            }
            if (entity.Status != (int)Hyt.Model.WorkflowStatus.WarehouseStatus.入库单状态.已入库)
            {
                return;
            }
            if (entity.SourceType != (int)WarehouseStatus.入库单据类型.RMA单)
            {
                return;
            }
            if (rma == null)
            {
                throw new HytException("退换货单不存在");
            }
            if (rma.RMAItems == null || rma.RMAItems.Count < 1)
            {
                throw new HytException("退换货单明细不存在");
            }
            if (rma.RmaType == (int)Hyt.Model.WorkflowStatus.RmaStatus.RMA类型.售后退货)
            {
                //待入库变成待退款
                if (rma.Status == (int)Hyt.Model.WorkflowStatus.RmaStatus.退换货状态.待入库)
                {
                    //发票状态
                    var invoiceStatus = new SoOrder() { SysNo = rma.OrderSysNo }.InvoiceStatus();
                    if (isPickUpInvoice)//已取回非过期发票
                    {
                        if (invoiceStatus != FinanceStatus.订单发票状态.已过期)
                        {
                            rma.RefundTotalAmount += rma.DeductedInvoiceAmount;
                            rma.DeductedInvoiceAmount = 0;
                        }
                        rma.IsPickUpInvoice = (int)Hyt.Model.WorkflowStatus.RmaStatus.是否取回发票.是;
                    }
                    rma.Status = (int)Hyt.Model.WorkflowStatus.RmaStatus.退换货状态.待退款;
                    Hyt.DataAccess.RMA.IRcReturnDao.Instance.Update(rma);//保存状态
                }
                #region 创建付款单
                FnPaymentVoucher pv = Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.GetEntity((int)FinanceStatus.付款来源类型.退换货单, rma.SysNo);//判断付款单是否已经存在
                if (pv == null || pv.SysNo == 0)
                {
                    pv = new FnPaymentVoucher()
                    {
                        PayableAmount = rma.RefundTotalAmount,
                        PaidAmount = autocomplete ? rma.RefundTotalAmount : 0,
                        CreatedBy = user.SysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = user.SysNo,
                        LastUpdateDate = DateTime.Now,
                        CustomerSysNo = rma.CustomerSysNo,
                        Source = (int)FinanceStatus.付款来源类型.退换货单,
                        SourceSysNo = rma.SysNo,
                        RefundBank = rma.RefundBank,
                        RefundAccountName = rma.RefundAccountName,
                        RefundAccount = rma.RefundAccount,
                        TransactionSysNo = rma.TransactionSysNo,
                        PayDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        Status = autocomplete ? (int)FinanceStatus.付款单状态.已付款 : (int)FinanceStatus.付款单状态.待付款
                    };
                    pv.SysNo = Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.Insert(pv);
                    WriteRMALog(rma.SysNo, rma.TransactionSysNo, string.Format(Constant.RMA_NEWPAY, pv.SysNo), user);
                }
                #endregion

                #region 自动完成付款
                if (autocomplete)
                {
                    FnPaymentVoucherItem p = new FnPaymentVoucherItem();
                    p.Amount = pv.PayableAmount;
                    p.CreatedBy = user.SysNo;
                    p.CreatedDate = DateTime.Now;
                    p.LastUpdateBy = user.SysNo;
                    p.LastUpdateDate = DateTime.Now;
                    p.PayDate = DateTime.Now;
                    p.PaymentType = (int)FinanceStatus.付款单付款方式.现金;
                    p.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单明细状态.已付款;
                    p.PaymentVoucherSysNo = pv.SysNo;
                    p.PaymentToType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款方类型.仓库; //付款方
                    p.PaymentToSysNo = entity.WarehouseSysNo;                                      //付款仓库
                    p.SysNo = Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.InsertItem(p);
                    PaymentCompleteCallBack(pv, rma, user);
                    #region 操作EAS
                    CBFnPaymentVoucher newpay = new CBFnPaymentVoucher()
                    {
                        PayableAmount = pv.PayableAmount,
                        PaidAmount = pv.PaidAmount,
                        CreatedBy = pv.CreatedBy,
                        CreatedDate = pv.CreatedDate,
                        LastUpdateBy = pv.LastUpdateBy,
                        LastUpdateDate = pv.LastUpdateDate,
                        CustomerSysNo = pv.CustomerSysNo,
                        Source = pv.Source,
                        SourceSysNo = pv.SourceSysNo,
                        RefundBank = pv.RefundBank,
                        RefundAccountName = pv.RefundAccountName,
                        RefundAccount = pv.RefundAccount,
                        TransactionSysNo = pv.TransactionSysNo,
                        Status = pv.Status,
                        SysNo = pv.SysNo,
                        VoucherItems = new List<FnPaymentVoucherItem>()
                    };
                    newpay.VoucherItems.Add(p);
                    Hyt.BLL.Finance.FinanceBo.Instance.WriteEasPaymentVoucher(newpay, user);
                    #endregion
                }
                #endregion
            }
            else if (rma.RmaType == (int)Hyt.Model.WorkflowStatus.RmaStatus.RMA类型.售后换货)
            {
                var customer = CrCustomerBo.Instance.GetModel(rma.CustomerSysNo);

                rma.Status = (int)Hyt.Model.WorkflowStatus.RmaStatus.退换货状态.已完成;
                Hyt.DataAccess.RMA.IRcReturnDao.Instance.Update(rma);//保存状态

                #region 更新出库单明细的已退数量
                foreach (RcReturnItem rItem in rma.RMAItems)
                {
                    var stockOutItem = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.GetStockOutItem(rItem.StockOutItemSysNo);
                    stockOutItem.ReturnQuantity += rItem.RmaQuantity;//退换货数量
                    if (stockOutItem.ReturnQuantity > stockOutItem.ProductQuantity)
                    {
                        throw new HytException("退换货数量超过出库数量");
                    }
                    Hyt.DataAccess.Warehouse.IOutStockDao.Instance.UpdateOutItem(stockOutItem);
                }
                #endregion

                #region 创建RMA销售单

                var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
                var sum = rma.RMAItems.Sum(m => m.RefundProductAmount);
                SoOrder newOrder = new SoOrder()
                {
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    CustomerSysNo = rma.CustomerSysNo,
                    LevelSysNo = customer.LevelSysNo,
                    DefaultWarehouseSysNo = rma.WarehouseSysNo,
                    DeliveryTypeSysNo = rma.ShipTypeSysNo,
                    OrderCreatorSysNo = user.SysNo,
                    PayStatus = (int)OrderStatus.销售单支付状态.已支付,
                    PayTypeSysNo = PaymentType.售后换货,
                    ReceiveAddressSysNo = rma.ReceiveAddressSysNo,
                    OrderSource = (int)Model.WorkflowStatus.OrderStatus.销售单来源.RMA下单,
                    OrderSourceSysNo = rma.SysNo,
                    Status = (int)OrderStatus.销售单状态.待审核,
                    InternalRemarks = rma.InternalRemark,
                    CustomerMessage = rma.RMARemark,
                    CashPay = 0,
                    OrderAmount = sum,
                    OnlineStatus = Constant.OlineStatusType.待审核,
                    SalesType = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.售后订单,
                    AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    DealerSysNo = orderInfo.DealerSysNo,

                };
                #region 换货配送方式
                if (newOrder.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.百城当日达)
                {
                    newOrder.DeliveryTypeSysNo = Hyt.Model.SystemPredefined.DeliveryType.普通百城当日达;
                }
                else if (newOrder.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.自提)
                {
                    newOrder.DeliveryTypeSysNo = Hyt.Model.SystemPredefined.DeliveryType.门店自提;
                }
                var lstDelivery = WhWarehouseBo.Instance.GetLgDeliveryType(new ParaWhDeliveryTypeFilter() { WareHouseSysNo = newOrder.DefaultWarehouseSysNo });//仓库支持的配送方式
                if (lstDelivery!=null&&!lstDelivery.Any(m=>m.DeliveryTypeSysNo==newOrder.DeliveryTypeSysNo))//仓库不支持此配送方式
                {
                      if(lstDelivery.Any(m=>m.DeliveryTypeSysNo==Hyt.Model.SystemPredefined.DeliveryType.门店自提))
                      {
                          newOrder.DeliveryTypeSysNo = Hyt.Model.SystemPredefined.DeliveryType.门店自提;
                      }
                      else
                      {
                          bool Isdrd=false;//是否满足
                          var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(newOrder.ReceiveAddressSysNo);//收货地址
                          if (receiveAddress != null)
                          {
                              var whinfo = WhWarehouseBo.Instance.GetWarehouse(newOrder.DefaultWarehouseSysNo);//仓库信息
                              if (whinfo != null)
                              {
                                  var zb = Hyt.BLL.Map.BaiduMapAPI.Instance.Geocoder(receiveAddress.StreetAddress, whinfo.CityName);//仓库所在城市->收货地址
                                  if (zb != null)
                                  {
                                      Isdrd = LgDeliveryScopeBo.Instance.IsInScope(whinfo.CitySysNo, new Coordinate() { X = zb.Lng, Y = zb.Lat });//是否支持
                                  }
                              }      
                          }
                          newOrder.DeliveryTypeSysNo = Isdrd ? Hyt.Model.SystemPredefined.DeliveryType.普通百城当日达 : Hyt.Model.SystemPredefined.DeliveryType.第三方快递;
                      }
                }
                #endregion
                ISoOrderDao.Instance.InsertEntity(newOrder);//主表
                newOrder = SoOrderBo.Instance.GetEntity(newOrder.SysNo);
                foreach (RcReturnItem rItem in rma.RMAItems)
                {
                    var soItem = new SoOrderItem()
                      {
                          TransactionSysNo = newOrder.TransactionSysNo,
                          OrderSysNo = newOrder.SysNo,
                          ProductName = rItem.ProductName,
                          ProductSysNo = rItem.ProductSysNo,
                          OriginalPrice = rItem.OriginPrice,
                          SalesUnitPrice = 0,
                          Quantity = rItem.RmaQuantity,
                          SalesAmount = rItem.RefundProductAmount,
                      };
                    soItem.SysNo = ISoOrderItemDao.Instance.Insert(soItem);
                    var whitem = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhStockOutItem(rItem.StockOutItemSysNo);//原出库单明细
                    if (whitem != null)
                    {
                        var soitem=new SoReturnOrderItem()
                         {
                             TransactionSysNo = newOrder.TransactionSysNo,
                             OrderItemSysNo = soItem.SysNo,
                             OrderItemQuantity = soItem.Quantity,
                             FromStockOutItemAmount = whitem.RealSalesAmount,
                             FromStockOutItemQuantity = whitem.ProductQuantity,
                             FromStockOutItemSysNo = whitem.SysNo
                         };

                        SoReturnOrderItem olditem = Hyt.BLL.RMA.RmaBo.Instance.GetSoReturnOrderItem(whitem.OrderItemSysNo);
                        if (olditem != null)//换货后再换货
                        {
                            soitem.FromStockOutItemAmount = olditem.FromStockOutItemAmount;
                            soitem.FromStockOutItemQuantity = olditem.FromStockOutItemQuantity;
                            soitem.FromStockOutItemSysNo = olditem.FromStockOutItemSysNo;
                        }
                        InsertSoReturnOrderItem(soitem);
                    }
                }
                //添加收款信息
                FnReceiptVoucher rv = new FnReceiptVoucher()
                {
                    TransactionSysNo = newOrder.TransactionSysNo,
                    Status = (int)FinanceStatus.收款单状态.已确认,
                    ConfirmedBy = newOrder.OrderCreatorSysNo,
                    ConfirmedDate = DateTime.Now,
                    CreatedBy = newOrder.OrderCreatorSysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = newOrder.OrderCreatorSysNo,
                    LastUpdateDate = DateTime.Now,
                    IncomeAmount = newOrder.CashPay,
                    ReceivedAmount = newOrder.CashPay,
                    IncomeType = (int)Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.预付,
                    Source = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.销售单,
                    SourceSysNo = newOrder.SysNo
                };
                Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Insert(rv);//保存收款信息
                SoOrderBo.Instance.WriteSoTransactionLog(newOrder.TransactionSysNo
                                       , string.Format(Constant.ORDER_TRANSACTIONLOG_CREATE, newOrder.SysNo)
                                       , user.UserName);

                //写订单池记录
                SyJobPoolPublishBo.Instance.OrderAuditBySysNo(newOrder.SysNo);
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("换货单已生成RMA销售单，销售单号:{0}",
                          newOrder.SysNo), newOrder.SysNo, null, user.SysNo);
                #endregion

                WriteRMALog(rma.SysNo, rma.TransactionSysNo, string.Format(Constant.RMA_NEWORDER, newOrder.SysNo), user);
            }
        }
        #endregion

        #region 入库单作废后，更新退换货信息
        /// <summary>
        /// 入库单作废后，更新退换货信息
        /// </summary>
        /// <param name="inStockSysNo">入库单编号</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public void RMAInStockCancelCallBack(int inStockSysNo, SyUser user)
        {
            //入库单作废销售单更新为待审核状态
            var inModel = InStockBo.Instance.GetStockIn(inStockSysNo);//入库单
            RMAInStockCancelCallBack(inModel, user);
        }

        /// <summary>
        /// 入库单作废后，更新退换货信息
        /// </summary>
        /// <param name="inModel">入库单</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public void RMAInStockCancelCallBack(WhStockIn inModel, SyUser user)
        {
            if (inModel == null)
            {
                throw new Exception("入库单信息不存在");
            }
            if (inModel.SourceType != (int)WarehouseStatus.入库单据类型.RMA单)
            {
                return;
            }
            var RMA = IRcReturnDao.Instance.GetEntity(inModel.SourceSysNO);//退换货单
            if (RMA.Status != (int)RmaStatus.退换货状态.待入库)
            {
                throw new Exception("退换货已经完成了入库，不能作废");
            }
            //如果取件单没有取件则作废
            var pick = Hyt.DataAccess.RMA.ILgPickUpDao.Instance.GetEntityByStockIn(inModel.SysNo);
            if (pick != null && pick.Status == (int)LogisticsStatus.取件单状态.待取件)
            {
                pick.Status = (int)LogisticsStatus.取件单状态.作废;
                Hyt.DataAccess.RMA.ILgPickUpDao.Instance.Update(pick);
            }
            if (RMA.Status == (int)RmaStatus.退换货状态.待入库)
            {
                RMA.Status = (int)RmaStatus.退换货状态.待审核;
                Hyt.DataAccess.RMA.IRcReturnDao.Instance.Update(RMA);
                WriteRMALog(inModel.SourceSysNO, RMA.TransactionSysNo, string.Format(Constant.RMA_INSTOCKCANCEL, inModel.SysNo), user);
            }
        }
        #endregion

        #region 付款单付款后,更新退换货信息
        /// <summary>
        /// 付款单付款后,更新退换货信息
        /// </summary>
        /// <param name="sysNo">付款单号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public void PaymentCompleteCallBack(int sysNo, SyUser user)
        {
            var pay = FinanceBo.Instance.GetPaymentVoucher(sysNo);
            if (pay != null && pay.Source == (int)FinanceStatus.付款来源类型.退换货单)
            {
                var RMA = GetRMA(pay.SourceSysNo);//退换货单
                PaymentCompleteCallBack(pay, RMA, user);
            }
        }

        /// <summary>
        ///  付款单付款后,更新退换货信息
        /// </summary>
        /// <param name="pay">付款单</param>
        /// <param name="rma">退换货单</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        private void PaymentCompleteCallBack(FnPaymentVoucher pay, CBRcReturn rma, SyUser user)
        {
            if (pay != null && pay.Source == (int)FinanceStatus.付款来源类型.退换货单)
            {
                if (pay.Status == (int)FinanceStatus.付款单状态.已付款)//
                {
                    if (rma != null)
                    {
                        rma.Status = (int)RmaStatus.退换货状态.已完成;
                        rma.RefundBy = user.SysNo;
                        rma.RefundDate = DateTime.Now;
                        IRcReturnDao.Instance.Update(rma);
                        #region 更新出库单明细的已退数量
                        //拒收或者部分签收物流组已经更新了退换货数量，所以此处不再次更新数量
                        if (!(rma.Source == (int)RmaStatus.退换货申请单来源.拒收 || rma.Source == (int)RmaStatus.退换货申请单来源.部分签收))
                        {
                            foreach (RcReturnItem rItem in rma.RMAItems)
                            {
                                var stockOutItem = IOutStockDao.Instance.GetStockOutItem(rItem.StockOutItemSysNo);
                                stockOutItem.ReturnQuantity += rItem.RmaQuantity;//退换货数量
                                if (stockOutItem.ReturnQuantity > stockOutItem.ProductQuantity)
                                {
                                    throw new Exception("退换货数量超过出库数量");
                                }
                                IOutStockDao.Instance.UpdateOutItem(stockOutItem);
                            }
                        }
                        #endregion
                        //减积分，或者金币 
                        if (rma.RefundPoint > 0 || rma.RefundCoin > 0)
                        {
                            PointBo.Instance.RMADecreasePoint(pay.CustomerSysNo, rma.OrderSysNo, (int)rma.RefundCoin, rma.RefundPoint, pay.TransactionSysNo);
                        }
                        WriteRMALog(rma.SysNo, rma.TransactionSysNo, Constant.RMA_COMPLETE, user);
                        var order = SoOrderBo.Instance.GetEntity(rma.OrderSysNo);//退换货对应的订单
                        if (order != null && order.OrderSource == Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱.GetHashCode())//分销商升舱订单
                        {
                            MallSeller.DsOrderBo.Instance.RmaRefund(rma.OrderSysNo, rma.SysNo, rma.RefundTotalAmount, user.SysNo);//退预存款
                        }
                        #region 发送短信

                        var customerInfo = CrCustomerBo.Instance.GetModel(pay.CustomerSysNo);
                        if (customerInfo != null && rma.Source != (int)Hyt.Model.WorkflowStatus.RmaStatus.退换货申请单来源.分销商)
                        {
                            if (VHelper.Do(customerInfo.MobilePhoneNumber, VType.Mobile))
                            {
                                if (rma.HandleDepartment == (int)RmaStatus.退换货处理部门.门店)
                                {
                                    BLL.Extras.SmsBO.Instance.发送门店退款完成短信(customerInfo.MobilePhoneNumber,
                                                                       rma.SysNo.ToString());
                                }
                                else
                                {
                                    BLL.Extras.SmsBO.Instance.发送退款完成短信(customerInfo.MobilePhoneNumber,
                                                                       rma.SysNo.ToString());
                                }
                            }
                            //if (VHelper.Do(customerInfo.EmailAddress, VType.Email))
                            //{
                            //    BLL.Extras.EmailBo.Instance.发送退款完成邮件(customerInfo.EmailAddress,
                            //                                        rma.CustomerSysNo.ToString(), rma.SysNo.ToString());
                            //}
                        }
                    }
                        #endregion
                }
            }
        }
        #endregion

        #region 查询

        /// <summary>
        /// 出库单是否可退换货
        /// </summary>
        /// <param name="stockOut">出库单</param>
        /// <returns>true:是 false:否</returns>
        /// <remarks>2013-08-22 黄志勇 创建</remarks>
        public bool CanReturn(WhStockOut stockOut)
        {
            if (stockOut != null && (stockOut.Status == (int)WarehouseStatus.出库单状态.已签收 || stockOut.Status == (int)WarehouseStatus.出库单状态.部分退货 || stockOut.Status == (int)WarehouseStatus.出库单状态.全部退货))
            {
                var item = LgSettlementBo.Instance.GetLgSettlementItemByStockOut(stockOut.SysNo);
                if (item != null)
                {
                    if (item.Status == (int)LogisticsStatus.结算单明细状态.已结算) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 退换货订单列表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>订单分页列表</returns>
        /// <remarks>2013-07-11 朱家宏 创建</remarks>
        public Pager<CBSoOrder> GetRmaSoOrders(ParaOrderFilter filter)
        {
            filter.SettlementStatus = (int)LogisticsStatus.结算单明细状态.已结算;
            var pager = new Pager<CBSoOrder> { CurrentPage = filter.Id };
     
            SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);
            return pager;
        }

        /// <summary>
        /// 退换货维护分页列表(客服)
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>退换货分页列表</returns>
        /// <remarks>2013-07-11 朱家宏 创建</remarks>
        public Pager<CBRcReturn> GetRmasForCallCenter(ParaRmaFilter filter)
        {
            //filter.HandleDepartments = new List<int>
            //    {
            //        (int)RmaStatus.退换货处理部门.客服中心
            //    };
            var pager = IRcReturnDao.Instance.GetAll(filter);
            return pager;
        }

        /// <summary>
        /// 退换货维护分页列表(门店)
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>退换货分页列表</returns>
        /// <remarks>2013-07-11 朱家宏 创建</remarks>
        public Pager<CBRcReturn> GetRmasForShop(ParaRmaFilter filter)
        {
            if (filter.StoreSysNoList == null || !filter.StoreSysNoList.Any())
            {
                filter.StoreSysNoList = new List<int> { -1 };
            }
            else
            {
                if (!filter.HasWarehouse)
                {
                    filter.StoreSysNoList = null;
                }
            }
            filter.HandleDepartments = new List<int> { (int)RmaStatus.退换货处理部门.门店 };
            var pager = IRcReturnDao.Instance.GetAll(filter);
            return pager;
        }

        /// <summary>
        /// 以rma编号获取rma相关信息
        /// </summary>
        /// <param name="sysNo">rma sysNo</param>
        /// <returns>付款单、入库单、退换货日志</returns>
        /// <remarks>2013-07-17 朱家宏 创建</remarks>
        /// <remarks>2013-07-22 黄志勇 修改</remarks>
        public CBRmaRelations GetRmaRelationsBySysNo(int sysNo)
        {
            var logs = IRcReturnLogDao.Instance.GetListByReturnSysNo(sysNo);
            var paymentVoucher =
                DataAccess.Finance.IFnPaymentVoucherDao.Instance.GetEntityByVoucherSource(
                    (int)FinanceStatus.付款来源类型.退换货单, sysNo);
            var stockIn =
                IInStockDao.Instance.GetWhStockInByVoucherSource(
                    (int)WarehouseStatus.入库单据类型.RMA单, sysNo);

            var rmaRelations = new CBRmaRelations
            {
                PaymentVoucher = paymentVoucher,
                StockIn = (stockIn != null && stockIn.Status != (int)WarehouseStatus.入库单状态.作废) ? stockIn : new WhStockIn(),
                RmaLogs = logs
            };
            return rmaRelations;
        }

        #endregion

        #region 操作

        /// <summary>
        /// 可退换货订单判断
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>判断结果</returns>
        /// <remarks>2013-07-23 朱家宏 创建</remarks>
        public Result CanReturn(int orderSysNo)
        {
            /*
                1.出库单明细表：商品数量>退换货数量
                2.出库单明细表: 状态有效
                3.退换货主表的状态：待审核(10),待入库(20),待退款(30)（排除）
             */
            var r = new Result { Status = false };

            var stocks = IOutStockDao.Instance.GetWhStockOutListByOrderID(orderSysNo);
            foreach (var stock in stocks)
            {
                var items = stock.Items.ToList();
                if (items.Exists(o =>
                                 o.Status == (int)WarehouseStatus.出库单明细状态.有效 &&
                                 o.ProductQuantity > o.ReturnQuantity))
                {
                    r.Status = true;
                    break;
                }
            }

            if (!r.Status)
            {
                r.Message = "出库单无效或商品数量小于了退换货数量，不能进行退换货";
                return r;
            }

            var filter = new ParaRmaFilter { OrderSysNo = orderSysNo, PageSize = 50, Id = 1 };
            var rcReturns = IRcReturnDao.Instance.GetAll(filter).Rows.ToList();
            var rmaStatuses = new[]
                {
                    (int) RmaStatus.退换货状态.待入库,
                    (int) RmaStatus.退换货状态.待审核,
                    (int) RmaStatus.退换货状态.待退款
                };

            r.Status = !rcReturns.Exists(o => rmaStatuses.Contains(o.Status));

            if (!r.Status)
            {
                r.Message = "该订单已在退换货，不能再进行操作。";
            }

            return r;
        }

        #endregion

        #region [前台退换货]
        /// <summary>
        /// 订单退换货请求
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="rmaType">退换货类型 Hyt.Model.WorkflowStatus.RmaStatus.RMA类型</param>
        /// <returns>true 允许退换货 false 不允许退换货</returns>
        /// <remarks>2013-08-15 朱成果 创建</remarks>
        public bool OrderRMARequest(int orderSysNo, int rmaType)
        {
            //退货15天
            //换货默认1年
            int tDay = 15;
            int hDay = 365;
            bool flg = false;
            var lst = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhStockOutListByOrderID(orderSysNo);
            if (lst != null && lst.Count > 0)
            {
                var rmaItemList = Hyt.BLL.RMA.RmaBo.Instance.GetItemListByOrder(orderSysNo);//已退换货明细 (2013-08-07 朱成果)
                foreach (WhStockOut item in lst)
                {
                    if (CanReturn(item))
                    {
                        if (rmaType == (int)RmaStatus.RMA类型.售后退货 && DateTime.Now.AddDays(-tDay) < item.SignTime)
                        {
                            flg = true;
                        }
                        else if (rmaType == (int)RmaStatus.RMA类型.售后换货 && DateTime.Now.AddDays(-hDay) < item.SignTime)
                        {
                            flg = true;
                        }
                        if (flg)//退换货日期满足
                        {
                            flg = false;
                            foreach (var p in item.Items)
                            {
                                int canReturnCount = p.ProductQuantity - Hyt.BLL.RMA.RmaBo.Instance.GetAllRmaQuantity(rmaItemList, p.SysNo, 0);
                                flg = canReturnCount > 0;
                                if (flg)//退换货数量满足
                                {
                                    return flg;
                                }
                            }
                        }
                    }
                }
            }
            return flg;
        }
        /// <summary>
        /// 订单退换货请求
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="productid">产品编号</param>
        /// <param name="rmaType">退换货类型 Hyt.Model.WorkflowStatus.RmaStatus.RMA类型</param>
        /// <returns>true 允许退换货 false 不允许退换货</returns>
        /// <remarks>2013-08-15 朱成果 创建</remarks>
        public bool OrderRMARequest(int orderSysNo, int productid, int rmaType)
        {
            //退货15天
            //换货默认1年
            int tDay = 15;
            int hDay = 365;
            bool flg = false;
            var listItem = IOutStockDao.Instance.GetWhStockOutItemList(orderSysNo, productid);//出库明细
            if (listItem != null && listItem.Count > 0)
            {
                var rmaItemList = Hyt.BLL.RMA.RmaBo.Instance.GetItemListByOrder(orderSysNo);//已退换货明细 (2013-08-07 朱成果)

                foreach (var p in listItem)//遍历明细
                {
                    WhStockOut item = IOutStockDao.Instance.GetEntity(p.StockOutSysNo);//出库单
                    if (CanReturn(item))//出库单
                    {
                        if (rmaType == (int)RmaStatus.RMA类型.售后退货 && DateTime.Now.AddDays(-tDay) < item.SignTime)
                        {
                            flg = true;
                        }
                        else if (rmaType == (int)RmaStatus.RMA类型.售后换货 && DateTime.Now.AddDays(-hDay) < item.SignTime)
                        {
                            flg = true;
                        }
                        if (flg)//退换货日期满足
                        {
                            flg = false;
                            int canReturnCount = p.ProductQuantity - Hyt.BLL.RMA.RmaBo.Instance.GetAllRmaQuantity(rmaItemList, p.SysNo, 0);
                            flg = canReturnCount > 0;
                            if (flg)//退换货数量满足
                            {
                                return flg;
                            }
                        }
                    }
                }
            }
            return flg;
        }
        #endregion

        #region 退换货图片
        /// <summary>
        /// 添加退换货图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns>退换货图片编号</returns>
        /// <remarks>2013-09-23 朱成果 创建</remarks>
        public int InsertRMAImg(RcReturnImage model)
        {
            return IRcReturnImageDao.Instance.Insert(model);
        }
        #endregion

        /// <summary>
        /// 根据出库单明细系统编号获取退换货申请单
        /// </summary>
        /// <param name="stockOutItemSysNo">出库单明细系统编号</param>
        /// <param name="sourceType">退换货申请单来源</param>
        /// <returns>退换货申请单列表</returns>
        /// <remarks>
        /// 2013/8/21 何方 创建
        /// </remarks>
        public IList<RcReturn> Get(int stockOutItemSysNo, RmaStatus.退换货申请单来源? sourceType = null)
        {
            return IRcReturnDao.Instance.Get(stockOutItemSysNo, sourceType);
        }

        #region 获取退换货历史

        /// <summary>
        /// 根据会员编号获取退换货历史
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>退换货历史集合</returns>
        /// <remarks>
        /// 2013-09-13 沈强 创建
        /// </remarks>
        public IList<ReturnHistory> GetRcReturnByCustomerSysNo(int customerSysNo)
        {
            //设置查询条件
            var paraRmaFilter = new ParaRmaFilter();
            paraRmaFilter.CustomerSysNo = customerSysNo;

            //获取退换货历史集合
            var pager = IRcReturnDao.Instance.GetAll(paraRmaFilter);

            var returnHistories = new List<ReturnHistory>();

            foreach (var cbRcReturn in pager.Rows)
            {
                //转换到相应对象
                var returnHistory = new ReturnHistory
                {
                    CreateDate = cbRcReturn.CreateDate,
                    RmaType = cbRcReturn.RmaType,
                    Status = cbRcReturn.Status,
                    SysNo = cbRcReturn.SysNo,
                    StatusText = ((RmaStatus.退换货状态)cbRcReturn.Status).ToString()
                };

                //获取退换货明细
                var rcReturnItems = IRcReturnItemDao.Instance.GetListByReturnSysNo(cbRcReturn.SysNo);

                var returnHistoryItems = new List<ReturnHistoryItem>();

                foreach (var rcReturnItem in rcReturnItems)
                {
                    //转换退换货明细到相应对象
                    var returnHistoryItem = new ReturnHistoryItem
                    {
                        ProductName = rcReturnItem.ProductName,
                        ProductSysNo = rcReturnItem.ProductSysNo,
                        RmaQuantity = rcReturnItem.RmaQuantity,
                        Thumbnail = BLL.Web.ProductImageBo.Instance.GetProductImagePath(Web.ProductThumbnailType.Image180, rcReturnItem.ProductSysNo)
                    };
                    returnHistoryItems.Add(returnHistoryItem);
                }

                returnHistory.Items = returnHistoryItems;
                returnHistories.Add(returnHistory);
            }

            return returnHistories;
        }

        #endregion

        #region 计算退货金额

        /// <summary>
        /// 根据出库单明细编号计算退换货对象
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <param name="stockOutItemSysNo">出库单明细系统编号</param>
        /// <returns>退换货对象</returns>
        /// <remarks>2013-11-01 吴文强 创建</remarks>
        public CBRcReturnCalculate CalculateRmaAmountByStockOutItem(int orderSysNo, Dictionary<int, int> stockOutItemSysNo)
        {
            var stockOutItems =
                IOutStockDao.Instance.GetStockOutItem(stockOutItemSysNo.Select(dic => dic.Key).ToArray());

            //根据出库单获取订单明细系统编号
            var dicOrderItem = new Dictionary<int, int>();
            foreach (var stockOut in stockOutItems)
            {
                var dicSoi = stockOutItemSysNo.FirstOrDefault(soi => soi.Key == stockOut.SysNo);
                if (dicOrderItem.ContainsKey(stockOut.OrderItemSysNo))
                {
                    dicOrderItem[stockOut.OrderItemSysNo] += dicSoi.Value;
                }
                else
                {
                    dicOrderItem.Add(stockOut.OrderItemSysNo, dicSoi.Value);
                }
            }

            //计算退换货金额
            var rmaAmount = CalculateRmaAmount(orderSysNo, dicOrderItem);

            //分摊出库单金额
            var dicStockOutItem = new Dictionary<int, decimal>();
            var dicAllocatedNumber = new Dictionary<int, int>();
            var dicAllocatedAmount = new Dictionary<int, decimal>();

            foreach (var stockOut in stockOutItems)
            {
                //订单明细退款金额
                var orderItemAmount = rmaAmount.OrderItemAmount.FirstOrDefault(t => t.Key == stockOut.OrderItemSysNo).Value;

                //订单明细总退换货数
                var dicOi = dicOrderItem.FirstOrDefault(oi => oi.Key == stockOut.OrderItemSysNo).Value;

                //出库单明细退换货数
                var dicSoi = stockOutItemSysNo.FirstOrDefault(soi => soi.Key == stockOut.SysNo).Value;

                decimal stockOutAmount = 0;

                if ((dicSoi == dicOi)
                    || (dicAllocatedNumber.ContainsKey(stockOut.OrderItemSysNo)
                    && dicAllocatedNumber[stockOut.OrderItemSysNo] + dicSoi == dicOi))
                {
                    stockOutAmount = orderItemAmount -
                                     (dicAllocatedAmount.ContainsKey(stockOut.OrderItemSysNo)
                                          ? dicAllocatedAmount[stockOut.OrderItemSysNo]
                                          : 0);
                }
                else
                {
                    stockOutAmount = decimal.Round(dicSoi / dicOi * orderItemAmount, 2);
                }

                if (dicAllocatedNumber.ContainsKey(stockOut.OrderItemSysNo))
                {
                    dicAllocatedNumber[stockOut.OrderItemSysNo] += dicSoi;
                    dicAllocatedAmount[stockOut.OrderItemSysNo] += stockOutAmount;
                }
                else
                {
                    dicAllocatedNumber.Add(stockOut.OrderItemSysNo, dicSoi);
                    dicAllocatedAmount.Add(stockOut.OrderItemSysNo, stockOutAmount);
                }
                dicStockOutItem.Add(stockOut.SysNo, stockOutAmount);
            }
            rmaAmount.StockOutItemAmount = new List<KeyValuePair<int, decimal>>(dicStockOutItem);

            return rmaAmount;
        }

        /// <summary>
        /// 计算退货对象
        /// </summary>
        /// <param name="orderSysNo">销售单系统编号</param>
        /// <param name="orderItemSysNo">当前退货销售单明细编号和数量</param>
        /// <returns>退货计算对象</returns>
        /// <remarks>2013-09-16 吴文强 创建</remarks>
        /// <remarks>2014-06-12 吴文强 修改 分摊，先舍去2位以上的金额，最后一个商品补全退款总金额。</remarks>
        public CBRcReturnCalculate CalculateRmaAmount(int orderSysNo, Dictionary<int, int> orderItemSysNo)
        {
            //订单信息
            var order = SoOrderBo.Instance.GetEntity(orderSysNo);

            //客户信息
            var customer = CrCustomerBo.Instance.GetCrCustomerItem(order.CustomerSysNo);

            //1.获取有效销售订单明细
            var beforeOrderItems = IRcReturnDao.Instance.GetValidSalesOrderItem(orderSysNo).Where(t => t.Quantity != 0);
            var originalOrderItems = beforeOrderItems.ToList();

            //1.1.有效销售订单明细，转换为购物车明细对象并计算购物车对象
            var beforeShoppingCart = OrderItemToShoppinggCart(order, customer, beforeOrderItems);
            beforeShoppingCart = CalculateItemChangeAmount(beforeShoppingCart, orderSysNo);

            //2.计算退货后的有效销售订单明细（有效销售订单明细-将当前退货明细数量）
            var afterOrderItems = beforeOrderItems;
            var soOrderItems = afterOrderItems as SoOrderItem[] ?? afterOrderItems.ToArray();
            foreach (var orderItem in soOrderItems)
            {
                if (orderItemSysNo.ContainsKey(orderItem.SysNo))
                {
                    orderItem.Quantity = orderItem.Quantity - orderItemSysNo[orderItem.SysNo];
                }
            }
            afterOrderItems = soOrderItems.Where(t => t.Quantity != 0);

            //2.1 退货后的有效销售订单明细，转换为购物车明细对象并计算购物车对象
            var afterShoppingCart = OrderItemToShoppinggCart(order, customer, afterOrderItems);
            afterShoppingCart = CalculateItemChangeAmount(afterShoppingCart, orderSysNo);

            //3.退货金额 = 退货前商品销售金额– 退货后商品销售金额–（退货后是否可以使用优惠券?0 : 优惠券金额）
            var returnCalculate = new CBRcReturnCalculate();
            returnCalculate.CouponAmount = beforeShoppingCart.CouponAmount > afterShoppingCart.CouponAmount
                                               ? beforeShoppingCart.CouponAmount
                                               : 0;

            var refundCalculate = CalculateRefundRmaAmount(orderSysNo,
                                                           beforeShoppingCart.SettlementAmount -
                                                           afterShoppingCart.SettlementAmount, true);
            returnCalculate.OrginAmount = refundCalculate.RefundProductAmount;
            returnCalculate.OrginCoin = refundCalculate.RefundCoin;
            returnCalculate.RefundCoin = refundCalculate.RefundCoin;
            returnCalculate.OrginPoint = refundCalculate.OrginPoint;//余勇 修改 将应退积分计算移入CalculateRefundRmaAmount方法 refundCalculate.RefundPoint - (int)refundCalculate.RefundCoin < 0 ? 0 : refundCalculate.RefundPoint - (int)refundCalculate.RefundCoin;
            returnCalculate.RefundPoint = refundCalculate.RefundPoint; //余勇 修改 将实退积分计算移入CalculateRefundRmaAmount方法 refundCalculate.RefundPoint - (int)refundCalculate.RefundCoin < 0 ? 0 : refundCalculate.RefundPoint - (int)refundCalculate.RefundCoin;
            returnCalculate.RefundProductAmount = refundCalculate.RefundProductAmount;
            returnCalculate.DeductedInvoiceAmount = refundCalculate.DeductedInvoiceAmount;
            returnCalculate.RedeemAmount = refundCalculate.RedeemAmount;
            returnCalculate.RefundTotalAmount = refundCalculate.RefundTotalAmount;

            //计算销售单明细项退款金额
            var itemKeyValuePair = new List<KeyValuePair<int, decimal>>();
            var beforeItems = beforeShoppingCart.GetShoppingCartItem();
            var afterItems = afterShoppingCart.GetShoppingCartItem();

            #region 附加赠品系统编号
            foreach (var cartItem in beforeItems)
            {
                if (cartItem.SysNo == 0)
                {
                    var item =
                        originalOrderItems.FirstOrDefault(
                            t =>
                            t.ProductSalesType == cartItem.ProductSalesType && t.ProductSysNo == cartItem.ProductSysNo &&
                            t.UsedPromotions == cartItem.UsedPromotions);

                    if (item != null)
                    {
                        cartItem.SysNo = item.SysNo;
                        cartItem.SalesUnitPrice = item.SalesUnitPrice;
                        cartItem.SaleTotalAmount = item.SalesAmount;
                    }
                }
            }
            foreach (var cartItem in afterItems)
            {
                if (cartItem.SysNo == 0)
                {
                    var item =
                        originalOrderItems.FirstOrDefault(
                            t =>
                            t.ProductSalesType == cartItem.ProductSalesType && t.ProductSysNo == cartItem.ProductSysNo &&
                            t.UsedPromotions == cartItem.UsedPromotions);

                    if (item != null)
                    {
                        cartItem.SysNo = item.SysNo;
                        cartItem.SalesUnitPrice = item.SalesUnitPrice;
                        cartItem.SaleTotalAmount = item.SalesAmount;
                    }
                }
            }
            #endregion

            foreach (var item in orderItemSysNo)
            {
                var biProduct = beforeItems.FirstOrDefault(bi => bi.SysNo == item.Key);
                var aiProduct = afterItems.FirstOrDefault(ai => ai.SysNo == item.Key);

                decimal beforeItemAmount = 0;
                decimal afterItemAmount = 0;

                if (biProduct != null)
                {
                    beforeItemAmount = biProduct.SaleTotalAmount - biProduct.DiscountAmount;
                }

                if (aiProduct != null)
                {
                    afterItemAmount = aiProduct.SaleTotalAmount - aiProduct.DiscountAmount;
                }

                itemKeyValuePair.Add(new KeyValuePair<int, decimal>(item.Key, beforeItemAmount - afterItemAmount));
            }

            //计算分摊明细项退款金额
            returnCalculate.OrderItemAmount = new List<KeyValuePair<int, decimal>>();
            var itemTotalAmount = itemKeyValuePair.Sum(t => t.Value);
            var totalApportionAmount = refundCalculate.RefundProductAmount - itemTotalAmount;
            decimal apportionedAmount = 0;
            if (totalApportionAmount == 0)
            {
                returnCalculate.OrderItemAmount = itemKeyValuePair;
            }
            else
            {
                for (int i = 0; i < itemKeyValuePair.Count; i++)
                {
                    if (i < itemKeyValuePair.Count - 1)
                    {
                        //2014-06-12 吴文强 修改 分摊，先舍去2位以上的金额，最后一个商品补全退款总金额。
                        //var appAmount = decimal.Round(itemKeyValuePair[i].Value / itemTotalAmount * totalApportionAmount, 2);
                        var appAmount = (itemKeyValuePair[i].Value / itemTotalAmount * totalApportionAmount).RoundToShe(2);

                        returnCalculate.OrderItemAmount.Add(
                            new KeyValuePair<int, decimal>(itemKeyValuePair[i].Key, appAmount + itemKeyValuePair[i].Value));
                        apportionedAmount = apportionedAmount + appAmount;
                    }
                    else
                    {
                        returnCalculate.OrderItemAmount.Add(
                            new KeyValuePair<int, decimal>(itemKeyValuePair[i].Key,
                                                           totalApportionAmount - apportionedAmount +
                                                           itemKeyValuePair[i].Value));
                    }
                }
            }

            returnCalculate.ShowInvoiceExpire = false;
            if (order.InvoiceStatus() == FinanceStatus.订单发票状态.已过期)
            {
                returnCalculate.ShowInvoiceExpire = true;
            };

            return returnCalculate;
        }

        /// <summary>
        /// 计算实际退货对象
        /// </summary>
        /// <param name="orderSysNo">销售单系统编号</param>
        /// <param name="refundAmount">退款商品总额</param>
        /// <param name="isRecaptionInvoice">是否取回发票(发票过期会自动做取回计算)</param>
        /// <returns>退货计算对象</returns>
        /// <remarks>2013-09-17 余勇 创建</remarks>
        /// <remarks>2013-10-29 吴文强 更新</remarks>
        /// <remarks>2013-11-14 朱成果 更新</remarks>
        public CBRcReturnCalculate CalculateRefundRmaAmount(int orderSysNo, decimal refundAmount, bool isRecaptionInvoice)
        {
            var returnCalculate = new CBRcReturnCalculate() { RefundProductAmount = refundAmount };
            returnCalculate= CalculateRefundRmaAmount(orderSysNo, returnCalculate, isRecaptionInvoice);
            //余勇 修改 添加应退积分及实退积分计算
            returnCalculate.OrginPoint = returnCalculate.RefundPoint - (int)returnCalculate.RefundCoin < 0 ? 0 : returnCalculate.RefundPoint - (int)returnCalculate.RefundCoin;
            returnCalculate.RefundPoint = returnCalculate.RefundPoint - (int)returnCalculate.RefundCoin < 0 ? 0 : returnCalculate.RefundPoint - (int)returnCalculate.RefundCoin;
            return returnCalculate;
        }

        /// <summary>
        /// 计算实际退货对象
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="returnCalculate">实退商品信息</param>
        /// <param name="isRecaptionInvoice">是否收到发票</param>
        /// <returns></returns>
        /// <remarks>2013-11-14 朱成果 创建</remarks>
        public CBRcReturnCalculate CalculateRefundRmaAmount(int orderSysNo, CBRcReturnCalculate returnCalculate, bool isRecaptionInvoice)
        {
            var order = SoOrderBo.Instance.GetEntity(orderSysNo);
            //客户信息
            var customer = CrCustomerBo.Instance.GetCrCustomerItem(order.CustomerSysNo);
            //实退商品金额
            returnCalculate.RefundProductAmount = returnCalculate.RefundProductAmount;
            //发票扣款金额
            FinanceStatus.订单发票状态 invoiceStatus = order.InvoiceStatus();
            if (invoiceStatus == FinanceStatus.订单发票状态.无发票)
            {
                returnCalculate.DeductedInvoiceAmount = 0;
            }
            else
            {
                if (!isRecaptionInvoice || invoiceStatus == FinanceStatus.订单发票状态.已过期)
                {
                    var invoiceAmount = FnInvoiceBo.Instance.DeductedInvoiceAmount(order.SysNo,
                                                                                   returnCalculate.RefundProductAmount);
                    returnCalculate.DeductedInvoiceAmount = invoiceAmount > 5 ? invoiceAmount : 0;
                }
            }
            //计算实扣积分和现金补偿
            var orginPoint = PointBo.Instance.MoneyToPoint(returnCalculate.RefundProductAmount - returnCalculate.RefundCoin);
            if (order.Status == (int)OrderStatus.销售单状态.已完成 && customer.ExperiencePoint < orginPoint)
            {
                returnCalculate.RefundPoint = orginPoint;
                returnCalculate.RedeemAmount = 0;
                //returnCalculate.RefundPoint = customer.ExperiencePoint;
                //returnCalculate.RedeemAmount = PointBo.Instance.PointToMoney(orginPoint - returnCalculate.RefundPoint);
            }
            else
            {
                returnCalculate.RefundPoint = orginPoint;
                returnCalculate.RedeemAmount = 0;
            }
            //实退总金额
            returnCalculate.RefundTotalAmount = returnCalculate.RefundProductAmount
                                                - returnCalculate.DeductedInvoiceAmount
                                                - returnCalculate.RedeemAmount;
            //计算实退惠源币、实退总金额
            //订单剩余惠源币
            var orderResidueCoin = GetOrderResidueCoin(order.SysNo);
            if (orderResidueCoin > 0)
            {
                //剩余惠源币金额
                var coinAmount = PointBo.Instance.MoneyToExperienceCoin(orderResidueCoin);

                if (returnCalculate.RefundTotalAmount > coinAmount)
                {
                    returnCalculate.RefundTotalAmount = returnCalculate.RefundTotalAmount - coinAmount;
                    returnCalculate.RefundCoin = coinAmount;
                }
                else
                {
                    returnCalculate.RefundCoin = returnCalculate.RefundTotalAmount;
                    returnCalculate.RefundTotalAmount = 0;
                }
            }
            returnCalculate.ShowInvoiceExpire = false;
            if (order.InvoiceStatus() == FinanceStatus.订单发票状态.已过期)
            {
                returnCalculate.ShowInvoiceExpire = true;
            };
            return returnCalculate;
        }

        /// <summary>
        /// 销售单明细还原购物车对象(计算后的购物车)
        /// </summary>
        /// <param name="order">销售单</param>
        /// <param name="customer">客户信息</param>
        /// <param name="orderItems">销售单明细</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-16 吴文强 创建</remarks>
        private CrShoppingCart OrderItemToShoppinggCart(SoOrder order, CrCustomer customer, IEnumerable<SoOrderItem> orderItems)
        {
            //转CBCrShoppingCartItem
            IList<CBCrShoppingCartItem> shoppingCartItems = new List<CBCrShoppingCartItem>();
            var promotionSysNo = new List<int>();
            foreach (var orderItem in orderItems)
            {
                shoppingCartItems.Add(new CBCrShoppingCartItem()
                {
                    IsChecked = (int)CustomerStatus.是否选中.是,
                    SysNo = orderItem.SysNo,
                    ProductSysNo = orderItem.ProductSysNo,
                    ProductName = orderItem.ProductName,
                    Quantity = orderItem.Quantity,
                    OriginPrice = orderItem.OriginalPrice,
                    SalesUnitPrice = orderItem.SalesUnitPrice,
                    SaleTotalAmount = orderItem.SalesUnitPrice * orderItem.Quantity,
                    IsLock = string.IsNullOrEmpty(orderItem.GroupCode)
                                 ? (int)CustomerStatus.购物车是否锁定.否
                                 : (int)CustomerStatus.购物车是否锁定.是,
                    Promotions = orderItem.UsedPromotions,
                    UsedPromotions = orderItem.UsedPromotions,
                    GroupCode = orderItem.GroupCode,
                    ProductSalesType = (int)orderItem.ProductSalesType,
                    ProductSalesTypeSysNo = orderItem.ProductSalesTypeSysNo,
                });

                promotionSysNo.AddRange((orderItem.UsedPromotions ?? string.Empty)
                                            .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(int.Parse));
            }

            promotionSysNo.AddRange((order.UsedPromotions ?? string.Empty)
                                        .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(int.Parse));

            //获取优惠券代码
            var coupons = SoOrderBo.Instance.GetCouponByOrderSysNo(order.SysNo);
            var couponCode = string.Empty;
            if (coupons != null && coupons.Count > 0)
            {
                couponCode = coupons[0].CouponCode;
            }

            var promotionToPython = new SpPromotionToPython()
                {
                    IsReturn = true,
                    Order = order
                };

            //计算后的购物车对象
            var shoppingCart = SpPromotionEngineBo.Instance.CalculateShoppingCart(new[] { order.GetPromotionPlatformType() },
                customer, shoppingCartItems, promotionSysNo.Distinct().ToArray(), couponCode, false, null, order.DeliveryTypeSysNo, null, order.DefaultWarehouseSysNo, false, promotionToPython);

            //1 退换货时：当赠品不成立时，将赠品转为普通商品重新做计算
            //1.1 判断不成立的赠品
            var oldGiftItems = orderItems.Where(item => item.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品).ToList();
            //var newGiftItems = shoppingCart.Gifts();
            var productGroupGifts = shoppingCart.ProductGroupGifts();
            var orderGroupGifts = shoppingCart.OrderGroupGifts();
            if (oldGiftItems.Count != (productGroupGifts.Count + orderGroupGifts.Count))
            {
                //1.1.1 变量需要变更的赠品
                foreach (var oldGiftItem in oldGiftItems)
                {
                    var newGift = productGroupGifts.Count > 0 ? productGroupGifts : orderGroupGifts;
                    if (!newGift.Any(
                            ngi =>
                            ngi.ProductSysNo == oldGiftItem.ProductSysNo &&
                            ngi.PromotionSysNo.ToString().Equals(oldGiftItem.UsedPromotions)))
                    {
                        oldGiftItem.SalesUnitPrice = oldGiftItem.OriginalPrice;
                        oldGiftItem.ProductSalesType = (int)CustomerStatus.商品销售类型.普通;

                        //每次将1个赠品变为普通商品进行计算
                        break;
                    }
                }
                //1.1.2 递归重新计算订单商品
                return OrderItemToShoppinggCart(order, customer, orderItems);
            }

            return shoppingCart;
        }

        /// <summary>
        /// 计算订单明细调价金额
        /// </summary>
        /// <param name="shoppingCart">购物车</param>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns>调价后的购物车</returns>
        /// <remarks>2013-12-03 吴文强 创建</remarks>
        private CrShoppingCart CalculateItemChangeAmount(CrShoppingCart shoppingCart, int orderSysNo)
        {
            var orderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysNo);
            decimal totalChangeAmount = 0;
            foreach (var cartGroup in shoppingCart.ShoppingCartGroups)
            {
                foreach (var cartItem in cartGroup.ShoppingCartItems)
                {
                    var orderItem = orderItems.FirstOrDefault(oi => oi.SysNo == cartItem.SysNo);
                    if (orderItem != null && orderItem.ChangeAmount != 0)
                    {
                        var itemChangeAmount = decimal.Round(
                            orderItem.ChangeAmount / orderItem.Quantity * cartItem.Quantity, 2);
                        cartItem.SaleTotalAmount = cartItem.SaleTotalAmount + itemChangeAmount;

                        totalChangeAmount += itemChangeAmount;
                    }
                }
            }
            shoppingCart.SettlementAmount += totalChangeAmount;
            return shoppingCart;
        }

        /// <summary>
        /// 获取订单剩余金币
        /// (订单消费金币-退换货已退回的金币[待退回或已完成])
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns>订单剩余有效金币</returns>
        /// <remarks>2013-12-03 吴文强 创建</remarks>
        public decimal GetOrderResidueCoin(int orderSysNo)
        {
            var order = SoOrderBo.Instance.GetEntity(orderSysNo);
            return order.CoinPay - IRcReturnDao.Instance.GetReturnCoin(orderSysNo) - order.GetFreight().CoinDeduction;
        }

        #endregion

        /// <summary>
        /// 获取退换货相关商品图
        /// </summary>
        /// <param name="rmaSysNo">退换货编号</param>
        /// <returns>list</returns>
        /// <remarks>2013-09-23 朱家宏 创建</remarks>
        public IList<RcReturnImage> GetRmaImages(int rmaSysNo)
        {
            return IRcReturnImageDao.Instance.GetAll(rmaSysNo);
        }

        #region RMA销售单明细关联表
        /// <summary>
        /// 添加RMA销售单明细关联表数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <remarks>2014-06-16 朱成果 创建</remarks>
        public void InsertSoReturnOrderItem(SoReturnOrderItem entity)
        {
            ISoReturnOrderItemDao.Instance.Insert(entity);
        }

        /// <summary>
        /// 获销售单明细关系表详情
        /// </summary>
        /// <param name="orderItemSysNo">RMA销售单明细编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-16  朱成果 创建</remarks>
        public SoReturnOrderItem GetSoReturnOrderItem(int orderItemSysNo)
        {
            return ISoReturnOrderItemDao.Instance.GetSoReturnOrderItem(orderItemSysNo);
        }

        /// <summary>
        /// 获销售单明细关系表详情
        /// </summary>
        /// <param name="transactionSysNo">RMA销售单事务编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-16 杨浩 创建</remarks>
        public IList<SoReturnOrderItem> GetSoReturnOrderItem(string transactionSysNo)
        {
            return ISoReturnOrderItemDao.Instance.GetSoReturnOrderItem(transactionSysNo);
        }

        #endregion

         /// <summary>
        /// 获取RMA订单编号
        /// </summary>
        /// <param name="rmaid">退换货编号</param>
        /// <returns>RMA订单编号</returns>
        /// <remarks>
        /// 2014-06-17 朱成果 创建
        /// </remarks>
        public  int GetRMAOrderSysNo(int rmaid)
        {
            return IRcReturnDao.Instance.GetRMAOrderSysNo(rmaid);
        }

        /// <summary>
        /// 退换货返利扣除列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-6 杨浩 创建</remarks>
        public List<CBReurnDeductRebates> GetReurnDeductRebates(int orderSysNo)
        {
            return IRcReturnDao.Instance.GetReurnDeductRebates(orderSysNo);             
        }

        #region 小商品退换货
        /// <summary>
        /// 判断是否是小商品退换货
        /// </summary>
        /// <param name="rmaid">退换货编号</param>
        /// <returns></returns>
        /// <remarks>2014-11-18 朱成果 创建</remarks>
        public bool CheckSmallProductRma(int rmaid)
        {
            var model = GetSmallProductRma(rmaid);
            return model != null;
        }

        /// <summary>
        /// 获取小商品退换货记录
        /// </summary>
        /// <param name="rmaid">退换货编号</param>
        /// <returns></returns>
        /// <remarks>2014-11-18 朱成果 创建</remarks>
        public RcNoReturnExchange GetSmallProductRma(int rmaid)
        {
            var model = IRcNoReturnExchangeDao.Instance.GetModelByRmaID(rmaid);
            return model;
        }


        /// <summary>
        /// 添加小商品退换货记录
        /// </summary>
        /// <param name="entity">小商品退换货记录</param>
        /// <remarks>2014-11-18 朱成果 创建</remarks>
        public void InsertSmallProductRma(RcNoReturnExchange entity)
        {
            IRcNoReturnExchangeDao.Instance.Insert(entity);
        }

        /// <summary>
        /// 更新小商品退换货记录
        /// </summary>
        /// <param name="entity">小商品退换货记录</param>
        /// <remarks>2014-11-18 朱成果 创建</remarks>
        public void UpdateSmallProductRma(RcNoReturnExchange entity)
        {
            IRcNoReturnExchangeDao.Instance.Update(entity);
        }
        #endregion
    }
    
}
