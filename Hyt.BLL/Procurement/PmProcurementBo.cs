using Hyt.DataAccess.Procurement;
using Hyt.Model;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Procurement
{
    public class PmProcurementBo : BOBase<PmProcurementBo>
    {
        #region 创建采购单

        /// <summary>
        /// 添加采购单信息
        /// </summary>
        /// <param name="order"></param>
        /// <param name="itemList"></param>
        public void CreateOrUpdatePmProcurementOrder(CBPmProcurementOrder order)
        {
            int sysno = 0;
            ///创建采购申请单
            if (order.SysNo == 0)
            {
                
                sysno = IPmProcurementDao.Instance.CreatePmProcurementOrder(order);
                order.SysNo = sysno;
            }
            else
            {
                sysno = order.SysNo;
                IPmProcurementDao.Instance.UpdatePmProcurementOrder(order);
            }
            ///采购申请单明细
            foreach (PmProcurementOrderItem item in order.orderItemList)
            {
                item.Poi_PSysNo = sysno;
                int itemSysNo = 0;
                if (item.SysNo == 0)
                {
                    itemSysNo = IPmProcurementDao.Instance.CreatePmProcurementOrderItem(item);
                }
                else
                {
                    itemSysNo = item.SysNo;
                    IPmProcurementDao.Instance.UpdatePmProcurementOrderItem(item);
                }
                ///相关商品网上参考价格
                var priceModList = order.webPriceList.FindAll(p => p.Pwp_ProSysNo == item.Poi_ProSysNo);
                if (priceModList != null)
                {
                    foreach (var priceMod in priceModList)
                    {
                        priceMod.Pwp_OrderItemSysNo = itemSysNo;
                        if (priceMod.SysNo == 0)
                        {
                            IPmProcurementDao.Instance.CreatePmProcurementWebPrice(priceMod);
                        }
                        else
                        {
                            IPmProcurementDao.Instance.UpdatePmProcurementWebPrice(priceMod);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修改采购单状态
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <param name="Status">状态</param>
        /// <param name="UpdateBy">修改人员时间</param>
        public void ProcurmentOrderStatus(int SysNo, int Status, int UpdateBy)
        {
            IPmProcurementDao.Instance.UpdatePmProcurementOrderStatus(SysNo, Status, UpdateBy);
        }
        /// <summary>
        /// 网上类型页面
        /// </summary>
        /// <param name="pager"></param>
        public void GetProcurmentWebTypePager(ref Pager<PmProcurementWebType> pager)
        {
            IPmProcurementDao.Instance.GetProcurmentWebTypePager(pager);
        }
        /// <summary>
        /// 采购订单页面
        /// </summary>
        /// <param name="pager"></param>
        public void GetPmProcurementOrderPager(ref Pager<CBPmProcurementOrder> pager)
        {
            IPmProcurementDao.Instance.GetPmProcurementOrderPager(pager);
        }
        /// <summary>
        /// 获取采购申请单信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public CBPmProcurementOrder GetCBPmProcurementOrder(int SysNo)
        {
            return IPmProcurementDao.Instance.GetCBPmProcurementOrder(SysNo);
        }
        /// <summary>
        /// 其他网上参考价
        /// </summary>
        /// <returns></returns>
        public List<PmProcurementWebType> GetProcurementWebTypeList()
        {
            return IPmProcurementDao.Instance.GetProcurementWebTypeList();
        }

        #endregion
        #region 创建采购分货单
        /// <summary>
        /// 采购订单页面
        /// </summary>
        /// <param name="pager"></param>
        public void GetPmPointsOrderPager(ref Pager<CBPmPointsOrder> pager)
        {
            IPmPointsOrderDao.Instance.GetPmPointsOrderPager(ref pager);
        }
        public void CreateOrUpdatePmPointsOrder(CBPmPointsOrder order)
        {
            int sysNo = 0;
            if (order.SysNo == 0)
            {
                sysNo = IPmPointsOrderDao.Instance.CreatePointsOrder(order);
            }
            else
            {
                sysNo = order.SysNo;
                IPmPointsOrderDao.Instance.UpdatePointsOrder(order);
            }

            foreach (PmPointsOrderItem item in order.listItems)
            {
                item.Poi_PSysNo = sysNo;
                if (item.SysNo == 0)
                {
                    IPmPointsOrderDao.Instance.CreatePointsOrderItem(item);
                    IPmProcurementDao.Instance.UpdatePmProcurementOrderItemStatus(item.Poi_ProcurementItemSysNo,1);
                }
                else
                {
                    IPmPointsOrderDao.Instance.UpdatePointsOrderItem(item);
                }
            }
        }
        public CBPmPointsOrder GetCBPmPointsOrder(int SysNo) 
        {
            return IPmPointsOrderDao.Instance.GetPmPointsOrder(SysNo);
        }

        public List<CBPmPointsOrderItem> GetCBPmPointsOrderItems(int SysNo)
        {
            return IPmPointsOrderDao.Instance.GetPointsOrderItems(SysNo);
        }
        public void DeletePointsOrderData(string delSysNos)
        {
            IPmPointsOrderDao.Instance.DeletePointsOrderData(delSysNos);
        }

        public void UpdatePointOrderStatus(int SysNo, int Status)
        {
            IPmPointsOrderDao.Instance.UpdatePointsOrderStatus(SysNo, Status);
        }

        public List<CBPmPointsOrder> GetPointsOrderListByPSysNo(string pSysNoList)
        {
            return IPmPointsOrderDao.Instance.GetPointsOrderListByPSysNo(pSysNoList);
        }
        #endregion

        #region 创建采购配送单
        /// <summary>
        /// 获取未创建物流配送的订单的信息
        /// </summary>
        /// <param name="pSysNo"></param>
        /// <returns></returns>
        public CBPmGoodsDelivery GetCBPmGoodsDeliveryByPSysNo(int pSysNo)
        {
           return  IPmDeliveryDao.Instance.GetCBPmGoodsDeliveryByPSysNo(pSysNo);
        }

        /// <summary>
        /// 获取未创建物流配送的订单的信息
        /// </summary>
        /// <param name="pSysNo"></param>
        /// <returns></returns>
        public List<CBPmGoodsDeliveryItem> GetPmGoodsDeliveryItemByOrderPSysNo(int pSysNo)
        {
            return IPmDeliveryDao.Instance.GetPmGoodsDeliveryItemByOrderPSysNo(pSysNo);
        }

        /// <summary>
        /// 获取以创建的包裹数据的信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public CBPmGoodsDelivery GetCBPmGoodsDeliveryBySysNo(int SysNo)
        {
            return IPmDeliveryDao.Instance.GetCBPmGoodsDeliveryBySysNo(SysNo);
        }

        public void CreateOrUpdateDeliveryOrder(CBPmGoodsDelivery deliveryMod)
        {
            int sysNo = IPmDeliveryDao.Instance.InnerGoodsDelivery(deliveryMod);
            deliveryMod.SysNo = sysNo;
            foreach (PmGoodsDeliveryItem item in deliveryMod.ListItems)
            {
                item.gdi_PSysNo = sysNo;
                IPmDeliveryDao.Instance.InnerGoodsDeliveryItem(item);
            }
        }

        public void UpdateDeliveryOrder(PmGoodsDelivery deliveryMod)
        {
            IPmDeliveryDao.Instance.UpdateGoodsDelivery(deliveryMod);
        }

        public PmGoodsDelivery GetDeliveryBySysNo(int SysNo)
        {
            return IPmDeliveryDao.Instance.GetGoodsDelivery(SysNo);
        }
        #endregion



        public List<PmGoodsDelivery> GetDeliveryListByPSysNo(string pSysNoList)
        {
            return IPmDeliveryDao.Instance.GetDeliveryListByPSysNo(pSysNoList);
        }

        public List<PmGoodsDeliveryItem> GetPmGoodsDeliveryItemByPSysNo(int SysNo)
        {
            return IPmDeliveryDao.Instance.GetPmGoodsDeliveryItemByPSysNo(SysNo);
        }

        public void GetPmGoodsDeliveryPager(ref Pager<CBPmGoodsDelivery> pager)
        {
             IPmDeliveryDao.Instance.GetPmGoodsDeliveryPager(ref pager);
        }

        public List<CBPmPointsOrder> GetPointsOrderListBySinglePSysNo(int pSysNo)
        {
            return IPmPointsOrderDao.Instance.GetPointsOrderListBySinglePSysNo(pSysNo);
        }

        public CBPmProcurementOrder GetCBPmProcurementOrder(string Number)
        {
            return IPmProcurementDao.Instance.GetCBPmProcurementOrder(Number);
        }

        public List<CBPmProcurementOrderItem> GetPmProcurementOrderItem(int[] pSysNo)
        {
            return IPmProcurementDao.Instance.GetPmProcurementOrderItem(pSysNo);
        }
    }
}
