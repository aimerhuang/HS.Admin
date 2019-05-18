using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 兴业嘉同步日志sql
    /// </summary>
    /// <remarks>2018-03-22 罗熙 创建</remarks>
    public class XingYeDaoImpl : IXingYeDao
    {
        /// <summary>
        /// 添加XingYe同步日志
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>日志ID</returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        public override int XingYeSyncLogCreate(XingYeSyncLog model)
        {
            return Context.Insert("XingYeSyncLog", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 根据流程编号和接口类型获取单据编号
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <returns></returns>
        /// <remarks>2017-1-4 杨浩 创建</remarks>
        public override string GetVoucherNoByTlowIdentify(int interfaceType, string flowIdentify)
        {
            return Context.Sql("select top 1  VoucherNo from [XingYeSyncLog]  where  FlowIdentify=@flowIdentify and  InterfaceType=@interfaceType")
               .Parameter("interfaceType", interfaceType)
               .Parameter("flowIdentify", flowIdentify)
               .QuerySingle<string>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>日志分页</returns>
        /// <remarks>2013-10-22 黄志勇 创建
        /// 修改XingYe日志查询方法，仓库编号为多个编号时进行查询（余勇 2014-07-02）
        /// 拼接Sql解决无法使用索引的问题(null is null or 无法使用索引) 2014-09-03 杨浩 修改
        /// </remarks>
        public override Pager<CBXingYeSyncLog> GetList(ParaXingYeSyncLogFilter filter)
        {
            #region 短路sql

            //            string sql =
            //                   @"(select a.*,h.WarehouseName
            //                            from XingYeSyncLog a                           
            //                            left join WhWarehouse h on h.sysno=a.warehousesysno
            //                            
            //                    where    1=1                    
            //                            (:Status is null or a.Status=:Status) and                      --状态
            //                            (:Name is null or charindex(a.Name,:Name) > 0) and                 --接口名称
            //                            (:VoucherNo is null or charindex(a.VoucherNo,:VoucherNo) > 0) and  --单据号
            //                            (:Remarks is null or charindex(a.Remarks,:Remarks) > 0) and        --备注
            //                            (:BeginDate is null or a.CreatedDate>=:BeginDate) and          --创建日期(起)
            //                            (:EndDate is null or a.CreatedDate<:EndDate)  and              --创建日期(止) 
            //                            (:LastsyncBeginTime is null or a.LastsyncTime>=:LastsyncBeginTime) and          --同步日期(起)
            //                            (:LastsyncEndDate is null or a.LastsyncTime<:LastsyncEndDate)  and              --同步日期(止) 
            //                            (:FlowIdentify is null or a.FlowIdentify=:FlowIdentify) and    --流程标识
            //                            (:StatusCode is null or a.StatusCode=:StatusCode)           --状态码
            //                           --(:WarehouseSysNo is null or a.WarehouseSysNo=:WarehouseSysNo)  
            //                            and (:WarehouseSysNo is null or exists (select 1 from table(splitstr(:WarehouseSysNo, ',')) tmp where tmp.column_value = a.WarehouseSysNo)) --仓库
            //                            and exists (select 1 from table(splitstr(:Warehouses, ',')) tmp where tmp.column_value = a.warehousesysno)
            //                    ) tb";

            //            //查询日期上限+1
            //            filter.EndDate = filter.EndDate == null ? (DateTime?)null : filter.EndDate.Value.AddDays(1);

            //            var paras = new object[]
            //            {
            //                filter.Status, filter.Status,
            //                filter.Name, filter.Name,
            //                filter.VoucherNo, filter.VoucherNo != null ? filter.VoucherNo.Trim() : null,
            //                filter.Remarks, filter.Remarks != null ? filter.Remarks.Trim() : null,
            //                filter.BeginDate, filter.BeginDate,
            //                filter.EndDate, filter.EndDate,
            //                filter.LastsyncBeginTime, filter.LastsyncBeginTime,
            //                filter.LastsyncEndDate, filter.LastsyncEndDate,
            //                filter.FlowIdentify, filter.FlowIdentify,
            //                filter.StatusCode, filter.StatusCode,
            //                filter.WarehouseSysNo,filter.WarehouseSysNo,
            //                filter.Warehouses
            //            };

            #endregion

            #region 条件

            string sql =
                @"(select a.*,h.WarehouseName
                            from XingYeSyncLog a                           
                            left join WhWarehouse h on h.sysno=a.warehousesysno                            
                    where    1=1 {0} ) tb ";
            string where = string.Empty;
            var paras = new ArrayList();
            int i = 0;
            //拼接Sql解决无法使用索引的问题(null is null or 无法使用索引) 杨浩 修改
            if (filter.Status != null)
            {
                where += " and a.Status=@p0p" + i;
                paras.Add(filter.Status);
                i++;
            }
            if (filter.Name != null)
            {
                where += " and CHARINDEX(a.Name,@p0p" + i + ") > 0 ";
                i++;
                paras.Add(filter.Name);
            }
            if (filter.Remarks != null)
            {
                where += " and CHARINDEX(a.Remarks,@p0p" + i + ") >0 ";
                i++;
                paras.Add(filter.Remarks);
            }
            if (filter.VoucherNo != null)
            {
                where += " and a.VoucherNo=@p0p" + i;
                i++;
                paras.Add(filter.VoucherNo);
            }
            if (filter.BeginDate != null)
            {
                where += " and a.CreatedDate>=@p0p" + i;
                i++;
                paras.Add(filter.BeginDate);
            }
            if (filter.EndDate != null)
            {
                where += " and a.CreatedDate<@p0p" + i;
                i++;
                paras.Add(filter.EndDate);
            }
            if (filter.LastsyncBeginTime != null)
            {
                where += " and a.LastsyncTime>=@p0p" + i;
                i++;
                paras.Add(filter.LastsyncBeginTime);
            }
            if (filter.LastsyncEndDate != null)
            {
                where += " and a.LastsyncTime<@p0p" + i;
                i++;
                paras.Add(filter.LastsyncEndDate);
            }
            if (filter.FlowIdentify != null)
            {
                where += " and a.FlowIdentify=@p0p" + i;
                i++;
                paras.Add(filter.FlowIdentify);
            }
            if (filter.StatusCode != null)
            {
                where += " and a.StatusCode=@p0p" + i;
                i++;
                paras.Add(filter.StatusCode);
            }
            if (filter.WarehouseSysNo != null)
            {
                where +=
                    @"  and exists (select 1 from splitstr(@p0p" + i + ", ',') tmp where tmp.col = a.WarehouseSysNo)";
                i++;
                where +=
                   @"   and exists (select 1 from splitstr(@p0p" + i + ", ',') tmp where tmp.col = a.warehousesysno)";
                i++;
                paras.Add(filter.WarehouseSysNo);
                paras.Add(filter.Warehouses);
            }

            sql = string.Format(sql, where);

            #endregion

            var dataList = Context.Select<CBXingYeSyncLog>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBXingYeSyncLog>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            pager.Rows = dataList.OrderBy(" tb.FLOWIDENTIFY desc, tb.interfacetype desc,tb.sysno asc")
                                 .Paging(pager.CurrentPage, filter.PageSize)
                                 .QueryMany();

            return pager;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>日志</returns>
        /// <remarks>2013-10-23 黄志勇 创建</remarks>
        public override XingYeSyncLog GetEntity(int sysNo)
        {
            return Context.Sql("select * from XingYeSyncLog where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo).QuerySingle<XingYeSyncLog>();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-10-23 黄志勇 创建</remarks>
        public override int Update(XingYeSyncLog entity)
        {
            return Context.Update("XingYeSyncLog", entity)
                .AutoMap(o => o.SysNo)
                .Where("SysNo", entity.SysNo).Execute();
        }

        /// <summary>
        /// 数据是否已导入
        /// </summary>
        /// <param name="md5">数据md5</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-06 杨浩 创建</remarks>
        public override bool IsImport(string md5)
        {
            return Context.Sql("select count(1) from  XingYeSyncLog where datamd5=@0", md5).QuerySingle<int>() > 0;
        }

        /// <summary>
        /// 获取XingYe同步等待日志（任务）
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public override List<XingYeSyncLog> GetSyncWaitList(int count)
        {
            #region
            const string command = @"select esl.* from XingYeSyncLog esl
                          right join (
                                 select * from (
                                   select FlowIdentify,max(lastupdatedate) as maxlastupdatedate,(row_number() OVER(ORDER BY max(lastupdatedate) asc)) as row_number 
                                   from XingYeSyncLog e where 
                                    e.flowidentify  not  in (select flowidentify  from XingYesynclog  where status=@0)  and status=@1
                                   group by FlowIdentify        
                                   ) p
                                 where p.row_number<=@2
                               ) temp  
                           on esl.flowidentify=temp.FlowIdentify where (esl.status=@3 or esl.status=@4) 
                            order by esl.flowidentify desc, esl.interfacetype desc,esl.sysno asc ";
            #endregion
            var paras = new object[]
            {
               (int)SystemStatus.XingYe同步日志状态.失败, (int)SystemStatus.XingYe同步日志状态.等待同步,
               count,
               (int)SystemStatus.XingYe同步日志状态.失败, (int)SystemStatus.XingYe同步日志状态.等待同步
            };
            return Context.Sql(command, paras).QueryMany<XingYeSyncLog>();
        }

        /// <summary>
        /// 获取XingYe同步失败的日志（任务）
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public override List<XingYeSyncLog> GetSyncFailureList(int count)
        {
            #region command
            const string command = @"select esl.* from XingYeSyncLog esl
                          right join (
                                 select * from (
                                   select FlowIdentify,max(lastupdatedate) as maxlastupdatedate,(row_number() OVER(ORDER BY max(lastupdatedate) asc)) as row_number 
                                   from XingYeSyncLog where (status=@0)
                                   group by FlowIdentify        
                                   ) p
                                 where p.row_number<=@1
                               ) temp  
                           on esl.flowidentify=temp.FlowIdentify where (esl.status=@2 or esl.status=@3) 
                            order by esl.flowidentify desc, esl.interfacetype desc,esl.sysno asc ";
            #endregion

            var paras = new object[]
            {
               (int)SystemStatus.XingYe同步日志状态.失败,
               count,
               (int)SystemStatus.XingYe同步日志状态.失败, (int)SystemStatus.XingYe同步日志状态.等待同步
            };

            return Context.Sql(command, paras).QueryMany<XingYeSyncLog>();
        }

        /// <summary>
        /// 获取XingYe同步日志列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-4-9 杨浩 创建</remarks>
        public override List<XingYeSyncLog> GetRelateList(string flowIdentify)
        {
            string command = @"select * from XingYeSyncLog  
                                     where flowIdentify=@0 and (status={0} or status={1})
                                     order by FLOWIDENTIFY desc,interfacetype desc,sysno asc ";
            command = string.Format(command, (int)SystemStatus.XingYe同步日志状态.失败, (int)SystemStatus.XingYe同步日志状态.等待同步);

            return Context.Sql(command, flowIdentify).QueryMany<XingYeSyncLog>();
        }
        /// <summary>
        /// 获取XingYe同步日志
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public override IList<XingYeSyncLog> GetList(string sysNos)
        {
            string command = @"select * from XingYeSyncLog  
                                     where  SysNo in ({0}) ";
            command = string.Format(command, sysNos);

            return Context.Sql(command).QueryMany<XingYeSyncLog>();
        }
        /// <summary>
        /// 获取Kis单据编号
        /// </summary>
        /// <param name="flowType">流程类型</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <returns></returns>
        /// <remarks>2017-05-05 杨浩 创建</remarks>
        public override string GetVoucherNo(string flowType, string flowIdentify)
        {
            string command = @"select top 1 VoucherNo from XingYeSyncLog  
                                     where  flowType=@flowType and flowIdentify=@flowIdentify and  Status=1 ";

            return Context.Sql(command)
                .Parameter("flowType", flowType)
                .Parameter("flowIdentify", flowIdentify)
                .QuerySingle<string>();
        }

        /// <summary>
        /// 获取没有同步金蝶的出库单
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-09-28 杨浩 创建</remarks>
        public override IList<WhStockOut> GetNoSyncStockOutList(int warehouseSysNo)
        {
            string command = "select * from WhStockOut where [Status]=60 and TransactionSysNo not in (select FlowIdentify from XingYeSyncLog where WarehouseSysNo={0}) and WarehouseSysNo={0} ";
            command = string.Format(command, warehouseSysNo);
            return Context.Sql(command).QueryMany<WhStockOut>();
        }
    }
}
