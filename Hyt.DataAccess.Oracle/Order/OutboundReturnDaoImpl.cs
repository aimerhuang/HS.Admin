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
    public class OutboundReturnDaoImpl : IOutboundReturnDao
    {
        /// <summary>
        /// 一号仓包裹
        /// </summary>
        /// <param name="filter">包裹返回信息</param>
        /// <returns>包裹返回信息</returns>
        /// <remarks>2015-09-22 王耀发 创建</remarks>
        public override Pager<OutboundReturn> GetOutboundReturnList(ParaOutboundReturnFilter filter)
        {
            const string sql = @"(select a.* from OutboundReturn a 
                    where          
                    (@0 is null or charindex(a.OutboundOrderNo,@1)>0) and 
                    Code = '0') tb";

            var dataList = Context.Select<OutboundReturn>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.OutboundOrderNo,filter.OutboundOrderNo
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<OutboundReturn>
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
        /// 插进推送运单返回值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>2015-09-18 王耀发 创建</returns>
        public override int InsertOutboundReturn(OutboundReturn entity)
        {
            entity.SysNo = Context.Insert("OutboundReturn", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OutboundOrderNo"></param>
        /// <returns>2015-09-18 王耀发 创建</returns>
        public override OutboundReturn GetEntityByOutboundOrderNo(string OutboundOrderNo)
        {

            return Context.Sql("select a.* from OutboundReturn a where a.OutboundOrderNo=@OutboundOrderNo")
                   .Parameter("OutboundOrderNo", OutboundOrderNo)
              .QuerySingle<OutboundReturn>();
        }
        /// <summary>
        /// 获取电话
        /// </summary>
        /// <param name="soOrderSysNo"></param>
        /// <returns>2015-09-18 王耀发 创建</returns>
        public override SoReceiveAddress GetSoReceiveAddressBysoOrderSysNo(int soOrderSysNo)
        {

            return Context.Sql("select b.* from soOrder a left join SoReceiveAddress b on a.ReceiveAddressSysNo = b.sysno where a.SysNo=@SysNo")
                   .Parameter("SysNo", soOrderSysNo)
              .QuerySingle<SoReceiveAddress>();
        }
    }
}
