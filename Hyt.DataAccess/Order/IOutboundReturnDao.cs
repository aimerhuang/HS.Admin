using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 取推送返回信息访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    public abstract class IOutboundReturnDao : Hyt.DataAccess.Base.DaoBase<IOutboundReturnDao>
    {
        /// <summary>
        /// 一号仓包裹
        /// </summary>
        /// <param name="filter">包裹返回信息</param>
        /// <returns>包裹返回信息</returns>
        /// <remarks>2015-09-22 王耀发 创建</remarks>
        public abstract Pager<OutboundReturn> GetOutboundReturnList(ParaOutboundReturnFilter filter);
        /// <summary>
        /// 插进推送运单返回值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>2015-09-18 王耀发 创建</returns>
        public abstract int InsertOutboundReturn(OutboundReturn entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OutboundOrderNo"></param>
        /// <returns>2015-09-18 王耀发 创建</returns>
        public abstract OutboundReturn GetEntityByOutboundOrderNo(string OutboundOrderNo);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OutboundOrderNo"></param>
        /// <returns>2015-09-18 王耀发 创建</returns>
        public abstract SoReceiveAddress GetSoReceiveAddressBysoOrderSysNo(int soOrderSysNo);
    }
}

