using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Order;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 推送返回信息
    /// </summary>
    /// <remarks>
    /// 2015-09-05 王耀发 创建
    /// </remarks>
    public class OutboundReturnBo : BOBase<OutboundReturnBo>
    {

        #region 推送返回信息
        /// <summary>
        /// 取推送返回信息访问类
        /// </summary>
        /// <remarks>
        /// 2015-08-26 王耀发 创建
        public Pager<OutboundReturn> GetOutboundReturnList(ParaOutboundReturnFilter filter)
        {
            return IOutboundReturnDao.Instance.GetOutboundReturnList(filter);
        }
        public Result InsertOutboundReturn(OutboundReturn model, int user)
        {
            Result r = new Result();
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = user;
            model.LastUpdateBy = user;
            model.LastUpdateDate = DateTime.Now;
            Hyt.DataAccess.Order.IOutboundReturnDao.Instance.InsertOutboundReturn(model);
            r.StatusCode = model.SysNo;
            r.Status = true;
            return r;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OutboundOrderNo"></param>
        /// <returns>2015-09-18 王耀发 创建</returns>
        public OutboundReturn GetEntityByOutboundOrderNo(string OutboundOrderNo)
        {
            return Hyt.DataAccess.Order.IOutboundReturnDao.Instance.GetEntityByOutboundOrderNo(OutboundOrderNo);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OutboundOrderNo"></param>
        /// <returns>2015-09-18 王耀发 创建</returns>
        public SoReceiveAddress GetSoReceiveAddressBysoOrderSysNo(int soOrderSysNo)
        {
            return Hyt.DataAccess.Order.IOutboundReturnDao.Instance.GetSoReceiveAddressBysoOrderSysNo(soOrderSysNo);
        }
        #endregion
    }
}
