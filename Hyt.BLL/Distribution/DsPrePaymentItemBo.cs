using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Distribution;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商预存款往来账明细DsPrePaymentItemBo
    /// </summary>
    /// <remarks>2013-09-11 周唐炬 创建</remarks>
    public class DsPrePaymentItemBo : BOBase<DsPrePaymentItemBo>
    {
        /// <summary>
        /// 根据来源信息获取到期返利的预存款明细
        /// </summary>
        /// <param name="source">来源</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="delayDay">到期天数</param>
        /// <returns></returns>
        /// <remarks>2015-1-8 杨浩 创建</remarks>
        public List<DsPrePaymentItem> GetExpireListBySource(int source, int delayDay, int dealerSysNo = 0, int orderSysNo=0)
        {
            return IDsPrePaymentItemDao.Instance.GetExpireListBySource(source, delayDay, dealerSysNo, orderSysNo);
        }
        /// <summary>
        /// 更新预存款明细状态
        /// </summary>
        /// <param name="sourceSysNo">来源单据</param>
        /// <param name="source">来源编号</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        /// <remarks>2016-1-7 杨浩 创建</remarks>
        public int UpdatePrePaymentItemStatus(int sourceSysNo, int source, int status)
        {
            return IDsPrePaymentItemDao.Instance.UpdatePrePaymentItemStatus(sourceSysNo, source, status);
        }
        /// <summary>
        /// 创建分销商预存款往来账明细
        /// </summary>
        /// <param name="model">分销商预存款往来账明细实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-09-11 周唐炬 创建</remarks>
        public int Create(DsPrePaymentItem model)
        {
            return IDsPrePaymentItemDao.Instance.Insert(model);
        }

        /// <summary>
        /// 获取分销商预存款往来账明细
        /// </summary>
        /// <param name="sysNo">分销商预存款往来账明细系统编号</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-09-11 周唐炬 创建</remarks>
        public CBDsPrePaymentItem GetDsPrePaymentItem(int sysNo)
        {
            return (CBDsPrePaymentItem)IDsPrePaymentItemDao.Instance.GetEntity(sysNo);
        }
        /// <summary>
        /// 获取明细记录
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public DsPrePaymentItem GetEntity(int sysNo)
        {
            return IDsPrePaymentItemDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 创建分销商预存款往来账明细
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>分销商预存款往来账明细列表</returns>
        /// <remarks>2013-09-11 周唐炬 创建</remarks>
        public PagedList<CBDsPrePaymentItem> GetDsPrePaymentItemList(ParaDealerFilter filter)
        {
            if (filter != null)
            {
                var model = new PagedList<CBDsPrePaymentItem>();
                filter.PageSize = model.PageSize;
                var pager = IDsPrePaymentItemDao.Instance.GetDsPrePaymentItemList(filter);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.CurrentPage;
                }
                return model;
            }
            return null;
        }
        /// <summary>
        /// 更新付款单明细状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="SendStatus"></param>
        /// <remarks>2015-09-17 王耀发  创建</remarks>
        public void UpdatePaymentItemStatus(int SysNo, int Status)
        {
            IDsPrePaymentItemDao.Instance.UpdatePaymentItemStatus(SysNo, Status);
        }
    }
}
