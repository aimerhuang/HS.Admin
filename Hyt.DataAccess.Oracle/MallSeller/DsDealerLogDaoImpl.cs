using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.MallSeller
{
    /// <summary>
    /// 分销商升舱错误日志
    /// </summary>
    /// <remarks>2014-03-31 唐文均 创建</remarks>
    public class DsDealerLogDaoImpl : IDsDealerLog
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2014-03-31 唐文均 创建</remarks>
        public override int Insert(DsDealerLog entity)
        {
            var sysNo = Context.Insert("DsDealerLog", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>null</returns>
        /// <remarks>2014-03-31 唐文均 创建</remarks>
        public override void Update(DsDealerLog entity)
        {

            Context.Update("DsDealerLog", entity)
                   .AutoMap(o => o.SysNo, o => o.CreatedBy, o => o.CreatedDate)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-03-31 唐文均 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("DsDealerLog")
                   .Where("SysNo", sysNo)
                   .Execute();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-03-31 朱家宏 创建</remarks>
        public override Pager<DsDealerLog> Query(ParaDsDealerLogFilter filter)
        {
            string sql = @"DsDealerLog a where {0} ";

            #region 构造sql

            var paras = new ArrayList();
            var where = "1=1 ";
            int i = 0;
            if (!string.IsNullOrWhiteSpace(filter.MallOrderId))
            {
                where += " and a.MallOrderID=@p0p" + i;
                paras.Add(filter.MallOrderId);
                i++;
            }
            if (filter.BeginDate.HasValue)
            {
                where += " and a.CreatedDate>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate.HasValue)
            {
                where += " and a.CreatedDate<@p0p" + i;
                paras.Add(filter.EndDate);
                i++;
            }
            if (filter.OrderSysNo != null)
            {
                where += " and a.SoOrderSysNo=@p0p" + i;
                paras.Add(filter.OrderSysNo);
                i++;
            }
            if (filter.MallTypeSysNo>0)
            {
                where += " and a.MallTypeSysNo=@p0p" + i;
                paras.Add(filter.MallTypeSysNo.ToString());
                i++;
            }
            if (filter.Status > 0)
            {
                where += " and a.Status=@p0p" + i;
                paras.Add(filter.Status.ToString());
                i++;
            }
            sql = string.Format(sql, where);

            #endregion

            var dataList = Context.Select<DsDealerLog>("a.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<DsDealerLog>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("a.sysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }
        /// <summary>
        /// 检查订单号是否存在
        /// </summary>
        /// <param name="mallOrderId">商商城订单号</param>
        /// <param name="status">待解决(10),已解决(20)</param>
        /// <param name="mallSysNo">商城系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-11-06 杨浩 创建</remarks>
        public override bool ChekMallOrderId(string mallOrderId, int status, int mallSysNo)
        {
          return Context.Sql("select count(1) from DsDealerLog where mallOrderId=@mallOrderId and status=@status and mallSysNo=@mallSysNo")
                .Parameter("mallOrderId", mallOrderId)
                .Parameter("status", status)
                .Parameter("mallSysNo", mallSysNo)
                .QuerySingle<int>() > 0;
        }
        /// <summary>
        /// 取单条数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>model</returns>
        /// <remarks>2014-03-31 朱家宏 创建</remarks>
        public override DsDealerLog Get(int sysNo)
        {
            return Context.Sql("select * from DsDealerLog where sysNo=@sysNo")
                          .Parameter("sysNo", sysNo)
                          .QuerySingle<DsDealerLog>();
        }
    }
}
