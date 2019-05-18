using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Hyt.BLL.Authentication;
using Hyt.BLL.CRM;
using Hyt.BLL.Logistics;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Warehouse;
using Hyt.Util.Validator;
using Hyt.DataAccess.RMA;

namespace Hyt.BLL.RMA
{
    /// <summary>
    /// 退款业务
    /// </summary>
    /// <remarks>2016-08-29 罗远康 创建</remarks>
    public class RefundReturnDao : BOBase<RefundReturnDao>
    {
        /// <summary>
        /// 更新退款数据
        /// </summary>
        /// <param name="entity">退换货实体</param>
        /// <returns></returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        /// 
        public void Update(RcRefundReturn entity)
        {
            IRcRefundReturnDao.Instance.Update(entity);
        }

        /// <summary>
        /// 获取退款单实体
        /// </summary>
        /// <param name="sysNo">退换货单编号</param>
        /// <returns>退换货单实体</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public RcRefundReturn GetEntity(int sysNo)
        {
            return IRcRefundReturnDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 根据订单获取退款单实体（非作废）
        /// </summary>
        /// <param name="OrderSysNo">订单编号</param>
        /// <returns>退换货单实体</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public RcRefundReturn GetOrderEntity(int OrderSysNo)
        {
            return IRcRefundReturnDao.Instance.GetOrderEntity(OrderSysNo);
        }
        /// <summary>
        /// 退款单分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>退换货表</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public Pager<RcRefundReturn> GetAll(ParaRefundFilter filter)
        {
            var pager = IRcRefundReturnDao.Instance.GetAll(filter);
            return pager;
        }

        /// <summary>
        /// 付款单付款后,更新退换货信息
        /// </summary>
        /// <param name="sysNo">付款单号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2016-08-29 罗远康 创建</remarks>
        public void PaymentCompleteCallBack(int sysNo, SyUser user)
        {
            var pay = BLL.Finance.FinanceBo.Instance.GetPaymentVoucher(sysNo);
            if (pay != null && pay.Source == (int)FinanceStatus.付款来源类型.销售单)
            {
                RcRefundReturn Refund = GetOrderEntity(pay.SourceSysNo);//获取退款详情
                if (Refund != null)
                {
                    Refund.Status = 50;//改为已完成
                    Refund.RefundBy = user.SysNo;//付款人编号
                    Refund.CancelDate = DateTime.Now;
                    Refund.LastUpdateDate = DateTime.Now;
                    Update(Refund);
                }
            }
        }
    }
}
