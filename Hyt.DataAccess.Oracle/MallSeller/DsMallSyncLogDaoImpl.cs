using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.MallSeller
{
    public class DsMallSyncLogDaoImpl : IDsMallSyncLogDao
    {
        /// <summary>
        /// 创建升舩商城同步日志表
        /// </summary>
        /// <param name="model">升舩商城同步日志表</param>
        /// <returns>
        /// 创建的升舩商城同步日志表sysNo
        /// </returns>
        /// <remarks>
        /// 2014-07-28 杨文兵 创建
        /// </remarks>
        public override int Create(Model.Generated.DsMallSyncLog model)
        {
            var sysNo = Context.Insert("DsMallSyncLog", model)
                            .AutoMap(o => o.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }


        #region 获取日志中需要同步的订单
        /// <summary>
        /// 获取日志中需要同步的订单
        /// </summary>
        /// <returns>订单日志</returns>
        /// 吴琨 2017-8-30 创建
        public override List<DsMallSyncLog> GetSynchroOrder(int sysno = 0)
        {
            string whereStr = "";
            if (sysno > 0)
                whereStr = "  and sysno="+sysno;

            return Context.Sql("select * from dsMallSyncLog where SyncType=10 and Status=20 and SyncNumber<200  " + whereStr + " order by SysNo desc ")
                .QueryMany<DsMallSyncLog>(); ;
        }


        /// <summary>
        /// 获取商城订单号和商城类型
        /// </summary>
        /// <returns>商城订单号</returns>
        /// 吴琨 2017-8-30 创建
        public override SynchroMallSyncLog GetOrder(string OrderTransactionSysNo)
        {
            return Context.Sql("select a.MallOrderId as 商城订单号,b.MallTypeSysNo as 商城编号  from DsOrder a inner join DsDealerMall b  on a.DealerMallSysNo=b.SysNo where a.OrderTransactionSysNo=@OrderTransactionSysNo")
                .Parameter("OrderTransactionSysNo", OrderTransactionSysNo)
                .QuerySingle<SynchroMallSyncLog>(); ;
        }

        /// <summary>
        /// 获取物流公司名称
        /// </summary>
        /// <returns>商城订单号</returns>
        /// 吴琨 2017-8-30 创建
        public override string GetDeliveryName(int DeliveryType)
        {
            return Context.Sql("select top 1 DeliveryTypeName from LgDeliveryType where SysNo=@SysNo  ").Parameter("SysNo", DeliveryType).QuerySingle<string>(); ;
        }


        /// <summary>
        /// 更新同步日志状态
        /// </summary>
        /// <param name="Message">修改状态备注</param>
        /// <param name="Status">状态  等待(0),成功(10),失败(20),作废(-10)</param>
        /// <param name="SysNo">修改的系统编号</param>
        /// <param name="elapsedMilliseconds">执行接口时间</param>
        /// <returns></returns>
        /// 吴琨 2017-8-31 创建
        public override bool UpdateDsMallSyncLogStatus(string Message, int Status, int SysNo, int elapsedMilliseconds)
        {
            return Context.Sql(@"update DsMallSyncLog set Message=@Message,SyncNumber=SyncNumber+1,LastSyncTime=@LastSyncTime,Status=@Status,LastUpdateDate=@LastUpdateDate,ElapsedTime=@elapsedTime where SysNo=@SysNo ")
                .Parameter("Message", Message)
                .Parameter("Status", Status)
                .Parameter("SysNo", SysNo)
                .Parameter("LastSyncTime", DateTime.Now)
                .Parameter("LastUpdateDate",DateTime.Now)
                .Parameter("elapsedTime", elapsedMilliseconds)
                .Execute()>0;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>日志分页</returns>
        /// <remarks>2017-11-1 杨浩 创建</remarks>
        public override Pager<CBDsMallSyncLog> GetList(ParaDsMallSyncLogFilter filter)
        {
          
            #region 条件

            string sql =
                @"(select a.*
                           from DsMallSyncLog a                                                                              
                    where    1=1 {0} ) tb ";
            string where = string.Empty;
            var paras = new List<string>();
            if (filter.LastsyncBeginTime != null)
            {
                where += " and tb.LastSyncTime>=@"+paras.Count;
                paras.Add(filter.LastsyncBeginTime.ToString());
            }
            if (filter.LastsyncEndDate != null)
            {
                where += " and a.LastSyncTime<=@" + paras.Count;
                paras.Add(filter.LastsyncEndDate.ToString());
            }
            if (!string.IsNullOrWhiteSpace(filter.OrderId))
            {
               
                if (filter.Name == 10)
                {
                    int orderSysno = 0;
                    if (int.TryParse(filter.OrderId, out orderSysno))
                        where += " and a.Data like '{\"TransactionSysNo\":\"T%" +int.Parse(filter.OrderId)+ "\"%}'";                
                }
                else
                {
                    where += " and a.Data like @" + paras.Count;
                    paras.Add("%"+filter.OrderId+"%");
                }
            }
            if (filter.Status!=0)
            {
                where += " and a.Status=@" + paras.Count;
                paras.Add(filter.Status.ToString());
            }

            if (filter.Name > 0)
            {
                where += " and a.SyncType=@" + paras.Count;
                paras.Add(filter.Name.ToString());
            }
            sql = string.Format(sql, where);

            #endregion

            var dataList = Context.Select<CBDsMallSyncLog>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras.ToArray());
            dataCount.Parameters(paras.ToArray());

            var pager = new Pager<CBDsMallSyncLog>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            pager.Rows = dataList.OrderBy(" tb.sysno asc")
                                 .Paging(pager.CurrentPage, filter.PageSize)
                                 .QueryMany();

            return pager;
        }
        #endregion
    }
}
