using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.RMA;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.RMA
{
    /// <summary>
    /// 退款
    /// </summary>
    public class ReRefundReturnDaoImpl : IRcRefundReturnDao
    {
        /// <summary>
        /// 更新退款数据
        /// </summary>
        /// <param name="entity">退换货实体</param>
        /// <returns></returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public override void Update(Model.RcRefundReturn entity)
        {
            Context.Update("RcRefundReturn", entity)
                .AutoMap(o => o.SysNo)
                .Where("SysNo", entity.SysNo).Execute();
        }

        /// <summary>
        /// 根据订单获取退款单实体（已经审核待付款）
        /// </summary>
        /// <param name="OrderSysNo">订单编号</param>
        /// <returns>退换货单实体</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public override RcRefundReturn GetOrderEntity(int OrderSysNo)
        {
            return Context.Sql("select * from RcRefundReturn where OrderSysNo=@OrderSysNo AND Status<>-10")
                 .Parameter("OrderSysNo", OrderSysNo).QuerySingle<RcRefundReturn>();
        }

        /// <summary>
        /// 获取退款单实体
        /// </summary>
        /// <param name="sysNo">退换货单编号</param>
        /// <returns>退换货单实体</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public override RcRefundReturn GetEntity(int sysNo)
        {
            return Context.Sql("select * from RcRefundReturn where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo).QuerySingle<RcRefundReturn>();
        }
        /// <summary>
        /// 退款单分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>退换货表</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public override Pager<RcRefundReturn> GetAll(ParaRefundFilter filter)
        {
            const string sql =
                 @"(SELECT  * FROM RcRefundReturn RC
                 WHERE   
                     (@0 is null or RC.CustomerSysNo=@0) and                --会员号
                     (@1 is null or RC.OrderSysNo=@1) and                   --订单编号
                     (@2 is null or RC.Source=@2) and      --申请单来源
                     (@3 is null or RC.Status=@3) and          --退款状态
                     (@4 is null or RC.createDate>=@4) and       --创建日期(起)
                     (@5 is null or RC.createDate<@5) and          --创建日期(止) 
                     (@6 is null or RC.SysNo=@6) and               --退款单编号 
                     (@7 is null or RC.HandleDepartment=@7)             --申请单处理部门
                    ) tb";
            //查询日期上限+1
            filter.EndDate = filter.EndDate == null ? (DateTime?)null : filter.EndDate.Value.AddDays(1);

            var paras = new object[]
                {
                    filter.CustomerSysNo,    //会员编号
                    filter.OrderSysNo,        //订单编号,
                    filter.Source,           //申请单来源
                    filter.Status,           //退款状态
                    filter.BeginDate,        //创建日期(起)
                    filter.EndDate,          //创建日期(止)
                    filter.SysNo,            //退款单编号
                    filter.HandleDepartments  //申请单处理部门
                };

            var dataList = Context.Select<RcRefundReturn>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<RcRefundReturn>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            pager.Rows = dataList.OrderBy("tb.createdate desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();

            return pager;

        }
    }
}
