using System.Collections.Generic;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Warehouse
{
    public interface IInvoiceBo
    {
        /// <summary>
        ///  根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        PagedList<CBFnInvoice> Search(InvoiceSearchCondition condition);

        /// <summary>
        /// 根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        /// <remarks>2013-11-14 周唐炬 重构</remarks>
        PagedList<SoOrder> QuickSearch(InvoiceSearchCondition condition);
    }
}
