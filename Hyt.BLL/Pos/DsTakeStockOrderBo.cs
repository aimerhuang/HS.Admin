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
    public class DsTakeStockOrderBo : BOBase<DsTakeStockOrderBo>
    {
        public int Insert(DsTakeStockOrder mod)
        {
            return IDsTakeStockOrderDao.Instance.Insert(mod);
        }
        public void InsertItem(DsTakeStockItem item)
        {
                IDsTakeStockOrderDao.Instance.InsertItem(item);
        }

        public object GetTakeStockOrderListPagerByDsSysNo(int? pageIndex, int dsSysNo, DateTime? datetime, int? ProfitLoss)
        {
            var returnValue = new PagedList<CBDsTakeStockOrder>();

            var pager = new Pager<CBDsTakeStockOrder>
            {
                PageFilter = new CBDsTakeStockOrder
                {
                    DsSysNo = dsSysNo,
                    DateTime = datetime,
                    ProfitLoss = ProfitLoss
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            IDsTakeStockOrderDao.Instance.GetTakeStockOrderListPagerByDsSysNo(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        public DsTakeStockOrder GetTakeStockOrder(int SysNo)
        {
            return IDsTakeStockOrderDao.Instance.GetTakeStockOrder(SysNo);
        }

        public object GetTakeStockOrderItems(int SysNo)
        {
            return IDsTakeStockOrderDao.Instance.GetTakeStockOrderItems(SysNo);
        }
        public List<CBDsTakeStockOrder> GetTakeStockOrderList(int? dsSysNo, DateTime? datetime)
        {
            return IDsTakeStockOrderDao.Instance.GetTakeStockOrderList(dsSysNo, datetime);
        }
        public List<DsTakeStockItem> GettTakeStockItems(List<int> SysNos)
        {
            return IDsTakeStockOrderDao.Instance.GettTakeStockItems(SysNos);
        }
    }
}
