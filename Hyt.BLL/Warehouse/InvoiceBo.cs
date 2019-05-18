using System;
using System.Collections.Generic;
using System.Linq;
using Hyt.BLL.Order;
using Hyt.DataAccess.Warehouse;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 发票相关业务处理
    /// </summary>
    /// <remarks>2013-07-09 周瑜 创建</remarks>
    public class InvoiceBo : BOBase<InvoiceBo>, IInvoiceBo
    {
        /// <summary>
        /// 订单发票事务
        /// </summary>
        /// <param name="model">发票实体</param>
        /// <param name="user">操作人</param>
        /// <remarks>2013-12-05 周唐炬 创建</remarks>
        public void InvoiceTransaction(FnInvoice model, SyUser user)
        {
            var order = SoOrderBo.Instance.GetByTransactionSysNo(model.TransactionSysNo);
            if (order == null) throw new HytException("未找到订单，请刷新重试！");
            model.InvoiceAmount = order.CashPay;
            model.TransactionSysNo = order.TransactionSysNo;
            //已经开具过发票
            if (model.SysNo > 0)
            {
                //如果订单已存在开票数据，并且跟本次发票号不匹配，不能在新开发票
                if (order.InvoiceSysNo > 0 && order.InvoiceSysNo != model.SysNo) throw new HytException("该订单已经开具过发票，请搜索订单号重新开具！");
                if (order.Status == OrderStatus.销售单状态.作废.GetHashCode())
                {
                    throw new HytException("该订单已经作废，不能开票!");
                }
                SoOrderBo.Instance.UpdateOrderInvoice(model);
            }
            else //新开
            {
                model.CreatedBy = user.SysNo;
                model.CreatedDate = DateTime.Now;
                var newInvoiceSysNo = SoOrderBo.Instance.InsertOrderInvoice(model);
                if (newInvoiceSysNo > 0)
                {
                    //发票信息更新到订单
                    SoOrderBo.Instance.UpdateOrderInvoice(order.SysNo, newInvoiceSysNo);

                    //更新应收金额大于0 的出库单的发票系统编号
                    var list = WhWarehouseBo.Instance.GetModelByTransactionSysNo(order.TransactionSysNo);

                    var master = list.Where(m => m.Receivable > 0).ToList();
                    if (master.Any())
                    {
                        master.ForEach(x =>
                        {
                            x.InvoiceSysNo = newInvoiceSysNo;
                            WhWarehouseBo.Instance.UpdateStockOut(x);
                        });
                    }
                }
            }
        }
        /// <summary>
        ///     根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        /// <remarks>2013-11-14 周唐炬 重构</remarks>
        public PagedList<CBFnInvoice> Search(InvoiceSearchCondition condition)
        {
            PagedList<CBFnInvoice> model = null;
            if (condition != null)
            {
                model = new PagedList<CBFnInvoice>();
                var result = IInvoiceDao.Instance.Search(condition, model.PageSize);
                if (result != null)
                {
                    model.TData = result.Rows;
                    model.TotalItemCount = result.TotalRows;
                    model.CurrentPageIndex = condition.CurrentPage;
                }
            }
            return model;
        }

        /// <summary>
        ///     根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        /// <remarks>2013-11-14 周唐炬 重构</remarks>
        public PagedList<SoOrder> QuickSearch(InvoiceSearchCondition condition)
        {
            PagedList<SoOrder> model = null;
            if (condition != null)
            {
                model = new PagedList<SoOrder>();
                var result = IInvoiceDao.Instance.QuickSearch(condition, model.PageSize);
                if (result != null)
                {
                    model.TData = result.Rows;
                    model.TotalItemCount = result.TotalRows;
                    model.CurrentPageIndex = condition.CurrentPage;
                }
            }
            return model;
        }
        /// <summary>
        /// 根据主键获取发票
        /// </summary>
        /// <param name="outStockSysNo">发票系统编号.</param>
        /// <returns>发票实体</returns>
        /// <remarks>2013-07-10 周瑜 创建 </remarks>
        public FnInvoice GetModel(int outStockSysNo)
        {
            return IInvoiceDao.Instance.GetModel(outStockSysNo);
        }
        /// <summary>
        /// 根据事务获取发票
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>发票实体</returns>
        /// <remarks>2013-11-12 周唐炬 创建</remarks>
        public FnInvoice GetInvoiceByTransactionSysNo(string transactionSysNo)
        {
            return IInvoiceDao.Instance.GetInvoiceByTransactionSysNo(transactionSysNo);
        }
        #region 不在需要作废逻辑
        ///// <summary>
        ///// 作废发票
        ///// </summary>
        ///// <param name="sysno">发票系统编号</param>
        ///// <returns>成功返回true,失败返回false</returns>
        ///// <remarks>2013-07-10 周瑜 创建</remarks>
        //public bool CancelInvoice(int sysno)
        //{
        //    int result = 0;
        //    var queryResult = GetModel(sysno);
        //    if (null != queryResult)
        //    {
        //        queryResult.Status = -10;
        //        queryResult.LastUpdateBy = 52;//todo: change to login user id
        //        queryResult.LastUpdateDate = DateTime.Now;
        //        result = IInvoiceDao.Instance.Update(queryResult);
        //    }

        //    return result > 0;
        //}
        #endregion
    }
}
