using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.SellBusiness
{
    /// <summary>
    /// 会员提现数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-09-15 王耀发 创建
    /// </remarks>
    public class CrPredepositCashDaoImpl : ICrPredepositCashDao
    {
        
        /// <summary>
        /// 会员提现信息
        /// </summary>
        /// <param name="filter">会员提现信息</param>
        /// <returns>返回会员提现信息</returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        public override Pager<CrPredepositCash> GetCrPredepositCashList(ParaCrPredepositCashFilter filter)
        {

            string sql = @"(select a.* from CrPredepositCash a 
                    where          
                    (@0 is null or charindex(a.PdcTradeNo,@1)>0) and 
                    (@2 is null or charindex(a.PdcTradeNo,@3)>0)  "+ (string.IsNullOrWhiteSpace(filter.SysNoList)?"":"  and a.sysNo in("+filter.SysNoList+") ")+@"
                                   ) tb";

            var dataList = Context.Select<CrPredepositCash>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.PdcTradeNo,filter.PdcTradeNo,
                    filter.PdcUserName,filter.PdcUserName
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CrPredepositCash>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.PdcAddTime desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }

        /// <summary>
        /// 更新支付状态
        /// </summary>
        /// <param name="SysNo">会员提现单编号</param>
        /// <param name="PdcPayState">状态值</param>
        /// <returns></returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        public override void UpdatePdcPayState(int SysNo, int PdcPayState)
        {
            Context.Sql("update CrPredepositCash set PdcPayState=@PdcPayState where SysNo=@SysNo")
                   .Parameter("PdcPayState", PdcPayState)
                   .Parameter("SysNo", SysNo).Execute();
        }
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2016-1-6 王耀发 创建</remarks>
        public override int Insert(CrPredepositCash entity)
        {
            entity.SysNo = Context.Insert("CrPredepositCash", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新提现订单审核状态
        /// </summary>
        /// <param name="sysNo">提现订单系统编号</param>
        /// <param name="status">审核状态（0:未审核 1:已审核 -1 作废）</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        public override bool UpdateStatus(int sysNo, int status)
        {
           return  Context.Sql("update CrPredepositCash set status=@status where SysNo=@sysNo")
                  .Parameter("status", status)
                  .Parameter("sysNo", sysNo).Execute()>0;
        }

        /// <summary>
        /// 获取提现订单详情
        /// </summary>
        /// <param name="sysNo">提现订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        public override CrPredepositCash GetModel(int sysNo)
        {
           return Context.Sql(string.Format("select * from CrPredepositCash where sysNo={0}", sysNo))
                .QuerySingle<CrPredepositCash>();
        }
    }
}
