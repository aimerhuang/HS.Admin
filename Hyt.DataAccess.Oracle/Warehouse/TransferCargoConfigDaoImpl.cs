using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Hyt.Model;
using Hyt.DataAccess.Warehouse;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    ///调货配置 数据访问实现
    ///</summary>
    /// <remarks> 2016-04-01 朱成果 创建</remarks>
    public class TransferCargoConfigDaoImpl : ITransferCargoConfigDao
    {
        #region 自动生成代码
        /// <summary>
        /// 插入(调货配置)
        ///</summary>
        /// <param name="entity">调货配置</param>
        /// <returns>新增记录编号</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public override int Insert(TransferCargoConfig entity)
        {
            entity.SysNo = Context.Insert("TransferCargoConfig", entity)
                                .AutoMap(o => o.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新(调货配置)
        ///</summary>
        /// <param name="entity">调货配置</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public override int Update(TransferCargoConfig entity)
        {
            return Context.Update("TransferCargoConfig", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取(调货配置)
        ///</summary>
        /// <param name="sysno">编号</param>
        /// <returns>调货配置</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public override TransferCargoConfig GetEntity(int sysno)
        {
            return Context.Sql("select * from TransferCargoConfig where SysNo=@sysno")
                           .Parameter("sysno", sysno)
                           .QuerySingle<TransferCargoConfig>();

        }

        /// <summary>
        /// 删除(调货配置)
        ///</summary>
        /// <param name="sysno">编号</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public override int Delete(int sysno)
        {
            return Context.Sql("delete  from TransferCargoConfig where SysNo=@sysno")
                           .Parameter("sysno", sysno)
                           .Execute();
        }
        #endregion

        #region 根据 "申请调货仓库编号" 获取 "配货仓库编号"

        /// <summary>
        /// 根据 "申请调货仓库编号" 获取 "配货仓库编号"
        ///</summary>
        /// <param name="applyWarehouseSysNo">申请调货仓库编号</param>
        /// <returns>配货仓库编号</returns>
        /// <remarks> 2016-04-05 王江 创建</remarks>
        public override int GetDeliveryWarehouseSysNoByApplyWarehouseSysNo(int applyWarehouseSysNo)
        {
            return Context.Sql("select m.DELIVERYWAREHOUSESYSNO from TransferCargoConfig m where m.APPLYWAREHOUSESYSNO=@ApplyWarehouseSysNo")
                          .Parameter("ApplyWarehouseSysNo", applyWarehouseSysNo)
                          .QuerySingle<int>();
        }

        #endregion

        #region 根据 配货仓库编号 获取 申请调货仓库列表

        /// <summary>
        /// 根据 配货仓库编号 获取 申请调货仓库列表
        ///</summary>
        /// <param name="deliveryWarehouseSysNo">配货仓库编号</param>
        /// <returns>申请调货仓库列表</returns>
        /// <remarks> 2016-04-06 王江 创建</remarks>
        public override IList<CBTransferCargoConfig> GetApplyWarehouseListByDeliveryWarehouseSysNo(int deliveryWarehouseSysNo)
        {
            return Context.Sql(@"select m.sysno,m.applywarehousesysno,n.warehousename ApplyWarehouseName,m.deliverywarehousesysno,p.warehousename DeliveryWarehouseName,m.status,s.username CreateUserName,m.createddate,m.lastupdatedate,t.username UpdateUserName
                                 from TransferCargoConfig m inner join whwarehouse n on m.applywarehousesysno=n.sysno 
                                 left join whwarehouse p on m.deliverywarehousesysno=p.sysno
                                 left join SyUser s on m.createdby=s.sysno 
                                 left join SyUser t on m.lastupdateby=t.sysno 
                                 where m.deliverywarehousesysno=@DeliveryWarehouseSysNo")

                          .Parameter("DeliveryWarehouseSysNo", deliveryWarehouseSysNo)
                          .QueryMany<CBTransferCargoConfig>();
        }

        #endregion

        #region 获取所有已存在的申请调货仓库

        /// <summary>
        /// 获取所有已存在的申请调货仓库
        ///</summary>
        /// <returns>已存在的申请调货仓库结果集</returns>
        /// <remarks> 2016-04-19 王江 创建</remarks>
        public override IList<int> GetAllApplyWarehouseSysNo()
        {
            return Context.Sql("select t.applywarehousesysno from TransferCargoConfig t")
                          .QueryMany<int>();
        }

        #endregion

        #region 根据 配货仓库编号 申请调货仓库编号 获取单条记录

        /// <summary>
        /// 根据 配货仓库编号 申请调货仓库编号 获取单条记录
        /// </summary>
        /// <param name="deliveryWarehouseSysNo">配货仓库编号</param>
        /// <param name="applyWarehouseSysNo">申请调货仓库编号</param>
        /// <returns>调货配置</returns>
        /// <remarks> 2016-04-06 王江 创建</remarks>
        public override TransferCargoConfig QuerySingle(int deliveryWarehouseSysNo, int applyWarehouseSysNo)
        {
            return Context.Sql("select m.* from TransferCargoConfig m where m.deliverywarehousesysno=@deliverywarehousesysno and  m.applywarehousesysno=@applywarehousesysno")
                          .Parameter("deliverywarehousesysno", deliveryWarehouseSysNo)
                          .Parameter("applywarehousesysno", applyWarehouseSysNo)
                          .QuerySingle<TransferCargoConfig>();

        }

        #endregion

        #region 调货配置分页列表

        /// <summary>
        /// 界面分页列表
        /// </summary>
        /// <param name="filter">筛选字段</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-10-9 王江 创建</remarks>
        public override Pager<CBTransferCargoConfig> GetTransferCargoConfigList(Model.Parameter.ParaTransferCargoConfigFilter filter)
        {
            string sqlSelect = @"m.sysno,m.applywarehousesysno,n.warehousename ApplyWarehouseName,m.deliverywarehousesysno,p.warehousename DeliveryWarehouseName,m.status,
                                 s.username CreateUserName,m.createddate,m.lastupdatedate,t.username UpdateUserName ";

            string sqlFrom = @"TransferCargoConfig m inner join whwarehouse n on m.applywarehousesysno=n.sysno 
                               left join whwarehouse p on m.deliverywarehousesysno=p.sysno 
                               left join SyUser s on m.createdby=s.sysno 
                               left join SyUser t on m.lastupdateby=t.sysno";

            var parameters = new ArrayList();
            System.Text.StringBuilder strWhere = new System.Text.StringBuilder("1=1");

            if (filter.IsQueryPickingWarehouse)
            {
                strWhere.AppendFormat(" and m.sysno in (select min(sysno) from TransferCargoConfig t group by t.deliverywarehousesysno)");
            }

            if (!string.IsNullOrWhiteSpace(filter.ApplyWarehouseName)) // 申请调货仓库名称
            {
                strWhere.AppendFormat(" and n.warehousename=@ApplyWarehouseName", filter.ApplyWarehouseName);
                parameters.Add(filter.ApplyWarehouseName);
            }

            if (!string.IsNullOrWhiteSpace(filter.DeliveryWarehouseName)) // 配货仓库名称
            {
                strWhere.AppendFormat(" and p.warehousename=@DeliveryWarehouseName", filter.DeliveryWarehouseName);
                parameters.Add(filter.DeliveryWarehouseName);
            }

            var pager = new Pager<CBTransferCargoConfig>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };

            using (var context = Context.UseSharedConnection(true))
            {
                var results = context.Select<CBTransferCargoConfig>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(strWhere.ToString())
                                       .Parameters(parameters)
                                       .Paging(filter.Id, filter.PageSize)
                                       .OrderBy("m.SysNo desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(strWhere.ToString())
                                   .Parameters(parameters)
                                   .QuerySingle();
                pager.Rows = results;
                pager.TotalRows = count;
            }
            return pager;
        }

        #endregion

        #region 查询可用的调货配置表
        /// <summary>
        /// 查询可用的调货配置表
        ///</summary>
        /// <param name="applyWarehouseSysNo">申请调货仓库编号</param>
        /// <returns>调货配置表</returns>
        /// <remarks> 2016-04-05 朱成果 创建</remarks>
        public override TransferCargoConfig GetEntityByApplyWarehouseSysNo(int applyWarehouseSysNo)
        {
            return Context.Sql("select * from TransferCargoConfig m where m.APPLYWAREHOUSESYSNO=@ApplyWarehouseSysNo and Status=1")
                        .Parameter("ApplyWarehouseSysNo", applyWarehouseSysNo)
                        .QuerySingle<TransferCargoConfig>();
        }
        #endregion
    }
}
