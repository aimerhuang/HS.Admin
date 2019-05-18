using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Order;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 取推送返回信息访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class SendOrderReturnDaoImpl : ISendOrderReturnDao
    {
        /// <summary>
        /// 推送返回信息
        /// </summary>
        /// <param name="filter">推送返回信息</param>
        /// <returns>返回推送返回信息</returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        public override Pager<SendOrderReturn> GetSendOrderReturnList(ParaSendOrderReturnFilter filter)
        {
            const string sql = @"(select a.* from SendOrderReturn a 
                    where          
                    (@0 is null or charindex(a.OrderNo,@1)>0) and 
                    (@2 is null or charindex(a.Msg,@3)>0) 
                                   ) tb";

            var dataList = Context.Select<SendOrderReturn>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.OrderNo,filter.OrderNo,
                    filter.Msg,filter.Msg
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<SendOrderReturn>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        /// <summary>
        /// 根据快递公司编号，快递单号编号返回记录
        /// </summary>
        /// <param name="OverseaCarrier"></param>
        /// <param name="OverseaTrackingNo"></param>
        /// <returns></returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        public override SendOrderReturn GetEntityByOversea(string OverseaCarrier, string OverseaTrackingNo)
        {

            return Context.Sql("select a.* from SendOrderReturn a where a.Code = '0' and a.OverseaCarrier=@OverseaCarrier and a.OverseaTrackingNo=@OverseaTrackingNo")
                   .Parameter("OverseaCarrier", OverseaCarrier)
                   .Parameter("OverseaTrackingNo", OverseaTrackingNo)
              .QuerySingle<SendOrderReturn>();
        }

        /// <summary>
        /// 根据包裹单号返回记录
        /// </summary>
        /// <param name="OverseaCarrier"></param>
        /// <param name="OverseaTrackingNo"></param>
        /// <returns></returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        public override SendOrderReturn GetEntityByOTrackingNo(string OverseaTrackingNo)
        {

            return Context.Sql("select a.* from SendOrderReturn a where a.Code = '0' and a.OverseaTrackingNo=@OverseaTrackingNo")
                   .Parameter("OverseaTrackingNo", OverseaTrackingNo)
              .QuerySingle<SendOrderReturn>();
        }

        /// <summary>
        /// 根据订单系统编号获取推送记录
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <remarks> 2016-5-11 刘伟豪 创建 </remarks>
        public override SendOrderReturn GetEntityByOrderSysNo(int orderSysNo)
        {
            return Context.Sql("select * from SendOrderReturn where Code = '1' and soOrderSysNo=@0", orderSysNo)
              .QuerySingle<SendOrderReturn>();
        }
    }
}
