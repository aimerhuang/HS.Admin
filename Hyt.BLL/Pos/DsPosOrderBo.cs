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
    public class DsPosOrderBo : BOBase<DsPosOrderBo>
    {
        public int Insert(DsPosOrder order) 
        {
            return IDsPosOrderDao.Instance.Insert(order);
        }
        public void Update(DsPosOrder order) 
        {
            IDsPosOrderDao.Instance.Update(order);
        }

        public int InsertItem(DsPosOrderItem item) 
        {
            return IDsPosOrderDao.Instance.InsertItem(item);
        }
        public void UpdateItem(DsPosOrderItem item) 
        {
            IDsPosOrderDao.Instance.UpdateItem(item);
        }
        public void DeletePosOrder(int SysNo)
        {
            IDsPosOrderDao.Instance.DeleteDsPosOrder(SysNo);
        }
        public DsPosOrder GetEntity(int sysNo) 
        {
            return IDsPosOrderDao.Instance.GetEntity(sysNo);
        }
        public List<DsPosOrderItem> GetEntityItems(int pSysNo) 
        {
            return IDsPosOrderDao.Instance.GetEntityItems(pSysNo);
        }
        public List<DsPosOrder> GetList(int dsSysNo) 
        {
            return IDsPosOrderDao.Instance.GetList(dsSysNo);
        }
        public List<DBDsPosOrder> GetDBModList(int dsSysNo, DateTime? BeginDate, DateTime? EndDate)
        {
            return IDsPosOrderDao.Instance.GetDBModList(dsSysNo, BeginDate, EndDate);
        }
        public List<DBDsPosOrder> GetPosOrderList(int posSysNo, DateTime startday, DateTime endday, int DsSysNo)
        {
            return IDsPosOrderDao.Instance.GetPosOrderList(posSysNo, startday,endday, DsSysNo);
        }

        /// <summary>
        /// 获取销售单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="dsSysNo"></param>
        /// <returns></returns>
        public PagedList<DBDsPosOrder> GetPosOrderListPagerByDsSysNo(int? pageIndex, int dsSysNo, 
            DateTime? BeginDate, DateTime? EndDate)
        {
            var returnValue = new PagedList<DBDsPosOrder>();

            var pager = new Pager<DBDsPosOrder>
            {
                PageFilter = new DBDsPosOrder
                {
                     DsSysNo=dsSysNo,
                     BeginDate = BeginDate,
                     EndDate = EndDate
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            IDsPosOrderDao.Instance.GetPosOrderListPagerByDsSysNo(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }
        /// <summary>
        /// 获取销售单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="dsSysNo"></param>
        /// <returns></returns>
        public PagedList<DBDsPosOrder> GetPosOrderListPagerByDsSysNo(int? pageIndex, int dsSysNo, int dsPosSysNo,
            DateTime? BeginDate, DateTime? EndDate)
        {
            var returnValue = new PagedList<DBDsPosOrder>();

            var pager = new Pager<DBDsPosOrder>
            {
                PageFilter = new DBDsPosOrder
                {
                    DsSysNo = dsSysNo,
                    DsPosSysNo = dsPosSysNo,
                    BeginDate = BeginDate,
                    EndDate = EndDate
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            IDsPosOrderDao.Instance.GetPosOrderListPagerByDsSysNo(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }
        public DsPosOrder GetEntityBySellNumber(int dsSysNo, string sellNumber)
        {
            return IDsPosOrderDao.Instance.GetEntityBySellNumber(dsSysNo,sellNumber);
        }

        public List<DsPosOrderItem> GetEntityItems(List<int> soSysNoList)
        {
            return IDsPosOrderDao.Instance.GetEntityItems(soSysNoList);
        }



        public List<DsPosOrder> GetEntityBySellNumbers(int dsSysNo, List<string> orderNos)
        {
            return IDsPosOrderDao.Instance.GetEntityBySellNumbers(dsSysNo, orderNos);
        }

        public List<DsPosOrder> GetRepeatOrderList()
        {
            return IDsPosOrderDao.Instance.GetRepeatOrderList();
        }

        public void DeleteRepeatBySysNo(int SysNo)
        {
            IDsPosOrderDao.Instance.DeleteRepeatBySysNo(SysNo);
        }

        public List<DBDsPosOrderItemData> GetPosOrderListByNoBindSale()
        {
            return IDsPosOrderDao.Instance.GetPosOrderListByNoBindSale();
        }

        public List<WareDsPosOrder> GetPosOrderListByDate(DateTime dateTime)
        {
            return IDsPosOrderDao.Instance.GetPosOrderListByDate(dateTime);
        }
    }
}
