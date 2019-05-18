using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Extras;
using Hyt.BLL.Order;
using Hyt.BLL.Sys;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Pager;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 调货单业务类
    /// </summary>
    public class TransferCargoBo : BOBase<TransferCargoBo>
    {
        /// <summary>
        /// 根据主键编号获取调货单实体
        /// </summary>
        /// <param name="sysno">主键编号</param>
        /// <returns>调货单实体</returns>
        /// <remarks>2016-04-06 杨浩 创建</remarks>
        public TransferCargo Get(int sysno)
        {
            return ITransferCargoDao.Instance.GetEntity(sysno);
        }

        /// <summary>
        /// 更新调货单
        /// </summary>
        /// <param name="model">调货单实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2016-04-06 杨浩 创建</remarks>
        public int Update(TransferCargo model)
        {
            return ITransferCargoDao.Instance.Update(model);
        }

        /// <summary>
        /// 确认调货
        /// </summary>
        /// <param name="sysNo">调货单号</param>
        /// <param name="userid">用户编号</param>
        /// <remarks>2016-04-08 朱成果 创建</remarks>
        public void AffirmTransferCargo(int sysNo, int userid)
        {
            var master = TransferCargoBo.Instance.Get(sysNo);
            if (master != null && master.Status == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.调货状态.待处理)
            {
                master.Status = (int)Hyt.Model.WorkflowStatus.WarehouseStatus.调货状态.已确认;
                master.LastUpdateBy = userid;
                master.LastUpdateDate = DateTime.Now;
                TransferCargoBo.Instance.Update(master);
                var stockOut = WhWarehouseBo.Instance.Get(master.StockOutSysNo);
                if (stockOut.Status != (int)WarehouseStatus.出库单状态.调货中)
                {
                    throw new HytException("出库单状态非调货中");
                }
                if (!Authentication.AdminAuthenticationBo.Instance.HasWareHousePrivilege(master.DeliveryWarehouseSysNo))
                {
                    throw new Exception("用户没有编号为" + master.DeliveryWarehouseSysNo + "仓库的权限");
                }
                stockOut.Status = (int)WarehouseStatus.出库单状态.待出库;
                stockOut.LastUpdateBy = userid;
                stockOut.LastUpdateDate = DateTime.Now;
                WhWarehouseBo.Instance.UpdateStockOut(stockOut);
                WhWarehouseBo.Instance.UpdateErpProductNumber(master.StockOutSysNo);//// Extra.Erp.Model.ErpBillSource.单据来源.调货);
                var soOrderEn = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);
                if (soOrderEn != null)
                {
                    SoOrderBo.Instance.WriteSoTransactionLog(soOrderEn.TransactionSysNo,
                        string.Format("调货单{0}已确认调货", master.SysNo),
                        SyUserBo.Instance.GetUserName(userid), OrderStatus.是否对客户显示订单日志.否.GetHashCode());
                }
            }
        }

        /// <summary>
        /// 是否允许调货
        /// </summary>
        /// <param name="stockOut">出库单</param>
        /// <returns></returns>
        /// <remarks>2016-04-08 朱成果 创建</remarks>
        public Hyt.Model.Result<TransferCargoConfig> CanTransferCargo(WhStockOut stockOut)
        {
            Hyt.Model.Result<TransferCargoConfig> result = new Hyt.Model.Result<TransferCargoConfig>() { Status = false };
            if (stockOut == null)
            {
                result.Message = string.Format("当前出库单不存在");
                return result;
            }
            if (!(stockOut.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.普通百城当日达 || stockOut.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.门店自提))
            {
                result.Message = string.Format("当前配送方式出库单不允许调货");
                return result;
            }
            if (stockOut.IsCOD == Hyt.Model.WorkflowStatus.WarehouseStatus.是否到付.是.GetHashCode())
            {
                result.Message = string.Format("货到付款订单不允许调货");
                return result;
            }
            if (stockOut.Status != (int)WarehouseStatus.出库单状态.待出库)
            {
                result.Message = string.Format("只有待出库的出库单才允许申请调货");
                return result;
            }
            //if (WhWarehouseBo.Instance.IsSelfSupport(stockOut.WarehouseSysNo))
            //{
            //    result.Message = string.Format("自营仓库不允许申请调货");
            //    return result;
            //}
            var config = Hyt.BLL.Basic.TransferCargoConfigBo.Instance.GetEntityByApplyWarehouseSysNo(stockOut.WarehouseSysNo);//有调货配置
            if (config == null)
            {
                result.Message = string.Format("申请仓库编号为:{0}的仓库未配置配货仓库，请在基础管理-调货配置中配置", stockOut.WarehouseSysNo);
                return result;
            }
            result.Data = config;
            var exist = ExistTransferCargoByStockOutSysno(stockOut.SysNo);
            if (exist)
            {
                result.Message = string.Format("出库单号为:{0}的出库单已申请过调货，不能再次申请", stockOut.SysNo);
                return result;
            }
            result.Status = true;
            return result;

        }


        /// <summary>
        /// 调货单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="userSysno">操作人</param>
        /// <param name="hasAllWarehouse">仓库权限</param>
        /// <returns>列表</returns>
        /// <remarks>2016-04-05 杨浩 创建</remarks>
        public PagedList<CBTransferCargo> SearchTransferCargo(ParaTransferCargoFilter filter, int pageIndex, int pageSize, int userSysno, bool hasAllWarehouse)
        {
            if (filter == null)
            {
                return null;
            }
            var model = new PagedList<CBTransferCargo>();
            var result = ITransferCargoDao.Instance.SearchTransferCargo(filter, pageIndex, pageSize, userSysno, hasAllWarehouse);
            model.TData = result.Rows;
            model.PageSize = pageSize;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;
            return model;
        }

        /// <summary>
        /// 根据订单号查询调货单
        /// </summary>
        /// <param name="orderSysno">订单号</param>
        /// <returns>调货单</returns>
        /// <remarks>2016-04-08 杨浩 创建</remarks>
        public IList<CBTransferCargo> SearchTransferCargoesByOrderSysno(int orderSysno)
        {
            return ITransferCargoDao.Instance.GetTransferCargoesByOrderSysno(orderSysno);
        }


        /// <summary>
        /// 获取调货单
        /// </summary>
        /// <param name="stockOutSysno">出库单</param>
        /// <returns></returns>
        /// <remarks>2016-04-11 朱成果 创建</remarks>
        public TransferCargo GetTransferCargo(int stockOutSysno)
        {
            var model = ITransferCargoDao.Instance.GetExistTransferCargoByStockOutSysno(stockOutSysno);
            return model;
        }

        /// <summary>
        /// 出库单是否已申请过调货
        /// </summary>
        /// <param name="stockOutSysno">出库单编号</param>
        /// <returns>已申请过调货返回true,否则返回false</returns>
        /// <remarks>2016-04-07 杨浩 创建</remarks>
        public bool ExistTransferCargoByStockOutSysno(int stockOutSysno)
        {
            var model = GetTransferCargo(stockOutSysno);
            if (model != null)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 申请调货
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="userSysno">申请人</param>
        /// <returns></returns>
        /// <remarks>2016-04-01 杨浩 创建</remarks>
        public Result TransferCargo(int sysNo, int userSysno)
        {
            var result = new Result { Status = false };
            var stockOut = WhWarehouseBo.Instance.Get(sysNo);
            var r = CanTransferCargo(stockOut);
            result.Status = r.Status;
            result.Message = r.Message;
            if (!r.Status)
            {
                return result;
            }
            if (!Authentication.AdminAuthenticationBo.Instance.HasWareHousePrivilege(stockOut.WarehouseSysNo))
            {
                throw new Exception("用户没有编号为" + stockOut.WarehouseSysNo + "仓库的权限");
            }
            stockOut.Remarks = "申请调货中";
            stockOut.Status = (int)WarehouseStatus.出库单状态.调货中;
            IOutStockDao.Instance.Update(stockOut);
            var molel = new TransferCargo
            {
                ApplyWarehouseSysNo = stockOut.WarehouseSysNo,
                CreatedBy = userSysno,
                CreatedDate = DateTime.Now,
                DeliveryWarehouseSysNo = r.Data.DeliveryWarehouseSysNo,
                LastUpdateBy = userSysno,
                LastUpdateDate = DateTime.Now,
                OrderSysNo = stockOut.OrderSysNO,
                Remarks = "仓库缺货申请调货",
                Status = (int)WarehouseStatus.调货状态.待处理,
                StockOutSysNo = sysNo
            };
            var transferCargoSysno =  ITransferCargoDao.Instance.Insert(molel);
            result.Status = true;
            if (stockOut.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.门店自提)
            {
                //var config = WhWarehouseConfigBo.Instance.GetEntityByWarehouseSysNo(stockOut.WarehouseSysNo);
                //var order = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);
                //var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                //if (config != null && order != null && receiveAddress != null)
                //{
                //    SmsBO.Instance.发送门店自提订单申请调货时的短信(receiveAddress.MobilePhoneNumber, receiveAddress.Name, order.SysNo, config.ReceiveMobileNo);
                //}
            }
            var soOrderEn = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);
            if (soOrderEn != null)
            {
                SoOrderBo.Instance.WriteSoTransactionLog(soOrderEn.TransactionSysNo,
                    string.Format("出库单{0}已申请调货,调货单{1}，待配货仓库处理", sysNo, transferCargoSysno),
                    SyUserBo.Instance.GetUserName(userSysno), OrderStatus.是否对客户显示订单日志.否.GetHashCode());
            }
            return result;
        }



        /// <summary>
        /// 作废出库单时，如果出库单申请了调货单，则同时作废调货单
        /// </summary>
        /// <param name="stockOutSysno">出库单编号</param>
        /// <param name="userSysno">操作人</param>
        /// <remarks>2016-04-06 杨浩 创建</remarks>
        public void CancelTransferCargoOnCancelStockOut(int stockOutSysno, int userSysno)
        {
            var model = GetTransferCargo(stockOutSysno);
            if (model != null)
            {
                if (model.Status == (int)WarehouseStatus.调货状态.待处理)
                {
                    model.Status = (int)WarehouseStatus.调货状态.已作废;
                    model.LastUpdateBy = userSysno;
                    model.LastUpdateDate = DateTime.Now;
                    ITransferCargoDao.Instance.Update(model);
                }
                else if (model.Status == (int)WarehouseStatus.调货状态.已确认)
                {
                    var stockOut = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.Get(stockOutSysno);
                    if (stockOut != null)
                    {
                        var stockIn = new WhStockIn
                        {
                            CreatedBy = userSysno,
                            CreatedDate = DateTime.Now,
                            DeliveryType = (int)WarehouseStatus.入库物流方式.已调货出库单作废,
                            IsPrinted = (int)WarehouseStatus.是否已经打印拣货单.否,
                            Remarks = "已调货出库单（" + stockOutSysno + ")作废",
                            SourceSysNO = stockOutSysno,
                            SourceType = Hyt.Model.WorkflowStatus.WarehouseStatus.入库单据类型.出库单.GetHashCode(),
                            Status = (int)WarehouseStatus.入库单状态.已入库,
                            TransactionSysNo = stockOut.TransactionSysNo,
                            WarehouseSysNo = stockOut.WarehouseSysNo,
                            ItemList = new List<WhStockInItem>(),
                            LastUpdateBy = userSysno,
                            LastUpdateDate = DateTime.Now
                        };

                        foreach (var stockOutItem in stockOut.Items)
                        {
                            stockIn.ItemList.Add(new WhStockInItem
                            {
                                CreatedBy = userSysno,
                                CreatedDate = DateTime.Now,
                                ProductName = stockOutItem.ProductName,
                                ProductSysNo = stockOutItem.ProductSysNo,
                                StockInQuantity = stockOutItem.ProductQuantity,//退货数量
                                RealStockInQuantity = stockOutItem.ProductQuantity,
                                LastUpdateBy = userSysno,
                                LastUpdateDate = DateTime.Now,
                                SourceItemSysNo = stockOutItem.SysNo
                            });
                        }
                        InStockBo.Instance.CreateStockIn(stockIn);//插入入库单，写入补货退货EAS单据
                        var soOrderEn = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);
                        if (soOrderEn != null)
                        {
                            SoOrderBo.Instance.WriteSoTransactionLog(soOrderEn.TransactionSysNo,
                                string.Format("调货单{0}已撤消", model.SysNo),
                                SyUserBo.Instance.GetUserName(userSysno), OrderStatus.是否对客户显示订单日志.否.GetHashCode());
                        }
                    }
                }
            }
        }

    }
}
