using Hyt.DataAccess.Pos;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    public class DsPosReturnOrderBo : BOBase<DsPosReturnOrderBo>
    {
        public int Insert(DsPosReturnOrder rOrder)
        {
            return IDsPosReturnOrderDao.Instance.Insert(rOrder);
        }
        public int InsertItem(DsPosReturnOrderItem rOrderItem)
        {
            return IDsPosReturnOrderDao.Instance.InsertItem(rOrderItem);
        }

        public void Update(DsPosReturnOrder rOrder)
        {
            IDsPosReturnOrderDao.Instance.Update(rOrder);
        }
        public void UpdateItem(DsPosReturnOrderItem rOrderItem)
        {
            IDsPosReturnOrderDao.Instance.UpdateItem(rOrderItem);
        }

        public DsPosReturnOrder GetEntity(int SysNo)
        {
            return IDsPosReturnOrderDao.Instance.GetEntity(SysNo);
        }
        public List<CBDsPosReturnOrderItem> GetEntityItemList(int pSysNo)
        { 
            return IDsPosReturnOrderDao.Instance.GetEntityItemList(pSysNo);
        }

        public List<DsPosReturnOrder> GetPosReturnOrderList(int posSysNo, DateTime startday, DateTime endday, int DsSysNo)
        {
            return IDsPosReturnOrderDao.Instance.GetPosReturnOrderList(posSysNo, startday,endday, DsSysNo);
        }

        public PagedList<CBDsPosReturnOrder> GetPosReturnOrderListPagerByDsSysNo(int? pageIndex, int dsSysNo, string PosName="")
        {
            var returnValue = new PagedList<CBDsPosReturnOrder>();

            var pager = new Pager<CBDsPosReturnOrder>
            {
                PageFilter = new CBDsPosReturnOrder
                {
                      DsSysNo  = dsSysNo,
                      PosName = PosName
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            IDsPosReturnOrderDao.Instance.GetPosReturnOrderListPagerByDsSysNo(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        public CBDsPosReturnOrder GetCBPosReturnOrder(int SysNo)
        {
            return IDsPosReturnOrderDao.Instance.GetCBPosReturnOrder(SysNo);
        }

        public List<DsPosReturnOrder> GetAllReturnOrder()
        {
            return IDsPosReturnOrderDao.Instance.GetAllReturnOrder();
        }
        public List<DsPosReturnOrderItem> GetAllReturnOrderItem()
        {
            return IDsPosReturnOrderDao.Instance.GetAllReturnOrderItem();
        }
    }
}
